<template>
  <div class="mb-24">
    <h1 class="text-6xl font-bold">Image Data Hiding</h1>
    <div v-if="loggedIn">
      <div class="mt-12 mb-4 text-center">
        Logged in as <span class="text-lg font-semibold">{{ username }}</span>
      </div>
      <DeleteTimer />
      <div class="flex gap-x-4 w-72 mx-auto">
        <BaseButton
          class="w-full px-2 py-2 btn-primary"
          @click="handleLogout()"
        >
          Logout
        </BaseButton>
        <BaseButton class="w-full px-2 py-2 btn-danger" @click="handleDelete()">
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
import { authClient } from '../services/apiClient';
import {
  accountModuleState,
  accountModuleActions
} from '../store/modules/userModule';
import DeleteTimer from '../components/DeleteTimer.vue';

export default defineComponent({
  components: { DeleteTimer },
  computed: {
    ...accountModuleState
  },
  methods: {
    async handleLogout() {
      this.$router.push({ name: 'home' });
      await this.logout();
      await authClient
        .useFetch({ immediat: true })
        .execute('/identity/account/logout')
        .delete()
        .none().promise;
    },
    async handleDelete() {
      this.$router.push({ name: 'home' });
      await this.logout();
      await authClient
        .useFetch({ immediat: true })
        .execute('/identity/account/delete')
        .delete()
        .none().promise;
    },
    ...accountModuleActions
  }
});
</script>

<style scoped>
.main {
  width: 100%;
  max-width: 700px;
}
</style>
