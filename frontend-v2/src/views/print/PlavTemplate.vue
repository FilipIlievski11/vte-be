<script setup lang="ts">
/**
 * ПЛАВ (Plav / Blue) print template — a 3-page form used for re-registration /
 * transfer of ownership. Positions extracted DIRECTLY from the legacy DevExpress
 * `.prnx` rendered output (Downloads/PrintPlav.xml) via scripts/parse-prnx.ps1,
 * which folds panel offsets and converts 300-DPI px → mm (px / 11.811).
 *
 * Page layout (legacy PrintPlav):
 *   Page 1 — front: current owner block + current registration + new-reg prefix
 *            + variant checkbox + proofs + submitter org + reference number.
 *   Page 2 — vehicle technical data (maker, model, VIN, engine, masses, dims…).
 *   Page 3 — new owner block + new registration (issuer + plate).
 *
 * Field→data bindings cross-referenced from WinApp/Requests/PrintPlav.Designer.vb
 * (CurrentOwner.* / NewOwner.* / CurrentVehicle.*) and PrintPlav.vb.
 *
 * Layout editing uses the shared server-backed usePrintLayout system. Positions
 * persist to the backend PrintLayout store keyed 'plav' (only the diff vs the
 * built-in DEFAULT_POS is saved). Edit mode is entered by the admin editor via
 * ?edit=1; the shared PrintLayoutToolbar drives save/reset/font/nudge.
 */
import { computed, onMounted, ref } from 'vue';
import type { RequestPrintBundle } from '@/types';
import { usePrintLayout } from '@/composables/usePrintLayout';
import PrintLayoutToolbar from '@/components/PrintLayoutToolbar.vue';
import PrintRulers from '@/components/PrintRulers.vue';

const props = defineProps<{
  bundle: RequestPrintBundle;
  // Kept optional for backward compatibility; edit UI is driven by lay.editing.
  showGuides?: boolean;
  editMode?: boolean;
}>();

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
// S.x seat counts and several mass/axle fields print "0" in the legacy even when
// our migration nulled the zero. Render 0 instead of blank for those.
function numZ(v: number | null | undefined): string {
  return v == null ? '0' : String(Math.round(v));
}
// Legacy sentinel: a vehicle with no real plate is stored as "{Code}-000-AA".
function plateOrPrefix(raw: string | null | undefined): string {
  const s = raw || '';
  const m = s.match(/^([A-Za-zА-Яа-я]{1,3})-000-AA$/);
  return m ? `${m[1]}-` : s;
}

interface Pos { x: number; y: number; w: number; h?: number; align?: 'left'|'center'|'right'; bold?: boolean; size?: number; label?: string }
type PageMap = Record<string, Pos>;
interface AllPos { page1: PageMap; page2: PageMap; page3: PageMap }
type PageKey = 'page1' | 'page2' | 'page3';

