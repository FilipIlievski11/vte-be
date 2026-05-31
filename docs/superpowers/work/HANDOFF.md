# VTE2 — Handoff for the next Claude

**Last updated:** 2026-05-24
**Project:** VTE legacy VB.NET WinForms → .NET 9 Web API + Vue 3 SaaS rewrite
**Working directory:** `C:\Users\FilipIlievski\Downloads\trunk\trunk`
**Repo state:** NOT under git. There is no `git log`. Use filesystem mtimes (`ls -la`) and the audit progress log (frozen at 2026-04-30) to reconstruct history. Newer work after that date is only visible in the actual code under `backend/` and `frontend/`.

---

## TL;DR

1. The new database is called `VTE2`, lives on `(localdb)\MSSQLLocalDB`, ~73 tables.
2. To rebuild it from scratch on a clean machine, run **one** script: `docs/superpowers/work/bootstrap-vte2-full.sql`. It is idempotent.
3. The .NET 9 backend is at `backend/` and the Vue 3 frontend is at `frontend/`. Both run locally and talk to `VTE2`.
4. Default login: `admin` / `ChangeMe!Now1`.
5. The single source of truth for the legacy domain is `docs/superpowers/specs/2026-04-30-vte-domain-audit.md`. Business rules with IDs `BR-<MOD>-NNN` live in `*-business-rules.md` files next to this one.

---

## How to bring up the system from a fresh checkout

```powershell
# 1. Build the database (idempotent, ~2,500 lines, ~30 seconds)
sqlcmd -S "(localdb)\MSSQLLocalDB" -i docs\superpowers\work\bootstrap-vte2-full.sql -b -X -I

# 2. Start the backend
cd backend
dotnet run --project src\VTE.Api --launch-profile http
# → http://localhost:5258 (Swagger at /swagger)

# 3. In another shell, start the frontend
cd frontend
npm install         # one-time
npm run dev
# → http://localhost:5173 — login admin / ChangeMe!Now1
```

If the user needs to interactively run something (`gcloud auth login`-style), prefix with `!` at the prompt.

---

## What's where

### Database / schema

| File | Purpose |
|---|---|
| `docs/superpowers/work/bootstrap-vte2-full.sql` | **NEW.** Consolidated single-file bootstrap (~2,486 lines, sections 1–9, idempotent). |
| `docs/superpowers/work/bootstrap-vte2.sql` | Section 1: Identity + Stations + Operators. |
| `docs/superpowers/work/bootstrap-reference-data.sql` | Section 2: 29 REF lookup tables. |
| `docs/superpowers/work/bootstrap-customers.sql` | Section 3: Customers + ContactPersons + BankAccounts. |
| `docs/superpowers/work/bootstrap-vehicles.sql` | Section 4: Vehicles + Registrations + Axles + Tyres + CustomerVehicleRelations. |
| `docs/superpowers/work/bootstrap-requests.sql` | Section 5: RequestTypes (workflow engine) + Requests + proofs. |
| `docs/superpowers/work/bootstrap-payments.sql` | Section 6: PaymentTypes + DDVCatalog + PriceCatalog + PaymentDocuments + Details + Installments + InstallmentContracts. |
| `docs/superpowers/work/bootstrap-documents.sql` | Section 7: TrafficLicences + Permissions + IntlDrivingLicences + TechnicalExamReports + Details + VisualErrors. |
| `docs/superpowers/work/bootstrap-fks-and-attachments.sql` | Section 8: 40 deferred FKs + 5 attachment tables. |
| `migrate/seed-vte2-defaults.sql` | Section 9: default REF seed (16 countries, 31 cities, EU vehicle cats, VAT, 10 RequestTypes, etc.). |

Expected end state after consolidated bootstrap: **73 tables, 121 FKs, 20 CHECK constraints, ~250 non-PK indexes.**

### Backend (.NET 9)

