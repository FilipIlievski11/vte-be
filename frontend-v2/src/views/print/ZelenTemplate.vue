<script setup lang="ts">
/**
 * ZELEN (Зелен) print template — positions extracted DIRECTLY from the legacy
 * DevExpress `.prnx` rendered output (scripts/printZelen.xml).
 *
 * Each (x, y, w) below is taken from a `<Brick>` Rect in the prnx, converted
 * from pixels-at-300-DPI to millimeters:
 *
 *     mm = px / 11.811   (since 2481px = 210mm, 3507px = 297mm)
 *
 * For nested-panel bricks on page 2, the parent panel offsets are folded in.
 *
 * Edit mode still available (toggle in toolbar) for nudges if a real printout
 * shows any drift.
 */
import { computed, onMounted, ref, watch } from 'vue';
import type { RequestPrintBundle } from '@/types';

const props = defineProps<{
  bundle: RequestPrintBundle;
  showGuides?: boolean;
  editMode?: boolean;
}>();
const emit = defineEmits<{ (e: 'positions-changed', positions: AllPos): void }>();

const b = () => props.bundle;

function fmtDate(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (Number.isNaN(d.getTime())) return '';
  return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth() + 1).padStart(2, '0')}.${d.getFullYear()}`;
}
function num(v: number | null | undefined): string {
  return v == null ? '' : String(Math.round(v));
}
// Decimal with Macedonian comma separator, no rounding / trailing zeros.
// e.g. 3.3 → "3,3", 55 → "55". Used for engine power (P.2) which the legacy
// printout shows with the fractional part (e.g. "3,3"), unlike the rounded fields.
function dec(v: number | null | undefined): string {
  return v == null ? '' : String(v).replace('.', ',');
}

interface Pos { x: number; y: number; w: number; h?: number; align?: 'left'|'center'|'right'; bold?: boolean; size?: number; label?: string }
type PageMap = Record<string, Pos>;
interface AllPos { page1: PageMap; page2: PageMap }

// ============================================================================
//  Positions extracted from scripts/printZelen.xml (legacy .prnx).
//  Every value is the rect from the rendered document → mm (px / 11.811).
// ============================================================================
const DEFAULT_POS: AllPos = {
  page1: {
    // --- top of form ---
    printDate:      { x: 94.7,  y: 10.1,  w: 25.4, h: 6.3, size: 9, label: 'Датум на печатење' },
    toMvr:          { x: 51.9,  y: 12.7,  w: 42.3, h: 8.5, label: 'ДО МВР - ОУР' },

    // --- (A) Plate + variant checkbox ---
    plate:          { x: 92.6,  y: 72.0,  w: 108.0, h: 8.5, align: 'left', size: 12, label: '(A) Регистарска ознака' },
    variantMark:    { x: 50.8,  y: 85.7,  w: 4.2,  h: 4.2, align: 'center', label: 'A/Б/В/Г ✕' },

    // --- D.1–D.3 + 38 block ---
    marka:          { x: 61.4,  y: 121.2, w: 118.5, h: 6.9, label: 'D.1 Марка' },
    tip:            { x: 61.4,  y: 128.6, w: 118.5, h: 5.8, label: 'D.2 Тип/варијанта' },
    komerc:         { x: 61.4,  y: 134.4, w: 118.5, h: 5.3, label: 'D.3 Комерц. ознака' },
    // 38 Облик — legacy uses 9pt (Style 6) so the longer body-type strings fit.
    oblik:          { x: 73.0,  y: 141.3, w: 68.8, h: 5.3, size: 9, label: '38 Облик каросерија' },

    // --- C.2.1–C.2.4 owner ---
    surname:        { x: 102.1, y: 150.8, w: 100.0, h: 6.4, label: 'C.2.1 Презиме' },
    firstName:      { x: 61.4,  y: 158.2, w: 118.5, h: 6.4, label: 'C.2.2 Име' },
    address:        { x: 61.4,  y: 165.6, w: 118.5, h: 4.4, align: 'right', label: 'C.2.3 Адреса' },
    community:      { x: 61.4,  y: 173.7, w: 118.5, h: 6.4, label: 'C.2.3 Општина' },
    embg:           { x: 61.4,  y: 179.5, w: 118.5, h: 6.4, label: 'C.2.4 ЕМБГ' },

    // --- A1 Регистрација важи до ---
    validUntil:     { x: 86.8,  y: 187.9, w: 67.7, h: 6.3, label: 'A1 Регистрација важи до' },

    // --- Кон барањето: proofs ---
    ownershipProof: { x: 69.8,  y: 202.3, w: 110.1, h: 6.4, label: 'Доказ за потеклото' },
    paymentProof:   { x: 69.8,  y: 210.7, w: 110.1, h: 6.4, label: 'Потврда за платени давачки' },

    // --- bottom (submitter + reference) ---
    company:        { x: 19.0,  y: 236.7, w: 97.4, h: 4.4, label: 'Подносител (фирма)' },
    referenceNo:    { x: 45.0,  y: 254.9, w: 61.9, h: 4.4, label: 'Број на барање' },
  },
  page2: {
    // ----- ТЕХНИЧКИ ПОДАТОЦИ panel (top of page 2) -----
    // All page-2 controls are inside two nested panels.
    // Outer panel offset = (1074.80 / 11.81, 0)        = (91.0, 0)
    // Inner panel offset = (75.59 / 11.81, 193.70 / 11.81) = (6.4, 16.4)
    // Combined offset    = (97.4 mm, 16.4 mm) — already folded into the
    // values below; just read x/y in absolute page-2 mm.

    engineType:        { x: 139.7, y: 16.4, w: 36.5, h: 4.4, label: '3.1.3 Тип на мотор' },
    vin:               { x: 110.6, y: 22.1, w: 82.0, h: 4.4, label: 'E VIN' },
    yearOfManufacture: { x: 111.2, y: 30.0, w: 82.0, h: 4.4, label: '5A Година на производство' },
    bodyType:          { x: 98.4,  y: 36.8, w: 70.9, h: 4.1, size: 9, label: '38 Облик каросерија' },
    color:             { x: 112.2, y: 44.3, w: 46.0, h: 4.4, label: 'R Боја' },
    engineNumber:      { x: 112.2, y: 51.0, w: 83.1, h: 4.4, label: 'P5 Идентификационен број на моторот' },
    powerKw:           { x: 112.2, y: 57.8, w: 82.6, h: 4.4, label: 'P2 Сила (kW)' },
    cc:                { x: 112.2, y: 65.0, w: 80.4, h: 4.4, label: 'P.1 Зафатнина (cm³)' },
    mass:              { x: 112.2, y: 72.3, w: 77.8, h: 4.4, label: 'G Маса' },
    seats:             { x: 111.7, y: 78.9, w: 73.0, h: 4.4, label: 'S.1 Седишта' },
    standingSeats:     { x: 112.2, y: 84.9, w: 66.7, h: 4.4, label: 'S.2 Стоење' },
    category:          { x: 112.7, y: 90.9, w: 82.0, h: 4.4, label: 'J Категорија и вид' },

    // ----- Б. ПОДАТОЦИ ЗА СОПСТВЕНИКОТ panel (middle of page 2) -----
    // Only renders when the request mutates client data (e.g. А+Б types).
    // Outer panel (91.0, 0) + Б panel (0, 123.9) = panel origin (91.0, 123.9).
    bChangeMark: { x: 92.1, y: 123.9, w: 4.2, h: 5.0, align: 'center', label: 'Б Вид на промена ✕' },
    bSurname:    { x: 98.4, y: 129.7, w: 75.6, h: 4.4, label: 'Б C.2.1 Презиме' },
    bFirstName:  { x: 98.4, y: 137.5, w: 75.6, h: 4.4, label: 'Б C.2.2 Име' },
    bAddress:    { x: 98.4, y: 144.8, w: 75.6, h: 4.4, label: 'Б C.2.3 Адреса' },
    bCommunity:  { x: 98.4, y: 152.4, w: 75.6, h: 4.4, label: 'Б C.2.3 Општина' },
    bEmbg:       { x: 97.9, y: 158.6, w: 75.6, h: 4.4, label: 'Б C.2.4 ЕМБГ' },
  },
};

// Live editable copy (localStorage persisted)
const STORAGE_KEY = 'vte.v2.print.zelen.positions.v2';   // v2 — bump key when DEFAULT_POS changes meaningfully
const pos = ref<AllPos>(loadInitial());

function loadInitial(): AllPos {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw) {
      const saved = JSON.parse(raw);
      const merged: AllPos = { page1: {}, page2: {} };
      for (const p of ['page1', 'page2'] as const) {
        for (const k of Object.keys(DEFAULT_POS[p])) {
          merged[p][k] = { ...DEFAULT_POS[p][k], ...(saved?.[p]?.[k] ?? {}) };
        }
      }
      return merged;
    }
  } catch { /* ignore */ }
  return JSON.parse(JSON.stringify(DEFAULT_POS));
}
watch(pos, v => {
  try { localStorage.setItem(STORAGE_KEY, JSON.stringify(v)); } catch { /* quota */ }
  emit('positions-changed', v);
}, { deep: true });

function resetPositions() {
  pos.value = JSON.parse(JSON.stringify(DEFAULT_POS));
  localStorage.removeItem(STORAGE_KEY);
}
function exportPositions(): string {
  const minimal: Record<string, Record<string, { x: number; y: number }>> = {};
  for (const p of ['page1', 'page2'] as const) {
    minimal[p] = {};
    for (const k of Object.keys(pos.value[p])) {
      minimal[p][k] = { x: pos.value[p][k].x, y: pos.value[p][k].y };
    }
  }
  return JSON.stringify(minimal, null, 2);
}
defineExpose({ resetPositions, exportPositions });

// --- Drag handling ---
const dragging = ref<{ page: 'page1'|'page2'; key: string; startX: number; startY: number; origX: number; origY: number } | null>(null);
function startDrag(page: 'page1'|'page2', key: string, ev: MouseEvent) {
  if (!props.editMode) return;
  ev.preventDefault();
  ev.stopPropagation();
  const p = pos.value[page][key];
  dragging.value = { page, key, startX: ev.clientX, startY: ev.clientY, origX: p.x, origY: p.y };
}
function onMouseMove(ev: MouseEvent) {
  if (!dragging.value) return;
  const PX_PER_MM = 3.7795275591;
  const dx = (ev.clientX - dragging.value.startX) / PX_PER_MM;
  const dy = (ev.clientY - dragging.value.startY) / PX_PER_MM;
  const p = pos.value[dragging.value.page][dragging.value.key];
  p.x = Math.max(0, Math.min(210, dragging.value.origX + dx));
  p.y = Math.max(0, Math.min(297, dragging.value.origY + dy));
}
function onMouseUp() { dragging.value = null; }
onMounted(() => {
  window.addEventListener('mousemove', onMouseMove);
  window.addEventListener('mouseup', onMouseUp);
});

// Variant checkbox Y by request-type prefix (A=85.7, Б ≈ 92, В ≈ 99, Г ≈ 105)
// Real Y measured for А from prnx: 85.7. Each option is ~6.5mm spacing on
// the green paper based on photo measurement.
const variantY = computed(() => {
  const name = b().type.name.toUpperCase();
  if (/^А|^A/.test(name)) return 85.7;
  if (/^Б|^B/.test(name)) return 92.5;
  if (/^В|^V/.test(name)) return 99.0;
  if (/^Г|^G/.test(name)) return 105.5;
  return 85.7;
});

// Value resolvers — one per field key
const v = computed(() => ({
  page1: {
    printDate:      fmtDate(b().request.createdAt),
    // "ДО МВР - ОУР" = the destination MVR office. Legacy printZelen.vb resolves it from
    // the (new) owner's community via GetRegistrationIssuerInfoByCommunity — offices are
    // named "МВР {ОПШТИНА}" (e.g. owner in Велес → "МВР ВЕЛЕС"). Do NOT use the
    // VehicleRegistration child's issuer: for a re-registered vehicle that's the stale
    // OLD-municipality office (e.g. "МВР КОЧАНИ").
    toMvr:          (() => {
                       const com = b().client?.communityName?.trim() || b().client?.cityName?.trim();
                       return com ? `МВР ${com}` : '';
                     })(),
    // Plate source: legacy printZelen.vb binds lblNovaReg.Text to
    // CurrentVehicle.LastRegistration — backed by Vehicles.LastRegistratinNumber
    // (which migrate-vehicles.sql copies into Vehicle.Plate). The VehicleRegistration
    // child table often carries the sentinel "{CommunityCode}-000-AA" even when
    // Vehicle.Plate holds the real plate, so prefer the Vehicle column.
    // Legacy sentinel handling (PrintVehcileInfo.vb LastRegistration getter,
    // lines 339-343): when the vehicle has no real plate yet, collapse the
    // "{Code}-000-AA" sentinel to "{Code}-".
    plate:          (() => {
                       const raw = b().vehicle?.plate || b().lastRegistration?.plateNumber || '';
                       const m = raw.match(/^([A-Za-zА-Яа-я]{1,3})-000-AA$/);
                       return m ? `${m[1]}-` : raw;
                     })(),
    variantMark:    '✕',
    marka:          b().vehicle?.maker || '',
    // D.2 Тип/варијанта/изведба — technical type designation.
    // Legacy binds lblModel.Text to CurrentVehicleBindingSource.Tip — a
    // per-vehicle string column on the live Vehicles table (we migrated it
    // to Vehicle.TypeText). Example: "J 8HZ / JM8HZ / JM8HZC".
    // Falls back to VehicleModel.Code if the per-vehicle field is blank.
    tip:            b().vehicle?.typeText || b().vehicle?.modelCode || '',
    // D.3 Комерцијална ознака — commercial/popular name. Combines maker model
    // name with the per-vehicle variant. Legacy appends " TNG" when vehicle
    // has LPG.  Example: "C2 1.4 HDI" or "YUGO KORAL 45 TNG".
    komerc:         (() => {
                       const base = [b().vehicle?.model, b().vehicle?.variant].filter(Boolean).join(' ');
                       return b().vehicle?.hasLpg ? `${base} TNG` : base;
                     })(),
    oblik:          b().vehicle?.bodyType || '',
    surname:        b().client?.firstName || '',     // Macedonian DB: firstName = surname
    firstName:      b().client?.lastName || '',      // lastName = given name
    // C.2.3 — legacy prefixes the street label "УЛ. " (= улица / street).
    address:        b().client?.address ? `УЛ. ${b().client!.address}` : '',
    community:      b().client?.communityName || b().client?.cityName || '',
    embg:           b().client?.mb || '',
    validUntil:     fmtDate(b().lastRegistration?.validUntil),
    // Join ALL proofs with "; " like the legacy single label — e.g.
    // "СООБРАЌАЈНА ДОЗВОЛА 2446652; ДОГОВОР" (not just the first row).
    ownershipProof: b().ownershipProofs.map(p => [p.typeName, p.detail].filter(Boolean).join(' ')).filter(Boolean).join('; '),
    paymentProof:   b().paymentProofs.map(p => [p.typeName, p.detail].filter(Boolean).join(' ')).filter(Boolean).join('; '),
    company:        b().company?.name?.trim() || '',
    // Reference number — legacy formula is:
    //   {StationCode}{IdTechnicalExamReport}{OperatorId}/{Year}
    // Backfilled from snapshot via backfill-request-reference-no.sql.
    // Falls back to {requestId}/{year} for new (post-cutover) requests until
    // we build the Technical Exam module.
    referenceNo:    b().request.legacyReferenceNumber
                    || `${b().request.id}/${new Date(b().request.createdAt).getFullYear()}`,
  },
  page2: {
    engineType:        b().vehicle?.engineTypeName || '',
    vin:               b().vehicle?.vin || '',
    // Year of manufacture from legacy MakeDate (datetime) — show year only.
    yearOfManufacture: b().vehicle?.manufactureDate
                         ? String(new Date(b().vehicle!.manufactureDate!).getFullYear())
                         : '',
    bodyType:          b().vehicle?.bodyType || '',
    color:             [b().vehicle?.primaryColorCode, b().vehicle?.primaryColorName].filter(Boolean).join(' '),
    engineNumber:      b().vehicle?.engineNumber || '',
    powerKw:           dec(b().vehicle?.enginePowerKw),
    cc:                num(b().vehicle?.engineWorkingCapacityCc),
    mass:              num(b().vehicle?.emptyWeightKg),
    seats:             num(b().vehicle?.seats),
    // S.2 Број места за стоење — legacy always shows a number (0 for passenger
    // cars). If our migration nulled-out zeros, fall back to '0'.
    standingSeats:     b().vehicle?.standingSeats != null ? String(b().vehicle!.standingSeats) : '0',
    category:          b().vehicle?.category || '',

    // Б section "промена на податоци за сопственикот" binds to the NEW owner in
    // legacy printZelen (lblNew* → NewOwner.CustomerFirstName / .MB / .CommunityNameLiving
    // / .LivingAddress). For an ownership transfer that's a DIFFERENT person than the
    // page-1 client; for a plain owner-data change there is no separate new owner, so
    // fall back to the page-1 client. (Page-1 surname=firstName, given=lastName in the
    // Macedonian data — same mapping applies here.)
    bChangeMark: '✕',
    bSurname:    (b().newClient ?? b().client)?.firstName || '',
    bFirstName:  (b().newClient ?? b().client)?.lastName || '',
    // Legacy prefixes "УЛ. " on the Б-section address too (same as page 1).
    bAddress:    (() => { const o = b().newClient ?? b().client; return o?.address ? `УЛ. ${o.address}` : ''; })(),
    bCommunity:  (b().newClient ?? b().client)?.communityName || (b().newClient ?? b().client)?.cityName || '',
    bEmbg:       (b().newClient ?? b().client)?.mb || '',
  },
}));

function fieldStyle(p: Pos, key: string): Record<string, string> {
  // Map text-align to flex justify-content (we use inline-flex for vertical
  // centering, so text-align alone doesn't move the inner span).
  const align = p.align ?? 'left';
  const justify =
    align === 'right'  ? 'flex-end'   :
    align === 'center' ? 'center'     :
    'flex-start';
  return {
    left:           p.x + 'mm',
    top:            (key === 'variantMark' ? variantY.value : p.y) + 'mm',
    width:          p.w + 'mm',
    height:         (p.h ?? 5) + 'mm',
    fontSize:       (p.size ?? 10) + 'pt',
    fontWeight:     p.bold ? '700' : '400',
    textAlign:      align,
    justifyContent: justify,
  };
}

// Whether the Б (change-of-owner-data) panel should render on page 2.
// Shows whenever the request transfers ownership or its name contains
// "промена" (= "change"), which is how the legacy types are named:
//   "А1 - ПРОДОЛЖУВАЊЕ + ПРОМЕНА НА ПОДАТОЦИ"
//   "Пренос на сопственост"
const showBSection = computed(() => {
  const name = b().type.name || '';
  return b().type.transfersOwnership || /промена/i.test(name);
});
const B_KEYS = new Set(['bChangeMark', 'bSurname', 'bFirstName', 'bAddress', 'bCommunity', 'bEmbg']);
function shouldRender(key: string): boolean {
  if (B_KEYS.has(key)) return showBSection.value;
  return true;
}
</script>

<template>
  <!-- ============= PAGE 1 — FRONT ============= -->
  <article class="sheet" :class="{ guides: showGuides, edit: editMode }">
    <div v-if="showGuides" class="grid-overlay no-print"></div>
    <div v-if="showGuides" class="page-label no-print">ЗЕЛЕН · стр. 1 · ПРЕДНА</div>

    <template v-for="(p, key) in pos.page1" :key="'p1:' + key">
      <span
        class="f"
        :class="{ bold: p.bold, draggable: editMode }"
        :style="fieldStyle(p, key as string)"
        :data-key="'page1:' + key"
        @mousedown="startDrag('page1', key as string, $event)"
      >
        <span class="f-val">{{ (v.page1 as any)[key] }}</span>
        <span v-if="showGuides" class="f-tag no-print">{{ p.label || key }}</span>
      </span>
    </template>
  </article>

  <!-- ============= PAGE 2 — BACK ============= -->
  <article class="sheet" :class="{ guides: showGuides, edit: editMode }">
    <div v-if="showGuides" class="grid-overlay no-print"></div>
    <div v-if="showGuides" class="page-label no-print">ЗЕЛЕН · стр. 2 · ЗАДНА</div>

    <template v-for="(p, key) in pos.page2" :key="'p2:' + key">
      <span
        v-if="shouldRender(key as string)"
        class="f"
        :class="{ bold: p.bold, draggable: editMode }"
        :style="fieldStyle(p, key as string)"
        :data-key="'page2:' + key"
        @mousedown="startDrag('page2', key as string, $event)"
      >
        <span class="f-val">{{ (v.page2 as any)[key] }}</span>
        <span v-if="showGuides" class="f-tag no-print">{{ p.label || key }}</span>
      </span>
    </template>
  </article>
</template>

<style scoped>
.sheet {
  position: relative;
  width: 210mm;
  height: 297mm;
  background: #fff;
  color: #000;
  font-family: 'Arial', sans-serif;
  font-size: 10pt;
  overflow: hidden;
  box-sizing: border-box;
  margin: 0 auto 12px;
  box-shadow: 0 2px 14px rgba(0,0,0,.18);
  break-after: page;
}
.sheet:last-child { break-after: auto; }

.f {
  position: absolute;
  display: inline-flex;
  align-items: center;
  padding: 0 0.5mm;
  box-sizing: border-box;
  white-space: nowrap;
  overflow: hidden;
  line-height: 1.1;
  /* Legacy print is all uppercase, regardless of how the data is stored. */
  text-transform: uppercase;
}
.f.bold { font-weight: 700 }
.f-val { display: inline-block; max-width: 100% }

.sheet.guides .f {
  outline: 1px dashed rgba(255, 0, 0, .5);
  background: rgba(255, 255, 0, .15);
}
.f-tag {
  position: absolute; bottom: 100%; left: 0;
  font-family: system-ui, sans-serif; font-size: 7.5pt;
  color: #dc2626; background: rgba(255,255,255,.9);
  padding: 0 2px; border-radius: 2px; pointer-events: none;
  white-space: nowrap; line-height: 1;
}
.sheet.edit .f.draggable {
  cursor: move;
  outline: 1px dashed rgba(34, 197, 94, .6);
  background: rgba(187, 247, 208, .35);
}
.sheet.edit .f.draggable:hover {
  outline: 2px solid #16a34a;
  background: rgba(134, 239, 172, .55);
}

.grid-overlay {
  position: absolute; inset: 0;
  background-image:
    linear-gradient(to right, rgba(0,0,255,.08) 0 1px, transparent 1px 5mm),
    linear-gradient(to bottom, rgba(0,0,255,.08) 0 1px, transparent 1px 5mm);
  background-size: 5mm 5mm;
  pointer-events: none;
}
.page-label {
  position: absolute; top: 2mm; right: 2mm;
  font-family: system-ui, sans-serif; font-size: 9pt; font-weight: 600;
  color: rgba(220, 38, 38, .9); background: rgba(255,255,255,.85);
  padding: 1mm 2mm; border: 1px solid rgba(220,38,38,.4);
}

@media print {
  .sheet { box-shadow: none; margin: 0 }
  .sheet.guides .f, .sheet.edit .f.draggable {
    outline: none; background: transparent;
  }
  .f-tag { display: none !important }
  .no-print { display: none !important }
}
</style>
