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

  // Auth endpoints
  auth: {
    login: (credentials: { email: string; password: string }) =>
      api.post('/auth/login', credentials),
    register: (userData: {
      firstName: string;
      lastName: string;
      email: string;
      phone: string;
      role: string;
      password: string;
    }) =>
      api.post('/auth/register', userData),
  },

  // Drinks endpoints
  drinks: {
    getAll: () => api.get('/drinks'),
    getById: (id: number) => api.get(`/drinks/${id}`),
    create: (drinkData: any) => api.post('/drinks', drinkData),
    update: (id: number, drinkData: any) => api.put(`/drinks/${id}`, drinkData),
    delete: (id: number) => api.delete(`/drinks/${id}`),
  },

  // Orders endpoints
  orders: {
    getAll: () => api.get('/orders'),
    getById: (id: number) => api.get(`/orders/${id}`),
    getMyOrders: () => api.get('/orders/me'),
    create: (orderData: any) => api.post('/orders', orderData),
    updateStatus: (id: number, status: string) => api.put(`/orders/${id}/status`, { status }),
    cancel: (id: number) => api.post(`/orders/${id}/cancel`),
  },

  // Cart endpoints
  cart: {
    getMyCart: () => api.get('/cart/me'),
    addItem: (itemData: any) => api.post('/cart/items', itemData),
    updateItem: (itemId: number, itemData: any) => api.put(`/cart/items/${itemId}`, itemData),
    removeItem: (itemId: number) => api.delete(`/cart/items/${itemId}`),
    clearCart: () => api.post('/cart/clear'),
  },

  // Reviews endpoints
  reviews: {
    getByDrink: (drinkId: number) => api.get(`/reviews/drink/${drinkId}`),
    getMyReviews: () => api.get('/reviews/me'),
    create: (reviewData: any) => api.post('/reviews', reviewData),
    update: (id: number, reviewData: any) => api.put(`/reviews/${id}`, reviewData),
    delete: (id: number) => api.delete(`/reviews/${id}`),
  },

  // Users/Vendors endpoints
  users: {
    getVendorProfile: (id: number) => api.get(`/users/vendor/${id}`),
  },
};

export default api;
