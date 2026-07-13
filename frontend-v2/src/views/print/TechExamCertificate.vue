<script setup lang="ts">
import { nextTick, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { TechExamCertificate } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const cert = ref<TechExamCertificate | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; A4 width 210mm) ----
// This certificate is a SEMANTIC FLOW document (letterhead, title, table, signatures)
// that prints on blank A4. The flow is the visual base and is NEVER torn apart. Only a
// small set of standalone VALUE fields are made admin-repositionable: in NON-edit mode
// they render EXACTLY as the original inline flow (so real print output stays
// byte-identical); in edit mode (?edit=1) each becomes a draggable absolutely-positioned
// overlay driven by lay.resolve(key). Defaults sit roughly where the value renders today.
const lay = usePrintLayout('techexam-cert', 210);
const guides = ref(true);

// Editable value keys:
//   regNumber → the "Број:" certificate/reg number line (празна линија — број се пишува)
//   validTill → the validity ("нареден технички преглед на") date
//   placeDate → the signature-left "Место и датум:" line (issue date + place)
//
// Бидејќи ова е FLOW документ (нема вградени координати), стандардните позиции се
// МЕРАТ од реално рендерираниот документ при отворање на едиторот — така едит-режимот
// почнува со вредностите точно таму каде што се печатат. Мерењето е во mm (CSS mm =
// исти единици со кои се позиционираат overlay-ите), па се совпаѓа на секој екран.
const measured = ref(false);
const paperRef = ref<HTMLElement | null>(null);
function setPaper(el: HTMLElement | null) { paperRef.value = el; lay.setPageEl(el); }

function rectMm(r: DOMRect, paper: DOMRect): { x: number; y: number } {
  const k = 210 / paper.width;   // A4 width in mm / rendered px
  return { x: (r.left - paper.left) * k, y: (r.top - paper.top) * k };
}
function textRect(el: Element | null): DOMRect | null {
  if (!el) return null;
  const tn = [...el.childNodes].find(n => n.nodeType === Node.TEXT_NODE && (n.textContent ?? '').trim());
  if (!tn) return el.getBoundingClientRect();
  const range = document.createRange();
  range.selectNodeContents(tn);
  return range.getBoundingClientRect();
}
async function measureDefaults() {
  await nextTick();
  const paper = paperRef.value;
  if (!paper) return;
  const pr = paper.getBoundingClientRect();
  const def: Record<string, { x: number; y: number; size?: number }> = {};

  // Датумите: точната позиција на текстот во токот.
  const vt = textRect(paper.querySelector('.date-underline'));
  if (vt) { const p = rectMm(vt, pr); def.validTill = { x: p.x, y: p.y, size: 12 }; }
  const pd = textRect(paper.querySelector('.sig-value'));
  if (pd) { const p = rectMm(pd, pr); def.placeDate = { x: p.x, y: p.y, size: 12 }; }
  // Бројот: празна линија — постави го текстот на самата линија.
  const fl = paper.querySelector('.fill-line')?.getBoundingClientRect();
  if (fl) {
    const p = rectMm(fl, pr);
    const k = 210 / pr.width;
    def.regNumber = { x: p.x + 2, y: p.y + fl.height * k - 4.6, size: 12 };
  }
  lay.setDefaults(def);
  measured.value = true;
}

function fStyle(key: string): string {
  const p = lay.resolve(key);
  return `left:${p.x}mm; top:${p.y}mm; font-size:${p.size}pt;`;
}
// A value renders as a positioned overlay (instead of its inline flow spot) when it is
// being edited (once measured) OR has a saved position — so admin edits actually move it
// on the PRINT, while the untouched default keeps the original flow layout (byte-identical).
function hasOverride(key: string): boolean { return !!lay.overrides.value[key]; }
function overlaid(key: string): boolean {
  return lay.editing.value ? measured.value : hasOverride(key);
}

