<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import type { Paged, PaymentListItem, PaymentTypeLookup } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import SelectButton from 'primevue/selectbutton';
import Tag from 'primevue/tag';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const toast = useToast();

type Status = 'all' | 'paid' | 'unpaid' | 'storno';

const items = ref<PaymentListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const q = ref('');
const status = ref<Status>('all');
const typeFilter = ref<number | null>(null);
const types = ref<PaymentTypeLookup[]>([]);
const page = ref(1);
const pageSize = ref(50);
const sortField = ref<string>('issue');
const sortOrder = ref<number>(-1);

const statusOptions = computed(() => [
  { value: 'all'    as Status, label: t('payments.tabs.all') },
  { value: 'paid'   as Status, label: t('payments.tabs.paid') },
  { value: 'unpaid' as Status, label: t('payments.tabs.unpaid') },
  { value: 'storno' as Status, label: t('payments.tabs.storno') },
]);

const typeOptions = computed(() => [
  { id: null as number | null, label: t('payments.allTypes') },
  ...types.value.map(p => ({ id: p.id, label: p.name })),
]);

let searchTimer: number | undefined;
let loadToken = 0;

async function loadTypes() {
  try { types.value = (await api.get<PaymentTypeLookup[]>('/payment-documents/payment-types')).data; }
  catch { /* ignore */ }
}

async function load() {
  const myToken = ++loadToken;
  loading.value = true;
  try {
    const { data } = await api.get<Paged<PaymentListItem>>('/payment-documents', {
      params: {
        q: q.value || undefined,
        status: status.value,
        paymentTypeId: typeFilter.value ?? undefined,
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
    toast.add({ severity: 'error', summary: t('payments.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    if (myToken === loadToken) loading.value = false;
  }
}

onMounted(async () => { await loadTypes(); await load(); });
watch(q, () => { window.clearTimeout(searchTimer); searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300); });
watch(status, () => { page.value = 1; load(); });
watch(typeFilter, () => { page.value = 1; load(); });

function onPage(ev: { page: number; rows: number }) { page.value = ev.page + 1; pageSize.value = ev.rows; load(); }
function onSort(ev: any) {
  sortField.value = (typeof ev?.sortField === 'string' && ev.sortField) ? ev.sortField : 'issue';
  sortOrder.value = ev?.sortOrder === 1 ? 1 : -1;
  page.value = 1; load();
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}
function fmtMoney(v: number | null | undefined): string {
  if (v == null) return '—';
  return v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
function statusTag(row: PaymentListItem): { label: string; severity: 'success' | 'warn' | 'danger' | 'secondary' } {
  if (row.stornoed)   return { label: t('payments.status.storno'), severity: 'danger' };
  if (row.paid)       return { label: t('payments.status.paid'),   severity: 'success' };
  return { label: t('payments.status.unpaid'), severity: 'warn' };
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('payments.title') }}</h1>
      <div class="subtitle">{{ t('payments.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <SelectButton
        v-model="status"
        :options="statusOptions"
        optionLabel="label" optionValue="value"
        :allowEmpty="false" size="small" class="status-tabs"
      />
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('payments.searchPlaceholder')" size="small" />
      </span>
      <Select
        v-model="typeFilter"
        :options="typeOptions"
        optionLabel="label" optionValue="id"
        :placeholder="t('payments.allTypes')"
        size="small" class="filter" showClear
      />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small" class="tight-table"
  >
    <Column :header="t('payments.col.docNumber')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.client')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.vehicle')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.type')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.issued')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.total')"><template #body><Skeleton /></template></Column>
    <Column :header="t('payments.col.status')"><template #body><Skeleton /></template></Column>
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
    @row-click="(e: any) => router.push(`/payments/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-wallet"
        :title="t('payments.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
      />
    </template>

    <Column field="documentNumber" sortField="doc" sortable :header="t('payments.col.docNumber')" style="width:140px">
      <template #body="{ data }"><span class="mono">{{ data.documentNumber }}</span></template>
    </Column>
    <Column :header="t('payments.col.client')">
      <template #body="{ data }">
        <span v-if="data.clientName">{{ data.clientName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column :header="t('payments.col.vehicle')">
      <template #body="{ data }">
        <div v-if="data.vehiclePlate || data.vehicleVin">
          <span v-if="data.vehiclePlate" class="plate">{{ data.vehiclePlate }}</span>
          <span v-if="data.vehicleVin"   class="muted small">{{ data.vehicleVin }}</span>
        </div>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column :header="t('payments.col.type')" style="width:140px">
      <template #body="{ data }">
        <span v-if="data.paymentTypeName">{{ data.paymentTypeName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="issue" sortable :header="t('payments.col.issued')" style="width:100px">
      <template #body="{ data }">{{ fmtDate(data.issueDate) }}</template>
    </Column>
    <Column sortField="due" sortable :header="t('payments.col.due')" style="width:100px">
      <template #body="{ data }">{{ fmtDate(data.dueDate) }}</template>
    </Column>
    <Column :header="t('payments.col.total')" style="width:110px; text-align:right">
      <template #body="{ data }"><span class="mono">{{ fmtMoney(data.linesTotal) }}</span></template>
    </Column>
    <Column :header="t('payments.col.status')" style="width:110px">
      <template #body="{ data }">
        <Tag :value="statusTag(data).label" :severity="statusTag(data).severity" />
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
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 260px; font-size: 0.8125rem; }
.filter { min-width: 170px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }
.status-tabs :deep(.p-togglebutton) { font-size: 0.8125rem; padding: .3rem .7rem; }
.mono { font-family: monospace; font-weight: 600; }
.plate { display: inline-block; font-family: monospace; font-weight: 600; padding-right: .5rem; }
.small { font-size: .7rem; }
.muted { color: var(--color-text-muted); }
.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
</style>
