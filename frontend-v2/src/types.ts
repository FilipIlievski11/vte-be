// Shared API types — kept in sync with backend-v2 DTOs.

export interface Paged<T> {
  page: number;
  pageSize: number;
  total: number;
  items: T[];
}

export interface LoginRequest {
  userName: string;
  password: string;
  useCookie?: boolean;
}

export interface LoginResponse {
  token: string;
  expiresAt: string;
  userId: string;
  userName: string;
  fullName: string | null;
  companyId: number | null;
  companyName: string | null;
  roles: string[];
}

export interface MeResponse {
  userId: string;
  userName: string;
  email: string | null;
  fullName: string | null;
  companyId: number | null;
  companyName: string | null;
  roles: string[];
}

/** Body for self-service profile update (PUT /api/auth/profile). */
export interface ProfileUpdate {
  fullName: string | null;
  email: string | null;
}

/** Body for self-service password change (POST /api/auth/change-password).
 *  No current-password challenge — the JWT/cookie already proves identity. */
export interface ChangePasswordRequest {
  newPassword: string;
}

export interface Country {
  id: number;
  name: string | null;
  shortName: string | null;
  active: boolean | null;
}

export interface Community {
  id: number;
  name: string;
  countryId: number;
  code: string | null;
  plateNumberPrefix: string | null;
  active: boolean | null;
}

export interface City {
  id: number;
  name: string;
  communityId: number;
  postalCode: string;
  active: boolean;
}

export interface Citizenship {
  id: number;
  countryId: number;
  name: string;
}

export interface DocumentIssuer {
  id: number;
  name: string;
  active: boolean;
}

export interface PersonalDataType {
  id: number;
  name: string;
}

export interface Company {
  id: number;
  name: string;
  createdAt: string;
  active: boolean;
}

export interface Station {
  id: number;
  companyId: number;
  name: string;
  active: boolean | null;
}

// ---------- User management (Admin only) ----------

export interface UserListItem {
  id: string;
  userName: string;
  fullName: string | null;
  email: string | null;
  companyId: number | null;
  companyName: string | null;
  role: string;
  isActive: boolean;
  createdAt: string;
}

export interface UserCreate {
  userName: string;
  email: string;
  password: string;
  fullName: string | null;
  companyId: number | null;
  role: string;
}

export interface UserUpdate {
  fullName: string | null;
  email: string | null;
  companyId: number | null;
  isActive: boolean | null;
}

export interface Client {
  id: number;
  companyId: number;
  cityId: number | null;
  citizenshipId: number | null;
  business: boolean | null;
  firstName: string | null;
  middleName: string | null;
  lastName: string | null;
  mb: string | null;
  address: string | null;
  taxNumber: string | null;
  phoneNumber: string | null;
  email: string | null;
  dateOfBirth: string | null;
  note: string | null;
  active: boolean | null;
  createdAt: string | null;
}

export type ClientWrite = Omit<Client, 'id' | 'companyId' | 'createdAt'>;

export interface ClientPersonalData {
  id: number;
  clientId: number;
  personalDataTypeId: number;
  documentIssuerId: number;
  number: string;
  createdAt: string;
  active: boolean;
}

// ---------- Vehicle module ----------

export interface VehicleBodyType        { id: number; code: string | null; name: string; active: boolean }
export interface VehicleCategory        { id: number; code: string | null; name: string; active: boolean }
export interface VehicleMaker           { id: number; countryId: number | null; name: string; trademark: string | null; active: boolean }
export interface VehicleModel           { id: number; makerId: number; code: string | null; name: string; productionStart: string | null; productionEnd: string | null; active: boolean }
export interface VehicleColor           { id: number; code: string | null; name: string; active: boolean }
export interface VehicleFuel            { id: number; name: string; active: boolean }
export interface VehicleEcoProgram      { id: number; name: string; active: boolean }
export interface VehicleEngineType      { id: number; code: string | null; name: string; active: boolean }
export interface VehiclePaymentCategory { id: number; name: string; active: boolean }
export interface ClientVehicleRelationType {
  id: number; name: string;
  isOwner: boolean; isAuthorized: boolean; isCustomerOnly: boolean;
  description: string | null; active: boolean;
}

