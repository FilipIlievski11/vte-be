<script setup lang="ts">
import { computed, onMounted, reactive, ref, watch } from 'vue';
import { RouterLink, RouterView, useRoute, useRouter } from 'vue-router';
import { useI18n } from 'vue-i18n';
import { useAuthStore } from '@/stores/auth';
import { api } from '@/api/client';
import { setLocale, type Locale } from '@/locales';
import Button from 'primevue/button';
import SelectButton from 'primevue/selectbutton';

const auth = useAuthStore();
const router = useRouter();
const route = useRoute();
const { t, locale } = useI18n();

interface MenuItem  { to: string; labelKey: string }
interface MenuGroup { id: string; labelKey: string; icon: string; items: MenuItem[]; admin?: boolean }

const groups: MenuGroup[] = [
  {
    id: 'admin', labelKey: 'nav.administration', icon: 'pi pi-shield', admin: true,
    items: [
      { to: '/companies',                labelKey: 'admin.companies' },
      { to: '/stations',                 labelKey: 'admin.stations' },
      { to: '/operators',                labelKey: 'admin.operators' },
      { to: '/ref/countries',            labelKey: 'admin.countries' },
      { to: '/ref/communities',          labelKey: 'admin.communities' },
      { to: '/ref/cities',               labelKey: 'admin.cities' },
      { to: '/ref/citizenships',         labelKey: 'admin.citizenships' },
      { to: '/ref/document-issuers',     labelKey: 'admin.documentIssuers' },
      { to: '/ref/personal-data-types',  labelKey: 'admin.personalDataTypes' },
      { to: '/request-types',                       labelKey: 'admin.requestTypes' },
      { to: '/ref/request-document-prints',         labelKey: 'admin.requestDocumentPrints' },
      { to: '/ref/request-ownership-proof-types',   labelKey: 'admin.requestOwnershipProofTypes' },
      { to: '/ref/request-payment-proof-types',     labelKey: 'admin.requestPaymentProofTypes' },
      { to: '/ref/request-attachment-types',        labelKey: 'admin.requestAttachmentTypes' },
      { to: '/prices',                              labelKey: 'admin.prices' },
      { to: '/billing-categories',                  labelKey: 'admin.billingCategories' },
      { to: '/print-templates',                     labelKey: 'admin.printTemplates' },
      { to: '/legacy-sync',                         labelKey: 'admin.legacySync' },
      { to: '/audit',                               labelKey: 'admin.audit' },
    ],
  },
];

const visibleGroups = computed(() => groups.filter(g => !g.admin || auth.isAdmin));

const STORAGE = 'vte.v2.menu.collapsed';
const collapsed = reactive<Record<string, boolean>>(JSON.parse(localStorage.getItem(STORAGE) || '{}'));

function isCollapsed(g: MenuGroup) {
  if (g.id in collapsed) return collapsed[g.id];
  return !g.items.some(i => route.path.startsWith(i.to.split('?')[0]));
}
function toggle(g: MenuGroup) {
  collapsed[g.id] = !isCollapsed(g);
  localStorage.setItem(STORAGE, JSON.stringify(collapsed));
}
function groupHasActive(g: MenuGroup): boolean {
  return g.items.some(i => {
    const p = i.to.split('?')[0];
    return route.path === p || (p !== '/' && route.path.startsWith(p + '/'));
  });
}

// Dark mode
const darkMode = ref<boolean>(localStorage.getItem('vte.v2.theme') === 'dark');
function applyTheme() {
  document.documentElement.classList.toggle('app-dark', darkMode.value);
  localStorage.setItem('vte.v2.theme', darkMode.value ? 'dark' : 'light');
}
onMounted(applyTheme);
watch(darkMode, applyTheme);

// Sidebar collapse
// Global "back" — operators kept getting stranded on detail/form pages.
const canGoBack = computed(() => route.path !== '/' && route.path !== '/dashboard');
function goBack() {
  if (window.history.length > 1) router.back();
  else router.push('/dashboard');
}

