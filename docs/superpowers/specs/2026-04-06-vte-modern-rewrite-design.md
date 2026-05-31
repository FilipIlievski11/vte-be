# VTE Modern Rewrite - Design Specification

**Date:** 2026-04-06
**Type:** Full rewrite of Vehicle Technical Examination & Registration Management System
**Target:** WPF on .NET 8 with MVVM, Entity Framework Core, Fluent UI

---

## 1. Overview

Rewrite the legacy VB.NET WinForms VTE application as a modern WPF desktop app. The system manages vehicle registration, technical inspections, document issuance, payments, and customer management for transport regulatory bodies. Full feature parity with the original, modern architecture, fresh database schema with migration from the legacy DB.

---

## 2. Architecture

### 2.1 Solution Structure

```
VTE.sln
├── src/
│   ├── VTE.Core/                    # Entities, enums, interfaces, value objects
│   ├── VTE.Application/             # Use cases, DTOs, validation, services
│   ├── VTE.Infrastructure/          # EF Core, DB context, repositories, printing, reports
│   ├── VTE.WPF/                     # Shell, navigation, shared controls, themes
│   ├── VTE.WPF.Customers/          # Customer module UI
│   ├── VTE.WPF.Vehicles/           # Vehicle module UI
│   ├── VTE.WPF.TechnicalExams/     # Technical exam module UI
│   ├── VTE.WPF.Documents/          # Documents & licenses module UI
│   ├── VTE.WPF.Payments/           # Payment module UI
│   ├── VTE.WPF.Requests/           # Request workflow module UI
│   ├── VTE.WPF.Reports/            # Pivot reports, printing, analytics
│   ├── VTE.WPF.Admin/              # Users, roles, privileges, settings
│   └── VTE.WPF.Lookups/            # All lookup/reference data management screens
├── tools/
│   └── VTE.Migration/              # Legacy DB → new DB migration console app
└── tests/
    ├── VTE.Core.Tests/
    ├── VTE.Application.Tests/
    └── VTE.Infrastructure.Tests/
```

### 2.2 Dependency Flow

```
WPF Modules → VTE.Application → VTE.Core
                    ↑
            VTE.Infrastructure
```

- **VTE.Core**: Zero dependencies. Entities, interfaces, enums.
- **VTE.Application**: References Core. Contains service interfaces, DTOs, validators, business rules.
- **VTE.Infrastructure**: References Core and Application. Implements repositories, EF Core DbContext, report generation, printing.
- **VTE.WPF.\***: References Application (never Infrastructure directly). Uses dependency injection.

### 2.3 Key Technology Choices

| Concern | Technology |
|---------|-----------|
| Framework | .NET 8, C#, WPF |
| UI Toolkit | WPF-UI (Fluent/Windows 11 design) |
| MVVM | CommunityToolkit.Mvvm |
| Dependency Injection | Microsoft.Extensions.DependencyInjection |
| ORM | Entity Framework Core 8 (code-first) |
| Database | SQL Server |
| Validation | FluentValidation |
| Reporting | QuestPDF (replacing Crystal Reports) |
| Localization | .resx resource files (mk-MK, en-US) |
| Logging | Serilog |
| Authentication | Custom (username/password against DB) |

---

## 3. Data Model (Fresh Schema)

### 3.1 Core Entities

#### Customers

