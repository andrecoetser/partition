import Vue from 'vue'
import App from './App'
import router from './router'
import VueMaterial from 'vue-material'
import VueResource from  'vue-resource'
import Moment from 'vue-moment'
import VueCharts from 'vue-chartjs'
import { Bar, Line } from 'vue-chartjs'


Vue.config.productionTip = false;

Vue.use(VueMaterial);
Vue.use(VueResource);

new Vue({
    el: '#app',
    router,
    template: '<App/>',
    components: { App }
})
