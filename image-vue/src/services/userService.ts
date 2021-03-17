import { reactive, ref } from 'vue';
import { useFetch } from '../composition';
import { PagedResponse } from './common';

type User = {
  id: number;
  username: string;
};

const BASE = '/api/user';

export const userService = {
  useGetAll(pageNumber?: number, pageSize?: number) {
    const { response: fetchResponse, loading, execute } = useFetch();

    const pagedResponse = reactive<PagedResponse<User>>({
      data: [],
      total: 0
    });

    const getAll = async (pageNumber: number, pageSize: number) => {
      await execute({
        uri: `${BASE}?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        method: 'GET',
        headers: { 'Content-Type': 'application/json' }
      });

      const data: PagedResponse<User> = await fetchResponse.value?.json();

      Object.assign(pagedResponse, data);
    };

    if (pageNumber && pageSize) {
      getAll(pageNumber, pageSize);
    }

    return {
      getAll,
      loading,
      pagedResponse
    };
  }
};
