<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import {
  TechnicalExamRequirement,
  type RequestType,
  type RequestDocumentPrint,
} from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Checkbox from 'primevue/checkbox';
import Select from 'primevue/select';
import Dialog from 'primevue/dialog';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';
import { useConfirm } from 'primevue/useconfirm';

const { t } = useI18n();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

const rows = ref<RequestType[]>([]);
const prints = ref<RequestDocumentPrint[]>([]);
const loading = ref(false);

const dialogVisible = ref(false);
const isNew = ref(false);
const editing = ref<RequestType>(blank());

function blank(): RequestType {
  return {
    id: 0,
    parentRequestTypeId: null,
    documentPrintId: prints.value[0]?.id ?? 0,
    name: '',
    description: null,
    technicalExamRequirement: TechnicalExamRequirement.Required,
    paymentRequired: true,
    issuesNewRegistration: false,
    deactivatesRelation: false,
    deactivatesVehicle: false,
    transfersOwnership: false,
    mutatesVehicleData: false,
    mutatesClientData: false,
    isSufficient: false,
    previousRegistrationRequired: false,
    active: true,
  };
}

const examReqOptions = computed(() => [
  { label: t('requestTypes.exam.notRequired'), value: TechnicalExamRequirement.NotRequired },
  { label: t('requestTypes.exam.required'),    value: TechnicalExamRequirement.Required },
  { label: t('requestTypes.exam.optional'),    value: TechnicalExamRequirement.Optional },
]);

const parentOptions = computed(() => [
  { id: null, name: t('requestTypes.form.noParent') },
  ...rows.value.filter(r => r.id !== editing.value.id).map(r => ({ id: r.id, name: r.name })),
]);

