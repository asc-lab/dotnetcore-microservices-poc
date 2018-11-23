<template>
    <div>
        <ProductDetails v-bind:productCode=productCode></ProductDetails>
    </div>
</template>

<script>
    import ProductDetails from "../components/ProductDetails";

    export default {
        name: "ProductDetailsView",
        components: {
            ProductDetails
        },
        props: {
            productCode: String
        }
    }
</script>