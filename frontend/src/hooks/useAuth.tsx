import { useState, useContext, createContext, useEffect, ReactNode } from 'react';
import { User, LoginDto, RegisterUserDto, AuthResponse } from '../types';
import apiService from '../services/apiService';

interface AuthContextType {
  user: User | null;
  login: (credentials: LoginDto) => Promise<boolean>;
  register: (userData: RegisterUserDto) => Promise<boolean>;
  logout: () => void;
  loading: boolean;
  error: string | null;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Verificar si hay un usuario logueado al cargar la aplicación
  useEffect(() => {
    const checkAuth = async () => {
      try {
        const token = apiService.getAuthToken();
        if (token) {
          const response = await apiService.getCurrentUser();
          if (response.success && response.data) {
            setUser(response.data);
          } else {
            // Token inválido, limpiar
            apiService.removeAuthToken();
          }
        }
      } catch (error) {
        console.error('Error checking auth:', error);
        apiService.removeAuthToken();
      } finally {
        setLoading(false);
      }
    };

    checkAuth();
  }, []);

  const login = async (credentials: LoginDto): Promise<boolean> => {
    try {
      setLoading(true);
      setError(null);

      const response = await apiService.login(credentials);
      
      if (response.success && response.data) {
        const authData: AuthResponse = response.data;
        
        // Guardar token y usuario
        apiService.setAuthToken(authData.token);
        localStorage.setItem('user', JSON.stringify(authData.user));
        setUser(authData.user);
        
        return true;
      } else {
        setError(response.message || 'Error al iniciar sesión');
        return false;
      }
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Error de conexión';
      setError(errorMessage);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const register = async (userData: RegisterUserDto): Promise<boolean> => {
    try {
      setLoading(true);
      setError(null);

      const response = await apiService.register(userData);
      
      if (response.success && response.data) {
        const authData: AuthResponse = response.data;
        
        // Guardar token y usuario
        apiService.setAuthToken(authData.token);
        localStorage.setItem('user', JSON.stringify(authData.user));
        setUser(authData.user);
        
        return true;
      } else {
        setError(response.message || 'Error al registrarse');
        return false;
      }
    } catch (error: any) {
      const errorMessage = error.response?.data?.message || 'Error de conexión';
      setError(errorMessage);
      return false;
    } finally {
      setLoading(false);
    }
  };

  const logout = () => {
    apiService.removeAuthToken();
    setUser(null);
    setError(null);
  };

  const value: AuthContextType = {
    user,
    login,
    register,
    logout,
    loading,
    error,
    isAuthenticated: !!user
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};

// Hook personalizado para usar el contexto de autenticación
export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth debe ser usado dentro de un AuthProvider');
  }
  return context;
};
