<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import Button from 'primevue/button'

interface DTO { id: number; documentNumber: string; datePay: string; totalAmount: number; totalInstallments: number; payed: boolean; storno: boolean; customerVehicleRelationId: number }
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const rows = ref<DTO[]>([])
const loading = ref(true)
const total = ref(0); const page = ref(1); const pageSize = ref(50)

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<DTO>>('/invoices', { params: { page: page.value, pageSize: pageSize.value } })
    rows.value = data.items; total.value = data.total
  } finally { loading.value = false }
}
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }

function pdf(id: number) { openAuthorizedPdf(`/reports/payment-documents/${id}/invoice`) }

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('menu.invoices') }}</h1><div class="subtitle">{{ total }} {{ t('common.records') }}</div></div>
  </div>
  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="documentNumber" :header="t('payments.documentNumber')" sortable />
    <Column field="datePay" :header="t('payments.datePay')" sortable />
    <Column field="totalAmount" :header="t('payments.totalAmount')">
      <template #body="{ data }">{{ data.totalAmount.toFixed(2) }}</template>
    </Column>
    <Column :header="t('payments.status')">
      <template #body="{ data }">
        <Tag v-if="data.storno" :value="t('payments.storno')" severity="danger" />
        <Tag v-else-if="data.payed" :value="t('payments.payed')" severity="success" />
        <Tag v-else :value="t('payments.unpaid')" severity="warn" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 110px">
      <template #body="{ data }"><Button icon="pi pi-file-pdf" text :label="'PDF'" @click="pdf(data.id)" /></template>
    </Column>
  </DataTable>
</template>