// ============================================================================
//  Positions from Downloads/PrintPlav.xml (legacy .prnx), absolute mm per page.
//  DEFAULT_POS carries the STATIC meta (x/y = built-in default; w/h/align/bold/
//  label/size) — the resolved x/y/size comes from the saved-layout composable.
// ============================================================================
const DEFAULT_POS: AllPos = {
  page1: {
    printDate:     { x: 112.2, y: 5.3,   w: 25.4,  h: 6.3, size: 9,  label: 'Датум на печатење' },
    toMvr:         { x: 65.6,  y: 12.7,  w: 45.5,  h: 6.3, label: 'ДО МВР (нова рег.)' },
    // New-registration prefix (lblNovaReg) — "{NewCommunityCode}-"
    newReg:        { x: 84.7,  y: 76.2,  w: 76.2,  h: 6.4, label: 'Нова регистрација' },
    variantMark:   { x: 45.5,  y: 105.8, w: 4.2,   h: 4.2, align: 'center', label: 'Вид барање ✕' },
    // ---- Current owner + current registration block (panel) ----
    curPlate:      { x: 103.7, y: 143.9, w: 72.0,  h: 6.4, label: 'Претходна рег. ознака' },
    curIssuer:     { x: 103.7, y: 152.4, w: 72.0,  h: 6.4, label: 'Претходен орган (МВР)' },
    curValidTill:  { x: 103.7, y: 160.9, w: 72.0,  h: 6.4, label: 'Рег. важи до' },
    curSurname:    { x: 124.9, y: 170.4, w: 50.8,  h: 5.3, label: 'Сопственик — Презиме' },
    curName:       { x: 103.8, y: 176.8, w: 72.0,  h: 5.3, label: 'Сопственик — Име' },
    curAddress:    { x: 124.9, y: 182.1, w: 50.8,  h: 5.3, label: 'Адреса' },
    curCommunity:  { x: 103.8, y: 188.4, w: 50.8,  h: 5.3, label: 'Општина' },
    curEmbg:       { x: 103.8, y: 193.7, w: 72.0,  h: 6.4, label: 'ЕМБГ' },
    // ---- Proofs / note ----
    ownershipProof:{ x: 82.6,  y: 205.3, w: 120.6, h: 6.4, label: 'Доказ за потекло' },
    paymentProof:  { x: 82.6,  y: 211.7, w: 118.5, h: 6.4, label: 'Платени давачки' },
    note:          { x: 87.8,  y: 219.1, w: 113.2, h: 6.4, label: 'Забелешка' },
    // ---- Footer (submitter org + reference) ----
    company:       { x: 24.4,  y: 234.9, w: 91.0,  h: 7.4, label: 'Подносител (фирма)' },
    // "ready" signature mark (legacy lblIsReady "__", bold Times New Roman).
    readyMark:     { x: 109.1, y: 245.5, w: 8.0,   h: 4.4, bold: true, label: 'Подготвено ✓' },
    referenceNo:   { x: 37.1,  y: 255.0, w: 64.6,  h: 4.4, label: 'Број на барање' },
  },
  page2: {
    // ---- Vehicle technical data (single panel) ----
    maker:           { x: 92.1,  y: 24.6,  w: 86.8, h: 5.0, label: 'D.1 Марка' },
    typeText:        { x: 92.1,  y: 31.8,  w: 86.8, h: 5.3, label: 'D.2 Тип' },
    model:           { x: 92.1,  y: 38.1,  w: 86.8, h: 5.3, label: 'D.3 Модел' },
    vin:             { x: 79.4,  y: 46.6,  w: 86.8, h: 5.3, label: 'E VIN' },
    year:            { x: 74.1,  y: 51.9,  w: 85.7, h: 5.3, label: 'Год. производство' },
    bodyType:        { x: 69.8,  y: 58.2,  w: 78.3, h: 6.4, size: 9, label: '38 Облик' },
    engineTypeCode:  { x: 103.7, y: 65.6,  w: 80.4, h: 5.3, label: 'Тип на мотор' },
    cc:              { x: 48.7,  y: 73.0,  w: 42.3, h: 5.3, size: 9, align: 'right', label: 'P.1 Зафатнина' },
    powerKw:         { x: 142.9, y: 74.1,  w: 42.3, h: 5.3, size: 9, align: 'right', label: 'P.2 Сила kW' },
    fuel:            { x: 46.6,  y: 81.5,  w: 28.0, h: 6.4, size: 9, align: 'right', label: 'P.3 Гориво' },
    engineNumber:    { x: 100.6, y: 86.8,  w: 84.7, h: 6.4, label: 'P.5 Бр. мотор' },
    emptyWeight:     { x: 65.6,  y: 100.5, w: 40.2, h: 5.3, label: 'G Празна маса' },
    maxConstructive: { x: 143.9, y: 100.5, w: 43.4, h: 5.3, size: 9, align: 'right', label: 'F.1 Техн. дозв. маса' },
    maxLegal:        { x: 134.4, y: 106.9, w: 50.3, h: 6.4, label: 'F.2 Дозв. маса' },
    maxLegalGroup:   { x: 134.4, y: 113.2, w: 50.0, h: 6.3, label: 'F.3 Маса на група (3130)' },
    length:          { x: 43.4,  y: 164.0, w: 27.5, h: 6.4, label: 'Должина' },
    width:           { x: 76.2,  y: 164.0, w: 29.6, h: 6.4, size: 9, align: 'center', label: 'Ширина' },
    height:          { x: 143.9, y: 164.0, w: 29.6, h: 6.4, size: 9, align: 'center', label: 'Висина' },
    seats:           { x: 44.5,  y: 187.3, w: 31.8, h: 6.4, label: 'S.1 Седишта' },
    standingSeats:   { x: 94.2,  y: 187.3, w: 27.5, h: 6.4, size: 9, align: 'center', label: 'S.2 Стоење' },
    lyingSeats:      { x: 157.7, y: 187.3, w: 28.6, h: 6.4, size: 9, align: 'right', label: 'S.3 Лежечки' },
    color:           { x: 95.3,  y: 201.1, w: 80.4, h: 5.3, label: 'R Боја' },

    // ---- Technical fields the legacy prints as "0" for passenger cars ----
    // Per-axle mass row (Маса по оска 1-5).
    massAxle1: { x: 25.4,  y: 134.4, w: 13.8, h: 5.3, size: 9, label: 'Маса/оска 1' },
    massAxle2: { x: 48.7,  y: 134.4, w: 13.8, h: 5.3, size: 9, label: 'Маса/оска 2' },
    massAxle3: { x: 88.9,  y: 134.4, w: 13.8, h: 5.3, size: 9, label: 'Маса/оска 3' },
    massAxle4: { x: 120.7, y: 133.9, w: 13.8, h: 5.3, size: 9, label: 'Маса/оска 4' },
    massAxle5: { x: 156.6, y: 134.1, w: 13.8, h: 5.3, size: 9, label: 'Маса/оска 5' },
    // Axle load row (Осно оптоварување 1-5). 1-2 from data, 3-5 zero-fill.
    axleLoad1: { x: 27.5,  y: 149.2, w: 13.8, h: 6.4, size: 9, label: 'Осно опт. 1' },
    axleLoad2: { x: 55.0,  y: 148.2, w: 13.8, h: 6.4, size: 9, label: 'Осно опт. 2' },
    axleLoad3: { x: 97.4,  y: 148.2, w: 13.8, h: 6.4, size: 9, label: 'Осно опт. 3' },
    axleLoad4: { x: 132.3, y: 148.2, w: 13.8, h: 6.4, size: 9, label: 'Осно опт. 4' },
    axleLoad5: { x: 165.1, y: 148.2, w: 13.8, h: 6.4, size: 9, label: 'Осно опт. 5' },
    // Misc technical specs (bound to real data; were wrongly hardcoded "0").
    z80:  { x: 147.1, y: 80.4,  w: 39.2, h: 6.4, size: 9, label: 'Број на вртежи' },
    z94:  { x: 65.6,  y: 94.2,  w: 39.2, h: 5.3, size: 9, label: 'Макс. брзина' },
    z121: { x: 64.6,  y: 121.7, w: 85.7, h: 5.3, size: 9, label: 'Број на оски' },
    z172: { x: 128.1, y: 172.5, w: 62.4, h: 6.4, size: 9, label: 'Приколка со кочница' },
    z179: { x: 128.1, y: 179.9, w: 60.3, h: 6.4, size: 9, label: 'Приколка без кочница' },
    z218: { x: 116.4, y: 218.0, w: 73.0, h: 6.4, size: 9, label: 'CO2' },
    z226: { x: 135.5, y: 226.5, w: 54.0, h: 7.4, size: 9, label: 'Носивост на приклучок' },
    z237: { x: 134.4, y: 237.0, w: 56.1, h: 6.4, size: 9, label: 'Бучава (мирување)' },
  },
  page3: {
    // ---- New registration (top-right) ----
    newRegIssuer:  { x: 99.5,  y: 32.2,  w: 80.4, h: 6.4, label: 'Нов орган (МВР)' },
    newPlate:      { x: 99.5,  y: 39.6,  w: 80.4, h: 6.4, label: 'Нова рег. ознака' },
    // ---- New owner block (panel) ----
    newSurname:    { x: 105.9, y: 77.7,  w: 69.8, h: 6.4, label: 'Нов сопственик — Презиме' },
    newName:       { x: 106.9, y: 87.2,  w: 69.8, h: 6.4, label: 'Нов сопственик — Име' },
    newAddress:    { x: 105.9, y: 95.7,  w: 69.8, h: 8.5, label: 'Адреса' },
    newCommunity:  { x: 104.8, y: 108.4, w: 69.8, h: 6.4, label: 'Општина' },
    newEmbg:       { x: 105.9, y: 116.8, w: 69.8, h: 6.4, label: 'ЕМБГ' },
  },
};

