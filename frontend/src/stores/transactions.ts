import { defineStore } from 'pinia';
import { transactionsApi } from '../api/transactionsApi';
import type { PagedResult, TransactionDto, TransactionType } from '../types/api';

interface TransactionsState {
  items: TransactionDto[];
  page: number;
  pageSize: number;
  totalCount: number;
  loading: boolean;
  error: string | null;
  filters: {
    fromUtc?: string;
    toUtc?: string;
    type?: TransactionType;
    accountId?: string;
    categoryId?: string;
    q?: string;
  };
}

export const useTransactionsStore = defineStore('transactions', {
  state: (): TransactionsState => ({
    items: [],
    page: 1,
    pageSize: 10,
    totalCount: 0,
    loading: false,
    error: null,
    filters: {}
  }),
  actions: {
    async fetchAll() {
      this.loading = true;
      this.error = null;
      try {
        const result: PagedResult<TransactionDto> = await transactionsApi.getAll({
          ...this.filters,
          page: this.page,
          pageSize: this.pageSize
        });
        this.items = result.items;
        this.totalCount = result.totalCount;
      } catch (error) {
        this.error = 'Impossible de charger les transactions.';
      } finally {
        this.loading = false;
      }
    },
    async add(payload: {
      accountId: string;
      categoryId: string;
      type: TransactionType;
      amount: number;
      description: string;
      occurredAtLocal: string;
    }) {
      const transaction = await transactionsApi.create(payload);
      this.items.unshift(transaction);
      this.totalCount += 1;
    },
    async update(id: string, payload: {
      accountId: string;
      categoryId: string;
      type: TransactionType;
      amount: number;
      description: string;
      occurredAtLocal: string;
    }) {
      const transaction = await transactionsApi.update(id, payload);
      const index = this.items.findIndex((item) => item.id === id);
      if (index >= 0) this.items[index] = transaction;
    },
    async remove(id: string) {
      await transactionsApi.remove(id);
      this.items = this.items.filter((item) => item.id !== id);
      this.totalCount = Math.max(0, this.totalCount - 1);
    },
    setFilters(filters: TransactionsState['filters']) {
      this.filters = filters;
      this.page = 1;
    }
  }
});
