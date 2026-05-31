<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { api } from '@/api/client';
import type { LegacySyncStatus, LegacySyncResult } from '@/types';
import Button from 'primevue/button';
import { useToast } from 'primevue/usetoast';

const { t } = useI18n();
const toast = useToast();

const status = ref<LegacySyncStatus | null>(null);
const result = ref<LegacySyncResult | null>(null);
const loadingStatus = ref(true);
const syncing = ref(false);

async function loadStatus() {
  loadingStatus.value = true;
  try {
    status.value = (await api.get<LegacySyncStatus>('/admin/legacy-sync/status')).data;
  } catch {
    status.value = null;
  } finally {
    loadingStatus.value = false;
  }
}

async function sync() {
  syncing.value = true;
  result.value = null;
  try {
    const { data } = await api.post<LegacySyncResult>('/admin/legacy-sync');
    result.value = data;
    const total = data.clients + data.vehicles + data.requests + data.technicalExamReports;
    toast.add({
      severity: total > 0 ? 'success' : 'info',
      summary: t('legacySync.doneTitle'),
      detail: t('legacySync.doneDetail', {
        clients: data.clients, vehicles: data.vehicles, requests: data.requests,
        ms: data.durationMs,
      }),
      life: 6000,
    });
    await loadStatus();
  } catch (e: any) {
    toast.add({
      severity: 'error',
      summary: t('legacySync.errorTitle'),
      detail: e?.response?.data?.detail ?? e?.response?.data?.title ?? e?.message ?? 'Failed',
      life: 9000,
    });
  } finally {
    syncing.value = false;
  }
}

onMounted(loadStatus);

const resultRows = () => result.value ? [
  { key: 'clients',         val: result.value.clients },
  { key: 'vehicles',        val: result.value.vehicles },
  { key: 'registrations',   val: result.value.registrations },
  { key: 'relations',       val: result.value.relations },
  { key: 'requests',        val: result.value.requests },
  { key: 'ownershipProofs', val: result.value.ownershipProofs },
  { key: 'paymentProofs',   val: result.value.paymentProofs },
  { key: 'references',      val: result.value.references },
  { key: 'technicalExamReports', val: result.value.technicalExamReports },
] : [];
</script>

<template>
  <div class="legacy-sync">
    <header class="page-head">
      <h1>{{ t('legacySync.title') }}</h1>
      <p class="sub">{{ t('legacySync.subtitle') }}</p>
    </header>

    <!-- Current state -->
    <section class="card">
      <h2>{{ t('legacySync.currentState') }}</h2>
      <div v-if="loadingStatus" class="muted">{{ t('legacySync.loading') }}…</div>
      <div v-else-if="status" class="stat-grid">
        <div class="stat"><span class="n">{{ status.clients.toLocaleString() }}</span><span class="l">{{ t('legacySync.clients') }}</span><span class="id">max #{{ status.maxClientId }}</span></div>
        <div class="stat"><span class="n">{{ status.vehicles.toLocaleString() }}</span><span class="l">{{ t('legacySync.vehicles') }}</span><span class="id">max #{{ status.maxVehicleId }}</span></div>
        <div class="stat"><span class="n">{{ status.requests.toLocaleString() }}</span><span class="l">{{ t('legacySync.requests') }}</span><span class="id">max #{{ status.maxRequestId }}</span></div>
        <div class="stat"><span class="n">{{ status.technicalExamReports.toLocaleString() }}</span><span class="l">{{ t('legacySync.technicalExamReports') }}</span></div>
      </div>
      <div v-else class="err">{{ t('legacySync.statusError') }}</div>
      <p v-if="status && !status.enabled" class="warn">{{ t('legacySync.disabled') }}</p>
    </section>

    <!-- Action -->
    <section class="card action">
      <div class="action-text">
        <h2>{{ t('legacySync.runTitle') }}</h2>
        <p class="muted">{{ t('legacySync.runHelp') }}</p>
      </div>
      <Button
        :label="syncing ? t('legacySync.syncing') : t('legacySync.syncNow')"
        icon="pi pi-sync"
        :loading="syncing"
        :disabled="syncing || (status != null && !status.enabled)"
        size="large"
        @click="sync"
      />
    </section>

    <!-- Result -->
    <section v-if="result" class="card">
      <h2>{{ t('legacySync.lastRun') }} <span class="ms">({{ result.durationMs }} ms)</span></h2>
      <table class="result-table">
        <tbody>
          <tr v-for="r in resultRows()" :key="r.key" :class="{ zero: r.val === 0 }">
            <td class="rk">{{ t('legacySync.' + r.key) }}</td>
            <td class="rv">+{{ r.val }}</td>
          </tr>
        </tbody>
      </table>
      <p v-if="result.clients + result.vehicles + result.requests + result.technicalExamReports === 0" class="muted uptodate">
        {{ t('legacySync.upToDate') }}
      </p>
    </section>
  </div>
</template>

<style scoped>
.legacy-sync { max-width: 820px; margin: 0 auto; padding: 1.25rem; display: flex; flex-direction: column; gap: 1.25rem; }
.page-head h1 { margin: 0; font-size: 1.5rem; }
.page-head .sub { margin: .25rem 0 0; color: var(--text-muted, #6b7280); }
.card { background: var(--surface-card, #fff); border: 1px solid var(--surface-border, #e5e7eb); border-radius: 10px; padding: 1.1rem 1.25rem; }
.card h2 { margin: 0 0 .85rem; font-size: 1.05rem; }
.stat-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(160px, 1fr)); gap: 1rem; }
.stat { display: flex; flex-direction: column; gap: .15rem; padding: .75rem; background: var(--surface-100, #f8fafc); border-radius: 8px; }
.stat .n { font-size: 1.6rem; font-weight: 700; }
.stat .l { color: var(--text-muted, #6b7280); font-size: .85rem; }
.stat .id { color: #9ca3af; font-size: .75rem; }
.action { display: flex; align-items: center; justify-content: space-between; gap: 1.5rem; }
.action-text h2 { margin: 0 0 .25rem; }
.action-text .muted { margin: 0; }
.muted { color: var(--text-muted, #6b7280); }
.warn { color: #b45309; margin: .75rem 0 0; }
.err { color: #b91c1c; }
.ms { color: #9ca3af; font-weight: 400; font-size: .85rem; }
.result-table { width: 100%; border-collapse: collapse; }
.result-table td { padding: .45rem .25rem; border-bottom: 1px solid var(--surface-border, #f1f5f9); }
.result-table .rv { text-align: right; font-variant-numeric: tabular-nums; font-weight: 700; color: #16a34a; width: 6rem; }
.result-table tr.zero .rv { color: #9ca3af; font-weight: 400; }
.uptodate { margin: .85rem 0 0; }
</style>