- `backend/VTE.slnx` — solution.
- `backend/src/VTE.Domain/` — entity POCOs.
- `backend/src/VTE.Infrastructure/` — `VteDbContext` (extends `IdentityDbContext`), EF mappings, tenant query filter.
- `backend/src/VTE.Api/` — controllers, JWT auth, tenancy, seed.
- `backend/src/VTE.Api/Controllers/` — **26 controllers** as of 2026-05-24 (Auth, Stations, Operators, Customers, CustomerVehicleRelations, Vehicles, VehicleCategories, VehicleBodyTypes, Requests, PaymentDocuments, PaymentCatalogs, PaymentReports, PivotReports, InstallmentContracts, TrafficLicences, TechnicalExamReports, Permissions, Attachments, ReferenceData, RefAdmin, Countries, Communities, Cities, Reports, Dashboard, Migration). The audit progress log only documents up to ~16; the rest were added in May 2026.
- `backend/src/VTE.Api/Tenancy/TenantContext.cs` — pulls `StationId` from JWT claim; null for Administrators.
- `backend/src/VTE.Api/appsettings.json` — connection string, JWT secret, seed admin creds, CORS origins.

### Frontend (Vue 3)

- `frontend/src/views/` — **33 views**. Major ones: Login, Dashboard, Customers, CustomerForm, Vehicles, VehicleForm, Requests, RequestForm, Payments, PaymentForm, Invoices, UnpaidDeals, InstallmentContracts, TechnicalExamReports, TechnicalExamForm, TrafficLicences, Permissions, IntlDrivingLicences, Operators, Stations, RefManager (admin REF editor), Migration, plus per-REF-table views (Countries, Cities, Communities, BodyTypes, Categories, etc.).
- `frontend/src/stores/auth.ts` — Pinia, persisted to localStorage.
- `frontend/src/api/client.ts` — Axios with JWT interceptor + 401 → `/login`.
- `frontend/src/locales/` — `mk` (default) + `en`. Macedonian is the canonical language.
- `frontend/vite.config.ts` — proxies `/api/*` → `http://localhost:5258`.

### Documentation

- `docs/superpowers/specs/2026-04-30-vte-domain-audit.md` (**2,996 lines**) — single source of truth. Contains 85 legacy CREATE TABLE blocks, 508-proc inventory, 71 BR-IDs, 27 open questions, full Macedonian-English glossary, **and the progress log appendix** (frozen at 2026-04-30).
- `docs/superpowers/specs/2026-04-30-vte-domain-audit-design.md` — design notes for the audit.
- `docs/superpowers/specs/2026-04-06-vte-modern-rewrite-design.md` — the original rewrite design.
- `docs/superpowers/plans/2026-04-30-vte-domain-audit.md` — task plan (Pass 1 complete).
- `docs/superpowers/plans/2026-04-06-vte-plan1-foundation.md` — foundation plan.
- `docs/superpowers/work/*-business-rules.md` — extracted BR-<MOD>-NNN rules with `file:line` source citations (Customers/Vehicles/Requests/Payments/Documents, 522 lines total).

### Legacy artifacts (READ-ONLY, DO NOT EXECUTE)

- `docs/superpowers/work/sqlData.utf8.sql` (1.9 MB) — full legacy SQL dump. **First statements are `DROP DATABASE`.** Never pipe to `sqlcmd`. Use grep/sed only.
- `docs/superpowers/work/emSecurity.utf8.sql` — near-duplicate of legacy SEC module. Same warning.
- `WinApp/` — legacy VB.NET WinForms code, kept for reference.
- `VTE.Library/` — legacy CSLA business objects. Read from here when extracting more business rules.

### Migration to VTE2 from a legacy station

- `migrate/README.md` — full runbook.
- `migrate/migrate-legacy-station.sql` — parameterized template (set `LEGACY_DB`, `STATION_NAME`, `STATION_CODE` via `:setvar`). Treated as a template — needs per-station validation per audit R-3 (per-station schema drift).
- `migrate/migrate-from-legacy.sql`, `migrate-from-snapshot.sql` — alternative migration paths.
- `migrate/snapshot-legacy.ps1`, `snapshot-operators.ps1` — PowerShell snapshot helpers.

