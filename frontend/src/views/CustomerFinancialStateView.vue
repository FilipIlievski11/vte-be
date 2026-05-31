<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { FilterMatchMode } from '@primevue/core/api'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputNumber from 'primevue/inputnumber'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'

interface DTO { id: number; customerId: number; date: string; description: string | null; debitAmount: number; creditAmount: number; runningBalance: number }

const { t } = useI18n()
const rows = ref<DTO[]>([])
const loading = ref(true)
const customerId = ref<number | null>(null)
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true
  const params = customerId.value ? { customerId: customerId.value } : {}
  const { data } = await api.get<DTO[]>('/customer-financial-state', { params })
  rows.value = data
  loading.value = false
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div><h1>{{ t('menu.customerFinancialState') }}</h1><div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div></div>
    <div class="actions" style="gap: 0.5rem">
      <span style="font-size: 0.875rem; color: var(--color-text-muted)">{{ t('cfs.customerId') }}:</span>
      <InputNumber v-model="customerId" style="width: 140px" />
      <Button :label="t('common.search')" icon="pi pi-search" outlined size="small" @click="load" />
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
    </div>
  </div>
  <DataTable :value="rows" :loading="loading" stripedRows
             v-model:filters="filters" :globalFilterFields="['description','date']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]" rowHover>
    <template #empty><div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div></template>
    <Column field="date"        :header="t('cfs.date')" sortable />
    <Column field="customerId"  :header="t('cfs.customer')" />
    <Column field="description" :header="t('cfs.description')" />
    <Column field="debitAmount" :header="t('cfs.debit')">
      <template #body="{ data }">{{ data.debitAmount.toFixed(2) }}</template>
    </Column>
    <Column field="creditAmount" :header="t('cfs.credit')">
      <template #body="{ data }">{{ data.creditAmount.toFixed(2) }}</template>
    </Column>
    <Column field="runningBalance" :header="t('cfs.balance')">
      <template #body="{ data }"><strong>{{ data.runningBalance.toFixed(2) }}</strong></template>
    </Column>
  </DataTable>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 180px; font-size: 0.8125rem; }
</style>
