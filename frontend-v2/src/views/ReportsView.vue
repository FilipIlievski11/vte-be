<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import Button from 'primevue/button';
import Select from 'primevue/select';
import SelectButton from 'primevue/selectbutton';
import InputNumber from 'primevue/inputnumber';
import DatePicker from 'primevue/datepicker';
import { useToast } from 'primevue/usetoast';

const { t } = useI18n();
const toast = useToast();

type Mode = 'distribution' | 'monthly' | 'preview';
const mode = ref<Mode>('distribution');
const modeOptions = computed(() => [
  { value: 'distribution', label: t('reports.mode.distribution') },
  { value: 'monthly', label: t('reports.mode.monthly') },
  { value: 'preview', label: t('reports.mode.preview') },
]);

function fmtMoney(v: number): string {
  return v.toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
}
function fmtDate(s: string): string {
  const d = new Date(s);
  return `${String(d.getDate()).padStart(2, '0')}.${String(d.getMonth() + 1).padStart(2, '0')}.${d.getFullYear()}`;
}
function isoDay(d: Date): string {
  return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
}

const loading = ref(false);
const exporting = ref(false);

// ===================== Дневна распределба =====================
interface InstitutionRow {
  calcId: number; name: string; bankAccount: string | null; bank: string | null; form: string | null;
  lineCount: number; total: number;
}
interface DistributionDto {
  from: string; to: string; companyName: string;
  institutions: InstitutionRow[]; institutionsTotal: number;
  stationOwnAccount: number; stationUnassigned: number; stationTotal: number;
  grandTotal: number; lineCount: number;
}
const today = new Date();
const distFrom = ref<Date>(new Date(today));
const distTo = ref<Date>(new Date(today));
const distribution = ref<DistributionDto | null>(null);