export interface VehicleListItem {
  id: number;                 // vehicle Id (for navigation)
  companyId: number;
  vin: string;
  plate: string | null;
  modelId: number | null;
  model: string | null;
  maker: string | null;
  bodyTypeId: number | null;
  bodyType: string | null;
  categoryId: number | null;
  category: string | null;
  primaryColorId: number | null;
  primaryColor: string | null;
  enginePowerKw: number | null;
  engineWorkingCapacityCc: number | null;
  ownerName: string | null;
  ownerEmbg: string | null;
  active: boolean;
  // Relation-driven row: each row is one ClientVehicleRelation
  relationId: number;
  relationTypeId: number;
  relationTypeName: string | null;
}

export interface Vehicle {
  id: number;
  companyId: number;
  vin: string;
  engineNumber: string | null;
  plate: string | null;
  categoryId: number | null;
  bodyTypeId: number | null;
  modelId: number | null;
  primaryColorId: number | null;
  secondaryColorId: number | null;
  madeCountryId: number | null;
  fuelId: number | null;
  secondFuelId: number | null;
  engineTypeId: number | null;
  ecoProgramId: number | null;
  paymentCategoryId: number | null;
  enginePowerKw: number | null;
  engineWorkingCapacityCc: number | null;
  maxRpm: number | null;
  maxSpeedKmh: number | null;
  hasLpg: boolean | null;
  lengthMm: number | null; widthMm: number | null; heightMm: number | null;
  emptyWeightKg: number | null; maxAllowedWeightKg: number | null;
  maxLegalTotalMassKg: number | null; maxConstructiveTotalMassKg: number | null;
  trailerMassWithBrakesKg: string | null; trailerMassWithoutBrakesKg: string | null;
  axleCount: number | null; wheelCount: number | null;
  axleLoad1Kg: number | null; axleLoad2Kg: number | null;
  seats: number | null; standingSeats: number | null;
  co2GKm: number | null; noiseStaticDb: number | null; noiseMovingDb: number | null;
  typeText: string | null; modelVariant: string | null; approvalMark: string | null;
  note: string | null; active: boolean; createdAt: string;
}

export interface VehicleRegistration {
  id: number;
  vehicleId: number;
  issuerId: number;
  issuerName: string | null;
  plateNumber: string;
  registeredDate: string;
  validUntil: string;
  isFirstRegistration: boolean;
  active: boolean;
}

export interface VehicleRelationDto {
  id: number;
  clientId: number;
  vehicleId: number | null;
  relationTypeId: number;
  relationTypeName: string | null;
  clientDisplayName: string | null;
  clientMb: string | null;
  vehicleVin: string | null;
  vehiclePlate: string | null;
  vehicleMaker: string | null;
  vehicleModel: string | null;
  startDate: string;
  endDate: string | null;
  startNote: string | null;
  endNote: string | null;
  active: boolean;
}

// ---------- Request module ----------

export interface RequestDocumentPrint {
  id: number;
  code: string;
  name: string;
  templatePath: string | null;
  active: boolean;
}

export interface RequestOwnershipProofType { id: number; name: string; active: boolean }
export interface RequestPaymentProofType   { id: number; name: string; active: boolean }
export interface RequestAttachmentType     { id: number; name: string; active: boolean }

/** Tri-state replacement for legacy IsTehnicalExamRequired int. */
export enum TechnicalExamRequirement {
  NotRequired = 0,
  Required = 1,
  Optional = 2,
}

