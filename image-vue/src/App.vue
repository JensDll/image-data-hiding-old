<template>
  <MainPage />
</template>

<script setup lang="ts">
import MainPage from './views/MainPage.vue';
</script>

<style>
#app {
  height: 100vh;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 15vh;
}

@layer base {
  textarea {
    min-height: 130px;
  }
}

@layer components {
  .btn-emerald {
    @apply bg-emerald-500 text-white;
    @apply hover:bg-opacity-80;
  }

  .btn-emerald:focus {
    @apply ring-2 ring-emerald-700;
  }

  .btn-danger {
    @apply bg-red-500 text-white;
    @apply hover:bg-opacity-80;
  }

  .btn-danger:focus {
    @apply ring-2 ring-red-700;
  }

  .btn-primary {
    @apply bg-emerald-500 text-white;
    @apply hover:bg-opacity-80;
  }

  .btn-primary:focus {
    @apply ring-2 ring-emerald-700;
  }

  .btn-default {
    @apply bg-amber-50 text-amber-700;
    @apply hover:bg-amber-100;
  }

  .btn-default:focus {
    @apply ring-2 ring-amber-700;
  }
}
</style>
