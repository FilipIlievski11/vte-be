import api from '@/api/client'

// Fetches a PDF (or any blob) with the auth header attached, opens it in a new
// tab via an object URL, and revokes the URL once that tab is closed.
// window.open(url, '_blank') alone cannot send Authorization, so a protected
// /api endpoint would 401 and show a blank tab.
export async function openAuthorizedPdf(url: string): Promise<void> {
  const { data, headers } = await api.get(url, { responseType: 'blob' })
  const type = (headers as any)?.['content-type'] ?? 'application/pdf'
  const blob = new Blob([data], { type })
  const objectUrl = URL.createObjectURL(blob)
  const win = window.open(objectUrl, '_blank')
  // Revoke once the new tab unloads (a few minutes later in Safari) — safe fallback after 60s.
  setTimeout(() => URL.revokeObjectURL(objectUrl), 60_000)
  if (!win) {
    // Pop-up blocked — fall back to a download.
    const a = document.createElement('a')
    a.href = objectUrl
    a.download = url.split('/').pop() ?? 'document.pdf'
    document.body.appendChild(a)
    a.click()
    a.remove()
  }
}
