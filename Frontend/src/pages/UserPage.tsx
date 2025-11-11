import React, { useState, useEffect } from 'react';
import { useAuth } from '../components/forms/AuthContext';
import { apiService } from '../utils/api';
import { ShoppingCart, Package, History, Star, MessageSquare, Home } from 'lucide-react';

const UserPage: React.FC = () => {
  const { user } = useAuth();
  const [activeSection, setActiveSection] = useState('dashboard');
  const [orders, setOrders] = useState<any[]>([]);
  const [cart, setCart] = useState<any>(null);
  const [products, setProducts] = useState<any[]>([]);
  const [reviews, setReviews] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [searchTerm, setSearchTerm] = useState('');
  const [selectedCategory, setSelectedCategory] = useState('');

  useEffect(() => {
    if (activeSection === 'orders') {
      loadOrders();
    } else if (activeSection === 'cart') {
      loadCart();
    } else if (activeSection === 'products') {
      loadProducts();
    } else if (activeSection === 'reviews') {
      loadReviews();
    }
  }, [activeSection]);

  const loadProducts = async () => {
    setLoading(true);
    try {
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

  const loadReviews = async () => {
    setLoading(true);
    try {
      const response = await apiService.reviews.getMyReviews();
      if (response.data.success) {
        setReviews(response.data.data || []);
      }
    } catch (error) {
      console.error('Error loading reviews:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadOrders = async () => {
    setLoading(true);
    try {
      const response = await apiService.orders.getMyOrders();
      if (response.data.success) {
        setOrders(response.data.data || []);
      }
    } catch (error) {
      console.error('Error loading orders:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadCart = async () => {
    setLoading(true);
    try {
      const response = await apiService.cart.getMyCart();
      if (response.data.success) {
        setCart(response.data.data);
      }
    } catch (error) {
      console.error('Error loading cart:', error);
    } finally {
      setLoading(false);
    }
  };

  const checkout = async () => {
    try {
      await apiService.orders.create({}); // Assuming cart items are used
      alert('Pedido realizado exitosamente');
      loadCart();
      loadOrders();
    } catch (error) {
      console.error('Error during checkout:', error);
      alert('Error al realizar el pedido');
    }
  };

  const addToCart = async (productId: number) => {
    try {
      await apiService.cart.addItem({ drinkId: productId, quantity: 1 });
      loadCart();
      alert('Producto agregado al carrito');
    } catch (error) {
      console.error('Error adding to cart:', error);
      alert('Error al agregar al carrito');
    }
  };

  const removeFromCart = async (itemId: number) => {
    try {
      await apiService.cart.removeItem(itemId);
      loadCart();
    } catch (error) {
      console.error('Error removing from cart:', error);
      alert('Error al remover del carrito');
    }
  };

  const menuItems = [
    { id: 'dashboard', label: 'Dashboard', icon: Home },
    { id: 'products', label: 'Productos', icon: Package },
    { id: 'cart', label: 'Carrito', icon: ShoppingCart },
    { id: 'orders', label: 'Mis Pedidos', icon: History },
    { id: 'reviews', label: 'Mis Reseñas', icon: Star },
    { id: 'messages', label: 'Mensajes', icon: MessageSquare },
  ];

  const renderContent = () => {
    switch (activeSection) {
      case 'dashboard':
        return (
          <div className="space-y-6">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="bg-blue-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-blue-800">Pedidos Activos</h3>
                <p className="text-3xl font-bold text-blue-600">{orders.filter(o => o.status !== 'Delivered').length}</p>
              </div>
              <div className="bg-green-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-green-800">Productos en Carrito</h3>
                <p className="text-3xl font-bold text-green-600">{cart?.items?.length || 0}</p>
              </div>
              <div className="bg-purple-50 p-6 rounded-lg">
                <h3 className="text-lg font-semibold text-purple-800">Total Pedidos</h3>
                <p className="text-3xl font-bold text-purple-600">{orders.length}</p>
              </div>
            </div>
            <div className="bg-white p-6 rounded-lg shadow-md">
              <h2 className="text-xl font-semibold mb-4">Bienvenido, {user?.name}</h2>
              <p className="text-gray-600">Aquí puedes gestionar tus compras, ver tu historial y más.</p>
            </div>
          </div>
        );
      case 'products':
        return (
          <div className="space-y-6">
            <div className="flex justify-between items-center">
              <h2 className="text-2xl font-bold">Productos Disponibles</h2>
              <div className="flex gap-2">
                <input
                  type="text"
                  placeholder="Buscar productos..."
                  value={searchTerm}
                  onChange={(e) => setSearchTerm(e.target.value)}
                  className="px-4 py-2 border rounded-lg"
                />
                <select 
                  value={selectedCategory}
                  onChange={(e) => setSelectedCategory(e.target.value)}
                  className="px-4 py-2 border rounded-lg"
                >
                  <option value="">Todas las categorías</option>
                  {/* Add unique categories from products */}
                  {[...new Set(products.map(p => p.category).filter(Boolean))].map(cat => (
                    <option key={cat} value={cat}>{cat}</option>
                  ))}
                </select>
              </div>
            </div>
            {loading ? (
              <p>Cargando productos...</p>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                {products
                  .filter(product => 
                    product.name.toLowerCase().includes(searchTerm.toLowerCase()) &&
                    (selectedCategory === '' || product.category === selectedCategory)
                  )
                  .map((product) => (
                  <div key={product.id} className="bg-white p-4 rounded-lg shadow-md">
                    <img src={product.imageUrl || '/placeholder.jpg'} alt={product.name} className="w-full h-48 object-cover rounded-lg mb-4" />
                    <h3 className="text-lg font-semibold">{product.name}</h3>
                    <p className="text-gray-600">{product.description}</p>
                    <p className="text-xl font-bold text-green-600">${product.price}</p>
                    <button 
                      onClick={() => addToCart(product.id)}
                      className="mt-2 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                    >
                      Agregar al Carrito
                    </button>
                  </div>
                ))}
              </div>
            )}
          </div>
        );
      case 'cart':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-bold">Mi Carrito</h2>
            {loading ? (
              <p>Cargando carrito...</p>
            ) : cart?.items?.length > 0 ? (
              <div className="space-y-4">
                {cart.items.map((item: any) => (
                  <div key={item.id} className="bg-white p-4 rounded-lg shadow-md flex justify-between items-center">
                    <div>
                      <h3 className="text-lg font-semibold">{item.drink.name}</h3>
                      <p>Cantidad: {item.quantity}</p>
                      <p>Precio: ${item.drink.price * item.quantity}</p>
                    </div>
                    <button 
                      onClick={() => removeFromCart(item.id)}
                      className="bg-red-500 text-white px-4 py-2 rounded hover:bg-red-600"
                    >
                      Remover
                    </button>
                  </div>
                ))}
                <div className="bg-gray-100 p-4 rounded-lg">
                  <p className="text-xl font-bold">Total: ${cart.items.reduce((sum: number, item: any) => sum + item.drink.price * item.quantity, 0)}</p>
                  <button 
                    onClick={checkout}
                    className="mt-2 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600"
                  >
                    Proceder al Pago
                  </button>
                </div>
              </div>
            ) : (
              <p>Tu carrito está vacío.</p>
            )}
          </div>
        );
      case 'orders':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-bold">Mis Pedidos</h2>
            {loading ? (
              <p>Cargando pedidos...</p>
            ) : orders.length > 0 ? (
              <div className="space-y-4">
                {orders.map((order) => (
                  <div key={order.id} className="bg-white p-4 rounded-lg shadow-md">
                    <div className="flex justify-between items-center mb-4">
                      <h3 className="text-lg font-semibold">Pedido #{order.id}</h3>
                      <span className={`px-2 py-1 rounded text-sm ${
                        order.status === 'Pending' ? 'bg-yellow-100 text-yellow-800' :
                        order.status === 'Processing' ? 'bg-blue-100 text-blue-800' :
                        order.status === 'Delivered' ? 'bg-green-100 text-green-800' :
                        'bg-red-100 text-red-800'
                      }`}>
                        {order.status}
                      </span>
                    </div>
                    <p>Fecha: {new Date(order.createdAt).toLocaleDateString()}</p>
                    <p>Total: ${order.totalAmount}</p>
                    <div className="mt-4">
                      <h4 className="font-semibold">Productos:</h4>
                      <ul className="list-disc list-inside">
                        {order.items?.map((item: any) => (
                          <li key={item.id}>{item.drink.name} x{item.quantity}</li>
                        ))}
                      </ul>
                    </div>
                  </div>
                ))}
              </div>
            ) : (
              <p>No tienes pedidos aún.</p>
            )}
          </div>
        );
      case 'reviews':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-bold">Mis Reseñas</h2>
            {loading ? (
              <p>Cargando reseñas...</p>
            ) : reviews.length > 0 ? (
              <div className="space-y-4">
                {reviews.map((review) => (
                  <div key={review.id} className="bg-white p-4 rounded-lg shadow-md">
                    <div className="flex justify-between items-center mb-2">
                      <h3 className="text-lg font-semibold">{review.drinkName}</h3>
                      <div className="flex items-center">
                        {[...Array(5)].map((_, i) => (
                          <span key={i} className={i < review.rating ? 'text-yellow-500' : 'text-gray-300'}>
                            ★
                          </span>
                        ))}
                      </div>
                    </div>
                    <p className="text-gray-600">{review.comment}</p>
                    <p className="text-sm text-gray-500 mt-2">
                      {new Date(review.createdAt).toLocaleDateString()}
                    </p>
                  </div>
                ))}
              </div>
            ) : (
              <p>No has hecho reseñas aún.</p>
            )}
          </div>
        );
      case 'messages':
        return (
          <div className="space-y-6">
            <h2 className="text-2xl font-bold">Mensajes</h2>
            <p className="text-gray-600">Funcionalidad de mensajes próximamente.</p>
          </div>
        );
      default:
        return <div>Selecciona una sección del menú.</div>;
    }
  };

  return (
    <div className="min-h-screen bg-gray-100">
      <div className="flex">
        {/* Sidebar */}
        <div className="w-64 bg-white shadow-md min-h-screen">
          <div className="p-6">
            <h1 className="text-2xl font-bold text-gray-800">Panel de Usuario</h1>
            <p className="text-gray-600 mt-2">{user?.name}</p>
          </div>
          <nav className="mt-6">
            {menuItems.map((item) => (
              <button
                key={item.id}
                onClick={() => setActiveSection(item.id)}
                className={`w-full flex items-center px-6 py-3 text-left hover:bg-gray-100 ${
                  activeSection === item.id ? 'bg-blue-50 border-r-4 border-blue-500' : ''
                }`}
              >
                <item.icon className="w-5 h-5 mr-3" />
                {item.label}
              </button>
            ))}
          </nav>
        </div>

        {/* Main Content */}
        <div className="flex-1 p-8">
          {renderContent()}
        </div>
      </div>
    </div>
  );
};

export default UserPage;