<template>
    <div class="hello">
        <h1>{{ msg }}</h1>
        <h3>This is an example of a very simplified insurance sales system made in a microservice architecture using Micronaut.</h3>

        <img class="arch-image" alt="architecture" src="https://raw.githubusercontent.com/asc-lab/dotnetcore-microservices-poc/master/readme-images/dotnetcore-microservices-architecture.png" />
    </div>
</template>

<script>
    export default {
        name: 'Home',
        props: {
            msg: String
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }

    .arch-image {
        width: 100%;
    }
</style>
