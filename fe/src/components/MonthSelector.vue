<template>  
  <md-input-container>
    <label for="monthValue">{{ label }}</label>
    <md-select name="monthValue" id="monthValue" v-model="monthValue">
      <md-option v-for="month in static.months" :key="month.id" :value="month.id">{{ month.value }}</md-option>      
    </md-select>
  </md-input-container>       
</template>

<script>   
  import Static from '@/common'

  export default {
    name: 'month-selector',  
    props: ['label'], 
    data: function () {
      return {    
          static: Static,
          monthValue: null
        }
    },    
    watch: {    
      monthValue: function (value) {           
        this.$emit('input', value);
      }      
    }
  }
</script>
