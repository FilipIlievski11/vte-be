<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { api } from '@/api/client';
import type { TechExamZapisnik } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const z = ref<TechExamZapisnik | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; Letter width 215.9mm) ----
const lay = usePrintLayout('techexam-zapisnik', 215.9);
const guides = ref(true);

// Built-in default positions (mm) + font pt. Value fields carry size; checkbox slots
// are fixed-size glyphs (size omitted). Extracted from the legacy .prnx render.
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  org:            { x: 28.6,  y: 24.3,  size: 10 },
  madeDate:       { x: 174.6, y: 15.9,  size: 9 },
  regNumber:      { x: 174.6, y: 26.5,  size: 9 },
  plate:          { x: 128.1, y: 46.6,  size: 10 },
  customerName:   { x: 37,    y: 72,    size: 10 },
  cityName:       { x: 58.2,  y: 80.4,  size: 10 },
  communityName:  { x: 143.9, y: 80.4,  size: 10 },
  livingAddress:  { x: 47.6,  y: 87.8,  size: 9 },
  maker:          { x: 56.1,  y: 103.7, size: 9 },
  modelFull:      { x: 131.2, y: 103.7, size: 9 },
  makeYear:       { x: 66.7,  y: 110.3, size: 9 },
  colorFull:      { x: 142.9, y: 110.3, size: 9 },
  axleCount:      { x: 66.7,  y: 116.8, size: 9 },
  maxWeight:      { x: 142.9, y: 116.8, size: 9 },
  engine:         { x: 66.7,  y: 123.4, size: 9 },
  vin:            { x: 142.9, y: 123.4, size: 9 },
  capacity:       { x: 79.4,  y: 129.9, size: 9 },
  power:          { x: 147.1, y: 129.9, size: 9 },
  propAxis:       { x: 79.4,  y: 136.5, size: 9 },
  seats:          { x: 142.9, y: 136.5, size: 9 },
  emptyWeight:    { x: 79.4,  y: 143,   size: 9 },
  country:        { x: 79.4,  y: 148.5, size: 9 },
  // checkbox glyph slots (position-editable, fixed glyph size)
  social:     { x: 50.8, y: 160.4 },
  private:    { x: 50.8, y: 166.7 },
  redoven:    { x: 26.5, y: 177.3 },
  redovenNa6: { x: 49.7, y: 177.3 },
  delumno:    { x: 98.4, y: 177.3 },
  potpoln:    { x: 145,  y: 177.3 },
};
const CB_KEYS = ['social', 'private', 'redoven', 'redovenNa6', 'delumno', 'potpoln'];
lay.setDefaults(DEF);

function fStyle(key: string): string {
  const p = lay.resolve(key);
  return `left:${p.x}mm; top:${p.y}mm; font-size:${p.size}pt;`;
}
function cbStyle(key: string): string {
  const p = lay.resolve(key);
  return `left:${p.x}mm; top:${p.y}mm;`;
}

// Representative sample values used in layout-edit mode (no real record needed).
const SAMPLE: TechExamZapisnik = {
  organizationName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС', madeDate: '2026-06-15', regNumber: '123/2026',
  plate: 'VE-1234-AB', customerName: 'ПЕТАР ПЕТРОВСКИ', cityName: 'Велес', communityName: 'Велес',
  livingAddress: 'Браќа Миладиновци 21', maker: 'VOLKSWAGEN', modelFull: 'GOLF 1.6 TDI', makeYear: 2015,
  colorFull: 'СИВА МЕТАЛИК', axleCount: 2, maxAllowedWeightKg: 1800, engineTypeAndNum: 'ДИЗЕЛ / ABC123',
  vin: 'WVWZZZ1KZAW000000', engineCapacityCc: 1598, enginePowerKw: 77, propulsionAxis: 1, seats: 5,
  emptyWeightKg: 1320, madeCountry: 'ГЕРМАНИЈА', technicalExamTypeId: 1, isSocial: false,
} as unknown as TechExamZapisnik;

