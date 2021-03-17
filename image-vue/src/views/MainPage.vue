<template>
  <div class="mb-24">
    <h1 class="text-6xl font-bold">Image Data Hiding</h1>
    <div v-if="loggedIn">
      <div class="mt-12 mb-4 text-center">
        Logged in as <span class="text-lg font-semibold">{{ username }}</span>
      </div>
      <div class="flex gap-x-4 w-72 mx-auto">
        <BaseButton class="w-full px-2 py-2 btn-primary">Logout</BaseButton>
        <BaseButton class="w-full px-2 py-2 btn-danger">
          Delete Account
        </BaseButton>
      </div>
    </div>
  </div>
  <main class="main">
    <router-view />
  </main>
</template>

<script lang="ts">
import { defineComponent } from '@vue/runtime-core';
import { userModuleState } from '../store/modules/userModule';

export default defineComponent({
  computed: {
    ...userModuleState
  }
});
</script>

<style scoped>
.main {
  width: 100%;
  max-width: 700px;
}
</style>
