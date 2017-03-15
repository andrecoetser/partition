<template>  
  <md-input-container>
    <label for="yearValue">{{ label }}</label>
    <md-select name="yearValue" id="yearValue" v-model="yearValue">      
        <md-option v-for="year in static.years" :key="year" :value="year">{{ year }}</md-option>    
    </md-select>
  </md-input-container>        
</template>

<script>   
  import Static from '@/common'

  export default {
    name: 'year-selector',   
    props: ['label'],
    data: function () {
      return {    
          static: Static,
          yearValue: null
        }
    },    
    watch: {    
      yearValue: function (value) {           
        this.$emit('input', value);
      }      
    }
  }
</script>
