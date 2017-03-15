<template>
  <md-input-container>
    <label for="timeRangeType">Time range type</label>
    <md-select name="timeRangeType" id="timeRangeType" v-model="timeRangeType">
      <md-option :value="static.year">Years</md-option>
      <md-option :value="static.month">Months</md-option>
    </md-select>
  </md-input-container> 
</template>

<script> 
  import Static from '@/common'

  export default {
    name: 'time-range-type-selector',   
    data: function () {
      return {    
          timeRangeType: '',
          static: Static
        }
    },    
    watch: {    
      timeRangeType: function (value) {   
        this.$emit('input', value);
      }
    }
  }
</script>
