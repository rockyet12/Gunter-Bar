import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { apiService } from '../utils/api';
import { MapPin, Phone, Mail, Clock } from 'lucide-react';

const VendorProfile: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [vendor, setVendor] = useState<any>(null);
  const [products, setProducts] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    if (id) {
      loadVendorProfile();
      loadVendorProducts();
    }
  }, [id]);

  const loadVendorProfile = async () => {
    try {
      const response = await apiService.users.getVendorProfile(parseInt(id!));
      if (response.data.success) {
        setVendor(response.data.data);
      }
    } catch (error) {
      console.error('Error loading vendor profile:', error);
    }
  };

  const loadVendorProducts = async () => {
    try {
      // For now, load all products. In future, filter by vendor
      const response = await apiService.drinks.getAll();
      if (response.data.success) {
        setProducts(response.data.data || []);
      }
    } catch (error) {
      console.error('Error loading products:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading || !vendor) {
    return (
      <div className="min-h-screen bg-gray-100 flex items-center justify-center">
        <p>Cargando perfil del vendedor...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-100">
      {/* Header with bar image */}
      <div className="relative h-64 bg-gradient-to-r from-blue-500 to-purple-600">
        {vendor.bar?.imageUrl && (
          <img
            src={vendor.bar.imageUrl}
            alt={vendor.bar.name}
            className="w-full h-full object-cover"
          />
        )}
        <div className="absolute inset-0 bg-black bg-opacity-50 flex items-center justify-center">
          <div className="text-center text-white">
            <h1 className="text-4xl font-bold mb-2">{vendor.bar?.name || `${vendor.name}'s Bar`}</h1>
            <p className="text-xl">{vendor.bar?.description}</p>
          </div>
        </div>
      </div>

      <div className="max-w-6xl mx-auto px-4 py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Bar Information */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-lg shadow-md p-6">
              <h2 className="text-2xl font-bold mb-4">Información del Bar</h2>

              {vendor.bar?.address && (
                <div className="flex items-center mb-3">
                  <MapPin className="w-5 h-5 text-gray-500 mr-2" />
                  <span>{vendor.bar.address}</span>
                </div>
              )}

              {vendor.bar?.phoneNumber && (
                <div className="flex items-center mb-3">
                  <Phone className="w-5 h-5 text-gray-500 mr-2" />
                  <span>{vendor.bar.phoneNumber}</span>
                </div>
              )}

              {vendor.bar?.email && (
                <div className="flex items-center mb-3">
                  <Mail className="w-5 h-5 text-gray-500 mr-2" />
                  <span>{vendor.bar.email}</span>
                </div>
              )}

              {vendor.bar?.openingHours && (
                <div className="flex items-start mb-3">
                  <Clock className="w-5 h-5 text-gray-500 mr-2 mt-1" />
                  <div>
                    <p className="font-semibold">Horarios de apertura:</p>
                    <p>{vendor.bar.openingHours}</p>
                  </div>
                </div>
              )}

              {/* Vendor info */}
              <div className="mt-6 pt-6 border-t">
                <h3 className="text-lg font-semibold mb-2">Propietario</h3>
                <p className="text-gray-600">{vendor.name} {vendor.lastName}</p>
                {vendor.phoneNumber && <p className="text-gray-600">{vendor.phoneNumber}</p>}
              </div>
            </div>
          </div>

          {/* Products */}
          <div className="lg:col-span-2">
            <div className="bg-white rounded-lg shadow-md p-6">
              <h2 className="text-2xl font-bold mb-4">Nuestros Productos</h2>
              {products.length > 0 ? (
                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {products.slice(0, 6).map((product: any) => (
                    <div key={product.id} className="border rounded-lg p-4 hover:shadow-md transition-shadow">
                      <img
                        src={product.imageUrl || '/placeholder.jpg'}
                        alt={product.name}
                        className="w-full h-32 object-cover rounded-lg mb-3"
                      />
                      <h3 className="font-semibold">{product.name}</h3>
                      <p className="text-gray-600 text-sm mb-2">{product.description}</p>
                      <p className="text-lg font-bold text-green-600">${product.price}</p>
                    </div>
                  ))}
                </div>
              ) : (
                <p className="text-gray-600">No hay productos disponibles.</p>
              )}
            </div>

            {/* Reviews section could go here */}
            <div className="bg-white rounded-lg shadow-md p-6 mt-6">
              <h2 className="text-2xl font-bold mb-4">Reseñas</h2>
              <p className="text-gray-600">Funcionalidad de reseñas próximamente.</p>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default VendorProfile;