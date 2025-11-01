import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { User, Mail, Lock, Loader2 } from 'lucide-react';
import './Auth.css';

const schema = yup.object({
  name: yup.string().min(2, 'Nombre debe tener al menos 2 caracteres').required('Nombre es requerido'),
  email: yup.string().email('Email inválido').required('Email es requerido'),
  password: yup.string().min(6, 'Contraseña debe tener al menos 6 caracteres').required('Contraseña es requerida'),
  confirmPassword: yup.string().oneOf([yup.ref('password')], 'Las contraseñas no coinciden').required('Confirmar contraseña es requerida'),
});

type FormData = {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
};

const Register: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: yupResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    setIsLoading(true);
    try {
      // Handle register logic here
      console.log('Register:', data);
      // Simulate API call
      await new Promise(resolve => setTimeout(resolve, 2000));
    } catch (error) {
      console.error('Register error:', error);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="auth-card-header">
          <h3>Registrarse</h3>
        </div>
        <div className="auth-card-body">
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <div className="mb-3">
              <label htmlFor="name" className="auth-form-label flex items-center gap-2">
                <User size={18} />
                Nombre
              </label>
              <input
                type="text"
                className={`auth-form-control ${errors.name ? 'border-yellow-500' : ''}`}
                id="name"
                {...register('name')}
                placeholder="Ingresa tu nombre"
              />
              {errors.name && <p className="text-yellow-400 text-sm mt-1">{errors.name.message}</p>}
            </div>
            <div className="mb-3">
              <label htmlFor="email" className="auth-form-label flex items-center gap-2">
                <Mail size={18} />
                Email
              </label>
              <input
                type="email"
                className={`auth-form-control ${errors.email ? 'border-yellow-500' : ''}`}
                id="email"
                {...register('email')}
                placeholder="Ingresa tu email"
              />
              {errors.email && <p className="text-yellow-400 text-sm mt-1">{errors.email.message}</p>}
            </div>
            <div className="mb-3">
              <label htmlFor="password" className="auth-form-label flex items-center gap-2">
                <Lock size={18} />
                Contraseña
              </label>
              <input
                type="password"
                className={`auth-form-control ${errors.password ? 'border-yellow-500' : ''}`}
                id="password"
                {...register('password')}
                placeholder="Ingresa tu contraseña"
              />
              {errors.password && <p className="text-yellow-400 text-sm mt-1">{errors.password.message}</p>}
            </div>
            <div className="mb-3">
              <label htmlFor="confirmPassword" className="auth-form-label flex items-center gap-2">
                <Lock size={18} />
                Confirmar Contraseña
              </label>
              <input
                type="password"
                className={`auth-form-control ${errors.confirmPassword ? 'border-yellow-500' : ''}`}
                id="confirmPassword"
                {...register('confirmPassword')}
                placeholder="Confirma tu contraseña"
              />
              {errors.confirmPassword && <p className="text-yellow-400 text-sm mt-1">{errors.confirmPassword.message}</p>}
            </div>
            <button
              type="submit"
              className="auth-btn-success w-full flex items-center justify-center gap-2 disabled:opacity-50"
              disabled={isLoading}
            >
              {isLoading ? <Loader2 size={18} className="animate-spin" /> : null}
              {isLoading ? 'Registrando...' : 'Registrarse'}
            </button>
          </form>
          <div className="text-center mt-3">
            <span style={{ color: '#FFFFFF' }}>¿Ya tienes cuenta? </span>
            <a href="#" className="auth-link">Inicia sesión</a>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;