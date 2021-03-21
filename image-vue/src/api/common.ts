export type PagedResponse<T> = {
  data: T[];
  total: number;
};

export type Envelop<T> = {
  data: T;
};

export type JwtClaims = {
  exp: string;
  userId: string;
  username: string;
};

export const isCurrentTokenExpired = () => {
  const expiryTime = localStorage.getItem('tokenExpiryTime');

  return expiryTime ? Date.now() >= Number.parseInt(expiryTime) * 1000 : false;
};
