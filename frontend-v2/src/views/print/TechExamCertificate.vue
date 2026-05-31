<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { TechExamCertificate } from '@/types';

const props = defineProps<{ id: string }>();

const cert = ref<TechExamCertificate | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

function fmtDate(s: string | null): string {
  if (!s) return '—';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '—';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}.${mm}.${d.getFullYear()}`;
}

onMounted(async () => {
  try {
    const { data } = await api.get<TechExamCertificate>(`/technical-exams/${props.id}/print`);
    cert.value = data;
    // Auto-open the print dialog only for a compliant vehicle (legacy issues the
    // certificate only when VehicleIsRight = true).
    if (data.vehicleIsRight) setTimeout(() => window.print(), 250);
  } catch (e: any) {
    error.value = e?.response?.data?.error ?? e?.message ?? 'Failed to load';
  } finally {
    loading.value = false;
  }
});

function doPrint() { window.print(); }
function doClose() { window.close(); }
</script>

<template>
  <div class="print-wrap">
    <div v-if="loading" class="loading">Се вчитува…</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else-if="cert" class="paper">
      <!-- Letterhead -->
      <div class="letterhead">
        <div class="org-name">{{ (cert.organization.name || '').toUpperCase() }}</div>
        <div class="org-line" v-if="cert.organization.address">{{ cert.organization.address }}</div>
        <div class="org-line" v-if="cert.organization.cityLine">{{ cert.organization.cityLine }}</div>
        <div class="org-line" v-if="cert.organization.phone || cert.organization.fax">
          <span v-if="cert.organization.phone">Тел: {{ cert.organization.phone }}</span>
          <span v-if="cert.organization.fax">&nbsp;&nbsp;&nbsp;Факс: {{ cert.organization.fax }}</span>
        </div>
      </div>

      <p v-if="!cert.vehicleIsRight" class="warn no-print">
        Возилото не е технички исправно — потврда не се издава.
      </p>

      <!-- Preamble -->
      <p class="preamble">
        Врз основа на член 48 став 3 и 5 и член 67 став 1 алинеи 1 и 2, а во врска со член 52 став 2
        од Законот за возила се издава
      </p>

      <!-- Title -->
      <h1 class="title">ПОТВРДА</h1>
      <div class="reg-no">Број: <strong>{{ cert.regNumber || '—' }}</strong></div>

      <p class="descriptor">за извршен преглед на техничка исправност на моторното возило:</p>

      <!-- Vehicle table -->
      <table class="vehicle-table">
        <tr><td class="code">A</td>  <td class="lbl">Регистарска ознака на возилото:</td><td class="val">{{ cert.vehicle.registration || '—' }}</td></tr>
        <tr><td class="code">Ј</td>  <td class="lbl">Категорија и вид на возилото:</td>  <td class="val">{{ cert.vehicle.category || '—' }}</td></tr>
        <tr><td class="code">D.1</td><td class="lbl">Марка:</td>                          <td class="val">{{ cert.vehicle.maker || '—' }}</td></tr>
        <tr><td class="code">D.2</td><td class="lbl">Тип/варијанта/изведба:</td>          <td class="val">{{ cert.vehicle.typeText || '—' }}</td></tr>
        <tr><td class="code">D.3</td><td class="lbl">Комерцијална ознака:</td>            <td class="val">{{ cert.vehicle.model || '—' }}</td></tr>
        <tr><td class="code">E</td>  <td class="lbl">Идентификационен број на возилото (шасија/поставеност)</td><td class="val">{{ cert.vehicle.vin || '—' }}</td></tr>
      </table>

      <!-- Compliance statement -->
      <p class="statement">
        Потврдуваме дека горенаведеното возило ги исполнува условите пропишани со Правилникот за
        технички преглед на возила и е технички исправно.
      </p>

      <!-- Next exam -->
      <p class="next-exam">
        Возилото треба да го изврши наредниот технички преглед на
        <span class="date-underline">{{ fmtDate(cert.validTillDate) }}</span>
      </p>
      <p class="next-exam-caption">(датум на нареден технички преглед)</p>

      <!-- Signatures -->
      <div class="signatures">
        <div class="sig-left">
          <div>Место и датум:
            <span class="sig-value">{{ [cert.stationCity, fmtDate(cert.madeDate)].filter(Boolean).join(', ') }}</span>
          </div>
        </div>
        <div class="sig-right">
          <div class="sig-name" v-if="cert.controllerName">{{ cert.controllerName }}</div>
          <div class="sig-line"></div>
          <div class="sig-label">Потпис и печат:</div>
        </div>
      </div>

      <div class="toolbar no-print">
        <button @click="doPrint">Печати</button>
        <button @click="doClose">Затвори</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.print-wrap { background: #e0e0e0; min-height: 100vh; padding: 1rem 0; }
.paper {
  width: 210mm; min-height: 297mm; padding: 16mm 18mm;
  margin: 0 auto; background: #fff; color: #000;
  font-family: "Times New Roman", Georgia, serif; font-size: 12pt; line-height: 1.45;
  box-shadow: 0 0 12px rgba(0,0,0,.15);
}
.letterhead {
  border: 2px solid #000; padding: .6rem 1rem; text-align: center; margin-bottom: 1.6rem;
}
.org-name { font-family: Arial, sans-serif; font-weight: 700; font-size: 14pt; }
.org-line { font-family: Arial, sans-serif; font-weight: 700; font-size: 11pt; }
.preamble { text-align: center; margin: 0 0 1.2rem; }
.title { text-align: center; font-size: 18pt; font-weight: 700; margin: 0 0 .2rem; letter-spacing: 1px; }
.reg-no { text-align: center; margin-bottom: 1.4rem; }
.descriptor { text-align: center; margin: 0 0 .8rem; }
.vehicle-table { width: 100%; border-collapse: collapse; margin-bottom: 1.6rem; }
.vehicle-table td { border: 1px solid #000; padding: .3rem .5rem; vertical-align: middle; }
.vehicle-table .code { width: 36px; text-align: center; font-weight: 700; }
.vehicle-table .lbl { width: 55%; }
.vehicle-table .val { font-weight: 700; }
.statement { text-align: justify; margin: 0 0 2rem; }
.next-exam { margin: 0 0 .1rem; }
.date-underline { border-bottom: 1px solid #000; padding: 0 1.5rem; font-weight: 700; }
.next-exam-caption { font-size: 8pt; font-style: italic; margin: 0 0 3rem; }
.signatures { display: grid; grid-template-columns: 1fr 1fr; gap: 2rem; margin-top: 2.5rem; align-items: end; }
.sig-value { font-weight: 700; }
.sig-right { text-align: center; }
.sig-name { font-weight: 700; margin-bottom: .15rem; }
.sig-line { border-top: 1px solid #000; margin: 0 1rem .25rem; }
.sig-label { font-size: 11pt; }
.warn { color: #b91c1c; text-align: center; font-weight: 700; border: 1px solid #b91c1c; padding: .4rem; border-radius: 4px; }
.loading, .error { text-align: center; padding: 4rem 0; font-family: system-ui, sans-serif; }
.error { color: #b91c1c; }
.toolbar.no-print { position: fixed; right: 1rem; top: 1rem; display: flex; gap: .5rem; z-index: 999; }
.toolbar.no-print button { padding: .35rem .75rem; border: 1px solid #888; background: #fff; border-radius: 4px; cursor: pointer; font-size: .85rem; }

@media print {
  .print-wrap { background: #fff; padding: 0; }
  .paper { box-shadow: none; margin: 0; }
  .no-print { display: none !important; }
  @page { size: A4; margin: 14mm; }
}
</style>
