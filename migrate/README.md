# Migration runbook — legacy `VTEZVV` → v2 `VTE`

This folder contains every SQL/PowerShell script used to migrate data from the legacy `VTEZVV` (and `VTESecurity`) databases into the new `VTE` database that the .NET 10 / Vue 3 rewrite uses.

> **You don't need any of this just to run the app.** For a fresh dev setup, see [`../db/README.md`](../db/README.md) — the API auto-creates an empty `VTE` with default seed data on first start. The scripts here are only for importing real production data.

## Target databases

| Database               | Where it lives                  | What it is                                                            |
|------------------------|---------------------------------|-----------------------------------------------------------------------|
| `VTE`                  | `(localdb)\MSSQLLocalDB`        | **Target.** The v2 schema. Created by EF migrations.                  |
| `VTEZVV`               | `(localdb)\MSSQLLocalDB` (or wherever you restored it) | Legacy operational data (clients, vehicles, requests, payments, tech-exams). |
| `VTEZVV_Snapshot`      | `(localdb)\MSSQLLocalDB`        | Optional point-in-time snapshot of `VTEZVV` used for bulk migration so the live DB stays untouched. |
| `VTESecurity`          | `(localdb)\MSSQLLocalDB` (or remote) | Legacy users + permissions (decoupled from VTEZVV historically).      |

**Most scripts assume the legacy DB is attached/restored on the same LocalDB instance** and reference it by 3-part name (`VTEZVV.dbo.<Table>` or `VTEZVV_LIVE.VTEZVV.dbo.<Table>` for linked-server style). If you restored it under a different name, update the `:setvar` lines or 3-part names at the top of each script before running.

## Quick start — full data migration

Run these in order against an **empty `VTE`** (or one that already has the seed data from `db/README.md` — both work). All scripts are idempotent unless noted otherwise.

```cmd
:: 0) Make sure VTE exists and EF migrations have run.
::    Easiest: start the API once (see ../db/README.md), then stop it.

:: 1) Reference data (countries, communities, cities, …)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-countries.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-citizenships.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-communities.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-cities.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-document-issuers.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-legacy-station.sql

:: 2) Operators (users + roles)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-operators-and-permissions.sql

:: 3) Request workflow catalogs
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-request-catalogs.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i reseed-request-types.sql

:: 4) Clients + their vehicles
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-clients.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-vehicle-lookups.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-vehicles.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-vehicle-plate.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-vehicle-manufacture-date.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-vehicle-group-mass.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-vehicle-towing.sql

:: 5) Requests + their attachments/proofs
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-requests.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-request-reference-no.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i wire-request-operators.sql

:: 6) Technical exams + Записник details
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i create-tech-exam-operators.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i add-exam-detail-tables.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i seed-exam-parts.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-technical-exams.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i fix-tech-exam-org-name.sql

:: 7) Payments + price catalog
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-payment-category-zelenmap.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i backfill-pricecatalog-rules.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i add-pricecatalog-company-and-backfill.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i fix-pricecatalog-active-cascade.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-payments.sql

:: 8) Performance indexes (after bulk inserts — much faster this way)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i perf-indexes.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i perf-indexes-vte.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i perf-indexes-requests.sql

:: 9) Fixes (apply if you hit the documented issue)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i fix-part-category-name.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i fix-station-names.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i merge-nina-operator.sql

:: 10) Validation
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i parity-check-2026.sql
```

Expect ~150 s total wall-clock for ~228k bills + 1.15M lines + 254k installments + everything else.

## Incremental refresh (keep v2 in sync with legacy)

