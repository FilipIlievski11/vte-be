<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { RouterLink, useRouter } from 'vue-router';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { CustomerDebtRow, Paged, RequestListItem, RequestType, TechExamListItem } from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';
import Tree from 'primevue/tree';
import type { TreeNode } from 'primevue/treenode';
import Checkbox from 'primevue/checkbox';
import Dialog from 'primevue/dialog';
import Select from 'primevue/select';
import Button from 'primevue/button';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const { t } = useI18n();
const auth = useAuthStore();
const router = useRouter();
const toast = useToast();
const confirm = useConfirm();

const recentOpen = ref<RequestListItem[]>([]);
const recentExams = ref<TechExamListItem[]>([]);
const debts = ref<CustomerDebtRow[]>([]);
const debtCount = ref(0);
const debtTotal = ref(0);
const reqTree = ref<TreeNode[]>([]);
const expandedKeys = ref<Record<string, boolean>>({});
const loading = ref(true);

function fmtMoney(v: number | null | undefined): string {
  if (v == null) return '—';
  return v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}

/** Group flat debt rows by (relation → vehicle), preserving order — backend already sorted. */
interface DebtClientGroup {
  relationId: number;
  clientName: string | null;
  clientMB: string | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  vehicleMakerModel: string | null;
  rows: CustomerDebtRow[];
  total: number;
}
const debtGroups = computed<DebtClientGroup[]>(() => {
  const out: DebtClientGroup[] = [];
  for (const d of debts.value) {
    let g = out.length ? out[out.length - 1] : null;
    if (!g || g.relationId !== d.customerVehicleRelationId) {
      g = {
        relationId: d.customerVehicleRelationId,
        clientName: d.clientName,
        clientMB:   d.clientMB,
        vehiclePlate: d.vehiclePlate,
        vehicleVin:   d.vehicleVin,
        vehicleMakerModel: d.vehicleMakerModel,
        rows: [],
        total: 0,
      };
      out.push(g);
    }
    g.rows.push(d);
    g.total += d.price;
  }
  return out;
});

async function refreshDebts() {
  try {
    const [list, summary] = await Promise.all([
      api.get<Paged<CustomerDebtRow>>('/customer-debts', { params: { unpaidOnly: true, pageSize: 50 } }),
      api.get<{ count: number; total: number }>('/customer-debts/summary'),
    ]);
    debts.value = list.data.items ?? [];
    debtCount.value = summary.data.count ?? 0;
    debtTotal.value = summary.data.total ?? 0;
    // Drop selection ids that no longer appear (settled/deleted in another tab).
    const live = new Set(debts.value.map(d => d.id));
    if (selectedDebtIds.value.size > 0) {
      const next = new Set([...selectedDebtIds.value].filter(id => live.has(id)));
      if (next.size !== selectedDebtIds.value.size) selectedDebtIds.value = next;
    }
  } catch { /* swallow — empty state shows */ }
}

// --- Selection state for the Наплата panel ---
const selectedDebtIds = ref<Set<number>>(new Set());

function isSelected(id: number) { return selectedDebtIds.value.has(id); }
function toggleSelected(id: number, on: boolean) {
  const s = new Set(selectedDebtIds.value);
  if (on) s.add(id); else s.delete(id);
  selectedDebtIds.value = s;
}
// Group-level select toggle (used by the per-group checkbox in the row's client header).
function groupSelectState(group: { rows: { id: number }[] }): boolean | 'indeterminate' {
  const total = group.rows.length;
  const selected = group.rows.filter(r => selectedDebtIds.value.has(r.id)).length;
  if (selected === 0) return false;
  if (selected === total) return true;
  return 'indeterminate';
}
function toggleGroup(group: { rows: { id: number }[] }, on: boolean) {
  const s = new Set(selectedDebtIds.value);
  for (const r of group.rows) { if (on) s.add(r.id); else s.delete(r.id); }
  selectedDebtIds.value = s;
}

// --- Направи сметка (bill from selected debts) ---
const billDialogVisible = ref(false);
const billTypeId = ref<number | null>(null);
const billSaving = ref(false);
interface PaymentTypeOpt { id: number; name: string; isCash: boolean; isInstallment: boolean; prefix: string | null }
const paymentTypes = ref<PaymentTypeOpt[]>([]);

/** Distinct relation ids among the selected debt rows — a bill covers exactly one. */
const selectedRelationIds = computed(() => {
  const s = new Set<number>();
  for (const d of debts.value) if (selectedDebtIds.value.has(d.id)) s.add(d.customerVehicleRelationId);
  return s;
});

