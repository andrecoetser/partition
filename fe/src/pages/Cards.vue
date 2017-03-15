<template>
    <div class="row">
        <div class="col-md-4 card" v-on:click="navigate('/TimeProductStore/Product', $event)">
            <md-card md-with-hover>
                <md-card-header>
                    <div class="md-title">Time and Product</div>
                    <div class="md-subhead">View product sales data over time</div>
                </md-card-header>

                <md-card-content>
                    Choose a time-period and a product, single or combined.
                </md-card-content>

                <md-card-actions>
                    <md-button class="md-raised md-primary">Explore</md-button>
                </md-card-actions>
            </md-card>
        </div>
        <div class="col-md-4 card" v-on:click="navigate('/TimeProductStore/Store', $event)">
            <md-card md-with-hover>
                <md-card-header>
                    <div class="md-title">Time and Store</div>
                    <div class="md-subhead">View store sales data over time</div>
                </md-card-header>

                <md-card-content>
                    Choose a time-period and a store, single or combined.
                </md-card-content>

                <md-card-actions>
                    <md-button class="md-raised md-primary">Explore</md-button>
                </md-card-actions>
            </md-card>
        </div>
        <div id="product" class="col-md-4 card" v-on:click="navigate('/ProductStore', $event)">
            <md-card md-with-hover>
                <md-card-header>
                    <div class="md-title">Product and Store</div>
                    <div class="md-subhead">View store/product sales data </div>
                </md-card-header>

                <md-card-content>
                    Choose a point in time to view ranked stores or products.
                </md-card-content>

                <md-card-actions>
                    <md-button class="md-raised md-primary">Explore</md-button>
                </md-card-actions>
            </md-card>
        </div>
    </div>
</template>

<script>

export default {
  name: 'cards',
  methods: {
    navigate (dimension, event) {
      this.$router.push(dimension);  
    }
  }
}
</script>


<style scoped>
    .card {
        margin-bottom: 30px;
    }
</style>
