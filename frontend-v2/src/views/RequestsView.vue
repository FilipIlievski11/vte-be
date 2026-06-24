<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Company, Paged, RequestListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import SelectButton from 'primevue/selectbutton';
import Tag from 'primevue/tag';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

type Status = 'open' | 'closed' | 'all';

const items = ref<RequestListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const q = ref('');
const status = ref<Status>('open');
const companyFilter = ref<number | null>(null);
const companies = ref<Company[]>([]);
const page = ref(1);
const pageSize = ref(50);
const sortField = ref<string>('created');
const sortOrder = ref<number>(-1);   // -1 desc (newest first), 1 asc

const statusOptions = computed(() => [
  { value: 'open' as Status,   label: t('requests.tabs.open') },
  { value: 'closed' as Status, label: t('requests.tabs.closed') },
  { value: 'all' as Status,    label: t('requests.tabs.all') },
]);

const companyOptions = computed(() => [
  { id: null as number | null, name: t('requests.allCompanies') },
  ...companies.value,
]);

let searchTimer: number | undefined;
let loadToken = 0;

async function loadCompanies() {
  if (!auth.isAdmin) return;
  try { companies.value = (await api.get<Company[]>('/companies')).data; }
  catch { /* ignore */ }
}

async function load() {
  const myToken = ++loadToken;
  loading.value = true;
  try {
    const { data } = await api.get<Paged<RequestListItem>>('/requests', {
      params: {
        q: q.value || undefined,
        status: status.value,
        companyId: companyFilter.value ?? undefined,
        sort: sortField.value,
        dir: sortOrder.value === 1 ? 'asc' : 'desc',
        page: page.value,
        pageSize: pageSize.value,
      },
    });
    if (myToken !== loadToken) return;
    items.value = data.items;
    total.value = data.total;
  } catch (e: any) {
    if (myToken !== loadToken) return;
    toast.add({ severity: 'error', summary: t('requests.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    if (myToken === loadToken) loading.value = false;
  }
}

onMounted(async () => { await loadCompanies(); await load(); });

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});
watch(status, () => { page.value = 1; load(); });
watch(companyFilter, () => { page.value = 1; load(); });

function onPage(ev: { page: number; rows: number }) {
  page.value = ev.page + 1;
  pageSize.value = ev.rows;
  load();
}

function onSort(ev: any) {
  sortField.value = (typeof ev?.sortField === 'string' && ev.sortField) ? ev.sortField : 'created';
  sortOrder.value = ev?.sortOrder === 1 ? 1 : -1;
  page.value = 1;
  load();
}

function add() { router.push('/requests/new'); }
function edit(r: RequestListItem) { router.push(`/requests/${r.id}`); }

function remove(r: RequestListItem) {
  confirm.require({
    message: t('requests.deleteConfirm', { id: r.id }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/requests/${r.id}`);
        toast.add({ severity: 'success', summary: t('requests.deleted'), life: 2000 });
        load();
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  return d.toLocaleDateString();
}

function rowStatus(r: RequestListItem): { label: string; severity: 'success' | 'info' | 'danger' } {
  if (!r.active) return { label: t('requests.status.inactive'), severity: 'danger' };
  if (r.endedAt) return { label: t('requests.status.closed'), severity: 'info' };
  return { label: t('requests.status.open'), severity: 'success' };
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('requests.title') }}</h1>
      <div class="subtitle">{{ t('requests.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <SelectButton
        v-model="status"
        :options="statusOptions"
        optionLabel="label"
        optionValue="value"
        :allowEmpty="false"
        size="small"
        class="status-tabs"
      />
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('requests.searchHint')" size="small" style="min-width:300px" />
      </span>
      <Select
        v-if="auth.isAdmin"
        v-model="companyFilter"
        :options="companyOptions"
        optionLabel="name" optionValue="id"
        :placeholder="t('requests.allCompanies')"
        size="small" class="filter" showClear
      />
      <Button :label="t('requests.new')" icon="pi pi-plus" size="small" severity="success" @click="add" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small"
    class="tight-table"
  >
    <Column header="#"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.col.type')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.col.client')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.col.vehicle')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.col.created')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.col.status')"><template #body><Skeleton /></template></Column>
  </DataTable>

  <DataTable
    v-else
    class="tight-table"
    :value="items"
    :loading="loading"
    stripedRows size="small"
    lazy paginator
    :first="(page - 1) * pageSize"
    :rows="pageSize"
    :totalRecords="total"
    :rowsPerPageOptions="[25, 50, 100, 200]"
    @page="onPage"
    :sortField="sortField"
    :sortOrder="sortOrder"
    @sort="onSort"
    rowHover
    @row-click="(e: any) => router.push(`/requests/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-file-edit"
        :title="t('requests.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
        :ctaLabel="t('requests.new')"
        @cta="add"
      />
    </template>

    <Column field="id" sortField="id" sortable header="#" style="width:96px">
      <template #body="{ data }"><span class="req-no">{{ data.id }}</span></template>
    </Column>
    <Column field="requestTypeName" sortField="type" sortable :header="t('requests.col.type')">
      <template #body="{ data }">{{ data.requestTypeName || '—' }}</template>
    </Column>
    <Column field="clientDisplayName" sortField="client" sortable :header="t('requests.col.client')">
      <template #body="{ data }">
        <span v-if="data.clientDisplayName">{{ data.clientDisplayName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="vehicle" sortable :header="t('requests.col.vehicle')">
      <template #body="{ data }">
        <div v-if="data.vehicleVin || data.vehiclePlate">
          <span v-if="data.vehiclePlate" class="plate">{{ data.vehiclePlate }}</span>
          <span v-if="data.vehicleVin" class="muted small">{{ data.vehicleVin }}</span>
        </div>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="created" sortable :header="t('requests.col.created')" style="width:110px">
      <template #body="{ data }">{{ fmtDate(data.createdAt) }}</template>
    </Column>
    <Column sortField="status" sortable :header="t('requests.col.status')" style="width:110px">
      <template #body="{ data }">
        <Tag :value="rowStatus(data).label" :severity="rowStatus(data).severity" />
      </template>
    </Column>
    <Column field="createdByUserName" sortField="operator" sortable :header="t('requests.col.operator')" style="width:140px">
      <template #body="{ data }">{{ data.createdByUserName || '—' }}</template>
    </Column>
    <Column :header="t('requests.col.actions')" style="width:90px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="edit(data)" v-tooltip.left="t('common.edit')" />
        <Button icon="pi pi-trash"  text severity="danger" @click.stop="remove(data)" v-tooltip.left="t('common.delete')" />
      </template>
    </Column>
  </DataTable>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i {
  position: absolute; left: 0.5rem;
  color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem;
}
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 220px; font-size: 0.8125rem; }
.req-no { font-family: ui-monospace, monospace; font-weight: 600; }
.filter { min-width: 200px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }
.status-tabs :deep(.p-togglebutton) { font-size: 0.8125rem; padding: .3rem .7rem }
.plate { display: inline-block; font-family: monospace; font-weight: 600; padding-right: .5rem }
.small { font-size: .7rem }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-button.p-button-icon-only) { width: 1.75rem; height: 1.75rem; }
</style>
