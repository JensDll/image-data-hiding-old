<template>
  <p>{{ days }} : {{ hours }} : {{ minutes }} : {{ seconds }}</p>
</template>

<script lang="ts">
import { defineComponent } from '@vue/runtime-core';
import { useStore } from '../../store';
import { useCountdown } from '../../composition/useCountdown';
import { useRouter } from 'vue-router';

export default defineComponent({
  name: 'AccountLifeTime',
  setup() {
    const store = useStore();
    const router = useRouter();

    const state = useCountdown(
      store.state.userModule.user.deletionDate,
      async () => {
        router.push({ name: 'home' });
        store.commit('userModule/RESET_STATE');
        store.commit('accountModule/RESET_STATE');
      }
    );

    return { ...state };
  }
});
</script>