export interface RequestType {
  id: number;
  parentRequestTypeId: number | null;
  documentPrintId: number;
  name: string;
  description: string | null;
  technicalExamRequirement: TechnicalExamRequirement;
  paymentRequired: boolean;
  issuesNewRegistration: boolean;
  deactivatesRelation: boolean;
  deactivatesVehicle: boolean;
  transfersOwnership: boolean;
  mutatesVehicleData: boolean;
  mutatesClientData: boolean;
  isSufficient: boolean;
  previousRegistrationRequired: boolean;
  active: boolean;
}

export type RequestTypeWrite = Omit<RequestType, 'id'>;

// ---------- Request ----------

export interface RequestListItem {
  id: number;
  companyId: number;
  requestTypeId: number;
  requestTypeName: string | null;
  clientVehicleRelationId: number;
  clientDisplayName: string | null;
  vehicleId: number | null;
  vehicleVin: string | null;
  vehiclePlate: string | null;
  createdAt: string;
  modifiedAt: string | null;
  endedAt: string | null;
  createdByUserName: string | null;
  note: string | null;
  active: boolean;
}

export interface RequestRead {
  id: number;
  companyId: number;
  requestTypeId: number;
  clientVehicleRelationId: number;
  newClientVehicleRelationId: number | null;
  technicalExamReportId: number | null;
  previousRegistrationId: number | null;
  createdAt: string;
  modifiedAt: string | null;
  endedAt: string | null;
  createdByUserId: string;
  modifiedByUserId: string | null;
  endedByUserId: string | null;
  createdByUserName: string | null;
  modifiedByUserName: string | null;
  endedByUserName: string | null;
  vehicleDataChanged: boolean;
  clientDataChanged: boolean;
  note: string | null;
  active: boolean;
  legacyReferenceNumber: string | null;
  rowVersion: string;       // base64-encoded byte[]
}

export interface RequestWrite {
  requestTypeId: number;
  clientVehicleRelationId: number;
  newClientVehicleRelationId: number | null;
  newOwnerClientId?: number | null;
  technicalExamReportId: number | null;
  previousRegistrationId: number | null;
  note: string | null;
  active: boolean | null;
  companyId: number | null;
}

// ---------- Request children ----------

export interface RequestOwnershipProofDto {
  id: number;
  requestId: number;
  ownershipProofTypeId: number;
  ownershipProofTypeName: string | null;
  detail: string | null;
  active: boolean;
}

export interface RequestPaymentProofDto {
  id: number;
  requestId: number;
  paymentProofTypeId: number;
  paymentProofTypeName: string | null;
  detail: string | null;
  active: boolean;
}

export interface RequestAttachmentDto {
  id: number;
  requestId: number;
  attachmentTypeId: number;
  attachmentTypeName: string | null;
  fileName: string;
  contentType: string;
  sizeBytes: number;
  uploadedAt: string;
  uploadedByUserName: string | null;
  active: boolean;
}

// ---------- End-request result + Print bundle ----------

export interface EndRequestResult {
  id: number;
  endedAt: string;
  relationDeactivated: boolean;
  vehicleDeactivated: boolean;
  ownershipTransferred: boolean;
  needsNewRegistration: boolean;
  notes: string[];
}

export interface PrintClientMeta {
  id: number;
  fullName: string | null;
  firstName: string | null;
  middleName: string | null;
  lastName: string | null;
  mb: string | null;
  taxNumber: string | null;
  address: string | null;
  cityName: string | null;
  communityName: string | null;
  countryName: string | null;
  citizenshipName: string | null;
  phoneNumber: string | null;
  email: string | null;
  isBusiness: boolean | null;
  dateOfBirth: string | null;
  communityRegistrationCode: string | null;   // community plate prefix (e.g. "VE") for the Plav new-reg prefix
  communityRegistrationIssuer: string | null;  // destination MVR office for the community (e.g. "МВР ВЕЛЕС")
}

