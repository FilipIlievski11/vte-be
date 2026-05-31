<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import type { TechExamReportFull } from '@/types';
import Button from 'primevue/button';
import Tag from 'primevue/tag';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import { useToast } from 'primevue/usetoast';

const props = defineProps<{ id: string }>();
const { t } = useI18n();
const router = useRouter();
const toast = useToast();

const report = ref<TechExamReportFull | null>(null);
const loading = ref(true);

async function load() {
  loading.value = true;
  try {
    report.value = (await api.get<TechExamReportFull>(`/technical-exams/${props.id}`)).data;
  } catch (e: any) {
    toast.add({ severity: 'error', summary: t('techExam.loadFailed'), detail: e?.response?.data ?? e?.message, life: 4000 });
  } finally {
    loading.value = false;
  }
}
onMounted(load);

function printCertificate() {
  window.open(`/technical-exams/${props.id}/print`, '_blank');
}
function printZapisnik() {
  window.open(`/technical-exams/${props.id}/zapisnik`, '_blank');
}

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  return isNaN(d.getTime()) ? '—' : d.toLocaleDateString();
}
function num(v: number | null | undefined): string {
  return (v === null || v === undefined) ? '—' : String(v);
}
function axleLabel(axle: number): string {
  switch (axle) {
    case 1: return t('techExam.axle1');
    case 2: return t('techExam.axle2');
    case 3: return t('techExam.axle3');
    case 4: return t('techExam.axle4');
    default: return t('techExam.axleParking');
  }
}
function statusSeverity(id: number): 'success' | 'warn' | 'danger' {
  if (id === 1) return 'success';        // исправен
  if (id === 3) return 'danger';         // неисправен
  return 'warn';                          // вратен / друго
}

// Hide axle rows that have no measurement at all (keeps historical empty reports clean).
const axleRows = computed(() => (report.value?.axles ?? []).filter(a =>
  a.left != null || a.right != null || a.gj != null || a.leftRightDiff != null || a.coefficient != null));

const summaryRows = computed(() => {
  const r = report.value;
  if (!r) return [];
  return [
    { k: 'weight', label: t('techExam.weight'), v: r.weight },
    { k: 'wbe', label: t('techExam.workingBrakeEmpty'), v: r.effectOfWorkingBrakeEmpty },
    { k: 'wbf', label: t('techExam.workingBrakeFull'), v: r.effectOfWorkingBrakeFull },
    { k: 'sb', label: t('techExam.secondaryBrake'), v: r.effectOfSecondaryBrake },
    { k: 'pb', label: t('techExam.parkingBrake'), v: r.effectOfParkingBrake },
    { k: 'co', label: t('techExam.co'), v: r.co },
    { k: 'cot', label: t('techExam.coPlusTurns'), v: r.coPlusTurns },
    { k: 'rpm', label: t('techExam.engineRpm'), v: r.engineRpm },
    { k: 'lambda', label: t('techExam.lambda'), v: r.lambda },
    { k: 'opacity', label: t('techExam.pinpoints'), v: r.pinpoints },
    { k: 'turnspeed', label: t('techExam.speedOfTurns'), v: r.speedOfTurns },
    { k: 'noise', label: t('techExam.noise'), v: r.noise },
    { k: 'oiltemp', label: t('techExam.engineOilTemp'), v: r.engineOilTemp },
  ];
});
// Only show summary measurements that have a value.
const summaryShown = computed(() => summaryRows.value.filter(x => x.v != null));

const hasMeasurements = computed(() => axleRows.value.length > 0 || summaryShown.value.length > 0);
</script>

