import { InjectionKey } from 'vue';
import { createStore, Store, useStore as _useStore } from 'vuex';
import { authModule, AuthModuleState } from './modules/authModule';
import { userModule, UserModuleState } from './modules/userModule';

export type RootState = {
  authModule: AuthModuleState;
} & { userModule: UserModuleState };

export const storeKey: InjectionKey<Store<RootState>> = Symbol();

export const useStore = () => _useStore(storeKey);

export const store = createStore({
  modules: {
    authModule,
    userModule
  },
  strict: true,
  devtools: true
});
