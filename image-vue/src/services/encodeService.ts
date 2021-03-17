import { useStore } from '../store';
import { useFetch } from '../composition';

const BASE = '/api/image/encode';

export const encodeService = {
  useWithFile() {
    const { response: fetchResponse, loading, execute } = useFetch();

    const store = useStore();

    const encodeWithFile = async (formData: FormData) => {
      await execute({
        uri: `${BASE}/file`,
        method: 'POST',
        headers: {
          Authorization: `bearer ${store.state.userModule.token}`
        },
        body: formData
      });

      const blob = await fetchResponse.value?.blob();

      return URL.createObjectURL(blob);
    };

    return {
      loading,
      encodeWithFile
    };
  },
  useWithRandom() {
    const { response: fetchResponse, loading, execute } = useFetch();

    const store = useStore();

    const encodeWithRandom = async (formData: {
      username: string;
      message: string;
    }) => {
      await execute({
        uri: `${BASE}/random`,
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `bearer ${store.state.userModule.token}`
        },
        body: JSON.stringify(formData)
      });

      const blob = await fetchResponse.value?.blob();

      return URL.createObjectURL(blob);
    };

    return {
      encodeWithRandom,
      loading
    };
  }
};
