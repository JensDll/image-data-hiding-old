<template>
  <form class="form px-16 py-10" @submit.prevent="handleLogin()">
    <label>
      <div class="mb-3 font-medium">Username</div>
      <input
        class="w-full p-4 bg-emerald-50 focus:outline-none focus:ring-2 focus:ring-emerald-500"
        type="text"
        v-model="form.username.$value"
      />
    </label>
    <label>
      <div class="mb-3 font-medium">Password</div>
      <input
        class="w-full p-4 bg-emerald-50 focus:outline-none focus:ring-2 focus:ring-emerald-500"
        type="password"
        v-model="form.password.$value"
      />
    </label>
    <div class="flex gap-x-8 mt-6">
      <BaseButton
        class="px-6 py-4 w-full btn-primary"
        htmlType="submit"
        type="primary"
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
import { accountService } from '../../services/accountService';
import { userModuleActions } from '../../store/modules/userModule';

type FormData = {
  username: Field<string>;
  password: Field<string>;
};

export default defineComponent({
  setup() {
    const useVal = useValidation<FormData>({
      username: {
        $value: ''
      },
      password: {
        $value: ''
      }
    });

    return {
      ...useVal,
      ...accountService.useLogin()
    };
  },
  methods: {
    async handleLogin() {
      try {
        const { username, password } = await this.validateFields();
        const { loginResponse } = await this.login(username, password);

        if (loginResponse.value) {
          this.loginUser({
            token: loginResponse.value.token,
            username
          });

          this.$router.push({ name: 'main' });
        }
      } catch (err) {
        console.log(err);
      }
    },
    ...userModuleActions
  }
});
</script>

<style scoped>
.form {
  width: 100%;
  display: grid;
  row-gap: 30px;
}
</style>
