<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { ReceiptPrint } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const bundle = ref<ReceiptPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; A4 LANDSCAPE, width 297mm) ----
const lay = usePrintLayout('smetko', 297);
const guides = ref(true);

// ── СМЕТКОПОТВРДА — the legacy landscape receipt, TWO IDENTICAL COPIES side by
// side (right copy = left + 150.3mm). Geometry extracted from the live legacy
// print (СМЕТКОПОТВРДА.pdf) via pdfjs: text baselines, sizes and the dashed
// rules. Line money comes precomputed from the API (legacy FicalRound). ──

const COPY_OFFSET = 150.3;

function fmtD(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth() + 1).padStart(2, '0')}.${d.getFullYear()}`;
}
// Totals: legacy MK format — dot thousands, one comma decimal ("3.368,0").
function fmtT(v: number): string {
  const [int, dec] = v.toFixed(1).split('.');
  return int.replace(/\B(?=(\d{3})+(?!\d))/g, '.') + ',' + dec;
}
const fmtI = (v: number) => String(Math.round(v));

// Built-in default positions (mm from the PDF) for the editable VALUE fields.
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  orgName:       { x: 5.6,   y: 3.5,   size: 12 },
  orgTax:        { x: 110.9, y: 7.1,   size: 8 },
  orgAddress:    { x: 5.6,   y: 15.3,  size: 8 },
  orgPhone:      { x: 102.0, y: 15.3,  size: 8 },
  broj:          { x: 58.5,  y: 20.4,  size: 12 },
  datum:         { x: 119.9, y: 20.6,  size: 10 },
  nachin:        { x: 67.0,  y: 26.9,  size: 9.75 },
  clientName:    { x: 7.7,   y: 41.5,  size: 8 },
  clientAddress: { x: 7.7,   y: 45.7,  size: 8 },
  clientCityUp:  { x: 7.7,   y: 49.9,  size: 8 },
  clientCity:    { x: 45.8,  y: 49.8,  size: 8.25 },
  vid:           { x: 17.1,  y: 63.6,  size: 9 },
  plate:         { x: 109.4, y: 63.4,  size: 9.75 },
  makerModel:    { x: 31.6,  y: 68.0,  size: 9 },
  capacity:      { x: 124.2, y: 67.8,  size: 9.75 },
  vin:           { x: 29.5,  y: 72.2,  size: 9.75 },
  power:         { x: 119.9, y: 72.2,  size: 9.75 },
  engineNo:      { x: 37.3,  y: 76.6,  size: 9.75 },
  carry:         { x: 109.4, y: 76.6,  size: 9.75 },
};
lay.setDefaults(DEF);

// Static labels per copy: x, y(top), text, size.
type S = { x: number; y: number; t: string; size?: number };
const LABELS: S[] = [
  { x: 11.7,  y: 20.4,  t: 'СМЕТКОПОТВРДА бр.', size: 12 },
  { x: 107.4, y: 20.6,  t: 'Датум:', size: 10 },
  { x: 37.3,  y: 26.9,  t: 'начин на плаќање:' },
  { x: 7.7,   y: 31.7,  t: 'Сопственик', size: 12 },
  { x: 7.7,   y: 54.0,  t: 'Возило', size: 12 },
  { x: 7.7,   y: 63.9,  t: 'ВИД:', size: 8.25 },
  { x: 88.2,  y: 63.9,  t: 'РЕГ.ОЗНАКА:', size: 8.25 },
  { x: 7.7,   y: 68.3,  t: 'МАРКА И ТИП:', size: 8.25 },
  { x: 88.2,  y: 68.3,  t: 'РАБОТНА ЗАФАТНИНА:', size: 8.25 },
  { x: 7.7,   y: 72.7,  t: 'БР. ШАСИЈА:', size: 8.25 },
  { x: 88.2,  y: 72.7,  t: 'СИЛА НА МОТОРОТ:', size: 8.25 },
  { x: 7.7,   y: 77.1,  t: 'БРОЈ НА МОТОР:', size: 8.25 },
  { x: 88.2,  y: 77.1,  t: 'НОСИВОСТ:', size: 8.25 },
];
// Dashed rules per copy (x, y, w) — SITE линии со ист почеток (5.6) и крај (144.9);
// табеларните/footer линиите го добиваат истиот распон преку .tbl контејнерот.
const RULE_X = 5.6;
const RULE_W = 139.3;
const RULES = [
  { x: RULE_X, y: 40.6, w: RULE_W },
  { x: RULE_X, y: 62.9, w: RULE_W },
];

function vals(b: ReceiptPrint) {
  return {
    orgName: b.orgName ?? '',
    orgTax: b.orgTaxNumber ?? '',
    orgAddress: b.orgAddress ?? '',
    orgPhone: b.orgPhone ? `тел: ${b.orgPhone}` : '',
    broj: b.documentNumber,
    datum: fmtD(b.issueDate),
    nachin: b.paymentTypeName ?? '',
    clientName: b.clientName ?? '',
    clientAddress: b.clientAddress ?? '',
    clientCityUp: (b.clientCityName ?? '').toUpperCase(),
    clientCity: b.clientCityName ?? '',
    vid: b.vehicleCategoryLabel ?? '',
    plate: b.plate ?? '',
    makerModel: b.makerModel ?? '',
    capacity: b.workingCapacityCc != null ? fmtI(b.workingCapacityCc) : '',
    vin: b.vin ?? '',
    power: b.powerKw != null ? fmtI(b.powerKw) : '',
    engineNo: b.engineNumber ?? '',
    carry: fmtI(b.carryKg),
  } as Record<string, string>;
}

function vStyle(k: string) {
  const p = lay.resolve(k);
  const wrap = k === 'orgName' ? 'white-space:normal; width:100mm; line-height:4.9mm;' : 'white-space:nowrap;';
  return `left:${p.x.toFixed(2)}mm; top:${p.y.toFixed(2)}mm; font-size:${p.size}pt; ${wrap}`;
}
function sStyle(s: S) {
  return `left:${s.x}mm; top:${s.y}mm; font-size:${s.size ?? 9.75}pt;`;
}

// Representative sample used in layout-edit mode (mirrors the reference PDF).
const SAMPLE: ReceiptPrint = {
  id: 1, documentNumber: '01-37-47329/2026', issueDate: '2026-07-15',
  paymentTypeName: 'со кредитна картичка',
  orgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС-ВЕЛЕС', orgTaxNumber: 'ЕДБ МК4004014510915',
  orgAddress: 'АСНОМ 13', orgPhone: '072-224-518   070-355-222',
  clientName: 'МИТКОВ МИЛАН', clientAddress: 'УЛ. ГАНЧО ХАЏИ - ПАНЗОВ БР.19', clientCityName: 'Велес',
  vehicleCategoryLabel: 'M1 ПАТНИЧКО ВОЗИЛО', plate: 'VE 6433 AE', makerModel: 'VOLKSWAGEN, GOLF',
  workingCapacityCc: 1390, vin: 'WVWZZZ1KZBW208901', powerKw: 118, engineNumber: '273442', carryKg: 0,
  lines: [
    { name: 'Административна услуга', bezDdv: 424, popust: 0, ddv: 76, cena: 500 },
    { name: 'Буџет на РМ', bezDdv: 100, popust: 0, ddv: 0, cena: 100 },
    { name: 'Надоместок за животна средина', bezDdv: 450, popust: 0, ddv: 0, cena: 450 },
    { name: 'Надоместок за Комунална такса', bezDdv: 100, popust: 0, ddv: 0, cena: 100 },
    { name: 'Оперативни трошоци', bezDdv: 300, popust: 0, ddv: 54, cena: 354 },
    { name: 'Републички совет за безбедност', bezDdv: 22, popust: 0, ddv: 0, cena: 22 },
    { name: 'Републички совет за безбедност од јавни патишта', bezDdv: 22, popust: 0, ddv: 0, cena: 22 },
    { name: 'Технички преглед', bezDdv: 1441, popust: 0, ddv: 259, cena: 1700 },
    { name: 'Црвен крст', bezDdv: 120, popust: 0, ddv: 0, cena: 120 },
  ],
  totalBezDdv: 2979, totalDdv: 389, total: 3368,
  referentName: 'Елена Андоновска', note: null, paid: true, stornoed: false,
};

onMounted(async () => {
  await lay.load();
  if (lay.editing.value) {
    bundle.value = SAMPLE;
    loading.value = false;
    return;
  }
  try {
    const { data } = await api.get<ReceiptPrint>(`/payment-documents/${props.id}/receipt-print`);
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

    <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Сметкопотврда'" v-model:guides="guides" />

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <div v-else-if="bundle" class="page" :ref="(el) => lay.setPageEl(el as HTMLElement | null)"
         :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="297" :height-mm="210" />

      <div v-for="(off, ci) in [0, COPY_OFFSET]" :key="ci" class="copy" :style="`left:${off}mm;`">
        <div v-for="(s, i) in LABELS" :key="'s' + i" class="t" :style="sStyle(s)">{{ s.t }}</div>
        <div v-for="(r, i) in RULES" :key="'r' + i" class="dash" :style="`left:${r.x}mm; top:${r.y}mm; width:${r.w}mm;`"></div>

        <div v-for="(txt, k) in vals(bundle)" :key="k" class="t val" :class="{ ed: ci === 0, sel: ci === 0 && lay.selectedKey.value === k }"
             :style="vStyle(k as string)"
             @pointerdown="ci === 0 && lay.beginDrag(k as string, $event)">{{ txt }}</div>

        <!-- Table: header + rows + totals, flow (rows may wrap) -->
        <div class="tbl">
          <!-- header labels на точните PDF позиции (независни од row-гридот) -->
          <div class="tbl-head">
            <span class="h" style="left:2.1mm; font-size:11.25pt;">НАЗИВ НА УСЛУГА</span>
            <span class="h" style="left:58.5mm;">ЦЕНА БЕЗ ДДВ</span>
            <span class="h" style="left:88.6mm;">Попуст</span>
            <span class="h" style="left:109mm;">ДДВ</span>
            <span class="h" style="left:126.2mm;">ЦЕНА</span>
          </div>
          <div v-for="(l, i) in bundle.lines" :key="i" class="tbl-row">
            <span class="c-n">{{ i + 1 }}</span>
            <span class="c-name">{{ l.name }}</span>
            <span class="c-num">{{ fmtI(l.bezDdv) }}</span>
            <span class="c-num">{{ fmtI(l.popust) }}</span>
            <span class="c-num">{{ fmtI(l.ddv) }}</span>
            <span class="c-num">{{ fmtI(l.cena) }}</span>
          </div>
          <div class="tbl-total">
            <span class="tt-label">ВКУПНО:</span>
            <span class="tt-bez">{{ fmtT(bundle.totalBezDdv) }} ден.</span>
            <span class="tt-ddv">{{ fmtT(bundle.totalDdv) }} ден</span>
            <span class="tt-all">{{ fmtT(bundle.total) }} ден.</span>
          </div>
          <!-- Footer flows под табелата (не се судира со долги сметки) -->
          <div class="ftr-sign">
            <span class="fs-left">ПРИМИЛ УСЛУГА:</span>
            <span class="fs-right">РЕФЕРЕНТ: <b>{{ bundle.referentName ?? '' }}</b></span>
          </div>
          <div class="ftr-lines"><span class="fl-left"></span><span class="fl-right"></span></div>
          <div class="ftr-thanks">БЛАГОДАРИМЕ ЗА ДОВЕРБАТA</div>
          <div class="ftr-note">Забелешка: {{ bundle.note ?? '' }}</div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.screen { background: #e0e0e0; min-height: 100vh; padding: 8mm 0; }
.page {
  position: relative;
  width: 297mm; height: 210mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: Arial, "Helvetica Neue", sans-serif;
  font-weight: 400; font-size: 9.75pt;
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.copy { position: absolute; top: 0; width: 146.4mm; height: 100%; }
.t { position: absolute; white-space: nowrap; }
.dash { position: absolute; height: 0; border-bottom: 0.25mm dashed #000; }

/* Table (flow) — column edges match the PDF: №@5.6, name@12, right edges 85.5/101.7/118.1/139.3.
   Container width = 139.3mm so header/totals/footer rules END at 144.9 — исто со RULES. */
.tbl { position: absolute; left: 5.6mm; top: 82.7mm; width: 139.3mm; }
.tbl-row, .tbl-total {
  display: grid;
  grid-template-columns: 6.4mm 64mm 9.5mm 16.2mm 16.4mm 21.2mm;
  column-gap: 0;
  align-items: baseline;
}
.tbl-head {
  position: relative; height: 6.3mm;
  border-top: 0.25mm dashed #000; border-bottom: 0.25mm dashed #000;
}
.tbl-head .h { position: absolute; bottom: 0.7mm; white-space: nowrap; font-size: 9.75pt; }
.tbl-row { padding-top: 2.1mm; }  /* row pitch ≈ 6.4mm, as in the PDF */
.tbl-row .c-name { white-space: normal; padding-right: 2mm; }
.c-num { text-align: right; }
.tbl-total {
  /* right edges: без ддв 85.9 · ддв 117.3 · вкупно 139.3 (container-relative, per the PDF) */
  grid-template-columns: 6.4mm 50mm 29.5mm 0 31.4mm 22mm;
  border-top: 0.25mm dashed #000;
  margin-top: 2.2mm; padding-top: 1.6mm;
}
.tt-label { grid-column: 2; }
.tt-bez { grid-column: 3; text-align: right; }
.tt-ddv { grid-column: 5; text-align: right; }
.tt-all { grid-column: 6; text-align: right; }

/* Footer (flow, mirrors the PDF spacing under the totals) */
.ftr-sign { display: flex; justify-content: space-between; margin-top: 4.2mm; padding: 0 2.1mm 0 2.1mm; }
.ftr-sign .fs-right { margin-right: 2mm; }
/* потписните линии: левата почнува на работ (5.6), десната завршува на работ (144.9) */
.ftr-lines { display: flex; justify-content: space-between; margin-top: 4.4mm; }
.ftr-lines .fl-left { width: 57mm; border-bottom: 0.25mm dashed #000; }
.ftr-lines .fl-right { width: 72mm; border-bottom: 0.25mm dashed #000; }
.ftr-thanks { margin-top: 2.6mm; padding-left: 2.1mm; font-size: 9pt; }
.ftr-note { margin-top: 1.2mm; padding-left: 3.4mm; }

/* ---- Layout-edit affordances (screen only); values on COPY 1 are movable ---- */
.page.editing .val.ed { cursor: move; outline: 1px dashed rgba(37,99,235,.4); }
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
  .page { margin: 0; box-shadow: none; overflow: visible; }
  .no-print { display: none !important; }
  @page { size: A4 landscape; margin: 0; }
}
</style>
