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
import { useTechExamOrgs } from '@/composables/useReferenceData'

interface DTO { id: number; trafficLicenceNumber: string | null; madeDate: string; endDate: string; isActive: boolean }

const { t } = useI18n()
const orgs = useTechExamOrgs()
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }
const rows = ref<DTO[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const expiringOnly = ref(false)
const total = ref(0); const page = ref(1); const pageSize = ref(50)
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }
const form = ref({ customerVehicleRelationId: 0, issuingOrganizationId: null as number | null, trafficLicenceNumber: '', madeDate: new Date(), endDate: new Date(new Date().setFullYear(new Date().getFullYear() + 1)), note: '' })

async function load() {
  loading.value = true
  try {
    const params: any = { page: page.value, pageSize: pageSize.value }
    if (expiringOnly.value) params.expiringOnly = true
    const { data } = await api.get<Paged<DTO>>('/traffic-licences', { params })
    rows.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function openDialog() {
  form.value = { customerVehicleRelationId: 0, issuingOrganizationId: null, trafficLicenceNumber: '', madeDate: new Date(), endDate: new Date(new Date().setFullYear(new Date().getFullYear() + 1)), note: '' }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/traffic-licences', {
      customerVehicleRelationId: form.value.customerVehicleRelationId,
      issuingOrganizationId: form.value.issuingOrganizationId,
      trafficLicenceNumber: form.value.trafficLicenceNumber || null,
      madeDate: form.value.madeDate.toISOString().slice(0, 10),
      endDate: form.value.endDate.toISOString().slice(0, 10),
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
    <div>
      <h1>{{ t('menu.trafficLicences') }}</h1>
      <div class="subtitle">{{ total }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <Button :label="expiringOnly ? t('common.showAll') : t('trafficLicences.expiringOnly')" severity="secondary" outlined size="small" @click="expiringOnly = !expiringOnly; load()" />
      <Button :label="t('common.add')" icon="pi pi-plus" @click="openDialog" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="trafficLicenceNumber" :header="t('trafficLicences.number')" sortable />
    <Column field="madeDate" :header="t('trafficLicences.madeDate')" sortable />
    <Column field="endDate"  :header="t('trafficLicences.endDate')" sortable />
    <Column :header="t('common.actions')" style="width:110px">
      <template #body="{ data }"><Button icon="pi pi-file-pdf" text label="PDF" @click="openAuthorizedPdf(`/reports/traffic-licences/${data.id}/pdf`)" /></template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="t('trafficLicences.new')" modal :style="{ width: '480px' }">
    <div class="field"><label>{{ t('trafficLicences.cvr') }} *</label><InputNumber v-model="form.customerVehicleRelationId" /><span class="help">{{ t('trafficLicences.cvrHelp') }}</span></div>
    <div class="field"><label>{{ t('trafficLicences.number') }}</label><InputText v-model="form.trafficLicenceNumber" maxlength="50" /></div>
    <div class="field"><label>{{ t('trafficLicences.issuingOrg') }}</label>
      <Select v-model="form.issuingOrganizationId" :options="orgs" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
    </div>
    <div class="row">
      <div class="field"><label>{{ t('trafficLicences.madeDate') }} *</label><DatePicker v-model="form.madeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
      <div class="field"><label>{{ t('trafficLicences.endDate') }} *</label><DatePicker v-model="form.endDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
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
