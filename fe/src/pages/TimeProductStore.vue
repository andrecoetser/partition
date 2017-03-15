<template>
  <div>
    <div class="row">
      <div class="col-md-12 text-center"> 
        <span class="md-display-1">Time and {{ ProductStore }}</span>              
      </div>                
    </div>   
    <hr>      
    <div class="control-container">
      <div class="row"> 
        <div class="col-md-12">
         <time-range v-model="timeRangeValue"></time-range>
        </div>          
      </div> 
      <transition name="fade">
        <div v-if="computedTimeRangeComplete">
          <div class="row"> 
            <div class="col-md-12">
              <product-store-selector :product-store="ProductStore" v-model="productStoreSelectorValue"></product-store-selector>
            </div>          
          </div> 
          <transition name="fade">
            <div v-if="computedProductStoreSelectorComplete">
              <div class="row">                 
                <div class="col-md-8">
                  <aggregate :aggregateValue="aggregateValue" v-model="aggregateValue"></aggregate>
                </div>          
              </div> 
              <div v-if="computeAggregateComplete">
                <div class="row">                  
                  <div v-on:click="submitQuery">
                    <md-button class="md-raised md-primary">Submit query</md-button>
                  </div> 
                </div>               
              </div>
            </div>  
          </transition>             
        </div>  
      </transition>    
      <transition name="fade">
        <div v-if="renderChart" class="product-store-chart">
          <div class="row"> 
            <div class="col-md-12">
              <bar-chart :heading="chartHeading" :xaxis="xaxis" :yaxis="yaxis"></bar-chart>
            </div>
          </div>
        </div>
      </transition>   
      <div class="error-text" v-if="isError">
        <div class="row"> 
          <div class="col-md-12">
            The backend is not currently connected.
          </div>      
        </div> 
      </div>     
    </div>    
  </div>      
</template>

<script>
  import State from '@/state'
  import TimeRange from '@/components/TimeRange'
  import ProductStoreSelector from '@/components/ProductStoreSelector'
  import Aggregate from '@/components/Aggregate'
  import moment from 'moment'
  import BarChart from '@/components/BarChart'
  import Static from '@/common'

  export default {
    name: 'timeproductstore',
    data: function () {
      return {    
          ProductStore: '',
          isProduct: true,
          sharedState: State,
          static: Static,
          timeRangeValue: null,
          productStoreSelectorValue: null,    
          aggregateValue: null,
          isError: false,
          xaxis: [],
          yaxis: [],
          chartHeading: null,
          renderChart: false
        }
    },
    created: function () {
      this.ProductStore = this.$route.params.dimension;
      this.isProduct = this.ProductStore === 'Product';                
      this.sharedState.isChildPage = true;
    },
     methods: {  
      submitQuery: function() {
        this.renderChart = false;
        this.sharedState.isLoading = true;
        this.isError = false;
        var fromDate, toDate;

        if (this.timeRangeValue.timeRangeType === this.static.year) {
            var yearStartValue = parseInt(this.timeRangeValue.yearStartValue);
            var yearEndValue = parseInt(this.timeRangeValue.yearEndValue);

            if (yearStartValue > yearEndValue) {
              var tempYear = yearStartValue;
              yearStartValue = yearEndValue;
              yearEndValue = tempYear;
            }

            fromDate = new Date(yearStartValue, 0, 1);
            toDate = new Date(yearEndValue + 1, 0, 1);
        } else {
            
            var monthStartValue = parseInt(this.timeRangeValue.monthStartValue);
            var monthEndValue = parseInt(this.timeRangeValue.monthEndValue);
            var yearValue = parseInt(this.timeRangeValue.yearStartValue);

            if (monthStartValue > monthEndValue) {
              var tempYear = monthStartValue;
              monthStartValue = monthEndValue;
              monthEndValue = tempYear;
            }
    
            fromDate = new Date(yearValue, monthStartValue, 1);

            if (monthEndValue === 11) {
              monthEndValue = 0;
              yearValue = yearValue + 1;
            } else {
              monthEndValue = monthEndValue + 1;
            }

            toDate = new Date(yearValue, monthEndValue, 1);
        }        

        this.$http.get(this.static.url + this.static.timeRange +  this.static.objectToQuerystring({
          DateType: this.timeRangeValue.timeRangeType,
          FromDate: moment(fromDate).format(this.static.dateFormat),
          ToDate: moment(toDate).format(this.static.dateFormat),
          DimensionType: this.productStoreSelectorValue.productOrStore,
          DimensionTypeValue: this.productStoreSelectorValue.allOrOne === this.static.one ? parseInt(this.productStoreSelectorValue.allOrOneValue) : 0, 
          DataPointType: this.aggregateValue.field,
          AggregateType: this.aggregateValue.aggregateType
        })).then(function (response) {  
          this.chartHeading = this.aggregateValue.field + ' ($)';     
          
          this.xaxis = [];
          this.yaxis = [];
          
          for (var i = 0; i < response.body.length; i++) {          
            this.xaxis.push(this.timeRangeValue.timeRangeType === this.static.year ? response.body[i].dateType : this.static.months[parseInt(response.body[i].dateType) - 1].value);
            this.yaxis.push(parseFloat(response.body[i].dataPoint).toFixed(2));
          }          

          this.sharedState.isLoading = false;
          this.renderChart = true;
        }, function () {
          this.isError = true;
          this.sharedState.isLoading = false;
        });
      }
    },  
    computed: {
      computedProductOrStore: function () {        
        return this.productStoreSelectorValue.productOrStore === this.static.product ? this.static.store : this.static.product;
      },
      computeAggregateComplete: function () {
        return this.aggregateValue;    
      },
      computedTimeRangeComplete: function () {
        return this.timeRangeValue;
      },
      computedProductStoreSelectorComplete: function () {
        return this.productStoreSelectorValue;
      }
    },
    components: {      
      TimeRange,
      ProductStoreSelector,
      Aggregate,
      BarChart
    }
  }
</script>
