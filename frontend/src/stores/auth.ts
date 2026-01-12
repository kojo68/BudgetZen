import { defineStore } from 'pinia';
import { authApi } from '../api/authApi';
import type { AuthResponse } from '../types/api';

interface AuthState {
  accessToken: string | null;
  expiresAtUtc: string | null;
  loading: boolean;
  error: string | null;
}

const STORAGE_KEY = 'budgetzen.auth';

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    accessToken: null,
    expiresAtUtc: null,
    loading: false,
    error: null
  }),
  actions: {
    hydrate() {
      const raw = sessionStorage.getItem(STORAGE_KEY);
      if (!raw) return;
      const parsed = JSON.parse(raw) as AuthResponse;
      this.accessToken = parsed.accessToken;
      this.expiresAtUtc = parsed.expiresAtUtc;
    },
    async login(email: string, password: string) {
      this.loading = true;
      this.error = null;
      try {
        const result = await authApi.login({ email, password });
        this.setAuth(result);
      } catch (error) {
        this.error = 'Identifiants invalides.';
        throw error;
      } finally {
        this.loading = false;
      }
    },
    async register(email: string, password: string) {
      this.loading = true;
      this.error = null;
      try {
        const result = await authApi.register({ email, password });
        this.setAuth(result);
      } catch (error) {
        this.error = 'Impossible de créer le compte.';
        throw error;
      } finally {
        this.loading = false;
      }
    },
    setAuth(result: AuthResponse) {
      this.accessToken = result.accessToken;
      this.expiresAtUtc = result.expiresAtUtc;
      sessionStorage.setItem(STORAGE_KEY, JSON.stringify(result));
    },
    logout() {
      this.accessToken = null;
      this.expiresAtUtc = null;
      sessionStorage.removeItem(STORAGE_KEY);
    }
  }
});
