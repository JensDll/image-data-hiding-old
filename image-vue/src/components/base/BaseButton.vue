<template>
  <button
    :class="['button-base', { disabled }]"
    :type="htmlType"
    :disabled="disabled"
  >
    <slot></slot>
  </button>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';

export default defineComponent({
  name: 'BaseButton',
  props: {
    htmlType: {
      type: String as PropType<'button' | 'reset' | 'submit'>,
      default: 'button',
      validator: (htmlType: string) =>
        ['button', 'reset', 'submit'].includes(htmlType)
    },
    disabled: {
      type: Boolean
    }
  }
});
</script>

<!-- @vue-ignore -->
<style scoped>
.button-base {
  @apply block font-medium transition-colors select-none;

  outline: none;
}

.disabled {
  @apply pointer-events-none opacity-30 border-none bg-gray-200 text-black !important;
}
</style>
