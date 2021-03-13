<template>
  <form class="form">
    <textarea
      placeholder="My secret message"
      spellcheck="false"
      class="w-full p-4 focus:outline-none focus:ring-2 focus:ring-blue-500"
      v-model="formData.text"
    />
    <BaseButton class="px-6 py-3 ring-offset-blue-50" type="primary">
      Encode
    </BaseButton>
  </form>
</template>

<script lang="ts">
import { defineComponent, reactive } from 'vue';

export default defineComponent({
  setup() {
    const formData = reactive({
      text: ''
    });

    return {
      formData
    };
  }
});
</script>

<style scoped>
.form {
  background-color: theme('colors.blue.50');
  padding: 40px 60px;
  width: 100%;
  display: grid;
  row-gap: 30px;
}
</style>
