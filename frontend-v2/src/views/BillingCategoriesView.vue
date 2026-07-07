<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import type { Paged, PriceCatalog } from '@/types';
import Button from 'primevue/button';
import Checkbox from 'primevue/checkbox';
import Column from 'primevue/column';
import DataTable from 'primevue/datatable';
import Dialog from 'primevue/dialog';
import InputNumber from 'primevue/inputnumber';
import InputText from 'primevue/inputtext';
import Select from 'primevue/select';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';

const { t } = useI18n();
const auth = useAuthStore();
const toast = useToast();

interface GroupRow {
  id: number; name: string; calculationItemId: number | null; companyScope: number; active: boolean;
  ruleCount: number; activeRuleCount: number;
  accountName: string | null; bankAccount: string | null; bank: string | null; form: string | null;
}
interface CalcItem {
  id: number; name: string; bankAccount: string | null; bank: string | null; form: string | null;
  active: boolean; categoryCount: number;
}

// ---------- master rail ----------
const groups = ref<GroupRow[]>([]);
const catSearch = ref('');
const hideEmpty = ref(true);
// Legacy repeats every category once per firm the station operated under (scope 2/3 =
// old firms, 4 = АВТО-БЕЗБЕДНОСТ, 0 = global). Old-firm categories only hold historical
// rules (they never fire for the current tenant) — hidden by default.
const showAllScopes = ref(false);
const selectedId = ref<number | null>(null);

const tenantScope = computed(() => auth.companyId ?? 4);
const isRelevantScope = (g: GroupRow) =>
  g.id === 0 || g.companyScope === 0 || g.companyScope === tenantScope.value;

const filteredGroups = computed(() => {
  const q = catSearch.value.trim().toLowerCase();
  return groups.value.filter(g => {
    if (!showAllScopes.value && !isRelevantScope(g)) return false;
    if (hideEmpty.value && g.activeRuleCount === 0 && g.id !== 0) return false;
    if (q && !(g.name.toLowerCase().includes(q) || String(g.id).includes(q))) return false;
    return true;
  });
});
const selected = computed(() => groups.value.find(g => g.id === selectedId.value) ?? null);
const groupLabel = (g: GroupRow) => g.id === 0 ? t('billingCats.uncategorized') : g.name;

async function loadGroups(keepSelection = false) {
  const { data } = await api.get<GroupRow[]>('/payment-category-groups');
  // rail order: uncategorized bucket first, then by active-rule count desc, then name
  const bucket = data.filter(g => g.id === 0);
  const rest = data.filter(g => g.id !== 0)
    .sort((a, b) => (b.activeRuleCount - a.activeRuleCount) || a.name.localeCompare(b.name, 'mk'));
  groups.value = [...bucket, ...rest];
  if (!keepSelection || !groups.value.some(g => g.id === selectedId.value))
    selectedId.value = rest.find(g => g.activeRuleCount > 0 && isRelevantScope(g))?.id
      ?? bucket[0]?.id ?? null;
}

// ---------- money destination (уплатна сметка) ----------
const calcItems = ref<CalcItem[]>([]);
const calcOptions = computed(() => [
  { value: null as number | null, label: t('billingCats.noAccount') },
  ...calcItems.value.filter(c => c.active).map(c => ({
    value: c.id as number | null,
    label: `${c.name}${c.bankAccount ? ` · ${c.bankAccount}` : ''}`,
  })),
]);
const editCalcItemId = ref<number | null>(null);
watch(selected, g => { editCalcItemId.value = g?.calculationItemId ?? null; }, { immediate: true });
const selectedCalc = computed(() =>
  calcItems.value.find(c => c.id === editCalcItemId.value) ?? null);
const moneyDirty = computed(() =>
  !!selected.value && selected.value.id !== 0 && editCalcItemId.value !== selected.value.calculationItemId);

async function loadCalcItems() {
  const { data } = await api.get<CalcItem[]>('/calculation-items');
  calcItems.value = data;
}

