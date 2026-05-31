<script setup lang="ts">
import { ref, onMounted, watch, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useToast } from '@/composables/useToast'
import Button from 'primevue/button'
import Select from 'primevue/select'
import Textarea from 'primevue/textarea'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import DatePicker from 'primevue/datepicker'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import SplitButton from 'primevue/splitbutton'
import {
  useTechExamOrgs,
  useVehicleOwnershipProofTypes, usePaymentProofTypes,
} from '@/composables/useReferenceData'

interface CustomerOpt { id: number; firstName: string; surname: string | null; embg: string | null }
interface VehicleOpt  { id: number; shellNumber: string; lastRegistrationNumber: string; makerName?: string | null; modelName?: string | null }
interface RequestType { id: number; typeName: string; isNewCustomer: boolean; isPreviousRegistrationRequired: boolean }
interface OwnershipProofRow { id?: number; ownershipProofId: number | null; number: string | null; dateIssued: Date | null; note: string | null }
interface PaymentProofRow   { id?: number; paymentProofId: number | null; number: string | null; amount: number | null; dateIssued: Date | null; note: string | null }

const { t } = useI18n()
const router = useRouter()
const route = useRoute()
const toast = useToast()

const id = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!id.value)

const customers    = ref<CustomerOpt[]>([])
const vehicles     = ref<VehicleOpt[]>([])
const requestTypes = ref<RequestType[]>([])
const techExamOrgs        = useTechExamOrgs()
const ownershipProofTypes = useVehicleOwnershipProofTypes()
const paymentProofTypes   = usePaymentProofTypes()

const form = ref({
  requestTypeId: null as number | null,
  customerId: null as number | null,
  vehicleId: null as number | null,
  newCustomerId: null as number | null,
  technicalExamOrganizationId: null as number | null,
  note: '',
  dateEnded: null as string | null,
})

const ownershipProofs = ref<OwnershipProofRow[]>([])
const paymentProofs   = ref<PaymentProofRow[]>([])

const loading = ref(true)
const saving  = ref(false)
const error   = ref<string | null>(null)

function toDate(s: string | null | undefined): Date | null { return s ? new Date(s) : null }
function fromDate(d: Date | null): string | null { return d ? d.toISOString().slice(0, 10) : null }

const customerLabel = (c: CustomerOpt) => `${c.surname ?? ''} ${c.firstName}${c.embg ? ` · ${c.embg}` : ''}`.trim()
const vehicleLabel  = (v: VehicleOpt)  => {
  const maker = [v.makerName, v.modelName].filter(Boolean).join(' ')
  return [v.lastRegistrationNumber, v.shellNumber, maker].filter(Boolean).join(' · ')
}

const selectedRequestType = computed(() => requestTypes.value.find(r => r.id === form.value.requestTypeId))
const showNewOwner = computed(() => !!selectedRequestType.value?.isNewCustomer)
const isClosed    = computed(() => !!form.value.dateEnded)

async function load() {
  loading.value = true
  try {
    interface Paged<T> { items: T[]; total: number }
    const [c, v, rt] = await Promise.all([
      api.get<Paged<CustomerOpt>>('/customers', { params: { take: 200 } }),
      api.get<Paged<VehicleOpt>>('/vehicles',  { params: { take: 200 } }),
      api.get<RequestType[]>('/request-types'),
    ])
    customers.value = c.data.items
    vehicles.value  = v.data.items
    requestTypes.value = rt.data

    if (isEdit.value) {
      const { data } = await api.get(`/requests/${id.value}`)
      form.value.requestTypeId               = data.request.requestTypeId
      form.value.customerId                  = data.customerId
      form.value.vehicleId                   = data.vehicleId
      form.value.technicalExamOrganizationId = data.request.technicalExamOrganizationId
      form.value.note                        = data.request.note ?? ''
      form.value.dateEnded                   = data.request.dateEnded

      // Ensure the currently-bound customer + vehicle appear in the dropdowns even if they
      // aren't in the first 200 preloaded rows.
      if (data.customerId && !customers.value.some(x => x.id === data.customerId)) {
        const r = await api.get(`/customers/${data.customerId}`); customers.value.unshift(r.data)
      }
      if (data.vehicleId && !vehicles.value.some(x => x.id === data.vehicleId)) {
        const r = await api.get(`/vehicles/${data.vehicleId}`); vehicles.value.unshift(r.data.vehicle ?? r.data)
      }

      ownershipProofs.value = (data.ownershipProofs ?? []).map((p: any) => ({ id: p.id, ownershipProofId: p.ownershipProofId, number: p.number, dateIssued: toDate(p.dateIssued), note: p.note }))
      paymentProofs.value   = (data.paymentProofs   ?? []).map((p: any) => ({ id: p.id, paymentProofId:   p.paymentProofId,   number: p.number, amount: p.amount, dateIssued: toDate(p.dateIssued), note: p.note }))
    } else {
      const tid = Number(route.query.typeId)
      if (tid && requestTypes.value.some(r => r.id === tid)) form.value.requestTypeId = tid
    }
  } finally { loading.value = false }
}

