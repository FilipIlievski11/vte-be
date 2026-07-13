<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { IdlRequestPrint } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const bundle = ref<IdlRequestPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; A4 width 210mm) ----
// Only the bound VALUES (dataItems) are editable/persisted. The STATIC[] form text and
// UNDER[] fill-in rules keep their fixed extracted positions.
const lay = usePrintLayout('idl-req', 210);
const guides = ref(true);

// Values print exactly as stored (Cyrillic) — no transliteration.
function tl(s: string | null | undefined): string {
  return s ?? '';
}

function fmt(s: string | null | undefined, y4: boolean): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yy = y4 ? String(d.getFullYear()) : String(d.getFullYear()).slice(-2);
  return `${dd}-${mm}-${yy}`;
}

// Applicant line: name, DOB, birthplace, living place, EMBG, address — Latin, comma-separated.
const applicantLine = computed(() => {
  const b = bundle.value;
  if (!b) return '';
  const parts = [
    tl(b.clientFullName),
    fmt(b.dateOfBirth, false),
    tl(b.birthCityName),
    tl(b.livingCityName),
    b.embg ?? '',
    tl(b.livingAddress),
  ].filter(p => p && p.length);
  return parts.join(', ');
});

// ── Static text runs — EXACT baselines/x from pdf.js getTextContent on the legacy PDF ──
// (y = text baseline in mm from page top; s = font size in pt; b = bold)
type T = { p: number; x: number; y: number; s: number; b?: 0 | 1; t: string };
const STATIC: T[] = [
  // Page 1 — header
  { p: 1, x: 15.3, y: 53.2, s: 9.8, b: 1, t: 'РЕПУБЛИКА СЕВЕРНА МАКЕДОНИЈА' },
  { p: 1, x: 15.3, y: 57.6, s: 9.8, b: 1, t: 'МИНИСТЕРСТВО ЗА ВНАТРЕШНИ РАБОТИ _________________________________ или' },
  { p: 1, x: 15.3, y: 62.0, s: 9.8, b: 1, t: 'ПРАВНО ЛИЦЕ ЗА ВРШЕЊЕ НА ТЕХНИЧКИ ПРЕГЛЕД НА ВОЗИЛА ОВЛАСТЕНО ОД' },
  { p: 1, x: 15.3, y: 66.4, s: 9.8, b: 1, t: 'MИНИСТЕРСТВО ЗА ВНАТРЕШНИ РАБОТИ' },
  { p: 1, x: 168.7, y: 66.8, s: 9.0, b: 0, t: '(назив и седиште' },
  // title
  { p: 1, x: 87.6, y: 98.9, s: 16.0, b: 1, t: 'БАРАЊЕ' },
  { p: 1, x: 53.7, y: 103.8, s: 9.8, b: 1, t: 'ЗА ИЗДАВАЊЕ НА МЕЃУНАРОДНА ВОЗАЧКА ДОЗВОЛА' },
  { p: 1, x: 66.4, y: 109.6, s: 9.8, b: 1, t: '(да се пополни читливо со печатни букви)' },
  // applicant
  { p: 1, x: 9.8, y: 137.9, s: 9.8, b: 1, t: 'Од:' },
  { p: 1, x: 18.8, y: 142.0, s: 8.0, b: 0, t: '(име, презиме, датум на раѓање, место на раѓање, место на живеење, матичен број и адреса на законски престој)' },
  // section 1
  { p: 1, x: 9.8, y: 148.3, s: 9.8, b: 1, t: '1. Податоци од возачка дозвола:' },
  { p: 1, x: 9.8, y: 152.7, s: 9.8, b: 1, t: '- возачка дозвола број:' },
  { p: 1, x: 9.8, y: 157.3, s: 9.8, b: 1, t: '- орган кој ја издал:' },
  { p: 1, x: 9.8, y: 162.0, s: 9.8, b: 1, t: '- датум на издавање:' },
  { p: 1, x: 9.8, y: 166.7, s: 9.8, b: 1, t: '- датум на важење:' },
  { p: 1, x: 9.8, y: 171.3, s: 9.8, b: 1, t: '- категорија за која дозволата важи: AM, A1, A2, A, B1, B, BE, C1, C1E, C, CE, D1, D1E, D, DE, F, M и G' },
  { p: 1, x: 9.8, y: 175.2, s: 8.0, b: 0, t: '(заокружија категоријата)' },
  { p: 1, x: 9.8, y: 179.4, s: 9.8, b: 1, t: '- прекршочна санкција забрана за управување со моторно возило за која категорија се однесува' },
  { p: 1, x: 9.8, y: 183.8, s: 9.8, b: 1, t: 'од' },
  { p: 1, x: 40.9, y: 183.8, s: 9.8, b: 1, t: 'до' },
  { p: 1, x: 9.8, y: 189.6, s: 9.8, b: 1, t: '- Ограничување на користење на дозволата' },
  { p: 1, x: 9.8, y: 194.4, s: 8.0, b: 0, t: '(запишете го ограничувањето од возачката дозвола на основа на која се бара меѓународна возачка дозвола)' },
  // section 2
  { p: 1, x: 9.8, y: 202.4, s: 9.8, b: 1, t: '2. Лична карта бр.' },
  { p: 1, x: 73.8, y: 202.4, s: 9.8, b: 1, t: 'издадена од' },
  { p: 1, x: 138.1, y: 202.4, s: 9.8, b: 1, t: 'на ден' },
  // section 3-5
  { p: 1, x: 9.8, y: 208.9, s: 9.8, b: 1, t: '3. Датум и место на поднесување на барањето' },
  { p: 1, x: 9.8, y: 214.5, s: 9.8, b: 1, t: '4. Потпис на подносителот барањето' },
  { p: 1, x: 9.8, y: 219.7, s: 9.8, b: 1, t: '5. Податоци за меѓународна возачка дозвола :' },
  { p: 1, x: 9.8, y: 224.1, s: 9.8, b: 1, t: '- орган кој ја издал меѓународната возачка дозвола (заокружи):' },
  // Page 2
  { p: 2, x: 8.1, y: 44.2, s: 9.8, b: 1, t: 'а) Министерство за внатрешни работи' },
  { p: 2, x: 8.1, y: 48.6, s: 9.8, b: 1, t: 'б) правно лице за вршење на технички преглед на возила областено од Министерство за внатрешни работи' },
  { p: 2, x: 8.1, y: 55.3, s: 9.8, b: 1, t: '- датум на издавање:' },
  { p: 2, x: 8.1, y: 60.0, s: 9.8, b: 1, t: '- сериски број на дозволата:' },
  { p: 2, x: 8.1, y: 64.6, s: 9.8, b: 1, t: '- издадена меѓународна возачка дозвола за категорија на возила: AM, A1, A2, A, B1, B, BE, C1, C1E, C, CE, D1,' },
  { p: 2, x: 8.1, y: 68.6, s: 9.8, b: 1, t: 'D1E, D, DE, F, M и G (заокружи ја категоријата)' },
  { p: 2, x: 8.1, y: 81.2, s: 9.8, b: 1, t: 'Потпис на возачот' },
  { p: 2, x: 8.1, y: 85.6, s: 9.8, b: 1, t: 'Потпис на службеното лице кое го примило барањето' },
  { p: 2, x: 128.5, y: 91.8, s: 9.8, b: 1, t: 'М.П.' },
  { p: 2, x: 8.1, y: 99.5, s: 9.8, b: 1, t: 'ПРИЛОГ КОН БАРАЊЕТО' },
  { p: 2, x: 8.1, y: 103.5, s: 9.8, b: 1, t: '-важечка лична карта' },
  { p: 2, x: 8.1, y: 107.5, s: 9.8, b: 1, t: '-важечка возачка дозвола' },
  { p: 2, x: 8.1, y: 111.4, s: 9.8, b: 1, t: '-2 фотографии 45x35mm' },
  { p: 2, x: 8.1, y: 115.4, s: 9.8, b: 1, t: '-потврда од Министерство за внатрешни работи за прекршочна санкција заврана за управување на' },
  { p: 2, x: 8.1, y: 119.3, s: 9.8, b: 1, t: 'моторно возило (*)' },
  { p: 2, x: 8.1, y: 123.3, s: 9.8, b: 1, t: '-доказ за извршена уплата' },
  { p: 2, x: 8.1, y: 131.2, s: 9.8, b: 1, t: 'Доказите означени со (*) се смета дека се поднесени во прилог на барањето и истите Министерството' },
  { p: 2, x: 8.1, y: 135.1, s: 9.8, b: 1, t: 'за внатрешни работи ги прибавува по службена должност.' },
  { p: 2, x: 8.1, y: 148.3, s: 9.8, b: 1, t: 'За точноста на податоците одговарам лично и материјално, а за сите грешки во податоците' },
  { p: 2, x: 8.1, y: 152.2, s: 9.8, b: 1, t: 'согласен/на сум да ги сносам последиците.' },
  { p: 2, x: 65.2, y: 166.2, s: 9.8, b: 0, t: 'година' },
];

