import React, { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { apiService } from '../../utils/api';
import type { User } from '../../models';

interface AuthContextType {
  isAuthenticated: boolean;
  user: User | null;
  login: (email: string, password: string) => Promise<void>;
  register: (userData: { firstName: string; lastName: string; email: string; phone: string; role: string; password: string }) => Promise<void>;
  logout: () => void;
  loading: boolean;
  refreshUser: () => Promise<void>;
  updateUser: (updatedUser: User) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState<boolean>(true);

  // Check if user is authenticated on app start
  useEffect(() => {
    const validateToken = async (retryCount = 0) => {
      const token = localStorage.getItem('token');

      if (token) {
        try {
          // Validate token by fetching user profile
          const response = await apiService.auth.profile();

          if (response.data.success && response.data.data) {
            const backendUser = response.data.data;
            const userData = {
              ...backendUser,
              role: (backendUser.role === 'Client' || backendUser.role === 'Customer') ? 'User' : 'Seller'
            };
            setIsAuthenticated(true);
            setUser(userData);

            // Redirect to customer frontend if user is a customer
            if (userData.role === 'User') {
              window.location.href = 'http://localhost:5173';
            }
          } else {
            // Token is invalid, remove it
            localStorage.removeItem('token');
            setIsAuthenticated(false);
            setUser(null);
          }
        } catch (error: any) {
          // Only remove token if it's an authentication error (401)
          if (error.response?.status === 401) {
            localStorage.removeItem('token');
            setIsAuthenticated(false);
            setUser(null);
          } else if (retryCount < 3) {
            // Retry after a short delay for network/server errors
            setTimeout(() => validateToken(retryCount + 1), 1000 * (retryCount + 1));
            return;
          } else {
            // After 3 retries, remove the token to prevent infinite loading
            localStorage.removeItem('token');
            setIsAuthenticated(false);
            setUser(null);
          }
        }
      }
      setLoading(false);
    };

    validateToken();
  }, []);

  const login = async (email: string, password: string) => {
    try {
      const response = await apiService.auth.login({ email, password });

      if (response.data.success && response.data.data) {
        const { token, user: backendUser } = response.data.data;

        const userData = {
          ...backendUser,
          role: (backendUser.role === 'Client' || backendUser.role === 'Customer') ? 'User' : 'Seller'
        };

        // Save token in localStorage
        localStorage.setItem('token', token);

        setIsAuthenticated(true);
        setUser(userData);

        // Redirect to customer frontend if user is a customer
        if (userData.role === 'User') {
          window.location.href = 'http://localhost:5173';
        }
      } else {
        const errorMessage = response.data.message || 'Error al iniciar sesión';
        throw new Error(errorMessage);
      }
    } catch (error: any) {
      // Handle different types of errors
      if (error.response?.status === 401) {
        throw new Error('Correo electrónico o contraseña inválidos');
      } else if (error.response?.status === 400) {
        throw new Error(error.response.data?.message || 'Invalid login data');
      } else if (error.response?.data?.message) {
        throw new Error(error.response.data.message);
      } else if (error.message) {
        throw new Error(error.message);
      } else {
        throw new Error('Error al iniciar sesión. Por favor, inténtalo de nuevo.');
      }
    }
  };

  const register = async (userData: { firstName: string; lastName: string; email: string; phone: string; role: string; password: string }) => {
    try {
      const response = await apiService.auth.register(userData);

      if (response.data.success) {
        // Registration successful, but user needs to login
        console.log('Usuario registrado exitosamente:', response.data);
      } else {
        throw new Error(response.data.message || 'Error en el registro');
      }
    } catch (error: any) {
      console.error('Error en registro:', error);
      throw new Error(error.response?.data?.message || 'Error en el registro');
    }
  };

  const refreshUser = async () => {
    try {
      const response = await apiService.auth.profile();

      if (response.data.success && response.data.data) {
        const backendUser = response.data.data;
        const userData = {
          ...backendUser,
          role: (backendUser.role === 'Client' || backendUser.role === 'Customer') ? 'User' : 'Seller'
        };
        setUser(userData);
      }
    } catch (error: any) {
      // Only logout if it's an authentication error (401)
      if (error.response?.status === 401) {
        localStorage.removeItem('token');
        setIsAuthenticated(false);
        setUser(null);
      }
      // For other errors, don't remove the token
    }
  };

  const updateUser = (updatedUser: User) => {
    setUser(updatedUser);
  };

  const logout = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
    setUser(null);
  };

  const value: AuthContextType = {
    login,
    register,
    logout,
    user,
    isAuthenticated,
    loading,
    refreshUser,
    updateUser
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};