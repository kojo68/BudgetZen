<template>
  <div class="fixed right-4 top-4 z-50 space-y-2">
    <div
      v-for="toast in toasts.items"
      :key="toast.id"
      class="rounded-lg px-4 py-2 shadow-lg"
      :class="{
        'bg-emerald-500 text-white': toast.type === 'success',
        'bg-red-500 text-white': toast.type === 'error',
        'bg-slate-800 text-white': toast.type === 'info'
      }"
    >
      {{ toast.message }}
    </div>
  </div>
</template>

<script setup lang="ts">
import { useToastStore } from '../stores/toasts';

const toasts = useToastStore();
</script>
