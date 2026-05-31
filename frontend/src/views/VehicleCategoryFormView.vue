<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useShortcuts } from '@/composables/useShortcuts'
import { useToast } from '@/composables/useToast'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import Textarea from 'primevue/textarea'
import Checkbox from 'primevue/checkbox'
import Select from 'primevue/select'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import { useVehicleBodyTypes, useVehicleUses, useVehicleCategoriesForPayments } from '@/composables/useReferenceData'

interface RelationRow { id?: number; bodyTypeId: number | null; useId: number | null; categoryForPaymentsId: number | null; detailDescription: string | null }
interface FieldRow    { id?: number; fieldName: string }
interface CategoryForm {
  id: number | null; code: string; name: string; oldName: string
  mksjus: string; iso: string; mksjusDescription: string; picturePath: string
  description: string; detailDescription: string; isActive: boolean
  relations: RelationRow[]; requiredFields: FieldRow[]; disabledFields: FieldRow[]
}

const { t } = useI18n()
const route = useRoute()
const router = useRouter()
const id = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!id.value && id.value !== 'new')

const bodyTypes        = useVehicleBodyTypes()
const uses             = useVehicleUses()
const categoriesForPay = useVehicleCategoriesForPayments()

const form = ref<CategoryForm>(empty())
const loading = ref(false)
const saving  = ref(false)
const error   = ref<string | null>(null)
const toast   = useToast()

function empty(): CategoryForm {
  return {
    id: null, code: '', name: '', oldName: '',
    mksjus: '', iso: '', mksjusDescription: '', picturePath: '',
    description: '', detailDescription: '', isActive: true,
    relations: [], requiredFields: [], disabledFields: [],
  }
}

async function load() {
  if (!isEdit.value) return
  loading.value = true
  try {
    const { data } = await api.get(`/vehicle-categories/${id.value}`)
    form.value = {
      id: data.id, code: data.code ?? '', name: data.name, oldName: data.oldName ?? '',
      mksjus: data.mksjus ?? '', iso: data.iso ?? '', mksjusDescription: data.mksjusDescription ?? '',
      picturePath: data.picturePath ?? '', description: data.description ?? '', detailDescription: data.detailDescription ?? '',
      isActive: data.isActive,
      relations: (data.relations ?? []).map((r: any) => ({ id: r.id, bodyTypeId: r.bodyTypeId, useId: r.useId, categoryForPaymentsId: r.categoryForPaymentsId, detailDescription: r.detailDescription })),
      requiredFields: (data.requiredFields ?? []).map((f: any) => ({ id: f.id, fieldName: f.fieldName })),
      disabledFields: (data.disabledFields ?? []).map((f: any) => ({ id: f.id, fieldName: f.fieldName })),
    }
  } finally { loading.value = false }
}

async function save() {
  saving.value = true; error.value = null
  try {
    const payload = {
      code: form.value.code || null, name: form.value.name, oldName: form.value.oldName || null,
      mksjus: form.value.mksjus || null, iso: form.value.iso || null,
      mksjusDescription: form.value.mksjusDescription || null, picturePath: form.value.picturePath || null,
      description: form.value.description || null, detailDescription: form.value.detailDescription || null,
      isActive: form.value.isActive,
      relations: form.value.relations,
      requiredFields: form.value.requiredFields.filter(f => f.fieldName?.trim()),
      disabledFields: form.value.disabledFields.filter(f => f.fieldName?.trim()),
    }
    if (isEdit.value) await api.put(`/vehicle-categories/${id.value}`, payload)
    else {
      const { data } = await api.post('/vehicle-categories', payload)
      router.replace(`/ref/vehicle-categories/${data.id}`); toast.ok(t('common.saved')); return
    }
    toast.ok(t('common.saved')); router.push('/ref/vehicle-categories')
  } catch (e: any) {
    const msg = e?.response?.data?.error ?? 'Save failed.'
    error.value = msg; toast.error(msg)
  } finally { saving.value = false }
}

