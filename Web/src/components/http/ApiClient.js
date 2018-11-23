import axios from 'axios';
import {TOKEN_KEY} from './Auth';

export const HTTP = axios.create({
    baseURL: (process.env.VUE_APP_BACKEND_URL ? process.env.VUE_APP_BACKEND_URL : "/api/")
});

HTTP.interceptors.request.use(
    (request) => {
        request.headers.Authorization = 'Bearer ' + localStorage.getItem(TOKEN_KEY);
        return request;
    },
    (error) => Promise.reject(error)
);

HTTP.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        console.log(error);
        if (error.response.status === 401 || error.response.status === 403) {
            localStorage.removeItem(TOKEN_KEY);
            window.location.href = '/#/account';
        }
    }
);