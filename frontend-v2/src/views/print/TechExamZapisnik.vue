<script setup lang="ts">
import { onMounted, ref, computed } from 'vue';
import { api } from '@/api/client';
import type { TechExamZapisnik } from '@/types';

const props = defineProps<{ id: string }>();

const z = ref<TechExamZapisnik | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// Field positions in mm (top-left) extracted from the legacy .prnx render.
// Letter paper: 215.9 x 279.4 mm. Values are stamped onto the pre-printed form.
const F: Record<string, [number, number]> = {
  org:            [28.6, 24.3],
  madeDate:       [174.6, 15.9],
  regNumber:      [174.6, 26.5],
  plate:          [128.1, 46.6],
  customerName:   [37, 72],
  cityName:       [58.2, 80.4],
  communityName:  [143.9, 80.4],
  livingAddress:  [47.6, 87.8],
  maker:          [56.1, 103.7],
  modelFull:      [131.2, 103.7],
  makeYear:       [66.7, 110.3],
  colorFull:      [142.9, 110.3],
  axleCount:      [66.7, 116.8],
  maxWeight:      [142.9, 116.8],
  engine:         [66.7, 123.4],
  vin:            [142.9, 123.4],
  capacity:       [79.4, 129.9],
  power:          [147.1, 129.9],
  propAxis:       [79.4, 136.5],
  seats:          [142.9, 136.5],
  emptyWeight:    [79.4, 143],
  country:        [79.4, 148.5],
};
// Checkbox glyph positions (mm) — designer box + the render's ~1.6mm baseline offset.
const CB: Record<string, [number, number]> = {
  social:     [50.8, 160.4],
  private:    [50.8, 166.7],
  redoven:    [26.5, 177.3],
  redovenNa6: [49.7, 177.3],
  delumno:    [98.4, 177.3],
  potpoln:    [145, 177.3],
};

function pos(key: string, fs = 9) {
  const [x, y] = F[key];
  return `left:${x}mm; top:${y}mm; font-size:${fs}pt;`;
}
function cbPos(key: string | null) {
  if (!key || !CB[key]) return 'display:none;';
  const [x, y] = CB[key];
  return `left:${x}mm; top:${y}mm;`;
}

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

onMounted(async () => {
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

    <div v-else-if="z" class="paper">
      <span class="f org-hdr" :style="pos('org', 10)">{{ z.organizationName }}</span>
      <span class="f" :style="pos('madeDate')">{{ fmtDate(z.madeDate) }}</span>
      <span class="f" :style="pos('regNumber')">{{ z.regNumber }}</span>
      <span class="f" :style="pos('plate', 10)">{{ z.plate }}</span>
      <span class="f" :style="pos('customerName', 10)">{{ z.customerName }}</span>
      <span class="f" :style="pos('cityName', 10)">{{ z.cityName }}</span>
      <span class="f" :style="pos('communityName', 10)">{{ z.communityName }}</span>
      <span class="f" :style="pos('livingAddress')">{{ addr }}</span>

      <span class="f" :style="pos('maker')">{{ z.maker }}</span>
      <span class="f" :style="pos('modelFull')">{{ z.modelFull }}</span>
      <span class="f" :style="pos('makeYear')">{{ z.makeYear }}</span>
      <span class="f" :style="pos('colorFull')">{{ z.colorFull }}</span>
      <span class="f" :style="pos('axleCount')">{{ z.axleCount }}</span>
      <span class="f" :style="pos('maxWeight')">{{ num(z.maxAllowedWeightKg) }}</span>
      <span class="f" :style="pos('engine')">{{ z.engineTypeAndNum }}</span>
      <span class="f" :style="pos('vin')">{{ z.vin }}</span>
      <span class="f" :style="pos('capacity')">{{ num(z.engineCapacityCc) }}</span>
      <span class="f" :style="pos('power')">{{ num(z.enginePowerKw) }}</span>
      <span class="f" :style="pos('propAxis')">{{ num(z.propulsionAxis) }}</span>
      <span class="f" :style="pos('seats')">{{ z.seats }}</span>
      <span class="f" :style="pos('emptyWeight')">{{ num(z.emptyWeightKg) }}</span>
      <span class="f" :style="pos('country')">{{ z.madeCountry }}</span>

      <!-- Ownership + exam-type X marks -->
      <span class="cb" :style="cbPos(ownerBox)">X</span>
      <span v-if="examBox" class="cb" :style="cbPos(examBox)">X</span>

      <div class="toolbar no-print">
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
