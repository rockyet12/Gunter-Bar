import React, { useState, useEffect } from 'react';
import { useAuth } from '../components/forms/AuthContext';
import { apiService } from '../utils/api';
import { BarChart3, Package, ShoppingCart, Users, TrendingUp, DollarSign, Clock, AlertTriangle, Plus, Settings, LogOut } from 'lucide-react';
import type { Drink, Order } from '../models';

const SellerDashboard: React.FC = () => {
  const { user, logout } = useAuth();
  const [stats, setStats] = useState({
    totalProducts: 0,
    totalOrders: 0,
    totalRevenue: 0,
    activeCustomers: 0,
    pendingOrders: 0,
    lowStockItems: 0
  });
  const [recentOrders, setRecentOrders] = useState<Order[]>([]);
  const [topProducts, setTopProducts] = useState<Drink[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    setLoading(true);
    try {
      // Load products
      const productsResponse = await apiService.drinks.getAll();
      const products = productsResponse.data.success ? productsResponse.data.data || [] : [];

      // Load orders (assuming there's an endpoint for vendor orders)
      // For now, simulate with empty data
      const orders: Order[] = [];

      // Calculate stats
      const totalRevenue = orders.reduce((sum, order) => sum + order.total, 0);
      const activeCustomers = new Set(orders.map(order => order.userId)).size;
      const pendingOrders = orders.filter(order => order.status === 'pending').length;
      const lowStockItems = products.filter((product: Drink) => product.stock < 10).length;

      setStats({
        totalProducts: products.length,
        totalOrders: orders.length,
        totalRevenue,
        activeCustomers,
        pendingOrders,
        lowStockItems
      });

      setRecentOrders(orders.slice(0, 5));
      setTopProducts(products.slice(0, 5));
    } catch (error) {
      console.error('Error loading dashboard data:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white shadow-sm border-b">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-6">
          <div className="flex items-center justify-between">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">Panel de Vendedor</h1>
              <p className="text-gray-600 mt-1">Bienvenido de vuelta, {user?.name || 'Vendedor'}</p>
            </div>
            <div className="flex items-center space-x-4">
              <button className="p-2 text-gray-400 hover:text-gray-600">
                <Settings className="h-6 w-6" />
              </button>
              <button
                onClick={logout}
                className="flex items-center px-4 py-2 text-sm font-medium text-gray-700 bg-white border border-gray-300 rounded-md hover:bg-gray-50"
              >
                <LogOut className="h-4 w-4 mr-2" />
                Cerrar Sesión
              </button>
            </div>
          </div>
        </div>
      </div>

      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Stats Grid */}
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6 mb-8">
          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center">
              <div className="p-2 bg-blue-100 rounded-lg">
                <Package className="h-6 w-6 text-blue-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Productos Totales</p>
                <p className="text-2xl font-bold text-gray-900">{stats.totalProducts}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center">
              <div className="p-2 bg-green-100 rounded-lg">
                <ShoppingCart className="h-6 w-6 text-green-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Pedidos Totales</p>
                <p className="text-2xl font-bold text-gray-900">{stats.totalOrders}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center">
              <div className="p-2 bg-yellow-100 rounded-lg">
                <DollarSign className="h-6 w-6 text-yellow-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Ingresos Totales</p>
                <p className="text-2xl font-bold text-gray-900">${stats.totalRevenue.toFixed(2)}</p>
              </div>
            </div>
          </div>

          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center">
              <div className="p-2 bg-purple-100 rounded-lg">
                <Users className="h-6 w-6 text-purple-600" />
              </div>
              <div className="ml-4">
                <p className="text-sm font-medium text-gray-600">Clientes Activos</p>
                <p className="text-2xl font-bold text-gray-900">{stats.activeCustomers}</p>
              </div>
            </div>
          </div>
        </div>

        {/* Alerts */}
        {(stats.pendingOrders > 0 || stats.lowStockItems > 0) && (
          <div className="mb-8">
            <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4">
              <div className="flex items-center">
                <AlertTriangle className="h-5 w-5 text-yellow-600 mr-2" />
                <div>
                  {stats.pendingOrders > 0 && (
                    <p className="text-sm text-yellow-800">
                      Tienes {stats.pendingOrders} pedido{stats.pendingOrders !== 1 ? 's' : ''} pendiente{stats.pendingOrders !== 1 ? 's' : ''} de atención.
                    </p>
                  )}
                  {stats.lowStockItems > 0 && (
                    <p className="text-sm text-yellow-800">
                      {stats.lowStockItems} producto{stats.lowStockItems !== 1 ? 's' : ''} con stock bajo (menos de 10 unidades).
                    </p>
                  )}
                </div>
              </div>
            </div>
          </div>
        )}

        {/* Main Content Grid */}
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          {/* Recent Orders */}
          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold text-gray-900">Pedidos Recientes</h2>
              <Clock className="h-5 w-5 text-gray-400" />
            </div>
            {recentOrders.length === 0 ? (
              <p className="text-gray-500 text-center py-8">No hay pedidos recientes</p>
            ) : (
              <div className="space-y-4">
                {recentOrders.map((order) => (
                  <div key={order.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                    <div>
                      <p className="font-medium text-gray-900">Pedido #{order.id}</p>
                      <p className="text-sm text-gray-600">{order.user?.name || 'Cliente'}</p>
                    </div>
                    <div className="text-right">
                      <p className="font-medium text-gray-900">${order.total}</p>
                      <p className="text-sm text-gray-600 capitalize">{order.status}</p>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>

          {/* Top Products */}
          <div className="bg-white rounded-lg shadow-sm p-6">
            <div className="flex items-center justify-between mb-4">
              <h2 className="text-lg font-semibold text-gray-900">Productos Más Vendidos</h2>
              <TrendingUp className="h-5 w-5 text-gray-400" />
            </div>
            {topProducts.length === 0 ? (
              <p className="text-gray-500 text-center py-8">No hay productos para mostrar</p>
            ) : (
              <div className="space-y-4">
                {topProducts.map((product, index) => (
                  <div key={product.id} className="flex items-center justify-between p-3 bg-gray-50 rounded-lg">
                    <div className="flex items-center">
                      <div className="w-8 h-8 bg-blue-100 rounded-full flex items-center justify-center text-sm font-medium text-blue-600 mr-3">
                        {index + 1}
                      </div>
                      <div>
                        <p className="font-medium text-gray-900">{product.name}</p>
                        <p className="text-sm text-gray-600">${product.price}</p>
                      </div>
                    </div>
                    <div className="text-right">
                      <p className="text-sm text-gray-600">Stock: {product.stock}</p>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>

        {/* Quick Actions */}
        <div className="mt-8 bg-white rounded-lg shadow-sm p-6">
          <h2 className="text-lg font-semibold text-gray-900 mb-4">Acciones Rápidas</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <button className="flex items-center justify-center p-4 bg-blue-50 hover:bg-blue-100 rounded-lg transition-colors">
              <Plus className="h-6 w-6 text-blue-600 mr-2" />
              <span className="font-medium text-blue-900">Agregar Producto</span>
            </button>
            <button className="flex items-center justify-center p-4 bg-green-50 hover:bg-green-100 rounded-lg transition-colors">
              <ShoppingCart className="h-6 w-6 text-green-600 mr-2" />
              <span className="font-medium text-green-900">Ver Pedidos</span>
            </button>
            <button className="flex items-center justify-center p-4 bg-purple-50 hover:bg-purple-100 rounded-lg transition-colors">
              <BarChart3 className="h-6 w-6 text-purple-600 mr-2" />
              <span className="font-medium text-purple-900">Ver Reportes</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default SellerDashboard;