import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';
import { useAuthStore } from '@/stores/auth';

const routes: RouteRecordRaw[] = [
  {
    path: '/login',
    name: 'login',
    component: () => import('@/views/LoginView.vue'),
    meta: { public: true },
  },
  {
    // DEFAULT print view — feeds onto pre-printed MVR government paper.
    // Stamps values at hand-coded mm coordinates matching the legacy form.
    path: '/requests/:id/print',
    name: 'request-print',
    component: () => import('@/views/print/LegacyPaperPrint.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // Technical-exam certificate „Потврда за техничка исправност" on blank A4.
    path: '/technical-exams/:id/print',
    name: 'technical-exam-print',
    component: () => import('@/views/print/TechExamCertificate.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // Technical-exam record „Записник за технички преглед" stamped on pre-printed Letter paper.
    path: '/technical-exams/:id/zapisnik',
    name: 'technical-exam-zapisnik',
    component: () => import('@/views/print/TechExamZapisnik.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // A4 application form (legacy rptInternationalDrivLicenceRequest).
    path: '/international-driving-licences/:id/print',
    name: 'idl-print',
    component: () => import('@/views/print/InternationalDrivingLicenceRequest.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // Booklet/strip permit insert (legacy rptInternationalDriveingLicence).
    path: '/international-driving-licences/:id/permit',
    name: 'idl-permit',
    component: () => import('@/views/print/InternationalDrivingLicencePermit.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // Одобрение за туѓо возило — A4 landscape certificate (legacy rptOdobrenieZaTugoV).
    path: '/vehicle-permissions/:id/print',
    name: 'vehicle-permission-print',
    component: () => import('@/views/print/VehiclePermissionCertificate.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    // Барање за одобрение (legacy rptBaranjeZaOdobrenieZaTugoVozilo).
    path: '/vehicle-permissions/:id/request-print',
    name: 'vehicle-permission-request-print',
    component: () => import('@/views/print/VehiclePermissionRequest.vue'),
    props: true,
    meta: { requiresAuth: true },
  },
  {
    path: '/',
    component: () => import('@/components/AppLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: '', redirect: '/dashboard' },
      { path: 'dashboard', name: 'dashboard', component: () => import('@/views/DashboardView.vue') },
      { path: 'account', name: 'account', component: () => import('@/views/AccountSettingsView.vue') },
      { path: 'clients', name: 'clients', component: () => import('@/views/ClientsView.vue') },
      { path: 'clients/new', name: 'client-new', component: () => import('@/views/ClientFormView.vue') },
      { path: 'clients/:id', name: 'client-edit', component: () => import('@/views/ClientFormView.vue'), props: true },
      { path: 'vehicles', name: 'vehicles', component: () => import('@/views/VehiclesView.vue') },
      { path: 'vehicles/new', name: 'vehicle-new', component: () => import('@/views/VehicleFormView.vue') },
      { path: 'vehicles/:id', name: 'vehicle-edit', component: () => import('@/views/VehicleFormView.vue'), props: true },
      { path: 'requests', name: 'requests', component: () => import('@/views/RequestsView.vue') },
      { path: 'requests/new', name: 'request-new', component: () => import('@/views/RequestFormView.vue') },
      { path: 'requests/:id', name: 'request-edit', component: () => import('@/views/RequestFormView.vue'), props: true },
      { path: 'payments', name: 'payments', component: () => import('@/views/PaymentsView.vue') },
      { path: 'payments/:id', name: 'payment', component: () => import('@/views/PaymentView.vue'), props: true },
      { path: 'fiscal', name: 'fiscal', component: () => import('@/views/FiscalOptionsView.vue') },
      { path: 'reports', name: 'reports', component: () => import('@/views/ReportsView.vue') },
      { path: 'billing-categories', name: 'billing-categories', component: () => import('@/views/BillingCategoriesView.vue') },
      { path: 'technical-exams', name: 'technical-exams', component: () => import('@/views/TechnicalExamsView.vue') },
      { path: 'technical-exams/new', name: 'technical-exam-new', component: () => import('@/views/TechExamFormView.vue') },
      { path: 'technical-exams/:id', name: 'technical-exam', component: () => import('@/views/TechnicalExamReportView.vue'), props: true },
      { path: 'technical-exams/:id/edit', name: 'technical-exam-edit', component: () => import('@/views/TechExamFormView.vue'), props: true },
      { path: 'international-driving-licences', name: 'idl-list', component: () => import('@/views/InternationalDrivingLicencesView.vue') },
      { path: 'international-driving-licences/new', name: 'idl-new', component: () => import('@/views/InternationalDrivingLicenceFormView.vue') },
      { path: 'international-driving-licences/:id', name: 'idl-edit', component: () => import('@/views/InternationalDrivingLicenceFormView.vue'), props: true },
      { path: 'vehicle-permissions', name: 'vehicle-permissions', component: () => import('@/views/VehiclePermissionsView.vue') },
      { path: 'vehicle-permissions/new', name: 'vehicle-permission-new', component: () => import('@/views/VehiclePermissionFormView.vue') },
      { path: 'vehicle-permissions/:id', name: 'vehicle-permission-edit', component: () => import('@/views/VehiclePermissionFormView.vue'), props: true },
      { path: 'stations', name: 'stations', component: () => import('@/views/StationsView.vue') },
      {
        path: 'ref/:kind',
        name: 'ref-manager',
        component: () => import('@/views/RefManagerView.vue'),
        props: true,
      },
      {
        path: 'companies',
        name: 'companies',
        component: () => import('@/views/CompaniesView.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'operators',
        name: 'operators',
        component: () => import('@/views/OperatorsView.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'request-types',
        name: 'request-types',
        component: () => import('@/views/RequestTypesView.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'prices',
        name: 'prices',
        component: () => import('@/views/PricesView.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'legacy-sync',
        name: 'legacy-sync',
        component: () => import('@/views/LegacySyncView.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'print-templates',
        name: 'print-templates',
        component: () => import('@/views/PrintTemplatesView.vue'),
        meta: { requiresAdmin: true },
      },
    ],
  },
  { path: '/:pathMatch(.*)*', redirect: '/dashboard' },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to) => {
  const auth = useAuthStore();
  if (to.meta.public) return true;
  if (to.meta.requiresAuth && !auth.isAuthenticated) {
    return { name: 'login', query: { redirect: to.fullPath } };
  }
  if (to.meta.requiresAdmin && !auth.isAdmin) {
    return { name: 'dashboard' };
  }
  return true;
});
