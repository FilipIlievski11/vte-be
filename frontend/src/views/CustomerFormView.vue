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
import DatePicker from 'primevue/datepicker'
import Select from 'primevue/select'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import { useCountries, useCities, useStreets, useBusinessTypes, useRegistrationIssuers } from '@/composables/useReferenceData'

interface ContactPerson { id?: number; firstName: string; surname: string; embg?: string | null; phoneNumber?: string | null; mobileNumber?: string | null; email?: string | null }
interface BankAccount   { id?: number; bankAccount: string; deponentBank: string; taxNumber?: string | null }

interface CustomerForm {
  isCompany: boolean
  embg: string | null
  firstName: string
  surname: string | null
  parentName: string | null
  dateOfBirth: Date | null
  citizenshipId: number | null
  businessTypeId: number | null
  livingAddressId: number | null
  livingAddressNumber: string | null
  livingCityId: number | null
  birthCityId: number | null
  birthAddressId: number | null
  birthAddressNumber: string | null
  occupation: string | null
  worksInCompany: string | null
  phoneNumber: string | null
  fax: string | null
  email: string | null
  canSendNotifications: boolean
  idCardNumber: string | null
  idCardDateIssued: Date | null
  idCardIssuerId: number | null
  passportNumber: string | null
  passportDateIssued: Date | null
  passportIssuerId: number | null
  drivingLicenceNumber: string | null
  drivingLicenceDateIssued: Date | null
  drivingLicenceIssuerId: number | null
  taxNumber: string | null
  status: string | null
  note: string | null
  contactPersons: ContactPerson[]
  bankAccounts: BankAccount[]
}

const { t } = useI18n()
const route = useRoute()
const router = useRouter()
const id = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!id.value)

const countries           = useCountries()
const cities              = useCities()
const streets             = useStreets()
const businessTypes       = useBusinessTypes()
const registrationIssuers = useRegistrationIssuers()

const form = ref<CustomerForm>(empty())
const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)

function empty(): CustomerForm {
  return {
    isCompany: false, embg: null, firstName: '', surname: null, parentName: null,
    dateOfBirth: null, citizenshipId: null, businessTypeId: null,
    livingAddressId: null, livingAddressNumber: null, livingCityId: null,
    birthCityId: null, birthAddressId: null, birthAddressNumber: null,
    occupation: null, worksInCompany: null,
    phoneNumber: null, fax: null, email: null, canSendNotifications: false,
    idCardNumber: null, idCardDateIssued: null, idCardIssuerId: null,
    passportNumber: null, passportDateIssued: null, passportIssuerId: null,
    drivingLicenceNumber: null, drivingLicenceDateIssued: null, drivingLicenceIssuerId: null,
    taxNumber: null, status: null, note: null,
    contactPersons: [], bankAccounts: [],
  }
}

function toDate(s: string | null): Date | null { return s ? new Date(s) : null }
function fromDate(d: Date | null): string | null { return d ? d.toISOString().slice(0, 10) : null }

async function load() {
  if (!isEdit.value) return
  loading.value = true
  try {
    const { data } = await api.get(`/customers/${id.value}`)
    form.value = {
      ...data,
      dateOfBirth:               toDate(data.dateOfBirth),
      idCardDateIssued:          toDate(data.idCardDateIssued),
      passportDateIssued:        toDate(data.passportDateIssued),
      drivingLicenceDateIssued:  toDate(data.drivingLicenceDateIssued),
      contactPersons: data.contactPersons || [],
      bankAccounts:   data.bankAccounts   || [],
    }
  } finally { loading.value = false }
}

