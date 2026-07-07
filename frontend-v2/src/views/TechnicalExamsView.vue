<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { openPrintTab } from '@/utils/print';
import type { Paged, TechExamListItem, TechExamType } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import SelectButton from 'primevue/selectbutton';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import Skeleton from 'primevue/skeleton';
import { useToast } from 'primevue/usetoast';
import PagedTableEmpty from '@/components/PagedTableEmpty.vue';

const { t } = useI18n();
const router = useRouter();
const toast = useToast();

type Result = 'all' | 'pass' | 'fail';

const items = ref<TechExamListItem[]>([]);
const total = ref(0);
const loading = ref(true);

const q = ref('');
const result = ref<Result>('all');
const typeFilter = ref<number | null>(null);
const types = ref<TechExamType[]>([]);
const page = ref(1);
const pageSize = ref(50);
const sortField = ref<string>('made');
const sortOrder = ref<number>(-1);

const resultOptions = computed(() => [
  { value: 'all' as Result,  label: t('techExam.tabs.all') },
  { value: 'pass' as Result, label: t('techExam.tabs.pass') },
  { value: 'fail' as Result, label: t('techExam.tabs.fail') },
]);

const typeOptions = computed(() => [
  { id: null as number | null, label: t('techExam.allTypes') },
  ...types.value.map(ty => ({ id: ty.id, label: ty.code || ty.description })),
]);

let searchTimer: number | undefined;
let loadToken = 0;

async function loadTypes() {
  try { types.value = (await api.get<TechExamType[]>('/technical-exams/types')).data; }
  catch { /* ignore */ }
}

