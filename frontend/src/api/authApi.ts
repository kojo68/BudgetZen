import api from './client';
import type { AuthResponse } from '../types/api';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export const authApi = {
  async login(payload: LoginRequest) {
    const { data } = await api.post<AuthResponse>('/auth/login', payload);
    return data;
  },
  async register(payload: RegisterRequest) {
    const { data } = await api.post<AuthResponse>('/auth/register', payload);
    return data;
  }
};
