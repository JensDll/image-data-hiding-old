import { Store } from 'vuex';
import { FetchOptions } from '../composition/useFetch';
import { RootState } from '../store';
import { authClient } from './apiClient';

type EnocdeRandomRequest = {
  userId: number | string;
  username: string;
  message: string;
};

type EncodeResponse = {
  message: string;
};

export const imageService = (fetchOptions?: FetchOptions) => ({
  encodeWithFile(formData: FormData, store: Store<RootState>) {
    return authClient
      .useFetch<Blob>(fetchOptions)
      .execute('/api/image/encode/file', {})
      .post(formData)
      .blob(store);
  },
  encodeRandom(request: EnocdeRandomRequest, store: Store<RootState>) {
    return authClient
      .useFetch<Blob>(fetchOptions)
      .execute('/api/image/encode/random')
      .post(JSON.stringify(request))
      .blob(store);
  },
  decodeMessage(file: File, store: Store<RootState>) {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return authClient
      .useFetch<EncodeResponse>(fetchOptions)
      .execute('/api/image/decode', {})
      .post(formData)
      .json(store);
  }
});
