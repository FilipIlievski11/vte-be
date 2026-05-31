# VTE Domain & Schema Audit — Design Specification

**Date:** 2026-04-30
**Sub-project:** Phase 0 / Step 1 of the VTE rewrite
**Status:** Approved by stakeholder; ready for implementation-plan authoring

---

## 0. Context

The legacy VTE system is a Vehicle Technical Examination station management application: VB.NET WinForms on .NET 4.0, built ~2012, using the CSLA framework over a WCF data portal at `sk.bransys.com`. Bilingual UI (Macedonian `mk-MK` default, English `en-US` secondary). Two SQL Server databases (`VTE` main + `emSecurity` security). Hardware-locked licensing, TWAIN scanner integration, auto-updater, Macedonian Cyrillic print templates including legally-formatted blue/white/green request forms.

The agreed rewrite target is:
- Vue 3 frontend (Vite, mk/en i18n).
- .NET 9 backend.
- New multi-tenant database (one shared DB; every domain table carries a `StationId`).
- Migration script that loads each existing per-station legacy DB as one tenant row in the new DB.
- Hardware-locked licensing dropped. Auto-updater dropped. POS/government e-services not in scope.
- Receipt/fiscal printing replaced by browser PDF download to a standard printer.
- TWAIN scanning: per-station investigation, decision deferred until the Attachments sub-project.

The chosen migration strategy is **audit-first**: produce a complete domain document for the legacy system *before* any new design or implementation work begins. This document specifies the audit sub-project only — not the rewrite. Each later sub-project (auth, customers, vehicles, requests, documents, payments, reports, attachments, migration) will be a separate brainstorm → spec → plan → implementation cycle that consumes this audit's output.

---

## 1. Goal

Produce a single living document, `docs/superpowers/specs/2026-04-30-vte-domain-audit.md`, that fully describes the legacy VTE system as it exists today: tables, screens, business rules, print templates, security model, integrations.

After acceptance, this document is the single source of truth for every later sub-project. Future work reads the audit, not the legacy code.

---

## 2. Scope

### 2.1 In scope

- The `WinApp` VB.NET project: every form's `.vb` and `.Designer.vb`, every `.resx` (mk-MK and en-US).
- The `VTE.Library` and `VTE.BaseParts` projects: CSLA business objects (where most business rules live).
- `WinApp/sqlData.sql` (~27k lines, main DB).
- `WinApp/emSecurity.sql` (~2.3k lines, security DB).
- `WinApp/Uninstall.sql` (scanned for references only).
- `WinApp/izmeniBaza/` and `WinApp/importData/` (scanned for business rules embedded as SQL).
- `WinApp/app.config` and the `RijndaelCryptography` / `Crypt` connection-string encryption flow.
- `MainModul.vb`, `MainForm.vb`, `Login.vb`, `Splash.vb`, `Installer.vb`.
- Print templates under `WinApp/Stampa/`, `WinApp/Requests/` (`PrintPlav`, `printBel`, `printZelen`), and `WinApp/Payment/` (Faktura, payment-document variants, KasovIzvestaj).

### 2.2 Out of scope

The audit will list each excluded path in Appendix A with a one-line reason. Excluded:

- `KeyGen/`, `TakehardwareInfo/` — hardware-licensing projects; rewrite drops licensing.
- `SetupObicen/` — WinForms installer; web app does not need it.
- `TestWpf/` — experimental project, not production.
- `Backup/`, `Backup1/` — code archives.
- `_ReSharper.VTE/`, `_UpgradeReport_Files/`, `packages/`, `tools/`, `.vs/`, `WinApp/bin/`, `WinApp/obj/`, all `UpgradeLog*` files.
- `WinApp/AutoUpdate.vb` — auto-updater; rewrite drops it.

The audit also explicitly does *not* design the new schema, the new API, the new UI, or the migration script. Those are later sub-projects. Where the audit notices a target-system implication, it goes into the per-table *Migration notes* sub-section as a hint, not into the schema description as fact.

### 2.3 Audit depth

