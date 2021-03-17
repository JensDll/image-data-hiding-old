import { mapActions, mapState, Module } from 'vuex';
import { RootState } from '..';

export type UserModuleState = {
  loggedIn: boolean;
  username: string;
  token: string;
};

enum Mutations {
  SET_TOKEN = 'SET_TOKEN',
  SET_USERNAME = 'SET_USERNAME'
}

type UserModule = Module<UserModuleState, RootState>;

type LoginPayload = {
  username: string;
  token: string;
};

export const userModule: UserModule = {
  namespaced: true,
  state() {
    return {
      loggedIn: false,
      username: '',
      token: ''
    };
  },
  mutations: {
    [Mutations.SET_TOKEN](state, token: string) {
      state.token = token;
      state.loggedIn = true;
    },
    [Mutations.SET_USERNAME](state, username: string) {
      state.username = username;
    }
  },
  actions: {
    loginUser({ commit }, payload: LoginPayload) {
      commit(Mutations.SET_TOKEN, payload.token);
      commit(Mutations.SET_USERNAME, payload.username);
    }
  }
};

export const userModuleActions = mapActions('userModule', {
  loginUser: (dispatch, data: LoginPayload) => dispatch('loginUser', data)
});

export const userModuleState = mapState<
  UserModuleState,
  {
    [K in keyof UserModuleState]: (
      state: UserModuleState,
      // Remember to update this type according to the getters in gridModule
      getters: {}
    ) => UserModuleState[K];
  }
>('userModule', {
  token: state => state.token,
  loggedIn: state => state.loggedIn,
  username: state => state.username
});
