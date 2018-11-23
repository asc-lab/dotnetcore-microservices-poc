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
    import ws from './http/WebSocket'

    export default {
        name: "Chat",
        data() {
            return {
                user: {},
                webSocket: {},
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
            this.webSocket = ws.create(this.user.username);

            this.webSocket.onmessage = event => {
                console.log('Hello from websocket onmessage. Event: ' + event.data);
                this.appendToChat(event.data);
            };

            this.webSocket.onclose = () => {
                console.error("WebSocket connection closed");
            };
        },
        methods: {
            send () {
                const htmlMsg = '<p class="msg"><img class="avatar" src="' + this.user.avatar + '"/> [' + this.user.username + '] ' + this.message + '</p>';
                this.webSocket.send(htmlMsg);
                this.chat += htmlMsg.replace('<p class="msg">', '<p class="msg my-messages">');
                this.message = '';
            },
            appendToChat(text) {
                this.chat += text;
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