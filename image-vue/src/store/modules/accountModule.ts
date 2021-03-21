import { mapActions, mapState, Module } from 'vuex';
import jwtDecode from 'jwt-decode';
import { JwtClaims } from '../../api/common';
import { ApiUser } from './userModule';
import { apiClient, authClient } from '../../api/apiClient';
import { RootState } from '..';

type LoginData = {
  username: string;
  password: string;
};

type RegisterData = {
  username: string;
  password: string;
};

const INITIAL_STATE: AccountModuleState = {
  token: '',
  refreshToken: ''
};

type ApiTokens = {
  token: string;
  refreshToken: string;
};

type AccountModule = Module<AccountModuleState, RootState>;

export const MUTATIONS = {
  RESET_STATE: 'RESET_STATE',
  UPDATE_TOKENS: 'UPDATE_TOKENS',
  SET_LOADING: 'SET_LOADING'
};

export type AccountModuleState = {
  token: string;
  refreshToken: string;
};

export const accountModule: AccountModule = {
  namespaced: true,
  state() {
    return { ...INITIAL_STATE };
  },
  mutations: {
    [MUTATIONS.UPDATE_TOKENS](state, { token, refreshToken }: ApiTokens) {
      state.token = token;
      state.refreshToken = refreshToken;

      const claims = jwtDecode<JwtClaims>(token);

      localStorage.setItem('tokenExpiryTime', claims.exp.toString());
    },
    [MUTATIONS.RESET_STATE](state) {
      Object.assign(state, INITIAL_STATE);
    }
  },
  actions: {
    async login({ dispatch, commit }, loginData: LoginData) {
      const [
        { data: tokens, isValid: isValidTokens },
        { data: user, isValid: isValidUser }
      ] = await Promise.all([
        apiClient
          .useFetch<ApiTokens>()
          .execute('/identity/account/login')
          .post(JSON.stringify(loginData))
          .json().promise,
        apiClient
          .useFetch<ApiUser>()
          .execute(`/api/user/name/${loginData.username}`)
          .get()
          .json().promise
      ]);

      const isValid = isValidTokens.value && isValidUser.value;

      if (isValid) {
        commit(MUTATIONS.UPDATE_TOKENS, tokens.value);
        dispatch('userModule/setUser', user.value, { root: true });
      }

      return isValid;
    },
    async register({ dispatch, commit }, registerData: RegisterData) {
      const {
        data: tokens,
        isValid: isValidTokens
      } = await apiClient
        .useFetch<ApiTokens>()
        .execute('/identity/account/register')
        .post(JSON.stringify(registerData))
        .json().promise;

      const {
        data: user,
        isValid: isValidUser
      } = await apiClient
        .useFetch<ApiUser>()
        .execute(`/api/user/name/${registerData.username}`)
        .get()
        .json().promise;

      const isValid = isValidTokens.value && isValidUser.value;

      if (isValid) {
        commit(MUTATIONS.UPDATE_TOKENS, tokens.value);
        dispatch('userModule/setUser', user.value, { root: true });
      }

      return isValid;
    },
    async logout({ commit, dispatch }) {
      dispatch('userModule/resetState', null, { root: true });

      await authClient
        .useFetch()
        .execute('/identity/account/logout')
        .delete()
        .none(this).promise;

      commit(MUTATIONS.RESET_STATE);
    },
    async deleteAccount({ commit, dispatch }) {
      dispatch('userModule/resetState', null, { root: true });

      await authClient
        .useFetch()
        .execute('/identity/account/delete')
        .delete()
        .none(this).promise;

      commit(MUTATIONS.RESET_STATE);
    }
  }
};

export const accountModuleActions = mapActions('accountModule', {
  login: (dispatch, payload: LoginData) => dispatch('login', payload),
  regiser: (dispatch, payload: RegisterData) => dispatch('register', payload),
  logout: dispatch => dispatch('logout'),
  deleteAccount: dispatch => dispatch('deleteAccount')
});

export const accountModuleState = mapState<
  AccountModuleState,
  {
    [K in keyof AccountModuleState]: (
      state: AccountModuleState,
      // Remember to update this type according to the getters in gridModule
      getters: any
    ) => AccountModuleState[K];
  }
>('accountModule', {
  token: state => state.token,
  refreshToken: state => state.refreshToken
});
