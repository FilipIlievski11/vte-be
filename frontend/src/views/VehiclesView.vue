<script setup lang="ts">
import { ref, onMounted, watch } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useShortcuts } from '@/composables/useShortcuts'
import { useUrlState } from '@/composables/useUrlState'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import InputText from 'primevue/inputtext'
import Skeleton from 'primevue/skeleton'
import PagedTableEmpty from '@/components/PagedTableEmpty.vue'

interface VehicleListDto {
  id: number
  shellNumber: string
  lastRegistrationNumber: string
  vehicleCategoryId: number | null
  isActive: boolean
  makerName: string | null
  modelName: string | null
  ownerName: string | null
  ownerEMBG: string | null
}
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const router = useRouter()
const vehicles = ref<VehicleListDto[]>([])
const loading = ref(true)
const total = ref(0)
const searchInput = ref<HTMLInputElement | null>(null)

const state = useUrlState(
  { page: 1, pageSize: 50, q: '', sortBy: '', sortDir: 'asc' as 'asc' | 'desc' },
  { onChange: () => load() },
)

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<VehicleListDto>>('/vehicles', {
      params: {
        q: state.q.value || undefined,
        page: state.page.value, pageSize: state.pageSize.value,
        sortBy: state.sortBy.value || undefined,
        sortDir: state.sortBy.value ? state.sortDir.value : undefined,
      },
    })
    vehicles.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function onPage(ev: any) { state.page.value = ev.page + 1; state.pageSize.value = ev.rows }
function onSort(ev: any) { state.sortBy.value = ev.sortField || ''; state.sortDir.value = ev.sortOrder === 1 ? 'asc' : 'desc'; state.page.value = 1 }

let timer: any = null
watch(() => state.q.value, () => { clearTimeout(timer); timer = setTimeout(() => { state.page.value = 1 }, 300) })

useShortcuts({
  'F5':     () => load(),
  'Ctrl+F': () => searchInput.value?.focus(),
  'Ctrl+N': () => router.push('/vehicles/new'),
})

onMounted(load)
const skeletonRows = Array.from({ length: 8 })
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('menu.vehicles') }}</h1>
      <div class="subtitle">{{ total }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText ref="searchInput" v-model="state.q.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button :label="t('vehicles.new')" icon="pi pi-plus" size="small" @click="router.push('/vehicles/new')" v-tooltip.bottom="'Ctrl+N'" />
    </div>
  </div>

  <DataTable v-if="loading && vehicles.length === 0" :value="skeletonRows" stripedRows size="small" class="tight-table">
    <Column :header="t('vehicles.shellNumber')"           ><template #body><Skeleton /></template></Column>
    <Column :header="t('customers.customer')"             ><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.regNumber')"             ><template #body><Skeleton /></template></Column>
    <Column :header="t('customers.embg')"                 ><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.model')"                 ><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.maker')"                 ><template #body><Skeleton /></template></Column>
  </DataTable>

  <DataTable v-else class="tight-table" :value="vehicles" :loading="loading" stripedRows size="small" lazy paginator
             :first="(state.page.value - 1) * state.pageSize.value" :rows="state.pageSize.value" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" @sort="onSort"
             :sortField="state.sortBy.value || undefined" :sortOrder="state.sortDir.value === 'desc' ? -1 : 1"
             rowHover @row-click="(e: any) => router.push(`/vehicles/${e.data.id}`)" :pt="{ row: { style: 'cursor: pointer' } }">
    <template #empty>
      <PagedTableEmpty icon="pi-car"
        :title="state.q.value ? 'Нема резултати' : 'Нема возила'"
        :hint="state.q.value ? `Не е најдено ниту едно возило за \&quot;${state.q.value}\&quot;.` : 'Додадете возило за да започнете.'"
        :ctaLabel="t('vehicles.new')"
        @cta="router.push('/vehicles/new')" />
    </template>
    <Column field="shellNumber" :header="t('vehicles.shellNumber')" sortable />
    <Column field="ownerName"   :header="t('customers.customer')">
      <template #body="{ data }">
        <span v-if="data.ownerName">{{ data.ownerName }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column field="lastRegistrationNumber" :header="t('vehicles.regNumber')" sortable />
    <Column field="ownerEMBG" :header="t('customers.embg')">
      <template #body="{ data }">
        <span v-if="data.ownerEMBG">{{ data.ownerEMBG }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column field="modelName" :header="t('vehicles.model')">
      <template #body="{ data }">
        <span v-if="data.modelName">{{ data.modelName }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column field="makerName" :header="t('vehicles.maker')">
      <template #body="{ data }">
        <span v-if="data.makerName">{{ data.makerName }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 90px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="router.push(`/vehicles/${data.id}`)" v-tooltip.left="t('common.edit')" />
      </template>
    </Column>
  </DataTable>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
</style>
