import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/Home';
import Menu from './pages/Menu';
import Profile from './pages/Profile';
import Vendor from './pages/Vendor';
import { Login, Register, ProtectedRoute, PublicRoute, VendorRoute } from './components/forms';
import PublicLayout from './layout/PublicLayout';
import ProtectedLayout from './layout/ProtectedLayout';
import ProfileLayout from './layout/ProfileLayout';

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Public routes - anyone can access */}
      <Route path="/" element={<PublicLayout />}>
        <Route index element={<Home />} />
        <Route path="menu" element={<Menu />} />
      </Route>

      {/* Auth routes - redirect authenticated users */}
      <Route path="/login" element={
        <PublicRoute>
          <Login />
        </PublicRoute>
      } />
      <Route path="/register" element={
        <PublicRoute>
          <Register />
        </PublicRoute>
      } />

      {/* Protected routes - require authentication */}
      <Route path="/dashboard" element={
        <ProtectedRoute>
          <ProtectedLayout />
        </ProtectedRoute>
      }>
        {/* User-specific pages can go here */}
        <Route index element={<Home />} />
        <Route path="manage-products" element={<Menu />} />
      </Route>

      {/* Vendor routes - require vendor role */}
      <Route path="/vendor" element={
        <VendorRoute>
          <Vendor />
        </VendorRoute>
      } />

      {/* Profile routes - separate layout */}
      <Route path="/dashboard/profile" element={
        <ProtectedRoute>
          <ProfileLayout />
        </ProtectedRoute>
      }>
        <Route index element={<Profile />} />
      </Route>

      {/* Quick profile access */}
      <Route path="/profile" element={
        <ProtectedRoute>
          <ProtectedLayout />
        </ProtectedRoute>
      }>
        <Route index element={<Profile />} />
      </Route>

      {/* Admin routes - require admin role */}
      {/* <Route path="/admin" element={
        <ProtectedRoute requiredRole="Admin">
          <AdminLayout />
        </ProtectedRoute>
      }>
        <Route path="users" element={<UserManagement />} />
        <Route path="settings" element={<AdminSettings />} />
      </Route> */}
    </Routes>
  );
};

export default AppRoutes;
