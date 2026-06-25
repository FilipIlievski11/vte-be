# Legacy data migration

This document is the reference for importing real production data from the legacy
**VTEZVV** database (the VB.NET WinForms system used by the inspection station in Велес)
into the new v2 **VTE** database. It covers the snapshot pipeline, every migration script
under `migrate/`, the Id-preservation and idempotency conventions they rely on, the
incremental/refresh top-up flow that keeps v2 in sync with the still-live legacy DB, and
the data-corruption gotchas you must know before you trust a single migrated row.

> You do **not** need any of this just to run the app. On a fresh dev machine the API
> auto-creates an empty `VTE` with default seed data on first start (see the seeding done
> in `backend-v2/src/VTE.Api/Seed/DataSeeder.cs`). The `migrate/` scripts exist only to
> import the real legacy dataset. The authoritative runbook lives at `migrate/README.md`;
> this doc explains the *why* and the *gotchas* behind it.

Related docs: see [Payments & pricing](05-payments-and-pricing.md) for how the migrated
`PriceCatalog` / `CustomerDebt` data is consumed by the rule evaluator, and
[Deployment](DEPLOYMENT.md) for how a migrated local `VTE` is lifted to production.

---

## 1. The big picture

```
              ┌─────────────────────────────┐
              │  LEGACY (production, LIVE)   │
              │  SQL Server 195.26.159.162,  │
              │  7899  ·  DB = VTEZVV        │
              └───────────┬──────────────────┘
                          │
        snapshot-legacy.ps1│ (SqlBulkCopy, 1:1 schema copy)
                          ▼
              ┌─────────────────────────────┐         linked server
              │  (localdb)\MSSQLLocalDB      │         [VTEZVV_LIVE]
              │  DB = VTEZVV_Snapshot        │◄───────── reads LIVE for
              │  (point-in-time, read-only   │           additive top-ups
              │   bulk source)               │
              └───────────┬──────────────────┘
                          │
   migrate-*.sql / backfill-*.sql / fix-*.sql  (sqlcmd, idempotent)
                          ▼
              ┌─────────────────────────────┐
              │  (localdb)\MSSQLLocalDB      │
              │  DB = VTE   (v2 schema,      │
              │  created by EF migrations)   │
              └───────────┬──────────────────┘
                          │
        .bak restore / re-lift (see DEPLOYMENT.md)
                          ▼
              ┌─────────────────────────────┐
              │  PRODUCTION (Hetzner)        │
              │  Docker mssql container      │
              └─────────────────────────────┘
```

Two distinct legacy sources are used, on purpose:

| Source name in scripts | What it is | Used by |
|---|---|---|
| `VTEZVV_Snapshot` | A **local** point-in-time copy of legacy, on `(localdb)\MSSQLLocalDB`. | The bulk `migrate-*.sql` and most `backfill-*.sql` / `fix-*.sql` scripts. Reading from the snapshot means the bulk load never contends with or locks the live production DB. |
| `VTEZVV_LIVE` | A **linked server** pointing at the still-running production legacy SQL Server (`195.26.159.162,7899` / `VTEZVV`). | `migrate-incremental.sql`, `refresh-changed-2026.sql`, `parity-check-2026.sql`, `migrate-payments.sql`, `backfill-pricecatalog-rules.sql`, `fix-pricecatalog-active-cascade.sql`, `add-pricecatalog-company-and-backfill.sql`. Used for additive top-ups, live parity checks, and pricing/payment backfill. |

