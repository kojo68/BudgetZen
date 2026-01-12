<template>
  <div class="min-h-screen">
    <ToastList />
    <header class="border-b bg-white">
      <div class="mx-auto flex max-w-6xl items-center justify-between px-4 py-4">
        <h1 class="text-xl font-semibold text-slate-900">{{ t('app.title') }}</h1>
        <nav v-if="isAuthenticated" class="flex items-center gap-4 text-sm text-slate-600">
          <RouterLink to="/dashboard">{{ t('nav.dashboard') }}</RouterLink>
          <RouterLink to="/accounts">{{ t('nav.accounts') }}</RouterLink>
          <RouterLink to="/categories">{{ t('nav.categories') }}</RouterLink>
          <RouterLink to="/transactions">{{ t('nav.transactions') }}</RouterLink>
          <RouterLink to="/budgets">{{ t('nav.budgets') }}</RouterLink>
          <button class="rounded bg-slate-900 px-3 py-1 text-white" @click="logout">
            {{ t('app.logout') }}
          </button>
        </nav>
      </div>
    </header>

    <main class="mx-auto max-w-6xl px-4 py-6">
      <RouterView />
    </main>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue';
import { RouterLink, RouterView, useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useAuthStore } from './stores/auth';
import ToastList from './components/ToastList.vue';

const { t } = useI18n();
const authStore = useAuthStore();
const router = useRouter();

onMounted(() => {
  authStore.hydrate();
});

const isAuthenticated = computed(() => !!authStore.accessToken);

const logout = () => {
  authStore.logout();
  router.push('/login');
};
</script>
