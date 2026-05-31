<script setup lang="ts">
/**
 * Top-level print page. Picks the right template based on which colored form
 * the request needs (PLAV / BEL / ZELEN), and provides the chrome around it
 * (loading state, toolbar, print button, guide toggle, edit-position mode).
 */
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { RequestPrintBundle } from '@/types';
import ZelenTemplate from './ZelenTemplate.vue';
import PlavTemplate from './PlavTemplate.vue';

const props = defineProps<{ id: string }>();
const { t } = useI18n();

const bundle = ref<RequestPrintBundle | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);
const showGuides = ref(false);
const editMode = ref(false);

// Both templates expose the same { resetPositions, exportPositions } surface.
const tplRef = ref<{ resetPositions?: () => void; exportPositions?: () => string } | null>(null);

/**
 * Normalize the document-print code to one of PLAV / BEL / ZELEN.
 * The C# seeder uses Latin codes ("PLAV", "BEL", "ZELEN") but migrate-request-
 * catalogs.sql derives codes from the legacy Cyrillic Opis ("ПЛАВ", "БЕЛ",
 * "ЗЕЛЕН"), so migrated requests carry Cyrillic codes. Match both scripts.
 * Order matters: check ZELEN/PLAV before BEL so Latin "BEL" can't shadow them.
 */
const formKind = computed<'PLAV' | 'BEL' | 'ZELEN' | 'OTHER'>(() => {
  const c = (bundle.value?.type.documentPrintCode || '').toUpperCase();
  if (c.includes('ZELEN') || c.includes('ЗЕЛЕН')) return 'ZELEN';
  if (c.includes('PLAV')  || c.includes('ПЛАВ'))  return 'PLAV';
  if (c.includes('BEL')   || c.includes('БЕЛ'))   return 'BEL';
  return 'OTHER';
});

onMounted(async () => {
  try {
    const { data } = await api.get<RequestPrintBundle>(`/requests/${props.id}/print`);
    bundle.value = data;
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load';
  } finally {
    loading.value = false;
  }
});

function doPrint() {
  // Always turn off edit/guides before printing
  editMode.value = false;
  showGuides.value = false;
  setTimeout(() => window.print(), 50);
}
function doClose() { window.close(); }

function downloadPositions() {
  const json = tplRef.value?.exportPositions?.() ?? '';
  if (!json) return;
  const code = (bundle.value?.type.documentPrintCode || 'form').toLowerCase();
  const blob = new Blob([json], { type: 'application/json' });
  const url = URL.createObjectURL(blob);
  const a = document.createElement('a');
  a.href = url;
  a.download = `${code}-positions-${new Date().toISOString().slice(0,10)}.json`;
  document.body.appendChild(a);
  a.click();
  a.remove();
  URL.revokeObjectURL(url);
}
function resetPositions() {
  if (!confirm('Врати ги почетните позиции? Сите рачни поместувања ќе се изгубат.')) return;
  tplRef.value?.resetPositions?.();
}
</script>

<template>
  <div class="print-wrap">
    <div v-if="loading" class="status">{{ t('requests.print.loading') }}</div>
    <div v-else-if="error" class="status err">{{ error }}</div>

    <template v-else-if="bundle">
      <div class="toolbar no-print">
        <span class="toolbar-title">
          {{ bundle.type.documentPrintName }} · {{ t('requests.print.no') }} {{ bundle.request.id }}
        </span>
        <label class="cb">
          <input type="checkbox" v-model="showGuides" />
          {{ showGuides ? 'Скриј мрежа' : 'Покажи мрежа' }}
        </label>
        <label class="cb edit-toggle" :class="{ on: editMode }">
          <input type="checkbox" v-model="editMode" />
          {{ editMode ? '✓ Уредувам' : 'Уреди позиции' }}
        </label>
        <button v-if="editMode" @click="downloadPositions" class="primary">Изнеси JSON</button>
        <button v-if="editMode" @click="resetPositions">Врати ги почетните</button>
        <button @click="doPrint">{{ t('requests.print.printAgain') }}</button>
        <button @click="doClose">{{ t('requests.print.close') }}</button>
      </div>

      <div v-if="editMode" class="edit-hint no-print">
        <strong>Режим за уредување е активен.</strong>
        Влечи го секое зелено поле на правото место.
        Притисни <strong>Изнеси JSON</strong> кога ќе бидеш готов и испрати го фајлот за да се запишат во кодот.
        Промените се чуваат локално и при обновување на страницата.
      </div>

      <!-- Pick template by normalized form kind (handles Latin + Cyrillic codes) -->
      <template v-if="formKind === 'PLAV'">
        <PlavTemplate
          ref="tplRef"
          :bundle="bundle"
          :showGuides="showGuides"
          :editMode="editMode"
        />
      </template>
      <template v-else-if="formKind === 'ZELEN'">
        <ZelenTemplate
          ref="tplRef"
          :bundle="bundle"
          :showGuides="showGuides"
          :editMode="editMode"
        />
      </template>
      <template v-else>
        <ZelenTemplate
          ref="tplRef"
          :bundle="bundle"
          :showGuides="showGuides"
          :editMode="editMode"
        />
        <div class="fallback-note no-print">
          ⚠ Шаблонот за <strong>{{ bundle.type.documentPrintCode }}</strong> сè уште
          не е калибриран — се прикажува со зелениот шаблон.
        </div>
      </template>
    </template>
  </div>
</template>

<style scoped>
.print-wrap {
  background: #d4d4d8;
  min-height: 100vh;
  padding: 3rem 0 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
}
.status { padding: 4rem 0; font-family: system-ui, sans-serif }
.status.err { color: #b91c1c }

.toolbar.no-print {
  position: fixed;
  top: 0; left: 0; right: 0;
  z-index: 999;
  display: flex; gap: .5rem; align-items: center;
  padding: .5rem .75rem;
  background: #1f2937; color: #fff;
  font-family: system-ui, sans-serif; font-size: .85rem;
}
.toolbar-title { flex: 1; font-weight: 600 }
.toolbar button {
  padding: .3rem .75rem; border: 1px solid #6b7280; background: #374151;
  color: #fff; border-radius: 4px; cursor: pointer; font-size: .8rem;
}
.toolbar button:hover { background: #4b5563 }
.toolbar button.primary { background: #16a34a; border-color: #15803d }
.toolbar button.primary:hover { background: #15803d }
.toolbar .cb {
  display: inline-flex; align-items: center; gap: .35rem;
  padding: .25rem .65rem; border: 1px solid #6b7280; border-radius: 4px;
  cursor: pointer;
}
.toolbar .cb.edit-toggle.on { background: #16a34a; border-color: #15803d }
.toolbar .cb input { cursor: pointer }

.edit-hint {
  margin: .75rem 0 1rem;
  padding: .75rem 1rem;
  background: #ecfccb; border: 1px solid #65a30d; border-radius: 6px;
  font-family: system-ui, sans-serif; font-size: .85rem;
  color: #365314; max-width: 210mm;
}

.fallback-note {
  margin: 1rem 0;
  padding: .5rem 1rem;
  background: #fef3c7; border: 1px solid #f59e0b; border-radius: 6px;
  font-family: system-ui, sans-serif; font-size: .85rem;
  color: #78350f;
}

@media print {
  html, body { margin: 0; padding: 0; background: #fff; }
  .print-wrap { background: #fff; padding: 0; min-height: 0; display: block; }
  .no-print { display: none !important }
  @page { size: A4 portrait; margin: 0 }
}
</style>