// Fill-in underlines (mm): pixel-scanned from the 2x render of the legacy PDF; the few
// hairline (0-height) rules the scan can't see are derived as baseline + 1.25mm.
type U = { p: number; x: number; y: number; w: number };
const UNDER: U[] = [
  { p: 1, x: 89.8, y: 67.8, w: 78.2 },    // org name
  { p: 1, x: 18.35, y: 139.32, w: 181.6 },// applicant line
  { p: 1, x: 54.35, y: 154.15, w: 55.6 }, // licence number
  { p: 1, x: 47.65, y: 158.74, w: 62.3 }, // licence issuer
  { p: 1, x: 47.65, y: 163.51, w: 62.3 }, // licence issue date
  { p: 1, x: 44.47, y: 168.1, w: 65.5 },  // licence valid date
  { p: 1, x: 14.9, y: 185.05, w: 25.5 },  // од
  { p: 1, x: 46.1, y: 185.05, w: 34.1 },  // до
  { p: 1, x: 89.0, y: 190.85, w: 111.2 }, // ограничување
  { p: 1, x: 42.71, y: 203.94, w: 29.8 }, // ID card no
  { p: 1, x: 97.41, y: 203.94, w: 40.1 }, // ID card issuer
  { p: 1, x: 150.53, y: 203.94, w: 36.7 },// ID card date
  { p: 1, x: 91.94, y: 210.48, w: 103.2 },// submission date + place (continuous rule)
  { p: 1, x: 76.0, y: 215.75, w: 73.4 },  // signature
  { p: 2, x: 47.65, y: 56.68, w: 43.6 },  // intl issue date
  { p: 2, x: 66.0, y: 61.27, w: 43.6 },   // serial
  { p: 2, x: 44.1, y: 82.45, w: 48.2 },   // driver signature
  { p: 2, x: 104.7, y: 86.85, w: 53.3 },  // official signature
  { p: 2, x: 7.6, y: 93.1, w: 186.7 },    // (rule)
  { p: 2, x: 7.59, y: 167.75, w: 84.35 }, // place + date + година (continuous rule)
];

