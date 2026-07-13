<script setup lang="ts">
/**
 * Top-level print page for the colored registration forms (ПЛАВ / ЗЕЛЕН, with
 * БЕЛ falling back to ЗЕЛЕН until calibrated).
 *
 * Two modes:
 *  - NORMAL (no ?edit): fetch the request's print bundle by :id, render the right
 *    template, and auto-print. Only Print + Close chrome.
 *  - LAYOUT EDIT (?edit=1&form=plav|zelen): opened from the admin „Печатни обрасци"
 *    editor. Skips the fetch, feeds a representative SAMPLE bundle, and lets the
 *    chosen child render the shared PrintLayoutToolbar (drag / font / save to the
 *    server). No grey toolbar here — the child owns the edit chrome.
 */
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRoute } from 'vue-router';
import { api } from '@/api/client';
import type { RequestPrintBundle, PrintClientMeta, PrintVehicleMeta } from '@/types';
import ZelenTemplate from './ZelenTemplate.vue';
import PlavTemplate from './PlavTemplate.vue';

const props = defineProps<{ id: string }>();
const { t } = useI18n();
const route = useRoute();

// Layout-edit mode is entered by the admin editor via ?edit=1 (+ ?form=plav|zelen).
const editing = route.query.edit === '1' || route.query.edit === 'true';

const bundle = ref<RequestPrintBundle | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

/**
 * Normalize the document-print code to one of PLAV / BEL / ZELEN.
 * The C# seeder uses Latin codes ("PLAV", "BEL", "ZELEN") but migrate-request-
 * catalogs.sql derives codes from the legacy Cyrillic Opis ("ПЛАВ", "БЕЛ",
 * "ЗЕЛЕН"), so migrated requests carry Cyrillic codes. Match both scripts.
 * Order matters: check ZELEN/PLAV before BEL so Latin "BEL" can't shadow them.
 */
const formKind = computed<'PLAV' | 'BEL' | 'ZELEN' | 'OTHER'>(() => {
  const c = (bundle.value?.type.documentPrintCode || '').toUpperCase();
  if (c.includes('ZELEN') || c.includes('ЗЕЛЕН')) return 'ZELEN';
  if (c.includes('PLAV')  || c.includes('ПЛАВ'))  return 'PLAV';
  if (c.includes('BEL')   || c.includes('БЕЛ'))   return 'BEL';
  return 'OTHER';
});

// Which child to render. In edit mode the admin picks it via ?form; otherwise it
// comes from the request's document-print code. (BEL/OTHER still fall back to Zelen.)
const activeTemplate = computed<'PLAV' | 'ZELEN'>(() => {
  if (editing) return (String(route.query.form || '').toLowerCase() === 'plav') ? 'PLAV' : 'ZELEN';
  return formKind.value === 'PLAV' ? 'PLAV' : 'ZELEN';
});
const isUncalibrated = computed(() => !editing && formKind.value !== 'PLAV' && formKind.value !== 'ZELEN');

onMounted(async () => {
  if (editing) {
    // Layout-edit mode: representative sample data, never fetch or auto-print.
    bundle.value = sampleBundle();
    loading.value = false;
    return;
  }
  try {
    const { data } = await api.get<RequestPrintBundle>(`/requests/${props.id}/print`);
    bundle.value = data;
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load';
  } finally {
    loading.value = false;
  }
});

function doPrint() { setTimeout(() => window.print(), 50); }
function doClose() { window.close(); }

