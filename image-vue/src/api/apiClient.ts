import { Store } from 'vuex';
import { createFetch } from '../composition';
import { RootState } from '../store';
import { isCurrentTokenExpired } from './common';
import { MUTATIONS } from '../store/modules/authModule';
import { authService } from './authService';

const BASE_URI = 'https://localhost:5001';

export const apiClient = createFetch(BASE_URI);

export const authClient = createFetch<Store<RootState>>(BASE_URI, {
  async beforeEach(options, store) {
    const token = store.state.authModule.token;
    const refreshToken = store.state.authModule.refreshToken;

    if (isCurrentTokenExpired()) {
      const { data } = await authService().refreshTokens({
        token,
        refreshToken
      }).promise;

      if (data.value) {
        store.commit(`authModule/${MUTATIONS.UPDATE_TOKENS}`, data.value);
        options.headers.Authorization = `Bearer ${data.value.token}`;
      }
    } else {
      options.headers.Authorization = `Bearer ${token}`;
    }
  }
});
