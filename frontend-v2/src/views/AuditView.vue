<script setup lang="ts">
import { onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { AuditRow, Paged } from '@/types';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import Select from 'primevue/select';
import InputText from 'primevue/inputtext';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Paginator from 'primevue/paginator';

const { t } = useI18n();

const rows = ref<AuditRow[]>([]);
const total = ref(0);
const page = ref(1);
const pageSize = 50;
const loading = ref(false);
const actionFilter = ref<string>('all');
const q = ref('');

const ACTION_OPTIONS = [
  { value: 'all',         labelKey: 'audit.filter.all' },
  { value: 'bill.create',           labelKey: 'audit.action.billCreate' },
  { value: 'bill.storno',           labelKey: 'audit.action.billStorno' },
  { value: 'bill.paid',             labelKey: 'audit.action.billPaid' },
  { value: 'bill.line-price',       labelKey: 'audit.action.billLinePrice' },
  { value: 'bill.installment-paid', labelKey: 'audit.action.billInstallment' },
  { value: 'debt.create',           labelKey: 'audit.action.debtCreate' },
  { value: 'debt.price',            labelKey: 'audit.action.debtPrice' },
  { value: 'debt.delete',           labelKey: 'audit.action.debtDelete' },
];

async function load() {
  loading.value = true;
  try {
    const params: Record<string, unknown> = { page: page.value, pageSize };
    if (actionFilter.value !== 'all') params.action = actionFilter.value;
    if (q.value.trim()) params.q = q.value.trim();
    const { data } = await api.get<Paged<AuditRow>>('/admin/audit', { params });
    rows.value = data.items;
    total.value = data.total;
  } finally {
    loading.value = false;
  }
}
onMounted(load);
watch(actionFilter, () => { page.value = 1; load(); });

let qTimer: ReturnType<typeof setTimeout> | undefined;
watch(q, () => { clearTimeout(qTimer); qTimer = setTimeout(() => { page.value = 1; load(); }, 350); });

function onPage(e: { page: number }) { page.value = e.page + 1; load(); }

function fmtWhen(s: string): string {
  const d = new Date(s.endsWith('Z') ? s : s + 'Z');
  return isNaN(d.getTime()) ? '—' : d.toLocaleString();
}

function actionSeverity(a: string): 'danger' | 'warn' | 'success' | 'info' | 'secondary' {
  if (a === 'bill.storno' || a.startsWith('debt.delete')) return 'danger';
  if (a.endsWith('price')) return 'warn';
  if (a === 'bill.create' || a === 'bill.installment-paid') return 'success';
  if (a === 'bill.paid') return 'info';
  return 'secondary';
}

function actionLabel(a: string): string {
  const hit = ACTION_OPTIONS.find(o => o.value === a);
  if (hit) return t(hit.labelKey);
  if (a === 'debt.delete-batch') return t('audit.action.debtDeleteBatch');
  return a;
}

/** Bill actions link to the bill page. */
function entityLink(r: AuditRow): string | null {
  return r.entityType === 'PaymentDocument' ? `/payments/${r.entityId}` : null;
}
</script>

<template>
  <div class="audit-view">
    <header class="page-head">
      <div>
        <h1>{{ t('audit.title') }}</h1>
        <p class="sub">{{ t('audit.subtitle') }}</p>
      </div>
      <Button icon="pi pi-refresh" text rounded :loading="loading" @click="load"
              v-tooltip.bottom="t('common.search')" />
    </header>

    <div class="controls">
      <Select v-model="actionFilter" :options="ACTION_OPTIONS" optionValue="value"
              :optionLabel="(o: any) => t(o.labelKey)" class="ctl-action" size="small" />
      <InputText v-model="q" :placeholder="t('audit.searchPlaceholder')" class="ctl-q" size="small" />
      <span class="muted count">{{ t('audit.records', { count: total }) }}</span>
    </div>

    <DataTable :value="rows" :loading="loading" stripedRows size="small" class="tight-table" dataKey="id">
      <Column :header="t('audit.col.when')" style="width:150px">
        <template #body="{ data }"><span class="mono small">{{ fmtWhen(data.atUtc) }}</span></template>
      </Column>
      <Column :header="t('audit.col.user')" style="width:110px">
        <template #body="{ data }">{{ data.userName || '—' }}</template>
      </Column>
      <Column :header="t('audit.col.action')" style="width:150px">
        <template #body="{ data }">
          <Tag :value="actionLabel(data.action)" :severity="actionSeverity(data.action)" class="action-tag" />
        </template>
      </Column>
      <Column :header="t('audit.col.what')">
        <template #body="{ data }">
          <router-link v-if="entityLink(data)" :to="entityLink(data)!" class="sum-link">{{ data.summary }}</router-link>
          <span v-else>{{ data.summary }}</span>
        </template>
      </Column>
      <template #empty>
        <div class="muted pad">{{ t('audit.empty') }}</div>
      </template>
    </DataTable>

    <Paginator v-if="total > pageSize" :rows="pageSize" :totalRecords="total"
               :first="(page - 1) * pageSize" @page="onPage" />
  </div>
</template>

<style scoped>
.audit-view { max-width: 1100px; margin: 0 auto; padding: 1.25rem; display: flex; flex-direction: column; gap: 1rem; }
.page-head { display: flex; align-items: flex-start; justify-content: space-between; }
.page-head h1 { margin: 0; font-size: 1.3rem; }
.page-head .sub { margin: .25rem 0 0; color: var(--text-muted, #6b7280); }
.controls { display: flex; align-items: center; gap: .6rem; flex-wrap: wrap; }
.ctl-action { min-width: 15rem; }
.ctl-q { width: 20rem; }
.count { margin-left: auto; font-size: .85rem; }
.muted { color: var(--text-muted, #6b7280); }
.small { font-size: .78rem; }
.mono { font-family: monospace; }
.pad { padding: 1rem; }
.action-tag :deep(.p-tag-label), .action-tag { font-size: .72rem; }
.sum-link { color: inherit; text-decoration: none; border-bottom: 1px dashed var(--p-surface-400); }
.sum-link:hover { color: var(--p-primary-600); border-bottom-color: var(--p-primary-400); }
.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) { padding: .3rem .5rem; font-size: .8125rem; }
</style>
