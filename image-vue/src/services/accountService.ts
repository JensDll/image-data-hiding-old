import { reactive, Ref, ref } from 'vue';
import { useFetch } from '../composition';

type LoginResponse = {
  token: string;
};

type ErrorResponse = {
  errorMessages: string[];
};

const BASE = '/identity/account';

export const accountService = {
  useLogin() {
    const { response: fetchResponse, loading, hasError, execute } = useFetch();

    const login = async (username: string, password: string) => {
      const loginResponse = ref<LoginResponse>();
      const errorResponse = ref<ErrorResponse>();

      await execute({
        uri: `${BASE}/login`,
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
      });

      const data = await fetchResponse.value?.json();

      if (hasError.value) {
        errorResponse.value = data;
      } else {
        loginResponse.value = data;
      }

      return {
        loginResponse,
        errorResponse
      };
    };

    return {
      login,
      loading
    };
  }
};
