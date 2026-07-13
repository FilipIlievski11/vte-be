<script setup lang="ts">
// Администрација → „Печатни обрасци": преглед на сите печатни обрасци + отворање на
// визуелен едитор за позициите/фонтот на вредностите. Секој образец се отвора на својата
// печатна рута со ?edit=1 (sample податоци, drag+фонт, Зачувај на серверот).
import { onMounted, ref } from 'vue';
import { useI18n } from 'vue-i18n';
import { useRouter } from 'vue-router';
import { api } from '@/api/client';
import Button from 'primevue/button';
import Tag from 'primevue/tag';

const { t } = useI18n();
const router = useRouter();

interface TemplateEntry {
  code: string;
  nameKey: string;
  group: string;               // module heading
  routeName: string;
  query?: Record<string, string>;
}

// Catalog of every editable print template. `code` matches the backend PrintLayout key.
const catalog: TemplateEntry[] = [
  { code: 'plav',              nameKey: 'plav',        group: 'requests',   routeName: 'request-print',                  query: { form: 'plav' } },
  { code: 'zelen',             nameKey: 'zelen',       group: 'requests',   routeName: 'request-print',                  query: { form: 'zelen' } },
  { code: 'techexam-cert',     nameKey: 'techCert',    group: 'techexam',   routeName: 'technical-exam-print' },
  { code: 'techexam-zapisnik', nameKey: 'techZap',     group: 'techexam',   routeName: 'technical-exam-zapisnik' },
  { code: 'idl-req',           nameKey: 'idlReq',      group: 'idl',        routeName: 'idl-print' },
  { code: 'idl-permit',        nameKey: 'idlPermit',   group: 'idl',        routeName: 'idl-permit' },
  { code: 'perm-cert',         nameKey: 'permCert',    group: 'permission', routeName: 'vehicle-permission-print' },
  { code: 'perm-req',          nameKey: 'permReq',     group: 'permission', routeName: 'vehicle-permission-request-print' },
];

const groups = ['requests', 'techexam', 'idl', 'permission'];

const saved = ref<Record<string, string>>({});   // code → UpdatedAt ISO
const loading = ref(true);

async function load() {
  loading.value = true;
  try {
    const { data } = await api.get<{ code: string; updatedAt: string }[]>('/print-layouts');
    saved.value = Object.fromEntries(data.map(r => [r.code, r.updatedAt]));
  } catch { saved.value = {}; }
  finally { loading.value = false; }
}
onMounted(load);

function edit(e: TemplateEntry) {
  router.push({ name: e.routeName, params: { id: 'sample' }, query: { edit: '1', ...(e.query ?? {}) } });
}
function fmt(iso: string | undefined): string {
  if (!iso) return '';
  const d = new Date(iso);
  return isNaN(d.getTime()) ? '' : d.toLocaleDateString();
}
function entriesFor(g: string) { return catalog.filter(c => c.group === g); }
</script>

<template>
  <div class="page-header">
    <div>
      <h1>{{ t('printTemplates.title') }}</h1>
      <div class="subtitle">{{ t('printTemplates.subtitle') }}</div>
    </div>
  </div>

  <div class="pt-intro">
    <i class="pi pi-info-circle" />
    <span>{{ t('printTemplates.intro') }}</span>
  </div>

  <div v-for="g in groups" :key="g" class="pt-group">
    <div class="pt-group-title">{{ t(`printTemplates.groups.${g}`) }}</div>
    <div class="pt-grid">
      <div v-for="e in entriesFor(g)" :key="e.code" class="pt-card">
        <div class="pt-card-body">
          <div class="pt-name">{{ t(`printTemplates.names.${e.nameKey}`) }}</div>
          <div class="pt-meta">
            <Tag v-if="saved[e.code]" :value="t('printTemplates.customized', { date: fmt(saved[e.code]) })" severity="warn" />
            <span v-else class="pt-default">{{ t('printTemplates.default') }}</span>
          </div>
        </div>
        <div class="pt-card-actions">
          <Button :label="t('printTemplates.edit')" icon="pi pi-pencil" size="small" @click="edit(e)" />
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.subtitle { font-size: .78rem; color: var(--p-text-muted-color); margin-top: .15rem; }
.pt-intro {
  display: flex; align-items: center; gap: .5rem;
  background: color-mix(in srgb, var(--p-primary-color) 8%, transparent);
  border: 1px solid color-mix(in srgb, var(--p-primary-color) 22%, transparent);
  border-radius: 10px; padding: .55rem .8rem; font-size: .8rem;
  color: var(--p-text-color); margin-bottom: 1rem;
}
.pt-intro .pi { color: var(--p-primary-color); }
.pt-group { margin-bottom: 1.2rem; }
.pt-group-title {
  font-size: .72rem; font-weight: 700; text-transform: uppercase; letter-spacing: .04em;
  color: var(--p-text-muted-color); margin: 0 0 .5rem .1rem;
}
.pt-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(260px, 1fr)); gap: .6rem; }
.pt-card {
  display: flex; align-items: center; justify-content: space-between; gap: .6rem;
  background: var(--p-content-background); border: 1px solid var(--p-content-border-color);
  border-radius: 10px; padding: .7rem .85rem;
  transition: box-shadow .15s ease, border-color .15s ease;
}
.pt-card:hover { border-color: color-mix(in srgb, var(--p-content-border-color) 60%, var(--p-primary-color)); box-shadow: var(--shadow-sm); }
.pt-name { font-weight: 600; font-size: .86rem; line-height: 1.25; }
.pt-meta { margin-top: .35rem; font-size: .72rem; }
.pt-default { color: var(--p-text-muted-color); }
.pt-card-actions { flex: 0 0 auto; }
</style>
