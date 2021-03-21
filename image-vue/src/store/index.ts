import { InjectionKey } from 'vue';
import { createStore, Store, useStore as _useStore } from 'vuex';
import { accountModule, AccountModuleState } from './modules/accountModule';
import { userModule, UserModuleState } from './modules/userModule';

export type RootState = {
  accountModule: AccountModuleState;
} & { userModule: UserModuleState };

export const storeKey: InjectionKey<Store<RootState>> = Symbol();

export const useStore = () => _useStore(storeKey);

export const store = createStore({
  modules: {
    userModule,
    accountModule
  },
  strict: true,
  devtools: true
});