async function save() {
  error.value = null
  if (!form.value.requestTypeId) { error.value = t('requests.errType'); return }
  if (!isEdit.value) {
    if (!form.value.customerId) { error.value = t('requests.errCustomer'); return }
    if (!form.value.vehicleId)  { error.value = t('requests.errVehicle'); return }
    if (showNewOwner.value && !form.value.newCustomerId) {
      error.value = 'Потребно е да се внесе новиот сопственик.'
      return
    }
  }

  saving.value = true
  try {
    const ownPayload = ownershipProofs.value
      .filter(p => p.ownershipProofId)
      .map(p => ({ ...p, dateIssued: fromDate(p.dateIssued) }))
    const payPayload = paymentProofs.value
      .filter(p => p.paymentProofId)
      .map(p => ({ ...p, dateIssued: fromDate(p.dateIssued) }))

    if (isEdit.value) {
      await api.put(`/requests/${id.value}`, {
        relationTypeId: null,
        newCustomerVehicleRelationId: null,
        previousRegistrationId: null,
        technicalExamOrganizationId: form.value.technicalExamOrganizationId,
        note: form.value.note || null,
        ownershipProofs: ownPayload,
        paymentProofs: payPayload,
      })
      toast.ok(t('common.saved'))
    } else {
      const { data } = await api.post('/requests', {
        requestTypeId: form.value.requestTypeId,
        customerId: form.value.customerId,
        vehicleId: form.value.vehicleId,
        newCustomerId: showNewOwner.value ? form.value.newCustomerId : null,
        relationTypeId: null,
        technicalExamOrganizationId: form.value.technicalExamOrganizationId,
        note: form.value.note || null,
        ownershipProofs: ownPayload,
        paymentProofs: payPayload,
      })
      router.replace(`/requests/${data.id}`)
      toast.ok(t('common.saved'))
      return
    }
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

// Server-side search for the vehicle + customer dropdowns. PrimeVue's local filter runs
// against the current :options the moment a key is pressed — that would flash "No results"
// while we wait for the server. So we flip a loading ref the instant the user types
// (Select shows a spinner in place of the empty message), debounce the server query 150ms,
// and ignore stale responses. Selected option is pinned so the chip stays visible if the
// user clears the filter and reopens.
const vehiclesLoading  = ref(false)
const customersLoading = ref(false)
let vehicleDebounce:  number | undefined
let customerDebounce: number | undefined
let vehicleSeq = 0
let customerSeq = 0
async function searchVehicles(q: string) {
  const seq = ++vehicleSeq
  vehiclesLoading.value = true
  try {
    const r = await api.get<{ items: VehicleOpt[] }>('/vehicles', { params: { q: q || undefined, take: 200 } })
    if (seq !== vehicleSeq) return
    const selected = vehicles.value.find(x => x.id === form.value.vehicleId)
    const items = r.data.items
    vehicles.value = selected && !items.some(i => i.id === selected.id) ? [selected, ...items] : items
  } finally {
    if (seq === vehicleSeq) vehiclesLoading.value = false
  }
}
async function searchCustomers(q: string) {
  const seq = ++customerSeq
  customersLoading.value = true
  try {
    const r = await api.get<{ items: CustomerOpt[] }>('/customers', { params: { q: q || undefined, take: 200 } })
    if (seq !== customerSeq) return
    const cur = customers.value.find(x => x.id === form.value.customerId)
    const nw  = customers.value.find(x => x.id === form.value.newCustomerId)
    const items = r.data.items
    const pinned = [cur, nw].filter((x): x is CustomerOpt => !!x && !items.some(i => i.id === x.id))
    customers.value = pinned.length ? [...pinned, ...items] : items
  } finally {
    if (seq === customerSeq) customersLoading.value = false
  }
}
function onVehicleFilter(ev: { value: string }) {
  vehiclesLoading.value = true
  window.clearTimeout(vehicleDebounce)
  vehicleDebounce = window.setTimeout(() => searchVehicles(ev.value), 150)
}

// When the user picks a vehicle, mirror the legacy uxRequestEdit behaviour:
// resolve the vehicle's current owner via the relation table and pin them
// into the customer dropdown so the right-hand "Сопственик" select fills in.
// In edit mode we don't auto-overwrite (the request already has its CVR set).
async function onVehicleSelected(vehId: number | null) {
  if (!vehId || isEdit.value) return
  try {
    const rels = await api.get<{ id: number; customerId: number; vehicleId: number; isActive: boolean }[]>(
      '/customer-vehicle-relations', { params: { vehicleId: vehId } })
    const active = rels.data.find(r => r.isActive) ?? rels.data[0]
    if (!active) return
    if (!customers.value.some(c => c.id === active.customerId)) {
      const r = await api.get<CustomerOpt>(`/customers/${active.customerId}`)
      customers.value = [r.data, ...customers.value]
    }
    form.value.customerId = active.customerId
  } catch { /* leave the customer empty if lookup fails */ }
}
function onCustomerFilter(ev: { value: string }) {
  customersLoading.value = true
  window.clearTimeout(customerDebounce)
  customerDebounce = window.setTimeout(() => searchCustomers(ev.value), 150)
}

function addOwn() { ownershipProofs.value.push({ ownershipProofId: null, number: '', dateIssued: null, note: null }) }
function removeOwn(i: number) { ownershipProofs.value.splice(i, 1) }
function addPay() { paymentProofs.value.push({ paymentProofId: null, number: '', amount: null, dateIssued: null, note: null }) }
function removePay(i: number) { paymentProofs.value.splice(i, 1) }

async function printPdf(kind: 'plav' | 'bel' | 'zelen') {
  if (!id.value) return
  try {
    const resp = await api.get(`/requests/${id.value}/print/${kind}`, { responseType: 'blob' })
    const blob = new Blob([resp.data], { type: 'application/pdf' })
    const url = URL.createObjectURL(blob)
    window.open(url, '_blank', 'noopener')
    setTimeout(() => URL.revokeObjectURL(url), 60_000)
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Print failed.'
  }
}
const printMenu = [
  { label: t('requests.printPlav'),  icon: 'pi pi-file-pdf', command: () => printPdf('plav')  },
  { label: t('requests.printBel'),   icon: 'pi pi-file-pdf', command: () => printPdf('bel')   },
  { label: t('requests.printZelen'), icon: 'pi pi-file-pdf', command: () => printPdf('zelen') },
]

onMounted(load)
watch(id, load)
watch(() => form.value.vehicleId, (newVehId, oldVehId) => {
  if (newVehId && newVehId !== oldVehId) onVehicleSelected(newVehId)
})
</script>

<template>
  <div class="request-form">
    <div class="page-header">
      <div>
        <h1 v-if="selectedRequestType">{{ selectedRequestType.typeName }}</h1>
        <h1 v-else>{{ isEdit ? t('requests.edit') : t('requests.new') }}</h1>
        <div class="subtitle">
          <Tag v-if="isClosed" :value="t('requests.closed')" severity="secondary" />
          <span v-if="!selectedRequestType" class="muted">{{ t('requests.subtitle') }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>

    <template v-else>
      <!-- Request type -->
      <div class="card">
        <div class="card-header">{{ t('requests.section.workflow') }}</div>
        <div class="card-body">
          <div class="field">
            <label>{{ t('requests.type') }} *</label>
            <Select v-model="form.requestTypeId" :options="requestTypes" optionLabel="typeName" optionValue="id"
                    :placeholder="t('requests.pickType')" filter :disabled="isEdit" />
            <div v-if="selectedRequestType" class="flags">
              <span v-if="selectedRequestType.isNewCustomer" class="flag flag-info"><i class="pi pi-user-plus" />{{ t('requests.flagNewCustomer') }}</span>
              <span v-if="selectedRequestType.isPreviousRegistrationRequired" class="flag flag-warn"><i class="pi pi-history" />{{ t('requests.flagPrevReg') }}</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Existing relation -->
      <div class="card">
        <div class="card-header">{{ t('requests.section.relation') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field">
              <label>{{ t('requests.vehicleSearch') }}</label>
              <Select v-model="form.vehicleId" :options="vehicles" :optionLabel="vehicleLabel" optionValue="id"
                      :placeholder="t('requests.pickVehicle')" filter showClear :disabled="isEdit"
                      :loading="vehiclesLoading" :emptyFilterMessage="t('common.searching')"
                      @filter="onVehicleFilter" />
            </div>
            <div class="field">
              <label>{{ t('requests.customerSearch') }}</label>
              <Select v-model="form.customerId" :options="customers" :optionLabel="customerLabel" optionValue="id"
                      :placeholder="t('requests.pickCustomer')" filter showClear :disabled="isEdit"
                      :loading="customersLoading" :emptyFilterMessage="t('common.searching')"
                      @filter="onCustomerFilter" />
            </div>
          </div>
        </div>
      </div>

      <!-- Conditional new owner section -->
      <div v-if="showNewOwner" class="card">
        <div class="card-header">{{ t('requests.section.newOwner') }}</div>
        <div class="card-body">
          <div class="field">
            <label>{{ t('requests.newOwnerSearch') }}</label>
            <Select v-model="form.newCustomerId" :options="customers" :optionLabel="customerLabel" optionValue="id"
                    :placeholder="t('requests.pickCustomer')" filter showClear
                    :loading="customersLoading" :emptyFilterMessage="t('common.searching')"
                    @filter="onCustomerFilter" />
          </div>
        </div>
      </div>

      <!-- Technical exam org -->
      <div class="card">
        <div class="card-header">{{ t('requests.examOrg') }}</div>
        <div class="card-body">
          <div class="field">
            <Select v-model="form.technicalExamOrganizationId" :options="techExamOrgs" optionLabel="name" optionValue="id"
                    :placeholder="'—'" filter showClear />
          </div>
        </div>
      </div>

      <!-- Attached documents (BOTH create and edit modes) -->
      <div class="card">
        <div class="card-header">{{ t('requests.section.attachments') }}</div>
        <div class="card-body">
          <div class="proof-row">
            <div class="proof-col">
              <div class="proof-head">
                <span>{{ t('requests.section.ownershipProofs') }}</span>
                <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined :disabled="isClosed" @click="addOwn" />
              </div>
              <DataTable :value="ownershipProofs" v-if="ownershipProofs.length" stripedRows size="small">
                <Column :header="t('requests.proofType')" style="width: 38%">
                  <template #body="{ data }">
                    <Select v-model="data.ownershipProofId" :options="ownershipProofTypes" optionLabel="name" optionValue="id" :placeholder="'—'" filter showClear size="small" :disabled="isClosed" />
                  </template>
                </Column>
                <Column :header="t('requests.proofNumber')">
                  <template #body="{ data }"><InputText v-model="data.number" maxlength="100" size="small" :disabled="isClosed" /></template>
                </Column>
                <Column :header="t('common.actions')" style="width: 60px">
                  <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" :disabled="isClosed" @click="removeOwn(index)" /></template>
                </Column>
              </DataTable>
              <div v-else class="empty proof-empty"><i class="pi pi-inbox" />{{ t('requests.noOwnershipProofs') }}</div>
            </div>

            <div class="proof-col">
              <div class="proof-head">
                <span>{{ t('requests.section.paymentProofs') }}</span>
                <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined :disabled="isClosed" @click="addPay" />
              </div>
              <DataTable :value="paymentProofs" v-if="paymentProofs.length" stripedRows size="small">
                <Column :header="t('requests.proofType')" style="width: 38%">
                  <template #body="{ data }">
                    <Select v-model="data.paymentProofId" :options="paymentProofTypes" optionLabel="name" optionValue="id" :placeholder="'—'" filter showClear size="small" :disabled="isClosed" />
                  </template>
                </Column>
                <Column :header="t('requests.proofNumber')">
                  <template #body="{ data }"><InputText v-model="data.number" maxlength="100" size="small" :disabled="isClosed" /></template>
                </Column>
                <Column :header="t('common.actions')" style="width: 60px">
                  <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" :disabled="isClosed" @click="removePay(index)" /></template>
                </Column>
              </DataTable>
              <div v-else class="empty proof-empty"><i class="pi pi-inbox" />{{ t('requests.noPaymentProofs') }}</div>
            </div>
          </div>

          <div class="field note-row">
            <label>{{ t('common.note') }}</label>
            <Textarea v-model="form.note" rows="2" autoResize maxlength="250" />
          </div>
        </div>
      </div>

      <div v-if="error" class="error">{{ error }}</div>

      <div class="footer-actions">
        <SplitButton :label="t('requests.print')" severity="secondary" outlined size="small"
                     icon="pi pi-print" :model="printMenu" :disabled="!isEdit"
                     @click="printPdf('plav')" />
        <Button :label="t('common.cancel')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/requests')" />
        <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" :disabled="isClosed" @click="save" />
      </div>
    </template>
  </div>
</template>

<style scoped>
.request-form { max-width: 1300px; margin: 0 auto; padding-bottom: 3.5rem; }
.request-form :deep(.page-header)             { margin-bottom: 0.75rem; }
.request-form :deep(.page-header h1)          { font-size: 1.125rem; letter-spacing: -0.01em; }
.request-form :deep(.page-header .subtitle)   { font-size: 0.75rem; display: flex; align-items: center; gap: 0.5rem; }
.muted { color: var(--color-text-muted); }

.request-form :deep(.card + .card)            { margin-top: 0.5rem; }
.request-form :deep(.card .card-header) {
  position: relative; padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.request-form :deep(.card .card-header)::before {
  content: ''; position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}
.request-form :deep(.card .card-body)         { padding: 0.625rem 0.875rem; }
.request-form :deep(.row)                     { gap: 0.625rem; }
.request-form :deep(.field)                   { gap: 0.15rem; margin-bottom: 0.45rem; }
.request-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; }
.request-form :deep(.p-inputtext),
.request-form :deep(.p-select),
.request-form :deep(.p-textarea),
.request-form :deep(.p-datepicker)            { width: 100%; }
.request-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.request-form :deep(.p-select-label)          { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.request-form :deep(.p-datatable-tbody td),
.request-form :deep(.p-datatable-thead th)    { padding: 0.25rem 0.5rem; font-size: 0.8125rem; }

.flags { display: flex; gap: 0.5rem; margin-top: 0.5rem; flex-wrap: wrap; }
.flag { display: inline-flex; align-items: center; gap: 0.375rem; padding: 0.25rem 0.625rem; border-radius: 999px; font-size: 0.75rem; font-weight: 500; }
.flag i { font-size: 0.7rem; }
.flag-info { background: var(--color-info-50); color: var(--color-brand-700); border: 1px solid var(--color-brand-200); }
.flag-warn { background: var(--color-warn-50); color: var(--color-warn-600); border: 1px solid #fde68a; }

/* Two side-by-side proof tables inside the Attached documents card */
.proof-row { display: flex; gap: 0.625rem; align-items: stretch; }
.proof-col { flex: 1; display: flex; flex-direction: column; gap: 0.375rem; }
.proof-head {
  display: flex; align-items: center; justify-content: space-between;
  padding: 0.25rem 0.375rem; background: var(--color-surface-2);
  border: 1px solid var(--color-border); border-radius: var(--radius-sm);
  font-size: 0.75rem; font-weight: 600; color: var(--color-text-secondary);
}
.proof-empty {
  padding: 0.875rem 0.5rem; text-align: center; font-size: 0.75rem;
  color: var(--color-text-muted); background: var(--color-bg);
  border: 1px dashed var(--color-border); border-radius: var(--radius-sm);
}
.note-row { margin-top: 0.625rem; }

.footer-actions {
  position: fixed; bottom: 0; left: var(--sidebar-w); right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border); z-index: 5;
}
</style>
