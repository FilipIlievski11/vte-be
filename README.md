# VTE — Vehicle Technical Examination

Legacy VB.NET system + .NET 10 / Vue 3 rewrite + SQL migration scripts, all in one repo.

## Quick start (fresh laptop)

```bash
# 1) Clone
git clone https://github.com/FilipIlievski11/vte.git
cd vte

# 2) Backend — auto-creates VTE database, applies migrations, seeds admin user
cd backend-v2/src/VTE.Api
dotnet restore
dotnet run            # listens on https://localhost:7165 by default

# 3) Frontend — new terminal
cd ../../../frontend-v2
npm install
npm run dev           # listens on http://localhost:5173
```

Open http://localhost:5173 and log in with:

| Field    | Value           |
|----------|-----------------|
| Username | `admin`         |
| Password | `ChangeMe!Now1` |

Change the password from the Account Settings panel as soon as you're in.

If `dotnet run` fails because the DB can't be created or migrated, see [`db/README.md`](db/README.md) for the by-hand SQL path.

**Want the real production data on this machine?** Auto-seed gives you an empty-but-functional app. To restore real clients/vehicles/requests/bills, see [`db/SEED-PRODUCTION-DATA.md`](db/SEED-PRODUCTION-DATA.md) — fastest path is restoring a `.bak` from OneDrive.

## Prerequisites

| Tool                          | Version           | Install                                           |
|-------------------------------|-------------------|---------------------------------------------------|
| Windows 10 / 11               | —                 | LocalDB is Windows-only                           |
| SQL Server LocalDB            | 2019 / 2022 / 17  | comes with SSMS or SQL Server Express             |
| .NET SDK                      | 10.0              | `winget install Microsoft.DotNet.SDK.10`          |
| Node.js                       | ≥ 22 LTS          | `winget install OpenJS.NodeJS.LTS`                |
| Git                           | 2.40+             | `winget install Git.Git`                          |
| (optional) GitHub CLI         | 2.x               | `winget install GitHub.cli`                       |
| (optional) `dotnet ef` tool   | 10.x              | `dotnet tool install -g dotnet-ef`                |

## Folder layout

| Folder              | What it is                                                                   |
|---------------------|------------------------------------------------------------------------------|
| `backend-v2/`       | **The v2 backend.** ASP.NET Core (.NET 10) + EF Core. Domain / Infrastructure / Api projects. |
| `frontend-v2/`      | **The v2 frontend.** Vue 3 + TypeScript + Vite + Pinia + PrimeVue 4.        |
| `db/`               | Schema script + setup README — what you need to recreate the v2 DB from scratch. |
| `migrate/`          | SQL + PowerShell scripts to import real data from the legacy `VTEZVV` database. Requires legacy DB access. See [`migrate/README.md`](migrate/README.md). |
| `docs/`             | Business rules, audit notes, legacy schema reverse-engineering, print-form blueprints. |
| `scripts/`          | One-off Node/PowerShell utilities (e.g. parser for legacy print-form designers). |
| `VTE/`, `WinApp/`, `VTE.BaseParts/`, `VTE.Library/` | **Legacy VB.NET** WinForms application. Kept for reference and parity-checking against the v2 rewrite. |
| `KeyGen/`, `SetupObicen/`, `TakehardwareInfo/`, `TestWpf/` | Legacy utility projects (licence key generator, installer, hardware fingerprint, WPF playground). |
| `tools/`            | Build-time tools (ResGen.exe).                                               |
| `backend/`, `frontend/` | **Older v1 attempts** at the rewrite. Superseded by `backend-v2/` and `frontend-v2/`. Don't run these. |

## How the rewrite is structured

```
backend-v2/
├── src/
│   ├── VTE.Domain/         ← entities, enums (pure POCOs, no EF deps)
│   ├── VTE.Infrastructure/ ← VteDbContext, EF configurations, Migrations,
│   │                         tenancy filters, pricing/debt services
│   └── VTE.Api/            ← controllers, DTOs, Program.cs, DataSeeder

frontend-v2/
├── src/
│   ├── api/                ← axios client + JWT
│   ├── views/              ← one .vue per top-level screen
│   ├── components/         ← shared widgets
│   ├── stores/             ← Pinia (auth, etc.)
│   ├── router/             ← Vue Router + guards
│   ├── locales/            ← i18n (Macedonian + English)
│   └── types/              ← TS type definitions
```

Multi-tenant model: every business entity carries `CompanyId`; an EF query filter on `ITenantOwned` scopes reads/writes to the caller's company automatically.

## Common dev tasks

```bash
# Type-check the frontend
cd frontend-v2 && npx vue-tsc -b --force

# Add an EF migration after a model change
cd backend-v2
dotnet ef migrations add MyChange \
  --project src/VTE.Infrastructure \
  --startup-project src/VTE.Api

# Apply pending migrations to LocalDB
dotnet ef database update \
  --project src/VTE.Infrastructure \
  --startup-project src/VTE.Api

# Regenerate db/schema.sql after a migration change
dotnet ef migrations script --no-build --idempotent \
  --project src/VTE.Infrastructure \
  --startup-project src/VTE.Api \
  --output ../db/schema.sql
# Then re-append the manual drift section at the bottom (see db/README.md).

# Connect to LocalDB by hand
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE
```

## Security notes

- **Default admin password is `ChangeMe!Now1`** — change immediately after first login.
- The `appsettings.json` files currently contain plain-text connection strings. They're scoped to local LocalDB (Windows integrated security) so no secret is leaking — but if you connect to a remote SQL Server, move the connection string to `appsettings.Development.json` (gitignored) or to env vars.
- The repo is **private** on GitHub. Don't flip it to public without scrubbing first.

## License / ownership

Internal. Not for distribution.
