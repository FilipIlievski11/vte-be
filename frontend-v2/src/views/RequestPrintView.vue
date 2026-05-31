<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { RequestPrintBundle } from '@/types';

const props = defineProps<{ id: string }>();
const { t } = useI18n();

const bundle = ref<RequestPrintBundle | null>(null);
const loading = ref(true);
const error = ref<string | null>(null);

const printCode = computed(() => bundle.value?.type.documentPrintCode ?? 'PLAV');

const headerClass = computed(() => {
  switch (printCode.value) {
    case 'BEL':   return 'form-header bel';
    case 'ZELEN': return 'form-header zelen';
    case 'PLAV':
    default:      return 'form-header plav';
  }
});

function fmtDateTime(s: string | null): string {
  if (!s) return '—';
  return new Date(s).toLocaleString();
}

onMounted(async () => {
  try {
    const { data } = await api.get<RequestPrintBundle>(`/requests/${props.id}/print`);
    bundle.value = data;
    // Wait a tick so the layout paints before the print dialog opens
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
    <div v-if="loading" class="loading">{{ t('requests.print.loading') }}</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else-if="bundle" class="paper">
      <div :class="headerClass">
        <div class="company">{{ bundle.company.name }}</div>
        <div class="form-title">{{ bundle.type.documentPrintName }}</div>
        <div class="form-meta">
          <span>{{ t('requests.print.no') }} {{ bundle.request.id }}</span>
          <span>{{ fmtDateTime(bundle.request.createdAt) }}</span>
        </div>
      </div>

      <h2 class="section">{{ t('requests.print.requestType') }}</h2>
      <div class="kv">
        <strong>{{ bundle.type.name }}</strong>
        <span v-if="bundle.type.description" class="muted"> — {{ bundle.type.description }}</span>
      </div>

      <h2 class="section">{{ t('requests.form.client') }}</h2>
      <table class="data-table">
        <tr><td>{{ t('clients.form.firstName') }} / {{ t('clients.form.lastName') }}</td><td>{{ bundle.client.fullName ?? '—' }}</td></tr>
        <tr><td>EMBG</td><td>{{ bundle.client.mb ?? '—' }}</td></tr>
        <tr><td>{{ t('clients.form.taxNumber') }}</td><td>{{ bundle.client.taxNumber ?? '—' }}</td></tr>
        <tr><td>{{ t('clients.form.address') }}</td><td>{{ bundle.client.address ?? '—' }}</td></tr>
        <tr><td>{{ t('clients.form.phone') }}</td><td>{{ bundle.client.phoneNumber ?? '—' }}</td></tr>
        <tr><td>{{ t('clients.form.email') }}</td><td>{{ bundle.client.email ?? '—' }}</td></tr>
      </table>

      <template v-if="bundle.vehicle">
        <h2 class="section">{{ t('nav.vehicles') }}</h2>
        <table class="data-table">
          <tr><td>VIN</td><td>{{ bundle.vehicle.vin }}</td></tr>
          <tr><td>{{ t('vehicles.col.plate') }}</td><td>{{ bundle.vehicle.plate ?? '—' }}</td></tr>
          <tr><td>{{ t('vehicles.col.maker') }}</td><td>{{ bundle.vehicle.maker ?? '—' }}</td></tr>
          <tr><td>{{ t('vehicles.col.model') }}</td><td>{{ bundle.vehicle.model ?? '—' }}</td></tr>
        </table>
      </template>

      <template v-if="bundle.newClient">
        <h2 class="section">{{ t('requests.form.newOwner') }}</h2>
        <table class="data-table">
          <tr><td>{{ t('clients.form.firstName') }} / {{ t('clients.form.lastName') }}</td><td>{{ bundle.newClient.fullName ?? '—' }}</td></tr>
          <tr><td>EMBG</td><td>{{ bundle.newClient.mb ?? '—' }}</td></tr>
          <tr><td>{{ t('clients.form.address') }}</td><td>{{ bundle.newClient.address ?? '—' }}</td></tr>
        </table>
      </template>

      <template v-if="bundle.ownershipProofs.length">
        <h2 class="section">{{ t('requests.form.sections.ownershipProofs') }}</h2>
        <table class="data-table">
          <tr v-for="p in bundle.ownershipProofs" :key="p.id">
            <td>{{ p.typeName ?? '—' }}</td><td>{{ p.detail ?? '—' }}</td>
          </tr>
        </table>
      </template>

      <template v-if="bundle.paymentProofs.length">
        <h2 class="section">{{ t('requests.form.sections.paymentProofs') }}</h2>
        <table class="data-table">
          <tr v-for="p in bundle.paymentProofs" :key="p.id">
            <td>{{ p.typeName ?? '—' }}</td><td>{{ p.detail ?? '—' }}</td>
          </tr>
        </table>
      </template>

      <template v-if="bundle.request.note">
        <h2 class="section">{{ t('requests.form.note') }}</h2>
        <p>{{ bundle.request.note }}</p>
      </template>

      <div class="signatures">
        <div>
          <div class="line"></div>
          <div class="label">{{ t('requests.print.operatorSig') }}</div>
          <div class="muted small">{{ bundle.request.createdByUserName ?? '' }}</div>
        </div>
        <div>
          <div class="line"></div>
          <div class="label">{{ t('requests.print.clientSig') }}</div>
        </div>
      </div>

      <div class="footer">
        <span>{{ t('requests.print.footer', { id: bundle.request.id }) }}</span>
        <span v-if="bundle.request.endedAt">· {{ t('requests.form.ended') }}: {{ fmtDateTime(bundle.request.endedAt) }}</span>
      </div>

      <div class="toolbar no-print">
        <button @click="doPrint">{{ t('requests.print.printAgain') }}</button>
        <button @click="doClose">{{ t('requests.print.close') }}</button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.print-wrap { background: #e0e0e0; min-height: 100vh; padding: 1rem 0 }
.paper {
  width: 210mm; min-height: 297mm; padding: 18mm 18mm 16mm;
  margin: 0 auto;
  background: #fff; color: #000;
  font-family: Georgia, "Times New Roman", serif; font-size: 11pt; line-height: 1.4;
  box-shadow: 0 0 12px rgba(0,0,0,.15);
}
.form-header {
  padding: .9rem 1rem; margin: 0 -1mm 1.4rem;
  border: 2px solid; color: #fff;
}
.form-header.plav  { background: #1d4ed8; border-color: #1e40af }
.form-header.bel   { background: #ffffff; color: #000; border-color: #1d1d1d }
.form-header.zelen { background: #15803d; border-color: #166534 }
.company { font-size: .9rem; opacity: .9 }
.form-title { font-size: 1.3rem; font-weight: 700; margin-top: .15rem }
.form-meta {
  display: flex; justify-content: space-between; margin-top: .35rem;
  font-size: .85rem; opacity: .9
}
h2.section {
  font-size: 1rem; font-weight: 700; margin: 1.2rem 0 .4rem;
  border-bottom: 1px solid #888; padding-bottom: .15rem
}
.kv { padding: .3rem .1rem }
.muted { color: #555 }
.small { font-size: .8rem }
.data-table { width: 100%; border-collapse: collapse }
.data-table td { padding: .3rem .5rem; border-bottom: 1px solid #ddd; vertical-align: top }
.data-table td:first-child { width: 35%; font-weight: 600; color: #444 }
.signatures {
  display: grid; grid-template-columns: 1fr 1fr; gap: 2rem;
  margin-top: 3rem
}
.signatures .line { border-top: 1px solid #000; margin-bottom: .25rem }
.signatures .label { font-size: .8rem; color: #444; text-align: center }
.footer {
  margin-top: 2rem; padding-top: .5rem; border-top: 1px solid #ccc;
  font-size: .75rem; color: #666; display: flex; gap: .5rem
}
.toolbar.no-print {
  position: fixed; right: 1rem; top: 1rem;
  display: flex; gap: .5rem; z-index: 999
}
.toolbar.no-print button {
  padding: .35rem .75rem; border: 1px solid #888; background: #fff;
  border-radius: 4px; cursor: pointer; font-size: .85rem
}
.loading, .error {
  text-align: center; padding: 4rem 0; font-family: system-ui, sans-serif
}
.error { color: #b91c1c }

@media print {
  .print-wrap { background: #fff; padding: 0 }
  .paper { box-shadow: none; margin: 0 }
  .no-print { display: none !important }
  @page { size: A4; margin: 12mm }
}
</style>
