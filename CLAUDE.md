# Project context for Claude

> **Repo split (2026-09-24):** this is the **BE repo** (`Repos\VTE\BE`) — backend
> (`backend-v2/`), SQL миграции (`migrate/`), deploy скрипти (`deploy/`) и docs.
> Frontend-от живее во соседното **FE repo** (`Repos\VTE\FE`, само `frontend-v2/`).
> Легаси VB.NET кодот (VTE/, WinApp/…) остана САМО во стариот trunk
> (`Repos\trunk\trunk`) — таму се бара легаси ground truth. Двете нови репоа ја
> носат целата git историја. `deploy/build-release.ps1` го наоѓа FE билдот во
> `..\FE\frontend-v2` автоматски.

You're working on **VTE** — a multi-tenant SaaS rewrite (.NET 10 + Vue 3) of a legacy VB.NET WinForms vehicle-inspection system used in Macedonia. This repo holds the backend (`backend-v2/`) and SQL migration scripts (`migrate/`); the Vue frontend is in the sibling FE repo, and the legacy code (`VTE/`, `WinApp/`, etc.) stays in the old trunk repo for parity-checking.

Read [`README.md`](README.md) for repo layout, [`db/README.md`](db/README.md) for fresh-machine setup, and [`migrate/README.md`](migrate/README.md) to import real data. **Full reference docs live in [`docs/`](docs/README.md)** — architecture, every module, fiscal, prints, deployment, migration, and an operator guide.

## Stack at a glance

