<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'

interface GroupRow { group: string; count: number }
interface Pivot { total: number; passedTotal: number; failedTotal: number; byMonth: GroupRow[]; byType: GroupRow[] }

const { t } = useI18n()
const data = ref<Pivot | null>(null)
const loading = ref(true)
onMounted(async () => { try { data.value = (await api.get<Pivot>('/reports/pivot/exams-count')).data } finally { loading.value = false } })
</script>

<template>
  <div class="page-header"><div><h1>{{ t('menu.report.examsCount') }}</h1></div></div>
  <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>
  <template v-else-if="data">
    <div class="tiles">
      <div class="tile"><div class="tile-icon"><i class="pi pi-check-square" /></div><div class="tile-value">{{ data.total }}</div><div class="tile-label">{{ t('reports.totalExams') }}</div></div>
      <div class="tile success"><div class="tile-icon"><i class="pi pi-check" /></div><div class="tile-value">{{ data.passedTotal }}</div><div class="tile-label">{{ t('exams.pass') }}</div></div>
      <div class="tile danger"><div class="tile-icon"><i class="pi pi-times" /></div><div class="tile-value">{{ data.failedTotal }}</div><div class="tile-label">{{ t('exams.fail') }}</div></div>
    </div>
    <div class="card" style="margin-bottom: 1rem">
      <div class="card-header">{{ t('reports.byMonth') }}</div>
      <DataTable :value="data.byMonth" stripedRows>
        <Column field="group" :header="t('reports.month')" />
        <Column field="count" :header="t('reports.count')" />
      </DataTable>
    </div>
    <div class="card">
      <div class="card-header">{{ t('reports.byType') }}</div>
      <DataTable :value="data.byType" stripedRows :paginator="data.byType.length > 15" :rows="15">
        <Column field="group" :header="t('exams.type')" />
        <Column field="count" :header="t('reports.count')" sortable />
      </DataTable>
    </div>
  </template>
</template>
