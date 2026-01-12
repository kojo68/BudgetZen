import api from './client';
import type { PagedResult, TransactionDto, TransactionType } from '../types/api';

export interface TransactionPayload {
  accountId: string;
  categoryId: string;
  type: TransactionType;
  amount: number;
  description: string;
  occurredAtLocal: string;
}

export interface TransactionFilters {
  fromUtc?: string;
  toUtc?: string;
  type?: TransactionType;
  accountId?: string;
  categoryId?: string;
  q?: string;
  page?: number;
  pageSize?: number;
}

export const transactionsApi = {
  async getAll(filters: TransactionFilters) {
    const { data } = await api.get<PagedResult<TransactionDto>>('/transactions', { params: filters });
    return data;
  },
  async create(payload: TransactionPayload) {
    const { data } = await api.post<TransactionDto>('/transactions', payload);
    return data;
  },
  async update(id: string, payload: TransactionPayload) {
    const { data } = await api.put<TransactionDto>(`/transactions/${id}`, payload);
    return data;
  },
  async remove(id: string) {
    await api.delete(`/transactions/${id}`);
  }
};
