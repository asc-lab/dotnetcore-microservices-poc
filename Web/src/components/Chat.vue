<template>
    <div>
        <b-form-input id="message"
                      type="text"
                      v-model="message"
                      required
                      @keyup.native.enter="send"
                      placeholder="Type your message">
        </b-form-input>

        <div class="messages-container" v-html="chat">
        </div>
    </div>
</template>

<script>
    import auth from './http/Auth'
    
    import chat from './http/WebSocket'

    export default {
        name: "Chat",
        data() {
            return {
                user: {},
                hubConnection: {},
                chat: '',
                message: ''
            }
        },
        beforeCreate() {
            if(!auth.isAuthenticated())
                window.location.href = "/";
        },
        created() {
            this.user = auth.getAuthDetails();

            this.hubConnection = chat.createHub();

            this.hubConnection
                .start()
                .then(()=>console.info("connected to hub"))
                .catch(err => console.error(err));
            
            this.hubConnection.on("ReceiveMessage",(usr, avatar,msg) =>{
                this.appendMsgToChat(usr,avatar,msg);
            });

            this.hubConnection.on("ReceiveNotification", msg => {
                this.appendAlertToChat(msg);
            });
        },
        methods: {
            send () {
                
                this.hubConnection.invoke("SendMessage", this.message);
                this.message = '';
            },
            appendMsgToChat(usr, avatar, msg) {
                const htmlMsg = '<p class="msg"><img class="avatar" src="' + avatar + '"/> [' + usr + '] ' + msg + '</p>';
                this.chat = htmlMsg + this.chat;
            },
            appendAlertToChat(msg) {
                const htmlMsg = '<p class="msg">' + msg + '</p>';
                this.chat = htmlMsg + this.chat;
            }
        }
    }
</script>

<style>
    .messages-container {
        width: 80%;
        margin: 0 auto;
    }

    #message {
        margin-bottom: 20px;
    }

    .avatar {
        max-height: 50px;
    }

    .msg {
        border: 1px lightblue solid;
        padding: 5px;
        border-radius: 30px;
    }

    .my-messages {
        background-color: lightblue;
    }
</style>