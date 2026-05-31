<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue'
import { useI18n } from 'vue-i18n'
import { useRouter } from 'vue-router'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import InputText from 'primevue/inputtext'
import DatePicker from 'primevue/datepicker'
import Select from 'primevue/select'
import { usePaymentTypes } from '@/composables/useReferenceData'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { formatMoney as fmtMoney, formatShortDate as fmtShortDate } from '@/utils/format'
import { useShortcuts } from '@/composables/useShortcuts'
import { useUrlState } from '@/composables/useUrlState'
import Skeleton from 'primevue/skeleton'
import PagedTableEmpty from '@/components/PagedTableEmpty.vue'

interface PaymentDocumentRow {
  id: number; documentNumber: string; datePay: string; dateRequired: string
  totalAmount: number; totalInstallments: number
  payed: boolean; storno: boolean
  paymentTypeId: number | null; paymentTypeName: string | null
  customerVehicleRelationId: number; customerName: string | null; vehicleReg: string | null
}
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }
interface Summary { total: number; totalPaid: number; totalUnpaid: number; totalStorno: number }

const { t } = useI18n()
const router = useRouter()
const paymentTypes = usePaymentTypes()

const docs = ref<PaymentDocumentRow[]>([])
const summary = ref<Summary>({ total: 0, totalPaid: 0, totalUnpaid: 0, totalStorno: 0 })
const loading = ref(true)
const total = ref(0)
const skeletonRows = Array.from({ length: 8 })

// URL-synced state
type StatusFilter = 'all' | 'unpaid' | 'paid' | 'storno'
const state = useUrlState(
  { page: 1, pageSize: 50, sortBy: '', sortDir: 'desc' as 'asc' | 'desc',
    q: '', status: 'all' as StatusFilter, from: '', to: '', paymentTypeId: 0 },
  { onChange: () => load() },
)
const search = state.q
const fromDate = ref<Date | null>(state.from.value ? new Date(state.from.value) : null)
const toDate   = ref<Date | null>(state.to.value   ? new Date(state.to.value)   : null)

const statusOptions = computed(() => [
  { value: 'all',    label: t('payments.filterAll') },
  { value: 'unpaid', label: t('payments.unpaid') },
  { value: 'paid',   label: t('payments.payed') },
  { value: 'storno', label: t('payments.storno') },
])

function buildParams() {
  const p: any = { page: state.page.value, pageSize: state.pageSize.value }
  if (state.sortBy.value) { p.sortBy = state.sortBy.value; p.sortDir = state.sortDir.value }
  if (state.q.value)            p.q = state.q.value
  if (state.from.value)         p.from = state.from.value
  if (state.to.value)           p.to   = state.to.value
  if (state.paymentTypeId.value) p.paymentTypeId = state.paymentTypeId.value
  if (state.status.value === 'unpaid') p.unpaid = true
  if (state.status.value === 'paid')   { p.unpaid = false; p.storno = false }
  if (state.status.value === 'storno') p.storno = true
  return p
}

async function load() {
  loading.value = true
  try {
    const [docsRes, sumRes] = await Promise.all([
      api.get<Paged<PaymentDocumentRow>>('/payment-documents', { params: buildParams() }),
      api.get<Summary>('/payment-documents/summary', {
        params: { from: state.from.value || undefined, to: state.to.value || undefined },
      }),
    ])
    docs.value = docsRes.data.items; total.value = docsRes.data.total
    summary.value = sumRes.data
  } finally { loading.value = false }
}

function onPage(ev: any) { state.page.value = ev.page + 1; state.pageSize.value = ev.rows }
function onSort(ev: any) { state.sortBy.value = ev.sortField || ''; state.sortDir.value = ev.sortOrder === 1 ? 'asc' : 'desc'; state.page.value = 1 }
function applyFilters() {
  state.from.value = fromDate.value ? fromDate.value.toISOString().slice(0,10) : ''
  state.to.value   = toDate.value   ? toDate.value.toISOString().slice(0,10)   : ''
  state.page.value = 1
}
function resetFilters() {
  fromDate.value = null; toDate.value = null
  state.q.value = ''; state.from.value = ''; state.to.value = ''
  state.paymentTypeId.value = 0; state.status.value = 'all'; state.page.value = 1
}

let searchTimer: any
watch(search, () => { clearTimeout(searchTimer); searchTimer = setTimeout(applyFilters, 300) })

