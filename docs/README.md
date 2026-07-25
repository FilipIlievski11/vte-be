# VTE — documentation

**VTE** is a multi-tenant SaaS rewrite (.NET 10 + Vue 3) of a legacy VB.NET WinForms
vehicle technical-inspection system used by an inspection station in Велес, Macedonia.
It manages clients, vehicles, inspection **requests** (барања), **technical exams**
(технички преглед), **pricing & billing** (наплата) with **fiscal receipts**, and
prints onto the pre-printed МВР government forms.

This folder is the full developer + operator reference. Every document is grounded in
the actual source (each was drafted from the code and then fact-checked against it).
For the terse, day-to-day cheat-sheet and the "hot gotchas", see [`../CLAUDE.md`](../CLAUDE.md).

## Start here

New to the codebase? Read **01 → 02 → 03** in order, then jump to whichever module you need.
Running or deploying it? Go straight to **09**. Setting up a station? **11** (+ **06** for the device).

## Contents

| # | Document | What's inside |
|---|----------|---------------|
| 01 | [Overview & architecture](01-overview-and-architecture.md) | What VTE is, the tech stack, solution layout, multi-tenancy (`ITenantOwned` + global query filter), soft-delete, JWT auth, startup/seeding, config, and how to run locally. |
| 02 | [Domain data model](02-data-model.md) | Every entity by module, key relationships, the `ClientVehicleRelation` anchor, the overloaded ids, reference tables, EF mappings & migrations, data scale. |
| 03 | [Requests (Барања)](03-requests.md) | Request types & document forms, the Open→End lifecycle, the anchor + new-owner resolution, ownership-transfer routing, proofs/attachments, End side-effects, auto-tech-exam, the print bundle. |
| 04 | [Technical exams (Технички преглед)](04-technical-exams.md) | Exam reports, regular vs irregular, controllers/operators, lookups, CRUD, the debt hook, and the Записник + certificate prints. |
| 05 | [Payments, pricing & debts (Наплата)](05-payments-and-pricing.md) | The 3-level legacy pricing hierarchy → `PriceCatalog`, `PricingEvaluator`, `DebtService`, `CustomerDebt`, the Наплата panel, bill creation, payment types, and installments. |
| 06 | [Fiscal integration (Accent PF-500)](06-fiscal.md) | The file-exchange fiscal protocol, the exact command-file format, the cash/VAT gates, Z/X reports, and the per-PC operator setup (folder pairing). |
| 07 | [Print system (МВР forms)](07-prints.md) | How the Zelen/Plav/Bel templates position text from the legacy `.prnx`, the print bundle, the reference number, and the new-registration/destination-MVR resolution. |
| 08 | [Frontend architecture](08-frontend.md) | The Vue 3 app: stores, api client, router/guards, i18n (MK+EN), PrimeVue, conventions, and the view-per-module map. |
| 09 | [Deployment & operations](09-deployment-and-operations.md) | The Hetzner/Docker prod stack, the build-release → deploy flow, secrets, the SSH tunnel for local dev against prod, EF auto-migrate, and the DB re-lift procedure. |
| 10 | [Legacy data migration](10-data-migration.md) | Importing real data from the legacy VTEZVV DB: the snapshot, the `migrate/*.sql` scripts, incremental top-up, and the migration gotchas. |
| 11 | [Operator & admin guide](11-operator-guide.md) | Task-oriented daily-use walkthrough (in Macedonian terms): create a request, finish it, print, do an exam, bill, fiscal-print, and admin screens. |
| 12 | [Рачен sync со легаси](12-legacy-sync.md) | How to pull fresh legacy data into prod yourself: the one-click `deploy/run-legacy-sync.ps1`, what the sync does, the manual 3-window fallback, and troubleshooting. |
| 13 | [Рачно качување на прод](13-deploy.md) | How to ship a new version yourself: the one-click `deploy/deploy-to-prod.ps1` (build → upload → docker rebuild → verify), server-side release backups, rollback, and troubleshooting. |

> The older [`DEPLOYMENT.md`](DEPLOYMENT.md) predates this set — **09** is the canonical deployment reference.
