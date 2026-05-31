<script setup lang="ts">
import { useAuthStore } from '@/stores/auth'
import { useRouter, useRoute } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { setLocale, type Locale } from '@/locales'
import Button from 'primevue/button'
import SelectButton from 'primevue/selectbutton'
import { ref, watch, computed, reactive, onMounted } from 'vue'

const auth = useAuthStore()
const router = useRouter()
const route = useRoute()
const { t, locale } = useI18n()

const lang = ref<Locale>(locale.value as Locale)
watch(lang, (v) => { setLocale(v); locale.value = v })
const langOptions = [{ label: 'MK', value: 'mk' as Locale }, { label: 'EN', value: 'en' as Locale }]

function logout() { auth.logout(); router.push('/login') }

// Dark-mode toggle. Sets `app-dark` class on <html>, persisted to localStorage.
const darkMode = ref<boolean>(localStorage.getItem('vte.theme') === 'dark')
function applyTheme() {
  document.documentElement.classList.toggle('app-dark', darkMode.value)
  localStorage.setItem('vte.theme', darkMode.value ? 'dark' : 'light')
}
onMounted(applyTheme)
watch(darkMode, applyTheme)

// Sidebar collapse — persisted so the user's preference survives reloads.
const sidebarCollapsed = ref<boolean>(localStorage.getItem('vte.sidebar') === 'collapsed')
watch(sidebarCollapsed, (v) => { localStorage.setItem('vte.sidebar', v ? 'collapsed' : 'open') })
const initials = computed(() => (auth.userName || 'A').slice(0, 2).toUpperCase())

interface MenuItem  { to: string; labelKey: string; admin?: boolean }
interface MenuGroup { id: string; labelKey: string; icon: string; items: MenuItem[]; admin?: boolean }