### Patches / fixes applied to live VTE2 (post-bootstrap)

These were applied to the live database but are NOT in the consolidated bootstrap. If the next Claude rebuilds VTE2 from `bootstrap-vte2-full.sql`, decide whether to replay them:

| File | What it does |
|---|---|
| `migrate/fix-station-names.sql` | Renames seeded stations to `Бранзис Скопје`, `Битола`, `Тетово`. Station-specific — only useful for the dev/demo environment. |
| `migrate/fix-part-category-name.sql` | Renames TechnicalExamVehiclePartCategories Id=24 to `Општо`. Data fix. |
| `migrate/reseed-request-types.sql` | **DESTRUCTIVE** — deletes all Requests and RequestTypes, then reseeds with the legacy hierarchical structure (parent/child categories matching legacy screenshots). |
| `migrate/seed-exam-parts.sql` | Adds Macedonian-language inspection part rows under each TechnicalExamVehiclePartCategory. |
| `migrate/perf-indexes.sql` | Performance indexes — apply after data load if queries are slow. |
| `migrate/add-exam-detail-tables.sql` | Targets `VTE_Modern` (a different DB name) — appears to be a stale alternate. Verify before applying. |

---

## Key design decisions (don't relitigate these)

1. **Tenancy:** one DB (`VTE2`) shared by all stations. Per-row `StationId` discriminator. Enforced via EF Core query filter on tenant-scoped entities (Customer, Vehicle, Request, PaymentDocument, TrafficLicence, TechnicalExamReport, CalculationItem, PriceCatalog). Administrators bypass the filter.
2. **Auth:** ASP.NET Core Identity 9, two hard-coded roles (`Administrator`, `Operator`). No field/object/CSLA privileges. JWT bearer.
3. **Languages:** `mk` is the default; `en` is the secondary. UI labels live in `frontend/src/locales/{mk,en}.ts`.
4. **Default sort:** match legacy SPs — most have no `ORDER BY` → use `id asc`. **Customer is the exception** — legacy `getAll` SP sorts by `id desc`.
5. **Customer uniqueness:** **BR-CUS-001 relaxed.** No unique constraint on any Customer column, including EMBG. Duplicates are intentionally allowed (per stakeholder decision).
6. **Search + paging on every table view:** global search box in header + always-on paginator (25/50/100/200).
7. **UI density:** compact PrimeVue — small inputs, tight cell padding, smaller fonts. Not the default airy spacing.
8. **Reference-data tables:** global (no `StationId`). They don't vary per-tenant in the new design.

---

## Open questions still on the table (from audit §8)

- **Q-001** SecurityPolicies-only-in-sqlData — minor; SEC module is dropped anyway.
- **Q-002** `KenoBingo` template artifact in legacy `emSecurity.sql` — informational.
- **Q-003** SqlQueryNotification procs — vestigial, can be ignored.
- **Q-004** Password migration at cutover — current plan: passwords NOT migrated, every operator resets on first login.
- **Q-005..008** EMBG uniqueness, EMBG checksum, two TaxNumber columns, customer-related procs not yet read.
- **Q-009** 14 commented-out validation rules in legacy Vehicle.vb — keep or drop?
- **Q-010..018** Request workflow flag semantics, NewRegistration vs. PreviousRegistrationRequired ambiguity.
- **Q-019..027** Payments: numbering schemes, VAT-vs-discount interaction, late-fee logic, "Polisa" meaning, B2B billing override.
- **Q-028..033** Document regulatory units and Macedonian abbreviations.

These are documented in §8 of the audit doc, not in this handoff. Refer there for context.

---

## Likely next tasks (not started)

Pick from these based on what the user asks for. Each was either flagged in the progress log or is visible as a TODO in the code:

