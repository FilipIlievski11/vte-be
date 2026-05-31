<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useShortcuts } from '@/composables/useShortcuts'
import { useUrlState } from '@/composables/useUrlState'
import { formatShortDate } from '@/utils/format'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import Skeleton from 'primevue/skeleton'
import PagedTableEmpty from '@/components/PagedTableEmpty.vue'

interface RequestRow {
  id: number; requestTypeId: number; dateCreated: string; dateEnded: string | null; note: string | null
  customerName?: string | null; vehicleVin?: string | null; vehicleReg?: string | null
}
interface RequestType { id: number; typeName: string }
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const router = useRouter()
const requests = ref<RequestRow[]>([])
const types = ref<Record<number, string>>({})
const loading = ref(true)
const total = ref(0)

const state = useUrlState(
  { page: 1, pageSize: 50, open: 'true', sortBy: '', sortDir: 'desc' as 'asc' | 'desc' },
  { onChange: () => load() },
)
const showOpenOnly = computed({ get: () => state.open.value === 'true', set: v => { state.open.value = v ? 'true' : 'false'; state.page.value = 1 } })

async function load() {
  loading.value = true
  const params: any = { page: state.page.value, pageSize: state.pageSize.value }
  if (showOpenOnly.value) params.open = true
  if (state.sortBy.value) { params.sortBy = state.sortBy.value; params.sortDir = state.sortDir.value }
  const [r, ts] = await Promise.all([
    api.get<Paged<RequestRow>>('/requests', { params }),
    Object.keys(types.value).length > 0 ? null : api.get<RequestType[]>('/request-types'),
  ])
  requests.value = r.data.items
  total.value = r.data.total
  if (ts) types.value = Object.fromEntries((ts as any).data.map((t2: RequestType) => [t2.id, t2.typeName]))
  loading.value = false
}
function onPage(ev: any) { state.page.value = ev.page + 1; state.pageSize.value = ev.rows }
function onSort(ev: any) { state.sortBy.value = ev.sortField || ''; state.sortDir.value = ev.sortOrder === 1 ? 'asc' : 'desc'; state.page.value = 1 }

async function endRequest(id: number) {
  await api.post(`/requests/${id}/end`, { isCustomerChanged: false, isVehicleChanged: false })
  await load()
}

useShortcuts({
  'F5':     () => load(),
  'Ctrl+N': () => router.push('/requests/new'),
})

onMounted(load)
const skeletonRows = Array.from({ length: 8 })
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('menu.requests') }}</h1>
      <div class="subtitle">{{ total }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <Button :label="showOpenOnly ? t('requests.showAll') : t('requests.showOpenOnly')"
              severity="secondary" outlined size="small" @click="showOpenOnly = !showOpenOnly" />
      <Button :label="t('requests.new')" icon="pi pi-plus" @click="router.push('/requests/new')" v-tooltip.bottom="'Ctrl+N'" />
    </div>
  </div>

  <DataTable v-if="loading && requests.length === 0" :value="skeletonRows" stripedRows>
    <Column :header="t('requests.dateCreated')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.type')"       ><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.customer')"   ><template #body><Skeleton /></template></Column>
    <Column :header="t('vehicles.lastRegistrationNumber')"><template #body><Skeleton /></template></Column>
    <Column :header="t('requests.status')"     ><template #body><Skeleton /></template></Column>
  </DataTable>

  <DataTable v-else :value="requests" :loading="loading" stripedRows lazy paginator
             :first="(state.page.value - 1) * state.pageSize.value" :rows="state.pageSize.value" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" @sort="onSort"
             :sortField="state.sortBy.value || undefined" :sortOrder="state.sortDir.value === 'desc' ? -1 : 1"
             rowHover @row-click="(e: any) => router.push(`/requests/${e.data.id}`)" :pt="{ row: { style: 'cursor: pointer' } }">
    <template #empty>
      <PagedTableEmpty icon="pi-file"
        :title="showOpenOnly ? 'Нема активни барања' : 'Нема барања'"
        hint="Кликнете „Ново барање“ за да започнете нов работен тек."
        :ctaLabel="t('requests.new')"
        @cta="router.push('/requests/new')" />
    </template>
    <Column field="dateCreated" :header="t('requests.dateCreated')" sortable style="width:110px">
      <template #body="{ data }">{{ formatShortDate(data.dateCreated) }}</template>
    </Column>
    <Column :header="t('requests.type')">
      <template #body="{ data }">{{ types[data.requestTypeId] ?? '—' }}</template>
    </Column>
    <Column field="customerName" :header="t('requests.customer')" sortable>
      <template #body="{ data }">{{ data.customerName ?? '—' }}</template>
    </Column>
    <Column field="vehicleReg" :header="t('vehicles.lastRegistrationNumber')" sortable>
      <template #body="{ data }">{{ data.vehicleReg ?? '—' }}</template>
    </Column>
    <Column :header="t('requests.status')" style="width: 120px">
      <template #body="{ data }">
        <Tag v-if="data.dateEnded" :value="t('requests.ended')" severity="success" />
        <Tag v-else :value="t('requests.open')" severity="info" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 120px">
      <template #body="{ data }">
        <Button v-if="!data.dateEnded" :label="t('requests.end')" size="small" outlined @click="endRequest(data.id)" v-tooltip.left="t('requests.end')" />
      </template>
    </Column>
  </DataTable>
</template>
