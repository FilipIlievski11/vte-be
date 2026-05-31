<script setup lang="ts">
import { computed, onMounted, ref, reactive, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import type {
  Client, Paged, VehicleRelationDto,
  TechExamType, TechExamReportFull, TechExamWrite, TechExamDetailWrite,
  TechExamOrgLookup, TechExamStatusLookup, TechExamPartLookup, TechExamControllerLookup,
} from '@/types';
import Button from 'primevue/button';
import Card from 'primevue/card';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Select from 'primevue/select';
import InputNumber from 'primevue/inputnumber';
import Checkbox from 'primevue/checkbox';
import DatePicker from 'primevue/datepicker';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const props = defineProps<{ id?: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();
const confirm = useConfirm();

const isEdit = computed(() => !!props.id);
const loading = ref(false);
const saving = ref(false);

// ---- Catalogs ----
const types = ref<TechExamType[]>([]);
const organizations = ref<TechExamOrgLookup[]>([]);
const controllers = ref<TechExamControllerLookup[]>([]);
const parts = ref<TechExamPartLookup[]>([]);
const statuses = ref<TechExamStatusLookup[]>([]);

// ---- Header state ----
const technicalExamTypeId = ref<number | null>(null);
const organizationId = ref<number | null>(null);
const madeDate = ref<Date | null>(new Date());
const firstControllerLegacyId = ref<number | null>(null);
const secondControllerLegacyId = ref<number | null>(null);
const regNumber = ref<string | null>(null);

// ---- Notes ----
const explanationNote = ref('');
const driversWarning = ref('');
const note = ref('');
const technicalChanges = ref('');

// ---- Anchor: client → relation ----
const clientQuery = ref('');
const clientResults = ref<Client[]>([]);
const selectedClient = ref<Client | null>(null);
const relations = ref<VehicleRelationDto[]>([]);
const selectedRelationId = ref<number | null>(null);
let clientSearchTimer: number | undefined;

// ---- Brake-force grid (5 rows: axle 1-4 + parking=0) ----
interface AxleRow { axle: number; left: number | null; gj: number | null; leftRightDiff: number | null; coefficient: number | null; right: number | null }
function blankAxles(): AxleRow[] {
  return [1, 2, 3, 4, 0].map(a => ({ axle: a, left: null, gj: null, leftRightDiff: null, coefficient: null, right: null }));
}
const axles = ref<AxleRow[]>(blankAxles());

// ---- Summary measurements (keys match TechExamWrite) ----
const meas = reactive({
  weight: null as number | null,
  effectOfWorkingBrakeEmpty: null as number | null,
  effectOfWorkingBrakeFull: null as number | null,
  effectOfSecondaryBrake: null as number | null,
  effectOfParkingBrake: null as number | null,
  co: null as number | null,
  coPlusTurns: null as number | null,
  engineRpm: null as number | null,
  lambda: null as number | null,
  pinpoints: null as number | null,
  speedOfTurns: null as number | null,
  noise: null as number | null,
  engineOilTemp: null as number | null,
});
const measFields: { key: keyof typeof meas; labelKey: string }[] = [
  { key: 'weight', labelKey: 'techExam.weight' },
  { key: 'effectOfWorkingBrakeEmpty', labelKey: 'techExam.workingBrakeEmpty' },
  { key: 'effectOfWorkingBrakeFull', labelKey: 'techExam.workingBrakeFull' },
  { key: 'effectOfSecondaryBrake', labelKey: 'techExam.secondaryBrake' },
  { key: 'effectOfParkingBrake', labelKey: 'techExam.parkingBrake' },
  { key: 'co', labelKey: 'techExam.co' },
  { key: 'coPlusTurns', labelKey: 'techExam.coPlusTurns' },
  { key: 'engineRpm', labelKey: 'techExam.engineRpm' },
  { key: 'lambda', labelKey: 'techExam.lambda' },
  { key: 'pinpoints', labelKey: 'techExam.pinpoints' },
  { key: 'speedOfTurns', labelKey: 'techExam.speedOfTurns' },
  { key: 'noise', labelKey: 'techExam.noise' },
  { key: 'engineOilTemp', labelKey: 'techExam.engineOilTemp' },
];

// ---- Defect lines ----
interface DefectRow { vehiclePartId: number | null; statusId: number | null; front: boolean; back: boolean; onLeft: boolean; onRight: boolean; note: string }
const details = ref<DefectRow[]>([]);

// Braking-effect measurements are collapsed by default (secondary, often left blank).
const brakeCollapsed = ref(true);

// ---- Derived ----
const selectedType = computed<TechExamType | null>(() =>
  technicalExamTypeId.value != null ? types.value.find(x => x.id === technicalExamTypeId.value) ?? null : null);

const validTillDisplay = computed(() => {
  if (!madeDate.value) return '—';
  const days = selectedType.value?.validDays && selectedType.value.validDays > 0 ? selectedType.value.validDays : 365;
  const d = new Date(madeDate.value);
  d.setDate(d.getDate() + days);
  return d.toLocaleDateString();
});

// Pass/fail mirrors backend DerivePass: no lines OR every line "исправен" (status 1).
const vehicleIsRight = computed(() =>
  details.value.length === 0 || details.value.every(d => d.statusId === 1));

const typeOptions = computed(() =>
  types.value.map(x => ({ id: x.id, label: x.code ? `${x.code} · ${x.description}` : x.description })));
const orgOptions = computed(() =>
  organizations.value.map(o => ({ id: o.id, label: o.name || o.code || `#${o.id}` })));
const partOptions = computed(() =>
  parts.value.map(p => ({ id: p.id, label: p.code ? `${p.code} · ${p.description}` : p.description })));

function axleLabel(axle: number): string {
  switch (axle) {
    case 1: return t('techExam.axle1');
    case 2: return t('techExam.axle2');
    case 3: return t('techExam.axle3');
    case 4: return t('techExam.axle4');
    default: return t('techExam.axleParking');
  }
}

// ---- Catalog + edit-load ----
async function loadCatalogs() {
  const [ty, og, ct, pa, st] = await Promise.all([
    api.get<TechExamType[]>('/technical-exams/types'),
    api.get<TechExamOrgLookup[]>('/technical-exams/organizations'),
    api.get<TechExamControllerLookup[]>('/technical-exams/controllers'),
    api.get<TechExamPartLookup[]>('/technical-exams/vehicle-parts'),
    api.get<TechExamStatusLookup[]>('/technical-exams/detail-statuses'),
  ]);
  types.value = ty.data;
  organizations.value = og.data;
  controllers.value = ct.data;
  parts.value = pa.data;
  statuses.value = st.data;
}

function toDate(s: string | null | undefined): Date | null {
  return s ? new Date(s) : null;
}
function toDateOnly(d: Date | null): string | null {
  if (!d) return null;
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

async function loadReport() {
  if (!props.id) return;
  loading.value = true;
  try {
    const { data: r } = await api.get<TechExamReportFull>(`/technical-exams/${props.id}`);
    technicalExamTypeId.value = r.technicalExamTypeId;
    organizationId.value = r.organizationId;
    madeDate.value = toDate(r.madeDate);
    firstControllerLegacyId.value = r.firstControllerLegacyId;
    secondControllerLegacyId.value = r.secondControllerLegacyId;
    regNumber.value = r.regNumber;
    explanationNote.value = r.explanationNote ?? '';
    driversWarning.value = r.driversWarning ?? '';
    note.value = r.note ?? '';
    technicalChanges.value = r.technicalChanges ?? '';

    // Axles
    const rows = blankAxles();
    for (const a of r.axles ?? []) {
      const row = rows.find(x => x.axle === a.axle);
      if (row) { row.left = a.left; row.gj = a.gj; row.leftRightDiff = a.leftRightDiff; row.coefficient = a.coefficient; row.right = a.right; }
    }
    axles.value = rows;

    // Measurements
    meas.weight = r.weight;
    meas.effectOfWorkingBrakeEmpty = r.effectOfWorkingBrakeEmpty;
    meas.effectOfWorkingBrakeFull = r.effectOfWorkingBrakeFull;
    meas.effectOfSecondaryBrake = r.effectOfSecondaryBrake;
    meas.effectOfParkingBrake = r.effectOfParkingBrake;
    meas.co = r.co;
    meas.coPlusTurns = r.coPlusTurns;
    meas.engineRpm = r.engineRpm;
    meas.lambda = r.lambda;
    meas.pinpoints = r.pinpoints;
    meas.speedOfTurns = r.speedOfTurns;
    meas.noise = r.noise;
    meas.engineOilTemp = r.engineOilTemp;

    // Defects
    details.value = (r.details ?? []).map(d => ({
      vehiclePartId: d.vehiclePartId, statusId: d.statusId,
      front: d.front, back: d.back, onLeft: d.onLeft, onRight: d.onRight, note: d.note ?? '',
    }));

    // Anchor hydration
    selectedRelationId.value = r.customerVehicleRelationId;
    if (r.customerVehicleRelationId) {
      const { data: rel } = await api.get<VehicleRelationDto>(`/client-vehicle-relations/${r.customerVehicleRelationId}`)
        .catch(() => ({ data: null as unknown as VehicleRelationDto }));
      if (rel?.clientId) {
        const { data: c } = await api.get<Client>(`/clients/${rel.clientId}`);
        selectedClient.value = c;
        await loadRelationsForClient(c.id);
      }
    }
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('techExam.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

// ---- Anchor search ----
async function searchClients() {
  const term = clientQuery.value.trim();
  if (term.length < 2) { clientResults.value = []; return; }
  try {
    const { data } = await api.get<Paged<Client>>('/clients', { params: { q: term, pageSize: 15 } });
    clientResults.value = data.items;
  } catch { /* ignore */ }
}
watch(clientQuery, () => {
  window.clearTimeout(clientSearchTimer);
  clientSearchTimer = window.setTimeout(searchClients, 250);
});
async function loadRelationsForClient(clientId: number) {
  try {
    const { data } = await api.get<VehicleRelationDto[]>('/client-vehicle-relations', { params: { clientId, activeOnly: true } });
    relations.value = data;
  } catch { relations.value = []; }
}
function chooseClient(c: Client) {
  selectedClient.value = c;
  clientResults.value = [];
  clientQuery.value = '';
  selectedRelationId.value = null;
  loadRelationsForClient(c.id);
}
function clearClient() {
  if (isEdit.value) return;
  selectedClient.value = null;
  relations.value = [];
  selectedRelationId.value = null;
}
function clientDisplayName(c: Client): string {
  return [c.firstName, c.middleName, c.lastName].filter(Boolean).join(' ').trim() || `#${c.id}`;
}
function relationLabel(r: VehicleRelationDto): string {
  const parts: string[] = [];
  if (r.relationTypeName) parts.push(r.relationTypeName);
  if (r.vehiclePlate) parts.push(r.vehiclePlate);
  if (r.vehicleMaker || r.vehicleModel) parts.push([r.vehicleMaker, r.vehicleModel].filter(Boolean).join(' '));
  if (r.vehicleVin) parts.push(`VIN ${r.vehicleVin}`);
  return parts.join(' · ') || `#${r.id}`;
}

// ---- Defect handlers ----
function addDefect() {
  details.value.push({
    vehiclePartId: parts.value[0]?.id ?? null,
    statusId: statuses.value[0]?.id ?? null,
    front: false, back: false, onLeft: false, onRight: false, note: '',
  });
}
function removeDefect(idx: number) {
  details.value.splice(idx, 1);
}

// ---- Save / delete ----
function validate(): string | null {
  if (!technicalExamTypeId.value) return t('techExam.form.typeRequired');
  if (!organizationId.value) return t('techExam.form.orgRequired');
  if (!madeDate.value) return t('techExam.form.dateRequired');
  if (!selectedRelationId.value) return t('techExam.form.relationRequired');
  for (const d of details.value) {
    if (!d.vehiclePartId || !d.statusId) return t('techExam.form.defectIncomplete');
  }
  return null;
}

function buildBody(): TechExamWrite {
  const byAxle = (n: number) => axles.value.find(a => a.axle === n)!;
  const a1 = byAxle(1), a2 = byAxle(2), a3 = byAxle(3), a4 = byAxle(4), ap = byAxle(0);
  const detailWrites: TechExamDetailWrite[] = details.value.map(d => ({
    vehiclePartId: d.vehiclePartId!, statusId: d.statusId!,
    front: d.front, back: d.back, onLeft: d.onLeft, onRight: d.onRight,
    note: d.note?.trim() ? d.note.trim() : null,
  }));
  return {
    customerVehicleRelationId: selectedRelationId.value,
    technicalExamTypeId: technicalExamTypeId.value!,
    organizationId: organizationId.value!,
    madeDate: toDateOnly(madeDate.value)!,
    firstControllerLegacyId: firstControllerLegacyId.value,
    secondControllerLegacyId: secondControllerLegacyId.value,
    explanationNote: explanationNote.value || null,
    driversWarning: driversWarning.value || null,
    note: note.value || null,
    technicalChanges: technicalChanges.value || null,
    axis1Left: a1.left, axis1Right: a1.right, axis1Gj: a1.gj, axis1LeftRightDiff: a1.leftRightDiff, axis1Coefficient: a1.coefficient,
    axis2Left: a2.left, axis2Right: a2.right, axis2Gj: a2.gj, axis2LeftRightDiff: a2.leftRightDiff, axis2Coefficient: a2.coefficient,
    axis3Left: a3.left, axis3Right: a3.right, axis3Gj: a3.gj, axis3LeftRightDiff: a3.leftRightDiff, axis3Coefficient: a3.coefficient,
    axis4Left: a4.left, axis4Right: a4.right, axis4Gj: a4.gj, axis4LeftRightDiff: a4.leftRightDiff, axis4Coefficient: a4.coefficient,
    axisParkingLeft: ap.left, axisParkingRight: ap.right, axisParkingGj: ap.gj, axisParkingLeftRightDiff: ap.leftRightDiff, axisParkingCoefficient: ap.coefficient,
    weight: meas.weight,
    effectOfWorkingBrakeEmpty: meas.effectOfWorkingBrakeEmpty,
    effectOfWorkingBrakeFull: meas.effectOfWorkingBrakeFull,
    effectOfSecondaryBrake: meas.effectOfSecondaryBrake,
    effectOfParkingBrake: meas.effectOfParkingBrake,
    speedOfTurns: meas.speedOfTurns,
    co: meas.co,
    engineRpm: meas.engineRpm,
    coPlusTurns: meas.coPlusTurns,
    lambda: meas.lambda,
    pinpoints: meas.pinpoints,
    noise: meas.noise,
    engineOilTemp: meas.engineOilTemp,
    details: detailWrites,
  };
}

async function save() {
  const err = validate();
  if (err) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: err, life: 4000 });
    return;
  }
  saving.value = true;
  try {
    if (isEdit.value) {
      await api.put(`/technical-exams/${props.id}`, buildBody());
      toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
      router.push(`/technical-exams/${props.id}`);
    } else {
      const { data } = await api.post<TechExamReportFull>('/technical-exams', buildBody());
      toast.add({ severity: 'success', summary: t('techExam.form.created'), life: 1500 });
      router.replace(`/technical-exams/${data.id}`);
    }
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    saving.value = false;
  }
}

function remove() {
  if (!isEdit.value) return;
  confirm.require({
    message: t('techExam.form.deleteConfirm'),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/technical-exams/${props.id}`);
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
        router.push('/technical-exams');
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

onMounted(async () => {
  await loadCatalogs();
  if (isEdit.value) await loadReport();
});
</script>

<template>
  <div class="page-header">
    <div>
      <h1>
        <span v-if="isEdit">{{ t('techExam.form.editTitle', { reg: regNumber || ('#' + props.id) }) }}</span>
        <span v-else>{{ t('techExam.form.newTitle') }}</span>
      </h1>
      <div class="subtitle">
        <Tag :value="vehicleIsRight ? t('techExam.pass') : t('techExam.fail')"
             :severity="vehicleIsRight ? 'success' : 'danger'" />
        <span class="muted">&nbsp;·&nbsp;{{ t('techExam.validTill') }}: {{ validTillDisplay }}</span>
      </div>
    </div>
    <div class="actions">
      <Button :label="t('common.back')" icon="pi pi-arrow-left" severity="secondary" size="small" outlined
              @click="router.push(isEdit ? `/technical-exams/${props.id}` : '/technical-exams')" />
      <Button v-if="isEdit" :label="t('common.delete')" icon="pi pi-trash" severity="danger" size="small" outlined @click="remove" />
      <Button :label="t('common.save')" icon="pi pi-check" size="small" :loading="saving" @click="save" />
    </div>
  </div>

  <div v-if="loading" class="muted pad">{{ t('common.loading') }}…</div>

  <div v-else class="cards-grid">
    <!-- Header -->
    <Card class="card">
      <template #title>{{ t('techExam.form.sections.header') }}</template>
      <template #content>
        <div class="grid two-col">
          <div class="field">
            <label>{{ t('techExam.type') }} *</label>
            <Select v-model="technicalExamTypeId" :options="typeOptions" optionLabel="label" optionValue="id"
                    :placeholder="t('techExam.form.pickType')" />
          </div>
          <div class="field">
            <label>{{ t('techExam.station') }} *</label>
            <Select v-model="organizationId" :options="orgOptions" optionLabel="label" optionValue="id"
                    filter :placeholder="t('techExam.form.pickOrg')" />
          </div>
          <div class="field">
            <label>{{ t('techExam.madeDate') }} *</label>
            <DatePicker v-model="madeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
          </div>
          <div class="field">
            <label>{{ t('techExam.validTill') }}</label>
            <div class="val readonly">{{ validTillDisplay }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.controller1') }}</label>
            <Select v-model="firstControllerLegacyId" :options="controllers" optionLabel="fullName" optionValue="id"
                    filter showClear :placeholder="t('techExam.form.pickController')" />
          </div>
          <div class="field">
            <label>{{ t('techExam.controller2') }}</label>
            <Select v-model="secondControllerLegacyId" :options="controllers" optionLabel="fullName" optionValue="id"
                    filter showClear :placeholder="t('techExam.form.pickController')" />
          </div>
          <div class="field" v-if="isEdit && regNumber">
            <label>{{ t('techExam.regNumber') }}</label>
            <div class="val readonly mono">{{ regNumber }}</div>
          </div>
        </div>
      </template>
    </Card>

    <!-- Anchor: client + relation -->
    <Card class="card">
      <template #title>{{ t('techExam.form.sections.anchor') }}</template>
      <template #content>
        <div v-if="!isEdit" class="field">
          <label>{{ t('requests.form.client') }} *</label>
          <div v-if="selectedClient" class="picked">
            <span class="picked-name">{{ clientDisplayName(selectedClient) }}</span>
            <span class="muted" v-if="selectedClient.mb">&nbsp;·&nbsp;EMBG {{ selectedClient.mb }}</span>
            <Button icon="pi pi-times" text size="small" @click="clearClient" />
          </div>
          <div v-else>
            <InputText v-model="clientQuery" :placeholder="t('requests.form.searchClient')" size="small" style="width:100%" />
            <ul v-if="clientResults.length" class="search-list">
              <li v-for="c in clientResults" :key="c.id" @click="chooseClient(c)">
                <strong>{{ clientDisplayName(c) }}</strong>
                <span class="muted" v-if="c.mb">&nbsp;· EMBG {{ c.mb }}</span>
              </li>
            </ul>
          </div>
        </div>
        <div v-else class="field">
          <label>{{ t('requests.form.client') }}</label>
          <div class="picked locked">
            <span class="picked-name" v-if="selectedClient">{{ clientDisplayName(selectedClient) }}</span>
            <span v-else class="muted">—</span>
            <i class="pi pi-lock muted" />
          </div>
        </div>

        <div class="field" v-if="selectedClient">
          <label>{{ t('requests.form.relation') }} *</label>
          <Select v-model="selectedRelationId" :options="relations" :optionLabel="relationLabel" optionValue="id"
                  :disabled="isEdit" :placeholder="t('requests.form.pickRelation')" />
          <div v-if="!relations.length && selectedClient" class="muted small">{{ t('requests.form.noRelations') }}</div>
        </div>
      </template>
    </Card>

    <!-- Brake-force grid -->
    <Card class="card">
      <template #title>
        <button type="button" class="collapse-toggle" @click="brakeCollapsed = !brakeCollapsed">
          <span>{{ t('techExam.measuredValues') }}</span>
          <i class="pi pi-chevron-down chevron" :class="{ collapsed: brakeCollapsed }" />
        </button>
      </template>
      <template #content>
        <div v-show="!brakeCollapsed">
        <table class="brake-table">
          <thead>
            <tr>
              <th></th>
              <th>{{ t('techExam.brakeLeft') }}</th>
              <th>{{ t('techExam.brakeGj') }}</th>
              <th>{{ t('techExam.brakeLeftPj') }}</th>
              <th>{{ t('techExam.brakePn') }}</th>
              <th>{{ t('techExam.brakeRight') }}</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="a in axles" :key="a.axle">
              <th class="rowhead">{{ axleLabel(a.axle) }}</th>
              <td><InputNumber v-model="a.left" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" inputClass="cell-num" /></td>
              <td><InputNumber v-model="a.gj" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" inputClass="cell-num" /></td>
              <td><InputNumber v-model="a.leftRightDiff" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" inputClass="cell-num" /></td>
              <td><InputNumber v-model="a.coefficient" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" inputClass="cell-num" /></td>
              <td><InputNumber v-model="a.right" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" inputClass="cell-num" /></td>
            </tr>
          </tbody>
        </table>

        <div class="measure-grid">
          <div v-for="f in measFields" :key="f.key" class="field">
            <label>{{ t(f.labelKey) }}</label>
            <InputNumber v-model="meas[f.key]" :useGrouping="false" :minFractionDigits="0" :maxFractionDigits="3" />
          </div>
        </div>

        <div class="field span-full mt">
          <label>{{ t('techExam.technicalChanges') }}</label>
          <Textarea v-model="technicalChanges" rows="2" autoResize />
        </div>
        </div>
      </template>
    </Card>

    <!-- Defects -->
    <Card class="card">
      <template #title>
        <div class="card-title-row">
          <span>{{ t('techExam.defects') }}</span>
          <span class="count">{{ details.length }}</span>
          <Button :label="t('common.new')" icon="pi pi-plus" size="small" severity="success" @click="addDefect" />
        </div>
      </template>
      <template #content>
        <table v-if="details.length" class="defects-table">
          <thead>
            <tr>
              <th class="part-col">{{ t('techExam.defectPart') }} *</th>
              <th class="status-col">{{ t('techExam.defectStatus') }} *</th>
              <th class="chk">{{ t('techExam.front') }}</th>
              <th class="chk">{{ t('techExam.back') }}</th>
              <th class="chk">{{ t('techExam.left') }}</th>
              <th class="chk">{{ t('techExam.right') }}</th>
              <th>{{ t('techExam.note') }}</th>
              <th class="act"></th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(d, idx) in details" :key="idx">
              <td><Select v-model="d.vehiclePartId" :options="partOptions" optionLabel="label" optionValue="id" filter
                          :placeholder="t('techExam.defectPart')" style="width:100%" /></td>
              <td><Select v-model="d.statusId" :options="statuses" optionLabel="name" optionValue="id"
                          :placeholder="t('techExam.defectStatus')" style="width:100%" /></td>
              <td class="chk"><Checkbox v-model="d.front" :binary="true" /></td>
              <td class="chk"><Checkbox v-model="d.back" :binary="true" /></td>
              <td class="chk"><Checkbox v-model="d.onLeft" :binary="true" /></td>
              <td class="chk"><Checkbox v-model="d.onRight" :binary="true" /></td>
              <td><InputText v-model="d.note" size="small" style="width:100%" /></td>
              <td class="act"><Button icon="pi pi-trash" text severity="danger" size="small" @click="removeDefect(idx)" /></td>
            </tr>
          </tbody>
        </table>
        <div v-else class="muted small">{{ t('techExam.noDefects') }}</div>
      </template>
    </Card>

    <!-- Notes -->
    <Card class="card">
      <template #title>{{ t('techExam.notesSection') }}</template>
      <template #content>
        <div class="field">
          <label>{{ t('techExam.explanationNote') }}</label>
          <Textarea v-model="explanationNote" rows="2" autoResize />
        </div>
        <div class="field">
          <label>{{ t('techExam.driversWarning') }}</label>
          <Textarea v-model="driversWarning" rows="2" autoResize />
        </div>
        <div class="field">
          <label>{{ t('techExam.note') }}</label>
          <Textarea v-model="note" rows="2" autoResize />
        </div>
      </template>
    </Card>
  </div>
</template>

<style scoped>
.cards-grid { display: grid; grid-template-columns: 1fr; gap: 1rem }
.card { width: 100% }
.grid.two-col { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem }
.field { display: flex; flex-direction: column; gap: .3rem; margin-bottom: .85rem }
.field label { font-weight: 600; font-size: .85rem; color: var(--p-text-muted-color) }
.field.span-full { grid-column: 1 / -1 }
.val.readonly { padding: .45rem .25rem; font-size: .9rem }
.mono { font-family: monospace }
.muted { color: var(--p-text-muted-color) }
.small { font-size: .75rem }
.mt { margin-top: .75rem }
.pad { padding: 1rem }
.subtitle { display: flex; align-items: center; gap: .4rem; font-size: .85rem }

.picked { display: flex; align-items: center; gap: .5rem;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  padding: .4rem .75rem; border-radius: 6px }
.picked-name { font-weight: 600 }
.picked.locked { background: transparent; opacity: .85 }
.search-list { margin: 0; padding: 0; list-style: none;
  border: 1px solid var(--p-content-border-color); border-radius: 6px;
  max-height: 220px; overflow-y: auto; background: var(--p-content-background) }
.search-list li { padding: .4rem .65rem; cursor: pointer }
.search-list li:hover { background: var(--p-highlight-background) }

.brake-table { border-collapse: collapse; width: 100%; font-size: .85rem }
.brake-table th, .brake-table td { border: 1px solid var(--p-content-border-color); padding: .2rem .3rem; text-align: center }
.brake-table thead th { background: var(--p-content-background); font-weight: 600 }
.brake-table .rowhead { background: var(--p-content-background); text-align: left; font-weight: 600; white-space: nowrap }
.brake-table :deep(.cell-num) { width: 100%; text-align: center; padding: .25rem }
.brake-table :deep(.p-inputnumber) { width: 100% }

.measure-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: .6rem 1rem; margin-top: 1rem }
.measure-grid .field { margin-bottom: 0 }
.measure-grid :deep(.p-inputnumber) { width: 100% }

.card-title-row { display: flex; align-items: center; gap: .6rem }
.count { display: inline-flex; align-items: center; justify-content: center;
  min-width: 1.5rem; height: 1.5rem; padding: 0 .4rem;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  border-radius: 999px; font-size: .8rem; font-weight: 600; color: var(--p-text-muted-color) }
.card-title-row > .p-button { margin-left: auto }

.collapse-toggle { display: flex; align-items: center; gap: .5rem; width: 100%;
  background: none; border: none; padding: 0; cursor: pointer; font: inherit; color: inherit; text-align: left }
.collapse-toggle .chevron { margin-left: auto; font-size: .8rem; transition: transform .15s; color: var(--p-text-muted-color) }
.collapse-toggle .chevron.collapsed { transform: rotate(-90deg) }

.defects-table { border-collapse: collapse; width: 100%; font-size: .82rem }
.defects-table th, .defects-table td { border: 1px solid var(--p-content-border-color); padding: .3rem .4rem; vertical-align: middle }
.defects-table thead th { background: var(--p-content-background); font-weight: 600; text-align: left }
.defects-table .chk { width: 56px; text-align: center }
.defects-table .act { width: 44px; text-align: center }
.defects-table .part-col { min-width: 220px }
.defects-table .status-col { min-width: 140px }
</style>
