<template>
  <section>
    <div class="mb-6 flex items-center justify-between">
      <h2 class="text-2xl font-semibold">Tableau de bord</h2>
      <div class="flex items-center gap-2 text-sm">
        <input v-model.number="year" type="number" class="w-24 rounded border px-2 py-1" />
        <input v-model.number="month" type="number" class="w-16 rounded border px-2 py-1" min="1" max="12" />
        <button class="rounded bg-slate-900 px-3 py-1 text-white" @click="loadSummary">
          Charger
        </button>
      </div>
    </div>

    <div v-if="dashboardStore.loading" class="text-slate-500">Chargement...</div>
    <div v-else-if="!dashboardStore.summary" class="text-slate-500">Aucune donnée pour cette période.</div>
    <div v-else class="grid gap-6 lg:grid-cols-3">
      <div class="rounded-lg bg-white p-4 shadow">
        <p class="text-sm text-slate-500">Revenus</p>
        <p class="text-2xl font-semibold">{{ dashboardStore.summary.totalIncome.toFixed(2) }} €</p>
      </div>
      <div class="rounded-lg bg-white p-4 shadow">
        <p class="text-sm text-slate-500">Dépenses</p>
        <p class="text-2xl font-semibold">{{ dashboardStore.summary.totalExpense.toFixed(2) }} €</p>
      </div>
      <div class="rounded-lg bg-white p-4 shadow">
        <p class="text-sm text-slate-500">Net</p>
        <p class="text-2xl font-semibold">{{ dashboardStore.summary.net.toFixed(2) }} €</p>
      </div>

      <div class="rounded-lg bg-white p-4 shadow lg:col-span-2">
        <h3 class="mb-3 text-lg font-semibold">Dépenses par catégorie</h3>
        <ul class="space-y-2">
          <li v-for="item in dashboardStore.summary.byCategory" :key="item.categoryId" class="flex justify-between">
            <span>{{ item.categoryName }}</span>
            <span class="font-medium">{{ item.total.toFixed(2) }} €</span>
          </li>
        </ul>
      </div>
      <div class="rounded-lg bg-white p-4 shadow">
        <h3 class="mb-3 text-lg font-semibold">Dépenses par semaine</h3>
        <ul class="space-y-2">
          <li v-for="item in dashboardStore.summary.byWeek" :key="item.week" class="flex justify-between">
            <span>Semaine {{ item.week }}</span>
            <span class="font-medium">{{ item.total.toFixed(2) }} €</span>
          </li>
        </ul>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useDashboardStore } from '../stores/dashboard';

const dashboardStore = useDashboardStore();
const now = new Date();
const year = ref(now.getFullYear());
const month = ref(now.getMonth() + 1);

const loadSummary = async () => {
  await dashboardStore.fetchSummary(year.value, month.value);
};

onMounted(loadSummary);
</script>