Schema-first across all data sources. Screens are read at Designer-level (field list, layout, labels) for every form. **Deep read** — meaning the form's `.vb` plus the matching CSLA business objects in `VTE.Library` / `VTE.BaseParts`, with full extraction of validation, calculations, status transitions, and authorization checks — is performed only on:

- Payments and installments: `PaymentDocuments`, `PaymentDocumentsDetails`, `PaymentDocumentsRata`, `DogovorZaRati`, `PaymentItems`, `PaymentItemParametars`, `CalculationItems`, `DDVCatalog`, `PriceCatalog`, `CustomerFinancialState`, Faktura, KasovIzvestaj, Garant.
- Technical Exam Reports: `DocumentsTehnicalExamsReports`, `DocumentsTehnicalExamsReportsDetails`, `DocumentsTehnicalExamsReportsDetailsStatus`, `TehnicalExamVehicleParts`, `TehnicalExamVehiclePartsCategories`, `TehnicalExamsTypes`, `TehnicalExamOrganizations`.
- Requests and the three colored print templates: `Requests`, `RequestTypes`, `Request.VehicleOwnershipProofs`, `Request.PaymentProof`, `PrintPlav.vb`, `printBel.vb`, `printZelen.vb`.
- Documents: Traffic Licences (with Extensions), Permissions, International Driving Licences (with Valid Categories), the Documents and DocumentTypes/DocumentTypePrint/DocumentTypesOptions tables.
- Login, MainForm, Dashboard.
- Privileges engine: the `emSecurity` database in its entirety, plus `dijFildsPrivilege.vb`, `dijRools.vb`, `uxCSLAObjects.vb`, `uxPrivileges.vb`.
- Connection-string encryption flow: `MainModul.SetupConnectionStrings`, `RijndaelCryptography.vb`, the `Crypt` class.

Everything else is Designer-level only.

---

## 3. Output

### 3.1 Single artifact

`docs/superpowers/specs/2026-04-30-vte-domain-audit.md`. Markdown. Estimated 80–150 pages when complete.

### 3.2 Fixed table of contents

Module sections share a single sub-heading template so the document stays diff-able and so future sub-project specs can cite stable anchors.

```
1. Executive summary                      (~1 page)
   - What VTE is, who uses it, what it produces
   - Headline numbers (# tables, # screens, # business objects, # languages)
   - Top 5 risks for the rewrite

2. Glossary (Macedonian → English)
   - Domain terms (Faktura → Invoice; DDV → VAT; Dogovor za Rati → Installment contract;
     Plav/Bel/Zelen → Blue/White/Green request form; Garant → Guarantor; etc.)
   - File/folder naming conventions (ux*, dij*, rpt*, print*)

3. Tenancy & auth model — TARGET
   - Confirms multi-tenant SaaS: every domain table gets StationId
   - Notes how this differs from legacy (one DB per station + emSecurity DB)
   - Migration implication: each existing station DB → one StationId

4. Per-module sections (× 10), each with the same sub-headings:
     4.x.1 Purpose                  (1 paragraph)
     4.x.2 Tables                   (every column: name, type, null, default, FK, index)
     4.x.3 Screens                  (form name → tables touched → fields → actions)
     4.x.4 Business rules           (validation, calc, status transitions; deep-read modules only)
     4.x.5 Print templates          (where applicable; field map, static text, paper size)
     4.x.6 Open questions           (numbered, link to register in §8)
     4.x.7 Migration notes          (per table: keep-rename / split / merge / drop / new)

   Module list (in dependency order — same order future sub-projects will ship in):
     4.1  Reference data
     4.2  Customers
     4.3  Vehicles
     4.4  Requests
     4.5  Documents
     4.6  Payments
     4.7  Reports & dashboard
     4.8  Operators & security
     4.9  Attachments & scanning
     4.10 Infrastructure (what we are explicitly NOT porting, plus connection-string
                          encryption, WCF data portal, i18n, MainForm shell, Login flow)

5. Cross-cutting concerns
   - i18n
   - Audit log / change tracking
   - Soft-delete / "active" flags
   - Numbering schemes (invoice numbers, request numbers, document numbers)
   - Date/time/timezone handling
   - Money/decimal precision and rounding rules

6. Migration master table
   - One row per legacy table:
     legacy name | row count expected | target table | transform | notes

7. Risks register
   - Numbered: description, likelihood, impact, mitigation

8. Open questions register
   - Numbered, cross-referenced from module sections
   - Each entry: question, source file:line, owner, working assumption, impact, resolution

Appendix A. Files NOT read (with one-line reason each)
Appendix B. Macedonian print-form facsimiles (links to images in audit-inputs/)
Appendix C. Interview script for station operators (deferred; populated as needed)
```

