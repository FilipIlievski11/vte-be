<script setup lang="ts">
import { reactive, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'
import api from '@/api/client'
import { useToast } from '@/composables/useToast'
import Button from 'primevue/button'
import Message from 'primevue/message'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'

interface Status { vteMaxId: number; legacyMaxId: number | null; pending: number }
interface Skip   { id: number; reason: string }
interface Result { inserted: number; skipped: number; newMaxId: number; total: number; skips: Skip[] }

interface Section {
  key: string
  titleKey: string
  endpoint: string       // e.g. 'customers' or 'streets'
  status: Status | null
  result: Result | null
  loadingStatus: boolean
  running: boolean
}

const { t } = useI18n()
const toast = useToast()

const sections = reactive<Section[]>([
  { key: 'customers',      titleKey: 'migration.section.customers',     endpoint: 'customers',      status: null, result: null, loadingStatus: false, running: false },
  { key: 'streets',        titleKey: 'migration.section.streets',       endpoint: 'streets',        status: null, result: null, loadingStatus: false, running: false },
  { key: 'vehicle-makers', titleKey: 'migration.section.vehicleMakers', endpoint: 'vehicle-makers', status: null, result: null, loadingStatus: false, running: false },
  { key: 'vehicle-models', titleKey: 'migration.section.vehicleModels', endpoint: 'vehicle-models', status: null, result: null, loadingStatus: false, running: false },
  { key: 'vehicles',       titleKey: 'migration.section.vehicles',      endpoint: 'vehicles',       status: null, result: null, loadingStatus: false, running: false },
  { key: 'relations',      titleKey: 'migration.section.relations',     endpoint: 'relations',      status: null, result: null, loadingStatus: false, running: false },
])

const anyLegacyUnreachable = computed(() => sections.some(s => s.status && s.status.legacyMaxId === null))

async function refreshOne(s: Section) {
  s.loadingStatus = true
  try {
    const { data } = await api.get<Status>(`/admin/migration/${s.endpoint}/status`)
    s.status = data
  } finally { s.loadingStatus = false }
}

async function refreshAll() { await Promise.all(sections.map(refreshOne)) }

async function run(s: Section) {
  s.running = true; s.result = null
  try {
    const { data } = await api.post<Result>(`/admin/migration/${s.endpoint}/delta`)
    s.result = data
    toast.ok(`${t('migration.done')}: +${data.inserted}`)
    await refreshOne(s)
  } catch (e: any) {
    toast.error(e?.response?.data?.error ?? 'Migration failed.')
  } finally { s.running = false }
}

function canRun(s: Section)  { return !!s.status && s.status.legacyMaxId !== null && s.status.pending > 0 }
function reachable(s: Section) { return !!s.status && s.status.legacyMaxId !== null }

onMounted(refreshAll)
</script>

<template>
  <div class="migration-page">
    <div class="page-header">
      <div>
        <h1>{{ t('migration.title') }}</h1>
        <div class="subtitle">{{ t('migration.subtitle') }}</div>
      </div>
      <Button :label="t('common.refresh')" icon="pi pi-refresh" outlined size="small"
              :loading="sections.some(s => s.loadingStatus)" @click="refreshAll" />
    </div>

    <Message v-if="anyLegacyUnreachable" severity="warn" :closable="false" class="mb-3">
      {{ t('migration.legacyUnreachable') }}
    </Message>

    <!-- One card per migratable resource -->
    <div v-for="s in sections" :key="s.key" class="card">
      <div class="card-header">{{ t(s.titleKey) }}</div>
      <div class="card-body">
        <div v-if="!s.status && s.loadingStatus" class="muted">{{ t('common.loading') }}</div>
        <div v-else-if="!s.status" class="muted">—</div>
        <template v-else>
          <div class="stats">
            <div class="stat">
              <div class="stat-label">{{ t('migration.vteMaxId') }}</div>
              <div class="stat-value">{{ s.status.vteMaxId.toLocaleString() }}</div>
            </div>
            <div class="stat">
              <div class="stat-label">{{ t('migration.legacyMaxId') }}</div>
              <div class="stat-value">{{ s.status.legacyMaxId !== null ? s.status.legacyMaxId.toLocaleString() : '—' }}</div>
            </div>
            <div class="stat highlight">
              <div class="stat-label">{{ t('migration.pending') }}</div>
              <div class="stat-value">{{ s.status.pending.toLocaleString() }}</div>
            </div>
          </div>

          <div class="run-row">
            <Button :label="t('migration.run')" icon="pi pi-play" severity="success" size="small"
                    :disabled="!canRun(s)" :loading="s.running" @click="run(s)" />
            <span v-if="!canRun(s) && reachable(s)" class="muted small">{{ t('migration.nothingToMigrate') }}</span>
          </div>

          <!-- Inline last-run for this resource -->
          <div v-if="s.result" class="last-run">
            <h3>{{ t('migration.lastRun') }}</h3>
            <div class="stats">
              <div class="stat success">
                <div class="stat-label">{{ t('migration.inserted') }}</div>
                <div class="stat-value">{{ s.result.inserted.toLocaleString() }}</div>
              </div>
              <div class="stat">
                <div class="stat-label">{{ t('migration.skipped') }}</div>
                <div class="stat-value">{{ s.result.skipped.toLocaleString() }}</div>
              </div>
              <div class="stat">
                <div class="stat-label">{{ t('migration.newMaxId') }}</div>
                <div class="stat-value">{{ s.result.newMaxId.toLocaleString() }}</div>
              </div>
              <div class="stat">
                <div class="stat-label">{{ t('migration.total') }}</div>
                <div class="stat-value">{{ s.result.total.toLocaleString() }}</div>
              </div>
            </div>
            <div v-if="s.result.skips.length" class="skips">
              <h4>{{ t('migration.skips') }}</h4>
              <DataTable :value="s.result.skips" size="small" stripedRows class="skip-table">
                <Column field="id"     :header="t('migration.col.id')"     style="width: 110px" />
                <Column field="reason" :header="t('migration.col.reason')" />
              </DataTable>
            </div>
          </div>
        </template>
      </div>
    </div>
  </div>
</template>

<style scoped>
.migration-page { max-width: 1100px; margin: 0 auto; }
.muted { color: var(--color-text-muted); }
.small { font-size: 0.8125rem; }
.mb-3  { margin-bottom: 0.75rem; }

.migration-page :deep(.card .card-header) {
  position: relative;
  padding: 0.5rem 0.875rem;
  font-size: 0.875rem;
  background: linear-gradient(180deg, color-mix(in srgb, var(--color-surface) 92%, var(--color-brand-500)) 0%, var(--color-surface) 100%);
}
.migration-page :deep(.card .card-header)::before {
  content: ''; position: absolute; left: 0; top: 0; bottom: 0;
  width: 3px; background: var(--color-brand-500); border-radius: 0 2px 2px 0;
}
.migration-page :deep(.card .card-body) { padding: 0.875rem 1rem; }
.migration-page :deep(.card + .card)    { margin-top: 0.625rem; }

.stats { display: grid; grid-template-columns: repeat(auto-fit, minmax(160px, 1fr)); gap: 0.625rem; margin-bottom: 0.625rem; }
.stat {
  background: var(--color-bg);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-sm);
  padding: 0.625rem 0.875rem;
  display: flex; flex-direction: column; gap: 0.125rem;
}
.stat-label { font-size: 0.7rem; color: var(--color-text-muted); text-transform: uppercase; letter-spacing: 0.04em; }
.stat-value { font-size: 1.25rem; font-weight: 600; color: var(--color-text); }
.stat.highlight {
  background: color-mix(in srgb, var(--color-brand-500) 8%, var(--color-bg));
  border-color: color-mix(in srgb, var(--color-border) 50%, var(--color-brand-500));
}
.stat.highlight .stat-value { color: var(--color-brand-600); }
.stat.success {
  background: var(--color-success-50);
  border-color: color-mix(in srgb, var(--color-border) 50%, var(--color-success-500));
}
.stat.success .stat-value { color: var(--color-success-600); }

.run-row { display: flex; align-items: center; gap: 0.625rem; padding-top: 0.125rem; }

.last-run { margin-top: 0.875rem; padding-top: 0.625rem; border-top: 1px dashed var(--color-border); }
.last-run h3 { font-size: 0.8125rem; margin: 0 0 0.5rem; color: var(--color-text-secondary); font-weight: 600; }
.skips h4 { font-size: 0.75rem; margin: 0.625rem 0 0.375rem; color: var(--color-text-muted); font-weight: 600; text-transform: uppercase; letter-spacing: 0.04em; }
.skip-table :deep(.p-datatable-tbody td),
.skip-table :deep(.p-datatable-thead th) { padding: 0.25rem 0.5rem; font-size: 0.8125rem; }
</style>
