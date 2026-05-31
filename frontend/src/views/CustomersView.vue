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
import Checkbox from 'primevue/checkbox'
import Select from 'primevue/select'
import Skeleton from 'primevue/skeleton'
import PagedTableEmpty from '@/components/PagedTableEmpty.vue'

interface CustomerListDto {
  id: number; firstName: string; surname: string | null; embg: string | null
  isCompany: boolean; phoneNumber: string | null; email: string | null; isActive: boolean
  livingStreet: string | null; livingAddressNumber: string | null; livingCity: string | null
}

function formatAddress(c: CustomerListDto): string {
  const street = [c.livingStreet, c.livingAddressNumber ? `бр.${c.livingAddressNumber}` : ''].filter(Boolean).join(' ')
  return [street, c.livingCity].filter(Boolean).join('; ')
}
interface Paged<T> { page: number; pageSize: number; total: number; items: T[] }

const { t } = useI18n()
const router = useRouter()
const customers = ref<CustomerListDto[]>([])
const loading = ref(true)
const total = ref(0)
const searchInput = ref<HTMLInputElement | null>(null)

// URL-synced state — back button restores filter & page
// isCompany filter: 'all' = unfiltered, 'true' = companies, 'false' = individuals.
const state = useUrlState(
  { page: 1, pageSize: 50, q: '', isCompany: 'all', sortBy: '', sortDir: 'asc' as 'asc' | 'desc' },
  { onChange: () => load() },
)

const companyOptions = [
  { label: () => t('customers.filterAll'),     value: 'all' },
  { label: () => t('customers.isCompany'),     value: 'true' },
  { label: () => t('customers.individual'),    value: 'false' },
].map(o => ({ ...o, label: o.label() }))

async function load() {
  loading.value = true
  try {
    const { data } = await api.get<Paged<CustomerListDto>>('/customers', {
      params: {
        q: state.q.value || undefined,
        isCompany: state.isCompany.value === 'all' ? undefined : state.isCompany.value,
        page: state.page.value, pageSize: state.pageSize.value,
        sortBy: state.sortBy.value || undefined,
        sortDir: state.sortBy.value ? state.sortDir.value : undefined,
      },
    })
    customers.value = data.items; total.value = data.total
  } finally { loading.value = false }
}

function onPage(ev: any)   { state.page.value = ev.page + 1; state.pageSize.value = ev.rows }
function onSort(ev: any)   { state.sortBy.value = ev.sortField || ''; state.sortDir.value = ev.sortOrder === 1 ? 'asc' : 'desc'; state.page.value = 1 }

let searchTimer: any
watch(() => state.q.value, () => { clearTimeout(searchTimer); searchTimer = setTimeout(() => { state.page.value = 1 }, 300) })
watch(() => state.isCompany.value, () => { state.page.value = 1 })

useShortcuts({
  'F5':     () => load(),
  'Ctrl+F': () => searchInput.value?.focus(),
  'Ctrl+N': () => router.push('/customers/new'),
})

onMounted(load)

// Skeleton placeholder rows for first paint
const skeletonRows = Array.from({ length: 8 })
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('customers.title') }}</h1>
      <div class="subtitle">{{ total }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText ref="searchInput" v-model="state.q.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Select v-model="state.isCompany.value" :options="companyOptions" optionLabel="label" optionValue="value"
              size="small" class="company-filter" />
      <Button :label="t('customers.new')" icon="pi pi-plus" size="small" @click="router.push('/customers/new')" v-tooltip.bottom="'Ctrl+N'" />
    </div>
  </div>

  <DataTable v-if="loading && customers.length === 0" :value="skeletonRows" stripedRows size="small" class="skeleton-table tight-table">
    <Column :header="t('customers.customer')"     ><template #body><Skeleton /></template></Column>
    <Column :header="t('customers.embg')"         ><template #body><Skeleton /></template></Column>
    <Column :header="t('customers.livingAddress')"><template #body><Skeleton /></template></Column>
    <Column :header="t('customers.isCompany')"    ><template #body><Skeleton /></template></Column>
  </DataTable>

  <DataTable v-else class="tight-table" :value="customers" :loading="loading" stripedRows size="small" lazy paginator
             :first="(state.page.value - 1) * state.pageSize.value" :rows="state.pageSize.value" :totalRecords="total"
             :rowsPerPageOptions="[25, 50, 100, 200]" @page="onPage" @sort="onSort"
             :sortField="state.sortBy.value || undefined" :sortOrder="state.sortDir.value === 'desc' ? -1 : 1"
             rowHover @row-click="(e: any) => router.push(`/customers/${e.data.id}`)" :pt="{ row: { style: 'cursor: pointer' } }">
    <template #empty>
      <PagedTableEmpty icon="pi-users"
        :title="state.q.value ? 'Нема резултати' : 'Нема клиенти'"
        :hint="state.q.value ? `Не е најден ниту еден клиент за \&quot;${state.q.value}\&quot;.` : t('customers.emptyHint', 'Додадете нов клиент за да започнете.')"
        :ctaLabel="t('customers.new')"
        @cta="router.push('/customers/new')" />
    </template>
    <Column field="customer" :header="t('customers.customer')" sortable>
      <template #body="{ data }">{{ [data.firstName, data.surname].filter(Boolean).join(' ') }}</template>
    </Column>
    <Column field="embg"        :header="t('customers.embg')" />
    <Column :header="t('customers.livingAddress')">
      <template #body="{ data }">{{ formatAddress(data) }}</template>
    </Column>
    <Column :header="t('customers.isCompany')" style="width: 110px" bodyStyle="text-align: center">
      <template #body="{ data }">
        <Checkbox :modelValue="data.isCompany" :binary="true" disabled />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 100px">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click.stop="router.push(`/customers/${data.id}`)" v-tooltip.left="t('common.edit')" />
      </template>
    </Column>
  </DataTable>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
.company-filter { min-width: 150px; }
.company-filter :deep(.p-select-label) { font-size: 0.8125rem; }
.skeleton-table :deep(.p-datatable-tbody tr) { height: 28px; }
.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) { padding: 0.2rem 0.5rem; font-size: 0.8125rem; line-height: 1.2; }
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-checkbox) { transform: scale(0.85); }
.tight-table :deep(.p-button.p-button-icon-only) { width: 1.75rem; height: 1.75rem; }
</style>
