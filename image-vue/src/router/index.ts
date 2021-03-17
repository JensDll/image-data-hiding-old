import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import CallToAction from '../components/hero/CallToAction.vue';
import LoginForm from '../components/hero/LoginForm.vue';
import RegisterForm from '../components/hero/RegisterForm.vue';
import Main from '../components/encode-decode/Main.vue';
import { store } from '../store';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    component: CallToAction
  },
  {
    path: '/login',
    component: LoginForm
  },
  {
    path: '/register',
    component: RegisterForm
  },
  {
    path: '/main',
    name: 'main',
    component: Main
  }
];

export const router = createRouter({
  history: createWebHistory(),
  routes
});

router.beforeEach((to, from) => {
  const userLoggedIn = store.state.userModule.loggedIn;

  if (to.name === 'main' && !userLoggedIn) {
    return '/';
  }
});
