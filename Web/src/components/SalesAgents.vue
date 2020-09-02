<template>
    <div class="card">
        <div class="card-header">Agents
            <div class="card-header-actions">
            <a class="card-header-action" href="http://www.chartjs.org" target="_blank">
                <small class="text-muted">docs</small>
            </a>
            </div>
        </div>
        <div class="card-body">
            <div class="chart-wrapper">
            <SalesBar :chart-data="datacollection"/>
            </div>
        </div>
    </div>
</template>

<script>
import SalesBar from "./SalesBar.js";
export default {
    name: 'SalesAgents',
    props: {
        agentTotals: {}
    },
    data() {
        return {
            datacollection: { labels:[], datasets: [] }
        }
    },
    watch: {
        agentTotals: function() {
            this.loadData();
        }
    },
    components: {
        SalesBar
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
                    label: 'Agents sales (EUR)',
                    backgroundColor: "#DF4220",
                    data: []
                }
            ]
            };
            Object.getOwnPropertyNames(this.agentTotals).forEach(key => {
                if (!isNaN(this.agentTotals[key])) {
                    data.labels.push(key);
                    data.datasets[0].data.push(this.agentTotals[key]);
                }
            });
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