After the initial bulk load, you can pull changes since a watermark:

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-incremental.sql
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i refresh-changed-2026.sql
```

Both scripts look at `ModifiedDate` columns on the source side; tune the `@since` parameter at the top of each.

## Script catalog

### Bulk migration (`migrate-*.sql`) — require legacy DB access

| Script                                  | Source → Target                              | Notes                                                            |
|-----------------------------------------|----------------------------------------------|------------------------------------------------------------------|
| `migrate-from-legacy.sql`               | `VTEZVV` → `VTE`                             | Master entrypoint (calls many of the others).                    |
| `migrate-from-snapshot.sql`             | `VTEZVV_Snapshot` → `VTE`                    | Same as above but reads from the snapshot DB (no contention with live legacy). |
| `migrate-vtezvv-to-vte.sql`             | `VTEZVV` → `VTE`                             | Alternate entrypoint with column-rename adjustments.             |
| `migrate-countries.sql`                 | `Countries`                                  | Pure lookup data.                                                |
| `migrate-citizenships.sql`              | `Citizenships`                               |                                                                  |
| `migrate-communities.sql`               | `Communities`                                |                                                                  |
| `migrate-cities.sql`                    | `Cities`                                     |                                                                  |
| `migrate-document-issuers.sql`          | `DocumentIssuers`                            |                                                                  |
| `migrate-clients.sql`                   | `Clients`                                    | Includes EMBG truncation fix for legacy school-name garbage.     |
| `migrate-vehicle-lookups.sql`           | 8 vehicle-related lookup tables              | Makers, models, body types, fuel types, etc.                     |
| `migrate-vehicles.sql`                  | `Vehicles`, `VehicleRegistration`, `ClientVehicleRelation` | Macedonian → English column rename map inside.                   |
| `migrate-requests.sql`                  | `Requests` + child tables                    | Maps legacy request types by name to seeded `RequestTypes`.      |
| `migrate-request-catalogs.sql`          | Request type / proof type catalogs           |                                                                  |
| `migrate-technical-exams.sql`           | `TechnicalExamReports` + line items          | ~228k rows.                                                      |
| `migrate-payments.sql`                  | `PaymentDocuments` + lines + installments    | Largest script, ~146 s wall-clock by itself.                     |
| `migrate-operators-and-permissions.sql` | `VTESecurity.Users` → `AspNetUsers` + `Operators` | Passwords NOT migrated — operators reset on first login.         |
| `migrate-legacy-station.sql`            | One legacy station → one new `Station` row   | Edit `:setvar` lines at top before running.                      |
| `migrate-incremental.sql`               | Catch-up for rows changed since a watermark  | Re-runnable.                                                     |
| `migrate-requests-diag.sql`             | (diagnostic only)                            | Prints row counts; no writes.                                    |

### Backfills (`backfill-*.sql`) — fill columns missed by the bulk pass

| Script                                  | What it fills                                                          |
|-----------------------------------------|------------------------------------------------------------------------|
| `backfill-vehicle-plate.sql`            | Plate, with sentinel handling for invalid legacy plates.               |
| `backfill-vehicle-manufacture-date.sql` | Year of manufacture from registration certificate dates.               |
| `backfill-vehicle-group-mass.sql`       | MaxLegalGroupMass (combined GVM + trailer).                            |
| `backfill-vehicle-towing.sql`           | Trailer-related towing fields.                                         |
| `backfill-request-reference-no.sql`     | Legacy formula-based reference numbers.                                |
| `backfill-payment-category-zelenmap.sql`| Maps payment categories to Zelen-form printout fields.                 |
| `backfill-pricecatalog-rules.sql`       | Trigger/VehicleField/Parametar columns on PriceCatalog from legacy `PaymentItemParametars`. |

### Fixes (`fix-*.sql`) — correct issues found post-migration

| Script                                  | Bug it fixes                                                            |
|-----------------------------------------|-------------------------------------------------------------------------|
| `fix-pricecatalog-active-cascade.sql`   | Cascades `Active=false` down `PaymentCategories → PaymentItems → PaymentItemParametars` so soft-deleted rules don't fire (815 rows). |
| `fix-tech-exam-org-name.sql`            | Normalizes legacy organization names.                                   |
| `fix-station-names.sql`                 | Trims trailing whitespace in station names.                             |
| `fix-part-category-name.sql`            | Renames a tech-exam part category that was mislabeled in legacy.        |
| `merge-nina-operator.sql`               | Merges two operator rows that referred to the same person.              |

### Schema additions (`add-*.sql`) — drift outside EF migrations

| Script                                  | Change                                                                  |
|-----------------------------------------|-------------------------------------------------------------------------|
| `add-pricecatalog-company-and-backfill.sql` | Adds `PriceCatalog.PriceCompanyId` (tinyint NULL) + backfills from legacy. **This column is also captured in `../db/schema.sql`.** Backfill requires legacy access. |
| `add-exam-detail-tables.sql`            | Adds tech-exam detail lookup tables not covered by the EF migration.    |

### Seed data (`seed-*.sql`) — non-legacy bootstrap data

| Script                                  | What it seeds                                                            |
|-----------------------------------------|--------------------------------------------------------------------------|
| `seed-vte2-defaults.sql`                | Defaults for an empty v2 schema (originally for "VTE2"; the v2 API now seeds these in code — see `Seed/DataSeeder.cs`). Kept for reference. |
| `seed-exam-parts.sql`                   | Tech-exam part categories + parts (Macedonian text).                     |
| `seed-utf16.sql`                        | Same content as `seed-*.sql` but in UTF-16 — use these if `sqlcmd` mangles Cyrillic text on your system. |
| `seed-exam-parts.utf16.sql`             | "                                                                        |
| `reseed-request-types.sql`              | Reseeds the request-type tree (Plav/Bel/Zelen roots + leaves).           |
| `reseed-request-types.utf16.sql`        | UTF-16 variant.                                                          |
| `fix-part-category-name.utf16.sql`      | UTF-16 variant.                                                          |
| `fix-station-names.utf16.sql`           | UTF-16 variant.                                                          |

### Performance (`perf-*.sql`) — apply AFTER bulk inserts

| Script                                  | Indexes it creates                                                       |
|-----------------------------------------|--------------------------------------------------------------------------|
| `perf-indexes.sql`                      | Cross-cutting (Clients, Vehicles, Requests).                             |
| `perf-indexes-vte.sql`                  | VTE-specific composite indexes.                                          |
| `perf-indexes-requests.sql`             | Request-list filter performance.                                         |

### Operators

| Script                                  | Purpose                                                                  |
|-----------------------------------------|--------------------------------------------------------------------------|
| `create-tech-exam-operators.sql`        | Recreates tech-exam controllers from `VTESecurity` (post bulk migration).|
| `wire-request-operators.sql`            | Links migrated requests to their `CreatedBy` / `Modified` / `Ended` users.|

### PowerShell helpers

| Script                                  | Purpose                                                                  |
|-----------------------------------------|--------------------------------------------------------------------------|
| `run-migration.ps1`                     | Wraps the full bulk-migration sequence.                                  |
| `run-requests-migration.ps1`            | Subset for just the requests pipeline.                                   |
| `snapshot-legacy.ps1`                   | Take a point-in-time copy of `VTEZVV` → `VTEZVV_Snapshot` so the bulk migration doesn't lock the live DB. |
| `snapshot-operators.ps1`                | Copy users from `VTESecurity` into a staging table.                      |
| `resnap-payment-details.ps1`            | Refresh just `PaymentDocumentDetails` since last snapshot.               |

### Obsolete — kept for history

| Script                                  | Why it was retired                                                       |
|-----------------------------------------|--------------------------------------------------------------------------|
| `OBSOLETE-backfill-pricecatalog-from-paymentitems.sql` | Wrong target table — `PriceCatalog.Id` actually maps to `PaymentItemParametars.Id`, not `PaymentItems.Id`. Superseded by `OBSOLETE-refix-pricecatalog-from-parametars.sql` which became `backfill-pricecatalog-rules.sql`. |
| `OBSOLETE-fix-pricecatalog-orphan-sentinel.sql` | Patched the wrong issue; replaced by `fix-pricecatalog-active-cascade.sql`. |
| `OBSOLETE-refix-pricecatalog-from-parametars.sql` | Folded into `backfill-pricecatalog-rules.sql`.                           |

### Validation

| Script                                  | What it checks                                                           |
|-----------------------------------------|--------------------------------------------------------------------------|
| `parity-check-2026.sql`                 | Row counts vs. legacy + spot-checks one client/vehicle/request fully.    |
| `refresh-changed-2026.sql`              | Catch-up for rows changed since a snapshot (used after `parity-check`).  |
| `reattach-vte.sql`                      | Re-attaches `VTE.mdf` to LocalDB after moving files between machines.    |

### Encoding notes

- Macedonian text needs UTF-16 in `sqlcmd` to preserve Cyrillic. If you see `?????` in seeded text, use the `.utf16.sql` variants and add `-f 65001` to `sqlcmd`, or run them via SSMS.

## Caveats / known issues

1. **No password migration.** Every migrated operator has `PasswordHash = NULL`. They must reset on first login (or an admin uses `POST /api/operators/{userId}/reset-password`).
2. **Per-station drift.** Each legacy station's DB might have extra columns, missing columns, or differently-named columns. The `migrate-*.sql` scripts assume a "common subset" — review the output of `migrate-requests-diag.sql` and patch column names where they differ.
3. **PriceCatalog deduplication.** Multiple companies historically had identically-named rules ("Оперативни трошоци" in 6 categories). The `add-pricecatalog-company-and-backfill.sql` + `fix-pricecatalog-active-cascade.sql` pair makes the rule engine pick the right one per company.
4. **Single-Trigger limitation.** v2 `PriceCatalog.Trigger` is one enum value per row. Legacy supports multiple `TrigerdBy*` flags on one rule — those need to be split into multiple v2 rows. Not yet automated.
5. **PriceCompanyId is schema drift.** Added via `add-pricecatalog-company-and-backfill.sql`, not via an EF migration. The model snapshot is stale on this one column. Fold into a real EF migration when convenient.