// Data fields (bindings) — printed as stored (Cyrillic). Static layout per field: page,
// bold, and DEFAULT x/y/size. Only x/y/size are editable — routed through lay.resolve(key).
// p and b are fixed. The DEFAULT numbers below are the EXACT original coordinates.
type DField = { k: string; p: number; x: number; y: number; s: number; b: 0 | 1 };
const DATA_FIELDS: DField[] = [
  { k: 'companyName',           p: 1, x: 91.2,  y: 66.4,  s: 9.8, b: 1 }, // legal-entity name in header
  { k: 'applicantLine',         p: 1, x: 18.8,  y: 137.9, s: 9.8, b: 1 }, // applicant line
  { k: 'nationalLicenceNumber', p: 1, x: 54.8,  y: 152.9, s: 9.8, b: 0 },
  { k: 'nationalLicenceIssuer', p: 1, x: 48.2,  y: 157.6, s: 9.8, b: 0 },
  { k: 'nationalLicenceDate',   p: 1, x: 48.2,  y: 162.3, s: 9.8, b: 0 },
  { k: 'nationalLicenceExpiry', p: 1, x: 45.0,  y: 166.9, s: 9.8, b: 0 },
  { k: 'idCardNumber',          p: 1, x: 43.1,  y: 202.7, s: 9.8, b: 0 },
  { k: 'idCardIssuer',          p: 1, x: 97.9,  y: 202.7, s: 9.8, b: 0 },
  { k: 'idCardDate',            p: 1, x: 151.0, y: 202.7, s: 9.8, b: 0 },
  { k: 'submissionDate',        p: 1, x: 92.3,  y: 208.9, s: 9.8, b: 0 },
  { k: 'submissionPlace',       p: 1, x: 136.2, y: 208.9, s: 9.8, b: 1 }, // place (bold in the original)
  { k: 'intlIssueDate',         p: 2, x: 48.2,  y: 55.3,  s: 9.8, b: 0 },
  { k: 'intlSerialNumber',      p: 2, x: 66.4,  y: 60.0,  s: 9.8, b: 0 },
  { k: 'footerPlace',           p: 2, x: 8.1,   y: 166.2, s: 9.8, b: 0 },
  { k: 'footerDate',            p: 2, x: 35.4,  y: 166.2, s: 9.8, b: 0 },
];

// Register the built-in defaults (mm + font pt) with the saved-layout system.
const DEF: Record<string, { x: number; y: number; size?: number }> = {};
for (const f of DATA_FIELDS) DEF[f.k] = { x: f.x, y: f.y, size: f.s };
lay.setDefaults(DEF);

