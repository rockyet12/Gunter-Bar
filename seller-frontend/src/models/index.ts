export interface User {
  id: number;
  name: string;
  lastName?: string;
  email: string;
  role: string;
  phoneNumber?: string;
  address?: string;
  profileImageUrl?: string;
  deliveryDescription?: string;
  birthDate?: string;
}

export interface CreateUserDto {
  name: string;
  lastName?: string;
  email: string;
  password: string;
  role: string;
  phoneNumber?: string;
  address?: string;
  profileImageUrl?: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface AuthResponseDto {
  token: string;
  refreshToken: string;
  user: User;
}

export interface AuthResponse {
  token: string;
  user: User;
}

export interface Drink {
  id: number;
  name: string;
  description: string;
  price: number;
  type: string;
  category?: string;
  imageUrl?: string;
  stock: number;
  ingredients?: string[];
  alcoholContent?: number;
  volume?: string;
  origin?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface Order {
  id: number;
  userId: number;
  user?: User;
  items: OrderItem[];
  total: number;
  status: 'pending' | 'confirmed' | 'preparing' | 'ready' | 'delivered' | 'cancelled';
  createdAt: string;
  updatedAt?: string;
  deliveryAddress?: string;
  notes?: string;
}

export interface OrderItem {
  id: number;
  drinkId: number;
  drink?: Drink;
  quantity: number;
  price: number;
  total: number;
}

export interface Review {
  id: number;
  userId: number;
  user?: User;
  drinkId: number;
  drink?: Drink;
  rating: number;
  comment?: string;
  createdAt: string;
}