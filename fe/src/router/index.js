import Vue from 'vue'
import Router from 'vue-router'
import Cards from '@/pages/Cards'
import TimeProductStore from '@/pages/TimeProductStore'
import ProductStore from '@/pages/ProductStore'

Vue.use(Router);

export default new Router({
  mode: 'history',
  routes: [
    {
      path: '/',
      name: 'Cards',
      component: Cards    
    },
    {
      path: '/TimeProductStore/:dimension',
      name: 'TimeProductStore',
      component: TimeProductStore
    },
    {
      path: '/ProductStore',
      name: 'ProductStore',
      component: ProductStore
    }
  ]
})
