<script setup lang="ts">
import { computed, onBeforeUnmount, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import type { Client, Paged, VehicleListItem } from '@/types';

const { t } = useI18n();
const router = useRouter();

const q = ref('');
const open = ref(false);
const loading = ref(false);
const clients = ref<Client[]>([]);
const vehicles = ref<VehicleListItem[]>([]);
const activeIndex = ref(-1);
const rootEl = ref<HTMLElement | null>(null);
const inputEl = ref<{ $el: HTMLElement } | HTMLInputElement | null>(null);

type Hit = { kind: 'client' | 'vehicle'; id: number; primary: string; secondary: string };

const hits = computed<Hit[]>(() => [
  ...clients.value.map(c => ({
    kind: 'client' as const,
    id: c.id,
    primary: [c.firstName, c.middleName, c.lastName].filter(Boolean).join(' ') || `#${c.id}`,
    secondary: [c.mb, c.address].filter(Boolean).join(' · '),
  })),
  ...vehicles.value.map(v => ({
    kind: 'vehicle' as const,
    id: v.id,
    primary: [v.plate, [v.maker, v.model].filter(Boolean).join(' ')].filter(Boolean).join(' · ') || v.vin,
    secondary: [v.vin, v.ownerName].filter(Boolean).join(' · '),
  })),
]);

// Debounce + stale-response guard (fast typers outrun the API).
let timer: ReturnType<typeof setTimeout> | undefined;
let seq = 0;
function onInput() {
  clearTimeout(timer);
  const term = q.value.trim();
  if (term.length < 2) {
    clients.value = []; vehicles.value = []; loading.value = false;
    open.value = term.length > 0;
    return;
  }
  loading.value = true;
  open.value = true;
  timer = setTimeout(search, 300);
}

async function search() {
  const term = q.value.trim();
  if (term.length < 2) return;
  const my = ++seq;
  try {
    const [c, v] = await Promise.all([
      api.get<Paged<Client>>('/clients', { params: { q: term, pageSize: 5 } }),
      api.get<Paged<VehicleListItem>>('/vehicles', { params: { q: term, pageSize: 5 } }),
    ]);
    if (my !== seq) return;
    clients.value = c.data.items;
    vehicles.value = v.data.items;
    activeIndex.value = hits.value.length > 0 ? 0 : -1;
  } catch {
    if (my === seq) { clients.value = []; vehicles.value = []; }
  } finally {
    if (my === seq) loading.value = false;
  }
}

function go(h: Hit) {
  open.value = false;
  focusedInput()?.blur();
  router.push(h.kind === 'client' ? `/clients/${h.id}` : `/vehicles/${h.id}`);
}

function focusedInput(): HTMLInputElement | null {
  const el = inputEl.value as any;
  if (!el) return null;
  return el instanceof HTMLInputElement ? el : (el.$el ?? null);
}

function onKeydown(e: KeyboardEvent) {
  if (e.key === 'Escape') { open.value = false; focusedInput()?.blur(); return; }
  if (!open.value || hits.value.length === 0) return;
  if (e.key === 'ArrowDown') { e.preventDefault(); activeIndex.value = (activeIndex.value + 1) % hits.value.length; }
  else if (e.key === 'ArrowUp') { e.preventDefault(); activeIndex.value = (activeIndex.value - 1 + hits.value.length) % hits.value.length; }
  else if (e.key === 'Enter' && activeIndex.value >= 0) { e.preventDefault(); go(hits.value[activeIndex.value]); }
}

// Ctrl+K (or Cmd+K) from anywhere focuses the box.
function onGlobalKey(e: KeyboardEvent) {
  if ((e.ctrlKey || e.metaKey) && e.key.toLowerCase() === 'k') {
    e.preventDefault();
    focusedInput()?.focus();
    focusedInput()?.select();
    if (q.value.trim().length >= 2) open.value = true;
  }
}
function onDocMouseDown(e: MouseEvent) {
  if (rootEl.value && !rootEl.value.contains(e.target as Node)) open.value = false;
}
onMounted(() => {
  window.addEventListener('keydown', onGlobalKey);
  document.addEventListener('mousedown', onDocMouseDown);
});
onBeforeUnmount(() => {
  window.removeEventListener('keydown', onGlobalKey);
  document.removeEventListener('mousedown', onDocMouseDown);
});

function groupOffset(kind: 'client' | 'vehicle'): number {
  return kind === 'client' ? 0 : clients.value.length;
}
</script>

<template>
  <div class="gsearch" ref="rootEl">
    <span class="gs-icon"><i class="pi pi-search" /></span>
    <input
      ref="inputEl"
      v-model="q"
      class="gs-input"
      type="text"
      :placeholder="t('search.placeholder')"
      autocomplete="off"
      @input="onInput"
      @focus="q.trim().length >= 2 && (open = true)"
      @keydown="onKeydown"
    />
    <kbd class="gs-kbd">Ctrl K</kbd>

    <div v-if="open" class="gs-panel">
      <div v-if="q.trim().length < 2" class="gs-note">{{ t('search.minChars') }}</div>
      <div v-else-if="loading" class="gs-note">{{ t('search.searching') }}…</div>
      <template v-else-if="hits.length">
        <template v-if="clients.length">
          <div class="gs-group">{{ t('search.clients') }}</div>
          <button v-for="(c, i) in clients" :key="'c' + c.id" class="gs-row"
                  :class="{ active: activeIndex === groupOffset('client') + i }"
                  @mouseenter="activeIndex = groupOffset('client') + i"
                  @click="go(hits[groupOffset('client') + i])">
            <i class="pi pi-user" />
            <span class="gs-primary">{{ hits[groupOffset('client') + i].primary }}</span>
            <span class="gs-secondary">{{ hits[groupOffset('client') + i].secondary }}</span>
          </button>
        </template>
        <template v-if="vehicles.length">
          <div class="gs-group">{{ t('search.vehicles') }}</div>
          <button v-for="(v, i) in vehicles" :key="'v' + v.id + '-' + i" class="gs-row"
                  :class="{ active: activeIndex === groupOffset('vehicle') + i }"
                  @mouseenter="activeIndex = groupOffset('vehicle') + i"
                  @click="go(hits[groupOffset('vehicle') + i])">
            <i class="pi pi-car" />
            <span class="gs-primary">{{ hits[groupOffset('vehicle') + i].primary }}</span>
            <span class="gs-secondary">{{ hits[groupOffset('vehicle') + i].secondary }}</span>
          </button>
        </template>
        <div class="gs-hint">↑↓ {{ t('search.hintMove') }} · Enter {{ t('search.hintOpen') }} · Esc {{ t('search.hintClose') }}</div>
      </template>
      <div v-else class="gs-note">{{ t('search.noResults', { q: q.trim() }) }}</div>
    </div>
  </div>
</template>

<style scoped>
.gsearch { position: relative; display: flex; align-items: center; width: clamp(220px, 30vw, 420px); }
.gs-icon { position: absolute; left: .6rem; color: var(--color-text-muted, #9ca3af); font-size: .8rem; pointer-events: none; }
.gs-input {
  width: 100%;
  padding: .38rem 3.4rem .38rem 2rem;
  font-size: .85rem;
  border: 1px solid var(--color-border, #d1d5db);
  border-radius: 8px;
  /* Тема-свесни бои од самата апликација (се флипуваат под html.app-dark) —
     PrimeVue surface токените овде даваа темни букви на темна позадина. */
  background: var(--color-surface, #fff);
  color: var(--color-text, #111827);
  outline: none;
}
.gs-input::placeholder { color: var(--color-text-muted, #9ca3af); }
.gs-input:focus { border-color: var(--p-primary-400, #60a5fa); box-shadow: 0 0 0 2px color-mix(in srgb, var(--p-primary-400, #60a5fa) 25%, transparent); }
.gs-kbd {
  position: absolute; right: .5rem;
  font-family: inherit; font-size: .62rem; color: var(--color-text-muted, #9ca3af);
  border: 1px solid var(--p-surface-300, #d1d5db); border-radius: 4px; padding: .05rem .3rem;
  pointer-events: none; background: var(--p-surface-100, #f3f4f6);
}
.gs-panel {
  position: absolute; top: calc(100% + 6px); left: 0; right: 0;
  min-width: 340px;
  background: var(--surface-card, #fff);
  border: 1px solid var(--surface-border, #e5e7eb);
  border-radius: 10px;
  box-shadow: 0 10px 30px rgba(0, 0, 0, .12);
  padding: .3rem;
  z-index: 1200;
  max-height: 70vh; overflow-y: auto;
}
.gs-group {
  font-size: .68rem; text-transform: uppercase; letter-spacing: .03em;
  color: var(--color-text-muted, #6b7280);
  padding: .4rem .5rem .2rem;
}
.gs-row {
  display: flex; align-items: baseline; gap: .5rem;
  width: 100%; text-align: left;
  border: 0; background: transparent; color: inherit;
  padding: .38rem .5rem; border-radius: 6px; cursor: pointer;
  font-size: .85rem;
}
.gs-row i { font-size: .75rem; color: var(--color-text-muted, #9ca3af); align-self: center; }
.gs-row.active { background: var(--p-surface-100, #f3f4f6); }
html.app-dark .gs-row.active { background: var(--p-surface-800, #27272a); }
/* Името (primary) има приоритет — секундарното се сече прво. */
.gs-primary { font-weight: 600; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; flex: 1 1 auto; min-width: 45%; }
.gs-secondary { color: var(--color-text-muted, #6b7280); font-size: .75rem; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; margin-left: auto; flex: 0 1 auto; max-width: 50%; }
.gs-note { padding: .6rem .5rem; font-size: .82rem; color: var(--color-text-muted, #6b7280); }
.gs-hint { padding: .4rem .5rem .2rem; font-size: .68rem; color: var(--color-text-muted, #9ca3af); border-top: 1px solid var(--surface-border, #f1f5f9); margin-top: .25rem; }

@media (max-width: 768px) { .gsearch { display: none; } }
</style>
