import { reactive, Ref, ToRefs, toRefs, UnwrapRef } from 'vue';

const isValidResponse = (response?: Response): response is Response =>
  response !== null && response !== undefined;

interface RequestOptions extends RequestInit {
  headers: Record<string, string>;
  uri: string;
}

type State<TData> = {
  response: Response | undefined;
  loading: boolean;
  isValid: boolean;
  data: TData | undefined;
  error: any;
};

type BaseOptions = {
  beforeEach?(options: RequestOptions): Promise<void>;
};

type FetchOptions = {
  immediat: boolean;
};

type UnwrapNestedRefs<T> = T extends Ref ? T : UnwrapRef<T>;

type ToRefsState<TData> = ToRefs<UnwrapNestedRefs<State<TData>>>;

export function createFetch(baseUri: string, baseOptions: BaseOptions = {}) {
  const useFetch = <TData>(
    fetchOptions: FetchOptions = { immediat: false }
  ) => {
    const state = reactive<State<TData>>({
      response: undefined,
      loading: false,
      isValid: false,
      data: undefined,
      error: undefined
    });

    const execute = (uri: string, headers?: RequestOptions['headers']) => {
      const parsingOptions = (options: RequestOptions) => {
        const makeRequest = async (options: RequestOptions) => {
          try {
            console.log(`FETCH ${options.uri}`);
            await baseOptions.beforeEach?.(options);
            state.loading = true;
            state.isValid = true;
            state.response = await fetch(options.uri, options);

            if (!state.response.ok) {
              state.isValid = false;
            }
          } catch {
            state.isValid = false;
          } finally {
            state.loading = false;
          }
        };

        const fetchWhenImmediat = (promise: Promise<ToRefsState<TData>>) => {
          if (fetchOptions.immediat) {
            promise
              .then(({ data }) => {
                state.data = data.value;
              })
              .catch(() => {
                console.warn('unexpected catch');
              });
          }
        };

        const getRequestPromise = async (type: 'json' | 'blob' | 'none') => {
          await makeRequest(options);

          if (
            isValidResponse(state.response) &&
            state.isValid &&
            type !== 'none'
          ) {
            console.log(type);
            state.data = await state.response[type]();
          }

          return toRefs(state);
        };

        return {
          json() {
            const promise = getRequestPromise('json');
            fetchWhenImmediat(promise);
            return { execute, promise, ...toRefs(state) };
          },
          blob() {
            const promise = getRequestPromise('blob');
            fetchWhenImmediat(promise);
            return { execute, promise, ...toRefs(state) };
          },
          none() {
            const promise = getRequestPromise('blob');
            fetchWhenImmediat(promise);
            return { execute, promise, ...toRefs(state) };
          }
        };
      };

      const methodOptions = (options: RequestOptions) => {
        return {
          get() {
            options.method = 'GET';
            return parsingOptions(options);
          },
          post(body?: any) {
            options.method = 'POST';
            options.body = body;
            return parsingOptions(options);
          },
          put(body?: any) {
            options.method = 'PUT';
            options.body = body;
            return parsingOptions(options);
          },
          delete(body?: any) {
            options.method = 'DELETE';
            options.body = body;
            return parsingOptions(options);
          }
        };
      };

      return headers
        ? methodOptions({ uri: baseUri + uri, headers })
        : methodOptions({
            uri: baseUri + uri,
            headers: { 'Content-Type': 'application/json' }
          });
    };

    return { execute, ...toRefs(state) };
  };

  return { useFetch };
}
