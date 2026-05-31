<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useToast, useConfirmDialog } from '@/composables/useToast'
import { formatMoney as fmtMoney } from '@/utils/format'
import Button from 'primevue/button'
import Select from 'primevue/select'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import DatePicker from 'primevue/datepicker'
import Textarea from 'primevue/textarea'
import Checkbox from 'primevue/checkbox'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import SplitButton from 'primevue/splitbutton'
import { usePaymentTypes } from '@/composables/useReferenceData'

interface CustomerOpt { id: number; firstName: string; surname: string | null; embg: string | null }
interface VehicleOpt  { id: number; shellNumber: string; lastRegistrationNumber: string; makerName?: string | null; modelName?: string | null }
interface PaymentTypeOpt { id: number; name: string; isInvoice: boolean; isCash: boolean; isFiscalCard: boolean; isAccount: boolean; isInstallments: boolean; prefix?: string | null }
interface PaymentItemOpt { id: number; paymentCategoryId: number; vehicleCategoryForPaymentsId: number | null; itemName: string }

interface DetailRow {
  id?: number
  paymentItemId: number | null
  itemName: string
  price: number
  ddvRate: number
  discountPercent: number
  prePayed: boolean
  note: string | null
}
interface InstallmentRow {
  id?: number
  installmentNumber: number
  price: number
  dueDate: Date | null
  payed: boolean
  datePayed: Date | null
}

const { t } = useI18n()
const route = useRoute()
const router = useRouter()
const toast = useToast()
const { ask } = useConfirmDialog()

const paymentTypes = usePaymentTypes() as unknown as { value: PaymentTypeOpt[] }

const id = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!id.value)

const customers = ref<CustomerOpt[]>([])
const vehicles  = ref<VehicleOpt[]>([])
const items     = ref<PaymentItemOpt[]>([])

const form = ref({
  paymentTypeId: null as number | null,
  customerId: null as number | null,
  vehicleId: null as number | null,
  customerVehicleRelationId: null as number | null,
  datePay: new Date() as Date | null,
  dateRequired: new Date() as Date | null,
  discountPercent: 0,
  note: '',
  payed: false,
  storno: false,
  documentNumber: null as string | null,
})

const details = ref<DetailRow[]>([])
const installments = ref<InstallmentRow[]>([])

const guarantor = ref({
  contractNumber: '',
  contractDate: new Date() as Date | null,
  numberOfInstallments: 3,
  firstInstallment: 0,
  firstInstallmentDate: new Date() as Date | null,
  guarantorName: '',
  guarantorAddress: '',
  guarantorEMBG: '',
})

const loading = ref(true)
const saving = ref(false)
const error = ref<string | null>(null)

// Pickers (server-side searched)
const vehiclesLoading  = ref(false)
const customersLoading = ref(false)
let vehicleDebounce: number | undefined
let customerDebounce: number | undefined

async function searchVehicles(q: string) {
  vehiclesLoading.value = true
  try {
    const r = await api.get<{ items: VehicleOpt[] }>('/vehicles', { params: { q: q || undefined, take: 200 } })
    const selected = vehicles.value.find(x => x.id === form.value.vehicleId)
    const list = r.data.items
    vehicles.value = selected && !list.some(i => i.id === selected.id) ? [selected, ...list] : list
  } finally { vehiclesLoading.value = false }
}
async function searchCustomers(q: string) {
  customersLoading.value = true
  try {
    const r = await api.get<{ items: CustomerOpt[] }>('/customers', { params: { q: q || undefined, take: 200 } })
    const cur = customers.value.find(x => x.id === form.value.customerId)
    const list = r.data.items
    customers.value = cur && !list.some(i => i.id === cur.id) ? [cur, ...list] : list
  } finally { customersLoading.value = false }
}
function onVehicleFilter(ev: { value: string })  { vehiclesLoading.value  = true; clearTimeout(vehicleDebounce);  vehicleDebounce  = window.setTimeout(() => searchVehicles(ev.value),  150) }
function onCustomerFilter(ev: { value: string }) { customersLoading.value = true; clearTimeout(customerDebounce); customerDebounce = window.setTimeout(() => searchCustomers(ev.value), 150) }

