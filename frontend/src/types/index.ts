// Tipos principales para la aplicación

export interface User {
  id: number;
  fullName: string;
  email: string;
  role: UserRole;
  phoneNumber?: string;
  address?: string;
  birthDate?: string;
}

export enum UserRole {
  Customer = 1,
  Employee = 2,
  SalesManager = 3
}

export enum DrinkType {
  Alcoholic = 1,
  NonAlcoholic = 2,
  Cocktail = 3,
  Hot = 4
}

export enum OrderStatus {
  Pending = 1,
  Confirmed = 2,
  Prepared = 3,
  Delivered = 4,
  Cancelled = 5
}

export interface Drink {
  id: number;
  name: string;
  description: string;
  price: number;
  type: DrinkType;
  alcoholContent: number;
  volumeInMl: number;
  imageUrl?: string;
  stock: number;
  isAvailable: boolean;
  ingredients: DrinkIngredient[];
}

export interface DrinkIngredient {
  id: number;
  ingredientName: string;
  quantity: number;
  unit: string;
  isOptional: boolean;
  notes?: string;
}

export interface Cart {
  id: number;
  userId: number;
  items: CartItem[];
  total: number;
  totalItems: number;
  updatedAt: string;
}

export interface CartItem {
  id: number;
  drinkId: number;
  drink: Drink;
  quantity: number;
  unitPrice: number;
  subtotal: number;
  addedAt: string;
}

export interface Order {
  id: number;
  orderNumber: string;
  userId: number;
  user: User;
  status: OrderStatus;
  total: number;
  deliveryAddress: string;
  contactPhone: string;
  notes?: string;
  orderDate: string;
  estimatedDelivery?: string;
  deliveredAt?: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  drinkId: number;
  drink: Drink;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

// DTOs para formularios
export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterUserDto {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
  phoneNumber?: string;
  address?: string;
  birthDate?: string;
  role?: UserRole;
}

export interface AddToCartDto {
  drinkId: number;
  quantity: number;
}

export interface CreateOrderDto {
  deliveryAddress: string;
  contactPhone: string;
  notes?: string;
}

// Respuestas de la API
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data?: T;
  errors: string[];
  timestamp: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  user: User;
}

export interface PaginatedResponse<T> {
  success: boolean;
  message: string;
  data: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  errors: string[];
}