// ---------------------------------------------------------------------------
//  Representative sample bundle for the layout editor (Macedonian values).
//  Covers a transfer-of-ownership + new-registration request so every value
//  field on both forms (page-1 previous block, page-3 new owner, Zelen Б panel)
//  is populated and draggable.
// ---------------------------------------------------------------------------
function sampleClient(kind: 'current' | 'new'): PrintClientMeta {
  const cur = kind === 'current';
  return {
    id: cur ? 1001 : 1002,
    fullName: cur ? 'ПЕТРОВСКИ ПЕТАР' : 'СТОЈАНОВСКА МАРИЈА',
    firstName: cur ? 'ПЕТРОВСКИ' : 'СТОЈАНОВСКА',   // Macedonian DB: firstName = surname
    middleName: null,
    lastName: cur ? 'ПЕТАР' : 'МАРИЈА',              // lastName = given name
    mb: cur ? '0101985450012' : '1503990455023',
    taxNumber: null,
    address: cur ? 'Браќа Миладиновци 21/3' : 'Маршал Тито 118',
    cityName: cur ? 'Велес' : 'Скопје',
    communityName: cur ? 'Велес' : 'Центар',
    countryName: 'Македонија',
    citizenshipName: 'Македонско',
    phoneNumber: null,
    email: null,
    isBusiness: false,
    dateOfBirth: null,
    communityRegistrationCode: cur ? 'VE' : 'SK',
    communityRegistrationIssuer: cur ? 'МВР ВЕЛЕС' : 'МВР ЦЕНТАР',
  };
}

function sampleVehicle(): PrintVehicleMeta {
  return {
    id: 5001,
    vin: 'WVWZZZ1KZAW000123',
    engineNumber: 'CAY123456',
    plate: 'VE-1234-AB',
    maker: 'VOLKSWAGEN',
    model: 'GOLF',
    modelCode: '1K',
    variant: '1.6 TDI',
    typeText: 'J 1K / 1KM / 1KMV',
    bodyType: 'ЗАТВОРЕНА ЛИМУЗИНА',
    category: 'M1',
    categoryForPayments: 'ПАТНИЧКО',
    categoryZelenMap: 1,
    primaryColorName: 'СИВА',
    primaryColorCode: 'X7',
    secondaryColorName: null,
    secondaryColorCode: null,
    fuelName: 'ДИЗЕЛ',
    engineTypeName: 'ДИЗЕЛ',
    engineTypeCode: 'CAYB',
    ecoProgramName: 'ЕВРО 5',
    enginePowerKw: 77,
    engineWorkingCapacityCc: 1598,
    emptyWeightKg: 1320,
    maxAllowedWeightKg: 1900,
    seats: 5,
    standingSeats: 0,
    madeCountry: 'ГЕРМАНИЈА',
    hasLpg: false,
    manufactureDate: '2015-06-01T00:00:00',
    lengthMm: 4255,
    widthMm: 1799,
    heightMm: 1452,
    maxLegalTotalMassKg: 1900,
    maxConstructiveTotalMassKg: 1950,
    maxLegalGroupMassKg: 3130,
    axleCount: 2,
    maxRpm: 4000,
    maxSpeedKmh: 190,
    co2GKm: 109,
    noiseStaticDb: 71,
    axleLoad1Kg: 950,
    axleLoad2Kg: 900,
    maxTrailerBrakedKg: 1500,
    maxTrailerUnbrakedKg: 640,
    maxHitchLoadKg: 75,
    approvalMark: 'e1*2007/46*0001',
    lastRegistrationValidUntil: '2026-05-31T00:00:00',
  };
}

