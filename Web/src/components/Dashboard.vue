<template>
  <div>
    <div class="container-fluid">
      <div class="row">
        <div class="col text-left">
          <div>
            <b-dropdown id="dropdown-1" v-model="filterProductsSelection" :text="filterProductsSelectionText" class="m-md-2">
              <b-dropdown-item v-for="option in productsFilterOptions" 
              v-bind:value="option.value" 
              :key="option.value"
              v-on:click="productFilterChange(option.value)">
              {{option.text}}
              </b-dropdown-item>
            </b-dropdown>
          </div>
        </div>
        <div class="col">
          <div class="float-right">
          <b-input-group>
            <b-input-group-prepend>
              <b-button variant="outline-info" v-on:click="changeDatesFilter('LAST_12_MONTHS')">Last 12 Months</b-button>
            </b-input-group-prepend>
            <b-input-group-append>
              <b-button variant="outline-info" v-on:click="changeDatesFilter('THIS_YEAR')">This year</b-button>
              <b-button variant="outline-info" v-on:click="changeDatesFilter('THIS_MONTH')">This month</b-button>
            </b-input-group-append>
          </b-input-group>
          </div>
        </div>
        

      </div>
    </div>

    <div class="container-fluid">
      <div class="row">
        <div>&nbsp;</div>
      </div>
    </div>

    <div class="container-fluid">
      <div class="row">
        <TotalSalesCard title="Total sales" :amount="totalAmount" icon="bitcoin" />

        <TotalSalesCard title="Num. policies" :amount="totalPolicies" icon="book" />

        <TotalSalesCard
          v-for="product in products"
          :key="product.code"
          :title="product.name"
          :amount="totalAmounts[product.code]"
          :policiesCount="totalCounts[product.code]"
          :icon="product.icon"
        />
      </div>
    </div>

    <div class="container-fluid">
      <div class="row">
        <div>&nbsp;</div>
      </div>
    </div>

    <div class="container-fluid">
      <div class="card-columns cols-2">
        <SalesTrendsLines :salesTrendsData="salesTrends" />
        <SalesDistribution :productTotals="totalAmounts" />
        <SalesAgents :agentTotals="agentsTotalAmounts" />
      </div>
    </div>
  </div>
</template>

<script>
import { HTTP } from "./http/ApiClient";
import SalesAgents from "./SalesAgents";
import SalesTrendsLines from "./SalesTrendsLines";
import SalesDistribution from "./SalesDistribution";
import TotalSalesCard from "./TotalSalesCard";
import {_} from 'vue-underscore';
import moment from 'moment';
export default {
  name: "Dashboard",
  props: {
    msg: String
  },
  components: {
    SalesAgents,
    SalesTrendsLines,
    SalesDistribution,
    TotalSalesCard
  },
  data() {
    return {
      products: [],
      totalAmounts: {},
      totalCounts: {},
      totalAmount: 0,
      totalPolicies: 0,
      agentsTotalAmounts: {},
      agentsTotalCounts: {},
      salesTrends: [],
      filterProductsSelection: 'ALL',
      filterProductsSelectionText: 'All products',
      productsFilterOptions: [
        {text: 'All products', value: 'ALL'}
      ],
      filteredPeriod: {
        option: 'LAST_12_MONTHS',
        startDate: null,
        endDate: null
      }
    };
  },
  created: function() {
    this.filteredPeriod.endDate = moment();
    this.filteredPeriod.startDate = moment().subtract(12,'months');
    HTTP.get("products").then(response => {
      this.products = response.data;
      this.products.forEach(product => {
        this.productsFilterOptions.push({text: product.name, value: product.code});
      });
      this.fetchTotalSales();
      this.fetchAgentsSales();
      this.fetchSalesTrends();
    });
  },
  methods: {
    fetchTotalSales() {
      HTTP
        .post("dashboard/total-sales", this.setupRequest({}))
        .then(response => {
          this.totalAmount = response.data.total.premiumAmount;
          this.totalPolicies = response.data.total.policiesCount;
          var amounts = {};
          var counts = {};
          for (var prop in response.data.perProductTotal) {
            amounts[prop] = response.data.perProductTotal[prop].premiumAmount;
            counts[prop] = response.data.perProductTotal[prop].policiesCount;
          }
          this.totalAmounts = amounts;
          this.totalCounts = counts;
      });
    },
    fetchAgentsSales() {
      HTTP
        .post("dashboard/agents-sales", this.setupRequest({}))
        .then(response => {
          var amounts = {};
          var counts = {};
          Object.getOwnPropertyNames(response.data.perAgentTotal).forEach(val => {
            amounts[val] = response.data.perAgentTotal[val].premiumAmount;
            counts[val] = response.data.perAgentTotal[val].policiesCount;
          });
          this.agentsTotalAmounts = amounts;
          this.agentsTotalCounts = counts;
      });
    },
    fetchSalesTrends() {
      HTTP
        .post("dashboard/sales-trends", this.setupRequest({}))
        .then(response => {
          this.salesTrends = response.data.periodsSales;
      });
    },
    productFilterChange(option) {
      this.filterProductsSelection = option;
      this.filterProductsSelectionText = _.find(this.productsFilterOptions,i=>i.value===option).text;
      this.fetchTotalSales();
      this.fetchAgentsSales();
      this.fetchSalesTrends();
    },
    changeDatesFilter(option) {
      this.filteredPeriod.option = option;
      if (option === 'LAST_12_MONTHS') {
        this.filteredPeriod.endDate = moment();
        this.filteredPeriod.startDate = moment().subtract(12,'months');
      } else if (option === 'THIS_YEAR') {
        this.filteredPeriod.endDate = moment();
        this.filteredPeriod.startDate = moment().month(0).date(1);
      } else if (option === 'THIS_MONTH') {
        this.filteredPeriod.endDate = moment();
        this.filteredPeriod.startDate = moment().date(1);
      }
      this.fetchTotalSales();
      this.fetchAgentsSales();
      this.fetchSalesTrends();
    },
    setupRequest(request) {
      request.salesDateFrom = moment(this.filteredPeriod.startDate).format('YYYY-MM-DD');
      request.salesDateTo = moment(this.filteredPeriod.endDate).format('YYYY-MM-DD'); 
      request.unit = this.filteredPeriod.option === 'THIS_MONTH' ? 'Week' : 'Month';
      request.productCode = this.filterProductsSelection==='ALL' ? null : this.filterProductsSelection;
      return request;
    }
  }
};
</script>

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