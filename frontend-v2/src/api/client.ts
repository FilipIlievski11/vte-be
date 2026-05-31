import axios, { type AxiosInstance } from 'axios';
import { useAuthStore } from '@/stores/auth';

export const api: AxiosInstance = axios.create({
  baseURL: '/api',
  timeout: 30000,
});

// Attach JWT on every request.
api.interceptors.request.use((config) => {
  const auth = useAuthStore();
  if (auth.token) {
    config.headers = config.headers ?? {};
    (config.headers as Record<string, string>).Authorization = `Bearer ${auth.token}`;
  }
  return config;
});

// 401 → drop session and bounce to /login.
api.interceptors.response.use(
  (r) => r,
  (err) => {
    if (err?.response?.status === 401) {
      const auth = useAuthStore();
      auth.clear();
      if (window.location.pathname !== '/login') {
        window.location.href = '/login';
      }
    }
    return Promise.reject(err);
  }
);
