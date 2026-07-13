<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { openPrintTab } from '@/utils/print';
import { onClientCreated } from '@/utils/clientBus';
import { useAuthStore } from '@/stores/auth';
import type {
  Citizenship, City, Client, ClientPersonalData, Country, DocumentIssuer, Paged, TechExamOrgLookup,
  Vehicle, VehicleBodyType, VehicleCategory, VehicleColor, VehicleEcoProgram, VehicleEngineType,
  VehicleFuel, VehicleListItem, VehicleMaker, VehicleModel, VehicleRegistration,
  VehiclePermissionFull, VehiclePermissionWrite,
} from '@/types';
import ClientFields from '@/components/PermissionClientFields.vue';
import Button from 'primevue/button';
import AutoComplete from 'primevue/autocomplete';
import InputText from 'primevue/inputtext';
import InputNumber from 'primevue/inputnumber';
import Textarea from 'primevue/textarea';
import Checkbox from 'primevue/checkbox';
import Select from 'primevue/select';
import DatePicker from 'primevue/datepicker';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';

const props = defineProps<{ id?: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();
const confirm = useConfirm();
const auth = useAuthStore();

const isEdit = computed(() => !!props.id);
const loading = ref(false);
const saving = ref(false);

// ---- Catalogs ----
const organizations = ref<TechExamOrgLookup[]>([]);
const documentIssuers = ref<DocumentIssuer[]>([]);
const cities = ref<City[]>([]);
const citizenships = ref<Citizenship[]>([]);
const bodyTypes = ref<VehicleBodyType[]>([]);
const vehCategories = ref<VehicleCategory[]>([]);
const makers = ref<VehicleMaker[]>([]);
const models = ref<VehicleModel[]>([]);
const colors = ref<VehicleColor[]>([]);
const fuels = ref<VehicleFuel[]>([]);
const ecoPrograms = ref<VehicleEcoProgram[]>([]);
const engineTypes = ref<VehicleEngineType[]>([]);
const countries = ref<Country[]>([]);

const issuerOptions = computed(() => documentIssuers.value.map(x => ({ id: x.id, label: x.name ?? `#${x.id}` })));
const documentIssuerNames = computed(() => documentIssuers.value.map(x => x.name).filter(Boolean) as string[]);
const cityOptions = computed(() => cities.value.map(c => ({ id: c.id, label: c.name })));
const orgOptions = computed(() =>
  organizations.value.map(o => ({ id: o.id, label: o.name || o.code || `#${o.id}` })));

// ---- Header/document state ----
const issuerOrganizationId = ref<number | null>(null);
const issuerId = ref<number | null>(null);
const issuingCityId = ref<number | null>(null);
const permissionNumber = ref('');
const trafficLicenceNumber = ref('');
const triptiqueNumber = ref('');
const issuedDate = ref<Date | null>(new Date());
const startDate = ref<Date | null>(new Date());
const validTillDate = ref<Date | null>(plusYears(new Date(), 1));
const note = ref('');

function plusYears(d: Date, n: number): Date {
  const r = new Date(d);
  r.setFullYear(r.getFullYear() + n);
  return r;
}

// Датум на важност = издавање + 1 година (легаси правило)
let suppressAutoValidTill = false;
watch(issuedDate, (d) => {
  if (suppressAutoValidTill) return;
  validTillDate.value = d ? plusYears(d, 1) : null;
}, { flush: 'sync' });

// =====================================================================
// 1. ВОЗИЛО — search → the REAL Vehicle record, fully editable (легаси екран
//    „Податоци за возило" до Бр. на тркала). Written back via PUT /vehicles/{id}.
// =====================================================================
type VehicleOpt = { relationId: number; label: string; sub: string };
const vehicleSel = ref<VehicleOpt | string | null>(null);
const vehicleSuggestions = ref<VehicleOpt[]>([]);
const vehicleRaw = new Map<number, VehicleListItem>();
const selectedRelationId = ref<number | null>(null);
const ownerClientId = ref<number | null>(null);
const vehicleId = ref<number | null>(null);
// true за мигрирани полномошна каде возилото постои само како snapshot (нема жив
// Vehicle врзан за релацијата) — техничкиот картон тогаш не е применлив.
const snapshotOnlyVehicle = ref(false);

const veh = ref<Partial<Vehicle>>({});
const selectedMakerId = ref<number | null>(null);
const manufactureYear = ref<number | null>(null);
const filteredModels = computed(() =>
  selectedMakerId.value ? models.value.filter(m => m.makerId === selectedMakerId.value) : models.value);

// Регистрации (последна/прва) — read-only контекст како во легаси екранот
const lastReg = ref<VehicleRegistration | null>(null);
const firstReg = ref<VehicleRegistration | null>(null);

function toVehicleOpt(v: VehicleListItem): VehicleOpt {
  const disp = [v.maker, v.model].filter(Boolean).join(' ');
  return {
    relationId: v.relationId,
    label: `${v.plate ?? '—'} · ${disp || v.vin}`,
    sub: `${v.ownerName ?? '—'}${v.relationTypeName ? ` · ${v.relationTypeName}` : ''}`,
  };
}
async function onVehicleComplete(e: { query: string }) {
  const term = (e.query || '').trim();
  if (term.length < 2) { vehicleSuggestions.value = []; return; }
  try {
    const { data } = await api.get<Paged<VehicleListItem>>('/vehicles', { params: { q: term, pageSize: 20 } });
    vehicleRaw.clear();
    data.items.forEach(v => vehicleRaw.set(v.relationId, v));
    vehicleSuggestions.value = data.items.map(toVehicleOpt);
  } catch { vehicleSuggestions.value = []; }
}
async function onVehicleSelect(e: { value: VehicleOpt }) {
  const row = vehicleRaw.get(e.value.relationId);
  if (!row) return;
  selectedRelationId.value = row.relationId;
  await loadVehicle(row.id);
  // сопственикот доаѓа со релацијата
  await loadOwnerFromRelation(row.relationId);
}
async function loadVehicle(id: number) {
  const { data: v } = await api.get<Vehicle>(`/vehicles/${id}`);
  vehicleId.value = v.id;
  veh.value = { ...v };
  manufactureYear.value = v.manufactureDate ? new Date(v.manufactureDate).getFullYear() : null;
  const model = v.modelId ? models.value.find(m => m.id === v.modelId) : null;
  selectedMakerId.value = model?.makerId ?? null;
  try {
    const { data: regs } = await api.get<VehicleRegistration[]>('/vehicle-registrations', { params: { vehicleId: id } });
    const act = regs.filter(r => r.active);
    lastReg.value = act.length ? act.reduce((a, b) => (a.registeredDate > b.registeredDate ? a : b)) : null;
    firstReg.value = regs.find(r => r.isFirstRegistration) ?? null;
  } catch { lastReg.value = null; firstReg.value = null; }
}
function clearVehicle() {
  if (isEdit.value) return;
  selectedRelationId.value = null;
  vehicleId.value = null;
  snapshotOnlyVehicle.value = false;
  ownerClientId.value = null;
  vehicleSel.value = null;
  vehicleSuggestions.value = [];
  veh.value = {};
  manufactureYear.value = null;
  selectedMakerId.value = null;
  lastReg.value = null;
  firstReg.value = null;
  owner.value = {};
  ownerDocs.value = { idCard: blankDoc(), passport: blankDoc(), licence: blankDoc() };
}

// =====================================================================
// 2/3. КЛИЕНТИ — full editable client (легаси „Податоци за сопственикот /
//      овластено лице") + 3 документи. Written back via PUT /clients/{id}.
// =====================================================================
interface DocForm { id: number; number: string; dateIssued: Date | null; expiresAt: Date | null; issuerName: string; }
function blankDoc(): DocForm { return { id: 0, number: '', dateIssued: null, expiresAt: null, issuerName: '' }; }
interface ClientDocs { idCard: DocForm; passport: DocForm; licence: DocForm; }

const owner = ref<Partial<Client>>({});
const ownerDocs = ref<ClientDocs>({ idCard: blankDoc(), passport: blankDoc(), licence: blankDoc() });
const authorized = ref<Partial<Client>>({});
const authorizedDocs = ref<ClientDocs>({ idCard: blankDoc(), passport: blankDoc(), licence: blankDoc() });
const authorizedClientId = ref<number | null>(null);

interface PermVehicleDefaults {
  relationId: number; ownerClientId: number; vehicleId: number | null;
  vehicleDisplay: string | null; plateNumber: string | null;
}
async function loadOwnerFromRelation(relationId: number): Promise<PermVehicleDefaults> {
  const { data: d } = await api.get<PermVehicleDefaults>('/vehicle-permissions/defaults', { params: { relationId } });
  ownerClientId.value = d.ownerClientId;
  const { data: c } = await api.get<Client>(`/clients/${d.ownerClientId}`);
  owner.value = { ...c };
  ownerDocs.value = await loadDocs(c.id);
  return d;
}

async function loadDocs(clientId: number): Promise<ClientDocs> {
  const out: ClientDocs = { idCard: blankDoc(), passport: blankDoc(), licence: blankDoc() };
  try {
    const { data: rows } = await api.get<ClientPersonalData[]>(`/client-personal-data/by-client/${clientId}`);
    const issuerName = (id: number) => documentIssuers.value.find(i => i.id === id)?.name ?? '';
    for (const [key, typeId] of [['idCard', 3], ['passport', 2], ['licence', 1]] as const) {
      const r = rows.find(x => x.personalDataTypeId === typeId && x.active);
      if (r) out[key] = {
        id: r.id, number: r.number,
        dateIssued: r.createdAt ? new Date(r.createdAt) : null,
        expiresAt: r.expiresAt ? new Date(r.expiresAt) : null,
        issuerName: issuerName(r.documentIssuerId),
      };
    }
  } catch { /* best-effort */ }
  return out;
}

// Овластено лице — picker
type ClientOpt = { id: number; name: string; mb: string | null; label: string };
const authorizedSel = ref<ClientOpt | string | null>(null);
const authorizedSuggestions = ref<ClientOpt[]>([]);
const authorizedRaw = new Map<number, Client>();

function clientDisplayName(c: Partial<Client>): string {
  return [c.firstName, c.middleName, c.lastName].filter(Boolean).join(' ').trim() || (c.id ? `#${c.id}` : '');
}
function toClientOpt(c: Client): ClientOpt {
  const name = clientDisplayName(c);
  return { id: c.id, name, mb: c.mb, label: c.mb ? `${name} · ЕМБГ ${c.mb}` : name };
}
async function onAuthorizedComplete(e: { query: string }) {
  const term = (e.query || '').trim();
  if (term.length < 2) { authorizedSuggestions.value = []; return; }
  try {
    const { data } = await api.get<Paged<Client>>('/clients', { params: { q: term, pageSize: 20 } });
    authorizedRaw.clear();
    data.items.forEach(c => authorizedRaw.set(c.id, c));
    authorizedSuggestions.value = data.items.map(toClientOpt);
  } catch { authorizedSuggestions.value = []; }
}
async function onAuthorizedSelect(e: { value: ClientOpt }) {
  const c = authorizedRaw.get(e.value.id);
  if (!c) return;
  authorizedClientId.value = c.id;
  authorizedSel.value = toClientOpt(c);
  authorizedSuggestions.value = [];
  const { data: full } = await api.get<Client>(`/clients/${c.id}`);
  authorized.value = { ...full };
  authorizedDocs.value = await loadDocs(c.id);
}
function clearAuthorized() {
  if (isEdit.value) return;
  authorizedClientId.value = null;
  authorizedSel.value = null;
  authorizedSuggestions.value = [];
  authorized.value = {};
  authorizedDocs.value = { idCard: blankDoc(), passport: blankDoc(), licence: blankDoc() };
}

// „Нов" — отвора нова картичка за клиент во ново јазиче (исто како кај Барања).
// Штом таму се сними, BroadcastChannel-от долу го пополнува пикерот тука автоматски.
function openAuthorizedNew() { window.open(router.resolve({ name: 'client-new' }).href, '_blank'); }
const offClientCreated = onClientCreated(async (c) => {
  // само на празна нова форма — не клоберувај веќе избрано овластено лице
  if (isEdit.value || authorizedClientId.value) return;
  authorizedClientId.value = c.id;
  authorizedSel.value = toClientOpt(c);
  authorizedSuggestions.value = [];
  authorized.value = { ...c };
  authorizedDocs.value = await loadDocs(c.id);
});
onUnmounted(offClientCreated);

// Operator's org is always their own station
const orgLocked = computed(() => auth.companyId != null && issuerOrganizationId.value != null);
const orgName = computed(() => orgOptions.value.find(o => o.id === issuerOrganizationId.value)?.label ?? '—');

async function loadCatalogs() {
  const [og, di, ci, cz, bt, vc, mk2, mdl, col, fu, eco, et, co] = await Promise.all([
    api.get<TechExamOrgLookup[]>('/technical-exams/organizations'),
    api.get<DocumentIssuer[]>('/document-issuers'),
    api.get<City[]>('/cities'),
    api.get<Citizenship[]>('/citizenships'),
    api.get<VehicleBodyType[]>('/vehicles/ref/body-types'),
    api.get<VehicleCategory[]>('/vehicles/ref/categories'),
    api.get<VehicleMaker[]>('/vehicles/ref/makers'),
    api.get<VehicleModel[]>('/vehicles/ref/models'),
    api.get<VehicleColor[]>('/vehicles/ref/colors'),
    api.get<VehicleFuel[]>('/vehicles/ref/fuels'),
    api.get<VehicleEcoProgram[]>('/vehicles/ref/eco-programs'),
    api.get<VehicleEngineType[]>('/vehicles/ref/engine-types'),
    api.get<Country[]>('/countries'),
  ]);
  organizations.value = og.data;
  documentIssuers.value = di.data;
  cities.value = ci.data;
  citizenships.value = cz.data;
  bodyTypes.value = bt.data;
  vehCategories.value = vc.data;
  makers.value = mk2.data;
  models.value = mdl.data;
  colors.value = col.data;
  fuels.value = fu.data;
  ecoPrograms.value = eco.data;
  engineTypes.value = et.data;
  countries.value = co.data;
}

function toDate(s: string | null | undefined): Date | null {
  return s ? new Date(s) : null;
}
function toIsoDate(d: Date | null | undefined): string | null {
  if (!d) return null;
  const dd = d instanceof Date ? d : new Date(d);
  const y = dd.getFullYear();
  const m = String(dd.getMonth() + 1).padStart(2, '0');
  const day = String(dd.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}
function fmtDate(s: string | null | undefined): string {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}

async function loadPermission() {
  if (!props.id) return;
  loading.value = true;
  try {
    const { data: r } = await api.get<VehiclePermissionFull>(`/vehicle-permissions/${props.id}`);
    selectedRelationId.value = r.clientVehicleRelationId;
    authorizedClientId.value = r.authorizedClientId;
    issuerId.value = r.issuerId;
    issuingCityId.value = r.issuingCityId;
    issuerOrganizationId.value = r.issuerOrganizationId;
    permissionNumber.value = r.permissionNumber ?? '';
    trafficLicenceNumber.value = r.trafficLicenceNumber;
    triptiqueNumber.value = r.triptiqueNumber ?? '';
    suppressAutoValidTill = true;
    issuedDate.value = toDate(r.issuedDate);
    suppressAutoValidTill = false;
    startDate.value = toDate(r.startDate);
    validTillDate.value = toDate(r.validTillDate);
    note.value = r.note ?? '';
    // вчитај ги живите записи (возило + двајцата клиенти). Возилото се разрешува
    // ДИРЕКТНО од релацијата (defaults.vehicleId) — не преку пребарување по таблица,
    // кое паѓаше кога snapshot таблицата се разликува/празна или возилото е одјавено.
    const defs = await loadOwnerFromRelation(r.clientVehicleRelationId);
    const disp = defs.vehicleDisplay || r.vehicleDisplay || '';
    const plate = defs.plateNumber || r.plateNumber || '—';
    const ownerLabel = [owner.value?.firstName, owner.value?.lastName].filter(Boolean).join(' ');
    if (defs.vehicleId) {
      // жив Vehicle запис постои → полн уредлив картон
      await loadVehicle(defs.vehicleId);
    } else if (r.plateNumber || r.vehicleDisplay) {
      // мигрирано полномошно: возилото е зачувано само како snapshot (нема жив
      // Vehicle врзан за релацијата). Прикажи ги идентификациските полиња од
      // snapshot-от за да се гледа кое е возилото; техничкиот картон останува празен.
      veh.value = { plate: r.plateNumber ?? undefined, vin: r.vehicleVin ?? undefined, engineNumber: r.vehicleEngineNumber ?? undefined };
      snapshotOnlyVehicle.value = true;
    }
    // прикажи во AutoComplete (v-model = VehicleOpt со .label) во секој случај
    if (defs.vehicleId || r.plateNumber || r.vehicleDisplay) {
      vehicleSel.value = {
        relationId: r.clientVehicleRelationId,
        label: `${plate} · ${disp || '—'}`,
        sub: ownerLabel,
      };
    }
    const { data: authC } = await api.get<Client>(`/clients/${r.authorizedClientId}`);
    authorized.value = { ...authC };
    authorizedDocs.value = await loadDocs(authC.id);
    authorizedSel.value = toClientOpt(authC);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('vehiclePermissions.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

// ---- Save (writes back vehicle + both clients + docs, then the permission) ----
function validate(): string | null {
  if (!selectedRelationId.value || !vehicleId.value) return t('vehiclePermissions.form.vehicleRequired');
  if (!authorizedClientId.value) return t('vehiclePermissions.form.authorizedRequired');
  if (!issuerOrganizationId.value) return t('vehiclePermissions.form.organization');
  if (!trafficLicenceNumber.value.trim()) return t('vehiclePermissions.form.trafficLicenceNumber');
  if (!issuerId.value) return t('vehiclePermissions.form.issuer');
  if (!issuingCityId.value) return t('vehiclePermissions.form.issuingCity');
  if (!issuedDate.value || !validTillDate.value || issuedDate.value >= validTillDate.value)
    return t('vehiclePermissions.form.validTillDate');
  return null;
}

function clientPayload(c: Partial<Client>): Record<string, unknown> {
  return {
    cityId: c.cityId ?? null,
    citizenshipId: c.citizenshipId ?? null,
    business: c.business ?? false,
    firstName: c.firstName?.trim() || null,
    middleName: c.middleName?.trim() || null,
    lastName: c.lastName?.trim() || null,
    mb: c.mb?.trim() || null,
    address: c.address?.trim() || null,
    taxNumber: c.taxNumber?.trim() || null,
    phoneNumber: c.phoneNumber?.trim() || null,
    email: c.email?.trim() || null,
    dateOfBirth: c.dateOfBirth ? toIsoDate(new Date(c.dateOfBirth)) : null,
    note: c.note ?? null,
    active: c.active ?? true,
    parentName: c.parentName?.trim() || null,
    birthCityId: c.birthCityId ?? null,
    fax: c.fax?.trim() || null,
    profession: c.profession?.trim() || null,
    employer: c.employer?.trim() || null,
    notificationsAllowed: c.notificationsAllowed ?? null,
  };
}

function issuerIdByName(name: string): number | null {
  const n = name.trim().toLowerCase();
  if (!n) return null;
  return documentIssuers.value.find(x => (x.name ?? '').trim().toLowerCase() === n)?.id ?? null;
}

async function saveDocs(clientId: number, docs: ClientDocs) {
  const items = [
    { typeId: 3, d: docs.idCard },
    { typeId: 2, d: docs.passport },
    { typeId: 1, d: docs.licence },
  ];
  for (const it of items) {
    const num = it.d.number.trim();
    if (!num) continue;
    const issuerId2 = issuerIdByName(it.d.issuerName);
    if (issuerId2 == null) continue; // непознат издавач — прескокни тивко
    const body = {
      clientId, personalDataTypeId: it.typeId, documentIssuerId: issuerId2,
      number: num, createdAt: toIsoDate(it.d.dateIssued), expiresAt: toIsoDate(it.d.expiresAt), active: true,
    };
    if (it.d.id > 0) await api.put(`/client-personal-data/${it.d.id}`, body);
    else {
      const { data } = await api.post<ClientPersonalData>('/client-personal-data', body);
      it.d.id = data.id;
    }
  }
}

function vehiclePayload(): Record<string, unknown> {
  const v = veh.value;
  const p: Record<string, unknown> = { ...v };
  delete p.id; delete p.createdAt; delete p.companyId;
  p.manufactureDate = manufactureYear.value ? `${manufactureYear.value}-01-01` : null;
  return p;
}

function cityName(id: number | null | undefined): string {
  return (id ? cities.value.find(c => c.id === id)?.name : '') ?? '';
}
function buildSnapshot(): Pick<VehiclePermissionWrite,
  'ownerName' | 'ownerIdNumber' | 'ownerAddress' | 'authorizedName' | 'authorizedEmbg'
  | 'authorizedIdCardNumber' | 'authorizedPassportNumber' | 'authorizedAddress'
  | 'vehicleDisplay' | 'plateNumber' | 'vehicleVin' | 'vehicleEngineNumber'> {
  const model = veh.value.modelId ? models.value.find(m => m.id === veh.value.modelId) : null;
  const maker = model ? makers.value.find(x => x.id === model.makerId) : null;
  const display = [maker?.name, model?.name, veh.value.modelVariant].filter(Boolean).join(' ');
  const s = (v: string | null | undefined) => (v ?? '').trim() || null;
  return {
    ownerName: s(clientDisplayName(owner.value)),
    ownerIdNumber: s(owner.value.mb),
    ownerAddress: s([owner.value.address, cityName(owner.value.cityId)].filter(Boolean).join(' ')),
    authorizedName: s(clientDisplayName(authorized.value)),
    authorizedEmbg: s(authorized.value.mb),
    authorizedIdCardNumber: s(authorizedDocs.value.idCard.number),
    authorizedPassportNumber: s(authorizedDocs.value.passport.number),
    authorizedAddress: s([authorized.value.address, cityName(authorized.value.cityId)].filter(Boolean).join(' ')),
    vehicleDisplay: s(display),
    plateNumber: s(veh.value.plate),
    vehicleVin: s(veh.value.vin),
    vehicleEngineNumber: s(veh.value.engineNumber),
  };
}

function buildBody(): VehiclePermissionWrite {
  const s = (v: string) => v.trim() || null;
  return {
    clientVehicleRelationId: selectedRelationId.value!,
    authorizedClientId: authorizedClientId.value!,
    issuerId: issuerId.value!,
    issuingCityId: issuingCityId.value!,
    issuerOrganizationId: issuerOrganizationId.value!,
    permissionNumber: s(permissionNumber.value),
    trafficLicenceNumber: trafficLicenceNumber.value.trim(),
    triptiqueNumber: s(triptiqueNumber.value),
    issuedDate: toIsoDate(issuedDate.value)!,
    startDate: toIsoDate(startDate.value),
    validTillDate: toIsoDate(validTillDate.value)!,
    note: s(note.value),
    ...buildSnapshot(),
  };
}

function openPrints(id: number) {
  openPrintTab(router.resolve({ name: 'vehicle-permission-print', params: { id } }).href);
  openPrintTab(router.resolve({ name: 'vehicle-permission-request-print', params: { id } }).href);
}

/** Сними и Печати: возило + двајцата клиенти + документите + полномошното, па двата печата. */
async function savePrint() {
  const err = validate();
  if (err) {
    toast.add({ severity: 'warn', summary: t('vehiclePermissions.form.saveFailed'), detail: err, life: 4000 });
    return;
  }
  saving.value = true;
  try {
    // 1) write back the master records (легаси екраните ги уредуваа истите)
    await api.put(`/vehicles/${vehicleId.value}`, vehiclePayload());
    await api.put(`/clients/${ownerClientId.value}`, clientPayload(owner.value));
    await api.put(`/clients/${authorizedClientId.value}`, clientPayload(authorized.value));
    await saveDocs(ownerClientId.value!, ownerDocs.value);
    await saveDocs(authorizedClientId.value!, authorizedDocs.value);

    // 2) the permission itself (+ debt on create)
    if (isEdit.value) {
      await api.put(`/vehicle-permissions/${props.id}`, buildBody());
      toast.add({ severity: 'success', summary: t('vehiclePermissions.form.saved'), life: 1500 });
      openPrints(Number(props.id));
    } else {
      const { data } = await api.post<VehiclePermissionFull>('/vehicle-permissions', buildBody());
      toast.add({ severity: 'success', summary: t('vehiclePermissions.form.saved'), life: 1500 });
      openPrints(data.id);
      router.replace(`/vehicle-permissions/${data.id}`);
    }
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('vehiclePermissions.form.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    saving.value = false;
  }
}

function remove() {
  if (!isEdit.value) return;
  confirm.require({
    message: t('vehiclePermissions.deleteConfirm'),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/vehicle-permissions/${props.id}`);
        toast.add({ severity: 'success', summary: t('vehiclePermissions.deleted'), life: 1500 });
        router.push('/vehicle-permissions');
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

onMounted(async () => {
  await loadCatalogs();
  if (isEdit.value) {
    await loadPermission();
  } else {
    if (auth.companyId != null) {
      const own = organizations.value.find(o => o.companyId === auth.companyId);
      if (own) issuerOrganizationId.value = own.id;
    }
    const veles = cities.value.find(c => c.name?.trim().toLowerCase() === 'велес');
    if (veles) issuingCityId.value = veles.id;
    const mvr = documentIssuers.value.find(i => (i.name ?? '').trim().toUpperCase() === 'МВР ВЕЛЕС');
    if (mvr) issuerId.value = mvr.id;
  }
});

const tp = (k: string) => t(`vehiclePermissions.form.${k}`);
</script>

<template>
  <div class="perm-form">
    <div class="page-header">
      <div>
        <h1>
          <span v-if="isEdit">{{ t('vehiclePermissions.form.editTitle', { id: props.id }) }}</span>
          <span v-else>{{ t('vehiclePermissions.form.newTitle') }}</span>
        </h1>
        <div v-if="veh.plate || owner.id" class="subtitle muted">
          {{ [veh.plate, clientDisplayName(owner)].filter(Boolean).join(' · ') }}
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty">
      <i class="pi pi-spin pi-spinner" />
      {{ t('common.loading') }}…
    </div>

    <template v-else>
      <!-- ===================== 1. ВОЗИЛО ===================== -->
      <div class="card">
        <div class="card-header">{{ tp('sections.vehicle') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field picker-field">
              <label>{{ tp('vehicleSearch') }} *</label>
              <div v-if="!isEdit" class="picker-row">
                <AutoComplete
                  v-model="vehicleSel"
                  :suggestions="vehicleSuggestions"
                  optionLabel="label"
                  :placeholder="tp('vehicleSearchPlaceholder')"
                  class="picker-ac"
                  @complete="onVehicleComplete"
                  @item-select="onVehicleSelect">
                  <template #option="{ option }">
                    <div class="ac-opt">
                      <span class="ac-main"><strong>{{ option.label }}</strong></span>
                      <span class="ac-sub">{{ option.sub }}</span>
                    </div>
                  </template>
                </AutoComplete>
                <Button v-if="vehicleId" icon="pi pi-times" severity="secondary" outlined size="small"
                        class="picker-clear" @click="clearVehicle" />
              </div>
              <div v-else class="picked locked">
                <i class="pi pi-car" />
                <span class="picked-name">{{ veh.plate ?? '—' }}</span>
                <i class="pi pi-lock muted lock-icon" />
              </div>
            </div>
          </div>

          <!-- Мигрирано полномошно: возилото постои само како snapshot -->
          <template v-if="snapshotOnlyVehicle">
            <div class="snapshot-note">
              <i class="pi pi-info-circle" /> {{ tp('vehicleSnapshotNote') }}
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('plateNumber') }}</label><InputText :modelValue="veh.plate ?? '—'" readonly /></div>
              <div class="field"><label>{{ tp('vehicleVin') }}</label><InputText :modelValue="veh.vin ?? '—'" readonly /></div>
              <div class="field"><label>{{ tp('vehicleEngine') }}</label><InputText :modelValue="veh.engineNumber ?? '—'" readonly /></div>
            </div>
          </template>

          <template v-if="vehicleId">
            <!-- Регистрации (read-only контекст) -->
            <div class="reg-strip">
              <span><b>{{ tp('lastRegDate') }}:</b> {{ fmtDate(lastReg?.registeredDate) }}</span>
              <span><b>{{ tp('regValidUntil') }}:</b> {{ fmtDate(lastReg?.validUntil) }}</span>
              <span><b>{{ tp('regIssuer') }}:</b> {{ lastReg ? (issuerOptions.find(i => i.id === lastReg!.issuerId)?.label ?? '—') : '—' }}</span>
              <span><b>{{ tp('firstRegDate') }}:</b> {{ fmtDate(firstReg?.registeredDate) }}</span>
              <span><b>{{ tp('firstRegPlate') }}:</b> {{ firstReg?.plateNumber ?? '—' }}</span>
            </div>

            <div class="row">
              <div class="field"><label>{{ tp('plateNumber') }}</label><InputText v-model="veh.plate" maxlength="20" /></div>
              <div class="field"><label>{{ tp('vehicleVin') }}</label><InputText v-model="veh.vin" maxlength="40" /></div>
              <div class="field"><label>{{ tp('vehicleEngine') }}</label><InputText v-model="veh.engineNumber" maxlength="40" /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehCategory') }}</label>
                <Select v-model="veh.categoryId" :options="vehCategories" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehBodyType') }}</label>
                <Select v-model="veh.bodyTypeId" :options="bodyTypes" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehMaker') }}</label>
                <Select v-model="selectedMakerId" :options="makers" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehModel') }}</label>
                <Select v-model="veh.modelId" :options="filteredModels" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehVariant') }}</label><InputText v-model="veh.modelVariant" maxlength="400" /></div>
              <div class="field"><label>{{ tp('vehEngineType') }}</label>
                <Select v-model="veh.engineTypeId" :options="engineTypes" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehMadeCountry') }}</label>
                <Select v-model="veh.madeCountryId" :options="countries" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehYear') }}</label><InputNumber v-model="manufactureYear" :useGrouping="false" :min="1900" :max="2100" /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehPrimaryColor') }}</label>
                <Select v-model="veh.primaryColorId" :options="colors" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehSecondaryColor') }}</label>
                <Select v-model="veh.secondaryColorId" :options="colors" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehFuel') }}</label>
                <Select v-model="veh.fuelId" :options="fuels" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehSecondFuel') }}</label>
                <Select v-model="veh.secondFuelId" :options="fuels" optionLabel="name" optionValue="id" placeholder="—" showClear filter /></div>
              <div class="field"><label>{{ tp('vehEco') }}</label>
                <Select v-model="veh.ecoProgramId" :options="ecoPrograms" optionLabel="name" optionValue="id" placeholder="—" showClear /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehPowerKw') }}</label><InputNumber v-model="veh.enginePowerKw" :minFractionDigits="0" :maxFractionDigits="2" /></div>
              <div class="field"><label>{{ tp('vehPowerKs') }}</label><InputText :modelValue="veh.enginePowerKw != null ? (veh.enginePowerKw * 1.35962).toFixed(1) : ''" readonly /></div>
              <div class="field"><label>{{ tp('vehCc') }}</label><InputNumber v-model="veh.engineWorkingCapacityCc" :minFractionDigits="0" :maxFractionDigits="0" /></div>
              <div class="field"><label>{{ tp('vehRpm') }}</label><InputNumber v-model="veh.maxRpm" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehPowerPerCc') }}</label><InputNumber v-model="veh.powerPerCc" :minFractionDigits="0" :maxFractionDigits="3" /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehEmptyWeight') }}</label><InputNumber v-model="veh.emptyWeightKg" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehMaxAllowed') }}</label><InputNumber v-model="veh.maxAllowedWeightKg" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehHitchLoad') }}</label><InputNumber v-model="veh.maxHitchLoadKg" :useGrouping="false" /></div>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehSeats') }}</label><InputNumber v-model="veh.seats" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehStanding') }}</label><InputNumber v-model="veh.standingSeats" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehLying') }}</label><InputNumber v-model="veh.lyingSeats" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehAxles') }}</label><InputNumber v-model="veh.axleCount" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehPropulsionAxles') }}</label><InputNumber v-model="veh.propulsionAxleCount" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehWheels') }}</label><InputNumber v-model="veh.wheelCount" :useGrouping="false" /></div>
              <div class="field"><label>{{ tp('vehDoors') }}</label><InputNumber v-model="veh.doorCount" :useGrouping="false" /></div>
            </div>
            <div class="row checks">
              <label class="chk"><Checkbox v-model="veh.hasLpg" :binary="true" /> {{ tp('vehTng') }}</label>
              <label class="chk"><Checkbox v-model="veh.hasHook" :binary="true" /> {{ tp('vehHook') }}</label>
              <label class="chk"><Checkbox v-model="veh.hasWinch" :binary="true" /> {{ tp('vehWinch') }}</label>
              <label class="chk"><Checkbox v-model="veh.forPublicTransport" :binary="true" /> {{ tp('vehPublicTransport') }}</label>
            </div>
            <div class="row">
              <div class="field"><label>{{ tp('vehEngineIdMethod') }}</label><InputText v-model="veh.engineIdMethod" maxlength="100" /></div>
              <div class="field"><label>{{ tp('vehNoiseSpec') }}</label><InputText v-model="veh.noiseTechSpec" maxlength="100" /></div>
            </div>
          </template>
        </div>
      </div>

      <!-- ===================== 2. СОПСТВЕНИК ===================== -->
      <div v-if="ownerClientId" class="card">
        <div class="card-header">{{ tp('sections.owner') }}</div>
        <div class="card-body">
          <ClientFields :c="owner" :docs="ownerDocs"
                        :cities="cities" :citizenships="citizenships" :documentIssuers="documentIssuers" />
        </div>
      </div>

      <!-- ===================== 3. ОВЛАСТЕНО ЛИЦЕ ===================== -->
      <div class="card">
        <div class="card-header">{{ tp('sections.authorized') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field picker-field">
              <label>{{ tp('authorizedSearch') }} *</label>
              <div v-if="!isEdit" class="picker-row">
                <AutoComplete
                  v-model="authorizedSel"
                  :suggestions="authorizedSuggestions"
                  optionLabel="label"
                  :placeholder="tp('authorizedSearchPlaceholder')"
                  class="picker-ac"
                  @complete="onAuthorizedComplete"
                  @item-select="onAuthorizedSelect">
                  <template #option="{ option }">
                    <div class="ac-opt">
                      <span class="ac-main"><strong>{{ option.name }}</strong></span>
                      <span v-if="option.mb" class="ac-sub">ЕМБГ {{ option.mb }}</span>
                    </div>
                  </template>
                </AutoComplete>
                <Button :label="t('common.new')" icon="pi pi-plus" severity="secondary" outlined size="small"
                        class="picker-new" @click="openAuthorizedNew"
                        v-tooltip.bottom="t('requests.form.newOwnerClientHint')" />
                <Button v-if="authorizedClientId" icon="pi pi-times" severity="secondary" outlined size="small"
                        class="picker-clear" @click="clearAuthorized" />
              </div>
              <div v-else class="picked locked">
                <i class="pi pi-user" />
                <span class="picked-name">{{ clientDisplayName(authorized) || '—' }}</span>
                <i class="pi pi-lock muted lock-icon" />
              </div>
            </div>
          </div>
          <ClientFields v-if="authorizedClientId" :c="authorized" :docs="authorizedDocs"
                        :cities="cities" :citizenships="citizenships" :documentIssuers="documentIssuers" />
        </div>
      </div>

      <!-- ===================== 4. ОДОБРЕНИЕ ===================== -->
      <div class="card">
        <div class="card-header">{{ tp('sections.details') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field" style="max-width: 480px">
              <label>{{ tp('organization') }} *</label>
              <div v-if="orgLocked" class="issuer-locked">
                <InputText :modelValue="orgName" readonly />
                <i class="pi pi-lock" />
              </div>
              <Select v-else v-model="issuerOrganizationId" :options="orgOptions" optionLabel="label" optionValue="id"
                      filter placeholder="—" />
            </div>
          </div>
          <div class="row">
            <div class="field"><label>{{ tp('permissionNumber') }}</label><InputText v-model="permissionNumber" maxlength="100" /></div>
            <div class="field"><label>{{ tp('trafficLicenceNumber') }} *</label><InputText v-model="trafficLicenceNumber" maxlength="100" /></div>
            <div class="field"><label>{{ tp('triptiqueNumber') }}</label><InputText v-model="triptiqueNumber" maxlength="100" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ tp('issuedDate') }} *</label><DatePicker v-model="issuedDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
            <div class="field"><label>{{ tp('startDate') }}</label><DatePicker v-model="startDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
            <div class="field"><label>{{ tp('validTillDate') }} *</label><DatePicker v-model="validTillDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" /></div>
          </div>
          <div class="row">
            <div class="field"><label>{{ tp('issuer') }} *</label>
              <Select v-model="issuerId" :options="issuerOptions" optionLabel="label" optionValue="id" filter placeholder="—" /></div>
            <div class="field"><label>{{ tp('issuingCity') }} *</label>
              <Select v-model="issuingCityId" :options="cityOptions" optionLabel="label" optionValue="id" filter placeholder="—" /></div>
          </div>
          <div class="field">
            <label>{{ tp('note') }}</label>
            <Textarea v-model="note" rows="2" autoResize maxlength="500" />
          </div>
        </div>
      </div>

      <div class="footer-actions">
        <Button :label="t('common.back')" severity="secondary" outlined icon="pi pi-arrow-left" size="small"
                @click="router.push('/vehicle-permissions')" />
        <Button v-if="isEdit" :label="t('common.delete')" severity="danger" outlined icon="pi pi-trash" size="small" @click="remove" />
        <Button :label="tp('saveAndPrint')" icon="pi pi-print" size="small" :loading="saving" @click="savePrint" />
      </div>
    </template>
  </div>
</template>

<style scoped>
/* House multi-card layout (идентично со IDL/Client формите) */
.perm-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }

.perm-form :deep(.page-header)            { margin-bottom: 0.75rem; }
.perm-form :deep(.page-header h1)         { font-size: 1.125rem; letter-spacing: -0.01em; }
.perm-form .page-header .subtitle         { font-size: 0.75rem; margin-top: 0.1rem; }

.perm-form .card + .card                  { margin-top: 0.5rem; }
.perm-form :deep(.card .card-header)      { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.perm-form :deep(.card .card-body)        { padding: 0.625rem 0.875rem; }

.perm-form :deep(.row)                     { gap: 0.625rem; }
.perm-form :deep(.field)                   { gap: 0.15rem; margin-bottom: 0.45rem; }
.perm-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; color: var(--color-text-secondary); }

.perm-form :deep(.p-select),
.perm-form :deep(.p-datepicker),
.perm-form :deep(.p-inputnumber),
.perm-form :deep(.p-inputtext)             { width: 100%; }
.perm-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.perm-form :deep(.p-textarea)              { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.perm-form :deep(.p-select)                { font-size: 0.8125rem; }
.perm-form :deep(.p-select-label)          { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.perm-form :deep(.p-select-dropdown)       { width: 1.625rem; }
.perm-form :deep(.p-datepicker-input)      { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.perm-form :deep(.p-checkbox)              { transform: scale(0.85); transform-origin: left center; }
.perm-form :deep(.p-button)                { font-size: 0.8125rem; }
.perm-form :deep(.p-button.p-button-sm)    { padding: 0.3rem 0.625rem; }

/* Brand accent stripe */
.perm-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.perm-form :deep(.card .card-header)::before {
  content: '';
  position: absolute;
  left: 0; top: 0; bottom: 0;
  width: 3px;
  background: var(--color-brand-500);
  border-radius: 0 2px 2px 0;
}
.perm-form :deep(.card) { transition: box-shadow 0.15s ease, border-color 0.15s ease; }
.perm-form :deep(.card:focus-within) {
  box-shadow: 0 1px 2px rgba(0,0,0,0.04), 0 4px 12px rgba(0,0,0,0.06);
  border-color: color-mix(in srgb, var(--color-border) 70%, var(--color-brand-500));
}
.perm-form :deep(.p-inputtext:focus),
.perm-form :deep(.p-textarea:focus),
.perm-form :deep(.p-select:not(.p-disabled).p-focus),
.perm-form :deep(.p-datepicker-input:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

/* Pickers */
.picker-field { max-width: 520px; }
.picker-row { display: flex; align-items: center; gap: 0.4rem; }
.picker-ac { flex: 1 1 auto; }
.picker-ac :deep(.p-autocomplete) { width: 100%; }
.picker-ac :deep(.p-autocomplete-input) { width: 100%; }
.picker-clear,
.picker-new { flex: 0 0 auto; }
.ac-opt { display: flex; flex-direction: column; line-height: 1.25; }
.ac-main { font-size: 0.85rem; color: var(--color-text); }
.ac-sub { font-size: 0.75rem; color: var(--color-text-muted); }
.picked {
  display: flex; align-items: center; gap: 0.5rem;
  background: var(--color-surface-2); border: 1px solid var(--color-border);
  padding: 0.4rem 0.6rem; border-radius: 8px;
}
.picked > .pi-user, .picked > .pi-car { color: var(--color-brand-500); }
.picked-name { font-weight: 600; color: var(--color-text); }
.picked.locked { background: transparent; }
.picked.locked .lock-icon { margin-left: auto; font-size: 0.8rem; }

/* Locked org */
.issuer-locked { position: relative; }
.issuer-locked :deep(.p-inputtext) { width: 100%; padding-right: 1.9rem; background: var(--color-surface-2); cursor: default; }
.issuer-locked .pi-lock { position: absolute; right: 0.6rem; top: 50%; transform: translateY(-50%); color: var(--color-text-muted); font-size: 0.8rem; pointer-events: none; }

/* Registration context strip */
.reg-strip {
  display: flex; flex-wrap: wrap; gap: 0.4rem 1.25rem;
  margin: 0.2rem 0 0.7rem; padding: 0.45rem 0.65rem;
  border-radius: 8px; font-size: 0.75rem; color: var(--color-text-secondary);
  background: color-mix(in srgb, var(--color-brand-500) 7%, transparent);
  border: 1px solid color-mix(in srgb, var(--color-brand-500) 20%, transparent);
}
.reg-strip b { font-weight: 600; color: var(--color-text); }

.snapshot-note {
  display: flex; align-items: center; gap: 0.4rem;
  margin: 0.2rem 0 0.7rem; padding: 0.45rem 0.65rem;
  border-radius: 8px; font-size: 0.75rem; color: var(--color-text-secondary);
  background: color-mix(in srgb, #f59e0b 10%, transparent);
  border: 1px solid color-mix(in srgb, #f59e0b 28%, transparent);
}
.snapshot-note i { color: #d97706; }

/* Checkbox row */
.row.checks { display: flex; flex-wrap: wrap; gap: 0.4rem 1.5rem; margin-bottom: 0.45rem; }
.chk { display: flex; align-items: center; gap: 0.45rem; font-size: 0.8125rem; color: var(--color-text); cursor: pointer; }

.muted { color: var(--color-text-muted); }

/* Sticky footer */
.footer-actions {
  position: fixed;
  bottom: 0; left: var(--sidebar-w); right: 0;
  padding: 0.625rem 2rem;
  display: flex; justify-content: flex-end; gap: 0.5rem;
  background: color-mix(in srgb, var(--color-surface) 94%, transparent);
  backdrop-filter: blur(8px);
  border-top: 1px solid var(--color-border);
  z-index: 5;
}
</style>
