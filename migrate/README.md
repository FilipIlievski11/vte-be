# Migration runbook — legacy VTE → VTE2

Move one legacy station's data into the new multi-tenant `VTE2` database as a tenant row.

## Prerequisites

- VTE2 already exists with all bootstrap scripts applied:
  1. `docs/superpowers/work/bootstrap-vte2.sql`
  2. `docs/superpowers/work/bootstrap-customers.sql`
  3. `docs/superpowers/work/bootstrap-vehicles.sql`
  4. `docs/superpowers/work/bootstrap-requests.sql`
  5. `docs/superpowers/work/bootstrap-payments.sql`
  6. `docs/superpowers/work/bootstrap-documents.sql`
  7. `docs/superpowers/work/bootstrap-reference-data.sql`
  8. `docs/superpowers/work/bootstrap-fks-and-attachments.sql`
  9. `migrate/seed-vte2-defaults.sql`
- A **restored copy** of the legacy station's database on the same server. **Never run against the live legacy DB.** Restore the `.bak` to a sandbox database (e.g. `VTE_BITOLA_2018_BAK`) and run the migration against that.
- The legacy `app.config` decrypted (per audit R-1) so you have the actual DB name.

## Step 1: seed defaults (one-time, before any migration)

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -d VTE2 -i migrate\seed-vte2-defaults.sql -b -X -I
```

Idempotent — safe to re-run.

## Step 2: migrate one station

Edit the three `:setvar` lines at the top of `migrate-legacy-station.sql`:

```sql
:setvar LEGACY_DB    "VTE_BITOLA_BAK"      -- the restored legacy DB name
:setvar STATION_NAME "Bitola Inspection Station"
:setvar STATION_CODE "BIT"                  -- short, unique
```

Then run **with the `-i` flag and `-x` to enable `:setvar`**:

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -d VTE2 -i migrate\migrate-legacy-station.sql -b -X -I
```

The script:
1. Verifies the legacy DB exists.
2. Verifies the new station code isn't already taken.
3. Creates a new `Stations` row.
4. Dedupe-merges reference data (Cities, Streets, Countries, BusinessTypes, RegistrationIssuers, VehicleBodyTypes, …).
5. Migrates Customers + ContactPersons + BankAccounts.
6. Migrates Vehicles (with all the Macedonian → English column renames).
7. Migrates CustomerVehicleRelations.
8. Migrates Requests, mapping legacy RequestType to the new seeded RequestTypes by name.
9. Migrates TrafficLicences.
10. Migrates PaymentDocuments + Details + Installments, mapping legacy PaymentType flags to the seed PaymentTypes.
11. Migrates Users from `emSecurity.Users` → `AspNetUsers` + `Operators` rows. **Passwords are NOT migrated** — every operator must reset their password on first login.
12. Prints a row-count verification table.

## Step 3: review the row-count verification

The script prints something like:

```
Source                    Legacy   New
Customers                 5421     5421
Vehicles                  6892     6892
CustomerVehicleRelations  7104     7104
Requests                  18403    18403
TrafficLicences           18021    17984    ← review this
PaymentDocuments          22156    22156
```

Mismatches are normal in some cases:
- TrafficLicences: rows with NULL `MadeDate` or `EndDate` are skipped (they would violate `BR-DOC-101/102`).
- Requests: rows whose legacy `IdCustomerVehicleRelation` is invalid are skipped.

For each mismatch, decide: investigate and patch, or accept the loss and document.

## Step 4: post-migration tasks

1. **Issue password resets.** Every migrated user has `PasswordHash = NULL`, so login will fail. Either:
   - Have an Administrator hit `POST /api/operators/{userId}/reset-password` (endpoint not yet built — to add).
   - Or use ASP.NET Identity's `UserManager.GeneratePasswordResetTokenAsync()` and email each operator a reset link.

2. **Spot-check the data.** Open SSMS, connect to `VTE2`, and verify a known customer and a known vehicle by their legacy IDs.

3. **Test a workflow.** Log in as an Administrator, switch to the migrated station, list customers, list vehicles, list open requests.

4. **Run for the next station.** Repeat steps 1–3 with a different legacy DB and station code.

## Caveats

- **This is a TEMPLATE.** Per audit R-3 (per-station schema drift), each legacy DB may have extra columns, missing columns, or differently-named columns. Open the `migrate-legacy-station.sql` file in SSMS and adjust column names where they don't match your station's actual schema.

- **Reference-data tables in the migration script are abbreviated.** The pattern is shown for `BusinessTypes`, `Cities`, `Streets`, `Countries`, `RegistrationIssuers`, `VehicleBodyTypes`. You'll need to fill in the same dedupe-merge pattern for: `VehicleCategories`, `VehicleUse`, `VehicleEngineTypes`, `VehicleEnginePowerSourceTypes`, `VehicleEngineEcoProgram`, `VehicleGearBox`, `VehicleBrakes`, `VehicleSupporting`, `Colors`, `VehicleCategoryForPayments`, `VehicleMakers`, `VehicleModel`, `TehnicalExamOrganizations`, `TehnicalExamsTypes`, `TehnicalExamVehicleParts`, `DriveingLicenceCtegories`, `CustomerVehiclesRelationTypes`. Each one is ~6 lines of SQL following the same template.

- **Permissions, IntlDrivingLicences, and TechnicalExamReports are NOT yet in the migration template.** They follow the same pattern as TrafficLicences. Add them when you have a sample legacy DB to validate column names.

- **PaymentDocumentDetails and PaymentDocumentInstallments are NOT yet migrated.** They depend on `PriceCatalog` mapping which requires station-by-station work (each station has its own price catalog).

## Why this is a template, not a one-shot

Per audit §3.5 and R-3, a real migration to multi-tenant SaaS requires per-station validation. Some stations may have:
- Custom columns added over the years.
- Cleaner or messier reference data.
- Different password hashing (the legacy `Crypt` class).
- Custom stored procedures that wrote data the audit didn't see.

Run the template, look at what fails or comes through wrong, patch the script, and run again. After the first 2–3 stations the script will stabilize.
