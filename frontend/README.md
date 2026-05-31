# VTE frontend (Vue 3)

Web UI for the rewritten VTE (vehicle technical-inspection) system.

## Stack

- Vue 3 + `<script setup>` + TypeScript
- Vite (dev server + build)
- Vue Router (with role-based guards)
- Pinia (auth store, persisted to `localStorage`)
- Axios (with JWT interceptor + 401 → /login redirect)
- vue-i18n (`mk` default + `en` fallback)
- PrimeVue 4 (Aura theme) + PrimeIcons

## Prerequisites

- Node.js 20+ (verified: 22.x).
- The backend running at `http://localhost:5258` (Vite dev server proxies `/api/*` to it).

## Run

```bash
cd frontend
npm install      # one-time
npm run dev      # → http://localhost:5173
```

Default login (matches backend seed):
- Username: `admin`
- Password: `ChangeMe!Now1`

## Build

```bash
npm run build    # → dist/
```

## Layout

```
frontend/
├── src/
│   ├── api/client.ts            ← axios instance (JWT interceptor, 401 → /login)
│   ├── stores/auth.ts           ← Pinia auth store (token, roles, stationId; persisted)
│   ├── locales/                 ← mk (default) + en + i18n bootstrap
│   ├── router/index.ts          ← routes + role-based guards
│   ├── components/AppLayout.vue ← sidebar + topbar + locale switcher + logout
│   ├── views/
│   │   ├── LoginView.vue
│   │   ├── DashboardView.vue
│   │   ├── CustomersView.vue    ← list + create dialog
│   │   └── StationsView.vue     ← Administrator only
│   ├── styles/global.css
│   ├── App.vue
│   └── main.ts
├── vite.config.ts               ← /api proxy to backend
└── package.json
```

## Adding new routes

1. Create the view in `src/views/`.
2. Register it in `src/router/index.ts`. To restrict to Administrators, add `meta: { roles: ['Administrator'] }`.
3. Add a nav link in `src/components/AppLayout.vue` (use `v-if="auth.isAdministrator"` for admin-only items).
4. Add new translation keys to `src/locales/mk.ts` and `src/locales/en.ts`.

## Tenancy

The frontend is largely tenant-unaware — the JWT carries the `stationId` claim and the backend's EF Core query filter does the tenant scoping automatically. The frontend just calls `/api/customers`, `/api/vehicles`, etc., and gets only the rows for the operator's station. Administrators get cross-tenant data because their JWT has no `stationId` claim.
