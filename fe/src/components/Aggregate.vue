<template>
  <div class="row">     
      <div class="col-md-6">
        <md-input-container>
          <label for="field">Select field</label>
          <md-select name="field" id="field" v-model="field">
            <md-option value="Profit">Profit</md-option>
            <md-option value="SalePrice">Sale price</md-option>
            <md-option value="CostPrice">Cost price</md-option>
          </md-select>
        </md-input-container> 
      </div>
      <div class="col-md-6">
        <md-input-container>
          <label for="aggregateType">Select aggregate</label>
          <md-select name="aggregateType" id="aggregateType" v-model="aggregateType">
            <md-option value="Min">Min</md-option>
            <md-option value="Max">Max</md-option>
            <md-option value="Sum">Sum</md-option>
            <md-option value="Avg">Avg</md-option>
          </md-select>
        </md-input-container> 
      </div>    
  </div>
</template>

<script> 
  export default {
    name: 'aggregate',  
    props: ['aggregateValue'], 
    data: function () {
      return {    
          field: this.aggregateValue != null ? this.aggregateValue.field : null,
          aggregateType: this.aggregateValue != null ? this.aggregateValue.aggregateType : null,
        }
    },    
    watch: {    
      aggregateType: function (value) {     
        if (this.field != null) {      
          this.complete();
        }
      },
      field: function (value) { 
        if (this.aggregateType != null) {            
          this.complete();
        }
      }
    },
    methods: {     
      complete: function () {
        this.$emit('input', { aggregateType : this.aggregateType, field: this.field });
      }
    }
  }
</script>