1. **Wire up password-reset endpoint** for migrated operators (`POST /api/operators/{userId}/reset-password`) — referenced in `migrate/README.md` step 4.
2. **Fill out the migration template** for the abbreviated REF tables (`VehicleMakers`, `VehicleModel`, `Colors`, …) — see `migrate/README.md` caveats.
3. **Add migration paths for Permissions, IntlDrivingLicences, TechnicalExamReports** — not in `migrate-legacy-station.sql` yet.
4. **Add migration for PaymentDocumentDetails + PaymentDocumentInstallments** — depends on per-station PriceCatalog mapping.
5. **Add tests.** `backend/tests/VTE.Api.Tests/` is scaffolded but empty.
6. **Validate the consolidated bootstrap on a clean DB.** If verifying, run it against a fresh `VTE3` (rename the DB at top of section 1) and compare table counts.
7. **Backfill the audit doc progress log past 2026-04-30** to cover the May 2026 controller/view additions. (Most modules now have full backend CRUD + frontend forms.)

---

## Landmines / things to watch

- **No git.** No reflog, no diff between sessions. If you change files, document them in this HANDOFF.md or the audit doc progress log.
- **Legacy SQL files are destructive.** `sqlData.utf8.sql` and `emSecurity.utf8.sql` start with `DROP DATABASE`. The README in `docs/superpowers/work/` warns about this.
- **`add-exam-detail-tables.sql` references `VTE_Modern`, not `VTE2`.** Looks like a stale alternate. Verify before running.
- **`reseed-request-types.sql` is destructive** — deletes all Requests. Don't run on a production DB.
- **JWT secret in `appsettings.json` is a placeholder.** Production deployment must override `Jwt:Secret`.
- **The `Default Station` is seeded on first backend run.** If you delete it and the Stations table becomes empty, the next API start will re-seed.
- **EF Core query filter silently scopes results.** When debugging "why don't I see this row?", check whether your JWT has a `stationId` claim that doesn't match.
- **Stakeholder confirmed UI density preference** — when adding new views, match the existing compact PrimeVue styling, not the default airy spacing.
- **Reference data is global, but per-station-merged on legacy → VTE2 migration.** The migrate script dedupes by name; legacy stations with the same `Скопје` city row collapse into one global row.

---

## Macedonian glossary cheatsheet (top 10 only — full list in audit §2)

| MK | EN |
|---|---|
| Возило | Vehicle |
| Сопственик | Owner |
| Регистрација | Registration |
| Технички преглед | Technical examination |
| Сообраќајна дозвола | Traffic licence |
| Барање | Request |
| Уплата | Payment |
| Фактура | Invoice |
| Рата | Installment |
| Кочници | Brakes |

---

## How to verify the system end-to-end (smoke test)

```powershell
# 1. DB up?
sqlcmd -S "(localdb)\MSSQLLocalDB" -d VTE2 -Q "SELECT COUNT(*) AS Tables FROM sys.tables;"
# expect: 73 (or close — newer May 2026 work may have added entities)

# 2. Backend up?
Invoke-RestMethod http://localhost:5258/api/auth/login -Method POST -ContentType "application/json" `
  -Body '{"userName":"admin","password":"ChangeMe!Now1"}'
# expect: JSON with token + roles=["Administrator"] + stationId=null

# 3. Frontend up?
# Open http://localhost:5173 in a browser, log in as admin / ChangeMe!Now1.
# Sidebar should show: Dashboard, Customers, Vehicles, Requests, Payments,
# Technical exams, Traffic licences, Permissions, Operators, Stations,
# Reference data (admin-only), Migration (admin-only).
```

If any step fails, **read the actual code under `backend/src/` and `frontend/src/`** before assuming the bootstrap is at fault. Most evolution since 2026-04-30 is in the application layer, not the schema.

---

## Memory persistence

The user's auto-memory lives at `C:\Users\FilipIlievski\.claude\projects\C--Users-FilipIlievski-Downloads-trunk-trunk\memory\`. It auto-loads into every new Claude conversation. Existing memories cover: project overview, backend stack, frontend stack, plans-and-specs location, audit doc, repo not under git, no-Python-for-shell, compact UI preference, duplicates allowed on Customer, search+paging pattern, match-legacy-sort. **Add new memories when you learn something durable about how the user wants to work or about persistent project state** — but don't memorize file paths or code patterns (those can be re-derived).
