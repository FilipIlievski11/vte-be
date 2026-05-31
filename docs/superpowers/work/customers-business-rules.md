# Customer module — business rules extracted from legacy

**Sources read:**
- `VTE.Library/Customers/Customer.vb` (1465 lines — main editable CSLA business object)
- `VTE.Library/Customers/CustomersContactPerson.vb` (child collection)
- `VTE.Library/Customers/CustomerBankAccount.vb` (child collection)
- Legacy table DDL (already in audit doc §4.2.2): `Customers`, `Customers.ContactPersons`, `CustomersBankAccounts`, `CustomerFinancialState`

**Status:** rules extracted from CSLA `AddBusinessRules` and validation helpers. Stored-procedure bodies (31 customer-related procs) NOT read in this pass — rules they contain are still latent.

## Validation rules — Customer (root entity)

| Rule | Statement | Source |
|---|---|---|
| **BR-CUS-001** | `EMBG` (legacy `Mb`) max length 13 chars | `Customer.vb:391` |
| **BR-CUS-002** | `EMBG` should be unique per non-company customer (uniqueness check via `NumOfCustomerMBExists` proc; commented-out in code but still surfaces an error message `"Матичниот број мора да биде единствен"` — Q-005: is this enforced?) | `Customer.vb:457-471, 1441-1444` |
| **BR-CUS-003** | When `IsCompany = false` and `EMBG` is non-empty, `EMBG` must pass `MaticenBroj.CheckMaticenBroj()` checksum (commented-out in code; possibly disabled — Q-006) | `Customer.vb:472-490` |
| **BR-CUS-004** | `Surname` (legacy `CustomerSurname`) max 100 | `Customer.vb:393` |
| **BR-CUS-005** | `FirstName` (legacy `CustomerFirstName`) **required** + max 100 | `Customer.vb:395-396` |
| **BR-CUS-006** | `PhoneNumber` max 20 | `Customer.vb:398` |
| **BR-CUS-007** | `Fax` max 20 | `Customer.vb:400` |
| **BR-CUS-008** | `LivingAddressNumber` max 100 | `Customer.vb:402` |
| **BR-CUS-009** | `BirthAddressNumber` (legacy typo `BrithAddressNumber`) max 100 | `Customer.vb:404` |
| **BR-CUS-010** | `Occupation` max 50 | `Customer.vb:406` |
| **BR-CUS-011** | `DateOfBirth` **required** (commented as `StringRequired` but applied to a date — works in legacy because SmartDate has a string surrogate; in new system, enforce as NOT NULL when `IsCompany = false`) | `Customer.vb:415` |
| **BR-CUS-012** | `Email` regex-validated against standard email pattern, only when non-empty (CSLA `RegExPatterns.Email`) | `Customer.vb:417, 425-444` |
| **BR-CUS-013** | If `CanSendNotifications = true`, `Email` is **required**. Macedonian error message: `"Потребно е да се внесе Email"` ("An email must be entered") | `Customer.vb:420-421, 446-455` |
| **BR-CUS-014** | `IsCompany` (boolean) distinguishes individuals from legal entities. Affects which validations apply (notably BR-CUS-003). | `Customer.vb:240, 474` |

## Validation rules — child entities

### CustomerContactPerson (child of Customer)

