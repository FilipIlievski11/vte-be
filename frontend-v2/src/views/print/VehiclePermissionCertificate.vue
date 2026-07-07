<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { VehiclePermissionPrint } from '@/types';

const props = defineProps<{ id: string }>();

const bundle = ref<VehiclePermissionPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ── Одобрение за управување со туѓо возило — OVERLAY printed onto the official
// pre-printed two-part form. Positions extracted 1:1 from legacy
// rptOdobrenieZaTugoV.Designer.vb (ReportUnit = 0.1mm, Tabloid sheet; content sits in
// the top ~130mm × 206mm). All values are UPPERCASE LATIN (legacy printPermisionInfo
// ToLat) in Arial bold, centre-aligned inside their boxes. ──

const MK2LAT: Record<string, string> = {
  А: 'A', Б: 'B', В: 'V', Г: 'G', Д: 'D', Ѓ: 'GJ', Е: 'E', Ж: 'ZH', З: 'Z', Ѕ: 'DZ',
  И: 'I', Ј: 'J', К: 'K', Л: 'L', Љ: 'LJ', М: 'M', Н: 'N', Њ: 'NJ', О: 'O', П: 'P',
  Р: 'R', С: 'S', Т: 'T', Ќ: 'KJ', У: 'U', Ф: 'F', Х: 'H', Ц: 'C', Ч: 'CH', Џ: 'DJ', Ш: 'SH',
  а: 'a', б: 'b', в: 'v', г: 'g', д: 'd', ѓ: 'gj', е: 'e', ж: 'zh', з: 'z', ѕ: 'dz',
  и: 'i', ј: 'j', к: 'k', л: 'l', љ: 'lj', м: 'm', н: 'n', њ: 'nj', о: 'o', п: 'p',
  р: 'r', с: 's', т: 't', ќ: 'kj', у: 'u', ф: 'f', х: 'h', ц: 'c', ч: 'ch', џ: 'dj', ш: 'sh',
};
function lat(s: string | null | undefined): string {
  if (!s) return '';
  return [...s.trim()].map(ch => MK2LAT[ch] ?? ch).join('').toUpperCase();
}
function fmt(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}.${mm}.${d.getFullYear()}`;
}

// Owner across two lines, exactly like legacy printPermisionInfo:
//   line 1: NAME [бр.лк. X]   ·   line 2: ADDRESS
const ownerLine1 = computed(() => {
  const b = bundle.value;
  if (!b) return '';
  const idPart = b.authorizedIdCardNumber ? '' : ''; // owner's лк not stored separately in v2
  return lat(`${b.ownerName ?? ''}${idPart}`);
});
const ownerLine2 = computed(() => lat(bundle.value?.ownerAddress));

// Fields: (x, y, w, h) box in mm from the Designer, text centred. `s` overrides font pt.
type F = { x: number; y: number; w: number; h: number; t: string; s?: number };
const fields = computed<F[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  const vehDisp = lat(b.vehicleDisplay);
  return [
    // ---- left part (барање половина) ----
    { x: 40.2, y: 14.8, w: 62.0, h: 6.5, t: lat(b.authorizedName) },
    { x: 34.9, y: 30.7, w: 34.0, h: 6.5, t: fmt(b.issuedDate) },
    { x: 77.3, y: 30.7, w: 25.0, h: 6.5, t: fmt(b.validTillDate) },
    { x: 6.4,  y: 52.9, w: 94.0, h: 6.5, t: vehDisp },
    { x: 32.8, y: 70.9, w: 69.0, h: 6.5, t: b.plateNumber ?? '' },
    { x: 38.1, y: 83.6, w: 64.0, h: 6.5, t: b.trafficLicenceNumber },
    { x: 47.6, y: 96.3, w: 54.0, h: 6.5, t: b.triptiqueNumber ?? '' },
    { x: 6.4,  y: 108.0, w: 94.0, h: 6.5, t: ownerLine1.value },
    { x: 6.4,  y: 115.4, w: 94.0, h: 6.5, t: ownerLine2.value },
    // ---- right part (одобрение картичка) ----
    { x: 111.1, y: 44.4, w: 94.0, h: 6.5, t: lat(b.authorizedName) },
    { x: 111.1, y: 58.2, w: 40.5, h: 6.5, t: b.authorizedPassportNumber ?? b.authorizedIdCardNumber ?? '' },
    { x: 163.0, y: 58.2, w: 41.5, h: 6.5, t: lat(b.issuerName) },
    // legacy: > 19 chars → font drops to 9pt on the card side
    { x: 113.2, y: 80.4, w: 43.5, h: 6.5, t: vehDisp, s: vehDisp.length > 19 ? 9 : undefined },
    { x: 161.9, y: 80.4, w: 42.3, h: 6.6, t: b.plateNumber ?? '' },
    { x: 141.8, y: 97.4, w: 62.5, h: 6.5, t: b.trafficLicenceNumber },
    { x: 149.2, y: 110.1, w: 25.7, h: 6.6, t: fmt(b.issuedDate) },
    { x: 179.9, y: 110.1, w: 25.7, h: 6.6, t: fmt(b.validTillDate) },
    { x: 111.1, y: 118.5, w: 94.0, h: 6.5, t: `${lat(b.issuingCityName)}, ${fmt(b.issuedDate)}` },
  ];
});

function fStyle(f: F) {
  return `left:${f.x}mm; top:${f.y}mm; width:${f.w}mm; height:${f.h}mm; line-height:${f.h}mm; font-size:${f.s ?? 11.25}pt;`;
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
    </div>
  </div>
</template>

<style scoped>
.screen { background: #e0e0e0; min-height: 100vh; padding: 8mm 0; }
.page {
  position: relative;
  /* Legacy sheet is Tabloid (279.4×431.8mm) but ALL content sits within the top
     130mm × 206mm — it fits on A4, which is what the station's printers take. */
  width: 210mm; height: 297mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: Arial, "Helvetica Neue", sans-serif;
  font-weight: 700;
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.t {
  position: absolute; white-space: nowrap;
  text-align: center; overflow: hidden;
}

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
