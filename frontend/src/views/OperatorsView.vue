<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { FilterMatchMode } from '@primevue/core/api'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Select from 'primevue/select'
import Tag from 'primevue/tag'

interface OperatorDto { userId: string; userName: string; email: string | null; stationId: number | null; fullName: string; isActive: boolean }
interface StationDto  { id: number; name: string; code: string; isActive: boolean }

const { t } = useI18n()
const rows = ref<OperatorDto[]>([])
const stations = ref<StationDto[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const form = ref({ userName: '', email: '', password: '', stationId: null as number | null, fullName: '', embg: '' })
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true
  const [a, b] = await Promise.all([
    api.get<OperatorDto[]>('/operators'),
    api.get<StationDto[]>('/stations'),
  ])
  rows.value = a.data
  stations.value = b.data
  loading.value = false
}

function openDialog() {
  form.value = { userName: '', email: '', password: '', stationId: stations.value[0]?.id ?? null, fullName: '', embg: '' }
  error.value = null; showDialog.value = true
}

async function save() {
  saving.value = true; error.value = null
  try {
    await api.post('/operators', {
      userName: form.value.userName,
      email: form.value.email || null,
      password: form.value.password,
      stationId: form.value.stationId,
      fullName: form.value.fullName,
      embg: form.value.embg || null,
    })
    showDialog.value = false; await load()
  } catch (e: any) {
    const errs = e?.response?.data?.errors
    error.value = Array.isArray(errs) ? errs.join('; ') : (e?.response?.data?.error ?? 'Save failed.')
  } finally { saving.value = false }
}

const toast = useToast(); const { ask } = useConfirmDialog()
function deactivate(o: OperatorDto) {
  ask({
    message: t('operators.confirmDeactivate', { name: o.fullName }),
    danger: true, acceptLabel: t('operators.deactivate'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try { await api.post(`/operators/${o.userId}/deactivate`); toast.ok(t('common.saved')); await load() }
      catch (e: any) { toast.error(e?.response?.data?.error ?? 'Failed') }
    },
  })
}

onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('menu.operators') }}</h1>
      <div class="subtitle">{{ rows.length }} {{ t('common.records') }}</div>
    </div>
    <div class="actions">
      <span class="search-wrap">
        <i class="pi pi-search" />
        <InputText v-model="filters.global.value" :placeholder="t('common.search')" size="small" />
      </span>
      <Button :label="t('operators.new')" icon="pi pi-plus" size="small" @click="openDialog" />
    </div>
  </div>

  <DataTable :value="rows" :loading="loading" stripedRows rowHover
             v-model:filters="filters" :globalFilterFields="['userName','fullName','email']"
             :sortField="'userId'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]">
    <template #empty>
      <div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div>
    </template>
    <Column field="userName" :header="t('operators.userName')" sortable />
    <Column field="fullName" :header="t('operators.fullName')" sortable />
    <Column field="email"    :header="t('customers.email')" />
    <Column field="stationId" :header="t('operators.station')">
      <template #body="{ data }">
        <span v-if="data.stationId">#{{ data.stationId }}</span>
        <span v-else style="color: var(--color-text-muted)">—</span>
      </template>
    </Column>
    <Column :header="t('common.status')" style="width: 140px">
      <template #body="{ data }">
        <Tag :value="data.isActive ? t('common.active') : t('common.inactive')" :severity="data.isActive ? 'success' : 'secondary'" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 130px">
      <template #body="{ data }">
        <Button v-if="data.isActive" :label="t('operators.deactivate')" severity="danger" text size="small" @click="deactivate(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="t('operators.new')" modal :style="{ width: '460px' }">
    <div class="field"><label>{{ t('operators.userName') }} *</label><InputText v-model="form.userName" /></div>
    <div class="field"><label>{{ t('operators.fullName') }} *</label><InputText v-model="form.fullName" /></div>
    <div class="field"><label>{{ t('customers.email') }}</label><InputText v-model="form.email" /></div>
    <div class="field"><label>{{ t('operators.embg') }}</label><InputText v-model="form.embg" maxlength="13" /></div>
    <div class="field"><label>{{ t('operators.station') }} *</label>
      <Select v-model="form.stationId" :options="stations" optionLabel="name" optionValue="id" :placeholder="'—'" />
    </div>
    <div class="field"><label>{{ t('operators.password') }} *</label>
      <Password v-model="form.password" :feedback="false" toggleMask input-class="w-full" />
      <span class="help">{{ t('operators.passwordHelp') }}</span>
    </div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="saving" @click="save" />
    </template>
  </Dialog>
</template>

<style scoped>
:deep(.p-inputtext), :deep(.p-select), :deep(.p-password) { width: 100%; }
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
</style>
