<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Checkbox from 'primevue/checkbox'
import DatePicker from 'primevue/datepicker'
import Select from 'primevue/select'
import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import { useTechExamOrgs, useTechExamTypes, useTechExamPartStatuses } from '@/composables/useReferenceData'

interface CustomerOpt { id: number; firstName: string; surname: string | null; embg: string | null; phoneNumber: string | null }
interface VehicleOpt  { id: number; shellNumber: string; lastRegistrationNumber: string }
interface OperatorOpt { userId: string; userName: string; fullName: string }
interface PartLeaf    { id: number; name: string }
interface PartCategory { id: number; name: string; parts: PartLeaf[] }
interface Fault {
  technicalExamVehiclePartId: number; partName: string; statusId: number | null
  front: boolean; back: boolean; onLeft: boolean; onRight: boolean
  dateEnter: Date; note: string
}

const { t } = useI18n()
const router = useRouter()
const route  = useRoute()
const editId = computed<number | null>(() => {
  const v = route.params.id
  if (!v) return null
  const n = Number(v); return isNaN(n) ? null : n
})
const isEdit = computed(() => editId.value !== null)

const examOrgs   = useTechExamOrgs()
const examTypes  = useTechExamTypes()
const statuses   = useTechExamPartStatuses()

const operators  = ref<OperatorOpt[]>([])
const partsTree  = ref<PartCategory[]>([])
const activeCategoryId = ref<number | null>(null)

// Customer search
const customerQuery = ref('')
const customerResults = ref<CustomerOpt[]>([])
const selectedCustomer = ref<CustomerOpt | null>(null)
const customerSearching = ref(false)

// Vehicle search
const vehicleQuery = ref('')
const vehicleResults = ref<VehicleOpt[]>([])
const selectedVehicle = ref<VehicleOpt | null>(null)
const vehicleSearching = ref(false)

// Form fields
const technicalExamTypeId = ref<number | null>(null)
const organizationForTechnicalExamId = ref<number | null>(null)
const firstInspectorOperatorUserId = ref<string | null>(null)
const secondInspectorOperatorUserId = ref<string | null>(null)
const regNumber = ref<string>('')
const madeDate = ref<Date>(new Date())
const _validTillInit = new Date(); _validTillInit.setFullYear(_validTillInit.getFullYear() + 1)
const validTillDate = ref<Date>(_validTillInit)
const vehicleIsRight = ref(true)
const explanationNote = ref<string>('')
const driversWarning = ref<string>('')
const note = ref<string>('')
const firstSigned = ref(false)
const secondSigned = ref(false)

// Faults
const faults = ref<Fault[]>([])

// Visual errors
interface VisualError { description: string; severity: string }
const visualErrors = ref<VisualError[]>([])

const saving = ref(false)
const error = ref<string | null>(null)

const customerLabel = (c: CustomerOpt) =>
  `${c.surname ?? ''} ${c.firstName}${c.embg ? ` · ${c.embg}` : ''}`.trim()
const vehicleLabel  = (v: VehicleOpt) =>
  `${v.lastRegistrationNumber} · ${v.shellNumber}`

let customerTimer: number | undefined
function onCustomerInput() {
  selectedCustomer.value = null
  if (customerTimer) window.clearTimeout(customerTimer)
  const q = customerQuery.value.trim()
  if (q.length < 3) { customerResults.value = []; return }
  customerTimer = window.setTimeout(async () => {
    customerSearching.value = true
    try {
      const { data } = await api.get<{ items: CustomerOpt[] }>('/customers', { params: { q, take: 20 } })
      customerResults.value = data.items
    } finally { customerSearching.value = false }
  }, 250)
}
function pickCustomer(c: CustomerOpt) {
  selectedCustomer.value = c
  customerQuery.value = customerLabel(c)
  customerResults.value = []
}

