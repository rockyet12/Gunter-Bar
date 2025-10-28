import React from 'react'
import AppRoutes from './routes'
import { AuthProvider, useAuth } from './components/forms/AuthContext'
import { CartProvider } from './components/common/CartContext'
import './App.css'

function AppContent(): React.JSX.Element {
  const { loading } = useAuth();

  if (loading) {
    return (
      <div className="loading-screen">
        <div className="loading-spinner"></div>
        <p>Verificando sesión...</p>
      </div>
    );
  }

  return <AppRoutes />
}

function App(): React.JSX.Element {
  return (
    <AuthProvider>
      <CartProvider>
        <AppContent />
      </CartProvider>
    </AuthProvider>
  )
}

export default App
