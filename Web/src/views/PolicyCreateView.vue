<template>
    <div>
        <PolicyCreate v-bind:offerNumber=offerNumber></PolicyCreate>
    </div>
</template>

<script>
    import PolicyCreate from "../components/PolicyCreate";

    export default {
        name: "PolicyCreateView",
        components: {
            PolicyCreate
        },
        props: {
            offerNumber: String
        }
    }
</script>