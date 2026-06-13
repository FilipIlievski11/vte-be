# Project context for Claude

You're working on **VTE** — a multi-tenant SaaS rewrite (.NET 10 + Vue 3) of a legacy VB.NET WinForms vehicle-inspection system used in Macedonia. The repo holds the new code (`backend-v2/`, `frontend-v2/`), the legacy code (`VTE/`, `WinApp/`, etc.) kept for parity-checking, and SQL migration scripts (`migrate/`).

Read [`README.md`](README.md) for repo layout, [`db/README.md`](db/README.md) for fresh-machine setup, and [`migrate/README.md`](migrate/README.md) to import real data.

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
| 8 | **Sentinel multiplier rule**: when `ParametarFrom = 0 AND ParametarTo = 0 AND VehicleField != "Null"`, the price is `Price * vehicle.<VehicleField>`. (Currently NOT implemented in `PricingEvaluator.cs` — deferred.) |
| 9 | **"Оперативни трошоци" is `TrigerdByRequest`** in legacy — not `TrigerdByTechnicalExam`. Caught this when user asked "are you sure this is right, too many same OT". |
| 10 | **Irregular tech exams**: `TechnicalExamReports.TechnicalExamTypeId > 1` → use `DebtOrigin.TechnicalExamIrregular` + `PriceTrigger.TechnicalExamIrregular`. |
| 11 | **Ownership transfer**: when `requestType.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue`, debts route to `NewClientVehicleRelationId.Value` — NOT `ClientVehicleRelationId`. |
| 12 | **Community filter**: resolve customer's living community via `relation → client → city.CommunityId` and feed into rule lookup. |
| 13 | **Single-Trigger limitation**: v2 `PriceCatalog.Trigger` is one enum value per row. Legacy supports multiple `TrigerdBy*` flags on one rule — those need split into multiple v2 rows. Not yet automated. |
| 14 | **Duplicate PaymentType ids**: the legacy `PaymentTypes` table repeats each type once per company (no company column), and the same fee can carry a different doc-number `Prefix` per company. `usedOnly` dedup groups by **trimmed Name** (legacy names have stray leading spaces) and keeps the id whose **most recent bill** is newest. Picking a stale sibling forks the doc-number sequence (hit this with "со кредитна картичка" 19 vs 24 and "по договор" 18 vs 23). |
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
- ⏳ **#70 BelTemplate.vue** — white ownership-transfer form, pixel-perfect from `legacy/rptBel.Designer.vb`. Layout JSON already in `scripts/print-layout-bel.json`.
- 🟡 **Deferred**: sentinel-VehicleField price multiplier (legacy `Request.vb` ~line 905); multi-Trigger support on one `PriceCatalog` row; traffic-licence / permission / IDL debt hooks (those modules don't exist in v2 yet).

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
- `LegacySync` is disabled in prod (`LegacySync__Enabled=false` in `.env`)

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
