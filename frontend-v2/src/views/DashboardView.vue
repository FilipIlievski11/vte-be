<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
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
import InputNumber from 'primevue/inputnumber';
import InputText from 'primevue/inputtext';
import AutoComplete from 'primevue/autocomplete';
import { printFiscalForDocument } from '@/fiscal/fiscal';
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
const requestsToday = ref(0);      // денешни барања (KPI)
const examsPassedToday = ref(0);   // денешни исправни технички прегледи (KPI)
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
  total: number;        // amount still owed (unpaid rows only)
  unpaidCount: number;
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
        unpaidCount: 0,
      };
      out.push(g);
    }
    g.rows.push(d);
    if (!d.paid) { g.total += d.price; g.unpaidCount++; }   // paid rows stay listed but don't add to "owed"
  }
  // Legacy parity: a client only appears in Наплата while it has at least one UNPAID
  // debt. Paid rows still show inside an otherwise-open client (so billing one item
  // doesn't make it vanish), but a fully-settled client (all paid) drops off entirely.
  return out.filter(g => g.unpaidCount > 0);
});

// Стандардните авто-забелешки → кликабилен чип што води до изворот (преглед/барање).
function noteRef(note: string | null): { icon: string; label: string; to: string } | null {
  const s = (note ?? '').trim();
  let m = s.match(/^по\s+технички\s+преглед\s+бр\.?\s*(\d+)$/i);
  if (m) return { icon: 'pi pi-clipboard', label: t('dashboard.naplata.noteExam', { n: m[1] }), to: `/technical-exams/${m[1]}` };
  m = s.match(/^по\s+барање\s+бр\.?\s*(\d+)$/i);
  if (m) return { icon: 'pi pi-file-edit', label: t('dashboard.naplata.noteRequest', { n: m[1] }), to: `/requests/${m[1]}` };
  return null;
}

