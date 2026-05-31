import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const placeholder = (titleKey: string) => ({
  component: () => import('@/views/PlaceholderView.vue'),
  meta: { titleKey },
})

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/login', component: () => import('@/views/LoginView.vue'), meta: { anonymous: true } },
    {
      path: '/',
      component: () => import('@/components/AppLayout.vue'),
      children: [
        { path: '',          component: () => import('@/views/DashboardView.vue') },

        { path: 'customers',     component: () => import('@/views/CustomersView.vue') },
        { path: 'customers/new', component: () => import('@/views/CustomerFormView.vue') },
        { path: 'customers/:id', component: () => import('@/views/CustomerFormView.vue') },

        { path: 'vehicles',     component: () => import('@/views/VehiclesView.vue') },
        { path: 'vehicles/new', component: () => import('@/views/VehicleFormView.vue') },
        { path: 'vehicles/:id', component: () => import('@/views/VehicleFormView.vue') },

        { path: 'requests',     component: () => import('@/views/RequestsView.vue') },
        { path: 'requests/new', component: () => import('@/views/RequestFormView.vue') },
        { path: 'requests/:id(\\d+)', component: () => import('@/views/RequestFormView.vue') },
        { path: 'exams',           component: () => import('@/views/TechnicalExamReportsView.vue') },
        { path: 'exams/approved',  component: () => import('@/views/TechnicalExamReportsView.vue') },
        { path: 'exams/failed',    component: () => import('@/views/TechnicalExamReportsView.vue') },
        { path: 'exams/new',       component: () => import('@/views/TechnicalExamFormView.vue') },
        { path: 'exams/:id(\\d+)', component: () => import('@/views/TechnicalExamFormView.vue') },

        { path: 'payments',                 component: () => import('@/views/PaymentsView.vue') },
        { path: 'payments/new',             component: () => import('@/views/PaymentFormView.vue') },
        { path: 'payments/:id(\\d+)',       component: () => import('@/views/PaymentFormView.vue') },
        { path: 'installment-contracts',    component: () => import('@/views/InstallmentContractsView.vue') },
        { path: 'invoices',                 component: () => import('@/views/InvoicesView.vue') },
        { path: 'unpaid-deals',             component: () => import('@/views/UnpaidDealsView.vue') },
        { path: 'customer-financial-state', component: () => import('@/views/CustomerFinancialStateView.vue') },
        { path: 'calculation-items',        component: () => import('@/views/CalculationItemsView.vue') },
        { path: 'price-catalog',            component: () => import('@/views/PriceCatalogView.vue') },
        { path: 'ddv-catalog',              component: () => import('@/views/DdvCatalogView.vue') },

        { path: 'traffic-licences',        component: () => import('@/views/TrafficLicencesView.vue') },
        { path: 'permissions',             component: () => import('@/views/PermissionsView.vue') },
        { path: 'intl-driving-licences',   component: () => import('@/views/IntlDrivingLicencesView.vue') },

        { path: 'ref/countries',                component: () => import('@/views/CountriesView.vue') },
        { path: 'ref/cities',                   component: () => import('@/views/CitiesView.vue') },
        { path: 'ref/communities',              component: () => import('@/views/CommunitiesView.vue') },
        { path: 'ref/vehicle-body-types',       component: () => import('@/views/VehicleBodyTypesView.vue') },
        { path: 'ref/vehicle-categories',       component: () => import('@/views/VehicleCategoriesView.vue') },
        { path: 'ref/vehicle-categories/new',   component: () => import('@/views/VehicleCategoryFormView.vue') },
        { path: 'ref/vehicle-categories/:id',   component: () => import('@/views/VehicleCategoryFormView.vue') },
        { path: 'ref/:kind',                    component: () => import('@/views/RefManagerView.vue') },

        { path: 'stations',  component: () => import('@/views/StationsView.vue'),  meta: { roles: ['Administrator'] } },
        { path: 'operators', component: () => import('@/views/OperatorsView.vue'), meta: { roles: ['Administrator'] } },
        { path: 'company',    ...placeholder('menu.company'),     meta: { roles: ['Administrator'], titleKey: 'menu.company' } },
        { path: 'privileges', ...placeholder('menu.privileges'),  meta: { roles: ['Administrator'], titleKey: 'menu.privileges' } },
        { path: 'migration',  component: () => import('@/views/MigrationView.vue'), meta: { roles: ['Administrator'] } },

        // Reports — all 7 wired
        { path: 'reports/customers',        component: () => import('@/views/reports/CustomersPivotView.vue') },
        { path: 'reports/vehicles',         component: () => import('@/views/reports/VehiclesPivotView.vue') },
        { path: 'reports/customer-vehicle', component: () => import('@/views/reports/CustomerVehiclePivotView.vue') },
        { path: 'reports/requests',         component: () => import('@/views/reports/RequestsPivotView.vue') },
        { path: 'reports/exams-count',      component: () => import('@/views/reports/ExamsCountPivotView.vue') },
        { path: 'reports/exams-failed',     component: () => import('@/views/reports/ExamsFailedView.vue') },
        { path: 'reports/payments',         component: () => import('@/views/reports/PaymentsPivotView.vue') },
      ],
    },
    { path: '/:pathMatch(.*)*', redirect: '/' },
  ],
})

router.beforeEach((to) => {
  const auth = useAuthStore()
  if (to.meta.anonymous) return true
  if (!auth.isAuthenticated) return { path: '/login', query: { redirect: to.fullPath } }
  const requiredRoles = to.meta.roles as string[] | undefined
  if (requiredRoles && !requiredRoles.some((r) => auth.roles.includes(r))) return { path: '/' }
  return true
})

export default router
