# Domain data model

This is the authoritative reference for VTE's persistence model: every domain entity, its columns, types, relationships, and the EF Core mapping that produces the SQL Server schema. The POCO entities live in `backend-v2/src/VTE.Domain/**` (no EF dependency); their mapping lives entirely in one file, `backend-v2/src/VTE.Infrastructure/Persistence/VteDbContext.cs` (`OnModelCreating`). The schema is owned by EF migrations under `backend-v2/src/VTE.Infrastructure/Migrations/`. Read this alongside [Overview & architecture](01-overview-and-architecture.md) §4 (how `CompanyId` + the global query filter work) and [Payments, pricing & debts](05-payments-and-pricing.md) (how the pricing/debt tables are actually used).

> **Authoring note:** every claim below is grounded in the entity `.cs` files and the `VteDbContext` configuration as read on 2026-06-25. Where a field's meaning is subtle (overloaded columns, loose/no-FK links, legacy-sentinel handling) the inline citation points at the exact source.

---

## 1. Conventions that apply across the whole model

These patterns recur everywhere, so they are stated once.

| Concept | How it shows up | Source |
|---|---|---|
| **Tenant ownership** | Business entities implement `ITenantOwned` (a marker interface exposing `byte CompanyId`). | `VTE.Domain/Common/ITenantOwned.cs` |
| **Global query filter** | Tenant-owned entities get `e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId)`. Admins bypass; operators are auto-scoped. You never write `.Where(CompanyId == …)` in controllers. The eight explicitly-filtered roots are `Station`, `Client`, `Vehicle`, `Request`, `TechnicalExamReport`, `InstallmentAgreement`, `PaymentDocument`, `CustomerDebt`. | `VteDbContext.cs` (lines 122, 205, 333, 457, 568, 637, 664, 725) |
| **Soft delete** | Nearly every entity carries `bool Active` (some `bool?`). Business records are deactivated, not hard-deleted. Note: the `Active` flag is **not** part of the global query filter for most entities — only specific filtered indexes (e.g. `IX_Request_Open`, `IX_CustomerDebt_Open`) reference it. | entity `.cs` files |
| **`tinyint` PKs** | Small lookup/catalog tables and the tenant key use `byte` PKs mapped to `tinyint` (max 255 rows). Mid-size lookups use `short`→`smallint` or `int`. Transactional tables use `long`→`bigint`. See §10 for the full id-type table. | `VteDbContext.cs` `HasColumnType` calls |
| **Loose (no-FK) links** | Some id columns reference legacy ids that may not exist in the v2 lookup, so they have **no FK constraint** (most are also indexed): `VehicleMaker.CountryId` (indexed), `DocumentIssuer.CommunityId` (indexed), `TechnicalExamOrganization.CityId` (indexed) and `TechnicalExamOrganization.CommunityId` (column only, **not** indexed). | `VteDbContext.cs` lines 172-173, 246, 542-543 |
| **Delete behavior** | Parent→child uses `Cascade` (e.g. `ClientPersonalData`, request/exam/payment children); FK→lookup and FK→tenant uses `Restrict`. | `VteDbContext.cs` per-entity `OnDelete(...)` |
| **Concurrency** | Heavy transactional roots carry `byte[] RowVersion` mapped `IsRowVersion()`: `Request`, `TechnicalExamReport`, `PaymentDocument`. | `VteDbContext.cs` 434, 556, 650 |
| **Legacy traceability** | Migrated rows often keep a `LegacyId`, each with a filtered index `WHERE [LegacyId] IS NOT NULL`. Only `CustomerDebt.LegacyId` is **unique** (line 723) — that's the one that makes legacy-sync idempotent. `PaymentDocument.LegacyId`'s index is filtered but **not** unique (line 662). | `VteDbContext.cs` 662, 723 |

`ITenantContext` (resolved from JWT claims) supplies `IsAdmin` and `CompanyId` to the filter; it is injected into `VteDbContext` via the constructor (`VteDbContext.cs` 19-25).

---

## 2. Identity, Companies, Stations

### `Company` — the tenant
`VTE.Domain/Companies/Company.cs` → table `Company`.

| Column | Type | Notes |
|---|---|---|
| `Id` | `tinyint` IDENTITY | the tenant key referenced by every `CompanyId` |
| `Name` | `nvarchar(255)` required | |
| `CreatedAt` | `datetime2` | defaults `DateTime.UtcNow` |
| `Active` | `bit` | |

