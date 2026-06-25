# Requests (Барања)

A **Request** (Macedonian: *Барање*) is a single operator transaction — a work order — opened at an inspection station. It anchors one **Client + Vehicle** (via a `ClientVehicleRelation`), captures ownership/payment proofs and file attachments, and when *ended* (*Заврши*) fires a set of side-effects driven by flags on its **RequestType**. Each Request maps to one of the three legacy paper forms (Plav / Bel / Zelen) for printing.

This document covers the module end to end: the catalog (`RequestType`, `RequestDocumentPrint`, the proof/attachment type tables), the Open→End lifecycle, the anchor and the two-field FE picker, the "Нов сопственик" new-owner flow, debt routing on ownership transfer, the auto-create-technical-exam hook, the transactional End side-effects, the print bundle, and the list/search behaviour. Every claim is grounded in the v2 source; key files are cited inline.

Related docs: [Payments & pricing](05-payments-and-pricing.md), [Technical exams](04-technical-exams.md), [Data model](02-data-model.md) (Clients / Vehicles / `ClientVehicleRelation`), [Overview & architecture](01-overview-and-architecture.md), [Prints](07-prints.md).

---

## 1. Data model

### 1.1 `Request` entity

Source: `backend-v2/src/VTE.Domain/Requests/Request.cs`. Table `Request` (EF config: `backend-v2/src/VTE.Infrastructure/Persistence/VteDbContext.cs:423-458`). Implements `ITenantOwned` (carries `CompanyId`), so a global EF query filter scopes every read/write to the caller's company:

```csharp
e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
```

| Field | Type | Notes |
|---|---|---|
| `Id` | `long` (bigint, identity) | The **# number** shown in the list. Migrated rows preserve the legacy `Requests.Id`. |
| `CompanyId` | `byte` | Tenant. Locked after creation. |
| `RequestTypeId` | `byte` | FK → `RequestType` (`OnDelete: Restrict`). |
| `ClientVehicleRelationId` | `long` | **The anchor** (required, `BR-REQ-004`). FK → `ClientVehicleRelation`. |
| `NewClientVehicleRelationId` | `long?` | Required when `RequestType.TransfersOwnership`. **Overloaded** — see §4. |
| `TechnicalExamReportId` | `long?` | Optional link to a technical exam. |
| `PreviousRegistrationId` | `long?` | FK → `VehicleRegistration` (the prior registration on a re-registration/transfer). |
| `CreatedAt` / `ModifiedAt` / `EndedAt` | `DateTime` / `DateTime?` / `DateTime?` | **Status is derived from these** (see §2). All UTC. |
| `CreatedByUserId` / `ModifiedByUserId` / `EndedByUserId` | `string` / `string?` / `string?` | ASP.NET Identity user ids (`nvarchar(450)`). |
| `VehicleDataChanged` / `ClientDataChanged` | `bool` | Trace of mutations stamped at End time. |
| `Note` | `string?` | Free text, max 500. |
| `Active` | `bool` (default `true`) | Soft-delete flag. |
| `LegacyReferenceNumber` | `string?` (max 50) | Reference printed at the bottom of the paper form, e.g. `167403128/2026`. Backfilled for migrated rows; stays `NULL` for new rows (see §11). |
| `RowVersion` | `byte[]` | EF concurrency token (`IsRowVersion()`). |

Indexes (from EF config): `(CompanyId, CreatedAt)`, `ClientVehicleRelationId`, `RequestTypeId`, `PreviousRegistrationId`, and a **filtered "open requests" index** `IX_Request_Open` on `(CompanyId, CreatedAt)` with filter `[Active] = 1 AND [EndedAt] IS NULL`.

### 1.2 Child collections

All three children FK back to `Request` with `OnDelete: Cascade`, and back to their type catalog with `OnDelete: Restrict`.

