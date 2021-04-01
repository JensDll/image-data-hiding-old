<template>
  <div class="mb-24">
    <h1 class="text-6xl font-bold">Image Data Hiding</h1>
    <div
      v-if="$store.state.userModule.loggedIn"
      class="flex flex-col items-center"
    >
      <p class="text-xl font-semibold mt-12">
        {{ $store.state.userModule.user.username }}
      </p>
      <p>Account valid for</p>
      <AccountLifeTime class="mb-4" />
      <div class="flex gap-x-4 w-72 mx-auto">
        <BaseButton
          class="w-full px-2 py-2 btn-primary"
          @click="handleLogout()"
        >
          Logout
        </BaseButton>
        <BaseButton
          class="w-full px-2 py-2 btn-danger"
          @click="handleDeleteAccount()"
        >
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
import { authModuleActions } from '../store/modules/authModule';

export default defineComponent({
  methods: {
    async handleLogout() {
      this.$router.push({ name: 'home' });
      await this.logout();
    },
    async handleDeleteAccount() {
      this.$router.push({ name: 'home' });
      await this.deleteAccount();
    },
    ...authModuleActions
  }
});
</script>

<style scoped>
.main {
  width: 100%;
  max-width: 700px;
}
</style>
