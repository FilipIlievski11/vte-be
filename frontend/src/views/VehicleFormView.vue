<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useShortcuts } from '@/composables/useShortcuts'
import { useToast } from '@/composables/useToast'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import InputNumber from 'primevue/inputnumber'
import Textarea from 'primevue/textarea'
import Checkbox from 'primevue/checkbox'
import DatePicker from 'primevue/datepicker'
import Select from 'primevue/select'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import {
  useCountries, useRegistrationIssuers, useColors,
  useVehicleBodyTypes, useVehicleCategories, useVehicleUses,
  useVehicleMakers, useVehicleModels,
  useVehicleEngineTypes, useVehicleEnginePowerSourceTypes, useVehicleEngineEcoPrograms,
  useVehicleGearBoxes, useVehicleBrakes, useVehicleSupportings,
  useVehicleCategoriesForPayments, useVehicleTireTypes,
} from '@/composables/useReferenceData'

const { t } = useI18n()
const route = useRoute()
const router = useRouter()
const id = computed(() => route.params.id as string | undefined)
const isEdit = computed(() => !!id.value)

// Reference dropdowns
const countries           = useCountries()
const issuers             = useRegistrationIssuers()
const colors              = useColors()
const bodyTypes           = useVehicleBodyTypes()
const categories          = useVehicleCategories()
const uses                = useVehicleUses()
const makers              = useVehicleMakers()
const models              = useVehicleModels()
const engineTypes         = useVehicleEngineTypes()
const enginePowerSources  = useVehicleEnginePowerSourceTypes()
const engineEcoPrograms   = useVehicleEngineEcoPrograms()
const gearBoxes           = useVehicleGearBoxes()
const brakes              = useVehicleBrakes()
const supportings         = useVehicleSupportings()
const categoriesForPay    = useVehicleCategoriesForPayments()
const tireTypes           = useVehicleTireTypes()

interface RegistrationRow { id?: number; issuerId: number | null; registrationNumber: string; makeDate: Date | null; validTill: Date | null }
interface AxleRow         { id?: number; axleNumber: number; isPropulsion: boolean; isSteering: boolean; note: string | null }
interface AxleDistanceRow { id?: number; fromAxleNumber: number; toAxleNumber: number; distance: number | null }
interface TyreRow         { id?: number; tireTypeId: number | null; positionNote: string | null; dimensions: string | null; pressureFront: number | null; pressureRear: number | null }

interface VehicleForm {
  shellNumber: string; firstRegistrationNumber: string; lastRegistrationNumber: string
  bodyTypeId: number | null; vehicleCategoryId: number | null; vehicleUseId: number | null
  vehicleModelId: number | null; vehicleModelAdding: string | null
  madeCountryId: number | null; vehicleCategoryForPaymentsId: number | null
  engineNumber: string | null; engineTypeId: number | null
  enginePowerSourceId: number | null; engineSecondPowerSourceId: number | null
  engineEcoProgramId: number | null
  enginePowerKw: number | null; engineWorkingCapacity: number | null
  rpm: number | null; gearBoxId: number | null
  brakesId: number | null; supportingId: number | null
  vehicleHeight: number | null; vehicleWidth: number | null; vehicleLength: number | null
  numberOfDoors: number | null; numberOfSeats: number | null
  numberOfStandingSeats: number | null; numberOfLyingSeats: number | null
  numberOfAxes: number | null; numberOfPropulsionAxes: number | null
  numberOfWheels: number | null; numberOfPropulsionWheels: number | null
  emptyWeight: number | null; maxAllowedWeight: number | null
  makeDate: Date | null
  firstRegistrationMakeDate: Date | null; firstRegistrationValidTill: Date | null
  lastRegistrationMakeDate: Date | null;  lastRegistrationValidTill: Date | null
  firstRegistrationIssuerId: number | null; lastRegistrationIssuerId: number | null
  colorCode: string | null; primaryColorId: number | null; secondaryColorId: number | null
  suffocation: boolean; hook: boolean; winch: boolean; tng: boolean
  note: string | null
}

