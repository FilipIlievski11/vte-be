// Tiny keyboard-shortcut composable. Wires onMounted/onUnmounted listeners.
//
//   useShortcuts({
//     'F2':       () => save(),
//     'Ctrl+S':   () => save(),
//     'Escape':   () => router.back(),
//     'F5':       () => load(),
//     'Ctrl+F':   () => searchRef.value?.focus(),
//   })
//
// Combos: prefix with `Ctrl+`, `Shift+`, `Alt+`. Letter casing is normalised.
// Shortcuts that match a typed key are pre-empted (preventDefault).
import { onBeforeUnmount, onMounted } from 'vue'

type Handler = (ev: KeyboardEvent) => void

function normalise(ev: KeyboardEvent): string {
  const parts: string[] = []
  if (ev.ctrlKey || ev.metaKey) parts.push('Ctrl')
  if (ev.shiftKey)              parts.push('Shift')
  if (ev.altKey)                parts.push('Alt')
  let key = ev.key
  if (key.length === 1) key = key.toUpperCase()
  parts.push(key)
  return parts.join('+')
}

export function useShortcuts(map: Record<string, Handler>) {
  const lookup: Record<string, Handler> = {}
  for (const k in map) lookup[k] = map[k]

  function onKey(ev: KeyboardEvent) {
    const tag = (ev.target as HTMLElement | null)?.tagName ?? ''
    const isInput = tag === 'INPUT' || tag === 'TEXTAREA' || (ev.target as HTMLElement | null)?.isContentEditable
    const combo = normalise(ev)
    const handler = lookup[combo]
    if (!handler) return
    // Allow F1-F12 and Esc inside inputs; gate single letters & Ctrl shortcuts more strictly.
    const isFunctionOrSpecial = /^F\d+$/.test(ev.key) || ev.key === 'Escape'
    if (isInput && !isFunctionOrSpecial && !ev.ctrlKey && !ev.metaKey) return
    ev.preventDefault()
    handler(ev)
  }

  onMounted(() => window.addEventListener('keydown', onKey))
  onBeforeUnmount(() => window.removeEventListener('keydown', onKey))
}
