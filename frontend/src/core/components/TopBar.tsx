import React from 'react';
import { AppBar, Toolbar, Typography, Button, Box, Snackbar, Alert } from '@mui/material';
import { useAuth } from '../../features/auth/contexts/AuthContext';

export default function TopBar() {
  const { isAuthenticated, user, logout } = useAuth();
  const [snackbarOpen, setSnackbarOpen] = React.useState(false);

  const handleLogout = async () => {
    await logout();
    setSnackbarOpen(true);
  };

  return (
    <>
      <AppBar position="static" color="primary">
        <Toolbar>
          <Typography variant="h6" sx={{ flexGrow: 1 }}>
            Gunter Bar
          </Typography>
          {isAuthenticated && (
            <>
              <Typography variant="body1" sx={{ mr: 2 }}>
                {user?.firstName || user?.email}
              </Typography>
              <Button color="inherit" onClick={handleLogout} aria-label="Cerrar sesión">
                Cerrar sesión
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
