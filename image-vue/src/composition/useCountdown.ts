import { computed, onBeforeUnmount } from '@vue/runtime-core';
import { ref } from 'vue';

const getDistance = (time: number | Date): number => {
  if (time instanceof Date) {
    return time > new Date() ? time.getTime() - Date.now() : 0;
  } else {
    return time * 1000;
  }
};

export function useCountdown(time: number | Date, done?: () => void) {
  const distance = ref(getDistance(time));

  const ms = {
    DAY: 86400000, // 1000 * 60 * 60 * 24
    HOUR: 3600000, // 1000 * 60 * 60
    MINUTE: 60000, // 1000 * 60
    SECOND: 1000
  };

  const days = computed(() => Math.floor(distance.value / ms.DAY));
  const hours = computed(() => Math.floor((distance.value % ms.DAY) / ms.HOUR));
  const minutes = computed(() =>
    Math.floor((distance.value % ms.HOUR) / ms.MINUTE)
  );
  const seconds = computed(() =>
    Math.floor((distance.value % ms.MINUTE) / ms.SECOND)
  );

  const interval = setInterval(() => {
    if ((distance.value -= ms.SECOND) <= 0) {
      distance.value = 0;
      done?.();
      clearInterval(interval);
    }
  }, ms.SECOND);

  onBeforeUnmount(() => {
    clearInterval(interval);
  });

  const normalizeLength = (x: number) => (x < 10 ? `0${x}` : x.toString());

  return {
    days: computed(() => normalizeLength(days.value)),
    hours: computed(() => normalizeLength(hours.value)),
    minutes: computed(() => normalizeLength(minutes.value)),
    seconds: computed(() => normalizeLength(seconds.value))
  };
}
