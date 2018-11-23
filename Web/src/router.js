import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/HomeView.vue'

Vue.use(Router);

function loadView(view) {
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    return () => import(/* webpackChunkName: "view-[request]" */ `./views/${view}View.vue`)
}

export default new Router({
    routes: [
        {
            path: '/',
            name: 'home',
            component: Home
        },
        {
            path: '/chat',
            name: 'chat',
            component: loadView('Chat')
        },
        {
            path: '/chatbot',
            name: 'chatbot',
            component: loadView('Chatbot')
        },
        {
            path: '/account',
            name: 'account',
            component: loadView('Account')
        },
        {
            path: '/products',
            name: 'products',
            component: loadView('Products')
        },
        {
            path: '/products/:productCode',
            name: 'product',
            props: true,
            component: loadView('ProductDetails')
        },
        {
            path: '/policy/fromOffer/:offerNumber',
            name: 'createPolicy',
            props: true,
            component: loadView('PolicyCreate')
        },
        {
            path: '/policies',
            name: 'policies',
            component: loadView('Policies')
        },
        {
            path: '/policies/:policyNumber',
            name: 'policyDetails',
            props: true,
            component: loadView('PolicyDetails')
        }
    ]
})
