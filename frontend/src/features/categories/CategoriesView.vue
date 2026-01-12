<template>
  <section>
    <h2 class="mb-4 text-2xl font-semibold">Catégories</h2>

    <form class="mb-6 grid gap-3 rounded-lg bg-white p-4 shadow md:grid-cols-3" @submit.prevent="create">
      <input v-model="name" class="rounded border px-3 py-2" placeholder="Nom" required />
      <select v-model="isIncome" class="rounded border px-3 py-2">
        <option :value="false">Dépense</option>
        <option :value="true">Revenu</option>
      </select>
      <button class="rounded bg-slate-900 px-4 py-2 text-white">Ajouter</button>
    </form>

    <div v-if="categoriesStore.loading" class="text-slate-500">Chargement...</div>
    <div v-else-if="categoriesStore.items.length === 0" class="text-slate-500">Aucune catégorie.</div>
    <div v-else class="space-y-3">
      <div v-for="category in categoriesStore.items" :key="category.id" class="flex items-center justify-between rounded-lg bg-white p-4 shadow">
        <div>
          <p class="font-medium">{{ category.name }}</p>
          <p class="text-sm text-slate-500">{{ category.isIncome ? 'Revenu' : 'Dépense' }}</p>
        </div>
        <button class="text-sm text-red-600" @click="remove(category.id)">Supprimer</button>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useCategoriesStore } from '../stores/categories';
import { useToastStore } from '../stores/toasts';

const categoriesStore = useCategoriesStore();
const toastStore = useToastStore();
const name = ref('');
const isIncome = ref(false);

onMounted(() => {
  categoriesStore.fetchAll();
});

const create = async () => {
  try {
    await categoriesStore.add(name.value, isIncome.value);
    name.value = '';
    isIncome.value = false;
    toastStore.push('Catégorie ajoutée.', 'success');
  } catch {
    toastStore.push('Erreur lors de la création.', 'error');
  }
};

const remove = async (id: string) => {
  try {
    await categoriesStore.remove(id);
    toastStore.push('Catégorie supprimée.', 'success');
  } catch {
    toastStore.push('Erreur lors de la suppression.', 'error');
  }
};
</script>
