# VTE Domain & Schema Audit

**Date started:** 2026-04-30
**Spec:** [2026-04-30-vte-domain-audit-design.md](2026-04-30-vte-domain-audit-design.md)
**Status:** Pass 1 complete; awaiting stakeholder checkpoint review.

---

## 1. Executive summary

(Filled at end of Pass 3.)

---

## 2. Glossary (Macedonian → English)

### 2.1 Domain terms

| Macedonian | English | Confirmed? | Notes |
|---|---|---|---|
| `AKTIVEN` | active | needs confirmation | Constraint suffix in DF_BAZI_AKTIVEN, DF_KORISNICI_AKTIVEN, DF_ULOGI_AKTIVEN |
| `BAZI` | databases | needs confirmation | Constraint name PK_BAZI / DF_BAZI_AKTIVEN on DataBases table |
| `BLK` | BLK | needs confirmation | Column [BLK] on Employes, Users — short alphanumeric, possibly Macedonian-state ID prefix |
| `Beleshka / Belezhka` | note | needs confirmation |  |
| `BrNaRati` | BrNaRati | needs confirmation | Column on DogovorZaRati |
| `Broj` | number | needs confirmation | Column [Broj] on DogovorZaRati |
| `Datum` | date | needs confirmation | Column [Datum] on DogovorZaRati |
| `Deskription` | description | needs confirmation | Column [Deskription] on Cities — likely Macedonian-influenced typo |
| `Dimenzions` | dimensions | needs confirmation | Column [Dimenzions] on VehicleTireTypes |
| `Dogovor` | agreement / contract | needs confirmation | Table DogovorZaRati = "agreement for installments"; column [IdDogovor] |
| `DogovorZaRati` | installment agreement | needs confirmation | Table name |
| `DDV` | VAT | needs confirmation | Table DDVCatalog and columns [IdDDV], [DDV], [DDVName], [DDVValue] |
| `EMBG` | EMBG | needs confirmation | char(13) column on Employes, Users |
| `Faktura` | invoice | needs confirmation | Boolean column [Faktura] on PaymentTypes |
| `Fiskalna_karticka` | fiskalna karticka | needs confirmation | Column on PaymentTypes — bit flag |
| `Fiskalna_kes` | fiskalna kes | needs confirmation | Column on PaymentTypes — bit flag; "kes" = "cash" |
| `Garant` | guarantor | needs confirmation | Columns [GarantNaziv], [GarantAdresa], [GartEMB] on DogovorZaRati |
| `GarantAdresa` | guarantor address | needs confirmation | Column on DogovorZaRati |
| `GarantNaziv` | guarantor name | needs confirmation | Column on DogovorZaRati |
| `GartEMB` | guarantor EMBG | needs confirmation | Column on DogovorZaRati — short for Garant EMBG |
| `JUS` | JUS | needs confirmation | Tables/columns VehicleJUSCategories, [CategoryJus*], MKSJUS |
| `Kasa / Kasi` | cashbox / cash desk(s) | needs confirmation | Translated as SecurityHouses table — needs confirmation |
| `KORISNICI` | users | needs confirmation | Constraint names PK_KORISNICI, DF_KORISNICI_AKTIVEN on Users table |
| `MB` | MB | needs confirmation | Column on Customers — needs confirmation |
| `Naziv` | name | needs confirmation | Used in GarantNaziv on DogovorZaRati |
| `Opis` | description | needs confirmation | Column [Opis] on DocumentTypePrint |
| `Parametars` | parameters | needs confirmation | Table name PaymentItemParametars — non-standard English |
| `Plav` | blue | needs confirmation | Suspected colour reference in print-form vocabulary (per task brief) |
| `Bel` | white | needs confirmation | Suspected colour reference in print-form vocabulary (per task brief) |
| `Prezime` | surname | needs confirmation | Column [Prezime] on Employes — coexists with English [FirstName] / [SureName] |
| `Provizija` | commission | needs confirmation | Per task brief — not directly observed in column names |
| `Rata` | installment | needs confirmation | See [Rati], [BrNaRati], PaymentDocumentsRata |
| `Rati` | installments | needs confirmation | Bit column on PaymentTypes; suffix on PaymentDocumentsRata, DogovorZaRati |
| `SecurityHouses` | SecurityHouses | needs confirmation | Table name; PK_SecurityHouses; column [SecurityHouseName] on PaymentDocumentsRata |
| `Smetka` | account / bill | needs confirmation | Boolean column [Smetka] on PaymentTypes |
| `Storno` | storno | needs confirmation | Bit column on PaymentDocuments |
| `Tehnical` | technical | needs confirmation | Prefix on TehnicalExam* tables — should be Technical |
| `Driveing` | driving | needs confirmation | Prefix on DriveingLicenceCtegories, DocumentsInternationalDriveingLicences |
| `Hireing` | hiring | needs confirmation | Column [DateOfHireing] on Employes, Users |
| `Ctegories` | categories | needs confirmation | Suffix on DriveingLicenceCtegories |
| `ULOGI` | roles | needs confirmation | Constraint names PK_ULOGI, DF_ULOGI_AKTIVEN on Roles table |
| `VidoviStampa` | VidoviStampa | needs confirmation | Constraint names PK_VidoviStampa, DF_VidoviStampa_Active on DocumentTypePrint table |
| `Vitlo` | winch | needs confirmation | Bit column on Vehicles — vehicle accessory |
| `ZelenMap` | zelen map | needs confirmation | Column on VehicleCategoryForPayments — needs confirmation; likely refers to Green Card international insurance |

### 2.2 File / folder naming conventions

| Prefix | Meaning | Example |
|---|---|---|
| `ux*` | User-control / form (main editing UI) | `uxCustomers.vb` |
| `dij*` | Dialog (modal sub-form) | `dijOperators.vb` |
| `rpt*` | Report (printed document) | `rptFaktura.vb` |
| `print*` | Print template (request forms) | `PrintPlav.vb` |

---

## 3. Tenancy & auth model — TARGET

### 3.1 Tenancy

Multi-tenant SaaS. Every domain table carries a `StationId` discriminator (the "tenant" column). Each existing legacy per-station database becomes one row in a new `Stations` table; all that station's data carries that `StationId`. Tenancy is enforced at the API layer (every authenticated request resolves to a `StationId`) and at the EF Core query level (a global query filter by `StationId` on every domain entity).

### 3.2 Auth

**Stack:** ASP.NET Core Identity (the standard .NET 9 identity stack), backed by EF Core. JWT bearer tokens for the Vue SPA. **No CSLA, no field-level privileges, no object-level privileges, no per-class privilege engine.** The legacy CSLA privilege model (`FieldsPrivileges`, `ObjectPrivileges`, `CSLAObjects`, `SecurityPolicies`, the `View_1` login join, and the 40 SEC stored procedures) is **dropped wholesale**. The new system relies on standard role-based authorization in ASP.NET Core (`[Authorize(Roles = "Administrator")]`, `[Authorize(Roles = "Operator")]`).

### 3.3 Roles

Exactly **two** roles, hard-coded in the system:

| Role | Scope | Capabilities |
|---|---|---|
| `Administrator` | system-wide, cross-tenant | full read/write access to every station's data; can create stations; can create operators for any station; can run reports across stations; back-office / Bransys-internal role |
| `Operator` | bound to exactly one `StationId` | full read/write access **only** to their own station's data; cannot see, list, or modify other stations' data; no cross-tenant queries |

Operators are bound to a station via a single FK column (`Operators.StationId NOT NULL`). The JWT carries both the role and the StationId claim; the API validates both on every request. Administrators have no `StationId` claim (or a special "all stations" sentinel); their requests are not station-scoped.

### 3.4 What this means for migration

