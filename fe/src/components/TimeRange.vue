<template>
  <div>
    <div class="row"> 
      <div class="col-md-4">
        <time-range-type-selector v-model="timeRangeType"></time-range-type-selector> 
      </div>
      <transition name="fade">        
        <div v-if="isTypeSelected" class="col-md-4">
          <year-selector :label="yearLabel" v-model="yearStartValue"></year-selector>
        </div>
      </transition>
      <transition name="fade"> 
        <div v-if="isTimeRangeYearSelected" class="col-md-4">
          <year-selector label="Select end year" v-model="yearEndValue"></year-selector>
        </div>
      </transition>
    </div>
    <transition name="fade"> 
      <div v-if="isTimeRangeMonthSelected" class="row">             
        <div class="col-md-4">
          <month-selector label="Select start month" v-model="monthStartValue"></month-selector>
        </div>
        <div class="col-md-4">
          <month-selector label="Select end month" v-model="monthEndValue"></month-selector>
        </div>
      </div> 
    </transition>       
  </div>
</template>

<script> 
  import TimeRangeTypeSelector from './TimeRangeTypeSelector'
  import Static from '@/common'
  import YearSelector from './YearSelector'
  import MonthSelector from './MonthSelector'

  export default {
    name: 'time-range',   
    data: function () {
      return {    
          static: Static,
          timeRangeType: null,
          yearStartValue: null,
          yearEndValue: null,
          monthStartValue: null,
          monthEndValue: null,
          yearLabel: ''
        }
    },    
    watch: {    
      yearStartValue: function () {           
        this.complete();
      },
      monthStartValue: function () {       
        this.complete();
      },
      yearEndValue: function () {           
        this.complete();
      },
      monthEndValue: function () {       
        this.complete();
      },
      timeRangeType: function () {                   
        this.yearLabel = 'Select ' + (this.isTimeRangeYearSelected ? 'start ' : '') + 'year';
        this.yearEndValue = null;
        this.monthStartValue = null;
        this.monthEndValue = null;
      }
    },
    methods: {     
      complete: function () {
        if ((this.isTimeRangeMonthSelected && this.isStartYearSelected && this.isStartMonthSelected && this.isEndMonthSelected) || 
           (this.isTimeRangeYearSelected && this.isStartYearSelected && this.isEndYearSelected)) {
           this.$emit('input', {timeRangeType : this.timeRangeType, yearStartValue: this.yearStartValue, monthStartValue: this.monthStartValue, yearEndValue: this.yearEndValue, monthEndValue: this.monthEndValue});
        } else {
          this.$emit('input', null);
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
      isStartMonthSelected: function () {
        return this.monthStartValue != null;
      },
      isStartYearSelected: function () {
        return this.yearStartValue != null;
      },
      isEndMonthSelected: function () {
        return this.monthEndValue != null;
      },
      isEndYearSelected: function () {
        return this.yearEndValue != null;
      },
      isTypeSelected: function () {
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
