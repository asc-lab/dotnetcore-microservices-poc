<template>
    <div>
        <div class="chatbot-container" v-if="chatbotUrl">
            <iframe :src='chatbotUrl'
                    style='min-width: 400px; width: 100%; min-height: 600px;'>
            </iframe>
        </div>
        <div v-else>
            You must define your chatbot url in app properties!
        </div>
    </div>
</template>

<script>
    export default {
        name: "Chatbot",
        data() {
            return {
                chatbotUrl: ''
            }
        },
        created() {
            this.chatbotUrl = process.env.VUE_APP_BOT_URL;
        }
    }
</script>

<style>
</style>