# VTE Domain & Schema Audit Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Produce `docs/superpowers/specs/2026-04-30-vte-domain-audit.md` — a single living document that fully describes the legacy VTE system (tables, screens, business rules, print templates, security model, integrations) so every later sub-project (new schema, backend, frontend, migration) can be designed against a known-correct picture.

**Architecture:** Three sequential reading passes over the legacy `WinApp` (VB.NET WinForms), `VTE.Library` and `VTE.BaseParts` (CSLA business objects), plus `WinApp/sqlData.sql` and `WinApp/emSecurity.sql`. Pass 1 = mechanical schema sweep. Pass 2 = screens + business rules per module in dependency order. Pass 3 = cross-cutting concerns + per-table migration decisions. Three checkpoints with the stakeholder, one per pass.

**Tech Stack:** Read-only operations on the legacy codebase. Write operations only on the audit document and `audit-inputs/` companion folder. No code execution. No DB connection. No git in this directory — durable progress is recorded by appending entries to a *Progress log* appendix at the bottom of the audit doc itself.

---

## Repo & path conventions used throughout this plan

- **Repo root:** `C:/Users/FilipIlievski/Downloads/trunk/trunk/` — referenced as `<root>` from here on.
- **Audit document (deliverable):** `<root>/docs/superpowers/specs/2026-04-30-vte-domain-audit.md`. Referenced as `<audit>` from here on.
- **Audit-inputs folder:** `<root>/docs/superpowers/specs/audit-inputs/`. Referenced as `<inputs>` from here on.
- **Spec:** `<root>/docs/superpowers/specs/2026-04-30-vte-domain-audit-design.md`. Referenced as `<spec>` from here on.
- **Legacy SQL files:** `<root>/WinApp/sqlData.sql` (main DB, ~26,883 lines) and `<root>/WinApp/emSecurity.sql` (security DB, ~2,349 lines).
- **Legacy WinForms project:** `<root>/WinApp/` and its subfolders (Customers, Vehicles, Documents, Payment, Requests, Privileges, Operators, PivotReports, Relations, Stampa, dijFormi, Twain, Hepers).
- **Legacy CSLA business objects:** `<root>/VTE.Library/` and `<root>/VTE.BaseParts/`.

## Module ID and BR ID conventions used throughout this plan

| Module ID | Module name             | Audit doc anchor |
|-----------|-------------------------|------------------|
| REF       | Reference data          | §4.1             |
| CUS       | Customers               | §4.2             |
| VEH       | Vehicles                | §4.3             |
| REQ       | Requests                | §4.4             |
| DOC       | Documents               | §4.5             |
| PAY       | Payments                | §4.6             |
| RPT       | Reports & dashboard     | §4.7             |
| SEC       | Operators & security    | §4.8             |
| ATT       | Attachments & scanning  | §4.9             |
| INF       | Infrastructure          | §4.10            |

Business-rule IDs use the format `BR-<module-id>-NNN`, e.g. `BR-PAY-014`. NNN is zero-padded three digits and unique within the module. IDs are stable once assigned — never renumbered.

Open-question IDs use `Q-NNN`, zero-padded three digits, globally unique across the document. IDs are stable once assigned.

## Table-to-module assignment (locked-in, pre-classified)

Decisions made up-front so Pass 1 is deterministic. Tables not in this list go to "Module: TBD" and are reclassified at end of Pass 1 (Task B5).

| Module | Tables (legacy names) |
|---|---|
| **REF (4.1)** | `BusinessTypes`, `Cities`, `Communities`, `Countries`, `Streets`, `Colors`, `ColorsDetails`, `AttachmentTypes`, `DDVCatalog`, `PriceCatalog`, `RequestTypes`, `RegistrationIssuers`, `DocumentTypes`, `DocumentTypePrint`, `DocumentTypesOptions`, `DocumentTypesOptionsDetails`, `DriveingLicenceCtegories`, `PaymentCategories`, `PaymentTypes`, `TehnicalExamsTypes`, `TehnicalExamOrganizations`, `TehnicalExamVehiclePartsCategories`, `TehnicalExamVehicleParts`, `VehicleMakers`, `VehicleModel`, `VehicleCategories`, `VehicleCategoriesRelations`, `VehicleCategoryForPayments`, `VehicleJUSCategories`, `VehicleBodytype`, `VehicleBrakes`, `VehicleGearBox`, `VehicleUse`, `VehicleTireTypes`, `VehicleSupporting`, `VehicleEngineTypes`, `VehicleEnginePowerSourceTypes`, `VehicleEngineEcoProgram`, `EngineTypeModelRelations`, `VehiclePayTollCategory`, `VehicleRequiredFields`, `VehicleDisabledFields`, `CustomerVehiclesRelationTypes` |
| **CUS (4.2)** | `Customers`, `Customers.ContactPersons`, `CustomersBankAccounts`, `CustomerFinancialState` |
| **VEH (4.3)** | `Vehicles`, `Vehicle.Registrations`, `Vehicle.Axis`, `Vehicle.BetweenAxesDestinations`, `Vehicle.Tyres`, `VehiclePayToll`, `CustomerVehiclesRelations` |
| **REQ (4.4)** | `Requests`, `Request.VehicleOwnershipProofs`, `Request.PaymentProof` |
| **DOC (4.5)** | `Documents`, `DocumentsTrafficLicences`, `DocumentsTrafficLicences.Extensions`, `DocumentsPermisions`, `DocumentsInternationalDriveingLicences`, `DocumentsInternationalDriveingLicences.ValidForCategories`, `DocumentsTehnicalExamsReports`, `DocumentsTehnicalExamsReportsDetails`, `DocumentsTehnicalExamsReportsDetailsStatus`, `DocumentVehicleOwnershipProof`, `DocumentPaymentProof` |
| **PAY (4.6)** | `PaymentDocuments`, `PaymentDocumentsDetails`, `PaymentDocumentsRata`, `PaymentItems`, `PaymentItemParametars`, `CalculationItems`, `DogovorZaRati`, `SecurityHouses` |
| **RPT (4.7)** | (no own tables; uses cross-module reads) |
| **SEC (4.8)** | (from `emSecurity.sql`) `Users`, `Roles`, `Employes`, `FieldsPrivileges`, `ObjectPrivileges`, `CSLAObjects`, `DataBases`, `SecurityPolicies` |
| **ATT (4.9)** | `DocumentAttachments` |
| **INF (4.10)** | (no own tables) |

