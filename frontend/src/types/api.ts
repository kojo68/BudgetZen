export interface AuthResponse {
  accessToken: string;
  expiresAtUtc: string;
}

export interface AccountDto {
  id: string;
  name: string;
  initialBalance: number;
  createdAtUtc: string;
}

export interface CategoryDto {
  id: string;
  name: string;
  isIncome: boolean;
  createdAtUtc: string;
}

export type TransactionType = 'Expense' | 'Income';

export interface TransactionDto {
  id: string;
  accountId: string;
  categoryId: string;
  type: TransactionType;
  amount: number;
  description: string;
  occurredAtUtc: string;
  createdAtUtc: string;
}

export interface BudgetDto {
  id: string;
  categoryId: string;
  monthlyLimit: number;
  month: number;
  year: number;
  createdAtUtc: string;
}

export interface PagedResult<T> {
  page: number;
  pageSize: number;
  totalCount: number;
  items: T[];
}

export interface DashboardSummaryDto {
  totalIncome: number;
  totalExpense: number;
  net: number;
  byCategory: { categoryId: string; categoryName: string; total: number }[];
  byWeek: { week: number; total: number }[];
}