// Mirrors the legacy DevExpress navigation panel exactly.
const groups: MenuGroup[] = [
  {
    id: 'customers', labelKey: 'menu.customers', icon: 'pi pi-users',
    items: [
      { to: '/customers/new',   labelKey: 'menu.new' },
      { to: '/customers',       labelKey: 'menu.showCustomers' },
      { to: '/ref/countries',   labelKey: 'ref.countries' },
      { to: '/ref/streets',     labelKey: 'ref.streets' },
      { to: '/ref/business-types', labelKey: 'ref.business-types' },
      { to: '/ref/communities', labelKey: 'ref.communities' },
      { to: '/ref/cities',      labelKey: 'ref.cities' },
      { to: '/ref/registration-issuers', labelKey: 'ref.registration-issuers' },
    ],
  },
  {
    id: 'vehicles', labelKey: 'menu.vehicles', icon: 'pi pi-car',
    items: [
      { to: '/vehicles/new',    labelKey: 'menu.new' },
      { to: '/vehicles',        labelKey: 'menu.showVehicles' },
      { to: '/ref/vehicle-categories',   labelKey: 'ref.vehicle-categories' },
      { to: '/ref/vehicle-body-types',   labelKey: 'ref.vehicle-body-types' },
      { to: '/ref/vehicle-makers',       labelKey: 'ref.vehicle-makers' },
      { to: '/ref/vehicle-models',       labelKey: 'ref.vehicle-models' },
      { to: '/ref/vehicle-uses',         labelKey: 'ref.vehicle-uses' },
      { to: '/ref/colors',               labelKey: 'ref.colors' },
      { to: '/ref/customer-vehicle-relation-types', labelKey: 'ref.customer-vehicle-relation-types' },
    ],
  },
  {
    id: 'vehicle-meta', labelKey: 'menu.vehicleMeta', icon: 'pi pi-cog',
    items: [
      { to: '/ref/vehicle-engine-types',              labelKey: 'ref.vehicle-engine-types' },
      { to: '/ref/vehicle-engine-power-source-types', labelKey: 'ref.vehicle-engine-power-source-types' },
      { to: '/ref/vehicle-engine-eco-programs',       labelKey: 'ref.vehicle-engine-eco-programs' },
      { to: '/ref/vehicle-gearboxes',                 labelKey: 'ref.vehicle-gearboxes' },
      { to: '/ref/vehicle-brakes',                    labelKey: 'ref.vehicle-brakes' },
      { to: '/ref/vehicle-supportings',               labelKey: 'ref.vehicle-supportings' },
      { to: '/ref/vehicle-tire-types',                labelKey: 'ref.vehicle-tire-types' },
      { to: '/ref/vehicle-categories-for-payments',   labelKey: 'ref.vehicle-categories-for-payments' },
    ],
  },
  {
    id: 'requests', labelKey: 'menu.requests', icon: 'pi pi-inbox',
    items: [
      { to: '/requests/new',       labelKey: 'menu.new' },
      { to: '/requests',           labelKey: 'menu.showRequests' },
      { to: '/ref/vehicle-ownership-proof-types', labelKey: 'ref.vehicle-ownership-proof-types' },
      { to: '/ref/payment-proof-types',           labelKey: 'ref.payment-proof-types' },
    ],
  },
  {
    id: 'exams', labelKey: 'menu.technicalExams', icon: 'pi pi-check-square',
    items: [
      { to: '/exams/new',                                 labelKey: 'menu.new' },
      { to: '/exams/approved',                            labelKey: 'menu.examsApproved' },
      { to: '/exams/failed',                              labelKey: 'menu.examsFailed' },
      { to: '/exams',                                     labelKey: 'menu.showExams' },
      { to: '/ref/technical-exam-types',                  labelKey: 'ref.technical-exam-types' },
      { to: '/ref/technical-exam-organizations',          labelKey: 'ref.technical-exam-organizations' },
      { to: '/ref/technical-exam-vehicle-parts',          labelKey: 'ref.technical-exam-vehicle-parts' },
      { to: '/ref/technical-exam-report-detail-statuses', labelKey: 'ref.technical-exam-report-detail-statuses' },
    ],
  },
  {
    id: 'payments', labelKey: 'menu.payments', icon: 'pi pi-credit-card',
    items: [
      { to: '/payments?new=1',     labelKey: 'menu.new' },
      { to: '/payments',           labelKey: 'menu.showPayments' },
      { to: '/installment-contracts', labelKey: 'menu.installmentContracts' },
      { to: '/invoices',           labelKey: 'menu.invoices' },
      { to: '/unpaid-deals',       labelKey: 'menu.unpaidDeals' },
      { to: '/customer-financial-state', labelKey: 'menu.customerFinancialState' },
      { to: '/calculation-items',  labelKey: 'menu.calculationItems' },
      { to: '/price-catalog',      labelKey: 'menu.priceCatalog' },
      { to: '/ref/payment-types',  labelKey: 'ref.payment-types' },
      { to: '/ddv-catalog',        labelKey: 'menu.ddvCatalog' },
    ],
  },
  {
    id: 'traffic-licences', labelKey: 'menu.trafficLicences', icon: 'pi pi-id-card',
    items: [
      { to: '/traffic-licences?new=1', labelKey: 'menu.new' },
      { to: '/traffic-licences',       labelKey: 'menu.showTrafficLicences' },
    ],
  },
  {
    id: 'permissions', labelKey: 'menu.permissions', icon: 'pi pi-key',
    items: [
      { to: '/permissions?new=1', labelKey: 'menu.new' },
      { to: '/permissions',       labelKey: 'menu.showPermissions' },
    ],
  },
  {
    id: 'intl-driving', labelKey: 'menu.intlDrivingLicences', icon: 'pi pi-globe',
    items: [
      { to: '/intl-driving-licences?new=1', labelKey: 'menu.new' },
      { to: '/intl-driving-licences',       labelKey: 'menu.showIntlDrivingLicences' },
      { to: '/ref/driving-licence-categories', labelKey: 'ref.driving-licence-categories' },
    ],
  },
  {
    id: 'admin', labelKey: 'menu.admin', icon: 'pi pi-shield', admin: true,
    items: [
      { to: '/stations',  labelKey: 'menu.stations' },
      { to: '/operators', labelKey: 'menu.operators' },
      { to: '/company',   labelKey: 'menu.company' },
      { to: '/privileges',labelKey: 'menu.privileges' },
      { to: '/migration', labelKey: 'menu.migration' },
    ],
  },
  {
    id: 'reports', labelKey: 'menu.reports', icon: 'pi pi-chart-bar',
    items: [
      { to: '/reports/customers',           labelKey: 'menu.report.customers' },
      { to: '/reports/vehicles',            labelKey: 'menu.report.vehicles' },
      { to: '/reports/customer-vehicle',    labelKey: 'menu.report.customerVehicle' },
      { to: '/reports/requests',            labelKey: 'menu.report.requests' },
      { to: '/reports/exams-count',         labelKey: 'menu.report.examsCount' },
      { to: '/reports/exams-failed',        labelKey: 'menu.report.examsFailed' },
      { to: '/reports/payments',            labelKey: 'menu.report.payments' },
    ],
  },
]

