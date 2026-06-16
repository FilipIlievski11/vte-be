<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter, useRoute } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type {
  Company,
  RequestRead, RequestWrite,
  RequestType,
  VehicleRelationDto,
  RequestOwnershipProofDto, RequestPaymentProofDto, RequestAttachmentDto,
  RequestOwnershipProofType, RequestPaymentProofType, RequestAttachmentType,
  EndRequestResult,
} from '@/types';
import Button from 'primevue/button';
import Card from 'primevue/card';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Select from 'primevue/select';
import Tag from 'primevue/tag';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Dialog from 'primevue/dialog';
import FileUpload from 'primevue/fileupload';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const props = defineProps<{ id?: string }>();
const { t } = useI18n();
const router = useRouter();
const route = useRoute();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

const isEdit = computed(() => !!props.id);
const loading = ref(false);
const saving = ref(false);

// ---- Catalog loads ----
const requestTypes = ref<RequestType[]>([]);
const companies = ref<Company[]>([]);
const ownershipTypes = ref<RequestOwnershipProofType[]>([]);
const paymentTypes = ref<RequestPaymentProofType[]>([]);
const attachmentTypes = ref<RequestAttachmentType[]>([]);

// ---- Children: ownership/payment proofs + attachments ----
const ownershipProofs = ref<RequestOwnershipProofDto[]>([]);
const paymentProofs = ref<RequestPaymentProofDto[]>([]);
const attachments = ref<RequestAttachmentDto[]>([]);

// Ownership proof dialog state
const ownershipDialogVisible = ref(false);
const ownershipEditing = ref<{ id: number; ownershipProofTypeId: number | null; detail: string; active: boolean }>({
  id: 0, ownershipProofTypeId: null, detail: '', active: true,
});

// Payment proof dialog state
const paymentDialogVisible = ref(false);
const paymentEditing = ref<{ id: number; paymentProofTypeId: number | null; detail: string; active: boolean }>({
  id: 0, paymentProofTypeId: null, detail: '', active: true,
});

// Attachment upload dialog state
const uploadDialogVisible = ref(false);
const uploadType = ref<number | null>(null);
const uploadFile = ref<File | null>(null);
const uploading = ref(false);

// ---- Form state ----
const requestId = ref<number | null>(null);
const requestTypeId = ref<number | null>(null);
const note = ref<string>('');
const companyId = ref<number | null>(null);
const newClientVehicleRelationId = ref<number | null>(null);
const active = ref(true);

// Read-only audit
const createdAt = ref<string | null>(null);
const modifiedAt = ref<string | null>(null);
const endedAt = ref<string | null>(null);
const createdByName = ref<string | null>(null);
const modifiedByName = ref<string | null>(null);
const endedByName = ref<string | null>(null);

// ---- Anchor: single vehicle+owner search ----
// One input replaces the old "pick client → then pick vehicle" two-step. Each
// result row is a client↔vehicle relation, matched by plate / VIN / maker /
// model / owner name / EMBG, so picking a row sets the whole anchor at once.
const relationQuery = ref('');
const relationResults = ref<VehicleRelationDto[]>([]);
const selectedRelation = ref<VehicleRelationDto | null>(null);
const searchingRelations = ref(false);
const relations = ref<VehicleRelationDto[]>([]);   // sibling relations of the picked client (new-owner picker)
const selectedRelationId = ref<number | null>(null);

let relationSearchTimer: number | undefined;

const selectedType = computed<RequestType | null>(() =>
  requestTypeId.value != null ? requestTypes.value.find(t => t.id === requestTypeId.value) ?? null : null,
);

// Derived: when the chosen type transfers ownership, "new owner" picker becomes required
const requiresNewOwner = computed(() => selectedType.value?.transfersOwnership ?? false);
const newOwnerOptions = computed(() =>
  relations.value.filter(r => r.id !== selectedRelationId.value),
);

const statusBadge = computed<{ label: string; severity: 'success' | 'info' | 'danger' }>(() => {
  if (!active.value) return { label: t('requests.status.inactive'), severity: 'danger' };
  if (endedAt.value) return { label: t('requests.status.closed'), severity: 'info' };
  return { label: t('requests.status.open'), severity: 'success' };
});

const isReadOnly = computed(() => !!endedAt.value);    // closed requests are locked

