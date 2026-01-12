import { defineStore } from 'pinia';
import { dashboardApi } from '../api/dashboardApi';
import type { DashboardSummaryDto } from '../types/api';

interface DashboardState {
  summary: DashboardSummaryDto | null;
  loading: boolean;
  error: string | null;
}

export const useDashboardStore = defineStore('dashboard', {
  state: (): DashboardState => ({
    summary: null,
    loading: false,
    error: null
  }),
  actions: {
    async fetchSummary(year: number, month: number) {
      this.loading = true;
      this.error = null;
      try {
        this.summary = await dashboardApi.getSummary(year, month);
      } catch (error) {
        this.error = 'Impossible de charger le tableau de bord.';
      } finally {
        this.loading = false;
      }
    }
  }
});
