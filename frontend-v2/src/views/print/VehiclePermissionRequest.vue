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
const lay = usePrintLayout('perm-req', 210);
const guides = ref(true);

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

const IN = 0.254; // 1/100 inch → mm

// Built-in default positions (mm) + font pt for the editable VALUE fields only.
// Values were legacy ReportUnit (1/100 inch) × IN — kept as the SAME expressions so the
// resolved default is numerically identical (byte-exact) to the fixed static coords.
// Static labels / headers / signature underline are NOT here — they keep fixed coords.
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  issuerOrg:          { x: 425 * IN, y: 50 * IN,  size: 9.75 },
  submittedBy:        { x: 150 * IN, y: 250 * IN, size: 9.75 },
  authorizedName:     { x: 225 * IN, y: 350 * IN, size: 9.75 },
  authorizedAddress:  { x: 225 * IN, y: 400 * IN, size: 9.75 },
  authorizedPassport: { x: 225 * IN, y: 450 * IN, size: 9.75 },
  plateNumber:        { x: 225 * IN, y: 500 * IN, size: 9.75 },
  ownerName:          { x: 225 * IN, y: 583 * IN, size: 9.75 },
};
lay.setDefaults(DEF);

// (x, y, w, h) box in mm; label boxes are MiddleLeft, the two headers are centred.
// k = stable key of an editable VALUE field (x/y/size come from lay.resolve); static
// labels omit k and keep their fixed legacy coords.
type F = { x: number; y: number; w: number; h: number; t: string; c?: 1; multi?: 1; k?: string };

const fields = computed<F[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  return [
    { x: 521 * IN, y: 17 * IN, w: 60, h: 25 * IN, t: 'До' },
    { x: 425 * IN, y: 50 * IN, w: 217 * IN, h: 58 * IN, t: b.issuerOrgName ?? b.companyName ?? '', c: 1, multi: 1, k: 'issuerOrg' },
    { x: 183 * IN, y: 150 * IN, w: 283 * IN, h: 58 * IN, t: 'БАРАЊЕ за издавање на одобрение за управување на туѓо моторно возило', c: 1, multi: 1 },
    { x: 25 * IN,  y: 250 * IN, w: 117 * IN, h: 25 * IN, t: 'Поднесено од:' },
    { x: 150 * IN, y: 250 * IN, w: 492 * IN, h: 25 * IN, t: up(b.ownerName), k: 'submittedBy' },
    { x: 25 * IN,  y: 292 * IN, w: 617 * IN, h: 25 * IN, t: 'Бараме да се издаде одобрение за управување на туѓо моторно возило' },
    { x: 25 * IN,  y: 350 * IN, w: 100 * IN, h: 25 * IN, t: 'За лицето' },
    { x: 225 * IN, y: 350 * IN, w: 417 * IN, h: 25 * IN, t: up(b.authorizedName), k: 'authorizedName' },
    { x: 25 * IN,  y: 400 * IN, w: 167 * IN, h: 25 * IN, t: 'Со адреса на живеење' },
    { x: 225 * IN, y: 400 * IN, w: 417 * IN, h: 25 * IN, t: up(b.authorizedAddress), k: 'authorizedAddress' },
    { x: 25 * IN,  y: 450 * IN, w: 125 * IN, h: 25 * IN, t: 'Број на пасош' },
    { x: 225 * IN, y: 450 * IN, w: 417 * IN, h: 25 * IN, t: b.authorizedPassportNumber ?? b.authorizedIdCardNumber ?? '', k: 'authorizedPassport' },
    { x: 25 * IN,  y: 500 * IN, w: 200 * IN, h: 25 * IN, t: 'Регистерски број на возило' },
    { x: 225 * IN, y: 500 * IN, w: 417 * IN, h: 25 * IN, t: b.plateNumber ?? '', k: 'plateNumber' },
    { x: 25 * IN,  y: 583 * IN, w: 200 * IN, h: 25 * IN, t: 'Возилото е сопственост на' },
    { x: 225 * IN, y: 583 * IN, w: 417 * IN, h: 25 * IN, t: up(b.ownerName), k: 'ownerName' },
    { x: 25 * IN,  y: 658 * IN, w: 175 * IN, h: 25 * IN, t: 'Однапред Ви благодариме' },
    { x: 483 * IN, y: 725 * IN, w: 92 * IN, h: 25 * IN, t: 'Барател' },
  ];
});
// signature line (static)
const line = { x: 425 * IN, y: 772 * IN, w: 200 * IN };

function fStyle(f: F) {
  const align = f.c ? 'center' : 'left';
  const wrap = f.multi ? 'normal' : 'nowrap';
  const lh = f.multi ? '1.25' : `${f.h}mm`;
  // Only VALUE fields (with a key) take x/y/size from the saved-layout resolver;
  // statics keep their fixed coords. w/h/align/wrap always stay template-driven.
  let x = f.x, y = f.y, size = '';
  if (f.k) {
    const p = lay.resolve(f.k);
    x = p.x; y = p.y; size = ` font-size:${p.size}pt;`;
  }
  return `left:${x.toFixed(2)}mm; top:${y.toFixed(2)}mm; width:${f.w.toFixed(2)}mm; min-height:${f.h.toFixed(2)}mm; line-height:${lh}; text-align:${align}; white-space:${wrap};${size}`;
}

// Representative sample used in layout-edit mode (no real record needed).
const SAMPLE: VehiclePermissionPrint = {
  id: 1, permissionNumber: '123/2026', trafficLicenceNumber: 'СА1234567', triptiqueNumber: null,
  issuedDate: '2026-07-10', startDate: '2026-07-10', validTillDate: '2027-07-10', note: null,
  ownerName: 'Петар Петровски', ownerIdNumber: '1234567890123', ownerAddress: 'ул. Македонија бр.10, Скопје',
  authorizedName: 'Марко Марковски', authorizedEmbg: '3210987654321', authorizedIdCardNumber: 'А1234567',
  authorizedPassportNumber: 'МК1234567', authorizedAddress: 'ул. Партизанска бр.5, Битола',
  vehicleDisplay: 'ФОЛКСВАГЕН ГОЛФ', plateNumber: 'SK-1234-AB', vehicleVin: 'WVWZZZ1KZAW000000',
  vehicleEngineNumber: 'ABC123456', issuerName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ', issuingCityName: 'Велес',
  issuerOrgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС', companyName: 'АВТО-БЕЗБЕДНОСТ МНС',
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
    <div v-if="!lay.editing.value" class="toolbar no-print">
      <button @click="doPrint">Печати</button>
      <button @click="doClose">Затвори</button>
    </div>

    <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Барање за полномошно'" v-model:guides="guides" />

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <div v-else-if="bundle" class="page" :ref="(el) => lay.setPageEl(el as HTMLElement | null)"
         :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
      <div v-for="(f, i) in fields" :key="i" class="t"
           :class="{ ed: !!f.k, sel: lay.selectedKey.value === f.k }"
           :style="fStyle(f)"
           @pointerdown="f.k && lay.beginDrag(f.k, $event)">{{ f.t }}</div>
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

/* ---- Layout-edit affordances (screen only); only VALUE fields (.ed) are movable ---- */
.page.editing .t.ed { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.page.editing .t.ed:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.page.editing .t.ed.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
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
