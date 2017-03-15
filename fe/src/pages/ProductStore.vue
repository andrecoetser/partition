<template>
  <div>
    <div class="row">
      <div class="col-md-12 text-center"> 
        <span class="md-display-1">Product and Store</span>              
      </div>                
    </div>   
    <hr> 
    <div class="control-container">
      <div class="row"> 
        <div class="col-md-12">
         <point-in-time v-model="pointInTimeValue"></point-in-time>
        </div>          
      </div> 
      <transition name="fade">
        <div v-if="computedPointInTimeComplete">
          <div class="row"> 
            <div class="col-md-12">
              <product-store-selector v-model="productStoreSelectorValue"></product-store-selector>
            </div>          
          </div> 
          <transition name="fade">
            <div v-if="computedProductStoreSelectorComplete">
              <div class="row"> 
                <div class="col-md-4">
                   <md-input-container>
                    <label for="rank">{{ computedProductOrStore }} rank</label>
                    <md-select name="rank" id="rank" v-model="rank">
                      <md-option value="desc">Top 10</md-option>
                      <md-option value="asc">Bottom 10</md-option>                
                    </md-select>
                  </md-input-container> 
                </div>          
                <div class="col-md-8">
                  <aggregate :aggregateValue="aggregateValue" v-model="aggregateValue"></aggregate>
                </div>          
              </div> 
              <div v-if="computeAggregateComplete && computeRankComplete">
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
  import PointInTime from '@/components/PointInTime'
  import ProductStoreSelector from '@/components/ProductStoreSelector'
  import Aggregate from '@/components/Aggregate'
  import moment from 'moment'
  import BarChart from '@/components/BarChart'
  import Static from '@/common'

  export default {
    name: 'productstore',
    data: function () {  
      return {
          sharedState: State,
          static: Static,
          pointInTimeValue: null,
          productStoreSelectorValue: null,
          rank: null,
          aggregateValue: null,
          isError: false,
          xaxis: [],
          yaxis: [],
          chartHeading: null,
          renderChart: false
        }
    },
    created: function () {
      this.sharedState.isChildPage = true;
    },
    methods: {     
      submitQuery: function() {
        this.renderChart = false;
        this.sharedState.isLoading = true;
        this.isError = false;
        var fromDate, toDate;

        if (this.pointInTimeValue.timeRangeType === this.static.year) {
            var yearValue = parseInt(this.pointInTimeValue.yearValue);

            fromDate = new Date(yearValue, 0, 1);
            toDate = new Date(yearValue + 1, 0, 1);
        } else {
            
            var monthValue = parseInt(this.pointInTimeValue.monthValue);
            var yearValue = parseInt(this.pointInTimeValue.yearValue);
    
            fromDate = new Date(yearValue, monthValue, 1);

            if (monthValue === 11) {
              monthValue = 0;
              yearValue = yearValue + 1;
            } else {
              monthValue = monthValue + 1;
            }

            toDate = new Date(yearValue, monthValue, 1);
        }        

        this.$http.get(this.static.url + this.static.productStore +  this.static.objectToQuerystring({
          DataOrder: this.rank,
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
            this.xaxis.push(response.body[i].dimension);
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
      computeRankComplete: function () {
        return this.rank;    
      },
      computedPointInTimeComplete: function () {
        return this.pointInTimeValue;
      },
      computedProductStoreSelectorComplete: function () {
        return this.productStoreSelectorValue;
      }
    },
    components: {      
      PointInTime,
      ProductStoreSelector,
      Aggregate,
      BarChart
    }
  }
</script>

