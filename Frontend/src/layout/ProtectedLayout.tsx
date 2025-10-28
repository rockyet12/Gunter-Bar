import React from 'react';
import { Outlet } from 'react-router-dom';
import ProtectedRoute from '../components/forms/ProtectedRoute';
import MainLayout from './MainLayout';

const ProtectedLayout: React.FC = () => {
  return (
    <ProtectedRoute>
      <MainLayout>
        <Outlet />
      </MainLayout>
    </ProtectedRoute>
  );
};

export default ProtectedLayout;