- The legacy `Users` table maps to ASP.NET Identity's `AspNetUsers` (the standard table). The legacy password hashes (whatever the `Crypt` class produced) **cannot be carried over directly** — ASP.NET Identity uses its own password hashing scheme. Operators will need to set a new password on first login (forced password reset) OR receive new credentials from an Administrator at cutover. Decision deferred to the migration sub-project; flagged Q-004.
- The legacy `Roles` table is **dropped** — the new system has only the two hard-coded roles above.
- The legacy `Employes` table maps to a new `Operators` table (since "Employee" in the legacy sense conflated personnel records with login users — in the new system they're the same thing).
- The legacy `FieldsPrivileges`, `ObjectPrivileges`, `CSLAObjects`, `SecurityPolicies`, and `DataBases` tables are **dropped**. They have no equivalent in the new system.
- The legacy `View_1` login query (Users JOIN Roles JOIN DataBases) is **dropped**. ASP.NET Identity replaces it.

### 3.5 What this means for the audit's SEC-related findings

- All 40 SEC stored procedures inventoried in §4.8.2 are **dropped**. The audit retains them as historical record only. They are NOT to be ported.
- All 3 SEC views in §4.8.2 (`View_1`, `FieldPrivilegesList`, `ObjectPrivilegesList`) are **dropped**.
- R-2 (business rules hidden in CSLA) is **partially mitigated for SEC**: even if such rules exist in the privilege engine, they don't matter — we're replacing the engine. R-2 still applies fully to the other modules.

### 3.6 What this means for sub-project (5) in §9.3

Sub-project (5) "Operators / roles / privileges" is **massively simplified**. Its scope is now:
- Implement `Operators` entity backed by ASP.NET Identity's `AspNetUsers`.
- Implement the StationId tenant filter middleware (resolves `StationId` from JWT, applies global query filter).
- Implement two role policies (`Administrator`, `Operator`).
- Implement an Operator-management UI (admin-only): create, deactivate, password reset, assign to station.
- Implement an Administrator-management UI (admin-only): create another administrator.

That's it. No CSLA, no field privileges, no object privileges, no per-property authorization rules.

---

## 4. Per-module sections

### 4.1 Reference data (REF)

#### 4.1.1 Purpose
#### 4.1.2 Tables

##### `VehicleDisabledFields`

Source: `WinApp/sqlData.sql:4783-4794`

```sql
CREATE TABLE [dbo].[VehicleDisabledFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[FieldName] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleDisabledFields_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleDisabledFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategoryForPayments`

Source: `WinApp/sqlData.sql:4848-4860`

```sql
CREATE TABLE [dbo].[VehicleCategoryForPayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ZelenMap] [int] NOT NULL CONSTRAINT [DF_VehicleCategoryForPayments_ZelenMap]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategoryForPayments_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleCategoryForPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentTypes`

Source: `WinApp/sqlData.sql:4884-4901`

```sql
CREATE TABLE [dbo].[PaymentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Fiskalna_kes] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Fiskalna_kes]  DEFAULT ((0)),
	[Fiskalna_karticka] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Fiskalna_karticka]  DEFAULT ((0)),
	[Rati] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Rati]  DEFAULT ((0)),
	[Smetka] [bit] NOT NULL,
	[Faktura] [bit] NOT NULL,
	[PrintText] [nvarchar](50) NOT NULL CONSTRAINT [DF_PaymentTypes_PrintText]  DEFAULT (N'print'),
	[Prefix] [nvarchar](15) NULL CONSTRAINT [DF_PaymentTypes_Prefix]  DEFAULT ((5)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleMakers`

Source: `WinApp/sqlData.sql:4915-4927`

```sql
CREATE TABLE [dbo].[VehicleMakers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCountry] [int] NOT NULL CONSTRAINT [DF_VehicleMakers_IdCountry]  DEFAULT ((1)),
	[CompanyName] [nvarchar](250) NOT NULL,
	[CompanyTrademark] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleMakers_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleMakers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleJUSCategories`

Source: `WinApp/sqlData.sql:5054-5066`

```sql
CREATE TABLE [dbo].[VehicleJUSCategories](
	[Id] [int] NOT NULL,
	[PictureJusPath] [nvarchar](250) NOT NULL,
	[CategoryJusCode] [nvarchar](20) NOT NULL,
	[CategoryJusName] [nvarchar](50) NOT NULL,
	[CategoryJusDescription] [ntext] NULL,
	[CategoryJusPath] [nvarchar](250) NULL,
 CONSTRAINT [PK_VehicleJUSCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

##### `PaymentCategories`

Source: `WinApp/sqlData.sql:5072-5092`

```sql
CREATE TABLE [dbo].[PaymentCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDDV] [int] NOT NULL,
	[IdCalculationItem] [int] NOT NULL CONSTRAINT [DF_PaymentCategories_IdCalculationItem]  DEFAULT ((0)),
	[CategoryName] [nvarchar](250) NOT NULL,
	[AllowDiscount] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_AllowDiscount]  DEFAULT ((0)),
	[TrigerdByRequest] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByRequest]  DEFAULT ((0)),
	[TrigerdByTechnicalExam] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByTechnicalExam]  DEFAULT ((0)),
	[TrigerdByTrafficLicence] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByTrafficLicence]  DEFAULT ((0)),
	[TrigerdByPremisionForVehicle] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByPremisionForVehicle]  DEFAULT ((0)),
	[TrigerdByInternationalDrivierLicence] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByInternationalDrivierLicence]  DEFAULT ((0)),
	[TrigerdByIrregularTechnicalExam] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByIrregularTechnicalExam]  DEFAULT ((0)),
	[VisibleOrder] [int] NOT NULL CONSTRAINT [DF_PaymentCategories_VisibleOrder]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleGearBox`

Source: `WinApp/sqlData.sql:5120-5131`

```sql
CREATE TABLE [dbo].[VehicleGearBox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GearBoxCode] [int] NULL,
	[GearBoxDescription] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesGearBox_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehiclesGearBox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleUse`

Source: `WinApp/sqlData.sql:5149-5160`

```sql
CREATE TABLE [dbo].[VehicleUse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UseDescription] [nvarchar](250) NOT NULL,
	[RegistrationMask] [nvarchar](150) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleUse_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleUse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleTireTypes`

Source: `WinApp/sqlData.sql:5225-5239`

```sql
CREATE TABLE [dbo].[VehicleTireTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleModel] [int] NULL CONSTRAINT [DF_VehicleTireTypes_IdVehicleModel]  DEFAULT ((1)),
	[Seria] [nvarchar](50) NOT NULL,
	[TireType] [nvarchar](150) NOT NULL,
	[Dimenzions] [decimal](18, 0) NOT NULL,
	[Note] [nvarchar](50) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleTireTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleTireTypes_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleSupporting`

Source: `WinApp/sqlData.sql:5327-5338`

```sql
CREATE TABLE [dbo].[VehicleSupporting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupportingCode] [int] NULL,
	[SupportingDescription] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleSupporting_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleSupporting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamOrganizations`

Source: `WinApp/sqlData.sql:5351-5363`

```sql
CREATE TABLE [dbo].[TehnicalExamOrganizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationName] [nvarchar](150) NOT NULL,
	[Station] [nvarchar](150) NULL,
	[IdCity] [int] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamOrganizations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamOrganizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamVehiclePartsCategories`

Source: `WinApp/sqlData.sql:5477-5488`

```sql
CREATE TABLE [dbo].[TehnicalExamVehiclePartsCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](150) NOT NULL,
	[CategoryCode] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamCategoryOfVehicleParts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamCategoryOfVehicleParts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamVehicleParts`

Source: `WinApp/sqlData.sql:5494-5507`

```sql
CREATE TABLE [dbo].[TehnicalExamVehicleParts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategoryVehicleParts] [int] NOT NULL CONSTRAINT [DF_TehnicalExamVehicleParts_IdCategoryVehicleParts]  DEFAULT ((0)),
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[PicturePath] [nvarchar](150) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamVehicleParts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamVehicleParts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamsTypes`

Source: `WinApp/sqlData.sql:5558-5571`

```sql
CREATE TABLE [dbo].[TehnicalExamsTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
	[Code] [nvarchar](10) NULL,
	[ValidNumOfDays] [int] NOT NULL,
	[PercentOfFullExam] [int] NOT NULL CONSTRAINT [DF_TehnicalExamsTypes_PercentOfFullExam]  DEFAULT ((100)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TypesOfTehnicalExams_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_TypesOfTehnicalExams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleRequiredFields`

Source: `WinApp/sqlData.sql:5594-5605`

```sql
CREATE TABLE [dbo].[VehicleRequiredFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[FieldName] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleRequiredFields_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleRequiredFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehiclePayTollCategory`

Source: `WinApp/sqlData.sql:5786-5794`

```sql
CREATE TABLE [dbo].[VehiclePayTollCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VehiclePaytollCategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VehiclePayTollCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Streets`

Source: `WinApp/sqlData.sql:5800-5811`

```sql
CREATE TABLE [dbo].[Streets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StreetName] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](150) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Streets_Active_1]  DEFAULT ((1)),
 CONSTRAINT [PK_Streets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Countries`

Source: `WinApp/sqlData.sql:6079-6091`

```sql
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](150) NOT NULL,
	[CountryShortName] [nvarchar](150) NULL,
	[Citizenship] [nvarchar](50) NOT NULL CONSTRAINT [DF_Countries_Citizenship]  DEFAULT (N'нема'),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Countries_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Communities`

Source: `WinApp/sqlData.sql:6163-6175`

```sql
CREATE TABLE [dbo].[Communities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CommunityCode] [nvarchar](50) NOT NULL,
	[CommunityName] [nvarchar](50) NOT NULL,
	[RegistrationCode] [nvarchar](2) NOT NULL CONSTRAINT [DF_Communities_RegistrationCode]  DEFAULT (N'SK'),
	[Active] [bit] NOT NULL CONSTRAINT [DF_Communities_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Communities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `ColorsDetails`

Source: `WinApp/sqlData.sql:6295-6307`

```sql
CREATE TABLE [dbo].[ColorsDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdColor] [int] NOT NULL,
	[ColorCode] [nvarchar](50) NOT NULL,
	[ColorDescription] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ColorsDetails_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ColorsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `RegistrationIssuers`

Source: `WinApp/sqlData.sql:6331-6341`

```sql
CREATE TABLE [dbo].[RegistrationIssuers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssuerName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_RegistrationIssuers_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_RegistrationIssuers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Colors`

Source: `WinApp/sqlData.sql:6372-6386`

```sql
CREATE TABLE [dbo].[Colors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ColorCode] [nvarchar](50) NOT NULL,
	[ColorDescription] [nvarchar](250) NOT NULL,
	[NewColorEffects] [nvarchar](1) NULL,
	[NewColorCode] [int] NULL,
	[NewColorDarkness] [nvarchar](1) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Colors_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `RequestTypes`

Source: `WinApp/sqlData.sql:6567-6590`

```sql
CREATE TABLE [dbo].[RequestTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRequestType] [int] NULL,
	[IdDocumentPrint] [int] NOT NULL,
	[IsTehnicalExamRequired] [bit] NOT NULL,
	[IsPayRequired] [bit] NOT NULL,
	[IsNewRegistration] [bit] NOT NULL,
	[IsPreviosRegistrationReqired] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_IsPreviosRegistrationReqired]  DEFAULT ((0)),
	[IsRelationDeleted] [bit] NOT NULL,
	[IsVehicleDeleted] [bit] NOT NULL,
	[IsNewCustomer] [bit] NOT NULL,
	[IsVehicleChanged] [bit] NOT NULL,
	[IsCustomerChanged] [bit] NOT NULL,
	[IsSufficient] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_IsSufficient]  DEFAULT ((0)),
	[TypeName] [nvarchar](250) NOT NULL,
	[TypeDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequestTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Cities`

Source: `WinApp/sqlData.sql:6630-6643`

```sql
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCommunityCode] [int] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[CityZip] [int] NULL,
	[IdCountry] [int] NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Cities_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `BusinessTypes`

Source: `WinApp/sqlData.sql:8386-8397`

```sql
CREATE TABLE [dbo].[BusinessTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessTypeCode] [nvarchar](5) NOT NULL,
	[BusinessTypeDescription] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_BusinessTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_BusinessTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `AttachmentTypes`

Source: `WinApp/sqlData.sql:8403-8413`

```sql
CREATE TABLE [dbo].[AttachmentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentType] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_AttachmentTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_AttachmentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DDVCatalog`

Source: `WinApp/sqlData.sql:8424-8435`

```sql
CREATE TABLE [dbo].[DDVCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DDVName] [nvarchar](50) NOT NULL,
	[DDVValue] [decimal](18, 0) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DDVCatalog_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DDVCatalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerVehiclesRelationTypes`

Source: `WinApp/sqlData.sql:8468-8482`

```sql
CREATE TABLE [dbo].[CustomerVehiclesRelationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RelationTypeName] [nvarchar](50) NOT NULL,
	[IsCustomerOnly] [bit] NOT NULL,
	[IsOwner] [bit] NULL,
	[IsAuthorized] [bit] NULL,
	[RelationDescription] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelationTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerVehiclesRelationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategories`

Source: `WinApp/sqlData.sql:8493-8511`

```sql
CREATE TABLE [dbo].[VehicleCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](5) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[PicturePath] [nvarchar](250) NULL,
	[Deskription] [nvarchar](250) NULL,
	[DetailDescription] [ntext] NULL,
	[OldCategoryName] [nvarchar](50) NULL,
	[MKSJUS] [nvarchar](50) NULL,
	[ISO] [nvarchar](50) NULL,
	[MKSJUSDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategories_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

##### `DocumentTypes`

Source: `WinApp/sqlData.sql:8556-8571`

```sql
CREATE TABLE [dbo].[DocumentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeName] [nvarchar](50) NOT NULL,
	[IsBidirectional] [bit] NOT NULL,
	[IsVehiceRequired] [bit] NOT NULL,
	[IsTechnicalExamRequired] [bit] NOT NULL,
	[IsPayRequired] [bit] NOT NULL,
	[IdDocumentTypePrint] [int] NOT NULL CONSTRAINT [DF_DocumentTypes_IdDocumentTypePrint]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleBrakes`

Source: `WinApp/sqlData.sql:8582-8593`

```sql
CREATE TABLE [dbo].[VehicleBrakes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BreakesCode] [int] NULL,
	[BreakesDescription] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesBrakes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehiclesBrakes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypePrint`

Source: `WinApp/sqlData.sql:8618-8628`

```sql
CREATE TABLE [dbo].[DocumentTypePrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Opis] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VidoviStampa_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VidoviStampa] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleBodytype`

Source: `WinApp/sqlData.sql:8639-8651`

```sql
CREATE TABLE [dbo].[VehicleBodytype](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BodytypeCode] [nvarchar](5) NOT NULL,
	[BodytypeDescriprion] [nvarchar](250) NOT NULL,
	[OldBodytypeDescription] [nvarchar](150) NULL CONSTRAINT [DF_VehicleBodytype_OldBodytypeDescription]  DEFAULT (''),
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleBodytype_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleBodyworkShapes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DriveingLicenceCtegories`

Source: `WinApp/sqlData.sql:9079-9090`

```sql
CREATE TABLE [dbo].[DriveingLicenceCtegories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](1) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DriveingLicenceCtegories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DriveingLicenceCtegories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `EngineTypeModelRelations`

Source: `WinApp/sqlData.sql:9252-9263`

```sql
CREATE TABLE [dbo].[EngineTypeModelRelations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEngineType] [int] NOT NULL,
	[IdVehicleModel] [int] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_EngineTypeModelRelations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_EngineTypeModelRelations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEnginePowerSourceTypes`

Source: `WinApp/sqlData.sql:9540-9550`

```sql
CREATE TABLE [dbo].[VehicleEnginePowerSourceTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PowerSourceName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesEnginePowerSourceTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehiclesEnginePowerSourceTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEngineEcoProgram`

Source: `WinApp/sqlData.sql:9564-9577`

```sql
CREATE TABLE [dbo].[VehicleEngineEcoProgram](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[EcoProgram] [nvarchar](250) NOT NULL,
	[TechincalDescription] [nvarchar](250) NOT NULL,
	[PercentForPayment] [real] NOT NULL CONSTRAINT [DF_VehicleEngineEcoProgram_PercentForPayment]  DEFAULT ((4)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleEngineMarks_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleEngineMarks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PriceCatalog`

Source: `WinApp/sqlData.sql:9615-9632`

```sql
CREATE TABLE [dbo].[PriceCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDDVCatalog] [int] NOT NULL,
	[IsRequest] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsRequest]  DEFAULT ((0)),
	[IsTehnicalExam] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsTehnicalExam]  DEFAULT ((0)),
	[IsTrafficLicence] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsTrafficLicence]  DEFAULT ((0)),
	[IsPermisionForVehicle] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsPermisionForVehicle]  DEFAULT ((0)),
	[IsInernationalDriveingLicence] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsInernationalDriveingLicence]  DEFAULT ((0)),
	[Name] [nvarchar](150) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PriceList_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PriceList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategoriesRelations`

Source: `WinApp/sqlData.sql:9642-9656`

```sql
CREATE TABLE [dbo].[VehicleCategoriesRelations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[IdBodytype] [int] NOT NULL,
	[IdUse] [int] NULL,
	[IdVehicleCategoryForPayments] [int] NULL,
	[DetailDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategoriesRelations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleCategoriesRelations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleModel`

Source: `WinApp/sqlData.sql:10758-10772`

```sql
CREATE TABLE [dbo].[VehicleModel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleMaker] [int] NOT NULL CONSTRAINT [DF_VehicleModel_IdVehicleMaker]  DEFAULT ((1)),
	[ModelCode] [nvarchar](250) NULL,
	[ModelName] [nvarchar](250) NOT NULL,
	[YearOfBeginingProduction] [datetime] NULL CONSTRAINT [DF_VehicleModel_YearOfBeginingProduction]  DEFAULT (getdate()),
	[YearOfEndingProduction] [datetime] NULL CONSTRAINT [DF_VehicleModel_YearOfEndingProduction]  DEFAULT (getdate()),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleModel_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypesOptions`

Source: `WinApp/sqlData.sql:12525-12540`

```sql
CREATE TABLE [dbo].[DocumentTypesOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentTypes] [int] NOT NULL,
	[OptionName] [nvarchar](500) NOT NULL,
	[IsNewRegistration] [bit] NOT NULL,
	[IsTehnicalExamRquired] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_IsTehnicalExamRquired]  DEFAULT ((0)),
	[RelationDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_RelationDeleted]  DEFAULT ((0)),
	[VehicleDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_VehicleDeleted]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentTypesOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypesOptionsDetails`

Source: `WinApp/sqlData.sql:12560-12574`

```sql
CREATE TABLE [dbo].[DocumentTypesOptionsDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentTypesOptions] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[IsVehicleDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsVehicleDeleted]  DEFAULT ((0)),
	[IsNewCustomer] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsNewCustomer]  DEFAULT ((0)),
	[IsRelationDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsRelationDeleted]  DEFAULT ((0)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_DocumentTypesOptionsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEngineTypes`

Source: `WinApp/sqlData.sql:12667-12683`

```sql
CREATE TABLE [dbo].[VehicleEngineTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleMaker] [int] NOT NULL,
	[EngineTypeCode] [nvarchar](250) NOT NULL,
	[TechincalDescription] [nvarchar](250) NULL,
	[IdDefaultPowerSource] [int] NULL,
	[DefaultPower] [real] NULL,
	[DefaultPowerOutPut] [real] NULL,
	[DefaultTorque] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleEngineTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleEngineTyes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 214 procs.

| Procedure | Source line |
|---|---|
| `NumOfCustomerCountryNameExists` | sqlData.sql:18762 |
| `NumOfCustomerCountryShortNameExists` | sqlData.sql:18748 |
| `PrintDocumentInternatiomalDriveingLicence` | sqlData.sql:16313 |
| `PrintDocumentsTehnicalExamsReports` | sqlData.sql:14261 |
| `TehnicalExamPassed` | sqlData.sql:20936 |
| `addBusinessType` | sqlData.sql:21129 |
| `addCitie` | sqlData.sql:20397 |
| `addColor` | sqlData.sql:20009 |
| `addColorsDetail` | sqlData.sql:19766 |
| `addCommunitie` | sqlData.sql:19265 |
| `addCountrie` | sqlData.sql:18776 |
| `addCustomerFinancialStatFromTehnicalExam` | sqlData.sql:24214 |
| `addDDVCatalo` | sqlData.sql:21512 |
| `addDocumentTypePrin` | sqlData.sql:22678 |
| `addDocumentType` | sqlData.sql:22442 |
| `addDocumentTypesOption` | sqlData.sql:22154 |
| `addDocumentTypesOptionsDetail` | sqlData.sql:19485 |
| `addDocumentsTehnicalExamsReportsDetail` | sqlData.sql:20093 |
| `addDocumentsTehnicalExamsReportsDetailsStatu` | sqlData.sql:23370 |
| `addDriveingLicenceCtegorie` | sqlData.sql:23410 |
| `addEngineTypeModelRelation` | sqlData.sql:24703 |
| `addPriceCatalog` | sqlData.sql:26415 |
| `addRegistrationIssuer` | sqlData.sql:20340 |
| `addRequestType` | sqlData.sql:20667 |
| `addStreet` | sqlData.sql:18672 |
| `addTehnicalExamOrganization` | sqlData.sql:16114 |
| `addTehnicalExamVehiclePart` | sqlData.sql:17908 |
| `addTehnicalExamVehiclePartsCategorie` | sqlData.sql:17738 |
| `addVehicleBrake` | sqlData.sql:22620 |
| `addVehicleCategorie` | sqlData.sql:22363 |
| `addVehicleCategoriesRelation` | sqlData.sql:15941 |
| `addVehicleDisabledField` | sqlData.sql:13056 |
| `addVehicleEnginePowerSourceType` | sqlData.sql:25992 |
| `addVehicleEngineType` | sqlData.sql:15743 |
| `addVehicleGearBox` | sqlData.sql:15211 |
| `addVehicleMaker` | sqlData.sql:14230 |
| `addVehicleRequiredField` | sqlData.sql:18090 |
| `addVehicleTireType` | sqlData.sql:15670 |
| `deleteBusinessType` | sqlData.sql:21157 |
| `deleteCitie` | sqlData.sql:20500 |
| `deleteColor` | sqlData.sql:19953 |
| `deleteColorsDetail` | sqlData.sql:19677 |
| `deleteCommunitie` | sqlData.sql:19324 |
| `deleteCountrie` | sqlData.sql:18835 |
| `deleteDDVCatalo` | sqlData.sql:21436 |
| `deleteDocumentTypePrin` | sqlData.sql:22663 |
| `deleteDocumentType` | sqlData.sql:22427 |
| `deleteDocumentTypesOption` | sqlData.sql:22259 |
| `deleteDocumentTypesOptionsDetail` | sqlData.sql:19470 |
| `deleteDocumentsTehnicalExamsReportsDetail` | sqlData.sql:20078 |
| `deleteDocumentsTehnicalExamsReportsDetailsStatu` | sqlData.sql:23395 |
| `deleteDriveingLicenceCtegorie` | sqlData.sql:23438 |
| `deleteEngineTypeModelRelation` | sqlData.sql:24688 |
| `deletePriceCatalog` | sqlData.sql:26461 |
| `deleteRegistrationIssuer` | sqlData.sql:20382 |
| `deleteRequestType` | sqlData.sql:20572 |
| `deleteStreet` | sqlData.sql:18376 |
| `deleteTehnicalExamOrganization` | sqlData.sql:16224 |
| `deleteTehnicalExamVehiclePart` | sqlData.sql:17893 |
| `deleteTehnicalExamVehiclePartsCategorie` | sqlData.sql:17766 |
| `deleteVehicleBrake` | sqlData.sql:22648 |
| `deleteVehicleCategorie` | sqlData.sql:22412 |
| `deleteVehicleCategoriesRelation` | sqlData.sql:15926 |
| `deleteVehicleDisabledField` | sqlData.sql:13084 |
| `deleteVehicleEnginePowerSourceType` | sqlData.sql:26017 |
| `deleteVehicleEngineType` | sqlData.sql:15786 |
| `deleteVehicleGearBox` | sqlData.sql:15196 |
| `deleteVehicleMaker` | sqlData.sql:14215 |
| `deleteVehicleRequiredField` | sqlData.sql:18195 |
| `deleteVehicleTireType` | sqlData.sql:15623 |
| `depListOfIncorectTehnicalExams` | sqlData.sql:14781 |
| `getBusinessTypeById` | sqlData.sql:21085 |
| `getBusinessTypes` | sqlData.sql:21068 |
| `getCitieById` | sqlData.sql:20461 |
| `getCitiesCommunitiesCountriesList` | sqlData.sql:18726 |
| `getCities` | sqlData.sql:20481 |
| `getColorById` | sqlData.sql:19988 |
| `getColorsDetailByIdColor` | sqlData.sql:19692 |
| `getColorsDetailById` | sqlData.sql:19727 |
| `getColorsDetailsList` | sqlData.sql:19746 |
| `getColorsDetails` | sqlData.sql:19711 |
| `getColors` | sqlData.sql:19968 |
| `getCommunitieById` | sqlData.sql:19357 |
| `getCommunities` | sqlData.sql:19339 |
| `getCountrieById` | sqlData.sql:18868 |
| `getCountries` | sqlData.sql:18850 |
| `getDDVCataloById` | sqlData.sql:21468 |
| `getDDVCatalog` | sqlData.sql:21451 |
| `getDocumentTypeById` | sqlData.sql:22516 |
| `getDocumentTypePrinById` | sqlData.sql:22743 |
| `getDocumentTypePrint` | sqlData.sql:22727 |
| `getDocumentTypesOptionByIdDocumentTypes` | sqlData.sql:22194 |
| `getDocumentTypesOptionById` | sqlData.sql:22216 |
| `getDocumentTypesOptionsDetailByIdDocumentTypesOptions` | sqlData.sql:19397 |
| `getDocumentTypesOptionsDetailById` | sqlData.sql:19376 |
| `getDocumentTypesOptionsDetails` | sqlData.sql:19418 |
| `getDocumentTypesOptions` | sqlData.sql:22238 |
| `getDocumentTypes` | sqlData.sql:22538 |
| `getDocumentsActiveTehnicalExamsReportByIdVehicle` | sqlData.sql:18905 |
| `getDocumentsInternationalDriveingLicencesList` | sqlData.sql:16176 |
| `getDocumentsRequestsWithoutTehnicalExam` | sqlData.sql:14581 |
| `getDocumentsTehnicalExamsReportsDetailByIdTehnicalExamsReports` | sqlData.sql:20167 |
| `getDocumentsTehnicalExamsReportsDetailById` | sqlData.sql:20142 |
| `getDocumentsTehnicalExamsReportsDetailsStatuById` | sqlData.sql:23353 |
| `getDocumentsTehnicalExamsReportsDetailsStatus` | sqlData.sql:23337 |
| `getDocumentsTehnicalExamsReportsDetails` | sqlData.sql:20194 |
| `getDocumentsTehnicalExamsReportsListBetweenDates` | sqlData.sql:14535 |
| `getDocumentsTehnicalExamsReportsListByVallidTill` | sqlData.sql:14305 |
| `getDocumentsTehnicalExamsReportsListVehicleRightOrNot` | sqlData.sql:14335 |
| `getDocumentsTehnicalExamsReportsList` | sqlData.sql:14504 |
| `getDriveingLicenceCtegorieById` | sqlData.sql:23479 |
| `getDriveingLicenceCtegories` | sqlData.sql:23497 |
| `getEngineTypeModelRelationById` | sqlData.sql:24731 |
| `getEngineTypeModelRelations` | sqlData.sql:24749 |
| `getLastVehicleTehnicalExam` | sqlData.sql:16196 |
| `getPriceCatalogById` | sqlData.sql:26568 |
| `getPriceCatalogForInernationalDriveingLicences` | sqlData.sql:26499 |
| `getPriceCatalogForPermisionForVehicles` | sqlData.sql:26545 |
| `getPriceCatalogForRequests` | sqlData.sql:26615 |
| `getPriceCatalogForTehnicalExams` | sqlData.sql:26592 |
| `getPriceCatalogForTrafficLicences` | sqlData.sql:26522 |
| `getPriceCatalog` | sqlData.sql:26476 |
| `getRegistrationIssuerById` | sqlData.sql:20258 |
| `getRegistrationIssuers` | sqlData.sql:20275 |
| `getRequestByIdTechnicalExamReport` | sqlData.sql:20879 |
| `getRequestTypeByIdRequestType` | sqlData.sql:20513 |
| `getRequestTypeById` | sqlData.sql:20587 |
| `getRequestTypes` | sqlData.sql:20543 |
| `getStreetById` | sqlData.sql:18421 |
| `getStreets` | sqlData.sql:18483 |
| `getTehnicalExamOrganizationById` | sqlData.sql:16434 |
| `getTehnicalExamOrganizationsBezMomentalnaStanica` | sqlData.sql:16521 |
| `getTehnicalExamOrganizations` | sqlData.sql:16416 |
| `getTehnicalExamVehiclePartById` | sqlData.sql:17873 |
| `getTehnicalExamVehiclePartsCategorieById` | sqlData.sql:17798 |
| `getTehnicalExamVehiclePartsCategories` | sqlData.sql:17781 |
| `getTehnicalExamVehiclePartsListByIdCategory` | sqlData.sql:17816 |
| `getTehnicalExamVehiclePartsList` | sqlData.sql:17836 |
| `getTehnicalExamVehicleParts` | sqlData.sql:17854 |
| `getVehicleBodytypeByIdCategory` | sqlData.sql:15906 |
| `getVehicleBodytypeCategoryForPayByIdCategory` | sqlData.sql:13335 |
| `getVehicleBodytypeCategoryForPay` | sqlData.sql:13315 |
| `getVehicleBodytype` | sqlData.sql:22779 |
| `getVehicleBrakeById` | sqlData.sql:22585 |
| `getVehicleBrakes` | sqlData.sql:22603 |
| `getVehicleCategorieById` | sqlData.sql:22298 |
| `getVehicleCategoriesRelationByIdCategory` | sqlData.sql:15865 |
| `getVehicleCategoriesRelationById` | sqlData.sql:15844 |
| `getVehicleCategoriesRelations` | sqlData.sql:15886 |
| `getVehicleCategories` | sqlData.sql:22274 |
| `getVehicleCategoryPaymentsByCategoryAndBodytype` | sqlData.sql:13255 |
| `getVehicleDisabledFieldByIdCategory` | sqlData.sql:13038 |
| `getVehicleDisabledFieldById` | sqlData.sql:13022 |
| `getVehicleDisabledFields` | sqlData.sql:13005 |
| `getVehicleEnginePowerSourceTypeById` | sqlData.sql:26072 |
| `getVehicleEnginePowerSourceTypes` | sqlData.sql:26056 |
| `getVehicleEngineTypeById` | sqlData.sql:15801 |
| `getVehicleEngineTypesByIdVehicleModel` | sqlData.sql:14929 |
| `getVehicleEngineTypesList` | sqlData.sql:14947 |
| `getVehicleEngineTypes` | sqlData.sql:15824 |
| `getVehicleGearBoxById` | sqlData.sql:15265 |
| `getVehicleGearBox` | sqlData.sql:15283 |
| `getVehicleMakerById` | sqlData.sql:14964 |
| `getVehicleMakers` | sqlData.sql:14983 |
| `getVehicleModel` | sqlData.sql:15321 |
| `getVehicleModelsList` | sqlData.sql:14880 |
| `getVehicleRequiredFieldByIdCategory` | sqlData.sql:18159 |
| `getVehicleRequiredFieldById` | sqlData.sql:18177 |
| `getVehicleRequiredFields` | sqlData.sql:18144 |
| `getVehicleSupporting` | sqlData.sql:16097 |
| `getVehicleTireTypeByIdVehicleModel` | sqlData.sql:15561 |
| `getVehicleTireTypeById` | sqlData.sql:15542 |
| `getVehicleTireTypesByIdModel` | sqlData.sql:15602 |
| `getVehicleTireTypes` | sqlData.sql:15582 |
| `getVehicleUseByCategoryAndBodytype` | sqlData.sql:15506 |
| `insertFinancialStatePriceCatalogForIternationalDriveingLicences` | sqlData.sql:24172 |
| `insertFinancialStatePriceCatalogForPermisions` | sqlData.sql:24130 |
| `insertFinancialStatePriceCatalogForRequests` | sqlData.sql:24077 |
| `insertFinancialStatePriceCatalogForTehnicalExams` | sqlData.sql:24024 |
| `insertFinancialStatePriceCatalogForTrafficLicences` | sqlData.sql:23982 |
| `updateBusinessType` | sqlData.sql:21103 |
| `updateCitie` | sqlData.sql:20431 |
| `updateColor` | sqlData.sql:20046 |
| `updateColorsDetail` | sqlData.sql:19797 |
| `updateCommunitie` | sqlData.sql:19296 |
| `updateCountrie` | sqlData.sql:18807 |
| `updateCustomerVehiclesRelationsByDocumentTypeOptionDetail` | sqlData.sql:16702 |
| `updateCustomerVehiclesRelationsByDocumentTypeOption` | sqlData.sql:16748 |
| `updateDDVCatalo` | sqlData.sql:21486 |
| `updateDocumentTypePrin` | sqlData.sql:22703 |
| `updateDocumentType` | sqlData.sql:22482 |
| `updateDocumentTypesOption` | sqlData.sql:22120 |
| `updateDocumentTypesOptionsDetail` | sqlData.sql:19438 |
| `updateDocumentsTehnicalExamsReportsDetail` | sqlData.sql:20218 |
| `updateDocumentsTehnicalExamsReportsDetailsStatu` | sqlData.sql:23313 |
| `updateDriveingLicenceCtegorie` | sqlData.sql:23453 |
| `updateEngineTypeModelRelation` | sqlData.sql:24766 |
| `updatePriceCatalog` | sqlData.sql:26377 |
| `updateRegistrationIssuer` | sqlData.sql:20316 |
| `updateRequestType` | sqlData.sql:20617 |
| `updateStreet` | sqlData.sql:18700 |
| `updateTehnicalExamOrganization` | sqlData.sql:16145 |
| `updateTehnicalExamVehiclePart` | sqlData.sql:17942 |
| `updateTehnicalExamVehiclePartsCategorie` | sqlData.sql:17712 |
| `updateVehicleBrake` | sqlData.sql:22559 |
| `updateVehicleCategorie` | sqlData.sql:22323 |
| `updateVehicleCategoriesRelation` | sqlData.sql:15978 |
| `updateVehicleDisabledField` | sqlData.sql:12979 |
| `updateVehicleEnginePowerSourceType` | sqlData.sql:26032 |
| `updateVehicleEngineType` | sqlData.sql:15707 |
| `updateVehicleGearBox` | sqlData.sql:15239 |
| `updateVehicleMaker` | sqlData.sql:14394 |
| `updateVehicleRequiredField` | sqlData.sql:18118 |
| `updateVehicleTireType` | sqlData.sql:15638 |

#### 4.1.3 Screens
#### 4.1.4 Business rules
#### 4.1.5 Print templates
#### 4.1.6 Open questions
#### 4.1.7 Migration notes

### 4.2 Customers (CUS)

#### 4.2.1 Purpose
#### 4.2.2 Tables

##### `CustomersBankAccounts`

Source: `WinApp/sqlData.sql:9381-9394`

```sql
CREATE TABLE [dbo].[CustomersBankAccounts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[BankAccount] [nvarchar](50) NOT NULL,
	[DeponentBank] [nvarchar](50) NOT NULL,
	[TaxNumber] [nvarchar](15) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomersBankAccounts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_CustomersBankAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Customers`

Source: `WinApp/sqlData.sql:10673-10704`

```sql
CREATE TABLE [dbo].[Customers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MB] [nvarchar](13) NULL,
	[CustomerSurname] [nvarchar](100) NULL,
	[CustomerFirstName] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Fax] [nvarchar](20) NULL,
	[IdLivingAddress] [int] NULL,
	[LivingAddressNumber] [nvarchar](100) NULL,
	[IdLivingCity] [int] NULL,
	[IdBirhCity] [int] NULL,
	[IdBirthAddress] [int] NULL,
	[BrithAddressNumber] [nvarchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[IdCitizenship] [int] NULL,
	[IsCompany] [bit] NOT NULL CONSTRAINT [DF_Customers_IsCompany]  DEFAULT ((0)),
	[Occupation] [nvarchar](50) NULL,
	[WorksInCompany] [nvarchar](150) NULL,
	[IdBusinessType] [int] NULL,
	[eMail] [nvarchar](100) NULL,
	[PassportNumber] [nvarchar](100) NULL,
	[BLK] [nvarchar](50) NULL,
	[CanSendNotifications] [bit] NOT NULL CONSTRAINT [DF_Customers_CanSendNotifications]  DEFAULT ((0)),
	[TaxNumber] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Customers_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerFinancialState`

Source: `WinApp/sqlData.sql:11842-11861`

```sql
CREATE TABLE [dbo].[CustomerFinancialState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL CONSTRAINT [DF_Table_1_IdCustomer]  DEFAULT ((0)),
	[IdDocument] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocument]  DEFAULT ((0)),
	[IdDocumentTehnicalExam] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentTehnicalExam]  DEFAULT ((0)),
	[IdDocumentsTrafficLicences] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentsTrafficLicences]  DEFAULT ((0)),
	[IdDocumentIternationalDriveingLicence] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentIternationalDriveingLicence]  DEFAULT ((0)),
	[IdDocumentPermisions] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentPermisions]  DEFAULT ((0)),
	[IdPriceCatalog] [int] NOT NULL,
	[Note] [nvarchar](150) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_CustomerFinancialState_Payed]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerFinancialState_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerFinancialState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Customers.ContactPersons`

Source: `WinApp/sqlData.sql:12628-12644`

```sql
CREATE TABLE [dbo].[Customers.ContactPersons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[MB] [nvarchar](13) NOT NULL,
	[PersonName] [nvarchar](50) NOT NULL,
	[PersonSurname] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[MobileNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Customers.ContactPersons_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Customers.ContactPersons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 31 procs.

| Procedure | Source line |
|---|---|
| `ExistsRelationCustomerOnly` | sqlData.sql:19056 |
| `NumOfCustomerMBExists` | sqlData.sql:24468 |
| `PrintCustomerById` | sqlData.sql:18563 |
| `PrintCustomer` | sqlData.sql:18503 |
| `addCustomerFinancialStat` | sqlData.sql:24274 |
| `addCustomer` | sqlData.sql:24563 |
| `addCustomersBankAccount` | sqlData.sql:24879 |
| `addCustomersContactPerson` | sqlData.sql:21829 |
| `deleteCustomerDept` | sqlData.sql:24415 |
| `deleteCustomerFinancialStat` | sqlData.sql:24400 |
| `deleteCustomer` | sqlData.sql:24482 |
| `deleteCustomersBankAccount` | sqlData.sql:24913 |
| `deleteCustomersContactPerson` | sqlData.sql:21940 |
| `depCustomerFinansicalState` | sqlData.sql:15004 |
| `getCustomerById` | sqlData.sql:24430 |
| `getCustomerFinancialStatById` | sqlData.sql:24374 |
| `getCustomerFinancialStateList` | sqlData.sql:16896 |
| `getCustomerFinancialState` | sqlData.sql:24349 |
| `getCustomersBankAccountByIdCustomer` | sqlData.sql:24790 |
| `getCustomersBankAccountById` | sqlData.sql:24810 |
| `getCustomersBankAccounts` | sqlData.sql:24830 |
| `getCustomersContactPersonByIdCustomer` | sqlData.sql:21917 |
| `getCustomersContactPersonById` | sqlData.sql:21894 |
| `getCustomersContactPersons` | sqlData.sql:21872 |
| `getCustomersListByIsCompany` | sqlData.sql:18588 |
| `getCustomersList` | sqlData.sql:18630 |
| `getCustomers` | sqlData.sql:24651 |
| `updateCustomerFinancialStat` | sqlData.sql:23917 |
| `updateCustomer` | sqlData.sql:24497 |
| `updateCustomersBankAccount` | sqlData.sql:24849 |
| `updateCustomersContactPerson` | sqlData.sql:21793 |

#### 4.2.3 Screens
#### 4.2.4 Business rules
#### 4.2.5 Print templates
#### 4.2.6 Open questions
#### 4.2.7 Migration notes

### 4.3 Vehicles (VEH)

#### 4.3.1 Purpose
#### 4.3.2 Tables

##### `Vehicles`

Source: `WinApp/sqlData.sql:5401-5471`

```sql
CREATE TABLE [dbo].[Vehicles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicleBodyType] [int] NULL,
	[IdVehicleCategories] [int] NOT NULL,
	[IdEngineType] [int] NULL,
	[IdEnginePowerSource] [int] NULL,
	[IdEngineSecondPowerSource] [int] NULL,
	[IdGearBox] [int] NULL,
	[IdBreakes] [int] NULL,
	[IdSupporting] [int] NULL,
	[IdVehicleModel] [int] NULL,
	[IdPrimaryColor] [int] NULL CONSTRAINT [DF_Vehicles_IdPrimaryColor]  DEFAULT ((0)),
	[IdSecondaryColor] [int] NULL,
	[IdVehicleCategoryForPayments] [int] NULL,
	[IdEngineEcoProgram] [int] NULL,
	[IdMadeCountry] [int] NULL,
	[ColorCode] [nvarchar](50) NULL,
	[EngineNumber] [nvarchar](20) NULL,
	[EnginePower] [real] NULL,
	[EngineTorque] [nvarchar](250) NULL,
	[EngineTorqueUnderGass] [real] NULL,
	[EngineWorkingCapacity] [real] NULL,
	[EnginePowerOutPut] [real] NULL,
	[ShellNumber] [nvarchar](17) NULL,
	[MakeDate] [datetime] NOT NULL,
	[NumberOfDoors] [int] NULL,
	[NumberOfSeats] [smallint] NULL,
	[NumberOfStandingSeats] [smallint] NULL,
	[NumberOfLieingSeats] [smallint] NULL,
	[EmptyWaight] [real] NULL,
	[MaximunAllowedWaight] [real] NULL,
	[TrailerWaightWithBreak] [nvarchar](20) NULL,
	[TrailerWaightWithoutBreak] [nvarchar](20) NULL,
	[NumberOfAxis] [int] NULL,
	[PropulsionAxis] [int] NULL,
	[NumberOfWheels] [int] NULL,
	[NumberOfPropulsionWheels] [int] NULL,
	[VehicleSizeHight] [real] NULL,
	[VehicleSizeWidth] [real] NULL,
	[VehicleSizeLength] [real] NULL,
	[Suffocation] [bit] NULL CONSTRAINT [DF_Vehicles_Suffocation]  DEFAULT ((0)),
	[Hook] [bit] NULL CONSTRAINT [DF_Vehicles_Hook]  DEFAULT ((0)),
	[Vitlo] [bit] NULL CONSTRAINT [DF_Vehicles_Vitlo]  DEFAULT ((0)),
	[VerticalBurdenOnTheSeat] [bit] NULL,
	[VerticalBurdenOnTheSeatNote] [nvarchar](250) NULL,
	[HologationSertificateNumber] [nvarchar](100) NULL,
	[NoiseStatic] [real] NULL,
	[NoiseMovment] [real] NULL,
	[CO] [real] NULL,
	[HC] [real] NULL,
	[NOx] [real] NULL,
	[HCNOx] [real] NULL,
	[Blackening] [nvarchar](20) NULL,
	[Pinpoints] [nvarchar](20) NULL,
	[CO2] [real] NULL,
	[FuelConsumption] [nvarchar](20) NULL,
	[CapacityFuelTank] [real] NULL,
	[Note] [nvarchar](500) NULL,
	[IsSocialNotPrivate] [bit] NULL CONSTRAINT [DF_Vehicles_IsSocialNotPrivate]  DEFAULT ((0)),
	[ForPrivateTransportNotPublic] [bit] NULL CONSTRAINT [DF_Vehicles_ForPrivateTransportNotPublic]  DEFAULT ((1)),
	[TNG] [bit] NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicles_Active]  DEFAULT ((1)),
	[IdVehicleUse] [int] NOT NULL CONSTRAINT [DF_Vehicles_IdVehicleUse]  DEFAULT ((0)),
	[VehicleModelAdding] [nvarchar](200) NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Registrations`

Source: `WinApp/sqlData.sql:8446-8462`

```sql
CREATE TABLE [dbo].[Vehicle.Registrations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[IdRegistrationIssuer] [int] NOT NULL CONSTRAINT [DF_Vehicle.Registrations_IdRegistrationIssuer]  DEFAULT ((1)),
	[RegistrationNumber] [nvarchar](10) NOT NULL,
	[DateOfRegistration] [datetime] NOT NULL,
	[DateRegistrationValidTill] [datetime] NOT NULL,
	[PlaceOfRegistration] [nvarchar](150) NOT NULL,
	[IsFirstRegistration] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Registrations_IsFirstRegistration]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Registration_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.Registration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerVehiclesRelations`

Source: `WinApp/sqlData.sql:10710-10726`

```sql
CREATE TABLE [dbo].[CustomerVehiclesRelations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRelationType] [int] NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[IdVehicle] [bigint] NULL,
	[StartDate] [datetime] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelations_StartDate]  DEFAULT (getdate()),
	[EndDate] [datetime] NULL,
	[BeginNote] [nvarchar](250) NULL,
	[TerminationNote] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelations_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerVehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Axis`

Source: `WinApp/sqlData.sql:11579-11592`

```sql
CREATE TABLE [dbo].[Vehicle.Axis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[AxisNumber] [int] NOT NULL,
	[CarryingCapacity] [real] NOT NULL,
	[AxisLength] [real] NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.AxisCarryingCapacity_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.AxisCarryingCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.BetweenAxesDestinations`

Source: `WinApp/sqlData.sql:11598-11610`

```sql
CREATE TABLE [dbo].[Vehicle.BetweenAxesDestinations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[FromTo] [nvarchar](5) NOT NULL,
	[Destination] [decimal](18, 0) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.BetweenAxesDestinations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Vehicle.BetweenAxesDestinations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Tyres`

Source: `WinApp/sqlData.sql:11767-11778`

```sql
CREATE TABLE [dbo].[Vehicle.Tyres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[IdTireType] [int] NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Tyres_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.Tyres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehiclePayToll`

Source: `WinApp/sqlData.sql:11820-11836`

```sql
CREATE TABLE [dbo].[VehiclePayToll](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehiclePaytollCategory] [int] NOT NULL,
	[LitreFrom] [decimal](18, 0) NOT NULL,
	[LitreTo] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[NumberOfSeatsFrom] [int] NULL,
	[NumberOfSeatsTo] [int] NULL,
	[TypeOfMeasure] [nvarchar](5) NULL,
	[MeasureFrom] [decimal](18, 0) NULL,
	[MeasureTo] [decimal](18, 0) NULL,
 CONSTRAINT [PK_VehiclePayToll] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 77 procs.

| Procedure | Source line |
|---|---|
| `EndPreviosRegistrationsForVehicleId` | sqlData.sql:21607 |
| `NumOfVehiclesRegistrationNumberExists` | sqlData.sql:21635 |
| `NumOfVehiclesShellExists` | sqlData.sql:16954 |
| `ProveriRelacijaSoVozilo` | sqlData.sql:18960 |
| `addCustomerVehicleRelationsHistor` | sqlData.sql:9355 |
| `addCustomerVehiclesRelationType` | sqlData.sql:22049 |
| `addCustomerVehiclesRelation` | sqlData.sql:19222 |
| `addVehicleAxi` | sqlData.sql:14151 |
| `addVehicleBetweenAxesDestination` | sqlData.sql:13127 |
| `addVehicleBodytyp` | sqlData.sql:22825 |
| `addVehicleMode` | sqlData.sql:15356 |
| `addVehicleSupportin` | sqlData.sql:16036 |
| `addVehicleTyre` | sqlData.sql:12859 |
| `addVehicle` | sqlData.sql:17121 |
| `deleteCustomerVehicleRelationsHistor` | sqlData.sql:9413 |
| `deleteCustomerVehiclesRelationType` | sqlData.sql:22086 |
| `deleteCustomerVehiclesRelation` | sqlData.sql:19078 |
| `deleteVehicleAxi` | sqlData.sql:14136 |
| `deleteVehicleBetweenAxesDestination` | sqlData.sql:13158 |
| `deleteVehicleBodytyp` | sqlData.sql:22856 |
| `deleteVehicleMode` | sqlData.sql:15341 |
| `deleteVehicleSupportin` | sqlData.sql:16064 |
| `deleteVehicleTyre` | sqlData.sql:12887 |
| `deleteVehicle` | sqlData.sql:16578 |
| `existsCustomerVehicleRelation` | sqlData.sql:19198 |
| `getCustomerFinancialStateByIdCustomerVehicleRelation` | sqlData.sql:16872 |
| `getCustomerVehicleRelationsHistorById` | sqlData.sql:9337 |
| `getCustomerVehicleRelationsHistory` | sqlData.sql:9313 |
| `getCustomerVehiclesRelationById` | sqlData.sql:19172 |
| `getCustomerVehiclesRelationByType` | sqlData.sql:19093 |
| `getCustomerVehiclesRelationIdByCustomerAndVehicle` | sqlData.sql:19116 |
| `getCustomerVehiclesRelationListByType` | sqlData.sql:16920 |
| `getCustomerVehiclesRelationTypeByCustomerAndVehicle` | sqlData.sql:19022 |
| `getCustomerVehiclesRelationTypeById` | sqlData.sql:21996 |
| `getCustomerVehiclesRelationTypesByIsCustomerOnly` | sqlData.sql:21955 |
| `getCustomerVehiclesRelationTypes` | sqlData.sql:21976 |
| `getCustomerVehiclesRelationsByCustomer` | sqlData.sql:17470 |
| `getCustomerVehiclesRelationsByVehicle` | sqlData.sql:17438 |
| `getCustomerVehiclesRelationsListByCustomerVehicleAndType` | sqlData.sql:17675 |
| `getCustomerVehiclesRelationsList` | sqlData.sql:17501 |
| `getCustomerVehiclesRelations` | sqlData.sql:19150 |
| `getVahicleFirstRegistration` | sqlData.sql:21695 |
| `getVehicleAxiByIdVehicle` | sqlData.sql:14096 |
| `getVehicleAxiById` | sqlData.sql:14116 |
| `getVehicleAxis` | sqlData.sql:14079 |
| `getVehicleBetweenAxesDestinationByIdVehicle` | sqlData.sql:13208 |
| `getVehicleBetweenAxesDestinationById` | sqlData.sql:13173 |
| `getVehicleBetweenAxesDestinations` | sqlData.sql:13192 |
| `getVehicleBodytypById` | sqlData.sql:22760 |
| `getVehicleById` | sqlData.sql:16968 |
| `getVehicleCurrentOwner` | sqlData.sql:18979 |
| `getVehicleFirstRegistration` | sqlData.sql:21649 |
| `getVehicleLastOwner` | sqlData.sql:19003 |
| `getVehicleLastRegistration` | sqlData.sql:21587 |
| `getVehicleModeById` | sqlData.sql:15300 |
| `getVehicleRegistrationByIdList` | sqlData.sql:20365 |
| `getVehicleRegistrationByIdVehicleList` | sqlData.sql:20289 |
| `getVehicleSupportinById` | sqlData.sql:16079 |
| `getVehicleTyreByIdVehicle` | sqlData.sql:12918 |
| `getVehicleTyreById` | sqlData.sql:12902 |
| `getVehicleTyres` | sqlData.sql:12936 |
| `getVehiclesListOdjaveniVozila` | sqlData.sql:14827 |
| `getVehiclesListVozilaZaBel` | sqlData.sql:14895 |
| `getVehiclesListWithOwners` | sqlData.sql:14755 |
| `getVehiclesList` | sqlData.sql:14725 |
| `getVehiclesNotActive` | sqlData.sql:17326 |
| `getVehicles` | sqlData.sql:17045 |
| `updateCustomerVehicleRelationsHistor` | sqlData.sql:9013 |
| `updateCustomerVehiclesRelationType` | sqlData.sql:22017 |
| `updateCustomerVehiclesRelation` | sqlData.sql:18924 |
| `updateVehicleAxi` | sqlData.sql:14185 |
| `updateVehicleBetweenAxesDestination` | sqlData.sql:13099 |
| `updateVehicleBodytyp` | sqlData.sql:22797 |
| `updateVehicleMode` | sqlData.sql:15393 |
| `updateVehicleSupportin` | sqlData.sql:16010 |
| `updateVehicleTyre` | sqlData.sql:12953 |
| `updateVehicle` | sqlData.sql:17531 |

#### 4.3.3 Screens
#### 4.3.4 Business rules
#### 4.3.5 Print templates
#### 4.3.6 Open questions
#### 4.3.7 Migration notes

### 4.4 Requests (REQ)

#### 4.4.1 Purpose
#### 4.4.2 Tables

##### `Request.VehicleOwnershipProofs`

Source: `WinApp/sqlData.sql:6277-6289`

```sql
CREATE TABLE [dbo].[Request.VehicleOwnershipProofs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequest] [bigint] NOT NULL,
	[IdVehicleOwnershipProof] [int] NOT NULL,
	[VehicleOwnershipProof] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Request.VehicleOwnershipProofs_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Request.VehicleOwnershipProofs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Request.PaymentProof`

Source: `WinApp/sqlData.sql:6313-6325`

```sql
CREATE TABLE [dbo].[Request.PaymentProof](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequest] [bigint] NOT NULL,
	[IdPaymentProof] [int] NOT NULL,
	[PaymentProof] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Request.PaymentProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Request.PaymentProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Requests`

Source: `WinApp/sqlData.sql:8357-8380`

```sql
CREATE TABLE [dbo].[Requests](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequestType] [int] NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationNew] [bigint] NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdOperatorModified] [int] NULL,
	[IdOperatorEnded] [int] NOT NULL CONSTRAINT [DF_Requests_IdOperatorEnded]  DEFAULT ((0)),
	[IdTechnicalExamReport] [bigint] NULL,
	[IdPreviousRegistration] [int] NOT NULL CONSTRAINT [DF_Requests_IdPreviousRegistration]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[DateEnded] [datetime] NULL,
	[Note] [nvarchar](250) NULL,
	[IsCustomerChanged] [bit] NOT NULL CONSTRAINT [DF_Requests_IsCustomerChanged]  DEFAULT ((0)),
	[IsVehicleChanged] [bit] NOT NULL CONSTRAINT [DF_Requests_IsVehicleChanged]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Requests_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 14 procs.