async function refreshDebts() {
  try {
    const [list, summary] = await Promise.all([
      // Legacy parity: the Наплата panel is a ledger of ALL active charges for the
      // company (getCustomerFinancialStateList filters on Active, not Payed) — a
      // billed charge stays listed, marked Платено, instead of disappearing.
      api.get<Paged<CustomerDebtRow>>('/customer-debts', { params: { unpaidOnly: false, pageSize: 200 } }),
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
// Selection only ever applies to UNPAID rows — paid charges stay listed (legacy
// ledger) but can't be re-billed or bulk-deleted.
function groupSelectState(group: { rows: CustomerDebtRow[] }): boolean | 'indeterminate' {
  const open = group.rows.filter(r => !r.paid);
  if (open.length === 0) return false;
  const selected = open.filter(r => selectedDebtIds.value.has(r.id)).length;
  if (selected === 0) return false;
  if (selected === open.length) return true;
  return 'indeterminate';
}
function toggleGroup(group: { rows: CustomerDebtRow[] }, on: boolean) {
  const s = new Set(selectedDebtIds.value);
  for (const r of group.rows) { if (r.paid) continue; if (on) s.add(r.id); else s.delete(r.id); }
  selectedDebtIds.value = s;
}

// --- Направи сметка (bill from selected debts) ---
const billDialogVisible = ref(false);
const billTypeId = ref<number | null>(null);
const billSaving = ref(false);
interface PaymentTypeOpt { id: number; name: string; isCash: boolean; isInstallment: boolean; prefix: string | null }
const paymentTypes = ref<PaymentTypeOpt[]>([]);

// installment ("по договор") fields
const selectedBillType = computed(() => paymentTypes.value.find(x => x.id === billTypeId.value) ?? null);
const billInstallments = ref(2);
const billFirstAmount = ref<number | null>(null);
const billGuarantorName = ref('');
const billGuarantorAddress = ref('');
const billGuarantorEmbg = ref('');
watch([billTypeId, billDialogVisible], () => {
  if (selectedBillType.value?.isInstallment && billFirstAmount.value == null)
    billFirstAmount.value = Math.round(selectedTotal.value / 2);
});

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
      paymentTypes.value = data;          // includes installment ("по договор") now
    } catch { /* dropdown stays empty; dialog still opens */ }
  }
  // default: cash ("во готово" — the un-prefixed cash type)
  billTypeId.value = paymentTypes.value.find(x => x.isCash && !x.prefix)?.id
    ?? paymentTypes.value.find(x => x.isCash)?.id
    ?? paymentTypes.value[0]?.id ?? null;
  // reset installment fields
  billInstallments.value = 2;
  billFirstAmount.value = null;
  // Гарант: легаси dijGarant ги пред-пополнуваше податоците на самиот клиент
  // (операторот ги менува ако гарант е друго лице). Адресата се влече дополнително.
  const sel = debts.value.find(d => selectedDebtIds.value.has(d.id));
  billGuarantorName.value = sel?.clientName ?? '';
  billGuarantorEmbg.value = sel?.clientMB ?? '';
  billGuarantorAddress.value = '';
  billDialogVisible.value = true;
  if (sel?.clientMB) {
    try {
      const { data } = await api.get<{ items: { mb: string | null; address: string | null }[] }>(
        '/clients', { params: { q: sel.clientMB, pageSize: 1 } });
      if (data.items[0]?.mb === sel.clientMB) billGuarantorAddress.value = data.items[0]?.address ?? '';
    } catch { /* адресата останува празна */ }
  }
}

async function createBill() {
  if (!billTypeId.value) return;
  const isRati = selectedBillType.value?.isInstallment === true;
  billSaving.value = true;
  try {
    const { data } = await api.post<{ id: number; documentNumber: string; linesTotal: number; lines: number }>(
      '/payment-documents/from-debts',
      {
        debtIds: [...selectedDebtIds.value],
        paymentTypeId: billTypeId.value,
        ...(isRati ? {
          installments: billInstallments.value,
          firstInstallmentAmount: billFirstAmount.value,
          guarantorName: billGuarantorName.value || null,
          guarantorAddress: billGuarantorAddress.value || null,
          guarantorEmbg: billGuarantorEmbg.value || null,
        } : {}),
      });
    billDialogVisible.value = false;
    selectedDebtIds.value = new Set();
    await refreshDebts();
    toast.add({ severity: 'success', summary: t('dashboard.naplata.billCreated', { no: data.documentNumber }), life: 3500 });
    // Legacy parity: fiscal receipt prints right after the bill is saved (for
    // installment bills, the down payment — rata 1). Quiet attempt; the bill
    // page has a manual button.
    const fiscal = await printFiscalForDocument(data.id, false, isRati ? 1 : undefined);
    if (fiscal.status === 'printed')
      toast.add({ severity: 'success', summary: t('fiscal.printedOk'), life: 2500 });
    else if (fiscal.status === 'no-folder' || fiscal.status === 'error')
      toast.add({ severity: 'warn', summary: t('fiscal.autoPrintFailed'), life: 4500 });
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

// --- Inline уредување на цена на долг (панел + дијалог) — легаси гридот го дозволуваше ---
const editDebtId = ref<number | null>(null);
const editDebtPrice = ref<number | null>(null);
const savingDebtPrice = ref(false);
function beginEditDebt(r: CustomerDebtRow) {
  if (r.paid) return;
  editDebtId.value = r.id;
  editDebtPrice.value = r.price;
}
function cancelEditDebt() { editDebtId.value = null; editDebtPrice.value = null; }
async function saveDebtPrice() {
  if (editDebtId.value == null || editDebtPrice.value == null || savingDebtPrice.value) return;
  savingDebtPrice.value = true;
  try {
    await api.put(`/customer-debts/${editDebtId.value}/price`, { price: editDebtPrice.value });
    cancelEditDebt();
    await Promise.all([refreshDebts(), detailVisible.value ? refreshDetail() : Promise.resolve()]);
    toast.add({ severity: 'success', summary: t('payments.priceSaved'), life: 2000 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('payments.priceSaveFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { savingDebtPrice.value = false; }
}

// Изведени такси (Закон за БСП, чл. 374 ст. 1): советските групи → процентуална основа.
const DERIVATION_GROUPS: Record<number, string> = {
  27: 'sovetTp', 63: 'sovetTp', 1063: 'sovetTp',   // 1,5% од технички преглед
  86: 'sovetPt', 1086: 'sovetPt',                  // 1% од патна такса
};
function derivationLabel(r: CustomerDebtRow): string | null {
  const key = r.paymentCategoryGroupId != null ? DERIVATION_GROUPS[r.paymentCategoryGroupId] : undefined;
  return key ? t(`dashboard.naplata.derivation.${key}`) : null;
}

// --- Детали на сметка (per-client account detail: view / add / delete ставки) ---
const detailVisible = ref(false);
const detailGroup = ref<DebtClientGroup | null>(null);
const detailRows = ref<CustomerDebtRow[]>([]);
const detailLoading = ref(false);

interface PriceCatalogOpt {
  id: number; name: string; code: string | null; basePrice: number;
  parametarFrom: number | null; parametarTo: number | null;
}
const pcOptions = ref<PriceCatalogOpt[]>([]);
const pcSelected = ref<PriceCatalogOpt | null>(null);
const addPrice = ref<number | null>(null);
const addNote = ref('');
const addSaving = ref(false);

/** Same {0}/{1} parametar-range substitution the backend does for debt rows. */
function pcLabel(p: PriceCatalogOpt): string {
  let n = p.name ?? '';
  n = n.replace('{0}', p.parametarFrom != null ? String(Math.trunc(p.parametarFrom)) : '');
  n = n.replace('{1}', p.parametarTo != null ? String(Math.trunc(p.parametarTo)) : '');
  return n;
}

async function searchPriceCatalog(e: { query: string }) {
  try {
    const { data } = await api.get<Paged<PriceCatalogOpt>>('/price-catalogs', {
      params: { search: e.query, activeOnly: true, pageSize: 20 },
    });
    pcOptions.value = data.items ?? [];
  } catch { pcOptions.value = []; }
}
function onPcSelect(e: { value: PriceCatalogOpt }) {
  addPrice.value = e.value.basePrice;
}

async function openDetail(g: DebtClientGroup) {
  detailGroup.value = g;
  detailVisible.value = true;
  pcSelected.value = null; addPrice.value = null; addNote.value = '';
  await refreshDetail();
}

async function refreshDetail() {
  if (!detailGroup.value) return;
  detailLoading.value = true;
  try {
    const { data } = await api.get<Paged<CustomerDebtRow>>('/customer-debts', {
      params: { customerVehicleRelationId: detailGroup.value.relationId, unpaidOnly: false, pageSize: 200 },
    });
    detailRows.value = data.items ?? [];
  } finally { detailLoading.value = false; }
}

const detailUnpaidRows = computed(() => detailRows.value.filter(r => !r.paid));
const detailUnpaidTotal = computed(() => detailUnpaidRows.value.reduce((s, r) => s + r.price, 0));

// Ставките групирани по извор (преглед / барање / рачно) — секоја група со линк до
// изворниот документ и меѓузбир на неплатеното. Редоследот го следи редот на внес.
interface DetailSourceGroup {
  key: string; icon: string; label: string; to: string | null;
  rows: CustomerDebtRow[]; subtotal: number;
}
const detailSourceGroups = computed<DetailSourceGroup[]>(() => {
  const map = new Map<string, DetailSourceGroup>();
  for (const r of detailRows.value) {
    let key: string; let icon: string; let label: string; let to: string | null = null;
    if (r.originTechnicalExamId != null) {
      key = `te-${r.originTechnicalExamId}`;
      icon = 'pi pi-clipboard';
      label = r.note?.trim() || t('dashboard.naplata.srcExam');
      to = `/technical-exams/${r.originTechnicalExamId}`;
    } else if (r.originRequestId != null) {
      key = `rq-${r.originRequestId}`;
      icon = 'pi pi-file-edit';
      label = r.note?.trim() || t('dashboard.naplata.srcRequest');
      to = `/requests/${r.originRequestId}`;
    } else {
      key = `mn-${r.note?.trim() || ''}`;
      icon = 'pi pi-pencil';
      label = r.note?.trim() || t('dashboard.naplata.srcManual');
    }
    let g = map.get(key);
    if (!g) { g = { key, icon, label, to, rows: [], subtotal: 0 }; map.set(key, g); }
    g.rows.push(r);
    if (!r.paid) g.subtotal += r.price;
  }
  return [...map.values()];
});

// Континуирано нумерирање на редовите низ групите (изглед на фактура).
const detailRowNo = computed<Record<number, number>>(() => {
  const out: Record<number, number> = {};
  let i = 1;
  for (const g of detailSourceGroups.value) for (const r of g.rows) out[r.id] = i++;
  return out;
});

async function addDetailItem() {
  if (!detailGroup.value || !pcSelected.value || typeof pcSelected.value === 'string') return;
  addSaving.value = true;
  try {
    await api.post('/customer-debts', {
      customerVehicleRelationId: detailGroup.value.relationId,
      priceCatalogId: pcSelected.value.id,
      price: addPrice.value,
      note: addNote.value.trim() || null,
    });
    pcSelected.value = null; addPrice.value = null; addNote.value = '';
    await Promise.all([refreshDetail(), refreshDebts()]);
    toast.add({ severity: 'success', summary: t('dashboard.naplata.itemAdded'), life: 2000 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('dashboard.naplata.addFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { addSaving.value = false; }
}

function deleteDetailRow(r: CustomerDebtRow) {
  confirm.require({
    message: t('dashboard.naplata.deleteConfirm', { service: r.composedName ?? r.priceCatalogName ?? '—', price: fmtMoney(r.price) }),
    header:  t('common.confirmDelete'),
    icon:    'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/customer-debts/${r.id}`);
        await Promise.all([refreshDetail(), refreshDebts()]);
        toast.add({ severity: 'success', summary: t('dashboard.naplata.deleted'), life: 2000 });
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
      }
    },
  });
}

/** Направи сметка for everything still unpaid in the detail dialog — selects those
 * rows and reuses the existing bill dialog/flow on top. */
function billDetailGroup() {
  if (!detailGroup.value) return;
  selectedDebtIds.value = new Set(detailUnpaidRows.value.map(r => r.id));
  if (selectedDebtIds.value.size === 0) return;
  openBillDialog();
}

const rolesText = computed(() => auth.roles.length ? auth.roles.join(', ') : t('dashboard.noRole'));

function fmtDate(s: string | null): string {
  if (!s) return '—';
  return new Date(s).toLocaleDateString();
}

// Build the request-type tree (hierarchy via parentRequestTypeId), mirroring the
// legacy Контролна табла tree: 3 roots, expanded; nodes coloured by print form.
// БЕЛ образец (documentPrintId 3) — регистрационен лист. Скриен од операторите:
// белите барања остануваат во базата (постоечките записи и админ-екранот и понатаму
// ги гледаат), но не се нудат за креирање ново барање.
const BEL_PRINT_ID = 3;

function buildReqTree(types: RequestType[]) {
  const active = types.filter(t => t.active && t.documentPrintId !== BEL_PRINT_ID);
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
    // Дневните бројки се сметаат од локална полноќ на операторот (серверот е UTC).
    const today = new Date();
    const todayIso = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(today.getDate()).padStart(2, '0')}`;
    const [types, openReq, exams, stats] = await Promise.all([
      api.get<RequestType[]>('/request-types', { params: { activeOnly: true } })
        .catch(() => ({ data: [] as RequestType[] })),
      api.get<Paged<RequestListItem>>('/requests', { params: { status: 'open', pageSize: 5 } })
        .catch(() => ({ data: { items: [], total: 0, page: 1, pageSize: 5 } as Paged<RequestListItem> })),
      api.get<Paged<TechExamListItem>>('/technical-exams', { params: { sort: 'made', dir: 'desc', pageSize: 5 } })
        .catch(() => ({ data: { items: [], total: 0, page: 1, pageSize: 5 } as Paged<TechExamListItem> })),
      api.get<{ requestsToday: number; examsPassedToday: number }>('/dashboard/stats', { params: { from: todayIso } })
        .catch(() => ({ data: { requestsToday: 0, examsPassedToday: 0 } })),
      refreshDebts(),
    ]);
    buildReqTree(types.data ?? []);
    recentOpen.value = openReq.data.items ?? [];
    recentExams.value = exams.data.items ?? [];
    requestsToday.value = stats.data.requestsToday ?? 0;
    examsPassedToday.value = stats.data.examsPassedToday ?? 0;
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

  <!-- KPI лента: брз пулс на станицата -->
  <div class="stat-strip">
    <div class="stat-tile clickable-tile" @click="router.push('/requests')">
      <div class="stat-ico si-blue"><i class="pi pi-file-edit" /></div>
      <div class="stat-body">
        <div class="stat-val">{{ requestsToday }}</div>
        <div class="stat-label">{{ t('dashboard.stats.requestsToday') }}</div>
      </div>
    </div>
    <div class="stat-tile clickable-tile" @click="router.push('/technical-exams')">
      <div class="stat-ico si-amber"><i class="pi pi-clipboard" /></div>
      <div class="stat-body">
        <div class="stat-val">{{ examsPassedToday }}</div>
        <div class="stat-label">{{ t('dashboard.stats.examsPassedToday') }}</div>
      </div>
    </div>
    <div class="stat-tile">
      <div class="stat-ico si-green"><i class="pi pi-money-bill" /></div>
      <div class="stat-body">
        <div class="stat-val">{{ fmtMoney(debtTotal) }} <span class="stat-unit">ден.</span></div>
        <div class="stat-label">{{ t('dashboard.stats.debtTotal') }}</div>
      </div>
    </div>
    <div class="stat-tile">
      <div class="stat-ico si-violet"><i class="pi pi-users" /></div>
      <div class="stat-body">
        <div class="stat-val">{{ debtGroups.length }}</div>
        <div class="stat-label">{{ t('dashboard.stats.debtClients') }}</div>
      </div>
    </div>
  </div>

  <!-- Main: лева колона (ново барање + брзи акции) | Наплата -->
  <div class="dash-main-grid">
    <div class="dash-side">
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

      <!-- Брзи акции — ги полни празнината под дрвото со реални кратенки -->
      <div class="recent-card shortcuts-card">
        <div class="recent-header">
          <h2>{{ t('dashboard.shortcuts.title') }}</h2>
        </div>
        <div class="shortcut-grid">
          <button class="shortcut" @click="router.push('/international-driving-licences/new')">
            <i class="pi pi-id-card" /><span>{{ t('dashboard.newIdl') }}</span>
          </button>
          <button class="shortcut" @click="router.push('/vehicle-permissions/new')">
            <i class="pi pi-file-check" /><span>{{ t('dashboard.newPermission') }}</span>
          </button>
          <button class="shortcut" @click="router.push('/clients/new')">
            <i class="pi pi-user-plus" /><span>{{ t('dashboard.shortcuts.newClient') }}</span>
          </button>
          <button class="shortcut" @click="router.push('/vehicles/new')">
            <i class="pi pi-car" /><span>{{ t('dashboard.shortcuts.newVehicle') }}</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Наплата — open customer debts grouped by client → vehicle (legacy CustomerFinancialState) -->
    <div class="recent-card naplata-card">
      <div class="recent-header">
        <h2>
          {{ t('dashboard.naplata.title') }}
          <span v-if="debtCount" class="count-pill">{{ debtCount }}</span>
        </h2>
        <div class="naplata-actions">
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
            <span v-if="g.vehiclePlate || g.vehicleVin" class="grp-vehicle">
              <span v-if="g.vehiclePlate" class="plate-chip">{{ g.vehiclePlate }}</span>
              <span v-else class="vin muted">{{ g.vehicleVin }}</span>
              <span v-if="g.vehicleMakerModel" class="grp-mm muted">{{ g.vehicleMakerModel }}</span>
            </span>
            <span class="grp-total mono">{{ fmtMoney(g.total) }} <span class="grp-den">ден.</span></span>
            <button class="grp-detail-btn" @click.stop="openDetail(g)"
                    v-tooltip.left="t('dashboard.naplata.details')">
              <i class="pi pi-window-maximize" />
            </button>
          </div>
          <div class="grp-rows">
            <div v-for="r in g.rows" :key="r.id" class="grp-row"
                 :class="{ selected: isSelected(r.id), 'row-paid': r.paid }">
              <Checkbox v-if="!r.paid"
                :modelValue="isSelected(r.id)"
                :binary="true"
                @update:modelValue="(v: boolean) => toggleSelected(r.id, v)"
              />
              <span v-else class="paid-dot" v-tooltip.right="t('dashboard.naplata.paidTip')"><i class="pi pi-check" /></span>
              <span class="col-service" :title="r.composedName ?? r.priceCatalogName ?? ''">
                {{ r.composedName || r.priceCatalogName || '—' }}
              </span>
              <span class="col-note" :title="r.note ?? ''">
                <button v-if="noteRef(r.note)" class="note-chip"
                        @click.stop="router.push(noteRef(r.note)!.to)">
                  <i :class="noteRef(r.note)!.icon" /><span>{{ noteRef(r.note)!.label }}</span>
                </button>
                <span v-else class="muted">{{ r.note || '' }}</span>
              </span>
              <span class="col-price mono" :class="{ 'price-zero': r.price === 0 }">
                <span v-if="editDebtId === r.id" class="price-edit-wrap" @click.stop>
                  <InputNumber v-model="editDebtPrice" :minFractionDigits="2" :maxFractionDigits="2" :min="0"
                               inputClass="price-edit-input" autofocus
                               @keydown.enter="saveDebtPrice" @keydown.esc="cancelEditDebt" />
                  <Button icon="pi pi-check" size="small" text severity="success" :loading="savingDebtPrice" @click.stop="saveDebtPrice" />
                  <Button icon="pi pi-times" size="small" text severity="secondary" :disabled="savingDebtPrice" @click.stop="cancelEditDebt" />
                </span>
                <template v-else>
                  <span class="price-val" :class="{ editable: !r.paid }"
                        :title="!r.paid ? t('payments.editPriceHint') : ''"
                        @click.stop="beginEditDebt(r)">{{ fmtMoney(r.price) }}</span>
                  <Tag v-if="r.paid" :value="t('dashboard.naplata.paid')" severity="success" class="paid-tag" />
                </template>
              </span>
            </div>
          </div>
        </div>

        <!-- Постојана акциска лента: копчињата се секогаш видливи, disabled без селекција. -->
        <div class="naplata-actionbar">
          <span class="sel-hint" :class="{ faded: selectedDebtIds.size === 0 }">
            {{ selectedDebtIds.size
              ? t('dashboard.naplata.selectedHint', { n: selectedDebtIds.size, sum: fmtMoney(selectedTotal) })
              : t('dashboard.naplata.selectHint') }}
          </span>
          <Button
            :label="selectedDebtIds.size ? t('dashboard.naplata.deleteSelected', { n: selectedDebtIds.size }) : t('dashboard.naplata.deleteSelectedPlain')"
            icon="pi pi-trash" severity="danger" size="small" outlined
            :disabled="selectedDebtIds.size === 0"
            @click="deleteSelectedDebts"
          />
          <Button
            :label="selectedDebtIds.size ? t('dashboard.naplata.makeBill', { n: selectedDebtIds.size }) : t('dashboard.naplata.makeBillPlain')"
            icon="pi pi-file" severity="primary" size="small"
            :disabled="selectedDebtIds.size === 0 || selectedRelationIds.size !== 1"
            v-tooltip.top="selectedDebtIds.size > 0 && selectedRelationIds.size !== 1 ? t('dashboard.naplata.billOneClient') : undefined"
            @click="openBillDialog"
          />
        </div>
      </div>
      <div v-else-if="loading" class="empty"><i class="pi pi-spin pi-spinner" /></div>
      <div v-else class="empty">
        <i class="pi pi-check-circle" />
        <span>{{ t('dashboard.naplata.empty') }}</span>
      </div>
    </div>
  </div>

  <!-- Bottom: последна активност, една до друга -->
  <div class="dash-bottom-grid">
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
  </div>

  <!-- Детали на сметка dialog — view / add / delete ставки for one client account -->
  <Dialog v-model:visible="detailVisible" :header="t('dashboard.naplata.details')"
          modal class="debt-detail-dialog" :style="{ width: '980px', maxWidth: '96vw' }">
    <div v-if="detailGroup" class="detail-head">
      <div class="dh-id">
        <div class="dh-name">{{ detailGroup.clientName || '—' }}</div>
        <div v-if="detailGroup.clientMB" class="dh-mb mono">{{ detailGroup.clientMB }}</div>
      </div>
      <div v-if="detailGroup.vehiclePlate" class="dh-plate">
        <span class="dh-plate-band">MK</span>
        <span class="dh-plate-no">{{ detailGroup.vehiclePlate }}</span>
      </div>
      <div v-if="detailGroup.vehicleMakerModel" class="dh-veh">{{ detailGroup.vehicleMakerModel }}</div>
      <span class="dh-spacer"></span>
      <div class="dh-count muted">{{ t('dashboard.naplata.unpaidCount', detailUnpaidRows.length) }}</div>
    </div>

    <div class="detail-rows">
      <div class="detail-row detail-row-head">
        <span class="col-no">#</span>
        <span class="col-service">{{ t('dashboard.naplata.col.service') }}</span>
        <span class="col-date">{{ t('dashboard.naplata.col.date') }}</span>
        <span class="col-price">{{ t('dashboard.naplata.col.price') }}</span>
        <span class="col-x"></span>
      </div>

      <template v-for="g in detailSourceGroups" :key="g.key">
        <div class="src-head">
          <i :class="g.icon" />
          <RouterLink v-if="g.to" :to="g.to" class="src-link">{{ g.label }}</RouterLink>
          <span v-else class="src-label">{{ g.label }}</span>
          <span class="src-line"></span>
          <span v-if="g.subtotal > 0 && detailSourceGroups.length > 1" class="src-subtotal mono">
            {{ fmtMoney(g.subtotal) }}
          </span>
        </div>
        <div v-for="r in g.rows" :key="r.id" class="detail-row" :class="{ 'row-paid': r.paid }">
          <span class="col-no muted">{{ detailRowNo[r.id] }}</span>
          <span class="col-service" :title="r.composedName ?? r.priceCatalogName ?? ''">
            {{ r.composedName || r.priceCatalogName || '—' }}
            <span v-if="derivationLabel(r)" class="deriv-chip">{{ derivationLabel(r) }}</span>
          </span>
          <span class="col-date muted">{{ fmtDate(r.createdAt) }}</span>
          <span class="col-price mono">
            <span v-if="editDebtId === r.id" class="price-edit-wrap" @click.stop>
              <InputNumber v-model="editDebtPrice" :minFractionDigits="2" :maxFractionDigits="2" :min="0"
                           inputClass="price-edit-input" autofocus
                           @keydown.enter="saveDebtPrice" @keydown.esc="cancelEditDebt" />
              <Button icon="pi pi-check" size="small" text severity="success" :loading="savingDebtPrice" @click.stop="saveDebtPrice" />
              <Button icon="pi pi-times" size="small" text severity="secondary" :disabled="savingDebtPrice" @click.stop="cancelEditDebt" />
            </span>
            <template v-else>
              <span class="price-val" :class="{ editable: !r.paid }"
                    :title="!r.paid ? t('payments.editPriceHint') : ''"
                    @click.stop="beginEditDebt(r)">{{ fmtMoney(r.price) }}</span>
              <Tag v-if="r.paid" :value="t('dashboard.naplata.paid')" severity="success" class="paid-tag" />
            </template>
          </span>
          <span class="col-x">
            <button v-if="!r.paid" class="row-del" @click="deleteDetailRow(r)"
                    v-tooltip.left="t('dashboard.naplata.delete')">
              <i class="pi pi-trash" />
            </button>
          </span>
        </div>
      </template>

      <div v-if="detailLoading" class="empty"><i class="pi pi-spin pi-spinner" /></div>
      <div v-else-if="!detailRows.length" class="empty"><span>{{ t('dashboard.naplata.empty') }}</span></div>
    </div>

    <div class="detail-add">
      <span class="da-title">{{ t('dashboard.naplata.addItemTitle') }}</span>
      <div class="da-fields">
        <AutoComplete
          v-model="pcSelected"
          :suggestions="pcOptions"
          :optionLabel="(p: any) => pcLabel(p)"
          :placeholder="t('dashboard.naplata.addItemPlaceholder')"
          class="add-service"
          @complete="searchPriceCatalog"
          @option-select="onPcSelect"
        >
          <template #option="{ option }">
            <div class="pc-opt">
              <span class="pc-opt-name">{{ pcLabel(option) }}</span>
              <span class="pc-opt-price mono">{{ fmtMoney(option.basePrice) }}</span>
            </div>
          </template>
        </AutoComplete>
        <InputNumber v-model="addPrice" :min="0" :maxFractionDigits="2"
                     :placeholder="t('dashboard.naplata.col.price')" class="add-price" />
        <InputText v-model="addNote" :placeholder="t('dashboard.naplata.col.note')" class="add-note" />
        <Button icon="pi pi-plus" :label="t('dashboard.naplata.addItem')" size="small"
                :disabled="!pcSelected || typeof pcSelected === 'string' || addSaving"
                :loading="addSaving" @click="addDetailItem" />
      </div>
    </div>

    <template #footer>
      <div class="detail-footer">
        <span class="df-label">{{ t('dashboard.naplata.totalDue') }}</span>
        <span class="df-total mono">{{ fmtMoney(detailUnpaidTotal) }}<small>&nbsp;{{ t('dashboard.naplata.den') }}</small></span>
        <span class="spacer"></span>
        <Button :label="t('dashboard.naplata.makeBill', { n: detailUnpaidRows.length })"
                icon="pi pi-file"
                :disabled="detailUnpaidRows.length === 0"
                @click="billDetailGroup" />
      </div>
    </template>
  </Dialog>

  <!-- Направи сметка dialog -->
  <Dialog v-model:visible="billDialogVisible" :header="t('dashboard.naplata.makeBillTitle')"
    :modal="true" :style="{ width: selectedBillType?.isInstallment ? '32rem' : '26rem' }">
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

      <!-- Installment ("по договор") fields -->
      <template v-if="selectedBillType?.isInstallment">
        <div class="bill-divider">{{ t('dashboard.naplata.rati.section') }}</div>
        <div class="bill-grid2">
          <div class="bill-field">
            <label>{{ t('dashboard.naplata.rati.count') }}</label>
            <InputNumber v-model="billInstallments" :min="2" :max="36" showButtons />
          </div>
          <div class="bill-field">
            <label>{{ t('dashboard.naplata.rati.firstAmount') }}</label>
            <InputNumber v-model="billFirstAmount" :min="1" :max="selectedTotal" :maxFractionDigits="0"
              suffix=" ден." />
          </div>
        </div>
        <div v-if="billFirstAmount" class="bill-row rati-hint">
          <span class="muted">{{ t('dashboard.naplata.rati.remaining') }}</span>
          <span class="mono">{{ fmtMoney(selectedTotal - billFirstAmount) }} ({{ billInstallments - 1 }}×)</span>
        </div>
        <div class="bill-field">
          <label>{{ t('dashboard.naplata.rati.guarantor') }}</label>
          <InputText v-model="billGuarantorName" :placeholder="t('dashboard.naplata.rati.guarantorName')" />
        </div>
        <div class="bill-grid2">
          <div class="bill-field">
            <label>{{ t('dashboard.naplata.rati.guarantorEmbg') }}</label>
            <InputText v-model="billGuarantorEmbg" />
          </div>
          <div class="bill-field">
            <label>{{ t('dashboard.naplata.rati.guarantorAddress') }}</label>
            <InputText v-model="billGuarantorAddress" />
          </div>
        </div>
      </template>
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
/* ===== KPI лента ===== */
.stat-strip {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
  gap: .6rem;
  margin-top: 1rem;
}
.stat-tile {
  display: flex; align-items: center; gap: .6rem;
  background: var(--p-content-background);
  border: 1px solid var(--p-content-border-color);
  border-radius: 10px;
  padding: .55rem .7rem;
}
.clickable-tile { cursor: pointer; transition: border-color .15s ease, box-shadow .15s ease }
.clickable-tile:hover { border-color: color-mix(in srgb, var(--p-primary-color) 45%, var(--p-content-border-color)); box-shadow: var(--shadow-sm, 0 1px 2px rgba(0,0,0,.06)) }
.stat-ico {
  width: 34px; height: 34px; flex: 0 0 auto;
  display: grid; place-items: center;
  border-radius: 9px; font-size: .95rem;
}
.si-blue   { background: color-mix(in srgb, #2563eb 13%, transparent); color: #2563eb }
.si-amber  { background: color-mix(in srgb, #d97706 14%, transparent); color: #d97706 }
.si-green  { background: color-mix(in srgb, #059669 13%, transparent); color: #059669 }
.si-violet { background: color-mix(in srgb, #7c3aed 12%, transparent); color: #7c3aed }
.stat-body { min-width: 0 }
.stat-val {
  font-size: 1.05rem; font-weight: 700; line-height: 1.15;
  font-variant-numeric: tabular-nums;
  white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
}
.stat-unit { font-size: .68rem; font-weight: 500; color: var(--p-text-muted-color) }
.stat-label { font-size: .68rem; color: var(--p-text-muted-color); line-height: 1.2 }

/* ===== Main grid: тесна лева колона + Наплата ===== */
.dash-main-grid {
  display: grid;
  grid-template-columns: minmax(280px, 370px) minmax(0, 1fr);
  gap: 1rem;
  margin-top: 1rem;
  align-items: start;   /* картичките не се растегнуваат — нема мртов простор */
}
.dash-side { display: flex; flex-direction: column; gap: 1rem; min-width: 0 }
.dash-main-grid > .recent-card, .dash-side > .recent-card { margin-top: 0 }

/* Брзи акции */
.shortcuts-card { padding: .65rem .85rem }
.shortcuts-card .recent-header { margin-bottom: .45rem }
.shortcut-grid { display: grid; grid-template-columns: 1fr; gap: .45rem }
.shortcut {
  display: flex; align-items: center; gap: .45rem;
  min-width: 0;
  padding: .45rem .55rem;
  background: transparent;
  border: 1px solid var(--p-content-border-color);
  border-radius: 8px;
  color: var(--p-text-color);
  font-size: .76rem; font-weight: 600; text-align: left;
  cursor: pointer;
  transition: border-color .15s ease, background .15s ease;
}
.shortcut i { color: var(--p-primary-color); font-size: .85rem; flex: 0 0 auto }
.shortcut span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.shortcut:hover {
  background: color-mix(in srgb, var(--p-primary-color) 6%, transparent);
  border-color: color-mix(in srgb, var(--p-primary-color) 40%, var(--p-content-border-color));
}

/* ===== Bottom: две табели една до друга ===== */
.dash-bottom-grid {
  display: grid;
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 1rem;
  margin-top: 1rem;
  align-items: start;
}
.dash-bottom-grid > .recent-card { margin-top: 0 }

@media (max-width: 1100px) {
  .dash-main-grid { grid-template-columns: 1fr }
  .dash-bottom-grid { grid-template-columns: 1fr }
}

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
.bill-field :deep(.p-inputnumber), .bill-field :deep(.p-inputnumber-input),
.bill-field :deep(.p-inputtext) { width: 100% }
.bill-divider { margin-top: .6rem; padding-top: .5rem; border-top: 1px solid var(--p-content-border-color);
  font-size: .78rem; font-weight: 600; color: var(--p-text-muted-color); text-transform: uppercase; letter-spacing: .02em }
.bill-grid2 { display: grid; grid-template-columns: 1fr 1fr; gap: .6rem }
.rati-hint { font-size: .82rem; margin-top: -.1rem }
.grp-row.selected { background: var(--p-highlight-background, rgba(37, 99, 235, .06)) }
.grp-row.row-paid .col-service, .grp-row.row-paid .col-note { opacity: .55 }
.grp-row.row-paid .col-price { opacity: .7 }
.paid-dot { display: inline-flex; align-items: center; justify-content: center; color: var(--p-green-500, #22c55e) }
.paid-dot i { font-size: .7rem }
.paid-tag { transform: scale(.72); transform-origin: right center; vertical-align: middle }
/* Shrink PrimeVue Checkbox to fit our 1.1rem column */
.naplata :deep(.p-checkbox), .naplata :deep(.p-checkbox-box) { width: .95rem; height: .95rem }
.naplata :deep(.p-checkbox-icon) { font-size: .65rem }
.clickable { cursor: pointer }
.clickable:hover { color: var(--p-primary-color) }
.naplata-head {
  font-size: .66rem; text-transform: uppercase; letter-spacing: .04em; font-weight: 600;
  color: var(--p-text-muted-color);
  padding: 0 .55rem 4px;
  margin-bottom: 2px;
}
.col-service { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.col-note    { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.col-price   { text-align: right; font-variant-numeric: tabular-nums }

/* Секој клиент = картичка: тонирано заглавие + редови со зебра. */
.naplata-group {
  border: 1px solid var(--p-content-border-color);
  border-radius: 8px;
  margin-bottom: .45rem;
  overflow: hidden;
  background: var(--p-content-background);
}
.grp-head {
  display: flex; align-items: center; gap: .5rem;
  padding: .28rem .55rem;
  background: color-mix(in srgb, var(--p-primary-color) 5%, var(--p-content-background));
  border-bottom: 1px solid var(--p-content-border-color);
  font-weight: 600; font-size: .8rem;
  cursor: pointer;
}
.grp-head:hover { background: color-mix(in srgb, var(--p-primary-color) 9%, var(--p-content-background)) }
.grp-client { flex: 0 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.grp-vehicle {
  flex: 1 1 auto; min-width: 0; display: inline-flex; align-items: center; gap: .4rem;
  overflow: hidden; white-space: nowrap;
}
.plate-chip {
  flex: 0 0 auto;
  font-family: ui-monospace, 'Cascadia Mono', Consolas, monospace;
  font-weight: 700; font-size: .68rem; letter-spacing: .04em; line-height: 1;
  padding: 2px 5px;
  border: 1px solid var(--p-content-border-color);
  border-radius: 4px;
  background: var(--p-content-background);
  color: var(--p-text-color);
}
.grp-mm { flex: 0 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; font-weight: 500; font-size: .72rem }
.vin { font-family: ui-monospace, monospace; font-size: .7rem }
.grp-total { flex: 0 0 auto; font-weight: 700; font-variant-numeric: tabular-nums }
.grp-den { font-weight: 500; font-size: .66rem; color: var(--p-text-muted-color) }

.grp-rows { padding: 2px 0 }
.grp-row { padding-left: .55rem; padding-right: .55rem }
.grp-rows .grp-row:nth-child(even) { background: color-mix(in srgb, var(--p-text-color) 2.5%, transparent) }
.grp-row:hover { background: var(--p-content-hover-background, rgba(0,0,0,.05)) !important }
.grp-row.selected { background: var(--p-highlight-background, rgba(37, 99, 235, .08)) !important }

/* Ставката води; категоријата („за X —") се повлекува како ситен сив суфикс. */

/* Извор-чип: „Тех. преглед бр.X" / „Барање бр.X" — кликабилен, води до записот. */
.note-chip {
  display: inline-flex; align-items: center; gap: 4px; max-width: 100%;
  border: 1px solid var(--p-content-border-color); background: transparent;
  border-radius: 999px; padding: 1px 8px;
  font-size: .67rem; line-height: 1.3; color: var(--p-text-muted-color);
  cursor: pointer; overflow: hidden;
}
.note-chip span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.note-chip i { font-size: .6rem; flex: 0 0 auto }
.note-chip:hover { color: var(--p-primary-color); border-color: color-mix(in srgb, var(--p-primary-color) 45%, transparent); background: color-mix(in srgb, var(--p-primary-color) 6%, transparent) }

.price-zero { opacity: .45 }

/* Постојана акциска лента на дното од Наплата */
.naplata-actionbar {
  display: flex; justify-content: flex-end; align-items: center; gap: .5rem;
  margin-top: .5rem; padding: .35rem .5rem;
  background: color-mix(in srgb, var(--p-primary-color) 5%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-primary-color) 16%, transparent);
  border-radius: 8px;
}
.sel-hint { margin-right: auto; font-size: .72rem; font-weight: 600; color: var(--p-text-color); font-variant-numeric: tabular-nums }
.sel-hint.faded { color: var(--p-text-muted-color); font-weight: 500 }
.plate { font-family: monospace; font-weight: 600 }
.mono  { font-family: monospace }

/* Изведени такси — мала ознака „1,5% од технички преглед" / „1% од патна такса" */
.deriv-chip {
  display: inline-block; margin-left: .4rem; padding: 0 .4rem;
  font-size: .68rem; line-height: 1.35; white-space: nowrap;
  color: var(--p-primary-700, #1d4ed8);
  background: color-mix(in srgb, var(--p-primary-color) 10%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-primary-color) 25%, transparent);
  border-radius: 999px; vertical-align: 1px;
}

/* --- Inline уредување на цена на долг --- */
.price-val.editable { cursor: pointer; border-bottom: 1px dashed var(--p-surface-400) }
.price-val.editable:hover { color: var(--p-primary-600); border-bottom-color: var(--p-primary-400) }
.price-edit-wrap { display: inline-flex; align-items: center; gap: 1px }
.price-edit-wrap :deep(.price-edit-input) { width: 74px; padding: 1px 4px; text-align: right; font-family: monospace; font-size: .78rem }
.price-edit-wrap :deep(.p-button) { width: 22px; height: 22px; padding: 0 }

/* --- Детали на сметка dialog --- */
.grp-detail-btn {
  flex: 0 0 auto;
  background: none; border: 0; color: var(--p-primary-color); cursor: pointer;
  padding: 0 2px; font-size: .72rem; line-height: 1;
}
.grp-detail-btn:hover { opacity: .8 }

/* Identity band: клиент · ЕМБГ · таблица · возило · бр. неплатени */
.detail-head {
  display: flex; align-items: center; gap: .9rem; flex-wrap: wrap;
  padding: .55rem .75rem; margin-bottom: .75rem;
  background: var(--p-content-hover-background, rgba(0,0,0,.03));
  border: 1px solid var(--p-content-border-color);
  border-radius: 8px;
}
.dh-id { display: flex; flex-direction: column; gap: 1px; min-width: 0 }
.dh-name { font-weight: 700; font-size: .95rem; letter-spacing: .01em }
.dh-mb { font-size: .72rem; color: var(--p-text-muted-color); letter-spacing: .05em }
.dh-plate {
  display: inline-flex; align-items: stretch; flex: 0 0 auto;
  border: 1.5px solid #1e293b; border-radius: 4px; overflow: hidden;
  background: #fff; line-height: 1;
}
.dh-plate-band {
  background: #1d4ed8; color: #fff; font-size: .5rem; font-weight: 700;
  display: flex; align-items: flex-end; padding: 2px 3px;
}
.dh-plate-no {
  color: #111827; font-family: ui-monospace, 'Cascadia Mono', Consolas, monospace;
  font-weight: 700; font-size: .82rem; letter-spacing: .1em; padding: 4px 7px 3px;
}
.dh-veh { font-size: .82rem; color: var(--p-text-muted-color); font-weight: 600 }
.dh-spacer { flex: 1 }
.dh-count { font-size: .75rem; white-space: nowrap }

/* Ставки — фактурен изглед: нумерирани редови, групи по извор со меѓузбир */
.detail-rows { font-size: .85rem; line-height: 1.35 }
.detail-row {
  display: grid;
  grid-template-columns: 1.7rem minmax(0, 1fr) 5.4rem 8.2rem 1.7rem;
  gap: .6rem; align-items: center;
  padding: 4px .4rem;
}
.detail-row:hover:not(.detail-row-head) { background: var(--p-content-hover-background, rgba(0,0,0,.04)) }
.detail-row-head {
  font-weight: 600; font-size: .68rem; letter-spacing: .06em;
  color: var(--p-text-muted-color);
  border-bottom: 1px solid var(--p-content-border-color);
  padding-bottom: 3px;
}
.detail-row:not(.detail-row-head) { border-bottom: 1px dashed var(--p-content-border-color, rgba(0,0,0,.06)) }
.detail-row .col-no { text-align: right; font-size: .72rem; font-variant-numeric: tabular-nums }
.detail-row .col-service { overflow-wrap: anywhere; white-space: normal }
.detail-row .col-price {
  text-align: right; white-space: nowrap;
  font-variant-numeric: tabular-nums; font-weight: 600;
}
.detail-row .col-date { white-space: nowrap; font-size: .75rem }
.detail-row.row-paid { opacity: .5 }
.detail-row.row-paid .col-service { text-decoration: line-through; text-decoration-color: rgba(127,127,127,.5) }
.row-del {
  background: none; border: 0; cursor: pointer; padding: 2px;
  color: var(--p-red-500, #ef4444); font-size: .72rem; line-height: 1;
  opacity: 0; transition: opacity .12s;
}
.detail-row:hover .row-del { opacity: 1 }
.row-del:hover { opacity: .75 }

/* Група по извор (преглед/барање/рачно) */
.src-head {
  display: flex; align-items: center; gap: .45rem;
  margin-top: .55rem; padding: .2rem .4rem 3px;
  font-size: .74rem; font-weight: 600;
}
.src-head > i { font-size: .7rem; color: var(--p-primary-color) }
.src-link { color: var(--p-primary-color); text-decoration: none }
.src-link::first-letter, .src-label::first-letter { text-transform: uppercase }
.src-link:hover { text-decoration: underline }
.src-label { color: var(--p-text-color) }
.src-line { flex: 1; border-top: 1px solid var(--p-content-border-color); opacity: .7 }
.src-subtotal {
  font-size: .72rem; font-weight: 600; color: var(--p-text-muted-color);
  font-variant-numeric: tabular-nums;
}

/* Додавање услуга */
.detail-add {
  margin-top: .8rem; padding: .55rem .6rem .6rem;
  border: 1px dashed var(--p-content-border-color);
  border-radius: 8px;
}
.da-title {
  display: block; font-size: .68rem; font-weight: 600; letter-spacing: .06em;
  text-transform: uppercase; color: var(--p-text-muted-color); margin-bottom: .35rem;
}
.da-fields { display: flex; gap: .4rem; align-items: center }
.detail-add .add-service { flex: 1 1 auto; min-width: 0 }
.detail-add .add-service :deep(input) { width: 100%; font-size: .78rem }
.detail-add .add-price { width: 6.5rem }
.detail-add .add-price :deep(input) { font-size: .78rem }
.detail-add .add-note { width: 9rem; font-size: .78rem }
.pc-opt { display: flex; justify-content: space-between; gap: 1rem; font-size: .78rem; max-width: 480px }
.pc-opt-name { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.pc-opt-price { flex: 0 0 auto }

/* Footer: истакнат вкупен износ + CTA */
.detail-footer {
  display: flex; align-items: center; gap: .6rem; width: 100%;
  padding-top: .1rem;
}
.df-label { font-size: .78rem; color: var(--p-text-muted-color); font-weight: 600 }
.df-total {
  font-size: 1.25rem; font-weight: 700; letter-spacing: .01em;
  font-variant-numeric: tabular-nums;
}
.df-total small { font-size: .72rem; font-weight: 500; color: var(--p-text-muted-color) }
.detail-footer .spacer { flex: 1 }
</style>