### 3.3 Conventions inside the document

- Every table appears as a fenced `sql` block with all columns; no prose-only descriptions of schema.
- Every screen is described as a Markdown table: *Field → Source table.column → Editable? → Validation*.
- Every business rule is written as a numbered, testable statement: `BR-PAY-014: when an installment is fully paid, PaymentDocumentsRata.PaidDate is set and Requests.Status becomes ...`. The numbers are stable IDs that the migration script and new-API specs will cite.
- Every business rule cites its source: `VTE.Library/Payments/PaymentDocument.vb:412` or `sqlData.sql:10732`.
- Every uncertainty becomes an entry in §8 with an explicit working assumption — no question blocks reading.
- Macedonian preserved verbatim on first appearance with English in parens; thereafter English (terms tracked in §2).

### 3.4 Companion folder

`docs/superpowers/specs/audit-inputs/` holds sample materials supplied by the stakeholder during the audit:

- A redacted production `app.config`. Original encrypted connection-string values are replaced with `«REDACTED — see audit-inputs/README»`. The actual decrypted strings (or the raw encrypted file plus key/salt) are kept out-of-git in a location accessible once for verifying the decryption flow.
- Redacted printed samples of each official document (traffic-licence extension, payment receipts, blue/white/green request forms, invoice, cash report) as PDF or image. Personal data redacted.
- A `README.md` explaining what's in the folder and the redaction policy.

Reasoning for this split: paper layouts are not sensitive and committing them in git makes the report-rebuild sub-project massively easier; the production `app.config` is a credential and its raw form does not belong in git.

---

## 4. Method

The audit is performed in three sequential passes over the source material. Each pass writes into the same document; later passes correct earlier ones.

### 4.1 Pass 1 — Schema sweep (broad, mechanical)

Read both SQL files end to end. For every `CREATE TABLE`, `CREATE VIEW`, `CREATE PROCEDURE`, `CREATE TRIGGER`, `CREATE INDEX`, every foreign key, every check constraint, every computed column:

- Append the `CREATE TABLE` block verbatim into the right module's *4.x.2 Tables*.
- Append a one-line summary into §6 "Migration master table" with `target table = TBD`, `transform = TBD`.
- Macedonian column names → English glossary entry in §2.
- Tables that can't be classified yet go into a "Module: TBD" bucket and are reclassified at the end of Pass 1, with a note explaining the call.

**Output of Pass 1.** The document has every table in its right place, every column visible, no business rules yet.

### 4.2 Pass 2 — Screens and business objects (the logic)

For each module in dependency order (reference data → customers → vehicles → requests → documents → payments → reports → security → attachments → infrastructure), two interleaved sub-passes:

- **Designer pass:** read every form's `*.Designer.vb` to extract field list, layout grouping, label text. Fills *4.x.3 Screens*. Mechanical.
- **Logic pass (deep-read modules only):** read the form's `.vb` AND the matching CSLA business objects in `VTE.Library` / `VTE.BaseParts`. Extract validation rules, calculations, status transitions, authorization checks. Each rule becomes a numbered `BR-<MODULE>-NNN` line in *4.x.4 Business rules*. Anything unclear → §8 with an explicit working assumption.

**Output of Pass 2.** Every screen mapped, every deep-read module has a numbered business-rules list, open questions captured.

### 4.3 Pass 3 — Cross-cutting and migration notes

Re-read what the audit document already contains (not the source code), looking specifically for cross-cutting patterns and tenant implications:

- i18n, soft-delete flags, numbering schemes, money precision, audit columns → fill §5.
- For each table in §6, decide one of: `keep-rename | split | merge | drop | new`. Add tenant-column notes (`StationId NOT NULL`).
- Walk §8 open-questions one more time; tag each with a target sub-project (so the relevant question gets answered when we get to it, not before).

