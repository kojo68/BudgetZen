import { defineStore } from 'pinia';
import { categoriesApi } from '../api/categoriesApi';
import type { CategoryDto } from '../types/api';

interface CategoriesState {
  items: CategoryDto[];
  loading: boolean;
  error: string | null;
}

export const useCategoriesStore = defineStore('categories', {
  state: (): CategoriesState => ({
    items: [],
    loading: false,
    error: null
  }),
  actions: {
    async fetchAll() {
      this.loading = true;
      this.error = null;
      try {
        this.items = await categoriesApi.getAll();
      } catch (error) {
        this.error = 'Impossible de charger les catégories.';
      } finally {
        this.loading = false;
      }
    },
    async add(name: string, isIncome: boolean) {
      const category = await categoriesApi.create({ name, isIncome });
      this.items.push(category);
    },
    async update(id: string, name: string, isIncome: boolean) {
      const category = await categoriesApi.update(id, { name, isIncome });
      const index = this.items.findIndex((item) => item.id === id);
      if (index >= 0) this.items[index] = category;
    },
    async remove(id: string) {
      await categoriesApi.remove(id);
      this.items = this.items.filter((item) => item.id !== id);
    }
  }
});
