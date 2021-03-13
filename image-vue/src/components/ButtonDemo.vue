<template>
  <div>
    <BaseButton class="button">Default</BaseButton>
    <BaseButton class="button" type="primary">Primary</BaseButton>
    <BaseButton class="button" type="danger">Danger</BaseButton>
    <BaseButton class="button" disabled>Default</BaseButton>
    <BaseButton class="button" type="primary" disabled>Primary</BaseButton>
    <BaseButton class="button" type="danger" disabled>Danger</BaseButton>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({});
</script>

<style scoped>
.button {
  @apply px-9 py-3 mt-4;
}
</style>
