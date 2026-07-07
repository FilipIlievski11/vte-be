<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { openPrintTab } from '@/utils/print';
import type { Paged, VehiclePermissionListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const toast = useToast();

const items = ref<VehiclePermissionListItem[]>([]);
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
    const { data } = await api.get<Paged<VehiclePermissionListItem>>('/vehicle-permissions', {
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
    toast.add({ severity: 'error', summary: t('vehiclePermissions.loadFailed'), detail: e?.message, life: 4000 });
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

// Reprint anytime — the permission stores the full snapshot, so old prints stay stable.
function openCertificate(id: number) {
  openPrintTab(router.resolve({ name: 'vehicle-permission-print', params: { id } }).href);
}
function openRequest(id: number) {
  openPrintTab(router.resolve({ name: 'vehicle-permission-request-print', params: { id } }).href);
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('vehiclePermissions.title') }}</h1>
      <div class="subtitle">{{ t('vehiclePermissions.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Button :label="t('vehiclePermissions.new')" icon="pi pi-plus" size="small" @click="router.push('/vehicle-permissions/new')" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small" class="tight-table"
  >
    <Column :header="t('vehiclePermissions.col.number')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehiclePermissions.col.plate')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehiclePermissions.col.owner')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehiclePermissions.col.authorized')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehiclePermissions.col.issued')"><template #body><Skeleton /></template></Column>
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
    @row-click="(e: any) => router.push(`/vehicle-permissions/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-file-check"
        :title="t('vehiclePermissions.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
      />
    </template>

    <Column field="permissionNumber" sortField="number" sortable :header="t('vehiclePermissions.col.number')" style="width:130px">
      <template #body="{ data }">
        <span v-if="data.permissionNumber" class="mono">{{ data.permissionNumber }}</span>
        <span v-else class="muted">#{{ data.id }}</span>
      </template>
    </Column>
    <Column field="plateNumber" sortField="plate" sortable :header="t('vehiclePermissions.col.plate')" style="width:120px">
      <template #body="{ data }"><span class="mono">{{ data.plateNumber ?? '—' }}</span></template>
    </Column>
    <Column :header="t('vehiclePermissions.col.vehicle')">
      <template #body="{ data }">{{ data.vehicleDisplay ?? '—' }}</template>
    </Column>
    <Column :header="t('vehiclePermissions.col.owner')">
      <template #body="{ data }">{{ data.ownerName ?? '—' }}</template>
    </Column>
    <Column :header="t('vehiclePermissions.col.authorized')">
      <template #body="{ data }">{{ data.authorizedName ?? '—' }}</template>
    </Column>
    <Column sortField="issued" sortable :header="t('vehiclePermissions.col.issued')" style="width:105px">
      <template #body="{ data }">{{ fmtDate(data.issuedDate) }}</template>
    </Column>
    <Column sortField="expires" sortable :header="t('vehiclePermissions.col.expires')" style="width:105px">
      <template #body="{ data }">{{ fmtDate(data.validTillDate) }}</template>
    </Column>
    <Column style="width:76px" bodyStyle="text-align:right">
      <template #body="{ data }">
        <Button icon="pi pi-print" text rounded size="small" severity="secondary"
                v-tooltip.left="t('vehiclePermissions.printCertificate')"
                @click.stop="openCertificate(data.id)" />
        <Button icon="pi pi-file" text rounded size="small" severity="secondary"
                v-tooltip.left="t('vehiclePermissions.printRequest')"
                @click.stop="openRequest(data.id)" />
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