**Output of Pass 3.** Cross-cutting section complete, migration master table has a decision per row, risks register filled.

### 4.4 Working rules

1. **Never block on a question.** Every uncertainty gets logged in §8 with a stated working assumption.
2. **No design decisions for the new system.** Target-system implications belong only in *4.x.7 Migration notes* as hints.
3. **Quote the source.** Business rules cite `file:line` or `table.column`. Auditable, not folklore.
4. **Mechanical first, interpretive second.** Pass 1 is transcription; Pass 2 is reading. Splitting them prevents premature interpretation.
5. **Macedonian preserved verbatim** on first appearance; thereafter English with §2 entry.

### 4.5 Tooling

Read-only operations: grep for `CREATE TABLE`, `CREATE PROCEDURE`, `CREATE VIEW`, `CREATE TRIGGER`, `^FOREIGN KEY`, `^ALTER TABLE`. Glob the WinApp tree by folder. Read CSLA business objects from `VTE.Library` / `VTE.BaseParts`. No code execution. No DB connection.

### 4.6 Checkpoints with the stakeholder

Three:

- **End of Pass 1** — schema dump complete; stakeholder spot-checks that no major table is misfiled.
- **End of Pass 2** — every module section filled; stakeholder spot-checks 1–2 modules end-to-end against the source.
- **End of Pass 3** — full document review, including the open-questions register, before transitioning to the next sub-project's brainstorm.

If any spot-check fails, the relevant pass is redone for that module before continuing.

---

## 5. Open-questions handling

Where unknowns live, how they flow, what blocks vs what doesn't.

### 5.1 Location and format

§8 of the audit document. One numbered entry per question. Fixed format:

```
Q-NNN  [module]  status: open | answered | decided
  Question:    one-sentence question
  Source:      file:line that triggered the question
  Owner:       who can answer (stakeholder, station operator, inspector, former dev, "unknown")
  Assumption:  the working assumption used to keep moving
  Impact:      what changes if the assumption is wrong
  Resolution:  filled when status flips to answered/decided
```

### 5.2 Lifecycle

1. While auditing, entries are created with `status: open`. Reading does not stop to wait for an answer.
2. At the end of each pass, the new open questions are batched and posted to the stakeholder so multiple can be answered at once.
3. When answered, `status` flips to `answered`, the relevant module section is updated, and the entry stays in the register for traceability.
4. If a question can't be answered before action is required, a *decision* (not an answer) is recorded with `status: decided` and rationale.

### 5.3 Cross-referencing

Every business rule, table note, or migration row that depends on an open question links to the `Q-NNN` anchor. When a question is answered, grep for the anchor and update every affected place.

### 5.4 What goes in the register

In: factual questions about how the legacy system actually behaves (`Customers.IsActive` is soft-delete or current-status; how `CustomerFinancialState` is recomputed; etc.).
Not in: design choices for the new system. Those are deferred to the relevant later sub-project's brainstorm.

### 5.5 Operator interviews

Some questions can only be answered by a station operator, not by the stakeholder and not by reading code (e.g., "what does the inspector do when the brake test fails partway through?"). For those, the audit produces a one-page "Interview script" appendix. Whether and when interviews actually happen is decided with the stakeholder after Pass 2.

---

## 6. Acceptance criteria

The audit is complete when **every one** of these is true.

### 6.1 Coverage — schema

1. Every `CREATE TABLE` in `sqlData.sql` and `emSecurity.sql` appears verbatim in exactly one *4.x.2 Tables* sub-section.
2. Every table appears in §6 "Migration master table" with a decision: `keep-rename | split | merge | drop | new`.
3. Every dropped table has a one-line *why* in §6.
4. Every stored procedure, view, trigger, and check constraint is recorded under its owning table or in a "Procedures" sub-section. None silently skipped.

### 6.2 Coverage — screens