useShortcuts({ 'F5': () => load() })

function downloadInvoice(id: number) { openAuthorizedPdf(`/reports/payment-documents/${id}/invoice`) }
function downloadCashReport() {
  const day = (fromDate.value ?? new Date()).toISOString().slice(0, 10)
  openAuthorizedPdf(`/reports/cash-report?day=${day}`)
}
const toast = useToast(); const { ask } = useConfirmDialog()

async function storno(id: number) {
  ask({
    message: t('payments.confirmStorno'), header: t('payments.storno'),
    danger: true, acceptLabel: t('payments.storno'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try { await api.post(`/payment-documents/${id}/storno`); toast.ok(t('common.saved')); await load() }
      catch (e: any) { toast.error(e?.response?.data?.error ?? 'Failed') }
    },
  })
}
const formatMoney = (n: number | null | undefined) => fmtMoney(n, '')
const formatShortDate = fmtShortDate

onMounted(load)
</script>

<template>
  <div class="payments-page">
    <header class="page-header">
      <div>
        <h1>{{ t('nav.payments') }}</h1>
        <div class="subtitle">{{ summary.total.toLocaleString('mk-MK') }} {{ t('common.records') }}</div>
      </div>
      <div class="actions">
        <Button :label="t('payments.cashReport')" icon="pi pi-file-pdf" severity="secondary" outlined @click="downloadCashReport" />
        <Button :label="t('common.add')" icon="pi pi-plus" @click="router.push('/payments/new')" />
      </div>
    </header>

    <!-- KPI strip — paid / unpaid / storno totals -->
    <section class="kpi-strip">
      <div class="kpi"><div class="kpi-label">{{ t('payments.kpi.paid') }}</div>
        <div class="kpi-value paid">{{ formatMoney(summary.totalPaid) }} ден.</div></div>
      <div class="kpi"><div class="kpi-label">{{ t('payments.kpi.unpaid') }}</div>
        <div class="kpi-value unpaid">{{ formatMoney(summary.totalUnpaid) }} ден.</div></div>
      <div class="kpi"><div class="kpi-label">{{ t('payments.kpi.storno') }}</div>
        <div class="kpi-value storno">{{ formatMoney(summary.totalStorno) }} ден.</div></div>
      <div class="kpi"><div class="kpi-label">{{ t('payments.kpi.netTotal') }}</div>
        <div class="kpi-value">{{ formatMoney(summary.totalPaid - summary.totalStorno) }} ден.</div></div>
    </section>

    <!-- Filter row -->
    <section class="filters">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="search" :placeholder="t('payments.searchPlaceholder')" />
      </span>
      <DatePicker v-model="fromDate" :placeholder="t('payments.from')" dateFormat="dd.mm.yy" showButtonBar @date-select="applyFilters" @clear-click="applyFilters" />
      <DatePicker v-model="toDate"   :placeholder="t('payments.to')"   dateFormat="dd.mm.yy" showButtonBar @date-select="applyFilters" @clear-click="applyFilters" />
      <Select v-model="state.paymentTypeId.value" :options="paymentTypes" optionLabel="name" optionValue="id"
              :placeholder="t('payments.allTypes')" showClear class="select-narrow" @change="applyFilters" />
      <Select v-model="state.status.value" :options="statusOptions" optionLabel="label" optionValue="value"
              class="select-narrow" @change="applyFilters" />
      <Button :label="t('common.cancel')" severity="secondary" outlined size="small" icon="pi pi-times" @click="resetFilters" />
    </section>

    <DataTable v-if="loading && docs.length === 0" :value="skeletonRows" stripedRows>
      <Column :header="t('payments.documentNumber')"><template #body><Skeleton /></template></Column>
      <Column :header="t('payments.datePay')"       ><template #body><Skeleton /></template></Column>
      <Column :header="t('payments.customer')"      ><template #body><Skeleton /></template></Column>
      <Column :header="t('payments.totalAmount')"   ><template #body><Skeleton /></template></Column>
      <Column :header="t('payments.status')"        ><template #body><Skeleton /></template></Column>
    </DataTable>

    <DataTable v-else :value="docs" :loading="loading" stripedRows lazy paginator
               :first="(state.page.value - 1) * state.pageSize.value" :rows="state.pageSize.value" :totalRecords="total"
               :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" @sort="onSort"
               :sortField="state.sortBy.value || undefined" :sortOrder="state.sortDir.value === 'desc' ? -1 : 1"
               rowHover class="payments-table" :rowClass="(row: PaymentDocumentRow) => row.storno ? 'row-storno' : ''"
               @row-click="(e: any) => router.push(`/payments/${e.data.id}`)" :pt="{ row: { style: 'cursor: pointer' } }">
      <template #empty>
        <PagedTableEmpty icon="pi-credit-card"
          :title="state.q.value ? 'Нема резултати' : 'Нема плаќања'"
          :hint="state.q.value ? `Не е најден ниту еден документ за \&quot;${state.q.value}\&quot;.` : 'Започнете нов налог за плаќање.'" />
      </template>
      <Column field="documentNumber" :header="t('payments.documentNumber')" sortable style="width:180px" />
      <Column field="datePay" :header="t('payments.datePay')" sortable style="width:110px">
        <template #body="{ data }">{{ formatShortDate(data.datePay) }}</template>
      </Column>
      <Column field="customerName" :header="t('payments.customer')" sortable />
      <Column field="vehicleReg"   :header="t('payments.vehicle')" sortable style="width:130px" />
      <Column field="paymentTypeName" :header="t('payments.type')" sortable style="width:130px">
        <template #body="{ data }">{{ data.paymentTypeName ?? '—' }}</template>
      </Column>
      <Column field="totalAmount" :header="t('payments.totalAmount')" sortable style="width:140px" :pt="{ bodyCell: { class: 'cell-amount' } }">
        <template #body="{ data }"><strong>{{ formatMoney(data.totalAmount) }}</strong></template>
      </Column>
      <Column field="totalInstallments" :header="t('payments.totalInstallments')" sortable style="width:140px" :pt="{ bodyCell: { class: 'cell-amount' } }">
        <template #body="{ data }">{{ formatMoney(data.totalInstallments) }}</template>
      </Column>
      <Column :header="t('payments.status')" style="width:120px">
        <template #body="{ data }">
          <Tag v-if="data.storno" :value="t('payments.storno')" severity="danger" />
          <Tag v-else-if="data.payed" :value="t('payments.payed')" severity="success" />
          <Tag v-else :value="t('payments.unpaid')" severity="warn" />
        </template>
      </Column>
      <Column :header="t('common.actions')" style="width:120px">
        <template #body="{ data }">
          <Button icon="pi pi-file-pdf" text rounded @click="downloadInvoice(data.id)" v-tooltip.left="t('common.print')" />
          <Button v-if="!data.storno" icon="pi pi-times" text rounded severity="danger" @click="storno(data.id)" v-tooltip.left="t('payments.storno')" />
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<style scoped>
.payments-page { display: flex; flex-direction: column; gap: 12px; }
.page-header { display: flex; align-items: flex-end; justify-content: space-between; }
.page-header .subtitle { color: var(--text-color-secondary); font-size: 12px; margin-top: 2px; }
.page-header .actions { display: flex; gap: 8px; }

