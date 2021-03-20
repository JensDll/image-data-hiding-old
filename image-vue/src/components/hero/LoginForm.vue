<template>
  <form class="form" @submit.prevent="handleLogin()">
    <label>
      <div class="mb-3 font-medium">Username</div>
      <input
        v-model="form.username.$value"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        :class="{
          'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500': loginHasError
        }"
        type="text"
        @input="loginHasError = false"
      />
      <p v-if="loginHasError" class="text-red-400 text-sm mt-2">
        Login information is not correct
      </p>
    </label>
    <label>
      <div class="mb-3 font-medium">Password</div>
      <input
        v-model="form.password.$value"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        :class="{
          'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500': loginHasError
        }"
        type="password"
        @input="loginHasError = false"
      />
      <p v-if="loginHasError" class="text-red-400 text-sm mt-2">
        Login information is not correct
      </p>
    </label>

    <div class="flex gap-x-8 mt-6">
      <BaseButton
        class="px-6 py-4 w-full btn-primary"
        html-type="submit"
        type="primary"
        :disabled="loading"
      >
        Login
      </BaseButton>
      <BaseButton
        class="px-6 py-4 w-full btn-default"
        @click="$router.push('/')"
      >
        Back
      </BaseButton>
    </div>
  </form>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import { Field, useValidation } from 'vue3-form-validation';
import {
  accountModuleActions,
  accountModuleState
} from '../../store/modules/userModule';

type FormData = {
  username: Field<string>;
  password: Field<string>;
};

export default defineComponent({
  setup() {
    const validation = useValidation<FormData>({
      username: {
        $value: ''
      },
      password: {
        $value: ''
      }
    });

    return {
      loginHasError: ref(false),
      ...validation
    };
  },
  computed: {
    ...accountModuleState
  },
  methods: {
    async handleLogin() {
      const { username, password } = await this.validateFields();
      const success = await this.login({ username, password });

      if (success) {
        this.$router.push({ name: 'main' });
      } else {
        this.loginHasError = true;
      }
    },
    ...accountModuleActions
  }
});
</script>

<style scoped>
.form {
  display: grid;
  row-gap: 30px;
}
</style>
