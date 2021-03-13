import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import HeroSection from '../views/HeroSection.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: HeroSection
  }
];

export const router = createRouter({
  history: createWebHistory(),
  routes
});
