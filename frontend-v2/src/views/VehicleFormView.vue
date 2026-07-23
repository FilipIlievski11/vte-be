<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type {
  Company, Country, Paged, TechExamListItem, Vehicle, VehicleBodyType, VehicleCategory, VehicleColor,
  VehicleEcoProgram, VehicleEngineType, VehicleFuel, VehicleMaker, VehicleModel,
  VehiclePaymentCategory, VehicleRegistration, VehicleRelationDto,
} from '@/types';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Textarea from 'primevue/textarea';
import Checkbox from 'primevue/checkbox';
import Select from 'primevue/select';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';
import Dialog from 'primevue/dialog';
import AutoComplete from 'primevue/autocomplete';
import DatePicker from 'primevue/datepicker';
import { useToast } from 'primevue/usetoast';

const props = defineProps<{ id?: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();
const auth = useAuthStore();

const isEdit = computed(() => !!props.id);
const loading = ref(false);
const saving = ref(false);
const errorBanner = ref<string | null>(null);

const form = ref<Partial<Omit<Vehicle, 'companyId' | 'active'>> & { companyId: number | null; active: boolean }>({
  companyId: null,
  vin: '',
  engineNumber: null,
  plate: null,
  categoryId: null, bodyTypeId: null, modelId: null,
  primaryColorId: null, secondaryColorId: null, madeCountryId: null,
  fuelId: null, secondFuelId: null,
  engineTypeId: null, ecoProgramId: null, paymentCategoryId: null,
  enginePowerKw: null, engineWorkingCapacityCc: null,
  maxRpm: null, maxSpeedKmh: null, hasLpg: false,
  lengthMm: null, widthMm: null, heightMm: null,
  emptyWeightKg: null, maxAllowedWeightKg: null,
  maxLegalTotalMassKg: null, maxConstructiveTotalMassKg: null,
  trailerMassWithBrakesKg: null, trailerMassWithoutBrakesKg: null,
  axleCount: null, wheelCount: null, axleLoad1Kg: null, axleLoad2Kg: null,
  seats: null, standingSeats: null,
  co2GKm: null, noiseStaticDb: null, noiseMovingDb: null,
  typeText: null, modelVariant: null, approvalMark: null,
  manufactureDate: null,
  note: null, active: true,
});

// Maker is a UI-only selector that drives the Model dropdown filter
const selectedMakerId = ref<number | null>(null);

// Годината се уредува како број; се чува како manufactureDate. Ако годината не е
// сменета, оригиналниот целосен датум останува недопрен.
const manufactureYear = ref<number | null>(null);

// Lookups
const companies     = ref<Company[]>([]);
const bodyTypes     = ref<VehicleBodyType[]>([]);
const categories    = ref<VehicleCategory[]>([]);
const makers        = ref<VehicleMaker[]>([]);
const models        = ref<VehicleModel[]>([]);     // filtered by maker
const colors        = ref<VehicleColor[]>([]);
const fuels         = ref<VehicleFuel[]>([]);
const ecoPrograms   = ref<VehicleEcoProgram[]>([]);
const engineTypes   = ref<VehicleEngineType[]>([]);
const paymentCats   = ref<VehiclePaymentCategory[]>([]);
const countries     = ref<Country[]>([]);

// Sub-grids
const owners        = ref<VehicleRelationDto[]>([]);
const registrations = ref<VehicleRegistration[]>([]);
const techExams     = ref<TechExamListItem[]>([]);

const filteredModels = computed(() =>
  selectedMakerId.value
    ? models.value.filter(m => m.makerId === selectedMakerId.value)
    : models.value
);

async function loadRefs() {
  const [bt, cat, mk, mdl, col, f, eco, et, pc, co] = await Promise.all([
    api.get<VehicleBodyType[]>('/vehicles/ref/body-types'),
    api.get<VehicleCategory[]>('/vehicles/ref/categories'),
    api.get<VehicleMaker[]>('/vehicles/ref/makers'),
    api.get<VehicleModel[]>('/vehicles/ref/models'),
    api.get<VehicleColor[]>('/vehicles/ref/colors'),
    api.get<VehicleFuel[]>('/vehicles/ref/fuels'),
    api.get<VehicleEcoProgram[]>('/vehicles/ref/eco-programs'),
    api.get<VehicleEngineType[]>('/vehicles/ref/engine-types'),
    api.get<VehiclePaymentCategory[]>('/vehicles/ref/payment-categories'),
    api.get<Country[]>('/countries'),
  ]);
  bodyTypes.value = bt.data;
  categories.value = cat.data;
  makers.value = mk.data;
  models.value = mdl.data;
  colors.value = col.data;
  fuels.value = f.data;
  ecoPrograms.value = eco.data;
  engineTypes.value = et.data;
  paymentCats.value = pc.data;
  countries.value = co.data;

  if (auth.isAdmin) {
    try {
      companies.value = (await api.get<Company[]>('/companies')).data;
      if (!isEdit.value && form.value.companyId == null && companies.value.length > 0) {
        form.value.companyId = companies.value[0].id;
      }
    } catch { /* ignore */ }
  }
}

async function loadVehicle() {
  if (!isEdit.value) return;
  const { data } = await api.get<Vehicle>(`/vehicles/${props.id}`);
  form.value = { ...data };
  manufactureYear.value = data.manufactureDate ? new Date(data.manufactureDate).getFullYear() : null;
  // Resolve current maker from the chosen model
  if (data.modelId) {
    const m = models.value.find(mm => mm.id === data.modelId);
    if (m) selectedMakerId.value = m.makerId;
  }
  // Load sub-grids
  const [own, reg, exams] = await Promise.all([
    api.get<VehicleRelationDto[]>(`/client-vehicle-relations?vehicleId=${data.id}`),
    api.get<VehicleRegistration[]>(`/vehicle-registrations?vehicleId=${data.id}`),
    api.get<Paged<TechExamListItem>>('/technical-exams', {
      params: { vehicleId: data.id, dir: 'desc', pageSize: 200 },
    }).catch(() => null),
  ]);
  owners.value = own.data;
  registrations.value = reg.data;
  techExams.value = exams?.data.items ?? [];
}

async function reloadOwners() {
  if (!isEdit.value) return;
  try {
    const { data } = await api.get<VehicleRelationDto[]>(`/client-vehicle-relations?vehicleId=${props.id}`);
    owners.value = data;
  } catch { /* non-fatal */ }
}

// =====================================================================
// Owner management: add a new owner; remove one — either into historical
// ownership (EndDate + inactive, row preserved) or fully deleted (backend
// refuses when documents reference the relation).
// =====================================================================
interface RelationTypeOpt { id: number; name: string; isOwner: boolean; isCustomerOnly: boolean }
const relationTypes = ref<RelationTypeOpt[]>([]);
// Add dialog
const addOwnerVisible = ref(false);
const addOwnerSaving = ref(false);
type ClientOpt = { id: number; label: string };
const ownerSel = ref<ClientOpt | string | null>(null);
const ownerSuggestions = ref<ClientOpt[]>([]);
const addRelationTypeId = ref<number | null>(null);
const addStartDate = ref<Date>(new Date());

async function openAddOwner() {
  if (!relationTypes.value.length) {
    try {
      const { data } = await api.get<RelationTypeOpt[]>('/client-vehicle-relations/types');
      relationTypes.value = data.filter(x => !x.isCustomerOnly);
    } catch { /* dialog still opens */ }
  }
  ownerSel.value = null;
  ownerSuggestions.value = [];
  addRelationTypeId.value = relationTypes.value.find(x => x.isOwner)?.id ?? relationTypes.value[0]?.id ?? null;
  addStartDate.value = new Date();
  addOwnerVisible.value = true;
}

async function onOwnerComplete(e: { query: string }) {
  const term = (e.query || '').trim();
  if (term.length < 2) { ownerSuggestions.value = []; return; }
  try {
    const { data } = await api.get<{ items: { id: number; firstName: string | null; middleName: string | null; lastName: string | null; mb: string | null }[] }>(
      '/clients', { params: { q: term, pageSize: 20 } });
    ownerSuggestions.value = data.items.map(c => ({
      id: c.id,
      label: [[c.firstName, c.middleName, c.lastName].filter(Boolean).join(' '), c.mb].filter(Boolean).join(' · '),
    }));
  } catch { ownerSuggestions.value = []; }
}

function toIsoDay(d: Date): string {
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
}

async function saveNewOwner() {
  const sel = ownerSel.value;
  if (!sel || typeof sel === 'string' || !addRelationTypeId.value) return;
  addOwnerSaving.value = true;
  try {
    await api.post('/client-vehicle-relations', {
      clientId: sel.id,
      vehicleId: Number(props.id),
      relationTypeId: addRelationTypeId.value,
      startDate: toIsoDay(addStartDate.value ?? new Date()),
      endDate: null, startNote: null, endNote: null, active: true,
    });
    addOwnerVisible.value = false;
    toast.add({ severity: 'success', summary: t('vehicles.owners.added'), life: 2200 });
    await reloadOwners();
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { addOwnerSaving.value = false; }
}

// Remove dialog (историска / целосно / откажи)
const removeVisible = ref(false);
const removeBusy = ref(false);
const removeTarget = ref<VehicleRelationDto | null>(null);
function openRemoveOwner(r: VehicleRelationDto) {
  removeTarget.value = r;
  removeVisible.value = true;
}
async function removeToHistory() {
  const r = removeTarget.value;
  if (!r) return;
  removeBusy.value = true;
  try {
    // send the operator's LOCAL today — the server's UTC date lags by a day at night
    await api.post(`/client-vehicle-relations/${r.id}/end`, { endDate: toIsoDay(new Date()) });
    removeVisible.value = false;
    toast.add({ severity: 'success', summary: t('vehicles.owners.movedToHistory'), life: 2500 });
    await reloadOwners();
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { removeBusy.value = false; }
}
async function removeCompletely() {
  const r = removeTarget.value;
  if (!r) return;
  removeBusy.value = true;
  try {
    await api.delete(`/client-vehicle-relations/${r.id}`);
    removeVisible.value = false;
    toast.add({ severity: 'success', summary: t('vehicles.owners.removed'), life: 2500 });
    await reloadOwners();
  } catch (e: any) {
    // typical: relation has documents → backend suggests историска
    toast.add({ severity: 'warn', summary: t('vehicles.owners.removeRefused'),
      detail: e?.response?.data?.error ?? e?.message, life: 6000 });
  } finally { removeBusy.value = false; }
}

onMounted(async () => {
  loading.value = true;
  try {
    await loadRefs();
    await loadVehicle();
  } catch (e: any) {
    errorBanner.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load.';
  } finally {
    loading.value = false;
  }
});

// When user changes the Maker selector, clear ModelId if it no longer belongs
watch(selectedMakerId, () => {
  if (selectedMakerId.value && form.value.modelId) {
    const m = models.value.find(mm => mm.id === form.value.modelId);
    if (!m || m.makerId !== selectedMakerId.value) form.value.modelId = null;
  }
});

async function save() {
  saving.value = true;
  errorBanner.value = null;
  try {
    const payload: Record<string, unknown> = { ...form.value };
    // Годината → manufactureDate: непроменета година го чува оригиналниот датум.
    const origYear = form.value.manufactureDate ? new Date(form.value.manufactureDate).getFullYear() : null;
    payload.manufactureDate = manufactureYear.value == null
      ? null
      : (manufactureYear.value === origYear ? form.value.manufactureDate : `${manufactureYear.value}-01-01`);
    // Don't send computed/echo fields
    delete payload.id; delete payload.createdAt; delete payload.companyId;
    if (auth.isAdmin && form.value.companyId != null) {
      payload.companyId = form.value.companyId;
    }

    let vehicleId: number;
    if (isEdit.value) {
      await api.put(`/vehicles/${props.id}`, payload);
      vehicleId = Number(props.id);
    } else {
      const { data } = await api.post<Vehicle>('/vehicles', payload);
      vehicleId = data.id;
    }

    toast.add({
      severity: 'success',
      summary: isEdit.value ? t('vehicles.form.saved') : t('vehicles.form.created'),
      detail: `Id ${vehicleId}`,
      life: 2500,
    });

    if (!isEdit.value) router.replace(`/vehicles/${vehicleId}`);
  } catch (e: any) {
    const msg = e?.response?.data?.error ?? e?.message ?? 'Save failed.';
    errorBanner.value = msg;
    toast.add({ severity: 'error', summary: t('vehicles.form.saveFailed'), detail: msg, life: 5000 });
  } finally {
    saving.value = false;
  }
}

const displayTitle = computed(() => {
  if (!isEdit.value) return t('vehicles.form.newTitle');
  const plate = form.value.plate?.trim();
  const vin = form.value.vin?.trim();
  const m = form.value.modelId ? models.value.find(mm => mm.id === form.value.modelId) : null;
  const mk = m ? makers.value.find(x => x.id === m.makerId) : null;
  const parts: string[] = [];
  if (plate) parts.push(plate);
  if (mk?.name) parts.push(mk.name);
  if (m?.name) parts.push(m.name);
  if (parts.length === 0 && vin) parts.push(vin);
  return parts.length ? parts.join(' · ') : `#${props.id}`;
});

function fmtDate(s: string | null | undefined) {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}
</script>

<template>
  <div class="vehicle-form">
    <div class="page-header">
      <div>
        <h1>{{ displayTitle }}</h1>
        <div class="subtitle">
          <Tag v-if="form.active === false" :value="t('common.no')" severity="danger" />
          <span v-else class="muted">{{ form.vin }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" /> Loading…</div>

    <template v-else>
      <!-- ===== Company (admin-only) ===== -->
      <div v-if="auth.isAdmin" class="card">
        <div class="card-header">{{ t('vehicles.sections.company') }}</div>
        <div class="card-body">
          <div class="field" style="max-width: 360px">
            <label>
              {{ t('clients.form.company') }}
              <i v-if="isEdit" class="pi pi-lock" style="font-size: 0.7rem; margin-left: 0.25rem; opacity: 0.6" />
            </label>
            <Select v-model="form.companyId" :options="companies" optionLabel="name" optionValue="id"
                    placeholder="—" filter :disabled="isEdit" />
            <span class="muted" style="font-size: 0.7rem">
              {{ isEdit ? t('clients.form.companyLocked') : t('clients.form.companyHint') }}
            </span>
          </div>
        </div>
      </div>

      <!-- ===== Identity ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.identity') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.vin') }} *</label>
              <InputText v-model="form.vin" maxlength="40" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.plate') }}</label>
              <InputText v-model="form.plate" maxlength="20" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.engineNumber') }}</label>
              <InputText v-model="form.engineNumber" maxlength="40" />
            </div>
            <div class="field" style="display: flex; align-items: center; padding-top: 1.4rem">
              <label style="display: flex; align-items: center; gap: 0.5rem; font-weight: 500">
                <Checkbox v-model="form.active" :binary="true" /> {{ t('vehicles.form.active') }}
              </label>
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Classification ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.classification') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.category') }}</label>
              <Select v-model="form.categoryId" :options="categories" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.bodyType') }}</label>
              <Select v-model="form.bodyTypeId" :options="bodyTypes" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.paymentCategory') }}</label>
              <Select v-model="form.paymentCategoryId" :options="paymentCats" optionLabel="name" optionValue="id" placeholder="—" showClear />
            </div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.maker') }}</label>
              <Select v-model="selectedMakerId" :options="makers" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.model') }}</label>
              <Select v-model="form.modelId" :options="filteredModels" optionLabel="name" optionValue="id" placeholder="—" showClear filter
                      :disabled="!selectedMakerId" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.madeCountry') }}</label>
              <Select v-model="form.madeCountryId" :options="countries" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.manufactureYear') }}</label>
              <InputNumber v-model="manufactureYear" :useGrouping="false" :min="1900" :max="2100" placeholder="—" />
            </div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.primaryColor') }}</label>
              <Select v-model="form.primaryColorId" :options="colors" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.secondaryColor') }}</label>
              <Select v-model="form.secondaryColorId" :options="colors" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Engine ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.engine') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.fuel') }}</label>
              <Select v-model="form.fuelId" :options="fuels" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.secondFuel') }}</label>
              <Select v-model="form.secondFuelId" :options="fuels" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.ecoProgram') }}</label>
              <Select v-model="form.ecoProgramId" :options="ecoPrograms" optionLabel="name" optionValue="id" placeholder="—" showClear />
            </div>
            <div class="field" style="display:flex; align-items:center; padding-top: 1.4rem">
              <label style="display:flex; align-items:center; gap:0.5rem">
                <Checkbox v-model="form.hasLpg" :binary="true" /> {{ t('vehicles.form.hasLpg') }}
              </label>
            </div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.engineType') }}</label>
              <Select v-model="form.engineTypeId" :options="engineTypes" optionLabel="name" optionValue="id" placeholder="—" showClear filter />
            </div>
            <div class="field"><label>{{ t('vehicles.form.enginePowerKw') }}</label>
              <InputNumber v-model="form.enginePowerKw" :minFractionDigits="0" :maxFractionDigits="2" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.engineCc') }}</label>
              <InputNumber v-model="form.engineWorkingCapacityCc" :minFractionDigits="0" :maxFractionDigits="0" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.maxRpm') }}</label>
              <InputNumber v-model="form.maxRpm" :useGrouping="false" />
            </div>
            <div class="field"><label>{{ t('vehicles.form.maxSpeed') }}</label>
              <InputNumber v-model="form.maxSpeedKmh" :minFractionDigits="0" :maxFractionDigits="0" />
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Mass & dimensions ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.mass') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.length') }}</label><InputNumber v-model="form.lengthMm" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.width') }}</label><InputNumber v-model="form.widthMm" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.height') }}</label><InputNumber v-model="form.heightMm" :useGrouping="false" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.emptyWeight') }}</label><InputNumber v-model="form.emptyWeightKg" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.maxAllowedWeight') }}</label><InputNumber v-model="form.maxAllowedWeightKg" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.maxLegalMass') }}</label><InputNumber v-model="form.maxLegalTotalMassKg" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.maxConstructiveMass') }}</label><InputNumber v-model="form.maxConstructiveTotalMassKg" :useGrouping="false" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.trailerMassWithBrakes') }}</label><InputText v-model="form.trailerMassWithBrakesKg" maxlength="40" /></div>
            <div class="field"><label>{{ t('vehicles.form.trailerMassWithoutBrakes') }}</label><InputText v-model="form.trailerMassWithoutBrakesKg" maxlength="40" /></div>
          </div>
        </div>
      </div>

      <!-- ===== Axles & seats ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.axles') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.axleCount') }}</label><InputNumber v-model="form.axleCount" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.wheelCount') }}</label><InputNumber v-model="form.wheelCount" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.axleLoad1') }}</label><InputNumber v-model="form.axleLoad1Kg" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.axleLoad2') }}</label><InputNumber v-model="form.axleLoad2Kg" :useGrouping="false" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.seats') }}</label><InputNumber v-model="form.seats" :useGrouping="false" /></div>
            <div class="field"><label>{{ t('vehicles.form.standingSeats') }}</label><InputNumber v-model="form.standingSeats" :useGrouping="false" /></div>
          </div>
        </div>
      </div>

      <!-- ===== Emissions & noise ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.emissions') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.co2') }}</label><InputNumber v-model="form.co2GKm" :minFractionDigits="0" :maxFractionDigits="2" /></div>
            <div class="field"><label>{{ t('vehicles.form.noiseStatic') }}</label><InputNumber v-model="form.noiseStaticDb" :minFractionDigits="0" :maxFractionDigits="1" /></div>
            <div class="field"><label>{{ t('vehicles.form.noiseMoving') }}</label><InputNumber v-model="form.noiseMovingDb" :minFractionDigits="0" :maxFractionDigits="1" /></div>
          </div>
        </div>
      </div>

      <!-- ===== Variant + note ===== -->
      <div class="card">
        <div class="card-header">{{ t('vehicles.sections.variant') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field"><label>{{ t('vehicles.form.typeText') }}</label><InputText v-model="form.typeText" maxlength="300" /></div>
            <div class="field"><label>{{ t('vehicles.form.modelVariant') }}</label><InputText v-model="form.modelVariant" maxlength="400" /></div>
            <div class="field"><label>{{ t('vehicles.form.approvalMark') }}</label><InputText v-model="form.approvalMark" maxlength="100" /></div>
          </div>
          <div class="field">
            <label>{{ t('vehicles.form.note') }}</label>
            <Textarea v-model="form.note" rows="2" autoResize maxlength="1000" />
          </div>
        </div>
      </div>

      <!-- ===== Owners ===== -->
      <div v-if="isEdit" class="card">
        <div class="card-header owners-header">
          {{ t('vehicles.sections.owners') }}
          <Button :label="t('vehicles.owners.add')" icon="pi pi-plus" size="small" outlined @click="openAddOwner" />
        </div>
        <div class="card-body">
          <DataTable :value="owners" size="small" stripedRows v-if="owners.length">
            <Column field="clientDisplayName" :header="t('vehicles.owners.colClient')">
              <template #body="{ data }">{{ data.clientDisplayName || `Client #${data.clientId}` }}</template>
            </Column>
            <Column field="relationTypeName" :header="t('vehicles.owners.colRelation')" />
            <Column :header="t('vehicles.owners.colFrom')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.startDate) }}</template>
            </Column>
            <Column :header="t('vehicles.owners.colTo')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.endDate) }}</template>
            </Column>
            <Column :header="t('vehicles.owners.colActive')" style="width: 90px" bodyStyle="text-align:center">
              <template #body="{ data }">
                <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
              </template>
            </Column>
            <Column style="width: 56px" bodyStyle="text-align:right">
              <template #body="{ data }">
                <Button v-if="data.active" icon="pi pi-trash" text rounded size="small" severity="danger"
                        v-tooltip.left="t('vehicles.owners.remove')" @click="openRemoveOwner(data)" />
              </template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 1.5rem">
            <i class="pi pi-users" /> {{ t('vehicles.owners.noOwners') }}
          </div>
        </div>
      </div>

      <!-- ===== Registrations (read-only) ===== -->
      <div v-if="isEdit" class="card">
        <div class="card-header">{{ t('vehicles.sections.registrations') }}</div>
        <div class="card-body">
          <DataTable :value="registrations" size="small" stripedRows v-if="registrations.length">
            <Column field="plateNumber" :header="t('vehicles.registrations.colPlate')" style="width: 140px" />
            <Column field="issuerName"  :header="t('vehicles.registrations.colIssuer')" />
            <Column :header="t('vehicles.registrations.colFrom')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.registeredDate) }}</template>
            </Column>
            <Column :header="t('vehicles.registrations.colTo')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.validUntil) }}</template>
            </Column>
            <Column :header="t('vehicles.registrations.colFirst')" style="width: 80px" bodyStyle="text-align:center">
              <template #body="{ data }">
                <i v-if="data.isFirstRegistration" class="pi pi-check" style="color: var(--color-success-600)"></i>
              </template>
            </Column>
            <Column :header="t('vehicles.registrations.colActive')" style="width: 90px" bodyStyle="text-align:center">
              <template #body="{ data }">
                <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
              </template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 1.5rem">
            <i class="pi pi-id-card" /> {{ t('vehicles.registrations.noRows') }}
          </div>
        </div>
      </div>

      <!-- ===== Историја на технички прегледи ===== -->
      <div v-if="isEdit" class="card">
        <div class="card-header">
          {{ t('vehicles.sections.techExams') }}
          <span v-if="techExams.length" class="count-chip">{{ techExams.length }}</span>
        </div>
        <div class="card-body">
          <DataTable :value="techExams" size="small" stripedRows v-if="techExams.length"
            rowHover scrollable scrollHeight="320px"
            @row-click="(e: any) => router.push(`/technical-exams/${e.data.id}`)"
            :pt="{ row: { style: 'cursor: pointer' } }" dataKey="id">
            <Column :header="t('vehicles.techExams.colDate')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.madeDate) }}</template>
            </Column>
            <Column field="typeName" :header="t('vehicles.techExams.colType')">
              <template #body="{ data }">{{ data.typeCode || data.typeName || '—' }}</template>
            </Column>
            <Column field="regNumber" :header="t('vehicles.techExams.colRegNumber')">
              <template #body="{ data }">{{ data.regNumber || '—' }}</template>
            </Column>
            <Column field="clientName" :header="t('vehicles.techExams.colClient')">
              <template #body="{ data }">{{ data.clientName || '—' }}</template>
            </Column>
            <Column :header="t('vehicles.techExams.colValidTill')" style="width: 110px">
              <template #body="{ data }">{{ fmtDate(data.validTillDate) }}</template>
            </Column>
            <Column :header="t('vehicles.techExams.colResult')" style="width: 110px" bodyStyle="text-align:center">
              <template #body="{ data }">
                <Tag :value="data.vehicleIsRight ? t('vehicles.techExams.pass') : t('vehicles.techExams.fail')"
                     :severity="data.vehicleIsRight ? 'success' : 'danger'" />
              </template>
            </Column>
            <Column style="width: 46px" bodyStyle="text-align:right">
              <template #body="{ data }">
                <Button icon="pi pi-external-link" text rounded size="small" severity="secondary"
                        v-tooltip.left="t('vehicles.techExams.open')"
                        @click.stop="router.push(`/technical-exams/${data.id}`)" />
              </template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 1.5rem">
            <i class="pi pi-clipboard" /> {{ t('vehicles.techExams.noRows') }}
          </div>
        </div>
      </div>

      <div v-if="errorBanner" class="error">{{ errorBanner }}</div>

      <div class="footer-actions">
        <Button :label="t('common.back')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/vehicles')" />
        <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" severity="success" @click="save" />
      </div>
    </template>
  </div>

  <!-- Додади сопственик -->
  <Dialog v-model:visible="addOwnerVisible" :header="t('vehicles.owners.add')" modal :style="{ width: '30rem' }">
    <div class="owner-dlg">
      <div class="field">
        <label>{{ t('vehicles.owners.client') }} *</label>
        <AutoComplete
          v-model="ownerSel" :suggestions="ownerSuggestions" optionLabel="label"
          :placeholder="t('vehicles.owners.clientPlaceholder')"
          @complete="onOwnerComplete" fluid />
      </div>
      <div class="field">
        <label>{{ t('vehicles.owners.relationType') }} *</label>
        <Select v-model="addRelationTypeId" :options="relationTypes" optionLabel="name" optionValue="id" fluid />
      </div>
      <div class="field">
        <label>{{ t('vehicles.owners.startDate') }}</label>
        <DatePicker v-model="addStartDate" dateFormat="dd.mm.yy" showIcon iconDisplay="input" fluid />
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined size="small" @click="addOwnerVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" size="small" :loading="addOwnerSaving"
              :disabled="!ownerSel || typeof ownerSel === 'string' || !addRelationTypeId" @click="saveNewOwner" />
    </template>
  </Dialog>

  <!-- Избриши сопственик: историска сопственост или целосно -->
  <Dialog v-model:visible="removeVisible" :header="t('vehicles.owners.removeTitle')" modal :style="{ width: '32rem' }">
    <div v-if="removeTarget" class="remove-dlg">
      <p class="remove-q">
        {{ t('vehicles.owners.removeQuestion', { name: removeTarget.clientDisplayName || `#${removeTarget.clientId}` }) }}
      </p>
      <div class="remove-options">
        <button class="remove-opt" :disabled="removeBusy" @click="removeToHistory">
          <i class="pi pi-history" />
          <span class="ro-title">{{ t('vehicles.owners.optHistory') }}</span>
          <span class="ro-sub">{{ t('vehicles.owners.optHistoryHint') }}</span>
        </button>
        <button class="remove-opt danger" :disabled="removeBusy" @click="removeCompletely">
          <i class="pi pi-trash" />
          <span class="ro-title">{{ t('vehicles.owners.optDelete') }}</span>
          <span class="ro-sub">{{ t('vehicles.owners.optDeleteHint') }}</span>
        </button>
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined size="small" :disabled="removeBusy" @click="removeVisible = false" />
    </template>
  </Dialog>
</template>

<style scoped>
/* Same v1-style cards as ClientFormView. */

/* Owner management */
.owners-header { display: flex; align-items: center; justify-content: space-between; }
.count-chip {
  margin-left: 0.45rem; font-size: 0.7rem; font-weight: 600;
  background: var(--p-primary-color); color: #fff;
  border-radius: 10px; padding: 0.05rem 0.45rem; vertical-align: middle;
}
.owner-dlg { display: flex; flex-direction: column; gap: 0.8rem; }
.owner-dlg .field { display: flex; flex-direction: column; gap: 0.25rem; }
.owner-dlg label { font-size: 0.8rem; color: var(--p-text-muted-color); }

.remove-q { margin: 0 0 0.9rem; font-size: 0.875rem; }
.remove-options { display: flex; flex-direction: column; gap: 0.5rem; }
.remove-opt {
  display: grid; grid-template-columns: auto 1fr; grid-template-rows: auto auto;
  column-gap: 0.6rem; align-items: center; text-align: left;
  padding: 0.6rem 0.8rem; border-radius: 10px; cursor: pointer;
  background: var(--p-content-background);
  border: 1px solid var(--p-content-border-color);
}
.remove-opt:hover:not(:disabled) { border-color: var(--p-primary-color); background: color-mix(in srgb, var(--p-primary-color) 5%, transparent); }
.remove-opt:disabled { opacity: 0.6; cursor: default; }
.remove-opt i { grid-row: span 2; font-size: 1rem; color: var(--p-primary-color); }
.remove-opt .ro-title { font-weight: 600; font-size: 0.85rem; }
.remove-opt .ro-sub { font-size: 0.72rem; color: var(--p-text-muted-color); }
.remove-opt.danger i { color: var(--p-red-500, #ef4444); }
.remove-opt.danger:hover:not(:disabled) { border-color: var(--p-red-500, #ef4444); background: color-mix(in srgb, #ef4444 5%, transparent); }
.vehicle-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }
.vehicle-form :deep(.page-header) { margin-bottom: 0.75rem; }
.vehicle-form :deep(.page-header h1) { font-size: 1.125rem; letter-spacing: -0.01em; }
.vehicle-form :deep(.page-header .subtitle) { font-size: 0.75rem; }
.vehicle-form :deep(.card + .card) { margin-top: 0.5rem; }
.vehicle-form :deep(.card .card-header) { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.vehicle-form :deep(.card .card-body) { padding: 0.625rem 0.875rem; }
.vehicle-form :deep(.row) { gap: 0.625rem; }
.vehicle-form :deep(.field) { gap: 0.15rem; margin-bottom: 0.45rem; }
.vehicle-form :deep(.field label) { font-size: 0.75rem; font-weight: 500; color: var(--color-text-secondary); }
.vehicle-form :deep(.p-select),
.vehicle-form :deep(.p-inputtext),
.vehicle-form :deep(.p-inputnumber) { width: 100%; }
.vehicle-form :deep(.p-inputtext) { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-inputnumber-input) { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.vehicle-form :deep(.p-textarea) { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-select-label) { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.vehicle-form :deep(.p-select-dropdown) { width: 1.625rem; }
.vehicle-form :deep(.p-checkbox) { transform: scale(0.85); transform-origin: left center; }

/* Brand accent stripe on each card header */
.vehicle-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.vehicle-form :deep(.card .card-header)::before {
  content: '';
  position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}
.vehicle-form :deep(.p-inputtext:focus),
.vehicle-form :deep(.p-inputnumber-input:focus),
.vehicle-form :deep(.p-textarea:focus),
.vehicle-form :deep(.p-select:not(.p-disabled).p-focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

.vehicle-form :deep(.empty) {
  display: flex; align-items: center; gap: 0.6rem;
  color: var(--color-text-muted); font-size: 0.85rem;
}

.footer-actions {
  position: fixed; bottom: 0; left: var(--sidebar-w); right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border);
  z-index: 5;
}
</style>
