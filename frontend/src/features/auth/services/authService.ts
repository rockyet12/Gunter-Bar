import axios, { AxiosError } from 'axios';

const API_URL = 'http://localhost:5221/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token');
      localStorage.removeItem('user');
    }
    return Promise.reject(error);
  }
);

export interface LoginDTO {
  email: string;
  password: string;
}

export interface RegisterDTO {
  name: string;
  email: string;
  password: string;
}

export interface User {
  id: number;
  name: string;
  email: string;
  role?: string;
  phoneNumber?: string;
  address?: string;
  profileImageUrl?: string;
  deliveryDescription?: string;
  birthDate?: string;
}

export interface AuthResponse {
  token: string;
  refreshToken?: string;
  user: User;
}

export const authService = {
  async login({ email, password }: LoginDTO): Promise<AuthResponse> {
    const resp = await api.post('/auth/login', { email, password });
    const data = resp.data?.data;
    if (!data?.token || !data?.user) throw new Error('Credenciales inválidas');
    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify(data.user));
    return { token: data.token, refreshToken: data.refreshToken, user: data.user };
  },

  async register({ name, email, password }: RegisterDTO): Promise<AuthResponse> {
    const resp = await api.post('/auth/register', { name, email, password });
    const data = resp.data?.data;
    if (!data?.token || !data?.user) throw new Error('No se pudo registrar');
    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify(data.user));
    return { token: data.token, refreshToken: data.refreshToken, user: data.user };
  },

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    return Promise.resolve();
  },

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  },

  setAuthData(auth: AuthResponse) {
    localStorage.setItem('token', auth.token);
    localStorage.setItem('user', JSON.stringify(auth.user));
  },
};