| Entity | Table | File | Key fields |
|---|---|---|---|
| `RequestOwnershipProof` | `RequestOwnershipProof` | `Requests/RequestOwnershipProof.cs` | `RequestId`, `OwnershipProofTypeId`, `Detail` (max 500), `Active` |
| `RequestPaymentProof` | `RequestPaymentProof` | `Requests/RequestPaymentProof.cs` | `RequestId`, `PaymentProofTypeId`, `Detail` (max 500), `Active` |
| `RequestAttachment` | `RequestAttachment` | `Requests/RequestAttachment.cs` | `RequestId`, `AttachmentTypeId`, `FileName`, `ContentType`, `SizeBytes`, `StoragePath`, `UploadedAt`, `UploadedByUserId`, `Active` |

Note: proof/attachment children are **not** `ITenantOwned`. Their tenant scoping is *implicit* — the proof/attachment controllers always check the parent `Request` exists first, and the parent is tenant-filtered, so a caller who cannot see the parent cannot touch its children (`RequestProofsController.cs:9-16`).

---

## 2. Lifecycle: Open → End

State is **derived** from the nullable timestamps, not stored as an enum (`Request.cs:8-11`):

| State | Macedonian (UI) | Condition |
|---|---|---|
| **Open** | Отворено | `EndedAt IS NULL` (and `Active = true`) |
| **Closed** | Завршено | `EndedAt IS NOT NULL` |
| **Inactive** | Неактивно | `Active = false` (soft-deleted) |

The FE status badge mirrors this exactly (`RequestsView.vue:137-141`, `RequestFormView.vue:148-152`): inactive → `danger`, ended → `info` (closed), else → `success` (open).

**Terminal once ended.** A closed Request is immutable:
- `Update` (`PUT /api/requests/{id}`) rejects with `"Завршеното барање не може да се менува."` if `EndedAt` is set (`RequestsController.cs:524-525`).
- Every proof/attachment write checks the same and returns the same error (`RequestProofsController.cs:60`, `:88`, `:108`, `:145`, `:173`, `:193`; `RequestAttachmentsController.cs:113-114`, `:197-198`).
- The FE locks the whole form via `isReadOnly = !!endedAt` (`RequestFormView.vue:154`), hiding Save/Delete/End and disabling all inputs.

**Soft delete.** `DELETE /api/requests/{id}` only sets `Active = false` (`RequestsController.cs:574-583`); the row stays and can be reactivated by `PUT` with `Active: true`.

---

## 3. The anchor and the two-field FE picker

The **anchor** is `ClientVehicleRelationId` — a single row joining one client to one vehicle. It is required on create (`RequestsController.cs:271-274`) and **locked on update**: the controller deliberately does *not* reassign it (`RequestsController.cs:558-561` — the assignment line is commented out). The FE disables both anchor inputs in edit mode and shows a lock hint `requests.form.anchorLocked` (`RequestFormView.vue:786`, `:814`, `:832-834`). To change the anchor, the operator soft-deletes the Request and creates a new one.

### The two linked AutoComplete fields

The "Anchor" card renders **two always-visible fields** (template `RequestFormView.vue:779-830`; field state declared at `:102-127`):

- **Vehicle field** (*Возило*) — search by VIN (chassis) or plate.
  - If no owner is chosen yet, it does a **global** relation search: `GET /api/client-vehicle-relations?q={term}&activeOnly=true` (`onVehicleComplete`, `:529-549`). Picking a result derives the owner from it.
  - If an owner is already chosen, suggestions are filtered **locally** to that owner's vehicles.
- **Owner field** (*Сопственик*) — search by EMBG or name via `GET /api/clients?q={term}&pageSize=20` (`onOwnerComplete`, `:556-563`). Picking an owner loads their relations:
  - exactly **one** vehicle → auto-selects it as the anchor;
  - **zero or many** → clears the vehicle field so the operator picks from the (now scoped) dropdown (`onOwnerSelect`, `:564-575`).

