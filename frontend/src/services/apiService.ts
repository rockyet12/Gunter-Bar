// ...existing code...
import axios, { AxiosInstance, AxiosResponse } from 'axios';
import { 
  ApiResponse, 
  AuthResponse, 
  LoginDto, 
  RegisterUserDto, 
  User,
  Drink,
  Cart,
  AddToCartDto,
  Order,
  CreateOrderDto,
  DrinkType
} from '../types';

class ApiService {
  async uploadProfileImage(userId: number, file: File): Promise<ApiResponse<User>> {
    const formData = new FormData();
    formData.append('file', file);
    const response: AxiosResponse<ApiResponse<User>> = await this.api.post(`/users/${userId}/profile-image`, formData, {
      headers: { 'Content-Type': 'multipart/form-data' }
    });
    return response.data;
  }

  async updateUser(userId: number, data: Partial<User>): Promise<ApiResponse<User>> {
    const response: AxiosResponse<ApiResponse<User>> = await this.api.put(`/users/${userId}`, data);
    return response.data;
  }
  async getAllUsers(): Promise<ApiResponse<User[]>> {
    const response: AxiosResponse<ApiResponse<User[]>> = await this.api.get('/users');
    return response.data;
  }

  async updateUserRole(userId: number, role: number): Promise<ApiResponse<User>> {
    const response: AxiosResponse<ApiResponse<User>> = await this.api.patch(`/users/${userId}/role`, role);
    return response.data;
  }
  private api: AxiosInstance;

  constructor() {
    this.api = axios.create({
      baseURL: process.env.REACT_APP_API_URL || 'https://localhost:7000/api',
      headers: {
        'Content-Type': 'application/json',
      },
    });

    // Interceptor para agregar token automáticamente
    this.api.interceptors.request.use(
      (config) => {
        const token = localStorage.getItem('authToken');
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => Promise.reject(error)
    );

    // Interceptor para manejar respuestas
    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response?.status === 401) {
          // Token expirado o inválido
          localStorage.removeItem('authToken');
          localStorage.removeItem('user');
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  // Métodos de autenticación
  async login(credentials: LoginDto): Promise<ApiResponse<AuthResponse>> {
    const response: AxiosResponse<ApiResponse<AuthResponse>> = await this.api.post('/auth/login', credentials);
    return response.data;
  }

  async register(userData: RegisterUserDto): Promise<ApiResponse<AuthResponse>> {
    const response: AxiosResponse<ApiResponse<AuthResponse>> = await this.api.post('/auth/register', userData);
    return response.data;
  }

  async getCurrentUser(): Promise<ApiResponse<User>> {
    const response: AxiosResponse<ApiResponse<User>> = await this.api.get('/auth/me');
    return response.data;
  }

  // Métodos de bebidas
  async getDrinks(): Promise<ApiResponse<Drink[]>> {
    const response: AxiosResponse<ApiResponse<Drink[]>> = await this.api.get('/drinks');
    return response.data;
  }

  async getDrinkById(id: number): Promise<ApiResponse<Drink>> {
    const response: AxiosResponse<ApiResponse<Drink>> = await this.api.get(`/drinks/${id}`);
    return response.data;
  }

  async getDrinksByType(type: DrinkType): Promise<ApiResponse<Drink[]>> {
    const response: AxiosResponse<ApiResponse<Drink[]>> = await this.api.get(`/drinks/type/${type}`);
    return response.data;
  }

  async searchDrinks(query: string): Promise<ApiResponse<Drink[]>> {
    const response: AxiosResponse<ApiResponse<Drink[]>> = await this.api.get(`/drinks/search?q=${encodeURIComponent(query)}`);
    return response.data;
  }

  // Métodos de carrito
  async getCart(): Promise<ApiResponse<Cart>> {
    const response: AxiosResponse<ApiResponse<Cart>> = await this.api.get('/cart');
    return response.data;
  }

  async addToCart(item: AddToCartDto): Promise<ApiResponse<Cart>> {
    const response: AxiosResponse<ApiResponse<Cart>> = await this.api.post('/cart/add', item);
    return response.data;
  }

  async updateCartItem(itemId: number, quantity: number): Promise<ApiResponse<Cart>> {
    const response: AxiosResponse<ApiResponse<Cart>> = await this.api.put(`/cart/items/${itemId}`, { quantity });
    return response.data;
  }

  async removeFromCart(itemId: number): Promise<ApiResponse<Cart>> {
    const response: AxiosResponse<ApiResponse<Cart>> = await this.api.delete(`/cart/items/${itemId}`);
    return response.data;
  }

  async clearCart(): Promise<ApiResponse<boolean>> {
    const response: AxiosResponse<ApiResponse<boolean>> = await this.api.delete('/cart/clear');
    return response.data;
  }

  // Métodos de órdenes
  async getOrders(): Promise<ApiResponse<Order[]>> {
    const response: AxiosResponse<ApiResponse<Order[]>> = await this.api.get('/orders');
    return response.data;
  }

  async getOrderById(id: number): Promise<ApiResponse<Order>> {
    const response: AxiosResponse<ApiResponse<Order>> = await this.api.get(`/orders/${id}`);
    return response.data;
  }

  async createOrder(orderData: CreateOrderDto): Promise<ApiResponse<Order>> {
    const response: AxiosResponse<ApiResponse<Order>> = await this.api.post('/orders', orderData);
    return response.data;
  }

  // Métodos utilitarios
  setAuthToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  removeAuthToken(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
  }

  getAuthToken(): string | null {
    return localStorage.getItem('authToken');
  }

  isAuthenticated(): boolean {
    return !!this.getAuthToken();
  }
}

export const apiService = new ApiService();
export default apiService;
