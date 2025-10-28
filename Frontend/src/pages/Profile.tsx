import React, { useState, useEffect } from 'react';
import { useAuth } from '../components/forms/AuthContext';
import { apiService } from '../utils/api';

interface UserProfile {
  id: number;
  name: string;
  lastName?: string;
  email: string;
  role: 'Admin' | 'Employee' | 'SalesManager' | 'Client';
  phoneNumber?: string;
  address?: string;
  dni?: string;
  deliveryDescription?: string;
  profileImageUrl?: string;
  birthDate?: string;
}

interface UpdateProfileData {
  name: string;
  lastName?: string;
  phoneNumber?: string;
  address?: string;
  dni?: string;
  deliveryDescription?: string;
}

const Profile: React.FC = () => {
  const { user, updateUser } = useAuth();
  const [profile, setProfile] = useState<UserProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [uploadingImage, setUploadingImage] = useState(false);
  const [error, setError] = useState<string>('');
  const [success, setSuccess] = useState<string>('');

  const [profileData, setProfileData] = useState<UpdateProfileData>({
    name: '',
    lastName: '',
    phoneNumber: '',
    address: '',
    dni: '',
    deliveryDescription: ''
  });

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    try {
      const response = await apiService.get<{ success: boolean; data: UserProfile }>('/auth/profile');
      if (response.data.success && response.data.data) {
        const userData = response.data.data;
        setProfile(userData);
        setProfileData({
          name: userData.name || '',
          lastName: userData.lastName || '',
          phoneNumber: userData.phoneNumber || '',
          address: userData.address || '',
          dni: userData.dni || '',
          deliveryDescription: userData.deliveryDescription || ''
        });
      }
    } catch (error) {
      setError('Error al cargar el perfil');
    } finally {
      setLoading(false);
    }
  };

  const handleProfileSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSaving(true);
    setError('');
    setSuccess('');

    try {
      const response = await apiService.put<{ success: boolean; data: UserProfile; message: string }>(`/users/${user?.id}`, profileData);
      if (response.data.success) {
        setProfile(response.data.data);
        updateUser(response.data.data);
        setSuccess('Perfil actualizado exitosamente');
      } else {
        setError(response.data.message || 'Error al actualizar el perfil');
      }
    } catch (error: any) {
      setError(error.response?.data?.message || 'Error al actualizar el perfil');
    } finally {
      setSaving(false);
    }
  };

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setProfileData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleImageUpload = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (!file) return;

    // Validate file type
    if (!file.type.startsWith('image/')) {
      setError('Por favor selecciona un archivo de imagen válido');
      return;
    }

    // Validate file size (max 5MB)
    if (file.size > 5 * 1024 * 1024) {
      setError('La imagen no puede ser mayor a 5MB');
      return;
    }

    setUploadingImage(true);
    setError('');
    setSuccess('');

    try {
      const formData = new FormData();
      formData.append('file', file);

      const response = await apiService.post<{ success: boolean; data: UserProfile; message: string }>(
        `/users/${user?.id}/profile-image`,
        formData
      );

      if (response.data.success && response.data.data) {
        setProfile(response.data.data);
        updateUser(response.data.data); // Update the auth context
        setSuccess('Foto de perfil actualizada exitosamente');
      } else {
        setError(response.data.message || 'Error al subir la imagen');
      }
    } catch (error: any) {
      setError(error.response?.data?.message || 'Error al subir la imagen');
    } finally {
      setUploadingImage(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center py-20">
        <div className="text-text-light text-xl">Cargando perfil...</div>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Header Section */}
      <div className="flex flex-wrap justify-between gap-3 pb-6 border-b border-white/10">
        <div className="flex min-w-72 flex-col gap-3">
          <p className="text-text-light text-4xl font-black leading-tight tracking-[-0.033em] font-display">Mi Perfil</p>
          <p className="text-text-muted text-base font-normal leading-normal font-display">
            Gestiona tu información personal y configuración de la cuenta.
          </p>
        </div>
      </div>

      {/* Profile Image and Info Section */}
      <div className="flex p-4 @container mt-6">
        <div className="flex w-full flex-col gap-4 @[520px]:flex-row @[520px]:justify-between @[520px]:items-center">
          <div className="flex gap-4 items-center">
            <div
              className="bg-center bg-no-repeat aspect-square bg-cover rounded-full w-24 h-24 sm:w-32 sm:h-32 border-4 border-accent"
              style={{
                backgroundImage: `url("${profile?.profileImageUrl || 'https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=150&h=150&fit=crop&crop=face'}")`
              }}
            />
            <div className="flex flex-col justify-center">
              <p className="text-text-light text-[22px] font-bold leading-tight tracking-[-0.015em] font-display">
                {profile?.name} {profile?.lastName}
              </p>
              <p className="text-text-muted text-base font-normal leading-normal font-display">
                {profile?.email}
              </p>
            </div>
          </div>

          {/* Upload Image Button */}
          <div className="flex flex-col items-start @[520px]:items-end gap-2">
            <label htmlFor="profileImage" className="cursor-pointer">
              <div className="flex items-center justify-center rounded-lg h-10 px-4 bg-primary text-white text-sm font-bold hover:bg-primary/90 transition-colors duration-200">
                {uploadingImage ? 'Subiendo...' : 'Cambiar Foto'}
              </div>
            </label>
            <input
              type="file"
              id="profileImage"
              accept="image/*"
              onChange={handleImageUpload}
              style={{ display: 'none' }}
              disabled={uploadingImage}
            />
            <p className="text-text-muted text-xs text-center @[520px]:text-right">
              Formatos: JPG, PNG, GIF. Máx: 5MB
            </p>
          </div>
        </div>
      </div>

      {/* Error/Success Messages */}
      {error && (
        <div className="bg-red-900/20 border border-red-500/20 text-red-400 px-4 py-3 rounded-lg">
          {error}
        </div>
      )}
      {success && (
        <div className="bg-green-900/20 border border-green-500/20 text-green-400 px-4 py-3 rounded-lg">
          {success}
        </div>
      )}

      {/* Profile Form */}
      <form onSubmit={handleProfileSubmit} className="mt-8 space-y-8">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 p-4">
          <label className="flex flex-col">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Nombre</p>
            <input
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary h-14 placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              type="text"
              name="name"
              value={profileData.name}
              onChange={handleInputChange}
              required
            />
          </label>

          <label className="flex flex-col">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Apellido</p>
            <input
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary h-14 placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              type="text"
              name="lastName"
              value={profileData.lastName}
              onChange={handleInputChange}
            />
          </label>

          <label className="flex flex-col">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Correo Electrónico</p>
            <div className="relative flex w-full flex-1 items-stretch">
              <input
                className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-muted focus:outline-0 focus:ring-0 border border-white/20 bg-background-dark h-14 placeholder:text-text-muted p-[15px] pr-12 text-base font-normal leading-normal font-display"
                type="email"
                value={profile?.email}
                readOnly
              />
              <div className="text-text-muted absolute right-0 top-0 h-full flex items-center justify-center pr-[15px]">
                <span className="material-symbols-outlined text-[24px]">lock</span>
              </div>
            </div>
          </label>

          <label className="flex flex-col">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Número de Teléfono</p>
            <input
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary h-14 placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              type="tel"
              name="phoneNumber"
              value={profileData.phoneNumber}
              onChange={handleInputChange}
              placeholder="+57 300 123 4567"
            />
          </label>

          <label className="flex flex-col md:col-span-2">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Dirección</p>
            <input
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary h-14 placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              type="text"
              name="address"
              value={profileData.address}
              onChange={handleInputChange}
              placeholder="Dirección completa para entregas"
            />
          </label>

          <label className="flex flex-col">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">DNI/Cédula</p>
            <input
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary h-14 placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              type="text"
              name="dni"
              value={profileData.dni}
              onChange={handleInputChange}
              placeholder="Número de identificación"
            />
          </label>

          <label className="flex flex-col md:col-span-2">
            <p className="text-text-light text-base font-medium leading-normal pb-2 font-display">Descripción para Delivery</p>
            <textarea
              className="form-input flex w-full min-w-0 flex-1 resize-none overflow-hidden rounded-lg text-text-light focus:outline-0 focus:ring-2 focus:ring-primary/50 border border-white/20 bg-background-dark focus:border-primary min-h-[100px] placeholder:text-text-muted p-[15px] text-base font-normal leading-normal font-display"
              name="deliveryDescription"
              value={profileData.deliveryDescription}
              onChange={handleInputChange}
              placeholder="Instrucciones especiales para el delivery (referencias, piso, etc.)"
              rows={3}
            />
          </label>
        </div>

        <div className="flex flex-col sm:flex-row gap-4 justify-end p-4 border-t border-white/10 pt-6">
          <button
            className="flex items-center justify-center rounded-lg h-10 px-4 bg-transparent text-text-light text-sm font-bold border border-white/20 hover:bg-white/10 transition-colors duration-200"
            type="button"
            onClick={() => window.location.reload()}
          >
            <span className="truncate">Cancelar</span>
          </button>
          <button
            className="flex items-center justify-center rounded-lg h-10 px-4 bg-primary text-white text-sm font-bold hover:bg-primary/90 transition-colors duration-200"
            type="submit"
            disabled={saving}
          >
            <span className="truncate">{saving ? 'Guardando...' : 'Guardar Cambios'}</span>
          </button>
        </div>
      </form>
    </div>
  );
};

export default Profile;
