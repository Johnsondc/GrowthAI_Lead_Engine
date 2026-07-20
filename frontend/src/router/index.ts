import { createRouter, createWebHistory } from 'vue-router';
import Dashboard from '../views/Dashboard.vue';
import LeadPool from '../views/LeadPool.vue';
import ContentCenter from '../views/ContentCenter.vue';
import LandingManager from '../views/LandingManager.vue';
import Analytics from '../views/Analytics.vue';
import Settings from '../views/Settings.vue';

export default createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/dashboard' },
    { path: '/dashboard', component: Dashboard },
    { path: '/leads', component: LeadPool },
    { path: '/content', component: ContentCenter },
    { path: '/landing', component: LandingManager },
    { path: '/analytics', component: Analytics },
    { path: '/settings', component: Settings }
  ]
});
