<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'

interface GroupRow { group: string; count: number }
interface Pivot { total: number; open: number; byType: GroupRow[]; byStatus: GroupRow[] }

const { t } = useI18n()
const data = ref<Pivot | null>(null)
const loading = ref(true)
onMounted(async () => { try { data.value = (await api.get<Pivot>('/reports/pivot/requests')).data } finally { loading.value = false } })
</script>

<template>
  <div class="page-header"><div><h1>{{ t('menu.report.requests') }}</h1></div></div>
  <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>
  <template v-else-if="data">
    <div class="tiles">
      <div class="tile"><div class="tile-icon"><i class="pi pi-inbox" /></div><div class="tile-value">{{ data.total }}</div><div class="tile-label">{{ t('reports.totalRequests') }}</div></div>
      <div class="tile warn"><div class="tile-icon"><i class="pi pi-clock" /></div><div class="tile-value">{{ data.open }}</div><div class="tile-label">{{ t('requests.open') }}</div></div>
    </div>
    <div class="card" style="margin-bottom: 1rem">
      <div class="card-header">{{ t('reports.byType') }}</div>
      <DataTable :value="data.byType" stripedRows :paginator="data.byType.length > 15" :rows="15">
        <Column field="group" :header="t('requests.type')" />
        <Column field="count" :header="t('reports.count')" sortable />
      </DataTable>
    </div>
    <div class="card">
      <div class="card-header">{{ t('requests.status') }}</div>
      <DataTable :value="data.byStatus" stripedRows>
        <Column field="group" :header="t('requests.status')" />
        <Column field="count" :header="t('reports.count')" />
      </DataTable>
    </div>
  </template>
</template>
