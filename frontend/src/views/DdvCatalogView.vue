<script setup lang="ts">
import { ref, onMounted } from 'vue'
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
import Checkbox from 'primevue/checkbox'
import Tag from 'primevue/tag'

interface DTO { id: number; name: string; rate: number; effectiveFrom: string; effectiveTo: string | null; isActive: boolean }

const { t } = useI18n()
const auth = useAuthStore()
const rows = ref<DTO[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const editing = ref<{ id: number | null; name: string; rate: number; effectiveFrom: Date; effectiveTo: Date | null; isActive: boolean }>({ id: null, name: '', rate: 18, effectiveFrom: new Date(), effectiveTo: null, isActive: true })
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() { loading.value = true; rows.value = (await api.get<DTO[]>('/ddv-catalog')).data; loading.value = false }
function newRow()  { editing.value = { id: null, name: '', rate: 18, effectiveFrom: new Date(), effectiveTo: null, isActive: true }; error.value = null; showDialog.value = true }
function editRow(r: DTO) {
  editing.value = { id: r.id, name: r.name, rate: r.rate, effectiveFrom: new Date(r.effectiveFrom), effectiveTo: r.effectiveTo ? new Date(r.effectiveTo) : null, isActive: r.isActive }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    const body = {
      name: editing.value.name, rate: editing.value.rate,
      effectiveFrom: editing.value.effectiveFrom.toISOString().slice(0,10),
      effectiveTo: editing.value.effectiveTo ? editing.value.effectiveTo.toISOString().slice(0,10) : null,
      isActive: editing.value.isActive,
    }
    if (editing.value.id == null) await api.post('/ddv-catalog', body)
    else await api.put(`/ddv-catalog/${editing.value.id}`, body)
    showDialog.value = false; await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('menu.ddvCatalog') }}</h1><div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div></div>
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
    <Column field="rate" :header="t('ddv.rate')" sortable>
      <template #body="{ data }"><strong>{{ data.rate }}%</strong></template>
    </Column>
    <Column field="effectiveFrom" :header="t('price.from')" sortable />
    <Column field="effectiveTo"   :header="t('price.to')" />
    <Column :header="t('common.status')" style="width: 130px">
      <template #body="{ data }"><Tag :value="data.isActive ? t('common.active') : t('common.inactive')" :severity="data.isActive ? 'success' : 'secondary'" /></template>
    </Column>
    <Column :header="t('common.actions')" style="width: 100px" v-if="auth.isAdministrator">
      <template #body="{ data }"><Button icon="pi pi-pencil" text @click="editRow(data)" /></template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="editing.id == null ? t('common.add') : t('common.edit')" modal :style="{ width: '460px' }">
    <div class="field"><label>{{ t('common.name') }} *</label><InputText v-model="editing.name" maxlength="100" /></div>
    <div class="field"><label>{{ t('ddv.rate') }} % *</label><InputNumber v-model="editing.rate" :minFractionDigits="2" :min="0" :max="100" suffix="%" /></div>
    <div class="row">
      <div class="field"><label>{{ t('price.from') }} *</label><DatePicker v-model="editing.effectiveFrom" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
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
:deep(.p-inputtext), :deep(.p-inputnumber), :deep(.p-datepicker) { width: 100%; }
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; width: auto; }
</style>