async function load() {
  const myToken = ++loadToken;
  loading.value = true;
  try {
    const { data } = await api.get<Paged<TechExamListItem>>('/technical-exams', {
      params: {
        q: q.value || undefined,
        result: result.value,
        typeId: typeFilter.value ?? undefined,
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
    toast.add({ severity: 'error', summary: t('techExam.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    if (myToken === loadToken) loading.value = false;
  }
}

onMounted(async () => { await loadTypes(); await load(); });

watch(q, () => {
  window.clearTimeout(searchTimer);
  searchTimer = window.setTimeout(() => { page.value = 1; load(); }, 300);
});
watch(result, () => { page.value = 1; load(); });
watch(typeFilter, () => { page.value = 1; load(); });

function onPage(ev: { page: number; rows: number }) {
  page.value = ev.page + 1;
  pageSize.value = ev.rows;
  load();
}
function onSort(ev: any) {
  sortField.value = (typeof ev?.sortField === 'string' && ev.sortField) ? ev.sortField : 'made';
  sortOrder.value = ev?.sortOrder === 1 ? 1 : -1;
  page.value = 1;
  load();
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}

// Печат од редот — записник секогаш; потврда само за исправно возило (како во деталите).
function printZapisnik(r: TechExamListItem) {
  openPrintTab(router.resolve({ name: 'technical-exam-zapisnik', params: { id: r.id } }).href);
}
function printCertificate(r: TechExamListItem) {
  if (!r.vehicleIsRight) return;
  openPrintTab(router.resolve({ name: 'technical-exam-print', params: { id: r.id } }).href);
}

const skeletonRows = Array.from({ length: 8 });
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('techExam.title') }}</h1>
      <div class="subtitle">{{ t('techExam.records', { count: total }) }}</div>
    </div>
    <div class="actions">
      <SelectButton
        v-model="result"
        :options="resultOptions"
        optionLabel="label" optionValue="value"
        :allowEmpty="false" size="small" class="status-tabs"
      />
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="q" :placeholder="t('common.search')" size="small" />
      </span>
      <Select
        v-model="typeFilter"
        :options="typeOptions"
        optionLabel="label" optionValue="id"
        :placeholder="t('techExam.allTypes')"
        size="small" class="filter" showClear
      />
      <Button :label="t('techExam.new')" icon="pi pi-plus" size="small" @click="router.push('/technical-exams/new')" />
    </div>
  </div>

  <DataTable
    v-if="loading && items.length === 0"
    :value="skeletonRows" stripedRows size="small" class="tight-table"
  >
    <Column :header="t('techExam.col.regNumber')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.client')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.vehicle')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.type')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.made')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.result')"><template #body><Skeleton /></template></Column>
    <Column :header="t('techExam.col.actions')"><template #body><Skeleton /></template></Column>
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
    @row-click="(e: any) => router.push(`/technical-exams/${e.data.id}`)"
    :pt="{ row: { style: 'cursor: pointer' } }"
    dataKey="id"
  >
    <template #empty>
      <PagedTableEmpty
        icon="pi-clipboard"
        :title="t('techExam.title')"
        :hint="q ? t('empty.noResultsFor', { q }) : t('empty.noRows')"
      />
    </template>

    <Column field="regNumber" sortField="regnumber" sortable :header="t('techExam.col.regNumber')" style="width:150px">
      <template #body="{ data }"><span class="mono">{{ data.regNumber || '—' }}</span></template>
    </Column>
    <Column :header="t('techExam.col.client')">
      <template #body="{ data }">
        <span v-if="data.clientName">{{ data.clientName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column :header="t('techExam.col.vehicle')">
      <template #body="{ data }">
        <div v-if="data.vehiclePlate || data.vehicleVin">
          <span v-if="data.vehiclePlate" class="plate">{{ data.vehiclePlate }}</span>
          <span v-if="data.vehicleVin" class="muted small">{{ data.vehicleVin }}</span>
        </div>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="type" sortable :header="t('techExam.col.type')" style="width:130px">
      <template #body="{ data }">
        <span v-if="data.typeCode || data.typeName" class="type-code">{{ data.typeCode || data.typeName }}</span>
        <span v-else class="muted">—</span>
      </template>
    </Column>
    <Column sortField="made" sortable :header="t('techExam.col.made')" style="width:110px">
      <template #body="{ data }">{{ fmtDate(data.madeDate) }}</template>
    </Column>
    <Column sortField="validtill" sortable :header="t('techExam.col.validTill')" style="width:110px">
      <template #body="{ data }">{{ fmtDate(data.validTillDate) }}</template>
    </Column>
    <Column sortField="result" sortable :header="t('techExam.col.result')" style="width:110px">
      <template #body="{ data }">
        <Tag
          :value="data.vehicleIsRight ? t('techExam.pass') : t('techExam.fail')"
          :severity="data.vehicleIsRight ? 'success' : 'danger'"
        />
      </template>
    </Column>
    <Column :header="t('techExam.col.actions')" style="width:76px" bodyStyle="text-align:right">
      <template #body="{ data }">
        <Button icon="pi pi-file" text rounded size="small" severity="secondary"
                v-tooltip.left="t('techExam.printZapisnik')"
                @click.stop="printZapisnik(data)" />
        <Button icon="pi pi-print" text rounded size="small" severity="secondary"
                :disabled="!data.vehicleIsRight"
                v-tooltip.left="data.vehicleIsRight ? t('techExam.printCertificate') : t('techExam.printDisabledHint')"
                @click.stop="printCertificate(data)" />
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
.filter { min-width: 170px; }
.filter :deep(.p-select-label) { font-size: 0.8125rem; }
.status-tabs :deep(.p-togglebutton) { font-size: 0.8125rem; padding: .3rem .7rem }
.mono { font-family: monospace; font-weight: 600; }
.plate { display: inline-block; font-family: monospace; font-weight: 600; padding-right: .5rem }
.type-code { font-family: monospace; font-size: .8rem; }
.small { font-size: .7rem }
.muted { color: var(--color-text-muted); }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) {
  padding: 0.2rem 0.5rem;
  font-size: 0.8125rem;
  line-height: 1.2;
}
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
</style>
