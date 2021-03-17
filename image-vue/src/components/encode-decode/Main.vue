<template>
  <div class="form-container">
    <nav class="steps">
      <div class="step" :class="{ active: step === 0 }" @click="step = 0">
        <div>Encode</div>
        <div class="circle"></div>
      </div>
      <div class="step" :class="{ active: step === 1 }" @click="step = 1">
        <div>Decode</div>
        <div class="circle"></div>
      </div>
      <div class="line"></div>
    </nav>
    <EncodeForm v-if="step === 0" />
    <DecodeForm v-if="step === 1" />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import EncodeForm from './EncodeForm.vue';
import DecodeForm from './DecodeForm.vue';

export default defineComponent({
  components: {
    EncodeForm,
    DecodeForm
  },
  data() {
    return {
      step: 0
    };
  }
});
</script>

<style scoped>
.form-container {
  /* @apply flex flex-col items-center; */

  display: flex;
  flex-direction: column;
  align-items: center;
}

.steps {
  position: relative;
  width: 250px;
  margin-bottom: 30px;
  display: grid;
  justify-content: space-between;
  grid-auto-flow: column;

  --diameter: 30px;

  & .step {
    @apply flex flex-col cursor-pointer;

    & .circle {
      @apply mt-2 border-4 border-indigo-500 bg-white rounded-full transition-transform duration-100 z-10 self-center;

      width: var(--diameter);
      height: var(--diameter);
    }

    &:hover {
      & .circle {
        transform: scale(1.2);
      }
    }

    &.active {
      @apply font-medium text-indigo-600;

      & .circle {
        background-color: theme('colors.indigo.50');
        transform: scale(1.2);
      }
    }
  }

  & .line {
    --height: 4px;

    background-color: theme('colors.indigo.500');
    height: var(--height);
    position: absolute;
    left: 20px;
    right: 20px;
    bottom: calc(var(--diameter) / 2);
    margin-bottom: calc(-1 * (var(--height) / 2));
  }
}
</style>