The committed pair is held in `selectedRelation` / `selectedRelationId`, and `selectedRelationId` is what's sent as `clientVehicleRelationId` (`:633-645`). Each field has its own **New** / **Edit** buttons that open the Vehicle or Client form in a new tab so the in-progress Request isn't lost (`:601-611`).

---

## 4. RequestType — the workflow catalog

Source: `backend-v2/src/VTE.Domain/Requests/RequestType.cs`. Table `RequestType`. The flags here drive *both* validation and the End side-effects. Admin-only CRUD lives in `RequestTypesController.cs` (`GET /api/request-types`, reads open to all authenticated users; each write — POST/PUT/DELETE — is gated with `[Authorize(Roles = Roles.Administrator)]`, `RequestTypesController.cs:87`, `:122`, `:162`).

| Flag (v2) | Legacy column | Effect |
|---|---|---|
| `TechnicalExamRequirement` (enum/byte) | `IsTehnicalExamRequired` | Tri-state `NotRequired=0 / Required=1 / Optional=2` — **but overloaded on prod**, see §4.2. |
| `PaymentRequired` | `IsPayRequired` | End requires ≥1 active payment proof. |
| `IssuesNewRegistration` | `IsNewRegistration` | End records a "issue new registration" hint (deferred to the Vehicle form). |
| `DeactivatesRelation` | `IsRelationDeleted` | End deactivates the anchor relation. |
| `DeactivatesVehicle` | `IsVehicleDeleted` | End deactivates the vehicle. |
| `TransfersOwnership` | `IsNewCustomer` | Requires a new owner; End flips relations. Routes debts to the new owner (§5). |
| `MutatesVehicleData` | `IsVehicleChanged` | Stamped into `VehicleDataChanged` at End. |
| `MutatesClientData` | `IsCustomerChanged` | Stamped into `ClientDataChanged` at End. |
| `IsSufficient` | `IsSufficient` | Carried through; no v2 side-effect wired. |
| `PreviousRegistrationRequired` | `IsPreviousRegistrationRequired` | End requires `PreviousRegistrationId`. |

Other columns: `Id` (byte), `ParentRequestTypeId` (self-FK, nullable — the catalog is hierarchical: parent category + child types), `DocumentPrintId` (→ `RequestDocumentPrint`), `Name`, `Description`, `Active`.

### 4.1 `RequestDocumentPrint` — Plav / Bel / Zelen

Source: `Requests/RequestDocumentPrint.cs`. Table `RequestDocumentPrint`. Each row is a coloured paper-form template; a `RequestType` points at exactly one via `DocumentPrintId`.

| `Code` | `Name` (seeded) | Use |
|---|---|---|
| `PLAV` (blue) | Барање за регистрација (плав образец) | Registration / renewal / first registration |
| `BEL` (white) | Барање за пренос на сопственост (бел образец) | Ownership transfer |
| `ZELEN` (green) | Барање за одјава (зелен образец) | De-registration / vehicle deactivation |

Seeded by `DataSeeder.SeedRequestCatalogsAsync` (`DataSeeder.cs:100-120`). The `Code` is what the print page switches on to choose the layout (`RequestPrintView.vue:14-23`). CRUD: `RequestLookupsController.cs` (`GET /api/request-document-prints`).

### 4.2 The overloaded `TechnicalExamRequirement` (CLAUDE.md #16)

The entity types this as the enum `TechnicalExamRequirement` (`NotRequired=0/Required=1/Optional=2`, `TechnicalExamRequirement.cs`), but **the production migration backfilled the raw legacy `IsTehnicalExamRequired` integer into it — which is actually the exam-TYPE id** (`0`=none, `1`=РЕД-12М, `9`=…). So on prod the column holds values like `9` that are **not named enum members**.

Two readers treat it two different ways, and both are correct in context:
- The **End pre-validation** compares it to the enum constant `Required` (`RequestsController.cs:624`). On prod this only matches rows whose value is literally `1`.
- The **auto-exam hook** reads it as an int: `var carriedTypeId = (int)type.TechnicalExamRequirement;` — `>0` means "create an exam" and the value *is* the `TechnicalExamType` id (`RequestsController.cs:443-444`). See §6.

