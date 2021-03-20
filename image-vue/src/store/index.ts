import { InjectionKey } from 'vue';
import { createStore, Store, useStore as _useStore } from 'vuex';
import { AccountModuleState, accountModule } from './modules/userModule';

export type RootState = {
  accountModule: AccountModuleState;
};

export const storeKey: InjectionKey<Store<RootState>> = Symbol();

export const useStore = () => _useStore(storeKey);

export const store = createStore({
  modules: {
    accountModule
  }
});