The target is always the v2 `VTE` database, whose schema is owned almost entirely by the EF
migration `InitialSchema` (the migration chain was squashed 2026-06-12 — never restore the
old `Persistence/Migrations/` chain; see `CLAUDE.md` gotcha #6). The 2026-06-12 squash folded
the *then-current live schema* into `InitialSchema`, so columns originally added by SQL (e.g.
`PriceCatalog.PriceCompanyId`) are now part of `InitialSchema` too. The one column added *after*
the squash by a real EF migration is `DocumentIssuer.CommunityId` (`AddDocumentIssuerCommunityId`,
2026-06-25) — see §7.

**Data scale** (the migrated production set, per `CLAUDE.md`): ~228k bills, ~1.15M payment
lines, ~254k installments, ~7k clients, ~6.9k vehicles, ~18k requests; tech-exam history is
on the order of ~107k reports. A full bulk migration from snapshot runs in ~150 s.

---

## 2. Taking the snapshot — `snapshot-legacy.ps1`

`migrate/snapshot-legacy.ps1` copies **every** table from the live production `VTEZVV` into
a fresh local `VTEZVV_Snapshot`, schema 1:1, using `SqlBulkCopy` for speed.

Key behaviors (from the script body):

- **Connection** (default `-SourceConn` param): `Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=True;Encrypt=False;...`.
  Target is `(localdb)\MSSQLLocalDB`.
- **Creates the DB with the legacy collation**: `CREATE DATABASE [VTEZVV_Snapshot] COLLATE Macedonian_FYROM_90_CI_AS;` — this preserves Cyrillic sort/compare semantics.
- **Resumable + idempotent-ish.** If `VTEZVV_Snapshot` already exists it *resumes* (despite the script's own stale top comment claiming it "drops & recreates … every run" — the body does not). Per table: if the snapshot copy already has `>=` the source row count (and the source is non-empty), it is **skipped**; if the copy exists but the row count is *short*, the table is **dropped and reloaded**.
- **Schema is regenerated per column** from `INFORMATION_SCHEMA.COLUMNS` + `COLUMNPROPERTY(..., 'IsIdentity')`, mapping each SQL type (`nvarchar(max)`, `decimal(p,s)`, `datetime2(s)`, etc.). `timestamp`/rowversion columns are stored as `binary(8)` (rowversion is not copyable).
- **Copies identities verbatim**: the bulk-copy options are `TableLock | KeepIdentity`, batch size 5000, no timeout. This is what makes downstream **Id preservation** possible (see §5).

Companion PowerShell helpers:

| Script | Purpose |
|---|---|
| `snapshot-operators.ps1` | Copies users from the (historically separate) `VTESecurity` DB into a staging table for the operator migration. |
| `resnap-payment-details.ps1` | `TRUNCATE`s and re-pulls the whole legacy `PaymentDocumentsDetails` table into the snapshot (the original snapshot truncated at ~590k of ~1.15M rows; this is huge and changes often). Bulk-copy batch size 10000, `TableLock | KeepIdentity`. |

> Note: legacy operator/user data lived in a separate `VTESecurity` DB historically. The
> snapshot of those rows lands in `VTEZVV_Snapshot.dbo.LegacyUsers` (referenced by
> `migrate-operators-and-permissions.sql`).

---

## 3. Running the full migration (order matters)

The canonical sequence is in `migrate/README.md`. Every script is invoked via `sqlcmd`
against the `VTE` target, e.g.:

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i migrate-countries.sql
```

The dependency order (FK-driven) is:

1. **Reference data** — `migrate-countries.sql`, `migrate-citizenships.sql`,
   `migrate-communities.sql`, `migrate-cities.sql`, `migrate-document-issuers.sql`,
   `migrate-legacy-station.sql`
2. **Operators** — `migrate-operators-and-permissions.sql`
3. **Request catalogs** — `migrate-request-catalogs.sql`, `reseed-request-types.sql`
4. **Clients + vehicles** — `migrate-clients.sql`, `migrate-vehicle-lookups.sql`,
   `migrate-vehicles.sql`, then the `backfill-vehicle-*.sql` set
5. **Requests** — `migrate-requests.sql`, `backfill-request-reference-no.sql`,
   `wire-request-operators.sql`
6. **Technical exams** — `create-tech-exam-operators.sql`, `add-exam-detail-tables.sql`,
   `seed-exam-parts.sql`, `migrate-technical-exams.sql`, `fix-tech-exam-org-name.sql`
7. **Payments + price catalog** — `backfill-payment-category-zelenmap.sql`,
   `backfill-pricecatalog-rules.sql`, `add-pricecatalog-company-and-backfill.sql`,
   `fix-pricecatalog-active-cascade.sql`, `migrate-payments.sql`
8. **Performance indexes** (apply *after* bulk inserts) — `perf-indexes.sql`,
   `perf-indexes-vte.sql`, `perf-indexes-requests.sql`
9. **Fixes** (apply if you hit the issue) — `fix-part-category-name.sql`,
   `fix-station-names.sql`, `merge-nina-operator.sql`
10. **Validation** — `parity-check-2026.sql`

`run-migration.ps1` wraps the sequence and adds robustness the raw `sqlcmd` calls lack: it
splits the SQL on `GO`, sets connection-level options up front
(`SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; ...`), surfaces `PRINT` output via the
`InfoMessage` event, and — crucially — **forces `IDENTITY_INSERT OFF` for any table left
stuck on** between batches (a cursor over `sys.tables` where
`OBJECTPROPERTY(..., 'TableHasIdentityInsert') = 1`). It continues past errors so one bad
`INSERT` doesn't block the rest. `run-requests-migration.ps1` is the same idea scoped to
just the requests pipeline.

**`sqlcmd` flag cheatsheet** seen in the script headers: `-b` (abort on error), `-X`
(disable startup scripts), `-I` (QUOTED_IDENTIFIER ON — required for the filtered indexes
on Identity tables and for distributed/linked-server queries), `-f 65001` (UTF-8 codepage,
needed for Cyrillic). See the encoding note in §9.

---

## 4. What each script does

### 4.1 Reference / lookup data

| Script | Source → Target | Notes |
|---|---|---|
| `migrate-countries.sql` | `Countries` → `Country` | Pure lookup. |
| `migrate-citizenships.sql` | curated `VALUES` list → `Citizenship` | **Not** a 1:1 copy of any legacy table — the legacy `Countries.Citizenship` column is unusable (mostly `нема`). Seeds a hard-coded list of Macedonian adjectival forms (`Австриско`, `Македонско`, …), `INNER JOIN`ed to `dbo.Country`, with `Citizenship.Id = Country.Id` 1:1. Clients later resolve citizenship by `Citizenship.CountryId = legacy.IdCitizenship`. **Refuses to wipe** if any `Client` has `CitizenshipId` set. |
| `migrate-communities.sql` | `Communities` → `Community` | Macedonian municipalities only. Defaults `CountryId` to the `Country` row matching `ShortName IN ('MK','MKD')` or `Name LIKE '%МАКЕДОН%'` (preferring `MK`). Maps `CommunityName → Name`, `CommunityCode → Code` and `RegistrationCode → PlateNumberPrefix` (with `NULLIF(LTRIM(RTRIM(...)),'')` on blanks). **Refuses to wipe** if any `City` references a `Community`. |
| `migrate-cities.sql` | `Cities` → `City` | Inner-joins `Community` so only cities whose `IdCommunityCode` resolves are inserted (gaps reported). `CityZip` (int) → `PostalCode` nvarchar: NULL/0 → `'0000'`. **Refuses to wipe** if any `Client` references a `City`. |
| `migrate-document-issuers.sql` | `RegistrationIssuers` → `DocumentIssuer` | Legacy Id is `int`, v2 PK is `tinyint` — rows with `Id` outside 1..255 are reported and skipped. Drops the legacy `IdCommunity` link (restored later by the community backfill, see §4.7). **Refuses to wipe** if any `ClientPersonalData` references a `DocumentIssuer`. |
| `migrate-legacy-station.sql` | one legacy station → one `Station` | **VTE2-era template** (`USE [VTE2]`, old plural schema `Stations`/`Customers`/`Operators`, `:setvar LEGACY_DB/STATION_NAME/STATION_CODE`). Edit the `:setvar` lines and reconcile table/column names before running against the current `VTE`. Aborts if a `Station` with that `Code` exists. |
| `migrate-vehicle-lookups.sql` | 10 vehicle lookup tables (body type, category, makers, models, colors, fuels/`EnginePowerSourceTypes`, eco-program, engine type, payment category, relation types) | Preserves Ids. **Important quirk**: `VehicleFuel`, `VehicleEcoProgram`, `VehiclePaymentCategory`, `ClientVehicleRelationType` have `tinyint` PKs that EF does **not** mark as IDENTITY — so those are inserted with plain `INSERT` and explicit Id values (no `IDENTITY_INSERT`). |

### 4.2 Clients — `migrate-clients.sql`

`VTEZVV.Customers → VTE.Client` (+ chained `ClientPersonalData`). Decisions baked in:

- `CompanyId = 4` for every client (АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ Велес — the single tenant the migration imports).
- **Name mapping follows the Macedonian convention**: legacy `CustomerFirstName → Client.FirstName` (which in MK practice is the *surname*), `CustomerSurname → Client.LastName`, `ParentName → MiddleName`. (The parity check and `refresh-changed-2026.sql` rely on this same mapping.)
- **Address is built** as `StreetName + ' ' + LivingAddressNumber + ' ' + City.Name` via `CONCAT_WS(N' ', ...)`, which drops NULL segments cleanly. `Streets` is looked up in the snapshot; the city name comes from the already-migrated `dbo.City`.
- `CitizenshipId` resolves via `Citizenship.CountryId = legacy.IdCitizenship`; no match → NULL (the client is never dropped).
- After clients land, **three legacy columns explode into `ClientPersonalData` rows**:
  - `BLK` → `PersonalDataTypeId = 3` (Personal Id / id card)
  - `PassportNumber` → `PersonalDataTypeId = 2` (Passport)
  - `DriveingLicenceNumber` → `PersonalDataTypeId = 1` (Driving Licence)
  - The issuer FK falls back to `DocumentIssuer` id 1 (`@FallbackIssuer = TOP 1 ... ORDER BY Id`) when the legacy issuer is NULL/0/out-of-range/missing.

### 4.3 Vehicles — `migrate-vehicles.sql` + backfills

`migrate-vehicles.sql` loads three tables in FK order (`CompanyId = 4`, Ids preserved):

1. `Vehicles → Vehicle` — every FK lookup is a `LEFT JOIN`, so an unresolved legacy id becomes NULL rather than dropping the row.
2. `[Vehicle.Registrations] → VehicleRegistration` — `INNER JOIN`ed to `Vehicle` + `DocumentIssuer`, and only rows with a non-blank `RegistrationNumber`.
3. `CustomerVehiclesRelations → ClientVehicleRelation` — `VehicleId` is allowed to be NULL for client-only relations (`ClientVehicleRelationType.IsCustomerOnly = 1`, legacy relation type 3).

This script is also ground zero for the **legacy column-name typos** (gotcha #4). The
Macedonian → English / typo-corrected rename map (a subset, with `NULLIF(x,0)` to turn
legacy "0 means blank" sentinels into NULLs):

| Legacy column | → v2 `Vehicle` property | Note |
|---|---|---|
| `ShellNumber` | `Vin` | |
| `LastRegistratinNumber` | `Plate` | legacy typo ("Registratin"); also the sentinel source (§6) |
| `EnginePowerOutPut` | `EnginePowerKw` | |
| `EngineWorkingCapacity` | `EngineWorkingCapacityCc` | |
| `BrojNaVrtezi` | `MaxRpm` | MK "број на вртежи" |
| `MaxSpeed` | `MaxSpeedKmh` | |
| `TNG` | `HasLpg` | MK ТНГ = LPG |
| `VehicleSizeLength/Width/Hight` | `LengthMm/WidthMm/HeightMm` | legacy typo "Hight" |
| `EmptyWaight` | `EmptyWeightKg` | legacy typo "Waight" |
| `MaximunAllowedWaight` | `MaxAllowedWeightKg` | double typo "Maximun"+"Waight" |
| `MaxLegVkMasa` | `MaxLegalTotalMassKg` | MK transliteration |
| `MaxKonstVkMasa` | `MaxConstructiveTotalMassKg` | |
| `NumberOfAxis` | `AxleCount` | |
| `NumberOfWheels` | `WheelCount` | |
| `OsnoOptovaruvanje1/2` | `AxleLoad1Kg/AxleLoad2Kg` | MK "осно оптоварување" |
| `NumberOfSeats` | `Seats` | |
| `NumberOfStandingSeats` | `StandingSeats` | |
| `NoiseMovment` | `NoiseMovingDb` | legacy typo "Movment" |
| `Tip` / `VehicleModelAdding` / `OznakaNaOdobrenie` | `TypeText` / `ModelVariant` / `ApprovalMark` | |

The base `migrate-vehicles.sql` does **not** populate every column. Four backfills fill the
gaps (run after it):

| Backfill | Fills | How |
|---|---|---|
| `backfill-vehicle-plate.sql` | `Vehicle.Plate` | Promotes the most-recent real `VehicleRegistration.PlateNumber` into vehicles still carrying the sentinel `"{Code}-000-AA"` plate. See §6. |
| `backfill-vehicle-manufacture-date.sql` | `Vehicle.ManufactureDate` | Copied from legacy `Vehicles.MakeDate`. **Also restores `StandingSeats = 0`** where the base migration's `NULLIF(NumberOfStandingSeats, 0)` wrongly NULLed a real zero (passenger cars have 0 standing seats). |
| `backfill-vehicle-group-mass.sql` | `MaxLegalGroupMassKg` | Combined GVM + trailer (`MaxLegVkMasaGrupa` in legacy). |
| `backfill-vehicle-towing.sql` | `MaxTrailerBrakedKg`, `MaxTrailerUnbrakedKg`, `MaxHitchLoadKg` | The real Plav trailer/hitch boxes, from legacy `MaxKonstVkMasaKocnaPrikolka` / `MaxKonstVkMasaNeKocnaPrikolka` / `MaxKonstOptovaruvanjeVoPriklucok`. `migrate-vehicles.sql` mapped the *empty* `TrailerWaightWithBreak`/`TrailerWaightWithoutBreak` columns into `TrailerMassWith[out]BrakesKg` instead, so these stay NULL until this runs. |

`backfill-payment-category-zelenmap.sql` backfills `VehiclePaymentCategory.ZelenMap` from
legacy `VehicleCategoryForPayments.ZelenMap` — this drives which category checkbox the
Plav/Zelen forms tick (`1`=passenger, `2`=cargo, `3`=trailer, `4`=moto; `0`/`5-11`=none).

### 4.4 Requests — `migrate-requests.sql` + `migrate-request-catalogs.sql`

`migrate-request-catalogs.sql` loads five catalog tables (request types, print forms,
ownership/payment/attachment proof types), Ids preserved (`tinyint` range), **before** any
requests exist. It **refuses to run if `dbo.Request` already has rows** (catalogs are FK
parents of requests). Wiping clobbers the starter catalog rows the API seeded on first
boot; `DataSeeder.cs` then skips re-seeding because it checks for *any* `RequestType` row.

`migrate-requests.sql` loads `Requests → Request` plus `RequestOwnershipProof` and
`RequestPaymentProof`. Decisions:

- `CompanyId = 4`, Ids preserved.
- **Operators map to the admin user.** Legacy `IdOperatorCreated/Modified/Ended` (ints) are replaced with the ASP.NET `admin` user's string Id; the original numeric operator ids are appended to `Note` in `[legacy operator ids C=.. M=.. E=..]` form so traceability survives. (`wire-request-operators.sql` later re-links the real migrated operators.)
- `TechnicalExamReportId` is set NULL here (tech-exam module migrated separately) and restored by `backfill-request-reference-no.sql` / the incremental script.
- `NewClientVehicleRelationId` is NULL unless the legacy `IdCustomerVehicleRelationNew` is non-zero **and** exists in target `ClientVehicleRelation`. (This is the ownership-transfer "Нов сопственик" link — see `CLAUDE.md` gotchas #11/#17.)
- `PreviousRegistrationId` kept only when `> 0` and present in `VehicleRegistration`.
- Requests whose `IdCustomerVehicleRelation` is orphaned or whose `IdRequestType` is out of range are **`INNER JOIN`-skipped** (skip count is logged).

`backfill-request-reference-no.sql` reconstructs the human-facing reference number the
legacy app prints (e.g. `167403128/2026`), formula from `VTE.Library/PrintZelenInfo.vb`:

```
{StationCode}{IdTechnicalExamReport}{OperatorId}/{Year(DateCreated)}
   "1"      ||   "67403"           ||  "28"     || "/" || "2026"   = "167403128/2026"
```

`StationCode` is hard-coded `"1"` (VELES / `CompanyId=4`) since only that station was
imported; revisit when onboarding more stations.

### 4.5 Technical exams — `migrate-technical-exams.sql`

Loads four lookups + two data tables from the snapshot (legacy spells it "Tehnical"):

| Legacy | → v2 |
|---|---|
| `TehnicalExamsTypes` | `TechnicalExamType` |
| `DocumentsTehnicalExamsReportsDetailsStatus` | `TechnicalExamDetailStatus` |
| `TehnicalExamVehicleParts` | `TechnicalExamVehiclePart` |
| `TehnicalExamOrganizations` | `TechnicalExamOrganization` |
| `DocumentsTehnicalExamsReports` | `TechnicalExamReport` |
| `DocumentsTehnicalExamsReportsDetails` | `TechnicalExamReportDetail` |

Notes:

- **Ids preserved** so `Request.TechnicalExamReportId` keeps lining up.
- **Measurements**: legacy stores `real NOT NULL` with `0` meaning "not measured"; migrated via `NULLIF(x, 0)` into nullable `float` (a true blank).
- **Brake column renames**: `{Axis}LeftPj → {Axis}LeftRightDiff`, `{Axis}PN → {Axis}Coefficient`. Other legacy typos handled inline: `Waight → Weight`, `EffectOfWorkingBreak* → EffectOfWorkingBrake*`, `NumEngineTurns → EngineRpm`, `TempOfEngineOil → EngineOilTemp`.
- `CompanyId = 4`. Reports whose `IdCustomerVehicleRelation` no longer resolves get a **NULL relation** (LEFT JOIN) rather than being dropped — every report migrates.
- **Re-runnable**: every section guards with `NOT EXISTS (SELECT 1 ... WHERE x.Id = src.Id)` (additive, not wipe-and-reload — unlike clients/vehicles/requests which wipe first).

Supporting tech-exam scripts: `add-exam-detail-tables.sql` (detail lookup tables outside
the EF migration), `seed-exam-parts.sql` (part categories + parts in Cyrillic),
`create-tech-exam-operators.sql` (recreates inspectors from `VTESecurity`),
`fix-tech-exam-org-name.sql` (normalizes org names), `fix-part-category-name.sql`.

### 4.6 Payments + price catalog — `migrate-payments.sql` and the PriceCatalog chain

This is the largest and most subtle area. **Read [Payments & pricing](05-payments-and-pricing.md)
and `CLAUDE.md` "Legacy hierarchy you MUST know" before touching it.** The legacy pricing
data lives across three levels:

```
PaymentCategories   (Оперативни трошоци, Атест, …)   ── IdCompany, TrigerdBy* flags, Active
   ↓ IdPymentCategory   [sic — legacy typo "Pyment"]
PaymentItems        (per category)                   ── IdVehicleCategoryForPayments, Active
   ↓ IdPaymentItem
PaymentItemParametars (per item, vehicle-conditional) ── VehicleField, ParametarFrom/To, Price, Active
```

**The single most important fact: `v2.PriceCatalog.Id == legacy.PaymentItemParametars.Id`**
— *not* `PaymentItems.Id`. The latter was an ~87% coincidental id overlap that produced a
plausible-but-wrong result early on. Everything that touches `PriceCatalog` joins through
`PaymentItemParametars`. (The wrong-target attempt and its fix are preserved as the
`OBSOLETE-*.sql` scripts — see §8.)

`migrate-payments.sql` (reads `VTEZVV_LIVE`, wipe-and-reload, idempotent) imports, in order:

1. `DDVCatalog → VatRate` (Id-preserving).
2. `PaymentTypes → PaymentType` (Id-preserving; bit columns staged into `#pt` because linked-server `COALESCE` on bits is parser-fragile). Maps `Fiskalna_kes/_karticka → IsCash/IsCard`, `Rati → IsInstallment`, `Smetka/Faktura → PrintsReceipt/PrintsInvoice`, `Prefix`. (Beware duplicate `PaymentType` ids across companies — `CLAUDE.md` gotcha #14.)
3. **`PriceCatalog`** rebuilt from `PaymentItemParametars JOIN PaymentItems`, with each row named `"{ItemName} — {ParametarName}"` (e.g. `"за Патнички возила — Оперативни трошоци"`) and `BasePrice = pip.Price`. A **sentinel row `Id = 999999`** (`"Непозната ставка (мигрирана)"`) catches any line whose `IdPriceCatalog` still doesn't resolve — a clean migration should leave it with zero references.
4. `DogovorZaRati → InstallmentAgreement` — staged into `#ia` so `GartEMB` can be truncated with `LEFT(GartEMB, 13)` (see EMBG corruption, §6).
5. `PaymentDocuments → PaymentDocument` — **skips** any bill whose `IdCustomerVehicleRelation` no longer exists in v2 (≈6% / ~15,327 rows) or whose `IdPaymentType` doesn't map; `AgreementId` only set when the `IdDogovor` survived. `LegacyId = p.Id` is recorded.
6. `PaymentDocumentsDetails → PaymentDocumentLine` — `PriceCatalogId = legacy IdPriceCatalog` directly (now correct), falling back to `999999` if the referenced parametar was deleted.
7. `PaymentDocumentsRata → InstallmentSchedule` — `SequenceNo` synthesized via `ROW_NUMBER() OVER (PARTITION BY IdPaymentDocument ...)` since legacy didn't store the schedule order.

The **PriceCatalog rule-evaluator columns** are populated by three more scripts (run after
the `AddCustomerDebtsAndExpandPriceCatalog` EF migration):

- **`backfill-pricecatalog-rules.sql`** — joins `PaymentItemParametars → PaymentItems → PaymentCategories` and fills `Trigger`, `VehiclePaymentCategoryId`, `CommunityId`, `PaymentCategoryGroupId`, `VehicleField`, `ParametarFrom`, `ParametarTo`. It contains the legacy-Vehicle-property → v2-property translation map (and nulls the literal string `'Null'`):

  | Legacy `VehicleField` | → v2 | Legacy `VehicleField` | → v2 |
  |---|---|---|---|
  | `''` / `'Null'` | NULL (fixed fee) | `EnginePowerOutPut` | `EnginePowerKw` |
  | `NumberOfSeats` | `Seats` | `EngineWorkingCapacity` | `EngineWorkingCapacityCc` |
  | `NumberOfStandingSeats` | `StandingSeats` | `IdVehicleCategories` | `CategoryId` |
  | `MaximunAllowedWaight` | `MaxAllowedWeightKg` | `IdVehicleBodyType` | `BodyTypeId` |
  | `TotalWaight` | `MaxLegalTotalMassKg` | `IdEnginePowerSource` | `FuelId` |

  The single `Trigger` is chosen by precedence from the category's `TrigerdBy*` flags:
  `IrregularTechExam(6) → TechExam(1) → Request(2) → TrafficLicence(3) → Permission(4) → IDL(5) → None(0)`.
  These tinyint values match `PriceTrigger` in `backend-v2/src/VTE.Domain` (see `CLAUDE.md`).
  Caveat (`CLAUDE.md` #13): v2 supports only **one** `Trigger` per row; legacy rules with
  multiple `TrigerdBy*` flags need splitting into multiple v2 rows — not yet automated.

- **`add-pricecatalog-company-and-backfill.sql`** — adds `PriceCatalog.PriceCompanyId tinyint NULL` (guarded `IF NOT EXISTS sys.columns`) and backfills it from `PaymentCategories.IdCompany` via `PaymentItemParametars → PaymentItems → PaymentCategories`. (On a current/fresh DB the column already exists from `InitialSchema`, so the `ALTER` is a no-op and the script only runs the backfill `UPDATE`.) Without this filter, every company's identically-named rule ("Оперативни трошоци" appeared in 6 categories) fired for the same vehicle, producing duplicate `CustomerDebt` lines (the test exam 167585 "6× Оперативни трошоци at 354.00" bug). `NULL` means "applies to all companies".

- **`fix-pricecatalog-active-cascade.sql`** — fixes gotcha #7. The migration originally set `PriceCatalog.Active = PaymentItemParametars.Active` alone, but the legacy `getPaymentCatalog` SP filters on the **AND of all three levels**: `PaymentCategoriesActive=1 AND PaymentItemsActive=1 AND PaymentItemParametarsActive=1`. This cascades that AND down into v2 `PriceCatalog.Active` (815 stale rows that were leaking into the evaluator and producing bogus debts). The `999999` sentinel is left alone.

### 4.7 Communities & document-issuers — the new community backfill

`migrate-document-issuers.sql` deliberately drops the legacy `RegistrationIssuers.IdCommunity`
link ("no equivalent in the new schema"). It turns out the Plav print **needs** it: to show
the *new* owner's destination MVR office ("ДО МВР") rather than echoing the old
registration's issuer.

`backfill-document-issuer-community.sql` (added 2026-06-25) restores
`DocumentIssuer.CommunityId` (a column added by EF migration
`AddDocumentIssuerCommunityId`). Because issuer Ids align 1:1 between legacy
`RegistrationIssuers` and v2 `DocumentIssuer`, and community Ids are identical across both
DBs, **only the integer community link is restored** — no Cyrillic transfer. The mapping is
a hard-coded `VALUES (IssuerId, CommunityId)` table of 83 rows
(generated from live `RegistrationIssuers WHERE IdCommunity > 0`), applied with a plain
`UPDATE ... JOIN (VALUES ...)` — fully idempotent and re-runnable. Example entries:
`(1,21), (2,87), (3,62), (37,64), (83,18)`.

### 4.8 Operators — `migrate-operators-and-permissions.sql`

Imports legacy `VTESecurity.Users` (snapshotted into `VTEZVV_Snapshot.dbo.LegacyUsers` by
`snapshot-operators.ps1`) into ASP.NET `AspNetUsers` + the operator table, maintaining a
`dbo.LegacyUserMap` (`LegacyId → ModernId GUID`).

> **Caveat — VTE2-era script.** This script still targets the older `VTE2` database
> (`USE VTE2`) with the old plural table names (`Operators`, `Stations`, `Permissions`,
> `CustomerFinancialState`, `TechnicalExamReports`) and binds operators to station code
> `BRZ-SK`, not the current single-tenant `VTE` schema (`Operator`, `Station`, singular).
> Treat it as a reference for the operator-import logic rather than a turnkey step in the
> current `VTE` pipeline; column/table names will need adjusting before it runs against `VTE`.

Notable behavior:

- **No password migration.** Every imported user is stamped with the *admin's* `PasswordHash`/`SecurityStamp` (so the default password `ChangeMe!Now1`), and must reset on first login (or an admin calls `POST /api/users/{id}/reset-password` on `UsersController`, `[Authorize(Roles = Administrator)]` — there is no `OperatorsController`).
- Usernames are de-duplicated: `ROW_NUMBER() OVER (PARTITION BY UPPER(UserName) ORDER BY LegacyId)`, with collisions suffixed `-{LegacyId}`.
- The same script also (Step 2) backfills `TechnicalExamReports.{First,Second}InspectorOperatorUserId` via `LegacyUserMap`, (Step 3) reloads `Permissions`, and (Step 4) rebuilds `CustomerFinancialState`.
- Companion fixes: `merge-nina-operator.sql` (merges two operator rows that were the same person); `wire-request-operators.sql` (relinks migrated requests' `CreatedBy/Modified/Ended` to the real operators via the legacy ids stashed in `Request.Note`).

---

## 5. Id preservation & idempotency conventions

These two conventions are the backbone of the whole migration. Understand them before
editing any script.

**Id preservation.** Almost every table keeps its legacy primary key. The pattern is:

```sql
SET IDENTITY_INSERT dbo.Target ON;
INSERT INTO dbo.Target (Id, ...) SELECT src.Id, ... FROM VTEZVV_Snapshot.dbo.Source src ...;
SET IDENTITY_INSERT dbo.Target OFF;
DECLARE @maxId bigint = (SELECT ISNULL(MAX(Id),0) FROM dbo.Target);
DBCC CHECKIDENT('dbo.Target', RESEED, @maxId) WITH NO_INFOMSGS;
```

Why it matters: cross-table FKs (`Request.ClientVehicleRelationId`,
`Request.TechnicalExamReportId`, `PaymentDocumentLine.PriceCatalogId`,
`Vehicle.Registrations.IdVehicle`, …) are simply the legacy ids, so joins between migrated
tables are trivial and the **2026 print-parity check compares by request Id** with no
mapping table. The final `DBCC CHECKIDENT(..., RESEED, @maxId)` advances the identity
counter past the highest imported id so v2-native inserts don't collide. Tables with
non-IDENTITY `tinyint` PKs (`VehicleFuel`, `VehicleEcoProgram`, `VehiclePaymentCategory`,
`ClientVehicleRelationType`) skip `IDENTITY_INSERT` and just insert explicit ids.

**Idempotency — two flavors:**

1. **Wipe-and-reload** (clients, vehicles, requests, payments): the script `DELETE`s the
   target (in FK order), `DBCC CHECKIDENT(..., RESEED, 0)`, then re-inserts. These scripts
   add safety guards that **refuse to wipe** when a downstream table would be orphaned
   (e.g. `migrate-communities.sql` refuses if any `City` references a community;
   `migrate-document-issuers.sql` refuses if any `ClientPersonalData` exists;
   `migrate-request-catalogs.sql` refuses if any `Request` exists).
2. **Additive / merge** (technical exams, the incremental top-up, the document-issuer
   community backfill): guarded with `NOT EXISTS (... WHERE x.Id = src.Id)` for inserts, or
   plain `UPDATE ... JOIN` for backfills. Safe to re-run; never wipes.

The single-batch bulk scripts (`migrate-clients/vehicles/requests/technical-exams.sql`,
the vehicle/exam `backfill-*` and `fix-*` scripts, `migrate-incremental.sql`, the `migrate-*`
reference loaders) wrap their work in `BEGIN TRY / BEGIN TRANSACTION ... COMMIT` with a `CATCH`
that rolls back, so a partial failure leaves the target unchanged. The multi-batch
payments/price-catalog chain (`migrate-payments.sql`, `backfill-pricecatalog-rules.sql`,
`add-pricecatalog-company-and-backfill.sql`, `fix-pricecatalog-active-cascade.sql`) and
`backfill-document-issuer-community.sql` instead split on `GO` and rely on `SET XACT_ABORT ON`
(each statement auto-rolls-back on error) rather than one wrapping transaction;
`refresh-changed-2026.sql` and `parity-check-2026.sql` are `UPDATE`-only / read-only and use no
explicit transaction. Every script sets `SET XACT_ABORT ON` (parity/refresh excepted) so a
failing statement aborts rather than half-applies.

---

## 6. Key gotchas (data corruption you must handle)

These map to the numbered gotchas in `CLAUDE.md`. Get them wrong and the migrated data is
subtly, silently wrong.

**Gotcha #5 — EMBG / `GartEMB` corruption.** Some legacy `DogovorZaRati.GartEMB`
(guarantor EMBG, expected to be a 13-char personal number) rows actually contain free text —
school names like `"ССОУ К.НЕДЕЛКОВ..."`. The import truncates with `LEFT(GartEMB, 13)` so
the column fits `GuarantorEmbg` without exploding (`migrate-payments.sql`, the staged `#ia`
insert). Treat `GuarantorEmbg` as best-effort, not validated.

**Gotcha #4 — legacy column-name typos.** Legacy column names don't match v2 properties:
`NumberOfSeats`, `MaximunAllowedWaight` (double typo, preserved), `EnginePowerOutPut`,
`LastRegistratinNumber`, `NoiseMovment`, `IdPymentCategory`, etc. The translation maps live
in `migrate-vehicles.sql` (vehicle columns), `backfill-pricecatalog-rules.sql`
(`VehicleField` values), and `migrate-technical-exams.sql` (brake columns). Always check the
exact legacy spelling against the snapshot before adding a new column to a migration.

**Gotcha #7 — Active cascade.** A pricing rule is firing-eligible only if **all three**
legacy levels (`PaymentCategories`, `PaymentItems`, `PaymentItemParametars`) are `Active=1`.
The naive single-level copy left 815 stale rules `Active=true` in v2, producing bogus debts.
`fix-pricecatalog-active-cascade.sql` cascades the AND down. The evaluator
(`PricingEvaluator.cs`) and any rule resolution must mirror the legacy `getPaymentCatalog`
filter: `PaymentCategoriesActive=1 AND PaymentItemsActive=1 AND PaymentItemParametarsActive=1
AND (IdCompany=@IdCompany OR IdCompany=0)`.

**`PriceCatalog.Id = PaymentItemParametars.Id`** (the headline mapping, §4.6). Not
`PaymentItems.Id`. Every PriceCatalog join goes through `PaymentItemParametars`.

**Plate sentinel.** Legacy stores `"{Community.RegistrationCode}-000-AA"` (e.g.
`"VE-000-AA"`) in `LastRegistratinNumber` when a vehicle has no real plate yet. The base
vehicle migration copies it straight into `Vehicle.Plate`. `backfill-vehicle-plate.sql`
promotes the most-recent real `VehicleRegistration.PlateNumber` (matched with the
`LIKE '[A-Z][A-Z]-000-AA'` pattern) for vehicles that have one; the rest keep the sentinel,
which the frontend Zelen template collapses to `"VE-"` at print time (mirroring legacy
`PrintVehcileInfo.vb`).

**SQL-Server quirks the scripts work around** (also in `CLAUDE.md`):
`[Trigger]` is a reserved word and is always bracketed (#3); `PRINT 'x' + (SELECT ...)` is
illegal so values are staged into a variable first (#2); linked-server `COALESCE` on bit
columns is staged into temp tables (`#pt`); UTF-16/`-f 65001` is needed for Cyrillic (§9);
and the build lock (#1) means EF tooling can fail while the API is running — direct
`sqlcmd` is the workaround the whole `migrate/` folder leans on.

---

## 7. Schema drift outside EF migrations

After the 2026-06-12 squash the EF `InitialSchema` migration owns essentially the full schema.
Two SQL-managed areas are worth calling out — one of which has since been absorbed into
`InitialSchema`, and one that is still genuinely outside any EF migration:

- **`PriceCatalog.PriceCompanyId`** — historically schema drift, added by
  `add-pricecatalog-company-and-backfill.sql` (`ALTER TABLE ... ADD PriceCompanyId tinyint NULL`,
  guarded by `IF NOT EXISTS sys.columns`). **As of the 2026-06-12 squash this column is now part
  of `InitialSchema`** (`InitialSchema.cs` line 665, `tinyint NULL`) and is owned by EF, so the
  script's `ALTER` is a no-op on a current DB — only its backfill matters. `migrate/README.md`
  still lists it as schema drift (caveat #5) and also captured in `../db/schema.sql`; that note
  predates the squash.
- **`add-exam-detail-tables.sql`** — adds tech-exam detail lookup tables not covered by the
  EF migration.

Each backfill script guards against its target column being missing — usually via
`COL_LENGTH('dbo.X','Col') IS NULL` → `RAISERROR('... Apply EF migration <Name> first ...')`.
The migration names those guards print (`AddPaymentCategoryZelenMap`,
`AddRequestLegacyReferenceNumber`, `AddCustomerDebtsAndExpandPriceCatalog`,
`AddVehicleTowingFields`, `AddVehicleManufactureDate`, `AddVehicleMaxLegalGroupMass`) are
**pre-squash names** — those discrete migrations no longer exist on disk. After the 2026-06-12
squash the entire schema (including all those columns) lives in the single `InitialSchema`
migration (`VTE.Infrastructure/Migrations/20260612231002_InitialSchema.cs`), so on a fresh DB
the guards simply pass. The **only** genuinely separate post-squash migration today is
`AddDocumentIssuerCommunityId` (`20260625115717_…`, adds `DocumentIssuer.CommunityId`, required
before the community backfill — see §4.7). All EF migrations auto-apply on API startup
(`Program.cs` `MigrateAsync`, line 190).

---

## 8. Obsolete scripts (kept for history)

| Script | Why retired |
|---|---|
| `OBSOLETE-backfill-pricecatalog-from-paymentitems.sql` | Joined `PaymentItems` (wrong target). `PriceCatalog.Id` maps to `PaymentItemParametars.Id`. |
| `OBSOLETE-refix-pricecatalog-from-parametars.sql` | The corrected re-do; folded into `backfill-pricecatalog-rules.sql`. |
| `OBSOLETE-fix-pricecatalog-orphan-sentinel.sql` | Patched the wrong issue; replaced by `fix-pricecatalog-active-cascade.sql`. |

These document the `PaymentItems`-vs-`PaymentItemParametars` wrong turn — keep them as a
warning, don't run them.

---

## 9. Encoding notes

Macedonian Cyrillic needs care in `sqlcmd`. If seeded/migrated text shows up as `?????`:

- Add `-f 65001` (UTF-8 codepage) to the `sqlcmd` invocation (it's in every script header), or
- Use the `*.utf16.sql` variants (`seed-utf16.sql`, `seed-exam-parts.utf16.sql`,
  `reseed-request-types.utf16.sql`, `fix-part-category-name.utf16.sql`,
  `fix-station-names.utf16.sql`), or
- Run via SSMS, which handles Unicode natively.

The snapshot DB is created with `COLLATE Macedonian_FYROM_90_CI_AS` so Cyrillic compares
correctly inside the snapshot.

---

## 10. Incremental refresh — keeping v2 in sync with live legacy

After the initial bulk load (which was a point-in-time snapshot), the legacy production
system keeps running. Two scripts pull changes via the `VTEZVV_LIVE` linked server.

### 10.1 `migrate-incremental.sql` — additive top-up

**Purely additive: pulls only NEW rows (`Id > current target max`) — never wipes or
mutates existing data** (except the `CustomerDebt` state-sync, see below). Prereqs: the
linked server `VTEZVV_LIVE` exists (`sp_addlinkedserver`, MSOLEDBSQL,
`@provstr='TrustServerCertificate=yes'`) and the API has booted at least once (admin user
present). Requires `SET QUOTED_IDENTIFIER ON` for distributed queries + filtered indexes.
Run: `sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-incremental.sql -b -X -I -f 65001`.

It captures "before" high-water marks (`@bClient`, `@bVeh`, `@bReg`, `@bRel`, `@bReq`, …),
then inserts new rows in FK order:

```
Client → ClientPersonalData → VehicleMaker/Model (lookup top-up) → Vehicle
  → VehicleRegistration → ClientVehicleRelation → Request
  → RequestOwnershipProof / RequestPaymentProof
  → TechnicalExam lookups (top-up) → TechnicalExamReport → ...Detail
  → reference numbers → CustomerDebt
```

It reuses the exact column mappings from `migrate-clients/vehicles/requests.sql`, swapping
the source to `VTEZVV_LIVE.VTEZVV.dbo.*` and adding `Id > @before` guards. Important
details:

- **Vehicle insert fills the later-added columns** (`MaxLegalGroupMassKg`, `MaxTrailer*Kg`,
  `MaxHitchLoadKg`) directly, so new vehicles don't need the separate backfills.
- **Lookup top-ups use `NOT EXISTS (by Id)`** (not an `Id > max` guard) so they catch both
  makers/models added after the snapshot *and* rows the original INNER JOIN skipped because
  their parent wasn't present yet. Lookups run *before* the entities that reference them.
- **Operators → admin** again, with legacy ids stashed in `Note`.
- **CustomerDebt is a state sync, not purely additive** — the Наплата panel. (a) `INSERT`
  every *open* legacy debt (`CustomerFinancialState.Payed=0 AND Active=1`) not yet imported,
  idempotent via the unique-filtered `CustomerDebt.LegacyId`. Historical *paid* rows are not
  imported (that history lives in `PaymentDocument*`; Наплата shows only open items). The
  `DebtOrigin` is derived: `IdDocumentTehnicalExam → 2`, `IdDocument → 1`, traffic-licence
  `→ 4`, permission `→ 5`, IDL `→ 6`, else `0`. (b) `UPDATE Paid/Active` on previously
  imported debts so legacy settlements/stornos close here too — **but only where
  `SettledByLineId IS NULL`**, so a debt billed *natively in v2* is never re-opened just
  because legacy still shows it unpaid (v2 is authoritative for its own payments; see
  `CLAUDE.md` task #99 and [Payments & pricing](05-payments-and-pricing.md)).

The script ends with a per-table "new rows added this run" summary, all inside one
transaction that rolls back on error.

### 10.2 `refresh-changed-2026.sql` — in-place field refresh

Some 2026 records were *edited* in live after the snapshot (plate re-issued, model
corrected, name fixed), and 44 vehicles synced before `ManufactureDate` existed carry a NULL
make-year. This **`UPDATE`s in place** only records reachable from a 2026 request, and only
where a print-relevant field actually differs:

- Pulls live 2026 vehicle fields (`Plate`, `ModelId`, `MakeDate`) and client names
  (`CustomerFirstName/Surname`) via `OPENQUERY(VTEZVV_LIVE, '...')` (the join runs on the
  remote server) into `#liveVeh` / `#liveCli`.
- Refreshes `Vehicle.Plate/ModelId/ManufactureDate` and `Client.FirstName/LastName` where
  they drifted. `ModelId` uses `COALESCE(mdl.Id, v.ModelId)` so a model added in live but
  absent from the v2 lookup never nulls an existing value or breaks the FK.

Re-runnable / idempotent (it only updates true diffs).

### 10.3 The end-to-end refresh → re-lift pipeline

```
1. snapshot-legacy.ps1                 # (optional) refresh VTEZVV_Snapshot from live
2. migrate-incremental.sql             # additive: new clients/vehicles/requests/exams/debts
3. refresh-changed-2026.sql            # in-place: fix edited 2026 plate/model/name/year
4. parity-check-2026.sql               # verify live vs v2 by request Id (read-only)
5. re-lift to prod                     # backup local VTE → gzip → scp → docker cp →
                                       #   RESTORE DATABASE ... WITH REPLACE (see DEPLOYMENT.md)
```

`parity-check-2026.sql` (read-only) stages live vs v2 print fields for every 2026 request
(by Id) and reports count parity, per-field mismatch counts (surname, given name, MB, VIN,
plate, model, year, tech-exam), sample mismatches, and any 2026 requests present in live but
missing in v2. Use it as the gate before lifting to production. `reattach-vte.sql`
re-attaches **both** `VTE.mdf` and `VTEZVV_Snapshot.mdf` to LocalDB (idempotent — skips a DB
that is already attached) after moving files between machines; the `.mdf`/`.ldf` paths are
hard-coded to `C:\Users\filip\…` and must be edited for another machine.

> Production note (`CLAUDE.md`): `LegacySync` is **disabled** in prod
> (`LegacySync__Enabled=false`). The production DB carries real data lifted via `.bak`
> restore from the local `VTE`; the re-lift procedure is in [Deployment](DEPLOYMENT.md).

---

## 11. Quick reference — script index

| Category | Scripts |
|---|---|
| Snapshot / helpers | `snapshot-legacy.ps1`, `snapshot-operators.ps1`, `resnap-payment-details.ps1`, `run-migration.ps1`, `run-requests-migration.ps1`, `reattach-vte.sql` |
| Reference data | `migrate-countries.sql`, `migrate-citizenships.sql`, `migrate-communities.sql`, `migrate-cities.sql`, `migrate-document-issuers.sql`, `migrate-legacy-station.sql`, `migrate-vehicle-lookups.sql` |
| Core entities | `migrate-clients.sql`, `migrate-vehicles.sql`, `migrate-requests.sql`, `migrate-request-catalogs.sql`, `migrate-technical-exams.sql`, `migrate-payments.sql`, `migrate-operators-and-permissions.sql` |
| Entrypoints | `migrate-vtezvv-to-vte.sql` (current — `USE VTE`, reads `VTEZVV_Snapshot`, destructively wipes the reference/client tables then re-inserts); `migrate-from-snapshot.sql` + `migrate-from-legacy.sql` (**VTE2-era**, `USE VTE2`, old plural schema — kept for reference, not part of the current `VTE` pipeline) |
| Backfills | `backfill-vehicle-plate.sql`, `backfill-vehicle-manufacture-date.sql`, `backfill-vehicle-group-mass.sql`, `backfill-vehicle-towing.sql`, `backfill-request-reference-no.sql`, `backfill-payment-category-zelenmap.sql`, `backfill-pricecatalog-rules.sql`, `backfill-document-issuer-community.sql` |
| Schema additions | `add-pricecatalog-company-and-backfill.sql`, `add-exam-detail-tables.sql` |
| Fixes | `fix-pricecatalog-active-cascade.sql`, `fix-tech-exam-org-name.sql`, `fix-station-names.sql`, `fix-part-category-name.sql`, `merge-nina-operator.sql`, `wire-request-operators.sql`, `create-tech-exam-operators.sql` |
| Seed (non-legacy) | `seed-vte2-defaults.sql`, `seed-exam-parts.sql`, `reseed-request-types.sql` (+ `.utf16` variants), `seed-utf16.sql` |
| Performance | `perf-indexes.sql`, `perf-indexes-vte.sql`, `perf-indexes-requests.sql` |
| Incremental / validation | `migrate-incremental.sql`, `refresh-changed-2026.sql`, `parity-check-2026.sql`, `migrate-requests-diag.sql` |
| Obsolete (do not run) | `OBSOLETE-backfill-pricecatalog-from-paymentitems.sql`, `OBSOLETE-refix-pricecatalog-from-parametars.sql`, `OBSOLETE-fix-pricecatalog-orphan-sentinel.sql` |

All paths are relative to `migrate/` at the repo root. The runbook with exact `sqlcmd`
command lines is `migrate/README.md`.
