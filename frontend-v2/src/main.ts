import { createApp } from 'vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import ToastService from 'primevue/toastservice';
import ConfirmationService from 'primevue/confirmationservice';
import Tooltip from 'primevue/tooltip';
import 'primeicons/primeicons.css';
import './styles.css';

import App from './App.vue';
import { router } from './router';
import { i18n } from './locales';

// After a redeploy the hashed chunk names change; a tab still running the old bundle
// 404s when it lazy-loads a route it hasn't visited yet. Vite fires `vite:preloadError`
// for exactly this — reload once to pick up the fresh index.html (guarded so a genuinely
// broken deploy can't put the tab in an endless reload loop).
window.addEventListener('vite:preloadError', (e) => {
  e.preventDefault(); // don't also surface the rejection in the console
  const key = 'vte.v2.chunkReloadAt';
  const last = Number(sessionStorage.getItem(key) ?? 0);
  if (Date.now() - last > 30_000) {
    sessionStorage.setItem(key, String(Date.now()));
    window.location.reload();
  }
});

const app = createApp(App);
app.use(createPinia());
app.use(router);
app.use(i18n);
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      // The app toggles `html.app-dark` (AppLayout), so scope PrimeVue's dark palette
      // to that class — otherwise NONE of PrimeVue's --p-* tokens flip and every overlay
      // (dropdowns, dialogs, menus, datepickers…) renders light in dark mode. The custom
      // --p-* overrides in styles.css are more specific (html.app-dark) and still win for
      // the families they tune; this fills in everything else.
      darkModeSelector: '.app-dark',
    },
  },
  ripple: false,
});
app.use(ToastService);
app.use(ConfirmationService);
app.directive('tooltip', Tooltip);

app.mount('#app');
