<template>
  <form class="form" @submit.prevent="hanldeRegister()">
    <label>
      <div class="mb-3 font-medium">Username</div>
      <input
        v-model="form.username.$value"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        :class="{
          'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500':
            form.username.$hasError
        }"
        type="text"
        @blur="form.username.$onBlur"
      />
      <div v-if="form.username.$hasError" class="text-red-400 text-sm mt-2">
        <p v-for="error in form.username.$errors" :key="error">{{ error }}</p>
      </div>
    </label>
    <label>
      <div class="mb-3 font-medium">Password</div>
      <input
        v-model="form.password.$value"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        :class="{
          'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500':
            form.password.$hasError
        }"
        type="password"
        @blur="form.password.$onBlur"
      />
      <div v-if="form.password.$hasError" class="text-red-400 text-sm mt-2">
        <p v-for="error in form.password.$errors" :key="error">{{ error }}</p>
      </div>
    </label>
    <label>
      <div class="mb-3 font-medium">Confirm Password</div>
      <input
        v-model="form.repeatPassword.$value"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
        :class="{
          'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500':
            form.repeatPassword.$hasError
        }"
        type="password"
        @blur="form.repeatPassword.$onBlur"
      />
      <div
        v-if="form.repeatPassword.$hasError"
        class="text-red-400 text-sm mt-2"
      >
        <p v-for="error in form.repeatPassword.$errors" :key="error">
          {{ error }}
        </p>
      </div>
    </label>
    <div class="flex gap-x-8 mt-6">
      <BaseButton
        class="px-6 py-4 w-full btn-primary"
        html-type="submit"
        type="primary"
        :disabled="loading"
      >
        Register
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
import { authModuleActions } from '../../store/modules/authModule';
import { userService } from '../../api/userService';

type FormData = {
  username: Field<string>;
  password: Field<string>;
  repeatPassword: Field<string>;
};

export default defineComponent({
  setup() {
    const password = ref('');
    const repeatPassword = ref('');

    const containsUppercase = (password: string) =>
      /[A-Z]/.test(password) || 'Password must have uppercase letters';

    const containsLowercase = (password: string) =>
      /[a-z]/.test(password) || 'Password must have lowercase letters';

    const containsNumber = (password: string) =>
      /[0-9]/.test(password) || 'Password must have numbers';

    const containsSpecialCharacter = (password: string) =>
      /[^a-zA-Z0-9]/.test(password) || 'Password must have special characters';

    const minLength = (password: string) =>
      password.length > 7 || 'Password must be longer than 7 characters';

    const validation = useValidation<FormData>({
      username: {
        $value: '',
        $rules: [
          username =>
            /^[0-9a-zA-Z-._@+]*$/g.test(username) ||
            'Username can not contain special characters (except - . _ @ +)',
          username => !username && 'Please select a username',
          async username =>
            username &&
            (await userService().isUsernameTaken(username).promise).data.value
              ?.data &&
            'This username is already taken'
        ]
      },
      password: {
        $value: password,
        $rules: [
          {
            key: 'password',
            rule: () =>
              password.value === repeatPassword.value || "Passwords don't match"
          },
          containsUppercase,
          containsLowercase,
          containsNumber,
          containsSpecialCharacter,
          minLength
        ]
      },
      repeatPassword: {
        $value: repeatPassword,
        $rules: [
          {
            key: 'password',
            rule: () =>
              password.value === repeatPassword.value || "Passwords don't match"
          },
          containsUppercase,
          containsLowercase,
          containsNumber,
          containsSpecialCharacter,
          minLength
        ]
      }
    });

    return {
      loading: ref(false),
      ...validation
    };
  },
  methods: {
    async hanldeRegister() {
      try {
        this.loading = true;
        const { username, password } = await this.validateFields();
        await this.regiser({ username, password });
        this.$router.push({ name: 'main' });
      } catch {
        //
      } finally {
        this.loading = false;
      }
    },
    ...authModuleActions
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
