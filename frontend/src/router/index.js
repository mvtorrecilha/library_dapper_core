import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import About from '../views/About.vue'
import Login from '../views/Login.vue'

Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/login',
    name: 'Login',
    component: Login
  },
  {
    path: '/about',
    name: 'About',
    component: About
  }
]


const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

//apply route guard  
router.beforeEach((to, from, next) => {
  const protectedRoutes = ['/'];

  if (protectedRoutes.includes(to.path)) {
    if (!localStorage.getItem('loggedUser'))
      return next('/login');
  }
  
  return next();
});

export default router