```
Customer
├── Id (long, PK)
├── IdentificationNumber (string) -- was "Mb"
├── FirstName (string)
├── LastName (string)
├── ParentName (string?)
├── DateOfBirth (DateTime?)
├── IsCompany (bool)
├── CompanyName (string?)
├── TaxNumber (string?)
├── Occupation (string?)
├── WorksInCompany (string?)
├── PhoneNumber (string?)
├── Fax (string?)
├── Email (string?)
├── PassportNumber (string?)
├── PassportDateIssued (DateTime?)
├── PassportIssuer (string?)
├── DrivingLicenseNumber (string?)
├── IdentityCardNumber (string?) -- was "BLK"
├── CanSendNotifications (bool)
├── Status (int)
├── Note (string?)
├── CitizenshipId (long?, FK → Country)
├── BirthCityId (long?, FK → City)
├── BirthAddressStreetId (long?, FK → Street)
├── LivingCityId (long?, FK → City)
├── LivingAddressStreetId (long?, FK → Street)
├── BusinessTypeId (long?, FK → BusinessType)
├── CreatedAt (DateTime)
├── ModifiedAt (DateTime?)
├── CreatedByUserId (long, FK → User)
├── ModifiedByUserId (long?, FK → User)
├── ContactPersons → List<CustomerContactPerson>
└── BankAccounts → List<CustomerBankAccount>

CustomerContactPerson
├── Id (long, PK)
├── CustomerId (long, FK)
├── FullName (string)
├── PhoneNumber (string?)
├── Email (string?)
└── Note (string?)

CustomerBankAccount
├── Id (long, PK)
├── CustomerId (long, FK)
├── BankName (string)
├── AccountNumber (string)
└── Note (string?)
```

#### Vehicles

```
Vehicle
├── Id (long, PK)
├── ShellNumber (string) -- chassis number
├── EngineNumber (string?)
├── MakeDate (DateTime?)
├── FirstRegistrationNumber (string?)
├── FirstRegistrationDate (DateTime?)
├── FirstRegistrationValidUntil (DateTime?)
├── FirstRegistrationIssuerId (long?, FK → RegistrationIssuer)
├── LastRegistrationNumber (string?)
├── LastRegistrationDate (DateTime?)
├── LastRegistrationValidUntil (DateTime?)
├── LastRegistrationIssuerId (long?, FK → RegistrationIssuer)
├── -- Technical specs --
├── VehicleModelId (long?, FK → VehicleModel)
├── BodyTypeId (long?, FK → VehicleBodyType)
├── CategoryId (long?, FK → VehicleCategory)
├── PaymentCategoryId (long?, FK → VehiclePaymentCategory)
├── UseTypeId (long?, FK → VehicleUseType)
├── EngineTypeId (long?, FK → EngineType)
├── PrimaryPowerSourceId (long?, FK → EnginePowerSourceType)
├── SecondaryPowerSourceId (long?, FK → EnginePowerSourceType)
├── GearBoxTypeId (long?, FK → GearBoxType)
├── BrakeTypeId (long?, FK → BrakeType)
├── SupportingTypeId (long?, FK → SupportingType)
├── EcoProgramId (long?, FK → EcoProgram)
├── MadeInCountryId (long?, FK → Country)
├── PrimaryColorId (long?, FK → Color)
├── SecondaryColorId (long?, FK → Color)
├── ColorCode (string?)
├── -- Engine specs --
├── EnginePowerKW (decimal?)
├── EngineTorqueNM (decimal?)
├── EngineWorkingCapacityCM3 (decimal?)
├── -- Dimensions --
├── HeightMM (int?)
├── WidthMM (int?)
├── LengthMM (int?)
├── -- Weight --
├── EmptyWeightKG (decimal?)
├── MaxAllowedWeightKG (decimal?)
├── TrailerWeightBrakedKG (decimal?)
├── TrailerWeightUnbrakedKG (decimal?)
├── -- Seating --
├── NumberOfDoors (int?)
├── NumberOfSeats (int?)
├── NumberOfStandingSeats (int?)
├── NumberOfLyingSeats (int?)
├── -- Axles & Wheels --
├── NumberOfAxles (int?)
├── PropulsionAxle (int?)
├── NumberOfWheels (int?)
├── NumberOfPropulsionWheels (int?)
├── -- Emissions --
├── CO (decimal?)
├── HC (decimal?)
├── NOx (decimal?)
├── HCNOx (decimal?)
├── CO2 (decimal?)
├── NoiseStaticDB (decimal?)
├── NoiseMovementDB (decimal?)
├── Blackening (decimal?)
├── Pinpoints (decimal?)
├── -- Fuel --
├── FuelConsumption (decimal?)
├── FuelTankCapacityL (decimal?)
├── HasLPG (bool)
├── -- Other --
├── MaxSpeedKMH (decimal?)
├── HasHook (bool)
├── IsSocialVehicle (bool)
├── IsPrivateTransport (bool)
├── CreatedAt (DateTime)
├── ModifiedAt (DateTime?)
├── Axles → List<VehicleAxle>
├── Tyres → List<VehicleTyre>
└── AxleDistances → List<VehicleAxleDistance>
```

