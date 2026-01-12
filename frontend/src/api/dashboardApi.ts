import api from './client';
import type { DashboardSummaryDto } from '../types/api';

export const dashboardApi = {
  async getSummary(year: number, month: number) {
    const { data } = await api.get<DashboardSummaryDto>('/dashboard/summary', {
      params: { year, month }
    });
    return data;
  }
};
