import { Ref, ref } from 'vue';

interface RequestOptions extends RequestInit {
  uri: string;
}

function useFetchBase(baseUri: string) {
  return (options?: RequestOptions) => {
    const loading = ref(false);
    const hasError = ref(false);
    const response = ref<Response>();

    const execute = async (options: RequestOptions) => {
      loading.value = true;
      hasError.value = false;

      try {
        console.log('FETCH');

        const _response = await fetch(baseUri + options.uri, options);

        response.value = _response;

        if (!_response.ok) {
          hasError.value = true;
        }
      } catch {
        hasError.value = true;
      } finally {
        loading.value = false;
      }
    };

    if (options) {
      execute(options);
    }

    return {
      execute,
      loading,
      hasError,
      response
    };
  };
}

export const useFetch = useFetchBase('https://localhost:5001');