| Procedure | Source line |
|---|---|
| `addRequestVehicleOwnershipProof` | sqlData.sql:19522 |
| `addRequest` | sqlData.sql:20954 |
| `deleteRequestVehicleOwnershipProof` | sqlData.sql:19608 |
| `deleteRequest` | sqlData.sql:19581 |
| `getRequestById` | sqlData.sql:20849 |
| `getRequestVehicleOwnershipProofByIdRequest` | sqlData.sql:19621 |
| `getRequestVehicleOwnershipProofById` | sqlData.sql:19640 |
| `getRequestVehicleOwnershipProofs` | sqlData.sql:19659 |
| `getRequests` | sqlData.sql:20909 |
| `printBel` | sqlData.sql:16345 |
| `printPlav` | sqlData.sql:16242 |
| `printZelen` | sqlData.sql:16456 |
| `updateRequestVehicleOwnershipProof` | sqlData.sql:19553 |
| `updateRequest` | sqlData.sql:21018 |

#### 4.4.3 Screens
#### 4.4.4 Business rules
#### 4.4.5 Print templates
#### 4.4.6 Open questions
#### 4.4.7 Migration notes

### 4.5 Documents (DOC)

#### 4.5.1 Purpose
#### 4.5.2 Tables

##### `DocumentsTrafficLicences.Extensions`