async function saveMoneyDestination() {
  const g = selected.value;
  if (!g || g.id === 0) return;
  try {
    await api.put(`/payment-category-groups/${g.id}`, {
      name: g.name, calculationItemId: editCalcItemId.value, active: g.active,
    });
    toast.add({ severity: 'success', summary: t('billingCats.moneySaved'), life: 2200 });
    await loadGroups(true);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}

// ---------- category rename / active ----------
const catDialogVisible = ref(false);
const catEditName = ref('');
const catEditActive = ref(true);
function openCatEdit() {
  const g = selected.value;
  if (!g || g.id === 0) return;
  catEditName.value = g.name;
  catEditActive.value = g.active;
  catDialogVisible.value = true;
}
async function saveCatEdit() {
  const g = selected.value;
  if (!g) return;
  try {
    await api.put(`/payment-category-groups/${g.id}`, {
      name: catEditName.value.trim(), calculationItemId: g.calculationItemId, active: catEditActive.value,
    });
    catDialogVisible.value = false;
    toast.add({ severity: 'success', summary: t('common.saved'), life: 2000 });
    await loadGroups(true);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}

// ---------- rules of the selected category ----------
const rules = ref<PriceCatalog[]>([]);
const rulesTotal = ref(0);
const rulesLoading = ref(false);
const ruleSearch = ref('');
const activeOnly = ref(true);

function ruleName(r: PriceCatalog): string {
  let n = r.name ?? '';
  n = n.replace('{0}', r.parametarFrom != null ? String(Math.trunc(r.parametarFrom)) : '');
  n = n.replace('{1}', r.parametarTo != null ? String(Math.trunc(r.parametarTo)) : '');
  return n;
}
function fmtMoney(v: number | null | undefined): string {
  return v == null ? '—' : v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
const triggerNames: Record<number, string> = {
  0: 'Нема', 1: 'Тех. преглед', 2: 'Барање', 3: 'Сообраќајна', 4: 'Полномошно', 5: 'МВД', 6: 'Нередовен ТП',
};

let rulesReq = 0;
async function loadRules() {
  if (selectedId.value == null) { rules.value = []; rulesTotal.value = 0; return; }
  const my = ++rulesReq;
  rulesLoading.value = true;
  try {
    const params: Record<string, unknown> = { pageSize: 500, activeOnly: activeOnly.value };
    if (selectedId.value === 0) params.uncategorized = true;
    else params.paymentCategoryGroupId = selectedId.value;
    if (ruleSearch.value.trim()) params.search = ruleSearch.value.trim();
    const { data } = await api.get<Paged<PriceCatalog>>('/price-catalogs', { params });
    if (my !== rulesReq) return;
    rules.value = data.items ?? [];
    rulesTotal.value = data.total ?? rules.value.length;
  } finally { if (my === rulesReq) rulesLoading.value = false; }
}
watch([selectedId, activeOnly], loadRules);
let searchTimer: ReturnType<typeof setTimeout> | undefined;
watch(ruleSearch, () => { clearTimeout(searchTimer); searchTimer = setTimeout(loadRules, 350); });

// ---------- rule edit (price / VAT / active) ----------
const ruleDialogVisible = ref(false);
const editingRule = ref<PriceCatalog | null>(null);
const ruleName_ = ref('');
const rulePrice = ref<number | null>(null);
const ruleActive = ref(true);
function openRuleEdit(r: PriceCatalog) {
  editingRule.value = r;
  ruleName_.value = r.name;
  rulePrice.value = r.basePrice;
  ruleActive.value = r.active;
  ruleDialogVisible.value = true;
}
async function saveRule() {
  const r = editingRule.value;
  if (!r) return;
  try {
    await api.put(`/price-catalogs/${r.id}`, {
      // full write DTO — unchanged fields passed back as-is
      code: r.code, name: ruleName_.value.trim(), basePrice: rulePrice.value ?? 0,
      vatRateId: r.vatRateId, trigger: r.trigger,
      vehiclePaymentCategoryId: r.vehiclePaymentCategoryId, communityId: r.communityId,
      priceCompanyId: r.priceCompanyId, paymentCategoryGroupId: r.paymentCategoryGroupId,
      vehicleField: r.vehicleField, parametarFrom: r.parametarFrom, parametarTo: r.parametarTo,
      vehicleCategoryFilter: r.vehicleCategoryFilter, bankAccount: r.bankAccount,
      paymentForm: r.paymentForm, active: ruleActive.value,
    });
    ruleDialogVisible.value = false;
    toast.add({ severity: 'success', summary: t('common.saved'), life: 2000 });
    await Promise.all([loadRules(), loadGroups(true)]);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}

// ---------- уплатни сметки CRUD dialog ----------
const accountsDialogVisible = ref(false);
const accEditing = ref<CalcItem | null>(null);   // null = list mode
const accIsNew = ref(false);
const accName = ref(''); const accAccount = ref(''); const accBank = ref(''); const accForm = ref('');
const accActive = ref(true);
function openAccounts() { accEditing.value = null; accountsDialogVisible.value = true; }
function accStartEdit(c: CalcItem | null) {
  accIsNew.value = c == null;
  accEditing.value = c ?? { id: 0, name: '', bankAccount: '', bank: '', form: '', active: true, categoryCount: 0 };
  accName.value = c?.name ?? '';
  accAccount.value = c?.bankAccount ?? '';
  accBank.value = c?.bank ?? '';
  accForm.value = c?.form ?? '';
  accActive.value = c?.active ?? true;
}
async function accSave() {
  const body = {
    name: accName.value.trim(), bankAccount: accAccount.value.trim() || null,
    bank: accBank.value.trim() || null, form: accForm.value.trim() || null, active: accActive.value,
  };
  try {
    if (accIsNew.value) await api.post('/calculation-items', body);
    else await api.put(`/calculation-items/${accEditing.value!.id}`, body);
    accEditing.value = null;
    toast.add({ severity: 'success', summary: t('common.saved'), life: 2000 });
    await Promise.all([loadCalcItems(), loadGroups(true)]);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('common.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  }
}

onMounted(async () => {
  await Promise.all([loadGroups(), loadCalcItems()]);
  await loadRules();
});
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('billingCats.title') }}</h1>
      <div class="subtitle">{{ t('billingCats.subtitle') }}</div>
    </div>
    <Button :label="t('billingCats.accounts')" icon="pi pi-credit-card" severity="secondary" outlined @click="openAccounts" />
  </div>

  <div class="bc-layout">
    <!-- MASTER: категории на наплата -->
    <aside class="cat-rail">
      <div class="cat-search">
        <i class="pi pi-search" />
        <InputText v-model="catSearch" :placeholder="t('common.search')" />
      </div>
      <div class="cat-scroll">
        <button v-for="g in filteredGroups" :key="g.id"
          class="cat-item"
          :class="{ active: g.id === selectedId, uncat: g.id === 0, dim: g.activeRuleCount === 0, inactive: !g.active }"
          @click="selectedId = g.id">
          <span class="cat-name" :title="`${groupLabel(g)} (#${g.id})`">
            <i v-if="g.id === 0" class="pi pi-inbox" />
            <i v-else-if="g.calculationItemId" class="pi pi-credit-card acc-ico" v-tooltip.right="g.bankAccount ?? ''" />
            {{ groupLabel(g) }}
          </span>
          <span v-if="showAllScopes && !isRelevantScope(g)" class="scope-chip"
                v-tooltip.left="t('billingCats.oldFirmTip')">Ф{{ g.companyScope }}</span>
          <span class="cat-count" :class="{ zero: g.activeRuleCount === 0 }">{{ g.activeRuleCount }}</span>
        </button>
      </div>
      <label class="hide-empty">
        <Checkbox v-model="hideEmpty" :binary="true" />
        <span>{{ t('billingCats.hideEmpty') }}</span>
      </label>
      <label class="hide-empty">
        <Checkbox v-model="showAllScopes" :binary="true" />
        <span>{{ t('billingCats.showAllScopes') }}</span>
      </label>
    </aside>

    <!-- DETAIL -->
    <section class="detail-pane" v-if="selected">
      <div class="detail-head">
        <h2 class="detail-title">
          {{ groupLabel(selected) }}
          <span v-if="selected.id !== 0" class="id-chip">#{{ selected.id }}</span>
          <span class="count-pill">{{ rulesTotal }}</span>
          <Tag v-if="!selected.active" :value="t('billingCats.inactiveCat')" severity="warn" />
        </h2>
        <Button v-if="auth.isAdmin && selected.id !== 0" icon="pi pi-pencil" text rounded size="small"
                v-tooltip.bottom="t('billingCats.editCat')" @click="openCatEdit" />
      </div>

      <!-- Каде одат парите -->
      <div v-if="selected.id !== 0" class="money-card">
        <div class="money-label"><i class="pi pi-arrow-circle-right" /> {{ t('billingCats.moneyGoesTo') }}</div>
        <div class="money-body">
          <Select v-model="editCalcItemId" :options="calcOptions" optionLabel="label" optionValue="value"
                  :disabled="!auth.isAdmin" filter size="small" class="money-select"
                  :placeholder="t('billingCats.noAccount')" />
          <Button v-if="moneyDirty" :label="t('common.save')" icon="pi pi-check" size="small" @click="saveMoneyDestination" />
        </div>
        <div v-if="selectedCalc" class="money-meta">
          <span class="mono acc-no">{{ selectedCalc.bankAccount ?? '—' }}</span>
          <span v-if="selectedCalc.form" class="form-chip">{{ selectedCalc.form }}</span>
          <span class="muted">{{ selectedCalc.bank }}</span>
        </div>
        <div v-else class="money-meta muted">{{ t('billingCats.noAccountHint') }}</div>
      </div>

      <div class="rules-toolbar">
        <span class="p-input-icon-left rule-search">
          <i class="pi pi-search" />
          <InputText v-model="ruleSearch" :placeholder="t('common.search')" />
        </span>
        <label class="active-toggle">
          <Checkbox v-model="activeOnly" :binary="true" />
          <span>{{ t('billingCats.activeOnly') }}</span>
        </label>
      </div>

      <DataTable :value="rules" :loading="rulesLoading" dataKey="id"
        scrollable scrollHeight="flex" stripedRows size="small" class="rules-table"
        :rowClass="(r: PriceCatalog) => r.active ? '' : 'row-inactive'">
        <template #empty><div class="empty-state">{{ t('billingCats.noRules') }}</div></template>
        <Column :header="t('billingCats.col.name')">
          <template #body="{ data }">
            <div class="name-cell">
              <div class="nm">{{ ruleName(data) }}</div>
              <div v-if="data.code" class="muted small"><span class="code-chip">{{ data.code }}</span></div>
            </div>
          </template>
        </Column>
        <Column :header="t('billingCats.col.trigger')" style="width: 8.5rem">
          <template #body="{ data }">
            <Tag :value="triggerNames[data.trigger] ?? data.trigger" :severity="data.trigger === 0 ? 'secondary' : 'info'" />
          </template>
        </Column>
        <Column :header="t('billingCats.col.price')" style="width: 7.5rem" class="ta-right">
          <template #body="{ data }"><span class="mono">{{ fmtMoney(data.basePrice) }}</span></template>
        </Column>
        <Column v-if="auth.isAdmin" style="width: 4rem">
          <template #body="{ data }">
            <Button icon="pi pi-pencil" text rounded size="small" @click="openRuleEdit(data)" />
          </template>
        </Column>
      </DataTable>
    </section>
  </div>

  <!-- Category rename/active dialog -->
  <Dialog v-model:visible="catDialogVisible" :header="t('billingCats.editCat')" modal :style="{ width: '26rem' }">
    <div class="dlg-grid">
      <div class="field">
        <label>{{ t('billingCats.catName') }} *</label>
        <InputText v-model="catEditName" />
      </div>
      <label class="active-toggle">
        <Checkbox v-model="catEditActive" :binary="true" />
        <span>{{ t('common.active') }}</span>
      </label>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="catDialogVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" @click="saveCatEdit" />
    </template>
  </Dialog>

  <!-- Rule edit dialog -->
  <Dialog v-model:visible="ruleDialogVisible" :header="t('billingCats.editRule')" modal :style="{ width: '30rem' }">
    <div class="dlg-grid">
      <div class="field">
        <label>{{ t('billingCats.col.name') }} *</label>
        <InputText v-model="ruleName_" />
      </div>
      <div class="field">
        <label>{{ t('billingCats.col.price') }} *</label>
        <InputNumber v-model="rulePrice" mode="decimal" :minFractionDigits="2" :maxFractionDigits="2" :min="0" />
      </div>
      <label class="active-toggle">
        <Checkbox v-model="ruleActive" :binary="true" />
        <span>{{ t('common.active') }}</span>
      </label>
    </div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="ruleDialogVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" @click="saveRule" />
    </template>
  </Dialog>

  <!-- Уплатни сметки dialog -->
  <Dialog v-model:visible="accountsDialogVisible" :header="t('billingCats.accounts')" modal :style="{ width: '46rem' }">
    <template v-if="!accEditing">
      <div class="acc-list">
        <div v-for="c in calcItems" :key="c.id" class="acc-row" :class="{ 'row-inactive': !c.active }">
          <div class="acc-main">
            <div class="acc-name">{{ c.name }}</div>
            <div class="acc-sub">
              <span class="mono acc-no">{{ c.bankAccount ?? '—' }}</span>
              <span v-if="c.form" class="form-chip">{{ c.form }}</span>
              <span class="muted">{{ c.bank }}</span>
            </div>
          </div>
          <span class="usage muted" v-tooltip.left="t('billingCats.usedBy')">{{ c.categoryCount }}</span>
          <Button v-if="auth.isAdmin" icon="pi pi-pencil" text rounded size="small" @click="accStartEdit(c)" />
        </div>
      </div>
      <div class="acc-actions" v-if="auth.isAdmin">
        <Button :label="t('billingCats.newAccount')" icon="pi pi-plus" size="small" @click="accStartEdit(null)" />
      </div>
    </template>
    <template v-else>
      <div class="dlg-grid">
        <div class="field">
          <label>{{ t('billingCats.accName') }} *</label>
          <InputText v-model="accName" />
        </div>
        <div class="field">
          <label>{{ t('billingCats.accNumber') }}</label>
          <InputText v-model="accAccount" class="mono" />
        </div>
        <div class="field">
          <label>{{ t('billingCats.accBank') }}</label>
          <InputText v-model="accBank" />
        </div>
        <div class="field">
          <label>{{ t('billingCats.accForm') }}</label>
          <InputText v-model="accForm" placeholder="ПП30 / ПП50" />
        </div>
        <label class="active-toggle">
          <Checkbox v-model="accActive" :binary="true" />
          <span>{{ t('common.active') }}</span>
        </label>
      </div>
      <div class="acc-actions">
        <Button :label="t('common.cancel')" severity="secondary" outlined size="small" @click="accEditing = null" />
        <Button :label="t('common.save')" icon="pi pi-check" size="small" @click="accSave" />
      </div>
    </template>
  </Dialog>
</template>

<style scoped>
.page-header { display: flex; justify-content: space-between; align-items: flex-end; margin-bottom: 1rem; }
.subtitle { color: var(--p-text-muted-color); font-size: .85rem }

.bc-layout { display: grid; grid-template-columns: 19rem 1fr; gap: 1rem; height: calc(100vh - 12rem); min-height: 30rem; }

/* --- master rail (same look as /prices) --- */
.cat-rail {
  display: flex; flex-direction: column;
  background: var(--p-content-background);
  border: 1px solid var(--p-content-border-color); border-radius: 10px;
  padding: .5rem; overflow: hidden;
}
.cat-search { position: relative; margin-bottom: .4rem }
.cat-search i { position: absolute; left: .55rem; top: 50%; transform: translateY(-50%); color: var(--p-text-muted-color); font-size: .8rem }
.cat-search :deep(input) { width: 100%; padding-left: 1.8rem; font-size: .82rem }
.cat-scroll { overflow-y: auto; flex: 1; margin: .15rem -.2rem; padding: 0 .2rem }
.cat-item {
  width: 100%; display: flex; align-items: center; gap: .5rem;
  background: none; border: 0; border-radius: 6px;
  padding: .32rem .5rem; cursor: pointer; text-align: left;
  font-size: .82rem; color: var(--p-text-color); line-height: 1.2;
}
.cat-item:hover { background: var(--p-content-hover-background, rgba(0,0,0,.04)) }
.cat-item.active { background: var(--p-highlight-background, rgba(37,99,235,.12)); font-weight: 600 }
.cat-item.uncat { font-style: italic }
.cat-item.dim { opacity: .55 }
.cat-item.inactive .cat-name { text-decoration: line-through; opacity: .6 }
.cat-name { flex: 1; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; display: flex; align-items: center; gap: .35rem }
.cat-name i { font-size: .72rem; opacity: .7 }
.acc-ico { color: var(--p-primary-color); opacity: 1 !important }
.cat-count {
  flex: 0 0 auto; min-width: 1.6rem; text-align: center;
  font-size: .72rem; font-variant-numeric: tabular-nums;
  background: var(--p-content-border-color);
  border-radius: 10px; padding: .05rem .4rem;
}
.scope-chip {
  flex: 0 0 auto; font-size: .62rem; font-weight: 700;
  color: #b45309; background: color-mix(in srgb, #f59e0b 16%, transparent);
  border-radius: 4px; padding: 0 .3rem;
}
.cat-item.active .cat-count { background: var(--p-primary-color); color: #fff }
.cat-count.zero { opacity: .5 }
.hide-empty { display: flex; align-items: center; gap: .4rem; font-size: .78rem; padding: .4rem .3rem 0; border-top: 1px solid var(--p-content-border-color); margin-top: .3rem; cursor: pointer; color: var(--p-text-muted-color) }

/* --- detail --- */
.detail-pane { display: flex; flex-direction: column; min-width: 0 }
.detail-head { display: flex; align-items: center; gap: .4rem; margin-bottom: .5rem }
.detail-title { font-size: 1.05rem; margin: 0; display: flex; align-items: center; gap: .5rem; min-width: 0 }
.id-chip { font-size: .7rem; color: var(--p-text-muted-color); font-weight: 400 }
.count-pill { font-size: .72rem; background: var(--p-primary-color); color: #fff; border-radius: 10px; padding: .1rem .5rem; font-weight: 600 }

.money-card {
  border: 1px solid color-mix(in srgb, var(--p-primary-color) 25%, transparent);
  background: color-mix(in srgb, var(--p-primary-color) 6%, transparent);
  border-radius: 10px; padding: .55rem .75rem; margin-bottom: .6rem;
}
.money-label { font-size: .72rem; font-weight: 700; letter-spacing: .03em; text-transform: uppercase; color: var(--p-primary-color); display: flex; align-items: center; gap: .35rem; margin-bottom: .35rem }
.money-body { display: flex; align-items: center; gap: .5rem }
.money-select { flex: 1; max-width: 34rem }
.money-meta { display: flex; align-items: center; gap: .6rem; margin-top: .35rem; font-size: .78rem; flex-wrap: wrap }
.acc-no { font-weight: 600 }
.form-chip { background: var(--p-content-border-color); border-radius: 4px; padding: 0 .35rem; font-size: .72rem; font-weight: 600 }

.rules-toolbar { display: flex; align-items: center; gap: .6rem; margin-bottom: .5rem }
.rule-search { position: relative }
.rule-search i { position: absolute; left: .55rem; top: 50%; transform: translateY(-50%); color: var(--p-text-muted-color); font-size: .8rem }
.rule-search :deep(input) { padding-left: 1.8rem; min-width: 14rem }
.active-toggle { display: flex; align-items: center; gap: .4rem; cursor: pointer; user-select: none; font-size: .82rem }

.rules-table { flex: 1; min-height: 0 }
.rules-table :deep(td) { padding: .3rem .55rem }
.rules-table :deep(.row-inactive) { opacity: .5 }
.rules-table :deep(.ta-right) { text-align: right }
.name-cell { line-height: 1.2 }
.name-cell .nm { font-weight: 500 }
.small { font-size: .72rem }
.code-chip { background: var(--p-content-border-color); border-radius: 4px; padding: 0 .3rem; font-family: ui-monospace, monospace }
.muted { color: var(--p-text-muted-color) }
.mono { font-variant-numeric: tabular-nums; font-family: ui-monospace, "SF Mono", Consolas, monospace }
.empty-state { text-align: center; color: var(--p-text-muted-color); padding: 2rem }

/* dialogs */
.dlg-grid { display: flex; flex-direction: column; gap: .8rem }
.dlg-grid .field { display: flex; flex-direction: column; gap: .25rem }
.dlg-grid label { font-size: .8rem; color: var(--p-text-muted-color) }
.dlg-grid :deep(input), .dlg-grid :deep(.p-inputnumber-input) { width: 100% }

.acc-list { display: flex; flex-direction: column; gap: .15rem; max-height: 24rem; overflow-y: auto }
.acc-row { display: flex; align-items: center; gap: .6rem; padding: .4rem .5rem; border-radius: 8px }
.acc-row:hover { background: var(--p-content-hover-background, rgba(0,0,0,.04)) }
.acc-row.row-inactive { opacity: .5 }
.acc-main { flex: 1; min-width: 0 }
.acc-name { font-size: .85rem; font-weight: 600; line-height: 1.25 }
.acc-sub { display: flex; align-items: center; gap: .5rem; font-size: .75rem; flex-wrap: wrap }
.usage { font-size: .75rem; font-variant-numeric: tabular-nums }
.acc-actions { display: flex; justify-content: flex-end; gap: .5rem; margin-top: .8rem }
</style>
