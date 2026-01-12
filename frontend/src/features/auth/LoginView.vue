<template>
  <div class="mx-auto max-w-md rounded-lg bg-white p-6 shadow">
    <h2 class="mb-4 text-xl font-semibold">{{ t('auth.login') }}</h2>
    <form class="space-y-4" @submit.prevent="submit">
      <div>
        <label class="block text-sm font-medium">{{ t('auth.email') }}</label>
        <input v-model="email" type="email" class="mt-1 w-full rounded border px-3 py-2" required />
      </div>
      <div>
        <label class="block text-sm font-medium">{{ t('auth.password') }}</label>
        <input v-model="password" type="password" class="mt-1 w-full rounded border px-3 py-2" required />
      </div>
      <button class="w-full rounded bg-slate-900 px-4 py-2 text-white" :disabled="authStore.loading">
        {{ t('auth.login') }}
      </button>
    </form>
    <p class="mt-4 text-sm text-slate-600">
      Pas de compte ? <RouterLink class="text-slate-900 underline" to="/register">Créer un compte</RouterLink>
    </p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter, RouterLink } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useAuthStore } from '../stores/auth';
import { useToastStore } from '../stores/toasts';

const { t } = useI18n();
const authStore = useAuthStore();
const toastStore = useToastStore();
const router = useRouter();

const email = ref('');
const password = ref('');

const submit = async () => {
  try {
    await authStore.login(email.value, password.value);
    toastStore.push('Connexion réussie.', 'success');
    router.push('/dashboard');
  } catch {
    toastStore.push(authStore.error ?? 'Erreur de connexion.', 'error');
  }
};
</script>
