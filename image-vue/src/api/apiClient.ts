import { Store } from 'vuex';
import { createFetch } from '../composition';
import { RootState } from '../store';
import { isCurrentTokenExpired } from './common';
import { MUTATIONS } from '../store/modules/accountModule';
import { accountService } from './accountService';

const BASE_URI = 'https://localhost:5001';

export const apiClient = createFetch(BASE_URI);

export const authClient = createFetch<Store<RootState>>(BASE_URI, {
  async beforeEach(options, store) {
    const token = store.state.accountModule.token;
    const refreshToken = store.state.accountModule.refreshToken;

    if (isCurrentTokenExpired()) {
      const { data } = await accountService().refreshTokens({
        token,
        refreshToken
      }).promise;

      if (data.value) {
        store.commit(`accountModule/${MUTATIONS.UPDATE_TOKENS}`, data.value);
        options.headers.Authorization = `Bearer ${data.value.token}`;
      }
    } else {
      options.headers.Authorization = `Bearer ${token}`;
    }
  }
});
