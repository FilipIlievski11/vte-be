import { createApp } from 'vue';
import { createPinia } from 'pinia';
import PrimeVue from 'primevue/config';
import Aura from '@primeuix/themes/aura';
import ToastService from 'primevue/toastservice';
import ConfirmationService from 'primevue/confirmationservice';
import 'primeicons/primeicons.css';
import './styles.css';

import App from './App.vue';
import { router } from './router';
import { i18n } from './locales';

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

app.mount('#app');
