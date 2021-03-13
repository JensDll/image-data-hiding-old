<template>
  <button
    :class="['button-base', type, { disabled }]"
    :type="htmlType"
    :disabled="disabled"
  >
    <slot></slot>
  </button>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';

export default defineComponent({
  props: {
    htmlType: {
      type: String as PropType<'button' | 'reset' | 'submit'>,
      default: 'button',
      validator: (htmlType: string) =>
        ['button', 'reset', 'submit'].includes(htmlType)
    },
    disabled: {
      type: Boolean
    },
    type: {
      type: String,
      default: 'default'
    }
  }
});
</script>

<!-- @vue-ignore -->
<style scoped>
.button-base {
  @apply block font-medium text-white transition-colors select-none;

  outline: none;
}

.disabled {
  @apply pointer-events-none opacity-30 border-none bg-gray-200 text-black !important;
}

.default {
  @apply bg-green-50 text-emerald-600;
  @apply hover:bg-emerald-100;
}

.default:focus {
  box-shadow: 0 0 4px theme('colors.emerald.200');
}

.primary {
  @apply bg-emerald-500;
  @apply hover:bg-opacity-80;
}

.primary:focus {
  box-shadow: 0 0 4px theme('colors.emerald.500');
}

.danger {
  @apply bg-red-500;
  @apply hover:bg-opacity-80;
}

.danger:focus {
  box-shadow: 0 0 4px theme('colors.red.500');
}
</style>