Source: `WinApp/sqlData.sql:8977-8990`

```sql
CREATE TABLE [dbo].[DocumentsTrafficLicences.Extensions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrafficLicence] [int] NOT NULL,
	[IdOperator] [int] NOT NULL,
	[ValidTill] [datetime] NOT NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTrafficLicences.Extensions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTrafficLicences.Extensions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTrafficLicences`

Source: `WinApp/sqlData.sql:9037-9052`

```sql
CREATE TABLE [dbo].[DocumentsTrafficLicences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdTehnicalExamOrganizationsIssuedBy] [int] NOT NULL,
	[TrafficLicenceNumber] [nvarchar](50) NULL,
	[MadeDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTrafficLicences_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTrafficLicences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReportsDetailsStatus`

Source: `WinApp/sqlData.sql:9058-9068`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReportsDetailsStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetailsStatus_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTehnicalExamsReportsDetailsStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentVehicleOwnershipProof`

Source: `WinApp/sqlData.sql:9221-9231`

```sql
CREATE TABLE [dbo].[DocumentVehicleOwnershipProof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VehicleOwnershipProofName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentVehicleOwnershipProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentVehicleOwnershipProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentPaymentProof`

Source: `WinApp/sqlData.sql:9426-9436`

```sql
CREATE TABLE [dbo].[DocumentPaymentProof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentProofName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentPaymentProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentPaymentProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsPermisions`

Source: `WinApp/sqlData.sql:9447-9467`

```sql
CREATE TABLE [dbo].[DocumentsPermisions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationOwner] [bigint] NOT NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdIssuer] [int] NOT NULL CONSTRAINT [DF_DocumentsPermisions_IdIssuer]  DEFAULT ((1)),
	[IdCityOfIssuing] [int] NOT NULL CONSTRAINT [DF_DocumentsPermisions_IdCityOfIssuing]  DEFAULT ((1)),
	[PermissionNumber] [nvarchar](50) NULL,
	[TrafficLicenceNumber] [nvarchar](50) NOT NULL CONSTRAINT [DF_DocumentsPermisions_TrafficLicenceNumber]  DEFAULT (N'непознат'),
	[TriptiqueNumber] [nvarchar](50) NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_DocumentsPermisions_DateCreated]  DEFAULT (getdate()),
	[ValidTillDate] [datetime] NOT NULL CONSTRAINT [DF_DocumentsPermisions_ValidTillDate]  DEFAULT (getdate()),
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsPermisions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsPermisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsInternationalDriveingLicences.ValidForCategories`

Source: `WinApp/sqlData.sql:9480-9492`

```sql
CREATE TABLE [dbo].[DocumentsInternationalDriveingLicences.ValidForCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdInternationalDrivingLicence] [bigint] NOT NULL,
	[IdLicenceCategorie] [int] NOT NULL,
	[IsCheck] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences.ValidForCategories_IsCheck]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences.ValidForCategories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsInternationalDriveingLicences.ValidForCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsInternationalDriveingLicences`

Source: `WinApp/sqlData.sql:9517-9534`

```sql
CREATE TABLE [dbo].[DocumentsInternationalDriveingLicences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdIssuer] [int] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_IdCountryIssuedFor]  DEFAULT ((1)),
	[NumberOfLicence] [nvarchar](50) NOT NULL,
	[NumberOfNationalLicence] [nvarchar](50) NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_NumberOfNationalLicence]  DEFAULT (N'непознат'),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_DateCreated]  DEFAULT (getdate()),
	[ValidTillDate] [datetime] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_ValidTillDate]  DEFAULT (getdate()),
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsInternationalDriveingLicences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReports`

Source: `WinApp/sqlData.sql:11552-11573`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReports](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdTypeOfTehnicalExam] [int] NOT NULL,
	[RegNumber] [nvarchar](25) NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReports_RegNumber]  DEFAULT (N'/'),
	[MadeDate] [datetime] NOT NULL,
	[ValidTillDate] [datetime] NOT NULL,
	[IdOrganizationForTehnicalExam] [int] NOT NULL,
	[IdFirsControler] [int] NOT NULL,
	[IdSecondControler] [int] NOT NULL,
	[VehicleIsRight] [bit] NOT NULL CONSTRAINT [DF_Vehicle.TehnicalExams_VehicleIsRight]  DEFAULT ((0)),
	[ExplanationNote] [nvarchar](250) NULL,
	[DriversWarning] [nvarchar](250) NULL,
	[Note] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.TehnicalExams_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Vehicle.TehnicalExams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReportsDetails`

