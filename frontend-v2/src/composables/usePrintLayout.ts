// Shared print-template layout system.
//
// Every coordinate-based print template registers its DEFAULT field positions (in the
// template's own unit — millimetres for all current forms) keyed by a STABLE field key.
// This composable fetches the admin-saved override map for the template `code`, merges it
// onto the defaults, and (in edit mode) provides drag / keyboard-nudge / font-size editing
// plus save/reset against the backend `PrintLayout` store.
//
// Only the DIFF vs the defaults is persisted, so a field that was never moved is absent from
// the saved JSON and keeps following the built-in default forever.
import { computed, ref } from 'vue';
import { useRoute } from 'vue-router';
import { api } from '@/api/client';

export interface LayoutOverride { x?: number; y?: number; size?: number }
export type LayoutOverrides = Record<string, LayoutOverride>;
/** A field's built-in default position (template unit = mm). size is font pt. */
export interface PosDefault { x: number; y: number; size?: number }
export interface ResolvedPos { x: number; y: number; size: number }

function round1(n: number): number { return Math.round(n * 10) / 10; }

export function usePrintLayout(code: string, pageWidthUnits: number) {
  const route = useRoute();
  // Edit mode is entered by the admin editor via ?edit=1 (or set programmatically).
  const editing = ref(route.query.edit === '1' || route.query.edit === 'true');

  const overrides = ref<LayoutOverrides>({});
  const defaults = ref<Record<string, PosDefault>>({});
  const selectedKey = ref<string | null>(null);
  const saving = ref(false);
  const loaded = ref(false);

  let pageEl: HTMLElement | null = null;
  function setPageEl(el: HTMLElement | null) { pageEl = el; }

  /** Template hands over its full default map once (key → {x,y,size}). */
  function setDefaults(map: Record<string, PosDefault>) { defaults.value = map; }

  async function load() {
    try {
      const { data } = await api.get<{ layoutJson: string }>(`/print-layouts/${code}`);
      overrides.value = data?.layoutJson ? (JSON.parse(data.layoutJson) as LayoutOverrides) : {};
    } catch { overrides.value = {}; }
    finally { loaded.value = true; }
  }

  function resolve(key: string, def?: PosDefault): ResolvedPos {
    const d = def ?? defaults.value[key] ?? { x: 0, y: 0 };
    const o = overrides.value[key];
    return { x: o?.x ?? d.x, y: o?.y ?? d.y, size: o?.size ?? d.size ?? 0 };
  }

  // ---- edit interactions ----
  function unitsPerPx(): number {
    if (pageEl) {
      const r = pageEl.getBoundingClientRect();
      if (r.width > 0) return pageWidthUnits / r.width;
    }
    return 25.4 / 96; // CSS mm ≈ 3.7795px fallback
  }

  function patch(key: string, p: LayoutOverride) {
    const next = { ...overrides.value };
    next[key] = { ...next[key], ...p };
    overrides.value = next;
  }

  let dragKey: string | null = null;
  let sx = 0, sy = 0, bx = 0, by = 0;
  function beginDrag(key: string, ev: PointerEvent) {
    if (!editing.value) return;
    ev.preventDefault();
    ev.stopPropagation();
    selectedKey.value = key;
    dragKey = key;
    const cur = resolve(key);
    sx = ev.clientX; sy = ev.clientY; bx = cur.x; by = cur.y;
    window.addEventListener('pointermove', onMove);
    window.addEventListener('pointerup', onUp);
  }
  function onMove(ev: PointerEvent) {
    if (!dragKey) return;
    const k = unitsPerPx();
    patch(dragKey, { x: round1(bx + (ev.clientX - sx) * k), y: round1(by + (ev.clientY - sy) * k) });
  }
  function onUp() {
    dragKey = null;
    window.removeEventListener('pointermove', onMove);
    window.removeEventListener('pointerup', onUp);
  }

  /** Arrow-key nudge of the selected field (units = mm). */
  function nudge(dx: number, dy: number) {
    const key = selectedKey.value;
    if (!key) return;
    const cur = resolve(key);
    patch(key, { x: round1(cur.x + dx), y: round1(cur.y + dy) });
  }
  function bumpSize(delta: number) {
    const key = selectedKey.value;
    if (!key) return;
    const cur = resolve(key);
    patch(key, { size: Math.max(4, round1(cur.size + delta)) });
  }
  function resetField(key: string | null = selectedKey.value) {
    if (!key) return;
    const next = { ...overrides.value };
    delete next[key];
    overrides.value = next;
  }

  const dirtyCount = computed(() => Object.keys(overrides.value).length);
  const selectedPos = computed<ResolvedPos | null>(() =>
    selectedKey.value ? resolve(selectedKey.value) : null);

  /** Persist only the fields that actually differ from their default — but store each
   *  touched field as a COMPLETE absolute {x,y,size}. Print views then position the
   *  field purely from the saved entry, independent of the built-in (or runtime-measured)
   *  defaults — critical for flow templates whose defaults are measured in the editor. */
  async function save(name: string): Promise<boolean> {
    saving.value = true;
    try {
      const pruned: LayoutOverrides = {};
      for (const [key, o] of Object.entries(overrides.value)) {
        const d = defaults.value[key] ?? { x: 0, y: 0 };
        const rx = o.x ?? d.x, ry = o.y ?? d.y, rs = o.size ?? d.size;
        const changed = rx !== d.x || ry !== d.y || (rs ?? 0) !== (d.size ?? 0);
        if (!changed) continue;
        const e: LayoutOverride = { x: rx, y: ry };
        if (rs != null && rs > 0) e.size = rs;
        pruned[key] = e;
      }
      await api.put(`/print-layouts/${code}`, { name, layoutJson: JSON.stringify(pruned) });
      overrides.value = pruned;
      return true;
    } catch { return false; }
    finally { saving.value = false; }
  }

  /** Reset the whole template to built-in defaults. */
  async function resetAll(): Promise<boolean> {
    saving.value = true;
    try {
      await api.delete(`/print-layouts/${code}`);
      overrides.value = {};
      return true;
    } catch { return false; }
    finally { saving.value = false; }
  }

  return {
    code, editing, loaded, overrides, selectedKey, selectedPos, saving, dirtyCount,
    setPageEl, setDefaults, load, resolve,
    beginDrag, nudge, bumpSize, resetField, save, resetAll,
  };
}
export type PrintLayoutApi = ReturnType<typeof usePrintLayout>;