// ---- Saved-layout system (positions in mm; A4 width 210mm) ----
const lay = usePrintLayout('plav', 210);
const guides = ref(true);

// Flat default map for the composable: `${page}.${key}` → {x,y,size}. Font size
// defaults to 10pt (matching the render's `size ?? 10`) so a size override is
// always relative to the printed default.
const DEF: Record<string, { x: number; y: number; size?: number }> = {};
for (const page of ['page1', 'page2', 'page3'] as const) {
  for (const [key, p] of Object.entries(DEFAULT_POS[page])) {
    DEF[`${page}.${key}`] = { x: p.x, y: p.y, size: p.size ?? 10 };
  }
}
lay.setDefaults(DEF);

// New-registration prefix: legacy prints "{NewCommunityCode}-". The newest
// registration row IS the newly-issued one (sentinel "{Code}-000-AA"), so
// collapsing it gives "VE-". (lastRegistration = newest by ValidUntil.)
// Legacy lblNovaReg (PrintPlav.vb): for an IsNewRegistration type it prints the
// NEW owner's community registration prefix ("{RegistrationCode}-", e.g. "VE-" for
// Велес) — NOT the old plate. Fall back to the old plate/prefix only when there's
// no new registration or the community code is missing.
const newRegValue = computed(() => {
  const regCode = regOwner.value?.communityRegistrationCode;
  if (b().type.issuesNewRegistration && regCode) return `${regCode}-`;
  return plateOrPrefix(b().lastRegistration?.plateNumber || b().newVehicle?.plate);
});