// Persisted collapsed state per group
const STORAGE = 'vte.menu.collapsed'
const collapsed = reactive<Record<string, boolean>>(JSON.parse(localStorage.getItem(STORAGE) || '{}'))

function isCollapsed(g: MenuGroup) {
  if (g.id in collapsed) return collapsed[g.id]
  return !g.items.some((i) => route.path.startsWith(i.to.split('?')[0]))
}
function toggle(g: MenuGroup) {
  collapsed[g.id] = !isCollapsed(g)
  localStorage.setItem(STORAGE, JSON.stringify(collapsed))
}

const visibleGroups = computed(() => groups.filter((g) => !g.admin || auth.isAdministrator))

function groupHasActive(g: MenuGroup): boolean {
  return g.items.some((i) => {
    const p = i.to.split('?')[0]
    return route.path === p || (p !== '/' && route.path.startsWith(p + '/'))
  })
}

const pageTitle = computed(() => {
  const path = route.path
  if (path === '/') return t('nav.dashboard')
  for (const g of groups) {
    const it = g.items.find((i) => {
      const p = i.to.split('?')[0]
      return p === path || (p !== '/' && path.startsWith(p + '/'))
    })
    if (it) return t(g.labelKey) + ' · ' + t(it.labelKey)
  }
  if (path.startsWith('/ref/')) {
    const kind = path.replace('/ref/', '')
    return t(`ref.${kind}`)
  }
  if (path.startsWith('/customers/')) return t('menu.customers')
  return ''
})
</script>

