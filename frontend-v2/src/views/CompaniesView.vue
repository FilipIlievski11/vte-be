<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { Company } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Checkbox from 'primevue/checkbox';
import Dialog from 'primevue/dialog';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';
import { useConfirm } from 'primevue/useconfirm';

const { t } = useI18n();
const rows = ref<Company[]>([]);
const loading = ref(false);
const showDialog = ref(false);
const editing = ref<Partial<Company>>({});
const isNew = ref(false);
const toast = useToast();
const confirm = useConfirm();

async function load() {
  loading.value = true;
  try {
    const { data } = await api.get<Company[]>('/companies');
    rows.value = data;
  } finally { loading.value = false; }
}
onMounted(load);

function openNew() { editing.value = { name: '', active: true }; isNew.value = true; showDialog.value = true; }
function openEdit(c: Company) { editing.value = { ...c }; isNew.value = false; showDialog.value = true; }

async function save() {
  try {
    if (isNew.value) await api.post('/companies', editing.value);
    else await api.put(`/companies/${editing.value.id}`, editing.value);
    showDialog.value = false;
    await load();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  }
}

function remove(c: Company) {
  confirm.require({
    message: t('companies.deleteConfirm', { name: c.name }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try { await api.delete(`/companies/${c.id}`); await load(); }
      catch (e: any) { toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 }); }
    },
  });
}
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('companies.title') }}</h1>
    </div>
    <div class="actions">
      <Button :label="t('companies.new')" icon="pi pi-plus" size="small" severity="success" @click="openNew" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" size="small" stripedRows dataKey="id">
    <template #empty>
      <PagedTableEmpty icon="pi-building" :title="t('empty.noRows')" :hint="t('empty.noRowsHint')"
        :ctaLabel="t('companies.new')" @cta="openNew" />
    </template>
    <Column field="id" :header="t('ref.fields.id')" style="width:80px" />
    <Column field="name" :header="t('ref.fields.name')" />
    <Column field="createdAt" :header="t('companies.created')" style="width:180px">
      <template #body="{ data }">{{ new Date(data.createdAt).toLocaleString() }}</template>
    </Column>
    <Column :header="t('companies.active')" style="width:90px">
      <template #body="{ data }">
        <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width:100px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text rounded severity="secondary" @click="openEdit(data)" />
        <Button icon="pi pi-trash" text rounded severity="danger" @click="remove(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" modal :header="isNew ? t('companies.new') : t('companies.editTitle', { id: editing.id })" style="width: 420px">
    <div class="form-grid">
      <div class="field full">
        <label>{{ t('companies.nameRequired') }}</label>
        <InputText v-model="editing.name" />
      </div>
      <div class="field full">
        <label style="display:flex; align-items:center; gap:0.4rem; margin:0">
          <Checkbox v-model="editing.active" :binary="true" /> {{ t('companies.active') }}
        </label>
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" text severity="secondary" @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" @click="save" />
    </template>
  </Dialog>
</template>