async function loadDistribution() {
  loading.value = true;
  try {
    const { data } = await api.get<DistributionDto>('/reports/institution-distribution', {
      params: { from: isoDay(distFrom.value), to: isoDay(distTo.value) },
    });
    distribution.value = data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.loadFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { loading.value = false; }
}

async function downloadDistributionXlsx() {
  exporting.value = true;
  try {
    const res = await api.get('/reports/institution-distribution/xlsx', {
      params: { from: isoDay(distFrom.value), to: isoDay(distTo.value) },
      responseType: 'blob',
    });
    const url = URL.createObjectURL(res.data as Blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Распределба ${isoDay(distFrom.value)}${isoDay(distFrom.value) !== isoDay(distTo.value) ? '–' + isoDay(distTo.value) : ''}.xlsx`;
    a.click();
    URL.revokeObjectURL(url);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.exportFailed'),
      detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { exporting.value = false; }
}

// ===================== Месечен извештај по категорија =====================
interface ReportRow { documentId: number; datePay: string; payer: string; vehicleCategory: string | null; plate: string | null; amount: number; }
interface ReportDto { year: number; month: number; from: string; to: string; companyName: string; total: number; rows: ReportRow[]; }
const prev = new Date(today.getFullYear(), today.getMonth() - 1, 1);
const year = ref(prev.getFullYear());
const month = ref(prev.getMonth() + 1);
const monthOptions = computed(() =>
  Array.from({ length: 12 }, (_, i) => ({ value: i + 1, label: t(`reports.months.${i + 1}`) })));
const report = ref<ReportDto | null>(null);

// Секоја институција може да наплаќа низ повеќе категории на наплата (ids од
// PaymentCategoryGroup; старофирмските id-а се вклучени за извештаи на стари месеци).
// Реп. совет = основен (1063/63) + „од јавни патишта" (1086/86) — иста уплатна сметка.
interface MonthlyCat { key: string; ids: string; label: string }
const monthlyCats = computed<MonthlyCat[]>(() => [
  { key: 'roads',       ids: '1',                label: t('reports.cats.roads') },
  { key: 'budget',      ids: '9',                label: t('reports.cats.budget') },
  { key: 'redcross',    ids: '7',                label: t('reports.cats.redcross') },
  { key: 'communal',    ids: '1002,2',           label: t('reports.cats.communal') },
  { key: 'environment', ids: '8',                label: t('reports.cats.environment') },
  { key: 'council',     ids: '1086,1063,86,63',  label: t('reports.cats.council') },
]);
const monthlyCatKey = ref('roads');
const selectedCat = computed(() => monthlyCats.value.find(c => c.key === monthlyCatKey.value) ?? monthlyCats.value[0]);
// Јавни патишта го задржува точниот легаси наслов (паритет со стариот XLS образец);
// останатите добиваат генеричен наслов со името на категоријата. На екранот насловот
// ја следи ВЧИТАНАТА категорија, не живата селекција.
function monthlyTitleFor(cat: MonthlyCat): string {
  return cat.key === 'roads' ? t('reports.docTitle') : t('reports.docTitleGeneric', { name: cat.label });
}
const monthlyDocTitle = computed(() => monthlyTitleFor(monthlyLoadedCat.value ?? selectedCat.value));

// Ист stale-guard + „вчитана категорија" како кај Прегледот — иста трка постои и тука.
const monthlyLoadedCat = ref<MonthlyCat | null>(null);
let monthlyReqSeq = 0;
async function loadMonthly() {
  const cat = selectedCat.value;
  const seq = ++monthlyReqSeq;
  loading.value = true;
  try {
    const { data } = await api.get<ReportDto>('/reports/public-roads', {
      params: { year: year.value, month: month.value, categoryGroupIds: cat.ids },
    });
    if (seq !== monthlyReqSeq) return;
    report.value = data;
    monthlyLoadedCat.value = cat;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { if (seq === monthlyReqSeq) loading.value = false; }
}
async function downloadMonthlyXlsx() {
  exporting.value = true;
  try {
    // xlsx-от секогаш повлекува свежи податоци за живата селекција — насловот оди во пар.
    const res = await api.get('/reports/public-roads/xlsx', {
      params: {
        year: year.value, month: month.value,
        categoryGroupIds: selectedCat.value.ids, title: monthlyTitleFor(selectedCat.value),
      },
      responseType: 'blob',
    });
    const url = URL.createObjectURL(res.data as Blob);
    const a = document.createElement('a');
    a.href = url;
    const catPart = monthlyCatKey.value === 'roads' ? '' : `${selectedCat.value.label} `;
    a.download = `Извештај ${catPart}${t(`reports.months.${month.value}`)} ${year.value}.xlsx`;
    a.click();
    URL.revokeObjectURL(url);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.exportFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { exporting.value = false; }
}

// ===================== Преглед за наплата (по вид на возило) =====================
// Легаси uxReportByPaymentCategory pivot: за период + институција — број на возила
// и наплатен износ по вид на возило. Реп. совет е ДВА одделни извештаи (основен и
// „од јавни патишта" — 1% од патниот надомест), како во легаси.
interface KindRow { kind: string; vehicles: number; amount: number }
interface CollectionPreviewDto {
  from: string; to: string; companyName: string; categoryName: string;
  totalVehicles: number; totalAmount: number; rows: KindRow[];
}
const prevFrom = ref<Date>(new Date(today.getFullYear(), today.getMonth(), 1));
const prevTo = ref<Date>(new Date(today));
const preview = ref<CollectionPreviewDto | null>(null);
const previewCats = computed<MonthlyCat[]>(() => [
  { key: 'roads',        ids: '1',         label: t('reports.cats.roads') },
  { key: 'budget',       ids: '9',         label: t('reports.cats.budget') },
  { key: 'redcross',     ids: '7',         label: t('reports.cats.redcross') },
  { key: 'communal',     ids: '1002,2',    label: t('reports.cats.communal') },
  { key: 'environment',  ids: '8',         label: t('reports.cats.environment') },
  { key: 'council',      ids: '1063,63',   label: t('reports.cats.council') },
  { key: 'councilRoads', ids: '1086,86',   label: t('reports.cats.councilRoads') },
]);
const previewCatKey = ref('redcross');
const selectedPreviewCat = computed(() =>
  previewCats.value.find(c => c.key === previewCatKey.value) ?? previewCats.value[0]);

// Насловот на листот се врзува за категоријата чии податоци се ВЧИТАНИ (не за живата
// селекција) — паѓање/задоцнет одговор инаку остава туѓ наслов врз туѓи бројки на печат.
const previewLoadedCat = ref<MonthlyCat | null>(null);
let previewReqSeq = 0;
async function loadPreview() {
  const cat = selectedPreviewCat.value;
  const seq = ++previewReqSeq;
  loading.value = true;
  try {
    const { data } = await api.get<CollectionPreviewDto>('/reports/collection-preview', {
      params: { from: isoDay(prevFrom.value), to: isoDay(prevTo.value), categoryGroupIds: cat.ids },
    });
    if (seq !== previewReqSeq) return;   // задоцнет одговор од претходна селекција — отфрли
    preview.value = data;
    previewLoadedCat.value = cat;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.loadFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { if (seq === previewReqSeq) loading.value = false; }
}
async function downloadPreviewXlsx() {
  exporting.value = true;
  try {
    const res = await api.get('/reports/collection-preview/xlsx', {
      params: {
        from: isoDay(prevFrom.value), to: isoDay(prevTo.value),
        categoryGroupIds: selectedPreviewCat.value.ids, title: selectedPreviewCat.value.label,
      },
      responseType: 'blob',
    });
    const url = URL.createObjectURL(res.data as Blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Преглед за наплата ${selectedPreviewCat.value.label} ${isoDay(prevFrom.value)}–${isoDay(prevTo.value)}.xlsx`;
    a.click();
    URL.revokeObjectURL(url);
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('reports.exportFailed'), detail: e?.response?.data?.error ?? e?.message, life: 4000 });
  } finally { exporting.value = false; }
}

