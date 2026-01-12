<template>
  <section>
    <h2 class="mb-4 text-2xl font-semibold">Transactions</h2>

    <div class="mb-6 grid gap-3 rounded-lg bg-white p-4 shadow md:grid-cols-6">
      <input v-model="filters.q" class="rounded border px-2 py-1" placeholder="Recherche" />
      <select v-model="filters.type" class="rounded border px-2 py-1">
        <option value="">Type</option>
        <option value="Expense">Dépense</option>
        <option value="Income">Revenu</option>
      </select>
      <select v-model="filters.accountId" class="rounded border px-2 py-1">
        <option value="">Compte</option>
        <option v-for="account in accountsStore.items" :key="account.id" :value="account.id">
          {{ account.name }}
        </option>
      </select>
      <select v-model="filters.categoryId" class="rounded border px-2 py-1">
        <option value="">Catégorie</option>
        <option v-for="category in categoriesStore.items" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>
      <input v-model="filters.fromUtc" type="date" class="rounded border px-2 py-1" />
      <input v-model="filters.toUtc" type="date" class="rounded border px-2 py-1" />
      <button class="rounded bg-slate-900 px-4 py-2 text-white md:col-span-2" @click="applyFilters">
        Filtrer
      </button>
    </div>

    <form class="mb-6 grid gap-3 rounded-lg bg-white p-4 shadow md:grid-cols-6" @submit.prevent="create">
      <select v-model="newTransaction.accountId" class="rounded border px-2 py-1" required>
        <option value="">Compte</option>
        <option v-for="account in accountsStore.items" :key="account.id" :value="account.id">
          {{ account.name }}
        </option>
      </select>
      <select v-model="newTransaction.categoryId" class="rounded border px-2 py-1" required>
        <option value="">Catégorie</option>
        <option v-for="category in categoriesStore.items" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>
      <select v-model="newTransaction.type" class="rounded border px-2 py-1" required>
        <option value="Expense">Dépense</option>
        <option value="Income">Revenu</option>
      </select>
      <input v-model.number="newTransaction.amount" type="number" class="rounded border px-2 py-1" placeholder="Montant" required />
      <input v-model="newTransaction.description" class="rounded border px-2 py-1" placeholder="Description" />
      <input v-model="newTransaction.occurredAtLocal" type="datetime-local" class="rounded border px-2 py-1" required />
      <button class="rounded bg-slate-900 px-4 py-2 text-white md:col-span-2">Ajouter</button>
    </form>

    <div v-if="transactionsStore.loading" class="text-slate-500">Chargement...</div>
    <div v-else-if="transactionsStore.items.length === 0" class="text-slate-500">Aucune transaction.</div>
    <div v-else class="space-y-3">
      <div
        v-for="transaction in transactionsStore.items"
        :key="transaction.id"
        class="flex items-center justify-between rounded-lg bg-white p-4 shadow"
      >
        <div>
          <p class="font-medium">{{ transaction.description || 'Sans description' }}</p>
          <p class="text-sm text-slate-500">
            {{ transaction.type === 'Expense' ? 'Dépense' : 'Revenu' }} ·
            {{ new Date(transaction.occurredAtUtc).toLocaleString() }}
          </p>
        </div>
        <div class="flex items-center gap-4">
          <span class="font-semibold">{{ transaction.amount.toFixed(2) }} €</span>
          <button class="text-sm text-red-600" @click="remove(transaction.id)">Supprimer</button>
        </div>
      </div>
    </div>

    <div class="mt-6 flex items-center justify-between" v-if="transactionsStore.totalCount > transactionsStore.pageSize">
      <button class="rounded border px-3 py-1" :disabled="transactionsStore.page === 1" @click="prevPage">
        Précédent
      </button>
      <span class="text-sm text-slate-500">
        Page {{ transactionsStore.page }} sur {{ totalPages }}
      </span>
      <button class="rounded border px-3 py-1" :disabled="transactionsStore.page === totalPages" @click="nextPage">
        Suivant
      </button>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, reactive } from 'vue';
import { useAccountsStore } from '../stores/accounts';
import { useCategoriesStore } from '../stores/categories';
import { useTransactionsStore } from '../stores/transactions';
import { useToastStore } from '../stores/toasts';
import type { TransactionType } from '../types/api';

const accountsStore = useAccountsStore();
const categoriesStore = useCategoriesStore();
const transactionsStore = useTransactionsStore();
const toastStore = useToastStore();

const filters = reactive({
  q: '',
  type: '',
  accountId: '',
  categoryId: '',
  fromUtc: '',
  toUtc: ''
});

const newTransaction = reactive({
  accountId: '',
  categoryId: '',
  type: 'Expense' as TransactionType,
  amount: 0,
  description: '',
  occurredAtLocal: ''
});

const totalPages = computed(() => Math.ceil(transactionsStore.totalCount / transactionsStore.pageSize));

const loadData = async () => {
  await Promise.all([accountsStore.fetchAll(), categoriesStore.fetchAll()]);
  await transactionsStore.fetchAll();
};

const applyFilters = async () => {
  transactionsStore.setFilters({
    q: filters.q || undefined,
    type: filters.type ? (filters.type as TransactionType) : undefined,
    accountId: filters.accountId || undefined,
    categoryId: filters.categoryId || undefined,
    fromUtc: filters.fromUtc ? new Date(filters.fromUtc).toISOString() : undefined,
    toUtc: filters.toUtc ? new Date(filters.toUtc).toISOString() : undefined
  });
  await transactionsStore.fetchAll();
};

const create = async () => {
  try {
    await transactionsStore.add({ ...newTransaction });
    toastStore.push('Transaction ajoutée.', 'success');
  } catch {
    toastStore.push('Erreur lors de la création.', 'error');
  }
};

const remove = async (id: string) => {
  try {
    await transactionsStore.remove(id);
    toastStore.push('Transaction supprimée.', 'success');
  } catch {
    toastStore.push('Erreur lors de la suppression.', 'error');
  }
};

const nextPage = async () => {
  transactionsStore.page += 1;
  await transactionsStore.fetchAll();
};

const prevPage = async () => {
  transactionsStore.page -= 1;
  await transactionsStore.fetchAll();
};

onMounted(loadData);
</script>
