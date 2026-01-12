import { defineStore } from 'pinia';
import { budgetsApi } from '../api/budgetsApi';
import type { BudgetDto } from '../types/api';

interface BudgetsState {
  items: BudgetDto[];
  loading: boolean;
  error: string | null;
}

export const useBudgetsStore = defineStore('budgets', {
  state: (): BudgetsState => ({
    items: [],
    loading: false,
    error: null
  }),
  actions: {
    async fetchAll() {
      this.loading = true;
      this.error = null;
      try {
        this.items = await budgetsApi.getAll();
      } catch (error) {
        this.error = 'Impossible de charger les budgets.';
      } finally {
        this.loading = false;
      }
    },
    async add(payload: { categoryId: string; monthlyLimit: number; month: number; year: number }) {
      const budget = await budgetsApi.create(payload);
      this.items.unshift(budget);
    },
    async update(id: string, monthlyLimit: number) {
      const budget = await budgetsApi.update(id, { monthlyLimit });
      const index = this.items.findIndex((item) => item.id === id);
      if (index >= 0) this.items[index] = budget;
    },
    async remove(id: string) {
      await budgetsApi.remove(id);
      this.items = this.items.filter((item) => item.id !== id);
    }
  }
});
