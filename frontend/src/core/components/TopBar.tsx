import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box, Snackbar, Alert, Avatar, Menu, MenuItem, IconButton, Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import LocalBarIcon from '@mui/icons-material/LocalBar';
import MenuIcon from '@mui/icons-material/Menu';
import { useAuth } from '../../features/auth/contexts/AuthContext';
import { useNavigate } from 'react-router-dom';

export default function TopBar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [snackbarOpen, setSnackbarOpen] = React.useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const [infoOpen, setInfoOpen] = React.useState(false);
  const navigate = useNavigate();
  const handleInfoOpen = () => setInfoOpen(true);
  const handleInfoClose = () => setInfoOpen(false);

  const handleLogout = async () => {
    await logout();
    setSnackbarOpen(true);
  };

  const handleProfileMenu = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleCloseMenu = () => setAnchorEl(null);
  const handleProfile = () => {
    handleCloseMenu();
    navigate('/profile');
  };

  return (
    <>
      <AppBar position="static" color="primary" elevation={3} sx={{ fontFamily: 'Montserrat, Roboto, Arial, sans-serif' }}>
        <Toolbar>
          <IconButton edge="start" color="inherit" aria-label="menu" onClick={handleInfoOpen} sx={{ mr: 2 }}>
            <MenuIcon fontSize="large" />
          </IconButton>
          <Box sx={{ display: 'flex', alignItems: 'center', flexGrow: 1, cursor: 'pointer' }} onClick={() => navigate('/') }>
            <LocalBarIcon sx={{ mr: 1, fontSize: 32, color: 'secondary.main' }} />
            <Typography variant="h5" sx={{ fontWeight: 700, letterSpacing: 1, fontFamily: 'Montserrat, Roboto, Arial, sans-serif', color: 'white' }}>
              Gunter Bar
            </Typography>
          </Box>
        </Toolbar>
      </AppBar>
      <Dialog open={infoOpen} onClose={handleInfoClose}>
        <DialogTitle>Menú</DialogTitle>
        <DialogContent>
          <Button fullWidth color="primary" variant="outlined" sx={{ mb: 2 }} onClick={() => { handleInfoClose(); navigate('/profile'); }}>
            Perfil
          </Button>
          <Button fullWidth color="primary" variant="outlined" sx={{ mb: 2 }} onClick={() => { handleInfoClose(); navigate('/orders'); }}>
            Pedidos
          </Button>
          <Button fullWidth color="primary" variant="outlined" sx={{ mb: 2 }} onClick={() => { handleInfoClose(); navigate('/vendedores'); }}>
            Vendedores
          </Button>
          <Typography variant="body2" color="text.secondary" sx={{ mt: 2 }}>
            Gunter Bar — Gestión de usuarios, pedidos y administración de bebidas.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleInfoClose} color="primary" variant="contained">Cerrar</Button>
        </DialogActions>
      </Dialog>
      <Snackbar
        open={snackbarOpen}
        autoHideDuration={3000}
        onClose={() => setSnackbarOpen(false)}
        anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
      >
        <Alert onClose={() => setSnackbarOpen(false)} severity="success" sx={{ width: '100%' }}>
          Sesión cerrada correctamente
        </Alert>
      </Snackbar>
    </>
  );
}