// Representative sample values used in layout-edit mode (no real record needed).
const SAMPLE: TechExamCertificate = {
  id: 0,
  regNumber: '0123/2026',
  madeDate: '2026-07-10',
  validTillDate: '2027-07-10',
  vehicleIsRight: true,
  stationCity: 'Велес',
  controllerName: 'ПЕТАР ПЕТРОВСКИ',
  organization: {
    name: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ',
    address: 'ул. Димитар Влахов бр. 12',
    cityLine: '1400 Велес',
    phone: '043/234-567',
    fax: '043/234-568',
  },
  vehicle: {
    registration: 'VE-1234-AB',
    category: 'Патничко возило',
    maker: 'VOLKSWAGEN',
    typeText: 'GOLF',
    model: '1.6 TDI',
    vin: 'WVWZZZ1KZAW000000',
  },
};

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '—';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}.${mm}.${d.getFullYear()}`;
}

function placeDateText(c: TechExamCertificate): string {
  return [c.stationCity, fmtDate(c.madeDate)].filter(Boolean).join(', ');
}

onMounted(async () => {
  await lay.load();
  if (lay.editing.value) {
    // Layout-edit mode: sample data, never auto-print. Прво се рендерира токот со
    // видливи вредности, се мерат нивните реални позиции, па се вклучуваат overlay-ите.
    cert.value = SAMPLE;
    loading.value = false;
    await measureDefaults();
    return;
  }
  try {
    const { data } = await api.get<TechExamCertificate>(`/technical-exams/${props.id}/print`);
    cert.value = data;
    // Auto-open the print dialog only for a compliant vehicle (legacy issues the
    // certificate only when VehicleIsRight = true).
    if (data.vehicleIsRight) setTimeout(() => window.print(), 250);
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

    <div v-else-if="cert" :ref="(el) => setPaper(el as HTMLElement | null)"
         class="paper" :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
      <!-- Letterhead -->
      <div class="letterhead">
        <div class="org-name">{{ (cert.organization.name || '').toUpperCase() }}</div>
        <div class="org-line" v-if="cert.organization.address">{{ cert.organization.address }}</div>
        <div class="org-line" v-if="cert.organization.cityLine">{{ cert.organization.cityLine }}</div>
        <div class="org-line" v-if="cert.organization.phone || cert.organization.fax">
          <span v-if="cert.organization.phone">Тел: {{ cert.organization.phone }}</span>
          <span v-if="cert.organization.fax">&nbsp;&nbsp;&nbsp;Факс: {{ cert.organization.fax }}</span>
        </div>
      </div>

      <p v-if="!cert.vehicleIsRight" class="warn no-print">
        Возилото не е технички исправно — потврда не се издава.
      </p>

      <!-- Preamble -->
      <p class="preamble">
        Врз основа на член 48 став 3 и 5 и член 67 став 1 алинеи 1 и 2, а во врска со член 52 став 2
        од Законот за возила се издава
      </p>

      <!-- Title -->
      <h1 class="title">ПОТВРДА</h1>
      <!-- Бројот: празна линија за рачно пишување — линијата останува секогаш;
           бројот се печати врз неа само ако админот зачувал позиција. -->
      <div class="reg-no">Број: <span class="fill-line"></span></div>

      <p class="descriptor">за извршен преглед на техничка исправност на моторното возило:</p>

      <!-- Vehicle table -->
      <table class="vehicle-table">
        <tr><td class="code">A</td>  <td class="lbl">Регистарска ознака на возилото:</td><td class="val">{{ cert.vehicle.registration || '—' }}</td></tr>
        <tr><td class="code">Ј</td>  <td class="lbl">Категорија и вид на возилото:</td>  <td class="val">{{ cert.vehicle.category || '—' }}</td></tr>
        <tr><td class="code">D.1</td><td class="lbl">Марка:</td>                          <td class="val">{{ cert.vehicle.maker || '—' }}</td></tr>
        <tr><td class="code">D.2</td><td class="lbl">Тип/варијанта/изведба:</td>          <td class="val">{{ cert.vehicle.typeText || '—' }}</td></tr>
        <tr><td class="code">D.3</td><td class="lbl">Комерцијална ознака:</td>            <td class="val">{{ cert.vehicle.model || '—' }}</td></tr>
        <tr><td class="code">E</td>  <td class="lbl">Идентификационен број на возилото (шасија/поставеност)</td><td class="val">{{ cert.vehicle.vin || '—' }}</td></tr>
      </table>

      <!-- Compliance statement -->
      <p class="statement">
        Потврдуваме дека горенаведеното возило ги исполнува условите пропишани со Правилникот за
        технички преглед на возила и е технички исправно.
      </p>

      <!-- Next exam -->
      <p class="next-exam">
        Возилото треба да го изврши наредниот технички преглед на
        <span class="date-underline" :class="{ ghost: overlaid('validTill') }">{{ fmtDate(cert.validTillDate) }}</span>
      </p>
      <p class="next-exam-caption">(датум на нареден технички преглед)</p>

      <!-- Signatures -->
      <div class="signatures">
        <div class="sig-left">
          <div>Место и датум:
            <span class="sig-value" :class="{ ghost: overlaid('placeDate') }">{{ placeDateText(cert) }}</span>
          </div>
        </div>
        <div class="sig-right">
          <!-- Без име — се потпишува рачно на празната линија. -->
          <div class="sig-line"></div>
          <div class="sig-label">Потпис и печат:</div>
        </div>
      </div>

      <!-- ===== Positioned value overlays. Shown while editing OR when a saved position
               exists — so admin edits move the value on the actual print too. ===== -->
      <span v-if="overlaid('regNumber')" class="f fbold" :class="{ sel: lay.selectedKey.value === 'regNumber' }"
            :style="fStyle('regNumber')" @pointerdown="lay.beginDrag('regNumber', $event)">{{ cert.regNumber || '' }}</span>
      <span v-if="overlaid('validTill')" class="f fbold" :class="{ sel: lay.selectedKey.value === 'validTill' }"
            :style="fStyle('validTill')" @pointerdown="lay.beginDrag('validTill', $event)">{{ fmtDate(cert.validTillDate) }}</span>
      <span v-if="overlaid('placeDate')" class="f fbold" :class="{ sel: lay.selectedKey.value === 'placeDate' }"
            :style="fStyle('placeDate')" @pointerdown="lay.beginDrag('placeDate', $event)">{{ placeDateText(cert) }}</span>

      <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Потврда за техничка исправност'" v-model:guides="guides" />

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
  width: 210mm; min-height: 297mm; padding: 16mm 18mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: "Times New Roman", Georgia, serif; font-size: 12pt; line-height: 1.45;
  box-shadow: 0 0 12px rgba(0,0,0,.15);
}
.letterhead {
  border: 2px solid #000; padding: .6rem 1rem; text-align: center; margin-bottom: 1.6rem;
}
.org-name { font-family: Arial, sans-serif; font-weight: 700; font-size: 14pt; }
.org-line { font-family: Arial, sans-serif; font-weight: 700; font-size: 11pt; }
.preamble { text-align: center; margin: 0 0 1.2rem; }
.title { text-align: center; font-size: 18pt; font-weight: 700; margin: 0 0 .2rem; letter-spacing: 1px; }
.reg-no { text-align: center; margin-bottom: 1.4rem; }
.fill-line { display: inline-block; width: 42mm; border-bottom: 1px solid #000; vertical-align: baseline; }
.descriptor { text-align: center; margin: 0 0 .8rem; }
.vehicle-table { width: 100%; border-collapse: collapse; margin-bottom: 1.6rem; }
.vehicle-table td { border: 1px solid #000; padding: .3rem .5rem; vertical-align: middle; }
.vehicle-table .code { width: 36px; text-align: center; font-weight: 700; }
.vehicle-table .lbl { width: 55%; }
.vehicle-table .val { font-weight: 700; }
.statement { text-align: justify; margin: 0 0 2rem; }
.next-exam { margin: 0 0 .1rem; }
.date-underline { border-bottom: 1px solid #000; padding: 0 1.5rem; font-weight: 700; }
/* Преместена вредност: текстот е проѕирен во токот (линијата ја задржува ширината),
   а се исцртува преку позиционираниот overlay. */
.ghost { color: transparent !important; }
.next-exam-caption { font-size: 8pt; font-style: italic; margin: 0 0 3rem; }
.signatures { display: grid; grid-template-columns: 1fr 1fr; gap: 2rem; margin-top: 2.5rem; align-items: end; }
.sig-value { font-weight: 700; }
.sig-right { text-align: center; }
.sig-name { font-weight: 700; margin-bottom: .15rem; }
.sig-line { border-top: 1px solid #000; margin: 0 1rem .25rem; }
.sig-label { font-size: 11pt; }
.warn { color: #b91c1c; text-align: center; font-weight: 700; border: 1px solid #b91c1c; padding: .4rem; border-radius: 4px; }
.loading, .error { text-align: center; padding: 4rem 0; font-family: system-ui, sans-serif; }
.error { color: #b91c1c; }
.toolbar.no-print { position: fixed; right: 1rem; top: 1rem; display: flex; gap: .5rem; z-index: 999; }
.toolbar.no-print button { padding: .35rem .75rem; border: 1px solid #888; background: #fff; border-radius: 4px; cursor: pointer; font-size: .85rem; }

/* ---- Layout-edit affordances (screen only) — draggable value overlays ---- */
.f { position: absolute; white-space: nowrap; line-height: 1; }
.fbold { font-weight: 700; }
.paper.editing .f { cursor: move; outline: 1px dashed rgba(37,99,235,.4); outline-offset: 0; }
.paper.editing .f:hover { outline-color: rgba(37,99,235,.9); background: rgba(37,99,235,.06); }
.paper.editing .f.sel { outline: 1.5px solid #2563eb; background: rgba(37,99,235,.12); }
.paper.guides {
  background-image:
    repeating-linear-gradient(0deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm),
    repeating-linear-gradient(90deg, transparent 0, transparent calc(10mm - 1px), rgba(37,99,235,.12) 10mm);
}

@media print {
  .print-wrap { background: #fff; padding: 0; }
  .paper { box-shadow: none; margin: 0; }
  .no-print { display: none !important; }
  @page { size: A4; margin: 14mm; }
}
</style>
