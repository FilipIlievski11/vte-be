<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Station, Company } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
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

const rows = ref<Station[]>([]);
const companies = ref<Company[]>([]);
const loading = ref(false);
const showDialog = ref(false);
const isNew = ref(false);
const editing = ref<Partial<Station>>({});

async function load() {
  loading.value = true;
  try {
    const [s, c] = await Promise.all([
      api.get<Station[]>('/stations'),
      auth.isAdmin ? api.get<Company[]>('/companies') : Promise.resolve({ data: [] as Company[] }),
    ]);
    rows.value = s.data;
    companies.value = c.data;
  } finally { loading.value = false; }
}
onMounted(load);

function openNew() {
  editing.value = { name: '', companyId: auth.companyId ?? companies.value[0]?.id ?? 1, active: true };
  isNew.value = true;
  showDialog.value = true;
}
function openEdit(s: Station) { editing.value = { ...s }; isNew.value = false; showDialog.value = true; }

async function save() {
  try {
    if (isNew.value) await api.post('/stations', editing.value);
    else await api.put(`/stations/${editing.value.id}`, editing.value);
    showDialog.value = false;
    await load();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  }
}

function remove(s: Station) {
  confirm.require({
    message: t('stations.deleteConfirm', { name: s.name }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try { await api.delete(`/stations/${s.id}`); await load(); }
      catch (e: any) { toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 }); }
    },
  });
}

function companyName(id: number) { return companies.value.find(c => c.id === id)?.name ?? `#${id}`; }
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('stations.title') }}</h1>
    </div>
    <div class="actions">
      <Button v-if="auth.isAdmin" :label="t('stations.new')" icon="pi pi-plus" size="small" severity="success" @click="openNew" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" size="small" stripedRows dataKey="id">
    <template #empty>
      <PagedTableEmpty icon="pi-map-marker" :title="t('stations.none')" :hint="t('empty.noRowsHint')"
        :ctaLabel="t('stations.new')" @cta="openNew" />
    </template>
    <Column field="id" :header="t('ref.fields.id')" style="width:80px" />
    <Column field="name" :header="t('ref.fields.name')" />
    <Column :header="t('admin.companies')">
      <template #body="{ data }">{{ companyName(data.companyId) }}</template>
    </Column>
    <Column :header="t('companies.active')" style="width:90px">
      <template #body="{ data }">
        <Tag :value="data.active ? t('common.yes') : t('common.no')" :severity="data.active ? 'success' : 'danger'" />
      </template>
    </Column>
    <Column v-if="auth.isAdmin" :header="t('common.actions')" style="width:100px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text rounded severity="secondary" @click="openEdit(data)" />
        <Button icon="pi pi-trash" text rounded severity="danger" @click="remove(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" modal :header="isNew ? t('stations.new') : t('stations.editTitle', { id: editing.id })" style="width: 420px">
    <div class="form-grid">
      <div class="field full">
        <label>{{ t('stations.nameRequired') }}</label>
        <InputText v-model="editing.name" />
      </div>
      <div v-if="auth.isAdmin" class="field full">
        <label>{{ t('stations.companyRequired') }}</label>
        <Select v-model="editing.companyId" :options="companies" optionLabel="name" optionValue="id" filter />
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
