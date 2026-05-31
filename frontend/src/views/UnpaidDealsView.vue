<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'

interface DTO { id: number; documentNumber: string; datePay: string; totalAmount: number; totalInstallments: number; payed: boolean; storno: boolean; customerVehicleRelationId: number }
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const rows = ref<DTO[]>([])
const loading = ref(true)
const totalRecords = ref(0); const page = ref(1); const pageSize = ref(50)
const total = computed(() => rows.value.reduce((s, r) => s + r.totalAmount, 0))

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<DTO>>('/unpaid-deals', { params: { page: page.value, pageSize: pageSize.value } })
    rows.value = data.items; totalRecords.value = data.total
  } finally { loading.value = false }
}
function onPage(ev: any) { page.value = ev.page + 1; pageSize.value = ev.rows; load() }

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('menu.unpaidDeals') }}</h1>
      <div class="subtitle">{{ totalRecords }} {{ t('common.records') }} · {{ t('payments.totalAmount') }}: <strong>{{ total.toFixed(2) }}</strong></div>
    </div>
  </div>
  <DataTable :value="rows" :loading="loading" stripedRows lazy paginator
             :first="(page - 1) * pageSize" :rows="pageSize" :totalRecords="totalRecords"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="documentNumber" :header="t('payments.documentNumber')" sortable />
    <Column field="datePay" :header="t('payments.datePay')" sortable />
    <Column field="totalAmount" :header="t('payments.totalAmount')">
      <template #body="{ data }">{{ data.totalAmount.toFixed(2) }}</template>
    </Column>
    <Column :header="t('payments.status')">
      <template #body="{ data }">
        <Tag :value="t('payments.unpaid')" severity="warn" />
      </template>
    </Column>
  </DataTable>
</template>
