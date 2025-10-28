export interface User {
  id: number;
  name: string;
  lastName?: string;
  email: string;
  role: 'Admin' | 'Employee' | 'SalesManager' | 'Client';
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
