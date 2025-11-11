import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { User, Mail, Lock, Loader2, LogIn, Phone, Eye, EyeOff } from 'lucide-react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from './AuthContext';
import Input from './Input';
import Select from './Select';
import './Auth.css';

const schema = yup.object({
  firstName: yup.string().min(2, 'Nombre debe tener al menos 2 caracteres').required('Nombre es requerido'),
  lastName: yup.string().min(2, 'Apellido debe tener al menos 2 caracteres').required('Apellido es requerido'),
  email: yup.string().email('Email inválido').required('Email es requerido'),
  phone: yup.string().matches(/^[0-9+\-\s()]+$/, 'Teléfono inválido').min(8, 'Teléfono debe tener al menos 8 dígitos').required('Teléfono es requerido'),
  role: yup.string().required('Rol es requerido'),
  password: yup.string().min(8, 'Contraseña debe tener al menos 8 caracteres').matches(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)/, 'La contraseña debe contener al menos una letra minúscula, una mayúscula y un número').required('Contraseña es requerida'),
  confirmPassword: yup.string().oneOf([yup.ref('password')], 'Las contraseñas no coinciden').required('Confirmar contraseña es requerida'),
});

type FormData = {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: string;
  password: string;
  confirmPassword: string;
};

const Register: React.FC = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);
  const [error, setError] = useState<string>('');
  const { register: registerUser } = useAuth();
  const navigate = useNavigate();
  const { register, handleSubmit, formState: { errors } } = useForm<FormData>({
    resolver: yupResolver(schema),
  });

  const onSubmit = async (data: FormData) => {
    setIsLoading(true);
    setError('');

    try {
      // Remove confirmPassword before sending to API
      const { confirmPassword, ...userData } = data;
      await registerUser(userData);

      // Registration successful - redirect based on role
      if (data.role === 'SalesManager') {
        navigate('/seller');
      } else {
        navigate('/login', {
          state: {
            message: '¡Registro exitoso! Ahora puedes iniciar sesión.',
            type: 'success'
          }
        });
      }
    } catch (error: any) {
      console.error('Registration error:', error);
      setError(error.message || 'Error al registrar usuario. Inténtalo de nuevo.');
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
          {error && (
            <div className="mb-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded-lg">
              {error}
            </div>
          )}
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
            <Input
              label="Nombre"
              type="text"
              placeholder="Ingresa tu nombre"
              icon={<User size={18} />}
              error={errors.firstName}
              {...register('firstName')}
            />
            <Input
              label="Apellido"
              type="text"
              placeholder="Ingresa tu apellido"
              icon={<User size={18} />}
              error={errors.lastName}
              {...register('lastName')}
            />
            <Input
              label="Email"
              type="email"
              placeholder="Ingresa tu email"
              icon={<Mail size={18} />}
              error={errors.email}
              {...register('email')}
            />
            <Input
              label="Teléfono"
              type="tel"
              placeholder="Ingresa tu número de teléfono"
              icon={<Phone size={18} />}
              error={errors.phone}
              {...register('phone')}
            />
            <Select
              label="Rol"
              placeholder="Selecciona tu rol"
              error={errors.role}
              {...register('role')}
            >
              <option value="SalesManager">Vendedor</option>
              <option value="Client">Cliente</option>
            </Select>
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
            <div className="relative">
              <Input
                label="Confirmar Contraseña"
                type={showConfirmPassword ? "text" : "password"}
                placeholder="Confirma tu contraseña"
                icon={<Lock size={18} />}
                error={errors.confirmPassword}
                {...register('confirmPassword')}
              />
              <button
                type="button"
                className="auth-password-toggle"
                onClick={() => setShowConfirmPassword(!showConfirmPassword)}
              >
                {showConfirmPassword ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
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
          <div className="text-center mt-4">
            <p className="text-white">
              ¿Ya tienes cuenta?{' '}
              <Link to="/login" className="auth-link inline-flex items-center gap-1">
                <LogIn size={16} />
                Inicia sesión aquí
              </Link>
            </p>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Register;
