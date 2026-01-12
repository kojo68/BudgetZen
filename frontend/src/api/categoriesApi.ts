import api from './client';
import type { CategoryDto } from '../types/api';

export interface CategoryPayload {
  name: string;
  isIncome: boolean;
}

export const categoriesApi = {
  async getAll() {
    const { data } = await api.get<CategoryDto[]>('/categories');
    return data;
  },
  async create(payload: CategoryPayload) {
    const { data } = await api.post<CategoryDto>('/categories', payload);
    return data;
  },
  async update(id: string, payload: CategoryPayload) {
    const { data } = await api.put<CategoryDto>(`/categories/${id}`, payload);
    return data;
  },
  async remove(id: string) {
    await api.delete(`/categories/${id}`);
  }
};
