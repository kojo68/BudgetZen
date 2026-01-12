import { defineStore } from 'pinia';
import { accountsApi } from '../api/accountsApi';
import type { AccountDto } from '../types/api';

interface AccountsState {
  items: AccountDto[];
  loading: boolean;
  error: string | null;
}

export const useAccountsStore = defineStore('accounts', {
  state: (): AccountsState => ({
    items: [],
    loading: false,
    error: null
  }),
  actions: {
    async fetchAll() {
      this.loading = true;
      this.error = null;
      try {
        this.items = await accountsApi.getAll();
      } catch (error) {
        this.error = 'Impossible de charger les comptes.';
      } finally {
        this.loading = false;
      }
    },
    async add(name: string, initialBalance: number) {
      const account = await accountsApi.create({ name, initialBalance });
      this.items.push(account);
    },
    async update(id: string, name: string, initialBalance: number) {
      const account = await accountsApi.update(id, { name, initialBalance });
      const index = this.items.findIndex((item) => item.id === id);
      if (index >= 0) this.items[index] = account;
    },
    async remove(id: string) {
      await accountsApi.remove(id);
      this.items = this.items.filter((item) => item.id !== id);
    }
  }
});
