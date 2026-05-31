<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import api from '@/api/client'
import { FilterMatchMode } from '@primevue/core/api'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import DatePicker from 'primevue/datepicker'
import Select from 'primevue/select'
import Checkbox from 'primevue/checkbox'
import Tag from 'primevue/tag'

interface DTO { id: number; name: string; price: number; calculationItemId: number | null; ddvId: number | null; effectiveFrom: string | null; effectiveTo: string | null; isActive: boolean }
interface Calc { id: number; itemName: string }
interface DDV  { id: number; name: string; rate: number }

const { t } = useI18n()
const auth = useAuthStore()
const rows = ref<DTO[]>([])
const calcItems = ref<Calc[]>([])
const ddvs = ref<DDV[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const editing = ref<{ id: number | null; name: string; price: number; calculationItemId: number | null; ddvId: number | null; effectiveFrom: Date | null; effectiveTo: Date | null; isActive: boolean }>({ id: null, name: '', price: 0, calculationItemId: null, ddvId: null, effectiveFrom: null, effectiveTo: null, isActive: true })
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

const calcOptions = computed(() => calcItems.value.map(c => ({ id: c.id, name: c.itemName })))
const ddvOptions  = computed(() => ddvs.value.map(d => ({ id: d.id, name: `${d.name} (${d.rate}%)` })))

async function load() {
  loading.value = true
  const [a, b, c] = await Promise.all([api.get<DTO[]>('/price-catalog'), api.get<Calc[]>('/calculation-items'), api.get<DDV[]>('/ddv-catalog')])
  rows.value = a.data; calcItems.value = b.data; ddvs.value = c.data
  loading.value = false
}

function newRow()  { editing.value = { id: null, name: '', price: 0, calculationItemId: null, ddvId: null, effectiveFrom: null, effectiveTo: null, isActive: true }; error.value = null; showDialog.value = true }
function editRow(r: DTO) {
  editing.value = { id: r.id, name: r.name, price: r.price, calculationItemId: r.calculationItemId, ddvId: r.ddvId,
    effectiveFrom: r.effectiveFrom ? new Date(r.effectiveFrom) : null,
    effectiveTo:   r.effectiveTo   ? new Date(r.effectiveTo)   : null, isActive: r.isActive }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    const body = {
      name: editing.value.name, price: editing.value.price,
      calculationItemId: editing.value.calculationItemId, ddvId: editing.value.ddvId,
      effectiveFrom: editing.value.effectiveFrom ? editing.value.effectiveFrom.toISOString().slice(0,10) : null,
      effectiveTo:   editing.value.effectiveTo   ? editing.value.effectiveTo.toISOString().slice(0,10)   : null,
      isActive: editing.value.isActive,
    }
    if (editing.value.id == null) await api.post('/price-catalog', body)
    else await api.put(`/price-catalog/${editing.value.id}`, body)
    showDialog.value = false; await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('menu.priceCatalog') }}</h1><div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div></div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button v-if="auth.isAdministrator" :label="t('common.add')" icon="pi pi-plus" size="small" @click="newRow" />
    </div>
  </div>
  <DataTable :value="rows" :loading="loading" stripedRows
             v-model:filters="filters" :globalFilterFields="['name']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="name" :header="t('common.name')" sortable />
    <Column field="price" :header="t('price.price')" sortable>
      <template #body="{ data }"><strong>{{ data.price.toFixed(2) }}</strong></template>
    </Column>
    <Column field="effectiveFrom" :header="t('price.from')" />
    <Column field="effectiveTo"   :header="t('price.to')" />
    <Column :header="t('common.status')" style="width: 130px">
      <template #body="{ data }"><Tag :value="data.isActive ? t('common.active') : t('common.inactive')" :severity="data.isActive ? 'success' : 'secondary'" /></template>
    </Column>
    <Column :header="t('common.actions')" style="width: 100px" v-if="auth.isAdministrator">
      <template #body="{ data }"><Button icon="pi pi-pencil" text @click="editRow(data)" /></template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="editing.id == null ? t('common.add') : t('common.edit')" modal :style="{ width: '480px' }">
    <div class="field"><label>{{ t('common.name') }} *</label><InputText v-model="editing.name" maxlength="200" /></div>
    <div class="row">
      <div class="field"><label>{{ t('price.price') }} *</label><InputNumber v-model="editing.price" :minFractionDigits="2" /></div>
      <div class="field"><label>{{ t('price.ddv') }}</label>
        <Select v-model="editing.ddvId" :options="ddvOptions" optionLabel="name" optionValue="id" :placeholder="'—'" showClear />
      </div>
    </div>
    <div class="field"><label>{{ t('price.calcItem') }}</label>
      <Select v-model="editing.calculationItemId" :options="calcOptions" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
    </div>
    <div class="row">
      <div class="field"><label>{{ t('price.from') }}</label><DatePicker v-model="editing.effectiveFrom" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ t('price.to') }}</label><DatePicker v-model="editing.effectiveTo" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
    </div>
    <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.875rem"><Checkbox v-model="editing.isActive" :binary="true" /> {{ t('common.active') }}</label>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="saving" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
:deep(.p-inputtext), :deep(.p-inputnumber), :deep(.p-datepicker), :deep(.p-select) { width: 100%; }
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; width: auto; }
</style>