const sidebarCollapsed = ref<boolean>(localStorage.getItem('vte.v2.sidebar') === 'collapsed');
watch(sidebarCollapsed, v => { localStorage.setItem('vte.v2.sidebar', v ? 'collapsed' : 'open'); });

// Mobile: the sidebar becomes an overlay drawer (hidden by default, hamburger
// opens it, backdrop/navigation closes it). isMobile via matchMedia so the same
// hamburger drives collapse on desktop and the drawer on phones.
const mq = window.matchMedia('(max-width: 768px)');
const isMobile = ref(mq.matches);
mq.addEventListener('change', e => { isMobile.value = e.matches; if (!e.matches) mobileNavOpen.value = false; });
const mobileNavOpen = ref(false);
function toggleSidebar() {
  if (isMobile.value) mobileNavOpen.value = !mobileNavOpen.value;
  else sidebarCollapsed.value = !sidebarCollapsed.value;
}
watch(() => route.fullPath, () => { mobileNavOpen.value = false; });

// Language toggle
const lang = ref<Locale>(locale.value as Locale);
watch(lang, v => { setLocale(v); locale.value = v; });
const langOptions = [
  { label: 'MK', value: 'mk' as Locale },
  { label: 'EN', value: 'en' as Locale },
];

const initials = computed(() => (auth.userName || 'A').slice(0, 2).toUpperCase());

const pageTitle = computed(() => {
  const path = route.path;
  if (path === '/' || path === '/dashboard') return t('nav.dashboard');
  if (path.startsWith('/clients')) return t('nav.clients');
  if (path.startsWith('/vehicles')) return t('nav.vehicles');
  if (path.startsWith('/requests')) return t('nav.requests');
  if (path.startsWith('/technical-exams')) return t('nav.technicalExams');
  if (path.startsWith('/international-driving-licences')) return t('nav.internationalDrivingLicences');
  if (path.startsWith('/vehicle-permissions')) return t('nav.vehiclePermissions');
  if (path.startsWith('/payments')) return t('nav.payments');
  if (path.startsWith('/fiscal')) return t('nav.fiscal');
  if (path.startsWith('/reports')) return t('nav.reports');
  if (path.startsWith('/billing-categories')) return `${t('nav.administration')} · ${t('admin.billingCategories')}`;
  if (path.startsWith('/print-templates')) return `${t('nav.administration')} · ${t('admin.printTemplates')}`;
  if (path.startsWith('/companies')) return `${t('nav.administration')} · ${t('admin.companies')}`;
  if (path.startsWith('/stations')) return `${t('nav.administration')} · ${t('admin.stations')}`;
  if (path.startsWith('/operators')) return `${t('nav.administration')} · ${t('admin.operators')}`;
  if (path.startsWith('/request-types')) return `${t('nav.administration')} · ${t('admin.requestTypes')}`;
  if (path.startsWith('/ref/')) {
    const kind = path.replace('/ref/', '');
    const titleKey = `ref.titles.${kind}`;
    const localised = t(titleKey);
    const fallback = localised === titleKey ? kind : localised;
    return `${t('nav.administration')} · ${fallback}`;
  }
  return '';
});

async function logout() {
  try { await api.post('/auth/logout'); } catch { /* ignore */ }
  auth.clear();
  router.push('/login');
}
</script>