// ===================== shared =====================
function load() {
  if (mode.value === 'distribution') loadDistribution();
  else if (mode.value === 'monthly') loadMonthly();
  else loadPreview();
}
function downloadXlsx() {
  if (mode.value === 'distribution') downloadDistributionXlsx();
  else if (mode.value === 'monthly') downloadMonthlyXlsx();
  else downloadPreviewXlsx();
}
function switchMode(m: Mode) { if (m === mode.value) return; mode.value = m; load(); }

// Printing: toggle a body-level class so a GLOBAL print rule can hide the app chrome.
function clearPrintMode() { document.body.classList.remove('printing-report'); }
function printReport() { document.body.classList.add('printing-report'); window.print(); }

onMounted(() => {
  loadDistribution();
  window.addEventListener('afterprint', clearPrintMode);
});
onUnmounted(() => {
  clearPrintMode();
  window.removeEventListener('afterprint', clearPrintMode);
});

const hasContent = computed(() =>
  mode.value === 'distribution' ? !!distribution.value
  : mode.value === 'monthly' ? !!report.value?.rows?.length
  : !!preview.value?.rows?.length);
</script>

<template>
  <div class="page reports-page">
    <!-- Стабилни две редици: горе наслов + табови, долу контроли + акции. Менувањето
         режим менува само содржина ВНАТРЕ во долната редица — ништо не скока. -->
    <div class="toolbar no-print">
      <div class="tb-row">
        <div class="tb-title">
          <div class="tb-icon"><i class="pi pi-chart-bar" /></div>
          <div>
            <h1>{{ t('reports.title') }}</h1>
            <p class="tb-sub">{{ t('reports.subtitle') }}</p>
          </div>
        </div>
        <SelectButton :modelValue="mode" :options="modeOptions" optionLabel="label" optionValue="value"
                      :allowEmpty="false" size="small" @update:modelValue="switchMode" />
      </div>

      <div class="tb-row">
        <div v-if="mode === 'distribution'" class="period-pick">
          <DatePicker v-model="distFrom" dateFormat="dd.mm.yy" size="small" class="ctl-date" showIcon iconDisplay="input" />
          <span class="dash">–</span>
          <DatePicker v-model="distTo" dateFormat="dd.mm.yy" size="small" class="ctl-date" showIcon iconDisplay="input" />
          <Button :label="t('reports.show')" icon="pi pi-search" size="small" :loading="loading" @click="loadDistribution" />
        </div>
        <div v-else-if="mode === 'monthly'" class="period-pick">
          <Select v-model="monthlyCatKey" :options="monthlyCats" optionLabel="label" optionValue="key"
                  size="small" class="ctl-cat" @change="loadMonthly" />
          <Select v-model="month" :options="monthOptions" optionLabel="label" optionValue="value" size="small" class="ctl-month" />
          <InputNumber v-model="year" :useGrouping="false" :min="2000" :max="2100" size="small" class="ctl-year" inputClass="year-input" />
          <Button :label="t('reports.show')" icon="pi pi-search" size="small" :loading="loading" @click="loadMonthly" />
        </div>
        <div v-else class="period-pick">
          <Select v-model="previewCatKey" :options="previewCats" optionLabel="label" optionValue="key"
                  size="small" class="ctl-cat" @change="loadPreview" />
          <DatePicker v-model="prevFrom" dateFormat="dd.mm.yy" size="small" class="ctl-date" showIcon iconDisplay="input" />
          <span class="dash">–</span>
          <DatePicker v-model="prevTo" dateFormat="dd.mm.yy" size="small" class="ctl-date" showIcon iconDisplay="input" />
          <Button :label="t('reports.show')" icon="pi pi-search" size="small" :loading="loading" @click="loadPreview" />
        </div>

        <div class="tb-actions">
          <Button :label="t('reports.print')" icon="pi pi-print" size="small" severity="secondary" outlined
                  :disabled="!hasContent" @click="printReport" />
          <Button :label="t('reports.excel')" icon="pi pi-file-excel" size="small" severity="success" outlined
                  :loading="exporting" :disabled="!hasContent" @click="downloadXlsx" />
        </div>
      </div>
    </div>

    <div v-if="loading" class="loading-state no-print"><i class="pi pi-spin pi-spinner" /></div>

    <!-- ===================== Дневна распределба ===================== -->
    <div v-else-if="mode === 'distribution' && distribution" class="report-sheet">
      <div class="report-head">
        <div class="rh-company">{{ distribution.companyName }}</div>
        <div class="rh-title">{{ t('reports.dist.docTitle') }}</div>
        <div class="rh-period">{{ distribution.from === distribution.to
          ? t('reports.dist.periodDay', { day: distribution.from })
          : t('reports.period', { from: distribution.from, to: distribution.to }) }}</div>
      </div>

      <div class="section-label">{{ t('reports.dist.toPay') }}</div>
      <table class="report-table">
        <thead>
          <tr>
            <th>{{ t('reports.dist.col.institution') }}</th>
            <th class="c-acc">{{ t('reports.dist.col.account') }}</th>
            <th class="c-form">{{ t('reports.dist.col.form') }}</th>
            <th class="c-amount">{{ t('reports.dist.col.amount') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="i in distribution.institutions" :key="i.calcId">
            <td>{{ i.name }}<span v-if="i.bank" class="bank-sub">{{ i.bank }}</span></td>
            <td class="c-acc mono">{{ i.bankAccount || '—' }}</td>
            <td class="c-form">{{ i.form || '' }}</td>
            <td class="c-amount mono">{{ fmtMoney(i.total) }}</td>
          </tr>
          <tr v-if="!distribution.institutions.length">
            <td colspan="4" class="empty-inline">{{ t('reports.dist.noInstitutions') }}</td>
          </tr>
        </tbody>
        <tfoot>
          <tr>
            <td colspan="3" class="total-label">{{ t('reports.dist.toPayTotal') }}</td>
            <td class="c-amount mono total-val">{{ fmtMoney(distribution.institutionsTotal) }}</td>
          </tr>
        </tfoot>
      </table>

      <div class="station-box">
        <div class="station-main">
          <span class="station-title">{{ t('reports.dist.stationKeeps') }}</span>
          <span class="station-amount mono">{{ fmtMoney(distribution.stationTotal) }} {{ t('reports.den') }}</span>
        </div>
        <div class="station-breakdown">
          <span>{{ t('reports.dist.ownAccount') }}: <b class="mono">{{ fmtMoney(distribution.stationOwnAccount) }}</b></span>
          <span v-if="distribution.stationUnassigned">
            {{ t('reports.dist.ownServices') }}: <b class="mono">{{ fmtMoney(distribution.stationUnassigned) }}</b>
          </span>
        </div>
      </div>

      <div class="grand-row">
        <span>{{ t('reports.dist.grandTotal') }}</span>
        <span class="mono"><b>{{ fmtMoney(distribution.grandTotal) }}</b> {{ t('reports.den') }}</span>
      </div>

      <div class="report-signature print-only">
        <div class="sig-mp">М.П.</div>
        <div class="sig-line"><div>____________________________</div><div>{{ t('reports.signature') }}</div></div>
      </div>
    </div>

    <!-- ===================== Месечно — Јавни патишта ===================== -->
    <div v-else-if="mode === 'monthly' && report" class="report-sheet">
      <div class="report-head">
        <div class="rh-company">{{ report.companyName }}</div>
        <div class="rh-title">{{ monthlyDocTitle }}</div>
        <div class="rh-period">{{ t('reports.period', { from: report.from, to: report.to }) }}</div>
      </div>

      <div v-if="report.rows.length" class="rh-meta no-print">
        {{ report.rows.length }} {{ t('reports.rowsWord') }} · {{ fmtMoney(report.total) }} {{ t('reports.den') }}
      </div>

      <table v-if="report.rows.length" class="report-table">
        <thead>
          <tr>
            <th class="c-n">{{ t('reports.col.n') }}</th>
            <th class="c-date">{{ t('reports.col.date') }}</th>
            <th>{{ t('reports.col.payer') }}</th>
            <th>{{ t('reports.col.category') }}</th>
            <th class="c-plate">{{ t('reports.col.plate') }}</th>
            <th class="c-amount">{{ t('reports.col.amount') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(r, i) in report.rows" :key="r.documentId + '-' + i">
            <td class="c-n">{{ i + 1 }}</td>
            <td class="c-date">{{ fmtDate(r.datePay) }}</td>
            <td>{{ r.payer }}</td>
            <td>{{ r.vehicleCategory || '—' }}</td>
            <td class="c-plate mono">{{ r.plate || '—' }}</td>
            <td class="c-amount mono">{{ fmtMoney(r.amount) }}</td>
          </tr>
        </tbody>
        <tfoot>
          <tr>
            <td colspan="5" class="total-label">{{ t('reports.totalLabel') }}</td>
            <td class="c-amount mono total-val">{{ fmtMoney(report.total) }} {{ t('reports.den') }}</td>
          </tr>
        </tfoot>
      </table>
      <div v-else class="empty-state"><i class="pi pi-inbox" /><span>{{ t('reports.empty') }}</span></div>

      <div v-if="report.rows.length" class="report-signature print-only">
        <div class="sig-mp">М.П.</div>
        <div class="sig-line"><div>____________________________</div><div>{{ t('reports.signature') }}</div></div>
      </div>
    </div>

    <!-- ===================== Преглед за наплата (по вид на возило) ===================== -->
    <div v-else-if="mode === 'preview' && preview" class="report-sheet">
      <div class="report-head">
        <div class="rh-company">{{ preview.companyName }}</div>
        <div class="rh-title">{{ t('reports.preview.docTitle', { name: (previewLoadedCat ?? selectedPreviewCat).label }) }}</div>
        <div class="rh-period">{{ t('reports.preview.period', { from: preview.from, to: preview.to }) }}</div>
      </div>

      <table v-if="preview.rows.length" class="report-table preview-table">
        <thead>
          <tr>
            <th>{{ t('reports.preview.col.kind') }}</th>
            <th class="c-veh">{{ t('reports.preview.col.vehicles') }}</th>
            <th class="c-amount">{{ t('reports.preview.col.amount') }}</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="r in preview.rows" :key="r.kind">
            <td>{{ r.kind }}</td>
            <td class="c-veh mono">{{ r.vehicles }}</td>
            <td class="c-amount mono">{{ fmtMoney(r.amount) }}</td>
          </tr>
        </tbody>
        <tfoot>
          <tr>
            <td class="total-label">{{ t('reports.totalLabel') }}</td>
            <td class="c-veh mono total-val">{{ preview.totalVehicles }}</td>
            <td class="c-amount mono total-val">{{ fmtMoney(preview.totalAmount) }} {{ t('reports.den') }}</td>
          </tr>
        </tfoot>
      </table>
      <div v-else class="empty-state"><i class="pi pi-inbox" /><span>{{ t('reports.empty') }}</span></div>

      <div v-if="preview.rows.length" class="report-signature print-only">
        <div class="sig-mp">М.П.</div>
        <div class="sig-line"><div>____________________________</div><div>{{ t('reports.signature') }}</div></div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.reports-page { display: flex; flex-direction: column; gap: 1rem }

/* --- Toolbar --- */
.toolbar {
  display: flex; flex-direction: column; gap: .6rem;
  /* глобалниот .toolbar има align-items:center (за ред-распоред) — тука колоната
     мора да ги растегне редиците на полна ширина за да не „шетаат" контролите */
  align-items: stretch;
  background: var(--p-content-background);
  border: 1px solid var(--p-content-border-color);
  border-radius: 12px;
  padding: .7rem 1rem;
}
.tb-row {
  display: flex; align-items: center; justify-content: space-between;
  gap: 1rem; flex-wrap: wrap;
}
.tb-title { display: flex; align-items: center; gap: .7rem }
.tb-icon {
  width: 38px; height: 38px; flex: 0 0 auto;
  display: grid; place-items: center;
  border-radius: 10px;
  background: color-mix(in srgb, var(--p-primary-color) 14%, transparent);
  color: var(--p-primary-color); font-size: 1.05rem;
}
.tb-title h1 { margin: 0; font-size: 1.05rem; font-weight: 700; line-height: 1.2 }
.tb-sub { margin: .1rem 0 0; font-size: .74rem; color: var(--p-text-muted-color) }

.period-pick {
  display: flex; align-items: center; gap: .4rem;
  background: var(--p-content-hover-background, rgba(0,0,0,.03));
  border: 1px solid var(--p-content-border-color);
  border-radius: 10px; padding: .3rem .35rem;
}
.period-pick .dash { color: var(--p-text-muted-color) }
.ctl-cat { min-width: 12rem; max-width: 16rem }
.ctl-cat :deep(.p-select-label) { overflow: hidden; text-overflow: ellipsis; white-space: nowrap }
.ctl-month { min-width: 8.5rem }
.ctl-year { flex: 0 0 auto }
.ctl-year :deep(.year-input) { width: 4.6rem; text-align: center }
.ctl-date :deep(input) { width: 8.2rem; font-size: .82rem }
.tb-actions { display: flex; gap: .4rem }

.loading-state { display: grid; place-items: center; padding: 3rem 0; color: var(--p-text-muted-color); font-size: 1.4rem }

/* --- The report document. Themeable via --sheet-* vars: light "paper" by
       default, dark-adapted under html.app-dark, forced back to paper for print. --- */
.report-sheet {
  --sheet-bg: #fff;
  --sheet-fg: #111;
  --sheet-faint: #555;         /* period line */
  --sheet-muted: #777;         /* meta / sub / muted */
  --sheet-strong: #444;        /* section label */
  --sheet-border: #cfcfcf;
  --sheet-head-bg: #f2f4f7;
  --sheet-head-fg: #333;
  --sheet-zebra: #fafbfc;
  --sheet-rule: #333;          /* grand-total rule */
  --station-fg: #15803d;
  --station-border: #16a34a;

  background: var(--sheet-bg); color: var(--sheet-fg);
  border: 1px solid var(--p-content-border-color);
  border-radius: 10px;
  box-shadow: 0 1px 3px rgba(0,0,0,.06), 0 6px 24px rgba(0,0,0,.05);
  max-width: 900px; width: 100%; margin: 0 auto;
  padding: 2rem 2.4rem;
}
.report-head { text-align: center; margin-bottom: 1rem }
.rh-company { font-weight: 700; font-size: 1rem; letter-spacing: .01em }
.rh-title { font-weight: 600; font-size: .86rem; margin-top: .55rem }
.rh-period { font-size: .78rem; color: var(--sheet-faint); margin-top: .2rem }
.rh-meta { text-align: center; font-size: .74rem; color: var(--sheet-muted); margin: -.4rem 0 .9rem; }

.section-label { font-size: .78rem; font-weight: 700; text-transform: uppercase; letter-spacing: .03em; color: var(--sheet-strong); margin: .3rem 0 .4rem; }

.report-table { width: 100%; border-collapse: collapse; font-size: .78rem; line-height: 1.3 }
.report-table th, .report-table td {
  border: 1px solid var(--sheet-border); padding: 3px 7px; text-align: left; vertical-align: top;
}
.report-table thead th {
  background: var(--sheet-head-bg); color: var(--sheet-head-fg);
  font-size: .66rem; letter-spacing: .03em; font-weight: 700; text-transform: uppercase;
}
.report-table .c-n { width: 3rem; text-align: right; color: var(--sheet-muted) }
.report-table .c-date { width: 6rem; white-space: nowrap }
.report-table .c-plate { width: 7.5rem; white-space: nowrap }
.report-table .c-acc { width: 11rem; white-space: nowrap }
.report-table .c-form { width: 4.5rem; white-space: nowrap }
.report-table .c-amount { width: 9rem; text-align: right; white-space: nowrap }
.report-table .c-veh { width: 8rem; text-align: right; white-space: nowrap }
.preview-table { max-width: 620px; margin: 0 auto }
.report-table tbody tr:nth-child(even) td { background: var(--sheet-zebra) }
.bank-sub { display: block; font-size: .68rem; color: var(--sheet-muted) }
.total-label { text-align: right; font-weight: 700; background: var(--sheet-head-bg) !important }
.total-val { font-weight: 700; background: var(--sheet-head-bg) !important }
.empty-inline { text-align: center; color: var(--sheet-muted); font-style: italic }
.mono { font-family: ui-monospace, 'Cascadia Mono', Consolas, monospace }

/* station box */
.station-box {
  margin-top: 1rem; border: 1.5px solid var(--station-border);
  border-radius: 10px; overflow: hidden;
  -webkit-print-color-adjust: exact; print-color-adjust: exact;
}
.station-main {
  display: flex; justify-content: space-between; align-items: baseline;
  padding: .55rem .9rem; background: color-mix(in srgb, var(--station-border) 12%, var(--sheet-bg));
}
.station-title { font-weight: 700; font-size: .9rem; color: var(--station-fg) }
.station-amount { font-weight: 700; font-size: 1.05rem; color: var(--station-fg) }
.station-breakdown {
  display: flex; gap: 1.5rem; flex-wrap: wrap;
  padding: .35rem .9rem; font-size: .74rem; color: var(--sheet-faint);
  border-top: 1px solid color-mix(in srgb, var(--station-border) 30%, transparent);
}

.grand-row {
  display: flex; justify-content: space-between; align-items: baseline;
  margin-top: .9rem; padding-top: .5rem; border-top: 2px solid var(--sheet-rule);
  font-size: .92rem; font-weight: 700;
}

.empty-state {
  display: flex; flex-direction: column; align-items: center; gap: .5rem;
  color: var(--sheet-muted); padding: 2.5rem 0; font-size: .9rem;
}
.empty-state i { font-size: 1.6rem }

.report-signature { display: flex; justify-content: space-between; align-items: flex-end; margin-top: 2.4rem; padding: 0 2rem }
.sig-mp { font-size: .82rem }
.sig-line { text-align: center; font-size: .8rem }
.print-only { display: none }
</style>

<!-- GLOBAL (unscoped): dark-mode adaptation for the report sheet + print rules
     that must reach the AppLayout chrome (outside this component's scoped styles). -->
<style>
/* Dark mode: adapt the sheet to a dark surface instead of a glaring white page.
   Only overrides the --sheet-* vars, which the scoped rules read. */
html.app-dark .report-sheet {
  --sheet-bg: #1c1f24;
  --sheet-fg: #e6e8ea;
  --sheet-faint: #9aa2ad;
  --sheet-muted: #8b929c;
  --sheet-strong: #c7ccd3;
  --sheet-border: #3a3f47;
  --sheet-head-bg: #262a30;
  --sheet-head-fg: #c7ccd3;
  --sheet-zebra: #21252b;
  --sheet-rule: #6b7280;
  --station-fg: #4ade80;
  --station-border: #22c55e;
  border-color: #3a3f47;
  box-shadow: 0 1px 3px rgba(0,0,0,.4), 0 6px 24px rgba(0,0,0,.35);
}

@media print {
  /* Always print as white paper, regardless of the on-screen theme. */
  body.printing-report .report-sheet {
    --sheet-bg: #fff !important;
    --sheet-fg: #000 !important;
    --sheet-faint: #333 !important;
    --sheet-muted: #444 !important;
    --sheet-strong: #000 !important;
    --sheet-border: #999 !important;
    --sheet-head-bg: #eee !important;
    --sheet-head-fg: #000 !important;
    --sheet-zebra: #fff !important;
    --sheet-rule: #000 !important;
    --station-fg: #15803d !important;
    --station-border: #16a34a !important;
  }

  body.printing-report .app-sidebar,
  body.printing-report .app-topbar { display: none !important; }
  body.printing-report .app-shell { display: block !important; }
  body.printing-report .app-content { padding: 0 !important; overflow: visible !important; }

  body.printing-report .no-print { display: none !important; }
  body.printing-report .print-only { display: flex !important; }

  body.printing-report .reports-page { gap: 0 }
  body.printing-report .report-sheet {
    box-shadow: none !important; border: 0 !important;
    border-radius: 0 !important; max-width: none !important;
    margin: 0 !important; padding: 0 !important;
  }
  body.printing-report .report-table { font-size: 9pt }
  body.printing-report .report-table thead { display: table-header-group; }
  body.printing-report .report-table tr { page-break-inside: avoid; }
  /* force the (paper) backgrounds to actually render on print */
  body.printing-report .report-sheet,
  body.printing-report .report-sheet * {
    -webkit-print-color-adjust: exact; print-color-adjust: exact;
  }
  body.printing-report .station-box { break-inside: avoid; }

  @page { size: A4 portrait; margin: 12mm 10mm; }
}
</style>
