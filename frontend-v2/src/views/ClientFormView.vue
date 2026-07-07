<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Client, City, Citizenship, ClientPersonalData, Company, DocumentIssuer, VehicleRelationDto } from '@/types';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Checkbox from 'primevue/checkbox';
import DatePicker from 'primevue/datepicker';
import Select from 'primevue/select';
import Tag from 'primevue/tag';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { useToast } from 'primevue/usetoast';

const props = defineProps<{ id?: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();
const auth = useAuthStore();

const isEdit = computed(() => !!props.id);
const loading = ref(false);
const saving = ref(false);

const displayTitle = computed(() => {
  if (!isEdit.value) return t('clients.form.newTitle');
  const parts = [form.value.firstName, form.value.middleName, form.value.lastName]
    .map(s => s?.trim())
    .filter(Boolean);
  // For business clients the legacy data tends to put the whole entity name in lastName,
  // so the same join still produces a sensible display.
  if (parts.length > 0) return parts.join(' ');
  // Fall back to "Сопственик #id" only when no name fields are populated.
  return t('clients.form.editTitle', { id: props.id });
});

interface DocCard {
  id: number;
  number: string;
  dateIssued: Date | null;
  expiresAt: Date | null;
  issuerId: number | null;
}

const form = ref({
  // admin-only tenant assignment (operators ignore this)
  companyId: null as number | null,
  // identity
  mb: null as string | null,
  business: false,
  firstName: null as string | null,
  middleName: null as string | null,
  lastName: null as string | null,
  taxNumber: null as string | null,
  // address
  citizenshipId: null as number | null,
  dateOfBirth: null as Date | null,
  address: null as string | null,
  cityId: null as number | null,
  // contact
  phoneNumber: null as string | null,
  email: null as string | null,
  note: null as string | null,
  active: true,
});

// 3 doc cards — each mirrors one of: PersonalDataType 3 (ID), 2 (Passport), 1 (Driving Licence)
const idCard          = ref<DocCard>(blankDoc());
const passport        = ref<DocCard>(blankDoc());
const drivingLicence  = ref<DocCard>(blankDoc());

function blankDoc(): DocCard {
  return { id: 0, number: '', dateIssued: null, expiresAt: null, issuerId: null };
}

// Reference dropdowns
const cities = ref<City[]>([]);
const citizenships = ref<Citizenship[]>([]);
const documentIssuers = ref<DocumentIssuer[]>([]);
const companies = ref<Company[]>([]);   // admin-only

// Related vehicles sub-grid (only populated when editing an existing client)
const relatedVehicles = ref<VehicleRelationDto[]>([]);

const errorBanner = ref<string | null>(null);

// ---------- load ----------

function toDate(s: string | null | undefined): Date | null {
  return s ? new Date(s) : null;
}
function fromDate(d: Date | null): string | null {
  if (!d) return null;
  // Local date-only (yyyy-MM-dd) — toISOString() would shift local midnight to the
  // previous day in UTC+ zones (dates here are calendar dates, not instants).
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

async function loadRefs() {
  const [c, cz, di] = await Promise.all([
    api.get<City[]>('/cities'),
    api.get<Citizenship[]>('/citizenships'),
    api.get<DocumentIssuer[]>('/document-issuers'),
  ]);
  cities.value = c.data;
  citizenships.value = cz.data;
  documentIssuers.value = di.data;

  if (auth.isAdmin) {
    try {
      const { data } = await api.get<Company[]>('/companies');
      companies.value = data;
      // For a brand-new client, default the Company select to the first one so
      // the field is never empty (backend would otherwise fall back anyway,
      // but showing the selection up front is clearer).
      if (!isEdit.value && form.value.companyId == null && companies.value.length > 0) {
        form.value.companyId = companies.value[0].id;
      }
    } catch { /* not fatal — admin endpoint might 403 in odd setups */ }
  }
}

async function loadClient() {
  if (!isEdit.value) return;
  const { data } = await api.get<Client>(`/clients/${props.id}`);
  form.value = {
    companyId: data.companyId,
    mb: data.mb,
    business: !!data.business,
    firstName: data.firstName,
    middleName: data.middleName,
    lastName: data.lastName,
    taxNumber: data.taxNumber,
    citizenshipId: data.citizenshipId,
    dateOfBirth: toDate(data.dateOfBirth),
    address: data.address,
    cityId: data.cityId,
    phoneNumber: data.phoneNumber,
    email: data.email,
    note: data.note,
    active: data.active ?? true,
  };

  const { data: pdRows } = await api.get<ClientPersonalData[]>(`/client-personal-data/by-client/${props.id}`);
  // Pick the first active row of each type for each card.
  // Only show active rows — a soft-deleted (Active=false) document should
  // not reappear after reload.
  const pickFirst = (typeId: number) =>
    pdRows.find(r => r.personalDataTypeId === typeId && r.active);

  const fillCard = (target: DocCard, row: ClientPersonalData | undefined) => {
    if (!row) return;
    target.id         = row.id;
    target.number     = row.number;
    target.dateIssued = toDate(row.createdAt);
    target.expiresAt  = toDate(row.expiresAt);
    target.issuerId   = row.documentIssuerId;
  };
  fillCard(idCard.value,         pickFirst(3));
  fillCard(passport.value,       pickFirst(2));
  fillCard(drivingLicence.value, pickFirst(1));

  // Related vehicles for this client (all relation types — owner / authorized / etc.).
  try {
    const { data: rels } = await api.get<VehicleRelationDto[]>(
      `/client-vehicle-relations?clientId=${props.id}`
    );
    relatedVehicles.value = rels;
  } catch { /* non-fatal — the rest of the form still works */ }

  // Документи: меѓународни дозволи + полномошна (валидна/истечена по датум).
  try {
    const { data: docs } = await api.get<ClientDocuments>(`/clients/${props.id}/documents`);
    clientDocs.value = docs;
  } catch { /* non-fatal */ }
}

// ---- Документи (МВД + полномошна) на дното од формата ----
interface ClientIdlRow {
  id: number; numberOfLicence: string; issuedDate: string; validTillDate: string;
}
interface ClientPermissionRow {
  id: number; permissionNumber: string | null; trafficLicenceNumber: string;
  plateNumber: string | null; vehicleDisplay: string | null;
  role: 'owner' | 'authorized'; otherPartyName: string | null;
  issuedDate: string; validTillDate: string;
}
interface ClientDocuments {
  internationalDrivingLicences: ClientIdlRow[];
  permissions: ClientPermissionRow[];
}
const clientDocs = ref<ClientDocuments | null>(null);

function isDocValid(validTill: string): boolean {
  const d = new Date(validTill);
  const today = new Date();
  today.setHours(0, 0, 0, 0);
  return d >= today;
}
function docStatus(validTill: string): { label: string; severity: 'success' | 'danger' } {
  return isDocValid(validTill)
    ? { label: t('clients.form.docs.valid'), severity: 'success' }
    : { label: t('clients.form.docs.expired'), severity: 'danger' };
}
function fmtDocDate(s: string): string { return new Date(s).toLocaleDateString(); }

onMounted(async () => {
  loading.value = true;
  try {
    await loadRefs();
    await loadClient();
    // Нов клиент: Државјанство default Македонско (станицата е македонска; странците
    // се реткост и се менуваат рачно). Resolve by name — the lookup has duplicate rows.
    if (!isEdit.value && form.value.citizenshipId == null) {
      const mk = citizenships.value.find(z => z.name?.trim() === 'Македонско');
      if (mk) form.value.citizenshipId = mk.id;
    }
  } catch (e: any) {
    errorBanner.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load.';
  } finally {
    loading.value = false;
  }
});

// ---------- save ----------

async function save() {
  saving.value = true;
  errorBanner.value = null;
  try {
    const payload: Record<string, unknown> = {
      cityId: form.value.cityId,
      citizenshipId: form.value.citizenshipId,
      business: form.value.business,
      firstName: form.value.firstName?.trim() || null,
      middleName: form.value.middleName?.trim() || null,
      lastName: form.value.lastName?.trim() || null,
      mb: form.value.mb?.trim() || null,
      address: form.value.address?.trim() || null,
      taxNumber: form.value.taxNumber?.trim() || null,
      phoneNumber: form.value.phoneNumber?.trim() || null,
      email: form.value.email?.trim() || null,
      dateOfBirth: fromDate(form.value.dateOfBirth),
      note: form.value.note?.trim() || null,
      active: form.value.active,
    };
    // Admin-only: include companyId so the backend uses it (Operators' tokens
    // ignore the field on the server side).
    if (auth.isAdmin && form.value.companyId != null) {
      payload.companyId = form.value.companyId;
    }

    let clientId: number;
    if (isEdit.value) {
      await api.put(`/clients/${props.id}`, payload);
      clientId = Number(props.id);
    } else {
      const { data } = await api.post<Client>('/clients', payload);
      clientId = data.id;
    }

    // Personal documents — upsert per card, delete if cleared
    await persistDoc(clientId, 3, idCard.value);
    await persistDoc(clientId, 2, passport.value);
    await persistDoc(clientId, 1, drivingLicence.value);

    toast.add({
      severity: 'success',
      summary: isEdit.value ? t('clients.form.saved') : t('clients.form.created'),
      detail: `Id ${clientId}`,
      life: 2500,
    });

    if (!isEdit.value) {
      router.replace(`/clients/${clientId}`);
    }
  } catch (e: any) {
    const msg = e?.response?.data?.error ?? e?.message ?? 'Save failed.';
    errorBanner.value = msg;
    toast.add({ severity: 'error', summary: t('clients.form.saveFailed'), detail: msg, life: 5000 });
  } finally {
    saving.value = false;
  }
}

async function persistDoc(clientId: number, typeId: number, card: DocCard) {
  const trimmed = (card.number ?? '').trim();
  const issuerId = card.issuerId ?? documentIssuers.value[0]?.id ?? 1;

  if (card.id > 0) {
    if (!trimmed) {
      // Cleared → soft-delete
      await api.delete(`/client-personal-data/${card.id}`);
      card.id = 0;
      return;
    }
    await api.put(`/client-personal-data/${card.id}`, {
      clientId,
      personalDataTypeId: typeId,
      documentIssuerId: issuerId,
      number: trimmed,
      createdAt: fromDate(card.dateIssued),   // date issued (CreatedAt doubles as it)
      expiresAt: fromDate(card.expiresAt),
      active: true,
    });
  } else if (trimmed) {
    const { data } = await api.post<ClientPersonalData>('/client-personal-data', {
      clientId,
      personalDataTypeId: typeId,
      documentIssuerId: issuerId,
      number: trimmed,
      createdAt: fromDate(card.dateIssued),
      expiresAt: fromDate(card.expiresAt),
      active: true,
    });
    card.id = data.id;
  }
}

/**
 * Delete a single document card (trash button). If the doc was persisted (id>0)
 * we soft-delete on the server immediately; otherwise we just clear the local
 * fields. Either way the inputs become blank so the user can start fresh.
 */
async function deleteDoc(card: DocCard, label: string) {
  const persisted = card.id > 0;
  if (persisted && !window.confirm(t('common.confirmDeleteGeneric', { name: label }))) return;

  if (persisted) {
    try {
      await api.delete(`/client-personal-data/${card.id}`);
      toast.add({ severity: 'success', summary: t('common.deleted'), life: 2000 });
    } catch (e: any) {
      toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      return;
    }
  }
  card.id = 0;
  card.number = '';
  card.dateIssued = null;
  card.expiresAt = null;
  card.issuerId = null;
}
</script>

<template>
  <div class="client-form">
    <div class="page-header">
      <div>
        <h1>{{ displayTitle }}</h1>
        <div class="subtitle">
          <Tag v-if="form.business" :value="t('clients.form.business')" severity="info" />
          <span v-else class="muted">{{ t('clients.filterPhysical') }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty">
      <i class="pi pi-spin pi-spinner" />
      Loading…
    </div>

    <template v-else>
      <!-- ===== Company (admin-only; locked after creation) ===== -->
      <div v-if="auth.isAdmin" class="card">
        <div class="card-header">{{ t('clients.form.sections.company') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field" style="max-width: 360px">
              <label>
                {{ t('clients.form.company') }}
                <i v-if="isEdit" class="pi pi-lock" style="font-size: 0.7rem; margin-left: 0.25rem; opacity: 0.6" />
              </label>
              <Select
                v-model="form.companyId"
                :options="companies"
                optionLabel="name" optionValue="id"
                placeholder="—" filter
                :disabled="isEdit"
              />
              <span class="muted" style="font-size: 0.7rem; margin-top: 0.15rem">
                {{ isEdit ? t('clients.form.companyLocked') : t('clients.form.companyHint') }}
              </span>
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Identity ===== -->
      <div class="card">
        <div class="card-header">{{ t('clients.form.sections.identity') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field" style="max-width: 220px">
              <label>{{ t('clients.form.mb') }}</label>
              <InputText v-model="form.mb" maxlength="13" />
            </div>
            <div class="field" style="display: flex; align-items: center; padding-top: 1.4rem">
              <label style="display: flex; align-items: center; gap: 0.5rem; font-weight: 500">
                <Checkbox v-model="form.business" :binary="true" /> {{ t('clients.form.business') }}
              </label>
            </div>
            <div class="field" style="display: flex; align-items: center; padding-top: 1.4rem">
              <label style="display: flex; align-items: center; gap: 0.5rem; font-weight: 500">
                <Checkbox v-model="form.active" :binary="true" /> {{ t('clients.form.active') }}
              </label>
            </div>
          </div>
          <div class="row">
            <div class="field">
              <label>{{ form.business ? t('clients.form.companyName') : t('clients.form.lastName') }}</label>
              <InputText v-model="form.lastName" maxlength="200" />
            </div>
            <div class="field">
              <label>{{ t('clients.form.firstName') }}</label>
              <InputText v-model="form.firstName" maxlength="200" />
            </div>
          </div>
          <div class="row">
            <div class="field">
              <label>{{ t('clients.form.middleName') }}</label>
              <InputText v-model="form.middleName" maxlength="200" />
            </div>
            <div class="field">
              <label>
                {{ t('clients.form.taxNumber') }}
                <span v-if="!form.business" class="muted" style="font-size: 0.7rem">
                  · {{ t('clients.form.business') }}
                </span>
              </label>
              <InputText
                v-model="form.taxNumber"
                maxlength="100"
                :disabled="!form.business"
              />
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Identity documents (3 sub-cards) ===== -->
      <div class="card">
        <div class="card-header">{{ t('clients.form.sections.documents') }}</div>
        <div class="card-body">
          <div class="docgrid">
            <div class="docblock">
              <h3>
                <span>{{ t('clients.form.idCard') }}</span>
                <Button
                  v-if="idCard.id > 0 || idCard.number"
                  icon="pi pi-trash" text rounded severity="danger" size="small"
                  class="doc-delete"
                  @click="deleteDoc(idCard, t('clients.form.idCard'))"
                  v-tooltip.top="t('common.delete')"
                />
              </h3>
              <div class="field">
                <label>{{ t('clients.form.docNumber') }}</label>
                <InputText v-model="idCard.number" maxlength="200" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateIssued') }}</label>
                <DatePicker v-model="idCard.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateExpires') }}</label>
                <DatePicker v-model="idCard.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.docIssuer') }}</label>
                <Select
                  v-model="idCard.issuerId"
                  :options="documentIssuers"
                  optionLabel="name" optionValue="id"
                  placeholder="—" showClear filter
                />
              </div>
            </div>
            <div class="docblock">
              <h3>
                <span>{{ t('clients.form.passport') }}</span>
                <Button
                  v-if="passport.id > 0 || passport.number"
                  icon="pi pi-trash" text rounded severity="danger" size="small"
                  class="doc-delete"
                  @click="deleteDoc(passport, t('clients.form.passport'))"
                  v-tooltip.top="t('common.delete')"
                />
              </h3>
              <div class="field">
                <label>{{ t('clients.form.docNumber') }}</label>
                <InputText v-model="passport.number" maxlength="200" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateIssued') }}</label>
                <DatePicker v-model="passport.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateExpires') }}</label>
                <DatePicker v-model="passport.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.docIssuer') }}</label>
                <Select
                  v-model="passport.issuerId"
                  :options="documentIssuers"
                  optionLabel="name" optionValue="id"
                  placeholder="—" showClear filter
                />
              </div>
            </div>
            <div class="docblock">
              <h3>
                <span>{{ t('clients.form.drivingLicence') }}</span>
                <Button
                  v-if="drivingLicence.id > 0 || drivingLicence.number"
                  icon="pi pi-trash" text rounded severity="danger" size="small"
                  class="doc-delete"
                  @click="deleteDoc(drivingLicence, t('clients.form.drivingLicence'))"
                  v-tooltip.top="t('common.delete')"
                />
              </h3>
              <div class="field">
                <label>{{ t('clients.form.docNumber') }}</label>
                <InputText v-model="drivingLicence.number" maxlength="200" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateIssued') }}</label>
                <DatePicker v-model="drivingLicence.dateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.dateExpires') }}</label>
                <DatePicker v-model="drivingLicence.expiresAt" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('clients.form.docIssuer') }}</label>
                <Select
                  v-model="drivingLicence.issuerId"
                  :options="documentIssuers"
                  optionLabel="name" optionValue="id"
                  placeholder="—" showClear filter
                />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Address ===== -->
      <div class="card">
        <div class="card-header">{{ t('clients.form.sections.address') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field">
              <label>{{ t('clients.form.citizenship') }}</label>
              <Select
                v-model="form.citizenshipId"
                :options="citizenships"
                optionLabel="name" optionValue="id"
                placeholder="—" showClear filter
              />
            </div>
            <div class="field">
              <label>{{ t('clients.form.dateOfBirth') }}</label>
              <DatePicker v-model="form.dateOfBirth" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
            </div>
          </div>
          <div class="row">
            <div class="field" style="flex: 2">
              <label>{{ t('clients.form.address') }}</label>
              <InputText v-model="form.address" maxlength="200" />
            </div>
            <div class="field">
              <label>{{ t('clients.form.city') }}</label>
              <Select
                v-model="form.cityId"
                :options="cities"
                optionLabel="name" optionValue="id"
                placeholder="—" showClear filter
              />
            </div>
          </div>
        </div>
      </div>

      <!-- ===== Contact ===== -->
      <div class="card">
        <div class="card-header">{{ t('clients.form.sections.contact') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field">
              <label>{{ t('clients.form.phone') }}</label>
              <InputText v-model="form.phoneNumber" maxlength="40" />
            </div>
            <div class="field">
              <label>{{ t('clients.form.email') }}</label>
              <InputText v-model="form.email" maxlength="200" />
            </div>
          </div>
          <div class="field">
            <label>{{ t('clients.form.note') }}</label>
            <Textarea v-model="form.note" rows="2" autoResize />
          </div>
        </div>
      </div>

      <!-- ===== Related vehicles (edit-only) ===== -->
      <div v-if="isEdit" class="card">
        <div class="card-header">
          <span>{{ t('clients.form.sections.vehicles') }}</span>
          <span class="muted" style="font-size: 0.7rem; font-weight: 400">
            {{ relatedVehicles.length }}
          </span>
        </div>
        <div class="card-body">
          <DataTable
            v-if="relatedVehicles.length"
            :value="relatedVehicles"
            size="small" stripedRows
            class="tight-table"
            dataKey="id"
            rowHover
            @row-click="(e: any) => e.data.vehicleId && router.push(`/vehicles/${e.data.vehicleId}`)"
            :pt="{ row: { style: 'cursor: pointer' } }"
          >
            <Column field="vehicleVin"   :header="t('clients.form.relatedVehicles.colVin')">
              <template #body="{ data }">{{ data.vehicleVin || '—' }}</template>
            </Column>
            <Column field="vehiclePlate" :header="t('clients.form.relatedVehicles.colPlate')">
              <template #body="{ data }">{{ data.vehiclePlate || '—' }}</template>
            </Column>
            <Column field="vehicleMaker" :header="t('clients.form.relatedVehicles.colMaker')">
              <template #body="{ data }">{{ data.vehicleMaker || '—' }}</template>
            </Column>
            <Column field="vehicleModel" :header="t('clients.form.relatedVehicles.colModel')">
              <template #body="{ data }">{{ data.vehicleModel || '—' }}</template>
            </Column>
            <Column field="relationTypeName" :header="t('clients.form.relatedVehicles.colRelation')" style="width: 160px">
              <template #body="{ data }">{{ data.relationTypeName || '—' }}</template>
            </Column>
            <Column :header="t('clients.form.relatedVehicles.colFrom')" style="width: 110px">
              <template #body="{ data }">{{ data.startDate ? new Date(data.startDate).toLocaleDateString() : '—' }}</template>
            </Column>
            <Column :header="t('clients.form.relatedVehicles.colTo')" style="width: 110px">
              <template #body="{ data }">{{ data.endDate ? new Date(data.endDate).toLocaleDateString() : '—' }}</template>
            </Column>
            <Column :header="t('clients.form.relatedVehicles.colActive')" style="width: 90px" bodyStyle="text-align: center">
              <template #body="{ data }">
                <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
              </template>
            </Column>
            <Column style="width: 60px">
              <template #body="{ data }">
                <Button
                  v-if="data.vehicleId"
                  icon="pi pi-external-link" text rounded severity="secondary"
                  @click.stop="router.push(`/vehicles/${data.vehicleId}`)"
                  v-tooltip.left="t('clients.form.relatedVehicles.open')"
                />
              </template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 1.5rem; display:flex; align-items:center; gap:0.6rem">
            <i class="pi pi-car" />
            {{ t('clients.form.relatedVehicles.noRows') }}
          </div>
        </div>
      </div>

      <!-- Документи: меѓународни возачки дозволи + полномошна -->
      <div v-if="isEdit" class="card">
        <div class="card-header">{{ t('clients.form.docs.section') }}</div>
        <div class="card-body docs-body">
          <template v-if="clientDocs && (clientDocs.internationalDrivingLicences.length || clientDocs.permissions.length)">
            <div v-if="clientDocs.internationalDrivingLicences.length" class="doc-group">
              <div class="doc-group-title"><i class="pi pi-id-card" /> {{ t('clients.form.docs.idls') }}</div>
              <div v-for="d in clientDocs.internationalDrivingLicences" :key="'idl' + d.id"
                   class="doc-row" @click="router.push(`/international-driving-licences/${d.id}`)">
                <span class="doc-no mono">{{ d.numberOfLicence }}</span>
                <span class="doc-dates muted">{{ fmtDocDate(d.issuedDate) }} — {{ fmtDocDate(d.validTillDate) }}</span>
                <Tag :value="docStatus(d.validTillDate).label" :severity="docStatus(d.validTillDate).severity" class="doc-tag" />
                <i class="pi pi-external-link doc-open muted" />
              </div>
            </div>

            <div v-if="clientDocs.permissions.length" class="doc-group">
              <div class="doc-group-title"><i class="pi pi-file-check" /> {{ t('clients.form.docs.permissions') }}</div>
              <div v-for="p in clientDocs.permissions" :key="'perm' + p.id + p.role"
                   class="doc-row" @click="router.push(`/vehicle-permissions/${p.id}`)">
                <span class="doc-no mono">{{ p.permissionNumber || p.trafficLicenceNumber }}</span>
                <span class="doc-veh">
                  <span v-if="p.plateNumber" class="plate">{{ p.plateNumber }}</span>
                  <span v-if="p.vehicleDisplay" class="muted small"> {{ p.vehicleDisplay }}</span>
                </span>
                <Tag :value="p.role === 'owner' ? t('clients.form.docs.roleOwner') : t('clients.form.docs.roleAuthorized')"
                     severity="secondary" class="doc-tag" />
                <span v-if="p.otherPartyName" class="doc-other muted" :title="p.otherPartyName">
                  {{ p.role === 'owner' ? t('clients.form.docs.forPerson') : t('clients.form.docs.fromPerson') }} {{ p.otherPartyName }}
                </span>
                <span class="doc-dates muted">{{ fmtDocDate(p.issuedDate) }} — {{ fmtDocDate(p.validTillDate) }}</span>
                <Tag :value="docStatus(p.validTillDate).label" :severity="docStatus(p.validTillDate).severity" class="doc-tag" />
                <i class="pi pi-external-link doc-open muted" />
              </div>
            </div>
          </template>
          <div v-else class="empty" style="padding: 1.5rem; display:flex; align-items:center; gap:0.6rem">
            <i class="pi pi-id-card" />
            {{ t('clients.form.docs.noRows') }}
          </div>
        </div>
      </div>

      <div v-if="errorBanner" class="error">{{ errorBanner }}</div>

      <div class="footer-actions">
        <Button :label="t('common.back')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/clients')" />
        <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" severity="success" @click="save" />
      </div>
    </template>
  </div>
</template>

<style scoped>
/* v1-style multi-card layout: capped width, dense spacing, sticky save bar. */

.client-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }

.client-form :deep(.page-header)               { margin-bottom: 0.75rem; }
.client-form :deep(.page-header h1)            { font-size: 1.125rem; letter-spacing: -0.01em; }
.client-form :deep(.page-header .subtitle)     { font-size: 0.75rem; }

.client-form :deep(.card + .card)              { margin-top: 0.5rem; }
.client-form :deep(.card .card-header)         { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.client-form :deep(.card .card-body)           { padding: 0.625rem 0.875rem; }

.client-form :deep(.row)                       { gap: 0.625rem; }
.client-form :deep(.field)                     { gap: 0.15rem; margin-bottom: 0.45rem; }
.client-form :deep(.field label)               { font-size: 0.75rem; font-weight: 500; color: var(--color-text-secondary); }

.docgrid                                       { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 0.5rem 0.875rem; }
.docblock h3 {
  font-size: 0.75rem; color: var(--color-text);
  margin: 0 0 0.4rem; padding-bottom: 0.25rem;
  border-bottom: 1px solid var(--color-border);
  font-weight: 600;
  display: flex; align-items: center; justify-content: space-between;
}
.docblock h3 .doc-delete                       { min-width: 0; padding: 0.1rem 0.25rem; }
.docblock h3 .doc-delete :deep(.p-button-icon) { font-size: 0.75rem; }
.docblock .field                               { margin-bottom: 0.4rem; }

.client-form :deep(.p-select),
.client-form :deep(.p-datepicker),
.client-form :deep(.p-inputtext)               { width: 100%; }
.client-form :deep(.p-inputtext)               { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.client-form :deep(.p-textarea)                { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.client-form :deep(.p-select)                  { font-size: 0.8125rem; }
.client-form :deep(.p-select-label)            { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.client-form :deep(.p-select-dropdown)         { width: 1.625rem; }
.client-form :deep(.p-datepicker-input)        { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }

.client-form :deep(.p-checkbox)                { transform: scale(0.85); transform-origin: left center; }
.client-form :deep(.p-button)                  { font-size: 0.8125rem; }
.client-form :deep(.p-button.p-button-sm)      { padding: 0.3rem 0.625rem; }

.client-form :deep(.error)                     { font-size: 0.75rem; padding: 0.4rem 0.6rem; margin-top: 0.5rem; }

/* Brand accent stripe on each card header */
.client-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.client-form :deep(.card .card-header)::before {
  content: '';
  position: absolute;
  left: 0; top: 0; bottom: 0;
  width: 3px;
  background: var(--color-brand-500);
  border-radius: 0 2px 2px 0;
}

.client-form :deep(.card) {
  transition: box-shadow 0.15s ease, border-color 0.15s ease;
}
.client-form :deep(.card:focus-within) {
  box-shadow: 0 1px 2px rgba(0,0,0,0.04), 0 4px 12px rgba(0,0,0,0.06);
  border-color: color-mix(in srgb, var(--color-border) 70%, var(--color-brand-500));
}

.client-form :deep(.p-inputtext:focus),
.client-form :deep(.p-textarea:focus),
.client-form :deep(.p-select:not(.p-disabled).p-focus),
.client-form :deep(.p-datepicker-input:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

/* Документи (МВД + полномошна) */
.docs-body { display: flex; flex-direction: column; gap: 0.7rem; }
.doc-group-title {
  display: flex; align-items: center; gap: 0.4rem;
  font-size: 0.72rem; font-weight: 700; text-transform: uppercase; letter-spacing: 0.03em;
  color: var(--color-text-secondary); margin-bottom: 0.25rem;
}
.doc-group-title i { font-size: 0.72rem; }
.doc-row {
  display: flex; align-items: center; gap: 0.7rem; flex-wrap: wrap;
  padding: 0.28rem 0.45rem; border-radius: 6px; cursor: pointer;
  font-size: 0.8125rem; line-height: 1.25;
}
.doc-row:hover { background: color-mix(in srgb, var(--color-brand-500) 6%, transparent); }
.doc-row .doc-no { font-weight: 600; min-width: 6.5rem; }
.doc-row .doc-veh { min-width: 0; }
.doc-row .doc-other { max-width: 18rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-size: 0.75rem; }
.doc-row .doc-dates { font-size: 0.75rem; white-space: nowrap; margin-left: auto; }
.doc-row .doc-tag :deep(.p-tag-label), .doc-row .doc-tag { font-size: 0.68rem; }
.doc-row .doc-open { font-size: 0.72rem; }
.doc-row .plate { font-family: monospace; font-weight: 600; }
.doc-row .doc-veh .small { margin-left: 0.4rem; }
.doc-row .mono { font-family: ui-monospace, monospace; }
.doc-row .small { font-size: 0.72rem; }
.doc-row .muted { color: var(--color-text-secondary); }

/* Sticky footer save bar, anchored to viewport bottom across the content area */
.footer-actions {
  position: fixed;
  bottom: 0;
  left: var(--sidebar-w);
  right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border);
  z-index: 5;
}
</style>