`Company` is **not** itself tenant-filtered (it's the tenant). Every `ITenantOwned` entity has a `Restrict` FK to it.

### `Station`
`VTE.Domain/Stations/Station.cs` → table `Station`. Implements `ITenantOwned`.

| Column | Type | Notes |
|---|---|---|
| `Id` | `smallint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict); tenant-filtered |
| `Name` | `nvarchar(200)` required | |
| `Active` | `bit` (`bool?`) | |

### `ApplicationUser` / `ApplicationRole`
`VTE.Domain/Identity/`. `ApplicationUser : IdentityUser`, `ApplicationRole : IdentityRole`, stored in the standard `AspNet*` tables (the context is `IdentityDbContext<ApplicationUser, ApplicationRole, string>`).

`ApplicationUser` extends the Identity base with:

| Column | Type | Notes |
|---|---|---|
| `CompanyId` | `tinyint` nullable | tenant discriminator on `AspNetUsers`. **No FK to `Company`** — tenancy is enforced by the JWT claim + query filter, not a constraint (`VteDbContext.cs` 91-96). `null` for system Administrators. |
| `FullName` | `nvarchar(200)` | |
| `IsActive` | `bit` | |
| `CreatedAt` | `datetime2` | |

Roles are the two constants in `Roles`: `"Administrator"` and `"Operator"` (`Identity/ApplicationRole.cs`). User ids are the Identity `string` (GUID) — that is why audit columns such as `Request.CreatedByUserId` are `nvarchar(450)` strings, not ints.

---

## 3. Geography & References (lookup tables)

These are small, mostly cross-tenant reference tables.

### Geography

**`Country`** (`Geography/Country.cs` → `Country`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `smallint` IDENTITY | |
| `Name`, `ShortName` | `nvarchar(150)` | both nullable |
| `Active` | `bit` (`bool?`) | |

**`Community`** (општина / municipality) (`Geography/Community.cs` → `Community`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `int` | |
| `Name` | `nvarchar(150)` required | |
| `CountryId` | `smallint` | FK → `Country` (Restrict) |
| `Code` | `nvarchar(50)` | |
| `PlateNumberPrefix` | `nvarchar(2)` | e.g. "VE" for Велес — used on plate composition |
| `Active` | `bit` (`bool?`) | |

**`City`** (`Geography/City.cs` → `City`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `int` | |
| `Name` | `nvarchar(150)` required | |
| `CommunityId` | `int` | FK → `Community` (Restrict) |
| `PostalCode` | `nvarchar(20)` required | |
| `Active` | `bit` | |

`City → Community → Country` is the geography chain. A `Client` points at a `City`; resolving a client's **community** (`relation → client → city.CommunityId`) feeds the pricing rule lookup (CLAUDE.md gotcha #12).

### References

**`Citizenship`** (`References/Citizenship.cs` → `Citizenship`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `tinyint` IDENTITY | |
| `CountryId` | `smallint` required | FK → `Country` (Restrict) |
| `Name` | `nvarchar(50)` required | |

(No `Active` column on this entity.)

**`PersonalDataType`** (`References/PersonalDataType.cs` → `PersonalDataType`) — a **fixed enumeration**, PK is **not** IDENTITY (`ValueGeneratedNever`, `VteDbContext.cs` 181). Seeded by hand as `1=Driving License, 2=Passport, 3=Personal Id` (per the entity's own XML doc).

| Column | Type | Notes |
|---|---|---|
| `Id` | `tinyint`, not identity | |
| `Name` | `nvarchar(30)` required | |

**`DocumentIssuer`** (the MVR registration office) (`References/DocumentIssuer.cs` → `DocumentIssuer`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `tinyint` IDENTITY | |
| `Name` | `nvarchar(200)` required | |
| `Active` | `bit` | |
| **`CommunityId`** | `int` nullable, **indexed, no FK** | **Restored** column. Legacy `RegistrationIssuers.IdCommunity`, dropped in the first migration and added back via the `AddDocumentIssuerCommunityId` migration (2026-06-25). It lets the **Plav** print show the new owner's destination MVR office community. The link is loose (no FK) because legacy data may reference communities absent from the lookup (`DocumentIssuer.cs` 8-11; `VteDbContext.cs` 170-173). |

`DocumentIssuer` is referenced by `ClientPersonalData.DocumentIssuerId` and `VehicleRegistration.IssuerId`.

---

## 4. Clients module

### `Client` (Комитент / customer)
`VTE.Domain/Clients/Client.cs` → table `Client`. Implements `ITenantOwned`, tenant-filtered.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict) |
| `CityId` | `int` nullable | FK → `City` (Restrict) |
| `CitizenshipId` | `tinyint` nullable | FK → `Citizenship` (Restrict) |
| `Business` | `bit` (`bool?`) | true = legal entity (фирма), false/null = natural person |
| `FirstName`, `MiddleName`, `LastName` | `nvarchar(100)` | all nullable (a business client may only have a name in `FirstName`) |
| `MB` | `nvarchar(13)` | EMBG (personal id) / registration number |
| `Address` | `nvarchar(100)` | |
| `TaxNumber` | `nvarchar(50)` | ЕДБ |
| `PhoneNumber` | `nvarchar(20)` | |
| `Email` | `nvarchar(100)` | |
| `DateOfBirth` | `datetime2` nullable | |
| `Note` | `nvarchar(250)` | |
| `Active` | `bit` (`bool?`) | |
| `CreatedAt` | `datetime2` (`DateTime?`) | |

### `ClientPersonalData`
`Clients/ClientPersonalData.cs` → table `ClientPersonalData`. A client's identity documents (one row per document). **Not** tenant-owned itself — scoping is inherited through the parent `Client` (the FK is `Cascade`).

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `ClientId` | `bigint` | FK → `Client` (**Cascade**) |
| `PersonalDataTypeId` | `tinyint` | FK → `PersonalDataType` (Restrict) |
| `DocumentIssuerId` | `tinyint` | FK → `DocumentIssuer` (Restrict) |
| `Number` | `nvarchar(100)` required | the document number |
| `CreatedAt` | `datetime2` | |
| `Active` | `bit` | |

---

## 5. Vehicles module

This is the largest module: a heavily-normalized `Vehicle` plus ten lookups and the **central anchor** `ClientVehicleRelation`.

### `Vehicle`
`VTE.Domain/Vehicles/Vehicle.cs` → table `Vehicle`. Implements `ITenantOwned`, tenant-filtered. Trimmed from the legacy 113-column schema down to the ~35 columns operators actually fill in (`Vehicle.cs` 5-9). Many columns map to differently-named (often typo'd) legacy columns — those mappings are kept inline as code comments on each property and in `migrate/backfill-pricecatalog-rules.sql` (CLAUDE.md gotcha #4).

Key columns (selected — see the entity for the exhaustive engine/mass/axle set):

| Column | Type | Legacy name | Notes |
|---|---|---|---|
| `Id` | `bigint` IDENTITY | | |
| `CompanyId` | `tinyint` | | FK → `Company` (Restrict) |
| `Vin` | `nvarchar(40)` required, **indexed** | `ShellNumber` | chassis/VIN |
| `EngineNumber` | `nvarchar(40)` | | |
| `Plate` | `nvarchar(20)`, **indexed** | `LastRegistratinNumber` [sic] | current plate |
| `CategoryId` | `smallint?` | | FK → `VehicleCategory` (Restrict) |
| `BodyTypeId` | `int?` | | FK → `VehicleBodyType` |
| `ModelId` | `int?` | | FK → `VehicleModel` |
| `PrimaryColorId` / `SecondaryColorId` | `smallint?` | | both FK → `VehicleColor` |
| `MadeCountryId` | `smallint?` | | FK → `Country` |
| `FuelId` / `SecondFuelId` | `tinyint?` | | both FK → `VehicleFuel` |
| `EngineTypeId` | `int?` | | FK → `VehicleEngineType` |
| `EcoProgramId` | `tinyint?` | | FK → `VehicleEcoProgram` |
| `PaymentCategoryId` | `tinyint?` | | FK → `VehiclePaymentCategory` — drives pricing |
| `EnginePowerKw` | `float?` | `EnginePowerOutPut` | |
| `EngineWorkingCapacityCc` | `float?` | | used as a pricing `VehicleField` |
| `MaxRpm` | `int?` | `BrojNaVrtezi` | |
| `MaxSpeedKmh` | `float?` | | |
| `HasLpg` | `bit?` | `TNG` | LPG flag |
| `ManufactureDate` | `datetime2?` | `MakeDate` | |
| `LengthMm` / `WidthMm` / `HeightMm` | `float?` | | dimensions |
| `EmptyWeightKg` | `float?` | `EmptyWaight` [sic] | |
| `MaxAllowedWeightKg` | `float?` | | |
| `MaxLegalTotalMassKg` / `MaxConstructiveTotalMassKg` / `MaxLegalGroupMassKg` | `float?` | `MaxLegVkMasa` / `MaxKonstVkMasa` / `MaxLegVkMasaGrupa` | |
| `TrailerMassWithBrakesKg` / `TrailerMassWithoutBrakesKg` | **`nvarchar(40)`** | | stored as **strings** (`VteDbContext.cs` 305-306) — legacy free-text values, distinct from the float towing fields below |
| `MaxTrailerBrakedKg` / `MaxTrailerUnbrakedKg` / `MaxHitchLoadKg` | `float?` | `MaxKonstVkMasaKocnaPrikolka` / `MaxKonstVkMasaNeKocnaPrikolka` / `MaxKonstOptovaruvanjeVoPriklucok` | the towing values the Plav form actually prints |
| `AxleCount` | `int?` | `NumberOfAxis` | |
| `WheelCount` | `int?` | | |
| `AxleLoad1Kg` / `AxleLoad2Kg` | `int?` | `OsnoOptovaruvanje1` / … | |
| `Seats` / `StandingSeats` | `smallint?` | | |
| `Co2GKm`, `NoiseStaticDb`, `NoiseMovingDb` | `float?` | | emissions/noise |
| `TypeText` / `ModelVariant` / `ApprovalMark` | `nvarchar(300/400/100)` | `Tip` / `VehicleModelAdding` / `OznakaNaOdobrenie` | |
| `Note` | `nvarchar(1000)` | | |
| `Active` | `bit` | | |
| `CreatedAt` | `datetime2` | | |

Indexes: `Vin`, `Plate`, `CompanyId` (`VteDbContext.cs` 328-330). All lookup FKs are `Restrict` and nullable. (The exhaustive property list, with each legacy column name as an inline comment, is in `Vehicle.cs`.)

### `VehicleRegistration`
`Vehicles/VehicleRegistration.cs` → table `VehicleRegistration`. One row per registration cycle. Scoping inherited from the parent `Vehicle` (Cascade FK). **Not** `ITenantOwned`.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `VehicleId` | `bigint`, **indexed** | FK → `Vehicle` (**Cascade**) |
| `IssuerId` | `tinyint` | FK → `DocumentIssuer` (Restrict) |
| `PlateNumber` | `nvarchar(20)` required, **indexed** | |
| `RegisteredDate` / `ValidUntil` | `datetime2` | |
| `IsFirstRegistration` | `bit` | |
| `Active` | `bit` | |

### `ClientVehicleRelation` — THE CENTRAL ANCHOR
`Vehicles/ClientVehicleRelation.cs` → table `ClientVehicleRelation`. Legacy table `CustomerVehiclesRelations`. **This entity is the backbone of the entire transactional model.** It links a `Client` to a `Vehicle` (or to nothing for a client-only relation), and **almost every transactional record anchors to a relation id, not to a client or vehicle directly**:

- `Request.ClientVehicleRelationId` (and `NewClientVehicleRelationId`)
- `TechnicalExamReport.CustomerVehicleRelationId`
- `PaymentDocument.CustomerVehicleRelationId`
- `CustomerDebt.CustomerVehicleRelationId`

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | the id every transaction points at |
| `ClientId` | `bigint`, **indexed** | FK → `Client` (**Cascade**) |
| `VehicleId` | `bigint?`, **indexed** | FK → `Vehicle` (**Restrict** — so hard-deleting a vehicle won't wipe relations). **Nullable** because `RelationType=3` (client-only) has no vehicle. |
| `RelationTypeId` | `tinyint` | FK → `ClientVehicleRelationType` (Restrict) |
| `StartDate` | `datetime2` | |
| `EndDate` | `datetime2?` | when the relation ended (ownership transferred away, etc.) |
| `StartNote` / `EndNote` | `nvarchar(500)` | |
| `Active` | `bit` (default `true`) | a relation created for a pending ownership transfer is created **`Active=false`** and flipped active by the request End flow (see §6) |

### `ClientVehicleRelationType`
`Vehicles/ClientVehicleRelationType.cs` → table `ClientVehicleRelationType` (`tinyint` PK). Three legacy values (`ClientVehicleRelationType.cs` 3-5):

| Id | Name | Flags |
|---|---|---|
| 1 | Owner (сопственик) | `IsOwner` |
| 2 | Authorized (полномошно лице) | `IsAuthorized` |
| 3 | Client-only (no vehicle bound) | `IsCustomerOnly` (here `VehicleId` is null) |

Columns: `Id`, `Name nvarchar(100)`, `IsOwner`, `IsAuthorized`, `IsCustomerOnly`, `Description nvarchar(500)`, `Active`.

### Vehicle lookups (catalogs)

| Entity / table | PK type | Key columns | Notes |
|---|---|---|---|
| `VehicleCategory` | `smallint` | `Code(20)`, `Name(100)` | A/B/C category etc. |
| `VehicleBodyType` | `int` | `Code(20)`, `Name(500)` | |
| `VehicleMaker` | `int` | `CountryId(short?)`, `Name(500)`, `Trademark(500)` | `CountryId` **indexed, no FK** (loose legacy id) |
| `VehicleModel` | `int` | `MakerId`, `Code(500)`, `Name(500)`, `ProductionStart/End` | FK → `VehicleMaker` (Restrict) |
| `VehicleColor` | `smallint` | `Code(100)`, `Name(500)` | |
| `VehicleFuel` | `tinyint` | `Name(200)` | legacy `VehicleEnginePowerSourceTypes` |
| `VehicleEcoProgram` | `tinyint` | `Name(200)` | Euro 0..6, legacy `VehicleEngineEcoProgram` |
| `VehicleEngineType` | `int` | `Code(200)`, `Name(500)` | 7k+ rows in legacy |
| `VehiclePaymentCategory` | `tinyint` | `Name(200)`, `ZelenMap(tinyint?)` | pricing category; `ZelenMap` 0-11 picks which Plav/Zelen checkbox is ticked (only 1-4 render a box) |

All carry `bool Active`.

---

## 6. Requests module (Барање)

A `Request` is a single operator transaction (work order). Its **status is derived** from timestamps: `EndedAt IS NULL` ⇒ OPEN, otherwise CLOSED (`Request.cs` 9-10). The workflow it runs is defined by its `RequestType`, whose boolean flags drive validation and the side-effects that fire at End time.

### `Request`
`VTE.Domain/Requests/Request.cs` → table `Request`. Implements `ITenantOwned`, tenant-filtered.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict) |
| `RequestTypeId` | `tinyint` | FK → `RequestType` (Restrict) |
| **`ClientVehicleRelationId`** | `bigint`, **indexed** | the anchor (required, BR-REQ-004). FK → `ClientVehicleRelation` (Restrict) |
| **`NewClientVehicleRelationId`** | `bigint?` | **OVERLOADED — see the box below.** Required when `RequestType.TransfersOwnership`. FK → `ClientVehicleRelation` (Restrict) |
| `TechnicalExamReportId` | `bigint?` | optional tie to a tech-exam (no FK configured) |
| `PreviousRegistrationId` | `bigint?`, **indexed** | conceptually FK → `VehicleRegistration` (only an index is configured, `VteDbContext.cs` 448) |
| `CreatedAt` / `ModifiedAt` / `EndedAt` | `datetime2(?)` | lifecycle; `EndedAt` derives status |
| `CreatedByUserId` | `nvarchar(450)` required | Identity user id |
| `ModifiedByUserId` / `EndedByUserId` | `nvarchar(450)` | |
| `VehicleDataChanged` / `ClientDataChanged` | `bit` | trace of mutations actually performed at end-time |
| `Note` | `nvarchar(500)` | |
| `Active` | `bit` | |
| `LegacyReferenceNumber` | `nvarchar(50)` | reference printed on Zelen/Plav/Bel forms; format `{StationCode}{IdTechnicalExamReport}{OperatorId}/{Year}`; backfilled by `migrate/backfill-request-reference-no.sql` (`Request.cs` 44-50) |
| `RowVersion` | `rowversion` | EF concurrency token |

Indexes: `(CompanyId, CreatedAt)`, `ClientVehicleRelationId`, `RequestTypeId`, `PreviousRegistrationId`, and a **partial filtered index `IX_Request_Open`** on `(CompanyId, CreatedAt)` `WHERE [Active] = 1 AND [EndedAt] IS NULL` (the open-requests dashboard query) (`VteDbContext.cs` 448-455).

> ### ⚠️ `Request.NewClientVehicleRelationId` is overloaded (CLAUDE.md gotcha #17)
> The column type is `long?` and the FK targets `ClientVehicleRelation`, but **its meaning differs by data origin**:
>
> - **Migrated rows:** the value is a **CLIENT id**, not a relation id. The legacy column `IdCustomerVehicleRelationNew` was itself overloaded (it held a *customer* id while editing the new-owner panel, a *relation* id only after save), and the migration carried the editing-time customer id through.
> - **v2-native rows:** the value is a real **relation id**. When the FE submits a transfers-ownership request it sends `RequestWriteDto.NewOwnerClientId` (a free client search by EMBG/name); `RequestsController.ResolveOrCreateNewOwnerRelationAsync` (`RequestsController.cs` 416) reuses-or-**creates** a `ClientVehicleRelation` (the new client + the anchor's vehicle, same relation-type as the anchor), created **`Active=false`** and flipped active by the End flow. A relation id supplied directly is still accepted (`RequestsController.cs` 305).
>
> Consumers must disambiguate at read time. The print bundle does exactly this (`RequestsController.cs` 819-851): if the value is a *client* that already has a relation on **this** vehicle → treat it as the migrated new-owner **client** id; otherwise treat it as a **relation** id and follow it to its client. When the type `TransfersOwnership` and the value is present, debts also route to `NewClientVehicleRelationId` instead of the anchor (gotcha #11; `RequestsController.cs` 342-343, 460-461).

### `RequestType` — workflow definition
`Requests/RequestType.cs` → table `RequestType` (`tinyint` PK). Self-referencing (`ParentRequestTypeId`) and tied to a print template.

| Column | Type | Notes |
|---|---|---|
| `Id` | `tinyint` IDENTITY | |
| `ParentRequestTypeId` | `tinyint?` | self-FK (Restrict) |
| `DocumentPrintId` | `tinyint` | FK → `RequestDocumentPrint` (Restrict) |
| `Name` | `nvarchar(250)` required, **indexed** | |
| `Description` | `nvarchar(500)` | |
| `TechnicalExamRequirement` | `tinyint` (enum, `HasConversion<byte>`) | **see overload note below** |
| `PaymentRequired` | `bit` | |
| `IssuesNewRegistration` | `bit` | |
| `DeactivatesRelation` | `bit` | |
| `DeactivatesVehicle` | `bit` | |
| `TransfersOwnership` | `bit` | when true, `NewClientVehicleRelationId` is required and debts route there |
| `MutatesVehicleData` / `MutatesClientData` | `bit` | |
| `IsSufficient` | `bit` | |
| `PreviousRegistrationRequired` | `bit` | |
| `Active` | `bit` | |

The `TechnicalExamRequirement` **enum** (`Requests/TechnicalExamRequirement.cs`) is `NotRequired=0, Required=1, Optional=2`.

> ### ⚠️ `RequestType.TechnicalExamRequirement` is overloaded on prod (CLAUDE.md gotcha #16)
> The property is typed as the tri-state enum above, but the production migration backfilled the **raw legacy `IsTehnicalExamRequired` integer** into this column — and that legacy value is actually the **exam-TYPE id** (0=none, 1=РЕД-12М, 9=…), not the tri-state. So on prod the column holds values like `9` that are not named enum members. Read it as `(int)type.TechnicalExamRequirement`: `> 0` means "create an exam", and the value **is** the `TechnicalExamType.Id` to create. This is how the auto-exam-on-renewal feature picks the exam type. (Station tech-exam org id = 37; РЕД-12М type id = 1, ValidDays = 365.)

### Request catalog tables

| Entity / table | PK | Columns | Replaces legacy |
|---|---|---|---|
| `RequestDocumentPrint` | `tinyint` | `Code(20)` (PLAV/BEL/ZELEN), `Name(200)`, `TemplatePath(400)`, `Active` | the three coloured paper forms |
| `RequestOwnershipProofType` | `tinyint` | `Name(200)`, `Active` | `DocumentVehicleOwnershipProof` |
| `RequestPaymentProofType` | `tinyint` | `Name(200)`, `Active` | `DocumentPaymentProof` |
| `RequestAttachmentType` | `tinyint` | `Name(200)`, `Active` | `AttachmentTypes` |

The three `RequestDocumentPrint` rows are seeded in `VTE.Api/Seed/DataSeeder.cs` (lines 103-105): `PLAV` = "Барање за регистрација (плав образец)", `BEL` = "Барање за пренос на сопственост (бел образец)", `ZELEN` = "Барање за одјава (зелен образец)".

### Request children (all FK → `Request` Cascade, indexed on `RequestId`)

**`RequestOwnershipProof`** (`Requests/RequestOwnershipProof.cs`)

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `RequestId` | `bigint`, indexed | FK → `Request` (Cascade) |
| `OwnershipProofTypeId` | `tinyint` | FK → `RequestOwnershipProofType` (Restrict) |
| `Detail` | `nvarchar(500)` | free text (e.g. "Contract #1234 …") |
| `Active` | `bit` | |

**`RequestPaymentProof`** — same shape, `PaymentProofTypeId` → `RequestPaymentProofType`.

**`RequestAttachment`** (`Requests/RequestAttachment.cs`) — metadata in DB, blob on disk at `StoragePath`.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `RequestId` | `bigint`, indexed | FK → `Request` (Cascade) |
| `AttachmentTypeId` | `tinyint` | FK → `RequestAttachmentType` (Restrict) |
| `FileName` | `nvarchar(260)` required | operator-visible name |
| `ContentType` | `nvarchar(150)` required | MIME |
| `SizeBytes` | `bigint` | |
| `StoragePath` | `nvarchar(500)` required | resolver-relative path (swap to S3/Blob later, no schema change) |
| `UploadedAt` | `datetime2` | |
| `UploadedByUserId` | `nvarchar(450)` required | |
| `Active` | `bit` | |

---

## 7. TechnicalExams module (Технички преглед)

A `TechnicalExamReport` records a vehicle inspection: type, organization, two inspectors, pass/fail, and the brake-force / emission measurements. Defective parts are itemized in child detail lines. Legacy source `DocumentsTehnicalExamsReports` (`TechnicalExamReport.cs` 12-20).

### `TechnicalExamReport`
`TechnicalExams/TechnicalExamReport.cs` → table `TechnicalExamReport`. Implements `ITenantOwned`, tenant-filtered.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict) |
| `CustomerVehicleRelationId` | `bigint?`, **indexed** | the inspected customer+vehicle anchor. **Nullable** — 131 legacy reports point at a relation that no longer exists and migrate as NULL rather than being dropped. FK → `ClientVehicleRelation` (Restrict) |
| `TechnicalExamTypeId` | `int` | FK → `TechnicalExamType` (Restrict). Legacy `IdTypeOfTehnicalExam` |
| `OrganizationId` | `int` | FK → `TechnicalExamOrganization` (Restrict) |
| `RegNumber` | `nvarchar(50)`, **indexed** | format `{StationId}-{seq}/{year}` |
| `MadeDate` / `ValidTillDate` | `date` (`DateOnly`) | `ValidTillDate = MadeDate + TechnicalExamType.ValidDays` |
| `FirstControllerLegacyId` / `SecondControllerLegacyId` | `int?` | inspector ids kept as **legacy** employee ids (their directory lives in a separate security DB); `0 → null` |
| `VehicleIsRight` | `bit` (default true) | pass/fail; **true ⇔ every detail line is status 1 (исправен)** |
| `ExplanationNote` / `DriversWarning` / `Note` | `nvarchar(500)` | |
| `TechnicalChanges` | `nvarchar(max)` | |
| Brake-force block | `double?` ×25 | 5 axles (`Axis1..Axis4`, `AxisParking`) × 5 measures each: `Left`, `Right`, `Gj`, `LeftRightDiff`, `Coefficient` |
| Summary measurements | `double?` | `Weight`, `EffectOfWorkingBrakeEmpty/Full`, `EffectOfSecondaryBrake`, `EffectOfParkingBrake`, `SpeedOfTurns`, `CO`, `EngineRpm`, `COPlusTurns`, `Lambda`, `Pinpoints`, `Noise`, `EngineOilTemp` |
| `Active` | `bit` | |
| `CreatedAt` / `ModifiedAt` | `datetime2(?)` | |
| `RowVersion` | `rowversion` | concurrency token |

> Improvement over legacy: measurements are nullable (real "not measured" instead of a `0` sentinel), dates are `DateOnly`, lookups are real FKs, and the row is tenant-scoped (`TechnicalExamReport.cs` 13-16).

Indexes: `CustomerVehicleRelationId`, `RegNumber`, `(CompanyId, MadeDate)` (`VteDbContext.cs` 564-566).

### `TechnicalExamReportDetail`
`TechnicalExams/TechnicalExamReportDetail.cs` → table `TechnicalExamReportDetail`. One inspected-part line. FK → `TechnicalExamReport` **Cascade**, indexed on parent.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `TechnicalExamReportId` | `bigint`, indexed | FK → `TechnicalExamReport` (Cascade) |
| `VehiclePartId` | `int` | FK → `TechnicalExamVehiclePart` (Restrict) |
| `StatusId` | `int` | FK → `TechnicalExamDetailStatus` (Restrict) |
| `Front` / `Back` / `OnLeft` / `OnRight` | `bit` | where the issue was located |
| `EnteredAt` | `datetime2` | legacy `DateEnter` |
| `Note` | `nvarchar(300)` | |
| `Active` | `bit` | |

### TechnicalExam catalogs

**`TechnicalExamType`** (`int` PK) — РЕД-12М, РЕД-06М, … (legacy `TehnicalExamsTypes`).

| Column | Type | Notes |
|---|---|---|
| `Id` | `int` | |
| `Code` | `nvarchar(20)` | e.g. "РЕД-12М" |
| `Description` | `nvarchar(300)` required | |
| `ValidDays` | `int` | added to `MadeDate` for validity (РЕД-12М = 365) |
| `PercentOfFullExam` | `int` | share of a full exam (pricing) |
| `IsInRegister` | `bit` | reported to national register |
| `Active` | `bit` | |

**`TechnicalExamDetailStatus`** (`int` PK): `Id`, `Name(100)`, `Active`. Three values: `1 = исправен (OK)`, `2 = вратен (returned)`, `3 = неисправен (defective)`. A report passes only when **every** detail is status 1 (`TechnicalExamDetailStatus.cs` 4-8).

**`TechnicalExamVehiclePart`** (`int` PK): `Id`, `CategoryId(int)` (indexed — a grouping bucket, no separate lookup), `Code(100)` required, `Description(500)` required, `Active`.

**`TechnicalExamOrganization`** (`int` PK) — the issuing org with letterhead details for the certificate (legacy `TehnicalExamOrganizations`). `CompanyId tinyint?` (0→null). Holds `Code(40)`, `Name(300)`, loose legacy `CityId/CommunityId` (both no-FK; only `CityId` is **indexed** — `VteDbContext.cs` 543), `Address(200)`, `Phone(200)`, `Fax(200)`, `BankAccount(510)`, `Depositor(200)`, `TaxNumber(100)`, `ResponsibleOfficer(200)`, `Secretary(200)`, `Active`. Kept as its own lookup (not reused from `Station`) so legacy report→org ids migrate 1:1 (`TechnicalExamOrganization.cs` 5-9). The station's org id is **37** (CLAUDE.md gotcha #16).

---

## 8. Payments module

Pricing rules, customer debts, bills, lines, and installment plans. This module flattens the legacy 5-table pricing hierarchy into `PriceCatalog`, and turns the legacy `CustomerFinancialState` into the idempotent `CustomerDebt`. Read [Payments & pricing](05-payments-and-pricing.md) for the evaluator/flow; here we cover the schema.

### `PriceCatalog` — one billable fee
`Payments/PriceCatalog.cs` → table `PriceCatalog` (`int` PK). **Cross-tenant** catalog (fees are statutory in Macedonia) — it does **not** implement `ITenantOwned` and has no tenant query filter. Flattens legacy `PriceCatalog + PaymentItems + PaymentCategories + PaymentItemParametars + VehicleCategoryForPayments`. **`v2.PriceCatalog.Id` maps to `legacy.PaymentItemParametars.Id`** (CLAUDE.md hot note — *not* `PaymentItems.Id`).

| Column | Type | Notes |
|---|---|---|
| `Id` | `int` | |
| `Code` | `nvarchar(40)` | e.g. "TEH-IPR" |
| `Name` | `nvarchar(250)` required | |
| `BasePrice` | `decimal(18,4)` | before discount + VAT |
| `VatRateId` | `int` | FK → `VatRate` (Restrict) |
| `Trigger` | `tinyint` (enum, `HasConversion<byte>`), **indexed** | which workflow auto-creates a debt — see `PriceTrigger` below |
| `VehiclePaymentCategoryId` | `int?` | loose ref, no FK. NULL = any. Part of `IX_PriceCatalog_Eval` |
| `CommunityId` | `int?` | optional municipality scope. NULL = any |
| `PriceCompanyId` | `tinyint?` | optional **tenant** scope. NULL = applies to every company. Without it, multiple companies' "Operating fee" rules all fire for the same vehicle → duplicate debts |
| `PaymentCategoryGroupId` | `int?` | forensic: legacy `PaymentCategories.Id` |
| `VehicleField` | `nvarchar(60)` | for ranged rules, the `Vehicle` property to read at eval-time (e.g. `EngineWorkingCapacityCc`, `AxleCount`). NULL = fixed-fee |
| `ParametarFrom` / `ParametarTo` | `float?` (`double?`) | inclusive bounds on that property |
| `VehicleCategoryFilter` | `nvarchar(200)` | optional CSV of `VehicleCategory` codes (empty = all) |
| `BankAccount` | `nvarchar(50)` | where fees route |
| `PaymentForm` | `nvarchar(20)` | bank form code (e.g. "PP30") |
| `Active` | `bit` | |

Indexes: `Trigger`, and the composite `IX_PriceCatalog_Eval` on `(Trigger, VehiclePaymentCategoryId)` for the rule evaluator (`VteDbContext.cs` 617-620).

The **`PriceTrigger`** enum (`Payments/PriceTrigger.cs`): `None=0, TechnicalExam=1, Request=2, TrafficLicence=3, Permission=4, InternationalDrivingLicence=5, TechnicalExamIrregular=6`.

### `CustomerDebt` — an open debt against a relation
`Payments/CustomerDebt.cs` → table `CustomerDebt` (`bigint` PK). Implements `ITenantOwned`, tenant-filtered. Legacy source `CustomerFinancialState`. Created automatically when a source document completes and the pricing evaluator finds applicable rules; collected later into a `PaymentDocument`.

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict) |
| `CustomerVehicleRelationId` | `bigint`, **indexed** | the anchor. FK → `ClientVehicleRelation` (Restrict) |
| `PriceCatalogId` | `int` | FK → `PriceCatalog` (Restrict) — the fee rule that triggered |
| `Price` | `decimal(18,4)` | **snapshot** of the fee at creation time (catalog can change later) |
| `VatPercent` | `float` (`double`) | snapshot of VAT % at creation time |
| `Note` | `nvarchar(300)` | |
| `Origin` | `tinyint` (enum, `HasConversion<byte>`) | which workflow created it — `DebtOrigin` |
| `OriginRequestId` | `bigint?`, filtered-indexed | set if `Origin = Request` |
| `OriginTechnicalExamId` | `bigint?`, filtered-indexed | set if `Origin = TechnicalExam(Irregular)` |
| `OrganizationId` | `int` | station/org that owns the debt |
| `Paid` | `bit` | true once settled |
| `SettledByLineId` | `bigint?` | FK → `PaymentDocumentLine` (Restrict) — the line that settled it |
| `CreatedAt` | `datetime2` | |
| `CreatedByUserId` | `nvarchar(450)` | |
| `Active` | `bit` | |
| `LegacyId` | `bigint?`, **unique filtered index** | legacy `CustomerFinancialState.Id`; makes legacy-sync idempotent (added by the `AddCustomerDebtLegacyId` migration) |

Indexes: `CustomerVehicleRelationId`; partial **`IX_CustomerDebt_Open`** on `(CustomerVehicleRelationId, Paid)` `WHERE [Active] = 1 AND [Paid] = 0`; filtered indexes on `OriginRequestId`, `OriginTechnicalExamId`; **unique** filtered `IX_CustomerDebt_LegacyId` (`VteDbContext.cs` 715-723).

> **Idempotency key:** a debt is uniquely identified by its source — `(Origin, OriginRequestId)` or `(Origin, OriginTechnicalExamId)`. Re-creating a debt for the same source is a no-op (CLAUDE.md "Idempotent ops"). The `DebtOrigin` enum (`Payments/DebtOrigin.cs`): `Manual=0, Request=1, TechnicalExam=2, TechnicalExamIrregular=3, TrafficLicence=4, Permission=5, InternationalDrivingLicence=6`.

### `PaymentDocument` — a bill / receipt / invoice
`Payments/PaymentDocument.cs` → table `PaymentDocument` (`bigint` PK). Implements `ITenantOwned`, tenant-filtered. Legacy source `PaymentDocuments` (≈244k rows).

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | FK → `Company` (Restrict) |
| `PaymentTypeId` | `int` | FK → `PaymentType` (Restrict) |
| `CustomerVehicleRelationId` | `bigint`, **indexed** | the anchor (required). FK → `ClientVehicleRelation` (Restrict) |
| `OperatorLegacyId` | `int?` | legacy operator id (fallback for migrated rows); v2 prefers `CreatedByUserId` |
| `OrganizationId` | `int` | issuing station/org |
| `DocumentNumber` | `nvarchar(50)` required, **indexed** | `{prefix}{seq}/{year}` |
| `IssueDate` / `DueDate` | `datetime2` | |
| `Discount` | `float?` (`double?`) | document-level % (0..100) |
| `Paid` | `bit` | |
| `Stornoed` | `bit` | reversed/cancelled |
| `StornoReason` | `nvarchar(500)` | new in v2 (legacy left it implicit) |
| `Note` | `nvarchar(500)` | |
| `AgreementId` | `bigint?` | FK → `InstallmentAgreement` (Restrict), only for installment bills |
| `InvoicedToCompanyId` | `int?` | B2B billing target (legacy `IdFakturiraNa`) |
| `FiscalPrintedAt` | `datetime2?` | outbox marker — set when the fiscal device confirms the print |
| `LegacyId` | `bigint?`, filtered-indexed | migration traceability |
| `Active`, `CreatedAt`, `ModifiedAt`, `CreatedByUserId`, `ModifiedByUserId` | | audit |
| `RowVersion` | `rowversion` | concurrency token |

Indexes: `CustomerVehicleRelationId`, `(CompanyId, IssueDate)`, `DocumentNumber`, filtered `LegacyId` (`VteDbContext.cs` 659-662).

### `PaymentDocumentLine`
`Payments/PaymentDocumentLine.cs` → table `PaymentDocumentLine` (`bigint` PK). Implements `ITenantOwned` (carries `CompanyId`) but has **no** explicit query filter in the config — it is scoped through its parent document. Legacy `PaymentDocumentsDetails` (≈1.16M rows).

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | |
| `PaymentDocumentId` | `bigint`, **indexed** | FK → `PaymentDocument` (Cascade) |
| `PriceCatalogId` | `int` | FK → `PriceCatalog` (Restrict) |
| `UnitPrice` | `decimal(18,4)` | snapshot at sale time |
| `VatPercent` | `float` (`double`) | snapshot |
| `Discount` | `float` (`double`) | per-line % |
| `Quantity` | `int` (default 1) | v2 supports multi-quantity (legacy assumed 1) |
| `Note` | `nvarchar(300)` | |
| `PrePaid` | `bit` | |
| `PrePaidNote` | `nvarchar(300)` | |
| `CustomerDebtId` | `bigint?`, filtered-indexed | the `CustomerDebt` this line settles (Phase 3). Nullable for ad-hoc sales |
| `Active` | `bit` | |

Indexes: `PaymentDocumentId`, filtered `CustomerDebtId` (`VteDbContext.cs` 679-680).

### `InstallmentAgreement` (Договор за рати)
`Payments/InstallmentAgreement.cs` → table `InstallmentAgreement` (`bigint` PK). Implements `ITenantOwned`, tenant-filtered. Legacy `DogovorZaRati` (≈6,796 rows).

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint`, indexed | FK → `Company` (Restrict) |
| `Number` | `nvarchar(40)` required, indexed | per-company sequence |
| `Date` | `date` (`DateOnly`) | |
| `TotalInstallments` | `int` | |
| `GuarantorName` | `nvarchar(200)` | |
| `GuarantorAddress` | `nvarchar(300)` | |
| `GuarantorEmbg` | `nvarchar(13)` | legacy `GartEMB` — note EMBG-corruption caveat (CLAUDE.md gotcha #5: some rows store school names; import uses `LEFT(GartEMB,13)`) |
| `Active`, `CreatedAt`, `ModifiedAt` | | |

### `InstallmentSchedule`
`Payments/InstallmentSchedule.cs` → table `InstallmentSchedule` (`bigint` PK). Implements `ITenantOwned` (carries `CompanyId`, no explicit filter). One scheduled installment row. Legacy `PaymentDocumentsRata` (≈254,348 rows).

| Column | Type | Notes |
|---|---|---|
| `Id` | `bigint` IDENTITY | |
| `CompanyId` | `tinyint` | |
| `PaymentDocumentId` | `bigint`, **indexed** | FK → `PaymentDocument` (Cascade) |
| `SequenceNo` | `int` | 1-based ordinal; **new in v2**. `(PaymentDocumentId, SequenceNo)` is **unique** |
| `Amount` | `decimal(18,4)` | scheduled amount |
| `DueDate` | `date?` (`DateOnly?`) | new in v2 |
| `Paid` | `bit` | |
| `PaidAt` | `datetime2?` | |
| `PaidAmount` | `decimal(18,4)?` | actual paid (≤ Amount); legacy didn't store separately |
| `OrganizationId` | `int?` | station that collected |
| `OperatorLegacyId` | `int?` | |
| `Note` | `nvarchar(300)` | |
| `Active` | `bit` | |

### Payment catalogs

**`VatRate`** (`int` PK) — `Code(20)`, `Name(100)` required, `Percent(double)`, `Active`. Legacy `DDVCatalog` (3 rows: 0%, 5%, 18%). Cross-tenant.

**`PaymentType`** (`int` PK) — how a bill is collected & what prints. Legacy `PaymentTypes` (≈30 rows). Cross-tenant.

| Column | Type | Notes |
|---|---|---|
| `Id` | `int` | |
| `Code` | `nvarchar(40)` | "CASH"/"CARD"/"INV"/"RATA" (v2-introduced) |
| `Name` | `nvarchar(150)` required | |
| `IsCash` / `IsCard` / `IsInstallment` | `bit` | |
| `PrintsReceipt` / `PrintsInvoice` | `bit` | Smetka / Faktura |
| `Prefix` | `nvarchar(20)` | `DocumentNumber` prefix |
| `Active` | `bit` | |

> **Duplicate-PaymentType caveat (CLAUDE.md gotcha #14):** the legacy `PaymentTypes` table repeats each type once per company (no company column) and the same fee can carry a different `Prefix` per company. The `usedOnly` dedup groups by **trimmed Name** and keeps the id whose most-recent bill is newest; picking a stale sibling forks the doc-number sequence.

---

## 9. Relationship map (how the anchor ties it together)

```
Company (tenant, tinyint PK)
  └─< Station, Client, Vehicle, Request, TechnicalExamReport,
      PaymentDocument, CustomerDebt, InstallmentAgreement   (all ITenantOwned)

Client ──< ClientPersonalData                 (Cascade)
Client ─┐
        ├──> ClientVehicleRelation <──┐       ★ THE ANCHOR (bigint Id)
Vehicle ┘   (ClientId Cascade,         │
             VehicleId? Restrict)      │
                                       │ every transaction points HERE:
Vehicle ──< VehicleRegistration        │
                                       ├── Request.ClientVehicleRelationId (req)
                                       ├── Request.NewClientVehicleRelationId (overloaded!)
                                       ├── TechnicalExamReport.CustomerVehicleRelationId (nullable)
                                       ├── PaymentDocument.CustomerVehicleRelationId (req)
                                       └── CustomerDebt.CustomerVehicleRelationId (req)

Request ──< RequestOwnershipProof / RequestPaymentProof / RequestAttachment   (Cascade)
TechnicalExamReport ──< TechnicalExamReportDetail                              (Cascade)
PaymentDocument ──< PaymentDocumentLine / InstallmentSchedule                  (Cascade)
PaymentDocumentLine.CustomerDebtId ──> CustomerDebt  (settlement link)
CustomerDebt.SettledByLineId ──> PaymentDocumentLine (back link, Restrict)
PriceCatalog ──< (PaymentDocumentLine, CustomerDebt)   ← cross-tenant catalog
```

**Why the anchor matters:** because debts, bills, exams, and requests all anchor on `ClientVehicleRelation.Id` rather than on `ClientId`/`VehicleId`, an ownership transfer is modelled by **ending one relation and starting a new one** (new `ClientVehicleRelation` for the new owner + same vehicle), and routing the transfer's debts to the new relation. That is exactly what the `Request` ownership-transfer path does with `NewClientVehicleRelationId` (§6).

---

## 10. EF id-type reference (note the `tinyint` PKs)

The PK type was chosen by expected row count. `tinyint` (≤255) is used for the tenant key and small catalogs; transactional tables use `bigint`.

| `tinyint` (byte) | `smallint` (short) | `int` | `bigint` (long) |
|---|---|---|---|
| `Company`, `Citizenship`, `DocumentIssuer`, `PersonalDataType`, `VehicleFuel`, `VehicleEcoProgram`, `VehiclePaymentCategory`, `ClientVehicleRelationType`, `RequestType`, `RequestDocumentPrint`, `RequestOwnershipProofType`, `RequestPaymentProofType`, `RequestAttachmentType` | `Station`, `Country`, `VehicleCategory`, `VehicleColor` | `Community`, `City`, `VehicleBodyType`, `VehicleMaker`, `VehicleModel`, `VehicleEngineType`, `TechnicalExamType`, `TechnicalExamOrganization`, `TechnicalExamVehiclePart`, `TechnicalExamDetailStatus`, `VatRate`, `PaymentType`, `PriceCatalog` | `Client`, `ClientPersonalData`, `Vehicle`, `VehicleRegistration`, `ClientVehicleRelation`, `Request`, `RequestOwnershipProof`, `RequestPaymentProof`, `RequestAttachment`, `TechnicalExamReport`, `TechnicalExamReportDetail`, `PaymentDocument`, `PaymentDocumentLine`, `InstallmentAgreement`, `InstallmentSchedule`, `CustomerDebt` |

`CompanyId` foreign keys are always `tinyint`. `byte`-typed FK columns to small lookups (e.g. `FuelId`, `RelationTypeId`, `DocumentIssuerId`) are likewise `tinyint`. Enums (`RequestType.TechnicalExamRequirement`, `PriceCatalog.Trigger`, `CustomerDebt.Origin`) are persisted as `tinyint` via `HasConversion<byte>`.

---

## 11. Migrations

The schema is owned entirely by EF Core migrations in `backend-v2/src/VTE.Infrastructure/Migrations/` (EF's default folder). There are three, applied in order:

| Migration | Date | What it does |
|---|---|---|
| **`20260612231002_InitialSchema`** | 2026-06-12 | The **squashed** single migration that creates the **full** schema: all `AspNet*` Identity tables + every business table (Company, Station, geography/references, Clients, Vehicles + lookups, Requests + children/catalogs, TechnicalExams, Payments). `InitialSchema.cs` is ~109 KB. |
| **`20260612234823_AddCustomerDebtLegacyId`** | 2026-06-12 | Adds `CustomerDebt.LegacyId bigint NULL` + a **unique filtered** index `IX_CustomerDebt_LegacyId WHERE [LegacyId] IS NOT NULL` (idempotent legacy-sync). |
| **`20260625115717_AddDocumentIssuerCommunityId`** | 2026-06-25 | Adds `DocumentIssuer.CommunityId int NULL` + index `IX_DocumentIssuer_CommunityId` (restores the dropped legacy `IdCommunity` for Plav prints). |

Current model state lives in `VteDbContextModelSnapshot.cs`.

> ### The squash (CLAUDE.md gotcha #6)
> Migrations were **squashed on 2026-06-12** into the single `InitialSchema`. The **old `Persistence/Migrations/` chain is gone** — its migration files were deleted (only an empty `Persistence/Migrations/` directory may linger) and must never be restored from git history. Reasons it was replaced:
> - the old chain could never build a fresh DB (it duplicated Identity tables, and ~10 entities such as `Company` were marked `ExcludeFromMigrations()` so their `CreateTable` existed nowhere);
> - `ExcludeFromMigrations` was removed everywhere — **EF now owns the full schema**, so a fresh database can be built from migrations alone (see the `OnModelCreating` comments at `VteDbContext.cs` 98-103, 221-223);
> - Filip's laptop DB had its `__EFMigrationsHistory` realigned to the single squash row.
>
> The two later migrations are normal additive deltas on top of the squash.

Bulk **data** (the migrated production set) is loaded by SQL scripts under `migrate/` (e.g. `migrate-technical-exams.sql`, `migrate-payments.sql`, `migrate-requests.sql`), **not** by EF — the migrations create empty tables; the scripts fill them. Reference/catalog seed rows (the three `RequestDocumentPrint` forms, default Company/Station/admin user, request types) are seeded at API startup by `VTE.Api/Seed/DataSeeder.cs`.

---

## 12. Data scale (sizing context)

From the migrated production set (CLAUDE.md "Data scale"):

| Table (legacy source) | Approx rows |
|---|---|
| Bills — `PaymentDocument` (`PaymentDocuments`) | ~228k–244k |
| Bill lines — `PaymentDocumentLine` (`PaymentDocumentsDetails`) | ~1.15M–1.16M |
| Installments — `InstallmentSchedule` (`PaymentDocumentsRata`) | ~254k |
| Installment agreements — `InstallmentAgreement` (`DogovorZaRati`) | ~6,796 |
| Clients | ~7k (local snapshot); **~32k on prod** |
| Vehicles | ~6.9k (local snapshot); **~66k on prod** |
| Requests | ~18k (local snapshot); **~128k on prod** |
| Technical-exam reports | **~100k on prod** |
| `VehicleEngineType` (lookup) | 7k+ |

The VTE database is ~785 MB total (~648 MB data + ~136 MB log after shrink). A full legacy→v2 migration run takes ~150 s. The local dev snapshot is smaller than prod; prod carries the real data lifted via `.bak` restore since 2026-06-13.

---

### Cross-references
- [Overview & architecture](01-overview-and-architecture.md) §4 — how `ITenantOwned` + `_tenant` produce the per-company scoping seen on every `HasQueryFilter` above (there is no standalone multi-tenancy doc; it lives here).
- [Requests (Барања)](03-requests.md) — the End side-effects driven by `RequestType` flags and the ownership-transfer relation creation.
- [Technical exams (Технички преглед)](04-technical-exams.md) — the exam-report workflow that anchors on `ClientVehicleRelation`.
- [Payments, pricing & debts (Наплата)](05-payments-and-pricing.md) — how `PriceCatalog` rules are evaluated into `CustomerDebt` rows and collected into `PaymentDocument` bills.
- [Legacy data migration](10-data-migration.md) — the SQL scripts under `migrate/` that fill these tables.