export interface PrintVehicleMeta {
  id: number;
  vin: string;
  engineNumber: string | null;
  plate: string | null;
  maker: string | null;
  model: string | null;
  modelCode: string | null;
  variant: string | null;
  typeText: string | null;            // legacy Vehicles.Tip column = D.2 source
  bodyType: string | null;
  category: string | null;
  categoryForPayments: string | null;
  categoryZelenMap: number | null;     // legacy ZelenMap (1-4 → category checkbox row)
  primaryColorName: string | null;
  primaryColorCode: string | null;
  secondaryColorName: string | null;
  secondaryColorCode: string | null;
  fuelName: string | null;
  engineTypeName: string | null;
  engineTypeCode: string | null;          // legacy EngineTypeCode (e.g. "192A1000")
  ecoProgramName: string | null;
  enginePowerKw: number | null;
  engineWorkingCapacityCc: number | null;
  emptyWeightKg: number | null;
  maxAllowedWeightKg: number | null;
  seats: number | null;
  standingSeats: number | null;
  madeCountry: string | null;
  hasLpg: boolean | null;
  manufactureDate: string | null;        // ISO datetime, format with `.getFullYear()` for legacy year display
  // Plav page-2 technical specs
  lengthMm: number | null;
  widthMm: number | null;
  heightMm: number | null;
  maxLegalTotalMassKg: number | null;
  maxConstructiveTotalMassKg: number | null;
  maxLegalGroupMassKg: number | null;
  axleCount: number | null;
  maxRpm: number | null;
  maxSpeedKmh: number | null;
  co2GKm: number | null;
  noiseStaticDb: number | null;
  axleLoad1Kg: number | null;
  axleLoad2Kg: number | null;
  maxTrailerBrakedKg: number | null;       // legacy MaxKonstVkMasaKocnaPrikolka
  maxTrailerUnbrakedKg: number | null;     // legacy MaxKonstVkMasaNeKocnaPrikolka
  maxHitchLoadKg: number | null;           // legacy MaxKonstOptovaruvanjeVoPriklucok
  approvalMark: string | null;
}

export interface RequestPrintBundle {
  request: RequestRead;
  type: {
    id: number;
    name: string;
    description: string | null;
    documentPrintId: number;
    documentPrintCode: string;       // PLAV / BEL / ZELEN
    documentPrintName: string;
    transfersOwnership: boolean;
    deactivatesRelation: boolean;
    deactivatesVehicle: boolean;
    issuesNewRegistration: boolean;
  };
  client: PrintClientMeta;
  vehicle: PrintVehicleMeta | null;
  newVehicle: PrintVehicleMeta | null;
  newClient: PrintClientMeta | null;
  ownershipProofs: { id: number; typeName: string | null; detail: string | null }[];
  paymentProofs:   { id: number; typeName: string | null; detail: string | null }[];
  company: { id: number; name: string; communityName: string | null };
  lastRegistration: {
    id: number;
    plateNumber: string;
    registeredDate: string;
    validUntil: string;
    issuer: string | null;
  } | null;
  // The PREVIOUS registration (matches the vehicle's stored plate). On a transfer
  // this is the old plate (e.g. "SN 4604 AC"); lastRegistration is the newest row.
  previousRegistration: {
    id: number;
    plateNumber: string;
    registeredDate: string;
    validUntil: string;
    issuer: string | null;
  } | null;
}

/** One positioned control from a legacy DevExpress XtraReport Designer.vb. */
export interface PrintLayoutControl {
  name: string;
  x: number;           // 0.1mm units (legacy DPI=254)
  y: number;
  w: number;
  h: number;
  text?: string;
  align?: string;
  visible?: boolean;
  bold?: boolean;
  italic?: boolean;
  fontFamily?: string;
  fontSize?: number;
  checked?: boolean;
}
export interface PrintLayoutManifest {
  page: { w: number; h: number };
  total: number;
  counts: Record<string, number>;
  controls: PrintLayoutControl[];
}