async function loadCatalogs() {
  const reqs: Promise<unknown>[] = [
    api.get<RequestType[]>('/request-types', { params: { activeOnly: true } })
      .then(({ data }) => { requestTypes.value = data; }),
    api.get<RequestOwnershipProofType[]>('/request-ownership-proof-types')
      .then(({ data }) => { ownershipTypes.value = data.filter(t => t.active); }),
    api.get<RequestPaymentProofType[]>('/request-payment-proof-types')
      .then(({ data }) => { paymentTypes.value = data.filter(t => t.active); }),
    api.get<RequestAttachmentType[]>('/request-attachment-types')
      .then(({ data }) => { attachmentTypes.value = data.filter(t => t.active); }),
  ];
  if (auth.isAdmin) {
    reqs.push(api.get<Company[]>('/companies').then(({ data }) => { companies.value = data; }));
  }
  await Promise.all(reqs);
}

async function loadChildren() {
  if (!props.id) return;
  try {
    const [op, pp, at] = await Promise.all([
      api.get<RequestOwnershipProofDto[]>(`/requests/${props.id}/ownership-proofs`),
      api.get<RequestPaymentProofDto[]>(`/requests/${props.id}/payment-proofs`),
      api.get<RequestAttachmentDto[]>(`/requests/${props.id}/attachments`),
    ]);
    ownershipProofs.value = op.data;
    paymentProofs.value = pp.data;
    attachments.value = at.data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  }
}

// ---- Ownership proof handlers ----

function openOwnershipNew() {
  ownershipEditing.value = { id: 0, ownershipProofTypeId: ownershipTypes.value[0]?.id ?? null, detail: '', active: true };
  ownershipDialogVisible.value = true;
}
function openOwnershipEdit(row: RequestOwnershipProofDto) {
  ownershipEditing.value = {
    id: row.id,
    ownershipProofTypeId: row.ownershipProofTypeId,
    detail: row.detail ?? '',
    active: row.active,
  };
  ownershipDialogVisible.value = true;
}
async function saveOwnership() {
  if (!props.id || !ownershipEditing.value.ownershipProofTypeId) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: t('requests.form.proofTypeRequired'), life: 3000 });
    return;
  }
  const body = {
    ownershipProofTypeId: ownershipEditing.value.ownershipProofTypeId,
    detail: ownershipEditing.value.detail || null,
    active: ownershipEditing.value.active,
  };
  try {
    if (ownershipEditing.value.id) await api.put(`/requests/${props.id}/ownership-proofs/${ownershipEditing.value.id}`, body);
    else await api.post(`/requests/${props.id}/ownership-proofs`, body);
    ownershipDialogVisible.value = false;
    await loadChildren();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}