#### Customer-Vehicle Relations

```
CustomerVehicleRelation
├── Id (long, PK)
├── CustomerId (long, FK → Customer)
├── VehicleId (long, FK → Vehicle)
├── RelationTypeId (long, FK → RelationType)
├── StartDate (DateTime)
├── EndDate (DateTime?)
├── BeginNote (string?)
└── TerminationNote (string?)
```

#### Technical Exam Reports

```
TechnicalExamReport
├── Id (long, PK)
├── CustomerVehicleRelationId (long, FK)
├── ExamTypeId (long, FK → TechnicalExamType)
├── OrganizationId (long, FK → TechnicalExamOrganization)
├── RegistrationNumber (string)
├── ExamDate (DateTime)
├── ValidUntilDate (DateTime)
├── FirstControllerId (long, FK → User)
├── SecondControllerId (long?, FK → User)
├── VehiclePassed (bool)
├── -- Brake measurements per axle (1-4 + parking) --
├── Axle1BrakeLeftKN (decimal?), Axle1BrakeRightKN (decimal?), ... (all 5 axles)
├── WorkingBrakeEffectivenessEmpty (decimal?)
├── WorkingBrakeEffectivenessFull (decimal?)
├── SecondaryBrakeEffectiveness (decimal?)
├── ParkingBrakeEffectiveness (decimal?)
├── VehicleWeightKG (decimal?)
├── -- Emissions --
├── EngineSpeedRPM (int?)
├── CO (decimal?)
├── EngineTurns (int?)
├── COPlusTurns (decimal?)
├── Lambda (decimal?)
├── Pinpoints (decimal?)
├── NoiseDB (decimal?)
├── EngineOilTemperatureC (decimal?)
├── TechnicalChanges (string?)
├── ExplanationNote (string?)
├── DriverWarning (string?)
├── Note (string?)
├── CreatedAt, ModifiedAt, CreatedByUserId, ModifiedByUserId
├── Details → List<TechnicalExamReportDetail>
└── VisualErrors → List<TechnicalExamVisualError>

TechnicalExamReportDetail
├── Id (long, PK)
├── ReportId (long, FK)
├── VehiclePartId (long, FK → TechnicalExamVehiclePart)
├── StatusId (long, FK → ExamDetailStatus)
└── Note (string?)

TechnicalExamVisualError
├── Id (long, PK)
├── ReportId (long, FK)
├── Description (string)
└── Note (string?)
```

#### Requests

```
Request
├── Id (long, PK)
├── RequestTypeId (long, FK → RequestType)
├── CustomerVehicleRelationId (long, FK)
├── NewCustomerVehicleRelationId (long?, FK) -- for ownership transfers
├── TechnicalExamReportId (long?, FK)
├── PreviousRegistrationId (long?)
├── IsCustomerChanged (bool)
├── IsVehicleChanged (bool)
├── OrganizationId (long?, FK)
├── Note (string?)
├── DateCreated (DateTime)
├── DateModified (DateTime?)
├── DateEnded (DateTime?)
├── CreatedByUserId, ModifiedByUserId, EndedByUserId
├── OwnershipProofs → List<RequestOwnershipProof>
├── PaymentProofs → List<RequestPaymentProof>
└── Attachments → List<RequestAttachment>
```

#### Documents

