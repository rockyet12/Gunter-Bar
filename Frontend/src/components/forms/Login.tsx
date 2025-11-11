import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { Mail, Lock, Loader2, UserPlus, Eye, EyeOff } from 'lucide-react';
import { Link, useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from './AuthContext';
import Input from './Input';
import './Auth.css';

const schema = yup.object({
  email: yup.string().email('Email inválido').required('Email es requerido'),
  password: yup.string().min(6, 'Contraseña debe tener al menos 6 caracteres').required('Contraseña es requerida'),
});

type FormData = {
  email: string;
  password: string;
};

const Login: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);
  const { login, isAuthenticated, user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: yupResolver(schema),
  });

  useEffect(() => {
    // Check for message from navigation state (e.g., from registration success)
    if (location.state?.message) {
      setMessage({
        text: location.state.message,
        type: location.state.type || 'success'
      });
      // Clear the state to prevent showing the message again on refresh
      navigate(location.pathname, { replace: true, state: {} });
    }
  }, [location, navigate]);

  useEffect(() => {
    if (isAuthenticated && user) {
      if (user.role === 'Seller') {
        navigate('/seller');
      } else {
        navigate('/dashboard');
      }
    }
  }, [isAuthenticated, user, navigate]);

  const onSubmit = async (data: FormData) => {
    setIsLoading(true);
    setMessage(null);

    try {
      await login(data.email, data.password);
      // Login successful - redirect will be handled by useEffect based on role
    } catch (error: any) {
      console.error('Login error:', error);
      setMessage({
        text: error.message || 'Error al iniciar sesión. Verifica tus credenciales.',
        type: 'error'
      });
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="auth-card-header">
          <h3>Iniciar Sesión</h3>
        </div>
        <div className="auth-card-body">
          {message && (
            <div className={`mb-4 p-3 border rounded-lg ${
              message.type === 'success'
                ? 'bg-green-100 border-green-400 text-green-700'
                : 'bg-red-100 border-red-400 text-red-700'
            }`}>
              {message.text}
            </div>
          )}
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Input
              label="Email"
              type="email"
              placeholder="Ingresa tu email"
              icon={<Mail size={18} />}
              error={errors.email}
              {...register('email')}
            />
            <div className="relative">
              <Input
                label="Contraseña"
                type={showPassword ? "text" : "password"}
                placeholder="Ingresa tu contraseña"
                icon={<Lock size={18} />}
                error={errors.password}
                {...register('password')}
              />
              <button
                type="button"
                className="auth-password-toggle"
                onClick={() => setShowPassword(!showPassword)}
              >
                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
            </div>
            <button
              type="submit"
              className="auth-btn-primary w-full flex items-center justify-center gap-2 disabled:opacity-50"
              disabled={isLoading}
            >
              {isLoading ? <Loader2 size={18} className="animate-spin" /> : null}
              {isLoading ? 'Iniciando...' : 'Iniciar Sesión'}
            </button>
          </form>
          <div className="text-center mt-4 space-y-2">
            <a href="#" className="auth-link block">¿Olvidaste tu contraseña?</a>
            <p className="text-white">
              ¿No tienes cuenta?{' '}
              <Link to="/register" className="auth-link inline-flex items-center gap-1">
                <UserPlus size={16} />
                Regístrate aquí
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;