const form = ref<VehicleForm>(empty())
const registrations = ref<RegistrationRow[]>([])
const axles         = ref<AxleRow[]>([])
const axleDistances = ref<AxleDistanceRow[]>([])
const tyres         = ref<TyreRow[]>([])
const loading = ref(false)
const saving = ref(false)
const error = ref<string | null>(null)

function empty(): VehicleForm {
  return {
    shellNumber: '', firstRegistrationNumber: '', lastRegistrationNumber: '',
    bodyTypeId: null, vehicleCategoryId: null, vehicleUseId: null,
    vehicleModelId: null, vehicleModelAdding: null,
    madeCountryId: null, vehicleCategoryForPaymentsId: null,
    engineNumber: null, engineTypeId: null,
    enginePowerSourceId: null, engineSecondPowerSourceId: null, engineEcoProgramId: null,
    enginePowerKw: null, engineWorkingCapacity: null, rpm: null, gearBoxId: null,
    brakesId: null, supportingId: null,
    vehicleHeight: null, vehicleWidth: null, vehicleLength: null,
    numberOfDoors: null, numberOfSeats: null, numberOfStandingSeats: null, numberOfLyingSeats: null,
    numberOfAxes: null, numberOfPropulsionAxes: null,
    numberOfWheels: null, numberOfPropulsionWheels: null,
    emptyWeight: null, maxAllowedWeight: null,
    makeDate: null,
    firstRegistrationMakeDate: null, firstRegistrationValidTill: null,
    lastRegistrationMakeDate: null,  lastRegistrationValidTill: null,
    firstRegistrationIssuerId: null, lastRegistrationIssuerId: null,
    colorCode: null, primaryColorId: null, secondaryColorId: null,
    suffocation: false, hook: false, winch: false, tng: false,
    note: null,
  }
}

function toDate(s: string | null | undefined): Date | null { return s ? new Date(s) : null }
function fromDate(d: Date | null): string | null { return d ? d.toISOString().slice(0, 10) : null }

async function load() {
  if (!isEdit.value) return
  loading.value = true
  try {
    const { data } = await api.get(`/vehicles/${id.value}`)
    const v = data.vehicle ?? data   // backward-compat if old shape ever returns
    form.value = {
      ...empty(), ...v,
      makeDate:                  toDate(v.makeDate),
      firstRegistrationMakeDate: toDate(v.firstRegistrationMakeDate),
      firstRegistrationValidTill:toDate(v.firstRegistrationValidTill),
      lastRegistrationMakeDate:  toDate(v.lastRegistrationMakeDate),
      lastRegistrationValidTill: toDate(v.lastRegistrationValidTill),
    }
    registrations.value = (data.registrations ?? []).map((r: any) => ({
      id: r.id, issuerId: r.issuerId, registrationNumber: r.registrationNumber,
      makeDate: toDate(r.makeDate), validTill: toDate(r.validTill),
    }))
    axles.value         = (data.axles ?? []).map((a: any) => ({ id: a.id, axleNumber: a.axleNumber, isPropulsion: a.isPropulsion, isSteering: a.isSteering, note: a.note }))
    axleDistances.value = (data.axleDistances ?? []).map((d: any) => ({ id: d.id, fromAxleNumber: d.fromAxleNumber, toAxleNumber: d.toAxleNumber, distance: d.distance }))
    tyres.value         = (data.tyres ?? []).map((t: any) => ({ id: t.id, tireTypeId: t.tireTypeId, positionNote: t.positionNote, dimensions: t.dimensions, pressureFront: t.pressureFront, pressureRear: t.pressureRear }))
  } finally { loading.value = false }
}