function addRelation() { form.value.relations.push({ bodyTypeId: null, useId: null, categoryForPaymentsId: null, detailDescription: '' }) }
function removeRelation(i: number) { form.value.relations.splice(i, 1) }
function addRequired() { form.value.requiredFields.push({ fieldName: '' }) }
function removeRequired(i: number) { form.value.requiredFields.splice(i, 1) }
function addDisabled() { form.value.disabledFields.push({ fieldName: '' }) }
function removeDisabled(i: number) { form.value.disabledFields.splice(i, 1) }

useShortcuts({
  'F2':     () => save(),
  'Ctrl+S': () => save(),
  'Escape': () => router.back(),
})

onMounted(load)
</script>

<template>
  <div class="vehicle-form">
    <div class="page-header">
      <div>
        <h1>{{ isEdit ? t('vehicleCategories.edit') : t('vehicleCategories.new') }}</h1>
        <div class="subtitle">
          <Tag v-if="form.code" :value="form.code" severity="info" />
          <span>{{ form.name || '—' }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>

    <template v-else>
      <!-- Identity -->
      <div class="card">
        <div class="card-header">{{ t('vehicleCategories.section.identity') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field" style="max-width: 160px"><label>{{ t('vehicleCategories.code') }} *</label><InputText v-model="form.code" maxlength="10" size="small" /></div>
            <div class="field"><label>{{ t('vehicleCategories.name') }} *</label><InputText v-model="form.name" maxlength="50" size="small" /></div>
            <div class="field"><label>{{ t('vehicleCategories.oldName') }}</label><InputText v-model="form.oldName" maxlength="50" size="small" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ t('vehicleCategories.mksjus') }}</label><InputText v-model="form.mksjus" maxlength="50" size="small" /></div>
            <div class="field"><label>{{ t('vehicleCategories.iso') }}</label><InputText v-model="form.iso" maxlength="50" size="small" /></div>
            <div class="field"><label>{{ t('vehicleCategories.picturePath') }}</label><InputText v-model="form.picturePath" maxlength="250" size="small" /></div>
          </div>
          <div class="field"><label>{{ t('vehicleCategories.mksjusDescription') }}</label><InputText v-model="form.mksjusDescription" maxlength="250" size="small" /></div>
          <div class="field"><label>{{ t('vehicleCategories.description') }}</label><InputText v-model="form.description" maxlength="500" size="small" /></div>
          <div class="field"><label>{{ t('vehicleCategories.detailDescription') }}</label><Textarea v-model="form.detailDescription" rows="3" autoResize /></div>
          <label class="check"><Checkbox v-model="form.isActive" :binary="true" />{{ t('common.active') }}</label>
        </div>
      </div>

      <!-- Relations grid -->
      <div class="card">
        <div class="card-header">
          <span>{{ t('vehicleCategories.section.relations') }}</span>
          <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addRelation" />
        </div>
        <div class="card-body">
          <DataTable :value="form.relations" v-if="form.relations.length" stripedRows size="small">
            <Column :header="t('vehicleCategories.bodyType')" style="width: 32%">
              <template #body="{ data }">
                <Select v-model="data.bodyTypeId" :options="bodyTypes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
              </template>
            </Column>
            <Column :header="t('vehicleCategories.use')" style="width: 24%">
              <template #body="{ data }">
                <Select v-model="data.useId" :options="uses" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
              </template>
            </Column>
            <Column :header="t('vehicleCategories.categoryForPayments')" style="width: 24%">
              <template #body="{ data }">
                <Select v-model="data.categoryForPaymentsId" :options="categoriesForPay" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
              </template>
            </Column>
            <Column :header="t('vehicleCategories.detailDescription')">
              <template #body="{ data }"><InputText v-model="data.detailDescription" maxlength="250" size="small" /></template>
            </Column>
            <Column :header="t('common.actions')" style="width: 70px">
              <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeRelation(index)" /></template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 1rem"><i class="pi pi-inbox" />{{ t('vehicleCategories.noRelations') }}</div>
        </div>
      </div>

      <!-- Required + Disabled fields, side by side on wide screens -->
      <div class="row fields-row">
        <div class="card" style="flex: 1">
          <div class="card-header">
            <span>{{ t('vehicleCategories.section.requiredFields') }}</span>
            <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addRequired" />
          </div>
          <div class="card-body">
            <DataTable :value="form.requiredFields" v-if="form.requiredFields.length" stripedRows size="small">
              <Column :header="t('vehicleCategories.fieldName')">
                <template #body="{ data }"><InputText v-model="data.fieldName" maxlength="100" size="small" /></template>
              </Column>
              <Column :header="t('common.actions')" style="width: 70px">
                <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeRequired(index)" /></template>
              </Column>
            </DataTable>
            <div v-else class="empty" style="padding: 1rem"><i class="pi pi-inbox" />{{ t('vehicleCategories.noRequiredFields') }}</div>
          </div>
        </div>
        <div class="card" style="flex: 1">
          <div class="card-header">
            <span>{{ t('vehicleCategories.section.disabledFields') }}</span>
            <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addDisabled" />
          </div>
          <div class="card-body">
            <DataTable :value="form.disabledFields" v-if="form.disabledFields.length" stripedRows size="small">
              <Column :header="t('vehicleCategories.fieldName')">
                <template #body="{ data }"><InputText v-model="data.fieldName" maxlength="100" size="small" /></template>
              </Column>
              <Column :header="t('common.actions')" style="width: 70px">
                <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeDisabled(index)" /></template>
              </Column>
            </DataTable>
            <div v-else class="empty" style="padding: 1rem"><i class="pi pi-inbox" />{{ t('vehicleCategories.noDisabledFields') }}</div>
          </div>
        </div>
      </div>

      <div v-if="error" class="error">{{ error }}</div>

      <div class="footer-actions">
        <Button :label="t('common.cancel')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/ref/vehicle-categories')" />
        <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" @click="save" />
      </div>
    </template>
  </div>
</template>

<style scoped>
.vehicle-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }
.vehicle-form :deep(.page-header)             { margin-bottom: 0.75rem; }
.vehicle-form :deep(.page-header h1)          { font-size: 1.125rem; letter-spacing: -0.01em; }
.vehicle-form :deep(.page-header .subtitle)   { font-size: 0.75rem; display: flex; align-items: center; gap: 0.5rem; }

