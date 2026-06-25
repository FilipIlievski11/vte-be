# Technical exams (Технички преглед)

The **TechnicalExam** module is the v2 port of the legacy `DocumentsTehnicalExamsReports` form
(`uxDocumentTehnicalExamReports`). A technical-exam report records a single roadworthiness
inspection of a vehicle: its type (РЕД-12М etc.), the issuing station, the two inspectors
(controllers), the pass/fail outcome, the measured brake-force and emission values, and a list
of defective parts. Saving a report (when it is anchored to a customer+vehicle) auto-creates the
exam-fee debt, and the report can be printed two ways — the certificate (**Уверение/Потврда**)
and the **Записник** stamped onto pre-printed paper.

This document is a maintainer's reference for the entities, the read/write API, the auto-debt
hook, and the two prints. Cross-links: pricing & the debt model live in
[Payments & pricing](05-payments-and-pricing.md); the auto-exam-on-renewal path is owned by the
[Requests](03-requests.md) module.

> Sources are cited inline as repo-relative paths. Every claim below is grounded in code read in
> this repo (controller, domain entities, EF config, debt service, frontend views, prints, and
> the migration scripts).

---

## 1. At a glance

| Thing | Value | Where |
|---|---|---|
| Controller | `TechnicalExamReportsController` | `backend-v2/src/VTE.Api/Controllers/TechnicalExamReportsController.cs` |
| Route prefix | `api/technical-exams` | controller `[Route("api/technical-exams")]`, `[Authorize]` |
| Domain entities | 6 classes | `backend-v2/src/VTE.Domain/TechnicalExams/` |
| EF config | `VteDbContext.OnModelCreating` (lines ~503–582) | `backend-v2/src/VTE.Infrastructure/Persistence/VteDbContext.cs` |
| Debt hook | `IDebtService.CreateDebtsForSourceAsync` on report `Create` | `backend-v2/src/VTE.Infrastructure/Pricing/DebtService.cs` |
| Frontend list | `TechnicalExamsView.vue` | `frontend-v2/src/views/TechnicalExamsView.vue` |
| Frontend detail | `TechnicalExamReportView.vue` | `frontend-v2/src/views/TechnicalExamReportView.vue` |
| Frontend editor | `TechExamFormView.vue` | `frontend-v2/src/views/TechExamFormView.vue` |
| Prints | `TechExamCertificate.vue`, `TechExamZapisnik.vue` | `frontend-v2/src/views/print/` |
| Bulk migration | `migrate-technical-exams.sql` | `migrate/migrate-technical-exams.sql` |