async function save() {
  saving.value = true; error.value = null
  try {
    const payload = {
      ...form.value,
      dateOfBirth:              fromDate(form.value.dateOfBirth),
      idCardDateIssued:         fromDate(form.value.idCardDateIssued),
      passportDateIssued:       fromDate(form.value.passportDateIssued),
      drivingLicenceDateIssued: fromDate(form.value.drivingLicenceDateIssued),
    }
    if (isEdit.value) {
      await api.put(`/customers/${id.value}`, payload)
    } else {
      const { data } = await api.post('/customers', payload)
      router.replace(`/customers/${data.id}`)
      toast.ok(t('common.saved'))
      return
    }
    toast.ok(t('common.saved'))
    router.push('/customers')
  } catch (e: any) {
    const msg = e?.response?.data?.error ?? 'Save failed.'
    error.value = msg; toast.error(msg)
  } finally {
    saving.value = false
  }
}

function addContact()   { form.value.contactPersons.push({ firstName: '', surname: '', embg: '', phoneNumber: '', mobileNumber: '', email: '' }) }
function removeContact(i: number) { form.value.contactPersons.splice(i, 1) }
function addAccount()   { form.value.bankAccounts.push({ bankAccount: '', deponentBank: '', taxNumber: '' }) }
function removeAccount(i: number) { form.value.bankAccounts.splice(i, 1) }

const toast = useToast()
useShortcuts({
  'F2':     () => save(),
  'Ctrl+S': () => save(),
  'Escape': () => router.back(),
})

onMounted(load)
</script>

