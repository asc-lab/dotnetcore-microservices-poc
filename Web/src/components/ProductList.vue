<template>
    <div class="row">
        <div class="col-sm-3" v-for="product in products" :key="product.code">
            <ProductCard :product="product"></ProductCard>
        </div>
    </div>
</template>

<script>
    import ProductCard from "./ProductCard";
    import {HTTP} from "./http/ApiClient";

    export default {
        name: 'ProductList',
        components: {ProductCard},
        data() {
            return {
                products: []
            };
        },
        created: function () {
            HTTP.get('products').then(response => {
                this.products = response.data;
            });
        }
    }
</script>

<style scoped lang="scss">

</style>