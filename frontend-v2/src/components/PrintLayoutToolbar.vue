<script setup lang="ts">
// Floating edit toolbar shown on a print template when opened in layout-edit mode
// (?edit=1). Drives the shared usePrintLayout composable: save/reset, per-field font
// size, arrow-key nudge, grid guides toggle. Rendered ONLY while lay.editing is true,
// so its global keyboard listener is scoped to the editing session.
import { onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useToast } from 'primevue/usetoast';
import { useConfirm } from 'primevue/useconfirm';
import Button from 'primevue/button';
import type { PrintLayoutApi } from '@/composables/usePrintLayout';

const props = defineProps<{ lay: PrintLayoutApi; name: string }>();
const guides = defineModel<boolean>('guides', { default: true });

const router = useRouter();
const { t } = useI18n();
const toast = useToast();
const confirm = useConfirm();

async function onSave() {
  const ok = await props.lay.save(props.name);
  toast.add(ok
    ? { severity: 'success', summary: t('printLayouts.saved'), life: 1800 }
    : { severity: 'error', summary: t('printLayouts.saveFailed'), life: 4000 });
}
function onResetAll() {
  confirm.require({
    message: t('printLayouts.resetAllConfirm'),
    header: t('printLayouts.resetAll'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: { label: t('common.cancel'), severity: 'secondary', outlined: true },
    acceptProps: { label: t('printLayouts.resetAll'), severity: 'danger' },
    accept: async () => {
      const ok = await props.lay.resetAll();
      toast.add(ok
        ? { severity: 'success', summary: t('printLayouts.resetDone'), life: 1800 }
        : { severity: 'error', summary: t('printLayouts.saveFailed'), life: 4000 });
    },
  });
}
function done() {
  // Back to the admin catalog (or history if reachable).
  if (window.history.length > 1) router.back();
  else router.push('/print-templates');
}

function onKey(e: KeyboardEvent) {
  if (!props.lay.editing.value) return;
  const step = e.shiftKey ? 2 : 0.5;
  if (e.key === 'ArrowLeft')  { props.lay.nudge(-step, 0); e.preventDefault(); }
  else if (e.key === 'ArrowRight') { props.lay.nudge(step, 0); e.preventDefault(); }
  else if (e.key === 'ArrowUp')    { props.lay.nudge(0, -step); e.preventDefault(); }
  else if (e.key === 'ArrowDown')  { props.lay.nudge(0, step); e.preventDefault(); }
  else if (e.key === '+' || e.key === '=') { props.lay.bumpSize(0.5); e.preventDefault(); }
  else if (e.key === '-' || e.key === '_') { props.lay.bumpSize(-0.5); e.preventDefault(); }
}
onMounted(() => window.addEventListener('keydown', onKey));
onUnmounted(() => window.removeEventListener('keydown', onKey));
</script>

<template>
  <div class="plt no-print">
    <div class="plt-head">
      <i class="pi pi-pencil" />
      <span class="plt-title">{{ name }}</span>
    </div>

    <div class="plt-row">
      <label class="plt-check">
        <input type="checkbox" v-model="guides" /> {{ t('printLayouts.guides') }}
      </label>
      <span class="plt-dirty" v-if="lay.dirtyCount.value">{{ t('printLayouts.changed', { n: lay.dirtyCount.value }) }}</span>
    </div>

    <div class="plt-field" v-if="lay.selectedKey.value">
      <div class="plt-field-key"><i class="pi pi-arrows-alt" /> {{ lay.selectedKey.value }}</div>
      <div class="plt-field-pos" v-if="lay.selectedPos.value">
        x {{ lay.selectedPos.value.x }} · y {{ lay.selectedPos.value.y }} · {{ lay.selectedPos.value.size }}pt
      </div>
      <div class="plt-field-ctl">
        <Button icon="pi pi-minus" size="small" text severity="secondary" v-tooltip.bottom="t('printLayouts.fontSmaller')" @click="lay.bumpSize(-0.5)" />
        <span class="plt-font">A</span>
        <Button icon="pi pi-plus" size="small" text severity="secondary" v-tooltip.bottom="t('printLayouts.fontBigger')" @click="lay.bumpSize(0.5)" />
        <Button icon="pi pi-replay" size="small" text severity="secondary" v-tooltip.bottom="t('printLayouts.resetField')" @click="lay.resetField()" />
      </div>
    </div>
    <div class="plt-hint" v-else>{{ t('printLayouts.pickHint') }}</div>

    <div class="plt-actions">
      <Button :label="t('common.back')" icon="pi pi-arrow-left" size="small" severity="secondary" outlined @click="done" />
      <Button :label="t('printLayouts.resetAll')" icon="pi pi-trash" size="small" severity="danger" outlined @click="onResetAll" />
      <Button :label="t('common.save')" icon="pi pi-save" size="small" :loading="lay.saving.value" @click="onSave" />
    </div>
    <div class="plt-keys">{{ t('printLayouts.keysHint') }}</div>
  </div>
</template>

<style scoped>
.plt {
  position: fixed; top: 12px; right: 12px; z-index: 9999;
  width: 250px; background: #fff; color: #111;
  border: 1px solid #cbd5e1; border-radius: 10px;
  box-shadow: 0 8px 30px rgba(0,0,0,.25);
  padding: .6rem .7rem; font-size: .78rem;
  display: flex; flex-direction: column; gap: .5rem;
}
.plt-head { display: flex; align-items: center; gap: .4rem; font-weight: 700; }
.plt-title { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.plt-head .pi { color: #2563eb; }
.plt-row { display: flex; align-items: center; justify-content: space-between; }
.plt-check { display: flex; align-items: center; gap: .35rem; cursor: pointer; }
.plt-dirty { color: #b45309; font-weight: 600; }
.plt-field { background: #f1f5f9; border-radius: 8px; padding: .4rem .5rem; display: flex; flex-direction: column; gap: .3rem; }
.plt-field-key { font-family: ui-monospace, monospace; font-weight: 700; display: flex; align-items: center; gap: .35rem; }
.plt-field-pos { font-family: ui-monospace, monospace; color: #475569; font-size: .72rem; }
.plt-field-ctl { display: flex; align-items: center; gap: .15rem; }
.plt-font { font-weight: 700; }
.plt-hint { color: #64748b; font-size: .72rem; }
.plt-actions { display: flex; gap: .35rem; justify-content: flex-end; flex-wrap: wrap; }
.plt-keys { color: #94a3b8; font-size: .68rem; line-height: 1.3; }
</style>