Source: `WinApp/sqlData.sql:11796-11814`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReportsDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdTehnicalExamsReports] [bigint] NOT NULL,
	[IdTehnicalExamVehivlePart] [int] NOT NULL,
	[IdStatus] [int] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_IdVehiclePartStatus]  DEFAULT ((3)),
	[Front] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Front]  DEFAULT ((0)),
	[Back] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Back]  DEFAULT ((0)),
	[OnLeft] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_OnLeft]  DEFAULT ((0)),
	[OnRight] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_OnRight]  DEFAULT ((0)),
	[DateEnter] [datetime] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Date]  DEFAULT (getdate()),
	[Note] [nvarchar](150) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTehnicalExamsReportsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Documents`

Source: `WinApp/sqlData.sql:12594-12622`

```sql
CREATE TABLE [dbo].[Documents](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdDocumentType] [int] NOT NULL,
	[IdDocumentTypeOption] [int] NULL,
	[IdDocumentTypeOptionDetail] [int] NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationHistory] [bigint] NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdOperatorModified] [int] NULL,
	[IdOperatorEnded] [int] NOT NULL CONSTRAINT [DF_Documents_IdOperatorEnded]  DEFAULT ((0)),
	[IdTechnicalExamReport] [bigint] NULL,
	[IdPreviousRegistration] [int] NOT NULL CONSTRAINT [DF_Documents_IdPreviousRegistration]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[DateEnded] [datetime] NULL,
	[IdVehicleOwnershipProof] [int] NULL,
	[IdPaymentProof] [int] NULL,
	[VehicleOwnershipProof] [nvarchar](250) NULL,
	[PaymentProof] [nvarchar](250) NULL,
	[IdPayAttachment] [bigint] NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Documents_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 43 procs.

| Procedure | Source line |
|---|---|
| `NumOfInternationLicenceExists` | sqlData.sql:25832 |
| `NumOfTrafficLicenceExists` | sqlData.sql:23299 |
| `PrintDocumentPermisionForCustomerByIdDocumentPermision` | sqlData.sql:16616 |
| `PrintDocumentPermisionForCustomer` | sqlData.sql:16593 |
| `PrintTrafficLicenceByIdVehicle` | sqlData.sql:14422 |
| `PrintVehicleByIdWhiteRequest` | sqlData.sql:14367 |
| `addCustomerFinancialStatFromDocument` | sqlData.sql:24244 |
| `addDocumentVehicleOwnershipProo` | sqlData.sql:23892 |
| `addDocumentsPermision` | sqlData.sql:25078 |
| `addDocumentsTrafficLicence` | sqlData.sql:23182 |
| `addDocumentsTrafficLicencesExtension` | sqlData.sql:22871 |
| `deleteDocumentVehicleOwnershipProo` | sqlData.sql:23877 |
| `deleteDocumentsPermision` | sqlData.sql:25133 |
| `deleteDocumentsTrafficLicence` | sqlData.sql:23167 |
| `deleteDocumentsTrafficLicencesExtension` | sqlData.sql:22905 |
| `depActiveDocuments` | sqlData.sql:14632 |
| `existsPremision` | sqlData.sql:25195 |
| `getCustomerVehiclesRelationsMinusTrafficLicence` | sqlData.sql:17394 |
| `getDocumentPermisionList` | sqlData.sql:14657 |
| `getDocumentVehicleOwnershipProoById` | sqlData.sql:23820 |
| `getDocumentVehicleOwnershipProof` | sqlData.sql:23837 |
| `getDocumentsList` | sqlData.sql:14608 |
| `getDocumentsNotEnded` | sqlData.sql:16843 |
| `getDocumentsPermisionById` | sqlData.sql:25025 |
| `getDocumentsPermisions` | sqlData.sql:25052 |
| `getDocumentsTrafficLicenceById` | sqlData.sql:23222 |
| `getDocumentsTrafficLicencesExtensionByIdTrafficLicence` | sqlData.sql:22957 |
| `getDocumentsTrafficLicencesExtensionById` | sqlData.sql:22939 |
| `getDocumentsTrafficLicencesExtensions` | sqlData.sql:22920 |
| `getDocumentsTrafficLicencesList` | sqlData.sql:14701 |
| `getDocumentsTrafficLicencesNumberByIdVehicle` | sqlData.sql:18887 |
| `getDocumentsTrafficLicencesValid` | sqlData.sql:16804 |
| `getDocumentsTrafficLicences` | sqlData.sql:23244 |
| `getValidDocumentsTrafficLicencesList` | sqlData.sql:16824 |
| `printDocumentPermisionById` | sqlData.sql:14466 |
| `printTrafficLicenceById` | sqlData.sql:13358 |
| `printVehcile` | sqlData.sql:13523 |
| `printVehiclePivotReport` | sqlData.sql:13487 |
| `updateDocumentVehicleOwnershipProo` | sqlData.sql:23853 |
| `updateDocumentsPermision` | sqlData.sql:25148 |
| `updateDocumentsTrafficLicenceSetEndDtae` | sqlData.sql:22101 |
| `updateDocumentsTrafficLicence` | sqlData.sql:23265 |
| `updateDocumentsTrafficLicencesExtension` | sqlData.sql:22977 |

#### 4.5.3 Screens
#### 4.5.4 Business rules
#### 4.5.5 Print templates
#### 4.5.6 Open questions
#### 4.5.7 Migration notes

### 4.6 Payments (PAY)

#### 4.6.1 Purpose
#### 4.6.2 Tables

##### `CalculationItems`

Source: `WinApp/sqlData.sql:8329-8342`

```sql
CREATE TABLE [dbo].[CalculationItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](150) NOT NULL,
	[BankAccount] [nvarchar](50) NOT NULL,
	[Bank] [nvarchar](150) NOT NULL,
	[Form] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CalculationItems_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CalculationItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DogovorZaRati`

Source: `WinApp/sqlData.sql:9103-9118`

```sql
CREATE TABLE [dbo].[DogovorZaRati](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Broj] [nvarchar](50) NOT NULL,
	[Datum] [datetime] NOT NULL,
	[GarantNaziv] [nvarchar](50) NOT NULL,
	[GarantAdresa] [nvarchar](250) NOT NULL,
	[GartEMB] [nvarchar](20) NOT NULL,
	[BrNaRati] [int] NOT NULL CONSTRAINT [DF_DogovorZaRati_BrNaRati]  DEFAULT ((1)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_DogovorZaRati_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_DogovorZaRati] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocuments`

Source: `WinApp/sqlData.sql:10732-10752`

```sql
CREATE TABLE [dbo].[PaymentDocuments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentType] [int] NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdOperator] [int] NOT NULL,
	[DocumentNumber] [nvarchar](25) NOT NULL CONSTRAINT [DF_PaymentDocuments_DocumentNumber]  DEFAULT (N'/'),
	[DatePay] [datetime] NOT NULL,
	[DateRequired] [datetime] NOT NULL CONSTRAINT [DF_PaymentDocuments_DateRequired_1]  DEFAULT (getdate()),
	[Discount] [real] NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Payed]  DEFAULT ((0)),
	[Note] [nvarchar](150) NULL,
	[Storno] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Storno]  DEFAULT ((0)),
	[IdDogovor] [bigint] NOT NULL CONSTRAINT [DF_PaymentDocuments_IdDogovor]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentItems`

Source: `WinApp/sqlData.sql:11220-11232`

```sql
CREATE TABLE [dbo].[PaymentItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPymentCategory] [int] NOT NULL,
	[IdVehicleCategoryForPayments] [int] NOT NULL,
	[ItemName] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentItems_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `SecurityHouses`

Source: `WinApp/sqlData.sql:11867-11878`

```sql
CREATE TABLE [dbo].[SecurityHouses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCity] [int] NOT NULL,
	[SecurityHouseName] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_SecurityHouses_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_SecurityHouses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentItemParametars`

Source: `WinApp/sqlData.sql:11906-11922`

```sql
CREATE TABLE [dbo].[PaymentItemParametars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPaymentItem] [int] NOT NULL,
	[PrametarName] [nvarchar](250) NOT NULL,
	[VehicleField] [nvarchar](150) NOT NULL,
	[ParametarFrom] [real] NOT NULL,
	[ParametarTo] [real] NOT NULL,
	[Price] [money] NOT NULL,
	[IsOptional] [bit] NOT NULL CONSTRAINT [DF_PaymentItemParametars_IsOptional]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentItemParametars_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentItemParametars] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocumentsDetails`

Source: `WinApp/sqlData.sql:12482-12499`

```sql
CREATE TABLE [dbo].[PaymentDocumentsDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentDocuments] [bigint] NOT NULL,
	[IdPriceCatalog] [int] NOT NULL,
	[Price] [money] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Price]  DEFAULT ((0)),
	[DDV] [real] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_DDV]  DEFAULT ((0)),
	[Note] [nvarchar](150) NULL,
	[PrePayed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Payed]  DEFAULT ((0)),
	[NotePrePayed] [nvarchar](150) NULL,
	[Discount] [real] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Discount]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocumentsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocumentsRata`

Source: `WinApp/sqlData.sql:12505-12519`

```sql
CREATE TABLE [dbo].[PaymentDocumentsRata](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentDocument] [bigint] NOT NULL,
	[Price] [money] NOT NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsRata_Payed]  DEFAULT ((0)),
	[DatePayed] [datetime] NULL,
	[Note] [nchar](10) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsRata_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocumentsRata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 56 procs.