/** Admin → Legacy Sync. Current high-water marks shown before syncing. */
export interface LegacySyncStatus {
  enabled: boolean;
  maxClientId: number;
  maxVehicleId: number;
  maxRequestId: number;
  clients: number;
  vehicles: number;
  requests: number;
  technicalExamReports: number;
}
/** Result of a one-click incremental sync from the live legacy DB. */
export interface LegacySyncResult {
  clients: number;
  vehicles: number;
  registrations: number;
  relations: number;
  requests: number;
  ownershipProofs: number;
  paymentProofs: number;
  references: number;
  durationMs: number;
  maxClientId: number;
  maxVehicleId: number;
  maxRequestId: number;
  technicalExamReports: number;
}

/** Technical-exam register row. */
export interface TechExamListItem {
  id: number;
  companyId: number;
  regNumber: string | null;
  madeDate: string;
  validTillDate: string;
  technicalExamTypeId: number;
  typeCode: string | null;
  typeName: string | null;
  organizationId: number;
  organizationName: string | null;
  customerVehicleRelationId: number | null;
  clientName: string | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  vehicleIsRight: boolean;
  active: boolean;
}

/** One brake-force axle row (axle 1-4, 0 = parking brake). */
export interface TechExamAxleReading {
  axle: number;
  left: number | null;
  right: number | null;
  gj: number | null;
  leftRightDiff: number | null;
  coefficient: number | null;
}

/** One defective-part line on a report. */
export interface TechExamDetailLine {
  id: number;
  vehiclePartId: number;
  partCode: string | null;
  partName: string | null;
  statusId: number;
  statusName: string | null;
  front: boolean;
  back: boolean;
  onLeft: boolean;
  onRight: boolean;
  enteredAt: string;
  note: string | null;
}

/** Full technical-exam report (header + measurements + defect lines). */
export interface TechExamReportFull {
  id: number;
  companyId: number;
  customerVehicleRelationId: number | null;
  clientName: string | null;
  clientMB: string | null;
  vehicleId: number | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  vehicleMakerModel: string | null;
  technicalExamTypeId: number;
  typeCode: string | null;
  typeName: string | null;
  typeValidDays: number;
  organizationId: number;
  organizationName: string | null;
  organizationCode: string | null;
  regNumber: string | null;
  madeDate: string;
  validTillDate: string;
  firstControllerLegacyId: number | null;
  secondControllerLegacyId: number | null;
  firstControllerName: string | null;
  secondControllerName: string | null;
  vehicleIsRight: boolean;
  explanationNote: string | null;
  driversWarning: string | null;
  note: string | null;
  technicalChanges: string | null;
  axles: TechExamAxleReading[];
  weight: number | null;
  effectOfWorkingBrakeEmpty: number | null;
  effectOfWorkingBrakeFull: number | null;
  effectOfSecondaryBrake: number | null;
  effectOfParkingBrake: number | null;
  co: number | null;
  coPlusTurns: number | null;
  engineRpm: number | null;
  lambda: number | null;
  pinpoints: number | null;
  speedOfTurns: number | null;
  noise: number | null;
  engineOilTemp: number | null;
  createdAt: string;
  modifiedAt: string | null;
  details: TechExamDetailLine[];
}

/** Записник print bundle (legacy rptTehnickiPregledZapisnik, stamped on pre-printed paper). */
export interface TechExamZapisnik {
  id: number;
  regNumber: string | null;
  madeDate: string;
  technicalExamTypeId: number;
  isSocial: boolean;
  organizationName: string | null;
  customerName: string | null;
  cityName: string | null;
  communityName: string | null;
  livingAddress: string | null;
  plate: string | null;
  maker: string | null;
  modelFull: string | null;
  makeYear: number | null;
  madeCountry: string | null;
  colorFull: string | null;
  vin: string | null;
  engineTypeAndNum: string | null;
  engineCapacityCc: number | null;
  enginePowerKw: number | null;
  emptyWeightKg: number | null;
  maxAllowedWeightKg: number | null;
  axleCount: number | null;
  propulsionAxis: number | null;
  seats: number | null;
}

/** Technical-exam type lookup (for the register filter). */
export interface TechExamType {
  id: number;
  code: string | null;
  description: string;
  validDays: number;
}

