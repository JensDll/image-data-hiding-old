<template>
  <label>
    <div class="flex justify-between font-semibold mb-3">
      {{ label }}
      <span
        v-if="isFileSelected"
        class="font-semibold text-indigo-500 hover:text-indigo-400 cursor-pointer"
        @click.prevent="setFile(null)"
      >
        Remove
      </span>
    </div>
    <div
      class="group border-2 border-dashed border-emerald-300 rounded-sm py-20 flex justify-center items-center relative"
      :class="{ 'bg-red-50 border-red-300': hasError }"
    >
      <input
        class="group w-full h-full absolute opacity-0 cursor-pointer"
        type="file"
        @change="handleChange"
      />
      <div class="text-center">
        <p>
          <span
            class="text-indigo-500 font-semibold group-hover:text-indigo-400"
            :class="{ 'text-red-500 group-hover:text-red-400': hasError }"
          >
            Upload a file
          </span>
          or drag and drop
        </p>
        <slot></slot>
        <div v-if="isFileSelected" class="flex flex-col items-center">
          <img :src="fileSrc" class="w-32 h-32 mx-auto mb-2 mt-8" />
          <p>{{ fileName }}</p>
        </div>
      </div>
    </div>
  </label>
</template>

<script lang="ts">
import { computed, defineComponent, ref } from 'vue';

export default defineComponent({
  props: {
    label: {
      type: String,
      default: 'Image'
    },
    hasError: {
      type: Boolean,
      default: false
    }
  },
  emits: ['upload'],
  setup(props, { emit }) {
    const file = ref<File | null>(null);

    const fileName = computed<string>(() => file.value?.name ?? '');

    const fileSrc = computed<string>(() =>
      file.value ? URL.createObjectURL(file.value) : ''
    );

    const isFileSelected = computed<boolean>(() => file.value instanceof File);

    const setFile = (value: File | null) => {
      file.value = value;
      emit('upload', value);
    };

    const handleChange = (e: Event) => {
      const input = e.target as HTMLInputElement;

      if (input.files?.length) {
        setFile(input.files[0]);
      }

      input.value = '';
    };

    return {
      file,
      isFileSelected,
      fileName,
      fileSrc,
      handleChange,
      setFile
    };
  }
});
</script>
