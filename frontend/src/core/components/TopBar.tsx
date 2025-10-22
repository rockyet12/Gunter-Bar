import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box, Snackbar, Alert, Avatar, Menu, MenuItem, IconButton } from '@mui/material';
import { useAuth } from '../../features/auth/contexts/AuthContext';
import { useNavigate } from 'react-router-dom';

export default function TopBar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [snackbarOpen, setSnackbarOpen] = React.useState(false);
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const navigate = useNavigate();

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
      <AppBar position="static" color="primary">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Gunter Bar
          </Typography>
          {isAuthenticated && user && (
            <>
              <Box sx={{ display: 'flex', alignItems: 'center', mr: 2 }}>
                <IconButton onClick={handleProfileMenu} color="inherit" size="small">
                  <Avatar src={user.profileImageUrl || ''} alt={user.name} sx={{ width: 32, height: 32, mr: 1 }} />
                  <Typography variant="body1" sx={{ ml: 1, fontWeight: 500 }}>
                    {user.name || user.email}
                  </Typography>
                </IconButton>
                <Menu
                  anchorEl={anchorEl}
                  open={Boolean(anchorEl)}
                  onClose={handleCloseMenu}
                  anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                  transformOrigin={{ vertical: 'top', horizontal: 'right' }}
                >
                  <MenuItem onClick={handleProfile}>Ver/Editar perfil</MenuItem>
                  <MenuItem onClick={handleLogout}>Cerrar sesión</MenuItem>
                </Menu>
              </Box>
              <Button color="inherit" href="/admin-bebidas" sx={{ mr: 2 }}>
                Admin Bebidas
              </Button>
            </>
          )}
        </Toolbar>
      </AppBar>
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
