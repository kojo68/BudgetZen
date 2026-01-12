import { defineStore } from 'pinia';

export type ToastType = 'success' | 'error' | 'info';

export interface ToastItem {
  id: string;
  message: string;
  type: ToastType;
}

interface ToastState {
  items: ToastItem[];
}

export const useToastStore = defineStore('toasts', {
  state: (): ToastState => ({
    items: []
  }),
  actions: {
    push(message: string, type: ToastType = 'info') {
      const id = crypto.randomUUID();
      this.items.push({ id, message, type });
      setTimeout(() => this.remove(id), 4000);
    },
    remove(id: string) {
      this.items = this.items.filter((item) => item.id !== id);
    }
  }
});
