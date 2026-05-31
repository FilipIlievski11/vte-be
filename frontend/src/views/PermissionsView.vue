<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import DatePicker from 'primevue/datepicker'
import Textarea from 'primevue/textarea'

interface DTO { id: number; permissionNumber: string | null; madeDate: string; endDate: string | null; isActive: boolean }

const { t } = useI18n()
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }
const rows = ref<DTO[]>([])
const loading = ref(true)
const total = ref(0); const page = ref(1); const pageSize = ref(50)
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const form = ref({ customerVehicleRelationId: 0, permissionNumber: '', madeDate: new Date(), endDate: null as Date | null, note: '' })

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<DTO>>('/permissions', { params: { page: page.value, pageSize: pageSize.value } })
    rows.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function openDialog() {
  form.value = { customerVehicleRelationId: 0, permissionNumber: '', madeDate: new Date(), endDate: null, note: '' }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/permissions', {
      customerVehicleRelationId: form.value.customerVehicleRelationId,
      permissionNumber: form.value.permissionNumber || null,
      madeDate: form.value.madeDate.toISOString().slice(0, 10),
      endDate: form.value.endDate ? form.value.endDate.toISOString().slice(0, 10) : null,
      note: form.value.note || null,
    })
    showDialog.value = false; await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('menu.permissions') }}</h1><div class="subtitle">{{ total }} {{ t('common.records') }}</div></div>
    <div class="actions"><Button :label="t('common.add')" icon="pi pi-plus" @click="openDialog" /></div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="permissionNumber" :header="t('permissions.number')" sortable />
    <Column field="madeDate" :header="t('permissions.madeDate')" sortable />
    <Column field="endDate"  :header="t('permissions.endDate')" sortable />
    <Column :header="t('common.actions')" style="width:110px">
      <template #body="{ data }"><Button icon="pi pi-file-pdf" text label="PDF" @click="openAuthorizedPdf(`/reports/permissions/${data.id}/pdf`)" /></template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="t('permissions.new')" modal :style="{ width: '480px' }">
    <div class="field"><label>{{ t('trafficLicences.cvr') }} *</label><InputNumber v-model="form.customerVehicleRelationId" /></div>
    <div class="field"><label>{{ t('permissions.number') }}</label><InputText v-model="form.permissionNumber" maxlength="50" /></div>
    <div class="row">
      <div class="field"><label>{{ t('permissions.madeDate') }} *</label><DatePicker v-model="form.madeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ t('permissions.endDate') }}</label><DatePicker v-model="form.endDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
    </div>
    <div class="field"><label>{{ t('common.note') }}</label><Textarea v-model="form.note" rows="2" autoResize /></div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="saving" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
:deep(.p-inputtext), :deep(.p-inputnumber), :deep(.p-datepicker), :deep(.p-textarea) { width: '100%' }
</style>
