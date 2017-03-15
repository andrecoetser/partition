<script>
import { Bar } from 'vue-chartjs'

export default Bar.extend({
  name: 'bar-chart',
  props: ['heading','xaxis', 'yaxis'],
  mounted () {
    this.renderChart({    
      labels: this.xaxis,
      datasets: [
        {
          label: this.heading,
          backgroundColor: '#3f51b5',
          data: this.yaxis
        }
      ]      
    }, 
    {
      responsive: true,
      maintainAspectRatio: false
    })
  }
})

</script>