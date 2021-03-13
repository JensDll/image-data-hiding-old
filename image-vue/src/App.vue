<template>
  <router-view />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ButtonDemo from './components/ButtonDemo.vue';

export default defineComponent({
  components: { ButtonDemo }
});
</script>

<style>
#app {
  height: 100vh;
}
</style>
