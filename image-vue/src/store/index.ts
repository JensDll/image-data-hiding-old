import { InjectionKey } from 'vue';
import { createStore, Store, useStore as _useStore } from 'vuex';
import { UserModuleState, userModule } from './modules/userModule';

export type RootState = {
  userModule: UserModuleState;
};

export const storeKey: InjectionKey<Store<RootState>> = Symbol();

export const useStore = () => _useStore(storeKey);

export const store = createStore({
  modules: {
    userModule
  }
});