### 4.3 Seeded vs. migrated catalog

- **Fresh dev DB** gets a 5-type starter catalog (`DataSeeder.cs:122-171`): *Прва регистрација* (PLAV, exam Required, issues reg), *Продолжување на регистрација* (PLAV, exam Required, issues reg, prev-reg required), *Пренос на сопственост* (BEL, exam Optional, transfers ownership, issues reg), *Одјава на возило* (ZELEN, deactivates relation), *Технички преглед* (PLAV, exam Required). Seeding is **skipped entirely if any `RequestType` already exists** (`DataSeeder.cs:91-98`) so migrated data is never polluted.
- **Migrated/real catalog** is the legacy hierarchy — see `migrate/reseed-request-types.sql`. Four parent categories (*Барање - Пријава*, *Барање за регистрација на моторно-приклучно возило*, *Регистрационен лист*, *Други барања*) each with child types carrying the А/Б/В/Г sub-codes and their legacy flag values.

Proof/attachment type catalogs are also seeded (`DataSeeder.cs:181-214`): ownership proofs (*Купопродажен договор*, *Договор за подарок*, *Решение за наследство*, *Сообраќајна дозвола*, *Фактура / профактура*, *Друго*), payment proofs (*Уплатница*, *Фискална сметка*, *Виримански налог*, *Картичка (POS)*, *Готовинска уплата*, *Друго*), attachment types (*Лична карта*, *Сообраќајна дозвола*, *Технички преглед*, *Договор*, *Слика од возилото*, *Друго*).

---

## 5. New owner ("Нов сопственик") + debt routing

### 5.1 The flow (CLAUDE.md #17)

For `TransfersOwnership` types the operator does a **free client search** — any *komitent* by EMBG/name, not restricted to the anchor's vehicle (mirrors legacy `uxRequestEdit` `IsNewCustomer` panel). The FE shows the "Нов сопственик" panel **as soon as a transfers-ownership type is picked** (gated on `requiresNewOwner`, not on the anchor — `RequestFormView.vue:144-146`, `:837`).

The FE sends the chosen **client id** as `RequestWriteDto.NewOwnerClientId` (`RequestDtos.cs:58-63`), *not* a relation id:

```ts
// RequestFormView.vue:636-639
newOwnerClientId: requiresNewOwner.value ? newOwnerClientId.value : null,
newClientVehicleRelationId: newOwnerClientId.value ? null : (newClientVehicleRelationId.value ?? null),
```

The backend turns the client into a relation via **`ResolveOrCreateNewOwnerRelationAsync`** (`RequestsController.cs:416-436`): it reuses an existing `(newClient + anchor's vehicle)` relation if one exists (ordered by `Active desc, StartDate desc`), otherwise **creates one** with the anchor's `RelationTypeId` and — critically — **`Active = false`**:

```csharp
var rel = new ClientVehicleRelation {
    ClientId = newOwnerClientId, VehicleId = vehicleId,
    RelationTypeId = ownerRelationTypeId, StartDate = DateTime.UtcNow,
    Active = false,   // activated by the End flow when ownership transfer completes
};
```

The resolved relation id is stored in `Request.NewClientVehicleRelationId`. Validation rejects: missing new owner, a new owner equal to the anchor, or a non-existent relation (`RequestsController.cs:287-304` on create; `:537-556` on update). A directly-supplied `NewClientVehicleRelationId` is still honoured for migrated data, but `NewOwnerClientId` takes precedence.

