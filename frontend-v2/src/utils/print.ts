/**
 * Open a print/document route in a new browser tab.
 *
 * Uses `noopener` deliberately: without it, a script-opened same-origin tab keeps an
 * opener link to the main app and Chrome puts BOTH in the same renderer process. The
 * print tab's blocking `window.print()` modal then freezes that shared process — so the
 * main app tab appears frozen while the print dialog is open. `noopener` severs the link,
 * landing the print tab in its own process. (`window.close()` still works on a
 * script-opened tab, so the print views' "close" button is unaffected.)
 */
export function openPrintTab(href: string): void {
  window.open(href, '_blank', 'noopener');
}
