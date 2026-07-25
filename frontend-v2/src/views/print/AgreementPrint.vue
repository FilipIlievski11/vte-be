<script setup lang="ts">
// ДОГОВОР за отплата на надоместок за извршена услуга — легаси
// rptPaymentDocumentDogovor, пресоздаден како A4 печат на празна хартија.
// Текстовите на членовите се земени 1:1 од легаси образецот (вкл. resx).
import { onMounted, ref } from 'vue';
import { api } from '@/api/client';
import type { AgreementPrint } from '@/types';

const props = defineProps<{ id: string }>();

const a = ref<AgreementPrint | null>(null);
const loading = ref(true);
const error = ref('');

function fmtDate(s: string | null | undefined): string {
  if (!s) return '';
  const d = new Date(s);
  if (isNaN(d.getTime())) return '';
  const p = (n: number) => String(n).padStart(2, '0');
  return `${p(d.getDate())}/${p(d.getMonth() + 1)}/${d.getFullYear()}`;
}
function fmtInt(v: number): string { return String(Math.round(v)); }

onMounted(async () => {
  try {
    a.value = (await api.get<AgreementPrint>(`/payment-documents/${props.id}/agreement-print`)).data;
    setTimeout(() => window.print(), 250);
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
    <div v-if="loading" class="screen-note">Се вчитува…</div>
    <div v-else-if="error" class="screen-note error">{{ error }}</div>

    <template v-else-if="a">
      <div class="toolbar no-print">
        <button @click="doPrint">Печати</button>
        <button @click="doClose">Затвори</button>
      </div>

      <div class="page">
        <div class="title-row">
          <span class="title">ДОГОВОР Бр: {{ a.documentNumber }}</span>
        </div>
        <div class="subtitle">за отплата на надоместок за извршена услуга</div>
        <div class="line-meta">
          Склучен со {{ a.communityName || '________' }} на ден {{ fmtDate(a.agreementDate) }} помеѓу:
        </div>

        <p class="just">1. {{ (a.orgName || '').toUpperCase() }} претставувано од {{ a.orgSecretary || '________' }} во понатамошниот текст давател на услуга</p>
        <p class="just">2. Корисникот на услуга {{ a.clientName || '' }} со адреса на живеење {{ a.clientAddress || '' }} и ЕМБГ: {{ a.clientEmbg || '' }} сопственик на возило со рег.бр {{ a.plate || '' }}</p>
        <p class="just">Гарантот на корисникот кој ги презема обврските за плаќање на корисникот {{ a.guarantorName || '________' }} {{ a.guarantorAddress || '' }} со ЕМБГ: {{ a.guarantorEmbg || '' }}</p>

        <div class="clen">член 1</div>
        <p class="just indent">Предмет на овој договор е начинот на отплата на надоместокот за извршени услуги при регистрација на моторни возила и други дополнителни услуги по договорени рати за одложено плаќање</p>

        <div class="clen">член 2</div>
        <p class="just indent">По барање на корисникот на услуги, давателот на услуги ги изврши следните услуги:</p>
        <div class="presmetka-row">
          <span>Пресметка бр.: <strong>{{ a.documentNumber }}</strong></span>
          <span>Износ: <strong>{{ fmtInt(a.total) }}</strong></span>
          <span>Останата сума за плаќање: <strong>{{ fmtInt(a.remaining) }}</strong></span>
        </div>
        <table class="uslugi">
          <thead>
            <tr><th>Услуга</th><th>Цена без ДДВ</th><th>ДДВ</th></tr>
          </thead>
          <tbody>
            <tr v-for="(s, i) in a.services" :key="i">
              <td class="left">{{ s.name }}</td>
              <td>{{ fmtInt(s.bezDdv) }}</td>
              <td>{{ fmtInt(s.ddv) }}</td>
            </tr>
          </tbody>
        </table>

        <div class="clen">член 3</div>
        <p class="just indent">Давателот на услугата и корисникот на услугата се согласија преостанатите рати да се прераспределат на следниов начин:</p>
        <table class="rati">
          <thead>
            <tr>
              <th style="width:12%">Ред. бр.</th>
              <th style="width:16%">Износ</th>
              <th style="width:13%">Платено</th>
              <th style="width:29%">Платено на ден / да се плати до</th>
              <th>Забелешка</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="r in a.installments" :key="r.sequenceNo">
              <td>{{ r.sequenceNo }}</td>
              <td>{{ fmtInt(r.amount) }}</td>
              <td>{{ r.paid ? 'ДА' : 'НЕ' }}</td>
              <td>{{ fmtDate(r.date) }}</td>
              <td class="left">{{ r.note || '' }}</td>
            </tr>
          </tbody>
        </table>

        <div class="clen">член 4</div>
        <p class="just indent">Корисникот на услуга е должен износот на доспеаните рати да го уплаќа на жиро сметка или во готово на благајната на давателот на услуга.</p>
        <p class="just indent">Доколку исплатата корисникот на услугата или гарантот на корисникот ја вршат со задржување на плата, исплатата ќе се врши преку правното лице каде се вработени. Во тој случај составен дел на овој договор ќе биде и изјавата за превземање на долг од гарантот.</p>

        <div class="clen">член 5</div>
        <p class="just indent">Потврдувам дека сум согласен моите лични податоци да се обработуваат согласно потребата од извршување на овој договор.</p>

        <div class="clen">член 6</div>
        <p class="just indent">Во случај на спор се применуваат одредбите од законот за облигациони односи</p>

        <div class="signatures">
          <div class="sig">
            <div class="sig-label">Корисник на услуга</div>
            <div class="sig-line"></div>
          </div>
          <div class="sig">
            <div class="sig-label">Гарант на услуга</div>
            <div class="sig-line"></div>
          </div>
          <div class="sig">
            <div class="sig-label">Референт</div>
            <div class="sig-line"></div>
          </div>
        </div>

        <p class="just">Примерок од договорот е доставен до секоја страна.</p>
      </div>
    </template>
  </div>
</template>

<style scoped>
.print-wrap { background: #777; min-height: 100vh; padding: 1rem 0; }
.screen-note { color: #fff; text-align: center; padding: 2rem; font-family: Arial, sans-serif; }
.screen-note.error { color: #fecaca; }
.toolbar { display: flex; gap: .5rem; justify-content: center; margin-bottom: .75rem; }
.toolbar button { padding: .35rem 1rem; cursor: pointer; }

.page {
  width: 210mm; min-height: 296mm; margin: 0 auto; background: #fff; color: #000;
  padding: 14mm 13mm 10mm;
  font-family: Arial, Helvetica, sans-serif; font-size: 10pt; line-height: 1.25;
  box-sizing: border-box;
}

.title-row { text-align: center; margin-top: 2mm; }
.title { font-size: 12.5pt; font-weight: 700; }
.subtitle { text-align: center; font-weight: 700; margin-top: 1.5mm; }
.line-meta { text-align: center; margin: 1.5mm 0 4mm; }

p { margin: 0 0 2.2mm; }
.just { text-align: justify; }
.indent { text-indent: 8mm; }

.clen { text-align: center; font-weight: 700; font-size: 11pt; margin: 3.5mm 0 1.5mm; }

.presmetka-row { display: flex; gap: 10mm; margin: 1mm 0 2mm; }

table { border-collapse: collapse; width: 88%; margin: 0 auto 2mm; font-size: 10pt; }
table td, table th { padding: .8mm 1.5mm; text-align: center; font-weight: 400; }
.uslugi thead th { border-bottom: 1px solid #000; }
.uslugi .left, .rati .left { text-align: left; }
.rati { width: 92%; }
.rati th, .rati td { border: 1px solid #000; }

.signatures { display: flex; justify-content: space-between; margin: 8mm 2mm 3mm; }
.sig { width: 47mm; }
.sig-label { margin-bottom: 7mm; }
.sig-line { border-top: 1px solid #000; }

@media print {
  .print-wrap { background: #fff; padding: 0; }
  .no-print { display: none !important; }
  .page { margin: 0; width: auto; min-height: auto; }
  @page { size: A4 portrait; margin: 0; }
}
</style>