| Rule | Statement | Source |
|---|---|---|
| **BR-CUS-020** | Contact-person `EMBG` (legacy `Mb`) max 13 | `CustomersContactPerson.vb:99` |
| **BR-CUS-021** | Contact-person `FirstName` (legacy `PersonName`) **required** + max 50 | `CustomersContactPerson.vb:101-102` |
| **BR-CUS-022** | Contact-person `Surname` (legacy `PersonSurname`) **required** + max 50 | `CustomersContactPerson.vb:104-105` |
| **BR-CUS-023** | Contact-person `PhoneNumber` max 20 | `CustomersContactPerson.vb:107` |
| **BR-CUS-024** | Contact-person `MobileNumber` max 20 | `CustomersContactPerson.vb:109` |
| **BR-CUS-025** | Contact-person `Email` max 50 (legacy is shorter than Customer's email field — keep as 200 in new schema for consistency) | `CustomersContactPerson.vb:111` |

### CustomerBankAccount (child of Customer)

| Rule | Statement | Source |
|---|---|---|
| **BR-CUS-030** | `BankAccount` **required** + max 50 | `CustomerBankAccount.vb:72-73` |
| **BR-CUS-031** | `DeponentBank` (depository bank name) **required** + max 50 | `CustomerBankAccount.vb:75-76` |
| **BR-CUS-032** | Bank-account `TaxNumber` max 15 (separate from Customer.TaxNumber — Q-007: why two?) | `CustomerBankAccount.vb:78` |

## Authorization rules — DROPPED

The legacy code has ~30 lines per editable property of `If user.IsInFieldsPrivilegesCanWrite("FieldName") Then AllowWrite Else DenyWrite`. **All of this is dropped** per audit §3.2 (the new auth model is two roles, no field-level privileges). Both Administrator and Operator have full write access to all customer fields. Tenancy alone (StationId scoping) governs visibility.

## Stored-procedure references found

The CSLA `Customer` entity uses these procs (names visible in `DataPortal_Insert/Update/Delete`):

- `addCustomer` — insert (returns `@newId`, `@newLastChanged` rowversion)
- `updateCustomer` — update with concurrency check via rowversion
- `deleteCustomer` — delete by Id
- `getCustomerById` — fetch single
- `NumOfCustomerMBExists` — uniqueness check on EMBG (the BR-CUS-002 query)

Plus the child-table procs: `addCustomersContactPerson` / `updateCustomersContactPerson` / `deleteCustomersContactPerson` and `addCustomersBankAccount` / `updateCustomersBankAccount` / `deleteCustomersBankAccount`.

Bodies of these procs are not transcribed in this pass; they may contain additional rules (FK validation, side-effect inserts, audit logging). Flagged for Q-008.

## Open questions surfaced during this extraction

- **Q-005:** Is the EMBG uniqueness check (BR-CUS-002) actually applied at runtime, or is the only enforcement the commented-out validation rule plus the proc-side check? The proc `NumOfCustomerMBExists` exists, suggesting yes — but the form may not call it. Audit later.
- **Q-006:** Is the EMBG checksum check (BR-CUS-003) disabled in production? The validation rule is commented out in source.
- **Q-007:** Why do both `Customers.TaxNumber` and `CustomerBankAccounts.TaxNumber` exist? Same value? Different (e.g., bank's tax number vs customer's)?
- **Q-008:** What additional rules live inside the customer-related stored proc bodies (31 procs in inventory)?

## Schema rename map (legacy → new)

| Legacy | New | Reason |
|---|---|---|
| `Mb` | `EMBG` | clearer name (EMBG is the Macedonian term used in real life) |
| `BLK` | `IDCardNumber` | clearer name |
| `BLKDateIssued` | `IDCardDateIssued` | match |
| `BLKIssuer` | `IDCardIssuerId` | match + `Id` suffix convention |
| `IdBirhCity` | `IdBirthCity` | **typo fix** |
| `BrithAddressNumber` | `BirthAddressNumber` | **typo fix** |
| `DriveingLicenceNumber` | `DrivingLicenceNumber` | **typo fix** |
| `DriveingLicenceDateIssued` | `DrivingLicenceDateIssued` | **typo fix** |
| `DriveingLicenceIssuer` | `DrivingLicenceIssuerId` | typo fix + `Id` suffix |
| `CustomerFirstName` | `FirstName` | redundant prefix removed |
| `CustomerSurname` | `Surname` | redundant prefix removed |
| `IdLivingCity` | `LivingCityId` | `Id` suffix convention |
| `IdLivingAddress` | `LivingAddressId` | match |
| `IdBusinessType` | `BusinessTypeId` | match |
| `IdCitizenship` | `CitizenshipId` | match |
| `Customers.ContactPersons` | `CustomerContactPersons` | dot in name → underscore-free name |
| `CustomersBankAccounts` | `CustomerBankAccounts` | drop redundant plural |

The migration script (later sub-project) handles the legacy-to-new column mapping.
