import axios from 'axios';

export const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE,
    withCredentials: true,
    timeout: 10000,
    headers: { 
    'Content-Type': 'application/json',
    'X-Requested-With': 'XMLHttpRequest'
    },
});

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        localStorage.removeItem('accesstoken');
        window.location.href = '/login';
      }
      return Promise.reject(error);
    }
  );