| Procedure | Source line |
|---|---|
| `GetPaymentDocumentForFiscalPrintByIdDocument` | sqlData.sql:13860 |
| `GetPaymentDocumentPrefix` | sqlData.sql:14019 |
| `GetPaymentDocumentRataForFiscalPrintByIdDocument` | sqlData.sql:18442 |
| `GetPaymentDocumentRataForFiscalPrintByIdRata` | sqlData.sql:18394 |
| `addCalculationItem` | sqlData.sql:20800 |
| `addDocumentPaymentProo` | sqlData.sql:24943 |
| `addDogovorZaRat` | sqlData.sql:23606 |
| `addPaymentDocumentsDetailFinace` | sqlData.sql:24329 |
| `addPaymentDocumentsDetail` | sqlData.sql:26127 |
| `addPaymentDocumentsRat` | sqlData.sql:25955 |
| `addPaymentItemParametar` | sqlData.sql:23007 |
| `addPaymentItem` | sqlData.sql:25345 |
| `addRequestPaymentProo` | sqlData.sql:19853 |
| `deleteCalculationItem` | sqlData.sql:20834 |
| `deleteDocumentPaymentProo` | sqlData.sql:24928 |
| `deleteDogovorZaRat` | sqlData.sql:23591 |
| `deletePaymentDocumentsDetail` | sqlData.sql:26173 |
| `deletePaymentDocumentsRat` | sqlData.sql:25940 |
| `deletePaymentItemParametar` | sqlData.sql:23050 |
| `deletePaymentItem` | sqlData.sql:25458 |
| `deleteRequestPaymentProo` | sqlData.sql:19921 |
| `getCalculationItemById` | sqlData.sql:20750 |
| `getCalculationItems` | sqlData.sql:20731 |
| `getDocumentPaymentProoById` | sqlData.sql:24992 |
| `getDocumentPaymentProof` | sqlData.sql:25009 |
| `getDogovorZaRatById` | sqlData.sql:23535 |
| `getDogovorZaRati` | sqlData.sql:23514 |
| `getPaymentCatalog` | sqlData.sql:13436 |
| `getPaymentDocumentsDetailByIdPaymentDocuments` | sqlData.sql:26235 |
| `getPaymentDocumentsDetailById` | sqlData.sql:26211 |
| `getPaymentDocumentsDetails` | sqlData.sql:26188 |
| `getPaymentDocumentsRatByIdPaymentDocument` | sqlData.sql:25878 |
| `getPaymentDocumentsRatById` | sqlData.sql:25919 |
| `getPaymentDocumentsRata` | sqlData.sql:25899 |
| `getPaymentItemByIdPymentCategory` | sqlData.sql:25439 |
| `getPaymentItemById` | sqlData.sql:25422 |
| `getPaymentItemParametarByIdPaymentItem` | sqlData.sql:23086 |
| `getPaymentItemParametarById` | sqlData.sql:23065 |
| `getPaymentItemParametars` | sqlData.sql:23109 |
| `getPaymentItems` | sqlData.sql:25404 |
| `getRequestPaymentProoByIdRequest` | sqlData.sql:19934 |
| `getRequestPaymentProoById` | sqlData.sql:19884 |
| `getRequestPaymentProof` | sqlData.sql:19903 |
| `printPaymentDocumentByDate` | sqlData.sql:13679 |
| `printPaymentDocument` | sqlData.sql:13731 |
| `printPaymentDocumetnByIdDocumetn` | sqlData.sql:13585 |
| `printShortPivotPaymentDocumentByDate` | sqlData.sql:13889 |
| `updateCalculationItem` | sqlData.sql:20770 |
| `updateDocumentPaymentProo` | sqlData.sql:24968 |
| `updateDogovorZaRat` | sqlData.sql:23557 |
| `updatePaymentDocumentsDetailFinace` | sqlData.sql:23962 |
| `updatePaymentDocumentsDetail` | sqlData.sql:26089 |
| `updatePaymentDocumentsRat` | sqlData.sql:25846 |
| `updatePaymentItemParametar` | sqlData.sql:23131 |
| `updatePaymentItem` | sqlData.sql:25376 |
| `updateRequestPaymentProo` | sqlData.sql:19825 |

#### 4.6.3 Screens
#### 4.6.4 Business rules
#### 4.6.5 Print templates
#### 4.6.6 Open questions
#### 4.6.7 Migration notes

### 4.7 Reports & dashboard (RPT)

#### 4.7.1 Purpose
#### 4.7.2 Tables
##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 6 procs.

| Procedure | Source line |
|---|---|
| `RequestPivotReport` | sqlData.sql:16552 |
| `TechnicalExamPivotReport` | sqlData.sql:16642 |
| `printCustomerPivotReport` | sqlData.sql:18540 |
| `printCustomerVehiclePivotReportCurrentOwners` | sqlData.sql:13781 |
| `printCustomerVehiclePivotReportPrevOwners` | sqlData.sql:13640 |
| `printCustomerVehiclePivotReport` | sqlData.sql:13822 |

#### 4.7.3 Screens
#### 4.7.4 Business rules
#### 4.7.5 Print templates
#### 4.7.6 Open questions
#### 4.7.7 Migration notes

### 4.8 Operators & security (SEC)

#### 4.8.1 Purpose
#### 4.8.2 Tables

##### `DataBases`

Source: `WinApp/sqlData.sql:285-297`

```sql
CREATE TABLE [dbo].[DataBases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EndUserName] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NULL,
	[ConnetionString] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_BAZI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_BAZI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CSLAObjects`

Source: `WinApp/sqlData.sql:306-318`

```sql
CREATE TABLE [dbo].[CSLAObjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCSLAObject] [int] NULL,
	[CSLAObjectName] [nvarchar](100) NOT NULL,
	[CSLAObjectType] [nvarchar](100) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CSLAObjects_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_CSLAObjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Employes`

Source: `WinApp/sqlData.sql:461-479`

```sql
CREATE TABLE [dbo].[Employes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdWorkPosition] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Prezime] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[EMBG] [char](13) NOT NULL,
	[BLK] [nvarchar](10) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[DateOfHireing] [datetime] NOT NULL CONSTRAINT [DF_Employes_DateOfHireing]  DEFAULT (getdate()),
	[RFID] [nvarchar](50) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Employes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Employes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Roles`

Source: `WinApp/sqlData.sql:493-503`

```sql
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ULOGI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ULOGI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Users`

Source: `WinApp/sqlData.sql:516-538`

