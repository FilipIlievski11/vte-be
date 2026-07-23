<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { VehiclePermissionPrint } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const bundle = ref<VehiclePermissionPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; A4 width 210mm) ----
const lay = usePrintLayout('perm-cert', 210);
const guides = ref(true);

// Base font (pt) used by every box unless it carries its own `s`.
const BASE_PT = 11.25;

// Built-in default positions (mm) + font pt, keyed by meaning. x/y route through
// resolve() so admins can move each VALUE; the two halves of the pre-printed form
// (барање / картичка) repeat several values, so keys are suffixed per half.
// Позициите се извлечени 1:1 од легаси PDF рендер (rptOdobrenieZaTugoV.pdf):
// за секоја вредност центарот на кутијата = центар на рендерираниот текст, а врвот
// = baseline − 4.4mm (кутија h=6.5mm со вертикално центриран 11.25pt текст).
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  // ---- left part (барање половина) ----
  authName:      { x: 41.1,  y: 8.9,   size: BASE_PT },
  validFrom:     { x: 32.0,  y: 23.8,  size: BASE_PT },
  validTill:     { x: 76.5,  y: 23.8,  size: BASE_PT },
  vehicle:       { x: 7.4,   y: 46.8,  size: BASE_PT },
  plate:         { x: 34.1,  y: 62.7,  size: BASE_PT },
  trafficLic:    { x: 39.1,  y: 73.8,  size: BASE_PT },
  // триптикот го нема во референтниот PDF (празен кај тој запис) — позициониран
  // според растојанието од легаси дизајнерот под сообраќајната дозвола.
  triptique:     { x: 48.6,  y: 86.5,  size: BASE_PT },
  ownerName:     { x: 7.4,   y: 98.4,  size: BASE_PT },
  ownerAddress:  { x: 7.4,   y: 107.3, size: BASE_PT },
  ownerIdCard:   { x: 5.6,   y: 116.3, size: BASE_PT },
  // ---- right part (одобрение картичка) ----
  cardAuthName:  { x: 109.0, y: 42.0,  size: BASE_PT },
  cardAuthEmbg:  { x: 111.1, y: 53.1,  size: BASE_PT },
  cardIssuer:    { x: 161.9, y: 52.4,  size: BASE_PT },
  cardVehicle:   { x: 136.7, y: 76.9,  size: BASE_PT },
  cardPlate:     { x: 136.7, y: 88.4,  size: BASE_PT },
  cardTrafficLic:{ x: 141.6, y: 99.7,  size: BASE_PT },
  cardValidFrom: { x: 144.7, y: 110.4, size: BASE_PT },
  cardValidTill: { x: 177.3, y: 110.4, size: BASE_PT },
  cardIssuePlace:{ x: 111.1, y: 118.7, size: BASE_PT },
};
lay.setDefaults(DEF);

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
// Легаси PDF формат: 14-07-2026 (со цртички).
function fmt(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}-${mm}-${d.getFullYear()}`;
}

// Owner across three lines, exactly like the legacy PDF:
//   NAME  ·  ADDRESS  ·  BR.LK.<бр. на лична карта>
const ownerLine1 = computed(() => lat(bundle.value?.ownerName));
const ownerLine2 = computed(() => lat(bundle.value?.ownerAddress));
const ownerIdLine = computed(() => {
  const id = bundle.value?.ownerIdNumber;
  return id ? `BR.LK.${lat(id)}` : '';
});

// Fields: stable key `k`, (w, h) box in mm from the Designer, text centred. x/y come
// from resolve(k); `s` is the field's dynamic default font pt (undefined → BASE_PT).
type F = { k: string; w: number; h: number; t: string; s?: number };
const fields = computed<F[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  const vehDisp = lat(b.vehicleDisplay);
  return [
    // ---- left part (барање половина) ----
    { k: 'authName',      w: 62.0, h: 6.5, t: lat(b.authorizedName) },
    { k: 'validFrom',     w: 34.0, h: 6.5, t: fmt(b.issuedDate) },
    { k: 'validTill',     w: 25.0, h: 6.5, t: fmt(b.validTillDate) },
    { k: 'vehicle',       w: 94.0, h: 6.5, t: vehDisp },
    { k: 'plate',         w: 69.0, h: 6.5, t: b.plateNumber ?? '' },
    { k: 'trafficLic',    w: 64.0, h: 6.5, t: b.trafficLicenceNumber },
    { k: 'triptique',     w: 54.0, h: 6.5, t: b.triptiqueNumber ?? '' },
    { k: 'ownerName',     w: 94.0, h: 6.5, t: ownerLine1.value },
    { k: 'ownerAddress',  w: 94.0, h: 6.5, t: ownerLine2.value },
    { k: 'ownerIdCard',   w: 94.0, h: 6.5, t: ownerIdLine.value },
    // ---- right part (одобрение картичка) ----
    { k: 'cardAuthName',  w: 94.0, h: 6.5, t: lat(b.authorizedName) },
    { k: 'cardAuthEmbg',  w: 40.5, h: 6.5, t: lat(b.authorizedPassportNumber ?? b.authorizedIdCardNumber) },
    { k: 'cardIssuer',    w: 41.5, h: 6.5, t: lat(b.issuerName) },
    // legacy: > 19 chars → font drops to 9pt on the card side
    { k: 'cardVehicle',   w: 43.5, h: 6.5, t: vehDisp, s: vehDisp.length > 19 ? 9 : undefined },
    { k: 'cardPlate',     w: 42.3, h: 6.6, t: b.plateNumber ?? '' },
    { k: 'cardTrafficLic',w: 62.5, h: 6.5, t: b.trafficLicenceNumber },
    { k: 'cardValidFrom', w: 25.7, h: 6.6, t: fmt(b.issuedDate) },
    { k: 'cardValidTill', w: 25.7, h: 6.6, t: fmt(b.validTillDate) },
    { k: 'cardIssuePlace',w: 94.0, h: 6.5, t: `${lat(b.issuingCityName)}, ${fmt(b.issuedDate)}` },
  ];
});

function fStyle(f: F) {
  const p = lay.resolve(f.k);
  // Font: admin size override wins; else the box's dynamic `s`; else the resolved
  // (base) default. Keeps the legacy 9pt drop when no override exists.
  const size = lay.overrides.value[f.k]?.size ?? f.s ?? p.size;
  return `left:${p.x}mm; top:${p.y}mm; width:${f.w}mm; height:${f.h}mm; line-height:${f.h}mm; font-size:${size}pt;`;
}

// Representative sample values used in layout-edit mode (no real record needed).
const SAMPLE: VehiclePermissionPrint = {
  id: 0, permissionNumber: '123/2026', trafficLicenceNumber: 'СА0123456', triptiqueNumber: 'T-99887',
  issuedDate: '2026-06-15', startDate: '2026-06-15', validTillDate: '2027-06-15', note: null,
  ownerName: 'ПЕТАР ПЕТРОВСКИ', ownerIdNumber: 'A1234567', ownerAddress: 'Браќа Миладиновци 21, Велес',
  authorizedName: 'МАРИЈА ЈОВАНОВСКА', authorizedEmbg: '2506990450012', authorizedIdCardNumber: 'B7654321',
  authorizedPassportNumber: null, authorizedAddress: 'Кеј 13 Ноември 5, Скопје',
  vehicleDisplay: 'VOLKSWAGEN GOLF 1.6 TDI', plateNumber: 'VE-1234-AB', vehicleVin: 'WVWZZZ1KZAW000000',
  vehicleEngineNumber: 'ABC123', issuerName: 'ГОРАН СТОЈАНОВСКИ', issuingCityName: 'Велес',
  issuerOrgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС', companyName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС',
};

onMounted(async () => {
  await lay.load();
  if (lay.editing.value) {
    // Layout-edit mode: use sample data, never auto-print.
    bundle.value = SAMPLE;
    loading.value = false;
    return;
  }
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

    <div v-else-if="bundle" :ref="(el) => lay.setPageEl(el as HTMLElement | null)" class="page"
         :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
      <div v-for="f in fields" :key="f.k" class="t" :class="{ sel: lay.selectedKey.value === f.k }"
           :style="fStyle(f)" @pointerdown="lay.beginDrag(f.k, $event)">{{ f.t }}</div>

      <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Одобрение за туѓо возило'" v-model:guides="guides" />
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

/* ---- Layout-edit affordances (screen only) ---- */
.page.editing .t { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.page.editing .t:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.page.editing .t.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
.page.guides {
  background-image:
    repeating-linear-gradient(0deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm),
    repeating-linear-gradient(90deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm);
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
