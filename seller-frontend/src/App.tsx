import { useEffect } from 'react';
import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/forms/AuthContext';
import SellerRoutes from './components/SellerRoutes';
import axios from 'axios';
import './App.css';

function App() {
  useEffect(() => {
    const token = localStorage.getItem('gunter_token');
    const role = localStorage.getItem('gunter_role');

    // Si no hay token o no es seller → lo echamos
    if (!token || role !== 'seller') {
      alert('Acceso denegado. Debes ser vendedor.');
      window.location.href = 'http://localhost:5173'; // vuelve al frontend normal
      return;
    }

    // Opcional: validar token con el backend
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;

    // Aquí ya está logueado como vendedor → muestra el dashboard
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
