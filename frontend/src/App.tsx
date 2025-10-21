import React from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import theme from './core/theme';
import { AuthProvider } from './features/auth/contexts/AuthContext';
import { AppRoutes } from './Routes';
import { routerConfig } from './core/router/config';
import { OrderProvider } from './features/orders/contexts/OrderContext';
import TopBar from './core/components/TopBar';

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router future={routerConfig.future}>
        <AuthProvider>
          <OrderProvider>
            <TopBar />
            <AppRoutes />
          </OrderProvider>
        </AuthProvider>
      </Router>
    </ThemeProvider>
  );
}

export default App;
