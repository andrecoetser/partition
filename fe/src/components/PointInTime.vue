<template>
  <div class="row"> 
    <div class="col-md-4">
      <time-range-type-selector v-model="timeRangeType"></time-range-type-selector> 
    </div>
    <transition name="fade">
      <div>
        <div v-if="isSomethingSelected" class="col-md-4">
         <year-selector label="Select year" v-model="yearValue"></year-selector>
        </div>
        <div v-if="isTimeRangeMonthSelected" class="col-md-4">
         <month-selector label="Select month" v-model="monthValue"></month-selector>
        </div>
      </div>
    </transition>
  </div>
</template>

<script> 
  import TimeRangeTypeSelector from './TimeRangeTypeSelector'
  import YearSelector from './YearSelector'
  import MonthSelector from './MonthSelector'
  import Static from '@/common'

  export default {
    name: 'point-in-time',   
    data: function () {
      return {    
          static: Static,
          timeRangeType: null,
          yearValue: null,
          monthValue: null
        }
    },    
    watch: {    
      yearValue: function () {           
        this.complete();
      },
      monthValue: function () {       
        this.complete();
      },
      timeRangeType: function () {           
        if (this.isTimeRangeMonthSelected) {
          this.monthValue = null;
          
          this.$emit('input', null);
        } else {
          this.complete();
        }
      }
    },
    methods: {     
      complete: function () {
        if (this.isYearSelected && (this.isTimeRangeYearSelected || this.isMonthSelected)) {
          this.$emit('input', {timeRangeType : this.timeRangeType, yearValue: this.yearValue, monthValue: this.monthValue});
        }
      }
    },    
    computed: {
      isTimeRangeYearSelected: function () {    
        return this.timeRangeType === this.static.year;
      },
      isTimeRangeMonthSelected: function () {
        return this.timeRangeType === this.static.month;
      },
      isMonthSelected: function () {
        return this.monthValue != null;
      },
      isYearSelected: function () {
        return this.yearValue != null;
      },
      isSomethingSelected: function () {
        return this.isTimeRangeYearSelected ||  this.isTimeRangeMonthSelected;
      }
    },
    components: {      
      TimeRangeTypeSelector,
      YearSelector,
      MonthSelector
    }
  }
</script>
