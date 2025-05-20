let accessToken = null;
let refreshToken = null;

export const setAccessToken = (token) => {
  accessToken = token;
};

export const getAccessToken = () => accessToken;

export const clearAccessToken = () => {
  accessToken = null;
};

export const setRefreshToken = (token) => {
  refreshToken = token;
};

export const getRefreshToken = () => refreshToken;

export const clearRefreshToken = () => {
  refreshToken = null;
};