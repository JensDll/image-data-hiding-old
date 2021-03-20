import jwtDecode, { JwtPayload } from 'jwt-decode';
import { createFetch } from '../composition';
import { isCurrentTokenExpired } from './common';

const BASE_URI = 'https://localhost:5001';

type RefreshTokenResponse = {
  token: string;
  refreshToken: string;
};

export const apiClient = createFetch(BASE_URI);

export const authClient = createFetch(BASE_URI, {
  async beforeEach(options) {
    const token = localStorage.getItem('token');
    const refreshToken = localStorage.getItem('refreshToken');

    if (isCurrentTokenExpired()) {
      const { data } = await apiClient
        .useFetch<RefreshTokenResponse>({ immediat: true })
        .execute('/identity/account/refresh')
        .post(JSON.stringify({ token, refreshToken }))
        .json().promise;

      if (data.value) {
        localStorage.setItem('token', data.value.token);
        localStorage.setItem('refreshToken', data.value.refreshToken);

        const claims = jwtDecode<JwtPayload>(data.value.token);
        if (claims.exp) {
          localStorage.setItem('tokenExpiryTime', claims.exp.toString());
        }

        options.headers.Authorization = `Bearer ${data.value.token}`;
        return;
      }
    }

    options.headers.Authorization = `Bearer ${token}`;
  }
});