let vehicleTimer: number | undefined
function onVehicleInput() {
  selectedVehicle.value = null
  if (vehicleTimer) window.clearTimeout(vehicleTimer)
  const q = vehicleQuery.value.trim()
  if (q.length < 4) { vehicleResults.value = []; return }
  vehicleTimer = window.setTimeout(async () => {
    vehicleSearching.value = true
    try {
      const { data } = await api.get<{ items: VehicleOpt[] }>('/vehicles', { params: { q, take: 20 } })
      vehicleResults.value = data.items
    } finally { vehicleSearching.value = false }
  }, 250)
}
function pickVehicle(v: VehicleOpt) {
  selectedVehicle.value = v
  vehicleQuery.value = vehicleLabel(v)
  if (!regNumber.value) regNumber.value = v.lastRegistrationNumber
  vehicleResults.value = []
}

const activeCategory = computed(() => partsTree.value.find(c => c.id === activeCategoryId.value) ?? null)

function isFaulted(partId: number) { return faults.value.some(f => f.technicalExamVehiclePartId === partId) }

function togglePart(p: PartLeaf) {
  const idx = faults.value.findIndex(f => f.technicalExamVehiclePartId === p.id)
  if (idx >= 0) faults.value.splice(idx, 1)
  else {
    const defaultStatus = statuses.value[0]?.id ?? null
    faults.value.push({
      technicalExamVehiclePartId: p.id, partName: p.name, statusId: defaultStatus,
      front: false, back: false, onLeft: false, onRight: false,
      dateEnter: new Date(), note: '',
    })
  }
}
function removeFault(i: number) { faults.value.splice(i, 1) }

function addVisualError() { visualErrors.value.push({ description: '', severity: '' }) }
function removeVisualError(i: number) { visualErrors.value.splice(i, 1) }

watch(vehicleIsRight, (v) => { if (v) faults.value = [] })

async function load() {
  const [opsRes, treeRes] = await Promise.all([
    api.get<OperatorOpt[]>('/operators').catch(() => ({ data: [] as OperatorOpt[] })),
    api.get<PartCategory[]>('/ref/exam-parts-tree'),
  ])
  operators.value = opsRes.data
  partsTree.value = treeRes.data
  activeCategoryId.value = partsTree.value[0]?.id ?? null

  if (!isEdit.value) return

  // Edit mode — hydrate the form from the existing report
  const { data } = await api.get<{
    report: any; customer: CustomerOpt | null; vehicle: VehicleOpt | null
    details: { id: number; technicalExamVehiclePartId: number; statusId: number;
               front: boolean; back: boolean; onLeft: boolean; onRight: boolean;
               dateEnter: string; note: string | null }[]
    visualErrors: { id: number; description: string; severity: string | null }[]
  }>(`/technical-exam-reports/${editId.value}/full`)

  const r = data.report as any
  if (data.customer) {
    selectedCustomer.value = data.customer
    customerQuery.value = customerLabel(data.customer)
  }
  if (data.vehicle) {
    selectedVehicle.value = data.vehicle
    vehicleQuery.value = vehicleLabel(data.vehicle)
  }
  technicalExamTypeId.value = r.technicalExamTypeId
  organizationForTechnicalExamId.value = r.organizationForTechnicalExamId
  firstInspectorOperatorUserId.value = r.firstInspectorOperatorUserId
  secondInspectorOperatorUserId.value = r.secondInspectorOperatorUserId
  regNumber.value = r.regNumber ?? ''
  madeDate.value = r.madeDate ? new Date(r.madeDate) : madeDate.value
  validTillDate.value = r.validTillDate ? new Date(r.validTillDate) : validTillDate.value
  vehicleIsRight.value = !!r.vehicleIsRight
  explanationNote.value = r.explanationNote ?? ''
  driversWarning.value = r.driversWarning ?? ''
  note.value = r.note ?? ''

  // Hydrate faults — look up the part name from the parts tree
  const partLookup = new Map<number, string>()
  for (const cat of partsTree.value) for (const p of cat.parts) partLookup.set(p.id, p.name)
  faults.value = data.details.map(d => ({
    technicalExamVehiclePartId: d.technicalExamVehiclePartId,
    partName: partLookup.get(d.technicalExamVehiclePartId) ?? `#${d.technicalExamVehiclePartId}`,
    statusId: d.statusId,
    front: d.front, back: d.back, onLeft: d.onLeft, onRight: d.onRight,
    dateEnter: new Date(d.dateEnter),
    note: d.note ?? '',
  }))

  visualErrors.value = data.visualErrors.map(v => ({ description: v.description, severity: v.severity ?? '' }))
}

