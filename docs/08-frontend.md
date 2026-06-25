# Frontend architecture

The VTE frontend (`frontend-v2/`) is a single-page Vue 3 + TypeScript application built with Vite. It talks to the .NET API over a JWT-bearer REST interface, renders an `AppLayout` shell (dark sidebar + topbar) with module views, ships a bilingual UI (Macedonian default, English fallback), and includes a set of full-page **print** routes that reproduce legacy government/inspection paperwork pixel-for-pixel. This document is the reference for how the app is wired: stack, folder layout, auth/session, the axios client, routing + guards, i18n, theming/dense-UI conventions, the view-per-module map, and dev conventions.

Cross-links: see [Requests](03-requests.md), [Technical exams](04-technical-exams.md), [Payments & pricing](05-payments-and-pricing.md), [Fiscal](06-fiscal.md), and [Prints](07-prints.md) for the backend behavior behind the screens described here.

---

## 1. Stack

| Concern | Library | Version (`frontend-v2/package.json`) |
|---|---|---|
| Framework | `vue` (Composition API, `<script setup>`) | ^3.5.32 |
| Build tool / dev server | `vite` | ^8.0.10 |
| Vue SFC plugin | `@vitejs/plugin-vue` | ^6.0.6 |
| Router | `vue-router` | ^4.6.4 |
| State | `pinia` | ^3.0.4 |
| UI kit | `primevue` (v4) + `@primeuix/themes` (Aura) + `primeicons` | ^4.5.5 / ^2.0.3 / ^7.0.0 |
| i18n | `vue-i18n` (v9, non-legacy) | ^9.14.5 |
| HTTP | `axios` | ^1.15.2 |
| Type-check | `vue-tsc` + `typescript` | ^3.2.7 / ~6.0.2 |

There is no CSS framework (Tailwind etc.); styling is hand-written CSS variables in `frontend-v2/src/styles.css` plus per-component `<style scoped>` blocks. PrimeVue supplies the components (DataTable, Button, Select, Dialog, Toast, ConfirmDialog, …).

### npm scripts (`frontend-v2/package.json`)

```json
"scripts": {
  "dev": "vite",
  "build": "vue-tsc -b && vite build",
  "preview": "vite preview"
}
```

`build` runs the type-checker (`vue-tsc -b`) **before** the bundler, so a type error fails the production build.

---

## 2. Folder layout

```
frontend-v2/
├── index.html                 # mounts #app, loads /src/main.ts; <title>SMFSolution</title>
├── vite.config.ts             # @ alias, dev port 5174, /api proxy
├── env.d.ts                   # /// <reference types="vite/client" />
├── tsconfig.json              # project references → app + node
├── tsconfig.app.json          # extends @vue/tsconfig/tsconfig.dom.json; @/* path alias
├── tsconfig.node.json
├── public/                    # static-assets dir (currently empty)
├── scripts/
│   └── parse-prnx.ps1         # DevExpress .prnx(.xml)→JSON layout parser (build-time tooling)
└── src/
    ├── main.ts                # app bootstrap (Pinia, Router, i18n, PrimeVue, Toast, Confirm)
    ├── App.vue                # <RouterView/> + global <Toast/> + <ConfirmDialog/>
    ├── styles.css             # design tokens + base styles + dark theme + dense overrides
    ├── types.ts               # ALL shared API DTO types (single file, ~980 lines)
    ├── api/
    │   └── client.ts          # axios instance + JWT/401 interceptors
    ├── stores/
    │   └── auth.ts            # Pinia auth store (localStorage-persisted session)
    ├── router/
    │   └── index.ts           # routes + global beforeEach guard
    ├── locales/
    │   ├── index.ts           # createI18n + setLocale + detect()
    │   ├── mk.ts              # Macedonian messages (default)
    │   └── en.ts             # English messages (fallback)
    ├── fiscal/
    │   └── fiscal.ts          # Accent PF-500 fiscal printing via File System Access API
    ├── components/
    │   ├── AppLayout.vue      # the shell (sidebar groups, topbar, RouterView)
    │   └── PagedTableEmpty.vue# reusable empty-state for DataTables
    └── views/
        ├── *.vue              # one view per route (module screens, listed in §8)
        └── print/             # full-page print templates + layout JSON
```

