import React from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../forms/AuthContext';

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

  if (user.role !== 'Seller') {
    return <Navigate to="/home" replace />;
  }

  return <>{children}</>;
};

export default VendorRoute;