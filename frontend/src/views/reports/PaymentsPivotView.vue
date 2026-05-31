<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'

interface GroupSumRow { group: string; count: number; sum: number }
interface Pivot { total: number; sumTotal: number; byMonth: GroupSumRow[]; byStatus: GroupSumRow[] }

const { t } = useI18n()
const data = ref<Pivot | null>(null)
const loading = ref(true)
onMounted(async () => { try { data.value = (await api.get<Pivot>('/reports/pivot/payments')).data } finally { loading.value = false } })
</script>

<template>
  <div class="page-header"><div><h1>{{ t('menu.report.payments') }}</h1></div></div>
  <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>
  <template v-else-if="data">
    <div class="tiles">
      <div class="tile"><div class="tile-icon"><i class="pi pi-credit-card" /></div><div class="tile-value">{{ data.total }}</div><div class="tile-label">{{ t('reports.totalDocs') }}</div></div>
      <div class="tile success"><div class="tile-icon"><i class="pi pi-money-bill" /></div><div class="tile-value">{{ data.sumTotal.toFixed(2) }}</div><div class="tile-label">{{ t('reports.sumTotal') }}</div></div>
    </div>
    <div class="card" style="margin-bottom: 1rem">
      <div class="card-header">{{ t('reports.byMonth') }}</div>
      <DataTable :value="data.byMonth" stripedRows>
        <Column field="group" :header="t('reports.month')" />
        <Column field="count" :header="t('reports.count')" />
        <Column field="sum"   :header="t('payments.totalAmount')">
          <template #body="{ data: r }">{{ r.sum.toFixed(2) }}</template>
        </Column>
      </DataTable>
    </div>
    <div class="card">
      <div class="card-header">{{ t('payments.status') }}</div>
      <DataTable :value="data.byStatus" stripedRows>
        <Column field="group" :header="t('payments.status')" />
        <Column field="count" :header="t('reports.count')" />
        <Column field="sum"   :header="t('payments.totalAmount')">
          <template #body="{ data: r }">{{ r.sum.toFixed(2) }}</template>
        </Column>
      </DataTable>
    </div>
  </template>
</template>