<template>
  <div class="customer-form">
  <div class="page-header">
    <div>
      <h1>{{ isEdit ? t('customers.edit') : t('customers.new') }}</h1>
      <div class="subtitle">
        <Tag v-if="form.isCompany" :value="t('customers.isCompany')" severity="info" />
        <span v-else>{{ t('customers.individual') }}</span>
      </div>
    </div>
  </div>

  <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>

  <template v-else>
    <!-- Header / identity -->
    <div class="card">
      <div class="card-header">{{ t('customers.section.identity') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field" style="max-width: 220px"><label>{{ t('customers.embg') }}</label>
            <InputText v-model="form.embg" maxlength="13" size="small" />
          </div>
          <div class="field" style="display: flex; align-items: center; padding-top: 1.5rem">
            <label style="display: flex; align-items: center; gap: 0.5rem; font-weight: 500">
              <Checkbox v-model="form.isCompany" :binary="true" /> {{ t('customers.isCompany') }}
            </label>
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ form.isCompany ? t('customers.companyName') : t('customers.surname') }}</label><InputText v-model="form.surname" /></div>
          <div class="field"><label>{{ t('customers.firstName') }} *</label><InputText v-model="form.firstName" required /></div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('customers.parentName') }}</label><InputText v-model="form.parentName" /></div>
          <div class="field"><label>{{ t('customers.businessType') }}</label>
            <Select v-model="form.businessTypeId" :options="businessTypes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('customers.taxNumber') }}</label><InputText v-model="form.taxNumber" maxlength="15" /></div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('customers.occupation') }}</label><InputText v-model="form.occupation" maxlength="50" /></div>
          <div class="field"><label>{{ t('customers.worksInCompany') }}</label><InputText v-model="form.worksInCompany" /></div>
        </div>
      </div>
    </div>

    <!-- Identity documents (3 sub-cards) -->
    <div class="card">
      <div class="card-header">{{ t('customers.section.idDocs') }}</div>
      <div class="card-body">
        <div class="docgrid">
          <div class="docblock">
            <h3>{{ t('customers.idCard') }}</h3>
            <div class="field"><label>{{ t('customers.docNumber') }}</label><InputText v-model="form.idCardNumber" maxlength="20" /></div>
            <div class="field"><label>{{ t('customers.dateIssued') }}</label><DatePicker v-model="form.idCardDateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
            <div class="field"><label>{{ t('customers.issuer') }}</label>
              <Select v-model="form.idCardIssuerId" :options="registrationIssuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
            </div>
          </div>
          <div class="docblock">
            <h3>{{ t('customers.passport') }}</h3>
            <div class="field"><label>{{ t('customers.docNumber') }}</label><InputText v-model="form.passportNumber" maxlength="20" /></div>
            <div class="field"><label>{{ t('customers.dateIssued') }}</label><DatePicker v-model="form.passportDateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
            <div class="field"><label>{{ t('customers.issuer') }}</label>
              <Select v-model="form.passportIssuerId" :options="registrationIssuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
            </div>
          </div>
          <div class="docblock">
            <h3>{{ t('customers.drivingLicence') }}</h3>
            <div class="field"><label>{{ t('customers.docNumber') }}</label><InputText v-model="form.drivingLicenceNumber" maxlength="20" /></div>
            <div class="field"><label>{{ t('customers.dateIssued') }}</label><DatePicker v-model="form.drivingLicenceDateIssued" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
            <div class="field"><label>{{ t('customers.issuer') }}</label>
              <Select v-model="form.drivingLicenceIssuerId" :options="registrationIssuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Address + birth place -->
    <div class="card">
      <div class="card-header">{{ t('customers.section.address') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('customers.citizenship') }}</label>
            <Select v-model="form.citizenshipId" :options="countries" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('customers.dateOfBirth') }}</label><DatePicker v-model="form.dateOfBirth" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
        </div>
        <h3 class="subhead">{{ t('customers.living') }}</h3>
        <div class="row">
          <div class="field"><label>{{ t('customers.street') }}</label>
            <Select v-model="form.livingAddressId" :options="streets" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field" style="max-width: 160px"><label>{{ t('customers.streetNumber') }}</label><InputText v-model="form.livingAddressNumber" maxlength="100" /></div>
          <div class="field"><label>{{ t('customers.city') }}</label>
            <Select v-model="form.livingCityId" :options="cities" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <h3 class="subhead">{{ t('customers.birth') }}</h3>
        <div class="row">
          <div class="field"><label>{{ t('customers.street') }}</label>
            <Select v-model="form.birthAddressId" :options="streets" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field" style="max-width: 160px"><label>{{ t('customers.streetNumber') }}</label><InputText v-model="form.birthAddressNumber" maxlength="100" /></div>
          <div class="field"><label>{{ t('customers.city') }}</label>
            <Select v-model="form.birthCityId" :options="cities" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
      </div>
    </div>

    <!-- Contact -->
    <div class="card">
      <div class="card-header">{{ t('customers.section.contact') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('customers.phone') }}</label><InputText v-model="form.phoneNumber" maxlength="20" /></div>
          <div class="field"><label>{{ t('customers.fax') }}</label><InputText v-model="form.fax" maxlength="20" /></div>
          <div class="field"><label>{{ t('customers.email') }}</label><InputText v-model="form.email" /></div>
        </div>
        <div class="row" style="gap: 1.5rem">
          <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.875rem">
            <Checkbox v-model="form.canSendNotifications" :binary="true" />
            {{ t('customers.canSendNotifications') }}
          </label>
          <div class="field" style="max-width: 220px"><label>{{ t('customers.status') }}</label><InputText v-model="form.status" maxlength="50" /></div>
        </div>
        <div class="field"><label>{{ t('common.note') }}</label><Textarea v-model="form.note" rows="2" autoResize /></div>
      </div>
    </div>

    <!-- Bank accounts -->
    <div class="card">
      <div class="card-header">
        <span>{{ t('customers.section.bankAccounts') }}</span>
        <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addAccount" />
      </div>
      <div class="card-body">
        <DataTable :value="form.bankAccounts" v-if="form.bankAccounts.length">
          <Column :header="t('customers.bankAccount')">
            <template #body="{ data }"><InputText v-model="data.bankAccount" maxlength="50" /></template>
          </Column>
          <Column :header="t('customers.deponentBank')">
            <template #body="{ data }"><InputText v-model="data.deponentBank" maxlength="50" /></template>
          </Column>
          <Column :header="t('customers.taxNumber')">
            <template #body="{ data }"><InputText v-model="data.taxNumber" maxlength="15" /></template>
          </Column>
          <Column :header="t('common.actions')" style="width: 80px">
            <template #body="{ index }">
              <Button icon="pi pi-trash" text severity="danger" @click="removeAccount(index)" />
            </template>
          </Column>
        </DataTable>
        <div v-else class="empty" style="padding: 1.5rem"><i class="pi pi-inbox" />{{ t('customers.noBankAccounts') }}</div>
      </div>
    </div>

    <!-- Contact persons -->
    <div class="card">
      <div class="card-header">
        <span>{{ t('customers.section.contactPersons') }}</span>
        <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addContact" />
      </div>
      <div class="card-body">
        <DataTable :value="form.contactPersons" v-if="form.contactPersons.length">
          <Column :header="t('customers.firstName')">
            <template #body="{ data }"><InputText v-model="data.firstName" maxlength="50" /></template>
          </Column>
          <Column :header="t('customers.surname')">
            <template #body="{ data }"><InputText v-model="data.surname" maxlength="50" /></template>
          </Column>
          <Column :header="t('customers.embg')">
            <template #body="{ data }"><InputText v-model="data.embg" maxlength="13" /></template>
          </Column>
          <Column :header="t('customers.phone')">
            <template #body="{ data }"><InputText v-model="data.phoneNumber" maxlength="20" /></template>
          </Column>
          <Column :header="t('customers.mobile')">
            <template #body="{ data }"><InputText v-model="data.mobileNumber" maxlength="20" /></template>
          </Column>
          <Column :header="t('customers.email')">
            <template #body="{ data }"><InputText v-model="data.email" maxlength="200" /></template>
          </Column>
          <Column :header="t('common.actions')" style="width: 80px">
            <template #body="{ index }">
              <Button icon="pi pi-trash" text severity="danger" @click="removeContact(index)" />
            </template>
          </Column>
        </DataTable>
        <div v-else class="empty" style="padding: 1.5rem"><i class="pi pi-inbox" />{{ t('customers.noContactPersons') }}</div>
      </div>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <div class="footer-actions">
      <Button :label="t('customers.cancel')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/customers')" />
      <Button :label="t('customers.save')" :loading="saving" icon="pi pi-check" size="small" @click="save" />
    </div>
  </template>
  </div>
