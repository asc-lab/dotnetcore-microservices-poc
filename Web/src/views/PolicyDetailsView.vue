<template>
    <div>
        <PolicyDetails v-bind:policyNumber=policyNumber></PolicyDetails>
    </div>
</template>

<script>
    import PolicyDetails from "../components/PolicyDetails";

    export default {
        name: "PolicyDetailsView",
        components: {
            PolicyDetails
        },
        props: {
            policyNumber: String
        }
    }
</script>