5. Every WinForm `*.vb` under `WinApp/` is listed in exactly one *4.x.3 Screens* sub-section, with its tables-touched mapping. Out-of-scope-project forms are listed in Appendix A with their reason.
6. Each deep-read module has a non-empty *4.x.4 Business rules* sub-section. Empty is not allowed for deep-read modules — if no rules are found, the section says so explicitly with the path read.

### 6.3 Coverage — business objects

7. Every public class in `VTE.Library` and `VTE.BaseParts` is mapped to: (a) its owning module, (b) the table(s) it represents, (c) a one-line summary. Deep-read modules get full rule extraction.

### 6.4 Cross-cutting and migration

8. §5 has a non-empty entry for each of: i18n, audit log, soft-delete, numbering schemes, dates/timezones, money/precision, status-transition pattern. "Not present in legacy" is a valid entry and must say so.
9. §6 has a row-count column. Where no real count is available, the value is `unknown — needs prod sample` and a §8 question records the gap.
10. §7 has at least the five risks listed in §7 of *this* spec, plus anything found during the audit.

### 6.5 Quality bar

11. Every business rule in *4.x.4* has a source citation (`file:line` or `table.column`). No folklore.
12. Every Macedonian term that survives in the doc has an English entry in §2.
13. §8 has zero entries with `status: open` *that would block the next sub-project's brainstorm*. Open questions targeted at later sub-projects can stay open. Triaged, not necessarily resolved.
14. The doc renders cleanly in a Markdown viewer (TOC links work, code fences close, no orphan headings).

### 6.6 Not completion criteria

- New schema design — that is the next sub-project.
- Migration SQL — a later sub-project.
- Resolving every open question — only those that block the *next* brainstorm.
- Reading the out-of-scope projects.

---

## 7. Risks

### R-1 — Encrypted connection strings, no key

The legacy `app.config` keeps both connection strings encrypted via `Crypt` / `RijndaelCryptography`. The repo's `app.config` shows blank fields, so production keys/IVs are not in the codebase being read. Without access to a real station's `app.config` plus the encryption key/salt, the migration sub-project cannot connect to the source DB to extract data.

*Likelihood:* high. *Impact:* blocks migration sub-project, not the audit. *Mitigation:* fully document the encryption scheme during the infrastructure deep-read; the stakeholder supplies a redacted `app.config` plus key material into `audit-inputs/`. If the original key is unrecoverable, the alternative is per-station DB-level credentials supplied by station owners — workable but a real fork that should be known early.

### R-2 — Business rules that live nowhere readable

CSLA puts authorization, validation, and calculations inside `AddBusinessRules`, `AddAuthorizationRules`, and event handlers in derived `BusinessBase` classes. If those rules are smeared across partial classes and form code-behind, the deep-read passes can miss them. Worst case: a privilege check or a rounding rule ships missing into the new system and only surfaces when an inspector hits it.

*Likelihood:* medium-high. *Impact:* incorrect behavior in the rewrite. *Mitigation:* the deep-read passes for high-logic modules grep both `VTE.Library` / `VTE.BaseParts` and the relevant `*.vb` form. Logic outside expected places gets a §8 entry. The 80–150 page audit accepts that rule discovery happens; what's not acceptable is silent loss.

### R-3 — Per-station drift

Multi-tenant SaaS assumes existing stations share the same schema. After 13 years of in-place upgrades, with `izmeniBaza` and `importData` folders in the repo, different stations are almost certainly on slightly different schema versions — extra columns, extra tables, tweaked stored procedures, different defaults.

*Likelihood:* high. *Impact:* migration sub-project must handle per-station schema differences, not the audit. *Mitigation:* audit captures schema *as seen in the repo*. The migration sub-project later runs a schema-diff step against each production DB before extracting data. The audit produces the baseline; the diff tells us per-station what's extra.

### R-4 — Macedonian-only domain knowledge

Form labels, column comments, and printed-document text are Macedonian Cyrillic. Station-specific shorthand, legal references, and abbreviations (`ДДВ`, `Платено`, `Договор за рати`) may be ambiguous out of context. Translations rely on stakeholder confirmation.

*Likelihood:* medium. *Impact:* glossary errors propagate into module sections and into the new-system specs. *Mitigation:* every Macedonian term added to §2 is flagged "needs confirmation" until signed off. Glossary becomes its own short review cycle once Pass 1 finishes.

