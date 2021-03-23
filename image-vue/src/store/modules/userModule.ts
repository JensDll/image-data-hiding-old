import { Module } from 'vuex';
import { RootState } from '..';
import { ApiUser } from '../../api/userService';

const types = {
  SET_USER: 'SET_USER',
  RESET_STATE: 'RESET_STATE'
};

const INIT_STATE: UserModuleState = {
  loggedIn: false,
  user: {
    id: 0,
    username: '',
    deletionDate: new Date()
  }
};

type UserModule = Module<UserModuleState, RootState>;

export type User = {
  id: number;
  username: string;
  deletionDate: Date;
};

export type UserModuleState = {
  loggedIn: boolean;
  user: User;
};

export const userModule: UserModule = {
  namespaced: true,
  state() {
    return {
      loggedIn: INIT_STATE.loggedIn,
      user: { ...INIT_STATE.user }
    };
  },
  mutations: {
    [types.SET_USER](state, user: ApiUser) {
      state.loggedIn = true;
      state.user = {
        id: user.id,
        username: user.username,
        deletionDate: new Date(user.deletionDate)
      };
    },
    [types.RESET_STATE](state) {
      Object.assign(state, INIT_STATE);
    }
  },
  actions: {
    setUser({ commit }, user: ApiUser) {
      commit(types.SET_USER, user);
    },
    resetState({ commit }) {
      commit(types.RESET_STATE);
    }
  }
};
