<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Checkbox from 'primevue/checkbox';
import InputNumber from 'primevue/inputnumber';
import Select from 'primevue/select';
import Dialog from 'primevue/dialog';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const props = defineProps<{ kind: string }>();
const { t } = useI18n();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

interface FieldDef {
  key: string;
  labelKey: string;
  type: 'text' | 'bool' | 'number' | 'select';
  required?: boolean;
  options?: () => any[];
  optionLabel?: string;
  optionValue?: string;
}

interface KindConfig {
  endpoint: string;
  titleKey: string;
  fields: FieldDef[];
}

const countries = ref<any[]>([]);
const communities = ref<any[]>([]);

const kinds: Record<string, KindConfig> = {
  'countries': {
    endpoint: '/countries',
    titleKey: 'ref.titles.countries',
    fields: [
      { key: 'name',      labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'shortName', labelKey: 'ref.fields.shortName', type: 'text' },
      { key: 'active',    labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'communities': {
    endpoint: '/communities',
    titleKey: 'ref.titles.communities',
    fields: [
      { key: 'name',      labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'countryId', labelKey: 'ref.fields.country', type: 'select', required: true,
        options: () => countries.value, optionLabel: 'name', optionValue: 'id' },
      { key: 'code',      labelKey: 'ref.fields.code', type: 'text' },
      { key: 'plateNumberPrefix', labelKey: 'ref.fields.plateNumberPrefix', type: 'text' },
      { key: 'active',    labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'cities': {
    endpoint: '/cities',
    titleKey: 'ref.titles.cities',
    fields: [
      { key: 'name',        labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'communityId', labelKey: 'ref.fields.community', type: 'select', required: true,
        options: () => communities.value, optionLabel: 'name', optionValue: 'id' },
      { key: 'postalCode',  labelKey: 'ref.fields.postalCode', type: 'text', required: true },
      { key: 'active',      labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'citizenships': {
    endpoint: '/citizenships',
    titleKey: 'ref.titles.citizenships',
    fields: [
      { key: 'name',      labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'countryId', labelKey: 'ref.fields.country', type: 'select', required: true,
        options: () => countries.value, optionLabel: 'name', optionValue: 'id' },
    ],
  },
  'document-issuers': {
    endpoint: '/document-issuers',
    titleKey: 'ref.titles.document-issuers',
    fields: [
      { key: 'name',   labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'active', labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'personal-data-types': {
    endpoint: '/personal-data-types',
    titleKey: 'ref.titles.personal-data-types',
    fields: [
      { key: 'id',   labelKey: 'ref.fields.id', type: 'number', required: true },
      { key: 'name', labelKey: 'ref.fields.name', type: 'text', required: true },
    ],
  },
  'request-document-prints': {
    endpoint: '/request-document-prints',
    titleKey: 'ref.titles.request-document-prints',
    fields: [
      { key: 'code',         labelKey: 'ref.fields.code', type: 'text', required: true },
      { key: 'name',         labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'templatePath', labelKey: 'ref.fields.templatePath', type: 'text' },
      { key: 'active',       labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'request-ownership-proof-types': {
    endpoint: '/request-ownership-proof-types',
    titleKey: 'ref.titles.request-ownership-proof-types',
    fields: [
      { key: 'name',   labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'active', labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'request-payment-proof-types': {
    endpoint: '/request-payment-proof-types',
    titleKey: 'ref.titles.request-payment-proof-types',
    fields: [
      { key: 'name',   labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'active', labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
  'request-attachment-types': {
    endpoint: '/request-attachment-types',
    titleKey: 'ref.titles.request-attachment-types',
    fields: [
      { key: 'name',   labelKey: 'ref.fields.name', type: 'text', required: true },
      { key: 'active', labelKey: 'ref.fields.active', type: 'bool' },
    ],
  },
};

const config = computed<KindConfig | null>(() => kinds[props.kind] ?? null);

const rows = ref<any[]>([]);
const loading = ref(false);
const dialogVisible = ref(false);
const editing = ref<any>({});
const isNew = ref(false);

async function loadDeps() {
  if (props.kind === 'communities' || props.kind === 'citizenships' || props.kind === 'cities') {
    if (!countries.value.length) {
      const { data } = await api.get('/countries');
      countries.value = data;
    }
  }
  if (props.kind === 'cities') {
    if (!communities.value.length) {
      const { data } = await api.get('/communities');
      communities.value = data;
    }
  }
}

async function load() {
  if (!config.value) return;
  loading.value = true;
  try {
    await loadDeps();
    const { data } = await api.get(config.value.endpoint);
    rows.value = data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

watch(() => props.kind, load, { immediate: false });
onMounted(load);

function blankRow() {
  if (!config.value) return {};
  const o: any = {};
  for (const f of config.value.fields) {
    o[f.key] = f.type === 'bool' ? true : (f.type === 'number' ? 0 : '');
  }
  return o;
}

function openNew() { editing.value = blankRow(); isNew.value = true; dialogVisible.value = true; }
function openEdit(row: any) { editing.value = { ...row }; isNew.value = false; dialogVisible.value = true; }

async function save() {
  if (!config.value) return;
  try {
    if (isNew.value) await api.post(config.value.endpoint, editing.value);
    else await api.put(`${config.value.endpoint}/${editing.value.id}`, editing.value);
    dialogVisible.value = false;
    await load();
    toast.add({ severity: 'success', summary: t('clients.form.saved'), life: 1500 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  }
}

function remove(row: any) {
  if (!config.value) return;
  const ep = config.value.endpoint;
  confirm.require({
    message: t('common.confirmDeleteGeneric', { name: row.name ?? row.id }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`${ep}/${row.id}`);
        await load();
        toast.add({ severity: 'success', summary: t('common.deleted'), life: 1500 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

function visibleColumns() {
  return config.value?.fields ?? [];
}
function selectLabel(f: FieldDef, value: any) {
  const opts = f.options ? f.options() : [];
  const optionValue = f.optionValue ?? 'id';
  const optionLabel = f.optionLabel ?? 'name';
  return opts.find((o: any) => o[optionValue] === value)?.[optionLabel] ?? value;
}
</script>

<template>
  <template v-if="config">
    <div class="page-header">
      <div>
        <h1>{{ t(config.titleKey) }}</h1>
      </div>
      <div class="actions">
        <Button v-if="auth.isAdmin" :label="t('ref.newEntry')" icon="pi pi-plus" size="small" severity="success" @click="openNew" />
      </div>
    </div>

    <DataTable :value="rows" :loading="loading" size="small" stripedRows :paginator="true" :rows="50" :rowsPerPageOptions="[25,50,100,200]" dataKey="id">
      <template #empty><div class="muted" style="padding:1rem">{{ t('empty.noRows') }}</div></template>
      <Column field="id" :header="t('ref.fields.id')" style="width:80px" />
      <Column v-for="f in visibleColumns()" :key="f.key" :field="f.key" :header="t(f.labelKey)">
        <template #body="{ data }">
          <template v-if="f.type === 'bool'">
            <Tag :value="data[f.key] ? t('common.yes') : t('common.no')" :severity="data[f.key] ? 'success' : 'danger'" />
          </template>
          <template v-else-if="f.type === 'select'">{{ selectLabel(f, data[f.key]) }}</template>
          <template v-else>{{ data[f.key] }}</template>
        </template>
      </Column>
      <Column v-if="auth.isAdmin" :header="t('common.actions')" style="width:100px">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" text rounded severity="secondary" @click="openEdit(data)" />
          <Button icon="pi pi-trash" text rounded severity="danger" @click="remove(data)" />
        </template>
      </Column>
    </DataTable>

    <Dialog v-model:visible="dialogVisible" modal :header="isNew ? t('ref.newEntry') : t('ref.editEntry', { id: editing.id })" style="width: 440px">
      <div class="form-grid">
        <div v-for="f in config.fields" :key="f.key" class="field full">
          <label>{{ t(f.labelKey) }}{{ f.required ? ' *' : '' }}</label>
          <InputText v-if="f.type === 'text'" v-model="editing[f.key]" />
          <InputNumber v-else-if="f.type === 'number'" v-model="editing[f.key]" :useGrouping="false" :disabled="!isNew && f.key === 'id'" />
          <Select
            v-else-if="f.type === 'select'"
            v-model="editing[f.key]"
            :options="f.options ? f.options() : []"
            :optionLabel="f.optionLabel ?? 'name'"
            :optionValue="f.optionValue ?? 'id'"
            filter
          />
          <label v-else-if="f.type === 'bool'" style="display:flex; align-items:center; gap:0.4rem; margin:0">
            <Checkbox v-model="editing[f.key]" :binary="true" /> {{ t(f.labelKey) }}
          </label>
        </div>
      </div>
      <template #footer>
        <Button :label="t('common.cancel')" text severity="secondary" @click="dialogVisible = false" />
        <Button :label="t('common.save')" icon="pi pi-check" @click="save" />
      </template>
    </Dialog>
  </template>
  <div v-else class="page-header"><p class="muted">{{ t('ref.unknownKind', { kind: props.kind }) }}</p></div>
</template>