- **Backend**: .NET 10, ASP.NET Core, EF Core (SqlServer), ASP.NET Identity, JWT auth
- **Frontend**: Vue 3 + TypeScript, Vite, Pinia, PrimeVue 4, vue-i18n (MK + EN)
- **DB**: SQL Server LocalDB — `(localdb)\MSSQLLocalDB`, database `VTE` (pipe path varies — `LOCALDB#SH864637\tsql\query` was Filip's main dev box)
- **Default login**: `admin` / `ChangeMe!Now1` (seeded by `Seed/DataSeeder.cs`)

## Architecture

```
backend-v2/src/
├── VTE.Domain/         POCO entities + enums. No EF deps.
├── VTE.Infrastructure/ VteDbContext, EF configs, Migrations/, tenancy, pricing.
└── VTE.Api/            Controllers, DTOs, Program.cs, Seed/DataSeeder.cs.
```

**Multi-tenant:** every business entity implements `ITenantOwned` (carries `CompanyId`). A global EF query filter scopes all reads/writes to the caller's company automatically — you never write `.Where(x => x.CompanyId == ...)` in controllers. The tenant is resolved from JWT claims via `ITenantContext`.

**Soft delete:** `Active = false`. Don't hard-delete user-facing entities. The query filter typically excludes `Active = false`.

**Idempotent ops:** `CustomerDebt` uses `(Origin, OriginRequestId)` or `(Origin, OriginTechnicalExamId)` as a uniqueness key — re-creating a debt for the same source is a no-op.

## Legacy hierarchy you MUST know

Pricing rules in the legacy DB live across **three levels**:

```
PaymentCategories  (Operativni trosoci, Atest, ...)
   ↓ IdPymentCategory  [sic: legacy typo]
PaymentItems       (per category)
   ↓ IdPaymentItem
PaymentItemParametars (per item, vehicle-conditional)
```

**`v2.PriceCatalog.Id` maps to `legacy.PaymentItemParametars.Id`** — not `PaymentItems.Id`. (The latter was an 87% coincidental match that fooled us once. The fix script is `migrate/OBSOLETE-refix-pricecatalog-from-parametars.sql` → `migrate/backfill-pricecatalog-rules.sql`.)

Legacy `getPaymentCatalog` SP filter (mirror this in any rule resolution):

```sql
PaymentCategoriesActive    = 1
AND PaymentItemsActive     = 1
AND PaymentItemParametarsActive = 1
AND (IdCompany = @IdCompany OR IdCompany = 0)
```

## Hot gotchas (learn these — they bit us already)

| # | Gotcha |
|---|--------|
| 1 | **Build lock**: `dotnet build` fails with MSB3027 while the API is running (Visual Studio / IIS Express holds the exe). For EF commands during dev, use `--no-build`. |
| 2 | **`PRINT` + subquery**: SQL Server forbids `PRINT 'x' + CAST((SELECT ...) AS varchar)`. Stage into a variable first. |
| 3 | **`Trigger` is reserved**: bracket it — `[Trigger]` — in any column reference. |
| 4 | **Legacy column names**: `NumberOfSeats`, `MaximunAllowedWaight` (typo preserved), `EnginePowerOutPut`, etc. don't match v2 `Vehicle` properties. There's a translation map in `migrate/backfill-pricecatalog-rules.sql`. |
| 5 | **EMBG corruption**: some legacy `GartEMB` rows store school names (e.g. "ССОУ К.НЕДЕЛК..."). Import uses `LEFT(GartEMB, 13)`. |
| 6 | **Migrations were squashed 2026-06-12** into a single `InitialSchema` (now in `VTE.Infrastructure/Migrations/`, EF's default folder — the old `Persistence/Migrations/` chain is gone). Reason: the old chain could never build a fresh DB (duplicated Identity tables + ten `ExcludeFromMigrations()` entities like `Company` whose CreateTable existed nowhere). EF now owns the FULL schema; `ExcludeFromMigrations` was removed everywhere. Filip's laptop DB had its `__EFMigrationsHistory` realigned to the single squash row. NEVER restore the old migration files from git history. |
| 7 | **Active cascade**: a rule is only firing-eligible if **all three** legacy levels have `Active=true`. Cascading down found 815 stale rules. See `migrate/fix-pricecatalog-active-cascade.sql`. |
| 8 | **Sentinel multiplier rule**: when `ParametarFrom = 0 AND ParametarTo = 0 AND VehicleField != "Null"`, the price is `Price * vehicle.<VehicleField>`. NOT implemented in `PricingEvaluator.cs` — **deliberately**, verified safe 2026-07-25: prod has exactly 2 active sentinel rows (973, 2153), both `BasePrice=0` (×anything=0), one scoped to foreign company 2 + dead category 11, the other `Trigger=None` (never evaluated). Re-check this query if new price rules are ever imported. |
| 9 | **"Оперативни трошоци" is `TrigerdByRequest`** in legacy — not `TrigerdByTechnicalExam`. Caught this when user asked "are you sure this is right, too many same OT". |
| 10 | **Irregular tech exams**: `TechnicalExamReports.TechnicalExamTypeId > 1` → use `DebtOrigin.TechnicalExamIrregular` + `PriceTrigger.TechnicalExamIrregular`. |
| 11 | **Ownership transfer**: when `requestType.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue`, debts route to `NewClientVehicleRelationId.Value` — NOT `ClientVehicleRelationId`. |
| 17 | **New owner ("Нов сопственик") = a CLIENT, resolved to a relation**: for `TransfersOwnership` types the FE sends `RequestWriteDto.NewOwnerClientId` (a free client search, any komitent by EMBG/name — mirrors legacy `uxRequestEdit` `IsNewCustomer` panel). `RequestsController.ResolveOrCreateNewOwnerRelationAsync` turns it into `NewClientVehicleRelationId` by reusing-or-**creating** a `ClientVehicleRelation` (new client + anchor's vehicle, same relation-type as anchor), created **`Active=false`** and flipped active by the End flow. `NewClientVehicleRelationId` is still accepted directly (migrated data). The legacy field name `IdCustomerVehicleRelationNew` is misleading — it holds a *customer* id while editing, a *relation* id after save. FE shows the panel as soon as a transfers-ownership type is picked (not gated on the anchor). |
| 12 | **Community filter**: resolve customer's living community via `relation → client → city.CommunityId` and feed into rule lookup. |
| 13 | **Single-Trigger limitation**: v2 `PriceCatalog.Trigger` is one enum value per row; legacy has six `TrigerdBy*` flags on **PaymentCategories**. The three „Технички преглед" categories (11/53/1011) are TE+IrregularTE. **The 2026-07-30 „already split" conclusion was WRONG** — the 48 group-11/53/1011 rules existed ONLY as Trigger=6, so regular exams never charged the MAIN exam fee (e.g. 1603 „за Патнички возила M1" 1700 ден; caught live 2026-08-11 on the first v2 renewal, only exam 10000004 affected — backfilled). Fixed by `migrate/fix-techexam-trigger1-twins.sql`: Trigger=1 twins for all active Trigger=6 rules in those groups (48 twins, applied local+prod). Re-check the flags query if new legacy categories appear. |
| 14 | **Duplicate PaymentType ids**: the legacy `PaymentTypes` table repeats each type once per company (no company column), and the same fee can carry a different doc-number `Prefix` per company. `usedOnly` dedup groups by **trimmed Name** (legacy names have stray leading spaces) and keeps the id whose **most recent bill** is newest. Picking a stale sibling forks the doc-number sequence (hit this with "со кредитна картичка" 19 vs 24 and "по договор" 18 vs 23). |
| 16 | **`RequestType.TechnicalExamRequirement` is overloaded on prod**: the entity types it as the enum `TechnicalExamRequirement` (NotRequired=0/Required=1/Optional=2), but the migration backfilled the raw legacy `IsTehnicalExamRequired` integer into it — which is actually the **exam-TYPE id** (0=none, 1=РЕД-12М, 9=…). So on prod the column holds values like 9 that aren't named enum members. Read it as `(int)type.TechnicalExamRequirement`: `>0` = "create an exam", and the value = the TechnicalExamType id. This is how the auto-exam-on-renewal feature (Requests:AutoCreateTechExam) picks the exam type. The station's tech-exam org is **37** (АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ); РЕД-12М exam type id=1, ValidDays=365. |
| 18 | **Every firing PriceCatalog rule MUST have a valid `PaymentCategoryGroupId`** — the fiscal composition (legacy SP parity) silently DROPS lines whose category chain is broken, so a groupless rule makes its bill report „нема износ за фискализација" (hit live 2026-08-05 on the first real МВД bill; v2-seeded rules 1000000/1000001 lacked groups — fixed by `migrate/fix-idl-permission-category-groups.sql`). When seeding new rules, always set the group (company-4 block = 1000+ ids, e.g. МВД→1004, одобрение→1003). |
| 19 | **Non-РЕД-12М exam types scale the main exam fee**: legacy `AddDeptsToCustomer` multiplies items of the „Технички преглед" category (groups 11/53/1011) by `TechnicalExamType.PercentOfFullExam/100` for every type > 1, and creates NO debts at all when the percent is 0 (АТЕСТ, ПОВТ-РЕГ, ИЗД-ГАСОВИ, ОСЛОБОДЕН). Ported 2026-08-11 via `DebtService.CreateDebtsForSourceAsync(techExamScalePercent:)`. .NET 10 pitfall hit here: `int[].Contains` in an EF lambda binds to the span overload and blows up the funcletizer — use `List<int>`. Debt-creation catch blocks must LOG (a silent catch hid this for a day). |
| 15 | **NEVER pass the prod admin password through a bash double-quoted string** — it's `Vte!1...` and bash history-expands `!1`, sending a wrong password. ~5 wrong tries trips ASP.NET lockout ("Account is locked."). Log in via a Node script that reads `deploy/prod.secrets.local` directly. To clear a lockout: `UPDATE AspNetUsers SET LockoutEnd=NULL, AccessFailedCount=0`. ASP.NET Identity password hashes are self-contained/portable — you can copy a known-good hash between the local and prod `AspNetUsers` to reset a password by SQL (sqlcmd needs `-I` for QUOTED_IDENTIFIER on that table's filtered index). |

## Domain enums (memorize)

```csharp
public enum PriceTrigger {
  None = 0, TechnicalExam = 1, Request = 2, TrafficLicence = 3,
  Permission = 4, IDL = 5, TechnicalExamIrregular = 6
}

public enum DebtOrigin {
  Manual, Request, TechnicalExam, TechnicalExamIrregular,
  TrafficLicence, Permission, InternationalDrivingLicence
}
```

## Key files to read first (in order)

When picking up a task, glance at:

1. [`backend-v2/src/VTE.Domain/Payments/CustomerDebt.cs`](backend-v2/src/VTE.Domain/Payments/CustomerDebt.cs) — debt entity
2. [`backend-v2/src/VTE.Domain/Payments/PriceCatalog.cs`](backend-v2/src/VTE.Domain/Payments/PriceCatalog.cs) — rule schema (note extra cols: `Trigger`, `VehiclePaymentCategoryId`, `CommunityId`, `PaymentCategoryGroupId`, `VehicleField`, `ParametarFrom/To`, `PriceCompanyId`)
3. [`backend-v2/src/VTE.Infrastructure/Pricing/PricingEvaluator.cs`](backend-v2/src/VTE.Infrastructure/Pricing/PricingEvaluator.cs) — reflection-based rule eval (mirrors legacy `GetPaymentForDepts`)
4. [`backend-v2/src/VTE.Infrastructure/Pricing/DebtService.cs`](backend-v2/src/VTE.Infrastructure/Pricing/DebtService.cs) — evaluator + idempotent debt save
5. [`backend-v2/src/VTE.Api/Controllers/TechnicalExamReportsController.cs`](backend-v2/src/VTE.Api/Controllers/TechnicalExamReportsController.cs) — see `Create` hook
6. [`backend-v2/src/VTE.Api/Controllers/RequestsController.cs`](backend-v2/src/VTE.Api/Controllers/RequestsController.cs) — see `Create` hook + ownership-transfer routing
7. [`backend-v2/src/VTE.Api/Controllers/CustomerDebtsController.cs`](backend-v2/src/VTE.Api/Controllers/CustomerDebtsController.cs) — read API + delete + bulk-delete
8. [`frontend-v2/src/views/DashboardView.vue`](frontend-v2/src/views/DashboardView.vue) — Наплата panel (multi-select bulk-delete)

For legacy references when you need ground truth:

- `WinApp/UC/uxDashboard.vb` — dashboard / bill creation
- `VTE/Request.vb` — request workflow + bill-generation flow
- `VTE/<TechExam>.vb` — tech-exam workflow

## Working style

- **Terse responses.** Skip preamble. No "I'll now…" lead-ins.
- **No emojis** unless explicitly asked. Don't add them to code/docs.
- **Edit over Write.** Only write whole files for genuinely new ones.
- **Type-check after frontend edits**: `cd frontend-v2 && npx vue-tsc -b --force`.
- **Dense UI.** Filip prefers compact grids (recent dashboard refactor went 38px→18px row height, .86rem→.78rem font). When in doubt, smaller.
- **Macedonian + English locale keys** for any new user-facing string. Two files: `frontend-v2/src/locales/{mk,en}.ts`. Mirror the structure.
- **Soft-delete preferred** over hard-delete for any business entity.
- **Direct SQL is acceptable** when EF tooling is blocked. The user often runs the API during dev, so EF commands fail on rebuild. Use `sqlcmd` via `(localdb)\MSSQLLocalDB`.
- **Idempotent migrations.** Every SQL script in `migrate/` is safe to re-run.

## Recent work / status

- ✅ **Phase 1–3 of payments integration done**: read API, frontend list/detail, `CustomerDebt` entity, `PricingEvaluator` (reflection-based), `DebtService`, tech-exam + request auto-hooks, community filter, ownership-transfer routing, per-tenant `PriceCompanyId` rule scoping, Active-cascade fix.
- ✅ **Bulk delete on Наплата panel**: per-row + group + header checkboxes; `POST /api/customer-debts/delete-batch`; refuses paid rows.
- ✅ **Tech-exam Записник + certificate prints**.
- ✅ **Plav (3-page) + Zelen prints pixel-perfect**.
- ❌ **#70 BelTemplate.vue — DROPPED by Filip's decision 2026-07-26 („zaboravi go beliot obrazec")**. Do not resurrect. (Layout JSON stays at `scripts/print-layout-bel.json` if he ever changes his mind.)
- ❌ **Duplicate-client merge tool — DROPPED by Filip's decision 2026-07-30 („5 ne go pravi nikako")**. Do not propose or build it again.
- ✅ Permission + IDL debt hooks EXIST (VehiclePermissionsController / InternationalDrivingLicencesController inject IDebtService) — the old "deferred" note was stale.
- 🟡 **Deferred**: sentinel-VehicleField price multiplier (verified dead on prod — gotcha #8); multi-Trigger support on one `PriceCatalog` row (see gotcha #13).

## Data scale (for sizing decisions)

- VTE database ~785 MB total (~648 MB data + ~136 MB log after shrink)
- ~228k bills, ~1.15M lines, ~254k installments, ~7k clients, ~6.9k vehicles, ~18k requests in the migrated production set
- A full migration from legacy snapshot takes ~150 s

## Production server (live since 2026-06-12)

- **URL**: https://116.202.8.155.sslip.io — Hetzner CPX22 (Falkenstein), Ubuntu 24.04
- Stack at `/opt/vte` on the server: Docker Compose — `mssql` (SQL Server 2025 Express, 1.5 GB cap), `api` (.NET 10 + SPA in wwwroot), `caddy` (auto-HTTPS via sslip.io)
- Secrets: `deploy/prod.secrets.local` on Filip's laptop (gitignored) — SA password, JWT secret, admin password; mirrored in `/opt/vte/.env` on the server
- SSH: `ssh -i ~/.ssh/vte_deploy root@116.202.8.155`
- **Redeploy** = `.\deploy\build-release.ps1` → scp `release.zip` to `/opt/vte/app/` → unzip to `publish/` → `docker compose up -d --build api`
- Production DB carries the REAL data since 2026-06-13 (lifted via .bak restore from Filip's laptop: ~32k clients / 66k vehicles / 128k requests / 100k tech-exams). Re-lift = backup local VTE → gzip → scp → `docker cp` into mssql container → `RESTORE DATABASE ... WITH REPLACE` (stop api container first). Admin password on prod = the strong one in `deploy/prod.secrets.local`, NOT the local dev default.
- `LegacySync` is ENABLED in prod since 2026-07-25 (`LegacySync__Enabled: "true"` in docker-compose.yml api env) with a server-side scheduler: `LegacySync__AutoTimesUtc: "03:00,10:00"` (= 05:00/12:00 MK summer) runs `LegacySyncScheduler`/`LegacySyncService` (VTE.Api/Services) — same embedded SQL as the admin button. Admin UI „Синхронизација" shows schedule + last run; manual runs from the prod UI work too. Laptop script `run-legacy-sync.ps1` still useful (pulls the offsite DB backup).
- **DB backups**: nightly cron 01:15 UTC runs `/opt/vte/backup-db.sh` (source: `deploy/backup-db.sh`) — BACKUP+VERIFYONLY → `/opt/vte/backups/vte-YYYYMMDD.bak.gz`, keeps 14. `run-legacy-sync.ps1` step 5 pulls the newest to `C:\Users\filip\VTE-backups` (keeps 10). Restore procedure: `docs/14-db-backup.md`.
- **Operator accounts**: migrated legacy operators live in AspNetUsers with **Id == legacy operator id (string)** — that equality is how `OperatorLegacyId` on bills resolves to a display name (PaymentDocumentsController referent/OperatorName fallback). To give a legacy operator a clean login, RENAME the migrated account (PUT /api/users/{id} with userName — same id keeps all history); never create a parallel new account for the same person. Renamed so far: 119→`makedonka` (Македонка Бузалкова), 122→`elena` (Елена Андоновска); newer staff (e.g. `nina`/Марија Гичева, id 128) are v2-native. Temp passwords go in `deploy/operator-credentials.local` (gitignored), users change them via Подеси профил.

## How to run

```bash
# Backend (auto-migrates DB + seeds admin user on first start)
cd backend-v2/src/VTE.Api
dotnet run

# Frontend (new terminal)
cd frontend-v2
npm install   # first time only
npm run dev
```

Frontend at http://localhost:5173, backend at https://localhost:7165.

## Important: I (Claude) won't remember this conversation across sessions

Each new Claude Code session starts fresh. Read this file at the start of every new session to pick up where we left off. If a task you're picking up isn't covered here, ask the user for context — don't guess.
