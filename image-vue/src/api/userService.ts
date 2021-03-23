import { FetchOptions } from '../composition/useFetch';
import { apiClient } from './apiClient';
import { Envelop, PagedResponse } from './common';

export type ApiUser = {
  id: number;
  username: string;
  deletionDate: string;
};

export const userService = (fetchOptions?: FetchOptions) => ({
  getAll(pageNumber: number, pageSize: number) {
    return apiClient
      .useFetch<PagedResponse<ApiUser>>(fetchOptions)
      .execute(`/api/user?pageNumber=${pageNumber}&pageSize=${pageSize}`)
      .get()
      .json();
  },
  getById(id: number | string) {
    return apiClient
      .useFetch<ApiUser>(fetchOptions)
      .execute(`/api/user/${id}`)
      .get()
      .json();
  },
  getByName(username: string) {
    return apiClient
      .useFetch<ApiUser>(fetchOptions)
      .execute(`/api/user/name/${username}`)
      .get()
      .json();
  },
  isUsernameTaken(username: string) {
    return apiClient
      .useFetch<Envelop<boolean>>(fetchOptions)
      .execute(`/api/user/name/${username}/taken`)
      .get()
      .json();
  }
});
