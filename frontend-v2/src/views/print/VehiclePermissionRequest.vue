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

// ── БАРАЊЕ за издавање на одобрение за управување со туѓо моторно возило во
// странство — the official two-page table form. Geometry extracted from the live
// legacy print (rptBaranjeZaOdobrenieZaTugoVozilo2.pdf): text baselines and cell
// borders via pdfjs, PT→MM. Arial 9.75pt regular; captions 8pt; title 16pt.
// Value tops = PDF baseline − 3.81mm (the cert-proven offset scaled 11.25→9.75pt). ──

function fmtD(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}-${mm}-${d.getFullYear()}`;
}
// Birthdates print with a two-digit year in legacy composites (15-01-81).
function fmtY2(s: string | null | undefined): string {
  const f = fmtD(s);
  return f ? f.slice(0, 6) + f.slice(8) : '';
}
const dash = (parts: (string | null | undefined)[]) =>
  parts.map((s) => (s ?? '').trim()).filter(Boolean).join(',');

// Built-in default positions (mm from the PDF) + font pt for the editable VALUE fields.
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  issuerOrg:        { x: 32.3,  y: 63.7,  size: 9.75 },
  userLine:         { x: 23.9,  y: 123.3, size: 9.75 },
  ownerPersonLine:  { x: 42.9,  y: 147.4, size: 9.75 },
  ownerCompanyLine: { x: 42.9,  y: 166.7, size: 9.75 },
  passportNo:       { x: 148.3, y: 202.0, size: 9.75 },
  plateNo:          { x: 80.9,  y: 208.4, size: 9.75 },
  vehicleMake:      { x: 80.9,  y: 214.7, size: 9.75 },
  trafficLicNo:     { x: 96.2,  y: 221.1, size: 9.75 },
  issueDate:        { x: 54.4,  y: 226.2, size: 9.75 },
  validTill:        { x: 54.4,  y: 232.6, size: 9.75 },
  permSerial:       { x: 72.5,  y: 238.7, size: 9.75 },
  cityAndDate:      { x: 15.6,  y: 115.9, size: 9.75 }, // page 2
};
lay.setDefaults(DEF);

// Bordered cells of the form (static — cell borders + label text stay put).
type Cell = { x: number; y: number; w: number; h: number; t?: string; c?: 1; size?: number; lines?: string[] };
const CELLS1: Cell[] = [
  { x: 30.8, y: 51.2, w: 82.6, h: 4.6, t: 'РЕПУБЛИКА СЕВЕРНА МАКЕДОНИЈА' },
  { x: 30.8, y: 55.8, w: 159.3, h: 8.6, lines: ['МИНИСТЕРСТВО ЗА ВНАТРЕШНИ РАБОТИ / правно лице за вршење на технички', 'преглед на возила'] },
  { x: 30.8, y: 64.4, w: 78.4, h: 4.7 },                                             // issuer org (value field)
  { x: 88.6, y: 91.5, w: 29.4, h: 6.8, t: 'БАРАЊЕ', c: 1, size: 16 },
  { x: 61.3, y: 113.5, w: 76.8, h: 4.6, t: '(да се пополни читливо со печатни букви)', c: 1 },
  { x: 14.3, y: 123.6, w: 9.1, h: 4.6, t: 'Од:' },
  { x: 14.3, y: 140.4, w: 127.3, h: 4.6, t: 'Податоци за сопственикот на возилото:' },
  { x: 14.3, y: 148.1, w: 28.1, h: 4.6, t: 'Физичко лице:' },
  { x: 14.3, y: 167.4, w: 28.1, h: 4.5, t: 'Правно лице:' },
  { x: 14.3, y: 182.5, w: 121.3, h: 6.4, t: 'Во прилог кој барањето се поднесуваат:' },
  { x: 14.3, y: 188.9, w: 94.9, h: 6.3, t: '1. Важечка лична карта на сопственикот на возилото' },
  { x: 109.2, y: 188.9, w: 46.2, h: 6.3 },
  { x: 14.3, y: 195.2, w: 94.9, h: 6.4, t: '2. Овластување од правното лице' },
  { x: 14.0, y: 201.6, w: 133.5, h: 6.3, t: '3. Важечка патна исправа на корисникот (лична карта/пасош број)' },
  { x: 147.8, y: 201.6, w: 46.3, h: 6.3 },
  { x: 14.0, y: 207.9, w: 66.0, h: 6.4, t: '4. Регистарски таблици на возилото' },
  { x: 80.4, y: 207.9, w: 67.4, h: 6.4 },
  { x: 14.3, y: 214.3, w: 66.1, h: 6.3, t: '5. Марка и тип на возилото' },
  { x: 80.4, y: 214.3, w: 57.7, h: 6.3 },
  { x: 14.3, y: 220.6, w: 81.4, h: 6.3, t: '6. Сериски број на сообраќајната дозвола' },
  { x: 95.7, y: 220.6, w: 45.9, h: 6.4 },
  { x: 14.3, y: 227.0, w: 39.6, h: 6.3, t: '7. Датум на издавање' },
  { x: 53.9, y: 226.9, w: 37.2, h: 4.9 },
  { x: 14.3, y: 233.3, w: 39.6, h: 6.4, t: '8. Период на важење' },
  { x: 53.9, y: 233.3, w: 37.2, h: 4.8 },
  { x: 14.3, y: 239.7, w: 57.4, h: 6.3, t: '9. Сериски број на одобрението' },
  { x: 72.0, y: 239.4, w: 37.2, h: 4.9 },
];
const CELLS2: Cell[] = [
  { x: 14.3, y: 42.5, w: 65.7, h: 6.3, t: 'Потпис на сопственик на возилото' },
  { x: 80.0, y: 42.5, w: 44.2, h: 6.3 },
  { x: 14.3, y: 48.8, w: 65.7, h: 6.4, t: 'Потпис на корисни на возилото' },
  { x: 80.0, y: 48.8, w: 44.2, h: 6.4 },
  { x: 85.1, y: 69.5, w: 62.4, h: 4.5, t: 'Потпис на овластено правно лице' },
  { x: 141.6, y: 77.1, w: 31.0, h: 4.6, t: 'М.П.' },
  { x: 15.1, y: 116.6, w: 57.1, h: 4.5 },                                            // место, датум (value field)
  { x: 72.2, y: 116.6, w: 27.3, h: 4.5, t: 'година' },
];

// Plain (borderless) static texts. y = top (baseline − 3.11mm at 9.75pt / 2.55mm at 8pt).
type Txt = { x: number; y: number; t: string; size?: number; w?: number; c?: 1; lh?: number };
const TXT1: Txt[] = [
  { x: 14, y: 101.9, w: 187.5, c: 1, lh: 3.95, t: 'ЗА ИЗДАВАЊЕ НА ОДОБРЕНИЕ ЗА УПРАВУВАЊЕ СО ТУЃО МОТОРНО ВОЗИЛО ВО\nСТРАНСТВО' },
  { x: 22.3, y: 128.9, size: 8, t: '(име, презиме, место на раѓање, датум на раѓање,место на живеење, матичен број и важечка лична карта и орган кој ја издал и' },
  { x: 88.4, y: 132.1, size: 8, t: 'адреса на законски престој)' },
  { x: 47.0, y: 153.4, size: 8, t: '(име, презиме, место на раѓање, датум на раѓање,место на живеење, лична карта, сообраќајна дозвола од возилото и' },
  { x: 116.3, y: 156.6, size: 8, t: 'матичен број)' },
  { x: 62.4, y: 172.3, size: 8, t: 'назив на правното лице, седиште, матичен број на субјектот, цообраќајна дозвола од возилото' },
];
const TXT2: Txt[] = [
  { x: 17.0, y: 90.4, t: 'За точноста на податоците одговарам лично и материјално, а за сите грешки во податоците' },
  { x: 17.0, y: 94.4, t: 'согласен/на сум да ги сносам последиците.' },
  { x: 15.6, y: 137.1, t: '*ЗАБЕЛЕШКА: За странец одобрението за управување со туѓо моторно возило во странство се однесува само' },
  { x: 15.6, y: 141.1, t: 'за М1 категорија, согласно член 317 став (7) од законот за безбедност на сообраќајот на патиштата ("Службен' },
  { x: 15.6, y: 145.0, t: 'весник на Република Северна Македонија" бр. 169/15, 226/15 и 55/16)' },
];
// Standalone rules (signature line + the underlined liability sentence).
const LINES2 = [
  { x: 147.5, y: 69.5, w: 53.2 },
  { x: 17.0, y: 93.9, w: 158.7 },
  { x: 17.0, y: 97.8, w: 74.9 },
];

// ---- Value fields (draggable) ----
type V = { k: string; t: string; page: 1 | 2 };
const values = computed<V[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  const business = b.ownerIsBusiness === true;
  return [
    { k: 'issuerOrg', page: 1, t: b.issuerOrgName ?? b.companyName ?? '' },
    { k: 'userLine', page: 1, t: dash([b.authorizedName, b.authorizedBirthCityName, fmtY2(b.authorizedDateOfBirth), b.authorizedLivingCityName, b.authorizedEmbg, b.authorizedIdCardNumber || b.authorizedPassportNumber, b.authorizedAddress]) },
    { k: 'ownerPersonLine', page: 1, t: business ? '' : dash([b.ownerName, b.ownerBirthCityName, fmtY2(b.ownerDateOfBirth), b.ownerLivingCityName, b.trafficLicenceNumber, b.ownerIdNumber]) },
    { k: 'ownerCompanyLine', page: 1, t: business ? dash([b.ownerName, b.ownerAddress, b.ownerIdNumber, b.trafficLicenceNumber]) : '' },
    { k: 'passportNo', page: 1, t: b.authorizedIdCardNumber || b.authorizedPassportNumber || '' },
    { k: 'plateNo', page: 1, t: b.plateNumber ?? '' },
    { k: 'vehicleMake', page: 1, t: b.vehicleDisplay ?? '' },
    { k: 'trafficLicNo', page: 1, t: b.trafficLicenceNumber ?? '' },
    { k: 'issueDate', page: 1, t: fmtD(b.issuedDate) },
    { k: 'validTill', page: 1, t: fmtD(b.validTillDate) },
    { k: 'permSerial', page: 1, t: String(b.id) },
    { k: 'cityAndDate', page: 2, t: `${b.issuingCityName ?? ''}, ${fmtD(b.issuedDate)}` },
  ];
});

function cellStyle(c: Cell) {
  const size = c.size ?? 9.75;
  const lh = c.lines ? '3.95mm' : `${(c.h - 0.44).toFixed(2)}mm`;
  return `left:${c.x}mm; top:${c.y}mm; width:${c.w}mm; height:${c.h}mm; font-size:${size}pt; line-height:${lh}; text-align:${c.c ? 'center' : 'left'};`;
}
function txtStyle(t: Txt) {
  const size = t.size ?? 9.75;
  const w = t.w != null ? `width:${t.w}mm; ` : '';
  const lh = t.lh != null ? `${t.lh}mm` : '1';
  return `left:${t.x}mm; top:${t.y}mm; ${w}font-size:${size}pt; line-height:${lh}; text-align:${t.c ? 'center' : 'left'};`;
}
function vStyle(k: string) {
  const p = lay.resolve(k);
  return `left:${p.x.toFixed(2)}mm; top:${p.y.toFixed(2)}mm; height:5.6mm; line-height:5.6mm; font-size:${p.size}pt;`;
}

// Representative sample used in layout-edit mode (no real record needed).
const SAMPLE: VehiclePermissionPrint = {
  id: 4476, permissionNumber: '123/2026', trafficLicenceNumber: '1805799', triptiqueNumber: null,
  issuedDate: '2026-07-15', startDate: '2026-07-15', validTillDate: '2027-07-15', note: null,
  ownerName: 'ДПГТУ ПЕСКАРА-ВЕЛ ДООЕЛ', ownerIdNumber: '5508495', ownerAddress: 'Академик Пенчо Давчев 128 Велес',
  authorizedName: 'САШКО МАНЕВ', authorizedEmbg: '1501981480042', authorizedIdCardNumber: 'М1140928',
  authorizedPassportNumber: null, authorizedAddress: 'Мицко Козар 20',
  vehicleDisplay: 'VOLKSWAGEN GOLF 1.6 TDI', plateNumber: 'VE 3414 AC', vehicleVin: 'WVWZZZ1KZAW000000',
  vehicleEngineNumber: 'ABC123456', issuerName: 'МВР Велес', issuingCityName: 'Велес',
  issuerOrgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ Велес', companyName: 'АВТО-БЕЗБЕДНОСТ МНС',
  ownerIsBusiness: true, ownerBirthCityName: null, ownerDateOfBirth: null, ownerLivingCityName: null,
  authorizedBirthCityName: 'ВЕЛЕС', authorizedDateOfBirth: '1981-01-15', authorizedLivingCityName: 'Велес',
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
    <div v-if="!lay.editing.value" class="toolbar no-print">
      <button @click="doPrint">Печати</button>
      <button @click="doClose">Затвори</button>
    </div>

    <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Барање за полномошно'" v-model:guides="guides" />

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <template v-else-if="bundle">
      <!-- PAGE 1 -->
      <div class="page" :ref="(el) => lay.setPageEl(el as HTMLElement | null)"
           :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
        <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
        <div v-for="(c, i) in CELLS1" :key="'c' + i" class="cell" :style="cellStyle(c)">
          <template v-if="c.lines"><div v-for="(l, j) in c.lines" :key="j">{{ l }}</div></template>
          <template v-else>{{ c.t ?? '' }}</template>
        </div>
        <div v-for="(t, i) in TXT1" :key="'t' + i" class="txt" :style="txtStyle(t)">{{ t.t }}</div>
        <div v-for="v in values.filter((x) => x.page === 1)" :key="v.k" class="val ed"
             :class="{ sel: lay.selectedKey.value === v.k }" :style="vStyle(v.k)"
             @pointerdown="lay.beginDrag(v.k, $event)">{{ v.t }}</div>
      </div>

      <!-- PAGE 2 -->
      <div class="page" :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
        <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
        <div v-for="(c, i) in CELLS2" :key="'c' + i" class="cell" :style="cellStyle(c)">{{ c.t ?? '' }}</div>
        <div v-for="(t, i) in TXT2" :key="'t' + i" class="txt" :style="txtStyle(t)">{{ t.t }}</div>
        <div v-for="(l, i) in LINES2" :key="'l' + i" class="u" :style="`left:${l.x}mm; top:${l.y}mm; width:${l.w}mm;`"></div>
        <div v-for="v in values.filter((x) => x.page === 2)" :key="v.k" class="val ed"
             :class="{ sel: lay.selectedKey.value === v.k }" :style="vStyle(v.k)"
             @pointerdown="lay.beginDrag(v.k, $event)">{{ v.t }}</div>
      </div>
    </template>
  </div>
</template>

<style scoped>
.screen { background: #e0e0e0; min-height: 100vh; padding: 8mm 0; }
.page {
  position: relative;
  width: 210mm; height: 297mm;
  margin: 0 auto 8mm; background: #fff; color: #000;
  font-family: Arial, "Helvetica Neue", sans-serif;
  font-weight: 400; font-size: 9.75pt;
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.cell {
  position: absolute; border: 0.22mm solid #000; box-sizing: border-box;
  padding-left: 0.5mm; white-space: nowrap; overflow: visible;
}
.txt { position: absolute; white-space: pre; }
.val { position: absolute; white-space: nowrap; }
.u { position: absolute; height: 0; border-bottom: 0.25mm solid #000; }

/* ---- Layout-edit affordances (screen only); only VALUE fields (.ed) are movable ---- */
.page.editing .val.ed { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.page.editing .val.ed:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.page.editing .val.ed.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
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
  .page { margin: 0; box-shadow: none; overflow: visible; break-after: page; }
  .page:last-of-type { break-after: auto; }
  .no-print { display: none !important; }
  @page { size: A4 portrait; margin: 0; }
}
</style>