// Strip the migration's bookkeeping note (e.g. "[legacy operator ids C=119 …]").
const cleanNote = computed(() => {
  const n = (b().request.note || '').trim();
  return /^\[legacy operator ids/i.test(n) ? '' : n;
});

// Legacy LivingAddress (PrintCustomerInfo.vb lines 12-30): prepends "УЛ. " ONLY
// when a street name is present; rural settlements (no street) print as-is.
// Our migrated Client.Address is a single flattened string, so approximate the
// rule: prepend "УЛ. " when the address contains a street number (a digit).
//   "Славко Ѓорѓиев … 1/1-5 Велес" → "УЛ. …"   |   "САРАМЗАЛИНО" → unchanged
function formatAddress(addr: string | null | undefined): string {
  const s = (addr || '').trim();
  if (!s) return '';
  return /\d/.test(s) ? `УЛ. ${s}` : s;
}

// Estimate how many lines an (uppercased) address wraps to inside its field, to
// mimic the legacy CanGrow that pushes the community/EMBG rows down. Uses a
// canvas to measure real Arial metrics. 1mm ≈ 3.7795px at 96dpi.
let _measureCtx: CanvasRenderingContext2D | null | undefined;
function addressLineCount(text: string, fieldWidthMm: number, fontPt = 10): number {
  if (!text || typeof document === 'undefined') return 1;
  if (_measureCtx === undefined) _measureCtx = document.createElement('canvas').getContext('2d');
  if (!_measureCtx) return 1;
  _measureCtx.font = `${fontPt}pt Arial`;
  const usablePx = (fieldWidthMm - 1) * 3.7795275591;   // minus ~1mm padding
  const w = _measureCtx.measureText(text.toUpperCase()).width;
  return Math.max(1, Math.min(2, Math.ceil(w / usablePx)));  // legacy box fits at most 2 lines
}
const LINE_MM = 3.3;   // legacy row pitch added per extra address line (188.4→191.6)

// Legacy RegNumberDolg (PrintPlavInfo.vb lines 206-220):
//   {StationCode}{IdTechnicalExamReport}{operatorId}/{Year}
// Plav has no exam report (→ "0") and operatorId is the *printing* employee id
// at runtime — NOT stored on the request — so the exact legacy value can't be
// recovered. Use the request's created-by legacy operator (parsed from the
// migration note "C=NNN") as the closest stable proxy: "0{op}/{year}".
const referenceValue = computed(() => {
  if (b().request.legacyReferenceNumber) return b().request.legacyReferenceNumber;
  const year = new Date(b().request.createdAt).getFullYear();
  const m = (b().request.note || '').match(/C=(\d+)/i);
  return m ? `0${m[1]}/${year}` : `${b().request.id}/${year}`;
});

// Join all ownership/payment proofs with "; " like the legacy single label.
function joinProofs(list: { typeName: string | null; detail: string | null }[]): string {
  return list
    .map(p => [p.typeName, p.detail].filter(Boolean).join(' '))
    .filter(Boolean)
    .join('; ');
}

// Value resolvers — one per field key
const v = computed(() => ({
  page1: {
    // Legacy XrPageInfo1 prints the print-machine clock (today), NOT createdAt —
    // a reprint shows the date it was printed, not when the request was filed.
    printDate:     fmtDate(new Date().toISOString()),
    // Destination MVR ("ДО МВР") = the registration issuer of the NEW owner's
    // community (legacy lblToOrganization = GetRegistrationIssuerInfoByCommunity),
    // NOT the old registration's issuer. Blank when the community has no issuer.
    toMvr:         regOwner.value?.communityRegistrationIssuer || '',
    newReg:        newRegValue.value,
    variantMark:   '✕',
    // PREVIOUS registration block — the old plate + its issuer + validity.
    // curPlate = vehicle's stored plate (legacy LastRegistration); issuer/validity
    // come from the matching previous-registration row (NOT the newest).
    curPlate:      plateOrPrefix(b().previousRegistration?.plateNumber || b().vehicle?.plate),
    curIssuer:     b().previousRegistration?.issuer || '',
    // Prefer the vehicle's authoritative last-registration expiry (legacy
    // Vehicles.LastRegistrationValidTill); the registration row may be a sentinel.
    curValidTill:  fmtDate(b().vehicle?.lastRegistrationValidUntil || b().previousRegistration?.validUntil),
    // Macedonian DB: firstName = surname (Презиме), lastName = given name (Име).
    curSurname:    b().client?.firstName || '',
    curName:       b().client?.lastName || '',
    curAddress:    formatAddress(b().client?.address),
    curCommunity:  b().client?.communityName || b().client?.cityName || '',
    curEmbg:       b().client?.mb || '',
    ownershipProof:joinProofs(b().ownershipProofs),
    paymentProof:  joinProofs(b().paymentProofs),
    note:          cleanNote.value,
    company:       b().company?.name?.trim() || '',
    readyMark:     '__',
    referenceNo:   referenceValue.value,
  },
  page2: {
    maker:           b().vehicle?.maker || '',
    typeText:        b().vehicle?.typeText || '',
    model:           [b().vehicle?.model, b().vehicle?.variant].filter(Boolean).join(' '),
    vin:             b().vehicle?.vin || '',
    year:            b().vehicle?.manufactureDate
                       ? String(new Date(b().vehicle!.manufactureDate!).getFullYear())
                       : '',
    bodyType:        b().vehicle?.bodyType || '',
    engineTypeCode:  b().vehicle?.engineTypeCode || b().vehicle?.engineTypeName || '',
    cc:              num(b().vehicle?.engineWorkingCapacityCc),
    powerKw:         num(b().vehicle?.enginePowerKw),
    fuel:            b().vehicle?.fuelName || '',
    engineNumber:    b().vehicle?.engineNumber || '',
    emptyWeight:     num(b().vehicle?.emptyWeightKg),
    maxConstructive: num(b().vehicle?.maxConstructiveTotalMassKg),
    maxLegal:        num(b().vehicle?.maxLegalTotalMassKg),
    maxLegalGroup:   num(b().vehicle?.maxLegalGroupMassKg),
    length:          num(b().vehicle?.lengthMm),
    width:           num(b().vehicle?.widthMm),
    height:          num(b().vehicle?.heightMm),
    seats:           num(b().vehicle?.seats),
    standingSeats:   numZ(b().vehicle?.standingSeats),
    lyingSeats:      '0',
    color:           [b().vehicle?.primaryColorCode, b().vehicle?.primaryColorName].filter(Boolean).join(' '),

    // Per-axle masses (МасаПоОска) — not in our DTO; legacy prints "0" here.
    massAxle1: '0', massAxle2: '0', massAxle3: '0', massAxle4: '0', massAxle5: '0',
    // Axle loads (ОсноОптоварување) — 1-2 from data, 3-5 zero-fill.
    axleLoad1: numZ(b().vehicle?.axleLoad1Kg),
    axleLoad2: numZ(b().vehicle?.axleLoad2Kg),
    axleLoad3: '0', axleLoad4: '0', axleLoad5: '0',
    // Real technical specs (were wrongly hardcoded "0"; the first-reg HONDA proves
    // these carry real values: rpm, top speed, axle count, trailer masses, CO2, noise).
    z80:  numZ(b().vehicle?.maxRpm),                          // BrojNaVrtezi (rpm)
    z94:  numZ(b().vehicle?.maxSpeedKmh),                     // MaxSpeed
    z121: numZ(b().vehicle?.axleCount),                       // NumberOfAxis
    z172: numZ(b().vehicle?.maxTrailerBrakedKg),             // trailer w/ brakes (MaxKonstVkMasaKocnaPrikolka)
    z179: numZ(b().vehicle?.maxTrailerUnbrakedKg),           // trailer w/o brakes (MaxKonstVkMasaNeKocnaPrikolka)
    z218: numZ(b().vehicle?.co2GKm),                          // CO2
    // Confirmed against Honda 90517 (NoiseStatic=71, HitchLoad=75) vs the legacy
    // .prnx (top=75, bottom=71): the TOP box is the hitch load, the BOTTOM is noise.
    z226: numZ(b().vehicle?.maxHitchLoadKg),                 // max hitch load (MaxKonstOptovaruvanjeVoPriklucok)
    z237: numZ(b().vehicle?.noiseStaticDb),                   // noise (static, dB)
  },
  page3: {
    // New-registration issuer = the NEW owner's community MVR office (same source as
    // page-1 "ДО МВР"), not the old registration's issuer. Plate stays the newest row.
    newRegIssuer:  regOwner.value?.communityRegistrationIssuer || '',
    newPlate:      b().lastRegistration?.plateNumber || b().newVehicle?.plate || '',
    // New owner (newClient). Macedonian convention as above.
    newSurname:    regOwner.value?.firstName || '',
    newName:       regOwner.value?.lastName || '',
    newAddress:    formatAddress(regOwner.value?.address),
    newCommunity:  regOwner.value?.communityName || regOwner.value?.cityName || '',
    newEmbg:       regOwner.value?.mb || '',
  },
}));

// Category checkbox (variantMark) Y is driven by the payment category's legacy
// ZelenMap. From PrintPlav.Designer.vb CheckBoxVehicleCategory1-4 Locations
// (DPI 254 → /10 = mm), all at x=45.5:
//   1 = 105.8 (passenger/bus), 2 = 111.1 (cargo), 3 = 115.4 (trailer), 4 = 119.6 (moto).
// ZelenMap 0 or 5-11 has no checkbox on the form → hidden.
const CATEGORY_CHECKBOX_Y: Record<number, number> = { 1: 105.8, 2: 111.1, 3: 115.4, 4: 119.6 };
const categoryCheckboxY = computed<number | null>(() => {
  const zm = b().vehicle?.categoryZelenMap;
  return zm != null ? (CATEGORY_CHECKBOX_Y[zm] ?? null) : null;
});

// Resolve a field's style from the saved-layout composable, then re-apply the
// EXISTING data-driven dynamic shifts on top of the resolved y (identical logic
// to the pre-server-layout version — only the base x/y/size now come from resolve).
function fieldStyle(page: PageKey, key: string): Record<string, string> {
  const meta = DEFAULT_POS[page][key];
  const base = lay.resolve(`${page}.${key}`);
  const align = meta.align ?? 'left';
  const justify =
    align === 'right'  ? 'flex-end'   :
    align === 'center' ? 'center'     :
    'flex-start';
  // The category checkbox moves to the row matching the vehicle's ZelenMap.
  let top = base.y;
  if (key === 'variantMark' && categoryCheckboxY.value != null) top = categoryCheckboxY.value;
  // Rows below a (possibly wrapped) address shift down, like the legacy CanGrow.
  if (key === 'curCommunity' || key === 'curEmbg') top += page1AddrShiftMm.value;
  if (key === 'newCommunity' || key === 'newEmbg') top += page3AddrShiftMm.value;

  const style: Record<string, string> = {
    left:           base.x + 'mm',
    top:            top + 'mm',
    width:          meta.w + 'mm',
    height:         (meta.h ?? 5) + 'mm',
    fontSize:       base.size + 'pt',
    fontWeight:     meta.bold ? '700' : '400',
    textAlign:      align,
    justifyContent: justify,
  };
  // Address fields wrap to a 2nd line (top-aligned) instead of clipping, like legacy.
  if (key === 'curAddress' || key === 'newAddress') {
    style.whiteSpace = 'normal';
    style.alignItems = 'flex-start';
    style.height = 'auto';
    style.lineHeight = LINE_MM + 'mm';
  }
  return style;
}

const isTransfer = computed(() => b().type.transfersOwnership);

// The owner being registered (page 3):
//  - transfer        → the NEW owner (newClient)
//  - first reg / else → the sole owner (client)
// On a first registration ("по прв пат") there is no previous owner, so the
// page-1 "previous owner + registration" block is hidden and the only owner
// appears on page 3.
const regOwner = computed(() =>
  (isTransfer.value && b().newClient) ? b().newClient! : b().client);

// How far the community/EMBG rows shift down because the address wrapped.
// Page-1 address field width = 50.8mm, page-3 = 69.8mm.
const page1AddrShiftMm = computed(() =>
  (addressLineCount(formatAddress(b().client?.address), 50.8) - 1) * LINE_MM);
const page3AddrShiftMm = computed(() =>
  (addressLineCount(formatAddress(regOwner.value?.address), 69.8) - 1) * LINE_MM);

// Page-1 previous-owner / previous-registration block: only for transfers.
const PREV_KEYS = new Set([
  'curPlate', 'curIssuer', 'curValidTill',
  'curSurname', 'curName', 'curAddress', 'curCommunity', 'curEmbg',
]);
function shouldRenderP1(key: string): boolean {
  if (key === 'variantMark') return categoryCheckboxY.value != null;  // only categories with a checkbox
  return PREV_KEYS.has(key) ? isTransfer.value : true;
}

// Page 3 (the new/sole owner + new registration) is always present on Plav.
const showPage3 = computed(() => true);

onMounted(async () => {
  await lay.load();
});
</script>

<template>
  <!-- ============= PAGE 1 — FRONT ============= -->
  <article
    class="sheet"
    :ref="(el) => lay.setPageEl(el as HTMLElement | null)"
    :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }"
  >
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
    <div v-if="lay.editing.value && guides" class="grid-overlay no-print"></div>
    <div v-if="lay.editing.value && guides" class="page-label no-print">ПЛАВ · стр. 1 · ПРЕДНА</div>

    <template v-for="(p, key) in DEFAULT_POS.page1" :key="'p1:' + key">
      <span
        v-if="shouldRenderP1(key as string)"
        class="f"
        :class="{ bold: p.bold, sel: lay.selectedKey.value === 'page1.' + key }"
        :style="fieldStyle('page1', key as string)"
        :data-key="'page1:' + key"
        @pointerdown="lay.beginDrag('page1.' + key, $event)"
      >
        <span class="f-val">{{ (v.page1 as any)[key] }}</span>
        <span v-if="lay.editing.value && guides" class="f-tag no-print">{{ p.label || key }}</span>
      </span>
    </template>
  </article>

  <!-- ============= PAGE 2 — VEHICLE ============= -->
  <article class="sheet" :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
    <div v-if="lay.editing.value && guides" class="grid-overlay no-print"></div>
    <div v-if="lay.editing.value && guides" class="page-label no-print">ПЛАВ · стр. 2 · ВОЗИЛО</div>

    <template v-for="(p, key) in DEFAULT_POS.page2" :key="'p2:' + key">
      <span
        class="f"
        :class="{ bold: p.bold, sel: lay.selectedKey.value === 'page2.' + key }"
        :style="fieldStyle('page2', key as string)"
        :data-key="'page2:' + key"
        @pointerdown="lay.beginDrag('page2.' + key, $event)"
      >
        <span class="f-val">{{ (v.page2 as any)[key] }}</span>
        <span v-if="lay.editing.value && guides" class="f-tag no-print">{{ p.label || key }}</span>
      </span>
    </template>
  </article>

  <!-- ============= PAGE 3 — NEW OWNER ============= -->
  <article v-if="showPage3" class="sheet" :class="{ editing: lay.editing.value, guides: lay.editing.value && guides }">
      <PrintRulers v-if="lay.editing.value" :pos="lay.selectedPos.value" :width-mm="210" :height-mm="297" />
    <div v-if="lay.editing.value && guides" class="grid-overlay no-print"></div>
    <div v-if="lay.editing.value && guides" class="page-label no-print">ПЛАВ · стр. 3 · НОВ СОПСТВЕНИК</div>

    <template v-for="(p, key) in DEFAULT_POS.page3" :key="'p3:' + key">
      <span
        class="f"
        :class="{ bold: p.bold, sel: lay.selectedKey.value === 'page3.' + key }"
        :style="fieldStyle('page3', key as string)"
        :data-key="'page3:' + key"
        @pointerdown="lay.beginDrag('page3.' + key, $event)"
      >
        <span class="f-val">{{ (v.page3 as any)[key] }}</span>
        <span v-if="lay.editing.value && guides" class="f-tag no-print">{{ p.label || key }}</span>
      </span>
    </template>
  </article>

  <PrintLayoutToolbar v-if="lay.editing.value" :lay="lay" :name="'ПЛАВ образец'" v-model:guides="guides" />
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

/* Guide highlight (field outlines + labels) while editing with guides on. */
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
/* Drag affordance in edit mode. */
.sheet.editing .f {
  cursor: move;
  outline: 1px dashed rgba(37, 99, 235, .5);
  outline-offset: 0;
}
.sheet.editing .f:hover {
  outline: 2px solid rgba(37, 99, 235, .9);
  background: rgba(37, 99, 235, .06);
}
.sheet.editing .f.sel {
  outline: 1.5px solid #2563eb;
  background: rgba(37, 99, 235, .12);
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
  .sheet.guides .f, .sheet.editing .f {
    outline: none; background: transparent;
  }
  .f-tag { display: none !important }
  .no-print { display: none !important }
}
</style>
