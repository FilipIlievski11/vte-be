# Print system (МВР forms)

This document describes how VTE prints a finished **Барање** (request) onto the
pre-printed colored government paper supplied by the Ministry of Interior (МВР —
Министерство за внатрешни работи). The browser does **not** draw the form boxes,
labels, or borders — those are already printed on the physical sheet the operator
feeds into the printer. VTE only overlays the *values* (plate, owner name, VIN,
engine data, dates…) at the exact millimetre coordinates where the blank fields
sit on the paper. Positions were lifted directly from the legacy DevExpress
`.prnx` rendered output, so a v2 printout lands on the same lines the legacy
VB.NET WinForms app produced.

Three colored forms exist, keyed by `RequestType.documentPrintCode`:
**ЗЕЛЕН** (green, `ZELEN`), **ПЛАВ** (blue, `PLAV`), and **БЕЛ** (white, `BEL`).
The green and blue templates are implemented and calibrated; the white one is
still pending (see [Bel (#70 — pending)](#bel-белwhite--70-pending)).

For the technical-exam paper prints (Записник, certificate), which use the same
overlay technique on a different government form, see
[Technical exams](04-technical-exams.md).

---

## 1. Entry point and template selection

The print page is `frontend-v2/src/views/print/LegacyPaperPrint.vue`, routed at
**`/requests/:id/print`** (the *default* print route —
`frontend-v2/src/router/index.ts`):

```ts
// DEFAULT print view — feeds onto pre-printed MVR government paper.
{ path: '/requests/:id/print', name: 'request-print',
  component: () => import('@/views/print/LegacyPaperPrint.vue') },
// A self-contained, fully-styled rendering (draws its own boxes/labels)
// for cases where pre-printed paper isn't available.
{ path: '/requests/:id/print/styled', name: 'request-print-styled',
  component: () => import('@/views/RequestPrintView.vue') },
```

> There are two print routes. `LegacyPaperPrint.vue` is the **overlay-on-paper**
> one this document covers. `RequestPrintView.vue` is a separate, self-contained
> rendering that *draws its own form chrome* (boxes/labels) — used when blank
> pre-printed paper isn't on hand. They share the same `RequestPrintBundle` DTO
> but otherwise are unrelated.

On mount, `LegacyPaperPrint.vue` fetches the print bundle and picks a template by
normalizing the document-print code to one of `PLAV` / `BEL` / `ZELEN`:

```ts
const formKind = computed<'PLAV' | 'BEL' | 'ZELEN' | 'OTHER'>(() => {
  const c = (bundle.value?.type.documentPrintCode || '').toUpperCase();
  if (c.includes('ZELEN') || c.includes('ЗЕЛЕН')) return 'ZELEN';
  if (c.includes('PLAV')  || c.includes('ПЛАВ'))  return 'PLAV';
  if (c.includes('BEL')   || c.includes('БЕЛ'))   return 'BEL';
  return 'OTHER';
});
```

### Why both Latin AND Cyrillic codes are matched

The code can arrive in either alphabet depending on how the request type got into
the DB:

| Source | `documentPrintCode` value | Where set |
|---|---|---|
| Fresh seed (new installs) | `PLAV`, `BEL`, `ZELEN` (Latin) | `backend-v2/src/VTE.Api/Seed/DataSeeder.cs` (`new RequestDocumentPrint { Code = "PLAV", … }`) |
| Migrated production data | `ПЛАВ`, `БЕЛ`, `ЗЕЛЕН` (Cyrillic) | `migrate/migrate-request-catalogs.sql` derives `Code` from the legacy `DocumentTypePrint.Opis`: `LEFT(UPPER(REPLACE(LTRIM(RTRIM(p.Opis)), N' ', N'_')), 20)` |

So a production DB carries Cyrillic codes (sluggified from the Macedonian
`Opis`), while a freshly-seeded dev DB carries Latin codes. `formKind` matches
both. **Order matters**: `ZELEN`/`PLAV` are checked before `BEL` so the substring
`BEL` (which appears inside no other code, but the comment flags the principle)
can't shadow them.

The dispatch in the template:

```vue
<PlavTemplate  v-if="formKind === 'PLAV'"  :bundle :showGuides :editMode />
<ZelenTemplate v-else-if="formKind === 'ZELEN'" … />
<ZelenTemplate v-else … />   <!-- OTHER / BEL fall back to Zelen + a warning -->
```

When `formKind` is `BEL` or `OTHER`, the page renders the **Zelen** template as a
fallback and shows a yellow note (`⚠ Шаблонот за {code} сè уште не е калибриран`).
This is a stopgap until `BelTemplate.vue` is built.

### Toolbar / chrome (no-print)

`LegacyPaperPrint.vue` wraps the chosen template in a dark fixed toolbar that is
hidden at print time (`.no-print { display: none !important }` inside
`@media print`). Controls:

| Control | Behavior |
|---|---|
| **Покажи мрежа / Скриј мрежа** (show/hide grid) | toggles a 5mm blue grid overlay + per-field red label tags, for visual calibration (`showGuides`) |
| **Уреди позиции / ✓ Уредувам** (edit positions) | drag-mode toggle (`editMode`) — see [§7 Edit-mode](#7-edit-mode-position-calibration) |
| **Изнеси JSON** (export JSON) | (edit mode only) `downloadPositions()` — downloads the current positions as JSON |
| **Врати ги почетните** (reset) | (edit mode only) `resetPositions()` after a `confirm()` — clears localStorage overrides, restores `DEFAULT_POS` |
| **Печати** (print) | `doPrint()` — turns off edit+guides, then `window.print()` after a 50 ms `setTimeout` (label is `requests.print.printAgain` = "Печати") |
| **Затвори** (close) | `doClose()` → `window.close()` |

The `@page` rule sets `size: A4 portrait; margin: 0` so the browser doesn't add
its own margins that would shift every field off the pre-printed boxes.

---

## 2. The millimetre coordinate system (px → mm)

The legacy app rendered each form with DevExpress XtraReports at **300 DPI**, so
an A4 page is `2481 × 3507 px` = `210 × 297 mm`. Every position in the templates
is the rect from that rendered `.prnx`, converted to millimetres:

```
mm = px / 11.811        (because 2481 px / 210 mm = 11.811 px per mm)
```

This constant appears in three places:

- `frontend-v2/scripts/parse-prnx.ps1` — `$PX_PER_MM = 11.811`, with
  `Conv([double]$px) { [math]::Round($px / $PX_PER_MM, 1) }`.
- The big header comments in `ZelenTemplate.vue` and `PlavTemplate.vue`.
- The source XML page rect itself: `scripts/printZelen.xml` →
  `<Page1 Rect="0,0,2481,3507">`.

Each positioned field is an absolutely-positioned `<span class="f">` whose
`left/top/width/height` are set in `mm` via inline style (`fieldStyle()`), inside
a `210mm × 297mm` `.sheet`. The sheet uses `font-family: 'Arial'` (the legacy
form font — `scripts/printZelen.xml` shows `Font="Arial, 9.75pt"`) and
`text-transform: uppercase` (the legacy print is all-caps regardless of how the
data is stored).

### Folding panel offsets

DevExpress reports nest controls inside panels, whose child rects are *relative*
to the panel origin. To get an **absolute** page coordinate you must add the
parent panel offsets. `parse-prnx.ps1` does this recursively (`Walk` accumulates
`$ox/$oy`); the templates have the folded result baked in. Example from
`ZelenTemplate.vue` page 2:

```
// Outer panel offset = (1074.80 / 11.81, 0)            = (91.0, 0)
// Inner panel offset = (75.59 / 11.81, 193.70 / 11.81) = (6.4, 16.4)
// Combined offset    = (97.4 mm, 16.4 mm) — already folded into the values below.
```

### The layout-JSON manifests (`print-layout-*.json`)

There are two generations of layout data, and it's important not to confuse them:

1. **`print-layout-{plav,bel,zelen}.json`** (in both `scripts/` and
   `frontend-v2/src/views/print/`) — an earlier manifest parsed from the
   DevExpress **`Designer.vb`** files. Coordinates are in **0.1mm units**
   (legacy designer DPI = 254, so `x_mm = x / 10`), keyed by the original
   DevExpress control names (`lblType`, `XrLabel4`, `CheckBoxVehicleCategory1`,
   `XrPanel2`, …). These were the first attempt and are consumed by
   `legacy-field-map.ts` (control-name → value resolver). The Zelen JSON, for
   instance, shows a page-2 `XrPanel2` at `(874, 2977)` 0.1mm with child labels
   like `lblEngineType`, `lblShellNumber`, `lblNewSurname`, etc.

2. **The `DEFAULT_POS` objects hand-coded inside `ZelenTemplate.vue` /
   `PlavTemplate.vue`** — the *authoritative, calibrated* positions, taken from
   the **rendered `.prnx`** (`scripts/printZelen.xml`, `Downloads/PrintPlav.xml`)
   in **mm**, then nudged for pixel-perfect parity against real printouts. These
   are what actually drive the live templates.

`legacy-field-map.ts` (the control-name resolver against the Designer JSON) is
**not** wired into the live Zelen/Plav templates — those use their own inline
`v` resolvers (§4). The field map remains the bridge that *could* feed a future
generic JSON-driven renderer, but it mostly covers control names shared across
the forms; it is **not** a complete resolver for the white **Bel** form (whose
template doesn't exist yet, and whose Designer manifest is the sparsest of the
three — see [Bel (#70 — pending)](#bel-белwhite--70-pending)).

---

## 3. The print bundle DTO — `GET /requests/{id}/print`

The backend assembles everything the templates need into a single response.
Endpoint: `RequestsController.PrintBundle` →
`backend-v2/src/VTE.Api/Controllers/RequestsController.cs`
(`[HttpGet("{id:long}/print")]`). The response shape (`PrintBundleDto`):

| Field | Type | Source / meaning |
|---|---|---|
| `request` | `RequestReadDto` | request header (id, dates, `legacyReferenceNumber`, note, flags) — built inline, mirror of `Get()` |
| `type` | `RequestTypeMeta` | request type + its `RequestDocumentPrint` (`documentPrintCode`, `documentPrintName`) + workflow flags |
| `client` | `ClientMeta` | the **current/anchor** owner (from `ClientVehicleRelation.ClientId`) |
| `vehicle` | `VehicleMeta?` | the anchor relation's vehicle, fully denormalized |
| `newVehicle` | `VehicleMeta?` | vehicle of the new-owner relation (transfers) — usually the same vehicle |
| `newClient` | `ClientMeta?` | the **new** owner (`Request.NewClientVehicleRelationId`) — see resolution note below |
| `ownershipProofs` | `ProofMeta[]` | active `RequestOwnershipProofs` (type name + detail), ordered by id |
| `paymentProofs` | `ProofMeta[]` | active `RequestPaymentProofs`, ordered by id |
| `company` | `CompanyMeta` | submitting company (id, name, communityName — currently null) |
| `lastRegistration` | `RegistrationMeta?` | **newest** registration by `ValidUntil` then `RegisteredDate` — on a transfer this is the *newly-issued* row |
| `previousRegistration` | `RegistrationMeta?` | the registration whose plate equals the vehicle's stored `Plate` — the **OLD** plate on a transfer |

`RegistrationMeta` = `(Id, PlateNumber, RegisteredDate, ValidUntil, Issuer)`.
`ProofMeta` = `(Id, TypeName, Detail)`.

### Per-field resolvers in the backend

The bundle is built by several private helpers in `RequestsController.cs`, each
denormalizing a foreign-keyed value to a printable string:

- **`BuildClientMeta(clientId)`** — joins Client → City → Community → Country and
  Citizenship. Notably it resolves two community-derived print fields:
  - `CommunityRegistrationCode` = `Communities.PlateNumberPrefix` (e.g. `"VE"`),
    used to build the Plav new-registration prefix `"VE-"`.
  - `CommunityRegistrationIssuer` = the first **active** `DocumentIssuer` for the
    client's community, ordered by id (mirror of legacy
    `GetRegistrationIssuerInfoByCommunity`) — the destination МВР office name
    (e.g. `"МВР ВЕЛЕС"`). **Consumed only by Plav** (`toMvr` + page-3
    `newRegIssuer`). Zelen ignores this field and instead derives its `toMvr` as
    `` `МВР ${communityName}` `` directly in the template (see §4).

  Both `CommunityRegistrationCode` and `CommunityRegistrationIssuer` are resolved
  from the **City → Community** join: `City.CommunityId` → `Communities.PlateNumberPrefix`
  and → `DocumentIssuers` filtered by `CommunityId` + `Active`, ordered by `Id`.
- **`BuildVehicleMeta(vehicleId)`** — joins model→maker, body type, category,
  payment category (incl. `ZelenMap`), two colors (name+code), fuel, engine type
  (name+code), eco program, made-country, and copies the numeric tech specs.
  Two formatting parities baked in here:
  - Body type rendered as `"{Code}  {Name}"` (two spaces) — e.g.
    `"AB  VOZILO SO PODVIZHNA ZADNA VRATA"`.
  - Category `J` rendered as `"{Code} {Name}"` — e.g. `"M1 ПАТНИЧКО ВОЗИЛО"`.
- **`BuildLastRegistrationMeta(vehicleId)`** — newest active registration by
  `ValidUntil`, then `RegisteredDate`.
- **`BuildPreviousRegistrationMeta(vehicleId, vehiclePlate)`** — prefers the
  registration row whose `PlateNumber` matches the vehicle's stored `Plate`;
  falls back to the second-newest row (the one before the newly-issued reg), or
  the only row if there's just one.

### New-owner (`newClient`) resolution quirk

`Request.NewClientVehicleRelationId` is overloaded in legacy data (see CLAUDE.md
gotcha #11/#17): it holds a *customer* id on migrated rows but a *relation* id on
v2-native rows. `PrintBundle` disambiguates:

```csharp
// if the value is a CLIENT that has a relation on THIS vehicle → migrated customer id
bool isClientOnVehicle = await _db.ClientVehicleRelations.AsNoTracking()
    .AnyAsync(x => x.VehicleId == newVid && x.ClientId == newRef);
if (isClientOnVehicle) { newClientMeta = await BuildClientMeta(newRef); … }
else { /* treat newRef as a relation id, take its client */ }
```

This covers both true transfers (`newClient` is a different person) and plain
owner-data changes (`newClient` resolves back to the page-1 client).

---

## 4. Per-field value resolvers (frontend)

Each template has a `v = computed(() => ({ page1: {…}, page2: {…} }))` object —
one entry per field key, mapping the bundle to the printed string. Keys line up
1:1 with the `DEFAULT_POS` position keys; the template iterates the position map
and renders `v.pageN[key]` inside each positioned span. Shared helpers:

- `fmtDate(s)` → `DD.MM.YYYY` (returns `''` for null/invalid).
- `num(v)` → rounded integer string (`''` for null).
- `numZ(v)` (Plav) → like `num` but renders `0` instead of blank.
- `dec(v)` (Zelen) → decimal with Macedonian comma, no rounding (`3.3 → "3,3"`),
  used for engine power P.2 which the legacy printout shows fractionally.

Naming gotcha used by **every** resolver: in the Macedonian data,
`client.firstName` actually holds the **surname** (Презиме) and `client.lastName`
holds the **given name** (Име). The resolvers swap them on the way out.

### Notable computed fields (both templates)

| Field | Resolution logic (source) |
|---|---|
| **Plate** | prefer `vehicle.plate` over the registration child's `plateNumber` (the child often carries a sentinel). Collapse `"{Code}-000-AA"` → `"{Code}-"` (legacy sentinel for a vehicle with no real plate yet). |
| **toMvr (ДО МВР)** | **The two templates resolve this differently.** Zelen builds the string in-template as `` `МВР ${client.communityName ?? client.cityName}` `` (e.g. `"МВР ВЕЛЕС"`) — it does **not** read the bundle's `communityRegistrationIssuer`. Plav uses `regOwner.communityRegistrationIssuer` (the new owner's community `DocumentIssuer`, from the backend) verbatim, blank when the community has no issuer. |
| **Address** | prefix `"УЛ. "` (= улица/street). Zelen always prefixes; Plav's `formatAddress()` only prefixes when the string contains a digit (rural settlements with no street number print as-is, mirroring legacy `PrintCustomerInfo.vb`). |
| **Ownership / payment proofs** | join **all** rows with `"; "` (e.g. `"СООБРАЌАЈНА ДОЗВОЛА 2446652; ДОГОВОР"`), not just the first — matching the legacy single label. |
| **D.3 Комерцијална ознака** (Zelen) | `[model, variant].join(' ')`, with `" TNG"` appended when `vehicle.hasLpg` (LPG). |
| **Year of manufacture** | `new Date(vehicle.manufactureDate).getFullYear()` — year only, from legacy `MakeDate`. |
| **Color** | `[primaryColorCode, primaryColorName].join(' ')`. |

---

## 5. The reference number (Број на барање)

The reference number printed at the bottom of every form is the legacy
**`RegNumberDolg`** value. Its formula (documented in
`migrate/backfill-request-reference-no.sql`, decoded from legacy
`PrintZelenInfo.vb` / `PrintPlavInfo.vb`):

```
{StationCode}{IdTechnicalExamReport}{OperatorId}/{Year(DateCreated)}
```

Worked example for request 210483 → `167403128/2026`:

| Segment | Value | Source |
|---|---|---|
| `StationCode` | `1` | `TehnicalExamOrganizations.Code` for VELES (hard-coded `'1'` in the backfill, since only that station was imported) |
| `IdTechnicalExamReport` | `67403` | `Requests.IdTechnicalExamReport` |
| `OperatorId` | `28` | `Requests.IdOperatorCreated` (the **creating** operator) |
| `Year` | `2026` | `YEAR(Requests.DateCreated)` |

The backfill stores this as `Request.LegacyReferenceNumber` for every migrated
request (it first restores `TechnicalExamReportId` from the legacy snapshot,
which `migrate-requests.sql` had nulled). The templates print
`request.legacyReferenceNumber` verbatim when present.

### Why the operator segment is unrecoverable for new requests

For **Plav specifically**, legacy `RegNumberDolg` used the **printing** employee
id (resolved at print time), not a value stored on the request — so the exact
legacy string can't be reproduced for a post-cutover request. The fallbacks:

- **Zelen** (`ZelenTemplate.vue`): `legacyReferenceNumber || "{requestId}/{year}"`.
- **Plav** (`PlavTemplate.vue`): `legacyReferenceNumber`, else parse the legacy
  operator id from the migration note `"C=NNN"` and emit `"0{op}/{year}"`
  (Plav has no exam report, so the report segment is `0`), else
  `"{requestId}/{year}"`:

```ts
const referenceValue = computed(() => {
  if (b().request.legacyReferenceNumber) return b().request.legacyReferenceNumber;
  const year = new Date(b().request.createdAt).getFullYear();
  const m = (b().request.note || '').match(/C=(\d+)/i);
  return m ? `0${m[1]}/${year}` : `${b().request.id}/${year}`;
});
```

## 5b. The print date

The two templates differ deliberately on the date field:

- **Plav**: `printDate = fmtDate(new Date().toISOString())` — the **print-time
  now** (the machine clock). Legacy `XrPageInfo1` printed the clock, so a reprint
  shows the date it was printed, not when the request was filed.
- **Zelen**: `printDate = fmtDate(request.createdAt)` — the request creation date.

(Both are noted inline in their respective `v` resolvers.)

---

## 6. The three forms in detail

### Zelen (ЗЕЛЕН / green) — `ZelenTemplate.vue`

Two-page form (front + back). Source positions from `scripts/printZelen.xml`.

**Page 1 (front)** — top: print date + `toMvr` (destination МВР); section (A)
plate + a variant checkbox `✕`; D.1–D.3 + 38 (марка / тип / комерц / облик
каросерија); C.2.1–C.2.4 owner block (surname / first name / address / community
/ ЕМБГ); A1 registration-valid-until; proofs (ownership + payment); footer
(company submitter + reference number).

**Page 2 (back)** — ТЕХНИЧКИ ПОДАТОЦИ panel (engine type, VIN, year, body type,
color, engine number, kW, cc, mass, seats, standing seats, category) and the
conditional **Б panel**.

**The Б panel (change-of-owner data).** The "Б. ПОДАТОЦИ ЗА СОПСТВЕНИКОТ" block
(`bChangeMark`, `bSurname`, `bFirstName`, `bAddress`, `bCommunity`, `bEmbg`)
renders only when the request changes owner data:

```ts
const showBSection = computed(() => {
  const name = b().type.name || '';
  return b().type.transfersOwnership || /промена/i.test(name);
});
```

When shown, it binds to the **new** owner (`newClient ?? client`) — a different
person on a transfer, falling back to the page-1 client on a plain data change.
The `B_KEYS` set + `shouldRender(key)` gate suppress those spans otherwise.

**Variant checkbox Y.** The А/Б/В/Г variant checkmark moves vertically by request
type prefix: `А`=85.7mm, `Б`≈92.5, `В`≈99.0, `Г`≈105.5 (`variantY` computed,
measured from the prnx for А, ~6.5mm spacing for the rest).

### Plav (ПЛАВ / blue) — `PlavTemplate.vue`

Three-page form for re-registration / transfer of ownership. Source positions
from `Downloads/PrintPlav.xml` via `frontend-v2/scripts/parse-prnx.ps1` (the
green `scripts/printZelen.xml` is checked in, but the rendered Plav `.prnx`
lived in the developer's `Downloads/` and is **not** in the repo — only the
folded result baked into `PlavTemplate.vue`'s `DEFAULT_POS` survives).

- **Page 1 — front**: print date, `toMvr` (new reg), new-registration prefix,
  variant/category checkbox, the **previous owner + previous registration** block
  (only on transfers), proofs, note, submitter org, "ready" mark, reference no.
- **Page 2 — vehicle**: full technical data — maker/type/model, VIN, year, body
  type, engine type, cc, kW, fuel, engine number, masses (empty / F.1 / F.2 /
  F.3), dimensions (L/W/H), seats (S.1/S.2/S.3), color, per-axle masses & axle
  loads, and misc specs (rpm, max speed, axle count, trailer masses braked/
  unbraked, CO₂, hitch load, static noise).
- **Page 3 — new owner**: new-registration issuer + plate, and the new/sole
  owner block.

**`regOwner`** drives page 3 and the destination МВР: it's the `newClient` on a
transfer, else the sole `client` (a first registration "по прв пат" has no
previous owner, so the page-1 previous block is hidden and the only owner appears
on page 3).

**New-registration prefix + destination МВР.** For an `issuesNewRegistration`
type the form prints the **new owner's community** registration prefix as
`"{communityRegistrationCode}-"` (e.g. `"VE-"` for Велес) — *not* the old plate —
and the destination МВР (`toMvr`) is that community's registration issuer:

```ts
const newRegValue = computed(() => {
  const regCode = regOwner.value?.communityRegistrationCode;
  if (b().type.issuesNewRegistration && regCode) return `${regCode}-`;
  return plateOrPrefix(b().lastRegistration?.plateNumber || b().newVehicle?.plate);
});
// toMvr: regOwner.value?.communityRegistrationIssuer || ''
```

**Category checkbox row.** The variant checkmark's Y is driven by the vehicle
payment category's legacy `ZelenMap` (1=105.8mm passenger/bus, 2=111.1 cargo,
3=115.4 trailer, 4=119.6 moto). `ZelenMap` 0 or 5–11 has no checkbox → hidden.

**Address wrap.** A canvas-measured `addressLineCount()` estimates whether an
uppercased address wraps to a 2nd line and shifts the community/ЕМБГ rows down by
`LINE_MM` (3.3mm), mimicking the legacy `CanGrow` behavior.

**Zero-fill specifics (`numZ`).** Several Plav fields (per-axle masses, seat
counts, trailer/CO₂/noise) print `0` rather than blank, matching the legacy
output even where our migration nulled the zeros. Per-axle masses are hardcoded
`'0'` (not in the DTO); axle loads 1–2 come from data, 3–5 are `'0'`.

### Bel (БЕЛ / white) — #70 pending

`BelTemplate.vue` does **not exist yet** (task #70). A request whose
`documentPrintCode` resolves to `BEL` currently falls back to `ZelenTemplate`
with a yellow "not calibrated" warning. The layout data is already prepared:

- `frontend-v2/src/views/print/print-layout-bel.json` — the parsed DevExpress
  Designer manifest, extracted from `WinApp/Stampa/PrintRequests/rptBel.Designer.vb`
  by `scripts/extract-print-layout.mjs`. It is the **sparsest** of the three
  manifests: most controls came through as generic `XrLabel1…XrLabel40` (their
  bound expressions weren't statically resolvable), plus a handful of named ones:
  `lblCompanyName`, `lblVehicleCtegory` (sic), `lblAllowedCarringWaight` (sic),
  `lblCountryMade`, `lblPrimaryPowerSource`, `lblSecondaryPowerSource`,
  `lblIsReady` / `lblIsNotReady`, the `CheckBoxDocTyprOption1–5` (sic) row, and
  the vehicle-kind checkboxes `chkZemjodelskiTraktor`, `chkRabotnaMasina`,
  `chkPriklucno`, `chkMultikultivator`, `chkVelosipedSoMoteor`.
- `legacy-field-map.ts` covers the **shared/common** control names (it predates a
  full Bel pass) but maps almost none of Bel's specific controls — of the named
  Bel controls only the generic `lblIsReady` / `lblIsNotReady` resolvers exist.
  It is **not** a turnkey Bel resolver.

To build it, mirror the Zelen/Plav pattern: a `DEFAULT_POS` in mm (convert the
`print-layout-bel.json` 0.1mm coords via `/10`, or — preferably — re-derive from
a rendered `.prnx` the way Zelen/Plav were done, since the generic `XrLabel*`
names give no field meaning), a `v` resolver per field, and the conditional
previous-owner / new-owner blocks gated on `transfersOwnership`.

---

## 7. Edit-mode position calibration

Both templates expose a drag-to-calibrate mode, toggled by **Уреди позиции** in
the toolbar. Implementation (identical in both templates):

- A live `pos = ref<AllPos>` copy of `DEFAULT_POS`, **persisted to
  `localStorage`** under a versioned key:
  - Zelen: `vte.v2.print.zelen.positions.v2`
  - Plav:  `vte.v2.print.plav.positions.v2`
  - The `v2` suffix is a cache-buster — *bump it when `DEFAULT_POS` changes
    meaningfully* so stale saved positions don't shadow the new defaults.
- `loadInitial()` deep-merges any saved `{x,y}` overrides over `DEFAULT_POS` (so
  new fields added to defaults still appear).
- `startDrag/onMouseMove/onMouseUp` move a field; the delta is converted px→mm via
  `PX_PER_MM = 3.7795275591` (96-DPI screen px per mm, *not* the 11.811
  print-DPI constant), clamped to the A4 box.
- `exportPositions()` emits a minimal `{ page: { key: {x,y} } }` JSON.
  `downloadPositions()` (in `LegacyPaperPrint.vue`) saves it as
  `{code}-positions-YYYY-MM-DD.json`; the operator sends the file back so the new
  numbers can be baked into `DEFAULT_POS` in code.
- `resetPositions()` restores `DEFAULT_POS` and clears the localStorage key.

**Workflow**: calibrate on screen with the grid on, drag fields to match a real
printout, **Изнеси JSON**, hand the file to a developer to fold into the
`DEFAULT_POS` constants, then bump the storage key version. The localStorage
copy is a *local-only* convenience; it is not a server-side setting.

---

## 8. Tech-exam prints (cross-reference)

Two other government-paper overlays use the same absolute-mm technique but are
keyed off a technical exam, not a request:

- `frontend-v2/src/views/print/TechExamCertificate.vue` —
  route `/technical-exams/:id/print` (`name: 'technical-exam-print'`).
- `frontend-v2/src/views/print/TechExamZapisnik.vue` ("Записник за технички
  преглед", stamped on pre-printed Letter paper) —
  route `/technical-exams/:id/zapisnik` (`name: 'technical-exam-zapisnik'`).

Both routes are registered in `frontend-v2/src/router/index.ts`. See
[Technical exams](04-technical-exams.md) for those forms, their endpoints, and
controller-name/signature resolution.

---

## Source map

| Concern | File |
|---|---|
| Print page + template dispatch + toolbar | `frontend-v2/src/views/print/LegacyPaperPrint.vue` |
| Green form (2 pages) | `frontend-v2/src/views/print/ZelenTemplate.vue` |
| Blue form (3 pages) | `frontend-v2/src/views/print/PlavTemplate.vue` |
| White form (pending) | `print-layout-bel.json` + `legacy-field-map.ts` (no template yet) |
| Print bundle endpoint + resolvers | `backend-v2/src/VTE.Api/Controllers/RequestsController.cs` (`PrintBundle`, `BuildClientMeta`, `BuildVehicleMeta`, `BuildLastRegistrationMeta`, `BuildPreviousRegistrationMeta`) |
| Bundle TS types | `frontend-v2/src/types.ts` (`RequestPrintBundle`, `PrintClientMeta`, `PrintVehicleMeta`) |
| Control-name → value map (Designer JSON path) | `frontend-v2/src/views/print/legacy-field-map.ts` |
| Designer-parsed layout manifests (0.1mm) | `frontend-v2/src/views/print/print-layout-{plav,bel,zelen}.json` (also in `scripts/`) |
| `Designer.vb` → layout-JSON extractor | `scripts/extract-print-layout.mjs` (reads `WinApp/Stampa/PrintRequests/rpt{Plav,Bel,Zelen}.Designer.vb`) |
| `.prnx` → mm parser | `frontend-v2/scripts/parse-prnx.ps1` |
| Rendered legacy source (green, checked in) | `scripts/printZelen.xml` |
| Reference-number backfill | `migrate/backfill-request-reference-no.sql` |
| Document-print code derivation (migration) | `migrate/migrate-request-catalogs.sql` |
| Document-print seed (fresh installs) | `backend-v2/src/VTE.Api/Seed/DataSeeder.cs` |
| Self-contained styled print (alt route) | `frontend-v2/src/views/RequestPrintView.vue` |
