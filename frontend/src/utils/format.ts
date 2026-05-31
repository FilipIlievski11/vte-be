// Shared formatting helpers — keep one source of truth so every view shows
// numbers and dates the same way.

export function formatMoney(n: number | null | undefined, suffix = ' ден.'): string {
  if (n == null || isNaN(n)) return '—'
  return n.toLocaleString('mk-MK', { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + suffix
}

export function formatNumber(n: number | null | undefined, fractionDigits = 0): string {
  if (n == null || isNaN(n)) return '—'
  return n.toLocaleString('mk-MK', { minimumFractionDigits: fractionDigits, maximumFractionDigits: fractionDigits })
}

// "4.5.2026" — short date, locale-friendly without zero-padding (matches legacy WinForms output)
export function formatShortDate(iso: string | Date | null | undefined): string {
  if (!iso) return ''
  const d = iso instanceof Date ? iso : new Date(iso)
  if (isNaN(d.getTime())) return ''
  return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}`
}

// "4.5.2026, 13:42" — date + minute. For activity timestamps.
export function formatDateTime(iso: string | Date | null | undefined): string {
  if (!iso) return ''
  const d = iso instanceof Date ? iso : new Date(iso)
  if (isNaN(d.getTime())) return ''
  const hh = String(d.getHours()).padStart(2, '0')
  const mm = String(d.getMinutes()).padStart(2, '0')
  return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}, ${hh}:${mm}`
}

// "in 14 days" / "5 days ago" — friendly relative time
export function formatRelative(iso: string | Date | null | undefined): string {
  if (!iso) return ''
  const d = iso instanceof Date ? iso : new Date(iso)
  if (isNaN(d.getTime())) return ''
  const ms = d.getTime() - Date.now()
  const days = Math.round(ms / 86_400_000)
  if (days === 0) return 'денес'
  if (days === 1) return 'утре'
  if (days === -1) return 'вчера'
  if (days > 0) return `за ${days} дена`
  return `пред ${-days} дена`
}
