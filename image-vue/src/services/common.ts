export type PagedResponse<T> = {
  data: T[];
  total: number;
};

export type Envelop<T> = {
  data: T;
};

export const isCurrentTokenExpired = () => {
  const expiryTime = localStorage.getItem('tokenExpiryTime');

  return expiryTime ? Date.now() >= Number.parseInt(expiryTime) * 1000 : false;
};
