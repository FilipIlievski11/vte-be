<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import { useShortcuts } from '@/composables/useShortcuts'
import { useUrlState } from '@/composables/useUrlState'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { formatShortDate } from '@/utils/format'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Skeleton from 'primevue/skeleton'
import InputText from 'primevue/inputtext'
import DatePicker from 'primevue/datepicker'
import PagedTableEmpty from '@/components/PagedTableEmpty.vue'

interface ExamReport {
  id: number; regNumber: string | null; madeDate: string; validTillDate: string; vehicleIsRight: boolean
  ownerName: string | null; vehicleShellNumber: string | null
  vehicleMaker: string | null; vehicleModel: string | null; examTypeName: string | null
}
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const router = useRouter()
const route   = useRoute()
const reports = ref<ExamReport[]>([])
const loading = ref(true)
const total   = ref(0)
const selected = ref<ExamReport | null>(null)
const toast = useToast(); const { ask } = useConfirmDialog()

// Listen to URL query state (page, sort, filters)
const state = useUrlState(
  { page: 1, pageSize: 50, sortBy: '', sortDir: 'desc' as 'asc' | 'desc',
    q: '', from: '', to: '' },
  { onChange: () => load() },
)

// `status` is decided by the route — /exams/approved | /exams/failed | /exams
const variant = computed<'approved' | 'failed' | 'all'>(() => {
  const p = route.path
  if (p.endsWith('/approved')) return 'approved'
  if (p.endsWith('/failed'))   return 'failed'
  return 'all'
})
const titleKey = computed(() =>
  variant.value === 'approved' ? 'exams.listApproved' :
  variant.value === 'failed'   ? 'exams.listFailed'   : 'menu.technicalExams')

// Local Date refs that mirror the URL strings
const fromDate = ref<Date | null>(state.from.value ? new Date(state.from.value) : null)
const toDate   = ref<Date | null>(state.to.value   ? new Date(state.to.value)   : null)

