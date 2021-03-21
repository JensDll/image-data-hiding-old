<template>
  <section class="flex flex-col items-center">
    <h1 class="text-4xl font-semibold mb-10">An unexpected Error occured!</h1>
    <BaseButton class="btn-primary px-16 py-4" @click="$router.push('/')">
      Go Home
    </BaseButton>
  </section>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({});
</script>