// Per-field text values (key → printed string), computed from the bound bundle.
const dataValues = computed<Record<string, string>>(() => {
  const b = bundle.value;
  if (!b) return {} as Record<string, string>;
  const r: Record<string, string> = {
    companyName:           (b.companyName ?? '').trim(),
    applicantLine:         applicantLine.value,
    nationalLicenceNumber: b.numberOfNationalLicence,
    nationalLicenceIssuer: tl(b.nationalLicenceIssuerName),
    nationalLicenceDate:   fmt(b.nationalLicenceDate, true),
    nationalLicenceExpiry: fmt(b.nationalLicenceExpiry, true),
    idCardNumber:          b.idCardNumber ?? '',
    idCardIssuer:          tl(b.idCardIssuerName),
    idCardDate:            fmt(b.idCardDate, false),
    submissionDate:        fmt(b.issuedDate, false),
    submissionPlace:       b.stationCityName ?? '',
    intlIssueDate:         fmt(b.issuedDate, false),
    intlSerialNumber:      b.numberOfLicence,
    footerPlace:           b.stationCityName ?? '',
    footerDate:            fmt(b.issuedDate, false),
  };
  return r;
});

function staticsFor(pg: number): T[] { return STATIC.filter(i => i.p === pg); }
function dataFor(pg: number): DField[] { return DATA_FIELDS.filter(f => f.p === pg); }
function undFor(pg: number): U[] { return UNDER.filter(u => u.p === pg); }
// The extracted y coordinates are text BASELINES (pdf text-matrix), not box tops — anchor
// the CSS box so glyphs sit just above the fill-in lines (shift up ~0.30 × font-size).
function tStyle(i: T) { return `left:${i.x}mm; top:${(i.y - i.s * 0.30).toFixed(2)}mm; font-size:${i.s}pt; font-weight:${i.b ? 700 : 400};`; }
// Editable value fields: x/y/size come from resolve(key); bold stays fixed. Same baseline shift.
function dStyle(f: DField) {
  const p = lay.resolve(f.k);
  return `left:${p.x}mm; top:${(p.y - p.size * 0.30).toFixed(2)}mm; font-size:${p.size}pt; font-weight:${f.b ? 700 : 400};`;
}
function uStyle(u: U) { return `left:${u.x}mm; top:${u.y}mm; width:${u.w}mm;`; }

// Representative sample used in layout-edit mode (no real record needed).
const SAMPLE: IdlRequestPrint = {
  id: 0,
  clientFullName: 'ПЕТАР ПЕТРОВСКИ',
  dateOfBirth: '1985-03-12',
  birthCityName: 'Велес',
  citizenshipName: 'Македонско',
  numberOfNationalLicence: 'ВЕ1234567',
  numberOfLicence: 'МК-000123',
  passportNumber: 'A1234567',
  passportIssuerName: 'МВР Велес',
  passportDate: '2020-05-01',
  idCardNumber: 'A0987654',
  idCardIssuerName: 'МВР Велес',
  idCardDate: '2019-09-20',
  nationalLicenceIssuerName: 'МВР Велес',
  nationalLicenceDate: '2018-04-10',
  nationalLicenceExpiry: '2028-04-10',
  livingAddress: 'Браќа Миладиновци 21',
  livingCityName: 'Велес',
  embg: '1203985450012',
  issuerOrgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС',
  companyName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС',
  stationCityName: 'Велес',
  issuedDate: '2026-06-15',
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
    const { data } = await api.get<IdlRequestPrint>(`/international-driving-licences/${props.id}/print`);
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
    <!-- toolbar kept BEFORE the pages so .page:last-of-type matches the real last page -->
    <div v-if="!lay.editing.value" class="toolbar no-print">
      <button @click="doPrint">Печати</button>
      <button @click="doClose">Затвори</button>
    </div>

    <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Меѓународна дозвола — барање'" v-model:guides="guides" />

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <template v-else-if="bundle">
      <div v-for="pg in [1, 2]" :key="pg" class="page"
           :ref="(el) => { if (pg === 1) lay.setPageEl(el as HTMLElement | null); }"
           :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
        <!-- underline rules (fixed) -->
        <div v-for="(u, i) in undFor(pg)" :key="'u' + i" class="u" :style="uStyle(u)"></div>
        <!-- static form text (fixed) -->
        <div v-for="(it, i) in staticsFor(pg)" :key="'s' + i" class="t" :style="tStyle(it)">{{ it.t }}</div>
        <!-- bound VALUES (editable — position/size from saved layout) -->
        <div v-for="f in dataFor(pg)" :key="f.k" class="t f" :class="{ sel: lay.selectedKey.value === f.k }"
             :style="dStyle(f)" @pointerdown="lay.beginDrag(f.k, $event)">{{ dataValues[f.k] }}</div>
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
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.t { position: absolute; white-space: nowrap; line-height: 1; }
.u { position: absolute; height: 0; border-bottom: 0.2mm solid #000; }

/* ---- Layout-edit affordances (screen only; only .f value fields are editable) ---- */
.page.editing .f { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.page.editing .f:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.page.editing .f.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
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
  .page { margin: 0; box-shadow: none; page-break-after: always; }
  .page:last-of-type { page-break-after: auto; }
  .no-print { display: none !important; }
  @page { size: A4; margin: 0; }
}
</style>
