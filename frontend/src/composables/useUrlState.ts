// Persists list-view state (page, pageSize, sortBy, sortDir, q, ...) into the
// URL query string so the browser back-button restores the same filter, and
// links can be shared.
//
//   const state = useUrlState({ page: 1, pageSize: 50, q: '' }, { onChange: load })
//   state.page.value = 5  → router pushes /customers?page=5
import { ref, watch, type Ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'

type Json = string | number | boolean | null

export function useUrlState<T extends Record<string, Json>>(
  defaults: T,
  opts: { onChange?: () => void } = {},
) {
  const route = useRoute()
  const router = useRouter()

  // Hydrate from URL or fall back to defaults.
  const refs = {} as { [K in keyof T]: Ref<T[K]> }
  for (const k of Object.keys(defaults) as (keyof T)[]) {
    const fromUrl = route.query[k as string]
    let val: any = defaults[k]
    if (typeof fromUrl === 'string') {
      if (typeof defaults[k] === 'number')      val = Number(fromUrl) || defaults[k]
      else if (typeof defaults[k] === 'boolean') val = fromUrl === 'true'
      else                                       val = fromUrl
    }
    refs[k] = ref(val) as Ref<T[K]>
  }

  // Push a debounced URL update when any ref changes.
  let pushTimer: any
  function push() {
    clearTimeout(pushTimer)
    pushTimer = setTimeout(() => {
      const q: Record<string, string> = { ...(route.query as Record<string, string>) }
      for (const k of Object.keys(refs) as (keyof T)[]) {
        const v = refs[k].value as any
        if (v == null || v === '' || v === defaults[k]) delete q[k as string]
        else q[k as string] = String(v)
      }
      router.replace({ query: q }).then(() => opts.onChange?.()).catch(() => {})
    }, 50)
  }
  for (const k of Object.keys(refs) as (keyof T)[]) watch(refs[k], push)

  return refs
}