```
Document
├── Id (long, PK)
├── DocumentTypeId (long, FK → DocumentType)
├── DocumentTypeOptionId (long?, FK)
├── DocumentTypeOptionDetailId (long?, FK)
├── CustomerVehicleRelationId (long, FK)
├── TechnicalExamReportId (long?, FK)
├── PreviousRegistrationId (long?)
├── OwnershipProofId (long?, FK → OwnershipProofType)
├── PaymentProofId (long?, FK → PaymentProofType)
├── Note (string?)
├── DateCreated (DateTime)
├── DateModified (DateTime?)
├── DateEnded (DateTime?)
├── CreatedByUserId, ModifiedByUserId, EndedByUserId
└── Attachments → List<DocumentAttachment>

TrafficLicense
├── Id (long, PK)
├── DocumentId (long, FK → Document)
├── LicenseNumber (string)
├── IssuedDate (DateTime)
├── ValidUntilDate (DateTime?)
├── PlateNumber (string?)
└── Extensions → List<TrafficLicenseExtension>

InternationalDrivingLicense
├── Id (long, PK)
├── CustomerId (long, FK)
├── LicenseNumber (string)
├── IssuedDate (DateTime)
├── ValidUntilDate (DateTime)
└── ValidCategories → List<InternationalDrivingLicenseCategory>

Permission
├── Id (long, PK)
├── DocumentId (long, FK → Document)
├── PermissionNumber (string)
├── IssuedDate (DateTime)
├── ValidUntilDate (DateTime?)
└── Note (string?)
```

#### Payments

```
PaymentDocument
├── Id (long, PK)
├── PaymentTypeId (long, FK → PaymentType)
├── CustomerVehicleRelationId (long, FK)
├── DocumentNumber (string)
├── PaymentDate (DateTime?)
├── DueDate (DateTime?)
├── DiscountPercent (decimal)
├── IsPaid (bool)
├── IsCancelled (bool) -- was "Storno"
├── InsurancePolicy (decimal?)
├── AgreementId (long?)
├── InvoicedToId (long?)
├── OrganizationId (long?, FK)
├── Note (string?)
├── CreatedAt, CreatedByUserId
├── LineItems → List<PaymentLineItem>
└── Installments → List<PaymentInstallment>

PaymentLineItem
├── Id (long, PK)
├── PaymentDocumentId (long, FK)
├── PaymentItemId (long, FK → PaymentCatalogItem)
├── Description (string)
├── Quantity (int)
├── UnitPrice (decimal)
├── VATPercent (decimal)
├── VATAmount (decimal, computed)
├── TotalAmount (decimal, computed)
└── SortOrder (int)

PaymentInstallment
├── Id (long, PK)
├── PaymentDocumentId (long, FK)
├── InstallmentNumber (int)
├── DueDate (DateTime)
├── Amount (decimal)
├── IsPaid (bool)
└── PaidDate (DateTime?)
```

#### Administration

```
User
├── Id (long, PK)
├── Username (string, unique)
├── PasswordHash (string) -- bcrypt, not plain text like legacy
├── FullName (string)
├── FirstName (string)
├── LastName (string)
├── Address (string?)
├── IdentificationNumber (string?) -- EMBG
├── IdentityCardNumber (string?) -- BLK
├── DateOfBirth (DateTime?)
├── DateOfHiring (DateTime?)
├── RFID (string?)
├── RoleId (long, FK → Role)
├── OrganizationId (long, FK → TechnicalExamOrganization)
├── IsActive (bool)
├── CreatedAt (DateTime)
└── ModifiedAt (DateTime?)

Role
├── Id (long, PK)
├── Name (string)
└── Privileges → List<RolePrivilege>

RolePrivilege
├── Id (long, PK)
├── RoleId (long, FK)
├── EntityName (string) -- e.g. "Customer", "Vehicle"
├── CanCreate (bool)
├── CanRead (bool)
├── CanUpdate (bool)
└── CanDelete (bool)

TechnicalExamOrganization -- stations/centers
├── Id (long, PK)
├── Name (string)
├── Address (string?)
├── CompanyId (long, FK → Company)
└── IsActive (bool)

Company
├── Id (long, PK)
├── Name (string)
├── TaxNumber (string?)
├── Address (string?)
└── PhoneNumber (string?)
```

### 3.2 Lookup Tables

