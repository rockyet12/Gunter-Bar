import { BrowserRouter as Router } from 'react-router-dom';
import { AuthProvider } from './components/forms/AuthContext';
import SellerRoutes from './components/SellerRoutes';
import './App.css';

function App() {
  return (
    <AuthProvider>
      <Router>
        <SellerRoutes />
      </Router>
    </AuthProvider>
  );
}

export default App;
