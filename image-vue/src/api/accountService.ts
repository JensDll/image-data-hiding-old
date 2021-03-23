import { Store } from 'vuex';
import { FetchOptions } from '../composition/useFetch';
import { RootState } from '../store';
import { apiClient, authClient } from './apiClient';

export type LoginRequest = {
  username: string;
  password: string;
};

export type RegisterRequest = {
  username: string;
  password: string;
};

export type RefreshTokensRequest = {
  token: string;
  refreshToken: string;
};

export type ApiTokens = {
  token: string;
  refreshToken: string;
};

export const accountService = (fetchOptions?: FetchOptions) => ({
  login(request: LoginRequest) {
    return apiClient
      .useFetch<ApiTokens>(fetchOptions)
      .execute('/identity/account/login')
      .post(JSON.stringify(request))
      .json();
  },
  register(request: RegisterRequest) {
    return apiClient
      .useFetch<ApiTokens>(fetchOptions)
      .execute('/identity/account/register')
      .post(JSON.stringify(request))
      .json();
  },
  refreshTokens(tokens: RefreshTokensRequest) {
    return apiClient
      .useFetch<ApiTokens>()
      .execute('/identity/account/refresh')
      .post(JSON.stringify(tokens))
      .json();
  },
  logout(store: Store<RootState>) {
    return authClient
      .useFetch(fetchOptions)
      .execute('/identity/account/logout')
      .delete()
      .none(store);
  },
  deleteAccount(store: Store<RootState>) {
    return authClient
      .useFetch(fetchOptions)
      .execute('/identity/account/delete')
      .delete()
      .none(store);
  }
});
