<template>
  <section class="hero">
    <h1 class="text-6xl font-bold mb-24">Image Data Hiding</h1>
    <CallToAction />
  </section>
</template>

<script setup lang="ts">
import CallToAction from '../components/call-to-action/CallToAction.vue';
</script>

<style scoped>
.hero {
  padding-top: 15vh;
  width: 100%;
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style>
