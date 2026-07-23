# Payments, pricing & debts (Наплата)

This is the reference for VTE's pricing + billing engine: how a finished workflow turns into an
open debt, how those debts are listed and collected on the dashboard's **Наплата** panel, and how
selected debts become a bill (**Направи сметка**) with optional installments and fiscal receipts.
Everything here is grounded in the v2 code; legacy behaviour is cited where v2 mirrors it.

The data flow, end to end:

```
finished workflow                pricing engine                     billing
─────────────────                ──────────────                     ───────
TechnicalExamReport.Create  ─┐
RequestsController.Create    ─┼─▶ DebtService.CreateDebtsForSource ─▶ CustomerDebt rows (open)
                             │     └─ PricingEvaluator.Evaluate          │
                             │        (matches PriceCatalog rules)       │  Наплата panel groups
                             │                                           │  them by client→vehicle
                             ▼                                           ▼
                                              POST /payment-documents/from-debts
                                              → PaymentDocument + PaymentDocumentLine[]
                                              → marks debts Paid + SettledByLineId
                                              → optional InstallmentAgreement + schedule
                                              → fiscal receipt file (Accent PF-500)
```

Related docs: see [Requests workflow](03-requests.md) and [Technical exams](04-technical-exams.md)
for the workflows that fire the debt hooks, and [Architecture & multi-tenancy](01-overview-and-architecture.md)
for the tenant query filter referenced throughout.

---

## 1. The 3-level legacy hierarchy → flat `PriceCatalog`

In the legacy VB.NET DB, a billable fee lives across **three** tables (plus VAT + vehicle-category
side tables). The cascade and column names matter because the v2 migration scripts mirror them
exactly:

```
PaymentCategories       (Оперативни трошоци, Атест, …)   — trigger flags + community + company
   ↓ IdPymentCategory   [sic: legacy typo, no "a"]
PaymentItems            (per category)                    — vehicle-category + payment-category id
   ↓ IdPaymentItem
PaymentItemParametars   (per item, vehicle-conditional)   — VehicleField + From/To + Price
```

**Critical mapping (CLAUDE.md):** `v2.PriceCatalog.Id` maps to `legacy.PaymentItemParametars.Id` —
**not** `PaymentItems.Id`. (The latter was an 87% coincidental match that fooled the migration once;
the fix chain is `migrate/OBSOLETE-refix-pricecatalog-from-parametars.sql` →
`migrate/backfill-pricecatalog-rules.sql`.)

The legacy `getPaymentCatalog` stored-procedure filter that the v2 evaluator must mirror
(CLAUDE.md):

```sql
PaymentCategoriesActive         = 1
AND PaymentItemsActive          = 1
AND PaymentItemParametarsActive = 1
AND (IdCompany = @IdCompany OR IdCompany = 0)
```

