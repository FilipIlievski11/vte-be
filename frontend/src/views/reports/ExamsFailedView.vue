<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { openAuthorizedPdf } from '@/utils/pdf'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Button from 'primevue/button'

interface FailedRow { id: number; madeDate: string; regNumber: string | null; note: string | null }
const { t } = useI18n()
const rows = ref<FailedRow[]>([])
const loading = ref(true)
onMounted(async () => { try { rows.value = (await api.get<FailedRow[]>('/reports/pivot/exams-failed')).data } finally { loading.value = false } })
function pdf(id: number) { openAuthorizedPdf(`/reports/technical-exam-reports/${id}/pdf`) }
</script>

<template>
  <div class="page-header"><div><h1>{{ t('menu.report.examsFailed') }}</h1><div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div></div></div>
  <DataTable :value="rows" :loading="loading" stripedRows :paginator="rows.length > 25" :rows="25" rowHover>
    <template #empty><div class="empty"><i class="pi pi-check" /><div>{{ t('reports.noFailures') }}</div></div></template>
    <Column field="madeDate"  :header="t('exams.madeDate')" sortable />
    <Column field="regNumber" :header="t('vehicles.lastRegistrationNumber')" />
    <Column field="note"      :header="t('common.note')" />
    <Column :header="t('common.actions')" style="width: 110px">
      <template #body="{ data }"><Button icon="pi pi-file-pdf" text label="PDF" @click="pdf(data.id)" /></template>
    </Column>
  </DataTable>
</template>
