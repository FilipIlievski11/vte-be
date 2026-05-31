import { ref, type Ref } from 'vue'
import api from '@/api/client'

export interface RefItem { id: number; name: string; code?: string }

const cache: Record<string, Ref<RefItem[]>> = {}

export function useRef(key: string, path: string) {
  if (!cache[key]) {
    const data = ref<RefItem[]>([])
    cache[key] = data
    api.get<RefItem[]>(path).then((r) => { data.value = r.data }).catch(() => {})
  }
  return cache[key]
}

// People / places
export const useCountries           = () => useRef('countries',           '/ref/countries')
export const useCommunities         = () => useRef('communities',         '/ref/communities')
export const useCities              = () => useRef('cities',              '/ref/cities')
export const useStreets             = () => useRef('streets',             '/ref/streets')
export const useBusinessTypes       = () => useRef('business-types',      '/ref/business-types')
export const useRegistrationIssuers = () => useRef('registration-issuers','/ref/registration-issuers')
export const useRelationTypes       = () => useRef('cv-relation-types',   '/ref/customer-vehicle-relation-types')

// Vehicle
export const useVehicleBodyTypes      = () => useRef('vbt',  '/ref/vehicle-body-types')
export const useVehicleCategories     = () => useRef('vc',   '/ref/vehicle-categories')
export const useVehicleUses           = () => useRef('vu',   '/ref/vehicle-uses')
export const useVehicleMakers         = () => useRef('vm',   '/ref/vehicle-makers')
export const useVehicleModels         = () => useRef('vmd',  '/ref/vehicle-models')
export const useVehicleEngineTypes    = () => useRef('vet',  '/ref/vehicle-engine-types')
export const useVehicleEnginePowerSourceTypes = () => useRef('veps','/ref/vehicle-engine-power-source-types')
export const useVehicleEngineEcoPrograms = () => useRef('veep','/ref/vehicle-engine-eco-programs')
export const useVehicleGearBoxes      = () => useRef('vgb',  '/ref/vehicle-gearboxes')
export const useVehicleBrakes         = () => useRef('vb',   '/ref/vehicle-brakes')
export const useVehicleSupportings    = () => useRef('vs',   '/ref/vehicle-supportings')
export const useVehicleTireTypes      = () => useRef('vtt',  '/ref/vehicle-tire-types')
export const useColors                = () => useRef('colors', '/ref/colors')
export const useVehicleCategoriesForPayments = () => useRef('vcfp', '/ref/vehicle-categories-for-payments')

// Other
export const useDrivingLicenceCategories = () => useRef('dlc',   '/ref/driving-licence-categories')
export const useTechExamOrgs   = () => useRef('teo',  '/ref/technical-exam-organizations')
export const useTechExamTypes  = () => useRef('tet',  '/ref/technical-exam-types')
export const useTechExamPartStatuses = () => useRef('teps', '/ref/technical-exam-report-detail-statuses')
export const usePaymentTypes   = () => useRef('pt',   '/ref/payment-types')
export const useVehicleOwnershipProofTypes = () => useRef('vopt', '/ref/vehicle-ownership-proof-types')
export const usePaymentProofTypes          = () => useRef('ppt',  '/ref/payment-proof-types')
