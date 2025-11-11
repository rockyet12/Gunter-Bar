import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import Cookies from 'js-cookie';
import { apiService } from '../../utils/api';
import { AuthResponseDto, User } from '../../models/user';

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
      const token = Cookies.get('token');

      if (token) {
        try {
          // Validate token by fetching user profile
          const response = await apiService.get<{ success: boolean; message: string; data: User }>('/auth/profile');

          if (response.data.success && response.data.data) {
            const backendUser = response.data.data;
            const userData = {
              ...backendUser,
              role: (backendUser.role === 'Client' || backendUser.role === 'Customer') ? 'User' : 'Seller'
            };
            setIsAuthenticated(true);
            setUser(userData);
          } else {
            // Token is invalid, remove it
            Cookies.remove('token');
            setIsAuthenticated(false);
            setUser(null);
          }
        } catch (error: any) {
          // Only remove cookie if it's an authentication error (401)
          // For network errors or server issues, retry a few times before giving up
          if (error.response?.status === 401) {
            Cookies.remove('token');
            setIsAuthenticated(false);
            setUser(null);
          } else if (retryCount < 3) {
            // Retry after a short delay for network/server errors
            console.warn(`Token validation failed, retrying... (${retryCount + 1}/3)`);
            setTimeout(() => validateToken(retryCount + 1), 1000 * (retryCount + 1));
            return;
          } else {
            // After 3 retries, remove the cookie to prevent infinite loading
            console.error('Token validation failed after retries, clearing session');
            Cookies.remove('token');
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
      const response = await apiService.post<{ success: boolean; message: string; data: AuthResponseDto }>('/auth/login', {
        email,
        password
      });

      if (response.data.success && response.data.data) {
        const { token, user: backendUser } = response.data.data;

        const userData = {
          ...backendUser,
          role: (backendUser.role === 'Client' || backendUser.role === 'Customer') ? 'User' : 'Seller'
        };

        // Save token in cookie (expires in 7 days)
        // In development, don't use secure flag since we use HTTP
        const isProduction = import.meta.env.PROD;
        Cookies.set('token', token, {
          expires: 7,
          secure: isProduction, // Only secure in production (HTTPS)
          sameSite: 'lax', // Changed from 'strict' to 'lax' for cross-origin requests
          path: '/' // Ensure cookie is available for all paths
        });

        setIsAuthenticated(true);
        setUser(userData);
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
        // Optionally auto-login after registration
        // await login(userData.email, userData.password);
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
      const response = await apiService.get<{ success: boolean; message: string; data: User }>('/auth/profile');

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
      // For network errors or server issues, keep the session
      if (error.response?.status === 401) {
        Cookies.remove('token');
        setIsAuthenticated(false);
        setUser(null);
      }
      // For other errors, don't remove the cookie - allow retry later
    }
  };

  const updateUser = (updatedUser: User) => {
    setUser(updatedUser);
  };

  const logout = () => {
    Cookies.remove('token');
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
