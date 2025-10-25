import Home from './pages/Home';
import Orders from './pages/Orders';
import NewOrder from './pages/NewOrder';
import AdminBebidas from './pages/AdminBebidas';
import Profile from './pages/Profile';
import { ReactNode, useEffect } from 'react';
import { Routes, Route, Navigate, useNavigate, useLocation } from 'react-router-dom';
import { Box } from '@mui/material';
import { useAuth } from './features/auth/contexts/AuthContext';
import { LoginForm } from './features/auth/components/LoginForm';
import { RegisterForm } from './features/auth/components/RegisterForm';

interface PrivateRouteProps {
  children: ReactNode;
}

const PrivateRoute = ({ children }: PrivateRouteProps) => {
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  useEffect(() => {
    if (!isAuthenticated) {
      navigate('/login', { state: { from: location } });
    }
  }, [isAuthenticated, navigate, location]);

  return isAuthenticated ? <>{children}</> : null;
};

const HomePage = () => (
  <Box className="home">Home</Box>
);

export const AppRoutes = () => {
  const { isAuthenticated, user } = useAuth();
  // Normaliza el rol para evitar problemas de tipo
  const role = typeof user?.role === 'string' ? Number(user.role) : user?.role;

  return (
    <Routes>
      <Route
        path="/login"
        element={isAuthenticated ? <Navigate to="/" replace /> : <LoginForm />}
      />
      <Route
        path="/register"
        element={isAuthenticated ? <Navigate to="/" replace /> : <RegisterForm />}
      />
      <Route
        path="/"
        element={
          <PrivateRoute>
            <Home />
          </PrivateRoute>
        }
      />
      {/* Panel de usuario */}
      {role === 1 && (
        <Route
          path="/orders"
          element={
            <PrivateRoute>
              <Orders />
            </PrivateRoute>
          }
        />
      )}
      {role === 1 && (
        <Route
          path="/orders/new"
          element={
            <PrivateRoute>
              <NewOrder />
            </PrivateRoute>
          }
        />
      )}
          <Route
            path="/profile"
            element={
              <PrivateRoute>
                <Profile />
              </PrivateRoute>
            }
          />
    <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}