const customerLabel = (c: CustomerOpt) => `${c.surname ?? ''} ${c.firstName}${c.embg ? ` · ${c.embg}` : ''}`.trim()
const vehicleLabel  = (v: VehicleOpt)  => [v.lastRegistrationNumber, v.shellNumber, [v.makerName, v.modelName].filter(Boolean).join(' ')].filter(Boolean).join(' · ')

const selectedType = computed<PaymentTypeOpt | undefined>(() => paymentTypes.value?.find(p => p.id === form.value.paymentTypeId))
const showInstallments = computed(() => !!selectedType.value?.isInstallments)

const grandTotal = computed(() => {
  const lines = details.value
    .filter(d => !d.prePayed)
    .reduce((sum, d) => sum + d.price * (1 - (d.discountPercent || 0) / 100), 0)
  return Math.round(lines * (1 - (form.value.discountPercent || 0) / 100) * 100) / 100
})

const prePayedTotal = computed(() =>
  details.value.filter(d => d.prePayed).reduce((s, d) => s + d.price, 0))

// Load on mount
async function load() {
  loading.value = true
  try {
    // Preload some vehicles + customers + items
    const [c, v, i] = await Promise.all([
      api.get<{ items: CustomerOpt[] }>('/customers', { params: { take: 100 } }),
      api.get<{ items: VehicleOpt[] }>('/vehicles', { params: { take: 100 } }),
      api.get<PaymentItemOpt[]>('/ref/payment-items'),
    ])
    customers.value = c.data.items
    vehicles.value  = v.data.items
    items.value     = i.data

    if (isEdit.value) {
      const d = (await api.get(`/payment-documents/${id.value}`)).data
      form.value.paymentTypeId = d.paymentTypeId
      form.value.customerId = d.customerId
      form.value.vehicleId = d.vehicleId
      form.value.customerVehicleRelationId = d.customerVehicleRelationId
      form.value.datePay = d.datePay ? new Date(d.datePay) : null
      form.value.dateRequired = d.dateRequired ? new Date(d.dateRequired) : null
      form.value.discountPercent = d.discountPercent ?? 0
      form.value.note = d.note ?? ''
      form.value.payed = d.payed
      form.value.storno = d.storno
      form.value.documentNumber = d.documentNumber
      details.value = (d.details || []).map((x: any) => ({
        id: x.id, paymentItemId: x.paymentItemId, itemName: x.itemName ?? '',
        price: x.price, ddvRate: x.ddvRate, discountPercent: x.discountPercent,
        prePayed: x.prePayed, note: x.note ?? '',
      }))
      installments.value = (d.installments || []).map((x: any) => ({
        id: x.id, installmentNumber: x.installmentNumber, price: x.price,
        dueDate: x.dueDate ? new Date(x.dueDate) : null,
        payed: x.payed, datePayed: x.datePayed ? new Date(x.datePayed) : null,
      }))

      // hydrate customer/vehicle if not in loaded list
      if (form.value.customerId && !customers.value.some(c => c.id === form.value.customerId)) {
        const r = await api.get<CustomerOpt>(`/customers/${form.value.customerId}`); customers.value.unshift(r.data)
      }
      if (form.value.vehicleId && !vehicles.value.some(v => v.id === form.value.vehicleId)) {
        const r = await api.get(`/vehicles/${form.value.vehicleId}`); vehicles.value.unshift(r.data.vehicle ?? r.data)
      }
    }
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Load failed.'
  } finally { loading.value = false }
}

// When the user picks a vehicle on a new document, resolve its current owner
// + relation id so the customer dropdown auto-fills (legacy parity).
async function onVehicleSelected(vehId: number | null) {
  if (!vehId || isEdit.value) return
  try {
    const rels = await api.get<{ id: number; customerId: number; vehicleId: number; isActive: boolean }[]>(
      '/customer-vehicle-relations', { params: { vehicleId: vehId } })
    const active = rels.data.find(r => r.isActive) ?? rels.data[0]
    if (!active) return
    form.value.customerVehicleRelationId = active.id
    if (!customers.value.some(c => c.id === active.customerId)) {
      const r = await api.get<CustomerOpt>(`/customers/${active.customerId}`); customers.value.unshift(r.data)
    }
    form.value.customerId = active.customerId
  } catch {}
}

