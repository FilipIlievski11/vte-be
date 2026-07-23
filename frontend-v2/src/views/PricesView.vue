<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import { useAuthStore } from '@/stores/auth';
import {
  PriceTrigger,
  type Paged,
  type Company,
  type PriceCatalog,
  type PriceCatalogWrite,
  type PriceCatalogLookups,
  type PriceCategoryNode,
} from '@/types';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Select from 'primevue/select';
import Checkbox from 'primevue/checkbox';
import Dialog from 'primevue/dialog';
import Tag from 'primevue/tag';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const { t } = useI18n();
const auth = useAuthStore();
const toast = useToast();
const confirm = useConfirm();

// ---- master rail (categories) ----
const categories = ref<PriceCategoryNode[]>([]);
const catSearch = ref('');
const hideEmpty = ref(true);
// selected category: undefined = "all", null = "uncategorized", number = a category id
const selectedCatId = ref<number | null | undefined>(undefined);

// ---- detail (rules in the selected category) ----
const rows = ref<PriceCatalog[]>([]);
const total = ref(0);
const loading = ref(false);

// ---- detail sub-filters ----
const ruleSearch = ref('');
const filterTrigger = ref<number | null>(null);
const filterActiveOnly = ref(true);

// ---- lookups (loaded once) ----
const lookups = ref<PriceCatalogLookups>({ triggers: [], companies: [], vehicleFields: [], categoryGroups: [] });
const companies = ref<Company[]>([]);

// ---- edit dialog ----
const dialogVisible = ref(false);
const isNew = ref(false);
const editing = ref<PriceCatalog>(blank());

function blank(): PriceCatalog {
  return {
    id: 0, code: null, name: '', basePrice: 0, vatRateId: 0,
    trigger: PriceTrigger.None,
    vehiclePaymentCategoryId: typeof selectedCatId.value === 'number' ? selectedCatId.value : null,
    communityId: null, priceCompanyId: null, paymentCategoryGroupId: null,
    vehicleField: null, parametarFrom: null, parametarTo: null,
    ageFrom: null, ageTo: null,
    vehicleCategoryFilter: null, bankAccount: null, paymentForm: null,
    active: true,
  };
}

// „возраст 11–20 год." / „возраст над 30 год." — за мета-линијата под името.
function fmtAge(from: number | null, to: number | null): string {
  if (from == null && to == null) return '';
  if (to == null) return `возраст над ${(from ?? 1) - 1} год.`;
  return `возраст ${from ?? 0}–${to} год.`;
}

// --- computed ---
const totalActiveAll = computed(() =>
  categories.value.reduce((s, c) => s + c.activeRuleCount, 0));

const filteredCats = computed(() => {
  const term = catSearch.value.trim().toLowerCase();
  return categories.value.filter(c => {
    if (c.id === null) return true; // always keep the uncategorized bucket visible
    if (hideEmpty.value && c.ruleCount === 0) return false;
    if (term && !c.name.toLowerCase().includes(term)) return false;
    return true;
  });
});

const triggerOptionsForForm = computed(() =>
  lookups.value.triggers.map(x => ({ value: x.id, label: triggerLabel(x.id) })));

const triggerFilterOptions = computed(() => [
  { value: null, label: t('prices.filter.allTriggers') },
  ...lookups.value.triggers.map(x => ({ value: x.id, label: triggerLabel(x.id) })),
]);

const vehicleFieldOptions = computed(() => [
  { value: null, label: t('prices.form.vehicleFieldNone') },
  ...lookups.value.vehicleFields.map(f => ({ value: f, label: f })),
]);

const categoryOptionsForForm = computed(() => [
  { value: null, label: t('prices.cat.uncategorized') },
  ...categories.value.filter(c => c.id !== null).map(c => ({ value: c.id, label: c.name })),
]);

const companyOptionsForForm = computed(() => [
  { value: null, label: t('prices.form.companyAll') },
  ...companies.value.map(c => ({ value: c.id, label: c.name })),
]);

function companyName(id: number | null): string {
  if (id == null) return '';
  return companies.value.find(c => c.id === id)?.name ?? `#${id}`;
}

const selectedCatName = computed(() => {
  if (selectedCatId.value === undefined) return t('prices.cat.all');
  if (selectedCatId.value === null) return t('prices.cat.uncategorized');
  return categories.value.find(c => c.id === selectedCatId.value)?.name ?? '—';
});

