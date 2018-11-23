<template>
    <table class="table">
        <thead>
        <tr>
            <th>#</th>
            <th>Risk</th>
            <th>Sum insured</th>
        </tr>
        </thead>
        <tbody>
        <tr v-for="(cover,index) in covers" :key="cover.code">
            <th scope="row">{{index+1}}</th>
            <td>{{cover.name}}</td>
            <td class="text-right">{{cover.sumInsured}} <span v-if="cover.sumInsured">EUR</span></td>
        </tr>
        </tbody>
    </table>
</template>

<script>
    export default {
        name: 'CoverList',
        props: ['covers']
    }
</script>

<style scoped lang="scss">

</style>