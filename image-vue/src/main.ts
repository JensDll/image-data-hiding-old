import { createApp } from 'vue';
import App from './App.vue';
import { router } from './router';
import BaseButton from './components/BaseButton.vue';
import 'tailwindcss/tailwind.css';

createApp(App).use(router).component('BaseButton', BaseButton).mount('#app');
