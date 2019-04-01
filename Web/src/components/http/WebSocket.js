const CHAT_URL = (process.env.VUE_APP_CHAT_URL ? process.env.VUE_APP_CHAT_URL : "/");

import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
    createHub() {
        return new HubConnectionBuilder()
            .withUrl( CHAT_URL + '/agentsChat', { accessTokenFactory: () => localStorage.getItem("jwt") })
            .configureLogging(LogLevel.Information)
            .build();
    }
}