import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import CallToAction from '../components/hero/CallToAction.vue';
import LoginForm from '../components/hero/LoginForm.vue';
import RegisterForm from '../components/hero/RegisterForm.vue';
import Main from '../components/encode-decode/Main.vue';
import { store } from '../store';
import ErrorPage from '../views/ErrorPage.vue';

const routes: RouteRecordRaw[] = [
  {
    path: '/',
    name: 'home',
    component: CallToAction
  },
  {
    path: '/error',
    name: 'name',
    component: ErrorPage
  },
  {
    path: '/login',
    name: 'login',
    component: LoginForm
  },
  {
    path: '/register',
    name: 'register',
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

router.beforeEach(to => {
  const userLoggedIn = store.state.accountModule.loggedIn;

  if (
    typeof to.name === 'string' &&
    /home|login|register|error/.test(to.name) &&
    userLoggedIn
  ) {
    store.dispatch('accountModule/logout');
  }

  if (to.name === 'main' && !userLoggedIn) {
    return '/';
  }
});
