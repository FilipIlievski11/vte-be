<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import DatePicker from 'primevue/datepicker'
import Textarea from 'primevue/textarea'

interface ContractDto { id: number; contractNumber: string; contractDate: string; numberOfInstallments: number; guarantorName: string | null }
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const rows = ref<ContractDto[]>([])
const loading = ref(true)
const total = ref(0); const page = ref(1); const pageSize = ref(50)
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const form = ref({
  contractNumber: '',
  contractDate: new Date(),
  numberOfInstallments: 6,
  guarantorName: '',
  guarantorAddress: '',
  guarantorEMBG: '',
  note: '',
})

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<ContractDto>>('/installment-contracts', { params: { page: page.value, pageSize: pageSize.value } })
    rows.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function openDialog() {
  form.value = { contractNumber: '', contractDate: new Date(), numberOfInstallments: 6, guarantorName: '', guarantorAddress: '', guarantorEMBG: '', note: '' }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/installment-contracts', {
      contractNumber: form.value.contractNumber,
      contractDate: form.value.contractDate.toISOString().slice(0, 10),
      numberOfInstallments: form.value.numberOfInstallments,
      guarantorName: form.value.guarantorName || null,
      guarantorAddress: form.value.guarantorAddress || null,
      guarantorEMBG: form.value.guarantorEMBG || null,
      note: form.value.note || null,
    })
    showDialog.value = false
    await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally {
    saving.value = false
  }
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('menu.installmentContracts') }}</h1>
      <div class="subtitle">{{ total }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <Button :label="t('common.add')" icon="pi pi-plus" @click="openDialog" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty>
      <div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div>
    </template>
    <Column field="contractNumber" :header="t('contracts.number')" sortable />
    <Column field="contractDate"   :header="t('contracts.date')" sortable />
    <Column field="numberOfInstallments" :header="t('contracts.numberOfInstallments')" />
    <Column field="guarantorName"  :header="t('contracts.guarantor')" />
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="t('contracts.new')" modal :style="{ width: '480px' }">
    <div class="row">
      <div class="field"><label>{{ t('contracts.number') }} *</label><InputText v-model="form.contractNumber" maxlength="50" /></div>
      <div class="field"><label>{{ t('contracts.date') }} *</label><DatePicker v-model="form.contractDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
    </div>
    <div class="field"><label>{{ t('contracts.numberOfInstallments') }} * (2–60)</label>
      <InputNumber v-model="form.numberOfInstallments" :min="2" :max="60" />
    </div>
    <h3 class="subhead">{{ t('contracts.guarantor') }}</h3>
    <div class="row">
      <div class="field"><label>{{ t('contracts.guarantorName') }}</label><InputText v-model="form.guarantorName" maxlength="50" /></div>
      <div class="field"><label>{{ t('contracts.guarantorEMBG') }}</label><InputText v-model="form.guarantorEMBG" maxlength="20" /></div>
    </div>
    <div class="field"><label>{{ t('contracts.guarantorAddress') }}</label><InputText v-model="form.guarantorAddress" maxlength="250" /></div>
    <div class="field"><label>{{ t('common.note') }}</label><Textarea v-model="form.note" rows="2" autoResize maxlength="500" /></div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="saving" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
.subhead { font-size: 0.8125rem; color: var(--color-text-muted); text-transform: uppercase; letter-spacing: 0.05em; font-weight: 600; margin: 1rem 0 0.5rem; }
:deep(.p-inputtext), :deep(.p-inputnumber), :deep(.p-datepicker), :deep(.p-textarea) { width: 100%; }
</style>
