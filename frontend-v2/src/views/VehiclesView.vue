<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Company, Paged, VehicleListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

const items = ref<VehicleListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const q = ref('');
const companyFilter = ref<number | null>(null);   // admin only; null = all companies
const companies = ref<Company[]>([]);
const page = ref(1);
const pageSize = ref(50);
const sortBy = ref<string>('');
const sortDir = ref<'asc' | 'desc'>('desc');

const companyOptions = computed(() => [
  { id: null as number | null, name: t('vehicles.allCompanies') },
  ...companies.value,
]);

const searchInput = ref<HTMLInputElement | null>(null);
let searchTimer: number | undefined;
let loadToken = 0;   // monotonic counter — only the latest in-flight load gets to mutate state

async function loadCompanies() {
  if (!auth.isAdmin) return;
  try { companies.value = (await api.get<Company[]>('/companies')).data; }
  catch { /* ignore — admin endpoint might be unreachable */ }
}

async function load() {
  const myToken = ++loadToken;
  loading.value = true;
  try {
    const { data } = await api.get<Paged<VehicleListItem>>('/vehicles', {
      params: {
        q: q.value || undefined,
        companyId: companyFilter.value ?? undefined,
        page: page.value,
        pageSize: pageSize.value,
        sortBy: sortBy.value || undefined,
        sortDir: sortBy.value ? sortDir.value : undefined,
      },
    });
    // Stale-response guard: a newer load() has already been started, drop this result.
    if (myToken !== loadToken) return;
    items.value = data.items;
    total.value = data.total;
  } catch (e: any) {
    if (myToken !== loadToken) return;  // stale error too
    toast.add({ severity: 'error', summary: t('vehicles.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    if (myToken === loadToken) loading.value = false;
  }
}

onMounted(async () => { await loadCompanies(); await load(); });

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});
watch(companyFilter, () => { page.value = 1; load(); });

function onPage(ev: { page: number; rows: number }) {
  page.value = ev.page + 1;
  pageSize.value = ev.rows;
  load();
}
function onSort(ev: any) {
  sortBy.value = typeof ev?.sortField === 'string' ? ev.sortField : '';
  sortDir.value = ev?.sortOrder === 1 ? 'asc' : 'desc';
  page.value = 1;
  load();
}

function add() { router.push('/vehicles/new'); }
function edit(v: VehicleListItem) { router.push(`/vehicles/${v.id}`); }

function remove(v: VehicleListItem) {
  confirm.require({
    message: t('vehicles.deleteConfirm', { id: v.id }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/vehicles/${v.id}`);
        toast.add({ severity: 'success', summary: t('vehicles.deleted'), life: 2000 });
        load();
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('vehicles.title') }}</h1>
      <div class="subtitle">{{ t('vehicles.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText ref="searchInput" v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Select
        v-if="auth.isAdmin"
        v-model="companyFilter"
        :options="companyOptions"
        optionLabel="name" optionValue="id"
        :placeholder="t('vehicles.allCompanies')"
        size="small" class="filter" showClear
      />
      <Button :label="t('vehicles.new')" icon="pi pi-plus" size="small" severity="success" @click="add" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small"
    class="tight-table"
  >
    <Column :header="t('vehicles.col.vin')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.col.owner')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.col.plate')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.col.embg')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.col.model')"><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.col.maker')"><template #body><Skeleton /></template></Column>
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
    @sort="onSort"
    :sortField="sortBy || undefined"
    :sortOrder="sortDir === 'desc' ? -1 : 1"
    rowHover
    @row-click="(e: any) => router.push(`/vehicles/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="relationId"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-car"
        :title="t('vehicles.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
        :ctaLabel="t('vehicles.new')"
        @cta="add"
      />
    </template>

    <Column field="vin" :header="t('vehicles.col.vin')" sortable />
    <Column field="ownerName" :header="t('vehicles.col.owner')">
      <template #body="{ data }">
        <span v-if="data.ownerName">{{ data.ownerName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column field="plate" :header="t('vehicles.col.plate')" sortable>
      <template #body="{ data }">{{ data.plate || '—' }}</template>
    </Column>
    <Column field="ownerEmbg" :header="t('vehicles.col.embg')">
      <template #body="{ data }">
        <span v-if="data.ownerEmbg">{{ data.ownerEmbg }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column field="model" :header="t('vehicles.col.model')">
      <template #body="{ data }">{{ data.model || '—' }}</template>
    </Column>
    <Column field="maker" :header="t('vehicles.col.maker')">
      <template #body="{ data }">{{ data.maker || '—' }}</template>
    </Column>
    <Column :header="t('vehicles.col.actions')" style="width: 90px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="edit(data)" v-tooltip.left="t('common.edit')" />
        <Button v-if="auth.isAdmin" icon="pi pi-trash" text severity="danger" @click.stop="remove(data)" v-tooltip.left="t('common.delete')" />
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
.filter { min-width: 200px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-button.p-button-icon-only) { width: 1.75rem; height: 1.75rem; }
</style>
