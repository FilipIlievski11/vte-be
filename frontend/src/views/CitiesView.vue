<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { useAuthStore } from '@/stores/auth'
import api from '@/api/client'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { FilterMatchMode } from '@primevue/core/api'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Select from 'primevue/select'
import Checkbox from 'primevue/checkbox'
import Tag from 'primevue/tag'

interface City      { id: number; name: string; postalCode: string | null; communityId: number | null; communityName: string | null; countryId: number | null; countryName: string | null; isActive: boolean }
interface Community { id: number; name: string }
interface Country   { id: number; name: string }
interface EditForm  { id: number | null; name: string; postalCode: string; communityId: number | null; countryId: number | null; isActive: boolean }

const { t } = useI18n()
const auth = useAuthStore()
const toast = useToast(); const { ask } = useConfirmDialog()

const rows = ref<City[]>([])
const communities = ref<Community[]>([])
const countries = ref<Country[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const editing = ref<EditForm>({ id: null, name: '', postalCode: '', communityId: null, countryId: null, isActive: true })

const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true; error.value = null
  try {
    const [a, b, c] = await Promise.all([
      api.get<City[]>('/cities'),
      api.get<Community[]>('/communities'),
      api.get<Country[]>('/countries'),
    ])
    rows.value = a.data
    communities.value = b.data.filter((x: any) => x.isActive !== false).map(x => ({ id: x.id, name: x.name }))
    countries.value   = c.data.filter((x: any) => x.isActive !== false).map(x => ({ id: x.id, name: x.name }))
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Load failed.'
  } finally { loading.value = false }
}

function newRow()  { editing.value = { id: null, name: '', postalCode: '', communityId: null, countryId: null, isActive: true }; error.value = null; showDialog.value = true }
function editRow(r: City) {
  editing.value = { id: r.id, name: r.name, postalCode: r.postalCode ?? '', communityId: r.communityId, countryId: r.countryId, isActive: r.isActive }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    const payload = {
      name: editing.value.name,
      postalCode: editing.value.postalCode || null,
      communityId: editing.value.communityId,
      countryId: editing.value.countryId,
      isActive: editing.value.isActive,
    }
    if (editing.value.id == null) await api.post('/cities', payload)
    else                          await api.put(`/cities/${editing.value.id}`, payload)
    showDialog.value = false
    await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

function removeRow(r: City) {
  ask({
    message: t('common.confirmDelete', { name: r.name }),
    danger: true, acceptLabel: t('common.delete'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try { await api.delete(`/cities/${r.id}`); toast.ok(t('common.saved')); await load() }
      catch (e: any) { toast.error(e?.response?.data?.error ?? 'Delete failed.') }
    },
  })
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('ref.cities') }}</h1>
      <div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button v-if="auth.isAdministrator" :label="t('common.add')" icon="pi pi-plus" size="small" @click="newRow" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows size="small"
             v-model:filters="filters" :globalFilterFields="['name','postalCode','communityName','countryName']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]" rowHover dataKey="id">
    <template #empty>
      <div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div>
    </template>
    <Column field="name" :header="t('cities.name')" sortable />
    <Column field="postalCode" :header="t('cities.postalCode')" sortable style="width: 140px">
      <template #body="{ data }">
        <span v-if="data.postalCode">{{ data.postalCode }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column field="communityName" :header="t('cities.community')" sortable style="width: 220px">
      <template #body="{ data }">
        <span v-if="data.communityName">{{ data.communityName }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column field="countryName" :header="t('cities.country')" sortable style="width: 200px">
      <template #body="{ data }">
        <span v-if="data.countryName">{{ data.countryName }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column :header="t('common.status')" style="width: 110px">
      <template #body="{ data }">
        <Tag :value="data.isActive ? t('common.active') : t('common.inactive')" :severity="data.isActive ? 'success' : 'secondary'" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 90px" v-if="auth.isAdministrator">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click="editRow(data)" />
        <Button icon="pi pi-trash"  text severity="danger" @click="removeRow(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="editing.id == null ? t('common.add') : t('common.edit')" modal :style="{ width: '500px' }">
    <div class="field"><label>{{ t('cities.name') }} *</label><InputText v-model="editing.name" autofocus size="small" /></div>
    <div class="row">
      <div class="field" style="flex: 1"><label>{{ t('cities.postalCode') }}</label><InputText v-model="editing.postalCode" maxlength="20" size="small" /></div>
      <div class="field" style="flex: 2"><label>{{ t('cities.country') }}</label>
        <Select v-model="editing.countryId" :options="countries" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
      </div>
    </div>
    <div class="field"><label>{{ t('cities.community') }}</label>
      <Select v-model="editing.communityId" :options="communities" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
    </div>
    <div class="row" style="padding: 0.25rem 0">
      <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.875rem">
        <Checkbox v-model="editing.isActive" :binary="true" /> {{ t('common.active') }}
      </label>
    </div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined size="small" @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" size="small" :loading="saving" @click="save" />
    </template>
  </Dialog>

  <div v-if="error && !showDialog" class="error">{{ error }}</div>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
:deep(.p-inputtext), :deep(.p-select) { width: 100%; }
</style>
