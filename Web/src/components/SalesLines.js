import { Line, mixins } from 'vue-chartjs';
const { reactiveProp } = mixins;

export default {
  extends: Line,
  mixins: [reactiveProp],
  data: () => ({
    options: {
      responsive: true,
      maintainAspectRatio: false,
      scales: {
          xAxes: [{
            barPercentage: 0.5,
            minBarLength: 2,
            gridLines: {
                offsetGridLines: true
            }
          }],
          yAxes: [{
              ticks: {
                        beginAtZero:true,
                        min: 0    
                    }
          }]
      }
    }
  }),

  mounted () {
    this.renderChart(this.chartdata, this.options)
  }
}