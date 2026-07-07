<script setup lang="ts">
import { computed, onMounted, ref, watch } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import { openPrintTab } from '@/utils/print';
import { useAuthStore } from '@/stores/auth';
import type {
  Client, ClientPersonalData, DocumentIssuer, Paged,
  TechExamOrgLookup, IdlCategoryOption, IdlFull, IdlWrite, IdlApplicant,
} from '@/types';
import Button from 'primevue/button';
import AutoComplete from 'primevue/autocomplete';
import InputText from 'primevue/inputtext';
import Textarea from 'primevue/textarea';
import Select from 'primevue/select';
import Checkbox from 'primevue/checkbox';
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
const categories = ref<IdlCategoryOption[]>([]);
const documentIssuers = ref<DocumentIssuer[]>([]);
const documentIssuerNames = computed(() => documentIssuers.value.map(x => x.name).filter(Boolean));

// ---- Header state ----
const issuerOrganizationId = ref<number | null>(null);
const numberOfLicence = ref('');
const numberOfNationalLicence = ref('');
const issuedDate = ref<Date | null>(new Date());
const validTillDate = ref<Date | null>(plusYears(new Date(), 3));
const note = ref('');

function plusYears(d: Date, n: number): Date {
  const r = new Date(d);
  r.setFullYear(r.getFullYear() + n);
  return r;
}

let suppressAutoValidTill = false;

// ---- Anchor: client (direct — no vehicle relation) — PrimeVue AutoComplete ----
type ClientOpt = { id: number; name: string; mb: string | null; label: string };
const selectedClient = ref<Client | null>(null);
const clientSel = ref<ClientOpt | string | null>(null);
const clientSuggestions = ref<ClientOpt[]>([]);
const clientRaw = new Map<number, Client>();
function toClientOpt(c: Client): ClientOpt {
  const name = clientDisplayName(c);
  return { id: c.id, name, mb: c.mb, label: c.mb ? `${name} · ЕМБГ ${c.mb}` : name };
}

// ---- Applicant snapshot (editable, printed; NOT written back to the client) ----
interface ApplicantForm {
  firstName: string; lastName: string; parentName: string; citizenship: string;
  dateOfBirth: Date | null; birthPlace: string; address: string;
  passportNumber: string; passportIssuer: string; passportDate: Date | null; passportExpiry: Date | null;
  idCardNumber: string; idCardIssuer: string; idCardDate: Date | null; idCardExpiry: Date | null;
  nationalLicenceIssuer: string; nationalLicenceDate: Date | null; nationalLicenceExpiry: Date | null;
}
function blankApplicant(): ApplicantForm {
  return {
    firstName: '', lastName: '', parentName: '', citizenship: '',
    // Место на раѓање defaults to Велес (the station's town) — editable per applicant.
    dateOfBirth: null, birthPlace: 'Велес', address: '',
    passportNumber: '', passportIssuer: '', passportDate: null, passportExpiry: null,
    idCardNumber: '', idCardIssuer: '', idCardDate: null, idCardExpiry: null,
    nationalLicenceIssuer: '', nationalLicenceDate: null, nationalLicenceExpiry: null,
  };
}
const applicant = ref<ApplicantForm>(blankApplicant());

// Датум на важност follows Датум на издавање + 3 години (IDP validity), но никогаш подоцна
// од датумот на важност на националната возачка — меѓународната не може да ја надживее.
// Auto-recomputes when the issue date changes; suppressed while loading an existing licence
// so the stored value isn't clobbered. (Declared after `applicant` — the getters read it.)
function capToNationalExpiry(d: Date | null): Date | null {
  const cap = applicant.value.nationalLicenceExpiry;
  if (d && cap && d > cap) return new Date(cap);
  return d;
}
watch(issuedDate, (d) => {
  if (suppressAutoValidTill) return;
  validTillDate.value = capToNationalExpiry(d ? plusYears(d, 3) : null);
}, { flush: 'sync' });
// When the national licence expiry arrives (auto-fill) or changes, pull the international
// validity down if it currently exceeds it.
watch(() => applicant.value.nationalLicenceExpiry, (cap) => {
  if (cap && validTillDate.value && validTillDate.value > cap) validTillDate.value = new Date(cap);
});

// ---- Categories (checkbox grid) ----
const checkedCategoryIds = ref<Set<number>>(new Set());

const orgOptions = computed(() =>
  organizations.value.map(o => ({ id: o.id, label: o.name || o.code || `#${o.id}` })));

