import axios, { type AxiosInstance, type AxiosResponse } from 'axios';

const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

class ApiService {
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: API_BASE_URL,
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Request interceptor to add auth token
    this.api.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('token');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // Response interceptor to handle errors
    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          localStorage.removeItem('token');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Generic request methods
  async get<T>(url: string): Promise<AxiosResponse<T>> {
    return this.api.get(url);
  }

  async post<T>(url: string, data?: any): Promise<AxiosResponse<T>> {
    return this.api.post(url, data);
  }

  async put<T>(url: string, data?: any): Promise<AxiosResponse<T>> {
    return this.api.put(url, data);
  }

  async delete<T>(url: string): Promise<AxiosResponse<T>> {
    return this.api.delete(url);
  }

  // Auth endpoints
  auth = {
    login: (data: { email: string; password: string }) =>
      this.post<{ success: boolean; message: string; data: { token: string; user: any } }>('/auth/login', data),

    register: (data: any) =>
      this.post<{ success: boolean; message: string }>('/auth/register', data),

    profile: () =>
      this.get<{ success: boolean; message: string; data: any }>('/auth/profile'),
  };

  // Drinks/Products endpoints
  drinks = {
    getAll: () =>
      this.get<{ success: boolean; message: string; data: any[] }>('/drinks'),

    create: (data: any) =>
      this.post<{ success: boolean; message: string; data: any }>('/drinks', data),

    update: (id: number, data: any) =>
      this.put<{ success: boolean; message: string; data: any }>(`/drinks/${id}`, data),

    delete: (id: number) =>
      this.delete<{ success: boolean; message: string }>(`/drinks/${id}`),
  };

  // Orders endpoints
  orders = {
    getAll: () =>
      this.get<{ success: boolean; message: string; data: any[] }>('/orders'),

    getById: (id: number) =>
      this.get<{ success: boolean; message: string; data: any }>(`/orders/${id}`),

    updateStatus: (id: number, status: string) =>
      this.put<{ success: boolean; message: string; data: any }>(`/orders/${id}/status`, { status }),
  };

  // Reviews endpoints
  reviews = {
    getAll: () =>
      this.get<{ success: boolean; message: string; data: any[] }>('/reviews'),
  };
}

export const apiService = new ApiService();