.vehicle-form :deep(.card + .card)            { margin-top: 0.5rem; }
.vehicle-form :deep(.card .card-header) {
  position: relative;
  padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.vehicle-form :deep(.card .card-header)::before {
  content: ''; position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}
.vehicle-form :deep(.card .card-body)         { padding: 0.625rem 0.875rem; }

.vehicle-form :deep(.row)                     { gap: 0.625rem; }
.fields-row { display: flex; gap: 0.5rem; align-items: stretch; margin-top: 0.5rem; }
.fields-row > .card { margin-top: 0 !important; }
.vehicle-form :deep(.field)                   { gap: 0.15rem; margin-bottom: 0.45rem; }
.vehicle-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; }
.check { display: flex; align-items: center; gap: 0.4rem; font-size: 0.8125rem; padding: 0.25rem 0; }

.vehicle-form :deep(.p-select),
.vehicle-form :deep(.p-inputtext)             { width: 100%; }
.vehicle-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-textarea)              { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-select)                { font-size: 0.8125rem; }
.vehicle-form :deep(.p-select-label)          { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.vehicle-form :deep(.p-checkbox)              { transform: scale(0.85); transform-origin: left center; }

.vehicle-form :deep(.p-datatable-tbody td),
.vehicle-form :deep(.p-datatable-thead th)    { padding: 0.25rem 0.5rem; font-size: 0.8125rem; }

.footer-actions {
  position: fixed; bottom: 0; left: var(--sidebar-w); right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border);
  z-index: 5;
}
</style>
