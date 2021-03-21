<template>
  <form class="form mb-64" @submit.prevent="handleSubmit()">
    <label>
      <div class="font-semibold mb-3">Send To</div>
      <div class="relative">
        <select
          v-model="form.user.$value"
          class="w-full appearance-none p-4 my-select relative border border-emerald-300 focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
          :class="{
            'bg-red-50 border-red-300 focus:border-red-500 focus:ring-red-500':
              form.user.$errors.length
          }"
        >
          <option
            v-for="user in fetchingUsers ? [] : pagedResponse?.data"
            :key="user.id"
            class="block p-4 bg-white"
            :value="user"
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
        v-for="(error, index) in form.user.$errors"
        :key="index"
        class="text-sm text-red-500 mt-1"
      >
        {{ error }}
      </div>
    </label>
    <label>
      <div class="font-semibold mb-3">Message</div>
      <textarea
        v-model="form.message.$value"
        spellcheck="false"
        class="w-full p-4 border border-emerald-300 focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500 focus:outline-none"
      />
    </label>
    <FileUpload @upload="file = $event">
      <p class="text-sm text-gray-500">
        Leave this empty and receive a random image
      </p>
    </FileUpload>
    <BaseButton
      class="px-6 py-3 btn-primary mt-4"
      html-type="submit"
      :disabled="encoding"
    >
      Encode
    </BaseButton>
  </form>
</template>

<script lang="ts">
import { computed, ref, defineComponent } from 'vue';
import { Field, useValidation } from 'vue3-form-validation';
import { useDownload } from '../../composition';
import { PagedResponse } from '../../api/common';
import { apiClient, authClient } from '../../api/apiClient';
import { useStore } from '../../store';

type FormData = {
  user: Field<User>;
  message: Field<string>;
};

type User = {
  id: number;
  username: string;
};

export default defineComponent({
  setup() {
    const store = useStore();
    const { form, validateFields } = useValidation<FormData>({
      user: {
        $value: null as any,
        $rules: [user => !user && 'Please select a username']
      },
      message: {
        $value: ''
      }
    });

    const { data: pagedResponse, loading: fetchingUsers } = apiClient
      .useFetch<PagedResponse<User>>({ immediat: true })
      .execute('/api/user?pageNumber=1&pageSize=200')
      .get()
      .json();

    const {
      loading: loadingRandom,
      execute: encodeRandom
    } = authClient.useFetch<Blob>();

    const {
      loading: loadingWithFile,
      execute: encodeWithFile
    } = authClient.useFetch<Blob>();

    const file = ref<File>();

    const handleSubmit = async () => {
      try {
        const { user, message } = await validateFields();

        const formData = new FormData();

        if (file.value) {
          formData.append('file', file.value, file.value.name);
          formData.append('userId', user.id.toString());
          formData.append('username', user.username);
          formData.append('message', message);
        }

        const promise = file.value
          ? encodeWithFile('/api/image/encode/file', {})
              .post(formData)
              .blob(store).promise
          : encodeRandom('/api/image/encode/random')
              .post(
                JSON.stringify({
                  userId: user.id,
                  username: user.username,
                  message
                })
              )
              .blob(store).promise;

        const { isValid, data: image } = await promise;

        if (isValid.value && image.value) {
          useDownload().saveImage(image.value, 'secret.png');
        }
      } catch (e) {
        console.log(e);
      }
    };

    return {
      file,
      form,
      pagedResponse,
      fetchingUsers,
      encoding: computed(() => loadingWithFile.value || loadingRandom.value),
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