.kpi-strip {
  display: grid; grid-template-columns: repeat(4, 1fr); gap: 12px;
}
.kpi {
  background: var(--color-surface); border: 1px solid var(--color-border);
  border-radius: 8px; padding: 12px 16px;
  box-shadow: var(--shadow-sm, 0 1px 2px rgba(0,0,0,0.04));
}
.kpi-label { font-size: 11px; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-color-secondary); }
.kpi-value { font-size: 18px; font-weight: 700; margin-top: 4px; }
.kpi-value.paid   { color: #16a34a; }
.kpi-value.unpaid { color: #d97706; }
.kpi-value.storno { color: #dc2626; }

.filters {
  display: flex; flex-wrap: wrap; gap: 8px; align-items: center;
  padding: 10px 12px; background: var(--surface-100, #f1f5f9);
  border: 1px solid var(--color-border); border-radius: 6px;
}
.filters .search-wrap { position: relative; display: inline-flex; align-items: center; flex: 1; min-width: 220px; }
.filters .search-wrap i { position: absolute; left: 10px; color: var(--text-color-secondary); pointer-events: none; }
.filters .search-wrap :deep(input) { padding-left: 32px; width: 100%; }
.filters .select-narrow { min-width: 160px; }

.payments-table :deep(.row-storno) { color: var(--text-color-secondary); text-decoration: line-through; }
.payments-table :deep(.cell-amount) { text-align: right; font-variant-numeric: tabular-nums; }
</style>
