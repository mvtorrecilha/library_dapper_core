import Axios from 'axios'
import Vue from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify';

Vue.config.productionTip = false

Axios.defaults.baseURL = process.env.VUE_APP_URL_SERVER_BASE
Axios.defaults.headers.get['Pragma'] = 'no-cache'

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app')