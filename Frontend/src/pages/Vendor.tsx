import React from 'react';
import { useAuth } from '../components/forms/AuthContext';

const Vendor: React.FC = () => {
  const { user } = useAuth();

  // Aquí iría la lógica para gestionar productos del vendedor
  // Por ahora, solo un placeholder

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Panel de Vendedor</h1>
      <p className="mb-4">Bienvenido, {user?.name}. Aquí puedes gestionar tus productos.</p>
      
      {/* Placeholder para funcionalidades futuras */}
      <div className="bg-white p-6 rounded-lg shadow-md">
        <h2 className="text-xl font-semibold mb-4">Mis Productos</h2>
        <p className="text-gray-600">Funcionalidad para agregar, editar y eliminar productos próximamente.</p>
      </div>
    </div>
  );
};

export default Vendor;