**Why `Active=false` matters (gotcha #17):** the new-owner relation stays inactive until the Request is *ended*. If the Request is abandoned, the vehicle never ends up with two active owners. The End flow (§7) flips it active and deactivates the anchor.

### 5.2 The overloaded `NewClientVehicleRelationId`

Legacy named the column `IdCustomerVehicleRelationNew`, but it holds a **customer id** while editing and a **relation id** after save — so migrated v2 rows may store either. Both the print bundle and the FE resolve it **client-id-first**: try the value as a client id, but only accept that reading when the client actually owns a relation on the anchor vehicle (`isClientOnVehicle` gate); otherwise treat it as a relation id (`RequestsController.cs:818-852`; FE mirror `RequestFormView.vue:466-496`).

### 5.3 Debt routing on ownership transfer (CLAUDE.md #11)

Auto-debt creation fires only on **Insert** (`Create`), mirroring legacy `DataPortal_Insert` — never on `Update` (`RequestsController.cs:336-389`). The triggering relation is the **new owner** when transferring:

```csharp
// RequestsController.cs:342-344  (mirror of legacy Request.vb:856-861)
var debtRelationId = (type.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue)
    ? entity.NewClientVehicleRelationId.Value
    : entity.ClientVehicleRelationId;
```

The request-fee debt is then created via `IDebtService.CreateDebtsForSourceAsync` with `origin: DebtOrigin.Request`, `trigger: PriceTrigger.Request`, `originId: entity.Id`, and a `communityId` resolved from the debt relation's client → city → `CommunityId` (CLAUDE.md #12; `RequestsController.cs:349-365`). `organizationId` is inherited from the linked tech-exam when present, else `0` (`:367-375`). The whole block is best-effort wrapped in `try/catch` — a debt failure never rolls back the Request (a transactional outbox is planned for Phase 5). Debts are idempotent on `(Origin, OriginRequestId)` (CLAUDE.md). See [Payments & pricing](05-payments-and-pricing.md).

---

## 6. Auto-create technical exam on renewal

`TryAutoCreateExamAsync` (`RequestsController.cs:438-513`) ports the legacy "auto technical exam on request save" (`uxRequestEdit.vb:173-195`). It runs on **Create**, *before* the request-fee debt block, so that block inherits the new exam's organization. Like the debt hook it is best-effort (`try { ... } catch { }`, `:331-332`).

Gates (all must pass):
1. Config flag `Requests:AutoCreateTechExam` must be `true` (`appsettings.json:53`; legacy `objCurentTehExamOrganization.AutmateProceses`).
2. `Request.TechnicalExamReportId` must be `null` (legacy `IdTechnicalExamReport = 0` guard).
3. `(int)type.TechnicalExamRequirement > 0` — i.e. the overloaded column carries an exam-type id (§4.2).

It resolves the exam type from that carried id, falling back to `Requests:DefaultTechnicalExamTypeId` (default `1`) if unknown; the station org comes from `Requests:DefaultTechnicalExamOrganizationId` (prod `37` — *АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ*, CLAUDE.md #16). It anchors the exam to the **new owner on transfer, else the anchor** (same routing as debts, `:460-462`), builds a `RegNumber` of `{org}-{seq}/{year}` continuing the org's sequence (`:470-480`), creates a *passing* `TechnicalExamReport` (`VehicleIsRight = true`), back-links it into `Request.TechnicalExamReportId` (`:499`), and creates the **exam-fee debt** — `DebtOrigin.TechnicalExamIrregular` + `PriceTrigger.TechnicalExamIrregular` when `typeId > 1`, else `TechnicalExam` (CLAUDE.md #10; `:504-512`). See [Technical exams](04-technical-exams.md).

> On a fresh dev DB the gate at step 3 rarely fires because the seeded enum values are small; on prod the carried exam-type ids make this the real renewal automation.

---

## 7. The End flow and its transactional side-effects

`POST /api/requests/{id}/end` (`RequestsController.cs:599-708`). Returns `EndRequestResultDto { Id, EndedAt, RelationDeactivated, VehicleDeactivated, OwnershipTransferred, NeedsNewRegistration, Notes[] }`.

### 7.1 Pre-validation (server mirror of UI checks)

Rejects (BadRequest) before any mutation:
- already ended → `"Барањето е веќе завршено."`; inactive → `"Барањето е неактивно."` (`:607-610`).
- `PaymentRequired` and no active `RequestPaymentProof` → `"Потребен е најмалку еден доказ за уплата."` (`:617-623`).
- `TechnicalExamRequirement == Required` and `TechnicalExamReportId is null` → `"Потребен е технички преглед."` (`:624-628`).
- `TransfersOwnership` and `NewClientVehicleRelationId` null/missing → `"Новиот сопственик не е избран."` / `"Новата врска не постои."` (`:629-637`).
- `PreviousRegistrationRequired` and `PreviousRegistrationId is null` → `"Потребна е претходна регистрација."` (`:638-639`).

### 7.2 Side-effects (atomic — one `BeginTransactionAsync`)

All wrapped in a transaction (`:642`) and committed together (`:701-702`). The anchor relation is loaded once (`:649-655`); a missing anchor rolls back.

| Flag | Effect |
|---|---|
| `TransfersOwnership` | Activate the new relation (`newRel.Active = true`), deactivate the anchor (`anchor.Active = false`, `EndDate = now`). `ownershipTransferred = true`. Note: *"Сопственоста е префрлена на новата врска."* (`:657-667`) |
| `DeactivatesRelation` (only if not transferring) | Deactivate the anchor (`Active = false`, `EndDate = now`). `relationDeactivated = true`. (`:668-674`) |
| `DeactivatesVehicle` | Set the anchor vehicle's `Active = false`. `vehicleDeactivated = true`. (`:676-685`) |
| `IssuesNewRegistration` | **Deferred** — no row created here. Recorded as a hint note so the UI can prompt the operator to add the registration from the Vehicle form. `needsNewRegistration = true`. (`:687-693`) |

Finally it stamps `EndedAt = now`, `EndedByUserId`, and the change traces `VehicleDataChanged = type.MutatesVehicleData || vehicleDeactivated`, `ClientDataChanged = type.MutatesClientData` (`:696-700`).

> No debts are created at End — debts fire on **Create** only (§5.3). End is purely about applying the relation/vehicle state changes and closing the Request.

The FE confirms the End with a preview of side-effects (`endSideEffects`, `RequestFormView.vue:374-401`) and re-loads the Request + children afterward to reflect the closed state (`doEnd`, `:403-427`).

---

## 8. Ownership / payment proofs + attachments

### 8.1 Proofs — `RequestProofsController.cs`

Nested under the parent: routes `/api/requests/{requestId}/ownership-proofs[/{id}]` and `/api/requests/{requestId}/payment-proofs[/{id}]`. Each supports GET (list, joins the type name), POST (add), PUT (update), DELETE (soft-delete `Active=false`). Every write first loads the parent Request and refuses if it's ended (§2). The type id is validated against the catalog.

The print bundle and the End check consume only **active** proofs (`p.Active`, `RequestsController.cs:619-620`, `:855-875`).

### 8.2 Attachments — `RequestAttachmentsController.cs`

Route `/api/requests/{requestId}/attachments`. Multipart upload (`UploadFormDto { IFormFile File; byte AttachmentTypeId }` — bundled in one DTO because Swashbuckle can't mix a separate `IFormFile` param). Storage:
- Root: config `Storage:RequestAttachmentsRoot` (default `./storage/request-attachments`, resolved relative to ContentRoot).
- Bucketed `yyyy/MM/{requestId}`; each blob renamed `{guid:N}{ext}` to avoid collisions and path traversal (`:125-135`).
- Validation: non-empty, size ≤ `Storage:MaxAttachmentBytes` (default 20 MB; framework hard ceiling 50 MB via `[RequestSizeLimit]`), MIME in `Storage:AllowedAttachmentMimeTypes` (default jpeg/png/gif/webp/pdf — `appsettings.json:36-43`).
- `GET .../{id}/download` re-resolves the path and **guards against path traversal** (resolved file must live under the root, `:180-189`).
- `DELETE` is soft (`Active=false`); the blob stays on disk for a later housekeeping sweep.

FE: new-request form pre-populates editable proof rows (legacy defaults *СООБРАЌАЈНА ДОЗВОЛА* / *СМЕТКА*) held in memory, then persisted after the Request is created (`RequestFormView.vue:57-66`, `:653-666`, `:713-715`). In edit mode, proofs and attachments are managed through dialogs (`:867-1106`).

---

## 9. Create / Update endpoints

`POST /api/requests` (`RequestsController.cs:248-392`):
1. Resolve tenant — admins may pass `CompanyId` (validated); operators use their own (`:254-268`).
2. Validate the anchor relation exists & is visible, and the request type exists (`:270-279`).
3. Resolve the new owner if `TransfersOwnership` (§5.1).
4. Insert the `Request` (`CreatedAt = UtcNow`, `CreatedByUserId = tenant.UserId`, `Active = dto.Active ?? true`).
5. Best-effort auto-exam (§6), then best-effort request-fee debt (§5.3).
6. Return the full read DTO (re-fetches via `Get`).

`PUT /api/requests/{id}` (`:515-572`): rejects if ended; `CompanyId` immutable; **anchor immutable** (§3); re-resolves the new owner; updates `RequestTypeId`, `NewClientVehicleRelationId`, `TechnicalExamReportId`, `PreviousRegistrationId`, `Note`, `Active`, and stamps `ModifiedAt`/`ModifiedByUserId`. Returns `204`.

`RequestWriteDto` (`RequestDtos.cs:48-63`): `[Required] RequestTypeId`, `[Required] ClientVehicleRelationId`, `NewClientVehicleRelationId?`, `TechnicalExamReportId?`, `PreviousRegistrationId?`, `Note?` (max 500), `Active?`, `CompanyId?` (admin override), `NewOwnerClientId?`.

`GET /api/requests/{id}` returns `RequestReadDto` with creator/modifier/ender usernames resolved in one batch (`:221-246`).

---

## 10. The print bundle — `GET /api/requests/{id}/print`

`PrintBundle` (`RequestsController.cs:770-897`) returns everything the FE needs to render the paper form, replacing the legacy `printPlav/Bel/Zelen` procs. Shape (`PrintBundleDto`, `:712-727`):

- `Request` (`RequestReadDto`) + `Type` (`RequestTypeMeta`, including `DocumentPrintCode` / `DocumentPrintName`).
- `Client` + `Vehicle` (from the anchor relation), and `NewClient` + `NewVehicle` (resolved client-id-first, §5.2).
- `OwnershipProofs` + `PaymentProofs` (active only).
- `Company` header.
- `LastRegistration` (newest by `ValidUntil` then `RegisteredDate`) and `PreviousRegistration` (the row whose plate matches the vehicle's stored plate — on a transfer this is the *old* plate, distinct from the newly-issued one; `BuildPreviousRegistrationMeta`, `:1126-1154`).

`ClientMeta` carries community plate prefix (`CommunityRegistrationCode`, e.g. `VE`) and destination MVR office (`CommunityRegistrationIssuer`) used by the Plav print to build the new-registration prefix/issuer. `VehicleMeta` is exhaustive (Plav page-2 technical specs: dimensions, masses, axle loads, CO₂, noise, trailer/hitch limits, approval mark). The FE `RequestPrintView.vue` switches the layout by `documentPrintCode` (PLAV/BEL/ZELEN — `printCode`/`headerClass`, `:14-23`) and auto-opens the browser print dialog (`window.print()` on a 250 ms timer after the bundle loads, `:35`). Status of the pixel-perfect templates: Plav + Zelen done; Bel (`BelTemplate.vue`) is the one remaining work item (CLAUDE.md task #70).

---

## 11. List + search

`GET /api/requests` (`RequestsController.cs:38-219`) returns `PagedDto<RequestListItem>`.

Query params: `q`, `status` (`open` default / `closed` / `all`), `companyId` (admin), `clientVehicleRelationId`, `includeInactive` (default false → only `Active`), `sort`, `dir`, `page`, `pageSize` (1–200, default 50). Status maps to `EndedAt IS NULL` (open) / `IS NOT NULL` (closed).

**Search** pre-resolves matching ids via small subqueries to avoid EF's 2100-parameter limit (`:64-109`). A term matches when it hits any of:
- the **request # (Id)** — exact `long` match, or partial `LIKE` on the Id string;
- the `LegacyReferenceNumber` (e.g. `168077128/2026`);
- a client (FirstName/MiddleName/LastName/MB) → its relations;
- a vehicle (VIN/Plate) → its relations;
- a request-type name.

**Sorting** (default newest-first by `CreatedAt`): `id`, `created`, `status` (by `EndedAt`), `type`, `operator`, `client`, `vehicle` — join columns sort via correlated subqueries so order spans the whole filtered set (`:113-134`).

The page rows resolve type names, client names, vehicle VIN/plate, and operator name in **batched** lookups (`:148-216`). `RequestListItem` (`RequestDtos.cs:7-22`) carries `Id, CompanyId, RequestTypeId, RequestTypeName, ClientVehicleRelationId, ClientDisplayName, VehicleId, VehicleVin, VehiclePlate, CreatedAt, ModifiedAt, EndedAt, CreatedByUserName, Note, Active`.

**FE list** (`RequestsView.vue`): status `SelectButton` tabs (Отворени/Завршени/Сите), debounced search (300 ms), admin company filter, lazy server-side paging/sorting. The first column header is `#` and renders the Id in a monospace bold style (`req-no`, `:221-223`).

---

## 12. Configuration reference

| Key | Default | Purpose |
|---|---|---|
| `Requests:AutoCreateTechExam` | `true` | Gate for §6 auto-exam. |
| `Requests:DefaultTechnicalExamTypeId` | `1` | Fallback exam type (РЕД-12М). |
| `Requests:DefaultTechnicalExamOrganizationId` | `37` | Station tech-exam org (prod = АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ). |
| `Storage:RequestAttachmentsRoot` | `./storage/request-attachments` | Attachment blob root. |
| `Storage:MaxAttachmentBytes` | `20971520` (20 MB) | Per-file size cap. |
| `Storage:AllowedAttachmentMimeTypes` | jpeg/png/gif/webp/pdf | Upload allow-list. |

(All from `backend-v2/src/VTE.Api/appsettings.json`.)

---

## 13. Migration notes (legacy → v2)

Scripts in `migrate/`:
- `migrate-request-catalogs.sql` / `reseed-request-types.sql` — the catalog + hierarchy.
- `migrate-requests.sql` — Requests + ownership/payment proofs. Preserves legacy Ids, sets `CompanyId=4` (VELES), maps all operators to the `admin` user (original numeric operator id appended to `Note` in `[]` for traceability). `IdCustomerVehicleRelationNew` → `NewClientVehicleRelationId` (NULL when 0 or not present in target); `IdTechnicalExamReport` initially NULL; `IsVehicleChanged`/`IsCustomerChanged` → `VehicleDataChanged`/`ClientDataChanged`. Requests whose anchor relation is missing in the target are **skipped**.
- `backfill-request-reference-no.sql` — restores `TechnicalExamReportId` from legacy, then computes `LegacyReferenceNumber` as `{StationCode}{IdTechnicalExamReport}{OperatorId}/{Year(DateCreated)}` (e.g. `167403128/2026`; station code hard-coded `1` for VELES).
- `wire-request-operators.sql` — wires `CreatedBy`/`ModifiedBy`/`EndedBy` to real operator users.
- `perf-indexes-requests.sql` — supplemental indexes.

Migrated production scale (CLAUDE.md): ~18k requests in the snapshot set (~128k after the full prod .bak lift).