function removeOwnership(row: RequestOwnershipProofDto) {
  confirm.require({
    message: t('common.confirmDeleteGeneric', { name: row.ownershipProofTypeName ?? `#${row.id}` }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/requests/${props.id}/ownership-proofs/${row.id}`);
        await loadChildren();
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

// ---- Payment proof handlers ----

function openPaymentNew() {
  paymentEditing.value = { id: 0, paymentProofTypeId: paymentTypes.value[0]?.id ?? null, detail: '', active: true };
  paymentDialogVisible.value = true;
}
function openPaymentEdit(row: RequestPaymentProofDto) {
  paymentEditing.value = {
    id: row.id,
    paymentProofTypeId: row.paymentProofTypeId,
    detail: row.detail ?? '',
    active: row.active,
  };
  paymentDialogVisible.value = true;
}
async function savePayment() {
  if (!props.id || !paymentEditing.value.paymentProofTypeId) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: t('requests.form.proofTypeRequired'), life: 3000 });
    return;
  }
  const body = {
    paymentProofTypeId: paymentEditing.value.paymentProofTypeId,
    detail: paymentEditing.value.detail || null,
    active: paymentEditing.value.active,
  };
  try {
    if (paymentEditing.value.id) await api.put(`/requests/${props.id}/payment-proofs/${paymentEditing.value.id}`, body);
    else await api.post(`/requests/${props.id}/payment-proofs`, body);
    paymentDialogVisible.value = false;
    await loadChildren();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}
function removePayment(row: RequestPaymentProofDto) {
  confirm.require({
    message: t('common.confirmDeleteGeneric', { name: row.paymentProofTypeName ?? `#${row.id}` }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/requests/${props.id}/payment-proofs/${row.id}`);
        await loadChildren();
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

// ---- Attachment handlers ----

function openUpload() {
  uploadType.value = attachmentTypes.value[0]?.id ?? null;
  uploadFile.value = null;
  uploadDialogVisible.value = true;
}
function onFileSelected(ev: { files: File[] }) {
  uploadFile.value = ev.files?.[0] ?? null;
}
async function uploadAttachment() {
  if (!props.id || !uploadFile.value || !uploadType.value) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: t('requests.form.attachmentMissing'), life: 3000 });
    return;
  }
  const form = new FormData();
  form.append('file', uploadFile.value);
  form.append('attachmentTypeId', String(uploadType.value));
  uploading.value = true;
  try {
    await api.post(`/requests/${props.id}/attachments`, form, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
    uploadDialogVisible.value = false;
    uploadFile.value = null;
    await loadChildren();
    toast.add({ severity: 'success', summary: t('requests.form.attachmentUploaded'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    uploading.value = false;
  }
}
async function downloadAttachment(row: RequestAttachmentDto) {
  try {
    const res = await api.get(`/requests/${props.id}/attachments/${row.id}/download`, { responseType: 'blob' });
    const url = window.URL.createObjectURL(new Blob([res.data], { type: row.contentType }));
    const a = document.createElement('a');
    a.href = url;
    a.download = row.fileName;
    document.body.appendChild(a);
    a.click();
    a.remove();
    window.URL.revokeObjectURL(url);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  }
}
function removeAttachment(row: RequestAttachmentDto) {
  confirm.require({
    message: t('common.confirmDeleteGeneric', { name: row.fileName }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/requests/${props.id}/attachments/${row.id}`);
        await loadChildren();
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

function fmtBytes(b: number): string {
  if (b < 1024) return `${b} B`;
  if (b < 1024 * 1024) return `${(b / 1024).toFixed(1)} KB`;
  return `${(b / 1024 / 1024).toFixed(1)} MB`;
}

// ---- End-request flow ----

const ending = ref(false);

const endSideEffects = computed<string[]>(() => {
  const r: string[] = [];
  const tp = selectedType.value;
  if (!tp) return r;
  if (tp.paymentRequired) r.push(t('requests.end.willRequirePayment'));
  if (tp.transfersOwnership) r.push(t('requests.end.willTransferOwnership'));
  else if (tp.deactivatesRelation) r.push(t('requests.end.willDeactivateRelation'));
  if (tp.deactivatesVehicle) r.push(t('requests.end.willDeactivateVehicle'));
  if (tp.issuesNewRegistration) r.push(t('requests.end.willIssueRegistration'));
  return r;
});

function confirmEnd() {
  if (!requestId.value) return;
  const list = endSideEffects.value;
  const body = list.length
    ? `<ul style="margin:.5rem 0 0 1rem">${list.map(s => `<li>${s}</li>`).join('')}</ul>`
    : '';
  confirm.require({
    message: `${t('requests.end.confirm')}${body}`,
    header: t('requests.end.confirmHeader'),
    icon: 'pi pi-flag',
    acceptIcon: 'pi pi-check',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('requests.end.button'), severity: 'success' },
    accept: doEnd,
  });
}

async function doEnd() {
  if (!requestId.value) return;
  ending.value = true;
  try {
    const { data } = await api.post<EndRequestResult>(`/requests/${requestId.value}/end`);
    toast.add({
      severity: 'success',
      summary: t('requests.end.ended'),
      detail: data.notes.join(' · ') || t('requests.end.endedDetail'),
      life: 5000,
    });
    // Refresh both the request and its children to reflect the new ended state
    await loadRequest();
    await loadChildren();
  } catch (e: any) {
    toast.add({
      severity: 'error',
      summary: t('requests.end.failed'),
      detail: e?.response?.data?.error ?? e?.message,
      life: 6000,
    });
  } finally {
    ending.value = false;
  }
}

function openPrint() {
  if (!requestId.value) return;
  const route = router.resolve({ name: 'request-print', params: { id: requestId.value } });
  window.open(route.href, '_blank');
}

async function loadRequest() {
  if (!props.id) return;
  loading.value = true;
  try {
    const { data: r } = await api.get<RequestRead>(`/requests/${props.id}`);
    requestId.value = r.id;
    requestTypeId.value = r.requestTypeId;
    note.value = r.note ?? '';
    companyId.value = r.companyId;
    newClientVehicleRelationId.value = r.newClientVehicleRelationId;
    active.value = r.active;
    createdAt.value = r.createdAt;
    modifiedAt.value = r.modifiedAt;
    endedAt.value = r.endedAt;
    createdByName.value = r.createdByUserName;
    modifiedByName.value = r.modifiedByUserName;
    endedByName.value = r.endedByUserName;

    // Hydrate the anchor: read the relation, then the client's sibling relations.
    selectedRelationId.value = r.clientVehicleRelationId;
    const { data: rel } = await api.get<VehicleRelationDto>(
      `/client-vehicle-relations/${r.clientVehicleRelationId}`,
    ).catch(() => ({ data: null as unknown as VehicleRelationDto }));
    if (rel?.clientId) {
      selectedRelation.value = rel;
      await loadRelationsForClient(rel.clientId);
    }
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

async function searchRelations() {
  const term = relationQuery.value.trim();
  if (term.length < 2) { relationResults.value = []; return; }
  searchingRelations.value = true;
  try {
    const { data } = await api.get<VehicleRelationDto[]>('/client-vehicle-relations', {
      params: { q: term, activeOnly: true },
    });
    relationResults.value = data;
  } catch { /* ignore */ }
  finally { searchingRelations.value = false; }
}

watch(relationQuery, () => {
  window.clearTimeout(relationSearchTimer);
  relationSearchTimer = window.setTimeout(searchRelations, 250);
});

async function loadRelationsForClient(clientId: number) {
  try {
    const { data } = await api.get<VehicleRelationDto[]>('/client-vehicle-relations', {
      params: { clientId, activeOnly: true },
    });
    relations.value = data;
  } catch { relations.value = []; }
}

function chooseRelation(r: VehicleRelationDto) {
  selectedRelation.value = r;
  selectedRelationId.value = r.id;
  relationResults.value = [];
  relationQuery.value = '';
  newClientVehicleRelationId.value = null;
  loadRelationsForClient(r.clientId);   // siblings power the new-owner picker
}

function clearRelation() {
  if (isEdit.value) return;             // anchor is locked on edit
  selectedRelation.value = null;
  relations.value = [];
  selectedRelationId.value = null;
  newClientVehicleRelationId.value = null;
}

function relationLabel(r: VehicleRelationDto): string {
  const parts: string[] = [];
  if (r.relationTypeName) parts.push(r.relationTypeName);
  if (r.vehiclePlate)     parts.push(r.vehiclePlate);
  if (r.vehicleMaker || r.vehicleModel) parts.push([r.vehicleMaker, r.vehicleModel].filter(Boolean).join(' '));
  if (r.vehicleVin)       parts.push(`VIN ${r.vehicleVin}`);
  return parts.join(' · ') || `Relation #${r.id}`;
}

function fmtDateTime(s: string | null): string {
  if (!s) return '—';
  return new Date(s).toLocaleString();
}

function validate(): string | null {
  if (!requestTypeId.value) return t('requests.form.typeRequired');
  if (!selectedRelationId.value) return t('requests.form.relationRequired');
  if (requiresNewOwner.value && !newClientVehicleRelationId.value)
    return t('requests.form.newOwnerRequired');
  return null;
}

async function save() {
  const err = validate();
  if (err) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: err, life: 4000 });
    return;
  }
  const body: RequestWrite = {
    requestTypeId: requestTypeId.value!,
    clientVehicleRelationId: selectedRelationId.value!,
    newClientVehicleRelationId: newClientVehicleRelationId.value ?? null,
    technicalExamReportId: null,
    previousRegistrationId: null,
    note: note.value || null,
    active: active.value,
    companyId: auth.isAdmin ? companyId.value : null,
  };
  saving.value = true;
  try {
    if (isEdit.value) {
      await api.put(`/requests/${props.id}`, body);
      toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
    } else {
      const { data } = await api.post<RequestRead>('/requests', body);
      toast.add({ severity: 'success', summary: t('requests.form.created'), life: 1500 });
      router.replace(`/requests/${data.id}`);
    }
  } catch (e: any) {
    toast.add({
      severity: 'error',
      summary: t('common.saveFailed'),
      detail: e?.response?.data?.error ?? e?.message,
      life: 5000,
    });
  } finally {
    saving.value = false;
  }
}

function remove() {
  if (!isEdit.value || !requestId.value) return;
  confirm.require({
    message: t('requests.deleteConfirm', { id: requestId.value }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/requests/${requestId.value}`);
        toast.add({ severity: 'success', summary: t('requests.deleted'), life: 1500 });
        router.push('/requests');
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

onMounted(async () => {
  await loadCatalogs();
  if (isEdit.value) {
    await loadRequest();
    await loadChildren();
  } else {
    // Dashboard "create request by type" shortcut: preselect from ?typeId=
    const pre = Number(route.query.typeId);
    if (Number.isFinite(pre) && pre > 0 && requestTypes.value.some(rt => rt.id === pre)) {
      requestTypeId.value = pre;
    }
  }
});
</script>

<template>
  <div class="page-header">
    <div>
      <h1>
        <span v-if="isEdit">{{ t('requests.form.editTitle', { id: requestId }) }}</span>
        <span v-else>{{ t('requests.form.newTitle') }}</span>
      </h1>
      <div class="subtitle" v-if="isEdit">
        <Tag :value="statusBadge.label" :severity="statusBadge.severity" />
        <span class="muted">&nbsp;·&nbsp;{{ t('requests.form.created') }}: {{ fmtDateTime(createdAt) }}</span>
      </div>
    </div>
    <div class="actions">
      <Button :label="t('common.back')" icon="pi pi-arrow-left" severity="secondary" size="small" outlined @click="router.push('/requests')" />
      <Button v-if="isEdit" :label="t('requests.print.button')" icon="pi pi-print" severity="secondary" size="small" outlined @click="openPrint" />
      <Button v-if="isEdit && !isReadOnly" :label="t('common.delete')" icon="pi pi-trash" severity="danger" size="small" outlined @click="remove" />
      <Button v-if="!isReadOnly" :label="t('common.save')" icon="pi pi-check" size="small" :loading="saving" @click="save" />
      <Button v-if="isEdit && !isReadOnly" :label="t('requests.end.button')" icon="pi pi-flag" severity="success" size="small" :loading="ending" @click="confirmEnd" />
    </div>
  </div>

  <div class="cards-grid">
    <!-- Header: type, status, company (admin only on new) -->
    <Card class="card">
      <template #title>{{ t('requests.form.sections.header') }}</template>
      <template #content>
        <div class="grid two-col">
          <div class="field">
            <label>{{ t('requests.form.type') }} *</label>
            <Select
              v-model="requestTypeId"
              :options="requestTypes" optionLabel="name" optionValue="id"
              :disabled="isReadOnly"
              :placeholder="t('requests.form.pickType')" />
          </div>
          <div class="field" v-if="auth.isAdmin && !isEdit">
            <label>{{ t('clients.form.company') }}</label>
            <Select
              v-model="companyId"
              :options="companies" optionLabel="name" optionValue="id"
              showClear
              :placeholder="t('clients.form.companyHint')" />
          </div>
        </div>
        <div class="field">
          <label>{{ t('requests.form.note') }}</label>
          <Textarea v-model="note" rows="2" autoResize :disabled="isReadOnly" />
        </div>
        <div v-if="selectedType?.description" class="type-hint">
          <i class="pi pi-info-circle" />&nbsp;{{ selectedType.description }}
        </div>
      </template>
    </Card>

    <!-- Anchor: single vehicle + owner search -->
    <Card class="card">
      <template #title>{{ t('requests.form.sections.anchor') }}</template>
      <template #content>
        <!-- Combined search (new only — anchor locked on edit) -->
        <div v-if="!isEdit" class="field">
          <label>{{ t('requests.form.vehicleOwner') }} *</label>
          <div v-if="selectedRelation" class="picked">
            <span class="picked-name">{{ selectedRelation.clientDisplayName || ('#' + selectedRelation.clientId) }}</span>
            <span class="muted" v-if="selectedRelation.clientMb">&nbsp;·&nbsp;EMBG {{ selectedRelation.clientMb }}</span>
            <span class="picked-veh">{{ relationLabel(selectedRelation) }}</span>
            <Button icon="pi pi-times" text size="small" @click="clearRelation" v-tooltip.left="t('common.cancel')" />
          </div>
          <div v-else>
            <InputText v-model="relationQuery" :placeholder="t('requests.form.searchVehicleOwner')" size="small" style="width:100%" />
            <ul v-if="relationResults.length" class="search-list">
              <li v-for="r in relationResults" :key="r.id" @click="chooseRelation(r)">
                <div class="res-line">
                  <strong>{{ r.clientDisplayName || ('#' + r.clientId) }}</strong>
                  <span class="muted" v-if="r.clientMb">&nbsp;· EMBG {{ r.clientMb }}</span>
                </div>
                <div class="res-veh muted">{{ relationLabel(r) }}</div>
              </li>
            </ul>
            <div v-else-if="relationQuery.trim().length >= 2 && !searchingRelations" class="muted small">
              {{ t('requests.form.noVehicleMatches') }}
            </div>
            <div class="muted small">{{ t('requests.form.searchVehicleOwnerHint') }}</div>
          </div>
        </div>

        <!-- Locked anchor display on edit -->
        <div v-else class="field">
          <label>{{ t('requests.form.vehicleOwner') }}</label>
          <div class="picked locked">
            <template v-if="selectedRelation">
              <span class="picked-name">{{ selectedRelation.clientDisplayName || ('#' + selectedRelation.clientId) }}</span>
              <span class="picked-veh">{{ relationLabel(selectedRelation) }}</span>
            </template>
            <span v-else class="muted">—</span>
            <i class="pi pi-lock muted" v-tooltip.left="t('clients.form.companyLocked')" />
          </div>
        </div>

        <!-- New owner (only when type.transfersOwnership) -->
        <div class="field" v-if="requiresNewOwner && selectedRelation">
          <label>{{ t('requests.form.newOwner') }} *</label>
          <Select
            v-model="newClientVehicleRelationId"
            :options="newOwnerOptions"
            :optionLabel="relationLabel"
            optionValue="id"
            :disabled="isReadOnly"
            :placeholder="t('requests.form.pickNewOwner')" />
          <div class="muted small">{{ t('requests.form.newOwnerHint') }}</div>
        </div>
      </template>
    </Card>

    <!-- Ownership proofs (edit-only) -->
    <Card v-if="isEdit" class="card">
      <template #title>
        <div class="card-title-row">
          <span>{{ t('requests.form.sections.ownershipProofs') }}</span>
          <span class="count">{{ ownershipProofs.length }}</span>
          <Button v-if="!isReadOnly" :label="t('common.new')" icon="pi pi-plus" size="small" severity="success" @click="openOwnershipNew" />
        </div>
      </template>
      <template #content>
        <DataTable v-if="ownershipProofs.length" :value="ownershipProofs" size="small" stripedRows>
          <Column field="ownershipProofTypeName" :header="t('requests.form.ownershipProofs.colType')" />
          <Column field="detail" :header="t('requests.form.ownershipProofs.colDetail')">
            <template #body="{ data }">{{ data.detail || '—' }}</template>
          </Column>
          <Column field="active" :header="t('ref.fields.active')" style="width:90px" bodyStyle="text-align:center">
            <template #body="{ data }">
              <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
            </template>
          </Column>
          <Column :header="t('common.actions')" style="width:110px" bodyStyle="text-align:right">
            <template #body="{ data }">
              <Button v-if="!isReadOnly" icon="pi pi-pencil" size="small" text @click="openOwnershipEdit(data)" />
              <Button v-if="!isReadOnly" icon="pi pi-trash"  size="small" text severity="danger" @click="removeOwnership(data)" />
            </template>
          </Column>
        </DataTable>
        <div v-else class="muted small">{{ t('requests.form.ownershipProofs.empty') }}</div>
      </template>
    </Card>

    <!-- Payment proofs (edit-only) -->
    <Card v-if="isEdit" class="card">
      <template #title>
        <div class="card-title-row">
          <span>{{ t('requests.form.sections.paymentProofs') }}</span>
          <span class="count">{{ paymentProofs.length }}</span>
          <Button v-if="!isReadOnly" :label="t('common.new')" icon="pi pi-plus" size="small" severity="success" @click="openPaymentNew" />
        </div>
      </template>
      <template #content>
        <DataTable v-if="paymentProofs.length" :value="paymentProofs" size="small" stripedRows>
          <Column field="paymentProofTypeName" :header="t('requests.form.paymentProofs.colType')" />
          <Column field="detail" :header="t('requests.form.paymentProofs.colDetail')">
            <template #body="{ data }">{{ data.detail || '—' }}</template>
          </Column>
          <Column field="active" :header="t('ref.fields.active')" style="width:90px" bodyStyle="text-align:center">
            <template #body="{ data }">
              <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
            </template>
          </Column>
          <Column :header="t('common.actions')" style="width:110px" bodyStyle="text-align:right">
            <template #body="{ data }">
              <Button v-if="!isReadOnly" icon="pi pi-pencil" size="small" text @click="openPaymentEdit(data)" />
              <Button v-if="!isReadOnly" icon="pi pi-trash"  size="small" text severity="danger" @click="removePayment(data)" />
            </template>
          </Column>
        </DataTable>
        <div v-else class="muted small">{{ t('requests.form.paymentProofs.empty') }}</div>
      </template>
    </Card>

    <!-- Attachments (edit-only) -->
    <Card v-if="isEdit" class="card">
      <template #title>
        <div class="card-title-row">
          <span>{{ t('requests.form.sections.attachments') }}</span>
          <span class="count">{{ attachments.length }}</span>
          <Button v-if="!isReadOnly" :label="t('requests.form.attachments.upload')" icon="pi pi-upload" size="small" severity="success" @click="openUpload" />
        </div>
      </template>
      <template #content>
        <DataTable v-if="attachments.length" :value="attachments" size="small" stripedRows>
          <Column field="fileName" :header="t('requests.form.attachments.colFile')">
            <template #body="{ data }">
              <a href="#" @click.prevent="downloadAttachment(data)" class="dl-link">
                <i class="pi pi-file" />&nbsp;{{ data.fileName }}
              </a>
            </template>
          </Column>
          <Column field="attachmentTypeName" :header="t('requests.form.attachments.colType')" style="width:160px" />
          <Column field="sizeBytes" :header="t('requests.form.attachments.colSize')" style="width:100px">
            <template #body="{ data }">{{ fmtBytes(data.sizeBytes) }}</template>
          </Column>
          <Column field="uploadedAt" :header="t('requests.form.attachments.colUploaded')" style="width:140px">
            <template #body="{ data }">{{ fmtDateTime(data.uploadedAt) }}</template>
          </Column>
          <Column field="uploadedByUserName" :header="t('requests.form.attachments.colBy')" style="width:140px">
            <template #body="{ data }">{{ data.uploadedByUserName || '—' }}</template>
          </Column>
          <Column :header="t('common.actions')" style="width:110px" bodyStyle="text-align:right">
            <template #body="{ data }">
              <Button icon="pi pi-download" size="small" text @click="downloadAttachment(data)" v-tooltip.left="t('requests.form.attachments.download')" />
              <Button v-if="!isReadOnly" icon="pi pi-trash" size="small" text severity="danger" @click="removeAttachment(data)" v-tooltip.left="t('common.delete')" />
            </template>
          </Column>
        </DataTable>
        <div v-else class="muted small">{{ t('requests.form.attachments.empty') }}</div>
      </template>
    </Card>

    <!-- Audit (edit-only) -->
    <Card v-if="isEdit" class="card">
      <template #title>{{ t('requests.form.sections.audit') }}</template>
      <template #content>
        <table class="audit-table">
          <tr>
            <td>{{ t('requests.form.created') }}</td>
            <td>{{ fmtDateTime(createdAt) }}</td>
            <td>{{ createdByName ?? '—' }}</td>
          </tr>
          <tr v-if="modifiedAt">
            <td>{{ t('requests.form.modified') }}</td>
            <td>{{ fmtDateTime(modifiedAt) }}</td>
            <td>{{ modifiedByName ?? '—' }}</td>
          </tr>
          <tr v-if="endedAt">
            <td>{{ t('requests.form.ended') }}</td>
            <td>{{ fmtDateTime(endedAt) }}</td>
            <td>{{ endedByName ?? '—' }}</td>
          </tr>
        </table>
      </template>
    </Card>
  </div>

  <!-- Ownership proof dialog -->
  <Dialog
    v-model:visible="ownershipDialogVisible"
    modal
    :header="ownershipEditing.id ? t('requests.form.ownershipProofs.editTitle') : t('requests.form.ownershipProofs.newTitle')"
    :style="{ width: '520px' }">
    <div class="form-grid">
      <div class="field">
        <label>{{ t('requests.form.ownershipProofs.colType') }} *</label>
        <Select v-model="ownershipEditing.ownershipProofTypeId"
                :options="ownershipTypes" optionLabel="name" optionValue="id" />
      </div>
      <div class="field">
        <label>{{ t('requests.form.ownershipProofs.colDetail') }}</label>
        <Textarea v-model="ownershipEditing.detail" rows="2" autoResize />
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" text @click="ownershipDialogVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" @click="saveOwnership" />
    </template>
  </Dialog>

  <!-- Payment proof dialog -->
  <Dialog
    v-model:visible="paymentDialogVisible"
    modal
    :header="paymentEditing.id ? t('requests.form.paymentProofs.editTitle') : t('requests.form.paymentProofs.newTitle')"
    :style="{ width: '520px' }">
    <div class="form-grid">
      <div class="field">
        <label>{{ t('requests.form.paymentProofs.colType') }} *</label>
        <Select v-model="paymentEditing.paymentProofTypeId"
                :options="paymentTypes" optionLabel="name" optionValue="id" />
      </div>
      <div class="field">
        <label>{{ t('requests.form.paymentProofs.colDetail') }}</label>
        <Textarea v-model="paymentEditing.detail" rows="2" autoResize />
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" text @click="paymentDialogVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" @click="savePayment" />
    </template>
  </Dialog>

  <!-- Attachment upload dialog -->
  <Dialog
    v-model:visible="uploadDialogVisible"
    modal
    :header="t('requests.form.attachments.uploadTitle')"
    :style="{ width: '560px' }">
    <div class="form-grid">
      <div class="field">
        <label>{{ t('requests.form.attachments.colType') }} *</label>
        <Select v-model="uploadType"
                :options="attachmentTypes" optionLabel="name" optionValue="id" />
      </div>
      <div class="field">
        <label>{{ t('requests.form.attachments.file') }} *</label>
        <FileUpload
          mode="basic"
          :auto="false"
          :maxFileSize="20971520"
          accept="image/*,application/pdf"
          :chooseLabel="t('requests.form.attachments.choose')"
          @select="onFileSelected" />
        <div v-if="uploadFile" class="muted small">
          {{ uploadFile.name }} · {{ fmtBytes(uploadFile.size) }}
        </div>
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" text @click="uploadDialogVisible = false" />
      <Button :label="t('requests.form.attachments.upload')" icon="pi pi-upload" :loading="uploading" @click="uploadAttachment" />
    </template>
  </Dialog>
</template>

<style scoped>
.cards-grid { display: grid; grid-template-columns: 1fr; gap: 1rem }
.card { width: 100% }
.grid.two-col { display: grid; grid-template-columns: 1fr 1fr; gap: 1rem }
.field { display: flex; flex-direction: column; gap: .3rem; margin-bottom: .85rem }
.field label { font-weight: 600; font-size: .85rem; color: var(--p-text-muted-color) }
.muted { color: var(--p-text-muted-color) }
.small { font-size: .75rem }
.subtitle { display: flex; align-items: center; gap: .4rem; font-size: .85rem }
.type-hint {
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  padding: .5rem .75rem; border-radius: 6px; font-size: .85rem
}
.picked {
  display: flex; align-items: center; gap: .5rem;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  padding: .4rem .75rem; border-radius: 6px;
}
.picked-name { font-weight: 600 }
.picked.locked { background: transparent; opacity: .85 }
.search-list {
  margin: 0; padding: 0; list-style: none;
  border: 1px solid var(--p-content-border-color); border-radius: 6px;
  max-height: 220px; overflow-y: auto; background: var(--p-content-background);
}
.search-list li { padding: .4rem .65rem; cursor: pointer }
.search-list li:hover { background: var(--p-highlight-background) }
.res-line { display: flex; align-items: baseline; gap: .35rem; flex-wrap: wrap }
.res-veh { font-size: .78rem; margin-top: .1rem }
.picked-veh { font-size: .82rem; color: var(--p-text-muted-color) }
.picked { flex-wrap: wrap }
.audit-table { width: 100%; font-size: .85rem }
.audit-table td { padding: .25rem .5rem }
.audit-table td:first-child { font-weight: 600; color: var(--p-text-muted-color); width: 7rem }

.card-title-row { display: flex; align-items: center; gap: .6rem }
.count {
  display: inline-flex; align-items: center; justify-content: center;
  min-width: 1.5rem; height: 1.5rem; padding: 0 .4rem;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  border-radius: 999px; font-size: .8rem; font-weight: 600;
  color: var(--p-text-muted-color);
}
.card-title-row > .p-button { margin-left: auto }
.form-grid { display: flex; flex-direction: column; gap: .85rem }
.dl-link { color: var(--p-primary-color); text-decoration: none }
.dl-link:hover { text-decoration: underline }
</style>
