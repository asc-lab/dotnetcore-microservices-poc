import Vue from 'vue';
import VueResource from 'vue-resource'
import App from './App.vue';
import BootstrapVue from 'bootstrap-vue'
import router from './router';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.config.productionTip = false;
Vue.use(BootstrapVue);
Vue.use(VueResource);

new Vue({
    router,
    render: h => h(App)
}).$mount('#app');
