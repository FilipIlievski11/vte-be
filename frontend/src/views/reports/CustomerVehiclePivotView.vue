<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'

interface Row { customer: string; vin: string | null; regNumber: string | null; relationType: string | null }
const { t } = useI18n()
const rows = ref<Row[]>([])
const loading = ref(true)
onMounted(async () => { try { rows.value = (await api.get<Row[]>('/reports/pivot/customer-vehicle')).data } finally { loading.value = false } })
</script>

<template>
  <div class="page-header"><div><h1>{{ t('menu.report.customerVehicle') }}</h1><div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div></div></div>
  <DataTable :value="rows" :loading="loading" stripedRows :paginator="rows.length > 25" :rows="25" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="customer"     :header="t('customers.title')" sortable />
    <Column field="vin"          :header="t('vehicles.shellNumber')" />
    <Column field="regNumber"    :header="t('vehicles.lastRegistrationNumber')" />
    <Column field="relationType" :header="t('reports.relationType')" />
  </DataTable>
</template>
