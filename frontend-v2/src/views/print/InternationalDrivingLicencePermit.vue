<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { IdlPermitPrint } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{ id: string }>();

const bundle = ref<IdlPermitPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

// ---- Saved-layout system (positions in mm; custom booklet sheet width 173.6mm) ----
const lay = usePrintLayout('idl-permit', 173.6);
const guides = ref(true);

// Built-in default anchors (mm) + font pt, one per editable VALUE. These are the exact
// pdf.js baselines from the legacy rptInternationalDriveingLicence sample — do NOT re-derive.
// size = the per-item font pt ('s'); the rotation baseline correction in tStyle uses it.
const DEF: Record<string, { x: number; y: number; size?: number }> = {
  // Page 1 — validity / issuer / national licence number
  validTill:   { x: 90.9,  y: 56.7, size: 11.3 },
  companyName: { x: 99.2,  y: 81.8, size: 10.3 },
  issuingCity: { x: 106.3, y: 56.3, size: 11.3 },
  validFrom:   { x: 114.5, y: 56.7, size: 11.3 },
  licenceNo:   { x: 122, y: 43.4, size: 11.3 },
  // Page 2 — driver identity
  surname:     { x: 7.8,   y: 83.8, size: 9 },
  firstName:   { x: 12,  y: 83.9, size: 9 },
  birthPlace:  { x: 16.6,  y: 83.8, size: 9 },
  dob:         { x: 20.6,  y: 83.9, size: 9 },
  livingPlace: { x: 24.5,  y: 83.8, size: 9 },
};
lay.setDefaults(DEF);

// ── The booklet is an OVERLAY: only data values, printed directly onto the official
// IDP licence paper (custom 173.6 × 209.9 mm sheet), all text rotated 90° and in
// Latin script (international document). Positions are exact pdf.js baselines from
// the legacy rptInternationalDriveingLicence sample. ──

// Macedonian Cyrillic → Latin (booklet standard, matches the legacy output)
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
function fmt(s: string | null | undefined, y4: boolean): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  const yy = y4 ? String(d.getFullYear()) : String(d.getFullYear()).slice(-2);
  return `${dd}-${mm}-${yy}`;
}

// Items: `k` is the STABLE layout key (x/y/size come from lay.resolve(k)); `p` = page
// and `f` = font family stay static on the item, only the data VALUE is editable.
// Fonts match the legacy PDF exactly: everything is BOLD — values in Arial Bold,
// the licence number in Microsoft Sans Serif Bold (f: 'ms').
type T = { p: number; k: string; f?: 'ms'; t: string };
const items = computed<T[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  return [
    // Page 1 — validity / issuer / national licence number
    { p: 1, k: 'validTill',   t: fmt(b.validTillDate, false) },
    { p: 1, k: 'companyName', t: lat(b.companyName) },
    { p: 1, k: 'issuingCity', t: lat(b.issuingCityName) },
    { p: 1, k: 'validFrom',   t: fmt(b.issuedDate, false) },
    { p: 1, k: 'licenceNo',   f: 'ms' as const, t: b.numberOfNationalLicence },
    // Page 2 — driver identity
    { p: 2, k: 'surname',     t: lat(b.clientSurname) },
    { p: 2, k: 'firstName',   t: lat(b.clientFirstName) },
    { p: 2, k: 'birthPlace',  t: lat(b.birthCityName) },
    { p: 2, k: 'dob',         t: fmt(b.dateOfBirth, true) },
    { p: 2, k: 'livingPlace', t: lat(b.livingCityName) },
  ];
});
function itemsFor(pg: number): T[] { return items.value.filter(i => i.p === pg); }

// Rotated baseline anchoring: with rotate(-90deg) about the element's top-left corner,
// local +x runs UP the page and local +y runs RIGHT. The glyph baseline sits ~0.847em
// below the box top, i.e. at page-x = left + 0.847em — so shift left by −0.30 × size
// to land the baseline exactly on the extracted x. x/y/size come from the saved layout
// (resolve returns the DEF default when there is no override → byte-identical output).
function tStyle(i: T) {
  const p = lay.resolve(i.k);
  const fam = i.f === 'ms' ? `font-family:'Microsoft Sans Serif', Arial, sans-serif;` : '';
  return `left:${(p.x - p.size * 0.30).toFixed(2)}mm; top:${p.y}mm; font-size:${p.size}pt; ${fam}`;
}

// Representative sample used in layout-edit mode (no real record needed).
const SAMPLE: IdlPermitPrint = {
  id: 0,
  issuedDate: '2026-06-15',
  validTillDate: '2027-06-15',
  issuingCityName: 'Велес',
  issuingOrgName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ',
  companyName: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС',
  numberOfLicence: 'MK 000123',
  numberOfNationalLicence: 'VE1234567',
  clientSurname: 'Петровски',
  clientFirstName: 'Петар',
  birthCityName: 'Велес',
  dateOfBirth: '1985-03-21',
  livingCityName: 'Скопје',
  note: null,
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
    const { data } = await api.get<IdlPermitPrint>(`/international-driving-licences/${props.id}/permit`);
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
    <div class="toolbar no-print">
      <button @click="doPrint">Печати</button>
      <button @click="doClose">Затвори</button>
    </div>

    <div v-if="loading" class="msg">Се вчитува…</div>
    <div v-else-if="error" class="msg err">{{ error }}</div>

    <template v-else-if="bundle">
      <div v-for="pg in [1, 2]" :key="pg" class="page"
           :ref="(el) => lay.setPageEl(el as HTMLElement | null)"
           :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="173.6" :height-mm="209.9" />
        <div v-for="it in itemsFor(pg)" :key="it.k" class="t"
             :class="{ sel: lay.selectedKey.value === it.k }" :style="tStyle(it)"
             @pointerdown="lay.beginDrag(it.k, $event)">{{ it.t }}</div>
      </div>
    </template>

    <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'Меѓународна дозвола — образец'" v-model:guides="guides" />
  </div>
</template>

<style scoped>
.screen { background: #e0e0e0; min-height: 100vh; padding: 8mm 0; }
.page {
  position: relative;
  width: 173.6mm; height: 209.9mm;
  margin: 0 auto 8mm; background: #fff; color: #000;
  font-family: Arial, "Helvetica Neue", sans-serif;
  font-weight: 700; /* the legacy booklet prints everything bold (Arial Bold) */
  box-shadow: 0 0 10px rgba(0, 0, 0, .2);
  overflow: hidden;
}
.t {
  position: absolute; white-space: nowrap; line-height: 1;
  transform: rotate(-90deg);
  transform-origin: 0 0;
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
  /* A4 paper, content anchored to the TOP-LEFT corner — every mark lands at the same
     distance from the sheet's top-left as in the legacy document (whose page origin is
     the booklet sheet's own corner). No centring: on the 173.6mm-wide layout that would
     shift everything +18.2mm right on A4. */
  .screen { background: #fff; padding: 0; min-height: auto; }
  .page { margin: 0; box-shadow: none; page-break-after: always; overflow: visible; }
  .page:last-of-type { page-break-after: auto; }
  .no-print { display: none !important; }
  @page { size: A4 portrait; margin: 0; }
}
</style>