**Station-specific constants (this deployment).** The station's tech-exam organization id is
**37** (АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ). The regular exam type is **РЕД-12М, `TechnicalExamType.Id = 1`,
`ValidDays = 365`**. Any `TechnicalExamTypeId > 1` is treated as *irregular* (вонреден) for debt
routing — see [§7](#7-the-debt-hook-irregular-vs-regular) (gotcha #10 / #16 in `CLAUDE.md`).

---

## 2. Domain entities

All six POCOs live in `backend-v2/src/VTE.Domain/TechnicalExams/`. Only `TechnicalExamReport`
is tenant-owned (`ITenantOwned`); the four lookups and the detail line are not.

### 2.1 `TechnicalExamReport` — the exam header

`TechnicalExamReport.cs`. The aggregate root. Legacy source `DocumentsTehnicalExamsReports`.
Improvements over legacy noted in the entity doc-comment: measurements are nullable (a real
"not measured" instead of a `0` sentinel), dates are `DateOnly`, lookups are real FKs, the row
is tenant-scoped, and the dead `LastChanged`/VisualErrors columns are dropped.

| Property | Type | Notes / legacy column |
|---|---|---|
| `Id` | `long` | identity |
| `CompanyId` | `byte` | tenant key (`ITenantOwned`) |
| `CustomerVehicleRelationId` | `long?` | the inspected customer+vehicle (anchor). **Nullable** — 131 legacy reports point at a deleted relation and migrate as NULL. Legacy `IdCustomerVehicleRelation` |
| `TechnicalExamTypeId` | `int` | FK → `TechnicalExamType`. Legacy `IdTypeOfTehnicalExam` |
| `OrganizationId` | `int` | FK → `TechnicalExamOrganization` (issuing station). Legacy `IdOrganizationForTehnicalExam` |
| `RegNumber` | `string?` | report number, format `"{OrganizationId}-{seq}/{year}"`. Legacy `RegNumber` |
| `MadeDate` | `DateOnly` | exam date |
| `ValidTillDate` | `DateOnly` | `MadeDate + TechnicalExamType.ValidDays` |
| `FirstControllerLegacyId` | `int?` | first inspector — legacy VTESecurity user id (`0 → null`). Legacy `IdFirsControler` |
| `SecondControllerLegacyId` | `int?` | second inspector. Legacy `IdSecondControler` |
| `VehicleIsRight` | `bool` (default `true`) | pass (`true`) / fail (`false`). Legacy `VehicleIsRight` |
| `ExplanationNote` / `DriversWarning` / `Note` | `string?` | free-text notes (each max 500) |
| `TechnicalChanges` | `string?` | `nvarchar(max)` — recorded technical modifications |
| Brake-force grid | `double?` ×25 | 4 axles + parking; see below |
| Summary measurements | `double?` ×13 | weight, brake effects, emissions; see below |
| `Active` | `bool` | soft-delete flag |
| `CreatedAt` / `ModifiedAt` | `DateTime` / `DateTime?` | audit |
| `RowVersion` | `byte[]` | EF concurrency token (`IsRowVersion()`) |

**Brake-force grid** — 5 rows (axle 1–4 + parking brake), each with 5 columns. The column
mapping is documented in the entity:

| Column | Meaning | Legacy source column (per axle) |
|---|---|---|
| `Axis{n}Left` / `Axis{n}Right` | measured force per wheel | `Axis{n}Left` / `Axis{n}Right` |
| `Axis{n}Gj` | legacy "Gj" (imbalance/limit), kept verbatim | `Axis{n}Gj` |
| `Axis{n}LeftRightDiff` | left/right difference % | `Axis{n}LeftPj` |
| `Axis{n}Coefficient` | efficiency coefficient % | `Axis{n}PN` |

The parking-brake row uses the `AxisParking*` properties. In the API and both frontends the
parking row is keyed as **axle `0`** (see `Get` building the `axles` list and `blankAxles()` in
the form).

**Summary measurements** (all `double?`, all `NULLIF(...,0)` in migration):

| Property | Legacy column |
|---|---|
| `Weight` | `Waight` (typo preserved in legacy) |
| `EffectOfWorkingBrakeEmpty` | `EffectOfWorkingBreakEmpty` |
| `EffectOfWorkingBrakeFull` | `EffectOfWorkingBreakFull` |
| `EffectOfSecondaryBrake` | `EffectOfSecondaryBreak` |
| `EffectOfParkingBrake` | `EffectOfParkingBreak` |
| `SpeedOfTurns` | `SpeedOfTurns` |
| `CO` | `CO` |
| `EngineRpm` | `NumEngineTurns` |
| `COPlusTurns` | `COPlusTurns` |
| `Lambda` | `Lambda` |
| `Pinpoints` | `Pinpoints` |
| `Noise` | `Noise` |
| `EngineOilTemp` | `TempOfEngineOil` |

**Business rules** (restated from the entity doc-comment, all enforced in the controller):
- `RegNumber = "{OrganizationId}-{seq}/{year}"`.
- `ValidTillDate = MadeDate + TechnicalExamType.ValidDays`.
- `VehicleIsRight = true` ⇔ every detail line is status `1` (исправен), else `false`.

### 2.2 `TechnicalExamReportDetail` — one defective-part line

`TechnicalExamReportDetail.cs`. Legacy `DocumentsTehnicalExamsReportsDetails`. **Not** tenant-owned
(scoped through its parent report).

| Property | Type | Notes / legacy column |
|---|---|---|
| `Id` | `long` | |
| `TechnicalExamReportId` | `long` | FK → report (cascade delete). Legacy `IdTehnicalExamsReports` |
| `VehiclePartId` | `int` | FK → `TechnicalExamVehiclePart`. Legacy `IdTehnicalExamVehivlePart` (typo preserved) |
| `StatusId` | `int` | FK → `TechnicalExamDetailStatus`. Legacy `IdStatus` |
| `Front` / `Back` / `OnLeft` / `OnRight` | `bool` | where on the vehicle the issue was found |
| `EnteredAt` | `DateTime` | Legacy `DateEnter` |
| `Note` | `string?` | max 300 |
| `Active` | `bool` | |

### 2.3 Lookups

| Entity | File | Legacy table | Key columns |
|---|---|---|---|
| `TechnicalExamType` | `TechnicalExamType.cs` | `TehnicalExamsTypes` | `Code` (e.g. "РЕД-12М"), `Description`, `ValidDays` (legacy `ValidNumOfDays`), `PercentOfFullExam`, `IsInRegister` (legacy `IsInRegistar`), `Active` |
| `TechnicalExamDetailStatus` | `TechnicalExamDetailStatus.cs` | `DocumentsTehnicalExamsReportsDetailsStatus` | `Name`. **1 = исправен (OK), 2 = вратен (returned/pending), 3 = неисправен (defective)** |
| `TechnicalExamVehiclePart` | `TechnicalExamVehiclePart.cs` | `TehnicalExamVehicleParts` | `CategoryId` (legacy `IdCategoryVehicleParts`, plain int — no lookup table), `Code`, `Description` |
| `TechnicalExamOrganization` | `TechnicalExamOrganization.cs` | `TehnicalExamOrganizations` | letterhead fields — see below |

`TechnicalExamOrganization` is deliberately kept as its own lookup (not reused from `Station`)
"so legacy report → org Ids migrate 1:1" (entity doc-comment). Letterhead/print columns:
`Code`, `Name` (legacy `Station`), `CityId`/`CommunityId` (loose legacy ids, indexed, no FK),
`Address` (legacy `StationAddress`), `Phone` (`Tel`), `Fax`, `BankAccount` (`ZiroSmetka`),
`Depositor` (`Deponent`), `TaxNumber` (`EDB`), `ResponsibleOfficer` (`OdgovorenOrgan`),
`Secretary` (`Sekretar`), `CompanyId` (`IdCompany`, `0 → null`).

### 2.4 EF configuration & query filter

`VteDbContext.cs` (~lines 503–582). Tables are EF-owned, created by the `AddTechnicalExams`
migration; bulk historical data is loaded separately by `migrate-technical-exams.sql`. Table
names are **singular** (`TechnicalExamReport`, `TechnicalExamReportDetail`, …). Notable config:

- `TechnicalExamReport` and `TechnicalExamReportDetail` use `bigint` ids.
- FKs `Company`, `ClientVehicleRelation` (the anchor — optional, see the nullable note), `TechnicalExamType`, `TechnicalExamOrganization` are all `OnDelete(DeleteBehavior.Restrict)`.
- `TechnicalExamReportDetail → TechnicalExamReport` is `Cascade`; its part/status FKs are `Restrict`.
- Indexes: report → `CustomerVehicleRelationId`, `RegNumber`, `(CompanyId, MadeDate)`; detail → `TechnicalExamReportId`.
- **Tenant filter** on the report only:
  ```csharp
  e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
  ```
  Note this filter does **not** include `Active` — soft-deleted reports are excluded explicitly
  in the list query (`if (!includeInactive) query = query.Where(r => r.Active)`), not by the
  global filter.

> Dead script: `migrate/add-exam-detail-tables.sql` predates the current schema (it targets a
> `VTE_Modern` DB, uses **plural** table names and a `TechnicalExamReportVisualErrors` table that
> was dropped). It is superseded by the EF migration + `migrate-technical-exams.sql` and should be
> ignored.

---

## 3. The read API

All endpoints are under `api/technical-exams` and require auth. DTOs are declared as nested
`record`s inside the controller.

| Verb & path | Method | Purpose |
|---|---|---|
| `GET /api/technical-exams` | `List` | paged register (search + filters + sort) |
| `GET /api/technical-exams/{id}` | `Get` | full single report (header + measurements + defects) |
| `GET /api/technical-exams/types` | `Types` | active exam types (register filter) |
| `GET /api/technical-exams/{id}/print` | `Print` | certificate (Уверение/Потврда) bundle |
| `GET /api/technical-exams/{id}/zapisnik` | `Zapisnik` | Записник bundle |
| `GET /api/technical-exams/organizations` | `Organizations` | org/station picker lookup |
| `GET /api/technical-exams/detail-statuses` | `DetailStatuses` | defect-status picker lookup |
| `GET /api/technical-exams/vehicle-parts` | `VehicleParts` | defective-part picker lookup |
| `GET /api/technical-exams/controllers` | `Controllers` | inspector picker lookup |
| `POST /api/technical-exams` | `Create` | create (+ debt hook) |
| `PUT /api/technical-exams/{id}` | `Update` | update (replaces detail lines) |
| `DELETE /api/technical-exams/{id}` | `Delete` | soft-delete |

### 3.1 `GET /api/technical-exams` — the register (list)

Query params (`List(...)`): `q`, `result` (`"all"|"pass"|"fail"`, default `"all"`), `typeId`,
`customerVehicleRelationId`, `includeInactive` (default `false`), `sort`, `dir`, `page` (1-based),
`pageSize` (1–200, default 50). Returns `PagedDto<TechExamListItem>`.

- **Inactive filter:** unless `includeInactive`, `query.Where(r => r.Active)`.
- **Result filter:** `pass → VehicleIsRight`, `fail → !VehicleIsRight`.
- **Search (`q`)** is a multi-table `LIKE` that matches: the report `RegNumber`, **or** the
  anchor relation resolved from a matching client (`FirstName`/`MiddleName`/`LastName`/`MB`)
  **or** a matching vehicle (`Vin`/`Plate`). It builds two relation-id subqueries (`relationsByClient`,
  `relationsByVehicle`), `Union`s them, and matches `CustomerVehicleRelationId` against the union.
- **Sort** keys: `id`, `regnumber`, `validtill`, `result` (`VehicleIsRight`), `type` (by the
  type's `Code` via subquery); default `made` (`MadeDate`) descending (newest first).
- The page rows are fetched first, then joins (type, org, relation→client/vehicle) are
  **batch-resolved** into dictionaries to avoid N+1, and finally projected to `TechExamListItem`
  (`Id, CompanyId, RegNumber, MadeDate, ValidTillDate, TechnicalExamTypeId, TypeCode, TypeName,
  OrganizationId, OrganizationName, CustomerVehicleRelationId, ClientName, VehiclePlate,
  VehicleVin, VehicleIsRight, Active`).

### 3.2 `GET /api/technical-exams/{id}` — full report

Returns `TechExamReportFullDto` or `404`. Resolution steps (`Get`):
1. Load the report (tenant-filtered).
2. Type (`Code`, `Description`, `ValidDays`) and org (`Name`, `Code`).
3. **Client + vehicle via the anchor relation** — relation → client (name + `MB`) and →
   vehicle (`Plate`, `Vin`, and `Maker Model` resolved through `VehicleModel`/`VehicleMaker`).
4. **Defect lines** ordered by `Id`, with part `Code`/`Description` and status `Name`
   batch-resolved.
5. **Controllers → names.** The two `*ControllerLegacyId` ints are looked up against
   `AspNetUsers.Id` (the recreated operators are keyed `AspNetUsers.Id == legacy id`, see
   [§6](#6-controllers-inspectors)); resolves to `FullName ?? UserName`.
6. **Brake-force grid → 5 `AxleReadingDto` rows**: axles 1–4 then the parking row keyed
   `axle = 0` (`AxisParking*`).

### 3.3 Lookups

`Types` returns active types ordered by `Id` (`TechExamTypeDto: Id, Code, Description, ValidDays`).
`Organizations`/`DetailStatuses`/`VehicleParts` return only `Active` rows. `Controllers` is special —
see [§6](#6-controllers-inspectors).

---

## 4. Create / Update / Delete

### 4.1 Write DTO

`TechExamWriteDto` (controller) / `TechExamWrite` (`frontend-v2/src/types.ts`). The header fields,
all 25 brake-grid values, all 13 summary measurements, and a list of `TechExamDetailWriteDto`
(`VehiclePartId, StatusId, Front, Back, OnLeft, OnRight, Note`). `RegNumber`, `ValidTillDate`,
`VehicleIsRight`, and the audit fields are **server-derived**, never accepted from the client.

### 4.2 Shared helpers

- `ValidateWriteAsync` — returns a Macedonian error string (or `null`):
  - invalid/unknown `TechnicalExamTypeId` → `"Невалиден тип на технички преглед."`
  - invalid/unknown `OrganizationId` → `"Невалидна организација/станица."`
  - a supplied `CustomerVehicleRelationId` that doesn't exist → `"Врската сопственик–возило не постои."`
- `DerivePass(details)` — `true` when there are no lines **or** every line has `StatusId == 1`
  (исправен). This is the single source of truth for `VehicleIsRight`; the frontend mirrors it
  (`vehicleIsRight` computed in both `TechnicalExamReportView` and `TechExamFormView`).
- `ApplyMeasurements(report, dto)` — copies the brake grid + summary values onto the entity.

### 4.3 `POST /api/technical-exams` — `Create`

1. Requires `_tenant.UserId` (else `401`); validates (else `400 { error }`).
2. `validDays = type.ValidDays`; `ValidTillDate = MadeDate.AddDays(validDays > 0 ? validDays : 365)`
   — note the **365-day fallback** when the type has no validity period.
3. **RegNumber generation.** Reads the latest `RegNumber` for the same org (`StartsWith("{org}-")`,
   ordered by `Id` desc), parses the sequence between `-` and `/`, increments it, and forms
   `"{OrganizationId}-{seq}/{DateTime.UtcNow.Year}"`. The sequence is per-organization. (The
   request auto-exam path in `RequestsController.TryAutoCreateExamAsync` deliberately mirrors this
   exact format and shares the same sequence space.)
   > Caveat: the sequence is read **outside any lock/transaction**, so two concurrent creates for
   > the same org could collide on `seq`. There is no unique constraint on `RegNumber` (it is
   > indexed, not unique).
4. **First-controller default:** if the client didn't pass `FirstControllerLegacyId`, it defaults
   to the caller's own user id when that parses as an int (`int.TryParse(_tenant.UserId, out uid)`).
5. `CompanyId = _tenant.CompanyId ?? 4`. Inserts the report, then (separately) the detail lines.
6. **Debt hook** — see [§7](#7-the-debt-hook-irregular-vs-regular).
7. Returns the freshly-loaded full report by delegating to `Get(report.Id)`.

### 4.4 `PUT /api/technical-exams/{id}` — `Update`

Loads (tracked) the report or `404`; validates. Re-applies header + measurements, recomputes
`ValidTillDate` and `VehicleIsRight` (via `DerivePass`), sets `ModifiedAt`, and **replaces** the
detail lines wholesale (`RemoveRange(old)` then re-add from the DTO). Returns `204 No Content`.

> The debt hook does **not** fire on `Update` — only on `Create`. Editing an exam will not
> recreate or reconcile its debt (Phase 4+ concern; the debt service is idempotent per source
> anyway).

### 4.5 `DELETE /api/technical-exams/{id}` — `Delete`

Soft-delete: sets `Active = false`, saves, returns `204`. (Consistent with the project-wide
soft-delete rule in `CLAUDE.md`.)

---

## 5. Frontend

Routes (`frontend-v2/src/router/index.ts`):

| Path | Name | Component |
|---|---|---|
| `/technical-exams` | `technical-exams` | `TechnicalExamsView.vue` |
| `/technical-exams/new` | `technical-exam-new` | `TechExamFormView.vue` |
| `/technical-exams/:id` | `technical-exam` | `TechnicalExamReportView.vue` |
| `/technical-exams/:id/edit` | `technical-exam-edit` | `TechExamFormView.vue` |
| `/technical-exams/:id/print` | `technical-exam-print` | `print/TechExamCertificate.vue` |
| `/technical-exams/:id/zapisnik` | `technical-exam-zapisnik` | `print/TechExamZapisnik.vue` |

The two print routes sit **outside** the `AppLayout` shell (no sidebar/chrome) so they render as
bare paper pages.

### 5.1 `TechnicalExamsView.vue` — the register

A dense `DataTable` (28px rows, `.8125rem`). A `SelectButton` toggles result (`all/pass/fail`), a
debounced (300ms) search box drives `q`, a `Select` filters by type (loaded from `/types`). Lazy
server-side paging (`[25,50,100,200]`) and server-side sort. Columns: reg-number (mono), client,
vehicle (plate + VIN), type (code), made-date, valid-till, and result rendered as a `Tag`
(`success`/`danger`). Clicking a row navigates to the detail view; the toolbar "new" button goes
to `/technical-exams/new`.

### 5.2 `TechnicalExamReportView.vue` — read-only detail

Cards: **header** (owner + `MB`, vehicle plate/VIN/maker-model, type, station, reg-number, dates,
both controllers), **measured values** (brake-force grid + summary measurements, both filtering out
all-empty rows/values so historical empty reports stay clean; `technicalChanges` shown if present),
**defective parts** (a `DataTable`; status as a colour-coded `Tag` — `success` for id 1, `danger`
for id 3, `warn` otherwise; front/back/left/right as check icons), and **notes**
(explanation/driver-warning/note, shown only if present).

Toolbar buttons: **Edit**, **Печати записник** (`printZapisnik` → `techExam.printZapisnik`), and
**Печати потврда** (`printCertificate` → `techExam.printCertificate`). The certificate button is
**disabled when `!vehicleIsRight`** (with a tooltip hint, `techExam.printDisabledHint`) — a failing
vehicle gets no certificate. Both prints open in a new tab (`window.open(..., '_blank')`).

### 5.3 `TechExamFormView.vue` — create/edit

One component for both new and edit (`isEdit = !!props.id`). On mount it loads all catalogs in
parallel (`types`, `organizations`, `controllers`, `vehicle-parts`, `detail-statuses`); in edit
mode it then hydrates from `GET /{id}`.

- **Header card:** type, station (`filter`able `Select`), made-date (`DatePicker`, `yy-mm-dd`),
  a read-only computed **valid-till** preview (`MadeDate + validDays`, 365 fallback), both
  controllers (`filter`/`showClear` `Select`s bound to the `controllers` lookup), and the
  read-only reg-number (edit only).
- **Anchor card:** in *new* mode, a debounced client search (`GET /clients?q=`) → on pick,
  `GET /client-vehicle-relations?clientId=&activeOnly=true` populates the relation `Select`. In
  *edit* mode client + relation are **locked** (you cannot move an exam to a different anchor).
- **Measured values card:** collapsible (collapsed by default). The 5×5 brake-force grid of
  `InputNumber`s (parking row keyed axle `0`) plus a 3-column grid of the 13 summary measurements,
  and the `technicalChanges` textarea.
- **Defects card:** add/remove rows; each row picks part + status and toggles front/back/left/right.
- **Notes card:** explanation / drivers-warning / note.

Client-side `validate()` requires type, org, made-date, a relation, and complete defect lines
(part + status). The live pass/fail `Tag` and valid-till preview update as you edit. `save()`
POSTs (then `router.replace` to the new id) or PUTs (then navigates to the detail). `remove()`
confirms then soft-deletes.

### 5.4 Locale terms

UI strings are under the `techExam.*` key namespace in `frontend-v2/src/locales/{mk,en}.ts`
(e.g. `techExam.pass`/`fail`, `techExam.printCertificate`, `techExam.printZapisnik`, brake-grid
column headers, axle labels). Add both MK and EN keys for any new string.

---

## 6. Controllers (inspectors)

In legacy, `DocumentsTehnicalExamsReports.IdFirsControler`/`IdSecondControler` point at
`VTESecurity.dbo.Users` — a separate security DB that is **not** part of the v2 migration. v2
therefore stores them as raw ints (`FirstControllerLegacyId`/`SecondControllerLegacyId`).

`migrate/create-tech-exam-operators.sql` recreates each referenced controller as a v2 ASP.NET
Identity user with the **Operator** role, **keyed so reports wire up directly**:

```
AspNetUsers.Id = CAST(VTESecurity.Users.ID AS nvarchar)   -- e.g. "122"
```

so `report.FirstControllerLegacyId.ToString() == AspNetUsers.Id`. The API then resolves the
display name from `AspNetUsers.FullName`. Details from the script:
- `UserName = "{legacyUserName}.{id}"` to dodge collisions (legacy has a duplicate "ААА" and a
  "nina" that clashes with an existing v2 user).
- `PasswordHash = NULL` → these accounts cannot log in until an admin sets a password.
- `IsActive` mirrors the legacy `Active` flag; it reads VTESecurity over the `VTEZVV_LIVE`
  linked server and is re-runnable.

Two places consume this wiring:
- `Get` / `Print` resolve `*ControllerLegacyId` → name against `_db.Users` (`FullName ?? UserName`).
- `GET /controllers` (`ControllerLookupDto: Id, FullName`) lists users with the **OPERATOR** role
  (`Roles.NormalizedName == "OPERATOR"`) and `IsActive`, **keeping only those whose `Id` parses
  as an int** (`int.TryParse(u.Id, out n)`) — because the report stores controllers as legacy ints.
  The numeric id is returned, ordered by name.

---

## 7. The debt hook (irregular vs regular)

Mirrors the legacy `AddDeptsToCustomer` path that fired on every tech-exam save. It runs **only in
`Create`**, and **only when the report is anchored** to a relation (`CustomerVehicleRelationId.HasValue`)
— an orphan exam has no one to charge. The hook is wrapped in `try/catch` and is **best-effort**:
a debt failure must not roll back the saved exam (a comment notes Phase 5 will move this to a
transactional outbox). Exact call (`TechnicalExamReportsController.Create`):

```csharp
var isIrregular = report.TechnicalExamTypeId > 1;   // legacy convention: type=1 is regular
await _debts.CreateDebtsForSourceAsync(
    origin:                     isIrregular ? DebtOrigin.TechnicalExamIrregular : DebtOrigin.TechnicalExam,
    originId:                   report.Id,
    customerVehicleRelationId:  report.CustomerVehicleRelationId.Value,
    organizationId:             report.OrganizationId,
    trigger:                    isIrregular ? PriceTrigger.TechnicalExamIrregular : PriceTrigger.TechnicalExam,
    communityId:                null,
    note:                       $"технички преглед бр. {report.RegNumber}");
```

This is **gotcha #10**: `TechnicalExamTypeId > 1` ⇒ `DebtOrigin.TechnicalExamIrregular` +
`PriceTrigger.TechnicalExamIrregular`; the regular РЕД-12М (`Id = 1`) uses the plain
`TechnicalExam` origin/trigger.

`DebtService.CreateDebtsForSourceAsync` (`backend-v2/src/VTE.Infrastructure/Pricing/DebtService.cs`):
1. **Idempotency** — for a tech-exam origin it checks `CustomerDebt` for an existing row with the
   same `Origin` and `OriginTechnicalExamId == originId`; if any exists, it returns `0` (no-op).
   (`CustomerDebt.OriginTechnicalExamId` is the legacy `IdDocumentTehnicalExam`; the entity carries
   a filtered — **not unique** — index `HasFilter("[OriginTechnicalExamId] IS NOT NULL")` in
   `VteDbContext.cs` purely to speed this lookup. The idempotency is enforced in code, not by a DB
   constraint.)
2. Loads the relation + its vehicle, feeds them to `IPricingEvaluator.EvaluateAsync(trigger, …)`.
3. Inserts one `CustomerDebt` per matched `PriceCatalog` rule, snapshotting `Price` + `VatPercent`,
   stamping `OriginTechnicalExamId = report.Id`, `OrganizationId`, `CreatedByUserId = tenant.UserId`.

For the pricing rule model, the `PriceTrigger` enum, and how matches are evaluated, see
[Payments & pricing](05-payments-and-pricing.md).

### 7.1 Auto-created exams from request renewals

The [Requests](03-requests.md) module can auto-create a tech-exam on a renewal when the
`Requests:AutoCreateTechExam` config flag is on
(`RequestsController.TryAutoCreateExamAsync`). It reads the exam-type id from the overloaded
`RequestType.TechnicalExamRequirement` column (gotcha #16: the prod migration backfilled the raw
legacy `IsTehnicalExamRequired` integer — which is actually the **exam-type id** — into this enum
column, so values like `9` aren't named enum members; read it as `(int)`). If that carried id isn't a
known `TechnicalExamType`, it **falls back** to `Requests:DefaultTechnicalExamTypeId` (default `1`);
if the resolved id still doesn't exist it bails. It resolves the station from
`Requests:DefaultTechnicalExamOrganizationId` (must be a real org id, else it bails — no default
org), generates the RegNumber with the **same formula and sequence space** as `Create`, anchors to
the **new owner** on a transfers-ownership type (`NewClientVehicleRelationId`) else the current
relation, defaults `VehicleIsRight = true`, back-links the request
(`entity.TechnicalExamReportId = exam.Id`), and fires the **identical** debt hook (`typeId > 1`
⇒ irregular). The exam-fee debt is therefore owned by the exam's own id, not the request's. The whole
call is wrapped in `try/catch` at the call site (`RequestsController` ~lines 331-332), so an
auto-exam failure never fails the request save.

The defaults live in `backend-v2/src/VTE.Api/appsettings.json` (`Requests` section):
`AutoCreateTechExam: true`, `DefaultTechnicalExamTypeId: 1`, `DefaultTechnicalExamOrganizationId: 37`
— i.e. the РЕД-12М type and this station's org id.

---

## 8. The two prints

### 8.1 Certificate — Уверение / „Потврда за техничка исправност"

Endpoint `GET /api/technical-exams/{id}/print` → `TechExamCertificateDto`; rendered by
`frontend-v2/src/views/print/TechExamCertificate.vue` on a blank **A4** page.

Server bundle: report basics (`RegNumber`, `MadeDate`, `ValidTillDate`, `VehicleIsRight`), the
station city, the first controller's name (for the signature), the issuing **organization
letterhead** (`CertOrgDto: Name, Address, CityLine, Phone, Fax` — `CityLine` is `"{PostalCode}
{Name}"` resolved through `Cities`), and the **vehicle block** keyed to the registration-document
field codes (`CertVehicleDto`): **A** registration plate, **Ј** category+vid (`Code Name`),
**D.1** maker, **D.2** type/variant (`Vehicle.TypeText`), **D.3** commercial model, **E** VIN.

Frontend behavior: it renders the legal preamble (Закон за возила, член 48/67/52), the **ПОТВРДА**
title, the reg-number, the A/Ј/D.1/D.2/D.3/E vehicle table, the compliance statement, the next-exam
date (`ValidTillDate`, formatted `dd.mm.yyyy`), and the signature block (controller name + place &
date). It **auto-opens the print dialog only when `vehicleIsRight === true`** (matching the legacy
rule that the certificate is issued only for a compliant vehicle); a failing vehicle shows a
no-print warning instead.

### 8.2 Записник — „Записник за технички преглед"

Endpoint `GET /api/technical-exams/{id}/zapisnik` → `ZapisnikDto`; rendered by
`frontend-v2/src/views/print/TechExamZapisnik.vue`, **stamped onto pre-printed Letter paper**
(215.9 × 279.4 mm) at absolute mm coordinates extracted from the legacy `.prnx` render (mirrors
legacy `rptTehnickiPregledZapisnik`).

Server bundle (`ZapisnikDto`): `RegNumber`, `MadeDate`, `TechnicalExamTypeId`, `IsSocial`,
org name, **customer** (full name, city, community, living address), and **vehicle**
(plate, maker, `ModelFull` = `model + variant + "ТНГ" if HasLpg`, make-year, made-country, color
`"{Code}-{Name}"`, VIN, `EngineTypeAndNum` = `"{engineCode}/{engineNumber}"`, capacity cc, power
kW, empty/max weight kg, axle count, seats). Notes from the controller code:
- `IsSocial` is the best-available proxy: `Client.Business == true` → "social"
  (правно лице/company), individual → "private" (физичко лице). This selects the ownership
  checkbox on the form.
- `PropulsionAxis` is **not** modelled in v2 → always `null` (the frontend prints it as `0`).
- The frontend's `num()` prints `0` for null/blank (legacy stamps the raw numeric, so unset → `0`).
- Owner address is upper-cased and the first space-digit run gets a `"БР."` prefix, then the
  community is appended → e.g. `"БРАТСТВО БР.21 ГОРНО ОРИЗАРИ, Велес"`.
- The exam-type checkbox is chosen by `Select Case TechnicalExamTypeId` 1–4 →
  `redoven` / `redovenNa6` / `delumno` / `potpoln` (others → none). The ownership checkbox is
  `social` vs `private` from `IsSocial`.

Both prints auto-fire `window.print()` shortly after load (the certificate only when compliant),
and offer manual **Печати** / **Затвори** buttons that are hidden under `@media print`.

---

## 9. Migration & data scale

`migrate/migrate-technical-exams.sql` (single transaction, `IDENTITY_INSERT` per table, re-runnable
via `NOT EXISTS` guards) loads, in order: `TechnicalExamType` (from `TehnicalExamsTypes`),
`TechnicalExamDetailStatus`, `TechnicalExamVehiclePart`, `TechnicalExamOrganization`,
`TechnicalExamReport` (the ~107k-row history; every measurement wrapped in `NULLIF(...,0)`;
`CustomerVehicleRelationId` left NULL when the legacy relation no longer exists), and
`TechnicalExamReportDetail`. All reports are stamped `CompanyId = 4`. `create-tech-exam-operators.sql`
then recreates the inspectors (see [§6](#6-controllers-inspectors)). Production carries ~100k
tech-exams in the real dataset (`CLAUDE.md`).

---

## 10. Quick gotchas recap

- **Irregular = `TechnicalExamTypeId > 1`** → `DebtOrigin.TechnicalExamIrregular` +
  `PriceTrigger.TechnicalExamIrregular` (gotcha #10). Regular РЕД-12М is type id `1`.
- **Debt hook fires on `Create` only**, only when anchored, and is best-effort (`try/catch`).
- **Pass/fail is derived**, never sent by the client: no lines or all status-1 ⇒ pass
  (`DerivePass`).
- **Certificate needs a compliant vehicle** — the FE disables the button and only auto-prints
  when `vehicleIsRight`.
- **Controllers are legacy ints keyed to `AspNetUsers.Id`**; the `/controllers` lookup only returns
  operators whose id parses as an int.
- **Edit can't re-anchor** the exam (client/relation locked in the form) and **doesn't touch the
  debt**.
- **RegNumber** `"{org}-{seq}/{year}"` is per-org and generated without a lock — concurrent
  same-org creates could theoretically collide (no unique constraint).
- Station org id **37**; РЕД-12М type id **1**, `ValidDays = 365`.
