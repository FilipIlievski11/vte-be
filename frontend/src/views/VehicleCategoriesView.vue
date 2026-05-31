<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import api from '@/api/client'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { FilterMatchMode } from '@primevue/core/api'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Tag from 'primevue/tag'

interface Row { id: number; code: string | null; name: string; mksjus: string | null; iso: string | null; isActive: boolean }

const { t } = useI18n()
const router = useRouter()
const auth = useAuthStore()
const toast = useToast(); const { ask } = useConfirmDialog()

const rows = ref<Row[]>([])
const loading = ref(true)
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true
  try { rows.value = (await api.get<Row[]>('/vehicle-categories')).data }
  finally { loading.value = false }
}

function removeRow(r: Row) {
  ask({
    message: t('common.confirmDelete', { name: r.name }),
    danger: true, acceptLabel: t('common.delete'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try { await api.delete(`/vehicle-categories/${r.id}`); toast.ok(t('common.saved')); await load() }
      catch (e: any) { toast.error(e?.response?.data?.error ?? 'Delete failed.') }
    },
  })
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('ref.vehicle-categories') }}</h1>
      <div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button v-if="auth.isAdministrator" :label="t('common.add')" icon="pi pi-plus" size="small" @click="router.push('/ref/vehicle-categories/new')" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows size="small"
             v-model:filters="filters" :globalFilterFields="['code','name','mksjus','iso']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]" rowHover dataKey="id"
             @row-click="(e: any) => router.push(`/ref/vehicle-categories/${e.data.id}`)" :pt="{ row: { style: 'cursor: pointer' } }">
    <template #empty>
      <div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div>
    </template>
    <Column field="code" :header="t('vehicleCategories.code')" sortable style="width: 140px">
      <template #body="{ data }"><span v-if="data.code">{{ data.code }}</span><span v-else style="color: var(--color-text-muted)">—</span></template>
    </Column>
    <Column field="name" :header="t('vehicleCategories.name')" sortable />
    <Column :header="t('common.actions')" style="width: 90px" v-if="auth.isAdministrator">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="router.push(`/ref/vehicle-categories/${data.id}`)" />
        <Button icon="pi pi-trash"  text severity="danger" @click.stop="removeRow(data)" />
      </template>
    </Column>
  </DataTable>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
</style>