All follow the pattern: `Id (long, PK), Name (string), IsActive (bool)`

**Geographic:** Country, City (FK → Community), Community (FK → Country), Street (FK → City)

**Vehicle Catalogs:** VehicleBodyType, VehicleCategory, VehiclePaymentCategory, VehicleUseType, EngineType, EnginePowerSourceType, GearBoxType, BrakeType, SupportingType, EcoProgram, VehicleMaker, VehicleModel (FK → VehicleMaker), TireType, Color, RegistrationIssuer

**Document Catalogs:** DocumentType (with flags: IsVehicleRequired, IsTechnicalExamRequired, IsPaymentRequired), DocumentTypeOption (FK → DocumentType), DocumentTypeOptionDetail (FK → DocumentTypeOption), OwnershipProofType, PaymentProofType, AttachmentType

**Payment Catalogs:** PaymentType, PaymentCategory, PaymentCatalogItem (FK → PaymentCategory, FK → VehiclePaymentCategory), VATRate (DDV catalog)

**Exam Catalogs:** TechnicalExamType, TechnicalExamVehiclePart (self-referential hierarchy: ParentId → TechnicalExamVehiclePart), ExamDetailStatus

**Relations:** RelationType (owner, lessee, etc.), DrivingLicenseCategory

---

## 4. UI Design

### 4.1 Shell & Navigation

The app shell uses WPF-UI's NavigationView (left sidebar, Fluent style):

```
┌──────────────────────────────────────────────────────┐
│  VTE                                    [User] [⚙]  │
├──────────┬───────────────────────────────────────────┤
│          │  Tab1 | Tab2 | Tab3 | ...                 │
│ Dashboard│───────────────────────────────────────────│
│          │                                           │
│ Customers│  [Active Tab Content]                     │
│ Vehicles │                                           │
│ Requests │                                           │
│ Exams    │                                           │
│ Documents│                                           │
│ Payments │                                           │
│ Reports  │                                           │
│          │                                           │
│ ──────── │                                           │
│ Lookups  │                                           │
│ Admin    │                                           │
│ Settings │                                           │
├──────────┴───────────────────────────────────────────┤
│  Status: Ready           Station: [name]    [mk|en] │
└──────────────────────────────────────────────────────┘
```

- Left sidebar: NavigationView with grouped items (collapsible)
- Main area: TabControl — each opened form is a tab (same pattern as legacy)
- Status bar: current user, station, language toggle
- Tabs can be closed with middle-click or X button

### 4.2 Form Patterns

**List View (Grid)**
- DataGrid with search/filter bar at top
- Toolbar: New, Refresh, Print, Export
- Double-click row → opens detail tab
- Column sorting, grouping, filtering built-in

**Detail/Edit View**
- Header section with key info
- Tabbed sections for grouped fields (e.g., Customer: Personal, Address, Documents)
- Toolbar: Save, Cancel, Delete, Print
- Real-time validation with error indicators next to fields
- Dirty state tracking (unsaved changes warning on close)
- Breadcrumb or title showing entity context

**Lookup/Catalog View**
- Simple grid with inline editing for small catalogs
- Name + IsActive columns
- Add/Edit/Delete in-grid or via small dialog

### 4.3 Module Screens

**Dashboard**
- Summary cards: total customers, vehicles, exams today, pending payments
- Recent activity list
- Quick-action buttons (New Request, New Exam, etc.)

**Customers Module**
- Customer list (searchable grid)
- Customer detail form (personal info, address, company info, bank accounts, contact persons)
- Customer-vehicle relations view

**Vehicles Module**
- Vehicle list (searchable grid with filters)
- Vehicle detail form (registration, technical specs, dimensions, emissions, axles, tyres)
- Vehicle model/maker management
- All vehicle lookup catalogs (body type, category, engine type, etc.)

**Requests Module**
- Request list (with status filters: open, completed, cancelled)
- Request form (type selection, customer-vehicle link, ownership proofs, payment proofs, attachments)
- Request workflow: create → attach docs → link exam → process → complete

