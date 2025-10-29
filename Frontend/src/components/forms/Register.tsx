import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { apiService } from '../../utils/api';
import { CreateUserDto } from '../../models/user';
import { User, UserCheck, Mail, Phone, Shield, Lock, Eye, EyeOff, UserPlus } from 'lucide-react';
import './Auth.css';

const Register: React.FC = () => {
  const [formData, setFormData] = useState<CreateUserDto>({
    name: '',
    lastName: '',
    email: '',
    password: '',
    role: 'User',
    phoneNumber: '',
  });
  const [showPassword, setShowPassword] = useState<boolean>(false);
  const [error, setError] = useState<string>('');
  const [loading, setLoading] = useState<boolean>(false);
  const [acceptTerms, setAcceptTerms] = useState<boolean>(false);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');

    if (!acceptTerms) {
      setError('Debes aceptar los términos y condiciones');
      setLoading(false);
      return;
    }

    try {
      const response = await apiService.post<{ success: boolean; message: string; data: any; errors: string[] }>('/auth/register', formData);
      if (response.data.success) {
        navigate('/login');
      } else {
        setError(response.data.message || 'Error en el registro');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || err.response?.data?.errors?.[0] || 'Error en el registro');
    } finally {
      setLoading(false);
    }
  };

  const togglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <div className="auth-container">
      <div className="auth-card">
        <div className="auth-header">
          <h1 className="auth-title">Únete a nosotros</h1>
          <p className="auth-subtitle">Crea tu cuenta en Gunter Bar</p>
        </div>

        <form className="auth-form" onSubmit={handleSubmit}>
          {error && <div className="error-message">{error}</div>}

          <div className="form-group">
            <label htmlFor="name" className="form-label">Nombre</label>
            <div className="input-wrapper">
              <input
                type="text"
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                className="form-input"
                placeholder="Tu nombre"
                required
              />
              <User className="input-icon" size={20} />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="lastName" className="form-label">Apellido</label>
            <div className="input-wrapper">
              <input
                type="text"
                id="lastName"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                className="form-input"
                placeholder="Tu apellido"
              />
              <UserCheck className="input-icon" size={20} />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="email" className="form-label">Correo electrónico</label>
            <div className="input-wrapper">
              <input
                type="email"
                id="email"
                name="email"
                value={formData.email}
                onChange={handleChange}
                className="form-input"
                placeholder="tu@email.com"
                required
              />
              <Mail className="input-icon" size={20} />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="phoneNumber" className="form-label">Número de teléfono</label>
            <div className="input-wrapper">
              <input
                type="tel"
                id="phoneNumber"
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
                className="form-input"
                placeholder="+54 11 1234-5678"
              />
              <Phone className="input-icon" size={20} />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="role" className="form-label">Tipo de cuenta</label>
            <div className="input-wrapper">
              <select
                id="role"
                name="role"
                value={formData.role}
                onChange={handleChange}
                className="form-input"
                required
              >
                <option value="User">Cliente</option>
                <option value="Seller">Vendedor</option>
              </select>
              <Shield className="input-icon" size={20} />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="password" className="form-label">Contraseña</label>
            <div className="input-wrapper">
              <input
                type={showPassword ? 'text' : 'password'}
                id="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                className="form-input"
                placeholder="Mínimo 6 caracteres"
                required
                minLength={6}
              />
              <Lock className="input-icon" size={20} />
              <button
                type="button"
                className="password-toggle"
                onClick={togglePasswordVisibility}
                aria-label={showPassword ? 'Ocultar contraseña' : 'Mostrar contraseña'}
              >
                {showPassword ? <EyeOff size={18} /> : <Eye size={18} />}
              </button>
            </div>
          </div>

          <div className="form-group">
            <label className="checkbox-label">
              <input
                type="checkbox"
                id="acceptTerms"
                checked={acceptTerms}
                onChange={(e) => setAcceptTerms(e.target.checked)}
                className="checkbox-input"
                required
              />
              <span className="checkmark"></span>
              Acepto los <a href="#" className="terms-link">términos y condiciones</a>
            </label>
          </div>

          <button
            type="submit"
            className="auth-submit-btn"
            disabled={loading}
          >
            {loading ? (
              <>
                <span className="loading-spinner"></span>
                Creando cuenta...
              </>
            ) : (
              <>
                <UserPlus size={18} style={{ marginRight: '0.5rem' }} />
                Crear Cuenta
              </>
            )}
          </button>
        </form>

        <div className="auth-footer">
          <p>¿Ya tienes una cuenta? <Link to="/login" className="auth-link">Inicia sesión aquí</Link></p>
        </div>
      </div>
    </div>
  );
};

export default Register;
