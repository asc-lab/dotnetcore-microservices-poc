<template>
    <div class="col-6 col-lg-2">
        <div class="card">
            <div class="card-body p-3 d-flex align-items-center">
            <i v-bind:class="iconClass"></i>
            <div>
                <div class="text-value-sm text-primary">{{amount}}</div>
                <div class="text-muted text-uppercase font-weight-bold small">{{title}}</div>
                <div v-if="policiesCount" class="text-muted text-uppercase font-weight-bold small">{{policiesCount}} pcs.</div>
            </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: 'TotalSalesCard',
    props: ['title', 'icon', 'amount', 'policiesCount'],
    computed: {
        iconClass() {
            return "fa fa-" + this.icon + " bg-primary p-3 font-2xl mr-3";
        }
    }
}
</script>