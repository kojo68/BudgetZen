import api from './client';
import type { BudgetDto } from '../types/api';

export interface BudgetPayload {
  categoryId: string;
  monthlyLimit: number;
  month: number;
  year: number;
}

export const budgetsApi = {
  async getAll() {
    const { data } = await api.get<BudgetDto[]>('/budgets');
    return data;
  },
  async create(payload: BudgetPayload) {
    const { data } = await api.post<BudgetDto>('/budgets', payload);
    return data;
  },
  async update(id: string, payload: { monthlyLimit: number }) {
    const { data } = await api.put<BudgetDto>(`/budgets/${id}`, payload);
    return data;
  },
  async remove(id: string) {
    await api.delete(`/budgets/${id}`);
  }
};
