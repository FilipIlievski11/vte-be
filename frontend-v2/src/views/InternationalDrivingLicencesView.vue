<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { openPrintTab } from '@/utils/print';
import type { Paged, IdlListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const toast = useToast();

const items = ref<IdlListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const q = ref('');
const page = ref(1);
const pageSize = ref(50);
const sortField = ref<string>('id');
const sortOrder = ref<number>(-1);

let searchTimer: number | undefined;
let loadToken = 0;

async function load() {
  const myToken = ++loadToken;
  loading.value = true;
  try {
    const { data } = await api.get<Paged<IdlListItem>>('/international-driving-licences', {
      params: {
        q: q.value || undefined,
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
    toast.add({ severity: 'error', summary: t('internationalDrivingLicences.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    if (myToken === loadToken) loading.value = false;
  }
}

onMounted(load);

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});

function onPage(ev: { page: number; rows: number }) {
  page.value = ev.page + 1;
  pageSize.value = ev.rows;
  load();
}
function onSort(ev: any) {
  sortField.value = (typeof ev?.sortField === 'string' && ev.sortField) ? ev.sortField : 'id';
  sortOrder.value = ev?.sortOrder === 1 ? 1 : -1;
  page.value = 1;
  load();
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}

// Reprint anytime — the licence stores the full applicant snapshot, so old prints stay stable.
function openPrint(id: number) {
  openPrintTab(router.resolve({ name: 'idl-print', params: { id } }).href);
}
function openPermit(id: number) {
  openPrintTab(router.resolve({ name: 'idl-permit', params: { id } }).href);
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('internationalDrivingLicences.title') }}</h1>
      <div class="subtitle">{{ t('internationalDrivingLicences.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Button :label="t('internationalDrivingLicences.new')" icon="pi pi-plus" size="small" @click="router.push('/international-driving-licences/new')" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small" class="tight-table"
  >
    <Column :header="t('internationalDrivingLicences.col.numberOfLicence')"><template #body><Skeleton /></template></Column>
    <Column :header="t('internationalDrivingLicences.col.client')"><template #body><Skeleton /></template></Column>
    <Column :header="t('internationalDrivingLicences.col.issued')"><template #body><Skeleton /></template></Column>
    <Column :header="t('internationalDrivingLicences.col.expires')"><template #body><Skeleton /></template></Column>
    <Column :header="t('internationalDrivingLicences.col.categories')"><template #body><Skeleton /></template></Column>
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
    @row-click="(e: any) => router.push(`/international-driving-licences/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-id-card"
        :title="t('internationalDrivingLicences.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
      />
    </template>

    <Column field="numberOfLicence" sortField="number" sortable :header="t('internationalDrivingLicences.col.numberOfLicence')" style="width:180px">
      <template #body="{ data }"><span class="mono">{{ data.numberOfLicence }}</span></template>
    </Column>
    <Column :header="t('internationalDrivingLicences.col.client')">
      <template #body="{ data }">
        <span v-if="data.clientName">{{ data.clientName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="issued" sortable :header="t('internationalDrivingLicences.col.issued')" style="width:110px">
      <template #body="{ data }">{{ fmtDate(data.issuedDate) }}</template>
    </Column>
    <Column sortField="expires" sortable :header="t('internationalDrivingLicences.col.expires')" style="width:110px">
      <template #body="{ data }">{{ fmtDate(data.validTillDate) }}</template>
    </Column>
    <Column :header="t('internationalDrivingLicences.col.categories')">
      <template #body="{ data }">
        <Tag v-for="c in data.categoryCodes" :key="c" :value="c" severity="secondary" style="margin-right:.25rem" />
        <span v-if="!data.categoryCodes.length" class="muted">—</span>
      </template>
    </Column>
    <Column style="width:76px" bodyStyle="text-align:right">
      <template #body="{ data }">
        <Button icon="pi pi-print" text rounded size="small" severity="secondary"
                v-tooltip.left="t('internationalDrivingLicences.printRequest')"
                @click.stop="openPrint(data.id)" />
        <Button icon="pi pi-id-card" text rounded size="small" severity="secondary"
                v-tooltip.left="t('internationalDrivingLicences.printPermit')"
                @click.stop="openPermit(data.id)" />
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
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 240px; font-size: 0.8125rem; }
.mono { font-family: monospace; font-weight: 600; }
.muted { color: var(--color-text-muted); }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
</style>
