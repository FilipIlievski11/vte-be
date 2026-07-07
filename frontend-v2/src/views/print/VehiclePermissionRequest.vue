<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { VehiclePermissionPrint } from '@/types';

const props = defineProps<{ id: string }>();

const bundle = ref<VehiclePermissionPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ── Барање за издавање на одобрение за управување на туѓо моторно возило — plain A4
// letter. Positions from legacy rptBaranjeZaOdobrenieZaTugoVozilo.Designer.vb
// (ReportUnit = 1/100 inch → ×0.254mm). Arial 9.75pt bold; values UPPERCASE Cyrillic
// (legacy OwnerDisplayCyr / CustomerDisplayCyr). ──

function up(s: string | null | undefined): string {
  return (s ?? '').trim().toUpperCase();
}
function fmt(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}.${mm}.${d.getFullYear()}`;
}

// (x, y, w, h) box in mm; label boxes are MiddleLeft, the two headers are centred.
type F = { x: number; y: number; w: number; h: number; t: string; c?: 1; multi?: 1 };
const IN = 0.254; // 1/100 inch → mm

const fields = computed<F[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  return [
    { x: 521 * IN, y: 17 * IN, w: 60, h: 25 * IN, t: 'До' },
    { x: 425 * IN, y: 50 * IN, w: 217 * IN, h: 58 * IN, t: b.issuerOrgName ?? b.companyName ?? '', c: 1, multi: 1 },
    { x: 183 * IN, y: 150 * IN, w: 283 * IN, h: 58 * IN, t: 'БАРАЊЕ за издавање на одобрение за управување на туѓо моторно возило', c: 1, multi: 1 },
    { x: 25 * IN,  y: 250 * IN, w: 117 * IN, h: 25 * IN, t: 'Поднесено од:' },
    { x: 150 * IN, y: 250 * IN, w: 492 * IN, h: 25 * IN, t: up(b.ownerName) },
    { x: 25 * IN,  y: 292 * IN, w: 617 * IN, h: 25 * IN, t: 'Бараме да се издаде одобрение за управување на туѓо моторно возило' },
    { x: 25 * IN,  y: 350 * IN, w: 100 * IN, h: 25 * IN, t: 'За лицето' },
    { x: 225 * IN, y: 350 * IN, w: 417 * IN, h: 25 * IN, t: up(b.authorizedName) },
    { x: 25 * IN,  y: 400 * IN, w: 167 * IN, h: 25 * IN, t: 'Со адреса на живеење' },
    { x: 225 * IN, y: 400 * IN, w: 417 * IN, h: 25 * IN, t: up(b.authorizedAddress) },
    { x: 25 * IN,  y: 450 * IN, w: 125 * IN, h: 25 * IN, t: 'Број на пасош' },
    { x: 225 * IN, y: 450 * IN, w: 417 * IN, h: 25 * IN, t: b.authorizedPassportNumber ?? b.authorizedIdCardNumber ?? '' },
    { x: 25 * IN,  y: 500 * IN, w: 200 * IN, h: 25 * IN, t: 'Регистерски број на возило' },
    { x: 225 * IN, y: 500 * IN, w: 417 * IN, h: 25 * IN, t: b.plateNumber ?? '' },
    { x: 25 * IN,  y: 583 * IN, w: 200 * IN, h: 25 * IN, t: 'Возилото е сопственост на' },
    { x: 225 * IN, y: 583 * IN, w: 417 * IN, h: 25 * IN, t: up(b.ownerName) },
    { x: 25 * IN,  y: 658 * IN, w: 175 * IN, h: 25 * IN, t: 'Однапред Ви благодариме' },
    { x: 483 * IN, y: 725 * IN, w: 92 * IN, h: 25 * IN, t: 'Барател' },
  ];
});
// signature line
const line = { x: 425 * IN, y: 772 * IN, w: 200 * IN };

function fStyle(f: F) {
  const align = f.c ? 'center' : 'left';
  const wrap = f.multi ? 'normal' : 'nowrap';
  const lh = f.multi ? '1.25' : `${f.h}mm`;
  return `left:${f.x.toFixed(2)}mm; top:${f.y.toFixed(2)}mm; width:${f.w.toFixed(2)}mm; min-height:${f.h.toFixed(2)}mm; line-height:${lh}; text-align:${align}; white-space:${wrap};`;
}

onMounted(async () => {
  try {
    const { data } = await api.get<VehiclePermissionPrint>(`/vehicle-permissions/${props.id}/print`);
    bundle.value = data;
    setTimeout(() => window.print(), 300);
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
  <div class="screen">
    <!-- toolbar BEFORE the page so .page:last-of-type matches (no blank extra sheet) -->
    <div class="toolbar no-print">
      <button @click="doPrint">Печати</button>
      <button @click="doClose">Затвори</button>
    </div>

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <div v-else-if="bundle" class="page">
      <div v-for="(f, i) in fields" :key="i" class="t" :style="fStyle(f)">{{ f.t }}</div>
      <div class="u" :style="`left:${line.x.toFixed(2)}mm; top:${line.y.toFixed(2)}mm; width:${line.w.toFixed(2)}mm;`"></div>
    </div>
  </div>
</template>

<style scoped>
.screen { background: #e0e0e0; min-height: 100vh; padding: 8mm 0; }
.page {
  position: relative;
  width: 210mm; height: 297mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: Arial, "Helvetica Neue", sans-serif;
  font-weight: 700; font-size: 9.75pt;
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.t { position: absolute; }
.u { position: absolute; height: 0; border-bottom: 0.3mm solid #000; }

.msg { text-align: center; padding: 4rem 0; font-family: system-ui, sans-serif; }
.err { color: #b91c1c; }
.toolbar.no-print { position: fixed; right: 1rem; top: 1rem; display: flex; gap: .5rem; z-index: 999; }
.toolbar.no-print button { padding: .35rem .75rem; border: 1px solid #888; background: #fff; border-radius: 4px; cursor: pointer; }

@media print {
  .screen { background: #fff; padding: 0; min-height: auto; }
  .page { margin: 0; box-shadow: none; overflow: visible; }
  .no-print { display: none !important; }
  @page { size: A4 portrait; margin: 0; }
}
</style>