</template>

<style scoped>
/* ============================================================
   Compact, professional form layout.
   - Centered, max-width capped so fields aren't stretched on wide screens
   - Sticky bottom action bar (no need to scroll to save)
   - Tight spacing, 12px labels, ~28px inputs
   - Subtle section accent, refined focus rings, required-* highlight
   ============================================================ */

.customer-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }

.customer-form :deep(.page-header)                  { margin-bottom: 0.75rem; }
.customer-form :deep(.page-header h1)               { font-size: 1.125rem; letter-spacing: -0.01em; }
.customer-form :deep(.page-header .subtitle)        { font-size: 0.75rem; }

/* card box */
.customer-form :deep(.card + .card)                 { margin-top: 0.5rem; }
.customer-form :deep(.card .card-header)            { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.customer-form :deep(.card .card-body)              { padding: 0.625rem 0.875rem; }

/* rows + fields */
.customer-form :deep(.row)                          { gap: 0.625rem; }
.customer-form :deep(.field)                        { gap: 0.15rem; margin-bottom: 0.45rem; }
.customer-form :deep(.field label)                  { font-size: 0.75rem; font-weight: 500; }

/* sub-headings inside cards */
.subhead                                            { font-size: 0.7rem; color: var(--color-text-muted); text-transform: uppercase; letter-spacing: 0.05em; font-weight: 600; margin: 0.625rem 0 0.3rem; }

/* identity-documents 3-column grid */
.docgrid                                            { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 0.5rem 0.875rem; }
.docblock h3                                        { font-size: 0.75rem; color: var(--color-text); margin: 0 0 0.4rem; padding-bottom: 0.25rem; border-bottom: 1px solid var(--color-border); font-weight: 600; }
.docblock .field                                    { margin-bottom: 0.4rem; }

/* inputs — short, ~28-29px tall */
.customer-form :deep(.p-select),
.customer-form :deep(.p-datepicker),
.customer-form :deep(.p-inputtext)                  { width: 100%; }
.customer-form :deep(.p-inputtext)                  { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.customer-form :deep(.p-textarea)                   { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.customer-form :deep(.p-select)                     { font-size: 0.8125rem; }
.customer-form :deep(.p-select-label)               { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.customer-form :deep(.p-select-dropdown)            { width: 1.625rem; }
.customer-form :deep(.p-datepicker-input)           { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }

/* inline tables (bank accounts, contact persons) */
.customer-form :deep(.p-datatable-tbody td),
.customer-form :deep(.p-datatable-thead th)         { padding: 0.25rem 0.45rem; font-size: 0.8125rem; }
.customer-form :deep(.p-datatable-tbody .p-inputtext) { padding: 0.2rem 0.4rem; font-size: 0.8125rem; }

/* checkboxes (Company toggle, notifications toggle) */
.customer-form :deep(.p-checkbox)                   { transform: scale(0.85); transform-origin: left center; }

/* trailing footer buttons */
.customer-form :deep(.p-button)                     { font-size: 0.8125rem; }
.customer-form :deep(.p-button.p-button-sm)         { padding: 0.3rem 0.625rem; }

/* error banner */
.customer-form :deep(.error)                        { font-size: 0.75rem; padding: 0.4rem 0.6rem; margin-top: 0.5rem; }

/* ----- professional polish ----- */

/* subtle accent stripe on every card header */
.customer-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.customer-form :deep(.card .card-header)::before {
  content: '';
  position: absolute;
  left: 0; top: 0; bottom: 0;
  width: 3px;
  background: var(--color-brand-500);
  border-radius: 0 2px 2px 0;
}

/* card hover micro-elevation — feedback that something is interactive nearby */
.customer-form :deep(.card) {
  transition: box-shadow 0.15s ease, border-color 0.15s ease;
}
.customer-form :deep(.card:focus-within) {
  box-shadow: 0 1px 2px rgba(0,0,0,0.04), 0 4px 12px rgba(0,0,0,0.06);
  border-color: color-mix(in srgb, var(--color-border) 70%, var(--color-brand-500));
}

/* refined focus state on inputs — soft 2px ring in primary color */
.customer-form :deep(.p-inputtext:focus),
.customer-form :deep(.p-textarea:focus),
.customer-form :deep(.p-select:not(.p-disabled).p-focus),
.customer-form :deep(.p-datepicker-input:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

/* required-field asterisk pops in primary color */
.customer-form :deep(.field label) {
  color: var(--color-text-secondary);
}

/* sub-headings: small accent bar to anchor them */
.subhead {
  display: flex; align-items: center; gap: 0.4rem;
}
.subhead::before {
  content: '';
  width: 14px; height: 2px;
  background: var(--color-brand-500);
  border-radius: 1px;
  display: inline-block;
}

/* footer action bar — fixed to the actual viewport bottom (no gap possible).
   Spans the content area (sidebar to right edge). The form's padding-bottom
   reserves space so the last content isn't covered when scrolled to the end. */
.footer-actions {
  position: fixed;
  bottom: 0;
  left: var(--sidebar-w);
  right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border);
  z-index: 5;
}

/* page-header actions: smaller buttons for visual balance */
.customer-form :deep(.page-header .actions .p-button) { padding: 0.3rem 0.625rem; font-size: 0.8125rem; }

/* "empty" state inside section bodies — softer */
.customer-form :deep(.empty) {
  font-size: 0.75rem;
  color: var(--color-text-muted);
  background: color-mix(in srgb, var(--color-surface) 60%, var(--color-bg));
  border-radius: var(--radius);
  padding: 0.875rem !important;
}
</style>
