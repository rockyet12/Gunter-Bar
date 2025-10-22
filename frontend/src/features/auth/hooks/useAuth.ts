import { useState } from 'react';
import { authService, AuthResponse } from '../services/authService';

export interface UseAuth {
  isAuthenticated: boolean;
  user: AuthResponse['user'] | null;
  login: (email: string, password: string) => Promise<void>;
  register: (name: string, email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  updateUser: (userData: Partial<AuthResponse['user']>) => void;
}

export const useAuth = (): UseAuth => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(authService.isAuthenticated());
  const [user, setUser] = useState<AuthResponse['user'] | null>(() => {
    try {
      const savedUser = localStorage.getItem('user');
      return savedUser ? JSON.parse(savedUser) : null;
    } catch (error) {
      console.error('Error parsing user data:', error);
      localStorage.removeItem('user');
      return null;
    }
  });

  const login = async (email: string, password: string) => {
    try {
      const response = await authService.login({ email, password });
      authService.setAuthData(response);
      setIsAuthenticated(true);
      setUser(response.user);
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  };

  const register = async (name: string, email: string, password: string) => {
    try {
      const response = await authService.register({ name, email, password });
      authService.setAuthData(response);
      setIsAuthenticated(true);
      setUser(response.user);
    } catch (error) {
      console.error('Register error:', error);
      throw error;
    }
  };

  const logout = async () => {
    try {
      await authService.logout();
      setIsAuthenticated(false);
      setUser(null);
    } catch (error) {
      console.error('Logout error:', error);
      throw error;
    }
  };

  const updateUser = (userData: Partial<AuthResponse['user']>) => {
    if (!user) return;
    const updatedUser = { ...user, ...userData };
    localStorage.setItem('user', JSON.stringify(updatedUser));
    setUser(updatedUser);
  };

  return {
    isAuthenticated,
    user,
    login,
    register,
    logout,
    updateUser
  };
};