async function save() {
  error.value = null
  if (!selectedCustomer.value) { error.value = t('exam.errCustomer'); return }
  if (!selectedVehicle.value)  { error.value = t('exam.errVehicle'); return }
  if (!technicalExamTypeId.value) { error.value = t('exam.errType'); return }
  if (!organizationForTechnicalExamId.value) { error.value = t('exam.errOrg'); return }
  if (!firstInspectorOperatorUserId.value) { error.value = t('exam.errInspector'); return }
  if (validTillDate.value < madeDate.value) { error.value = t('exam.errDateOrder'); return }
  if (secondInspectorOperatorUserId.value && secondInspectorOperatorUserId.value === firstInspectorOperatorUserId.value) {
    error.value = t('exam.errSameInspector'); return
  }
  if (faults.value.some(f => !f.statusId)) { error.value = t('exam.errFaultStatus'); return }

  saving.value = true
  try {
    const { data: cvr } = await api.post('/customer-vehicle-relations/ensure', null, {
      params: { customerId: selectedCustomer.value.id, vehicleId: selectedVehicle.value.id },
    })

    const payload = {
      customerVehicleRelationId: cvr.id,
      technicalExamTypeId: technicalExamTypeId.value,
      organizationForTechnicalExamId: organizationForTechnicalExamId.value,
      firstInspectorOperatorUserId: firstInspectorOperatorUserId.value,
      secondInspectorOperatorUserId: secondInspectorOperatorUserId.value,
      regNumber: regNumber.value || null,
      madeDate: madeDate.value.toISOString().slice(0, 10),
      validTillDate: validTillDate.value.toISOString().slice(0, 10),
      vehicleIsRight: vehicleIsRight.value,
      explanationNote: explanationNote.value || null,
      driversWarning: driversWarning.value || null,
      note: note.value || null,
      faults: faults.value.map(f => ({
        technicalExamVehiclePartId: f.technicalExamVehiclePartId,
        statusId: f.statusId,
        front: f.front, back: f.back, onLeft: f.onLeft, onRight: f.onRight,
        dateEnter: f.dateEnter.toISOString().slice(0, 10),
        note: f.note || null,
      })),
      visualErrors: visualErrors.value.filter(v => v.description.trim().length > 0),
    }
    await api.post('/technical-exam-reports', payload)
    router.push('/exams')
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

function reset() {
  customerQuery.value = ''; selectedCustomer.value = null; customerResults.value = []
  vehicleQuery.value = ''; selectedVehicle.value = null; vehicleResults.value = []
  technicalExamTypeId.value = null; organizationForTechnicalExamId.value = null
  firstInspectorOperatorUserId.value = null; secondInspectorOperatorUserId.value = null
  regNumber.value = ''; explanationNote.value = ''; driversWarning.value = ''; note.value = ''
  vehicleIsRight.value = true; faults.value = []; visualErrors.value = []
  firstSigned.value = false; secondSigned.value = false
  madeDate.value = new Date(); const d = new Date(); d.setFullYear(d.getFullYear() + 1); validTillDate.value = d
  error.value = null
}

onMounted(load)
</script>

<template>
  <div class="exam-shell">
    <div class="action-bar">
      <Button :label="t('exam.print')" icon="pi pi-print" severity="secondary" outlined disabled />
      <Button :label="t('exam.new')" icon="pi pi-plus" severity="secondary" outlined @click="reset" />
      <Button :label="t('exam.save')" icon="pi pi-save" :loading="saving" @click="save" />
      <Button :label="t('common.cancel')" icon="pi pi-times" severity="secondary" outlined @click="router.back()" />
      <span class="grow" />
      <Button :label="t('exam.exit')" icon="pi pi-sign-out" severity="secondary" text @click="router.push('/exams')" />
    </div>

    <div v-if="error" class="error-banner">{{ error }}</div>

    <div class="card head-card">
      <h2>{{ t('exam.title') }}</h2>

      <div class="grid-2">
        <div class="search-block">
          <label>{{ t('exam.vehicleSearchLabel') }}</label>
          <div class="search-inline">
            <InputText v-model="vehicleQuery" :placeholder="t('exam.vehicleSearchPh')" @input="onVehicleInput" class="w-full" />
            <Button :label="t('exam.newShort')" severity="secondary" outlined size="small" @click="router.push('/vehicles/new')" />
          </div>
          <ul v-if="vehicleResults.length" class="search-list">
            <li v-for="v in vehicleResults" :key="v.id" @click="pickVehicle(v)">{{ vehicleLabel(v) }}</li>
          </ul>
          <div v-if="selectedVehicle" class="picked"><i class="pi pi-check" /> {{ vehicleLabel(selectedVehicle) }}</div>
        </div>

        <div class="search-block">
          <label>{{ t('exam.customerSearchLabel') }}</label>
          <div class="search-inline">
            <InputText v-model="customerQuery" :placeholder="t('exam.customerSearchPh')" @input="onCustomerInput" class="w-full" />
            <Button :label="t('exam.newShort')" severity="secondary" outlined size="small" @click="router.push('/customers/new')" />
          </div>
          <ul v-if="customerResults.length" class="search-list">
            <li v-for="c in customerResults" :key="c.id" @click="pickCustomer(c)">{{ customerLabel(c) }}</li>
          </ul>
          <div v-if="selectedCustomer" class="picked"><i class="pi pi-check" /> {{ customerLabel(selectedCustomer) }}</div>
        </div>
      </div>

      <div class="grid-3">
        <div class="field">
          <label>{{ t('exam.examType') }}</label>
          <Select v-model="technicalExamTypeId" :options="examTypes" optionLabel="name" optionValue="id" :placeholder="t('common.choose')" class="w-full" />
        </div>
        <div class="field">
          <label>{{ t('exam.org') }}</label>
          <Select v-model="organizationForTechnicalExamId" :options="examOrgs" optionLabel="name" optionValue="id" :placeholder="t('common.choose')" class="w-full" />
        </div>
        <div class="field">
          <label>{{ t('exam.regNumber') }}</label>
          <InputText v-model="regNumber" class="w-full" />
        </div>
        <div class="field">
          <label>{{ t('exam.madeDate') }}</label>
          <DatePicker v-model="madeDate" dateFormat="dd.mm.yy" showIcon class="w-full" />
        </div>
        <div class="field">
          <label>{{ t('exam.validTillDate') }}</label>
          <DatePicker v-model="validTillDate" dateFormat="dd.mm.yy" showIcon class="w-full" />
        </div>
        <div class="field" />
      </div>

      <div class="field">
        <label>{{ t('exam.note') }}</label>
        <Textarea v-model="note" rows="2" autoResize class="w-full" />
      </div>
    </div>

    <Tabs value="record">
      <TabList>
        <Tab value="record">{{ t('exam.tabRecord') }}</Tab>
        <Tab value="visual">{{ t('exam.tabVisual') }}</Tab>
        <Tab value="conclusion">{{ t('exam.tabConclusion') }}</Tab>
      </TabList>
      <TabPanels>
        <TabPanel value="record">
          <div class="parts-area">
            <div class="cat-list">
              <div class="cat-list-header">{{ t('exam.partCategories') }}</div>
              <ul>
                <li v-for="c in partsTree" :key="c.id" :class="{ active: c.id === activeCategoryId }" @click="activeCategoryId = c.id">
                  <span>{{ c.name }}</span>
                  <span class="cat-count">{{ c.parts.length }}</span>
                </li>
              </ul>
            </div>
            <div class="parts-grid">
              <div class="parts-grid-header">{{ activeCategory?.name ?? t('exam.partCategoriesPick') }}</div>
              <div class="parts-list">
                <label v-for="p in activeCategory?.parts ?? []" :key="p.id" class="part-item" :class="{ checked: isFaulted(p.id) }">
                  <Checkbox :modelValue="isFaulted(p.id)" :binary="true" @update:modelValue="togglePart(p)" />
                  <span>{{ p.name }}</span>
                </label>
                <div v-if="!activeCategory?.parts?.length" class="empty">{{ t('exam.noParts') }}</div>
              </div>
            </div>
          </div>

          <div class="card faults-card">
            <h3>{{ t('exam.faultsTitle') }}</h3>
            <div v-if="faults.length === 0" class="empty">{{ t('exam.faultsEmpty') }}</div>
            <table v-else class="faults-table">
              <thead>
                <tr>
                  <th>{{ t('exam.fPart') }}</th>
                  <th>{{ t('exam.fStatus') }}</th>
                  <th>{{ t('exam.fDate') }}</th>
                  <th>{{ t('exam.fFront') }}</th>
                  <th>{{ t('exam.fBack') }}</th>
                  <th>{{ t('exam.fLeft') }}</th>
                  <th>{{ t('exam.fRight') }}</th>
                  <th>{{ t('exam.fNote') }}</th>
                  <th />
                </tr>
              </thead>
              <tbody>
                <tr v-for="(f, i) in faults" :key="f.technicalExamVehiclePartId">
                  <td>{{ f.partName }}</td>
                  <td><Select v-model="f.statusId" :options="statuses" optionLabel="name" optionValue="id" class="w-full" /></td>
                  <td><DatePicker v-model="f.dateEnter" dateFormat="dd.mm.yy" showIcon /></td>
                  <td class="c"><Checkbox v-model="f.front" :binary="true" /></td>
                  <td class="c"><Checkbox v-model="f.back" :binary="true" /></td>
                  <td class="c"><Checkbox v-model="f.onLeft" :binary="true" /></td>
                  <td class="c"><Checkbox v-model="f.onRight" :binary="true" /></td>
                  <td><InputText v-model="f.note" class="w-full" /></td>
                  <td><Button icon="pi pi-trash" severity="danger" text @click="removeFault(i)" /></td>
                </tr>
              </tbody>
            </table>
          </div>

          <div class="grid-2">
            <div class="field">
              <label>{{ t('exam.explanation') }}</label>
              <Textarea v-model="explanationNote" rows="3" autoResize class="w-full" />
            </div>
            <div class="field">
              <label>{{ t('exam.driversWarning') }}</label>
              <Textarea v-model="driversWarning" rows="3" autoResize class="w-full" />
            </div>
          </div>
        </TabPanel>

        <TabPanel value="visual">
          <div class="card">
            <div class="row-between">
              <h3>{{ t('exam.visualErrors') }}</h3>
              <Button icon="pi pi-plus" :label="t('common.add')" size="small" @click="addVisualError" />
            </div>
            <div v-if="visualErrors.length === 0" class="empty">{{ t('exam.visualEmpty') }}</div>
            <table v-else class="faults-table">
              <thead><tr><th>{{ t('exam.vDescription') }}</th><th>{{ t('exam.vSeverity') }}</th><th /></tr></thead>
              <tbody>
                <tr v-for="(v, i) in visualErrors" :key="i">
                  <td><InputText v-model="v.description" class="w-full" /></td>
                  <td><InputText v-model="v.severity" class="w-full" /></td>
                  <td><Button icon="pi pi-trash" severity="danger" text @click="removeVisualError(i)" /></td>
                </tr>
              </tbody>
            </table>
          </div>
        </TabPanel>

        <TabPanel value="conclusion">
          <div class="card">
            <div class="grid-2">
              <div class="field">
                <label>{{ t('exam.firstInspector') }}</label>
                <div class="inspector-row">
                  <Select v-model="firstInspectorOperatorUserId" :options="operators" optionLabel="fullName" optionValue="userId" :placeholder="t('common.choose')" class="grow" />
                  <Button :label="t('exam.sign')" :severity="firstSigned ? 'success' : 'secondary'" :disabled="!firstInspectorOperatorUserId" @click="firstSigned = !firstSigned" />
                </div>
              </div>
              <div class="field">
                <label>{{ t('exam.secondInspector') }}</label>
                <div class="inspector-row">
                  <Select v-model="secondInspectorOperatorUserId" :options="operators" optionLabel="fullName" optionValue="userId" :placeholder="t('common.choose')" class="grow" showClear />
                  <Button :label="t('exam.sign')" :severity="secondSigned ? 'success' : 'secondary'" :disabled="!secondInspectorOperatorUserId" @click="secondSigned = !secondSigned" />
                </div>
              </div>
            </div>

            <div class="verdict">
              <Checkbox v-model="vehicleIsRight" :binary="true" inputId="ver" />
              <label for="ver">{{ t('exam.vehicleIsRight') }}</label>
            </div>
          </div>
        </TabPanel>
      </TabPanels>
    </Tabs>
  </div>
</template>

<style scoped>
.exam-shell { display: flex; flex-direction: column; gap: 16px; }
.action-bar { display: flex; gap: 8px; padding: 8px 12px; background: var(--surface-section, #f8fafc); border: 1px solid var(--surface-border); border-radius: 8px; align-items: center; }
.grow { flex: 1; }
.card { background: var(--surface-card, #fff); border: 1px solid var(--surface-border); border-radius: 8px; padding: 16px; }
.head-card h2 { margin: 0 0 16px 0; font-size: 18px; }
.error-banner { background: #fee2e2; color: #991b1b; padding: 10px 14px; border-radius: 6px; }
.grid-2 { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 14px; }
.grid-3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 16px; margin-bottom: 14px; }
.field { display: flex; flex-direction: column; gap: 4px; }
.field label { font-size: 12px; color: var(--text-color-secondary); font-weight: 500; }
.search-block { position: relative; }
.search-block label { font-size: 12px; color: var(--text-color-secondary); font-weight: 500; display: block; margin-bottom: 4px; }
.search-inline { display: flex; gap: 6px; }
.search-list { list-style: none; padding: 0; margin: 4px 0 0 0; max-height: 220px; overflow: auto; border: 1px solid var(--surface-border); border-radius: 6px; background: var(--color-surface, #fff); position: absolute; z-index: 30; left: 0; right: 0; }
.search-list li { padding: 8px 12px; cursor: pointer; }
.search-list li:hover { background: var(--surface-100, #f1f5f9); }
.picked { margin-top: 6px; font-size: 13px; color: #16a34a; }
.parts-area { display: grid; grid-template-columns: 280px 1fr; gap: 12px; margin-top: 12px; }
.cat-list, .parts-grid { background: var(--surface-card, #fff); border: 1px solid var(--surface-border); border-radius: 8px; }
.cat-list-header, .parts-grid-header { padding: 10px 14px; background: var(--surface-section, #f8fafc); font-weight: 600; border-bottom: 1px solid var(--surface-border); border-top-left-radius: 8px; border-top-right-radius: 8px; }
.cat-list ul { list-style: none; margin: 0; padding: 4px; max-height: 460px; overflow: auto; }
.cat-list li { display: flex; justify-content: space-between; align-items: center; padding: 8px 10px; border-radius: 4px; cursor: pointer; }
.cat-list li:hover { background: var(--surface-100, #f1f5f9); }
.cat-list li.active { background: var(--primary-color, #3b82f6); color: #fff; }
.cat-count { font-size: 11px; opacity: 0.6; }
.parts-list { padding: 10px; display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: 6px; max-height: 460px; overflow: auto; }
.part-item { display: flex; align-items: center; gap: 8px; padding: 6px 8px; border-radius: 4px; cursor: pointer; }
.part-item:hover { background: var(--surface-100, #f1f5f9); }
.part-item.checked { background: #fef2f2; color: #991b1b; }
.empty { color: var(--text-color-secondary); padding: 12px; font-size: 13px; }
.faults-card { margin-top: 12px; }
.faults-card h3 { margin: 0 0 10px 0; font-size: 15px; }
.faults-table { width: 100%; border-collapse: collapse; }
.faults-table th, .faults-table td { padding: 6px 8px; border-bottom: 1px solid var(--surface-border); text-align: left; vertical-align: middle; }
.faults-table th { font-size: 12px; color: var(--text-color-secondary); background: var(--surface-section, #f8fafc); }
.faults-table td.c { text-align: center; }
.row-between { display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px; }
.inspector-row { display: flex; gap: 8px; }
.verdict { margin-top: 16px; display: flex; gap: 10px; align-items: center; padding: 12px; background: #f0fdf4; border: 1px solid #bbf7d0; border-radius: 6px; }
</style>