async function save() {
  saving.value = true; error.value = null
  try {
    const payload = {
      ...form.value,
      makeDate:                  fromDate(form.value.makeDate),
      firstRegistrationMakeDate: fromDate(form.value.firstRegistrationMakeDate),
      firstRegistrationValidTill:fromDate(form.value.firstRegistrationValidTill),
      lastRegistrationMakeDate:  fromDate(form.value.lastRegistrationMakeDate),
      lastRegistrationValidTill: fromDate(form.value.lastRegistrationValidTill),
      registrations: registrations.value
        .filter(r => r.registrationNumber?.trim())
        .map(r => ({ ...r, makeDate: fromDate(r.makeDate), validTill: fromDate(r.validTill) })),
      axles: axles.value.filter(a => a.axleNumber > 0),
      axleDistances: axleDistances.value,
      tyres: tyres.value,
    }
    if (isEdit.value) await api.put(`/vehicles/${id.value}`, payload)
    else {
      const { data } = await api.post('/vehicles', payload)
      const newId = data.vehicle?.id ?? data.id
      router.replace(`/vehicles/${newId}`); toast.ok(t('common.saved')); return
    }
    toast.ok(t('common.saved')); router.push('/vehicles')
  } catch (e: any) {
    const msg = e?.response?.data?.error ?? 'Save failed.'
    error.value = msg; toast.error(msg)
  } finally { saving.value = false }
}

function addRegistration() { registrations.value.push({ issuerId: null, registrationNumber: '', makeDate: null, validTill: null }) }
function removeRegistration(i: number) { registrations.value.splice(i, 1) }
function addAxle()         { axles.value.push({ axleNumber: axles.value.length + 1, isPropulsion: false, isSteering: false, note: null }) }
function removeAxle(i: number) { axles.value.splice(i, 1) }
function addDistance()     { axleDistances.value.push({ fromAxleNumber: 1, toAxleNumber: 2, distance: null }) }
function removeDistance(i: number) { axleDistances.value.splice(i, 1) }
function addTyre()         { tyres.value.push({ tireTypeId: null, positionNote: null, dimensions: null, pressureFront: null, pressureRear: null }) }
function removeTyre(i: number) { tyres.value.splice(i, 1) }