Notes:
- `types.ts` is a **single** file holding every DTO interface/enum mirrored from the backend (the header comment: "Shared API types — kept in sync with backend-v2 DTOs."). When a backend DTO changes, this is the file to update.
- `src/views/print/` also contains the legacy layout manifests (`print-layout-{plav,zelen,bel}.json`) — emitted by `frontend-v2/scripts/parse-prnx.ps1` from the legacy DevExpress print files (decompressed `.prnx`/`.xml`) — and `legacy-field-map.ts`, a **hand-written** resolver map (legacy control name → value-from-`RequestPrintBundle`), not a generated file.

---

## 3. Bootstrap (`src/main.ts`)

```ts
const app = createApp(App);
app.use(createPinia());
app.use(router);
app.use(i18n);
app.use(PrimeVue, {
  theme: { preset: Aura, options: { darkModeSelector: '.dark-mode' } },
  ripple: false,
});
app.use(ToastService);
app.use(ConfirmationService);
app.mount('#app');
```

Order matters: Pinia first (the router guard and axios interceptor call `useAuthStore()`), then router, i18n, then PrimeVue + its Toast/Confirmation services. `App.vue` mounts the single `<RouterView/>` plus the global `<Toast position="top-right"/>` and `<ConfirmDialog/>` so any view can call `useToast()` / `useConfirm()`.

> **Theming gotcha:** PrimeVue's own `darkModeSelector` is configured as `.dark-mode`, but the app actually toggles dark mode by adding **`html.app-dark`** (see `AppLayout.vue` `applyTheme()` and `styles.css`). The app's dark theme is driven by the hand-written CSS-variable overrides under `html.app-dark`, not by PrimeVue's built-in dark selector. (`darkModeSelector: '.dark-mode'` is effectively inert.)

---

## 4. Auth store (`src/stores/auth.ts`)

A Pinia options-store, id `'auth'`. The session is persisted to `localStorage` under key **`vte.v2.auth`**.

### Persisted shape

```ts
interface PersistedAuth {
  token: string;
  expiresAt: string;          // ISO; checked against Date.now() on load
  userId: string;
  userName: string;
  fullName: string | null;
  companyId: number | null;   // null = cross-tenant admin
  companyName: string | null;
  roles: string[];
}
```

### Lifecycle

- **Hydration:** `state()` calls `loadFromStorage()`, which reads `vte.v2.auth`, JSON-parses it, and **drops it if `expiresAt` is in the past** (`new Date(parsed.expiresAt).getTime() < Date.now()`), removing the key. So an expired token never seeds the store.
- **Getters:**
  - `isAuthenticated` → `!!token`
  - `isAdmin` → `roles.includes('Administrator')`
  - `isOperator` → `roles.includes('Operator')`
- **Actions:**
  - `setSession(r: LoginResponse)` — copies all fields into state and writes them back to `localStorage` (called from `LoginView.vue` after `POST /auth/login`).
  - `updateFullName(name)` — patches both the in-memory `fullName` and the persisted blob, so the topbar name stays in sync after an Account Settings save.
  - `clear()` — nulls everything and removes the `localStorage` key (logout / 401).

The roles are **exact strings** `'Administrator'` and `'Operator'`; route guards and the sidebar gate on `isAdmin`.

---

## 5. API client (`src/api/client.ts`)

A single shared axios instance:

```ts
export const api: AxiosInstance = axios.create({
  baseURL: '/api',
  timeout: 30000,
});
```

