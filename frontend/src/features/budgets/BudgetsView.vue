<template>
  <section>
    <h2 class="mb-4 text-2xl font-semibold">Budgets</h2>

    <form class="mb-6 grid gap-3 rounded-lg bg-white p-4 shadow md:grid-cols-4" @submit.prevent="create">
      <select v-model="categoryId" class="rounded border px-3 py-2" required>
        <option value="">Catégorie</option>
        <option v-for="category in categoriesStore.items" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>
      <input v-model.number="monthlyLimit" type="number" class="rounded border px-3 py-2" placeholder="Limite mensuelle" required />
      <input v-model.number="month" type="number" class="rounded border px-3 py-2" min="1" max="12" />
      <input v-model.number="year" type="number" class="rounded border px-3 py-2" min="2000" max="2100" />
      <button class="rounded bg-slate-900 px-4 py-2 text-white md:col-span-4">Ajouter</button>
    </form>

    <div v-if="budgetsStore.loading" class="text-slate-500">Chargement...</div>
    <div v-else-if="budgetsStore.items.length === 0" class="text-slate-500">Aucun budget.</div>
    <div v-else class="space-y-3">
      <div v-for="budget in budgetsStore.items" :key="budget.id" class="flex items-center justify-between rounded-lg bg-white p-4 shadow">
        <div>
          <p class="font-medium">{{ categoryName(budget.categoryId) }}</p>
          <p class="text-sm text-slate-500">{{ budget.month }}/{{ budget.year }}</p>
        </div>
        <div class="flex items-center gap-4">
          <span class="font-semibold">{{ budget.monthlyLimit.toFixed(2) }} €</span>
          <button class="text-sm text-red-600" @click="remove(budget.id)">Supprimer</button>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useBudgetsStore } from '../stores/budgets';
import { useCategoriesStore } from '../stores/categories';
import { useToastStore } from '../stores/toasts';

const budgetsStore = useBudgetsStore();
const categoriesStore = useCategoriesStore();
const toastStore = useToastStore();

const now = new Date();
const categoryId = ref('');
const monthlyLimit = ref(0);
const month = ref(now.getMonth() + 1);
const year = ref(now.getFullYear());

onMounted(async () => {
  await categoriesStore.fetchAll();
  await budgetsStore.fetchAll();
});

const categoryName = (id: string) => categoriesStore.items.find((c) => c.id === id)?.name ?? 'N/A';

const create = async () => {
  try {
    await budgetsStore.add({
      categoryId: categoryId.value,
      monthlyLimit: monthlyLimit.value,
      month: month.value,
      year: year.value
    });
    toastStore.push('Budget ajouté.', 'success');
  } catch {
    toastStore.push('Erreur lors de la création.', 'error');
  }
};

const remove = async (id: string) => {
  try {
    await budgetsStore.remove(id);
    toastStore.push('Budget supprimé.', 'success');
  } catch {
    toastStore.push('Erreur lors de la suppression.', 'error');
  }
};
</script>