// ---------- Technical-exam create/edit ----------

/** Lookup for the org/station picker. */
export interface TechExamOrgLookup { id: number; code: string | null; name: string | null }
/** Lookup for the defect status picker. */
export interface TechExamStatusLookup { id: number; name: string }
/** Lookup for the defective-part picker. */
export interface TechExamPartLookup { id: number; categoryId: number; code: string; description: string }
/** Lookup for the inspector (controller) picker. */
export interface TechExamControllerLookup { id: number; fullName: string }

/** One defect line written back to the API. */
export interface TechExamDetailWrite {
  vehiclePartId: number;
  statusId: number;
  front: boolean;
  back: boolean;
  onLeft: boolean;
  onRight: boolean;
  note: string | null;
}

/** Create/update payload — mirrors backend TechExamWriteDto (all measurements optional). */
export interface TechExamWrite {
  customerVehicleRelationId: number | null;
  technicalExamTypeId: number;
  organizationId: number;
  madeDate: string;                       // yyyy-MM-dd (DateOnly)
  firstControllerLegacyId: number | null;
  secondControllerLegacyId: number | null;
  explanationNote: string | null;
  driversWarning: string | null;
  note: string | null;
  technicalChanges: string | null;
  axis1Left: number | null; axis1Right: number | null; axis1Gj: number | null; axis1LeftRightDiff: number | null; axis1Coefficient: number | null;
  axis2Left: number | null; axis2Right: number | null; axis2Gj: number | null; axis2LeftRightDiff: number | null; axis2Coefficient: number | null;
  axis3Left: number | null; axis3Right: number | null; axis3Gj: number | null; axis3LeftRightDiff: number | null; axis3Coefficient: number | null;
  axis4Left: number | null; axis4Right: number | null; axis4Gj: number | null; axis4LeftRightDiff: number | null; axis4Coefficient: number | null;
  axisParkingLeft: number | null; axisParkingRight: number | null; axisParkingGj: number | null; axisParkingLeftRightDiff: number | null; axisParkingCoefficient: number | null;
  weight: number | null;
  effectOfWorkingBrakeEmpty: number | null;
  effectOfWorkingBrakeFull: number | null;
  effectOfSecondaryBrake: number | null;
  effectOfParkingBrake: number | null;
  speedOfTurns: number | null;
  co: number | null;
  engineRpm: number | null;
  coPlusTurns: number | null;
  lambda: number | null;
  pinpoints: number | null;
  noise: number | null;
  engineOilTemp: number | null;
  details: TechExamDetailWrite[];
}

// ---------- Payment module (Phase 1: read-only) ----------

export interface PaymentListItem {
  id: number;
  companyId: number;
  documentNumber: string;
  issueDate: string;
  dueDate: string;
  paymentTypeId: number;
  paymentTypeName: string | null;
  customerVehicleRelationId: number;
  clientName: string | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  linesTotal: number;
  discount: number | null;
  paid: boolean;
  stornoed: boolean;
  active: boolean;
}

export interface PaymentLine {
  id: number;
  priceCatalogId: number;
  priceCatalogName: string | null;
  unitPrice: number;
  vatPercent: number;
  discount: number;
  quantity: number;
  note: string | null;
  prePaid: boolean;
  prePaidNote: string | null;
  customerDebtId: number | null;
  active: boolean;
}

export interface Installment {
  id: number;
  sequenceNo: number;
  amount: number;
  dueDate: string | null;
  paid: boolean;
  paidAt: string | null;
  paidAmount: number | null;
  note: string | null;
}

export interface InstallmentAgreement {
  id: number;
  number: string;
  date: string;
  totalInstallments: number;
  guarantorName: string | null;
  guarantorAddress: string | null;
  guarantorEmbg: string | null;
}

