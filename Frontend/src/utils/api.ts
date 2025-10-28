import axios, { AxiosResponse } from 'axios';
import Cookies from 'js-cookie';

const API_BASE_URL = 'http://localhost:5221/api';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
  withCredentials: true, // Enable sending cookies with requests
});

// Request interceptor to add auth token if available
api.interceptors.request.use(
  (config) => {
    const token = Cookies.get('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Handle unauthorized
      Cookies.remove('token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export const apiService = {
  get: <T>(url: string): Promise<AxiosResponse<T>> => api.get(url),
  post: <T>(url: string, data?: any): Promise<AxiosResponse<T>> => api.post(url, data),
  put: <T>(url: string, data?: any): Promise<AxiosResponse<T>> => api.put(url, data),
  delete: <T>(url: string): Promise<AxiosResponse<T>> => api.delete(url),
};

export default api;
