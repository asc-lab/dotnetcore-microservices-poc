<template>
    <div id="app">
        <div id="nav">
            <router-link to="/">Home</router-link>
            |
            <router-link to="/products">Products</router-link>
            |
            <router-link to="/policies">Policies</router-link>
            |
            <router-link to="/chat">Chat for Agents</router-link>
            |
            <router-link to="/account">Account</router-link>
        </div>

        <hr/>

        <div id="main-container">
            <router-view/>

            <div class="btn-chat" id="livechat-compact-container" style="visibility: visible; opacity: 1;">
                <div class="btn-holder">
                    <router-link to="/chatbot" class="link">Live Chat</router-link>
                </div>
            </div>
        </div>

        <footer>
            <img src="./assets/logo_horizontal.png" alt="Logo Footer"/>
            <p><a href="https://asc.altkom.pl">Altkom Software & Consulting LAB @ 2018</a></p>
        </footer>
    </div>
</template>

<style>
    #app {
        font-family: 'Avenir', Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    #nav {
        padding: 10px 10px 0 10px;
    }

    #nav a {
        font-weight: bold;
        color: #2c3e50;
    }

    #nav a.router-link-exact-active {
        color: #42b983;
    }

    #main-container {
        width: 80%;
        margin: 0 auto;
    }

    footer {
        text-align: center;
        border-top: 1px gray solid;
        margin-left: 10em;
        margin-right: 10em;
        margin-top: 30px;
    }

    footer > img {
        margin-top: 20px;
        height: 60px;
    }

    footer > hr {
        width: 90%;
    }

    footer > p > a {
        color: black;
    }

    #livechat-compact-container {
        height: 153px;
        position: fixed;
        right: 0;
        top: 30vh;
        z-index: 10000;
    }
    .btn-chat a {
        color: #fff;
        text-decoration: none;
        background: #1798F7;
        padding: 24px 20px 8px;
        display: block;
        font-weight: bold;
        font-size: 14px;
        box-shadow: 0 0 0 1px #03b2ff inset;
        border: 1px solid #144866;
        border-radius: 2px;
        -ms-transform: rotate(90deg) translate(0, -20px);
        -webkit-transform: rotate(90deg) translate(0, -20px);
        transform: rotate(90deg) translate(0, -20px);
        position: relative;
        right: -27px;
        transition: background 0.2s, right 0.2s;
    }
    .btn-chat a:hover {
        background: #47B6F5;
        right: -20px;
        transition: background 0.2s, right 0.2s;
    }
</style>