### R-5 — Print templates as legal artifacts

The blue/white/green request forms, the traffic-licence extension, and the cash report are regulatory documents with required layouts. New PDFs must match exactly (paper size, fixed text, stamp positions). Capturing them precisely from the WinForms designer files alone is harder than it sounds without physical samples.

*Likelihood:* medium. *Impact:* report-rebuild sub-project (later) is the riskiest thing on the project if samples never arrive. *Mitigation:* audit deep-reads report templates' Designer files and records every static string and field placement; stakeholder supplies redacted printed samples into `audit-inputs/`. Risk does not fully retire until samples exist.

### Risks deliberately not flagged here

Frontend framework choice, .NET 9 viability, EF Core performance, data volume / migration runtime, business priority / timeline. Those belong to later sub-projects.

---

## 8. Stakeholder asks

Listed by when they're needed.

1. **Now** — approval of this spec so the audit's implementation plan can be authored.
2. **Before the audit reading begins** (during the writing-plans step or shortly after) — one production `app.config` from any station, redacted into `audit-inputs/`, plus the encryption key/salt/passphrase out-of-git. If unrecoverable, a clear "no" so the migration approach can be replanned.
3. **During the audit (Pass 2 and Pass 3 checkpoints)** — answers to the batched §8 open-questions lists posted at each checkpoint. "I don't know — ask the operator" is a valid answer; those questions route to the interview-script appendix.
4. **No deadline, blocks the report-rebuild sub-project later** — one redacted printed sample of each official document (traffic-licence extension, blue/white/green request forms, payment receipt, invoice, cash report) into `audit-inputs/`.

---

## 9. Deliverable and transition

### 9.1 Deliverable

One file: `docs/superpowers/specs/2026-04-30-vte-domain-audit.md`, conforming to §3 structure and §6 acceptance criteria. Plus `docs/superpowers/specs/audit-inputs/` populated with the materials in §3.4 / §8.

No code, no schema design, no migration SQL.

### 9.2 Transition

Once this spec is approved by the stakeholder:

1. Hand off to `superpowers:writing-plans` to produce a step-by-step implementation plan for the audit (which passes happen in which order, what each pass produces, what the checkpoints look like, exactly which files get read in each pass).
2. Stakeholder approves the plan.
3. Audit reading begins, executing the plan.
4. Three checkpoints with the stakeholder, one per pass.
5. Once §6 acceptance criteria are met, the audit document is final and the next sub-project (new schema design) opens its own brainstorm.

### 9.3 Sub-projects this audit feeds

Each row below is a future brainstorm → spec → plan → implementation cycle. Listed in dependency order so the audit's role is clear.

| #  | Sub-project                                                          | Depends on              |
|----|----------------------------------------------------------------------|-------------------------|
| 1  | VTE Domain & Schema Audit *(this one)*                               | nothing                 |
| 2  | New schema design (target DB, multi-tenant)                          | (1)                     |
| 3  | .NET 9 backend skeleton (solution, EF Core, auth, conventions, CI)   | (2)                     |
| 4  | Vue 3 frontend skeleton (Vite, router, i18n mk/en, auth, design sys) | (3)                     |
| 5  | Operators / roles / privileges (replaces CSLA security)              | (3), (4)                |
| 6  | Reference data CRUD (cities, colors, vehicle bodytypes, etc.)        | (5)                     |
| 7  | Customers                                                            | (6)                     |
| 8  | Vehicles + customer↔vehicle relations                                | (7)                     |
| 9  | Requests + 3 print templates                                         | (8)                     |
| 10 | Documents (Traffic Licences, Permissions, Intl Driving, Tech Exam)   | (9)                     |
| 11 | Payments + installments + invoices + cash report                     | (10)                    |
| 12 | Pivot reports & dashboard                                            | (11)                    |
| 13 | Attachments / scanning                                               | (10)                    |
| 14 | Migration script + cutover plan                                      | (11), (12), (13)        |

No sub-project past (1) is pre-designed here. The audit's job is to make sure they *can* be designed when their turn comes.