**Technical Exams Module**
- Exam report list
- Exam report form (vehicle info, brake measurements per axle, emissions, visual inspection tree, pass/fail)
- Exam type and vehicle parts management

**Documents Module**
- Document list
- Document detail (type, options, attachments)
- Traffic license management (issue, extend, list)
- International driving license management
- Permission management

**Payments Module**
- Payment document list (with paid/unpaid/cancelled filters)
- Payment document form (line items from catalog, discounts, installments)
- Payment catalog management (items, categories, VAT rates)
- Unpaid payments report

**Reports Module**
- Pivot-style reports for: customers, vehicles, exams, payments, requests
- Date-range filtered reports
- Print preview with PDF export
- Cash register report, fiscal summary

**Admin Module**
- User management (CRUD, role assignment)
- Role management (CRUD, privilege matrix)
- Organization/station management
- Company management
- Application settings (skin, language, font size)

**Lookups Module**
- Geographic: countries, communities, cities, streets
- Document types, ownership proof types, payment proof types
- Relation types, driving license categories
- Attachment types

### 4.4 Localization

- Two languages: Macedonian (mk-MK, default) and English (en-US)
- Language toggle in status bar
- All UI strings in .resx resource files
- Culture-aware date/number formatting

### 4.5 Printing & Reports

- QuestPDF for generating reports as PDFs
- Print preview built into the app
- Report types:
  - Technical exam certificate
  - Invoice (compact and detailed formats)
  - Traffic license document
  - Registration plate labels (blue, white, green categories)
  - Exam registry report
  - Payment receipts
  - Cash register summaries

---

## 5. Authentication & Authorization

### 5.1 Authentication
- Login screen on startup (username + password)
- Passwords stored as bcrypt hashes (not plain text like legacy)
- Session maintained in-memory (no token needed for desktop)
- No hardware licensing in the new version (simplification)

### 5.2 Authorization (RBAC)
- Roles define permissions per entity (CRUD)
- Navigation items hidden/shown based on role
- Form buttons enabled/disabled based on role
- Enforced at service layer (not just UI)

---

## 6. Migration Strategy

### 6.1 VTE.Migration Console App

A standalone console tool that:
1. Connects to legacy SQL Server DB (old schema)
2. Reads all data via legacy stored procedures or direct table queries
3. Transforms and maps to new EF Core entities
4. Writes to new database

### 6.2 Migration Order (respecting FK dependencies)

1. Lookup tables (countries, cities, communities, streets, all catalogs)
2. Companies, organizations
3. Users, roles, privileges
4. Customers (with contact persons, bank accounts)
5. Vehicles (with axles, tyres, axle distances)
6. Customer-vehicle relations
7. Technical exam reports (with details, visual errors)
8. Requests (with attachments, proofs)
9. Documents (with attachments)
10. Traffic licenses, international driving licenses, permissions
11. Payment documents (with line items, installments)

### 6.3 Migration Considerations
- Legacy passwords: migrate as-is into a `LegacyPasswordHash` column, force password reset on first login
- Data cleanup: skip orphaned records, log warnings
- ID mapping: maintain a mapping table (old ID → new ID) for all entities
- Validation: run counts before/after, verify referential integrity

---

## 7. Non-Functional Requirements

- **Startup time**: < 3 seconds to login screen
- **Form load**: < 1 second for any list or detail form
- **Offline capability**: Not required (SQL Server assumed available)
- **Concurrent users**: Multi-user via shared DB (no record locking beyond DB transactions)
- **Data volume**: Support 100K+ customers, 100K+ vehicles, 500K+ documents
- **Audit trail**: CreatedAt/ModifiedAt/CreatedBy/ModifiedBy on all major entities

---

## 8. What's Excluded from V1

- Fiscal printer integration (Accent PF500) — can be added later as a plugin
- FTP auto-update mechanism — modern deployment (ClickOnce or MSIX) instead
- Hardware-based licensing — removed, standard login only
- TWAIN scanner integration — defer to V2
- WCF service endpoint — not needed for local data portal
