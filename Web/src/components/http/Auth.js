const API_URL = (process.env.VUE_APP_AUTH_URL ? process.env.VUE_APP_AUTH_URL : "/");
const LOGIN_URL = API_URL + 'login';

export const TOKEN_KEY = "jwt";
export const DETAILS_KEY = "auth-details";

export default {

    login(context, credentials) {
        this.clearToken();
        return context.$http.post(LOGIN_URL, credentials)
            .then(
                (response) => {
                    localStorage.setItem(TOKEN_KEY, response.body.access_token);
                    localStorage.setItem(DETAILS_KEY, JSON.stringify(response.body));
                },
                (error) => {
                    console.info(error);
                }
            )
    },

    logout() {
        this.clearToken();
    },

    clearToken() {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(DETAILS_KEY);
    },

    isAuthenticated() {
        return localStorage.getItem(TOKEN_KEY) != null;
    },

    getAuthDetails() {
        if (!this.isAuthenticated())
            return null;

        return JSON.parse(localStorage.getItem(DETAILS_KEY));
    }
}