// Auto-calculation: hits POST /payment-documents/calculate with the selected
// vehicle + Request trigger, then merges the proposed lines into `details`.
async function autoCalculate() {
  if (!form.value.vehicleId) { toast.error('Прво изберете возило.'); return }
  try {
    const r = await api.post<{ lines: any[]; warnings: string[] }>(
      '/payment-documents/calculate',
      { vehicleId: form.value.vehicleId, trigger: 'Request' })
    const newLines: DetailRow[] = r.data.lines.filter(l => !l.isOptional).map(l => ({
      paymentItemId: l.paymentItemId, itemName: `${l.categoryName} · ${l.itemName}${l.parametarName ? ' · ' + l.parametarName : ''}`,
      price: l.price, ddvRate: l.ddvRate, discountPercent: 0, prePayed: false, note: l.matchedVehicleField ? `${l.matchedVehicleField}=${l.matchedVehicleValue}` : null,
    }))
    if (newLines.length === 0) toast.info?.('Нема линии за автоматска пресметка.')
    details.value = [...details.value, ...newLines]
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Auto-calc failed.'
  }
}

function addLine() { details.value.push({ paymentItemId: null, itemName: '', price: 0, ddvRate: 0, discountPercent: 0, prePayed: false, note: null }) }
function removeLine(i: number) { details.value.splice(i, 1) }

function onItemPicked(row: DetailRow, picked: PaymentItemOpt | null) {
  if (!picked) return
  row.itemName = picked.itemName
  // If client has it cached, pre-fill price from first parametar (server is authoritative)
}

function addInstallment() {
  installments.value.push({ installmentNumber: installments.value.length + 1, price: 0, dueDate: null, payed: false, datePayed: null })
}
function removeInstallment(i: number) {
  installments.value.splice(i, 1)
  installments.value.forEach((x, idx) => x.installmentNumber = idx + 1)
}

function fromDate(d: Date | null) { return d ? d.toISOString().slice(0, 10) : null }

async function save() {
  if (!form.value.paymentTypeId)            { error.value = 'Изберете тип на плаќање.'; return }
  if (!form.value.customerVehicleRelationId) { error.value = 'Релацијата сопственик-возило не е поставена.'; return }
  if (details.value.length === 0)            { error.value = 'Внесете барем една ставка.'; return }
  saving.value = true; error.value = null
  try {
    const body: any = {
      paymentTypeId: form.value.paymentTypeId,
      customerVehicleRelationId: form.value.customerVehicleRelationId,
      datePay: fromDate(form.value.datePay),
      dateRequired: fromDate(form.value.dateRequired),
      discountPercent: form.value.discountPercent,
      note: form.value.note || null,
      details: details.value.map(d => ({
        priceCatalogId: d.paymentItemId ?? 0,  // legacy compat; real link is PaymentItemId
        paymentItemId: d.paymentItemId,
        price: d.price, ddvRate: d.ddvRate, discountPercent: d.discountPercent,
        prePayed: d.prePayed, note: d.note,
      })),
    }
    if (showInstallments.value && !isEdit.value) {
      body.guarantor = {
        contractNumber: guarantor.value.contractNumber,
        contractDate: fromDate(guarantor.value.contractDate),
        numberOfInstallments: guarantor.value.numberOfInstallments,
        firstInstallment: guarantor.value.firstInstallment,
        firstInstallmentDate: fromDate(guarantor.value.firstInstallmentDate),
        guarantorName: guarantor.value.guarantorName || null,
        guarantorAddress: guarantor.value.guarantorAddress || null,
        guarantorEMBG: guarantor.value.guarantorEMBG || null,
      }
    }
    if (isEdit.value) {
      await api.put(`/payment-documents/${id.value}`, body)
      toast.ok(t('common.saved'))
      await load()
    } else {
      const r = await api.post<{ id: number }>('/payment-documents', body)
      toast.ok(t('common.saved'))
      router.replace(`/payments/${r.data.id}`)
    }
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Save failed.'
  } finally { saving.value = false }
}

