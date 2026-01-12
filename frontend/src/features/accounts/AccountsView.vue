<template>
  <section>
    <h2 class="mb-4 text-2xl font-semibold">Comptes</h2>

    <form class="mb-6 grid gap-3 rounded-lg bg-white p-4 shadow md:grid-cols-3" @submit.prevent="create">
      <input v-model="name" class="rounded border px-3 py-2" placeholder="Nom du compte" required />
      <input v-model.number="initialBalance" type="number" class="rounded border px-3 py-2" placeholder="Solde initial" />
      <button class="rounded bg-slate-900 px-4 py-2 text-white">Ajouter</button>
    </form>

    <div v-if="accountsStore.loading" class="text-slate-500">Chargement...</div>
    <div v-else-if="accountsStore.items.length === 0" class="text-slate-500">Aucun compte.</div>
    <div v-else class="space-y-3">
      <div v-for="account in accountsStore.items" :key="account.id" class="flex items-center justify-between rounded-lg bg-white p-4 shadow">
        <div>
          <p class="font-medium">{{ account.name }}</p>
          <p class="text-sm text-slate-500">Solde initial: {{ account.initialBalance.toFixed(2) }} €</p>
        </div>
        <button class="text-sm text-red-600" @click="remove(account.id)">Supprimer</button>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useAccountsStore } from '../stores/accounts';
import { useToastStore } from '../stores/toasts';

const accountsStore = useAccountsStore();
const toastStore = useToastStore();
const name = ref('');
const initialBalance = ref(0);

onMounted(() => {
  accountsStore.fetchAll();
});

const create = async () => {
  try {
    await accountsStore.add(name.value, initialBalance.value);
    name.value = '';
    initialBalance.value = 0;
    toastStore.push('Compte ajouté.', 'success');
  } catch {
    toastStore.push('Erreur lors de la création.', 'error');
  }
};

const remove = async (id: string) => {
  try {
    await accountsStore.remove(id);
    toastStore.push('Compte supprimé.', 'success');
  } catch {
    toastStore.push('Erreur lors de la suppression.', 'error');
  }
};
</script>
