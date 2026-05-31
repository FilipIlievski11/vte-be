<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { FilterMatchMode } from '@primevue/core/api'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'

interface StationDto {
  id: number
  name: string
  code: string
  isActive: boolean
}

const { t } = useI18n()
const stations = ref<StationDto[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)

const form = ref({ name: '', code: '' })
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true
  const { data } = await api.get<StationDto[]>('/stations')
  stations.value = data
  loading.value = false
}

function openDialog() { form.value = { name: '', code: '' }; error.value = null; showDialog.value = true }

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/stations', form.value)
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
  <div class="toolbar">
    <h1>{{ t('stations.title') }}</h1>
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button :label="t('stations.new')" icon="pi pi-plus" size="small" @click="openDialog" />
    </div>
  </div>
  <DataTable :value="stations" :loading="loading" stripedRows
             v-model:filters="filters" :globalFilterFields="['name','code']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]">
    <Column field="name" :header="t('stations.name')" sortable />
    <Column field="code" :header="t('stations.code')" sortable />
    <Column field="isActive" :header="t('stations.isActive')">
      <template #body="{ data }">{{ data.isActive ? t('common.yes') : t('common.no') }}</template>
    </Column>
  </DataTable>
  <Dialog v-model:visible="showDialog" :header="t('stations.new')" modal :style="{ width: '420px' }">
    <div class="field"><label>{{ t('stations.name') }} *</label><InputText v-model="form.name" required /></div>
    <div class="field"><label>{{ t('stations.code') }} *</label><InputText v-model="form.code" required /></div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('customers.cancel')" severity="secondary" @click="showDialog = false" />
      <Button :label="t('customers.save')" :loading="saving" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
</style>
