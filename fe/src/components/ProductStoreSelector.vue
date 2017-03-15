<template>
  <div class="row">          
    <div class="col-md-4 radios" v-if="propMissing">
      <md-radio v-model="productOrStore" id="product" name="productOrStore" :md-value="static.product" class="md-primary">{{ static.product }}</md-radio>
      <md-radio v-model="productOrStore" id="store" name="productOrStore" :md-value="static.store" class="md-primary">{{ static.store }}</md-radio>
    </div>      
    <div class="col-md-4 radios">
      <md-radio v-model="allOrOne" id="all" name="allOrOne" :md-value="static.all" class="md-primary">{{ static.all }}</md-radio>
      <md-radio v-model="allOrOne" id="one" name="allOrOne" :md-value="static.one" class="md-primary">{{ static.one }}</md-radio>
    </div>
    <transition name="fade">
      <div v-if="computedIsOneSelected" class="col-md-4">
        <md-input-container :class="computedProductStoreNumberValidClass">
          <label>{{productOrStore}} number:</label>
          <md-input v-model="allOrOneValue"></md-input>
          <span class="md-error">Number must be between 1 and {{computedStoreProductNumber}}</span>
        </md-input-container>
      </div>
    </transition>
  </div>
</template>

<script> 
  import Static from '@/common'

  export default {
    name: 'product-store-selector',   
    props: ['productStore'],
    data: function () {
      return {    
          static: Static,
          allOrOne: '',
          productOrStore: '',
          allOrOneValue: null,
          allOrOneDirty: false,
          propMissing: true
        }
    },    
    mounted: function () {
      this.allOrOne = this.static.all;

      if (this.productStore){
        this.propMissing = false;
        this.productOrStore = this.productStore;
      } else {
        this.productOrStore = this.static.product;
      }      
    },
    watch: {    
      allOrOne: function (value) {   
        if (this.isOneSelected() && !this.allOrOneDirty) {           
          this.$emit('input', null)
        } else {
          this.complete()          
        }
      },
      allOrOneValue: function () {  
        this.allOrOneDirty = true;
        if (this.productStoreNumberValid()) {           
          this.complete()    
        } else {
          this.$emit('input', null)
        }
      },
      productOrStore: function (value) {         
        if (!this.isOneSelected()) {           
          this.complete()    
        } else {
          if (this.productStoreNumberValid() && this.allOrOneDirty) {
             this.complete() 
           } else {
            this.$emit('input', null)
          }
        }
      }
    },
    created: function () {
      this.complete();
    },
    methods: {
      timeRangeTypeValue:  function (value) { 
        this.timeRangeType = value;
      },
      complete: function () {
        this.$emit('input', {productOrStore : this.productOrStore, allOrOne: this.allOrOne, allOrOneValue: this.allOrOneValue});
      },
      isOneSelected: function () {       
        return this.allOrOne === this.static.one;
      },
      productStoreNumberValid: function () {     
        return !this.allOrOneDirty || (parseInt(this.allOrOneValue) > 0 && parseInt(this.allOrOneValue) <= this.computedStoreProductNumber);
      },
    },    
    computed: {     
      computedIsOneSelected: function () {
        return this.isOneSelected();
      },
      computedStoreProductNumber: function () {
        return this.productOrStore === this.static.product ? this.static.productCount : this.static.storeCount;
      },
      computedProductStoreNumberValidClass: function () {
        return this.productStoreNumberValid() ? 'md-input-valid' : 'md-input-invalid';
      }      
    }
  }
</script>


