import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from './AuthContext';

interface VendorRouteProps {
  children: React.ReactNode;
}

const VendorRoute: React.FC<VendorRouteProps> = ({ children }) => {
  const { user, loading } = useAuth();

  if (loading) {
    return <div>Cargando...</div>;
  }

  if (!user) {
    return <Navigate to="/login" replace />;
  }

  // Only allow users with 'Seller' role to access vendor pages
  if (user.role !== 'Seller') {
    return <Navigate to="/dashboard" replace />;
  }

  return <>{children}</>;
};

export default VendorRoute;