v2 flattens all three levels into **one row per fee** in `PriceCatalog`, carrying the trigger,
vehicle-category filter, parametar range, community scope and company scope as columns. The legacy
boolean cluster `IsRequest/IsTehnicalExam/IsTrafficLicence/IsPermision/IsIDL` collapses to a single
`Trigger` enum (see [§3](#3-the-pricetrigger-enum)).

### Active-cascade rule (gotcha #7)

A rule is firing-eligible only if **all three** legacy levels have `Active = 1`. The first migration
set `PriceCatalog.Active = PaymentItemParametars.Active` alone, leaving rules whose parent
`PaymentCategory` was deactivated still `Active = true` — they leaked into the evaluator and produced
bogus debts (the visible symptom: 6× "Оперативни трошоци" duplicates on a test exam, all 6 parents
deactivated in legacy). `migrate/fix-pricecatalog-active-cascade.sql` cascades the AND down:

```sql
UPDATE pc
SET pc.Active = CASE
    WHEN ISNULL(pip.Active, CONVERT(bit, 0)) = 1
     AND ISNULL(pi.Active,  CONVERT(bit, 0)) = 1
     AND ISNULL(pcat.Active,CONVERT(bit, 0)) = 1
    THEN CONVERT(bit, 1) ELSE CONVERT(bit, 0) END
FROM dbo.PriceCatalog pc
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip ON pip.Id   = pc.Id
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems          pi  ON pi.Id    = pip.IdPaymentItem
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentCategories     pcat ON pcat.Id = pi.IdPymentCategory
WHERE pc.Id <> 999999;   -- leave the unknown sentinel alone
```

This cascade found **815 stale rules** that needed deactivating.

---

## 2. `PriceCatalog` — the flat rule schema

`backend-v2/src/VTE.Domain/Payments/PriceCatalog.cs`. Cross-tenant catalog (fees are statutory in
Macedonia, shared across companies); the per-company override is `PriceCompanyId`.

| Field | Type | Meaning | Legacy source |
|---|---|---|---|
| `Id` | `int` | PK. **= `PaymentItemParametars.Id`**. | — |
| `Code` | `string?` | Stable short code (e.g. `"TEH-IPR"`). | — |
| `Name` | `string` | Human label. May contain `{0}`/`{1}` range placeholders (see [§9](#9-наплата-line-names--01-substitution)). | `Name` |
| `BasePrice` | `decimal` | Base price before discount + VAT. `decimal(18,4)`. | `Price` (money) |
| `VatRateId` | `int` | FK → `VatRate`. | `IdDDVCatalog` |
| `Trigger` | `PriceTrigger` (byte) | Which workflow auto-creates a debt with this fee. | the `Is*` boolean cluster |
| `VehiclePaymentCategoryId` | `int?` | Vehicle-payment-category this fee applies to (loose ref, no FK). NULL = any. | `PaymentItems.IdVehicleCategoryForPayments` |
| `CommunityId` | `int?` | Municipality scope. NULL = any. | `PaymentCategories.IdCommunity` |
| `PriceCompanyId` | `byte?` | Tenant scope. NULL = applies to every company. | `PaymentCategories.IdCompany` |
| `PaymentCategoryGroupId` | `int?` | Forensic grouping = legacy `PaymentCategories.Id`. | `PaymentCategories.Id` |
| `VehicleField` | `string?` | For ranged rules, the `Vehicle` property name read at eval time. NULL = fixed-fee rule. | `PaymentItemParametars.VehicleField` |
| `ParametarFrom` | `double?` | Lower bound (inclusive) on the vehicle property. | `ParametarFrom` |
| `ParametarTo` | `double?` | Upper bound (inclusive) on the vehicle property. | `ParametarTo` |
| `VehicleCategoryFilter` | `string?` | Optional CSV of vehicle-category codes (empty = all). | `VehicleCategoryForPayments` join |
| `BankAccount` | `string?` | Bank account fees route to. | `CalculationItems.BankAccount` |
| `PaymentForm` | `string?` | Bank payment-form code (e.g. `"PP30"`). | `CalculationItems.Form` |
| `Active` | `bool` | Firing-eligible flag (after active-cascade). | (AND of 3 levels) |

EF config (`VteDbContext.cs:604-621`): `Trigger` stored as `tinyint`; `IX_PriceCatalog_Eval`
composite index on `(Trigger, VehiclePaymentCategoryId)` for the evaluator; `VatRateId` FK with
`Restrict` delete.

### Backfilling the rule columns

`migrate/backfill-pricecatalog-rules.sql` joins the 3 legacy levels into one wide row per parametar
and populates `Trigger / VehiclePaymentCategoryId / CommunityId / PaymentCategoryGroupId /
VehicleField / ParametarFrom / ParametarTo`. Two parts are load-bearing:

**1. VehicleField translation map** — legacy property names (with their preserved typos, gotcha #4)
are mapped to v2 `Vehicle` property names; literal `'Null'` / empty / NULL all become NULL
("fixed fee"):

| Legacy `VehicleField` | v2 `Vehicle` property |
|---|---|
| `NumberOfSeats` | `Seats` |
| `NumberOfStandingSeats` | `StandingSeats` |
| `MaximunAllowedWaight` *(typo)* | `MaxAllowedWeightKg` |
| `TotalWaight` | `MaxLegalTotalMassKg` |
| `EnginePowerOutPut` | `EnginePowerKw` |
| `EngineWorkingCapacity` | `EngineWorkingCapacityCc` |
| `IdVehicleCategories` | `CategoryId` |
| `IdVehicleBodyType` | `BodyTypeId` |
| `IdEnginePowerSource` | `FuelId` |
| `''` / `'Null'` / NULL | NULL (fixed fee) |

**2. Single-trigger pick** — legacy supports multiple `TrigerdBy*` flags on one PaymentCategory;
v2's `Trigger` is one value. The script picks ONE by precedence (gotcha #13 — multi-trigger split is
not yet automated):

```
IrregularTechExam(6) → TechExam(1) → Request(2) → TrafficLicence(3) → Permission(4) → IDL(5) → None(0)
```

**`PriceCompanyId`** is added + backfilled separately by
`migrate/add-pricecatalog-company-and-backfill.sql` (chain
`PriceCatalog.Id == PaymentItemParametars.Id → PaymentItems.IdPymentCategory →
PaymentCategories.IdCompany`). Without it, every company's "Оперативни трошоци" rule fires for the
same vehicle → duplicate debt lines (the visible bug: 6× "Оперативни трошоци" at 354.00 on test exam
167585).

### Admin CRUD — `PriceCatalogsController`

`backend-v2/src/VTE.Api/Controllers/PriceCatalogsController.cs` (`/api/price-catalogs`). **Reads**
for any authenticated user (`[Authorize]` at class level — operators need them to render bills + the
price-pick widget); **writes** are `[Authorize(Roles = Roles.Administrator)]` only — a wrong rule
edit can cascade into thousands of mis-calculated debts.

| Method | Route | Notes |
|---|---|---|
| GET | `/api/price-catalogs` | Paged. Filters: `trigger`, `companyId`, `communityId`, `vehiclePaymentCategoryId`, `uncategorized` (NULL bucket, wins over an explicit id), `paymentCategoryGroupId`, `search` (Name/Code), `activeOnly`. |
| GET | `/api/price-catalogs/categories` | Vehicle-payment categories with rule counts; first node `Id=null` is the synthetic "uncategorized" bucket. |
| GET | `/api/price-catalogs/{id}` | Single rule. |
| GET | `/api/price-catalogs/lookups` | Distinct triggers / companies (resolved to names) / vehicle-fields / category-groups for filter dropdowns. |
| POST | `/api/price-catalogs` | Admin. `Validate` requires Name, non-negative BasePrice, valid `PriceTrigger`, `ParametarFrom ≤ ParametarTo`. |
| PUT | `/api/price-catalogs/{id}` | Admin. |
| DELETE | `/api/price-catalogs/{id}` | Admin. **Soft-delete only** (`Active=false`) — historical debts/lines reference rules; hard-delete would break receipt reprints. |

---

## 3. The `PriceTrigger` enum

`backend-v2/src/VTE.Domain/Payments/PriceTrigger.cs` — replaces the legacy `Is*` boolean cluster with
one typed value. A `PriceCatalog` row fires when its owning workflow ends.

```csharp
public enum PriceTrigger : byte {
  None = 0,
  TechnicalExam = 1,
  Request = 2,
  TrafficLicence = 3,
  Permission = 4,
  InternationalDrivingLicence = 5,
  TechnicalExamIrregular = 6,   // legacy TrigerdByIrregularTechnicalExam
}
```

> **Note:** CLAUDE.md's "Domain enums" block names value 5 `IDL`; the actual member name in the source
> is `InternationalDrivingLicence` (same value 5).

---

## 4. `DebtOrigin` enum + `CustomerDebt` entity

### `DebtOrigin`

`backend-v2/src/VTE.Domain/Payments/DebtOrigin.cs` — records which workflow created a debt (maps to
the legacy `TrigerdByX` cluster on `PaymentCategories`):

```csharp
public enum DebtOrigin : byte {
  Manual = 0, Request = 1, TechnicalExam = 2,
  TechnicalExamIrregular = 3,      // legacy: TrigerdByIrregularTechnicalExam
  TrafficLicence = 4, Permission = 5, InternationalDrivingLicence = 6,
}
```

> ⚠ **`DebtOrigin` and `PriceTrigger` do NOT share values.** `TechnicalExam` is `DebtOrigin = 2` but
> `PriceTrigger = 1`; `Request` is `DebtOrigin = 1` but `PriceTrigger = 2`. The auto-hooks pass both
> explicitly — don't assume one is a cast of the other.

### `CustomerDebt`

`backend-v2/src/VTE.Domain/Payments/CustomerDebt.cs`. An open debt against a customer–vehicle
relation. Legacy source: `CustomerFinancialState`. Implements `ITenantOwned` (tenant-scoped).

| Field | Type | Meaning | Legacy |
|---|---|---|---|
| `Id` | `long` | PK. | — |
| `CompanyId` | `byte` | Tenant. | — |
| `CustomerVehicleRelationId` | `long` | Anchor: who owes for what vehicle. | `IdCustomerVehicleRelation` |
| `PriceCatalogId` | `int` | The fee rule that triggered. | `IdPriceCatalog` |
| `Price` | `decimal` | **Snapshot** of the fee price at creation (catalog can change later). | `Price` |
| `VatPercent` | `double` | Snapshot of VAT % at creation. | — |
| `Note` | `string?` | e.g. `"технички преглед бр. {RegNumber}"`. | — |
| `Origin` | `DebtOrigin` | Which workflow created it. | `TrigerdByX` |
| `OriginRequestId` | `long?` | Set when `Origin = Request`. | `IdDocument` |
| `OriginTechnicalExamId` | `long?` | Set when `Origin = TechnicalExam(Irregular)`. | `IdDocumentTehnicalExam` |
| `OrganizationId` | `int` | Station/org that owns the debt. | `IdOrganization` |
| `Paid` | `bool` | True once settled by a `PaymentDocumentLine`. | `Payed` |
| `SettledByLineId` | `long?` | FK → the line that settled it (null while unpaid). | — |
| `CreatedAt` / `CreatedByUserId` | | Audit. | — |
| `Active` | `bool` | Soft-delete flag. | `Active` |
| `LegacyId` | `long?` | `CustomerFinancialState.Id` for synced rows; NULL for v2-native. **Unique filtered** → makes legacy-sync idempotent. | `Id` |

EF config (`VteDbContext.cs:698-726`):
- `Origin` stored as `tinyint`; `Price` is `decimal(18,4)`.
- `IX_CustomerDebt_Open` — partial index on `(CustomerVehicleRelationId, Paid)` filtered
  `[Active] = 1 AND [Paid] = 0` (keeps the open-debt query fast).
- Filtered indexes on `OriginRequestId` and `OriginTechnicalExamId` (forensic origin lookups).
- `LegacyId` unique filtered (`[LegacyId] IS NOT NULL`).
- `SettledByLineId` FK uses `Restrict` — deleting a paid line won't orphan the debt.

---

## 5. `PricingEvaluator` — reflection-based rule matching

`backend-v2/src/VTE.Infrastructure/Pricing/PricingEvaluator.cs`. Stateless / pure — given a trigger +
the vehicle being processed, returns every `PriceCatalog` row whose rule applies. Mirrors legacy
`PaymentCataologList.GetPaymentForDepts` (lines 40-86).

Signature (`IPricingEvaluator`):

```csharp
Task<IReadOnlyList<MatchedPrice>> EvaluateAsync(
    PriceTrigger trigger, Vehicle? vehicle, int? communityId, byte? companyId,
    CancellationToken ct = default);

public record MatchedPrice(int PriceCatalogId, decimal Price, double VatPercent);
```

### Algorithm

1. **`PriceTrigger.None` → empty.** No trigger means nothing fires.

2. **DB-side candidate filter** (the load-bearing `Where`):

   ```csharp
   p.Active && p.Trigger == trigger
     && (p.VehiclePaymentCategoryId == null || p.VehiclePaymentCategoryId == vehicleCategoryId)
     && (p.CommunityId == null   || p.CommunityId   == communityId)   // §community filter, gotcha #12
     && (p.PriceCompanyId == null || p.PriceCompanyId == companyId)    // §company scope
   ```

   `vehicleCategoryId = vehicle?.PaymentCategoryId`. A NULL on the rule side means "applies to every
   scope". `PriceCompanyId` is critical — without it every legacy company's "Operating fee" rule
   fires at once → duplicate debt lines.

3. **Registration-category filter (added 2026-07-16).** `VehicleCategoryFilter` is a CSV of
   `VehicleCategory` CODES (`"M2"`, `"M3,N2"` …), matched case-insensitively against the vehicle's
   `CategoryId → VehicleCategory.Code` (resolved once per evaluation, only when some candidate uses
   it). This splits fees that share a PAYMENT category but differ by registration category — e.g.
   минибуси (M2) vs автобуси (M3), both payment-cat 5. Data caveat: codes exist in Cyrillic-lookalike
   variants (`L1е`, `Т1`) — rule CSVs must list them (see `migrate/seed-sovet-fee-2026.sql`).
   Unknown/missing code on the vehicle → the rule can't match (operator handles manually).

4. **Age condition (added 2026-07-16).** `AgeFrom`/`AgeTo` (years, inclusive) gate the rule on the
   vehicle's age = `Today.Year − ManufactureDate.Year`. Unknown manufacture date → no match. This is
   the second rule dimension the Сл. весник 89/2022 eco tariff needs (категорија × старост ×
   зафатнина) which the legacy single-range model couldn't express — see
   `migrate/seed-eco-fee-rules-2022.sql` (+ `-bus.sql`).

5. **Ranged-rule filtering (client-side, needs reflection over `Vehicle`):**
   - `VehicleField` blank → **fixed-fee rule, always matches** → added to `matched`.
   - `VehicleField` set but `vehicle is null` → skip.
   - Otherwise read the named property off `Vehicle` via a cached `PropertyInfo` (`GetVehicleField`,
     case-insensitive), coerce to `double?` (`TryReadAsDouble` handles int/long/decimal/bool/etc.),
     and test `ParametarFrom ≤ value ≤ ParametarTo` (NULL bounds → `double.MinValue/MaxValue`).
     An **unknown property name is skipped, not thrown** (`prop is null` → continue).

6. **Sentinel fallback.** A rule with `ParametarFrom ∈ {null,0} AND ParametarTo ∈ {null,0}` is a
   "default tier" sentinel. Sentinels that pass the value test land in a separate `fallback` list.
   **Winners = `matched` if non-empty, else `fallback`** — sentinels only fire when no explicit
   range matched.

7. **VAT snapshot.** For each winner, look up `VatRate.Percent` by `VatRateId` and emit a
   `MatchedPrice`.

> ⚠ **Sentinel multiplier (gotcha #8) is NOT implemented here.** Legacy: when
> `ParametarFrom = 0 AND ParametarTo = 0 AND VehicleField != "Null"`, the price is
> `Price * vehicle.<VehicleField>` (a per-unit multiplier, legacy `Request.vb` ~line 905). v2 treats
> these only as a *default-tier fallback* — it does **not** multiply. This is deferred.

---

## 6. `DebtService` — evaluate + idempotent save

`backend-v2/src/VTE.Infrastructure/Pricing/DebtService.cs`. Wraps the evaluator + EF insert in one
unit, mirroring the legacy `AddDeptsToCustomer` path that fired on every document save.

```csharp
Task<int> CreateDebtsForSourceAsync(
    DebtOrigin origin, long originId, long customerVehicleRelationId,
    int organizationId, PriceTrigger trigger,
    int? communityId = null, string? note = null, CancellationToken ct = default);
```

Flow:

1. **Idempotency guard.** If any debt already exists for this source — keyed on
   `(Origin, OriginRequestId)` for requests or `(Origin, OriginTechnicalExamId)` for tech-exams —
   return `0` immediately. (It does **not** reconcile changes; re-running is a clean no-op. This is
   the in-app counterpart to the `LegacyId`-unique idempotency used by the sync.)
2. Load the `ClientVehicleRelation`; if missing → `0`. Load its `Vehicle` (if any).
3. `companyId = _tenant.CompanyId ?? (byte)4` (4 is the Велес station fallback).
4. `EvaluateAsync(...)`; if no matches → `0`.
5. Insert one `CustomerDebt` per match, snapshotting `Price` + `VatPercent`, stamping `Origin`,
   the right origin id (`OriginRequestId` xor `OriginTechnicalExamId`), `OrganizationId`,
   `CreatedByUserId = _tenant.UserId`, `Paid = false`, `Active = true`.
6. One `SaveChangesAsync`; return the count.

---

## 7. Auto-debt hooks on workflow completion

Both hooks are **best-effort** — wrapped in `try/catch` so a debt failure never rolls back the source
document. (CLAUDE.md / code comments note Phase 5 will move these to a transactional outbox.) They
fire only on **Insert**, matching the legacy `DataPortal_Insert` semantics (not on update).

### Technical exam — `TechnicalExamReportsController.Create`

`backend-v2/src/VTE.Api/Controllers/TechnicalExamReportsController.cs:707-730`. Fires only when a
vehicle relation is anchored (orphan exams have no one to charge):

```csharp
var isIrregular = report.TechnicalExamTypeId > 1;   // legacy: type=1 is regular (gotcha #10)
await _debts.CreateDebtsForSourceAsync(
    origin:  isIrregular ? DebtOrigin.TechnicalExamIrregular : DebtOrigin.TechnicalExam,
    originId: report.Id,
    customerVehicleRelationId: report.CustomerVehicleRelationId.Value,
    organizationId: report.OrganizationId,
    trigger:  isIrregular ? PriceTrigger.TechnicalExamIrregular : PriceTrigger.TechnicalExam,
    communityId: null,
    note: $"технички преглед бр. {report.RegNumber}");
```

Irregular exams (`TechnicalExamTypeId > 1`) route to the Irregular origin + trigger (gotcha #10).
`communityId` is intentionally `null` for tech exams — only requests resolve a community.

### Request — `RequestsController.Create`

`backend-v2/src/VTE.Api/Controllers/RequestsController.cs:334-389`. Three business rules:

- **BR-DEBT-1 — ownership-transfer routing (gotcha #11).** For `type.TransfersOwnership` requests
  with a `NewClientVehicleRelationId`, debts go to the **new** owner relation, not the anchor
  (legacy `Request.vb:856-861`):

  ```csharp
  var debtRelationId = (type.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue)
      ? entity.NewClientVehicleRelationId.Value
      : entity.ClientVehicleRelationId;
  ```

  (The "Нов сопственик" is a free client search resolved to a relation by
  `ResolveOrCreateNewOwnerRelationAsync` — see gotcha #17 in CLAUDE.md and the
  [Requests doc](03-requests.md).)

- **BR-DEBT-2 — community filter (gotcha #12).** Resolve the customer's living community via
  `relation → client.CityId → city.CommunityId` and feed it to the evaluator. Rules with a non-null
  `CommunityId` only fire when this matches; NULL-community rules always fire.

- **Org inheritance.** Inherit `OrganizationId` from the linked tech-exam when present, else `0`.
  (The auto-exam stub is created *before* this block specifically so the request fee inherits the
  new exam's org.)

```csharp
await _debts.CreateDebtsForSourceAsync(
    origin: DebtOrigin.Request, originId: entity.Id,
    customerVehicleRelationId: debtRelationId, organizationId: orgId,
    trigger: PriceTrigger.Request, communityId: communityId,
    note: $"по барање бр. {entity.Id}");
```

> **Not yet wired:** `TrafficLicence`, `Permission`, `InternationalDrivingLicence` triggers/origins
> exist in the enums but have no v2 workflow to fire them (those modules don't exist in v2 yet).

---

## 8. Read API — `CustomerDebtsController`

`backend-v2/src/VTE.Api/Controllers/CustomerDebtsController.cs` (`/api/customer-debts`). Backs the
Наплата panel. v2 mirror of `CustomerFinancialState`.

| Method | Route | Notes |
|---|---|---|
| GET | `/api/customer-debts` | Paged list. `customerVehicleRelationId?`, `unpaidOnly=true` (default), `page`, `pageSize` (≤500, default 100). **Sorted by `CustomerVehicleRelationId, Id`** so the frontend can group adjacent rows in one pass. |
| GET | `/api/customer-debts/summary` | `{ count, total }` over active + unpaid — the dashboard pill. |
| DELETE | `/api/customer-debts/{id}` | Soft-delete (`Active=false`). **Refuses a paid debt** → `400 "Веќе платена ставка не може да се избрише — потребно е сторно."` |
| POST | `/api/customer-debts/delete-batch` | Bulk soft-delete (≤500 ids). Skips paid rows; returns `{ requested, deleted, skippedPaid, skippedMissing }`. |

The list endpoint batch-resolves joins (relation → client + vehicle, price-catalog name, maker/model
labels) to avoid N+1, and runs every debt name through `FormatPriceName` (see [§9](#9-наплата-line-names--01-substitution)).
All queries inherit the tenant query filter — you only see/delete debts in your own company.

---

## 9. Наплата line names — `{0}`/`{1}` substitution

Legacy price-item names are `String.Format` templates with `{0}` / `{1}` placeholders for the matched
parametar range (e.g. `"за носивост од {0} до {1}"`). The controller substitutes the **rule's own**
`ParametarFrom`/`ParametarTo` (rendered as whole numbers) so the panel shows the real range
(`"…од 3001 до 5000"`), exactly like the legacy app
(`CustomerDebtsController.cs:47-53`):

```csharp
private static string? FormatPriceName(string? name, double? from, double? to)
{
    if (string.IsNullOrEmpty(name)) return name;
    if (name.Contains("{0}")) name = name.Replace("{0}", from.HasValue ? ((long)from.Value).ToString() : "");
    if (name.Contains("{1}")) name = name.Replace("{1}", to.HasValue ? ((long)to.Value).ToString() : "");
    return name;
}
```

It deliberately uses `Replace`, **not** `String.Format`, so a stray `{` in a name can't throw.

---

## 10. The Наплата dashboard panel

`frontend-v2/src/views/DashboardView.vue`. The top-right card of the dashboard. Compact ledger of
open customer debts grouped client → vehicle (legacy `CustomerFinancialState`).

**Load (legacy parity).** The panel fetches `unpaidOnly=false, pageSize=200` — it's a *ledger of all
active charges*, not just unpaid ones. The summary pill uses `/summary` (active + unpaid only):

```ts
api.get('/customer-debts', { params: { unpaidOnly: false, pageSize: 200 } });
api.get('/customer-debts/summary');
```

**Grouping (`debtGroups` computed).** Rows arrive pre-sorted by relation, so grouping is a single
forward pass: start a new group whenever `customerVehicleRelationId` changes. Each group sums only
**unpaid** rows into `total` and counts them in `unpaidCount`.

**Hide fully-paid clients.** `debtGroups` ends with `.filter(g => g.unpaidCount > 0)`. Legacy parity:
a client appears in Наплата only while it has ≥1 unpaid debt. Paid rows still show *inside* an
otherwise-open client (billing one item doesn't make it vanish, and they render with a green check
+ "Платено" tag), but a **fully-settled client drops off entirely**.

**Selection.** Per-row checkboxes appear only on **unpaid** rows. Group checkbox is tri-state
(`groupSelectState` → `true` / `'indeterminate'` / `false`) and toggles all unpaid rows in the group.
Selection that no longer exists (settled/deleted in another tab) is pruned on refresh.

**Auto-refresh.** A `visibilitychange` listener re-fetches debts when the tab regains focus — covers
the "save a tech-exam in another tab → come back to dashboard" loop.

**Bulk delete.** When any rows are selected a danger "Избриши (n)" button calls
`POST /customer-debts/delete-batch`; a confirm dialog gates it; the toast reports
`deleted` and, if any, `skippedPaid`.

**Per-group navigation.** Clicking a group's client name routes to
`/payments?customerVehicleRelationId={relationId}` (that client's bill register).

---

## 11. Bill creation — "Направи сметка"

`POST /api/payment-documents/from-debts`
(`backend-v2/src/VTE.Api/Controllers/PaymentDocumentsController.cs:343-514`). Turns selected open
debts into one `PaymentDocument` + a line per debt. Driven from the Наплата panel's
`openBillDialog`/`createBill` (`DashboardView.vue:157-218`).

### Request

```csharp
record CreateFromDebtsRequest(
    IReadOnlyList<long> DebtIds, int PaymentTypeId, string? Note, DateTime? DueDate,
    int? Installments = null, decimal? FirstInstallmentAmount = null,   // installment ("по договор")
    string? GuarantorName = null, string? GuarantorAddress = null, string? GuarantorEmbg = null);
```

### Validation (in order)

- `DebtIds` non-empty, ≤ 100.
- `PaymentType` exists + active.
- All requested debts exist (count matches), all `Active`, none `Paid`.
- **Single client/vehicle per bill** — every debt must share the first row's
  `CustomerVehicleRelationId`, else `400` (the frontend enforces this too: the *Направи сметка*
  button is disabled unless `selectedRelationIds.size === 1`).
- For installment types: `Installments ∈ [2, 36]` and `0 < FirstInstallmentAmount < billTotal`.

### Document numbering

Inside a **`Serializable`** transaction so two concurrent bills can't draw the same sequence. Format
is the legacy `[prefix-]{org}-{seq}/{year}`. The next `seq` is the max of the segment between the last
`-` and the `/`, scoped to `(OrganizationId, PaymentTypeId, /{year})`, via raw SQL with `TRY_CAST`
(skips legacy oddballs) — continuing the migrated numbering:

```sql
SELECT MAX(TRY_CAST(LEFT(tail, CHARINDEX('/', tail) - 1) AS bigint))
FROM ( SELECT RIGHT(DocumentNumber, CHARINDEX('-', REVERSE(DocumentNumber)) - 1) AS tail
       FROM dbo.PaymentDocument
       WHERE OrganizationId = {organizationId} AND PaymentTypeId = {req.PaymentTypeId}
         AND CHARINDEX('-', DocumentNumber) > 0 AND CHARINDEX('/', DocumentNumber) > 0
         AND DocumentNumber LIKE {"%/{year}"} ) t
WHERE CHARINDEX('/', tail) > 1
```

### Side effects

1. Insert `PaymentDocument`: `IssueDate = now`; `DueDate = req.DueDate ?? (IsCash ? now : now+15d)`;
   **`Paid = type.IsCash`** (cash settles on the spot); `CreatedByUserId = _tenant.UserId`.
2. Insert one `PaymentDocumentLine` per debt, snapshotting `UnitPrice = debt.Price` and
   `VatPercent` (re-resolved from the catalog's current `VatRate` at billing time), `Quantity = 1`,
   `CustomerDebtId = debt.Id`.
3. Mark each debt `Paid = true`, `SettledByLineId = line.Id`.
4. (Installments — see [§12](#12-installments-по-договор).)
5. `tx.Commit`. Returns `CreateBillResponse(Id, DocumentNumber, LinesTotal, Lines)`.

`billTotal` is summed with **whole-denar banker's rounding**
(`Math.Round(d.Price, 0, MidpointRounding.ToEven)`).

### Frontend follow-through (`createBill`)

On success the dialog closes, selection clears, debts refresh, and the page navigates to
`/payments/{id}`. A fiscal receipt prints immediately (legacy parity — for installment bills it's the
down-payment, rata 1) via `printFiscalForDocument(id, false, isRati ? 1 : undefined)`; the panel still
has a manual print button.

### PaymentType dedup gotcha (#14)

The legacy `PaymentTypes` table repeats each type once **per company** (no company column), and the
same fee can carry a different doc-number `Prefix` per company. `GET
/api/payment-documents/payment-types?usedOnly=true` (`PaymentDocumentsController.cs:712-740`) groups
by **trimmed `Name`** (legacy names have stray leading spaces) and keeps, per name, the id whose
**most recent bill is newest** (`Max(d.Id)`) — i.e. the one the station currently bills with. Grouping
by Name *alone* (not Name+Prefix) is deliberate: "по договор" exists under several prefixes and must
collapse to one choice. **Most-recent beats most-total** — an id can have more lifetime docs yet have
been retired years ago (hit with "со кредитна картичка" 19 vs 24, "по договор" 18 vs 23). The
create-bill dialog calls this with `usedOnly: true`; the bill register filter uses the un-deduped
full list.

---

## 12. Installments ("по договор")

When the chosen `PaymentType.IsInstallment`, `from-debts` also creates an `InstallmentAgreement`
(legacy `DogovorZaRati`) + an `InstallmentSchedule` per rata (legacy `PaymentDocumentsRata`)
inside the same transaction (`PaymentDocumentsController.cs:461-507`):

- **Agreement** — `Number = "{seq}/{year}"`, `Date = today`, `TotalInstallments = req.Installments`,
  guarantor name/address/EMBG (trimmed). The document's `Paid` is reset to `false` and `AgreementId`
  is linked.
- **Schedule** — Rata **1 = the down payment**, settled immediately (`Paid = true`, `PaidAt = now`,
  `PaidAmount = first`). The remainder (`billTotal - first`) splits into equal whole-denar monthly
  installments with `per = Math.Floor(remainder / (n-1))`; **the last rata absorbs the rounding
  remainder** (`remainder - per*(n-2)`). Each rata `i` is due `today.AddMonths(i-1)`; the document's
  `DueDate` becomes the last rata's due date.

### Paying a rata + per-rata receipts

`POST /api/payment-documents/{id}/installments/{seq}/pay`
(`PaymentDocumentsController.cs:520-543`): marks the rata paid (`PaidAmount = Amount`); when the last
open rata closes, the **master document flips to `Paid`**. Refuses an already-paid rata or a
stornoed document.

Fiscal: `GET /api/payment-documents/{id}/fiscal-file?installment={seq}` emits a single
"Uplata po rata" line at 0% VAT for the paid amount (legacy
`PecatiFiskalnaSmetaZaRataAccentPF500`). The bill detail view (`PaymentView.vue`) renders the
schedule with a "Плати рата" button per open rata and quietly prints its receipt on payment.

---

## 13. Bill register + detail (frontend)

- **`PaymentsView.vue`** (`/payments`, `GET /api/payment-documents`) — paged, searchable register:
  status tabs (`all`/`paid`/`unpaid`/`storno`), free-text search (doc number, client name/MB, plate,
  VIN), payment-type filter, sortable by issue/due/doc. Deep-linkable by
  `?customerVehicleRelationId=` from the Наплата panel.
- **`PaymentView.vue`** (`/payments/:id`, `GET /api/payment-documents/{id}`) — full bill: parties,
  lines (with VAT/discount/subtotal), installment agreement + schedule (with per-rata pay buttons),
  audit block, status tag (paid/unpaid/storno), and the fiscal "Печати сметка" button +
  `fiscalPrintedAt` marker.

### Fiscal printing (Accent PF-500)

For the full fiscal protocol (file format, byte layout, transliteration), see [Fiscal printing](06-fiscal.md).

`GET /api/payment-documents/{id}/fiscal-file` composes the exact command-file content the legacy
Accent PF-500 file-exchange driver expects (`FiskalModule.vb`'s `PecatiFiskalnaSmetaAccentPF500`); the
browser writes it into the operator's configured fiscal folder via the File System Access API
(`frontend-v2/src/fiscal/fiscal.ts`). Gates: only `IsCash` types fiscalize; zero-amount bills skip;
an unknown VAT rate is refused loudly (`400`) rather than silently truncated as legacy did. VAT class
byte: 18% → 192 (А), 5% → 193 (Б), 0% → 194 (В); names are transliterated Cyrillic→Latin (`ToLat`).
`POST /api/payment-documents/{id}/fiscal-printed` is the idempotent outbox marker (keeps the first
`FiscalPrintedAt`). `printFiscalForDocument` returns one of:
`printed` / `skipped` / `no-folder` / `unsupported` / `error`.

---

## 14. Legacy-sync state reconciliation (guarding v2 payments)

`migrate/migrate-incremental.sql` §10 keeps the Наплата ledger in sync with live legacy
`CustomerFinancialState`. Unlike the additive sections, this is a **state sync**:

- **(a) INSERT** every *open* legacy debt (`Payed=0 AND Active=1`) not yet imported — idempotent via
  the unique-filtered `CustomerDebt.LegacyId`. Historical *paid* rows are **not** imported (that
  history already lives in `PaymentDocument*`; Наплата only shows open items). Origin is mapped from
  the legacy `IdDocumentTehnicalExam` / `IdDocument` / traffic-licence / permission / IDL columns to
  the `DebtOrigin` enum.
- **(b) UPDATE** `Paid`/`Active` on previously-imported rows so debts settled or stornoed in legacy
  close here too.

**The v2-payment guard (task #99):** the update touches only rows where `LegacyId IS NOT NULL` **and
`SettledByLineId IS NULL`**. A debt billed *natively in v2* (so it has a `SettledByLineId`) is never
re-opened just because legacy still shows it unpaid — **v2 is authoritative for its own payments**.
v2-native debts (`LegacyId IS NULL`) are never touched at all.

```sql
UPDATE d SET d.Paid = s.Payed, d.Active = s.Active
FROM dbo.CustomerDebt d
INNER JOIN VTEZVV_LIVE.VTEZVV.dbo.CustomerFinancialState s ON s.Id = d.LegacyId
WHERE d.LegacyId IS NOT NULL
  AND d.SettledByLineId IS NULL          -- never re-open a debt we billed in v2
  AND (d.Paid <> s.Payed OR d.Active <> s.Active);
```

(`LegacySync` is disabled in prod — `LegacySync__Enabled=false` in `/opt/vte/.env` — but the
incremental script remains the live top-up path.)

---

## 15. Supporting payment entities

All under `backend-v2/src/VTE.Domain/Payments/`.

| Entity | File | Role | Legacy (rows) |
|---|---|---|---|
| `PaymentDocument` | `PaymentDocument.cs` | Bill/receipt/invoice master. `RowVersion` concurrency token; `FiscalPrintedAt` outbox marker; `Stornoed`+`StornoReason`. | `PaymentDocuments` (244,192) |
| `PaymentDocumentLine` | `PaymentDocumentLine.cs` | One billable line; snapshots `UnitPrice`+`VatPercent`; optional `CustomerDebtId` it settles. | `PaymentDocumentsDetails` (1,166,083) |
| `InstallmentAgreement` | `InstallmentAgreement.cs` | Договор за рати — guarantor + total installments. | `DogovorZaRati` (6,796) |
| `InstallmentSchedule` | `InstallmentSchedule.cs` | One scheduled rata; `SequenceNo`, `DueDate`, `PaidAmount` (new in v2). | `PaymentDocumentsRata` (254,348) |
| `PaymentType` | `PaymentType.cs` | Cash/card/installment/invoice + print flags + `Prefix`. | `PaymentTypes` (30) |
| `VatRate` | `VatRate.cs` | VAT catalog (0% / 5% / 18%). | `DDVCatalog` (3) |

EF notes (`VteDbContext.cs:589-696`): `PaymentDocument`/`Line`/`Schedule`/`Agreement` are
tenant-scoped; lines + schedules cascade-delete with their document; `PaymentType`/`VatRate`/
`PriceCatalog` are cross-tenant (no query filter). `decimal(18,4)` for all money columns.

---

## 16. Cheat sheet — gotchas touching this module

| # | Gotcha (CLAUDE.md) | Where it shows up here |
|---|---|---|
| 7 | Active cascade across all 3 legacy levels | [§1](#active-cascade-rule-gotcha-7), `fix-pricecatalog-active-cascade.sql` |
| 8 | Sentinel multiplier (`From=0,To=0,Field≠Null` → `Price*field`) — **deferred** | [§5](#5-pricingevaluator--reflection-based-rule-matching) |
| 9 | "Оперативни трошоци" is `TrigerdByRequest`, not TechExam | trigger precedence, `backfill-pricecatalog-rules.sql` |
| 10 | Irregular exam (`TechnicalExamTypeId > 1`) → Irregular origin+trigger | [§7](#technical-exam--technicalexamreportscontrollercreate) |
| 11 | Ownership transfer routes debts to `NewClientVehicleRelationId` | [§7](#request--requestscontrollercreate), BR-DEBT-1 |
| 12 | Community filter via `relation → client → city.CommunityId` | [§7](#request--requestscontrollercreate), BR-DEBT-2 |
| 13 | Single-`Trigger` limit (multi-flag legacy rules need splitting) | [§2](#backfilling-the-rule-columns) |
| 14 | Duplicate `PaymentType` ids per company → dedup by trimmed name + newest bill | [§11](#paymenttype-dedup-gotcha-14) |
