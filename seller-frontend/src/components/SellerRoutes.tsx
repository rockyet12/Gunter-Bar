import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import Login from '../components/forms/Login';
import SellerDashboard from '../pages/SellerDashboard';
import { useAuth } from '../components/forms/AuthContext';

const SellerRoutes: React.FC = () => {
  const { isAuthenticated, user, loading } = useAuth();

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <Routes>
      {/* Public routes */}
      <Route
        path="/login"
        element={
          isAuthenticated ? (
            user?.role === 'Seller' ? <Navigate to="/dashboard" replace /> : <Navigate to="/" replace />
          ) : (
            <Login />
          )
        }
      />

      {/* Protected routes - only for sellers */}
      <Route
        path="/dashboard"
        element={
          isAuthenticated && user?.role === 'Seller' ? (
            <SellerDashboard />
          ) : (
            <Navigate to="/login" replace />
          )
        }
      />

      {/* Redirect root to dashboard if authenticated as seller, otherwise to customer login */}
      <Route
        path="/"
        element={
          isAuthenticated ? (
            user?.role === 'Seller' ? <Navigate to="/dashboard" replace /> : (() => { window.location.href = 'http://localhost:5173'; return null; })()
          ) : (() => { window.location.href = 'http://localhost:5173/login'; return null; })()
        }
      />

      {/* Catch all - redirect to dashboard or login */}
      <Route
        path="*"
        element={
          isAuthenticated ? (
            user?.role === 'Seller' ? <Navigate to="/dashboard" replace /> : <Navigate to="/" replace />
          ) : (
            <Navigate to="/login" replace />
          )
        }
      />
    </Routes>
  );
};

export default SellerRoutes;