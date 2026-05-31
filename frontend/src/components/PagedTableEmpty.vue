<script setup lang="ts">
defineProps<{ icon?: string; title?: string; hint?: string; ctaLabel?: string }>()
defineEmits<{ (e: 'cta'): void }>()
</script>

<template>
  <div class="empty-state">
    <i class="pi" :class="icon ?? 'pi-inbox'" />
    <div class="empty-title">{{ title ?? 'Нема записи' }}</div>
    <div v-if="hint" class="empty-hint">{{ hint }}</div>
    <slot>
      <button v-if="ctaLabel" class="empty-cta" @click="$emit('cta')">
        <i class="pi pi-plus" /> {{ ctaLabel }}
      </button>
    </slot>
  </div>
</template>

<style scoped>
.empty-state {
  display: flex; flex-direction: column; align-items: center; justify-content: center;
  padding: 48px 16px; text-align: center; color: var(--text-color-secondary);
}
.empty-state i { font-size: 40px; opacity: 0.45; margin-bottom: 12px; }
.empty-title { font-size: 16px; font-weight: 600; color: var(--text-color); }
.empty-hint  { font-size: 13px; margin-top: 4px; max-width: 28rem; }
.empty-cta {
  margin-top: 16px; padding: 8px 16px; border-radius: 6px;
  background: var(--color-brand-600, #3b82f6); color: white; border: none; cursor: pointer;
  display: inline-flex; align-items: center; gap: 6px; font-weight: 500;
}
.empty-cta:hover { filter: brightness(1.05); }
</style>
