import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { TextField, Button, Box, Typography, Container, Paper } from '@mui/material';
import { useAuth } from '../contexts/AuthContext';

export const LoginForm = () => {
  const { login } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError('');
    setIsLoading(true);

    try {
      await login(email, password);
      navigate('/');
    } catch (err) {
      const errorMessage = err instanceof Error ? err.message : 'Email o contraseña inválidos';
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <Container component="main" maxWidth="xs" sx={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: 'linear-gradient(120deg, #f5f7fa 0%, #c3cfe2 100%)' }}>
      <Paper elevation={6} sx={{ p: 4, borderRadius: 4, boxShadow: 6, width: '100%', maxWidth: 400, mx: 'auto', animation: 'fadeDown 1s' }}>
        <Box sx={{ textAlign: 'center', mb: 2 }}>
          <img src="/logo192.png" alt="Logo" style={{ width: 48, marginBottom: 8, opacity: 0.85 }} />
          <Typography component="h1" variant="h5" align="center" sx={{ fontWeight: 700, letterSpacing: 1 }}>
            Iniciar sesión
          </Typography>
        </Box>
        {error && (
          <Typography color="error" align="center" sx={{ mt: 2, fontWeight: 500 }}>
            {error}
          </Typography>
        )}
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 3 }}>
          <TextField
            margin="normal"
            required
            fullWidth
            id="email"
            label="Correo electrónico"
            name="email"
            autoComplete="email"
            inputProps={{ 'aria-label': 'Correo electrónico', 'aria-required': true }}
            autoFocus
            value={email}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)}
            sx={{ mb: 2 }}
          />
          <TextField
            margin="normal"
            required
            fullWidth
            name="password"
            label="Contraseña"
            type="password"
            id="password"
            autoComplete="current-password"
            inputProps={{ 'aria-label': 'Contraseña', 'aria-required': true }}
            value={password}
            onChange={(e: React.ChangeEvent<HTMLInputElement>) => setPassword(e.target.value)}
          />
          <Button
            type="submit"
            fullWidth
            variant="contained"
            color="primary"
            sx={{ mt: 3, mb: 2 }}
            aria-label="Iniciar sesión"
            disabled={isLoading}
          >
            {isLoading ? 'Ingresando...' : 'Iniciar sesión'}
          </Button>
          <Box sx={{ mt: 2, textAlign: 'center' }}>
            <Button
              onClick={() => navigate('/register')}
              sx={{ textTransform: 'none' }}
              color="secondary"
              aria-label="Ir a registrarse"
            >
              ¿No tienes cuenta? Regístrate
            </Button>
          </Box>
        </Box>
      </Paper>
    </Container>
  );
};
