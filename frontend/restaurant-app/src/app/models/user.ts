export interface AuthResponse {
  token: string;
  id: number;
  name: string;
  email: string;
  role: string;
  customerId?: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  role: string;
  phone?: string;
}