const toast = useToast()
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
      <h1>{{ isEdit ? t('vehicles.edit') : t('vehicles.new') }}</h1>
      <div class="subtitle">{{ form.shellNumber || '—' }}</div>
    </div>
  </div>

  <div v-if="loading" class="empty"><i class="pi pi-spin pi-spinner" />{{ t('common.loading') }}</div>

  <template v-else>
    <!-- Identity / classification -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.identity') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.shellNumber') }} *</label>
            <InputText v-model="form.shellNumber" maxlength="17" />
            <span class="help">VIN, 4–17 chars</span>
          </div>
          <div class="field"><label>{{ t('vehicles.firstRegistrationNumber') }} *</label>
            <InputText v-model="form.firstRegistrationNumber" maxlength="50" />
          </div>
          <div class="field"><label>{{ t('vehicles.lastRegistrationNumber') }} *</label>
            <InputText v-model="form.lastRegistrationNumber" maxlength="50" />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.bodyType') }}</label>
            <Select v-model="form.bodyTypeId" :options="bodyTypes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.category') }}</label>
            <Select v-model="form.vehicleCategoryId" :options="categories" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.use') }}</label>
            <Select v-model="form.vehicleUseId" :options="uses" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.maker') }}</label>
            <Select :modelValue="null" :options="makers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
            <span class="help">{{ t('vehicles.modelHint') }}</span>
          </div>
          <div class="field"><label>{{ t('vehicles.model') }}</label>
            <Select v-model="form.vehicleModelId" :options="models" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.modelAdding') }}</label>
            <InputText v-model="form.vehicleModelAdding" maxlength="200" />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.madeCountry') }}</label>
            <Select v-model="form.madeCountryId" :options="countries" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.makeDate') }}</label>
            <DatePicker v-model="form.makeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
          </div>
          <div class="field"><label>{{ t('vehicles.categoryForPay') }}</label>
            <Select v-model="form.vehicleCategoryForPaymentsId" :options="categoriesForPay" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
      </div>
    </div>

    <!-- Engine -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.engine') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.engineNumber') }}</label>
            <InputText v-model="form.engineNumber" maxlength="50" />
          </div>
          <div class="field"><label>{{ t('vehicles.engineType') }}</label>
            <Select v-model="form.engineTypeId" :options="engineTypes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.engineEcoProgram') }}</label>
            <Select v-model="form.engineEcoProgramId" :options="engineEcoPrograms" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.enginePowerSource') }}</label>
            <Select v-model="form.enginePowerSourceId" :options="enginePowerSources" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.engineSecondPowerSource') }}</label>
            <Select v-model="form.engineSecondPowerSourceId" :options="enginePowerSources" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.gearBox') }}</label>
            <Select v-model="form.gearBoxId" :options="gearBoxes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.enginePowerKw') }}</label>
            <InputNumber v-model="form.enginePowerKw" :minFractionDigits="2" suffix=" kW" />
          </div>
          <div class="field"><label>{{ t('vehicles.engineDisplacement') }}</label>
            <InputNumber v-model="form.engineWorkingCapacity" :minFractionDigits="0" suffix=" cm³" />
          </div>
          <div class="field"><label>{{ t('vehicles.rpm') }}</label>
            <InputNumber v-model="form.rpm" />
          </div>
        </div>
      </div>
    </div>

    <!-- Brakes / suspension / geometry -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.chassis') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.brakes') }}</label>
            <Select v-model="form.brakesId" :options="brakes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.supporting') }}</label>
            <Select v-model="form.supportingId" :options="supportings" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.height') }} (mm)</label>
            <InputNumber v-model="form.vehicleHeight" :minFractionDigits="0" />
          </div>
          <div class="field"><label>{{ t('vehicles.width') }} (mm)</label>
            <InputNumber v-model="form.vehicleWidth" :minFractionDigits="0" />
          </div>
          <div class="field"><label>{{ t('vehicles.length') }} (mm)</label>
            <InputNumber v-model="form.vehicleLength" :minFractionDigits="0" />
          </div>
        </div>
      </div>
    </div>

    <!-- Doors / seats / wheels / axles -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.doorsAxles') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.numberOfDoors') }}</label><InputNumber v-model="form.numberOfDoors" /></div>
          <div class="field"><label>{{ t('vehicles.numberOfSeats') }}</label><InputNumber v-model="form.numberOfSeats" /></div>
          <div class="field"><label>{{ t('vehicles.numberOfStandingSeats') }}</label><InputNumber v-model="form.numberOfStandingSeats" /></div>
          <div class="field"><label>{{ t('vehicles.numberOfLyingSeats') }}</label><InputNumber v-model="form.numberOfLyingSeats" /></div>
        </div>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.numberOfAxes') }}</label><InputNumber v-model="form.numberOfAxes" /></div>
          <div class="field"><label>{{ t('vehicles.numberOfPropulsionAxes') }}</label>
            <InputNumber v-model="form.numberOfPropulsionAxes" />
            <span class="help">{{ t('vehicles.propulsionAxesHelp') }}</span>
          </div>
          <div class="field"><label>{{ t('vehicles.numberOfWheels') }}</label><InputNumber v-model="form.numberOfWheels" /></div>
          <div class="field"><label>{{ t('vehicles.numberOfPropulsionWheels') }}</label><InputNumber v-model="form.numberOfPropulsionWheels" /></div>
        </div>
      </div>
    </div>

    <!-- Mass -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.mass') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.emptyWeight') }} (kg)</label><InputNumber v-model="form.emptyWeight" :minFractionDigits="2" /></div>
          <div class="field"><label>{{ t('vehicles.maxAllowedWeight') }} (kg)</label><InputNumber v-model="form.maxAllowedWeight" :minFractionDigits="2" /></div>
        </div>
      </div>
    </div>

    <!-- Registrations -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.registration') }}</div>
      <div class="card-body">
        <h3 class="subhead">{{ t('vehicles.firstRegistration') }}</h3>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.regMakeDate') }}</label><DatePicker v-model="form.firstRegistrationMakeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
          <div class="field"><label>{{ t('vehicles.regValidTill') }}</label><DatePicker v-model="form.firstRegistrationValidTill" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
          <div class="field"><label>{{ t('vehicles.regIssuer') }}</label>
            <Select v-model="form.firstRegistrationIssuerId" :options="issuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
        <h3 class="subhead">{{ t('vehicles.lastRegistration') }}</h3>
        <div class="row">
          <div class="field"><label>{{ t('vehicles.regMakeDate') }}</label><DatePicker v-model="form.lastRegistrationMakeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
          <div class="field"><label>{{ t('vehicles.regValidTill') }}</label><DatePicker v-model="form.lastRegistrationValidTill" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
          <div class="field"><label>{{ t('vehicles.regIssuer') }}</label>
            <Select v-model="form.lastRegistrationIssuerId" :options="issuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
        </div>
      </div>
    </div>

    <!-- Color -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.color') }}</div>
      <div class="card-body">
        <div class="row">
          <div class="field"><label>{{ t('vehicles.primaryColor') }}</label>
            <Select v-model="form.primaryColorId" :options="colors" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.secondaryColor') }}</label>
            <Select v-model="form.secondaryColorId" :options="colors" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter />
          </div>
          <div class="field"><label>{{ t('vehicles.colorCode') }}</label>
            <InputText v-model="form.colorCode" maxlength="20" />
          </div>
        </div>
      </div>
    </div>

    <!-- Equipment -->
    <div class="card">
      <div class="card-header">{{ t('vehicles.section.equipment') }}</div>
      <div class="card-body">
        <div class="row" style="gap: 1.5rem">
          <label class="check"><Checkbox v-model="form.suffocation" :binary="true" />{{ t('vehicles.suffocation') }}</label>
          <label class="check"><Checkbox v-model="form.hook"        :binary="true" />{{ t('vehicles.hook') }}</label>
          <label class="check"><Checkbox v-model="form.winch"       :binary="true" />{{ t('vehicles.winch') }}</label>
          <label class="check"><Checkbox v-model="form.tng"         :binary="true" />{{ t('vehicles.tng') }}</label>
        </div>
        <div class="field"><label>{{ t('common.note') }}</label><Textarea v-model="form.note" rows="2" autoResize maxlength="500" /></div>
      </div>
    </div>

    <!-- Registration history -->
    <div class="card">
      <div class="card-header">
        <span>{{ t('vehicles.section.registrations') }}</span>
        <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addRegistration" />
      </div>
      <div class="card-body">
        <DataTable :value="registrations" v-if="registrations.length" stripedRows size="small">
          <Column :header="t('vehicles.regNumber')" style="width: 20%">
            <template #body="{ data }"><InputText v-model="data.registrationNumber" maxlength="50" size="small" /></template>
          </Column>
          <Column :header="t('vehicles.regMakeDate')" style="width: 22%">
            <template #body="{ data }"><DatePicker v-model="data.makeDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></template>
          </Column>
          <Column :header="t('vehicles.regValidTill')" style="width: 22%">
            <template #body="{ data }"><DatePicker v-model="data.validTill" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></template>
          </Column>
          <Column :header="t('vehicles.regIssuer')">
            <template #body="{ data }">
              <Select v-model="data.issuerId" :options="issuers" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
            </template>
          </Column>
          <Column :header="t('common.actions')" style="width: 60px">
            <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeRegistration(index)" /></template>
          </Column>
        </DataTable>
        <div v-else class="empty" style="padding: 0.875rem"><i class="pi pi-inbox" />{{ t('vehicles.noRegistrations') }}</div>
      </div>
    </div>

    <!-- Axles + Axle distances side by side -->
    <div class="row fields-row">
      <div class="card" style="flex: 1">
        <div class="card-header">
          <span>{{ t('vehicles.section.axles') }}</span>
          <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addAxle" />
        </div>
        <div class="card-body">
          <DataTable :value="axles" v-if="axles.length" stripedRows size="small">
            <Column :header="t('vehicles.axleNumber')" style="width: 70px">
              <template #body="{ data }"><InputNumber v-model="data.axleNumber" /></template>
            </Column>
            <Column :header="t('vehicles.isPropulsion')" style="width: 90px; text-align: center">
              <template #body="{ data }"><Checkbox v-model="data.isPropulsion" :binary="true" /></template>
            </Column>
            <Column :header="t('vehicles.isSteering')" style="width: 90px; text-align: center">
              <template #body="{ data }"><Checkbox v-model="data.isSteering" :binary="true" /></template>
            </Column>
            <Column :header="t('common.note')">
              <template #body="{ data }"><InputText v-model="data.note" maxlength="250" size="small" /></template>
            </Column>
            <Column :header="t('common.actions')" style="width: 60px">
              <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeAxle(index)" /></template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 0.875rem"><i class="pi pi-inbox" />{{ t('vehicles.noAxles') }}</div>
        </div>
      </div>
      <div class="card" style="flex: 1">
        <div class="card-header">
          <span>{{ t('vehicles.section.axleDistances') }}</span>
          <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addDistance" />
        </div>
        <div class="card-body">
          <DataTable :value="axleDistances" v-if="axleDistances.length" stripedRows size="small">
            <Column :header="t('vehicles.fromAxle')" style="width: 80px">
              <template #body="{ data }"><InputNumber v-model="data.fromAxleNumber" /></template>
            </Column>
            <Column :header="t('vehicles.toAxle')" style="width: 80px">
              <template #body="{ data }"><InputNumber v-model="data.toAxleNumber" /></template>
            </Column>
            <Column :header="t('vehicles.distance')">
              <template #body="{ data }"><InputNumber v-model="data.distance" :minFractionDigits="2" /></template>
            </Column>
            <Column :header="t('common.actions')" style="width: 60px">
              <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeDistance(index)" /></template>
            </Column>
          </DataTable>
          <div v-else class="empty" style="padding: 0.875rem"><i class="pi pi-inbox" />{{ t('vehicles.noAxleDistances') }}</div>
        </div>
      </div>
    </div>

    <!-- Tyres -->
    <div class="card">
      <div class="card-header">
        <span>{{ t('vehicles.section.tyres') }}</span>
        <Button :label="t('common.add')" icon="pi pi-plus" size="small" outlined @click="addTyre" />
      </div>
      <div class="card-body">
        <DataTable :value="tyres" v-if="tyres.length" stripedRows size="small">
          <Column :header="t('vehicles.tireType')" style="width: 22%">
            <template #body="{ data }">
              <Select v-model="data.tireTypeId" :options="tireTypes" optionLabel="name" optionValue="id" :placeholder="'—'" showClear filter size="small" />
            </template>
          </Column>
          <Column :header="t('vehicles.positionNote')" style="width: 18%">
            <template #body="{ data }"><InputText v-model="data.positionNote" maxlength="100" size="small" /></template>
          </Column>
          <Column :header="t('vehicles.dimensions')" style="width: 18%">
            <template #body="{ data }"><InputText v-model="data.dimensions" maxlength="50" size="small" /></template>
          </Column>
          <Column :header="t('vehicles.pressureFront')" style="width: 16%">
            <template #body="{ data }"><InputNumber v-model="data.pressureFront" :minFractionDigits="2" /></template>
          </Column>
          <Column :header="t('vehicles.pressureRear')" style="width: 16%">
            <template #body="{ data }"><InputNumber v-model="data.pressureRear" :minFractionDigits="2" /></template>
          </Column>
          <Column :header="t('common.actions')" style="width: 60px">
            <template #body="{ index }"><Button icon="pi pi-trash" text severity="danger" @click="removeTyre(index)" /></template>
          </Column>
        </DataTable>
        <div v-else class="empty" style="padding: 0.875rem"><i class="pi pi-inbox" />{{ t('vehicles.noTyres') }}</div>
      </div>
    </div>

    <div v-if="error" class="error">{{ error }}</div>

    <div class="footer-actions">
      <Button :label="t('common.cancel')" severity="secondary" outlined icon="pi pi-times" size="small" @click="router.push('/vehicles')" />
      <Button :label="t('common.save')" :loading="saving" icon="pi pi-check" size="small" @click="save" />
    </div>
  </template>
  </div>