```sql
CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdDataBase] [int] NOT NULL,
	[UserFullName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPass] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[SureName] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[EMBG] [char](13) NULL,
	[BLK] [nvarchar](10) NULL,
	[DateOfBirth] [datetime] NULL,
	[DateOfHireing] [datetime] NULL CONSTRAINT [DF_Users_DateOfHireing]  DEFAULT (getdate()),
	[RFID] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_KORISNICI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_KORISNICI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `FieldsPrivileges`

Source: `WinApp/sqlData.sql:550-562`

```sql
CREATE TABLE [dbo].[FieldsPrivileges](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdCSLAObject] [int] NOT NULL,
	[CSLAObjectPropertyName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_FieldsPrivileges_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_FieldsPrivileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `ObjectPrivileges`

Source: `WinApp/sqlData.sql:572-587`

```sql
CREATE TABLE [dbo].[ObjectPrivileges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdCSLAObject] [int] NOT NULL,
	[CanAddObject] [bit] NOT NULL,
	[CanGetObject] [bit] NOT NULL,
	[CanDeleteObject] [bit] NOT NULL,
	[CanEditObject] [bit] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ObjectPrivileges_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ObjectPrivileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `SecurityPolicies`

Source: `WinApp/sqlData.sql:12650-12661`

```sql
CREATE TABLE [dbo].[SecurityPolicies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdScurityHouse] [int] NOT NULL,
	[NumberOfPolicy] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_SecurityPolicies_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_SecurityPolicies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### Views (SEC database)

All 3 views found in sqlData.sql relate to the SEC module (privilege evaluation).

##### View `View_1`

Source: `WinApp/sqlData.sql:824-832`

```sql
CREATE VIEW [dbo].[View_1]
AS
SELECT     dbo.Users.ID AS IdUser, dbo.Users.IdRole, dbo.Users.IdDataBase, dbo.Users.UserFullName, dbo.Users.UserName, dbo.Users.UserPass, 
                      dbo.Roles.RoleName, dbo.DataBases.EndUserName, dbo.DataBases.DatabaseName, dbo.DataBases.ConnetionString
FROM         dbo.Users INNER JOIN
                      dbo.Roles ON dbo.Users.IdRole = dbo.Roles.Id INNER JOIN
                      dbo.DataBases ON dbo.Users.IdDataBase = dbo.DataBases.Id
WHERE     (dbo.Users.Active = 1)
GO
```

##### View `FieldPrivilegesList`

Source: `WinApp/sqlData.sql:1216-1224`

```sql
CREATE VIEW [dbo].[FieldPrivilegesList]
AS
SELECT     dbo.Roles.RoleName, dbo.CSLAObjects.CSLAObjectName, dbo.CSLAObjects.CSLAObjectType, dbo.FieldsPrivileges.CSLAObjectPropertyName, 
                      dbo.FieldsPrivileges.Id, dbo.Roles.Id AS IdRole, dbo.CSLAObjects.Id AS IdObject
FROM         dbo.Roles INNER JOIN
                      dbo.FieldsPrivileges ON dbo.Roles.Id = dbo.FieldsPrivileges.IdRole INNER JOIN
                      dbo.CSLAObjects ON dbo.FieldsPrivileges.IdCSLAObject = dbo.CSLAObjects.Id
WHERE     (dbo.FieldsPrivileges.Active = 1)
GO
```

##### View `ObjectPrivilegesList`

Source: `WinApp/sqlData.sql:1455-1464`

```sql
CREATE VIEW [dbo].[ObjectPrivilegesList]
AS
SELECT     dbo.ObjectPrivileges.Id, dbo.ObjectPrivileges.IdRole, dbo.Roles.RoleName, dbo.ObjectPrivileges.IdCSLAObject, dbo.CSLAObjects.CSLAObjectName, 
                      dbo.ObjectPrivileges.CanAddObject, dbo.ObjectPrivileges.CanGetObject, dbo.ObjectPrivileges.CanDeleteObject, 
                      dbo.ObjectPrivileges.CanEditObject
FROM         dbo.ObjectPrivileges INNER JOIN
                      dbo.CSLAObjects ON dbo.ObjectPrivileges.IdCSLAObject = dbo.CSLAObjects.Id INNER JOIN
                      dbo.Roles ON dbo.ObjectPrivileges.IdRole = dbo.Roles.Id
WHERE     (dbo.ObjectPrivileges.Active = 1)
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 40 procs.

| Procedure | Source line |
|---|---|
| `GetChildFieldsPrivileges` | sqlData.sql:1195 |
| `GetChildObjectPrivileges` | sqlData.sql:1166 |
| `GetPrivileges` | sqlData.sql:1147 |
| `Login` | sqlData.sql:1013 |
| `addCSLAObject` | sqlData.sql:1426 |
| `addDataBase` | sqlData.sql:1042 |
| `addEmploye` | sqlData.sql:2079 |
| `addFieldsPrivilege` | sqlData.sql:1722 |
| `addObjectPrivilege` | sqlData.sql:1753 |
| `addRole` | sqlData.sql:2265 |
| `addUser` | sqlData.sql:2003 |
| `deleteCSLAObject` | sqlData.sql:1411 |
| `deleteDataBase` | sqlData.sql:1073 |
| `deleteEmploye` | sqlData.sql:2064 |
| `deleteFieldsPrivilege` | sqlData.sql:1707 |
| `deleteObjectPrivilege` | sqlData.sql:1868 |
| `deleteRole` | sqlData.sql:2217 |
| `deleteUser` | sqlData.sql:1883 |
| `getCSLAObjectById` | sqlData.sql:1392 |
| `getCSLAObjects` | sqlData.sql:1374 |
| `getConnectionString` | sqlData.sql:1128 |
| `getDataBaseById` | sqlData.sql:1106 |
| `getDataBases` | sqlData.sql:1088 |
| `getEmployeById` | sqlData.sql:2152 |
| `getEmployes` | sqlData.sql:2128 |
| `getFieldsPrivilegeById` | sqlData.sql:1688 |
| `getFieldsPrivileges` | sqlData.sql:1670 |
| `getObjectPrivilegeById` | sqlData.sql:1812 |
| `getObjectPrivileges` | sqlData.sql:1791 |
| `getRoleById` | sqlData.sql:2248 |
| `getRoles` | sqlData.sql:2232 |
| `getUserById` | sqlData.sql:1926 |
| `getUsers` | sqlData.sql:1898 |
| `updateCSLAObject` | sqlData.sql:1614 |
| `updateDataBase` | sqlData.sql:982 |
| `updateEmploye` | sqlData.sql:2177 |
| `updateFieldsPrivilege` | sqlData.sql:1642 |
| `updateObjectPrivilege` | sqlData.sql:1834 |
| `updateRole` | sqlData.sql:2290 |
| `updateUser` | sqlData.sql:1955 |

#### 4.8.3 Screens
#### 4.8.4 Business rules
#### 4.8.5 Print templates
#### 4.8.6 Open questions
#### 4.8.7 Migration notes

### 4.9 Attachments & scanning (ATT)

#### 4.9.1 Purpose
#### 4.9.2 Tables

##### `DocumentAttachments`

Source: `WinApp/sqlData.sql:11884-11900`

```sql
CREATE TABLE [dbo].[DocumentAttachments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdDocument] [bigint] NULL,
	[IdAttachmentType] [int] NOT NULL,
	[AttachmentStatus] [nvarchar](50) NOT NULL,
	[AttachmentNotes] [ntext] NULL,
	[AttachmentPath] [nvarchar](550) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentAttachments_Active]  DEFAULT ((1)),
	[IdCustomer] [bigint] NULL,
	[IdVehicle] [bigint] NULL,
 CONSTRAINT [PK_DocumentAttachments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 13 procs.

| Procedure | Source line |
|---|---|
| `addAttachmentType` | sqlData.sql:21363 |
| `addDocumentAttachment` | sqlData.sql:18291 |
| `deleteAttachmentType` | sqlData.sql:21421 |
| `deleteDocumentAttachment` | sqlData.sql:8605 |
| `getAttachmentTypeById` | sqlData.sql:21388 |
| `getAttachmentTypes` | sqlData.sql:21405 |
| `getDocumentAttachmentByIdCustomer` | sqlData.sql:18332 |
| `getDocumentAttachmentByIdDocument` | sqlData.sql:9498 |
| `getDocumentAttachmentByIdVehicle` | sqlData.sql:18353 |
| `getDocumentAttachmentById` | sqlData.sql:18232 |
| `getDocumentAttachments` | sqlData.sql:18210 |
| `updateAttachmentType` | sqlData.sql:21339 |
| `updateDocumentAttachment` | sqlData.sql:18255 |

#### 4.9.3 Screens
#### 4.9.4 Business rules
#### 4.9.5 Print templates
#### 4.9.6 Open questions
#### 4.9.7 Migration notes

### 4.10 Infrastructure (INF)

#### 4.10.1 Purpose
#### 4.10.2 Tables
##### Stored procedures (inventory only — bodies deferred to Phase C deep-read)

Total: 14 procs.

| Procedure | Source line |
|---|---|
| `CISTI_BAZA` | sqlData.sql:16668 |
| `SqlQueryNotificationStoredProcedure-0bcda15c-9ca4-4534-b39d-65e778735041` | sqlData.sql:6270 |
| `SqlQueryNotificationStoredProcedure-1f25935b-aa2e-4812-9ec1-d3856e80ccf8` | sqlData.sql:9304 |
| `SqlQueryNotificationStoredProcedure-2bb92228-0840-4c1d-9529-4595568ecd2e` | sqlData.sql:6156 |
| `SqlQueryNotificationStoredProcedure-2c4598dd-1632-433c-ab27-39fac93eb6bf` | sqlData.sql:9404 |
| `SqlQueryNotificationStoredProcedure-370dcb25-bd83-4d4e-afc8-10062a352d13` | sqlData.sql:5320 |
| `SqlQueryNotificationStoredProcedure-375d0294-07fd-48fa-89e5-18e6b9220807` | sqlData.sql:9328 |
| `SqlQueryNotificationStoredProcedure-614ab11f-bbd3-4587-892b-5fb5cafa325c` | sqlData.sql:5142 |
| `SqlQueryNotificationStoredProcedure-82274b07-9757-4861-aec4-be560eae2cca` | sqlData.sql:9214 |
| `SqlQueryNotificationStoredProcedure-99e5de0d-5d5f-47e0-9162-dc971b750168` | sqlData.sql:6149 |
| `SqlQueryNotificationStoredProcedure-a3afa0ab-08ec-4047-b605-a96644d4aa4c` | sqlData.sql:6142 |
| `SqlQueryNotificationStoredProcedure-a6108f14-fb5f-4f8d-a826-4e47e94e5be4` | sqlData.sql:6072 |
| `SqlQueryNotificationStoredProcedure-bc3d2d69-378e-4ae7-a2d9-f55d9100f568` | sqlData.sql:9245 |
| `SqlQueryNotificationStoredProcedure-e7b2dcea-4f96-4602-8b58-64e1264f817d` | sqlData.sql:9473 |

#### 4.10.3 Screens
#### 4.10.4 Business rules
#### 4.10.5 Print templates
#### 4.10.6 Open questions
#### 4.10.7 Migration notes

---

## 5. Cross-cutting concerns

### 5.1 i18n
### 5.2 Audit log / change tracking
### 5.3 Soft-delete / "active" flags
### 5.4 Numbering schemes (invoice, request, document)
### 5.5 Date/time/timezone handling
### 5.6 Money / decimal precision and rounding rules
### 5.7 Status-transition pattern

---

## 6. Migration master table

| Legacy table | Row count | Target table | Decision | Transform | Notes |
|---|---|---|---|---|---|
| `AttachmentTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `BusinessTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Cities` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Colors` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `ColorsDetails` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Communities` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Countries` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CustomerVehiclesRelationTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DDVCatalog` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentTypePrint` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentTypesOptions` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentTypesOptionsDetails` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DriveingLicenceCtegories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `EngineTypeModelRelations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentCategories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PriceCatalog` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `RegistrationIssuers` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `RequestTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Streets` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `TehnicalExamOrganizations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `TehnicalExamsTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `TehnicalExamVehicleParts` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `TehnicalExamVehiclePartsCategories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleBodytype` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleBrakes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleCategories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleCategoriesRelations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleCategoryForPayments` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleDisabledFields` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleEngineEcoProgram` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleEnginePowerSourceTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleEngineTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleGearBox` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleJUSCategories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleMakers` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleModel` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehiclePayTollCategory` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleRequiredFields` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleSupporting` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleTireTypes` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehicleUse` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CustomerFinancialState` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Customers` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Customers.ContactPersons` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CustomersBankAccounts` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CustomerVehiclesRelations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Vehicle.Axis` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Vehicle.BetweenAxesDestinations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Vehicle.Registrations` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Vehicle.Tyres` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `VehiclePayToll` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Vehicles` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Request.PaymentProof` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Request.VehicleOwnershipProofs` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Requests` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentPaymentProof` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `Documents` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsInternationalDriveingLicences` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsInternationalDriveingLicences.ValidForCategories` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsPermisions` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsTehnicalExamsReports` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsTehnicalExamsReportsDetails` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsTehnicalExamsReportsDetailsStatus` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsTrafficLicences` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentsTrafficLicences.Extensions` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DocumentVehicleOwnershipProof` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CalculationItems` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `DogovorZaRati` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentDocuments` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentDocumentsDetails` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentDocumentsRata` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentItemParametars` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `PaymentItems` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `SecurityHouses` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |
| `CSLAObjects` | unknown — needs prod sample | — | drop | — | CSLA privilege engine dropped; ASP.NET Identity used instead |
| `DataBases` | unknown — needs prod sample | — | drop | — | Replaced by `Stations` (one row per legacy database) |
| `Employes` | unknown — needs prod sample | `Operators` | keep-rename | rename + tenant | + StationId on rewrite; merges with Users (one record per login) |
| `FieldsPrivileges` | unknown — needs prod sample | — | drop | — | Field-level privilege engine dropped |
| `ObjectPrivileges` | unknown — needs prod sample | — | drop | — | Object-level privilege engine dropped |
| `Roles` | unknown — needs prod sample | — | drop | — | Replaced by 2 hard-coded ASP.NET Identity roles (Administrator, Operator) |
| `SecurityPolicies` | unknown — needs prod sample | — | drop | — | CSLA privilege engine dropped |
| `Users` | unknown — needs prod sample | `AspNetUsers` + `Operators` | merge | AspNetUsers (Identity standard) + custom `Operators` with StationId | Password hashes cannot port; Q-004 covers migration |
| `DocumentAttachments` | unknown — needs prod sample | TBD | TBD | + StationId on rewrite | _(blank for now)_ |

Decision values: `keep-rename` | `split` | `merge` | `drop` | `new`.

---

## 7. Risks register

| ID | Description | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R-1 | Encrypted connection strings, no key (see spec §7) | high | blocks migration sub-project | Document encryption scheme; obtain redacted `app.config` + key material |
| R-2 | Business rules hidden in CSLA partial classes / form code-behind | medium-high | incorrect behavior in rewrite | Deep-read both library and form locations; flag anomalies in §8 |
| R-3 | Per-station schema drift | high | migration sub-project handles it, not audit | Capture baseline; defer per-station diff to migration |
| R-4 | Macedonian-only domain knowledge | medium | glossary errors propagate | Flag every term "needs confirmation" until stakeholder signs off |
| R-5 | Print templates as legal artifacts | medium | report-rebuild sub-project at risk | Capture Designer-level layouts; require redacted printed samples |
| R-6 | **Legacy `.sql` files contain destructive DDL headers** (`DROP DATABASE [vtesecurity]`, `DROP DATABASE [KenoBingo]` at lines 3-4 of `sqlData.sql` and `emSecurity.sql`) | high if misused | catastrophic — would destroy a production database if piped into `sqlcmd` against a live server | **Treat these files as read-only documentation, NOT deployment scripts.** Audit work uses only text utilities (grep, awk, sed, diff, iconv) that cannot execute SQL. Migration sub-project must build its own idempotent script from scratch — not reuse these legacy exports. |

(More risks added during Pass 3.)

---

## 8. Open questions register

| ID | Module | Status | Question | Source | Owner | Assumption | Impact | Resolution |
|---|---|---|---|---|---|---|---|---|
| Q-001 | SEC | open | Why does `SecurityPolicies` (sqlData.sql:12650) appear in the combined export but not in `emSecurity.sql`? Drift, dead table, or recent addition? | sqlData.sql:12650 | stakeholder | keep as SEC (4.8) and migrate; treat as per-station drift indicator | if dead table → drop in new schema; if recent addition → confirm all stations have it | _(open)_ |
| Q-002 | INF | open | What is `KenoBingo` and why does emSecurity.sql attempt to drop it? Is the dev team also building/maintaining a different product, and was the SQL export script reused from there? | emSecurity.sql:3-4 | stakeholder | template/clone artifact; safe to ignore for this rewrite | none for the rewrite; only matters for understanding the dev-team's history | _(open)_ |
| Q-003 | INF | open | The 14 `SqlQueryNotificationStoredProcedure-<GUID>` procs are SQL Server Service Broker query-notification procs auto-generated by ADO.NET / SqlDependency. Were they actually used by the legacy app, or are they vestigial? | sqlData.sql (multiple lines) | stakeholder | vestigial; drop in new schema (the rewrite uses SignalR or polling, not SqlDependency) | if actually used → need a replacement push mechanism | _(open)_ |
| Q-004 | SEC | open | At cutover, how do existing operators get their first password into the new ASP.NET Identity system? Force-reset on first login (email a one-time link), or admin-issued temporary password, or pre-seed by guessing legacy hashes (impossible — legacy uses Rijndael with unknown key)? | §3.4 | stakeholder | force-reset on first login: each operator logs in once with a one-time token issued by the Administrator, then sets a real password. | affects cutover UX and the migration script — needs Administrator to be able to issue tokens before operators are migrated | _(open)_ |

Status values: `open` | `answered` | `decided`.

---

## Appendix A. Files NOT read

| Path | Reason |
|---|---|
| `KeyGen/` | hardware-licensing project; rewrite drops licensing |
| `TakehardwareInfo/` | hardware-licensing project; rewrite drops licensing |
| `SetupObicen/` | WinForms installer; not needed for web |
| `TestWpf/` | experimental project, not production |
| `Backup/`, `Backup1/` | code archives |
| `_ReSharper.VTE/`, `_UpgradeReport_Files/` | IDE artifacts |
| `packages/`, `tools/`, `.vs/` | build/IDE artifacts |
| `WinApp/bin/`, `WinApp/obj/` | build output |
| All `UpgradeLog*` files | upgrade logs from VS migrations |
| `WinApp/AutoUpdate.vb` | auto-updater; rewrite drops it |

(More entries appended during Pass 1 and Pass 2 if encountered.)

---

## Appendix B. Macedonian print-form facsimiles

(Links to images in `audit-inputs/` once supplied.)

---

## Appendix C. Interview script for station operators

(Populated during Pass 2 if any §8 question has `Owner: station operator | inspector`.)

---

## Progress log

Each task in `docs/superpowers/plans/2026-04-30-vte-domain-audit.md` appends one line here on completion. Format: `YYYY-MM-DD — <task-id> — <one-line summary>`.

- 2026-04-30 — A1 — created audit-inputs/, README, audit doc skeleton
- 2026-04-30 — B1 — inventoried sqlData.sql: 85 CREATE TABLE, 3 CREATE VIEW, 508 CREATE PROC, 0 triggers, 0 explicit indexes, 0 functions, 359 ALTER TABLE (likely FK constraints). 0 Module-TBD tables — every table classifies cleanly under the locked-in module assignment. Distribution: SEC=8, REF=43, CUS=4, VEH=7, REQ=3, DOC=11, PAY=8, ATT=1. Notable: sqlData.sql is a combined dump including all 8 SEC tables (which also appear in emSecurity.sql) — overlap to be verified in B4.
- 2026-04-30 — B2 (now complete) — populated §2.1 with 44 needs-confirmation glossary entries; populated §6 with 85 TBD rows; transcribed all 85 CREATE TABLE blocks verbatim from sqlData.utf8.sql into §4.x.2 sections (REF=43, CUS=4, VEH=7, REQ=3, DOC=11, PAY=8, SEC=8, ATT=1). Verification: 85 ##### headings, 85 CREATE TABLE [dbo] statements present.
- 2026-04-30 — B3 — filed all 3 CREATE VIEW blocks (View_1, FieldPrivilegesList, ObjectPrivilegesList — all SEC, joined to Users/Roles/DataBases) verbatim into §4.8.2; produced per-module stored-procedure inventory (508 procs total) classified by name-prefix heuristics. Classification: REF=214, VEH=77, PAY=56, DOC=43, SEC=40, CUS=31, INF-INFRA=14 (SqlQueryNotification* + db maintenance), REQ=14, ATT=13, RPT=6. Procs filed in §4.x.2 as inventory tables (procedure name + line number); proc bodies deferred to Phase C deep-read per the (a') scope. Notable: View_1 is the legacy login query (Users JOIN Roles JOIN DataBases) — implies users are bound to a specific legacy database row at login time, which clarifies the per-station DB-routing model.
- 2026-04-30 — B4 — verified emSecurity.sql is a near-duplicate of the SEC portion of sqlData.sql. emSecurity.sql contains 7 tables (DataBases, CSLAObjects, Employes, Roles, Users, FieldsPrivileges, ObjectPrivileges), 3 views (same 3), 40 procs (subset of sqlData.sql's 508). All filed under §4.8.2 already in B2/B3 — no duplicate transcription needed. Only 3 lines differ between emSecurity.sql and the first 2349 lines of sqlData.sql, all in the destructive header (DROP DATABASE/CREATE DATABASE statements). Notable: sqlData.sql includes `SecurityPolicies` (line 12650) which is **NOT** in emSecurity.sql — flagged for §8 as a per-station drift indicator (Q-001). Also notable: emSecurity.sql header attempts to DROP a database literally named `KenoBingo` then CREATE database `VTE` — almost certainly a template/clone artifact from a different product. Do NOT run.
- 2026-04-30 — B5 — no-op: zero Module-TBD tables to reclassify (every legacy table classified cleanly during B1 against the locked-in module assignment). Added Q-001 (SecurityPolicies-only-in-sqlData), Q-002 (KenoBingo template artifact), Q-003 (SqlQueryNotification procs — vestigial?) to §8.
- 2026-04-30 — B6 — Pass 1 complete. Final audit-doc state: 2,936 lines; 85 CREATE TABLE blocks filed verbatim; 3 CREATE VIEW blocks filed verbatim; 508 stored procedures filed as per-module inventory (proc bodies deferred to Phase C deep-read per the (a') scope); 45 needs-confirmation glossary entries; 85 §6 master-table rows (decisions = TBD); 6 risks (R-1..R-6); 3 open questions (Q-001..Q-003). Status header at top of doc still says "Pass 1 not started" — to be updated when stakeholder approves the checkpoint summary.
- 2026-04-30 — STAKEHOLDER DECISION (during B6 review) — auth simplification confirmed. New system uses ASP.NET Core Identity (standard .NET 9), drops the legacy CSLA privilege model entirely (FieldsPrivileges, ObjectPrivileges, CSLAObjects, SecurityPolicies, the View_1 join, all 40 SEC stored procedures). Two hard-coded roles only: Administrator (system-wide, cross-tenant) and Operator (bound to one StationId). Updated §3 Tenancy & auth model with the full design (subsections 3.1–3.6). Updated §6 master-table SEC rows: DataBases→drop; CSLAObjects→drop; Roles→drop; FieldsPrivileges→drop; ObjectPrivileges→drop; SecurityPolicies→drop; Employes→keep-rename to Operators with StationId; Users→merge into AspNetUsers + Operators. Added Q-004 (password migration at cutover). Sub-project (5) "Operators / roles / privileges" massively simplified — no CSLA, no field/object privileges, no per-property auth rules.
- 2026-04-30 — STAKEHOLDER ACTION (during B6 review) — created the new database `VTE2` on local SQL Server (`(localdb)\MSSQLLocalDB`, SQL Server 2025 Express, Windows auth as `AzureAD\Filip.Ilievski`). Bootstrap script: `docs/superpowers/work/bootstrap-vte2.sql` (idempotent — safe to re-run). Tables created: `Stations` (tenant table), `Operators` (VTE-specific user data with optional `StationId` FK — null for Administrators), and the 7 standard ASP.NET Core Identity 9 tables (`AspNetRoles`, `AspNetUsers`, `AspNetUserClaims`, `AspNetUserLogins`, `AspNetUserRoles`, `AspNetUserTokens`, `AspNetRoleClaims`). Seeded the 2 hard-coded roles (Administrator, Operator). Warnings during creation about clustered-index key length ≥1800 bytes are expected for the standard Identity schema — values are GUIDs in practice, never trigger. Total: 9 tables, 2 roles, 0 users (Administrator account to be created by the future .NET API or by a separate seed script).
- 2026-04-30 — STAKEHOLDER PIVOT (during B6 review) — pushback that auth bootstrap was plumbing, the actual business is what matters. Pivoted into Phase C work for the Customers module. Read `VTE.Library/Customers/Customer.vb` (1465 lines), `CustomersContactPerson.vb`, `CustomerBankAccount.vb`. Extracted **22 business rules** (BR-CUS-001..014 on Customer, BR-CUS-020..025 on ContactPerson, BR-CUS-030..032 on BankAccount) — each with source citation; see `docs/superpowers/work/customers-business-rules.md`. Surfaced 4 new open questions: Q-005 (EMBG uniqueness enforcement), Q-006 (EMBG checksum disabled?), Q-007 (two TaxNumber columns), Q-008 (rules in customer-related stored proc bodies — 31 procs not yet read). Designed and applied new schema: `Customers`, `CustomerContactPersons`, `CustomerBankAccounts` to VTE2. New schema fixes 3 legacy typos (IdBirhCity→IdBirthCity, BrithAddressNumber→BirthAddressNumber, Driveing→Driving), renames Mb→EMBG and BLK→IDCardNumber for clarity, adds StationId tenant column, adds RowVersion concurrency, adds audit columns (CreatedUtc, LastModifiedUtc, CreatedByUserId, LastModifiedByUserId FKs to AspNetUsers). Authorization rules dropped per §3.2. Reference-data FKs (cities, streets, countries, business types, registration issuers) deferred until REF module migration. Bootstrap script: `docs/superpowers/work/bootstrap-customers.sql`. VTE2 now has 12 tables (was 9).
- 2026-04-30 — STAKEHOLDER PIVOT cont. — applied Phase-C deep-reads for Vehicles and Requests modules. Vehicles: read `Vehicle.vb` (3,691 lines), extracted **7 active validation rules** (`BR-VEH-001..007`) plus 14 commented-out/deactivated rules logged as Q-009. New schema: `Vehicles` (118 columns, includes 2 CHECK constraints enforcing axle/wheel propulsion consistency from BR-VEH-006/007), `VehicleRegistrations`, `VehicleAxles`, `VehicleAxleDistances`, `VehicleTyres`, `CustomerVehicleRelations`. ~30 transliterated-Macedonian columns translated to English (MasaPoOska→AxleLoad, MaxKonstVkMasa→MaxConstructiveTotalMass, etc.). 8+ typo fixes. Requests: read `Request.vb` (984 lines) + `RequestType.vb` (701 lines) — finding: **RequestType is a configurable workflow engine** with 10 boolean / int flags (IsTechnicalExamRequired, IsPayRequired, IsNewRegistration, IsRelationDeleted, IsVehicleDeleted, IsNewCustomer, IsVehicleChanged, IsCustomerChanged, IsSufficient, IsPreviousRegistrationRequired) driving Request validation and side-effects. **8 active rules** (BR-REQ-001..006 on Request, BR-REQ-020..021 on RequestType). New schema: `RequestTypes`, `Requests`, `RequestVehicleOwnershipProofs`, `RequestPaymentProofs`. Surfaced Q-009..Q-018. VTE2 now at **22 tables** (was 9 after auth bootstrap, 12 after Customers, 18 after Vehicles).
- 2026-04-30 — STAKEHOLDER PIVOT cont. — Payments module deep-read complete. Read `PaymentDocument.vb` (1,065 lines) + `PaymentDocumentsDetail.vb` + `PaymentDocumentsRata.vb` + `PaymentRatiDogovor.vb` + `CalculationItem.vb`. Extracted **19 numbered business rules** (BR-PAY-001..043 with gaps for organization). Two critical cross-field rules: **BR-PAY-007 (installment integrity: sum of installments ≤ sum of bill line items, error "Збирот на ратите не смее да биде поголем од вкупната сметка")** and **BR-PAY-012 (line discount must be < 100%)**. New schema: 9 tables — `PaymentTypes` (with IsInvoice/IsCash/IsFiscalCard/IsAccount/IsInstallments flags translated from legacy), `CalculationItems` (per-tenant fee catalog with target bank account per item), `DDVCatalog` (VAT rates with effective dates), `PriceCatalog` (per-tenant pricing tied to fee categories + VAT rates), `InstallmentContracts` (legacy DogovorZaRati with guarantor + 2..60 installment count CHECK), `PaymentDocuments` (with Storno/Payed/discount-< 100 CHECK), `PaymentDocumentDetails` (with VAT-rate-snapshot per line), `PaymentDocumentInstallments` (with InstallmentNumber unique per document), `CustomerFinancialState` (running AR-style balance per customer). Surfaced Q-019..Q-027 (numbering schemes, VAT-vs-discount interaction, late-fee logic, Polisa meaning, B2B billing override). VTE2 now at **31 tables** (was 22). Final stats: 14 CHECK constraints, 51 FK constraints, 232 indexes.

- 2026-04-30 — SESSION SUMMARY — In one extended session: completed Pass-1 schema audit + 4 business modules deep-read (Customers, Vehicles, Requests, Payments). Total 56 numbered business rules extracted with file:line citations. Total 22 new tables in VTE2 (from 9 auth bootstrap to 31 with core business). 18 open questions surfaced for stakeholder review (Q-001..Q-027 with gaps). Files: `docs/superpowers/work/customers-business-rules.md`, `vehicles-business-rules.md`, `requests-business-rules.md`, `payments-business-rules.md` and matching `bootstrap-*.sql`. Remaining modules NOT yet deep-read: Documents (Traffic Licences, Permissions, International Driving Licences, Tech Exam Reports — the most regulatory-sensitive module), Reference data (~43 lookup tables — mostly CRUD), Reports & dashboard, Attachments & scanning, Operators (already simplified per stakeholder decision in §3.2). Migration script (legacy → VTE2) is a future sub-project.
- 2026-04-30 — STAKEHOLDER PIVOT cont. — Documents module deep-read complete (the regulatory heart of the business). Read `Document.vb` (1,017 lines, generic workflow), `DocumentsTrafficLicence.vb` (670 lines), `DocumentsTehnicalExamsReport.vb` (1,738 lines), `DocumentsTehnicalExamsReportsDetail.vb` (414 lines). Extracted **15 numbered business rules** across 4 document types (BR-DOC-001..006 generic, BR-DOC-100..104 traffic licence, BR-DOC-400..404 tech exam, BR-DOC-450..453 detail). Surfaced **the inspection-result schema**: TechnicalExamReports has 56 columns including 25 brake-force measurements (5 measurements × 5 axles incl. parking), 4 brake-effect tests (working empty/full, secondary, parking), emissions (CO, lambda, RPM, pinpoints), noise, oil temp, plus a `VehicleIsRight` pass/fail BIT. Cross-field rules: BR-DOC-403 (`MadeDate ≤ ValidTillDate` enforced via CHECK), BR-DOC-404 (first/second inspector must differ — enforced via CHECK). New schema: 8 tables — `TrafficLicences`, `TrafficLicenceExtensions`, `Permissions`, `InternationalDrivingLicences`, `InternationalDrivingLicenceCategories`, `TechnicalExamReports`, `TechnicalExamReportDetails`, `TechnicalExamReportVisualErrors`. Surfaced Q-028..Q-033 (regulatory-units/Macedonian-abbreviation questions). VTE2 now at 39 tables.

- 2026-04-30 — REF data shipped — 29 reference-data tables added in one consolidated bootstrap (`bootstrap-reference-data.sql`). Includes geography (Countries, Communities, Cities, Streets), customer (BusinessTypes, RegistrationIssuers, CustomerVehicleRelationTypes), vehicle (BodyTypes, Categories, Uses, Makers, Models, EngineTypes, EnginePowerSourceTypes, EngineEcoPrograms, GearBoxes, Brakes, Supportings, Colors, CategoriesForPayments, TireTypes), tech-exam (Organizations, Types, VehiclePartCategories, VehicleParts, ReportDetailStatuses), driving licence (Categories), and document-proof-type lookups (VehicleOwnershipProofTypes, PaymentProofTypes). All global (no StationId — these don't vary per-tenant). Each follows the same pattern: Id IDENTITY + Name + optional Code + IsActive + audit cols + RowVersion. Deferred FK constraints from Customer/Vehicle/Document tables can now be added in a future ALTER-TABLE pass. VTE2 now at **68 tables**, 20 CHECK constraints, 71 FK constraints, 245 indexes.

- 2026-04-30 — SESSION SUMMARY (extended) — In one extended session, completed: Pass-1 schema audit + 6 module deep-reads (Customers, Vehicles, Requests, Payments, Documents) + REF data layer. **71 numbered business rules extracted** with file:line citations. **68 tables** in VTE2 from clean install. **27 open questions** logged for stakeholder review. Files: 6 business-rules markdown files + 6 idempotent bootstrap-*.sql files in `docs/superpowers/work/`. Remaining work: wire deferred FKs from Customer/Vehicle/Document tables to REF tables (mechanical ALTER TABLE pass), build legacy-to-VTE2 migration script (its own sub-project), build the .NET 9 backend + Vue 3 frontend. Reports & dashboard, attachments/scanning are also deferred — not blocking schema design or initial code build.
- 2026-04-30 — DB COMPLETE — wired all deferred FKs (40 FKs across Customers/Vehicles/Vehicle-related/Requests/Payments/Documents) + added 5 attachment tables (Customer/Vehicle/Request/TechnicalExamReport/PaymentDocument). Final VTE2 state: **73 tables, 121 FKs, 20 CHECKs, 250 indexes**. Bootstrap script: `docs/superpowers/work/bootstrap-fks-and-attachments.sql`. Schema-level work for the rewrite is now complete.

- 2026-04-30 — BACKEND SKELETON SHIPPED — created .NET 9 backend at `backend/`. Solution layout: VTE.Domain (POCOs, ASP.NET Identity Stores), VTE.Infrastructure (VteDbContext extending IdentityDbContext + tenant query filter), VTE.Api (controllers, JWT auth, tenancy, seed). Initial entities: Station, ApplicationUser, Operator, Customer, Vehicle, CustomerVehicleRelation. Identity wired to existing AspNet* tables. JWT bearer auth with role + stationId claims. Tenant-aware EF Core query filter on Customer + Vehicle (filters by StationId from JWT for Operators; bypasses for Administrators with null StationId). Swagger UI at /swagger with Bearer auth definition. CORS for Vue dev server (5173/5174). Seed-on-startup creates 2 roles (re-check), a default Station, and a default Administrator (`admin`/`ChangeMe!Now1` from appsettings.json — change in production). Verified working end-to-end: API starts, Swagger reachable, login returns JWT + role + null stationId for admin. Required one ALTER TABLE on Stations to add CreatedByUserId/LastModifiedByUserId/RowVersion to align with the entity model. Build succeeds clean (0 warnings, 0 errors). Backend README at `backend/README.md`.
- 2026-04-30 — BACKEND MODULES SHIPPED — added domain entities + DbContext mappings + controllers for all remaining modules. New entities: 31 reference-data classes (Country, City, Street, BusinessType, vehicle classifications, tech-exam refs, etc.) + RequestType + Request + InstallmentContract + PaymentDocument (+ Detail + Installment) + TrafficLicence + TechnicalExamReport. DbContext now exposes 41 DbSets. Tenant query filters on Customer, Vehicle, Request, PaymentDocument, TrafficLicence, TechnicalExamReport, CalculationItem, PriceCatalog (admins bypass). New controllers: OperatorsController (admin creates/lists/deactivates operators), VehiclesController (with BR-VEH-002/006/007 pre-validation), CustomerVehicleRelationsController, RequestTypesController + RequestsController (with BR-REQ-005/006 enforcement and an /end endpoint), PaymentDocumentsController (enforces BR-PAY-006/007/012 — installment integrity + discount caps — plus storno + pay-installment actions), InstallmentContractsController (BR-PAY-034 range), TechnicalExamReportsController (BR-DOC-403/404), TrafficLicencesController, ReferenceDataController (27 read-only lookup endpoints). End-to-end smoke test: API starts, Swagger lists **47 endpoints**, login returns JWT, GET /api/stations returns seeded Default Station, all REF endpoints respond 200. Build clean (0 warnings, 0 errors).
- 2026-04-30 — FRONTEND SKELETON SHIPPED — Vue 3 SPA scaffolded at `frontend/`. Stack: Vue 3 + `<script setup>` + TypeScript, Vite, Vue Router (role-based guards), Pinia (auth store persisted to localStorage), Axios (JWT interceptor + 401 → /login), vue-i18n (mk default + en fallback per audit §3.1), PrimeVue 4 with Aura theme + PrimeIcons. Files: api/client.ts, stores/auth.ts, locales/{mk,en,index}.ts, router/index.ts, components/AppLayout.vue (sidebar + topbar + locale switcher), views/LoginView.vue + DashboardView.vue + CustomersView.vue (list + create dialog enforcing BR-CUS-013) + StationsView.vue (Administrator-only via route meta + sidebar v-if). Vite proxy forwards `/api/*` → http://localhost:5258 (backend). Type check (vue-tsc) clean, vite build clean. End-to-end smoke test: backend + frontend dev server start together, vite proxy forwards login POST to backend, JWT returned, persisted auth restored on reload. README at `frontend/README.md`. Default login: admin / ChangeMe!Now1 (matches backend seed).
- 2026-04-30 — FRONTEND SKELETON SHIPPED — Vue 3 SPA scaffolded at `frontend/`. Stack: Vue 3 + `<script setup>` + TypeScript, Vite, Vue Router (role-based guards), Pinia (auth store persisted to localStorage), Axios (JWT interceptor + 401 → /login), vue-i18n (mk default + en fallback per audit §3.1), PrimeVue 4 with Aura theme + PrimeIcons. Files: api/client.ts, stores/auth.ts, locales/{mk,en,index}.ts, router/index.ts, components/AppLayout.vue (sidebar + topbar + locale switcher), views/LoginView.vue + DashboardView.vue + CustomersView.vue (list + create dialog enforcing BR-CUS-013 client-side) + StationsView.vue (Administrator-only via route meta + sidebar v-if). Vite proxy forwards /api/* → http://localhost:5258 (backend). Type check (vue-tsc) clean, vite build clean (12 chunks, ~870 KB total). End-to-end smoke test: backend + frontend dev server start together, vite proxy forwards login POST to backend, JWT returned. README at `frontend/README.md`.
- 2026-04-30 — SEED DATA + MIGRATION TEMPLATE SHIPPED — `migrate/` folder: (a) `seed-vte2-defaults.sql` populates the global REF data needed for any Macedonian inspection station to function — 16 countries (Macedonia + neighbours), 31 Macedonian cities with postal codes, 18 EU vehicle categories (L1..O4 + T,C), 15 body types, 7 engine types, 11 colors, 19 driving licence categories (AM..H), 3 VAT rates (0%/5%/18%), 5 payment types (with proper IsCash/IsInvoice/IsFiscalCard/IsAccount/IsInstallments flags), 6 technical exam types, 5 pass/fail statuses, 10 RequestTypes seeded with the actual workflow-flag combinations (Прва регистрација, Продолжување, Промена на сопственик, Дерегистрација, Бришење на возило, etc. — each with their IsTechnicalExamRequired/IsPayRequired/IsNewRegistration/IsRelationDeleted/IsVehicleDeleted/IsNewCustomer flags). Applied to VTE2: confirmed counts. (b) `migrate-legacy-station.sql` — parameterized template (LEGACY_DB / STATION_NAME / STATION_CODE via `:setvar`) covering the 11-step migration path: pre-flight checks, station creation, REF dedupe-merge (BusinessTypes/Cities/Streets/Countries/RegistrationIssuers/VehicleBodyTypes pattern shown — others to fill in per real legacy schema), Customers (with EMBG/IDCard/typo-fixed column renames), Customer children, Vehicles (~30 Macedonian→English column renames), CustomerVehicleRelations, Requests (legacy RequestType mapped by name to seeded ones), TrafficLicences, PaymentDocuments + their detail/installment children, Users → AspNetUsers + Operators (passwords NOT migrated — placeholder PasswordHash forces reset on first login). Ends with row-count verification per major table. (c) `README.md` runbook with step-by-step instructions, post-migration tasks, and explicit caveats (R-3 schema drift, abbreviated REF list, deferred Permissions/IDL/TechExamReport sections). Treated explicitly as a template — needs validation against each real legacy DB before production use.
- 2026-04-30 — REMAINING TASKS COMPLETED — Backend additions: 5 attachment domain entities (CustomerAttachment / VehicleAttachment / RequestAttachment / TechnicalExamReportAttachment / PaymentDocumentAttachment) wired into DbContext; AttachmentsController with one route family (POST/GET/DELETE) covering all 5 kinds via {kind} param + multipart upload + 20 MB cap; DashboardController returning Customers / Vehicles / OpenRequests / UnpaidPaymentDocuments / TrafficLicencesExpiringIn30Days / TechExamReportsLast30Days / FailedExamsLast30Days. PDF generation via QuestPDF: PdfReports class with three generators — ExamReportCertificate (header, brake-force tables per axle, emissions, pass/fail badge), Invoice (Faktura with line items + VAT + totals + installments table), CashReport (daily KasovIzvestaj with totals). ReportsController exposes them at GET /api/reports/{technical-exam-reports/{id}/pdf,payment-documents/{id}/invoice,cash-report?day=}. Frontend additions: rebuilt DashboardView with 7 colored tiles (calls /api/dashboard); VehiclesView (DataTable + create dialog with BR-VEH-002/006/007 fields); RequestsView (DataTable + open/all toggle + end-request action); PaymentsView (DataTable + storno + invoice-PDF download + cash-report PDF download); TechnicalExamReportsView (DataTable + pass/fail tag + certificate-PDF download). Router and AppLayout updated with new routes + nav links. mk + en locales extended with all the new keys. End-to-end smoke test: backend serves **53 endpoints** (was 47), `GET /api/dashboard` returns real stats, `GET /api/ref/countries` returns the 16 seeded countries, `GET /api/request-types` returns the 10 seeded RequestTypes, `GET /api/reports/cash-report` produces a valid 30 KB PDF. Frontend type check clean, vite build clean. **All 14 sub-projects from the original plan are now at least partially shipped, with the 8 highest-priority ones fully working end-to-end.**