function sampleBundle(): RequestPrintBundle {
  return {
    request: {
      id: 12345,
      companyId: 37,
      requestTypeId: 1,
      clientVehicleRelationId: 9001,
      newClientVehicleRelationId: 9002,
      technicalExamReportId: null,
      previousRegistrationId: 7001,
      createdAt: '2026-06-15T09:30:00',
      modifiedAt: null,
      endedAt: null,
      createdByUserId: 'sample',
      modifiedByUserId: null,
      endedByUserId: null,
      createdByUserName: 'Оператор',
      modifiedByUserName: null,
      endedByUserName: null,
      vehicleDataChanged: false,
      clientDataChanged: true,
      note: '[legacy operator ids C=119]',
      active: true,
      legacyReferenceNumber: null,
      rowVersion: '',
    },
    type: {
      id: 1,
      name: 'ПРЕНОС НА СОПСТВЕНОСТ + НОВА РЕГИСТРАЦИЈА',
      description: null,
      documentPrintId: 1,
      documentPrintCode: 'PLAV',
      documentPrintName: 'ПЛАВ образец',
      transfersOwnership: true,
      deactivatesRelation: false,
      deactivatesVehicle: false,
      issuesNewRegistration: true,
    },
    client: sampleClient('current'),
    vehicle: sampleVehicle(),
    newVehicle: null,
    newClient: sampleClient('new'),
    ownershipProofs: [
      { id: 1, typeName: 'СООБРАЌАЈНА ДОЗВОЛА', detail: '2446652' },
      { id: 2, typeName: 'ДОГОВОР ЗА КУПОПРОДАЖБА', detail: null },
    ],
    paymentProofs: [
      { id: 3, typeName: 'ПОТВРДА ЗА ПЛАТЕНИ ДАВАЧКИ', detail: '00123/2026' },
    ],
    company: { id: 37, name: 'АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС', communityName: 'Велес' },
    lastRegistration: {
      id: 7002,
      plateNumber: 'SK-000-AA',
      registeredDate: '2026-06-15T00:00:00',
      validUntil: '2027-06-15T00:00:00',
      issuer: 'МВР ЦЕНТАР',
    },
    previousRegistration: {
      id: 7001,
      plateNumber: 'VE-1234-AB',
      registeredDate: '2025-06-01T00:00:00',
      validUntil: '2026-05-31T00:00:00',
      issuer: 'МВР ВЕЛЕС',
    },
  };
}
</script>

<template>
  <div class="print-wrap" :class="{ 'edit-mode': editing }">
    <div v-if="loading" class="status">{{ t('requests.print.loading') }}</div>
    <div v-else-if="error" class="status err">{{ error }}</div>

    <template v-else-if="bundle">
      <!-- Print chrome only in normal mode; the layout editor uses the shared toolbar. -->
      <div v-if="!editing" class="toolbar no-print">
        <span class="toolbar-title">
          {{ bundle.type.documentPrintName }} · {{ t('requests.print.no') }} {{ bundle.request.id }}
        </span>
        <button @click="doPrint">{{ t('requests.print.printAgain') }}</button>
        <button @click="doClose">{{ t('requests.print.close') }}</button>
      </div>

      <PlavTemplate v-if="activeTemplate === 'PLAV'" :bundle="bundle" />
      <ZelenTemplate v-else :bundle="bundle" />

      <div v-if="isUncalibrated" class="fallback-note no-print">
        ⚠ Шаблонот за <strong>{{ bundle.type.documentPrintCode }}</strong> сè уште
        не е калибриран — се прикажува со зелениот шаблон.
      </div>
    </template>
  </div>
</template>

<style scoped>
.print-wrap {
  background: #d4d4d8;
  min-height: 100vh;
  padding: 3rem 0 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
}
/* In edit mode there's no grey top toolbar, so drop the top padding. */
.print-wrap.edit-mode { padding: 1rem 0; }
.status { padding: 4rem 0; font-family: system-ui, sans-serif }
.status.err { color: #b91c1c }

.toolbar.no-print {
  position: fixed;
  top: 0; left: 0; right: 0;
  z-index: 999;
  display: flex; gap: .5rem; align-items: center;
  padding: .5rem .75rem;
  background: #1f2937; color: #fff;
  font-family: system-ui, sans-serif; font-size: .85rem;
}
.toolbar-title { flex: 1; font-weight: 600 }
.toolbar button {
  padding: .3rem .75rem; border: 1px solid #6b7280; background: #374151;
  color: #fff; border-radius: 4px; cursor: pointer; font-size: .8rem;
}
.toolbar button:hover { background: #4b5563 }

.fallback-note {
  margin: 1rem 0;
  padding: .5rem 1rem;
  background: #fef3c7; border: 1px solid #f59e0b; border-radius: 6px;
  font-family: system-ui, sans-serif; font-size: .85rem;
  color: #78350f;
}

@media print {
  html, body { margin: 0; padding: 0; background: #fff; }
  .print-wrap { background: #fff; padding: 0; min-height: 0; display: block; }
  .no-print { display: none !important }
  @page { size: A4 portrait; margin: 0 }
}
</style>
