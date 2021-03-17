<template>
  <form class="form mb-64" @submit.prevent="handleSubmit()">
    <label>
      <div class="font-semibold mb-3">Send To</div>
      <div class="relative">
        <select
          v-model="form.username.$value"
          class="w-full appearance-none p-4 my-select relative border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
          :class="{
            'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500':
              form.username.$errors.length
          }"
        >
          <option
            class="block p-4 bg-white"
            v-for="user in pagedResponse.data"
            :key="user.id"
            :value="user.username"
          >
            {{ user.username }}
          </option>
        </select>
        <span
          class="absolute inset-y-0 right-0 flex items-center mr-3 pointer-events-none"
        >
          <svg
            class="h-5 w-5 text-gray-400"
            xmlns="http://www.w3.org/2000/svg"
            viewBox="0 0 20 20"
            fill="currentColor"
            aria-hidden="true"
          >
            <path
              fill-rule="evenodd"
              d="M10 3a1 1 0 01.707.293l3 3a1 1 0 01-1.414 1.414L10 5.414 7.707 7.707a1 1 0 01-1.414-1.414l3-3A1 1 0 0110 3zm-3.707 9.293a1 1 0 011.414 0L10 14.586l2.293-2.293a1 1 0 011.414 1.414l-3 3a1 1 0 01-1.414 0l-3-3a1 1 0 010-1.414z"
              clip-rule="evenodd"
            />
          </svg>
        </span>
      </div>
      <div
        v-for="(error, index) in form.username.$errors"
        :key="index"
        class="text-sm text-red-500 mt-1"
      >
        {{ error }}
      </div>
    </label>
    <label>
      <div class="font-semibold mb-3">Message</div>
      <textarea
        spellcheck="false"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500 focus:outline-none"
        v-model="form.message.$value"
      />
    </label>
    <FileUpload @upload="file = $event" />
    <BaseButton
      class="px-6 py-3 btn-primary relative"
      type="primary"
      :disabled="encoding"
    >
      Encode
    </BaseButton>
  </form>
</template>

<script lang="ts">
import { computed, ref, defineComponent } from 'vue';
import { Field, useValidation } from 'vue3-form-validation';
import { encodeService } from '../../services/encodeService';
import { userService } from '../../services/userService';
import FileUpload from './FileUpload.vue';
import { useDownload } from '../../composition';

type FormData = {
  username: Field<string>;
  message: Field<string>;
};

export default defineComponent({
  components: { FileUpload },
  setup() {
    const { form, validateFields } = useValidation<FormData>({
      username: {
        $value: '',
        $rules: [username => !username && 'Please select a username']
      },
      message: {
        $value: ''
      }
    });

    const { pagedResponse } = userService.useGetAll(1, 10);
    const {
      encodeWithFile,
      loading: encodeLoadingFile
    } = encodeService.useWithFile();
    const {
      encodeWithRandom,
      loading: encodeLoadingRandom
    } = encodeService.useWithRandom();

    const file = ref<File>();

    const handleSubmit = async () => {
      try {
        const { username, message } = await validateFields();
        const formData = new FormData();

        if (file.value) {
          formData.append('file', file.value, file.value.name);
          formData.append('username', username);
          formData.append('message', message);
        }

        const link = await (file.value
          ? encodeWithFile(formData)
          : encodeWithRandom({ username, message }));

        useDownload(link, 'secret.png');
      } catch {
        //
      }
    };

    return {
      file,
      form,
      pagedResponse,
      encoding: computed(
        () => encodeLoadingFile.value || encodeLoadingRandom.value
      ),
      handleSubmit
    };
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
