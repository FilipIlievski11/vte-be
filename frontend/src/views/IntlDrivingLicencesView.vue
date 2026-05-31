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
import Select from 'primevue/select'
import { useRegistrationIssuers } from '@/composables/useReferenceData'

interface DTO { id: number; licenceNumber: string; issuedDate: string; validTill: string; customerId: number; isActive: boolean }

const { t } = useI18n()
const issuers = useRegistrationIssuers()
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }
const rows = ref<DTO[]>([])
const loading = ref(true)
const total = ref(0); const page = ref(1); const pageSize = ref(50)
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const form = ref({ customerId: 0, licenceNumber: '', issuedDate: new Date(), validTill: new Date(new Date().setFullYear(new Date().getFullYear() + 1)), issuerId: null as number | null, note: '' })

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<DTO>>('/intl-driving-licences', { params: { page: page.value, pageSize: pageSize.value } })
    rows.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function openDialog() {
  form.value = { customerId: 0, licenceNumber: '', issuedDate: new Date(), validTill: new Date(new Date().setFullYear(new Date().getFullYear() + 1)), issuerId: null, note: '' }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/intl-driving-licences', {
      customerId: form.value.customerId,
      licenceNumber: form.value.licenceNumber,
      issuedDate: form.value.issuedDate.toISOString().slice(0, 10),
      validTill: form.value.validTill.toISOString().slice(0, 10),
      issuerId: form.value.issuerId,
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
    <div><h1>{{ t('menu.intlDrivingLicences') }}</h1><div class="subtitle">{{ total }} {{ t('common.records') }}</div></div>
    <div class="actions"><Button :label="t('common.add')" icon="pi pi-plus" @click="openDialog" /></div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="licenceNumber" :header="t('idl.number')" sortable />
    <Column field="customerId"    :header="t('idl.customer')" />
    <Column field="issuedDate"    :header="t('idl.issued')" sortable />
    <Column field="validTill"     :header="t('idl.validTill')" sortable />
    <Column :header="t('common.actions')" style="width:110px">
      <template #body="{ data }"><Button icon="pi pi-file-pdf" text label="PDF" @click="openAuthorizedPdf(`/reports/intl-driving-licences/${data.id}/pdf`)" /></template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="t('idl.new')" modal :style="{ width: '480px' }">
    <div class="field"><label>{{ t('idl.customer') }} *</label><InputNumber v-model="form.customerId" /><span class="help">{{ t('idl.customerHelp') }}</span></div>
    <div class="field"><label>{{ t('idl.number') }} *</label><InputText v-model="form.licenceNumber" maxlength="50" /></div>
    <div class="field"><label>{{ t('idl.issuer') }}</label>
      <Select v-model="form.issuerId" :options="issuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
    </div>
    <div class="row">
      <div class="field"><label>{{ t('idl.issued') }} *</label><DatePicker v-model="form.issuedDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ t('idl.validTill') }} *</label><DatePicker v-model="form.validTill" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
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
:deep(.p-inputtext), :deep(.p-inputnumber), :deep(.p-datepicker), :deep(.p-select), :deep(.p-textarea) { width: 100%; }
</style>