<template>
  <div class="app-shell" :class="{ 'shell-collapsed': sidebarCollapsed }">
    <aside class="app-sidebar">
      <div class="brand">
        <div class="brand-mark">VTE</div>
        <div class="brand-text">
          <strong>{{ t('app.title') }}</strong>
          <span>{{ t('app.navigation') }}</span>
        </div>
      </div>

      <div class="nav-scroll">
        <div class="nav-section">
          <ul class="nav">
            <li>
              <router-link to="/" class="topnav">
                <i class="pi pi-th-large" /><span>{{ t('nav.dashboard') }}</span>
              </router-link>
            </li>
          </ul>
        </div>

        <div v-for="g in visibleGroups" :key="g.id" class="nav-section" :class="{ 'has-active': groupHasActive(g) }">
          <button class="section-toggle" @click="toggle(g)">
            <i :class="g.icon" />
            <span>{{ t(g.labelKey) }}</span>
            <i class="pi pi-chevron-down chevron" :class="{ collapsed: isCollapsed(g) }" />
          </button>
          <ul class="nav nested" v-show="!isCollapsed(g)">
            <li v-for="i in g.items" :key="i.to">
              <router-link :to="i.to">
                <span>{{ t(i.labelKey) }}</span>
              </router-link>
            </li>
          </ul>
        </div>
      </div>

      <div class="footer">v0.1 · {{ new Date().getFullYear() }} · Bransys</div>
    </aside>

    <header class="app-topbar">
      <div class="left">
        <Button class="sidebar-toggle" severity="secondary" text rounded size="small"
                :icon="sidebarCollapsed ? 'pi pi-bars' : 'pi pi-angle-double-left'"
                @click="sidebarCollapsed = !sidebarCollapsed"
                v-tooltip.bottom="sidebarCollapsed ? t('app.showSidebar') : t('app.hideSidebar')" />
        <span class="page-title">{{ pageTitle }}</span>
        <span class="crumb" v-if="auth.stationId">· {{ t('app.station') }} #{{ auth.stationId }}</span>
      </div>
      <div class="right">
        <Button severity="secondary" outlined rounded size="small"
                :icon="darkMode ? 'pi pi-sun' : 'pi pi-moon'"
                @click="darkMode = !darkMode"
                v-tooltip.bottom="darkMode ? 'Светла тема' : 'Темна тема'" />
        <SelectButton v-model="lang" :options="langOptions" optionLabel="label" optionValue="value" :allowEmpty="false" size="small" />
        <div class="user-pill">
          <div class="user-avatar">{{ initials }}</div>
          <div class="user-meta"><strong>{{ auth.userName }}</strong><span>{{ auth.roles.join(', ') }}</span></div>
        </div>
        <Button severity="secondary" :label="t('app.logout')" icon="pi pi-sign-out" @click="logout" outlined size="small" />
      </div>
    </header>

    <main class="app-content"><router-view /></main>
  </div>
</template>

<style scoped>
/* sidebar-toggle button in the topbar */
.sidebar-toggle :deep(.p-button) { width: 2rem; height: 2rem; }
.sidebar-toggle { margin-right: 0.25rem; }

.nav-scroll {
  flex: 1; overflow-y: auto;
  padding: 0.5rem 0.625rem 0.75rem;
  display: flex; flex-direction: column; gap: 0.125rem;
}

/* Group header — clean, slightly muted, with a chevron and an active-indicator
   dot when one of its children is the current route. */
.section-toggle {
  width: 100%; display: flex; align-items: center; gap: 0.625rem;
  background: transparent; border: 0;
  color: var(--color-sidebar-fg-muted);
  padding: 0.5rem 0.625rem;
  font-size: 0.6875rem;
  font-weight: 600;
  text-transform: uppercase;
  letter-spacing: 0.08em;
  cursor: pointer; font-family: inherit;
  border-radius: var(--radius-sm);
  transition: color 0.15s ease, background-color 0.15s ease;
}
.section-toggle:hover { color: var(--color-sidebar-fg-active); background: rgba(255, 255, 255, 0.03); }
.section-toggle i:first-child { color: var(--color-sidebar-accent); font-size: 0.8125rem; width: 16px; opacity: 0.85; }
.section-toggle .chevron { margin-left: auto; transition: transform 0.2s ease; font-size: 0.625rem; opacity: 0.5; }
.section-toggle .chevron.collapsed { transform: rotate(-90deg); }
/* Group containing the active route — header turns bright + accent dot */
.nav-section.has-active .section-toggle { color: var(--color-sidebar-fg-active); }
.nav-section.has-active .section-toggle i:first-child { opacity: 1; }

/* Nested items — vertical guide line on the left, no dots. Active item gets
   its own bright bar (already styled in global.css via .router-link-active). */
.nav-section { padding: 0; }
.nav.nested {
  padding: 0.125rem 0 0.25rem 0.875rem;
  margin-left: 0.625rem;
  border-left: 1px solid rgba(255, 255, 255, 0.06);
}
.nav.nested a { padding: 0.35rem 0.625rem; font-size: 0.8125rem; }
.nav.nested a .dot { display: none; }

/* "Dashboard" — the top-level entry sits without a group */
.topnav { padding: 0.45rem 0.625rem !important; font-weight: 600 !important; }
.topnav i:first-child { color: var(--color-sidebar-accent); }
</style>
