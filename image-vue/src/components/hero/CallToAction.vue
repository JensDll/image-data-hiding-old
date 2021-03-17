<template>
  <div class="flex gap-x-8 justify-center">
    <BaseButton class="px-16 py-4 btn-primary" @click="toLogin()">
      Login
    </BaseButton>
    <BaseButton class="px-16 py-4 btn-default" @click="toRegister()">
      Register
    </BaseButton>
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';

const router = useRouter();

const toLogin = () => router.push('/login');

const toRegister = () => router.push('/register');
</script>