function fmtDate(s: string | null): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth() + 1).padStart(2, '0')}.${d.getFullYear()}`;
}
function num(v: number | null | undefined): string {
  // Legacy prints the raw numeric (0 when unset), so blank/null → "0".
  return (v === null || v === undefined) ? '0' : String(Math.round(v));
}

// Owner address: legacy uppercases the street line, prefixes the house number
// with "БР." and appends the community → "БРАТСТВО БР.21 ГОРНО ОРИЗАРИ, Велес".
const addr = computed(() => {
  if (!z.value) return '';
  let a = (z.value.livingAddress || '').toUpperCase();
  a = a.replace(/(\s)(\d)/, '$1БР.$2');   // first " <digit>" → " БР.<digit>"
  const c = z.value.communityName;
  return c ? (a ? `${a}, ${c}` : c) : a;
});

// Which exam-type checkbox: legacy Select Case on IdTypeOfTehnicalExam (1-4).
const examBox = computed(() => {
  switch (z.value?.technicalExamTypeId) {
    case 1: return 'redoven';
    case 2: return 'redovenNa6';
    case 3: return 'delumno';
    case 4: return 'potpoln';
    default: return null;
  }
});
const ownerBox = computed(() => (z.value?.isSocial ? 'social' : 'private'));

// Ordered value fields for rendering (key → text). cls flags the wide org header.
const fields = computed<{ k: string; v: string; cls?: string }[]>(() => {
  const d = z.value;
  if (!d) return [];
  const s = (v: string | null | undefined) => v ?? '';
  return [
    { k: 'org',           v: s(d.organizationName), cls: 'org-hdr' },
    { k: 'madeDate',      v: fmtDate(d.madeDate) },
    { k: 'regNumber',     v: s(d.regNumber) },
    { k: 'plate',         v: s(d.plate) },
    { k: 'customerName',  v: s(d.customerName) },
    { k: 'cityName',      v: s(d.cityName) },
    { k: 'communityName', v: s(d.communityName) },
    { k: 'livingAddress', v: addr.value },
    { k: 'maker',         v: s(d.maker) },
    { k: 'modelFull',     v: s(d.modelFull) },
    { k: 'makeYear',      v: String(d.makeYear ?? '') },
    { k: 'colorFull',     v: s(d.colorFull) },
    { k: 'axleCount',     v: String(d.axleCount ?? '') },
    { k: 'maxWeight',     v: num(d.maxAllowedWeightKg) },
    { k: 'engine',        v: s(d.engineTypeAndNum) },
    { k: 'vin',           v: s(d.vin) },
    { k: 'capacity',      v: num(d.engineCapacityCc) },
    { k: 'power',         v: num(d.enginePowerKw) },
    { k: 'propAxis',      v: num(d.propulsionAxis) },
    { k: 'seats',         v: String(d.seats ?? '') },
    { k: 'emptyWeight',   v: num(d.emptyWeightKg) },
    { k: 'country',       v: s(d.madeCountry) },
  ];
});

onMounted(async () => {
  await lay.load();
  if (lay.editing.value) {
    // Layout-edit mode: use sample data, never auto-print.
    z.value = SAMPLE;
    loading.value = false;
    return;
  }
  try {
    z.value = (await api.get<TechExamZapisnik>(`/technical-exams/${props.id}/zapisnik`)).data;
    setTimeout(() => window.print(), 250);
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load';
  } finally {
    loading.value = false;
  }
});
function doPrint() { window.print(); }
function doClose() { window.close(); }
</script>

<template>
  <div class="print-wrap">
    <div v-if="loading" class="loading">Се вчитува…</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else-if="z" :ref="(el) => lay.setPageEl(el as HTMLElement | null)" class="paper" :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="215.9" :height-mm="279.4" />
      <span v-for="fld in fields" :key="fld.k"
            class="f" :class="[fld.cls, { sel: lay.selectedKey.value === fld.k }]" :style="fStyle(fld.k)"
            @pointerdown="lay.beginDrag(fld.k, $event)">{{ fld.v }}</span>

      <!-- Checkboxes: in print mode only the active owner+exam X show; in edit mode ALL
           slots render so each can be positioned. -->
      <template v-if="lay.editing.value">
        <span v-for="k in CB_KEYS" :key="k" class="cb" :class="{ sel: lay.selectedKey.value === k }"
              :style="cbStyle(k)" @pointerdown="lay.beginDrag(k, $event)">X</span>
      </template>
      <template v-else>
        <span class="cb" :style="cbStyle(ownerBox)">X</span>
        <span v-if="examBox" class="cb" :style="cbStyle(examBox)">X</span>
      </template>

      <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Записник за технички преглед'" v-model:guides="guides" />

      <div v-if="!lay.editing.value" class="toolbar no-print">
        <button @click="doPrint">Печати</button>
        <button @click="doClose">Затвори</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.print-wrap { background: #e0e0e0; min-height: 100vh; padding: 1rem 0; }
.paper {
  position: relative;
  width: 215.9mm; height: 279.4mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: "Times New Roman", serif;
  box-shadow: 0 0 12px rgba(0,0,0,.15);
  overflow: hidden;
}
.f { position: absolute; white-space: nowrap; line-height: 1; }
.org-hdr { width: 78.3mm; white-space: normal; line-height: 1.15; }

/* ---- Layout-edit affordances (screen only) ---- */
.paper.editing .f, .paper.editing .cb { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.paper.editing .f:hover, .paper.editing .cb:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.paper.editing .f.sel, .paper.editing .cb.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
.paper.guides {
  background-image:
    repeating-linear-gradient(0deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm),
    repeating-linear-gradient(90deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm);
}
.cb {
  position: absolute;
  width: 4mm; height: 4mm;
  border: 0.3mm solid #000;
  box-sizing: border-box;
  display: flex; align-items: center; justify-content: center;
  font-family: Arial, sans-serif; font-weight: 700; font-size: 8pt; line-height: 1;
}
.loading, .error { text-align: center; padding: 4rem 0; font-family: system-ui, sans-serif; }
.error { color: #b91c1c; }
.toolbar.no-print { position: fixed; right: 1rem; top: 1rem; display: flex; gap: .5rem; z-index: 999; }
.toolbar.no-print button { padding: .35rem .75rem; border: 1px solid #888; background: #fff; border-radius: 4px; cursor: pointer; font-size: .85rem; }

@media print {
  .print-wrap { background: #fff; padding: 0; }
  .paper { box-shadow: none; margin: 0; }
  .no-print { display: none !important; }
  @page { size: Letter; margin: 0; }
}
</style>