export interface PaymentDetail {
  id: number;
  companyId: number;
  documentNumber: string;
  issueDate: string;
  dueDate: string;
  paymentTypeId: number;
  paymentTypeName: string | null;
  customerVehicleRelationId: number;
  clientName: string | null;
  clientMB: string | null;
  vehicleId: number | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  vehicleMakerModel: string | null;
  organizationId: number;
  operatorLegacyId: number | null;
  discount: number | null;
  paid: boolean;
  stornoed: boolean;
  stornoReason: string | null;
  note: string | null;
  agreementId: number | null;
  agreement: InstallmentAgreement | null;
  invoicedToCompanyId: number | null;
  fiscalPrintedAt: string | null;
  legacyId: number | null;
  active: boolean;
  createdAt: string;
  modifiedAt: string | null;
  linesTotal: number;
  lines: PaymentLine[];
  installments: Installment[];
}

export interface PaymentTypeLookup {
  id: number;
  code: string | null;
  name: string;
  isCash: boolean;
  isCard: boolean;
  isInstallment: boolean;
  printsReceipt: boolean;
  printsInvoice: boolean;
  prefix: string | null;
}

export interface VatRateLookup {
  id: number;
  code: string | null;
  name: string;
  percent: number;
}

/** A single open-debt row from /api/customer-debts. */
export interface CustomerDebtRow {
  id: number;
  companyId: number;
  customerVehicleRelationId: number;
  clientName: string | null;
  clientMB: string | null;
  vehicleId: number | null;
  vehiclePlate: string | null;
  vehicleVin: string | null;
  vehicleMakerModel: string | null;
  priceCatalogId: number;
  priceCatalogName: string | null;
  price: number;
  vatPercent: number;
  note: string | null;
  origin: number;
  originRequestId: number | null;
  originTechnicalExamId: number | null;
  organizationId: number;
  paid: boolean;
  settledByLineId: number | null;
  createdAt: string;
}

/** Certificate print bundle (legacy „Потврда за техничка исправност"). */
export interface TechExamCertOrg {
  name: string | null;
  address: string | null;
  cityLine: string | null;
  phone: string | null;
  fax: string | null;
}
export interface TechExamCertVehicle {
  registration: string | null;
  category: string | null;
  maker: string | null;
  typeText: string | null;
  model: string | null;
  vin: string | null;
}
export interface TechExamCertificate {
  id: number;
  regNumber: string | null;
  madeDate: string;
  validTillDate: string;
  vehicleIsRight: boolean;
  stationCity: string | null;
  controllerName: string | null;
  organization: TechExamCertOrg;
  vehicle: TechExamCertVehicle;
}

// ---------- PriceCatalog ----------

/** Mirrors VTE.Domain.Payments.PriceTrigger. */
export enum PriceTrigger {
  None = 0,
  TechnicalExam = 1,
  Request = 2,
  TrafficLicence = 3,
  Permission = 4,
  IDL = 5,
  TechnicalExamIrregular = 6,
}

export interface PriceCatalog {
  id: number;
  code: string | null;
  name: string;
  basePrice: number;
  vatRateId: number;
  trigger: PriceTrigger;
  vehiclePaymentCategoryId: number | null;
  communityId: number | null;
  priceCompanyId: number | null;
  paymentCategoryGroupId: number | null;
  vehicleField: string | null;
  parametarFrom: number | null;
  parametarTo: number | null;
  vehicleCategoryFilter: string | null;
  bankAccount: string | null;
  paymentForm: string | null;
  active: boolean;
}

export type PriceCatalogWrite = Omit<PriceCatalog, 'id'>;

export interface PriceCatalogLookups {
  triggers: { id: number; name: string }[];
  companies: number[];
  vehicleFields: string[];
  categoryGroups: number[];
}

/** A vehicle-payment category in the price-catalog master rail. id=null is the
 * synthetic "uncategorized" bucket (rules with no VehiclePaymentCategoryId). */
export interface PriceCategoryNode {
  id: number | null;
  name: string;
  zelenMap: number | null;
  active: boolean;
  ruleCount: number;
  activeRuleCount: number;
}
