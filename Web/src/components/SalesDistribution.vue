<template>
    <div class="card">
        <div class="card-header">Products
            <div class="card-header-actions">
            <a class="card-header-action" href="http://www.chartjs.org" target="_blank">
                <small class="text-muted">docs</small>
            </a>
            </div>
        </div>
        <div class="card-body">
            <div class="chart-wrapper">
            <sales-pie :chart-data="datacollection"/>
            </div>
        </div>
    </div>
</template>

<script>
import SalesPie from "./SalesPie.js";
export default {
    name: 'SalesDistribution',
    props: {
        productTotals: {}
    },
    data() {
        return {
            datacollection: { labels:[], datasets: [] }
        }
    },
    watch: {
        productTotals: function() {
            this.loadData();
        }
    },
    components: {
        SalesPie
    },
    mounted() {
        this.loadData();
    },
    methods: {
        loadData() {
            var data = {
            labels: [],
            datasets: [
                {
                    data: [],
                    backgroundColor: [
                        "#FF6384",
                        "#36A2EB",
                        "#FFCE56",
                        "#fe12ff",
                        "#0aff2d",
                        "#153fff",
                        "#ff196f",
                        "#fffbf0"
                    ]
                }
            ]
            };
            for (var key in this.productTotals) {
                data.labels.push(key);
                data.datasets[0].data.push(this.productTotals[key]);    
            }
            this.datacollection = data;
        }
    }   
}
</script>

<style>
@media (min-width: 576px) {
  .card-columns.cols-2 {
    -webkit-column-count: 2;
    -moz-column-count: 2;
    column-count: 2;
  }
    .card-header-actions {
    display: inline-block;
    float: right;
    margin-right: -0.25rem;
    }
    *[dir="rtl"] .card-header-actions {
    float: left;
    margin-right: auto;
    margin-left: -0.25rem;
    }
    .card-header-action {
    padding: 0 0.25rem;
    color: #73818f;
    }
    .card-header-action:hover {
    color: #23282c;
    text-decoration: none;
    }
}
</style>