async function load() {
  loading.value = true;
  try {
    const [tRes, pRes] = await Promise.all([
      api.get<RequestType[]>('/request-types'),
      api.get<RequestDocumentPrint[]>('/request-document-prints'),
    ]);
    rows.value = tRes.data;
    prints.value = pRes.data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}
onMounted(load);

function printLabel(id: number): string {
  return prints.value.find(p => p.id === id)?.name ?? `#${id}`;
}
function examLabel(v: number): string {
  return examReqOptions.value.find(o => o.value === v)?.label ?? '';
}

function openNew() {
  editing.value = blank();
  isNew.value = true;
  dialogVisible.value = true;
}
function openEdit(row: RequestType) {
  editing.value = { ...row };
  isNew.value = false;
  dialogVisible.value = true;
}

async function save() {
  if (!editing.value.name.trim()) {
    toast.add({ severity: 'warn', summary: t('common.saveFailed'), detail: t('requestTypes.form.nameRequired'), life: 3000 });
    return;
  }
  const body = { ...editing.value };
  // Backend uses positional record DTO without an id, but Omit allows id to be in payload — it's ignored.
  try {
    if (isNew.value) await api.post('/request-types', body);
    else await api.put(`/request-types/${editing.value.id}`, body);
    dialogVisible.value = false;
    await load();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  }
}

function remove(row: RequestType) {
  confirm.require({
    message: t('common.confirmDeleteGeneric', { name: row.name }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/request-types/${row.id}`);
        await load();
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('requestTypes.title') }}</h1></div>
    <div class="actions">
      <Button v-if="auth.isAdmin" :label="t('requestTypes.new')" icon="pi pi-plus" size="small" severity="success" @click="openNew" />
    </div>
  </div>

  <DataTable
    :value="rows"
    :loading="loading"
    size="small"
    stripedRows
    :paginator="true"
    :rows="50"
    :rowsPerPageOptions="[25,50,100]"
    dataKey="id">
    <template #empty>
        <PagedTableEmpty icon="pi-file-edit" :title="t('empty.noRows')" :hint="t('empty.noRowsHint')"
          :ctaLabel="t('requestTypes.new')" @cta="openNew" />
      </template>
    <Column field="id" :header="t('ref.fields.id')" style="width:70px" />
    <Column field="name" :header="t('requestTypes.col.name')" />
    <Column :header="t('requestTypes.col.print')" style="width:220px">
      <template #body="{ data }">{{ printLabel(data.documentPrintId) }}</template>
    </Column>
    <Column :header="t('requestTypes.col.exam')" style="width:130px">
      <template #body="{ data }">{{ examLabel(data.technicalExamRequirement) }}</template>
    </Column>
    <Column :header="t('requestTypes.col.flags')">
      <template #body="{ data }">
        <div class="flag-chips">
          <Tag v-if="data.paymentRequired"               severity="info"    :value="t('requestTypes.flags.paymentRequired')" />
          <Tag v-if="data.issuesNewRegistration"         severity="success" :value="t('requestTypes.flags.issuesNewRegistration')" />
          <Tag v-if="data.deactivatesRelation"           severity="warn"    :value="t('requestTypes.flags.deactivatesRelation')" />
          <Tag v-if="data.deactivatesVehicle"            severity="danger"  :value="t('requestTypes.flags.deactivatesVehicle')" />
          <Tag v-if="data.transfersOwnership"            severity="info"    :value="t('requestTypes.flags.transfersOwnership')" />
          <Tag v-if="data.previousRegistrationRequired"  severity="secondary" :value="t('requestTypes.flags.previousRegistrationRequired')" />
        </div>
      </template>
    </Column>
    <Column field="active" :header="t('ref.fields.active')" style="width:90px" bodyStyle="text-align:center">
      <template #body="{ data }">
        <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width:120px" bodyStyle="text-align:right">
      <template #body="{ data }">
        <Button v-if="auth.isAdmin" icon="pi pi-pencil" size="small" text @click="openEdit(data)" />
        <Button v-if="auth.isAdmin" icon="pi pi-trash"  size="small" text severity="danger" @click="remove(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog
    v-model:visible="dialogVisible"
    modal
    :header="isNew ? t('requestTypes.form.newTitle') : t('requestTypes.form.editTitle', { id: editing.id })"
    :style="{ width: '720px' }">
    <div class="form-grid">
      <div class="row">
        <label>{{ t('requestTypes.form.name') }} *</label>
        <InputText v-model="editing.name" autofocus />
      </div>
      <div class="row">
        <label>{{ t('requestTypes.form.description') }}</label>
        <Textarea v-model="editing.description" rows="2" autoResize />
      </div>
      <div class="row two-col">
        <div>
          <label>{{ t('requestTypes.form.print') }} *</label>
          <Select v-model="editing.documentPrintId" :options="prints" optionLabel="name" optionValue="id" />
        </div>
        <div>
          <label>{{ t('requestTypes.form.parent') }}</label>
          <Select v-model="editing.parentRequestTypeId" :options="parentOptions" optionLabel="name" optionValue="id" />
        </div>
      </div>
      <div class="row">
        <label>{{ t('requestTypes.form.exam') }}</label>
        <Select v-model="editing.technicalExamRequirement" :options="examReqOptions" optionLabel="label" optionValue="value" />
      </div>

      <fieldset class="flags">
        <legend>{{ t('requestTypes.form.flagsLegend') }}</legend>
        <div class="flag-grid">
          <label><Checkbox v-model="editing.paymentRequired"               binary />&nbsp;{{ t('requestTypes.flags.paymentRequired') }}</label>
          <label><Checkbox v-model="editing.issuesNewRegistration"         binary />&nbsp;{{ t('requestTypes.flags.issuesNewRegistration') }}</label>
          <label><Checkbox v-model="editing.deactivatesRelation"           binary />&nbsp;{{ t('requestTypes.flags.deactivatesRelation') }}</label>
          <label><Checkbox v-model="editing.deactivatesVehicle"            binary />&nbsp;{{ t('requestTypes.flags.deactivatesVehicle') }}</label>
          <label><Checkbox v-model="editing.transfersOwnership"            binary />&nbsp;{{ t('requestTypes.flags.transfersOwnership') }}</label>
          <label><Checkbox v-model="editing.previousRegistrationRequired"  binary />&nbsp;{{ t('requestTypes.flags.previousRegistrationRequired') }}</label>
          <label><Checkbox v-model="editing.mutatesVehicleData"            binary />&nbsp;{{ t('requestTypes.flags.mutatesVehicleData') }}</label>
          <label><Checkbox v-model="editing.mutatesClientData"             binary />&nbsp;{{ t('requestTypes.flags.mutatesClientData') }}</label>
          <label><Checkbox v-model="editing.isSufficient"                  binary />&nbsp;{{ t('requestTypes.flags.isSufficient') }}</label>
          <label><Checkbox v-model="editing.active"                        binary />&nbsp;{{ t('ref.fields.active') }}</label>
        </div>
      </fieldset>
    </div>

    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" text @click="dialogVisible = false" />
      <Button :label="t('common.save')"   icon="pi pi-check" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
.flag-chips { display: flex; flex-wrap: wrap; gap: 4px }
.form-grid { display: flex; flex-direction: column; gap: .85rem }
.row { display: flex; flex-direction: column; gap: .3rem }
.row label { font-weight: 600; font-size: .85rem; color: var(--p-text-muted-color) }
.row.two-col { flex-direction: row; gap: 1rem }
.row.two-col > div { flex: 1; display: flex; flex-direction: column; gap: .3rem }
.flags { border: 1px solid var(--p-content-border-color); border-radius: 8px; padding: .75rem 1rem }
.flags legend { padding: 0 .35rem; font-weight: 600; font-size: .85rem; color: var(--p-text-muted-color) }
.flag-grid { display: grid; grid-template-columns: 1fr 1fr; gap: .55rem .9rem; padding-top: .35rem }
.flag-grid label { display: flex; align-items: center }
</style>
