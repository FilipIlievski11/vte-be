<script setup lang="ts">
import { ref, computed, onMounted, watch, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'

interface RequestRow {
  id: number; requestTypeId: number; dateCreated: string; dateEnded: string | null; note: string | null
  customerName: string | null; vehicleVin: string | null; vehicleReg: string | null
}
interface RequestType {
  id: number; typeName: string;
  parentRequestTypeId: number | null;
  isNewRegistration: boolean; isRelationDeleted: boolean; isVehicleDeleted: boolean
  isNewCustomer: boolean; isVehicleChanged: boolean; isCustomerChanged: boolean
  isPreviousRegistrationRequired: boolean; isPayRequired: boolean; isSufficient: boolean
  isTechnicalExamRequired: number
}
interface ExamRow {
  id: number; regNumber: string | null; madeDate: string; validTillDate: string; vehicleIsRight: boolean
  ownerName: string | null; vehicleShellNumber: string | null
  vehicleMaker: string | null; vehicleModel: string | null
  examTypeName: string | null
}
interface PaymentDoc {
  id: number; documentNumber: string; datePay: string; totalAmount: number
  totalInstallments: number; payed: boolean; storno: boolean
}

const { t } = useI18n()
const router = useRouter()

const activeRequests = ref<RequestRow[]>([])
const requestTypes   = ref<RequestType[]>([])
const recentExams    = ref<ExamRow[]>([])
const paymentDocs    = ref<PaymentDoc[]>([])
const requestTypeSearch = ref('')

const selectedRequest = ref<RequestRow | null>(null)
const loading = ref(true)

// Persisted expand/collapse state per parent-id (collapsed by default, like legacy)
const expanded = reactive<Record<number, boolean>>(JSON.parse(localStorage.getItem('vte.tree.expanded') || '{}'))
function toggleNode(id: number) {
  expanded[id] = !(expanded[id] ?? false)   // collapsed by default
  localStorage.setItem('vte.tree.expanded', JSON.stringify(expanded))
}
function isExpanded(id: number) { return expanded[id] ?? false }

interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

async function loadAll() {
  loading.value = true
  try {
    const [r, types, e] = await Promise.all([
      api.get<Paged<RequestRow>>('/requests', { params: { open: true, page: 1, pageSize: 200 } }),
      api.get<RequestType[]>('/request-types'),
      api.get<Paged<ExamRow>>('/technical-exam-reports', { params: { page: 1, pageSize: 500 } }),
    ])
    activeRequests.value = r.data.items
    requestTypes.value   = types.data
    recentExams.value    = e.data.items
  } finally { loading.value = false }
}

watch(selectedRequest, async (req) => {
  paymentDocs.value = []
  if (!req) return
  const { data } = await api.get<Paged<PaymentDoc>>('/payment-documents', { params: { page: 1, pageSize: 10 } })
  paymentDocs.value = data.items
})

// Build tree: parents (parentRequestTypeId == null) at top, children grouped by parent
interface TreeParent { parent: RequestType; children: RequestType[] }
const tree = computed<TreeParent[]>(() => {
  const search = requestTypeSearch.value.toLowerCase()
  const parents = requestTypes.value
    .filter(rt => rt.parentRequestTypeId === null)
    .sort((a, b) => a.id - b.id)
  return parents.map(p => {
    const kids = requestTypes.value
      .filter(rt => rt.parentRequestTypeId === p.id)
      .filter(rt => !search || rt.typeName.toLowerCase().includes(search))
    return { parent: p, children: kids }
  }).filter(g => g.children.length > 0 || !search)
})

function startRequest(rt: RequestType) {
  router.push({ path: '/requests/new', query: { typeId: String(rt.id) } })
}
const toast = useToast(); const { ask } = useConfirmDialog()
function approveRequest(r: RequestRow) {
  ask({
    message: t('control.confirmApprove'),
    acceptLabel: t('control.act.approve'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try {
        await api.post(`/requests/${r.id}/end`, { isCustomerChanged: false, isVehicleChanged: false })
        toast.ok(t('common.saved')); await loadAll(); selectedRequest.value = null
      } catch (e: any) { toast.error(e?.response?.data?.error ?? 'Failed') }
    },
  })
}

function newPayment() { router.push('/payments') }
function newExam()    { router.push('/exams/new') }

function formatShortDate(iso: string): string {
  if (!iso) return ''
  const d = new Date(iso)
  return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}`
}
function examRowClass(_row: ExamRow): string { return 'row-exam' }
function groupLabelFor(data: ExamRow): string {
  const k = examGroupBy.value as keyof ExamRow | null
  if (!k) return ''
  const v = data[k] as string | null | undefined
  return v ? String(v) : '—'
}
const selectedExam = ref<ExamRow | null>(null)
function printSelectedExam() {
  const row = selectedExam.value ?? recentExams.value[0]
  if (row) openAuthorizedPdf(`/reports/technical-exam-reports/${row.id}/pdf`)
}

// Group by column for the open-exam grid (DevExpress-style drag header to group)
const examGroupBy = ref<string | null>(null)
const examGroupLabel = computed(() => {
  switch (examGroupBy.value) {
    case 'ownerName':    return t('exams.owner')
    case 'vehicleMaker': return t('exams.vehicle')
    case 'examTypeName': return t('exams.type')
    case 'madeDate':     return t('exams.madeDate')
    case 'regNumber':    return t('exams.regNumber')
    default: return ''
  }
})
const expandedExamGroups = ref<string[]>([])
const sortedExams = computed(() => {
  if (!examGroupBy.value) return recentExams.value
  const k = examGroupBy.value as keyof ExamRow
  return [...recentExams.value].sort((a, b) => String(a[k] ?? '').localeCompare(String(b[k] ?? '')))
})

function onHeaderDragStart(ev: DragEvent, field: string) {
  ev.dataTransfer?.setData('text/x-group-field', field)
  ev.dataTransfer!.effectAllowed = 'move'
}
function onDropZoneDragOver(ev: DragEvent) {
  if (ev.dataTransfer?.types.includes('text/x-group-field')) {
    ev.preventDefault()
    ev.dataTransfer.dropEffect = 'move'
  }
}
function onDropZoneDrop(ev: DragEvent) {
  const f = ev.dataTransfer?.getData('text/x-group-field')
  if (f) { examGroupBy.value = f; expandedExamGroups.value = [] }
  ev.preventDefault()
}
function clearExamGroup() { examGroupBy.value = null; expandedExamGroups.value = [] }

function rowClass(data: RequestRow): string {
  const created = new Date(data.dateCreated)
  const today = new Date(); today.setHours(0,0,0,0)
  return created.getTime() === today.getTime() ? 'row-today' : 'row-older'
}

onMounted(loadAll)
</script>

<template>
  <div class="control-panel">
    <!-- Pane 1: Active requests -->
    <section class="pane pane-active">
      <header>
        <h2><i class="pi pi-inbox" /> {{ t('control.activeRequests') }}</h2>
        <span class="badge">{{ activeRequests.length }}</span>
      </header>
      <DataTable :value="activeRequests" :loading="loading" stripedRows
                 selectionMode="single" v-model:selection="selectedRequest"
                 :rowClass="rowClass" scrollable scrollHeight="flex">
        <template #empty><div class="empty-mini">{{ t('control.noActiveRequests') }}</div></template>
        <Column field="id"           :header="t('control.col.number')" style="width: 80px" sortable />
        <Column field="customerName" :header="t('control.col.customer')" sortable />
        <Column field="vehicleReg"   :header="t('control.col.vehicle')" sortable />
        <Column field="dateCreated"  :header="t('control.col.date')" style="width: 110px" sortable />
      </DataTable>
      <footer class="actions-row">
        <Button :label="t('control.act.change')"  icon="pi pi-pencil"  size="small" outlined :disabled="!selectedRequest" @click="selectedRequest && router.push('/requests')" />
        <Button :label="t('control.act.approve')" icon="pi pi-check"   size="small" severity="success" :disabled="!selectedRequest" @click="selectedRequest && approveRequest(selectedRequest)" />
        <Button :label="t('control.act.refresh')" icon="pi pi-refresh" size="small" outlined @click="loadAll" />
      </footer>
    </section>

    <!-- Pane 2: Payment for selected -->
    <section class="pane pane-payment">
      <header>
        <h2><i class="pi pi-credit-card" /> {{ t('menu.payments') }}</h2>
      </header>
      <DataTable :value="paymentDocs" stripedRows scrollable scrollHeight="flex">
        <template #empty>
          <div class="empty-mini">
            <span v-if="!selectedRequest">{{ t('control.pickRequest') }}</span>
            <span v-else>{{ t('common.empty') }}</span>
          </div>
        </template>
        <Column field="documentNumber" :header="t('payments.documentNumber')" />
        <Column field="datePay"        :header="t('payments.datePay')" />
        <Column :header="t('payments.totalAmount')">
          <template #body="{ data }">{{ data.totalAmount.toFixed(2) }}</template>
        </Column>
        <Column :header="t('payments.status')" style="width: 110px">
          <template #body="{ data }">
            <Tag v-if="data.storno" :value="t('payments.storno')" severity="danger" />
            <Tag v-else-if="data.payed" :value="t('payments.payed')" severity="success" />
            <Tag v-else :value="t('payments.unpaid')" severity="warn" />
          </template>
        </Column>
      </DataTable>
      <footer class="actions-row">
        <Button :label="t('control.act.refreshData')" icon="pi pi-refresh" size="small" outlined />
        <Button :label="t('control.act.newInvoice')"  icon="pi pi-plus" size="small" @click="newPayment" />
        <Button :label="t('control.act.cashReport')"  icon="pi pi-file-pdf" size="small" outlined
                @click="openAuthorizedPdf('/reports/cash-report')" />
      </footer>
    </section>

    <!-- Pane 3: Request types tree (legacy hierarchy, double-click to create) -->
    <section class="pane pane-types">
      <header class="legacy-header">
        <span>{{ t('control.doubleClickToCreate') }}</span>
      </header>
      <div class="search-box">
        <i class="pi pi-search" />
        <InputText v-model="requestTypeSearch" :placeholder="t('common.search')" />
      </div>
      <div class="legacy-tree">
        <div v-for="(g, i) in tree" :key="g.parent.id" class="legacy-group" :class="`group-tone-${i % 3}`">
          <div class="legacy-parent" @click="toggleNode(g.parent.id)">
            <span class="legacy-toggle">{{ isExpanded(g.parent.id) ? '−' : '+' }}</span>
            <span class="legacy-parent-label">{{ g.parent.typeName }}</span>
          </div>
          <div v-show="isExpanded(g.parent.id)" class="legacy-children">
            <div v-for="rt in g.children" :key="rt.id" class="legacy-leaf"
                 @dblclick="startRequest(rt)" :title="t('control.doubleClickToCreate')">
              <span class="legacy-leaf-bullet">▸</span>
              <span class="legacy-leaf-name">{{ rt.typeName }}</span>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- Pane 4: Open tech-exam records (Отворени записници за технички преглед) -->
    <section class="pane pane-exams">
      <header>
        <h2><i class="pi pi-check-square" /> {{ t('control.openExams') }}</h2>
        <span class="badge">{{ recentExams.length }}</span>
      </header>
      <div class="drop-zone" :class="{ 'has-group': !!examGroupBy }"
           @dragover="onDropZoneDragOver" @drop="onDropZoneDrop">
        <span v-if="!examGroupBy" class="zone-hint">{{ t('control.dragHeaderHint') }}</span>
        <span v-else class="group-pill">
          <i class="pi pi-bars" /> {{ examGroupLabel }}
          <i class="pi pi-times pill-close" @click="clearExamGroup" />
        </span>
      </div>
      <DataTable :value="sortedExams" stripedRows scrollable scrollHeight="flex" :rowClass="examRowClass" selectionMode="single"
                 v-model:selection="selectedExam"
                 :rowGroupMode="examGroupBy ? 'subheader' : undefined"
                 :groupRowsBy="examGroupBy ?? undefined"
                 :expandableRowGroups="!!examGroupBy"
                 v-model:expandedRowGroups="expandedExamGroups">
        <template #empty><div class="empty-mini">{{ t('common.empty') }}</div></template>
        <template #groupheader="{ data }">
          <span class="group-header"><i class="pi pi-folder" /> {{ groupLabelFor(data) }}</span>
        </template>
        <Column v-if="examGroupBy !== 'ownerName'" field="ownerName" sortable>
          <template #header>
            <span class="drag-header" draggable="true" @mousedown.stop @click.stop @dragstart="onHeaderDragStart($event, 'ownerName')" :title="t('control.dragHeaderHint')">
              <i class="pi pi-bars drag-handle" />
            </span>
            <span>{{ t('exams.owner') }}</span>
          </template>
        </Column>
        <Column v-if="examGroupBy !== 'vehicleMaker'" sortField="vehicleShellNumber" sortable>
          <template #header>
            <span class="drag-header" draggable="true" @mousedown.stop @click.stop @dragstart="onHeaderDragStart($event, 'vehicleMaker')" :title="t('control.dragHeaderHint')">
              <i class="pi pi-bars drag-handle" />
            </span>
            <span>{{ t('exams.vehicle') }}</span>
          </template>
          <template #body="{ data }">
            <span>{{ data.vehicleShellNumber || '—' }}</span>
            <span v-if="data.vehicleMaker || data.vehicleModel" class="muted"> ({{ [data.vehicleMaker, data.vehicleModel].filter(Boolean).join(', ') }})</span>
          </template>
        </Column>
        <Column v-if="examGroupBy !== 'madeDate'" field="madeDate" sortable style="width:90px">
          <template #header>
            <span class="drag-header" draggable="true" @mousedown.stop @click.stop @dragstart="onHeaderDragStart($event, 'madeDate')" :title="t('control.dragHeaderHint')">
              <i class="pi pi-bars drag-handle" />
            </span>
            <span>{{ t('exams.madeDate') }}</span>
          </template>
          <template #body="{ data }">{{ formatShortDate(data.madeDate) }}</template>
        </Column>
        <Column v-if="examGroupBy !== 'regNumber'" field="regNumber" sortable style="width:110px">
          <template #header>
            <span class="drag-header" draggable="true" @mousedown.stop @click.stop @dragstart="onHeaderDragStart($event, 'regNumber')" :title="t('control.dragHeaderHint')">
              <i class="pi pi-bars drag-handle" />
            </span>
            <span>{{ t('exams.regNumber') }}</span>
          </template>
        </Column>
        <Column v-if="examGroupBy !== 'examTypeName'" field="examTypeName" sortable style="width:110px">
          <template #header>
            <span class="drag-header" draggable="true" @mousedown.stop @click.stop @dragstart="onHeaderDragStart($event, 'examTypeName')" :title="t('control.dragHeaderHint')">
              <i class="pi pi-bars drag-handle" />
            </span>
            <span>{{ t('exams.type') }}</span>
          </template>
        </Column>
      </DataTable>
      <footer class="actions-row">
        <Button :label="t('control.act.refreshData')" icon="pi pi-refresh" size="small" outlined @click="loadAll" />
        <Button :label="t('control.act.fill')"        icon="pi pi-pencil" size="small" outlined @click="newExam" />
        <Button :label="t('control.act.print')"       icon="pi pi-print"  size="small" outlined @click="printSelectedExam" />
        <Button :label="t('control.act.new')"         icon="pi pi-plus"   size="small" @click="newExam" />
        <Button :label="t('control.act.cancelDoc')"   icon="pi pi-times"  size="small" outlined severity="danger" @click="recentExams = []" />
      </footer>
    </section>
  </div>
</template>

<style scoped>
.control-panel {
  display: grid;
  grid-template-columns: 1.2fr 1fr;
  grid-template-rows: 1fr 1fr;
  gap: 1rem;
  height: calc(100vh - var(--topbar-h) - 3rem);
  min-height: 580px;
}
.pane-active   { grid-column: 1; grid-row: 1; }
.pane-types    { grid-column: 1; grid-row: 2; }
.pane-payment  { grid-column: 2; grid-row: 1; }
.pane-exams    { grid-column: 2; grid-row: 2; }

.pane {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow-sm);
  display: flex; flex-direction: column;
  min-height: 0; overflow: hidden;
}
.pane > header {
  display: flex; align-items: center; gap: 0.5rem;
  padding: 0.75rem 1rem;
  border-bottom: 1px solid var(--color-border);
  background: var(--color-surface-2);
}
.pane > header h2 { margin: 0; font-size: 0.875rem; font-weight: 600; display: flex; align-items: center; gap: 0.5rem; }
.pane > header h2 i { color: var(--color-brand-600); }
.drop-zone {
  display: flex; align-items: center; gap: 8px; min-height: 34px;
  padding: 6px 12px; font-size: 12px; color: var(--text-color-secondary);
  background: var(--surface-100, #f1f5f9); border-bottom: 1px solid var(--color-border);
  border: 1px dashed transparent; transition: background-color 120ms, border-color 120ms;
}
.drop-zone:hover { background: var(--surface-200, #e2e8f0); }
.drop-zone.has-group { background: #eff6ff; border-color: #3b82f6; }
.drop-zone .zone-hint { font-style: italic; }
.group-pill {
  display: inline-flex; align-items: center; gap: 6px;
  background: var(--color-brand-600, #3b82f6); color: white;
  padding: 4px 10px; border-radius: 999px; font-weight: 600; font-size: 12px;
}
.group-pill .pill-close { cursor: pointer; opacity: 0.85; }
.group-pill .pill-close:hover { opacity: 1; }
.drag-header {
  display: inline-flex; align-items: center; cursor: grab; user-select: none;
  padding: 0 6px 0 0; margin-right: 4px;
}
.drag-header:active { cursor: grabbing; }
.drag-handle { color: var(--text-color-secondary); font-size: 0.75em; }
.muted { color: var(--text-color-secondary); font-size: 0.85em; }

/* Request-types tree — legacy hierarchy, modern styling */
.legacy-header {
  background: var(--surface-100, #f1f5f9);
  padding: 8px 12px; font-size: 12px; color: var(--text-color-secondary);
  border-bottom: 1px solid var(--color-border);
  font-style: italic;
}
.legacy-tree { flex: 1; overflow: auto; padding: 6px 4px; background: var(--color-surface); font-size: 13px; }
.legacy-group + .legacy-group { margin-top: 4px; }
.legacy-parent {
  cursor: pointer; user-select: none;
  display: flex; align-items: center; gap: 8px;
  padding: 8px 10px;
  font-weight: 600; color: var(--text-color);
  background: var(--surface-100, #f1f5f9);
  border-radius: 4px;
  transition: background-color 120ms;
}
.legacy-parent:hover { background: var(--surface-200, #e2e8f0); }
.legacy-toggle {
  display: inline-flex; width: 16px; height: 16px; align-items: center; justify-content: center;
  background: var(--color-surface); border: 1px solid var(--color-border);
  border-radius: 3px; font-family: monospace; line-height: 1; font-size: 12px;
  color: var(--text-color-secondary);
}
.legacy-parent-label { flex: 1; }
.legacy-children { padding: 2px 0 6px 26px; }
.legacy-leaf {
  cursor: pointer; padding: 4px 8px;
  display: flex; align-items: center; gap: 8px;
  color: var(--color-brand-600, #3b82f6);
  border-radius: 3px;
}
.legacy-leaf:hover { background: var(--surface-100, #f1f5f9); }
.legacy-leaf-bullet { color: var(--text-color-secondary); font-size: 10px; }
.legacy-leaf-name { flex: 1; }

.row-exam { font-size: 13px; }
.group-header { font-weight: 600; color: var(--color-brand-600); }
.pane .badge { margin-left: auto; background: var(--color-info-50); color: var(--color-brand-700); border-radius: 999px; padding: 2px 10px; font-size: 0.75rem; font-weight: 600; }
.pane .hint { margin-left: auto; font-size: 0.7rem; color: var(--color-text-muted); }

.pane :deep(.p-datatable) { flex: 1; min-height: 0; border: 0; box-shadow: none; border-radius: 0; }
.pane :deep(.p-datatable-thead > tr > th) { font-size: 0.7rem; }

.actions-row { display: flex; gap: 0.5rem; padding: 0.625rem 0.75rem; border-top: 1px solid var(--color-border); flex-wrap: wrap; background: var(--color-surface-2); }

.empty-mini { padding: 1.5rem; text-align: center; color: var(--color-text-muted); font-size: 0.875rem; }

:deep(.row-today) td { background: var(--color-success-50) !important; }
:deep(.row-older) td { background: transparent !important; }

/* Tree */
.search-box { position: relative; padding: 0.5rem 0.75rem; border-bottom: 1px solid var(--color-border); }
.search-box i { position: absolute; left: 1.4rem; top: 50%; transform: translateY(-50%); color: var(--color-text-muted); pointer-events: none; }
.search-box :deep(input) { width: 100%; padding-left: 2rem; }
.types-tree { flex: 1; overflow-y: auto; padding: 0.5rem 0; }

.tree-group { user-select: none; }
.tree-parent {
  display: flex; align-items: center; gap: 0.5rem;
  padding: 0.5rem 0.75rem;
  background: linear-gradient(180deg, #f1f5f9, #e2e8f0);
  font-weight: 600; font-size: 0.8125rem;
  cursor: pointer;
  border-bottom: 1px solid var(--color-border);
}
.tree-parent:hover { background: linear-gradient(180deg, #e2e8f0, #cbd5e1); }
.tree-parent i { font-size: 0.7rem; color: var(--color-text-muted); width: 14px; }
.parent-label { flex: 1; color: var(--color-text); }
.parent-count { color: var(--color-text-muted); font-weight: 500; font-size: 0.7rem; }

.tree-children { background: var(--color-surface); }
.tree-leaf {
  display: flex; align-items: center; gap: 0.5rem;
  padding: 0.5rem 0.75rem 0.5rem 2.25rem;
  cursor: pointer;
  border-left: 3px solid transparent;
  font-size: 0.8rem;
  color: var(--color-brand-700);
}
.tree-leaf:hover { background: var(--color-info-50); border-left-color: var(--color-brand-500); }
.leaf-bullet { width: 4px; height: 4px; border-radius: 50%; background: var(--color-brand-400); margin-right: 0.25rem; flex-shrink: 0; }
.leaf-name { flex: 1; text-decoration: underline; text-decoration-color: transparent; }
.tree-leaf:hover .leaf-name { text-decoration-color: var(--color-brand-500); }

.leaf-flags { display: flex; gap: 0.25rem; }
.mini-flag {
  display: inline-flex; align-items: center; justify-content: center;
  width: 20px; height: 20px; border-radius: 50%; font-size: 0.6rem;
}
.mini-flag.flag-info   { background: var(--color-info-50);   color: var(--color-brand-700); }
.mini-flag.flag-warn   { background: var(--color-warn-50);   color: var(--color-warn-600); }
.mini-flag.flag-danger { background: var(--color-danger-50); color: var(--color-danger-600); }

@media (max-width: 1100px) {
  .control-panel { grid-template-columns: 1fr; grid-template-rows: auto auto auto auto; height: auto; }
  .pane { min-height: 320px; }
  .pane-active{grid-column:1;grid-row:1} .pane-types{grid-column:1;grid-row:2} .pane-payment{grid-column:1;grid-row:3} .pane-exams{grid-column:1;grid-row:4}
}
</style>
