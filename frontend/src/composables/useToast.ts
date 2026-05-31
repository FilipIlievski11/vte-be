// Wraps PrimeVue's useToast / useConfirm into nicer helpers.
// Replaces window.alert() and window.confirm() across the app.
import { useToast as usePrimeToast } from 'primevue/usetoast'
import { useConfirm } from 'primevue/useconfirm'

export interface ToastApi {
  ok: (msg: string, summary?: string) => void
  info: (msg: string, summary?: string) => void
  warn: (msg: string, summary?: string) => void
  error: (msg: string, summary?: string) => void
}

export function useToast(): ToastApi {
  const t = usePrimeToast()
  return {
    ok:    (msg, summary) => t.add({ severity: 'success', summary: summary ?? 'OK',     detail: msg, life: 3000 }),
    info:  (msg, summary) => t.add({ severity: 'info',    summary: summary ?? 'Info',   detail: msg, life: 3500 }),
    warn:  (msg, summary) => t.add({ severity: 'warn',    summary: summary ?? 'Внимание', detail: msg, life: 4500 }),
    error: (msg, summary) => t.add({ severity: 'error',   summary: summary ?? 'Грешка', detail: msg, life: 6000 }),
  }
}

export function useConfirmDialog() {
  const c = useConfirm()
  return {
    ask(opts: { message: string; header?: string; acceptLabel?: string; rejectLabel?: string; danger?: boolean; onAccept: () => void; onReject?: () => void }) {
      c.require({
        message: opts.message,
        header:  opts.header ?? 'Потврда',
        icon:    opts.danger ? 'pi pi-exclamation-triangle' : 'pi pi-question-circle',
        acceptClass: opts.danger ? 'p-button-danger' : undefined,
        acceptLabel: opts.acceptLabel ?? 'Да',
        rejectLabel: opts.rejectLabel ?? 'Откажи',
        accept: opts.onAccept,
        reject: opts.onReject,
      })
    },
  }
}
