<script setup lang="ts">
import { ref, watch, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
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
import Checkbox from 'primevue/checkbox'
import Tag from 'primevue/tag'

interface RefRow { id: number; name: string; isActive?: boolean }

const { t } = useI18n()
const route = useRoute()
const auth = useAuthStore()

const kind = computed(() => route.params.kind as string)
const titleKey = computed(() => `ref.${kind.value}`)

const rows = ref<RefRow[]>([])
const loading = ref(true)
const showDialog = ref(false)
const error = ref<string | null>(null)
const saving = ref(false)
const editing = ref<{ id: number | null; name: string; isActive: boolean }>({ id: null, name: '', isActive: true })
const filters = ref<any>({ global: { value: null, matchMode: FilterMatchMode.CONTAINS } })

async function load() {
  loading.value = true
  error.value = null
  try {
    const { data } = await api.get<RefRow[]>(`/ref/${kind.value}`)
    rows.value = data.map((r: any) => ({ id: r.id, name: r.name, isActive: r.isActive ?? true }))
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Load failed.'
  } finally {
    loading.value = false
  }
}

function newRow()  { editing.value = { id: null, name: '', isActive: true }; error.value = null; showDialog.value = true }
function editRow(r: RefRow) { editing.value = { id: r.id, name: r.name, isActive: r.isActive ?? true }; error.value = null; showDialog.value = true }

async function save() {
  saving.value = true; error.value = null
  try {
    if (editing.value.id == null) {
      await api.post(`/ref/${kind.value}`, { name: editing.value.name, isActive: editing.value.isActive })
    } else {
      await api.put(`/ref/${kind.value}/${editing.value.id}`, { name: editing.value.name, isActive: editing.value.isActive })
    }
    showDialog.value = false
    await load()
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally {
    saving.value = false
  }
}

const toast = useToast(); const { ask } = useConfirmDialog()

function removeRow(r: RefRow) {
  ask({
    message: t('common.confirmDelete', { name: r.name }),
    danger: true, acceptLabel: t('common.delete'), rejectLabel: t('common.cancel'),
    onAccept: async () => {
      try { await api.delete(`/ref/${kind.value}/${r.id}`); toast.ok(t('common.saved')); await load() }
      catch (e: any) { toast.error(e?.response?.data?.error ?? 'Delete failed.') }
    },
  })
}

watch(kind, load)
onMounted(load)
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t(titleKey) }}</h1>
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

  <DataTable :value="rows" :loading="loading" stripedRows
             v-model:filters="filters" :globalFilterFields="['name']"
             :sortField="'id'" :sortOrder="1"
             paginator :rows="25" :rowsPerPageOptions="[25, 50, 100, 200]" rowHover>
    <template #empty>
      <div class="empty"><i class="pi pi-inbox" /><div>{{ t('common.empty') }}</div></div>
    </template>
    <Column field="name" :header="t('common.name')" sortable />
    <Column :header="t('common.status')" style="width: 140px">
      <template #body="{ data }">
        <Tag :value="(data.isActive ?? true) ? t('common.active') : t('common.inactive')" :severity="(data.isActive ?? true) ? 'success' : 'secondary'" />
      </template>
    </Column>
    <Column :header="t('common.actions')" style="width: 120px" v-if="auth.isAdministrator">
      <template #body="{ data }">
        <Button icon="pi pi-pencil" text @click="editRow(data)" />
        <Button icon="pi pi-trash"  text severity="danger" @click="removeRow(data)" />
      </template>
    </Column>
  </DataTable>

  <Dialog v-model:visible="showDialog" :header="editing.id == null ? t('common.add') : t('common.edit')" modal :style="{ width: '420px' }">
    <div class="field"><label>{{ t('common.name') }} *</label><InputText v-model="editing.name" autofocus /></div>
    <div class="row" style="padding: 0.25rem 0">
      <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.875rem">
        <Checkbox v-model="editing.isActive" :binary="true" /> {{ t('common.active') }}
      </label>
    </div>
    <div v-if="error" class="error">{{ error }}</div>
    <template #footer>
      <Button :label="t('common.cancel')" severity="secondary" outlined @click="showDialog = false" />
      <Button :label="t('common.save')" icon="pi pi-check" :loading="saving" @click="save" />
    </template>
  </Dialog>

  <div v-if="error && !showDialog" class="error">{{ error }}</div>
</template>

<style scoped>
.search-wrap { position: relative; display: inline-flex; align-items: center; }
.search-wrap i { position: absolute; left: 0.5rem; color: var(--color-text-muted); pointer-events: none; font-size: 0.75rem; }
.search-wrap :deep(input) { padding-left: 1.625rem; min-width: 200px; font-size: 0.8125rem; }
</style>
