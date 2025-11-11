import React, { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import { useAuth } from '../components/forms/AuthContext';
import { apiService } from '../utils/api';
import { Plus, MessageSquare, Package, BarChart3, Users, Home, Edit, Eye } from 'lucide-react';

const schema = yup.object({
  name: yup.string().required('El nombre es requerido').max(100, 'Máximo 100 caracteres'),
  description: yup.string().max(500, 'Máximo 500 caracteres').optional(),
  price: yup.number().required('El precio es requerido').positive('Debe ser positivo'),
  type: yup.string().required('El tipo es requerido'),
  imageUrl: yup.string().url('URL inválida').optional(),
  stock: yup.number().required('El stock es requerido').min(0, 'No puede ser negativo'),
  category: yup.string().max(50, 'Máximo 50 caracteres').optional(),
});

const VendorPage: React.FC = () => {
  const { user } = useAuth();
  const [activeSection, setActiveSection] = useState('products'); // Cambiar a 'products' por defecto
  const [isLoading, setIsLoading] = useState(false);
  const [message, setMessage] = useState<{ text: string; type: 'success' | 'error' } | null>(null);
  const [products, setProducts] = useState<any[]>([]);
  const [loadingProducts, setLoadingProducts] = useState(false);

  const { register, handleSubmit, formState: { errors }, reset } = useForm({
    resolver: yupResolver(schema),
  });

  // Cargar productos cuando se selecciona la sección de productos
  useEffect(() => {
    if (activeSection === 'products' && products.length === 0) {
      loadProducts();
    }
  }, [activeSection]);

  const loadProducts = async () => {
    setLoadingProducts(true);
    try {
      const response = await apiService.drinks.getAll();
      if (response.data.success) {
        setProducts(response.data.data || []);
      }
    } catch (error) {
      console.error('Error loading products:', error);
    } finally {
      setLoadingProducts(false);
    }
  };

  const onSubmit = async (data: any) => {
    setIsLoading(true);
    setMessage(null);

    try {
      const response = await apiService.drinks.create({
        ...data,
        ingredients: [], // Simplificar, sin ingredients por ahora
      });

      if (response.data.success) {
        setMessage({ text: 'Producto agregado exitosamente', type: 'success' });
        reset();
        // Recargar productos si estamos en la sección de productos
        if (activeSection === 'products') {
          loadProducts();
        }
      } else {
        setMessage({ text: response.data.message || 'Error al agregar producto', type: 'error' });
      }
    } catch (error: any) {
      console.error('Error creating drink:', error);
      setMessage({ text: error.response?.data?.message || 'Error al agregar producto', type: 'error' });
    } finally {
      setIsLoading(false);
    }
  };

  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: Home },
    { id: 'products', label: 'Mis Productos', icon: Package },
    { id: 'add-product', label: 'Agregar Producto', icon: Plus },
    { id: 'sales', label: 'Ventas Semanales', icon: BarChart3 },
    { id: 'clients', label: 'Clientes Frecuentes', icon: Users },
    { id: 'queries', label: 'Consultas Clientes', icon: MessageSquare },
  ];

  const renderContent = () => {
    switch (activeSection) {
      case 'dashboard':
        return (
          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="bg-blue-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-blue-800">Productos Activos</h3>
                <p className="text-3xl font-bold text-blue-600">{products.length}</p>
              </div>
              <div className="bg-green-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-green-800">Ventas Esta Semana</h3>
                <p className="text-3xl font-bold text-green-600">0</p>
              </div>
              <div className="bg-purple-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-purple-800">Clientes Frecuentes</h3>
                <p className="text-3xl font-bold text-purple-600">0</p>
              </div>
            </div>
            <div className="bg-white p-6 rounded-lg shadow-md">
              <h2 className="text-xl font-semibold mb-4">Actividad Reciente</h2>
              <p className="text-gray-600">No hay actividad reciente para mostrar.</p>
            </div>
          </div>
        );

      case 'products':
        return (
          <div className="space-y-6">
            {/* Header de la sección */}
            <div className="flex justify-between items-center">
              <div>
                <h2 className="text-3xl font-bold text-gray-900">Mis productos</h2>
                <p className="text-gray-600 mt-1">{products.length} productos publicados</p>
              </div>
              <button
                onClick={() => setActiveSection('add-product')}
                className="bg-yellow-400 hover:bg-yellow-500 text-black font-semibold py-3 px-6 rounded-lg flex items-center gap-2 transition-colors shadow-md"
              >
                <Plus size={20} />
                Publicar producto
              </button>
            </div>

            {/* Mensaje si no hay productos */}
            {loadingProducts ? (
              <div className="text-center py-12">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                <p className="text-gray-500 mt-4">Cargando productos...</p>
              </div>
            ) : products.length === 0 ? (
              <div className="text-center py-12 bg-white rounded-lg shadow-sm">
                <Package size={64} className="mx-auto text-gray-400 mb-4" />
                <h3 className="text-xl font-semibold text-gray-900 mb-2">No tienes productos publicados</h3>
                <p className="text-gray-600 mb-6">Comienza a vender publicando tu primer producto</p>
                <button
                  onClick={() => setActiveSection('add-product')}
                  className="bg-yellow-400 hover:bg-yellow-500 text-black font-semibold py-3 px-8 rounded-lg inline-flex items-center gap-2 transition-colors"
                >
                  <Plus size={20} />
                  Publicar mi primer producto
                </button>
              </div>
            ) : (
              /* Grid de productos estilo Mercado Libre */
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-6">
                {products.map((product) => (
                  <div key={product.id} className="bg-white rounded-lg shadow-sm border hover:shadow-md transition-shadow">
                    {/* Imagen del producto */}
                    <div className="aspect-square bg-gray-100 rounded-t-lg overflow-hidden">
                      {product.imageUrl ? (
                        <img
                          src={product.imageUrl}
                          alt={product.name}
                          className="w-full h-full object-cover"
                          onError={(e) => {
                            e.currentTarget.src = 'https://via.placeholder.com/300x300?text=Sin+imagen';
                          }}
                        />
                      ) : (
                        <div className="w-full h-full flex items-center justify-center text-gray-400">
                          <Package size={48} />
                        </div>
                      )}
                    </div>

                    {/* Información del producto */}
                    <div className="p-4">
                      <h3 className="font-semibold text-lg text-gray-900 mb-1 line-clamp-2">{product.name}</h3>
                      <p className="text-gray-600 text-sm mb-2 line-clamp-2">{product.description}</p>

                      <div className="flex items-center justify-between mb-3">
                        <span className="text-2xl font-bold text-green-600">${product.price}</span>
                        <span className="text-sm text-gray-500 bg-gray-100 px-2 py-1 rounded">
                          Stock: {product.stock}
                        </span>
                      </div>

                      {/* Tipo y categoría */}
                      <div className="flex items-center justify-between text-sm text-gray-500 mb-4">
                        <span className="bg-blue-100 text-blue-800 px-2 py-1 rounded">
                          {product.type}
                        </span>
                        {product.category && (
                          <span className="bg-gray-100 text-gray-700 px-2 py-1 rounded">
                            {product.category}
                          </span>
                        )}
                      </div>

                      {/* Acciones */}
                      <div className="flex gap-2">
                        <button className="flex-1 bg-blue-600 text-white py-2 px-3 rounded text-sm hover:bg-blue-700 transition-colors flex items-center justify-center gap-1">
                          <Eye size={14} />
                          Ver
                        </button>
                        <button className="flex-1 bg-gray-600 text-white py-2 px-3 rounded text-sm hover:bg-gray-700 transition-colors flex items-center justify-center gap-1">
                          <Edit size={14} />
                          Editar
                        </button>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        );

      case 'add-product':
        return (
          <div className="max-w-4xl mx-auto">
            {/* Header del formulario */}
            <div className="bg-white rounded-lg shadow-sm p-6 mb-6">
              <div className="flex items-center gap-4 mb-4">
                <button
                  onClick={() => setActiveSection('products')}
                  className="text-blue-600 hover:text-blue-800 flex items-center gap-2"
                >
                  ← Volver a mis productos
                </button>
                <div className="h-6 border-l border-gray-300"></div>
                <h2 className="text-2xl font-bold text-gray-900">Publicar producto</h2>
              </div>
              <p className="text-gray-600">Completa los datos para publicar tu producto en GunterBar</p>
            </div>

            {/* Mensajes */}
            {message && (
              <div className={`mb-6 p-4 border rounded-lg ${
                message.type === 'success'
                  ? 'bg-green-50 border-green-200 text-green-800'
                  : 'bg-red-50 border-red-200 text-red-800'
              }`}>
                {message.text}
              </div>
            )}

            {/* Formulario */}
            <div className="bg-white rounded-lg shadow-sm p-8">
              <form onSubmit={handleSubmit(onSubmit)} className="space-y-8">
                {/* Información básica */}
                <div>
                  <h3 className="text-lg font-semibold text-gray-900 mb-4">Información del producto</h3>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div className="md:col-span-2">
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Título del producto *
                      </label>
                      <input
                        {...register('name')}
                        type="text"
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        placeholder="Ej: Mojito Clásico, Cerveza Artesanal IPA"
                      />
                      {errors.name && <p className="text-red-500 text-sm mt-1">{errors.name.message}</p>}
                    </div>

                    <div className="md:col-span-2">
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Descripción
                      </label>
                      <textarea
                        {...register('description')}
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        placeholder="Describe tu producto, ingredientes, características especiales..."
                        rows={4}
                      />
                      {errors.description && <p className="text-red-500 text-sm mt-1">{errors.description.message}</p>}
                    </div>

                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Tipo de bebida *
                      </label>
                      <select
                        {...register('type')}
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                      >
                        <option value="">Seleccionar tipo</option>
                        <option value="Beer">🍺 Cerveza</option>
                        <option value="Wine">🍷 Vino</option>
                        <option value="Cocktail">🍹 Cóctel</option>
                        <option value="SoftDrink">🥤 Refresco</option>
                        <option value="Spirit">🥃 Licor</option>
                      </select>
                      {errors.type && <p className="text-red-500 text-sm mt-1">{errors.type.message}</p>}
                    </div>

                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Categoría
                      </label>
                      <input
                        {...register('category')}
                        type="text"
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        placeholder="Ej: Nacional, Importado, Premium"
                      />
                      {errors.category && <p className="text-red-500 text-sm mt-1">{errors.category.message}</p>}
                    </div>
                  </div>
                </div>

                {/* Precio y stock */}
                <div>
                  <h3 className="text-lg font-semibold text-gray-900 mb-4">Precio y disponibilidad</h3>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Precio *
                      </label>
                      <div className="relative">
                        <span className="absolute left-3 top-3 text-gray-500">$</span>
                        <input
                          {...register('price')}
                          type="number"
                          step="0.01"
                          className="w-full pl-8 p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                          placeholder="0.00"
                        />
                      </div>
                      {errors.price && <p className="text-red-500 text-sm mt-1">{errors.price.message}</p>}
                    </div>

                    <div>
                      <label className="block text-sm font-medium text-gray-700 mb-2">
                        Stock disponible *
                      </label>
                      <input
                        {...register('stock')}
                        type="number"
                        className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        placeholder="Cantidad disponible"
                      />
                      {errors.stock && <p className="text-red-500 text-sm mt-1">{errors.stock.message}</p>}
                    </div>
                  </div>
                </div>

                {/* Imagen */}
                <div>
                  <h3 className="text-lg font-semibold text-gray-900 mb-4">Imagen del producto</h3>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-2">
                      URL de la imagen
                    </label>
                    <input
                      {...register('imageUrl')}
                      type="url"
                      className="w-full p-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                      placeholder="https://ejemplo.com/imagen.jpg"
                    />
                    {errors.imageUrl && <p className="text-red-500 text-sm mt-1">{errors.imageUrl.message}</p>}
                    <p className="text-sm text-gray-500 mt-1">
                      Recomendamos imágenes de al menos 800x800 píxeles para mejor calidad
                    </p>
                  </div>
                </div>

                {/* Botones de acción */}
                <div className="flex gap-4 pt-6 border-t">
                  <button
                    type="button"
                    onClick={() => setActiveSection('products')}
                    className="flex-1 bg-gray-200 text-gray-800 py-3 px-6 rounded-lg hover:bg-gray-300 transition-colors font-medium"
                  >
                    Cancelar
                  </button>
                  <button
                    type="submit"
                    disabled={isLoading}
                    className="flex-1 bg-yellow-400 text-black py-3 px-6 rounded-lg hover:bg-yellow-500 disabled:opacity-50 transition-colors font-semibold flex items-center justify-center gap-2"
                  >
                    {isLoading ? (
                      <>
                        <div className="animate-spin rounded-full h-5 w-5 border-b-2 border-black"></div>
                        Publicando...
                      </>
                    ) : (
                      <>
                        <Plus size={20} />
                        Publicar producto
                      </>
                    )}
                  </button>
                </div>
              </form>
            </div>
          </div>
        );

      case 'sales':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-semibold">Ventas de la Semana</h2>
            <div className="bg-white p-6 rounded-lg shadow-md">
              <p className="text-gray-600">Funcionalidad para ver estadísticas de ventas próximamente.</p>
              <div className="mt-4 space-y-4">
                <div className="flex justify-between items-center p-4 bg-gray-50 rounded-lg">
                  <div>
                    <h3 className="font-semibold">Total Ventas</h3>
                    <p className="text-2xl font-bold text-green-600">$0.00</p>
                  </div>
                  <BarChart3 size={32} className="text-gray-400" />
                </div>
                <div className="flex justify-between items-center p-4 bg-gray-50 rounded-lg">
                  <div>
                    <h3 className="font-semibold">Pedidos Completados</h3>
                    <p className="text-2xl font-bold text-blue-600">0</p>
                  </div>
                  <Package size={32} className="text-gray-400" />
                </div>
              </div>
            </div>
          </div>
        );

      case 'clients':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-semibold">Clientes Frecuentes</h2>
            <div className="bg-white p-6 rounded-lg shadow-md">
              <p className="text-gray-600">Lista de clientes frecuentes próximamente.</p>
              <div className="mt-4 space-y-2">
                <div className="p-4 bg-gray-50 rounded-lg">
                  <h3 className="font-semibold">No hay clientes frecuentes aún</h3>
                  <p className="text-sm text-gray-600">Los clientes frecuentes aparecerán aquí una vez que realicen pedidos.</p>
                </div>
              </div>
            </div>
          </div>
        );

      case 'queries':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-semibold">Consultas de Clientes</h2>
            <div className="bg-white p-6 rounded-lg shadow-md">
              <p className="text-gray-600 mb-4">Responde a las consultas de tus clientes.</p>
              <div className="space-y-4">
                <div className="p-4 bg-gray-50 rounded-lg">
                  <div className="flex justify-between items-start mb-2">
                    <h3 className="font-semibold">Cliente: Juan Pérez</h3>
                    <span className="text-sm text-gray-500">Hace 2 horas</span>
                  </div>
                  <p className="text-gray-700 mb-3">¿Tienen delivery disponible en mi zona?</p>
                  <div className="flex gap-2">
                    <button className="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600">
                      Responder
                    </button>
                    <button className="bg-gray-500 text-white px-3 py-1 rounded text-sm hover:bg-gray-600">
                      Marcar como resuelto
                    </button>
                  </div>
                </div>

                <div className="p-4 bg-gray-50 rounded-lg">
                  <div className="flex justify-between items-start mb-2">
                    <h3 className="font-semibold">Cliente: María García</h3>
                    <span className="text-sm text-gray-500">Hace 5 horas</span>
                  </div>
                  <p className="text-gray-700 mb-3">¿Puedo reservar una mesa para 4 personas?</p>
                  <div className="flex gap-2">
                    <button className="bg-blue-500 text-white px-3 py-1 rounded text-sm hover:bg-blue-600">
                      Responder
                    </button>
                    <button className="bg-gray-500 text-white px-3 py-1 rounded text-sm hover:bg-gray-600">
                      Marcar como resuelto
                    </button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        );

      default:
        return null;
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header estilo Mercado Libre */}
      <header className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center h-16">
            {/* Logo y título */}
            <div className="flex items-center">
              <h1 className="text-2xl font-bold text-blue-600">GunterBar</h1>
              <span className="ml-2 text-sm text-gray-500">Panel de Vendedor</span>
            </div>

            {/* Usuario y acciones */}
            <div className="flex items-center space-x-4">
              <span className="text-sm text-gray-700">Hola, {user?.name}</span>

              {/* Botón Agregar Producto - SIEMPRE VISIBLE */}
              <button
                onClick={() => setActiveSection('add-product')}
                className="bg-yellow-400 hover:bg-yellow-500 text-black font-semibold py-2 px-4 rounded-lg flex items-center gap-2 transition-colors"
              >
                <Plus size={20} />
                Vender
              </button>
            </div>
          </div>

          {/* Navegación horizontal */}
          <nav className="flex space-x-8 py-3 border-t">
            {menuItems.map((item) => {
              const Icon = item.icon;
              return (
                <button
                  key={item.id}
                  onClick={() => setActiveSection(item.id)}
                  className={`flex items-center gap-2 px-3 py-2 rounded-md text-sm font-medium transition-colors ${
                    activeSection === item.id
                      ? 'bg-blue-100 text-blue-700 border-b-2 border-blue-500'
                      : 'text-gray-700 hover:text-blue-600 hover:bg-gray-50'
                  }`}
                >
                  <Icon size={16} />
                  {item.label}
                </button>
              );
            })}
          </nav>
        </div>
      </header>

      {/* Contenido principal */}
      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {renderContent()}
      </main>
    </div>
  );
};

export default VendorPage;