<template>
  <div class="te-detail">
    <div class="page-header">
      <div class="head-left">
        <Button icon="pi pi-arrow-left" text rounded @click="router.push('/technical-exams')"
                v-tooltip.bottom="t('common.back')" />
        <div>
          <h1>{{ t('techExam.detailTitle') }}<span v-if="report" class="reg"> · {{ report.regNumber || ('#' + report.id) }}</span></h1>
          <div class="subtitle" v-if="report">{{ t('techExam.col.made') }}: {{ fmtDate(report.madeDate) }}</div>
        </div>
      </div>
      <div class="head-right" v-if="report">
        <Button :label="t('common.edit')" icon="pi pi-pencil" size="small" outlined severity="secondary"
                @click="router.push(`/technical-exams/${props.id}/edit`)" />
        <Button :label="t('techExam.printZapisnik')" icon="pi pi-file" size="small" outlined severity="secondary"
                @click="printZapisnik" />
        <Button :label="t('techExam.printCertificate')" icon="pi pi-print" size="small" outlined
                :disabled="!report.vehicleIsRight" @click="printCertificate"
                v-tooltip.bottom="report.vehicleIsRight ? '' : t('techExam.printDisabledHint')" />
        <Tag
          :value="report.vehicleIsRight ? t('techExam.pass') : t('techExam.fail')"
          :severity="report.vehicleIsRight ? 'success' : 'danger'"
          class="big-tag" />
      </div>
    </div>

    <div v-if="loading" class="muted pad">{{ t('common.loading') }}…</div>

    <template v-else-if="report">
      <!-- Header -->
      <section class="card">
        <h2>{{ t('techExam.detailTitle') }}</h2>
        <div class="grid">
          <div class="field span2">
            <label>{{ t('techExam.owner') }}</label>
            <div class="val">
              {{ report.clientName || '—' }}
              <span v-if="report.clientMB" class="muted small"> · {{ report.clientMB }}</span>
            </div>
          </div>
          <div class="field span2">
            <label>{{ t('techExam.vehicle') }}</label>
            <div class="val">
              <span v-if="report.vehiclePlate" class="plate">{{ report.vehiclePlate }}</span>
              <span v-if="report.vehicleVin" class="muted">{{ report.vehicleVin }}</span>
              <span v-if="report.vehicleMakerModel"> · {{ report.vehicleMakerModel }}</span>
              <span v-if="!report.vehiclePlate && !report.vehicleVin">—</span>
            </div>
          </div>
          <div class="field">
            <label>{{ t('techExam.type') }}</label>
            <div class="val">{{ report.typeCode || report.typeName || '—' }}<span v-if="report.typeCode && report.typeName" class="muted small"> · {{ report.typeName }}</span></div>
          </div>
          <div class="field">
            <label>{{ t('techExam.station') }}</label>
            <div class="val">{{ report.organizationName || '—' }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.regNumber') }}</label>
            <div class="val mono">{{ report.regNumber || '—' }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.madeDate') }}</label>
            <div class="val">{{ fmtDate(report.madeDate) }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.validTill') }}</label>
            <div class="val">{{ fmtDate(report.validTillDate) }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.controller1') }}</label>
            <div class="val">{{ report.firstControllerName || (report.firstControllerLegacyId ? ('#' + report.firstControllerLegacyId) : '—') }}</div>
          </div>
          <div class="field">
            <label>{{ t('techExam.controller2') }}</label>
            <div class="val">{{ report.secondControllerName || (report.secondControllerLegacyId ? ('#' + report.secondControllerLegacyId) : '—') }}</div>
          </div>
        </div>
      </section>

      <!-- Measured values -->
      <section class="card">
        <h2>{{ t('techExam.measuredValues') }}</h2>
        <p v-if="!hasMeasurements" class="muted">{{ t('techExam.noMeasurements') }}</p>

        <template v-else>
          <!-- Brake-force grid -->
          <table v-if="axleRows.length" class="brake-table">
            <thead>
              <tr>
                <th></th>
                <th>{{ t('techExam.brakeLeft') }}</th>
                <th>{{ t('techExam.brakeGj') }}</th>
                <th>{{ t('techExam.brakeLeftPj') }}</th>
                <th>{{ t('techExam.brakePn') }}</th>
                <th>{{ t('techExam.brakeRight') }}</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="a in axleRows" :key="a.axle">
                <th class="rowhead">{{ axleLabel(a.axle) }}</th>
                <td>{{ num(a.left) }}</td>
                <td>{{ num(a.gj) }}</td>
                <td>{{ num(a.leftRightDiff) }}</td>
                <td>{{ num(a.coefficient) }}</td>
                <td>{{ num(a.right) }}</td>
              </tr>
            </tbody>
          </table>

          <!-- Summary measurements -->
          <div v-if="summaryShown.length" class="measure-grid">
            <div v-for="m in summaryShown" :key="m.k" class="field">
              <label>{{ m.label }}</label>
              <div class="val">{{ num(m.v) }}</div>
            </div>
          </div>
        </template>

        <div v-if="report.technicalChanges" class="field span-full mt">
          <label>{{ t('techExam.technicalChanges') }}</label>
          <div class="val">{{ report.technicalChanges }}</div>
        </div>
      </section>

      <!-- Defective parts -->
      <section class="card">
        <h2>{{ t('techExam.defects') }} <span class="count">({{ report.details.length }})</span></h2>
        <p v-if="report.details.length === 0" class="muted">{{ t('techExam.noDefects') }}</p>
        <DataTable v-else :value="report.details" stripedRows size="small" class="tight-table" dataKey="id">
          <Column :header="t('techExam.defectPart')">
            <template #body="{ data }">
              <span v-if="data.partCode" class="type-code">{{ data.partCode }}</span>
              <span v-if="data.partName"> {{ data.partName }}</span>
              <span v-if="!data.partCode && !data.partName" class="muted">—</span>
            </template>
          </Column>
          <Column :header="t('techExam.defectStatus')" style="width:120px">
            <template #body="{ data }">
              <Tag :value="data.statusName || ('#' + data.statusId)" :severity="statusSeverity(data.statusId)" />
            </template>
          </Column>
          <Column :header="t('techExam.front')" style="width:70px">
            <template #body="{ data }"><i v-if="data.front" class="pi pi-check ok" /><span v-else class="muted">·</span></template>
          </Column>
          <Column :header="t('techExam.back')" style="width:70px">
            <template #body="{ data }"><i v-if="data.back" class="pi pi-check ok" /><span v-else class="muted">·</span></template>
          </Column>
          <Column :header="t('techExam.left')" style="width:60px">
            <template #body="{ data }"><i v-if="data.onLeft" class="pi pi-check ok" /><span v-else class="muted">·</span></template>
          </Column>
          <Column :header="t('techExam.right')" style="width:60px">
            <template #body="{ data }"><i v-if="data.onRight" class="pi pi-check ok" /><span v-else class="muted">·</span></template>
          </Column>
          <Column :header="t('techExam.col.made')" style="width:100px">
            <template #body="{ data }">{{ fmtDate(data.enteredAt) }}</template>
          </Column>
          <Column :header="t('techExam.note')">
            <template #body="{ data }">{{ data.note || '—' }}</template>
          </Column>
        </DataTable>
      </section>

      <!-- Notes -->
      <section v-if="report.explanationNote || report.driversWarning || report.note" class="card">
        <h2>{{ t('techExam.notesSection') }}</h2>
        <div class="field span-full" v-if="report.explanationNote">
          <label>{{ t('techExam.explanationNote') }}</label>
          <div class="val">{{ report.explanationNote }}</div>
        </div>
        <div class="field span-full" v-if="report.driversWarning">
          <label>{{ t('techExam.driversWarning') }}</label>
          <div class="val">{{ report.driversWarning }}</div>
        </div>
        <div class="field span-full" v-if="report.note">
          <label>{{ t('techExam.note') }}</label>
          <div class="val">{{ report.note }}</div>
        </div>
      </section>
    </template>
  </div>
</template>

<style scoped>
.te-detail { max-width: 1000px; margin: 0 auto; display: flex; flex-direction: column; gap: 1rem; }
.page-header { display: flex; align-items: center; justify-content: space-between; }
.head-left { display: flex; align-items: center; gap: .5rem; }
.head-right { display: flex; align-items: center; gap: .75rem; }
.page-header h1 { margin: 0; font-size: 1.3rem; }
.page-header .reg { color: var(--color-text-muted); font-weight: 500; }
.subtitle { color: var(--color-text-muted); font-size: .85rem; }
.big-tag :deep(.p-tag), .big-tag { font-size: 1rem; padding: .4rem .8rem; }
.pad { padding: 1rem; }
.muted { color: var(--color-text-muted); }
.small { font-size: .75rem; }
.mono { font-family: monospace; }
.mt { margin-top: .75rem; }

.card {
  background: var(--surface-card, #fff);
  border: 1px solid var(--surface-border, #e5e7eb);
  border-radius: 10px;
  padding: 1rem 1.1rem;
}
.card h2 { margin: 0 0 .85rem; font-size: 1rem; }
.card .count { color: var(--color-text-muted); font-weight: 400; }

.grid { display: grid; grid-template-columns: repeat(4, 1fr); gap: .75rem 1rem; }
.measure-grid { display: grid; grid-template-columns: repeat(3, 1fr); gap: .6rem 1rem; margin-top: 1rem; }
.field { display: flex; flex-direction: column; gap: .15rem; min-width: 0; }
.field.span2 { grid-column: span 2; }
.field.span-full { grid-column: 1 / -1; }
.field label { font-size: .72rem; text-transform: uppercase; letter-spacing: .02em; color: var(--color-text-muted); }
.field .val { font-size: .9rem; word-break: break-word; }
.plate { display: inline-block; font-family: monospace; font-weight: 600; padding-right: .5rem; }
.type-code { font-family: monospace; font-weight: 600; }

.brake-table { border-collapse: collapse; width: 100%; font-size: .85rem; }
.brake-table th, .brake-table td {
  border: 1px solid var(--surface-border, #e5e7eb);
  padding: .3rem .5rem; text-align: center;
}
.brake-table thead th { background: var(--surface-100, #f8fafc); font-weight: 600; }
.brake-table .rowhead { background: var(--surface-100, #f8fafc); text-align: left; font-weight: 600; white-space: nowrap; }

.ok { color: #16a34a; }

.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) { padding: .25rem .5rem; font-size: .8125rem; }
</style>
