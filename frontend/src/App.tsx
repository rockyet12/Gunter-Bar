import React, { useState, useEffect } from 'react';
import { Box, Typography, Link } from '@mui/material';
import { BrowserRouter as Router } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from './core/theme';
import { AuthProvider } from './features/auth/contexts/AuthContext';
import { AppRoutes } from './Routes';
import { routerConfig } from './core/router/config';
import { OrderProvider } from './features/orders/contexts/OrderContext';
import TopBar from './core/components/TopBar';
import Loader from './components/Loader';


function App() {
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Simula carga inicial y permite mostrar loader en conexiones lentas
    const timer = setTimeout(() => setLoading(false), 1200);
    return () => clearTimeout(timer);
  }, []);

    return (
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <Router future={routerConfig.future}>
          <AuthProvider>
            <OrderProvider>
              {loading && <Loader fullscreen />}
              {!loading && <>
                <TopBar />
                <AppRoutes />
              </>}
              <Footer />
            </OrderProvider>
          </AuthProvider>
        </Router>
      </ThemeProvider>
    );
// Footer visual y responsivo
function Footer() {
  return (
    <Box component="footer" sx={{ bgcolor: 'primary.main', color: 'white', py: 3, mt: 6, textAlign: 'center', fontFamily: 'Montserrat, Roboto, Arial, sans-serif' }}>
      <Typography variant="body1" sx={{ fontWeight: 500 }}>
        © {new Date().getFullYear()} Gunter Bar — Todos los derechos reservados
      </Typography>
      <Box sx={{ mt: 1 }}>
        <Link href="/" color="inherit" underline="hover" sx={{ mx: 2 }}>
          Inicio
        </Link>
        <Link href="/profile" color="inherit" underline="hover" sx={{ mx: 2 }}>
          Perfil
        </Link>
        <Link href="/orders" color="inherit" underline="hover" sx={{ mx: 2 }}>
          Pedidos
        </Link>
        <Link href="/admin-bebidas" color="inherit" underline="hover" sx={{ mx: 2 }}>
          Admin
        </Link>
      </Box>
    </Box>
  );
}
}

export default App;