<template>
  <div class="app-shell" :class="{ 'shell-collapsed': sidebarCollapsed, 'mobile-nav-open': mobileNavOpen }">
    <div class="mobile-backdrop" v-if="mobileNavOpen" @click="mobileNavOpen = false"></div>
    <aside class="app-sidebar">
      <div class="brand">
        <div class="brand-mark"><i class="pi pi-car" /></div>
        <div class="brand-text">
          <strong>{{ t('app.navigation') }}</strong>
        </div>
      </div>

      <div class="nav-scroll">
        <div class="nav-section">
          <ul class="nav">
            <li>
              <RouterLink to="/dashboard" class="topnav">
                <i class="pi pi-th-large" /><span>{{ t('nav.dashboard') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/clients" class="topnav">
                <i class="pi pi-users" /><span>{{ t('nav.clients') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/vehicles" class="topnav">
                <i class="pi pi-car" /><span>{{ t('nav.vehicles') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/requests" class="topnav">
                <i class="pi pi-file-edit" /><span>{{ t('nav.requests') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/technical-exams" class="topnav">
                <i class="pi pi-clipboard" /><span>{{ t('nav.technicalExams') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/international-driving-licences" class="topnav">
                <i class="pi pi-id-card" /><span>{{ t('nav.internationalDrivingLicences') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/vehicle-permissions" class="topnav">
                <i class="pi pi-file-check" /><span>{{ t('nav.vehiclePermissions') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/payments" class="topnav">
                <i class="pi pi-wallet" /><span>{{ t('nav.payments') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/fiscal" class="topnav">
                <i class="pi pi-print" /><span>{{ t('nav.fiscal') }}</span>
              </RouterLink>
            </li>
            <li>
              <RouterLink to="/reports" class="topnav">
                <i class="pi pi-chart-bar" /><span>{{ t('nav.reports') }}</span>
              </RouterLink>
            </li>
          </ul>
        </div>

        <div
          v-for="g in visibleGroups"
          :key="g.id"
          class="nav-section"
          :class="{ 'has-active': groupHasActive(g) }"
        >
          <button class="section-toggle" @click="toggle(g)">
            <i :class="g.icon" />
            <span>{{ t(g.labelKey) }}</span>
            <i class="pi pi-chevron-down chevron" :class="{ collapsed: isCollapsed(g) }" />
          </button>
          <ul class="nav nested" v-show="!isCollapsed(g)">
            <li v-for="i in g.items" :key="i.to">
              <RouterLink :to="i.to"><span>{{ t(i.labelKey) }}</span></RouterLink>
            </li>
          </ul>
        </div>
      </div>

      <div class="footer">{{ new Date().getFullYear() }} · SMFSolution</div>
    </aside>

    <header class="app-topbar">
      <div class="left">
        <Button
          severity="secondary" text rounded size="small"
          :icon="isMobile ? 'pi pi-bars' : (sidebarCollapsed ? 'pi pi-bars' : 'pi pi-angle-double-left')"
          @click="toggleSidebar"
          v-tooltip.bottom="sidebarCollapsed ? t('app.showSidebar') : t('app.hideSidebar')"
        />
        <Button
          v-if="canGoBack"
          class="back-btn"
          severity="secondary" outlined size="small"
          icon="pi pi-arrow-left" :label="t('app.back')"
          @click="goBack"
          v-tooltip.bottom="t('app.back')"
        />
        <span class="page-title">{{ pageTitle }}</span>
        <span class="crumb" v-if="auth.companyId !== null">
          · {{ auth.companyName ?? `${t('app.companyShort')} #${auth.companyId}` }}
        </span>
        <span class="crumb" v-else-if="auth.isAdmin">· {{ t('app.crossTenant') }}</span>
      </div>
      <div class="right">
        <Button
          severity="secondary" outlined rounded size="small"
          :icon="darkMode ? 'pi pi-sun' : 'pi pi-moon'"
          @click="darkMode = !darkMode"
          v-tooltip.bottom="darkMode ? t('app.lightMode') : t('app.darkMode')"
        />
        <SelectButton
          v-model="lang"
          :options="langOptions"
          optionLabel="label"
          optionValue="value"
          :allowEmpty="false"
          size="small"
        />
        <RouterLink to="/account" class="user-pill" v-tooltip.bottom="t('app.accountSettings')">
          <div class="user-avatar">{{ initials }}</div>
          <div class="user-meta">
            <strong>{{ auth.fullName || auth.userName }}</strong>
            <span>{{ auth.roles.join(', ') }}</span>
          </div>
        </RouterLink>
        <Button severity="secondary" :label="t('app.signOut')" icon="pi pi-sign-out" outlined size="small" class="logout-btn" @click="logout" />
      </div>
    </header>

    <main class="app-content"><RouterView /></main>
  </div>
</template>
