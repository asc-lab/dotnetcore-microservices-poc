<template>
    <b-card :title=product.name
            :img-src=product.image
            :img-alt=product.name
            img-top
            tag="article">
        <p class="card-text">
            {{product.description}}
            <CoverList :covers="product.covers"></CoverList>
        </p>
        <router-link :to="{name: 'product', params: { productCode: product.code }}">
            <b-button type="submit" variant="primary">Buy</b-button>
        </router-link>
    </b-card>
</template>

<script>
    import CoverList from "./CoverList";

    export default {
        name: 'ProductCard',
        props: ['product'],
        components: {CoverList}
    }
</script>

<style scoped lang="scss">
    .card-img-top {
        max-height: 230px;
    }
</style>