const selectedTotal = computed(() =>
  debts.value.filter(d => selectedDebtIds.value.has(d.id)).reduce((sum, d) => sum + d.price, 0));

async function openBillDialog() {
  if (selectedRelationIds.value.size !== 1) {
    toast.add({ severity: 'warn', summary: t('dashboard.naplata.billOneClient'), life: 3000 });
    return;
  }
  if (paymentTypes.value.length === 0) {
    try {
      const { data } = await api.get<PaymentTypeOpt[]>('/payment-documents/payment-types', { params: { usedOnly: true } });
      paymentTypes.value = data.filter(x => !x.isInstallment);
    } catch { /* dropdown stays empty; dialog still opens */ }
  }
  // default: cash ("во готово" — the un-prefixed cash type)
  billTypeId.value = paymentTypes.value.find(x => x.isCash && !x.prefix)?.id
    ?? paymentTypes.value.find(x => x.isCash)?.id
    ?? paymentTypes.value[0]?.id ?? null;
  billDialogVisible.value = true;
}

async function createBill() {
  if (!billTypeId.value) return;
  billSaving.value = true;
  try {
    const { data } = await api.post<{ id: number; documentNumber: string; linesTotal: number; lines: number }>(
      '/payment-documents/from-debts',
      { debtIds: [...selectedDebtIds.value], paymentTypeId: billTypeId.value });
    billDialogVisible.value = false;
    selectedDebtIds.value = new Set();
    await refreshDebts();
    toast.add({ severity: 'success', summary: t('dashboard.naplata.billCreated', { no: data.documentNumber }), life: 3500 });
    router.push(`/payments/${data.id}`);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('dashboard.naplata.billFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4500 });
  } finally {
    billSaving.value = false;
  }
}

async function deleteSelectedDebts() {
  const ids = [...selectedDebtIds.value];
  if (ids.length === 0) return;
  confirm.require({
    message: t('dashboard.naplata.deleteBulkConfirm', { n: ids.length }),
    header:  t('common.confirmDelete'),
    icon:    'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        const { data } = await api.post<{ requested: number; deleted: number; skippedPaid: number; skippedMissing: number }>(
          '/customer-debts/delete-batch', { ids });
        selectedDebtIds.value = new Set();
        await refreshDebts();
        toast.add({
          severity: data.deleted ? 'success' : 'warn',
          summary: t('dashboard.naplata.deletedN', { n: data.deleted }),
          detail: data.skippedPaid > 0
            ? t('dashboard.naplata.skippedPaid', { n: data.skippedPaid })
            : undefined,
          life: 2200,
        });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

const rolesText = computed(() => auth.roles.length ? auth.roles.join(', ') : t('dashboard.noRole'));

function fmtDate(s: string | null): string {
  if (!s) return '—';
  return new Date(s).toLocaleDateString();
}

// Build the request-type tree (hierarchy via parentRequestTypeId), mirroring the
// legacy Контролна табла tree: 3 roots, expanded; nodes coloured by print form.
function buildReqTree(types: RequestType[]) {
  const active = types.filter(t => t.active);
  const byParent = new Map<number | null, RequestType[]>();
  for (const ty of active) {
    const p = ty.parentRequestTypeId ?? null;
    const arr = byParent.get(p);
    if (arr) arr.push(ty); else byParent.set(p, [ty]);
  }
  const make = (ty: RequestType): TreeNode => ({
    key: String(ty.id),
    label: ty.name,
    data: { id: ty.id, documentPrintId: ty.documentPrintId },
    children: (byParent.get(ty.id) ?? []).map(make),
  });
  const roots = (byParent.get(null) ?? []).map(make);
  const exp: Record<string, boolean> = {};
  for (const r of roots) exp[r.key as string] = true;   // expand top-level groups
  expandedKeys.value = exp;
  reqTree.value = roots;
}

function printClass(pid: number | undefined): string {
  return pid === 1 ? 'pf-zelen' : pid === 2 ? 'pf-plav' : pid === 3 ? 'pf-bel' : 'pf-none';
}
function createRequest(id: number) {
  router.push(`/requests/new?typeId=${id}`);
}

// Refresh debts whenever the dashboard regains visibility — covers the common
// "save a tech-exam in another tab → come back to dashboard" loop.
function onVisible() { if (!document.hidden) refreshDebts(); }
onMounted(() => document.addEventListener('visibilitychange', onVisible));
onUnmounted(() => document.removeEventListener('visibilitychange', onVisible));

onMounted(async () => {
  try {
    const [types, openReq, exams] = await Promise.all([
      api.get<RequestType[]>('/request-types', { params: { activeOnly: true } })
        .catch(() => ({ data: [] as RequestType[] })),
      api.get<Paged<RequestListItem>>('/requests', { params: { status: 'open', pageSize: 5 } })
        .catch(() => ({ data: { items: [], total: 0, page: 1, pageSize: 5 } as Paged<RequestListItem> })),
      api.get<Paged<TechExamListItem>>('/technical-exams', { params: { sort: 'made', dir: 'desc', pageSize: 5 } })
        .catch(() => ({ data: { items: [], total: 0, page: 1, pageSize: 5 } as Paged<TechExamListItem> })),
      refreshDebts(),
    ]);
    buildReqTree(types.data ?? []);
    recentOpen.value = openReq.data.items ?? [];
    recentExams.value = exams.data.items ?? [];
  } finally {
    loading.value = false;
  }
});
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('dashboard.welcome', { name: auth.fullName ?? auth.userName }) }}</h1>
      <div class="subtitle">{{ t('dashboard.role', { roles: rolesText }) }}</div>
    </div>
  </div>

  <!-- Top row: create-request tree + pending bills -->
  <div class="dash-top-grid">
    <!-- Create request by type (legacy Контролна табла tree) -->
    <div class="recent-card req-card">
      <div class="recent-header">
        <h2>{{ t('dashboard.createRequest.title') }}</h2>
        <RouterLink to="/requests/new" class="all-link">{{ t('dashboard.createRequest.blank') }} →</RouterLink>
      </div>
      <Tree
        v-if="reqTree.length"
        :value="reqTree"
        v-model:expandedKeys="expandedKeys"
        class="req-tree"
      >
        <template #default="{ node }">
          <a
            href="#"
            class="req-link"
            :class="printClass(node.data?.documentPrintId)"
            @click.prevent.stop="createRequest(node.data.id)"
            v-tooltip.right="t('dashboard.createRequest.tip')"
          >
            <span class="pf-dot" />
            <span class="req-label">{{ node.label }}</span>
          </a>
        </template>
      </Tree>
      <div v-else-if="loading" class="empty"><i class="pi pi-spin pi-spinner" /></div>
      <div v-else class="empty"><i class="pi pi-inbox" /><span>{{ t('dashboard.createRequest.empty') }}</span></div>
    </div>

    <!-- Наплата — open customer debts grouped by client → vehicle (legacy CustomerFinancialState) -->
    <div class="recent-card naplata-card">
      <div class="recent-header">
        <h2>
          {{ t('dashboard.naplata.title') }}
          <span v-if="debtCount" class="count-pill">{{ debtCount }}</span>
        </h2>
        <div class="naplata-actions">
          <Button
            v-if="selectedDebtIds.size > 0"
            :label="t('dashboard.naplata.makeBill', { n: selectedDebtIds.size })"
            icon="pi pi-file" severity="primary" size="small"
            :disabled="selectedRelationIds.size !== 1"
            v-tooltip.bottom="selectedRelationIds.size !== 1 ? t('dashboard.naplata.billOneClient') : undefined"
            @click="openBillDialog"
          />
          <Button
            v-if="selectedDebtIds.size > 0"
            :label="t('dashboard.naplata.deleteSelected', { n: selectedDebtIds.size })"
            icon="pi pi-trash" severity="danger" size="small" outlined
            @click="deleteSelectedDebts"
          />
          <button class="refresh-btn" @click="refreshDebts" v-tooltip.left="t('dashboard.naplata.refresh')">
            <i class="pi pi-refresh" />
          </button>
        </div>
      </div>

      <div v-if="debtGroups.length" class="naplata">
        <div class="naplata-head">
          <span class="col-check"></span>
          <span class="col-service">{{ t('dashboard.naplata.col.service') }}</span>
          <span class="col-note">{{ t('dashboard.naplata.col.note') }}</span>
          <span class="col-price">{{ t('dashboard.naplata.col.price') }}</span>
        </div>

        <div v-for="g in debtGroups" :key="g.relationId" class="naplata-group">
          <div class="grp-head">
            <Checkbox
              :modelValue="groupSelectState(g) === true"
              :indeterminate="groupSelectState(g) === 'indeterminate'"
              :binary="true"
              @update:modelValue="(v: boolean) => toggleGroup(g, v)"
              @click.stop
              v-tooltip.right="t('dashboard.naplata.selectGroup')"
            />
            <span
              class="grp-client clickable"
              @click="router.push(`/payments?customerVehicleRelationId=${g.relationId}`)"
            >{{ g.clientName || '—' }}</span>
            <span v-if="g.vehiclePlate || g.vehicleVin" class="grp-vehicle muted">
              <span v-if="g.vehiclePlate" class="plate">{{ g.vehiclePlate }}</span>
              <span v-else>{{ g.vehicleVin }}</span>
              <span v-if="g.vehicleMakerModel"> · {{ g.vehicleMakerModel }}</span>
            </span>
            <span class="grp-total mono">{{ fmtMoney(g.total) }}</span>
          </div>
          <div v-for="r in g.rows" :key="r.id" class="grp-row" :class="{ selected: isSelected(r.id) }">
            <Checkbox
              :modelValue="isSelected(r.id)"
              :binary="true"
              @update:modelValue="(v: boolean) => toggleSelected(r.id, v)"
            />
            <span class="col-service" :title="r.priceCatalogName ?? ''">{{ r.priceCatalogName || '—' }}</span>
            <span class="col-note muted" :title="r.note ?? ''">{{ r.note || '' }}</span>
            <span class="col-price mono">{{ fmtMoney(r.price) }}</span>
          </div>
        </div>

        <div class="naplata-footer">
          <span class="muted">{{ t('dashboard.naplata.total') }}</span>
          <span class="mono">{{ fmtMoney(debtTotal) }} <span class="muted">ден.</span></span>
        </div>
      </div>
      <div v-else-if="loading" class="empty"><i class="pi pi-spin pi-spinner" /></div>
      <div v-else class="empty">
        <i class="pi pi-check-circle" />
        <span>{{ t('dashboard.naplata.empty') }}</span>
      </div>
    </div>
  </div>

  <div class="recent-card">
    <div class="recent-header">
      <h2>{{ t('dashboard.recentOpen.title') }}</h2>
      <RouterLink to="/requests" class="all-link">{{ t('dashboard.recentOpen.all') }} →</RouterLink>
    </div>
    <DataTable
      v-if="recentOpen.length"
      :value="recentOpen"
      size="small"
      stripedRows
      rowHover
      @row-click="(e: any) => router.push(`/requests/${e.data.id}`)"
      :pt="{ row: { style: 'cursor: pointer' } }"
      dataKey="id"
    >
      <Column field="id" :header="t('requests.col.id')" style="width:70px" />
      <Column field="requestTypeName" :header="t('requests.col.type')" />
      <Column field="clientDisplayName" :header="t('requests.col.client')">
        <template #body="{ data }">{{ data.clientDisplayName || '—' }}</template>
      </Column>
      <Column :header="t('requests.col.vehicle')">
        <template #body="{ data }">
          <span v-if="data.vehiclePlate" class="plate">{{ data.vehiclePlate }}</span>
          <span v-else class="muted">—</span>
        </template>
      </Column>
      <Column :header="t('requests.col.created')" style="width:110px">
        <template #body="{ data }">{{ fmtDate(data.createdAt) }}</template>
      </Column>
      <Column :header="t('requests.col.status')" style="width:90px">
        <template #body>
          <Tag :value="t('requests.status.open')" severity="success" />
        </template>
      </Column>
    </DataTable>
    <div v-else class="empty">
      <i class="pi pi-inbox" />
      <span>{{ t('dashboard.recentOpen.empty') }}</span>
    </div>
  </div>

  <div class="recent-card">
    <div class="recent-header">
      <h2>{{ t('dashboard.recentExams.title') }}</h2>
      <RouterLink to="/technical-exams" class="all-link">{{ t('dashboard.recentExams.all') }} →</RouterLink>
    </div>
    <DataTable
      v-if="recentExams.length"
      :value="recentExams"
      size="small"
      stripedRows
      rowHover
      @row-click="(e: any) => router.push(`/technical-exams/${e.data.id}`)"
      :pt="{ row: { style: 'cursor: pointer' } }"
      dataKey="id"
    >
      <Column field="regNumber" :header="t('techExam.col.regNumber')" style="width:150px">
        <template #body="{ data }"><span class="plate">{{ data.regNumber || '—' }}</span></template>
      </Column>
      <Column :header="t('techExam.col.client')">
        <template #body="{ data }">{{ data.clientName || '—' }}</template>
      </Column>
      <Column :header="t('techExam.col.vehicle')">
        <template #body="{ data }">
          <span v-if="data.vehiclePlate" class="plate">{{ data.vehiclePlate }}</span>
          <span v-else class="muted">—</span>
        </template>
      </Column>
      <Column :header="t('techExam.col.type')" style="width:110px">
        <template #body="{ data }">{{ data.typeCode || data.typeName || '—' }}</template>
      </Column>
      <Column :header="t('techExam.col.made')" style="width:110px">
        <template #body="{ data }">{{ fmtDate(data.madeDate) }}</template>
      </Column>
      <Column :header="t('techExam.col.result')" style="width:90px">
        <template #body="{ data }">
          <Tag :value="data.vehicleIsRight ? t('techExam.pass') : t('techExam.fail')"
               :severity="data.vehicleIsRight ? 'success' : 'danger'" />
        </template>
      </Column>
    </DataTable>
    <div v-else class="empty">
      <i class="pi pi-inbox" />
      <span>{{ t('dashboard.recentExams.empty') }}</span>
    </div>
  </div>

  <!-- Направи сметка dialog -->
  <Dialog v-model:visible="billDialogVisible" :header="t('dashboard.naplata.makeBillTitle')"
    :modal="true" :style="{ width: '26rem' }">
    <div class="bill-form">
      <div class="bill-row">
        <span class="muted">{{ t('dashboard.naplata.billItems') }}</span>
        <span>{{ selectedDebtIds.size }}</span>
      </div>
      <div class="bill-row">
        <span class="muted">{{ t('dashboard.naplata.billTotal') }}</span>
        <span class="mono"><b>{{ fmtMoney(selectedTotal) }}</b> {{ t('dashboard.naplata.den') }}</span>
      </div>
      <div class="bill-field">
        <label>{{ t('dashboard.naplata.billType') }}</label>
        <Select v-model="billTypeId" :options="paymentTypes"
          optionLabel="name" optionValue="id" class="bill-type-select" />
      </div>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined :disabled="billSaving"
        @click="billDialogVisible = false" />
      <Button :label="t('dashboard.naplata.makeBillConfirm')" icon="pi pi-check" severity="primary"
        :loading="billSaving" :disabled="!billTypeId" @click="createBill" />
    </template>
  </Dialog>
</template>

<style scoped>
.recent-card {
  margin-top: 1.25rem;
  background: var(--p-content-background);
  border: 1px solid var(--p-content-border-color);
  border-radius: 8px;
  padding: 1rem;
}
.recent-header {
  display: flex; align-items: center; justify-content: space-between;
  margin-bottom: .75rem;
}
.recent-header h2 { font-size: 1rem; margin: 0 }
.all-link { font-size: .85rem; color: var(--p-primary-color); text-decoration: none }
.all-link:hover { text-decoration: underline }
.plate { font-family: monospace; font-weight: 600 }
.muted { color: var(--p-text-muted-color) }
.empty {
  display: flex; align-items: center; justify-content: center; gap: .5rem;
  padding: 2rem; color: var(--p-text-muted-color); font-size: .9rem
}
.empty i { font-size: 1.4rem }

/* Top row: tree | pending-bills.  Wraps to one column under ~960px. */
.dash-top-grid {
  display: grid;
  grid-template-columns: minmax(0, 1.1fr) minmax(0, 1fr);
  gap: 1rem;
  margin-top: 1.25rem;
}
@media (max-width: 760px) {
  .dash-top-grid { grid-template-columns: 1fr }
}
.dash-top-grid > .recent-card { margin-top: 0 }   /* parent grid owns the spacing */

/* Request-type shortcut tree — compact, dense, matches the dashboard scale */
.req-card { padding: .65rem .85rem; }
.req-card .recent-header { margin-bottom: .25rem }
.req-card .recent-header h2 { font-size: .9rem }

.req-tree { background: transparent }
/* Kill PrimeVue's default per-node padding for a true dense list */
.req-tree :deep(.p-tree-container)         { padding: 0; gap: 0 }
.req-tree :deep(.p-tree-node)              { padding: 0 }
.req-tree :deep(.p-tree-node-content)      { padding: 1px 4px; gap: .25rem; min-height: 0 }
.req-tree :deep(.p-tree-node-children)     { padding-left: .9rem; gap: 0 }
.req-tree :deep(.p-tree-node-toggle-button),
.req-tree :deep(.p-tree-node-toggler)      { width: 1rem; height: 1rem; font-size: .65rem; margin-right: 0 }
.req-tree :deep(.p-tree-node-leaf-icon)    { display: none }
.req-tree :deep(.p-tree-node-content:hover){ background: var(--p-content-hover-background, rgba(0,0,0,.04)) }

.req-link {
  display: flex; align-items: center; gap: .4rem; width: 100%;
  text-decoration: none; color: var(--p-text-color);
  font-size: .78rem; line-height: 1.2;
  padding: 0; border-radius: 3px;
}
.req-link:hover { color: var(--p-primary-color) }
.req-label { flex: 1 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.pf-dot { width: .42rem; height: .42rem; border-radius: 50%; flex: 0 0 auto; border: 1px solid rgba(0,0,0,.12) }
.pf-zelen .pf-dot { background: #16a34a }
.pf-plav  .pf-dot { background: #2563eb }
.pf-bel   .pf-dot { background: #e5e7eb }
.pf-none  .pf-dot { background: transparent; border-style: dashed }

/* Наплата — open customer debts grouped by client → vehicle (legacy CustomerFinancialState). */
.naplata-card { padding: .65rem .85rem; display: flex; flex-direction: column }
.count-pill {
  display: inline-flex; align-items: center; justify-content: center;
  min-width: 1.4rem; height: 1.1rem; padding: 0 .4rem; margin-left: .35rem;
  font-size: .7rem; font-weight: 600;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  border-radius: 999px; color: var(--p-text-muted-color);
}
.refresh-btn {
  background: none; border: 0; color: var(--p-primary-color); cursor: pointer;
  padding: 2px 4px; font-size: .85rem;
}
.refresh-btn:hover { color: var(--p-primary-color-hover, var(--p-primary-color)); opacity: .8 }

/* 4-column grid: ☐ | Услуга | Заблешка | Цена */
.naplata { font-size: .78rem; line-height: 1.2 }
.naplata-head, .grp-row {
  display: grid;
  grid-template-columns: 1.1rem minmax(0, 1.6fr) minmax(0, 1.2fr) 6.5rem;
  gap: .5rem;
  align-items: center;
  padding: 1px .35rem;
}
.col-check { width: 1.1rem }
.naplata-head .col-service,
.naplata-head .col-note,
.naplata-head .col-price { line-height: 1 }
.naplata-actions { display: flex; gap: .35rem; align-items: center }
.bill-form { display: flex; flex-direction: column; gap: .6rem }
.bill-row { display: flex; justify-content: space-between; font-size: .9rem }
.bill-field { display: flex; flex-direction: column; gap: .3rem; margin-top: .4rem }
.bill-field label { font-size: .8rem; color: var(--p-text-muted-color) }
.bill-type-select { width: 100% }
.grp-row.selected { background: var(--p-highlight-background, rgba(37, 99, 235, .06)) }
/* Shrink PrimeVue Checkbox to fit our 1.1rem column */
.naplata :deep(.p-checkbox), .naplata :deep(.p-checkbox-box) { width: .95rem; height: .95rem }
.naplata :deep(.p-checkbox-icon) { font-size: .65rem }
.clickable { cursor: pointer }
.clickable:hover { color: var(--p-primary-color) }
.naplata-head {
  font-size: .68rem; text-transform: uppercase; letter-spacing: .02em;
  color: var(--p-text-muted-color);
  border-bottom: 1px solid var(--p-content-border-color);
  padding-bottom: 3px; margin-bottom: 2px;
}
.col-service { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.col-note    { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.col-price   { text-align: right }

.naplata-group { margin-bottom: .25rem }
.grp-head {
  display: flex; align-items: baseline; justify-content: space-between; gap: .5rem;
  padding: 2px .35rem;
  background: var(--p-content-background); border-radius: 4px;
  font-weight: 600; font-size: .8rem;
  cursor: pointer;
  border-bottom: 1px solid var(--p-content-border-color);
  margin-top: 2px;
}
.grp-head:hover { background: var(--p-content-hover-background, rgba(0,0,0,.04)) }
.grp-client { flex: 1 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.grp-vehicle { flex: 0 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-weight: 500; font-size: .73rem }
.grp-total { flex: 0 0 auto }
.grp-row { padding-left: 1.25rem }
.grp-row:hover { background: var(--p-content-hover-background, rgba(0,0,0,.04)) }

.naplata-footer {
  display: flex; justify-content: flex-end; gap: .75rem; align-items: baseline;
  margin-top: .45rem; padding-top: .35rem;
  border-top: 1px solid var(--p-content-border-color);
  font-weight: 600;
}
.plate { font-family: monospace; font-weight: 600 }
.mono  { font-family: monospace }
</style>