// An operator's issuer is ALWAYS their own station — lock it (no picker). Only admins (no
// company) choose. Falls back to the picker if an operator's station can't be resolved.
const issuerLocked = computed(() => auth.companyId != null && issuerOrganizationId.value != null);
const issuerName = computed(() => orgOptions.value.find(o => o.id === issuerOrganizationId.value)?.label ?? '—');

async function loadCatalogs() {
  const [og, cat, di] = await Promise.all([
    api.get<TechExamOrgLookup[]>('/technical-exams/organizations'),
    api.get<IdlCategoryOption[]>('/international-driving-licences/categories'),
    api.get<DocumentIssuer[]>('/document-issuers'),
  ]);
  organizations.value = og.data;
  categories.value = cat.data;
  documentIssuers.value = di.data;
}

function toDate(s: string | null | undefined): Date | null {
  return s ? new Date(s) : null;
}
function toIsoDate(d: Date | null): string | null {
  if (!d) return null;
  const y = d.getFullYear();
  const m = String(d.getMonth() + 1).padStart(2, '0');
  const day = String(d.getDate()).padStart(2, '0');
  return `${y}-${m}-${day}`;
}

async function loadIdl() {
  if (!props.id) return;
  loading.value = true;
  try {
    const { data: r } = await api.get<IdlFull>(`/international-driving-licences/${props.id}`);
    issuerOrganizationId.value = r.issuerOrganizationId;
    numberOfLicence.value = r.numberOfLicence;
    numberOfNationalLicence.value = r.numberOfNationalLicence;
    suppressAutoValidTill = true;
    issuedDate.value = toDate(r.issuedDate);       // sync watch skipped — keep the stored valid-till
    suppressAutoValidTill = false;
    validTillDate.value = toDate(r.validTillDate);
    note.value = r.note ?? '';
    checkedCategoryIds.value = new Set(r.categoryIds);

    const a = r.applicant;
    applicant.value = {
      firstName: a?.firstName ?? '', lastName: a?.lastName ?? '', parentName: a?.parentName ?? '',
      citizenship: a?.citizenship ?? '', dateOfBirth: toDate(a?.dateOfBirth), birthPlace: a?.birthPlace ?? 'Велес',
      address: a?.address ?? '',
      passportNumber: a?.passportNumber ?? '', passportIssuer: a?.passportIssuer ?? '', passportDate: toDate(a?.passportDate), passportExpiry: toDate(a?.passportExpiry),
      idCardNumber: a?.idCardNumber ?? '', idCardIssuer: a?.idCardIssuer ?? '', idCardDate: toDate(a?.idCardDate), idCardExpiry: toDate(a?.idCardExpiry),
      nationalLicenceIssuer: a?.nationalLicenceIssuer ?? '', nationalLicenceDate: toDate(a?.nationalLicenceDate),
      nationalLicenceExpiry: toDate(a?.nationalLicenceExpiry),
    };

    const { data: c } = await api.get<Client>(`/clients/${r.clientId}`);
    selectedClient.value = c;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('internationalDrivingLicences.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}

// ---- Anchor search (AutoComplete: panel teleports to body — no card-overflow clipping) ----
async function onClientComplete(e: { query: string }) {
  const term = (e.query || '').trim();
  if (term.length < 2) { clientSuggestions.value = []; return; }
  try {
    const { data } = await api.get<Paged<Client>>('/clients', { params: { q: term, pageSize: 20 } });
    clientRaw.clear();
    data.items.forEach(c => clientRaw.set(c.id, c));
    clientSuggestions.value = data.items.map(toClientOpt);
  } catch { clientSuggestions.value = []; }
}
async function onClientSelect(e: { value: ClientOpt }) {
  const c = clientRaw.get(e.value.id);
  if (c) await chooseClient(c);
}
async function chooseClient(c: Client) {
  selectedClient.value = c;
  clientSel.value = toClientOpt(c);
  clientSuggestions.value = [];

  // Auto-fill the editable applicant snapshot from the picked client (name, citizenship,
  // DOB, address, and the 3 on-file documents). The operator can then edit everything for
  // the print — none of it is written back to the client.
  try {
    const { data: a } = await api.get<IdlApplicant>(`/international-driving-licences/applicant-defaults/${c.id}`);
    applicant.value = {
      firstName: a.firstName ?? '', lastName: a.lastName ?? '', parentName: a.parentName ?? '',
      citizenship: a.citizenship ?? '', dateOfBirth: toDate(a.dateOfBirth), birthPlace: a.birthPlace ?? 'Велес',
      address: a.address ?? '',
      passportNumber: a.passportNumber ?? '', passportIssuer: a.passportIssuer ?? '', passportDate: toDate(a.passportDate), passportExpiry: toDate(a.passportExpiry),
      idCardNumber: a.idCardNumber ?? '', idCardIssuer: a.idCardIssuer ?? '', idCardDate: toDate(a.idCardDate), idCardExpiry: toDate(a.idCardExpiry),
      nationalLicenceIssuer: a.nationalLicenceIssuer ?? '', nationalLicenceDate: toDate(a.nationalLicenceDate),
      nationalLicenceExpiry: toDate(a.nationalLicenceExpiry),
    };
  } catch { /* best-effort — operator can fill the fields manually */ }

  // Also seed the header's national-licence NUMBER (separate field) if not already typed.
  if (!numberOfNationalLicence.value.trim()) {
    try {
      const { data } = await api.get<import('@/types').ClientPersonalData[]>(`/client-personal-data/by-client/${c.id}`);
      const licence = data.find(d => d.personalDataTypeId === 1 && d.active);
      if (licence) numberOfNationalLicence.value = licence.number;
    } catch { /* best-effort */ }
  }
}
function clearClient() {
  if (isEdit.value) return;
  selectedClient.value = null;
  clientSel.value = null;
  clientSuggestions.value = [];
  applicant.value = blankApplicant();
}
function clientDisplayName(c: Client): string {
  return [c.firstName, c.middleName, c.lastName].filter(Boolean).join(' ').trim() || `#${c.id}`;
}

function toggleCategory(id: number) {
  if (checkedCategoryIds.value.has(id)) checkedCategoryIds.value.delete(id);
  else checkedCategoryIds.value.add(id);
  // Re-assign to trigger reactivity (Set mutation isn't tracked by Vue's ref).
  checkedCategoryIds.value = new Set(checkedCategoryIds.value);
}

// ---- Save / delete ----
function validate(): string | null {
  if (!selectedClient.value) return t('internationalDrivingLicences.form.clientRequired');
  if (!issuerOrganizationId.value) return t('internationalDrivingLicences.form.issuerRequired');
  if (!numberOfLicence.value.trim() || !numberOfNationalLicence.value.trim())
    return t('internationalDrivingLicences.form.numberRequired');
  if (!issuedDate.value || !validTillDate.value) return t('internationalDrivingLicences.form.numberRequired');
  if (issuedDate.value > validTillDate.value) return t('internationalDrivingLicences.form.dateOrder');
  if (checkedCategoryIds.value.size === 0) return t('internationalDrivingLicences.form.categoriesRequired');
  // Лични податоци: сè задолжително освен Име на родител.
  const a = applicant.value;
  if (!a.firstName.trim() || !a.lastName.trim() || !a.citizenship.trim()
      || !a.dateOfBirth || !a.birthPlace.trim() || !a.address.trim())
    return t('internationalDrivingLicences.form.applicantRequired');
  // Документи: лична карта + национална возачка се задолжителни (пасошот не е — не е во
  // официјалниот ПРИЛОГ кон барањето).
  if (!a.idCardNumber.trim() || !a.idCardDate || !a.idCardIssuer.trim())
    return t('internationalDrivingLicences.form.idCardRequired');
  if (!a.nationalLicenceDate || !a.nationalLicenceExpiry || !a.nationalLicenceIssuer.trim())
    return t('internationalDrivingLicences.form.licenceDocRequired');
  // Меѓународната не смее да важи подолго од националната возачка (ни при рачна промена).
  if (a.nationalLicenceExpiry && validTillDate.value && validTillDate.value > a.nationalLicenceExpiry)
    return t('internationalDrivingLicences.form.validTillBeyondLicence');
  return null;
}

function buildBody(): IdlWrite {
  return {
    clientId: selectedClient.value!.id,
    issuerOrganizationId: issuerOrganizationId.value!,
    numberOfLicence: numberOfLicence.value.trim(),
    numberOfNationalLicence: numberOfNationalLicence.value.trim(),
    issuedDate: toIsoDate(issuedDate.value)!,
    validTillDate: toIsoDate(validTillDate.value)!,
    note: note.value.trim() || null,
    categoryIds: Array.from(checkedCategoryIds.value),
    applicant: applicantToDto(),
  };
}

function applicantToDto(): IdlApplicant {
  const a = applicant.value;
  const s = (v: string) => v.trim() || null;
  return {
    firstName: s(a.firstName), lastName: s(a.lastName), parentName: s(a.parentName),
    citizenship: s(a.citizenship), dateOfBirth: toIsoDate(a.dateOfBirth), birthPlace: s(a.birthPlace),
    address: s(a.address),
    passportNumber: s(a.passportNumber), passportIssuer: s(a.passportIssuer), passportDate: toIsoDate(a.passportDate), passportExpiry: toIsoDate(a.passportExpiry),
    idCardNumber: s(a.idCardNumber), idCardIssuer: s(a.idCardIssuer), idCardDate: toIsoDate(a.idCardDate), idCardExpiry: toIsoDate(a.idCardExpiry),
    nationalLicenceIssuer: s(a.nationalLicenceIssuer), nationalLicenceDate: toIsoDate(a.nationalLicenceDate),
    nationalLicenceExpiry: toIsoDate(a.nationalLicenceExpiry),
  };
}

// ---- Зачувај ги документите кај сопственикот (ClientPersonalData) ----
// Explicit write-back: unlike the print snapshot, this DOES update the client's on-file
// documents (number, date issued, важи до, издавач) — create-or-update per document type.
const docsSaving = ref(false);
function issuerIdByName(name: string): number | null {
  const n = name.trim().toLowerCase();
  if (!n) return null;
  return documentIssuers.value.find(x => (x.name ?? '').trim().toLowerCase() === n)?.id ?? null;
}
async function saveDocsToClient() {
  const c = selectedClient.value;
  if (!c) return;
  docsSaving.value = true;
  try {
    const { data: rows } = await api.get<ClientPersonalData[]>(`/client-personal-data/by-client/${c.id}`);
    const a = applicant.value;
    const items = [
      { typeId: 3, labelKey: 'docIdCard', number: a.idCardNumber, date: a.idCardDate, expiry: a.idCardExpiry, issuer: a.idCardIssuer },
      { typeId: 2, labelKey: 'docPassport', number: a.passportNumber, date: a.passportDate, expiry: a.passportExpiry, issuer: a.passportIssuer },
      { typeId: 1, labelKey: 'docLicence', number: numberOfNationalLicence.value, date: a.nationalLicenceDate, expiry: a.nationalLicenceExpiry, issuer: a.nationalLicenceIssuer },
    ];
    let saved = 0;
    const skipped: string[] = [];
    for (const it of items) {
      const num = (it.number ?? '').trim();
      if (!num) continue; // документот не е внесен — прескокни
      const issuerId = issuerIdByName(it.issuer);
      if (issuerId == null) { skipped.push(t(`internationalDrivingLicences.applicant.${it.labelKey}`)); continue; }
      const body = {
        clientId: c.id, personalDataTypeId: it.typeId, documentIssuerId: issuerId,
        number: num, createdAt: toIsoDate(it.date), expiresAt: toIsoDate(it.expiry), active: true,
      };
      const existing = rows.find(r => r.personalDataTypeId === it.typeId && r.active);
      if (existing) await api.put(`/client-personal-data/${existing.id}`, body);
      else await api.post('/client-personal-data', body);
      saved++;
    }
    if (saved > 0)
      toast.add({ severity: 'success', summary: t('internationalDrivingLicences.applicant.docsSaved', { n: saved }), life: 2500 });
    if (skipped.length > 0)
      toast.add({ severity: 'warn', summary: t('internationalDrivingLicences.applicant.docsSkipped', { docs: skipped.join(', ') }), life: 5000 });
    if (saved === 0 && skipped.length === 0)
      toast.add({ severity: 'info', summary: t('internationalDrivingLicences.applicant.docsNothing'), life: 3000 });
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('internationalDrivingLicences.form.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    docsSaving.value = false;
  }
}

function openPrints(id: number) {
  openPrintTab(router.resolve({ name: 'idl-print', params: { id } }).href);
  openPrintTab(router.resolve({ name: 'idl-permit', params: { id } }).href);
}

/** Legacy "Сними и Печати" — one action: save, then print both documents. */
async function savePrint() {
  const err = validate();
  if (err) {
    toast.add({ severity: 'warn', summary: t('internationalDrivingLicences.form.saveFailed'), detail: err, life: 4000 });
    return;
  }
  saving.value = true;
  try {
    if (isEdit.value) {
      await api.put(`/international-driving-licences/${props.id}`, buildBody());
      toast.add({ severity: 'success', summary: t('internationalDrivingLicences.form.saved'), life: 1500 });
      openPrints(Number(props.id));
    } else {
      const { data } = await api.post<IdlFull>('/international-driving-licences', buildBody());
      toast.add({ severity: 'success', summary: t('internationalDrivingLicences.form.created'), life: 1500 });
      openPrints(data.id);
      router.replace(`/international-driving-licences/${data.id}`);
    }
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('internationalDrivingLicences.form.saveFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
  } finally {
    saving.value = false;
  }
}

function remove() {
  if (!isEdit.value) return;
  confirm.require({
    message: t('internationalDrivingLicences.deleteConfirm'),
    header: t('common.confirmDelete'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('common.delete'), severity: 'danger' },
    accept: async () => {
      try {
        await api.delete(`/international-driving-licences/${props.id}`);
        toast.add({ severity: 'success', summary: t('internationalDrivingLicences.deleted'), life: 1500 });
        router.push('/international-driving-licences');
      } catch (e: any) {
        toast.add({ severity: 'error', summary: t('common.deleteFailed'), detail: e?.response?.data?.error ?? e?.message, life: 5000 });
      }
    },
  });
}

onMounted(async () => {
  await loadCatalogs();
  if (isEdit.value) {
    await loadIdl();
  } else if (auth.companyId != null) {
    // Default Издавач to the operator's own station — the tech-exam org whose CompanyId matches
    // the logged-in operator (e.g. an МНС ДООЕЛ Велес operator → that org). Admins (no company)
    // pick manually. Still editable if a different issuer is ever needed.
    const own = organizations.value.find(o => o.companyId === auth.companyId);
    if (own) issuerOrganizationId.value = own.id;
  }
});
</script>

<template>
  <div class="idl-form">
    <div class="page-header">
      <div>
        <h1>
          <span v-if="isEdit">{{ t('internationalDrivingLicences.form.editTitle', { number: numberOfLicence }) }}</span>
          <span v-else>{{ t('internationalDrivingLicences.form.newTitle') }}</span>
        </h1>
        <div v-if="selectedClient" class="subtitle muted">
          {{ clientDisplayName(selectedClient) }}<span v-if="selectedClient.mb"> · ЕМБГ {{ selectedClient.mb }}</span>
        </div>
      </div>
    </div>

    <div v-if="loading" class="empty">
      <i class="pi pi-spin pi-spinner" />
      {{ t('common.loading') }}…
    </div>

    <template v-else>
      <!-- ===== 1. Owner (Сопственик) — pick, then editable print details ===== -->
      <div class="card">
        <div class="card-header">{{ t('internationalDrivingLicences.form.sections.anchor') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field picker-field">
              <label>{{ t('internationalDrivingLicences.client') }} *</label>
              <!-- create: AutoComplete (teleported panel — matches the Барања / Технички форми) -->
              <div v-if="!isEdit" class="picker-row">
                <AutoComplete
                  v-model="clientSel"
                  :suggestions="clientSuggestions"
                  optionLabel="label"
                  :placeholder="t('internationalDrivingLicences.form.pickClient')"
                  class="picker-ac"
                  @complete="onClientComplete"
                  @item-select="onClientSelect">
                  <template #option="{ option }">
                    <div class="ac-opt">
                      <span class="ac-main"><strong>{{ option.name }}</strong></span>
                      <span v-if="option.mb" class="ac-sub">ЕМБГ {{ option.mb }}</span>
                    </div>
                  </template>
                </AutoComplete>
                <Button v-if="selectedClient" icon="pi pi-times" severity="secondary" outlined size="small"
                        class="picker-clear" @click="clearClient" />
              </div>
              <!-- edit: locked chip -->
              <div v-else class="picked locked">
                <i class="pi pi-user" />
                <span v-if="selectedClient" class="picked-name">{{ clientDisplayName(selectedClient) }}</span>
                <span v-else class="muted">—</span>
                <i class="pi pi-lock muted lock-icon" />
              </div>
            </div>
          </div>

          <!-- Editable applicant snapshot (printed; not saved back to the client) -->
          <template v-if="selectedClient || isEdit">
            <p class="autofill-hint"><i class="pi pi-info-circle" /> {{ t('internationalDrivingLicences.applicant.autofillHint') }}</p>

            <div class="row">
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.firstName') }} *</label>
                <InputText v-model="applicant.firstName" maxlength="100" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.lastName') }} *</label>
                <InputText v-model="applicant.lastName" maxlength="100" />
              </div>
            </div>
            <div class="row">
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.parentName') }}</label>
                <InputText v-model="applicant.parentName" maxlength="100" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.citizenship') }} *</label>
                <InputText v-model="applicant.citizenship" maxlength="100" />
              </div>
            </div>
            <div class="row">
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.dateOfBirth') }} *</label>
                <DatePicker v-model="applicant.dateOfBirth" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.birthPlace') }} *</label>
                <InputText v-model="applicant.birthPlace" maxlength="150" />
              </div>
            </div>
            <div class="row">
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.address') }} *</label>
                <InputText v-model="applicant.address" maxlength="200" />
              </div>
            </div>
          </template>
        </div>
      </div>

      <!-- ===== 2. Owner's documents (Документи) — 3 sub-blocks like the client form ===== -->
      <div v-if="selectedClient || isEdit" class="card">
        <div class="card-header">
          <span>{{ t('internationalDrivingLicences.applicant.documentsSection') }}</span>
          <Button
            :label="t('internationalDrivingLicences.applicant.saveDocs')"
            icon="pi pi-save" size="small" severity="secondary" outlined
            :loading="docsSaving" :disabled="!selectedClient"
            v-tooltip.left="t('internationalDrivingLicences.applicant.saveDocsHint')"
            @click="saveDocsToClient" />
        </div>
        <div class="card-body">
          <div class="docgrid">
            <div class="docblock">
              <h3>{{ t('internationalDrivingLicences.applicant.docIdCard') }}</h3>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColNumber') }} *</label>
                <InputText v-model="applicant.idCardNumber" maxlength="100" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColDate') }} *</label>
                <DatePicker v-model="applicant.idCardDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColExpiry') }}</label>
                <DatePicker v-model="applicant.idCardExpiry" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColIssuer') }} *</label>
                <Select v-model="applicant.idCardIssuer" :options="documentIssuerNames" editable placeholder="—" />
              </div>
            </div>
            <div class="docblock">
              <h3>{{ t('internationalDrivingLicences.applicant.docPassport') }}</h3>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColNumber') }}</label>
                <InputText v-model="applicant.passportNumber" maxlength="100" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColDate') }}</label>
                <DatePicker v-model="applicant.passportDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColExpiry') }}</label>
                <DatePicker v-model="applicant.passportExpiry" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColIssuer') }}</label>
                <Select v-model="applicant.passportIssuer" :options="documentIssuerNames" editable placeholder="—" />
              </div>
            </div>
            <div class="docblock">
              <h3>{{ t('internationalDrivingLicences.applicant.docLicence') }}</h3>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColNumber') }} *</label>
                <InputText v-model="numberOfNationalLicence" maxlength="50" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColDate') }} *</label>
                <DatePicker v-model="applicant.nationalLicenceDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColExpiry') }} *</label>
                <DatePicker v-model="applicant.nationalLicenceExpiry" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
              </div>
              <div class="field">
                <label>{{ t('internationalDrivingLicences.applicant.docColIssuer') }} *</label>
                <Select v-model="applicant.nationalLicenceIssuer" :options="documentIssuerNames" editable placeholder="—" />
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- ===== 3. Licence data (Основни податоци) ===== -->
      <div class="card">
        <div class="card-header">{{ t('internationalDrivingLicences.form.sections.header') }}</div>
        <div class="card-body">
          <div class="row">
            <div class="field" style="max-width: 480px">
              <label>{{ t('internationalDrivingLicences.issuer') }} *</label>
              <!-- Operator: locked to their own station (read-only input, aligns with the fields below). Admin: free pick. -->
              <div v-if="issuerLocked" class="issuer-locked">
                <InputText :modelValue="issuerName" readonly />
                <i class="pi pi-lock" />
              </div>
              <Select v-else v-model="issuerOrganizationId" :options="orgOptions" optionLabel="label" optionValue="id"
                      filter :placeholder="t('internationalDrivingLicences.form.pickIssuer')" />
            </div>
          </div>
          <!-- Бр. на национална дозвола се внесува во Документи → Национална возачка (нема дупликат тука). -->
          <div class="row">
            <div class="field">
              <label>{{ t('internationalDrivingLicences.numberOfLicence') }} *</label>
              <InputText v-model="numberOfLicence" maxlength="50" />
            </div>
            <div class="field">
              <label>{{ t('internationalDrivingLicences.issuedDate') }} *</label>
              <DatePicker v-model="issuedDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
            </div>
            <div class="field">
              <label>{{ t('internationalDrivingLicences.validTillDate') }} *</label>
              <DatePicker v-model="validTillDate" dateFormat="yy-mm-dd" showIcon iconDisplay="input" />
            </div>
          </div>
          <div class="field">
            <label>{{ t('internationalDrivingLicences.note') }}</label>
            <Textarea v-model="note" rows="2" autoResize maxlength="250" />
          </div>
        </div>
      </div>

      <!-- ===== 4. Categories ===== -->
      <div class="card">
        <div class="card-header">{{ t('internationalDrivingLicences.form.sections.categories') }}</div>
        <div class="card-body">
          <div class="category-grid">
            <label
              v-for="c in categories" :key="c.id"
              class="category-item" :class="{ checked: checkedCategoryIds.has(c.id) }"
              v-tooltip.top="c.description || undefined"
            >
              <Checkbox :modelValue="checkedCategoryIds.has(c.id)" :binary="true" @update:modelValue="toggleCategory(c.id)" />
              <span class="category-code">{{ c.code }}</span>
              <i v-if="c.description" class="pi pi-info-circle cat-info" />
            </label>
          </div>
        </div>
      </div>

      <div class="footer-actions">
        <Button :label="t('common.back')" severity="secondary" outlined icon="pi pi-arrow-left" size="small"
                @click="router.push('/international-driving-licences')" />
        <Button v-if="isEdit" :label="t('common.delete')" severity="danger" outlined icon="pi pi-trash" size="small" @click="remove" />
        <Button :label="t('internationalDrivingLicences.form.savePrint')" icon="pi pi-print" size="small" :loading="saving" @click="savePrint" />
      </div>
    </template>
  </div>
</template>

<style scoped>
/* House multi-card layout (mirrors ClientFormView): capped width, dense spacing, sticky save bar. */
.idl-form { max-width: 1200px; margin: 0 auto; padding-bottom: 3.5rem; }

.idl-form :deep(.page-header)            { margin-bottom: 0.75rem; }
.idl-form :deep(.page-header h1)         { font-size: 1.125rem; letter-spacing: -0.01em; }
.idl-form .page-header .subtitle         { font-size: 0.75rem; margin-top: 0.1rem; }

.idl-form .card + .card                  { margin-top: 0.5rem; }
.idl-form :deep(.card .card-header)      { padding: 0.45rem 0.875rem; font-size: 0.8125rem; font-weight: 600; }
.idl-form :deep(.card .card-body)        { padding: 0.625rem 0.875rem; }

.idl-form :deep(.row)                     { gap: 0.625rem; }
.idl-form :deep(.field)                   { gap: 0.15rem; margin-bottom: 0.45rem; }
.idl-form :deep(.field label)             { font-size: 0.75rem; font-weight: 500; color: var(--color-text-secondary); }

/* Inputs — compact, full-width */
.idl-form :deep(.p-select),
.idl-form :deep(.p-datepicker),
.idl-form :deep(.p-inputtext)             { width: 100%; }
.idl-form :deep(.p-inputtext)             { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.idl-form :deep(.p-textarea)              { padding: 0.3rem 0.5rem; font-size: 0.8125rem; min-height: auto; }
.idl-form :deep(.p-select)                { font-size: 0.8125rem; }
.idl-form :deep(.p-select-label)          { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.idl-form :deep(.p-select-dropdown)       { width: 1.625rem; }
.idl-form :deep(.p-datepicker-input)      { padding: 0.3rem 0.5rem; font-size: 0.8125rem; }
.idl-form :deep(.p-checkbox)              { transform: scale(0.85); transform-origin: left center; }
.idl-form :deep(.p-button)                { font-size: 0.8125rem; }
.idl-form :deep(.p-button.p-button-sm)    { padding: 0.3rem 0.625rem; }

/* Brand accent stripe on each card header */
.idl-form :deep(.card .card-header) {
  position: relative;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.idl-form :deep(.card .card-header)::before {
  content: '';
  position: absolute;
  left: 0; top: 0; bottom: 0;
  width: 3px;
  background: var(--color-brand-500);
  border-radius: 0 2px 2px 0;
}
.idl-form :deep(.card) { transition: box-shadow 0.15s ease, border-color 0.15s ease; }
.idl-form :deep(.card:focus-within) {
  box-shadow: 0 1px 2px rgba(0,0,0,0.04), 0 4px 12px rgba(0,0,0,0.06);
  border-color: color-mix(in srgb, var(--color-border) 70%, var(--color-brand-500));
}
.idl-form :deep(.p-inputtext:focus),
.idl-form :deep(.p-textarea:focus),
.idl-form :deep(.p-select:not(.p-disabled).p-focus),
.idl-form :deep(.p-datepicker-input:focus) {
  outline: none;
  border-color: var(--color-brand-500);
  box-shadow: 0 0 0 2px color-mix(in srgb, var(--color-brand-500) 25%, transparent);
}

/* Owner picker — AutoComplete (panel teleports to body, so it never clips inside the card) */
.picker-field { max-width: 520px; }
.picker-row { display: flex; align-items: center; gap: 0.4rem; }
.picker-ac { flex: 1 1 auto; }
.picker-ac :deep(.p-autocomplete) { width: 100%; }
.picker-ac :deep(.p-autocomplete-input) { width: 100%; }
.picker-clear { flex: 0 0 auto; }
/* option rows inside the overlay (carry this component's data-v attr, so scoped CSS reaches them) */
.ac-opt { display: flex; flex-direction: column; line-height: 1.25; }
.ac-main { font-size: 0.85rem; color: var(--color-text); }
.ac-sub { font-size: 0.75rem; color: var(--color-text-muted); }
/* edit-mode locked chip */
.picked {
  display: flex; align-items: center; gap: 0.5rem;
  background: var(--color-surface-2); border: 1px solid var(--color-border);
  padding: 0.4rem 0.6rem; border-radius: 8px;
}
.picked > .pi-user { color: var(--color-brand-500); }
.picked-name { font-weight: 600; color: var(--color-text); }
.picked.locked { background: transparent; }
.picked.locked .lock-icon { margin-left: auto; font-size: 0.8rem; }

/* Locked issuer (operator) — a read-only input so it aligns with the fields below (not a bold chip) */
.issuer-locked { position: relative; }
.issuer-locked :deep(.p-inputtext) { width: 100%; padding-right: 1.9rem; background: var(--color-surface-2); cursor: default; }
.issuer-locked .pi-lock { position: absolute; right: 0.6rem; top: 50%; transform: translateY(-50%); color: var(--color-text-muted); font-size: 0.8rem; pointer-events: none; }

/* Auto-fill hint banner */
.autofill-hint {
  display: flex; align-items: center; gap: 0.45rem;
  margin: 0.2rem 0 0.7rem; padding: 0.45rem 0.65rem;
  border-radius: 8px; font-size: 0.75rem; color: var(--color-text-secondary);
  background: color-mix(in srgb, var(--color-brand-500) 7%, transparent);
  border: 1px solid color-mix(in srgb, var(--color-brand-500) 20%, transparent);
}
.autofill-hint .pi { color: var(--color-brand-500); }

/* Documents sub-grid (mirrors the client form's docgrid/docblock) */
.docgrid { display: grid; grid-template-columns: repeat(auto-fit, minmax(240px, 1fr)); gap: 0.5rem 0.875rem; }
.docblock h3 {
  font-size: 0.75rem; color: var(--color-text);
  margin: 0 0 0.4rem; padding-bottom: 0.25rem;
  border-bottom: 1px solid var(--color-border); font-weight: 600;
}
.docblock .field { margin-bottom: 0.4rem; }

/* Categories — uniform compact cells; full description on hover (tooltip) */
.category-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(120px, 1fr)); gap: 0.4rem 0.5rem; }
.category-item {
  display: flex; align-items: center; gap: 0.5rem;
  border: 1px solid var(--color-border); border-radius: 6px;
  padding: 0.45rem 0.6rem; cursor: pointer; font-size: 0.8125rem;
  transition: background 0.12s ease, border-color 0.12s ease;
}
.category-item:hover {
  background: color-mix(in srgb, var(--color-brand-500) 6%, var(--color-surface));
  border-color: var(--color-border-strong);
}
.category-item.checked {
  background: color-mix(in srgb, var(--color-brand-500) 10%, var(--color-surface));
  border-color: var(--color-brand-500);
}
.category-code { font-family: monospace; font-weight: 700; min-width: 2rem; color: var(--color-text); }
.cat-info { margin-left: auto; color: var(--color-text-muted); font-size: 0.72rem; opacity: 0.65; }
.category-item:hover .cat-info { opacity: 1; color: var(--color-brand-500); }
.muted { color: var(--color-text-muted); }

/* Sticky footer save bar */
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