</template>

<style scoped>
/* ============================================================
   Same compact, professional form layout used by CustomerFormView.
   - Centered, max-width capped
   - Sticky bottom action bar pinned to the viewport
   - Tight spacing, 12px labels, ~28-30px inputs
   - Subtle section accent, refined focus rings
   ============================================================ */

.vehicle-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }

.vehicle-form :deep(.page-header)             { margin-bottom: 0.75rem; }
.vehicle-form :deep(.page-header h1)          { font-size: 1.125rem; letter-spacing: -0.01em; }
.vehicle-form :deep(.page-header .subtitle)   { font-size: 0.75rem; }

/* card box */
.vehicle-form :deep(.card + .card)            { margin-top: 0.5rem; }
.vehicle-form :deep(.card .card-header)       { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.vehicle-form :deep(.card .card-body)         { padding: 0.625rem 0.875rem; }

/* rows + fields */
.vehicle-form :deep(.row)                     { gap: 0.625rem; }
.vehicle-form :deep(.field)                   { gap: 0.15rem; margin-bottom: 0.45rem; }
.vehicle-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; }
.vehicle-form :deep(.field .help)             { font-size: 0.6875rem; color: var(--color-text-muted); margin-top: 0.125rem; }

/* sub-headings inside cards (e.g. first/last registration blocks) */
.subhead {
  display: flex; align-items: center; gap: 0.4rem;
  font-size: 0.7rem; color: var(--color-text-muted); text-transform: uppercase; letter-spacing: 0.05em; font-weight: 600;
  margin: 0.625rem 0 0.3rem;
}
.subhead::before {
  content: ''; width: 14px; height: 2px;
  background: var(--color-brand-500); border-radius: 1px; display: inline-block;
}

