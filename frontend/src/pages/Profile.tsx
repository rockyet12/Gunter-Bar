import { useAuth } from '../features/auth/contexts/AuthContext';
import { useState, useRef } from 'react';
import { Box, Typography, Avatar, Button, TextField, Container, CircularProgress, Alert } from '@mui/material';
import apiService from '../services/apiService';
import { UserRole } from '../types';
import axios from 'axios';
import dayjs from 'dayjs';

export default function Profile() {
  const { user, isAuthenticated } = useAuth();
  const [image, setImage] = useState<File | null>(null);
  const [preview, setPreview] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [address, setAddress] = useState(user?.address || '');
  const [phoneNumber, setPhoneNumber] = useState(user?.phoneNumber || '');
  const [deliveryDescription, setDeliveryDescription] = useState(user?.deliveryDescription || '');
  const [birthDate, setBirthDate] = useState(user?.birthDate || '');
  const [saving, setSaving] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const fileInput = useRef<HTMLInputElement>(null);
  const [vendorLoading, setVendorLoading] = useState(false);

  if (!isAuthenticated || !user) return null;

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setImage(e.target.files[0]);
      setPreview(URL.createObjectURL(e.target.files[0]));
    }
  };

  const handleUpload = async () => {
    if (!image) return;
    setLoading(true);
    setError(null);
    try {
      const formData = new FormData();
      formData.append('file', image);
      await axios.post(`/api/users/${user.id}/profile-image`, formData, {
        headers: { 'Content-Type': 'multipart/form-data' },
        withCredentials: true,
      });
      setLoading(false);
      setSuccess(true);
      setTimeout(() => setSuccess(false), 2000);
      window.location.reload();
    } catch (e) {
      setError('Error al subir la foto.');
      setLoading(false);
    }
  };

  const isPhoneValid = phoneNumber.length === 0 || /^\+?\d{7,15}$/.test(phoneNumber);
  const isAddressValid = address.length > 3;
  const isAdult = birthDate && dayjs().diff(dayjs(birthDate), 'year') >= 18;

  const handleSave = async () => {
    if (!isAdult) {
      setError('Debes ser mayor de 18 años para comprar.');
      return;
    }
    setSaving(true);
    setError(null);
    try {
      await axios.put(`/api/users/${user.id}`, {
        address,
        phoneNumber,
        deliveryDescription,
        birthDate,
      }, { withCredentials: true });
      setSaving(false);
      setSuccess(true);
      setTimeout(() => setSuccess(false), 2000);
      window.location.reload();
    } catch (e) {
      setError('Error al guardar los datos.');
      setSaving(false);
    }
  };

  const handleRequestVendor = async () => {
    setVendorLoading(true);
    setError(null);
    try {
      await apiService.updateUserRole(user.id, UserRole.Employee);
      setSuccess(true);
      setTimeout(() => setSuccess(false), 2000);
      window.location.reload();
    } catch {
      setError('No se pudo solicitar el cambio de rol.');
    } finally {
      setVendorLoading(false);
    }
  };

  return (
    <Container maxWidth="sm" sx={{ py: 6 }}>
      <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 2 }}>
        {success && <Alert severity="success">¡Datos guardados correctamente!</Alert>}
        {error && <Alert severity="error">{error}</Alert>}
        <Avatar
          src={preview || user.profileImageUrl || ''}
          alt={user.name}
          sx={{ width: 120, height: 120, mb: 2 }}
        />
        <input
          type="file"
          accept="image/*"
          style={{ display: 'none' }}
          ref={fileInput}
          onChange={handleFileChange}
        />
        <Button variant="outlined" onClick={() => fileInput.current?.click()}>
          Seleccionar foto
        </Button>
        {image && (
          <Button variant="contained" color="primary" onClick={handleUpload} disabled={loading}>
            {loading ? <CircularProgress size={24} /> : 'Subir foto de perfil'}
          </Button>
        )}
        <Typography variant="h6" sx={{ mt: 4 }}>{user.name}</Typography>
        <Typography variant="body2" color="text.secondary">{user.email}</Typography>
        <TextField
          label="Dirección"
          value={address}
          onChange={e => setAddress(e.target.value)}
          fullWidth
          sx={{ mt: 3 }}
          error={!isAddressValid}
          helperText={!isAddressValid ? 'La dirección debe tener al menos 4 caracteres.' : ''}
        />
        <TextField
          label="Número de teléfono"
          value={phoneNumber}
          onChange={e => setPhoneNumber(e.target.value)}
          fullWidth
          error={!isPhoneValid}
          helperText={!isPhoneValid ? 'Número inválido. Usa solo dígitos y opcional +.' : ''}
        />
        <TextField
          label="Descripción para el delivery"
          value={deliveryDescription}
          onChange={e => setDeliveryDescription(e.target.value)}
          fullWidth
          multiline
          minRows={2}
        />
        <TextField
          label="Fecha de nacimiento"
          type="date"
          value={birthDate}
          onChange={e => setBirthDate(e.target.value)}
          fullWidth
          InputLabelProps={{ shrink: true }}
          error={!!birthDate && !isAdult}
          helperText={!!birthDate && !isAdult ? 'Debes ser mayor de 18 años para comprar.' : ''}
          sx={{ mt: 2 }}
        />
        <Button variant="contained" color="success" onClick={handleSave} disabled={saving || !isAddressValid || !isPhoneValid || !isAdult} sx={{ mt: 2 }}>
          {saving ? <CircularProgress size={24} /> : 'Guardar cambios'}
        </Button>
  {Number(user.role) === UserRole.Customer && (
          <Button variant="outlined" color="secondary" onClick={handleRequestVendor} disabled={vendorLoading} sx={{ mt: 2 }}>
            {vendorLoading ? <CircularProgress size={20} /> : 'Solicitar ser Vendedor'}
          </Button>
        )}
      </Box>
    </Container>
  );
}
