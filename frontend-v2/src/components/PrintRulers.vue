<script setup lang="ts">
// Линијари (cm броеви + mm цртички) на горниот и левиот раб на печатната страница,
// прикажани само во layout-edit режим. CSS mm единиците се исти со координатите на
// полињата, па линијарот секогаш се совпаѓа со x/y вредностите во toolbar-от.
// Кога има селектирано поле (pos), се црта crosshair до линијарите + сини маркери
// со точната mm вредност — за лесно порамнување.
import { computed } from 'vue';

const props = defineProps<{
  widthMm: number;
  heightMm: number;
  pos?: { x: number; y: number } | null;
}>();

const hNums = computed(() => Array.from({ length: Math.floor(props.widthMm / 10) }, (_, i) => i + 1));
const vNums = computed(() => Array.from({ length: Math.floor(props.heightMm / 10) }, (_, i) => i + 1));
const p = computed(() => props.pos ?? null);
</script>

<template>
  <div class="ruler ruler-h no-print">
    <span v-for="n in hNums" :key="'h' + n" class="num" :style="{ left: n * 10 + 'mm' }">{{ n }}</span>
    <template v-if="p">
      <span class="mark mark-h" :style="{ left: p.x + 'mm' }" />
      <span class="mark-label" :style="{ left: p.x + 'mm' }">{{ p.x.toFixed(1) }}</span>
    </template>
  </div>
  <div class="ruler ruler-v no-print">
    <span v-for="n in vNums" :key="'v' + n" class="num num-v" :style="{ top: n * 10 + 'mm' }">{{ n }}</span>
    <template v-if="p">
      <span class="mark mark-v" :style="{ top: p.y + 'mm' }" />
      <span class="mark-label mark-label-v" :style="{ top: p.y + 'mm' }">{{ p.y.toFixed(1) }}</span>
    </template>
  </div>
  <!-- Водилки од селектираното поле до линијарите -->
  <template v-if="p">
    <div class="cross cross-v no-print" :style="{ left: p.x + 'mm' }" />
    <div class="cross cross-h no-print" :style="{ top: p.y + 'mm' }" />
  </template>
</template>

<style scoped>
.ruler {
  position: absolute;
  z-index: 60;
  pointer-events: none;
  background-color: rgba(248, 250, 252, .92);
  font-family: system-ui, sans-serif;
}
/* Горен линијар: cm цртичка (цела висина), 5mm (60%), 1mm (35%) — сите закачени долу,
   кон хартијата, како вистински линијар. */
.ruler-h {
  left: 0; top: 0; right: 0; height: 4mm;
  border-bottom: 1px solid #64748b;
  background-image:
    repeating-linear-gradient(90deg, #475569 0 0.25mm, transparent 0.25mm 10mm),
    repeating-linear-gradient(90deg, #94a3b8 0 0.2mm,  transparent 0.2mm 5mm),
    repeating-linear-gradient(90deg, #cbd5e1 0 0.15mm, transparent 0.15mm 1mm);
  background-size: 100% 100%, 100% 55%, 100% 30%;
  background-position: bottom, bottom, bottom;
  background-repeat: no-repeat;
}
.ruler-v {
  left: 0; top: 0; bottom: 0; width: 4mm;
  border-right: 1px solid #64748b;
  background-image:
    repeating-linear-gradient(180deg, #475569 0 0.25mm, transparent 0.25mm 10mm),
    repeating-linear-gradient(180deg, #94a3b8 0 0.2mm,  transparent 0.2mm 5mm),
    repeating-linear-gradient(180deg, #cbd5e1 0 0.15mm, transparent 0.15mm 1mm);
  background-size: 100% 100%, 55% 100%, 30% 100%;
  background-position: right, right, right;
  background-repeat: no-repeat;
}
.num {
  position: absolute; top: 0;
  transform: translateX(-100%) translateX(-0.4mm);
  font-size: 5.5px; line-height: 1; color: #334155;
}
.num-v {
  left: 0.2mm; top: auto;
  transform: translateY(-100%) translateY(-0.3mm);
}

/* Маркер на линијарот за селектираното поле + точна mm вредност */
.mark { position: absolute; background: #2563eb; }
.mark-h { top: 0; bottom: 0; width: 1px; transform: translateX(-0.5px); }
.mark-v { left: 0; right: 0; height: 1px; transform: translateY(-0.5px); }
.mark-label {
  position: absolute; top: -1px;
  transform: translateX(2px);
  font-size: 6.5px; line-height: 1; font-weight: 700; color: #2563eb;
  background: rgba(255,255,255,.9); padding: 0 1px; border-radius: 2px;
  white-space: nowrap;
}
.mark-label-v {
  top: auto; left: 0;
  transform: translateY(2px);
}

/* Водилки низ страницата од селектираното поле до двата линијара */
.cross {
  position: absolute; z-index: 55; pointer-events: none;
}
.cross-v { top: 0; bottom: 0; width: 0; border-left: 1px dashed rgba(37, 99, 235, .45); }
.cross-h { left: 0; right: 0; height: 0; border-top: 1px dashed rgba(37, 99, 235, .45); }
</style>
