import axios from 'axios';
import { getAccessToken, setAccessToken, clearAccessToken } from './tokenStore';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE,
  withCredentials: true,
});

api.interceptors.request.use(
  (config) => {
    const token = getAccessToken();
    if (token) {
      config.headers["Authorization"] = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

api.interceptors.response.use(
  (response) => response,
  async (error) => {
    const originalRequest = error.config;

    if (
      error.response?.status === 401 &&
      !originalRequest._retry &&
      !originalRequest.url.includes("/Auth/refresh")
    ) {
      originalRequest._retry = true;

      try {
        const { data } = await api.post("/Auth/refresh");

        if (data?.accessToken) {
          setAccessToken(data.accessToken);
          api.defaults.headers.common["Authorization"] = `Bearer ${data.accessToken}`;
          originalRequest.headers["Authorization"] = `Bearer ${data.accessToken}`;
          return api(originalRequest);
        } else {
          clearAccessToken();
        }
      } catch (refreshError) {
        clearAccessToken();
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

export default api;