async function load() {
  loading.value = true
  try {
    const params: any = {
      page: state.page.value, pageSize: state.pageSize.value,
      sortBy: state.sortBy.value || undefined,
      sortDir: state.sortBy.value ? state.sortDir.value : undefined,
      q: state.q.value || undefined,
      from: state.from.value || undefined,
      to:   state.to.value   || undefined,
    }
    if (variant.value === 'approved') params.vehicleIsRight = true
    if (variant.value === 'failed')   params.vehicleIsRight = false
    const { data } = await api.get<Paged<ExamReport>>('/technical-exam-reports', { params })
    reports.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function applyDateFilter() {
  state.from.value = fromDate.value ? fromDate.value.toISOString().slice(0,10) : ''
  state.to.value   = toDate.value   ? toDate.value.toISOString().slice(0,10)   : ''
  state.page.value = 1
}

function onPage(ev: any) { state.page.value = ev.page + 1; state.pageSize.value = ev.rows }
function onSort(ev: any) { state.sortBy.value = ev.sortField || ''; state.sortDir.value = ev.sortOrder === 1 ? 'asc' : 'desc'; state.page.value = 1 }

function downloadCertificate(id: number) { openAuthorizedPdf(`/reports/technical-exam-reports/${id}/pdf`) }
function selectRow() { if (selected.value) router.push(`/exams/${selected.value.id}`) }
function deleteSelected() {
  if (!selected.value) return
  ask({
    message: `Дали сте сигурни дека сакате да го избришете записникот ${selected.value.regNumber ?? selected.value.id}?`,
    danger: true, acceptLabel: t('common.delete'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try {
        await api.delete(`/technical-exam-reports/${selected.value!.id}`)
        toast.ok(t('common.saved')); selected.value = null; await load()
      } catch (e: any) { toast.error(e?.response?.data?.error ?? 'Failed') }
    },
  })
}

useShortcuts({
  'F5':     () => load(),
  'Ctrl+N': () => router.push('/exams/new'),
  'Escape': () => router.back(),
  'Delete': () => deleteSelected(),
})

// Vue Router reuses the same component for /exams, /exams/approved, /exams/failed.
// Re-fetch when the active variant changes; reset to page 1.
watch(variant, () => { state.page.value = 1; load() })

onMounted(load)
const skeletonRows = Array.from({ length: 8 })
</script>

<template>
  <div class="exam-list">
    <div class="page-header">
      <div>
        <h1>{{ t(titleKey) }}</h1>
        <div class="subtitle">{{ total.toLocaleString('mk-MK') }} {{ t('common.records') }}</div>
      </div>
      <div class="actions">
        <Button :label="t('exam.new')" icon="pi pi-plus" @click="router.push('/exams/new')" v-tooltip.bottom="'Ctrl+N'" />
      </div>
    </div>

    <!-- Date-range filter (Од / До / Прикажи) — matches legacy header bar -->
    <section class="range-bar">
      <div class="range-title">{{ t('exams.dateRange') }}</div>
      <div class="range-fields">
        <label>{{ t('exams.from') }}</label>
        <DatePicker v-model="fromDate" dateFormat="d.m.yy" showButtonBar showIcon />
        <label>{{ t('exams.to') }}</label>
        <DatePicker v-model="toDate" dateFormat="d.m.yy" showButtonBar showIcon />
        <Button :label="t('exams.show')" icon="pi pi-search" @click="applyDateFilter" />
      </div>
    </section>

    <!-- Free-text filter row (the "Drag column header here" hint + filter input) -->
    <div class="grid-hint">
      <span>{{ t('control.dragHeaderHint') }}</span>
      <span class="search-inline">
        <i class="pi pi-filter" />
        <InputText v-model="state.q.value" :placeholder="t('common.search')" @keydown.enter="state.page.value = 1" />
      </span>
    </div>

    <DataTable v-if="loading && reports.length === 0" :value="skeletonRows" stripedRows>
      <Column :header="t('exams.recordNumber')"><template #body><Skeleton /></template></Column>
      <Column :header="t('exams.owner')"      ><template #body><Skeleton /></template></Column>
      <Column :header="t('exams.regNumber')"  ><template #body><Skeleton /></template></Column>
      <Column :header="t('exams.shellNumber')"><template #body><Skeleton /></template></Column>
      <Column :header="t('exams.madeDate')"   ><template #body><Skeleton /></template></Column>
      <Column :header="t('exams.validTill')"  ><template #body><Skeleton /></template></Column>
    </DataTable>

    <DataTable v-else :value="reports" :loading="loading" stripedRows lazy paginator
               :first="(state.page.value - 1) * state.pageSize.value" :rows="state.pageSize.value" :totalRecords="total"
               :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" @sort="onSort"
               :sortField="state.sortBy.value || undefined" :sortOrder="state.sortDir.value === 'desc' ? -1 : 1"
               selectionMode="single" v-model:selection="selected"
               rowHover class="legacy-grid"
               @row-click="(e: any) => router.push(`/exams/${e.data.id}`)"
               @row-dblclick="(e: any) => router.push(`/exams/${e.data.id}`)">
      <template #empty>
        <PagedTableEmpty icon="pi-check-square"
          :title="state.q.value ? 'Нема резултати' : 'Нема технички прегледи'"
          hint="Започнете нов запис за технички преглед."
          :ctaLabel="t('exam.new')"
          @cta="router.push('/exams/new')" />
      </template>
      <Column field="regNumber" :header="t('exams.recordNumber')" sortable style="width:160px">
        <template #body="{ data }">
          <a class="link" @click.stop="router.push(`/exams/${data.id}`)">{{ data.regNumber ?? `#${data.id}` }}</a>
        </template>
      </Column>
      <Column field="ownerName" :header="t('exams.owner')" sortable>
        <template #body="{ data }"><a class="link">{{ data.ownerName ?? '—' }}</a></template>
      </Column>
      <Column field="regNumber" :header="t('exams.regNumber')" sortable style="width:140px">
        <template #body="{ data }"><a class="link">{{ data.regNumber ?? '—' }}</a></template>
      </Column>
      <Column field="vehicleShellNumber" :header="t('exams.shellNumber')" sortable style="width:200px">
        <template #body="{ data }"><a class="link">{{ data.vehicleShellNumber ?? '—' }}</a></template>
      </Column>
      <Column field="madeDate"      :header="t('exams.madeDate')" sortable style="width:110px">
        <template #body="{ data }">{{ formatShortDate(data.madeDate) }}</template>
      </Column>
      <Column field="validTillDate" :header="t('exams.validTill')" sortable style="width:110px">
        <template #body="{ data }">{{ formatShortDate(data.validTillDate) }}</template>
      </Column>
      <Column :header="t('common.actions')" style="width:60px">
        <template #body="{ data }">
          <Button icon="pi pi-file-pdf" text rounded @click.stop="downloadCertificate(data.id)" v-tooltip.left="t('exams.certificate')" />
        </template>
      </Column>
    </DataTable>

    <!-- Bottom action bar — Изberi / Избриши / Излези matches legacy footer -->
    <footer class="action-bar">
      <Button :label="t('exam.select')" icon="pi pi-check" outlined :disabled="!selected" @click="selectRow" />
      <Button :label="t('common.delete')" icon="pi pi-trash" severity="danger" outlined :disabled="!selected" @click="deleteSelected" />
      <Button :label="t('exam.exit')" icon="pi pi-sign-out" severity="secondary" outlined @click="router.back()" />
    </footer>
  </div>
</template>

<style scoped>
.exam-list { display: flex; flex-direction: column; gap: 12px; }

.range-bar {
  background: var(--color-surface, #fff);
  border: 1px solid var(--color-border, #e2e8f0); border-radius: 8px;
  padding: 12px 16px; display: flex; flex-direction: column; gap: 6px;
}
.range-bar .range-title { font-size: 11px; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-color-secondary); }
.range-bar .range-fields { display: flex; align-items: center; gap: 10px; flex-wrap: wrap; }
.range-bar label { font-size: 12px; color: var(--text-color-secondary); }

.grid-hint {
  display: flex; align-items: center; justify-content: space-between; gap: 12px;
  padding: 6px 12px; font-size: 12px; color: var(--text-color-secondary);
  background: var(--surface-100, #f1f5f9); border: 1px solid var(--color-border); border-radius: 6px 6px 0 0;
  font-style: italic;
}
.grid-hint .search-inline { position: relative; display: inline-flex; align-items: center; font-style: normal; }
.grid-hint .search-inline i { position: absolute; left: 8px; color: var(--text-color-secondary); pointer-events: none; }
.grid-hint .search-inline :deep(input) { padding-left: 28px; min-width: 220px; }

.legacy-grid :deep(.p-datatable-tbody tr) { cursor: pointer; }
.legacy-grid :deep(.p-datatable-tbody tr.p-highlight) { background: rgba(59, 130, 246, 0.18); }
.link {
  color: var(--color-brand-600, #1d4ed8); text-decoration: underline; cursor: pointer;
}
.link:hover { color: var(--color-brand-500, #2563eb); }

.action-bar {
  display: flex; gap: 8px; padding: 10px 12px;
  background: var(--surface-100, #f1f5f9); border: 1px solid var(--color-border); border-radius: 6px;
}
</style>