- **Base URL is the relative path `/api`.** In dev, Vite proxies `/api` to the backend (see §7); in production the SPA is served from the API's `wwwroot`, so `/api` hits the same origin — no env-specific base URL needed.
- **Request interceptor** attaches the JWT on every call: if `auth.token` is set it sets `Authorization: Bearer <token>`.
- **Response interceptor** handles `401`: it `auth.clear()`s the session and, if not already on `/login`, hard-redirects via `window.location.href = '/login'` (a full reload, not a router push — this resets all app state).

Every view imports `{ api } from '@/api/client'` and calls `api.get/post/put/delete` with typed generics, e.g. `api.get<Paged<Client>>('/clients', { params })`.

---

## 6. Routing & guards (`src/router/index.ts`)

`createWebHistory()` (HTML5 history mode — the server must fall back to `index.html` for unknown paths). Routes split into three tiers:

### 6.1 Public

| Path | Name | Component | Meta |
|---|---|---|---|
| `/login` | `login` | `views/LoginView.vue` | `{ public: true }` |

### 6.2 Standalone print routes (auth-required, **outside** AppLayout)

These render full-page, with no sidebar/topbar (they are not children of the layout route). `props: true` passes the `:id` route param straight into the component.

| Path | Name | Component | Purpose |
|---|---|---|---|
| `/requests/:id/print` | `request-print` | `print/LegacyPaperPrint.vue` | **Default** print — stamps values at hand-coded mm coordinates onto pre-printed MVR government paper. |
| `/requests/:id/print/styled` | `request-print-styled` | `RequestPrintView.vue` | Alternative "readable copy" on blank A4 with chrome. |
| `/technical-exams/:id/print` | `technical-exam-print` | `print/TechExamCertificate.vue` | „Потврда за техничка исправност" (technical-fitness certificate) on blank A4. |
| `/technical-exams/:id/zapisnik` | `technical-exam-zapisnik` | `print/TechExamZapisnik.vue` | „Записник за технички преглед" stamped on pre-printed Letter paper. |

