import { mapActions, mapState, Module } from 'vuex';
import { RootState } from '..';
import jwtDecode, { JwtPayload } from 'jwt-decode';
import { apiClient } from '../../services/apiClient';

export type AccountModuleState = {
  loggedIn: boolean;
  loading: boolean;
  username: string;
  token: string;
  refreshToken: string;
};

enum Mutations {
  RESET = 'RESET',
  SET_TOKENS = 'SET_TOKEN',
  SET_USERNAME = 'SET_USERNAME',
  SET_LOADING = 'SET_LOADING'
}

type UserModule = Module<AccountModuleState, RootState>;

type LoginPayload = {
  username: string;
  password: string;
};

type RegisterPayload = {
  username: string;
  password: string;
};

type LoginResponse = {
  token: string;
  refreshToken: string;
};

type RegisterResponse = {
  token: string;
  refreshToken: string;
};

const INITIAL_STATE: AccountModuleState = {
  loggedIn: false,
  loading: false,
  username: '',
  token: '',
  refreshToken: ''
};

export const accountModule: UserModule = {
  namespaced: true,
  state() {
    return { ...INITIAL_STATE };
  },
  mutations: {
    [Mutations.SET_TOKENS](
      state,
      { token, refreshToken }: { token: string; refreshToken: string }
    ) {
      state.token = token;
      state.refreshToken = refreshToken;
      state.loggedIn = true;

      const claims = jwtDecode<JwtPayload>(token);

      if (claims.exp) {
        localStorage.setItem('token', token);
        localStorage.setItem('refreshToken', refreshToken);
        localStorage.setItem('tokenExpiryTime', claims.exp.toString());
      }
    },
    [Mutations.SET_USERNAME](state, username: string) {
      state.username = username;
    },
    [Mutations.RESET](state) {
      Object.assign(state, INITIAL_STATE);
    },
    [Mutations.SET_LOADING](state, value) {
      state.loading = value;
    }
  },
  actions: {
    async login({ commit }, payload: LoginPayload) {
      commit(Mutations.SET_LOADING, true);

      const {
        data,
        isValid
      } = await apiClient
        .useFetch<LoginResponse>()
        .execute('/identity/account/login')
        .post(JSON.stringify(payload))
        .json().promise;

      if (isValid.value) {
        commit(Mutations.SET_TOKENS, data.value);
        commit(Mutations.SET_USERNAME, payload.username);
      }
      commit(Mutations.SET_LOADING, false);

      return isValid.value;
    },
    async register({ commit }, payload: RegisterPayload) {
      commit(Mutations.SET_LOADING, true);

      const {
        data,
        isValid
      } = await apiClient
        .useFetch<RegisterResponse>()
        .execute('/identity/account/register')
        .post(JSON.stringify(payload))
        .json().promise;

      if (isValid.value) {
        commit(Mutations.SET_TOKENS, data.value);
        commit(Mutations.SET_USERNAME, payload.username);
      }

      commit(Mutations.SET_LOADING, false);
    },
    logout({ commit }) {
      commit(Mutations.RESET);
    }
  }
};

export const accountModuleActions = mapActions('accountModule', {
  login: (dispatch, payload: LoginPayload) => dispatch('login', payload),
  regiser: (dispatch, payload: RegisterPayload) =>
    dispatch('register', payload),
  logout: dispatch => dispatch('logout')
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
  refreshToken: state => state.refreshToken,
  loading: state => state.loading,
  loggedIn: state => state.loggedIn,
  username: state => state.username
});