function triggerLabel(id: number): string {
  return t(`prices.trigger.${PriceTrigger[id]?.toLowerCase() ?? 'none'}`);
}
function catLabel(c: PriceCategoryNode): string {
  return c.id === null ? t('prices.cat.uncategorized') : c.name;
}
function isCatSelected(id: number | null | undefined): boolean {
  return selectedCatId.value === id;
}

// --- data loading ---
async function loadCategories() {
  try {
    const { data } = await api.get<PriceCategoryNode[]>('/price-catalogs/categories');
    categories.value = data;
  } catch { /* non-fatal */ }
}

async function loadRules() {
  loading.value = true;
  try {
    const params: Record<string, unknown> = { page: 1, pageSize: 500, activeOnly: filterActiveOnly.value };
    if (selectedCatId.value === null) params.uncategorized = true;
    else if (typeof selectedCatId.value === 'number') params.vehiclePaymentCategoryId = selectedCatId.value;
    if (ruleSearch.value.trim()) params.search = ruleSearch.value.trim();
    if (filterTrigger.value != null) params.trigger = filterTrigger.value;

    const { data } = await api.get<Paged<PriceCatalog>>('/price-catalogs', { params });
    rows.value = data.items ?? [];
    total.value = data.total ?? 0;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('prices.loadFailed'), detail: e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

async function loadLookups() {
  try {
    const { data } = await api.get<PriceCatalogLookups>('/price-catalogs/lookups');
    lookups.value = data;
  } catch { /* dropdowns just show empty */ }
}

async function loadCompanies() {
  try {
    const { data } = await api.get<Company[]>('/companies');
    companies.value = data;
  } catch { /* fall back to raw ids */ }
}

function selectCat(id: number | null | undefined) {
  selectedCatId.value = id;
  loadRules();
}

watch([ruleSearch, filterTrigger, filterActiveOnly], () => loadRules());

// --- edit / create ---
function openNew() {
  editing.value = blank();
  isNew.value = true;
  dialogVisible.value = true;
}
function openEdit(row: PriceCatalog) {
  editing.value = { ...row };
  isNew.value = false;
  dialogVisible.value = true;
}

async function save() {
  const e = editing.value;
  if (!e.name.trim()) {
    toast.add({ severity: 'warn', summary: t('prices.form.nameRequired'), life: 2500 });
    return;
  }
  const dto: PriceCatalogWrite = {
    code: e.code, name: e.name.trim(), basePrice: e.basePrice, vatRateId: e.vatRateId,
    trigger: e.trigger, vehiclePaymentCategoryId: e.vehiclePaymentCategoryId,
    communityId: e.communityId, priceCompanyId: e.priceCompanyId,
    paymentCategoryGroupId: e.paymentCategoryGroupId, vehicleField: e.vehicleField,
    parametarFrom: e.parametarFrom, parametarTo: e.parametarTo,
    ageFrom: e.ageFrom, ageTo: e.ageTo,
    vehicleCategoryFilter: e.vehicleCategoryFilter, bankAccount: e.bankAccount,
    paymentForm: e.paymentForm, active: e.active,
  };
  try {
    if (isNew.value) {
      const { data } = await api.post<PriceCatalog>('/price-catalogs', dto);
      toast.add({ severity: 'success', summary: t('prices.created', { n: data.name }), life: 2000 });
    } else {
      await api.put(`/price-catalogs/${e.id}`, dto);
      toast.add({ severity: 'success', summary: t('prices.updated', { n: e.name }), life: 2000 });
    }
    dialogVisible.value = false;
    await Promise.all([loadRules(), loadCategories()]);
  } catch (err: any) {
    toast.add({ severity: 'error', summary: t('prices.saveFailed'),
      detail: err?.response?.data?.error ?? err?.message, life: 4000 });
  }
}

function remove(row: PriceCatalog) {
  confirm.require({
    message: t('prices.deleteConfirm', { name: row.name }),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/price-catalogs/${row.id}`);
        toast.add({ severity: 'success', summary: t('prices.deleted', { name: row.name }), life: 2000 });
        await Promise.all([loadRules(), loadCategories()]);
      } catch (err: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'),
          detail: err?.response?.data?.error ?? err?.message, life: 4000 });
      }
    },
  });
}

function fmtMoney(v: number): string {
  return v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
function fmtRange(from: number | null, to: number | null): string {
  if (from == null && to == null) return '';
  if (from != null && to != null) return `${from}–${to}`;
  if (from != null) return `≥ ${from}`;
  return `≤ ${to}`;
}

onMounted(async () => {
  await Promise.all([loadLookups(), loadCategories(), loadCompanies()]);
  await loadRules();
});
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('prices.title') }}</h1>
      <div class="subtitle">{{ t('prices.subtitle') }}</div>
    </div>
    <Button v-if="auth.isAdmin" :label="t('prices.new')" icon="pi pi-plus" severity="primary" @click="openNew" />
  </div>

  <div class="prices-layout">
    <!-- MASTER: vehicle-payment categories -->
    <aside class="cat-rail">
      <div class="cat-search">
        <i class="pi pi-search" />
        <InputText v-model="catSearch" :placeholder="t('common.search')" />
      </div>

      <button class="cat-item" :class="{ active: isCatSelected(undefined) }" @click="selectCat(undefined)">
        <span class="cat-name"><i class="pi pi-list" /> {{ t('prices.cat.all') }}</span>
        <span class="cat-count">{{ totalActiveAll }}</span>
      </button>

      <div class="cat-scroll">
        <button v-for="c in filteredCats" :key="c.id ?? 'null'"
          class="cat-item"
          :class="{ active: isCatSelected(c.id), uncat: c.id === null, dim: c.activeRuleCount === 0 }"
          @click="selectCat(c.id)">
          <span class="cat-name" :title="catLabel(c)">
            <i v-if="c.id === null" class="pi pi-inbox" />
            {{ catLabel(c) }}
          </span>
          <span class="cat-count" :class="{ zero: c.activeRuleCount === 0 }">{{ c.activeRuleCount }}</span>
        </button>
      </div>

      <label class="hide-empty">
        <Checkbox v-model="hideEmpty" :binary="true" />
        <span>{{ t('prices.cat.hideEmpty') }}</span>
      </label>
    </aside>

    <!-- DETAIL: rules in the selected category -->
    <section class="rule-pane">
      <div class="rule-toolbar">
        <h2 class="rule-cat-title">
          {{ selectedCatName }}
          <span class="count-pill">{{ total }}</span>
        </h2>
        <span class="spacer" />
        <span class="p-input-icon-left rule-search">
          <i class="pi pi-search" />
          <InputText v-model="ruleSearch" :placeholder="t('common.search')" />
        </span>
        <Select v-model="filterTrigger" :options="triggerFilterOptions"
          optionLabel="label" optionValue="value"
          :placeholder="t('prices.filter.allTriggers')" class="trigger-filter" showClear />
        <label class="active-toggle">
          <Checkbox v-model="filterActiveOnly" :binary="true" />
          <span>{{ t('prices.filter.activeOnly') }}</span>
        </label>
      </div>

      <DataTable :value="rows" :loading="loading" dataKey="id"
        scrollable scrollHeight="flex" stripedRows size="small" class="prices-table"
        :rowClass="(r: PriceCatalog) => r.active ? '' : 'row-inactive'">
        <template #empty>
          <div class="empty-state">{{ t('prices.empty') }}</div>
        </template>

        <Column field="name" :header="t('prices.col.name')">
          <template #body="{ data }">
            <div class="name-cell">
              <div class="nm">{{ data.name }}</div>
              <div v-if="data.code || data.vehicleField || data.parametarFrom != null || data.parametarTo != null
                         || data.ageFrom != null || data.ageTo != null || data.vehicleCategoryFilter"
                   class="muted small">
                <span v-if="data.code" class="code-chip">{{ data.code }}</span>
                <span v-if="data.vehicleField">{{ data.vehicleField }}</span>
                <span v-if="fmtRange(data.parametarFrom, data.parametarTo)">
                  {{ data.vehicleField ? ' · ' : '' }}{{ fmtRange(data.parametarFrom, data.parametarTo) }}</span>
                <span v-if="fmtAge(data.ageFrom, data.ageTo)"> · {{ fmtAge(data.ageFrom, data.ageTo) }}</span>
                <span v-if="data.vehicleCategoryFilter"> · {{ data.vehicleCategoryFilter }}</span>
              </div>
            </div>
          </template>
        </Column>
        <Column :header="t('prices.col.trigger')" style="width: 9rem">
          <template #body="{ data }">
            <Tag :value="triggerLabel(data.trigger)" :severity="data.trigger === 0 ? 'secondary' : 'info'" />
          </template>
        </Column>
        <Column :header="t('prices.col.company')" style="width: 11rem">
          <template #body="{ data }">
            <span v-if="data.priceCompanyId != null" class="company-name"
                  :title="companyName(data.priceCompanyId)">
              {{ companyName(data.priceCompanyId) }}
            </span>
            <span v-else class="muted" :title="t('prices.form.companyAll')">{{ t('prices.companyAllShort') }}</span>
          </template>
        </Column>
        <Column field="basePrice" :header="t('prices.col.price')" style="width: 7rem" class="ta-right">
          <template #body="{ data }">
            <span class="mono">{{ fmtMoney(data.basePrice) }}</span>
          </template>
        </Column>
        <Column style="width: 6rem" :header="''" v-if="auth.isAdmin">
          <template #body="{ data }">
            <Button icon="pi pi-pencil" text rounded size="small" @click="openEdit(data)" />
            <Button v-if="data.active" icon="pi pi-trash" text rounded size="small"
              severity="danger" @click="remove(data)" />
          </template>
        </Column>
      </DataTable>
    </section>
  </div>

  <!-- Edit / Create dialog -->
  <Dialog v-model:visible="dialogVisible"
    :header="isNew ? t('prices.new') : t('prices.editTitle', { name: editing.name })"
    :modal="true" :style="{ width: '52rem' }">
    <div class="form-grid">
      <div class="field col-2">
        <label>{{ t('prices.form.name') }} *</label>
        <InputText v-model="editing.name" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.vehiclePaymentCategoryId') }}</label>
        <Select v-model="editing.vehiclePaymentCategoryId as any" :options="categoryOptionsForForm"
          optionLabel="label" optionValue="value" filter />
      </div>
      <div class="field">
        <label>{{ t('prices.form.code') }}</label>
        <InputText v-model="editing.code as any" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.basePrice') }} *</label>
        <InputNumber v-model="editing.basePrice" mode="decimal" :minFractionDigits="2" :maxFractionDigits="2" :min="0" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.vatRateId') }}</label>
        <InputNumber v-model="editing.vatRateId" :min="0" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.trigger') }} *</label>
        <Select v-model="editing.trigger" :options="triggerOptionsForForm" optionLabel="label" optionValue="value" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.priceCompanyId') }}</label>
        <Select v-model="editing.priceCompanyId as any" :options="companyOptionsForForm"
          optionLabel="label" optionValue="value" filter />
      </div>
      <div class="field">
        <label>{{ t('prices.form.communityId') }}</label>
        <InputNumber v-model="editing.communityId as any" :min="0" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.paymentCategoryGroupId') }}</label>
        <InputNumber v-model="editing.paymentCategoryGroupId as any" :min="0" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.vehicleField') }}</label>
        <Select v-model="editing.vehicleField as any" :options="vehicleFieldOptions"
          optionLabel="label" optionValue="value" :editable="true" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.parametarFrom') }}</label>
        <InputNumber v-model="editing.parametarFrom as any" mode="decimal" :maxFractionDigits="3" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.parametarTo') }}</label>
        <InputNumber v-model="editing.parametarTo as any" mode="decimal" :maxFractionDigits="3" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.ageFrom') }}</label>
        <InputNumber v-model="editing.ageFrom as any" :min="0" :useGrouping="false" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.ageTo') }}</label>
        <InputNumber v-model="editing.ageTo as any" :min="0" :useGrouping="false"
                     :placeholder="t('prices.form.ageToHelp')" />
      </div>
      <div class="field col-2">
        <label>{{ t('prices.form.vehicleCategoryFilter') }}</label>
        <InputText v-model="editing.vehicleCategoryFilter as any" :placeholder="t('prices.form.vehicleCategoryFilterHelp')" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.bankAccount') }}</label>
        <InputText v-model="editing.bankAccount as any" />
      </div>
      <div class="field">
        <label>{{ t('prices.form.paymentForm') }}</label>
        <InputText v-model="editing.paymentForm as any" />
      </div>
      <div class="field col-2 active-row">
        <label class="active-toggle">
          <Checkbox v-model="editing.active" :binary="true" />
          <span>{{ t('common.active') }}</span>
        </label>
      </div>
    </div>

    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="dialogVisible = false" />
      <Button :label="t('common.save')" icon="pi pi-check" severity="primary" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
.page-header { display: flex; justify-content: space-between; align-items: flex-end; margin-bottom: 1rem; }
.subtitle { color: var(--p-text-muted-color); font-size: .85rem }

/* master-detail split */
.prices-layout { display: grid; grid-template-columns: 17rem 1fr; gap: 1rem; height: calc(100vh - 12rem); min-height: 30rem; }

/* --- master rail --- */
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
.cat-item.active { background: var(--p-highlight-background, rgba(37,99,235,.12)); color: var(--p-highlight-color, inherit); font-weight: 600 }
.cat-item.uncat { font-style: italic }
.cat-item.dim { opacity: .55 }
.cat-name { flex: 1; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; display: flex; align-items: center; gap: .35rem }
.cat-name i { font-size: .72rem; opacity: .7 }
.cat-count {
  flex: 0 0 auto; min-width: 1.6rem; text-align: center;
  font-size: .72rem; font-variant-numeric: tabular-nums;
  background: var(--p-content-border-color); color: var(--p-text-color);
  border-radius: 10px; padding: .05rem .4rem;
}
.cat-item.active .cat-count { background: var(--p-primary-color); color: #fff }
.cat-count.zero { opacity: .5 }
.hide-empty { display: flex; align-items: center; gap: .4rem; font-size: .78rem; padding: .4rem .3rem 0; border-top: 1px solid var(--p-content-border-color); margin-top: .3rem; cursor: pointer; color: var(--p-text-muted-color) }

/* --- detail pane --- */
.rule-pane { display: flex; flex-direction: column; min-width: 0; }
.rule-toolbar { display: flex; align-items: center; gap: .6rem; margin-bottom: .6rem; flex-wrap: wrap }
.rule-cat-title { font-size: 1.05rem; margin: 0; display: flex; align-items: center; gap: .5rem }
.count-pill { font-size: .72rem; background: var(--p-primary-color); color: #fff; border-radius: 10px; padding: .1rem .5rem; font-weight: 600 }
.rule-toolbar .spacer { flex: 1 }
.rule-search { position: relative }
.rule-search i { position: absolute; left: .55rem; top: 50%; transform: translateY(-50%); color: var(--p-text-muted-color); font-size: .8rem }
.rule-search :deep(input) { padding-left: 1.8rem; min-width: 14rem }
.trigger-filter { min-width: 11rem }
.active-toggle { display: flex; align-items: center; gap: .4rem; cursor: pointer; user-select: none; font-size: .82rem }

.prices-table { flex: 1; min-height: 0 }
.prices-table :deep(td) { padding: .3rem .55rem }
.prices-table :deep(.row-inactive) { opacity: .5 }
.prices-table :deep(.ta-right) { text-align: right }
.name-cell { line-height: 1.2 }
.name-cell .nm { font-weight: 500 }
.small { font-size: .72rem }
.code-chip { background: var(--p-content-border-color); border-radius: 4px; padding: 0 .3rem; margin-right: .35rem; font-family: ui-monospace, monospace }
.company-name { display: inline-block; max-width: 10.5rem; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; vertical-align: bottom; font-size: .8rem }
.muted { color: var(--p-text-muted-color) }
.mono { font-variant-numeric: tabular-nums; font-family: ui-monospace, "SF Mono", Consolas, monospace }
.empty-state { text-align: center; color: var(--p-text-muted-color); padding: 2rem }

/* dialog form */
.form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: .9rem }
.form-grid .field { display: flex; flex-direction: column; gap: .25rem }
.form-grid .field.col-2 { grid-column: span 2 }
.form-grid label { font-size: .8rem; color: var(--p-text-muted-color) }
.form-grid :deep(input), .form-grid :deep(.p-inputtext), .form-grid :deep(.p-select), .form-grid :deep(.p-inputnumber-input) { width: 100% }
.active-row { padding-top: .25rem }
</style>
