<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { IdlPermitPrint } from '@/types';

const props = defineProps<{ id: string }>();

const bundle = ref<IdlPermitPrint | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

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

// Items: (x, y) = exact baseline anchor in mm; text runs UP the page (rotated 90°).
// Fonts match the legacy PDF exactly: everything is BOLD — values in Arial Bold,
// the licence number in Microsoft Sans Serif Bold (f: 'ms').
type T = { p: number; x: number; y: number; s: number; f?: 'ms'; t: string };
const items = computed<T[]>(() => {
  const b = bundle.value;
  if (!b) return [];
  return [
    // Page 1 — validity / issuer / national licence number
    { p: 1, x: 86.9, y: 56.7, s: 11.3, t: fmt(b.validTillDate, false) },
    { p: 1, x: 95.2, y: 81.8, s: 10.3, t: lat(b.companyName) },
    { p: 1, x: 103.3, y: 55.3, s: 11.3, t: lat(b.issuingCityName) },
    { p: 1, x: 112.5, y: 56.7, s: 11.3, t: fmt(b.issuedDate, false) },
    { p: 1, x: 120.0, y: 47.4, s: 11.3, f: 'ms' as const, t: b.numberOfNationalLicence },
    // Page 2 — driver identity
    { p: 2, x: 5.8, y: 83.8, s: 9.0, t: lat(b.clientSurname) },
    { p: 2, x: 10.0, y: 83.9, s: 9.0, t: lat(b.clientFirstName) },
    { p: 2, x: 14.6, y: 83.8, s: 9.0, t: lat(b.birthCityName) },
    { p: 2, x: 18.6, y: 83.9, s: 9.0, t: fmt(b.dateOfBirth, true) },
    { p: 2, x: 22.5, y: 83.8, s: 9.0, t: lat(b.livingCityName) },
  ];
});
function itemsFor(pg: number): T[] { return items.value.filter(i => i.p === pg); }

// Rotated baseline anchoring: with rotate(-90deg) about the element's top-left corner,
// local +x runs UP the page and local +y runs RIGHT. The glyph baseline sits ~0.847em
// below the box top, i.e. at page-x = left + 0.847em — so shift left by −0.30 × size
// to land the baseline exactly on the extracted x.
function tStyle(i: T) {
  const fam = i.f === 'ms' ? `font-family:'Microsoft Sans Serif', Arial, sans-serif;` : '';
  return `left:${(i.x - i.s * 0.30).toFixed(2)}mm; top:${i.y}mm; font-size:${i.s}pt; ${fam}`;
}

onMounted(async () => {
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
      <div v-for="pg in [1, 2]" :key="pg" class="page">
        <div v-for="(it, i) in itemsFor(pg)" :key="i" class="t" :style="tStyle(it)">{{ it.t }}</div>
      </div>
    </template>
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
