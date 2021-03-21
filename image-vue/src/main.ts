import { createApp } from 'vue';
import App from './App.vue';
import { router } from './router';
import { store, storeKey } from './store';
import BaseButton from './components/base/BaseButton.vue';
import FileUpload from './components/base/FileUpload.vue';
import AccountLifetime from './components/base/AccountLifetime.vue';
import 'tailwindcss/tailwind.css';

createApp(App)
  .use(store, storeKey)
  .use(router)

  .component('BaseButton', BaseButton)
  .component('FileUpload', FileUpload)
  .component('AccountLifeTime', AccountLifetime)
  .mount('#app');
