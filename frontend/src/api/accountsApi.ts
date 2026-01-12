import api from './client';
import type { AccountDto } from '../types/api';

export interface AccountPayload {
  name: string;
  initialBalance: number;
}

export const accountsApi = {
  async getAll() {
    const { data } = await api.get<AccountDto[]>('/accounts');
    return data;
  },
  async create(payload: AccountPayload) {
    const { data } = await api.post<AccountDto>('/accounts', payload);
    return data;
  },
  async update(id: string, payload: AccountPayload) {
    const { data } = await api.put<AccountDto>(`/accounts/${id}`, payload);
    return data;
  },
  async remove(id: string) {
    await api.delete(`/accounts/${id}`);
  }
};