`AttachmentTypes` is classified as REF (it's a lookup); `DocumentAttachments` (the actual attachment rows) is ATT.

## Form-to-module assignment (locked-in, pre-classified)

| Module | Forms |
|---|---|
| **REF (4.1)** | `WinApp/Customers/uxBusinessTypes.vb`, `uxCities.vb`, `uxCommunities.vb`, `uxCountries.vb`, `uxStreets.vb`; `WinApp/Vehicles/uxBrakes.vb`, `uxColors.vb`, `uxTehnicalExamOrganizations.vb`, `uxVehicleBodytypes.vb`, `uxVehicleCategories.vb`, `uxVehicleCategoryForPayments.vb`, plus the rest of `WinApp/Vehicles/` lookups; `WinApp/Documents/uxDocumentTypes.vb`, `uxDocumentTypePrint.vb`; `WinApp/Payment/uxDDVCatalog.vb`, `uxPaymentCategories.vb`, `uxPaymentTypes.vb`; `WinApp/Requests/uxRequestTypes.vb`; `WinApp/Relations/uxRelationsTypes.vb` |
| **CUS (4.2)** | `WinApp/Customers/uxCustomers.vb`, `uxCustomersList.vb`, `uxCustomerPivotReport.vb` |
| **VEH (4.3)** | `WinApp/Vehicles/VehicleMain/*` (all forms), `WinApp/Relations/uxAddNewRelation.vb` |
| **REQ (4.4)** | `WinApp/Requests/uxRequestEdit.vb`, `WinApp/Requests/PrintPlav.vb`, `printBel.vb`, `printZelen.vb`, `WinApp/Stampa/PrintRequests/*` |
| **DOC (4.5)** | `WinApp/Documents/InternationalDriveingLicences/*`, `Permisions/*`, `Requests/*`, `TehnicalExamReports/*`, `TrafficLicences/*`, `WinApp/Stampa/TehnicalExamReports/*` |
| **PAY (4.6)** | `WinApp/Payment/*` (all forms incl. rpt* report templates: `rptFaktura.vb`, `rptInvoiceCompact.vb`, `rptKasovIzvestaj.vb`, `rptPaymentDocumentByID*.vb`, `rptPaymentDocumentDogovor.vb`, `rptPaymentDocumentRataByIDCompact*.vb`; plus `dijGarant.vb`, `uxCalculationItems.vb`, `uxPaymentDocument.vb`, `uxPaymentDocumentsList.vb`, `uxFakturiList.vb`, `uxFinancialStateCreatePaymentDocuments.vb`, `uxUnpayedDealsList.vb`, `uxReportByParametarsFromToForPayment.vb`, `uxReportByPaymentCategory.vb`, `uxCaclulationPivot.vb`, `uxPaymentPivotReport.vb`, `uxPaymentPivotShort.vb`) |
| **RPT (4.7)** | `WinApp/uxDashboard.vb`, `WinApp/PivotReports/uxPivotReportCustomerVehicle.vb`, `WinApp/uxPrint.vb`, `WinApp/uxSplash.vb`, `WinApp/dijTest.vb` |
| **SEC (4.8)** | `WinApp/Operators/dijOperators.vb`, `dijChangeUserNameAndPass.vb`, `uxCompany.vb`, `WinApp/Privileges/*` (all forms: `dijFildsPrivilege.vb`, `dijRools.vb`, `uxCSLAObjects.vb`, `uxPrivileges.vb`) |
| **ATT (4.9)** | `WinApp/Twain/*` (scanner UI) |
| **INF (4.10)** | `WinApp/MainForm.vb`, `Login.vb`, `Splash.vb`, `Installer.vb`, `MainModul.vb`, `RijndaelCryptography.vb`, `AutoUpdate.vb` (referenced as out-of-scope for porting), `WinApp/dijFormi/*`, `WinApp/Hepers/*` |

Forms not in this list at audit-time go to "Module: TBD" and are reclassified at end of Pass 2 Designer-pass.

## Audit doc skeleton (used by Task A1)

The skeleton has every section heading from `<spec>` §3.2 with no content yet, plus a "Progress log" appendix used as the durable per-task checkpoint mechanism. The full skeleton text appears verbatim in Task A1, Step 3.

## Acceptance bar (used by every Pass-end task)

A pass is complete when its outputs match `<spec>` §6, restricted to the relevant criteria for that pass:

- **End of Pass 1:** §6 criteria 1, 2 (rows present, decision = TBD allowed at this stage), 4. Plus: every table is filed in *exactly one* module's *4.x.2 Tables* sub-section. No "Module: TBD" entries remain.
- **End of Pass 2:** §6 criteria 5, 6, 7. Plus: every form in `WinApp/` is filed in exactly one module's *4.x.3 Screens*. Deep-read modules have non-empty *4.x.4 Business rules* (or an explicit "no rules found, paths read: …" statement).
- **End of Pass 3:** §6 criteria 8, 9, 10, 13, plus a quality-bar pass against criteria 11, 12, 14.
- **Final:** all 14 §6 criteria simultaneously.

---

## Phase A — Setup (1 task)

### Task A1: Create the audit-inputs folder, README, and audit-document skeleton

**Files:**
- Create: `<inputs>/README.md`
- Create: `<audit>` (the audit document, with full ToC scaffold and empty sections)

- [ ] **Step 1: Verify audit-inputs folder does not yet exist**

Run (Bash): `ls "<root>/docs/superpowers/specs/audit-inputs/" 2>&1 || echo "does-not-exist"`
Expected: either an error/`does-not-exist` line, OR a directory listing. If the folder already exists with content, do not overwrite — read the content first and reconcile manually.

- [ ] **Step 2: Create audit-inputs README**

Write `<inputs>/README.md` with this exact content:

```markdown
# audit-inputs

Companion folder for the VTE Domain & Schema Audit
(`docs/superpowers/specs/2026-04-30-vte-domain-audit.md`).

## What goes here (committable)

- `app.config.redacted.xml` — a redacted production `app.config` from any
  station. All encrypted connection-string values must be replaced with the
  literal string `«REDACTED — see audit-inputs/README»`. The structure of the
  file (XML elements, attribute names, encryption indicators) is preserved.
- Redacted printed samples (PDF or image) of each official document:
  - `print-sample-traffic-licence-extension.*`
  - `print-sample-request-blue.*` (Plav)
  - `print-sample-request-white.*` (Bel)
  - `print-sample-request-green.*` (Zelen)
  - `print-sample-payment-receipt.*`
  - `print-sample-invoice-faktura.*`
  - `print-sample-cash-report-kasov-izvestaj.*`
  Personal data (names, ID numbers, plate numbers, addresses) redacted.

## What does NOT go here (NEVER committed)

- Raw encrypted `app.config` (it's a credential).
- Encryption key, salt, or passphrase used by the `Crypt` /
  `RijndaelCryptography` class.
- Production database backups or extracts.
- Customer-identifying data of any kind.

Out-of-git location for sensitive material: agreed separately with the
stakeholder.

## Redaction policy for printed samples

- Personal names → `«NAME»`.
- ID numbers (EMBG, passport, driver-licence number) → `«ID»`.
- Plate numbers → `«PLATE»`.
- Addresses → `«ADDRESS»`.
- Phone numbers → `«PHONE»`.
- Bank account numbers → `«ACCOUNT»`.
- Stamps and signatures stay (they're layout, not data).
- Static labels, headings, and form structure stay verbatim.
```

- [ ] **Step 3: Create audit-document skeleton**

Write `<audit>` with this exact content (full skeleton — every section header from `<spec>` §3.2, plus the Progress log):

```markdown
# VTE Domain & Schema Audit

**Date started:** 2026-04-30
**Spec:** [2026-04-30-vte-domain-audit-design.md](2026-04-30-vte-domain-audit-design.md)
**Status:** in progress (Pass 1 not started)

---

## 1. Executive summary

(Filled at end of Pass 3.)

---

## 2. Glossary (Macedonian → English)

### 2.1 Domain terms

| Macedonian | English | Confirmed? | Notes |
|---|---|---|---|
| _(populated during Pass 1 and Pass 2)_ | | | |

### 2.2 File / folder naming conventions

| Prefix | Meaning | Example |
|---|---|---|
| `ux*` | User-control / form (main editing UI) | `uxCustomers.vb` |
| `dij*` | Dialog (modal sub-form) | `dijOperators.vb` |
| `rpt*` | Report (printed document) | `rptFaktura.vb` |
| `print*` | Print template (request forms) | `PrintPlav.vb` |

---

## 3. Tenancy & auth model — TARGET

The new system is multi-tenant SaaS. Every domain table carries a `StationId` discriminator. Each existing legacy per-station database becomes one tenant row in the new shared database. Auth is shared across tenants; tenancy is enforced at the API and query level.

This differs from legacy, which deployed one DB per station plus a separate `emSecurity` DB containing CSLA users, roles, and field/object privileges.

(Detail filled during Pass 3.)

---

## 4. Per-module sections

### 4.1 Reference data (REF)

#### 4.1.1 Purpose
#### 4.1.2 Tables
#### 4.1.3 Screens
#### 4.1.4 Business rules
#### 4.1.5 Print templates
#### 4.1.6 Open questions
#### 4.1.7 Migration notes

### 4.2 Customers (CUS)

#### 4.2.1 Purpose
#### 4.2.2 Tables
#### 4.2.3 Screens
#### 4.2.4 Business rules
#### 4.2.5 Print templates
#### 4.2.6 Open questions
#### 4.2.7 Migration notes

### 4.3 Vehicles (VEH)

#### 4.3.1 Purpose
#### 4.3.2 Tables
#### 4.3.3 Screens
#### 4.3.4 Business rules
#### 4.3.5 Print templates
#### 4.3.6 Open questions
#### 4.3.7 Migration notes

### 4.4 Requests (REQ)

#### 4.4.1 Purpose
#### 4.4.2 Tables
#### 4.4.3 Screens
#### 4.4.4 Business rules
#### 4.4.5 Print templates
#### 4.4.6 Open questions
#### 4.4.7 Migration notes

### 4.5 Documents (DOC)

#### 4.5.1 Purpose
#### 4.5.2 Tables
#### 4.5.3 Screens
#### 4.5.4 Business rules
#### 4.5.5 Print templates
#### 4.5.6 Open questions
#### 4.5.7 Migration notes

### 4.6 Payments (PAY)

#### 4.6.1 Purpose
#### 4.6.2 Tables
#### 4.6.3 Screens
#### 4.6.4 Business rules
#### 4.6.5 Print templates
#### 4.6.6 Open questions
#### 4.6.7 Migration notes

### 4.7 Reports & dashboard (RPT)

#### 4.7.1 Purpose
#### 4.7.2 Tables
#### 4.7.3 Screens
#### 4.7.4 Business rules
#### 4.7.5 Print templates
#### 4.7.6 Open questions
#### 4.7.7 Migration notes

### 4.8 Operators & security (SEC)

#### 4.8.1 Purpose
#### 4.8.2 Tables
#### 4.8.3 Screens
#### 4.8.4 Business rules
#### 4.8.5 Print templates
#### 4.8.6 Open questions
#### 4.8.7 Migration notes

### 4.9 Attachments & scanning (ATT)

#### 4.9.1 Purpose
#### 4.9.2 Tables
#### 4.9.3 Screens
#### 4.9.4 Business rules
#### 4.9.5 Print templates
#### 4.9.6 Open questions
#### 4.9.7 Migration notes

### 4.10 Infrastructure (INF)

#### 4.10.1 Purpose
#### 4.10.2 Tables
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
| _(populated during Pass 1; decision filled in Pass 3)_ | | | | | |

Decision values: `keep-rename` | `split` | `merge` | `drop` | `new`.

---

## 7. Risks register

| ID | Description | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R-1 | Encrypted connection strings, no key (see `<spec>` §7) | high | blocks migration sub-project | Document encryption scheme; obtain redacted `app.config` + key material |
| R-2 | Business rules hidden in CSLA partial classes / form code-behind | medium-high | incorrect behavior in rewrite | Deep-read both library and form locations; flag anomalies in §8 |
| R-3 | Per-station schema drift | high | migration sub-project handles it, not audit | Capture baseline; defer per-station diff to migration |
| R-4 | Macedonian-only domain knowledge | medium | glossary errors propagate | Flag every term "needs confirmation" until stakeholder signs off |
| R-5 | Print templates as legal artifacts | medium | report-rebuild sub-project at risk | Capture Designer-level layouts; require redacted printed samples |

(More risks added during Pass 3.)

---

## 8. Open questions register

| ID | Module | Status | Question | Source | Owner | Assumption | Impact | Resolution |
|---|---|---|---|---|---|---|---|---|
| _(populated during all passes)_ | | | | | | | | |

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
```

- [ ] **Step 4: Verify the doc renders cleanly**

Run (Bash): `head -60 "<audit>"` and visually confirm: top-level `# VTE Domain & Schema Audit` heading, sections numbered 1–8 plus appendices, no orphan headings, no broken code fences.

Expected: clean Markdown structure ending with the Progress log section and one entry for A1.

- [ ] **Step 5: Confirm the inputs folder exists**

Run (Bash): `ls "<inputs>/"`
Expected: a single `README.md` file is listed.

---

## Phase B — Pass 1: Schema sweep (mechanical, broad)

**Goal of Phase B:** every `CREATE TABLE` from both SQL files appears verbatim in the right module's *4.x.2 Tables* sub-section, every table has a row in §6 with `decision = TBD`, every Macedonian column-name term appears in §2 glossary as "needs confirmation".

### Task B1: Inventory all tables, views, procs, triggers in `sqlData.sql`

**Files:**
- Read: `<root>/WinApp/sqlData.sql`
- Modify: `<audit>` (Progress log only at this task; tables filed in B2)

- [ ] **Step 1: Grep for `CREATE TABLE` in `sqlData.sql`**

Use Grep tool: pattern `^CREATE TABLE`, path `<root>/WinApp/sqlData.sql`, output_mode `content`, head_limit 0 (unlimited), `-n` true, `-i` true.

Expected: a list of ~80 lines, each `<line-number>:CREATE TABLE [dbo].[<TableName>](`. Save the full list mentally / in a scratch note (do NOT create a separate file).

- [ ] **Step 2: Grep for `CREATE VIEW` in `sqlData.sql`**

Use Grep tool: pattern `^CREATE VIEW`, same path, output_mode `content`, head_limit 0, `-n` true, `-i` true.
Expected: zero or more matches with line numbers.

- [ ] **Step 3: Grep for `CREATE PROCEDURE` in `sqlData.sql`**

Use Grep tool: pattern `^CREATE PROCEDURE|^CREATE PROC`, same path, output_mode `content`, head_limit 0, `-n` true, `-i` true.
Expected: zero or more matches with line numbers.

- [ ] **Step 4: Grep for `CREATE TRIGGER` in `sqlData.sql`**

Use Grep tool: pattern `^CREATE TRIGGER`, same path, output_mode `content`, head_limit 0, `-n` true, `-i` true.
Expected: zero or more matches with line numbers.

- [ ] **Step 5: Grep for `CREATE INDEX` and `CREATE UNIQUE INDEX` in `sqlData.sql`**

Use Grep tool: pattern `^CREATE (UNIQUE )?INDEX`, same path, output_mode `content`, head_limit 0, `-n` true, `-i` true.
Expected: a list of indexes with line numbers.

- [ ] **Step 6: Cross-check inventory against the locked-in module assignment**

Compare the table list from Step 1 against the **Table-to-module assignment** table at the top of this plan. Any table from Step 1 that is NOT in the locked-in assignment is a "Module: TBD" candidate — note it for reclassification at Task B5.

Expected: most tables are pre-classified; a small number (likely 0–5) are "Module: TBD".

- [ ] **Step 7: Append progress-log entry**

Use Edit tool on `<audit>`, replacing the last line of the Progress log with itself plus a new line:

old_string:
```
- 2026-04-30 — A1 — created audit-inputs/, README, audit doc skeleton
```
new_string:
```
- 2026-04-30 — A1 — created audit-inputs/, README, audit doc skeleton
- 2026-04-30 — B1 — inventoried sqlData.sql: <N> tables, <V> views, <P> procs, <T> triggers, <I> indexes; <X> Module-TBD tables flagged
```
(Replace `<N>`, `<V>`, `<P>`, `<T>`, `<I>`, `<X>` with the actual counts from Steps 1–6.)

### Task B2: Transcribe every `CREATE TABLE` from `sqlData.sql` into its §4.x.2 Tables sub-section

**Files:**
- Read: `<root>/WinApp/sqlData.sql` (table-block-by-table-block)
- Modify: `<audit>` §4.1.2 through §4.10.2

- [ ] **Step 1: For each table in the locked-in assignment, find the table block**

Procedure (repeat for every table):
- Use Grep with pattern `^CREATE TABLE \[dbo\]\.\[<TableName>\]`, path `<root>/WinApp/sqlData.sql`, output_mode `content`, `-n` true, head_limit 1. Get the start line number `<start>`.
- Use Grep with pattern `^GO$`, path `<root>/WinApp/sqlData.sql`, output_mode `content`, `-n` true. Find the next `GO` after `<start>`. Call its line `<end>`.
- Use Read tool on `<root>/WinApp/sqlData.sql` with `offset=<start>` and `limit=<end - start>`. Capture the verbatim table block (`CREATE TABLE` through the closing `)`, plus any `CONSTRAINT`, `WITH`, etc., up to but not including the `GO`).

Expected: one verbatim SQL block per table, ready to paste.

- [ ] **Step 2: Append the table block into the right module's §4.x.2 Tables sub-section**

For each table block from Step 1, locate the right module heading in `<audit>` (use the locked-in assignment) and use Edit tool to append the block under that heading. The append format is:

````markdown
##### `<TableName>`

Source: `WinApp/sqlData.sql:<start>-<end>`

```sql
<verbatim CREATE TABLE block>
```
````

Where the heading level is `#####` (5 hashes) so it nests under `#### 4.x.2 Tables` (4 hashes).

If a module's *4.x.2 Tables* sub-section is empty (only a heading), append directly under it. If it already has table entries, append after the last existing entry.

Expected: after Step 2 has run for every table, every module's *4.x.2* contains the verbatim SQL for its tables, each prefixed by a source citation.

- [ ] **Step 3: Append a row per table to §6 Migration master table**

For each table, append one row to the §6 table:

| `<TableName>` | unknown — needs prod sample | TBD | TBD | TBD |

Source citation column is omitted (the table itself encodes the source via §4.x.2).

Expected: §6 has one row per legacy table, all decisions = TBD, all row counts = "unknown — needs prod sample".

- [ ] **Step 4: Add Macedonian column-name terms to §2 glossary**

While transcribing, any column or table name that is Macedonian (Cyrillic or transliterated, e.g. `DogovorZaRati`, `Garant`, `Faktura`, `KasovIzvestaj`, `Plav`, `Bel`, `Zelen`, `dijFormi`, `Stampa`, `Hepers`, `izmeniBaza`, `Opcii`, `Opcija`, `Cena`, `Iznos`, `Datum`, `Broj`) gets a §2.1 row:

| Macedonian | English | Confirmed? | Notes |
|---|---|---|---|
| Dogovor za rati | Installment contract | needs confirmation | from table `DogovorZaRati` |
| ... | ... | ... | ... |

`Confirmed?` always starts as `needs confirmation`. Stakeholder signs off later.

- [ ] **Step 5: Verify no table block is duplicated**

Run (Grep): pattern `^##### \`(.*?)\``, path `<audit>`, output_mode `content`, `-n` true.
Compute occurrence counts. Any table name with count > 1 is a duplicate; remove the duplicate. Any table missing from the inventory (Task B1 Step 1) needs to be added.

Expected: each table name appears in exactly one §4.x.2 sub-section.

- [ ] **Step 6: Append progress-log entry**

Edit `<audit>` Progress log to add:
```
- 2026-04-30 — B2 — transcribed <N> CREATE TABLE blocks from sqlData.sql into §4.x.2 sub-sections; populated §6 with TBD rows
```

### Task B3: Transcribe non-table objects (views, procs, triggers, indexes) from `sqlData.sql`

**Files:**
- Read: `<root>/WinApp/sqlData.sql`
- Modify: `<audit>` §4.x.2 sub-sections (under the owning table where possible)

- [ ] **Step 1: Read and file each VIEW**

For every view from B1 Step 2: Read the block (Grep its CREATE line, find next `GO`, Read the range), then file under the §4.x.2 sub-section of the most-referenced underlying table. If a view spans many modules, file under the module of the table named first in the `FROM` clause; record a note in §8 if the choice is ambiguous.

Format under the owning table:

````markdown
##### View `<ViewName>`

Source: `WinApp/sqlData.sql:<start>-<end>`. Underlies: `<list of source tables>`.

```sql
<verbatim view DDL>
```
````

- [ ] **Step 2: Read and file each STORED PROCEDURE**

For every proc: Read the block, file under the §4.x.2 of the table the proc primarily writes to (or the table named first in the proc's first `INSERT`/`UPDATE`/`DELETE`). If a proc only reads, file under the table named first in `FROM`. Same Markdown format as views.

- [ ] **Step 3: Read and file each TRIGGER**

For every trigger: file under the §4.x.2 of the table the trigger is defined on (the `CREATE TRIGGER ... ON [dbo].[<TableName>]` clause makes ownership unambiguous). Same Markdown format.

- [ ] **Step 4: File indexes inline with their table**

For every `CREATE INDEX ... ON [dbo].[<TableName>](...)`: append the DDL line(s) to the existing `<TableName>` entry in §4.x.2 as a small `Indexes:` sub-block immediately after the table's `CREATE TABLE` block.

- [ ] **Step 5: Verify nothing was skipped**

Run (Grep) again on `<root>/WinApp/sqlData.sql` for `^CREATE` (pattern `^CREATE `, output_mode `count`). The count must equal: tables (Task B1 Step 1 result) + views + procs + triggers + indexes. Any leftover (e.g. `CREATE FUNCTION`, `CREATE TYPE`) gets filed under the most-relevant module with a §8 question.

- [ ] **Step 6: Append progress-log entry**

```
- 2026-04-30 — B3 — filed <V> views, <P> procs, <T> triggers, <I> indexes from sqlData.sql
```

### Task B4: Transcribe `emSecurity.sql` into §4.8 (Operators & security)

**Files:**
- Read: `<root>/WinApp/emSecurity.sql`
- Modify: `<audit>` §4.8.2

- [ ] **Step 1: Inventory `emSecurity.sql`**

Use Grep on `<root>/WinApp/emSecurity.sql` for `^CREATE TABLE`, `^CREATE VIEW`, `^CREATE PROCEDURE|^CREATE PROC`, `^CREATE TRIGGER`, `^CREATE (UNIQUE )?INDEX`, each in its own grep call, output_mode `content`, `-n` true, head_limit 0.
Expected: a list of all DDL objects in the security DB.

- [ ] **Step 2: Transcribe every CREATE TABLE block into §4.8.2**

Same procedure as B2 Step 1 + Step 2, but all tables go into §4.8.2 (the entire `emSecurity.sql` is the SEC module's source).

- [ ] **Step 3: Transcribe non-table objects under owning tables in §4.8.2**

Same procedure as B3.

- [ ] **Step 4: Add a §6 row per table**

Same as B2 Step 3.

- [ ] **Step 5: Note that §4.8 represents a separate database**

Use Edit tool to add a paragraph at the top of §4.8.2:

```markdown
> **Note.** All tables in §4.8.2 are from a separate legacy database called
> `emSecurity` (see `WinApp/app.config` `SecurityConnection`). In the new
> multi-tenant SaaS DB they merge into the same shared schema as the rest of
> the modules, with `StationId` discriminators where appropriate.
```

- [ ] **Step 6: Append progress-log entry**

```
- 2026-04-30 — B4 — transcribed emSecurity.sql into §4.8.2 (<N> tables, <V> views, <P> procs, <T> triggers, <I> indexes)
```

### Task B5: Reclassify "Module: TBD" tables

**Files:**
- Modify: `<audit>` §4.x.2 (move tables to their final module)

- [ ] **Step 1: List any tables currently in "Module: TBD"**

If Task B2 Step 1 left any tables un-classified, they were appended to a temporary "Module: TBD" sub-section at the bottom of the document. Use Grep on `<audit>` for `Module: TBD` to locate them.

If the result is zero matches, skip to Step 4.

- [ ] **Step 2: For each TBD table, decide its module**

Heuristics, applied in order:
1. If the table name starts with `Vehicle*`, `Tehnical*`, `JUS*`, `Color*`, `Brakes`, `GearBox`, `Tire*`, `EngineType*`, `EnginePower*`, `Body*`, `Maker*`, `Model` → REF.
2. If the table name starts with `Customer*` → CUS.
3. If the table name starts with `Vehicle.*` (with dot, indicating a child table) → VEH.
4. If the table name starts with `Request*` → REQ.
5. If the table name starts with `Document*` → DOC.
6. If the table name contains `Payment`, `Faktura`, `DDV`, `Calculation`, `Dogovor`, `Kasov`, `Garant`, `SecurityHouse` → PAY.
7. If the table name appears in the `emSecurity.sql` inventory → SEC.
8. If the table name contains `Attachment` → ATT, except `AttachmentTypes` (a lookup) which goes to REF.
9. Otherwise → log as a §8 question with `Owner: stakeholder` and assumption `INF (4.10)`.

- [ ] **Step 3: Move each TBD table block to its decided module**

Use Edit tool: cut the entire `##### \`<TableName>\`` block from "Module: TBD" and append to the chosen module's §4.x.2.

- [ ] **Step 4: Remove the "Module: TBD" sub-section**

Use Edit tool to delete the (now empty) "Module: TBD" sub-section from the document.

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — B5 — reclassified <X> Module-TBD tables to final modules
```

### Task B6: Pass 1 stakeholder checkpoint

**Files:**
- Read: `<audit>`
- No modifications

- [ ] **Step 1: Verify Pass 1 acceptance criteria**

Check:
- Every table from Task B1 Step 1 inventory appears in exactly one `##### \`<TableName>\`` heading under some §4.x.2 sub-section. Use Grep with pattern `^##### ` on `<audit>` to count entries; the count must equal (tables in `sqlData.sql`) + (tables in `emSecurity.sql`) + non-table-object headings.
- §6 has the same number of rows as there are tables (subtract non-table-object headings from the previous count).
- No "Module: TBD" sub-section remains.
- §2 glossary has at least one row per Macedonian-named table or column found.

- [ ] **Step 2: Produce a checkpoint summary for the stakeholder**

Write a short summary directly in this conversation (do NOT add to the audit doc as a section — it's ephemeral). The summary contains:

- Counts: tables, views, procs, triggers, indexes per SQL file.
- Per-module table count (from §4.x.2 entries).
- Tables that were Module-TBD and where they ended up.
- The current §2 glossary list, asking the stakeholder to confirm or correct each "needs confirmation" entry.

- [ ] **Step 3: Wait for stakeholder spot-check feedback**

Stakeholder picks 2–3 modules and walks through them against `<root>/WinApp/sqlData.sql` to confirm the schema matches reality. If any module fails the spot-check, that module's table-transcription work is redone (rerun B2 Steps 1–2 for that module's tables only).

- [ ] **Step 4: Append progress-log entry**

```
- 2026-04-30 — B6 — Pass 1 checkpoint: <pass | fail> — <stakeholder feedback summary>
```

If the checkpoint failed, do NOT proceed to Phase C. Re-run the failing module's transcription, then re-run B6.

---

## Phase C — Pass 2: Screens & business objects

**Goal of Phase C:** every WinForm in `<root>/WinApp/` is filed in exactly one module's *4.x.3 Screens* with its tables-touched mapping; every deep-read module has a numbered `BR-<MODULE>-NNN` business-rules list with source citations; every uncertainty is in §8.

**Module order is the dependency order from `<spec>` §3.2: REF → CUS → VEH → REQ → DOC → PAY → RPT → SEC → ATT → INF.**

### Task C0: Inventory CSLA business objects

**Files:**
- Read: `<root>/VTE.Library/`, `<root>/VTE.BaseParts/`
- Modify: `<audit>` (none yet — output is a working table used by C1–C10)

- [ ] **Step 1: List all `.vb` files under `VTE.Library` and `VTE.BaseParts`**

Use Glob with pattern `<root>/VTE.Library/**/*.vb` and `<root>/VTE.BaseParts/**/*.vb` (two glob calls).
Expected: a list of business-object source files.

- [ ] **Step 2: For each file, identify the public class(es) and the table it represents**

For each file:
- Read the file (full).
- Find every `Public Class <ClassName>` declaration and note the class name.
- Look for `BusinessBase`, `BusinessListBase`, `ReadOnlyBase`, `CommandBase` inheritance — these tell you it's a CSLA business object.
- Look for `LoadProperty` / `SetProperty` / `GetProperty` calls and `<TableName>` references in `DataPortal_Fetch`, `DataPortal_Insert`, `DataPortal_Update`, `DataPortal_DeleteSelf` methods to identify the underlying table.

- [ ] **Step 3: Build a per-class assignment table**

Add a new Markdown table at the top of §4.10.2 (Infrastructure → Tables, since this is a cross-module index) titled "CSLA business object index":

```markdown
##### CSLA business object index

| Class | Source file | Module | Underlying table(s) | Summary |
|---|---|---|---|---|
| `Customer` | `VTE.Library/Customers/Customer.vb` | CUS | `Customers`, `Customers.ContactPersons` | editable customer + child contact persons |
| ... | ... | ... | ... | ... |
```

Module assignment uses the same heuristics as B5 Step 2, but applied to class names.

- [ ] **Step 4: Append progress-log entry**

```
- 2026-04-30 — C0 — indexed <N> CSLA business objects across <M> modules
```

### Task C1: Pass 2 — REF (Reference data), Designer-only

**Files:**
- Read: every form in the REF row of "Form-to-module assignment" — Designer files only
- Modify: `<audit>` §4.1.3

- [ ] **Step 1: For each REF form, read its `*.Designer.vb`**

For each form:
- Use Read tool on `<form>.Designer.vb`.
- Extract: form name (from `Partial Class <Name>`), every control name (from `Friend WithEvents <ctrl> As <Type>`), every label text (from `<ctrl>.Text = "..."` lines), the underlying CSLA business class (from `bs.DataSource = GetType(<Class>)` or similar databinding lines).

- [ ] **Step 2: For each form, append a screen entry to §4.1.3**

Format:

````markdown
##### `<FormName>`

Source: `WinApp/<path>/<FormName>.Designer.vb`. Business class: `<ClassName>` (see CSLA index).
Underlying table(s): `<TableName(s)>`.

| Field | Source column | Editable? | Validation | Notes |
|---|---|---|---|---|
| `<ctrl1>` | `<TableName>.<ColumnName>` | yes/no | none / required / maxlen=N / regex / custom | _(Macedonian label preserved)_ |
| ... | ... | ... | ... | ... |
````

For REF forms, "Validation" is filled only with what's visible in the Designer (e.g. `MaxLength = 50`, required-field markers). Do NOT read the form's `.vb` for REF in C1 — that's deep-read, deferred to deep-read modules only.

- [ ] **Step 3: Macedonian label terms → §2 glossary**

Any Macedonian label seen → §2.1 row, `Confirmed? = needs confirmation`.

- [ ] **Step 4: Verify every REF form is filed**

Use Grep on `<audit>` for `^##### \`` occurrences under §4.1.3. Count must equal the count of forms in the REF row of the "Form-to-module assignment" table (~20+ forms). Any missing form is filed before progressing.

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — C1 — REF Designer-pass complete: <N> forms filed in §4.1.3
```

### Task C2: Pass 2 — CUS (Customers), Designer-only

**Files:**
- Read: CUS forms — Designer only
- Modify: `<audit>` §4.2.3

- [ ] **Step 1: Repeat C1 Steps 1–3 for the CUS form list** (`uxCustomers.vb`, `uxCustomersList.vb`, `uxCustomerPivotReport.vb`)

- [ ] **Step 2: Verify** — same as C1 Step 4 but for §4.2.3 vs CUS form list.

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — C2 — CUS Designer-pass complete: 3 forms filed in §4.2.3
```

### Task C3: Pass 2 — VEH (Vehicles), Designer-only

**Files:**
- Read: VEH forms — Designer only
- Modify: `<audit>` §4.3.3

- [ ] **Step 1: List VEH forms**

Use Glob: pattern `<root>/WinApp/Vehicles/VehicleMain/**/*.vb`. Plus `<root>/WinApp/Relations/uxAddNewRelation.vb`.

- [ ] **Step 2: Repeat C1 Steps 1–3 for each VEH form**

- [ ] **Step 3: Verify** — same as C1 Step 4 for §4.3.3.

- [ ] **Step 4: Append progress-log entry**

```
- 2026-04-30 — C3 — VEH Designer-pass complete: <N> forms filed in §4.3.3
```

### Task C4: Pass 2 — REQ (Requests), DEEP READ

**Files:**
- Read: `<root>/WinApp/Requests/uxRequestEdit.vb` and `.Designer.vb`, `PrintPlav.vb`, `printBel.vb`, `printZelen.vb`, `<root>/WinApp/Stampa/PrintRequests/*`, plus the matching CSLA classes from the C0 index.
- Modify: `<audit>` §4.4.3, §4.4.4, §4.4.5

- [ ] **Step 1: Designer pass for all REQ forms**

Same procedure as C1 Steps 1–3, output to §4.4.3.

- [ ] **Step 2: Logic pass — read `uxRequestEdit.vb`**

Read the full file. Look for and record:
- `Private Sub <ctrl>_<event>` handlers — what business action they trigger.
- Calls into the CSLA class (e.g. `oRequest.<Method>` or `oRequest.<Property> = ...`).
- Conditional logic gating UI (e.g. `If <user>.HasPriv(...) Then`).

For each business rule found, write a numbered entry under §4.4.4:

```markdown
- **BR-REQ-001:** _(rule statement)_. Source: `WinApp/Requests/uxRequestEdit.vb:<line>`.
```

- [ ] **Step 3: Logic pass — read the matching CSLA class(es)**

From the C0 index, find the classes underlying `uxRequestEdit` (likely `Request` and any child collections). Read each. For each `AddBusinessRules`, `AddAuthorizationRules`, or non-trivial method, extract a `BR-REQ-NNN` entry.

- [ ] **Step 4: Logic pass — read each print template**

For each of `PrintPlav.vb`, `printBel.vb`, `printZelen.vb` and any `Stampa/PrintRequests/*`:
- Read the `.vb` to capture which fields print where.
- Read the `.Designer.vb` to capture static text and layout.

For each print template, append an entry to §4.4.5:

```markdown
##### `<PrintTemplateName>`

Source: `WinApp/Requests/<file>.vb` + `.Designer.vb`.
Paper size: _(from Designer or "needs confirmation")_.
Static text: _(verbatim Macedonian)_.
Field placement:

| Field on form | Source column or BR ID | Position (approx.) |
|---|---|---|
| ... | ... | ... |
```

- [ ] **Step 5: Anything unclear → §8 question**

For any rule, status transition, calculation, or print decision that can't be confidently extracted, append a `Q-NNN` row to §8 with status `open`, owner = stakeholder or station operator, assumption = the working assumption used to keep moving, impact = what changes if the assumption is wrong.

- [ ] **Step 6: Verify §4.4.4 is non-empty**

If no business rules were found, replace the contents of §4.4.4 with: "No business rules found. Files read: `<list of files read with line ranges>`." (per §6.2 acceptance criterion #6 in `<spec>`).

- [ ] **Step 7: Append progress-log entry**

```
- 2026-04-30 — C4 — REQ deep-read complete: §4.4.3 (<F> forms), §4.4.4 (<R> rules), §4.4.5 (<P> print templates), §8 (+<Q> questions)
```

### Task C5: Pass 2 — DOC (Documents), DEEP READ

**Files:**
- Read: every form under `<root>/WinApp/Documents/*` (recursive) and `<root>/WinApp/Stampa/TehnicalExamReports/*`, plus the matching CSLA classes.
- Modify: `<audit>` §4.5.3, §4.5.4, §4.5.5

- [ ] **Step 1: Designer pass for all DOC forms**

Same procedure as C1 Steps 1–3, output to §4.5.3. Use Glob `<root>/WinApp/Documents/**/*.Designer.vb` and `<root>/WinApp/Stampa/TehnicalExamReports/**/*.Designer.vb`.

- [ ] **Step 2: Logic pass — read each DOC form's `.vb`**

For each form ending in `*.vb` (not `.Designer.vb`), Read fully. Extract `BR-DOC-NNN` rules.
Subdivide by document type for clarity:
- Traffic licences → BR-DOC-1xx
- Permissions → BR-DOC-2xx
- International driving licences → BR-DOC-3xx
- Tech-exam reports → BR-DOC-4xx
- Document-types config → BR-DOC-9xx

- [ ] **Step 3: Logic pass — CSLA classes**

From C0 index, find `Document*` classes; read each; extract rules.

- [ ] **Step 4: Logic pass — print templates under Documents**

Same procedure as C4 Step 4 for any `rpt*` or `print*` files under `Documents/` or `Stampa/TehnicalExamReports/`. Output to §4.5.5.

- [ ] **Step 5: Anything unclear → §8 question**

Same as C4 Step 5.

- [ ] **Step 6: Verify §4.5.4 is non-empty**

Same fallback as C4 Step 6.

- [ ] **Step 7: Append progress-log entry**

```
- 2026-04-30 — C5 — DOC deep-read complete: §4.5.3 (<F> forms), §4.5.4 (<R> rules), §4.5.5 (<P> print templates), §8 (+<Q> questions)
```

### Task C6: Pass 2 — PAY (Payments), DEEP READ — split into 6a, 6b, 6c

PAY is the largest deep-read module. Three sub-tasks.

#### Task C6a: PAY — PaymentDocument + Rata + Dogovor (the core payment flow)

**Files:**
- Read: `<root>/WinApp/Payment/uxPaymentDocument.vb` + `.Designer.vb`, `uxPaymentDocumentsList.vb`, `dijGarant.vb`; matching CSLA classes (`PaymentDocument`, `PaymentDocumentRata`, `DogovorZaRati`); related tables in §4.6.2.
- Modify: `<audit>` §4.6.3, §4.6.4

- [ ] **Step 1: Designer pass for these forms** (same as C1 Steps 1–3, output to §4.6.3)

- [ ] **Step 2: Logic pass — `uxPaymentDocument.vb` + matching CSLA classes**

Look specifically for:
- Validation rules on payment amounts (precision, sign, min/max).
- Computation of `PaidDate`, `Status`, `Total`, `Balance`.
- Status transition: e.g. `Open → Partial → Paid`. Each transition becomes a rule.
- Authorization checks (who can void, who can refund).
- Interaction with `DogovorZaRati`: when an installment contract is created, what gets pre-populated, how the schedule is generated.
- Interaction with `Garant` (guarantor): when required, how validated.

Write `BR-PAY-1xx` rules.

- [ ] **Step 3: Logic pass — installment math**

If the installment-schedule computation lives in code, transcribe the formula as a rule (`BR-PAY-150`-ish range). Cite source line. Note any rounding direction (`Math.Round(..., MidpointRounding.<X>)`) and decimal precision.

- [ ] **Step 4: Anything unclear → §8 question**

Same as C4 Step 5. Specifically expect questions about:
- Currency precision and rounding rules.
- What happens to a `DogovorZaRati` when a vehicle is sold mid-installments.
- Late-fee calculation (event-driven? on-demand? batch?).

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — C6a — PAY core flow deep-read complete: §4.6.3 (<F> forms), §4.6.4 (<R1> rules in BR-PAY-1xx range), §8 (+<Q> questions)
```

#### Task C6b: PAY — Items, Calculation, DDV, PriceCatalog

**Files:**
- Read: `<root>/WinApp/Payment/uxCalculationItems.vb`, `uxCaclulationPivot.vb`, `uxDDVCatalog.vb`; matching CSLA classes (`PaymentItem`, `PaymentItemParametars`, `CalculationItem`, `DDVCatalog`, `PriceCatalog`).
- Modify: `<audit>` §4.6.3, §4.6.4

- [ ] **Step 1: Designer pass** (same as before, output to §4.6.3)

- [ ] **Step 2: Logic pass — VAT (DDV) math**

Specifically:
- VAT rate lookup (which column in `DDVCatalog` is the rate, how it's date-effective).
- VAT inclusion vs exclusion (is `Iznos` net or gross?).
- Per-line vs per-document VAT accumulation.
- Rounding: per line or per total.

Write `BR-PAY-2xx` rules.

- [ ] **Step 3: Logic pass — price catalog → calculation flow**

How a price-catalog entry becomes a calculation item, how a calculation item becomes a payment item. Each step a `BR-PAY-2xx`.

- [ ] **Step 4: Anything unclear → §8 question** (same as C4 Step 5).

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — C6b — PAY items/calc/DDV deep-read complete: §4.6.4 (+<R2> rules in BR-PAY-2xx range), §8 (+<Q> questions)
```

#### Task C6c: PAY — Reports (Faktura, Kasov izvestaj, payment-document variants), and aggregate views

**Files:**
- Read: `<root>/WinApp/Payment/rptFaktura.vb`, `rptInvoiceCompact.vb`, `rptKasovIzvestaj.vb`, all `rptPaymentDocument*.vb` variants, `uxFakturiList.vb`, `uxFinancialStateCreatePaymentDocuments.vb`, `uxUnpayedDealsList.vb`, `uxReportByParametarsFromToForPayment.vb`, `uxReportByPaymentCategory.vb`, `uxPaymentPivotReport.vb`, `uxPaymentPivotShort.vb`.
- Modify: `<audit>` §4.6.3, §4.6.5 (print templates), §4.6.4

- [ ] **Step 1: Designer pass for all `rpt*` and `ux*` listed**

Output to §4.6.3.

- [ ] **Step 2: Logic pass — report templates**

For each `rpt*` file: same procedure as C4 Step 4. Output goes to §4.6.5 with one entry per report:
- `Faktura` (Macedonian invoice)
- `Invoice compact`
- `Kasov izvestaj` (cash report) — note: this is a daily summary, likely with regulatory content.
- `Payment document by ID` (and Compact / Compact Double variants)
- `Payment document Dogovor` (installment-contract receipt)
- `Payment document Rata by ID` (Compact / Compact Double variants)

- [ ] **Step 3: Logic pass — financial-state computations**

`uxFinancialStateCreatePaymentDocuments.vb` is suspicious: it implies `CustomerFinancialState` is recomputed by user action. Read the full file. Capture:
- When is `CustomerFinancialState` recomputed?
- What rows is it derived from?
- Is there a stored procedure or trigger that does the same thing? (Cross-check Task B3 trigger inventory.)

Write `BR-PAY-3xx` rules.

- [ ] **Step 4: Logic pass — aggregation reports**

For `uxReportByParametarsFromToForPayment`, `uxReportByPaymentCategory`, `uxPaymentPivotReport`, `uxPaymentPivotShort`, `uxUnpayedDealsList`:
- Capture filter parameters and output columns.
- Cite the SQL or LINQ that drives each.

Output: `BR-PAY-4xx` rules.

- [ ] **Step 5: Anything unclear → §8 question**

- [ ] **Step 6: Append progress-log entry**

```
- 2026-04-30 — C6c — PAY reports deep-read complete: §4.6.4 (+<R3> rules in BR-PAY-3xx and 4xx ranges), §4.6.5 (<P> print templates), §8 (+<Q> questions)
```

### Task C7: Pass 2 — RPT (Reports & dashboard), DEEP READ

**Files:**
- Read: `<root>/WinApp/uxDashboard.vb` + Designer, `<root>/WinApp/PivotReports/uxPivotReportCustomerVehicle.vb`, `<root>/WinApp/uxPrint.vb`, `<root>/WinApp/uxSplash.vb`, `<root>/WinApp/dijTest.vb`; matching CSLA classes if any.
- Modify: `<audit>` §4.7.3, §4.7.4

- [ ] **Step 1: Designer pass** (same as C1 Steps 1–3, output to §4.7.3)

- [ ] **Step 2: Logic pass — uxDashboard**

`uxDashboard.vb` is 26 KB — non-trivial. Read fully. Capture:
- Widget list (what metrics show on the dashboard).
- Data source per widget.
- Refresh policy (on-load? polling? on-demand?).
- Drill-through behavior.

Write `BR-RPT-NNN` rules.

- [ ] **Step 3: Logic pass — pivot reports**

Same procedure for `uxPivotReportCustomerVehicle.vb`.

- [ ] **Step 4: Logic pass — `uxPrint.vb`, `uxSplash.vb`, `dijTest.vb`**

These look like infrastructure / test forms; capture briefly and put any infrastructure-related findings into §4.10 instead.

- [ ] **Step 5: Anything unclear → §8 question**

- [ ] **Step 6: Append progress-log entry**

```
- 2026-04-30 — C7 — RPT deep-read complete: §4.7.3 (<F> forms), §4.7.4 (<R> rules), §8 (+<Q> questions)
```

### Task C8: Pass 2 — SEC (Operators & security), DEEP READ

**Files:**
- Read: `<root>/WinApp/Operators/dijOperators.vb` + Designer, `dijChangeUserNameAndPass.vb`, `uxCompany.vb`, `<root>/WinApp/Privileges/dijFildsPrivilege.vb`, `dijRools.vb`, `uxCSLAObjects.vb`, `uxPrivileges.vb`, plus matching CSLA classes from C0 index (likely `User`, `Role`, `FieldPrivilege`, `ObjectPrivilege`).
- Modify: `<audit>` §4.8.3, §4.8.4

- [ ] **Step 1: Designer pass** (output to §4.8.3)

- [ ] **Step 2: Logic pass — privilege evaluation**

`uxPrivileges.vb` and `dijFildsPrivilege.vb` are the privilege-management UI; the *enforcement* lives in CSLA `AuthorizationRules`. Read both UI forms and the underlying `User` / `Role` / `FieldPrivilege` / `ObjectPrivilege` CSLA classes.

Capture:
- How a privilege is *defined* (per role? per user? per object? per field?).
- Precedence order when both role and user privileges exist.
- How the *enforcement* works at runtime (called from `BusinessBase.AuthorizationRules`? per-form?).
- How privileges interact with `CSLAObjects` table.

Write `BR-SEC-NNN` rules.

- [ ] **Step 3: Logic pass — login + password change**

`dijChangeUserNameAndPass.vb` shows the password change flow. Capture:
- Password storage (hash? plaintext? encrypted with the same `Crypt` class?).
- Password rules (length, complexity).
- How a user authenticates against `emSecurity.Users`.

This intersects with INF (Login.vb), but the password rule lives in SEC. Cross-reference.

- [ ] **Step 4: Anything unclear → §8 question**

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — C8 — SEC deep-read complete: §4.8.3 (<F> forms), §4.8.4 (<R> rules), §8 (+<Q> questions)
```

### Task C9: Pass 2 — ATT (Attachments & scanning)

**Files:**
- Read: `<root>/WinApp/Twain/*` (all `.vb` and `.Designer.vb`); related CSLA classes; the `DocumentAttachments` and `AttachmentTypes` tables already in §4.9.2 and §4.1.2.
- Modify: `<audit>` §4.9.3, §4.9.4

- [ ] **Step 1: Designer pass for Twain folder** (output to §4.9.3)

- [ ] **Step 2: Logic pass — TWAIN flow**

Capture:
- What scanner library / API is called.
- File format produced by scanning (TIFF? PDF? JPEG?).
- How the scanned file lands in `DocumentAttachments` (file content stored as VARBINARY in the table? as a path?).
- What attachment types exist in the lookup.

Per the rewrite decision, TWAIN is replaced by browser file upload — so this section is mostly historical. Note that explicitly at the top of §4.9.

Write `BR-ATT-NNN` rules.

- [ ] **Step 3: Per-station scanner inventory placeholder**

Add a sub-section in §4.9 titled "Per-station scanner inventory" with this content:

```markdown
##### Per-station scanner inventory

Filled during the rewrite's Attachments sub-project planning, NOT during this audit.
Source: stakeholder will collect from each station: scanner make/model, drivers used,
DPI setting, file format. Decision (browser upload only vs native helper) deferred.
```

- [ ] **Step 4: Anything unclear → §8 question**

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — C9 — ATT deep-read complete: §4.9.3 (<F> forms), §4.9.4 (<R> rules), §8 (+<Q> questions)
```

### Task C10: Pass 2 — INF (Infrastructure), DEEP READ

**Files:**
- Read: `<root>/WinApp/MainForm.vb` (90 KB — long, deep read), `MainForm.Designer.vb` (78 KB), `Login.vb` + Designer, `Splash.vb`, `Installer.vb` + Designer, `MainModul.vb` (already read), `RijndaelCryptography.vb`, plus the `Crypt` class wherever it lives, plus `<root>/WinApp/dijFormi/*`, `<root>/WinApp/Hepers/*`.
- Modify: `<audit>` §4.10.3, §4.10.4

- [ ] **Step 1: Designer pass** (output to §4.10.3)

- [ ] **Step 2: Logic pass — Login flow**

Read `Login.vb`. Capture: where credentials are validated, what culture the app picks at login, what happens on auth failure, where the WCF data portal is bootstrapped.

Write `BR-INF-1xx` rules.

- [ ] **Step 3: Logic pass — connection-string encryption**

Read `RijndaelCryptography.vb` and find the `Crypt` class (Grep: pattern `Public Class Crypt` or `Class Crypt`, path `<root>/WinApp/`, output_mode `files_with_matches`).

Capture:
- Cipher (Rijndael / AES?), mode (CBC?), key derivation (passphrase + salt? PBKDF2?).
- Where the key/salt comes from (compiled-in constant? per-config? per-station?).
- Whether the same `Crypt` is used for password storage in `emSecurity.Users` (cross-reference C8 Step 3).
- Whether key material is recoverable from the source code alone.

This deep-read is the primary mitigation for **R-1** (encryption-key risk).

Write `BR-INF-2xx` rules. Also update R-1 in §7 with what was found ("key material is/isn't recoverable from source code; details in BR-INF-2xx").

- [ ] **Step 4: Logic pass — MainForm shell**

Read `MainForm.vb`. Capture:
- Menu structure (every menu item and the form it opens).
- Privilege checks gating menu items.
- Background tasks started on load (e.g., update-check via `AutoUpdate.vb` — note it's dropped).
- Any `Application.Run(...)` or `ShowDialog` that loads global state used by other forms.

Write `BR-INF-3xx` rules.

- [ ] **Step 5: Logic pass — i18n flow**

`MainModul.vb` Sub `ChangeCulture` is the i18n entry point. Document:
- How `My.Resources.Culture` propagates to forms.
- Where each form's `.resx` per-language strings live.
- Whether mid-session culture switch is supported.

Output: §5.1 (cross-cutting i18n) — *not* §4.10.4. Cross-cutting goes in §5 in Pass 3, but capture findings in a working note here so Pass 3 has them.

- [ ] **Step 6: Anything unclear → §8 question**

- [ ] **Step 7: Append progress-log entry**

```
- 2026-04-30 — C10 — INF deep-read complete: §4.10.3 (<F> forms), §4.10.4 (<R> rules incl. BR-INF-1xx, 2xx, 3xx), §8 (+<Q> questions)
```

### Task C11: Verify all forms in WinApp/ are filed

**Files:**
- Read: `<root>/WinApp/`
- Modify: `<audit>` (move any unfiled form to its module)

- [ ] **Step 1: List every `.vb` file under WinApp/ that is not a Designer file**

Use Glob: pattern `<root>/WinApp/**/*.vb`. Filter out files ending in `.Designer.vb` mentally / programmatically.

- [ ] **Step 2: Cross-check against §4.x.3 sub-sections**

For each form file, Grep `<audit>` for the form name. Any form not found is unfiled.

- [ ] **Step 3: File any unfiled form**

Apply the same heuristics as B5 Step 2 (path-based) and the locked-in form-to-module assignment. If the form does not match any heuristic, file it under INF (4.10.3) and add a §8 question with `Owner: stakeholder`, assumption `INF`.

- [ ] **Step 4: Append progress-log entry**

```
- 2026-04-30 — C11 — verified all <N> WinApp forms filed in §4.x.3 (<X> previously unfiled, now reclassified)
```

### Task C12: Pass 2 stakeholder checkpoint

**Files:**
- Read: `<audit>`
- No modifications

- [ ] **Step 1: Verify Pass 2 acceptance criteria**

- §6.2 #5: every form is in exactly one §4.x.3.
- §6.2 #6: each deep-read module's §4.x.4 is non-empty (or contains the explicit "no rules found" statement with paths).
- §6.3 #7: every public class in `VTE.Library` / `VTE.BaseParts` is in the C0 index with module + table + summary.

- [ ] **Step 2: Produce a checkpoint summary for the stakeholder**

In conversation:
- Per-module form count.
- Per-module business-rule count.
- Per-module print-template count.
- Top 10 open questions by impact (from §8).
- Glossary terms still flagged "needs confirmation".

- [ ] **Step 3: Stakeholder spot-checks 1–2 modules end-to-end**

Stakeholder picks one module (typically PAY or DOC) and walks through §4.x.3 + §4.x.4 + §4.x.5 against the source code with the implementer. If a spot-check fails for a module, redo C4–C10 for that module only.

- [ ] **Step 4: Stakeholder answers the batched §8 open questions**

For each `status: open` question with `Owner: stakeholder`, stakeholder either answers or routes to "ask the operator" (which moves the question into Appendix C).

- [ ] **Step 5: Apply stakeholder answers**

Use Edit tool: for each answered question, flip `status: open` → `status: answered`, fill `Resolution`, and update affected business-rule entries / module sections.

- [ ] **Step 6: Append progress-log entry**

```
- 2026-04-30 — C12 — Pass 2 checkpoint: <pass | fail> — <Q> questions answered, <O> still open, <I> routed to interview script
```

If failed, redo affected modules then re-run C12.

---

## Phase D — Pass 3: Cross-cutting & migration notes

**Goal of Phase D:** §5 cross-cutting filled, every §6 row has a real `Decision` (no TBDs), §7 risks register has at least R-1 through R-5 plus anything found, every §8 question is triaged (`answered`, `decided`, or tagged with target sub-project).

### Task D1: Fill §5 cross-cutting concerns

**Files:**
- Read: `<audit>` (re-read — Pass 3 reads only the audit doc itself, not the legacy code)
- Modify: `<audit>` §5

- [ ] **Step 1: §5.1 i18n**

From C10 Step 5 working notes: how the legacy app does culture switching, where translations live, mk-MK / en-US split, what the rewrite needs to preserve. Note the printed-document policy (Macedonian-only regardless of UI language, per `<spec>` §0).

- [ ] **Step 2: §5.2 Audit log / change tracking**

Grep `<audit>` for any column named `CreatedBy`, `CreatedDate`, `ModifiedBy`, `ModifiedDate`, `LastUpdated`, `RowVersion`, `Timestamp`. Pattern: `(CreatedBy|CreatedDate|ModifiedBy|ModifiedDate|LastUpdated|RowVersion|Timestamp)`, output_mode `files_with_matches` won't help — use `count` or `content`. Summarize: which tables track creator/modifier; is there a separate audit-log table; is row-versioning used.

If no audit-log columns are found anywhere, write: "Not present in legacy. Rewrite implication: introduce a generic audit log."

- [ ] **Step 3: §5.3 Soft-delete / "active" flags**

Grep `<audit>` for column names containing `Active`, `IsActive`, `Deleted`, `IsDeleted`. Summarize.

- [ ] **Step 4: §5.4 Numbering schemes**

For each of: invoice numbers (`Faktura`), request numbers (`Requests`), document numbers (`Documents`, `DocumentsTrafficLicences`, etc.), payment-document numbers — find the column that holds the number, find the BR-PAY/BR-REQ/BR-DOC rule that increments it (Pass 2 captured these).

Document per scheme: format, year-reset?, per-station-reset?, gap-handling.

- [ ] **Step 5: §5.5 Date/time/timezone handling**

Grep `<audit>` for SQL types `datetime`, `datetime2`, `date`, `smalldatetime`. Summarize how dates are stored. Note: WinForms / WCF era usually means local-time `datetime` with no timezone info — this is a real risk for SaaS multi-timezone deployments.

- [ ] **Step 6: §5.6 Money / decimal precision and rounding**

From BR-PAY-2xx (Task C6b) — transcribe the precision and rounding findings here.

- [ ] **Step 7: §5.7 Status-transition pattern**

From BR-REQ, BR-DOC, BR-PAY rules involving status — summarize the legacy pattern (string status column? FK to a status table? per-document status table like `DocumentsTehnicalExamsReportsDetailsStatus`?).

- [ ] **Step 8: Append progress-log entry**

```
- 2026-04-30 — D1 — §5 cross-cutting filled (subsections 1–7)
```

### Task D2: Fill §6 Migration master table decisions

**Files:**
- Modify: `<audit>` §6

- [ ] **Step 1: For each row in §6, decide `keep-rename | split | merge | drop | new`**

Apply these heuristics in order:

1. **drop** — table is empty in legacy, or contains only test data, or is referenced by zero forms and zero CSLA classes.
2. **split** — table mixes two concerns (e.g. a customer table that also has bank-account columns). Decompose into multiple target tables.
3. **merge** — the table is a near-duplicate of another table (e.g. both `Customers.ContactPersons` and a separate contact-persons table). Merge into one target.
4. **new** — the legacy table maps to a target table that didn't exist in legacy (rare during audit; this is mostly a Pass-3 design hint).
5. **keep-rename** — default. The table maps to one target with a clean English name and a `StationId` column added.

For every decision, fill the `Decision`, `Target table`, `Transform`, and `Notes` columns. Notes column always includes "+ StationId" if the target is multi-tenant.

- [ ] **Step 2: Verify no row has `Decision = TBD`**

Grep `<audit>` §6 for `TBD` literal. Expected: zero matches.

If matches remain, decide them now or convert to a §8 question with assumption = "`keep-rename` with English name TBD" and tagged for the new-schema-design sub-project (the very next sub-project after the audit).

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — D2 — §6 migration master table fully decided: <K> keep-rename, <S> split, <M> merge, <D> drop, <N> new
```

### Task D3: Update §7 risks register

**Files:**
- Modify: `<audit>` §7

- [ ] **Step 1: Update R-1 with encryption findings**

From C10 Step 3 findings, update R-1's Mitigation column with concrete language, e.g. "Key material is compiled into `Crypt.vb` constants — recoverable from source. Migration approach: decrypt each station's `app.config` programmatically using these constants." OR "Key material is per-station and not in source — migration requires per-station credentials."

- [ ] **Step 2: Add any new risks discovered**

For any §8 question marked `decided` with a non-trivial impact, evaluate whether it should also be a risk row. If yes, add it as R-6 onwards.

Common new risks during the VTE audit are likely:
- Stored procedure or trigger volume (if Pass 1 found many SPs/triggers, the rewrite either ports them or replaces them — both are work).
- Specific report layout fidelity (if any printed sample shows complex layout that the audit couldn't capture from Designer files alone).
- Date/timezone implications for multi-timezone SaaS.

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — D3 — §7 risks register updated: <N> total risks (<X> added in Pass 3)
```

### Task D4: Triage every §8 open question

**Files:**
- Modify: `<audit>` §8

- [ ] **Step 1: Re-read every §8 row**

For each `status: open` row, decide:

- If it can be answered now from already-collected audit content → flip to `status: answered`, fill `Resolution`.
- If it must be answered to design the next sub-project (new schema design) → tag in `Notes` column: `BLOCKS: schema-design`. These cannot stay open — they go to the stakeholder for answer at Task D5.
- If it's needed only by a later sub-project → tag in `Notes` column: `TARGETS: <sub-project name>`. Can stay `status: open`.
- If it can never be answered without an interview → move the question text to Appendix C and flip to `status: decided` with a recorded assumption.

- [ ] **Step 2: Verify no `BLOCKS: schema-design` questions are still `status: open` after the stakeholder pass**

If any remain after D5 below, the audit cannot be marked complete (per `<spec>` §6.5 #13). Loop back to D5 with a focused list.

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — D4 — §8 triage complete: <A> answered, <D> decided, <O> still open (all tagged TARGETS: <sub-project>; zero BLOCKS: schema-design open)
```

### Task D5: Pass 3 stakeholder checkpoint

**Files:**
- Read: `<audit>`
- No modifications

- [ ] **Step 1: Verify Pass 3 acceptance criteria**

- §6.4 #8: §5 has non-empty entries for i18n, audit log, soft-delete, numbering schemes, dates/timezones, money/precision, status-transition pattern. "Not present in legacy" is valid where applicable.
- §6.4 #9: §6 has a row-count column on every row.
- §6.4 #10: §7 has at least R-1 through R-5 plus anything found.
- §6.5 #13: zero `status: open` rows that are tagged `BLOCKS: schema-design`.

- [ ] **Step 2: Produce a final checkpoint summary for the stakeholder**

In conversation:
- §5 sub-section summary (one sentence per concern).
- §6 decision distribution (counts of each decision type).
- §7 final risk list.
- §8 final triage: counts of answered / decided / still-open-but-deferred.
- Any glossary terms still "needs confirmation".

- [ ] **Step 3: Stakeholder full-document review**

Stakeholder reads the audit doc end to end. Stakeholder may request changes anywhere. Apply changes via Edit; do NOT loop back to earlier passes unless a fundamental error is found — Pass-3 changes are line-edits, additions, and corrections, not module rewrites.

- [ ] **Step 4: Stakeholder signs off the §2 glossary**

For each "needs confirmation" entry, stakeholder confirms or corrects. Update each row's `Confirmed?` to `yes` or to the corrected English term.

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — D5 — Pass 3 checkpoint: <pass | fail> — stakeholder review applied, <G> glossary terms confirmed
```

---

## Phase E — Finalization

### Task E1: Verify all 14 §6 acceptance criteria simultaneously

**Files:**
- Read: `<audit>`
- Modify: `<audit>` (only to fix gaps)

- [ ] **Step 1: Walk each criterion**

For each of `<spec>` §6.1 #1–4, §6.2 #5–6, §6.3 #7, §6.4 #8–10, §6.5 #11–14: produce a yes/no checkmark with a quoted snippet of the audit doc as evidence.

- [ ] **Step 2: For any "no", fix inline**

Acceptable fixes only — do not rewrite passes. If a fix requires re-reading legacy code, the audit was incomplete and the relevant Phase-C task is reopened.

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — E1 — all 14 §6 criteria verified pass
```

### Task E2: Render check

**Files:**
- Read: `<audit>`

- [ ] **Step 1: Use Read to read the full document**

Read in chunks if needed (the doc may be 80–150 pages of Markdown). Watch for: orphan headings, unclosed code fences, broken table syntax, ToC link mismatches.

- [ ] **Step 2: Verify code fences are paired**

Use Grep on `<audit>` for ` ``` ` (three backticks) — `pattern` is the literal three backticks, `output_mode: count`. The result should be even.

- [ ] **Step 3: Verify Markdown headings are well-nested**

Use Grep on `<audit>` for `^#+` (heading lines), output_mode `content`, head_limit 0. Walk the heading list visually and confirm: no `###` immediately under `#`, no `#####` immediately under `###`, no orphan numbered sub-sections (e.g. `4.6.5` without a `4.6` parent).

- [ ] **Step 4: Fix any rendering issues inline**

- [ ] **Step 5: Append progress-log entry**

```
- 2026-04-30 — E2 — render check pass: <C> code fences (paired), <H> headings (well-nested)
```

### Task E3: Final stakeholder handoff

**Files:**
- Modify: `<audit>` (status field at the top)

- [ ] **Step 1: Update audit-doc status header**

Use Edit tool on the line `**Status:** in progress (Pass 1 not started)` near the top of `<audit>`, replacing it with:

```
**Status:** complete; ready to feed sub-project (2) New schema design.
**Completed:** 2026-MM-DD
```

(Use the actual date.)

- [ ] **Step 2: Notify stakeholder**

In conversation: "Audit complete. Document at `<audit>`. Open questions tagged `TARGETS:` are routed to their respective sub-projects. Next sub-project (new schema design) can begin its own brainstorm."

- [ ] **Step 3: Append progress-log entry**

```
- 2026-04-30 — E3 — audit complete; handed off to schema-design sub-project
```

---

## Self-review of this plan against the spec

(Performed during plan authoring. Documented here for traceability.)

**Spec coverage checklist:**

| Spec section | Plan task(s) covering it |
|---|---|
| §1 Goal — produce single living audit doc | A1, all subsequent tasks write into it |
| §2.1 In scope | covered by B1–B4 (SQL), C1–C11 (forms + CSLA), C10 (config + crypto) |
| §2.2 Out of scope | A1 Step 3 pre-populates Appendix A with the excluded-paths list |
| §2.3 Audit depth | C1–C3 are Designer-only; C4–C10 are deep-read; mapping verified |
| §3.1 Single artifact | A1 Step 3 writes the skeleton at the spec-named path |
| §3.2 Fixed ToC | A1 Step 3 reproduces the ToC verbatim |
| §3.3 Conventions (sql blocks, screen tables, BR IDs, source citations) | B2 Step 2, C1 Step 2, C4 Step 2, etc. all use the conventions |
| §3.4 Companion folder + redaction policy | A1 Step 1–2 creates folder + README with policy |
| §4 Method (3 passes) | Phase B = Pass 1; Phase C = Pass 2; Phase D = Pass 3 |
| §4.4 Working rules | encoded into per-task steps (cite source, never block on questions, etc.) |
| §4.6 Three checkpoints | Tasks B6, C12, D5 |
| §5 Open-questions handling | every Phase-C task has a "→ §8 question" step; D4 triages |
| §6 Acceptance criteria | E1 verifies all 14 |
| §7 Risks | A1 Step 3 pre-populates R-1 through R-5; D3 updates and adds |
| §8 Stakeholder asks | A1 Step 2 README documents asks; B6, C12, D5 are checkpoints |
| §9.1 Deliverable | E3 finalizes |
| §9.2 Transition | E3 hands off |
| §9.3 Sub-project chain | the audit feeds (2) schema-design — the only sub-project that needs to be unblocked here |

**Type-consistency check:** module IDs (`REF` / `CUS` / `VEH` / `REQ` / `DOC` / `PAY` / `RPT` / `SEC` / `ATT` / `INF`) used identically in the assignment tables, the BR-ID convention, the audit doc skeleton, and every per-module task. Decision verb (`keep-rename | split | merge | drop | new`) used identically in spec §3.2/§4.3/§6.1, the audit-doc §6 skeleton, and Task D2.

**Placeholder scan:** every step contains the actual content. Where steps loop over many tables/forms, the procedure is fully described and the assignment tables at the top of the plan supply the inventory. The literal string "TBD" appears only in places where TBD is correct *content* (e.g. `target table = TBD` as the Pass-1 intermediate state, removed in Task D2).