(The 3-page blue/green/white forms `PlavTemplate.vue`, `ZelenTemplate.vue`, `BelTemplate.vue` live under `print/` and are used by the print views above; `BelTemplate` is still in progress — task #70.)

### 6.3 App routes (children of `AppLayout.vue`, all `requiresAuth`)

Parent route `path: '/'` → `components/AppLayout.vue`, `meta: { requiresAuth: true }`. `''` redirects to `/dashboard`. Children:

| Path | Name | Component | Extra meta |
|---|---|---|---|
| `dashboard` | `dashboard` | `DashboardView.vue` | |
| `account` | `account` | `AccountSettingsView.vue` | |
| `clients` / `clients/new` / `clients/:id` | `clients` / `client-new` / `client-edit` | `ClientsView` / `ClientFormView` | |
| `vehicles` / `vehicles/new` / `vehicles/:id` | `vehicles` / `vehicle-new` / `vehicle-edit` | `VehiclesView` / `VehicleFormView` | |
| `requests` / `requests/new` / `requests/:id` | `requests` / `request-new` / `request-edit` | `RequestsView` / `RequestFormView` | |
| `payments` / `payments/:id` | `payments` / `payment` | `PaymentsView` / `PaymentView` | |
| `fiscal` | `fiscal` | `FiscalOptionsView.vue` | |
| `technical-exams` / `technical-exams/new` / `technical-exams/:id` / `technical-exams/:id/edit` | `technical-exams` / `technical-exam-new` / `technical-exam` / `technical-exam-edit` | `TechnicalExamsView` / `TechExamFormView` / `TechnicalExamReportView` / `TechExamFormView` | |
| `stations` | `stations` | `StationsView.vue` | |
| `ref/:kind` | `ref-manager` | `RefManagerView.vue` | generic reference-data CRUD, `:kind` drives config |
| `companies` | `companies` | `CompaniesView.vue` | `requiresAdmin: true` |
| `operators` | `operators` | `OperatorsView.vue` | `requiresAdmin: true` |
| `request-types` | `request-types` | `RequestTypesView.vue` | `requiresAdmin: true` |
| `prices` | `prices` | `PricesView.vue` | `requiresAdmin: true` |
| `legacy-sync` | `legacy-sync` | `LegacySyncView.vue` | `requiresAdmin: true` |

Catch-all: `/:pathMatch(.*)*` → redirect to `/dashboard`.

### 6.4 The guard

```ts
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
```

- Public routes pass unconditionally.
- Unauthenticated access to a `requiresAuth` route bounces to `/login` carrying `?redirect=<original path>`; `LoginView.submit()` reads `route.query.redirect` and pushes there after a successful login (falling back to `/dashboard`).
- A non-admin hitting a `requiresAdmin` route is silently redirected to `/dashboard` (defense-in-depth; the sidebar already hides those links — see §9).

Note the layered defense: the router guard blocks navigation, the sidebar hides admin links, and the axios 401 interceptor catches any server-side authorization failure that slips through.

---

## 7. Vite config (`vite.config.ts`)

```ts
export default defineConfig({
  plugins: [vue()],
  resolve: { alias: { '@': fileURLToPath(new URL('./src', import.meta.url)) } },
  server: {
    port: 5174,
    strictPort: true,
    proxy: { '/api': { target: 'http://localhost:5300', changeOrigin: true } },
  },
});
```

- **`@` alias → `src/`** (mirrored in `tsconfig.app.json` `paths`), so imports are `@/api/client`, `@/stores/auth`, `@/views/...`.
- **Dev server runs on port 5174** (`strictPort: true` — it will fail rather than pick another port).
- **`/api` is proxied to `http://localhost:5300`** with `changeOrigin: true`. This is why the axios `baseURL` can be the relative `/api`.

> **Discrepancy with `CLAUDE.md`:** the quick-reference says "Frontend at http://localhost:5173, backend at https://localhost:7165." The actual checked-in config is **dev port 5174** and proxy target **http://localhost:5300**. Trust `vite.config.ts` for the real values; the CLAUDE.md note appears stale.

---

## 8. View-per-module map (`src/views/`)

Each route maps to exactly one view component. Grouped by module:

**Core / shell**
- `LoginView.vue` — split-screen hero + login form; posts `/auth/login`, calls `auth.setSession`, honors `?redirect`.
- `DashboardView.vue` — landing tiles + Наплата (debts) panel with multi-select bulk-delete and recent-requests list.
- `AccountSettingsView.vue` — self-service profile (`PUT /api/auth/profile`) + password change (`POST /api/auth/change-password`); calls `auth.updateFullName` on save.

**Clients (Сопственици)** — `ClientsView.vue` (paged list + search + business/company filters), `ClientFormView.vue` (create/edit, used by both `clients/new` and `clients/:id`).

**Vehicles (Возила)** — `VehiclesView.vue` (relation-driven list), `VehicleFormView.vue` (multi-card editor).

**Requests (Барања)** — `RequestsView.vue` (paged register), `RequestFormView.vue` (header + anchor relation + proofs + attachments + End/Print), `RequestPrintView.vue` (styled A4 copy).

**Technical exams (Технички прегледи)** — `TechnicalExamsView.vue` (register list), `TechnicalExamReportView.vue` (read-only detail), `TechExamFormView.vue` (full editor; serves both `new` and `:id/edit`).

**Payments (Плаќања)** — `PaymentsView.vue` (bills/invoices list with paid/unpaid/storno tabs), `PaymentView.vue` (bill detail: lines, installments, audit).

**Fiscal** — `FiscalOptionsView.vue` (configure the Accent PF-500 fiscal folder, Z/X reports — see §11).

**Admin (gated by `requiresAdmin`)** — `CompaniesView.vue`, `OperatorsView.vue`, `RequestTypesView.vue`, `PricesView.vue`, `LegacySyncView.vue`, plus `StationsView.vue` and the generic `RefManagerView.vue`.

### 8.1 `RefManagerView.vue` — generic reference-data CRUD

One component drives **all** the `ref/:kind` reference screens via a `kinds: Record<string, KindConfig>` table keyed by the `:kind` route param. Each entry declares an `endpoint`, a `titleKey`, and a typed `fields[]` list (`text | bool | number | select`, with `options()` for selects). Registered kinds:

`countries`, `communities`, `cities`, `citizenships`, `document-issuers`, `personal-data-types`, `request-document-prints`, `request-ownership-proof-types`, `request-payment-proof-types`, `request-attachment-types` (more may be added — see the `kinds` object).

This is why the sidebar Admin group can link to `/ref/countries`, `/ref/cities`, etc. without a dedicated component per lookup.

### 8.2 Standard list-view pattern

List views follow a consistent recipe (see `ClientsView.vue` as the canonical example):
- `page-header` with `<h1>`, a `subtitle` record-count, and an `actions` cluster (search box, filter `Select`s, a "New" `Button`).
- A search box with a 300 ms debounce (`watch(q, …, setTimeout 300)`) that resets to page 1.
- A PrimeVue `DataTable` in **lazy + paginator** mode (`lazy paginator`, `:totalRecords`, `@page`, `@sort`), `rowsPerPageOptions=[25,50,100,200]`, `stripedRows size="small"`, `rowHover`, and `@row-click` navigation to the detail/edit route.
- A skeleton-rows table shown while `loading && items.length === 0`.
- `<template #empty>` uses the shared `PagedTableEmpty` component (icon + title + hint + CTA).
- Mutations go through `useConfirm()` (delete confirmation dialog) and `useToast()` (success/error toasts).
- Admin-only affordances (e.g. company filter, delete button) are wrapped in `v-if="auth.isAdmin"`.

---

## 9. AppLayout (`src/components/AppLayout.vue`)

The authenticated shell: a fixed dark **sidebar**, a **topbar**, and `<main class="app-content"><RouterView/></main>` for the active child route. CSS grid (`styles.css` `.app-shell`): columns `var(--sidebar-w) 1fr`, rows `var(--topbar-h) 1fr`, areas `sidebar topbar / sidebar content`.

### 9.1 Navigation structure

Two parts:
1. **Flat top nav** (hand-written `<RouterLink>`s) — Dashboard, Clients, Vehicles, Requests, Technical exams, Payments, Fiscal — each with a `pi pi-*` icon and a `nav.*` i18n label.
2. **Collapsible groups** from a `groups: MenuGroup[]` array. Currently one group, `admin` (`labelKey: 'nav.administration'`, `icon: 'pi pi-shield'`, `admin: true`) with items:

   Companies, Stations, Operators, then the `/ref/*` lookups (Countries, Communities, Cities, Citizenships, Document issuers, Personal-data types), Request types, Request document prints, ownership/payment-proof/attachment types, Prices, Legacy sync.

```ts
const visibleGroups = computed(() => groups.filter(g => !g.admin || auth.isAdmin));
```

So the entire **Administration** group is rendered only when `auth.isAdmin`. Group collapse state is persisted in `localStorage` under **`vte.v2.menu.collapsed`** (`{ [groupId]: boolean }`); a group auto-expands when the current route matches one of its items.

### 9.2 Topbar

- **Sidebar toggle** — collapses the whole sidebar (`shell-collapsed` class shrinks `--sidebar-w` to 0). State persisted in **`vte.v2.sidebar`** (`'collapsed' | 'open'`).
- **Back button** — shown when not on `/` or `/dashboard`; `router.back()` or fallback to `/dashboard` (added because "operators kept getting stranded on detail/form pages").
- **Page title** — derived from the route path (`pageTitle` computed) plus a breadcrumb showing the company (`auth.companyName` / `Компанија #id`) or `Сите компании` (cross-tenant) for admins.
- **Dark-mode toggle** — see §10.
- **Language toggle** — see §10.
- **User pill** — avatar initials (`userName.slice(0,2).toUpperCase()`), `fullName || userName`, role list; links to `/account`.
- **Sign out** — `logout()` posts `/auth/logout` (best-effort, errors ignored), then `auth.clear()` and pushes `/login`.

---

## 10. Theming, i18n & dense UI

### 10.1 Design tokens (`src/styles.css`)

A `:root` block defines the full token set: brand ramp (`--color-brand-50…900`, primary `#2563eb`), neutrals/surfaces/borders, semantic colors (success/warn/danger/info), a dark **sidebar** palette (`--color-sidebar-*`), layout (`--sidebar-w: 248px`, `--topbar-h: 60px`), radii, and shadows. The comment notes these are "Ported from frontend/ (v1) so v2 inherits the same visual language."

### 10.2 Dark mode

Toggled by adding the class **`app-dark` to `<html>`** (`document.documentElement.classList.toggle('app-dark', darkMode.value)`). Preference persisted in **`vte.v2.theme`** (`'dark' | 'light'`), applied `onMounted` and on change. `styles.css` has an `html.app-dark { … }` block that re-points the same CSS variables to dark values, plus targeted overrides for PrimeVue DataTables, cards, filters, scrollbars, and error boxes. (As noted in §3, PrimeVue's own `darkModeSelector: '.dark-mode'` is configured but not the mechanism actually used.)

### 10.3 PrimeVue + dense convention

PrimeVue 4 with the **Aura** preset, `ripple: false`. The project deliberately runs a **dense UI**: components are passed `size="small"`, and list tables use a `.tight-table` scoped style that compresses padding/row-height/font:

```css
.tight-table :deep(.p-datatable-tbody td),
.tight-table :deep(.p-datatable-thead th) { padding: 0.2rem 0.5rem; font-size: 0.8125rem; line-height: 1.2; }
.tight-table :deep(.p-datatable-tbody td) { height: 28px; }
.tight-table :deep(.p-checkbox) { transform: scale(0.85); }
```

Per CLAUDE.md the maintainer prefers compact grids ("when in doubt, smaller" — a recent dashboard refactor went 38px→18px row height, .86rem→.78rem font). New screens should match: `size="small"`, tight tables, `.page-header`/`.card`/`.filters` shared classes from `styles.css`.

### 10.4 i18n (`src/locales/`)

`vue-i18n` v9 in non-legacy mode:

```ts
export const i18n = createI18n({
  legacy: false,
  locale: detect(),         // stored vte.v2.locale, else 'mk'
  fallbackLocale: 'en',
  messages: { mk, en },
});
```

- **Supported locales:** `['mk', 'en']`; **MK is the default** ("per the original audit §3.1"). Active locale persisted in **`vte.v2.locale`**.
- `setLocale(loc)` sets `i18n.global.locale.value` and writes the storage key.
- The **topbar `SelectButton`** (MK / EN) is two-way bound to a local `lang` ref; a watcher calls `setLocale(v)` and updates `locale.value`.
- Messages live in two structurally-mirrored files, `mk.ts` and `en.ts`, organized into namespaces (`app`, `account`, `nav`, `payments`, `clients`, `ref.titles`, `ref.fields`, `admin`, `empty`, `common`, `login`, …). **Any new user-facing string must be added to both files** under the same key (CLAUDE.md rule). Views consume strings via `const { t } = useI18n()` and `t('namespace.key')`, with interpolation like `t('clients.records', { count: total })`.

Macedonian terms that surface in the UI (with English gloss): Барања (Requests), Наплата (Billing/collection), Технички преглед (Technical exam), Сопственици (Clients/owners), Возила (Vehicles), Плаќања (Payments), Потврда (Certificate), Записник (Record/minutes), Администрација (Administration).

---

## 11. Fiscal printing (`src/fiscal/fiscal.ts`)

Receipts for the **Accent PF-500** fiscal device use the legacy file-exchange protocol: the server composes the command-file bytes, and the **browser** writes them into the operator's fiscal folder using the **File System Access API** (Chrome/Edge only).

- `isFiscalSupported()` checks for `window.showDirectoryPicker`.
- The picked folder handle is stored in **IndexedDB** (`vte-fiscal` DB, store `kv`, key `fiscal-folder`) so it survives reloads; `ensureFolder(interactive)` re-checks `queryPermission`/`requestPermission` (the one-click per-session re-grant).
- `printFiscalForDocument(documentId, interactive, installmentSeq?)` fetches `GET /payment-documents/{id}/fiscal-file` (`{ printsFiscal, skipReason, fileName, contentBase64, fiscalPrintedAt }`), and if it fiscalizes, writes the decoded bytes via `createWritable()` (atomic on close, replacing the legacy `.tx→.txt` rename dance), then posts `/payment-documents/{id}/fiscal-printed` (skipped for installment receipts). Returns a discriminated `FiscalPrintResult` (`printed | skipped | no-folder | unsupported | error`).
- `printDailyClosure` / `printControlReport` drop one-line driver commands (`" E"` Z-report / `" E2"` X-report) — legacy `FiskalModule` parity.

`FiscalOptionsView.vue` is the UI for picking/clearing the folder and triggering Z/X reports. See [Fiscal](06-fiscal.md) for the backend file-composition side.

---

## 12. Print views (`src/views/print/`)

Print outputs are **separate full-page routes** (not children of `AppLayout`), so they render with no app chrome. See [Prints](07-prints.md) for the legacy-form parity work and the backend print bundles. Files under `src/views/print/`:

| File | Role |
|---|---|
| `LegacyPaperPrint.vue` | Default request print — stamps onto pre-printed MVR paper at mm coordinates. |
| `PlavTemplate.vue` | „Плав" 3-page blue registration form (pixel-perfect from `rptPlav`). |
| `ZelenTemplate.vue` | „Зелен" green form. |
| `(BelTemplate.vue)` | „Бел" white ownership-transfer form — in progress (task #70). |
| `TechExamCertificate.vue` | „Потврда за техничка исправност" certificate on blank A4. |
| `TechExamZapisnik.vue` | „Записник за технички преглед" on pre-printed Letter paper. |
| `print-layout-{plav,zelen,bel}.json` | Layout manifests emitted by `scripts/parse-prnx.ps1` from the legacy DevExpress print files (decompressed `.prnx`/`.xml`); control `name/x/y/w/h` in 0.1 mm units (matching `PrintLayoutControl`'s `// 0.1mm units (legacy DPI=254)` comment in `types.ts`). |
| `legacy-field-map.ts` | Hand-written resolver map: legacy control name → a function pulling the value out of `RequestPrintBundle`. |

Common print mechanics (e.g. `TechExamCertificate.vue`):
- Component takes the route `:id` as a prop, fetches its bundle in `onMounted`, and may **auto-open the browser print dialog** (`setTimeout(() => window.print(), 250)`) — e.g. the certificate auto-prints only when `vehicleIsRight` is true (legacy issues it only for a compliant vehicle).
- A fixed `.toolbar.no-print` gives manual Print/Close buttons (`window.print()` / `window.close()`).
- The "paper" is sized in millimeters (`width: 210mm; min-height: 297mm`) with an `@media print { … @page { size: A4; margin: 14mm; } .no-print { display: none !important; } }` block so screen chrome disappears when printing. Pre-printed-paper variants instead position absolutely at the legacy mm coordinates.

The `PrintLayoutControl` / `PrintLayoutManifest` types in `types.ts` describe the parsed legacy layout (`x/y/w/h` in 0.1 mm units, optional `text/align/bold/fontSize/checked`).

---

## 13. Types (`src/types.ts`)

A single file is the **source of truth for all API shapes** consumed by the frontend, kept in sync with backend DTOs. Highlights:

- Generic envelope `Paged<T> { page; pageSize; total; items: T[] }`.
- Auth: `LoginRequest`, `LoginResponse`, `MeResponse`, `ProfileUpdate`, `ChangePasswordRequest`.
- Reference data: `Country`, `Community`, `City`, `Citizenship`, `DocumentIssuer`, `PersonalDataType`, `Company`, `Station`.
- Clients: `Client` (+ `ClientWrite = Omit<…,'id'|'companyId'|'createdAt'>`), `ClientPersonalData`.
- Vehicle module: lookups (`VehicleBodyType`, `VehicleCategory`, `VehicleMaker`, `VehicleModel`, …), `VehicleListItem`, `Vehicle`, `VehicleRegistration`, `VehicleRelationDto`, `ClientVehicleRelationType`.
- Requests: `RequestType` (+ enum `TechnicalExamRequirement { NotRequired=0, Required=1, Optional=2 }`), `RequestListItem`, `RequestRead`, `RequestWrite` (note `newOwnerClientId` for ownership transfers), child DTOs (`RequestOwnershipProofDto`, `RequestPaymentProofDto`, `RequestAttachmentDto`), `EndRequestResult`, and the print bundle (`RequestPrintBundle`, `PrintClientMeta`, `PrintVehicleMeta`).
- Technical exams: `TechExamListItem`, `TechExamReportFull` (+ `TechExamAxleReading`, `TechExamDetailLine`), `TechExamWrite` / `TechExamDetailWrite`, lookups, `TechExamCertificate`, `TechExamZapisnik`, `TechExamType`.
- Payments / pricing: `PaymentListItem`, `PaymentDetail` (+ `PaymentLine`, `Installment`, `InstallmentAgreement`), `PaymentTypeLookup`, `VatRateLookup`, `CustomerDebtRow`, `PriceCatalog` (+ enum `PriceTrigger`), `PriceCatalogLookups`, `PriceCategoryNode`.
- Admin: `UserListItem` / `UserCreate` / `UserUpdate`, `LegacySyncStatus`, `LegacySyncResult`.

Notable: `RequestRead.rowVersion` is a base64-encoded `byte[]` (optimistic concurrency token round-tripped back on update); `RequestWrite.companyId` is nullable so admins can target a tenant.

---

## 14. Dev conventions

- **Type-check after frontend edits:** `cd frontend-v2 && npx vue-tsc -b --force` (CLAUDE.md). The production `build` script also runs `vue-tsc -b` and fails on type errors.
- **`@/` import alias** for everything under `src/` (configured in both `vite.config.ts` and `tsconfig.app.json`).
- **TS config:** `tsconfig.app.json` extends `@vue/tsconfig/tsconfig.dom.json`, `composite: true`, `ignoreDeprecations: "6.0"`. Project references split app vs. node config.
- **Locale parity:** add every new user-facing string to **both** `mk.ts` and `en.ts` under the same key.
- **Dense UI:** prefer `size="small"`, `.tight-table`, compact spacing.
- **Shared building blocks:** reuse `PagedTableEmpty` for empty states, the `.page-header` / `.card` / `.filters` classes, `useToast()` for feedback and `useConfirm()` for destructive actions.
- **`localStorage` keys used by the frontend** (all `vte.v2.*`):

  | Key | Meaning |
  |---|---|
  | `vte.v2.auth` | persisted session (`PersistedAuth`) |
  | `vte.v2.locale` | active UI locale (`mk` / `en`) |
  | `vte.v2.theme` | `dark` / `light` |
  | `vte.v2.sidebar` | `collapsed` / `open` |
  | `vte.v2.menu.collapsed` | per-group collapse map `{ [id]: boolean }` |

  Plus IndexedDB `vte-fiscal` → store `kv` → key `fiscal-folder` for the fiscal directory handle.
- **Production serving:** `vite build` emits to `frontend-v2/dist`; the deploy pipeline ships the SPA inside the API container's `wwwroot` so `/api` is same-origin (no CORS, relative axios base URL works unchanged).
