import { createRouter, createWebHistory } from 'vue-router';
import { useAuthStore } from '../stores/auth';
import LoginView from '../features/auth/LoginView.vue';
import RegisterView from '../features/auth/RegisterView.vue';
import DashboardView from '../features/dashboard/DashboardView.vue';
import AccountsView from '../features/accounts/AccountsView.vue';
import CategoriesView from '../features/categories/CategoriesView.vue';
import TransactionsView from '../features/transactions/TransactionsView.vue';
import BudgetsView from '../features/budgets/BudgetsView.vue';

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/dashboard' },
    { path: '/login', component: LoginView, meta: { public: true } },
    { path: '/register', component: RegisterView, meta: { public: true } },
    { path: '/dashboard', component: DashboardView },
    { path: '/accounts', component: AccountsView },
    { path: '/categories', component: CategoriesView },
    { path: '/transactions', component: TransactionsView },
    { path: '/budgets', component: BudgetsView }
  ]
});

router.beforeEach((to) => {
  const authStore = useAuthStore();
  if (!authStore.accessToken) {
    authStore.hydrate();
  }
  const isPublic = to.meta.public;
  if (!isPublic && !authStore.accessToken) {
    return '/login';
  }
  return true;
});

export default router;
