import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/Login.vue')
    },
    {
      path: '/',
      component: () => import('../layouts/AdminLayout.vue'),
      meta: { requiresAuth: true },
      children: [
        { path: '', redirect: '/dashboard' },
        { path: 'dashboard', name: 'Dashboard', component: () => import('../views/Dashboard.vue') },
        { path: 'leads', name: 'Leads', component: () => import('../views/Leads.vue') },
        { path: 'contents', name: 'Contents', component: () => import('../views/Contents.vue') },
        { path: 'sources', name: 'Sources', component: () => import('../views/Sources.vue') },
        { path: 'landing-pages', name: 'LandingPages', component: () => import('../views/LandingPages.vue') },
        { path: 'settings', name: 'Settings', component: () => import('../views/Settings.vue') },
      ]
    }
  ]
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  if (to.meta.requiresAuth && !auth.token) {
    next('/login')
  } else {
    next()
  }
})

export default router
