<template>
  <form @submit.prevent="handleSubmit">
    <label>
      <div>Select a file</div>
      <input ref="file-input" type="file" accept=".jpg, .jpeg, .png" />
    </label>
    <button class="block" type="submit">Submit</button>
  </form>
  <img src="" ref="img-ref" class="image" alt="Loading" />
  <button @click="randomImage()">Random Image</button>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  methods: {
    async handleSubmit() {
      const fileInput = this.$refs['file-input'] as HTMLInputElement;
      const image = fileInput.files?.[0];
      const formData = new FormData();

      if (image) {
        formData.append('file', image, image.name);

        const res = await fetch('https://localhost:5001/image', {
          method: 'POST',
          body: formData
        });

        const blob = await res.blob();
        const imageRef = this.$refs['img-ref'] as HTMLImageElement;

        console.log(blob);
        imageRef.src = URL.createObjectURL(blob);
      }
    },
    async randomImage() {
      const response = await fetch('https://localhost:5001/image', {
        method: 'GET'
      });
      const blob = await response.blob();
      const imageRef = this.$refs['img-ref'] as HTMLImageElement;
      imageRef.src = URL.createObjectURL(blob);
    }
  }
});
</script>

<style scoped>
.image {
  transform: scale(0.7);
}
</style>
