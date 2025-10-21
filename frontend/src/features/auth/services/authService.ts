import axios, { AxiosError } from 'axios';
import Cookies from 'js-cookie';

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

api.interceptors.request.use((config) => {
  const token = Cookies.get('token') || localStorage.getItem('token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Interceptor para manejar errores
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
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface ResetPasswordDTO {
  email: string;
}

export interface ChangePasswordDTO {
  oldPassword: string;
  newPassword: string;
}

export interface UpdateProfileDTO {
  firstName?: string;
  lastName?: string;
  email?: string;
}

export const authService = {
  async login(credentials: LoginDTO): Promise<AuthResponse> {
    try {
      const response = await api.post('/auth/login', credentials);
  this.setAuthData(response.data);
      return response.data;
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'isAxiosError' in err) {
        const error = err as AxiosError;
        if (error.response) {
          const { status } = error.response;
          const data = error.response.data as { message?: string };
          
          switch (status) {
            case 401:
              throw new Error('Email o contraseña incorrectos');
            case 400:
              throw new Error(data.message || 'Datos de inicio de sesión inválidos');
            case 500:
              throw new Error('Error en el servidor. Por favor intenta más tarde');
            default:
              throw new Error('Error en el inicio de sesión. Por favor intenta de nuevo');
          }
        }
      }
      throw new Error('No se pudo conectar con el servidor');
    }
  },

  async register(userData: RegisterDTO): Promise<AuthResponse> {
    try {
      const response = await api.post('/auth/register', userData);
      return response.data;
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'isAxiosError' in err) {
        const error = err as AxiosError;
        if (error.response) {
          const { status } = error.response;
          const data = error.response.data as { message?: string };
          
          switch (status) {
            case 409:
              throw new Error('El email ya está registrado');
            case 400:
              throw new Error(data.message || 'Datos de registro inválidos');
            case 500:
              throw new Error('Error en el servidor. Por favor intenta más tarde');
            default:
              throw new Error('Error en el registro. Por favor intenta de nuevo');
          }
        } else {
          throw new Error('No se pudo conectar con el servidor');
        }
      }
      throw new Error('Error en el registro. Por favor intenta de nuevo');
    }
  },

  async logout(): Promise<void> {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    Cookies.remove('token');
  },

  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  },

  getToken(): string | null {
    return Cookies.get('token') || localStorage.getItem('token');
  },



  setAuthData(data: AuthResponse): void {
    localStorage.setItem('token', data.token);
    localStorage.setItem('user', JSON.stringify(data.user));
    Cookies.set('token', data.token, { expires: 7, sameSite: 'strict' });
    Cookies.set('user', JSON.stringify(data.user), { expires: 7, sameSite: 'strict' });
  },

  clearAuthData(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    Cookies.remove('token');
    Cookies.remove('user');
  },

  async requestPasswordReset(email: string): Promise<void> {
    try {
      await api.post('/auth/reset-password', { email });
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'isAxiosError' in err) {
        const error = err as AxiosError;
        if (error.response) {
          const { status } = error.response;
          const data = error.response.data as { message?: string };
          
          switch (status) {
            case 404:
              throw new Error('No se encontró una cuenta con ese email');
            case 400:
              throw new Error(data.message || 'Email inválido');
            default:
              throw new Error('Error al solicitar el cambio de contraseña');
          }
        }
      }
      throw new Error('No se pudo conectar con el servidor');
    }
  },

  async changePassword(data: ChangePasswordDTO): Promise<void> {
    try {
      await api.post('/auth/change-password', data);
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'isAxiosError' in err) {
        const error = err as AxiosError;
        if (error.response) {
          const { status } = error.response;
          const data = error.response.data as { message?: string };
          
          switch (status) {
            case 401:
              throw new Error('La contraseña actual es incorrecta');
            case 400:
              throw new Error(data.message || 'Datos inválidos');
            default:
              throw new Error('Error al cambiar la contraseña');
          }
        }
      }
      throw new Error('No se pudo conectar con el servidor');
    }
  },

  async updateProfile(data: UpdateProfileDTO): Promise<User> {
    try {
      const response = await api.put('/auth/profile', data);
      const updatedUser = response.data;
      const currentUser = JSON.parse(localStorage.getItem('user') || '{}');
      const newUserData = { ...currentUser, ...updatedUser };
      localStorage.setItem('user', JSON.stringify(newUserData));
      return newUserData;
    } catch (err: unknown) {
      if (err && typeof err === 'object' && 'isAxiosError' in err) {
        const error = err as AxiosError;
        if (error.response) {
          const { status } = error.response;
          const data = error.response.data as { message?: string };
          
          switch (status) {
            case 409:
              throw new Error('El email ya está en uso');
            case 400:
              throw new Error(data.message || 'Datos inválidos');
            default:
              throw new Error('Error al actualizar el perfil');
          }
        }
      }
      throw new Error('No se pudo conectar con el servidor');
    }
  },

  getCurrentUser(): User | null {
    try {
      const userStr = Cookies.get('user') || localStorage.getItem('user');
      return userStr ? JSON.parse(userStr) : null;
    } catch (error) {
      console.error('Error parsing user data:', error);
      this.clearAuthData();
      return null;
    }
  },

  refreshToken(): Promise<void> {
    // TODO: Implementar refresh token cuando el backend lo soporte
    return Promise.resolve();
  }
};