async function markPayed() {
  if (!id.value) return
  await api.post(`/payment-documents/${id.value}/pay`)
  toast.ok(t('common.saved')); await load()
}

async function doStorno() {
  if (!id.value) return
  ask({
    message: t('payments.confirmStorno') ?? 'Storno?', header: t('payments.storno') ?? 'Storno',
    danger: true, acceptLabel: t('payments.storno') ?? 'Storno', rejectLabel: t('common.cancel'),
    onAccept: async () => {
      await api.post(`/payment-documents/${id.value}/storno`); toast.ok(t('common.saved')); await load()
    },
  })
}

const formatMoney = (n: number | null | undefined) => fmtMoney(n, '')

onMounted(load)
watch(id, load)
watch(() => form.value.vehicleId, (newId, oldId) => { if (newId && newId !== oldId) onVehicleSelected(newId) })

const printMenu = computed(() => id.value ? [
  { label: t('payments.printInvoice') ?? 'Фактура',   icon: 'pi pi-file-pdf', command: () => printDoc('invoice') },
  { label: t('payments.printSmetka')  ?? 'Сметка',   icon: 'pi pi-file-pdf', command: () => printDoc('smetka') },
  { label: t('payments.printRata')    ?? 'Рата',     icon: 'pi pi-file-pdf', command: () => printDoc('rata') },
  { label: t('payments.printDogovor') ?? 'Договор',  icon: 'pi pi-file-pdf', command: () => printDoc('dogovor') },
] : [])
async function printDoc(kind: string) {
  if (!id.value) return
  try {
    const resp = await api.get(`/payment-documents/${id.value}/print/${kind}`, { responseType: 'blob' })
    const url = URL.createObjectURL(new Blob([resp.data], { type: 'application/pdf' }))
    window.open(url, '_blank', 'noopener')
    setTimeout(() => URL.revokeObjectURL(url), 60_000)
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? 'Print failed.'
  }
}
</script>

