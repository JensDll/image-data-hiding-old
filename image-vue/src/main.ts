import { createApp } from 'vue';
import App from './App.vue';
import { router } from './router';
import { store, storeKey } from './store';
import BaseButton from './components/base/BaseButton.vue';
import 'tailwindcss/tailwind.css';

createApp(App)
  .use(router)
  .use(store, storeKey)
  .component('BaseButton', BaseButton)
  .mount('#app');