/* equipment checkboxes — compact horizontal row */
.check { display: flex; align-items: center; gap: 0.4rem; font-size: 0.8125rem; padding: 0.25rem 0; }

/* side-by-side two-card row (Axles + Distances) */
.fields-row { display: flex; gap: 0.5rem; align-items: stretch; margin-top: 0; }
.fields-row > .card { margin-top: 0 !important; flex: 1; }
.vehicle-form :deep(.p-datatable-tbody td),
.vehicle-form :deep(.p-datatable-thead th) { padding: 0.25rem 0.5rem; font-size: 0.8125rem; }

/* inputs — short, ~28-29px tall */
.vehicle-form :deep(.p-select),
.vehicle-form :deep(.p-datepicker),
.vehicle-form :deep(.p-inputtext),
.vehicle-form :deep(.p-inputnumber)           { width: 100%; }
.vehicle-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-textarea)              { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.vehicle-form :deep(.p-select)                { font-size: 0.8125rem; }
.vehicle-form :deep(.p-select-label)          { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.vehicle-form :deep(.p-select-dropdown)       { width: 1.625rem; }
.vehicle-form :deep(.p-datepicker-input)      { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.vehicle-form :deep(.p-inputnumber-input)     { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }

/* checkboxes */
.vehicle-form :deep(.p-checkbox)              { transform: scale(0.85); transform-origin: left center; }

/* page-header + footer buttons */
.vehicle-form :deep(.p-button)                { font-size: 0.8125rem; }
.vehicle-form :deep(.p-button.p-button-sm)    { padding: 0.3rem 0.625rem; }

/* error banner */
.vehicle-form :deep(.error)                   { font-size: 0.75rem; padding: 0.4rem 0.6rem; margin-top: 0.5rem; }

/* ----- professional polish ----- */

/* subtle accent stripe + gradient on every card header */
.vehicle-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.vehicle-form :deep(.card .card-header)::before {
  content: ''; position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}

/* card focus-within micro-elevation */
.vehicle-form :deep(.card) { transition: box-shadow 0.15s ease, border-color 0.15s ease; }
.vehicle-form :deep(.card:focus-within) {
  box-shadow: 0 1px 2px rgba(0,0,0,0.04), 0 4px 12px rgba(0,0,0,0.06);
  border-color: color-mix(in srgb, var(--color-border) 70%, var(--color-brand-500));
}

/* refined focus state on inputs */
.vehicle-form :deep(.p-inputtext:focus),
.vehicle-form :deep(.p-textarea:focus),
.vehicle-form :deep(.p-select:not(.p-disabled).p-focus),
.vehicle-form :deep(.p-datepicker-input:focus),
.vehicle-form :deep(.p-inputnumber-input:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

/* sticky footer action bar — pinned to viewport bottom, flush against page bottom */
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
</style>
