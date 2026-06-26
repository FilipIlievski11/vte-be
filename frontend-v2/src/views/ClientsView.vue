<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Client, Company, Paged } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import Checkbox from 'primevue/checkbox';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

const items = ref<Client[]>([]);
const total = ref(0);
const loading = ref(true);
const companies = ref<Company[]>([]);

const q = ref('');
const business = ref<'all' | 'true' | 'false'>('all');
const companyFilter = ref<number | null>(null);
const page = ref(1);
const pageSize = ref(50);
const sortBy = ref<string>('');
const sortDir = ref<'asc' | 'desc'>('desc');

const searchInput = ref<HTMLInputElement | null>(null);
let searchTimer: number | undefined;

const businessOptions = computed(() => [
  { label: t('clients.filterAll'),      value: 'all' },
  { label: t('clients.filterLegal'),    value: 'true' },
  { label: t('clients.filterPhysical'), value: 'false' },
]);

const companyOptions = computed(() => [
  { id: null as number | null, name: t('clients.allCompanies') },
  ...companies.value,
]);

async function loadCompanies() {
  if (!auth.isAdmin) return;
  try {
    const { data } = await api.get<Company[]>('/companies');
    companies.value = data;
  } catch { /* ignore */ }
}

async function load() {
  loading.value = true;
  try {
    const { data } = await api.get<Paged<Client>>('/clients', {
      params: {
        q: q.value || undefined,
        business: business.value === 'all' ? undefined : business.value,
        companyId: companyFilter.value ?? undefined,
        page: page.value,
        pageSize: pageSize.value,
        sortBy: sortBy.value || undefined,
        sortDir: sortBy.value ? sortDir.value : undefined,
      },
    });
    items.value = data.items;
    total.value = data.total;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('clients.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

onMounted(async () => {
  await loadCompanies();
  await load();
});

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});
watch([business, companyFilter], () => { page.value = 1; load(); });

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

function add() { router.push('/clients/new'); }
function edit(c: Client) { router.push(`/clients/${c.id}`); }
function remove(c: Client) {
  confirm.require({
    message: t('clients.deleteConfirm', { id: c.id }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/clients/${c.id}`);
        toast.add({ severity: 'success', summary: t('clients.deleted'), life: 2000 });
        load();
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('clients.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

function displayName(c: Client) {
  return [c.firstName, c.middleName, c.lastName].filter(Boolean).join(' ') || '—';
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('clients.title') }}</h1>
      <div class="subtitle">{{ t('clients.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText ref="searchInput" v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Select
        v-model="business"
        :options="businessOptions"
        optionLabel="label"
        optionValue="value"
        size="small"
        class="filter"
      />
      <Select
        v-if="auth.isAdmin"
        v-model="companyFilter"
        :options="companyOptions"
        optionLabel="name"
        optionValue="id"
        :placeholder="t('clients.allCompanies')"
        size="small"
        class="filter"
        showClear
      />
      <Button
        :label="t('clients.new')"
        icon="pi pi-plus"
        size="small"
        severity="success"
        @click="add"
      />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows"
    stripedRows size="small"
    class="tight-table"
  >
    <Column :header="t('clients.col.customer')"><template #body><Skeleton /></template></Column>
    <Column :header="t('clients.col.embg')"><template #body><Skeleton /></template></Column>
    <Column :header="t('clients.col.address')"><template #body><Skeleton /></template></Column>
    <Column :header="t('clients.col.business')" style="width:110px"><template #body><Skeleton /></template></Column>
    <Column :header="t('clients.col.actions')" style="width:100px"><template #body><Skeleton /></template></Column>
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
    @row-click="(e: any) => router.push(`/clients/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-users"
        :title="q ? t('empty.noResults') : t('empty.noClients')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.addToStart')"
        :ctaLabel="t('clients.new')"
        @cta="add"
      />
    </template>

    <Column field="customer" :header="t('clients.col.customer')" sortable>
      <template #body="{ data }">{{ displayName(data) }}</template>
    </Column>
    <Column field="mb" :header="t('clients.col.embg')" sortable />
    <Column :header="t('clients.col.address')">
      <template #body="{ data }">{{ data.address || '—' }}</template>
    </Column>
    <Column :header="t('clients.col.business')" style="width: 110px" bodyStyle="text-align: center">
      <template #body="{ data }">
        <Checkbox :modelValue="data.business" :binary="true" disabled />
      </template>
    </Column>
    <Column :header="t('clients.col.actions')" style="width: 100px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="edit(data)" v-tooltip.left="t('common.edit')" />
        <Button
          v-if="auth.isAdmin"
          icon="pi pi-trash" text severity="danger"
          @click.stop="remove(data)"
          v-tooltip.left="t('common.delete')"
        />
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

.filter { min-width: 150px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-checkbox) { transform: scale(0.85); }
.tight-table :deep(.p-button.p-button-icon-only) { width: 1.75rem; height: 1.75rem; }
</style>