<template>
  <div class="payment-form">
    <header class="page-header">
      <div>
        <h1 v-if="form.documentNumber">{{ form.documentNumber }}</h1>
        <h1 v-else>{{ isEdit ? t('payments.edit') : t('payments.new') }}</h1>
        <div class="subtitle">
          <Tag v-if="form.storno" :value="t('payments.storno')" severity="danger" />
          <Tag v-else-if="form.payed" :value="t('payments.payed')" severity="success" />
          <Tag v-else-if="isEdit" :value="t('payments.unpaid')" severity="warn" />
          <span v-if="!isEdit" class="muted">{{ t('payments.newSubtitle') ?? 'Внесете нов документ за плаќање' }}</span>
        </div>
      </div>
      <div class="actions">
        <SplitButton v-if="isEdit" :label="t('common.print')" severity="secondary" outlined size="small"
                     icon="pi pi-print" :model="printMenu" @click="printDoc('invoice')" />
        <Button v-if="isEdit && !form.payed && !form.storno" :label="t('payments.markPayed') ?? 'Означи како платено'"
                icon="pi pi-check-circle" size="small" severity="success" @click="markPayed" />
        <Button v-if="isEdit && !form.storno" :label="t('payments.storno') ?? 'Storno'"
                icon="pi pi-times" size="small" severity="danger" outlined @click="doStorno" />
      </div>
    </header>

    <template v-if="!loading">
      <!-- Header card: type + dates + discount -->
      <div class="card">
        <div class="card-header">{{ t('payments.section.header') ?? 'Документ' }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field">
              <label>{{ t('payments.type') ?? 'Тип' }} *</label>
              <Select v-model="form.paymentTypeId" :options="paymentTypes" optionLabel="name" optionValue="id"
                      :placeholder="t('payments.pickType') ?? 'Изберете тип…'" filter showClear :disabled="form.storno || isEdit" />
            </div>
            <div class="field">
              <label>{{ t('payments.datePay') ?? 'Датум на плаќање' }} *</label>
              <DatePicker v-model="form.datePay" dateFormat="dd.mm.yy" :disabled="form.storno" />
            </div>
            <div class="field">
              <label>{{ t('payments.dateRequired') ?? 'Датум на досп.' }}</label>
              <DatePicker v-model="form.dateRequired" dateFormat="dd.mm.yy" :disabled="form.storno" />
            </div>
            <div class="field">
              <label>{{ t('payments.discount') ?? 'Попуст (%)' }}</label>
              <InputNumber v-model="form.discountPercent" :min="0" :max="99" :maxFractionDigits="2" :disabled="form.storno" />
            </div>
          </div>
        </div>
      </div>

      <!-- Relation card: vehicle + customer -->
      <div class="card">
        <div class="card-header">{{ t('payments.section.relation') ?? 'Сопственик и возило' }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field">
              <label>{{ t('payments.vehicle') ?? 'Возило' }} *</label>
              <Select v-model="form.vehicleId" :options="vehicles" :optionLabel="vehicleLabel" optionValue="id"
                      :placeholder="t('payments.pickVehicle') ?? 'Изберете возило…'" filter showClear :disabled="form.storno || isEdit"
                      :loading="vehiclesLoading" @filter="onVehicleFilter" />
            </div>
            <div class="field">
              <label>{{ t('payments.customer') ?? 'Сопственик' }} *</label>
              <Select v-model="form.customerId" :options="customers" :optionLabel="customerLabel" optionValue="id"
                      :placeholder="t('payments.pickCustomer') ?? 'Изберете сопственик…'" filter showClear :disabled="form.storno || isEdit"
                      :loading="customersLoading" @filter="onCustomerFilter" />
            </div>
          </div>
        </div>
      </div>

      <!-- Details (line items) -->
      <div class="card">
        <div class="card-header">
          <span>{{ t('payments.section.details') ?? 'Ставки' }}</span>
          <div class="card-actions">
            <Button v-if="!form.storno" :label="t('payments.autoCalc') ?? 'Автоматска пресметка'" icon="pi pi-calculator"
                    size="small" severity="secondary" outlined @click="autoCalculate" :disabled="!form.vehicleId" />
            <Button v-if="!form.storno" :label="t('common.add') ?? 'Додади'" icon="pi pi-plus" size="small" outlined @click="addLine" />
          </div>
        </div>
        <div class="card-body">
          <DataTable :value="details" stripedRows class="details-table" :pt="{ tableContainer: { class: 'dense' } }">
            <Column :header="t('payments.item') ?? 'Ставка'">
              <template #body="{ data, index }">
                <Select v-model="data.paymentItemId" :options="items" optionValue="id" optionLabel="itemName"
                        :placeholder="data.itemName || (t('payments.pickItem') ?? 'Изберете…')"
                        filter :disabled="form.storno" class="full"
                        @change="onItemPicked(data, items.find(i => i.id === data.paymentItemId) ?? null)" />
              </template>
            </Column>
            <Column :header="t('payments.price') ?? 'Цена'" style="width:120px">
              <template #body="{ data }">
                <InputNumber v-model="data.price" :min="0" :maxFractionDigits="2" :disabled="form.storno" class="full" />
              </template>
            </Column>
            <Column :header="t('payments.ddv') ?? 'ДДВ %'" style="width:90px">
              <template #body="{ data }">
                <InputNumber v-model="data.ddvRate" :min="0" :max="100" :maxFractionDigits="2" :disabled="form.storno" class="full" />
              </template>
            </Column>
            <Column :header="t('payments.discount') ?? 'Попуст %'" style="width:90px">
              <template #body="{ data }">
                <InputNumber v-model="data.discountPercent" :min="0" :max="99" :maxFractionDigits="2" :disabled="form.storno" class="full" />
              </template>
            </Column>
            <Column :header="t('payments.prePayed') ?? 'Предплатено'" style="width:110px">
              <template #body="{ data }">
                <Checkbox v-model="data.prePayed" :binary="true" :disabled="form.storno" />
              </template>
            </Column>
            <Column :header="t('payments.note') ?? 'Забелешка'">
              <template #body="{ data }">
                <InputText v-model="data.note" :disabled="form.storno" class="full" />
              </template>
            </Column>
            <Column :header="t('payments.subtotal') ?? 'Износ'" style="width:120px">
              <template #body="{ data }">
                <strong>{{ formatMoney(data.price * (1 - (data.discountPercent || 0) / 100)) }}</strong>
              </template>
            </Column>
            <Column style="width:50px">
              <template #body="{ index }">
                <Button v-if="!form.storno" icon="pi pi-times" text rounded size="small" severity="danger" @click="removeLine(index)" />
              </template>
            </Column>
            <template #empty>
              <div class="empty">{{ t('payments.noLines') ?? 'Нема ставки. Кликнете „Автоматска пресметка“ или „Додади“.' }}</div>
            </template>
            <template #footer>
              <div class="totals">
                <span v-if="prePayedTotal > 0" class="muted">{{ t('payments.prePayedTotal') ?? 'Предплатено:' }} {{ formatMoney(prePayedTotal) }}</span>
                <strong>{{ t('payments.grandTotal') ?? 'Вкупно:' }} {{ formatMoney(grandTotal) }}</strong>
              </div>
            </template>
          </DataTable>
        </div>
      </div>

      <!-- Installments (only when payment type is Rati) -->
      <div v-if="showInstallments" class="card">
        <div class="card-header">{{ t('payments.section.installments') ?? 'Рати и гарант' }}</div>
        <div class="card-body">
          <div v-if="!isEdit" class="row guarantor">
            <div class="field">
              <label>{{ t('payments.contractNumber') ?? 'Бр. на договор' }}</label>
              <InputText v-model="guarantor.contractNumber" />
            </div>
            <div class="field">
              <label>{{ t('payments.contractDate') ?? 'Датум на договор' }}</label>
              <DatePicker v-model="guarantor.contractDate" dateFormat="dd.mm.yy" />
            </div>
            <div class="field">
              <label>{{ t('payments.numberOfInstallments') ?? 'Број рати' }}</label>
              <InputNumber v-model="guarantor.numberOfInstallments" :min="2" :max="60" />
            </div>
            <div class="field">
              <label>{{ t('payments.firstInstallment') ?? 'Прва рата' }}</label>
              <InputNumber v-model="guarantor.firstInstallment" :min="0" :maxFractionDigits="2" />
            </div>
            <div class="field">
              <label>{{ t('payments.firstInstallmentDate') ?? 'Дата на прва рата' }}</label>
              <DatePicker v-model="guarantor.firstInstallmentDate" dateFormat="dd.mm.yy" />
            </div>
            <div class="field">
              <label>{{ t('payments.guarantorName') ?? 'Гарант' }}</label>
              <InputText v-model="guarantor.guarantorName" />
            </div>
            <div class="field">
              <label>{{ t('payments.guarantorEMBG') ?? 'ЕМБГ гарант' }}</label>
              <InputText v-model="guarantor.guarantorEMBG" />
            </div>
            <div class="field full-row">
              <label>{{ t('payments.guarantorAddress') ?? 'Адреса гарант' }}</label>
              <InputText v-model="guarantor.guarantorAddress" />
            </div>
          </div>
          <DataTable v-if="installments.length > 0" :value="installments" stripedRows class="details-table">
            <Column field="installmentNumber" :header="'#'" style="width:50px" />
            <Column :header="t('payments.price') ?? 'Цена'" style="width:150px">
              <template #body="{ data }">{{ formatMoney(data.price) }}</template>
            </Column>
            <Column :header="t('payments.dueDate') ?? 'Доспева'" style="width:130px">
              <template #body="{ data }">{{ data.dueDate ? new Date(data.dueDate).toLocaleDateString('mk-MK') : '—' }}</template>
            </Column>
            <Column :header="t('payments.status') ?? 'Статус'" style="width:120px">
              <template #body="{ data }">
                <Tag v-if="data.payed" :value="t('payments.payed') ?? 'Платено'" severity="success" />
                <Tag v-else :value="t('payments.unpaid') ?? 'Неплатено'" severity="warn" />
              </template>
            </Column>
            <Column :header="t('common.actions') ?? 'Акции'" style="width:80px">
              <template #body="{ data }">
                <Button v-if="isEdit && !data.payed && !form.storno" icon="pi pi-check" text rounded size="small"
                        @click="api.post(`/payment-documents/installments/${data.id}/pay`).then(load)" />
              </template>
            </Column>
          </DataTable>
        </div>
      </div>

      <!-- Note + footer -->
      <div class="card">
        <div class="card-header">{{ t('payments.section.note') ?? 'Забелешка' }}</div>
        <div class="card-body">
          <Textarea v-model="form.note" rows="2" autoResize maxlength="150" :disabled="form.storno" />
        </div>
      </div>

      <div v-if="error" class="error">{{ error }}</div>

      <div class="footer-actions">
        <Button :label="t('common.cancel')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/payments')" />
        <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" :disabled="form.storno" @click="save" />
      </div>
    </template>
  </div>
</template>

<style scoped>
.payment-form { max-width: 1300px; margin: 0 auto; padding-bottom: 3.5rem; }
.payment-form :deep(.page-header)             { display: flex; align-items: flex-end; justify-content: space-between; margin-bottom: 0.75rem; }
.payment-form :deep(.page-header h1)          { font-size: 1.125rem; letter-spacing: -0.01em; margin: 0; }
.payment-form :deep(.page-header .subtitle)   { font-size: 0.75rem; display: flex; align-items: center; gap: 0.5rem; }
.payment-form :deep(.page-header .actions)    { display: flex; gap: 0.5rem; }
.muted { color: var(--color-text-muted); }

.payment-form :deep(.card)                    { background: var(--color-surface); border: 1px solid var(--color-border); border-radius: var(--radius-md); }
.payment-form :deep(.card + .card)            { margin-top: 0.5rem; }
.payment-form :deep(.card .card-header) {
  padding: 0.4rem 0.875rem; font-size: 0.8125rem; font-weight: 600;
  background: var(--color-surface-2); color: var(--color-text-secondary);
  border-bottom: 1px solid var(--color-border);
  display: flex; align-items: center; justify-content: space-between; gap: 0.5rem;
}
.payment-form :deep(.card .card-actions) { display: flex; gap: 0.375rem; }
.payment-form :deep(.card .card-body)         { padding: 0.625rem 0.875rem; }
.payment-form :deep(.row)                     { display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 0.625rem; }
.payment-form :deep(.row.guarantor)           { grid-template-columns: repeat(4, 1fr); }
.payment-form :deep(.field.full-row)          { grid-column: 1 / -1; }
.payment-form :deep(.field)                   { display: flex; flex-direction: column; gap: 0.15rem; margin-bottom: 0.45rem; }
.payment-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; }
.payment-form :deep(.p-inputtext),
.payment-form :deep(.p-select),
.payment-form :deep(.p-textarea),
.payment-form :deep(.p-datepicker),
.payment-form :deep(.p-inputnumber)           { width: 100%; }
.payment-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.payment-form :deep(.full)                    { width: 100%; }

.payment-form :deep(.details-table .p-datatable-tbody td) { padding: 0.25rem 0.4rem; vertical-align: top; }
.payment-form :deep(.details-table .p-datatable-thead th) { padding: 0.35rem 0.4rem; font-size: 0.75rem; }
.totals { display: flex; gap: 1rem; justify-content: flex-end; padding: 0.5rem; font-size: 0.875rem; }
.empty { padding: 1rem; text-align: center; font-size: 0.8125rem; color: var(--color-text-secondary); }
.error { color: #b91c1c; font-size: 0.8125rem; margin: 0.5rem 0; }
.footer-actions { display: flex; gap: 0.5rem; justify-content: flex-end; padding-top: 0.5rem; }
</style>
