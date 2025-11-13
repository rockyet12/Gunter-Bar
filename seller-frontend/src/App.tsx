import { useEffect } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/forms/AuthContext';
import SellerRoutes from './components/SellerRoutes';
import './App.css';

function App() {
  useEffect(() => {
    const token = localStorage.getItem('gunter_token');

    // Si no hay token, redirigir al login del frontend principal
    if (!token) {
      window.location.href = 'http://localhost:5173/login';
      return;
    }

    // El check de rol se maneja en AuthContext
  }, []);

  return (
    <AuthProvider>
      <Router>
        <SellerRoutes />
      </Router>
    </AuthProvider>
  );
}

export default App;
