# Operator & admin guide

A task-oriented manual for the people who use VTE every day at the inspection station in
Велес — the **оператори** (operators) who take in vehicles, run technical exams, and collect
payments, and the **администратор** (station admin) who keeps the catalogs and users in order.
It walks through each screen in the order you actually touch it on a normal day, using the
real Macedonian button labels you see on screen (with an English gloss in parentheses). For
the device side of fiscal printing see the [Fiscal printing](06-fiscal.md) guide; for how
prices turn into debts and bills see [Payments & pricing](05-payments-and-pricing.md).

Every label, button and behaviour below is taken from the actual Vue views in
`frontend-v2/src/views/**` and the locale file `frontend-v2/src/locales/mk.ts`. Where the
behaviour is enforced on the server, the controller is cited too.

---

## 1. Logging in and choosing your language

### Sign in

The login screen (`frontend-v2/src/views/LoginView.vue`) is a split panel: a hero on the left
(„Технички преглед на возила" / *Vehicle technical inspection*) and the sign-in card on the
right titled **Добредојдовте** (*Welcome*).

| Field | Macedonian label | What to enter |
|---|---|---|
| Username | **Корисничко име** | Your operator user name (the box is pre-filled with `admin`). |
| Password | **Лозинка** | Your password. The eye icon toggles visibility (`toggleMask`). |
| Session cookie | **Постави и колаче за сесија** (*Also set a session cookie*) | Optional checkbox. |

Press **Најави се** (*Sign in*). On success you land on the dashboard (`/dashboard`); if you
were redirected to login from another page, you return there instead. A failed attempt shows
**Најавата не успеа.** (*Sign-in failed*) — the message from the server is shown when
available (`LoginView.vue` `submit()`).

> **Lockout warning (admins):** ASP.NET Identity locks an account after repeated wrong
> passwords ("Account is locked."). An admin clears it in the database with
> `UPDATE AspNetUsers SET LockoutEnd=NULL, AccessFailedCount=0` (see `CLAUDE.md` gotcha #15).

### Language toggle (MK / EN)

The whole UI is bilingual via `vue-i18n`. In the top bar there is a small **MK / EN**
toggle (a PrimeVue `SelectButton` in `frontend-v2/src/components/AppLayout.vue`). Click it to
switch instantly; the choice is remembered. The default and primary language is Macedonian.
A sun/moon button next to it switches **Темна тема / Светла тема** (dark / light theme).

### Global search (Ctrl+K)

The box in the middle of the top bar searches **owners and vehicles from any screen**:
type a plate, EMBG, name or VIN (2+ characters). Results come grouped — **Сопственици**
and **Возила** — click one (or ↑↓ + Enter) to jump straight to that client/vehicle.
**Ctrl+K** focuses the box from anywhere; **Esc** closes it.

### The top bar and sidebar

- The left sidebar (**Навигација**) lists the main modules; the collapse button toggles
  **Покажи мени / Скриј мени** (show / hide menu).
- The top bar shows the page title, your company crumb (or „Сите компании" / *All companies*
  for a cross-tenant admin), your name and role, an **Подеси профил** (*Account settings*)
  pill linking to `/account`, and **Одјави се** (*Sign out*).
- A global **Назад** (*Back*) button appears on every page except the dashboard, because
  operators kept getting stranded on detail/form screens (`AppLayout.vue`).

### Account settings

`/account` (`AccountSettingsView.vue`, locale key `account`) — **Мој профил** lets you change
your **Име и презиме** (full name) and **Е-пошта** (email), and set a **Нова лозинка** (new
password, min 6 chars, confirmed). Username, role and company are read-only here.

### Roles

There are two roles (`operators.role`): **Оператор** (*Operator*) and **Администратор**
(*Administrator*). The whole **Администрација** section of the sidebar — and most create/edit
buttons in the reference screens — only appears when `auth.isAdmin` is true.

---

## 2. The dashboard (Контролна табла)

The dashboard (`DashboardView.vue`) is the operator's home base. It greets you with
**Добредојде, {име}** and your **Улога** (role), then shows four working areas.

### 2.1 Create a request by type (Креирај барање)

The top-left card is a **tree** of request types, grouped by their parent type, mirroring the
legacy Контролна табла tree. Each leaf is a clickable shortcut that opens the new-request form
with that type pre-selected (`createRequest(id)` → `/requests/new?typeId=<id>`). A small
coloured dot shows which МВР form the type prints (`printClass`):

| Dot colour | Print form | `documentPrintId` |
|---|---|---|
| Green | **Зелен** (Green) | 1 |
| Blue | **Плав** (Blue) | 2 |
| Grey | **Бел** (White) | 3 |
| Dashed/none | no form | other |

A **Празно барање** (*Blank request*) link opens `/requests/new` with nothing pre-selected.

### 2.2 Наплата (Collections) — the live debt ledger

The top-right card is **Наплата** — every outstanding charge for the company, grouped by
client → vehicle. This is the heart of the cashier workflow. See section 6 for the full
walkthrough. (Source: `DashboardView.vue` `refreshDebts`, `debtGroups`.)

### 2.3 Recent open requests (Најнови отворени барања)

A compact table of the latest open requests; clicking a row opens the request. **Види ги
сите** (*See all*) links to `/requests`.

### 2.4 Recent technical exams (Најнови технички прегледи)

The latest exams with a pass/fail tag (**Исправно / Неисправно**); a row opens the exam.

The Наплата panel auto-refreshes whenever you switch back to the dashboard tab
(`visibilitychange`), so the common "save a tech-exam in another tab, come back" loop stays
current.

---

## 3. Creating a request (Ново барање)

Open it from the dashboard tree, the **Ново барање** button on the Барања list, or
`/requests/new`. Source: `RequestFormView.vue`. The form is a stack of cards.

### 3.1 Барање (Request header)

| Field | Label | Notes |
|---|---|---|
| Type | **Вид барање** (*Request type*) * | Required. Pick from **Избери вид…**. Selecting a type drives the rest of the form (see flags below). If a type has a description it appears as a blue info hint. |
| Company | **Компанија** | Admins only, and only when creating. Operators always create under their own company. |
| Note | **Забелешка** | Free text. |

### 3.2 Сопственик и возило (Owner & vehicle) — the "anchor"

This card uses the legacy two-field linked picker. **Both fields are always shown** and they
are linked:

- **Бр. шасија или Рег. бр. на возило** (*Chassis no. or plate*) * — type a VIN or plate
  (min 2 chars) to search across the company's vehicles. Picking a vehicle auto-fills its
  owner.
- **ЕМБГ или Име Презиме сопственик** (*EMBG or owner name*) * — search a client by EMBG or
  name. Picking an owner scopes the vehicle field to that owner's vehicles. If the owner has
  exactly one vehicle it is auto-selected; if several, you pick from the dropdown.

Each field has its own **Нов** (*New*) and **Уреди** (*Edit*) buttons. **Нов** opens the
vehicle or client form **in a new browser tab** so your in-progress request isn't lost — you
register the new record, come back, and re-search (`openVehicleNew`, `openOwnerNew`).

On an existing request the anchor is **locked** („Сопственикот и возилото се заклучени за ова
барање." with a lock icon) — you can't move a saved request to a different vehicle/owner.

The chosen client↔vehicle relation is what the request hangs on (`selectedRelationId`).

### 3.3 Нов сопственик (New owner) — only for ownership transfers

As soon as you pick a request type whose **Префрла сопственост** (*transfers ownership*) flag
is set, a **Нов сопственик** block appears (`requiresNewOwner`). It's a free client search
(**ЕМБГ или Име Презиме на новиот сопственик**) — any client in the system, by EMBG or name.
The hint reads „Возилото се пренесува на овој сопственик кога барањето ќе се заврши."
(*The vehicle is transferred to this owner when the request is finished*).

You send the chosen **client** to the server; the server resolves-or-creates the actual
relation on finish. The new owner is **required** for transfer types — saving without it shows
„Новиот сопственик мора да биде избран за пренос на сопственост" (`validate()`). Internally
this becomes `NewOwnerClientId` on `RequestWriteDto`, and the server's
`ResolveOrCreateNewOwnerRelationAsync` turns it into a (initially inactive)
`ClientVehicleRelation` that the End flow activates (`RequestsController.cs`; see `CLAUDE.md`
gotcha #17).

### 3.4 Приложени документи (Attached documents) — new request only

When creating, you get two editable columns of proof rows, pre-filled with the legacy
defaults so the common case is one click:

| Column | Title | Default first row |
|---|---|---|
| Ownership | **Доказ за потеклото на возилото** (*Proof of vehicle origin*) | **СООБРАЌАЈНА ДОЗВОЛА** (traffic licence) |
| Payment | **Потврда за платени давачки** (*Proof of paid fees*) | **СМЕТКА** (bill/receipt) |

Each row is a type dropdown plus a **Број** (*number*) free-text box. Use **Додај ред** (*Add
row*) to add more, the red ✕ to remove one. These rows are held in memory and saved as
ownership-proof / payment-proof records right after the request is created
(`save()` → `pendingOwnership` / `pendingPayment`). The defaults come from
`typeIdByName(..., 'СООБРАЌАЈНА ДОЗВОЛА')` / `'СМЕТКА'`.

### 3.5 Saving

Press **Сочувај** (*Save*). Validation order (`validate()`):

1. Type required.
2. Owner + vehicle (a relation) required.
3. For transfer types, a new owner required.

After a successful create you are redirected to the saved request (`/requests/{id}`), where
the form switches into edit mode and the proof/attachment cards become full tables.

---

## 4. Working on a saved request

On an existing request (`RequestFormView.vue` edit mode) the cards become managed lists. The
status tag in the header is **Отворено** (open, green), **Завршено** (closed, blue), or
**Неактивно** (inactive/soft-deleted, red).

### Proofs & attachments

- **Докази за сопственост** / **Докази за уплата** — tables with **Нов** to add, pencil to
  edit, trash to remove. Each has a **ВИД** (type) and **ДЕТАЛИ** (detail).
- **Прилози** (*Attachments*) — **Прикачи** (*Upload*) opens a dialog: pick a **ВИД** (type)
  and a file (images or PDF, ≤ 20 MB), then **Прикачи**. Each attachment can be downloaded
  (**Преземи**) or removed. Stored via `POST /requests/{id}/attachments`.
- **Историја** (*Audit*) — created / modified / ended timestamps and the operator who did each.

A **closed** request is read-only everywhere (`isReadOnly` = has an `endedAt`); Save, Delete,
End and the proof/attachment edit buttons all disappear.

### Header actions

| Button | Label | When shown |
|---|---|---|
| Back | **Назад** | always |
| Print | **Печати** | edit mode (see §5) |
| Delete | **Избриши** | edit mode, not closed — soft-deactivates the request |
| Save | **Сочувај** | not closed |
| End | **Заврши** | edit mode, not closed (see §4.1) |

### 4.1 Finishing a request (Заврши) and what it triggers

**Заврши** (*Finish*) closes the request and applies its side-effects atomically. Pressing it
opens a confirm dialog **Заврши барање** listing exactly what will happen, built from the
type's flags (`endSideEffects` in the view, enforced in `RequestsController.End`):

| Type flag | Confirm line (MK) | Effect on End |
|---|---|---|
| `paymentRequired` | „Доказ за уплата мора веќе да биде прикачен" | **Blocks** finishing unless ≥ 1 active payment proof exists. |
| `transfersOwnership` | „Сопственоста ќе се префрли на новиот сопственик" | Activates the new relation, deactivates the old anchor relation. |
| `deactivatesRelation` (and not transfer) | „Врската сопственик–возило ќе биде затворена" | Deactivates the anchor relation, sets its end date. |
| `deactivatesVehicle` | „Возилото ќе биде деактивирано" | Marks the vehicle inactive. |
| `issuesNewRegistration` | „Потоа треба да се изда нова регистрација од формата на возилото" | A reminder only — you create the new registration yourself from the vehicle form afterward. |

The server also pre-validates on End and will refuse with a clear Macedonian error if:
the type **Required**s a technical exam but none is linked („Потребен е технички преглед."),
a transfer type has no new owner („Новиот сопственик не е избран."), or a previous
registration is required but missing. On success you get a toast **Барањето е завршено** with
the list of applied notes, and the form reloads in read-only closed state.
(Source: `RequestsController.cs` `End`, lines ~599–708.)

> Finishing **cannot be undone** — the confirm dialog says so („Ова не може да се поништи.").

---

## 5. Printing the right МВР form

From a saved request press **Печати** (*Print*). It opens `/requests/{id}/print` (route name
`request-print`) in a new tab, served by **`frontend-v2/src/views/print/LegacyPaperPrint.vue`**.
This is the *default* print page: it loads the print bundle (`GET /requests/{id}/print`) and
stamps the values at hand-coded millimetre coordinates so the sheet feeds onto the pre-printed
МВР government paper. It does **not** auto-fire the print dialog — a dark toolbar at the top has
**Печати** to send it to the printer, **Покажи мрежа / Скриј мрежа** (show/hide a calibration
grid) and an **Уреди позиции** (edit-positions) mode for re-calibrating field placement.

> There is also an alternative "readable copy" page on blank A4 with on-screen chrome at
> `/requests/{id}/print/styled` (`RequestPrintView.vue`), for when no pre-printed paper is on
> hand. *That* one does auto-open the print dialog. The **Печати** button only links to the
> default (`request-print`) page.

The form that prints is decided by the request type's document-print code, not by you —
`LegacyPaperPrint.vue` reads `bundle.type.documentPrintCode` (it accepts both the Latin seeder
codes and the Cyrillic codes the migration produced) and picks the matching template:

| Code (Latin / Cyrillic) | Form | Macedonian | Template |
|---|---|---|---|
| `PLAV` / `ПЛАВ` | Blue | **Плав** | the registration form (3-page, pixel-perfect — `print/PlavTemplate.vue`) |
| `ZELEN` / `ЗЕЛЕН` | Green | **Зелен** | the de-registration form (`print/ZelenTemplate.vue`) |
| `BEL` / `БЕЛ` | White | **Бел** | ownership-transfer form — **not yet calibrated**; it currently falls back to the Зелен template and shows a warning note („Шаблонот за … сè уште не е калибриран") (BelTemplate is task #70, still pending) |
| anything else | — | — | also falls back to the Зелен template with the same warning |

So: choose the correct **Вид барање** (which carries the right **Образец** / form), and the
print just works. If a request looks like it would print the wrong sheet, fix the **Образец**
on the request type (see §8.2), not the request. For the millimetre-coordinate layout system,
the position-edit workflow and per-form details, see [Print system](07-prints.md).

---

## 6. Наплата — reviewing debts and making a bill

The **Наплата** (*Collections*) panel on the dashboard is the cashier surface. It is a ledger
of **all active charges** for the company (not just unpaid ones) — billed charges stay listed,
marked **Платено** (*Paid*), instead of disappearing. A fully-settled client drops off the
panel once every one of its charges is paid (`debtGroups` filters on `unpaidCount > 0`).

### 6.1 Reading a client's debts

Charges are grouped **client → vehicle**. Each group header shows the client name (click it to
open that client's bills, filtered by relation), the plate/VIN and maker·model, and the group
total still owed. Each row under it is a single charge: **УСЛУГА** (service), **ЗАБЕЛЕШКА**
(note) and **ЦЕНА** (price). Paid rows show a green check and a **Платено** tag and cannot be
re-billed. The footer shows the company-wide **Вкупно** (total) in **ден.** (denars).

Where do these charges come from? They are created automatically when you finish a request or
save a technical exam, by the pricing engine (`PricingEvaluator` / `DebtService`). For the
rules behind them see [Payments & pricing](05-payments-and-pricing.md).

### 6.2 Selecting charges

Tick the checkbox on individual unpaid rows, or the group checkbox to select **all unpaid for
this client** („Избери ги сите за овој клиент"). Selection only ever applies to unpaid rows.
Two action buttons appear above the list once anything is selected:

- **Направи сметка ({n})** — make a bill from the selection (primary).
- **Избриши избрани ({n})** — bulk-delete selected charges (danger, outlined).

There's also a small **Освежи** (refresh) icon to reload the panel.

### 6.3 Направи сметка (Make a bill)

> **A bill covers exactly one client.** If your selection spans more than one client/relation,
> the button is disabled with the hint „Сметката мора да биде за еден клиент…"
> (`selectedRelationIds.size !== 1`).

Click **Направи сметка** to open the dialog **Нова сметка од избраните ставки**:

1. It shows the count of **Ставки** (items) and the **Вкупно** (total) in денари.
2. Pick a **Начин на плаќање** (*Payment method*) from the dropdown. The list is loaded from
   `/payment-documents/payment-types?usedOnly=true`. It defaults to cash — „во готово", the
   un-prefixed cash type.
3. Press **Направи сметка** (confirm). The bill is created via
   `POST /payment-documents/from-debts`; on success you see **Сметка бр. {број} е креирана** and
   are taken to the bill page (`/payments/{id}`). The selected charges flip to **Платено** in
   Наплата.

#### Installments (по договор / Договор за рати)

If you pick an installment-type payment method (`isInstallment`), the dialog grows a **Договор
за рати** (*Installment agreement*) section:

| Field | Label |
|---|---|
| Number of installments | **Број на рати** (2–36) |
| Down payment | **Прва рата (учество)** (*First installment / down payment*) — defaults to half the total |
| Remaining | **Остаток на рати** — computed, shown as amount × (n−1) |
| Guarantor | **Гарант** → **Име и презиме на гарантот**, **ЕМБГ на гарант**, **Адреса на гарант** |

The down payment (rata 1) is sent with the bill; the schedule is generated server-side.

### 6.4 The fiscal receipt (Фискална сметка)

Right after a bill is saved, VTE **automatically attempts to print the fiscal receipt** (for
installment bills it prints the down payment, rata 1). This is `printFiscalForDocument` in
`frontend-v2/src/fiscal/fiscal.ts`, called from `createBill`. Outcomes you may see as a toast:

| Result | Toast | Meaning |
|---|---|---|
| `printed` | **Фискалната сметка е испратена на принтерот** | success |
| `skipped` | **Оваа сметка не се фискализира** | this payment type / amount doesn't fiscalize (e.g. non-cash) |
| `no-folder` / `error` | **Сметката е креирана, но фискалниот принт не успеа…** | the fiscal folder isn't configured — open the bill and click **Фискална сметка** to retry |

You can always reprint manually from the bill page (§6.5). Fiscal printing needs the Accent
PF-500 folder configured on this PC — see §9 and the [Fiscal printing](06-fiscal.md) guide.

### 6.5 The bill page (Сметка)

`PaymentView.vue` (`/payments/{id}`) shows one bill in full: parties (**Сопственик**,
**Возило**), **Тип на плаќање**, **БРОЈ НА СМЕТКА**, issue/due dates, the **Ставки** (line
items) with quantity, unit price, **ДДВ** (VAT) %, line discount and subtotal, and the
**Вкупно** total. A status tag reads **Платено / Неплатено / Сторно** (paid / unpaid /
reversed).

Header actions:

- **Сметкопотврда** — print the A4 landscape receipt (two copies on one sheet).
- **Договор** (installment bills only) — print the ДОГОВОР за отплата (the contract the
  client and guarantor sign; legacy rptPaymentDocumentDogovor, A4 on blank paper).
- **Фискална сметка** (*Fiscal receipt*) — print/reprint the fiscal receipt. A
  **Фискализирана** (*Fiscalized*) tag with timestamp shows once it has been printed.
- **Означи платено / неплатено** — toggle the paid status.
- **Сторнирај** (*Storno*) — reverse a wrong bill. A dialog asks for the mandatory
  **Причина за сторнирање** (reason). On confirm: the bill flips to **Сторно**, the debts
  it settled **return to the Наплата panel** (so you can bill the client again correctly),
  and for a cash bill the fiscal **STORNO** receipt prints right away. A stornoed bill is
  frozen — no price edits, no paid toggle, no installment payments. Bills that came from
  the old system can't be stornoed here — storno them in the old program and the change
  arrives with the next sync.
- The status tag.

**Installments on the bill:** if the bill is an agreement, a **Рати и договор** section lists
the agreement (**Бр. на договор**, **Датум**, **Вкупно рати**, **Гарант**…) and the schedule.
Each unpaid installment row has a **Плати** (*Pay*) button: pressing it marks that rata paid
(`POST /payment-documents/{id}/installments/{seq}/pay`) and quietly prints the fiscal receipt
for that rata. You'll see **Рата {n} е платена**. Paid rows get a small print icon —
reprint the fiscal receipt for that rata (e.g. when the down payment's print didn't go
through the first time).

### 6.6 The Плаќања list (Payments / bills register)

`/payments` (`PaymentsView.vue`) is the searchable register of all bills. Filter by status
tabs **Сите / Платени / Неплатени / Сторнирани** (all / paid / unpaid / reversed), search by
**Бр. на сметка, име, ЕМБГ, рег. таблици или VIN**, and filter by payment type. Columns:
doc number, client, vehicle, type, issued, due, total, status. Click a row to open the bill.

---

## 7. Technical exams (Технички прегледи)

### 7.1 The register

`/technical-exams` (`TechnicalExamsView.vue`) lists exams with tabs **Сите / Исправни /
Неисправни** (all / passed / failed) and search by **ЕМБГ, име, рег. бр. или број во
регистар**. **Нов технички преглед** (*New technical exam*) opens the editor.

### 7.2 Doing an exam (TechExamFormView)

Source: `TechExamFormView.vue`. The header subtitle live-shows the result
(**Исправно / Неисправно**) and **Важност до** (*Valid until*) computed from the type's
`validDays` (defaulting to 365). Cards:

**Основни податоци (Header)**

| Field | Label |
|---|---|
| Type * | **Тип на технички преглед** (e.g. РЕД-12М) |
| Station * | **Станица** (the inspection organization; the station's tech-exam org is id 37) |
| Date * | **Датум на извршување** (*Date performed*), default today |
| Valid until | **Важност до** — read-only, derived |
| Controllers | **Прв контролор** / **Втор контролор** (first / second controller) |
| Reg. number | **Број во регистар** — read-only, shown on saved exams |

**Сопственик и возило (Owner & vehicle)** — search a client by name/EMBG (**Пребарај
сопственици…**), then pick the **Возило** (relation). Locked once saved.

**Мерење на ефектот на кочење (Brake-force measurements)** — collapsed by default. A grid of
the four axles plus **Паркирна сопирачка** (parking brake), each with **Лева / Gj (daN) / Лева
pj(kP) / pN (kP) / Десна** columns, then a grid of summary measurements (**Маса**, working
brake empty/full, CO %, lambda, RPM, noise, etc.) and **Технички промени на возилото**.

**Неисправности на возилото (Defects)** — **Нов** adds a defect row: **Неисправен уред или
опрема** (part), **Статус**, the position checkboxes **Напред / Назад / Лево / Десно**, and a
**Забелешка**. Each defect must have both a part and a status, or save fails
(„Секоја неисправност мора да има уред и статус.").

**Забелешки (Notes)** — **Објаснување за неправилности** (explanation), **Предупредување на
возачот** (driver warning), and a general **Забелешка**.

**Pass/fail** is automatic: the exam is **Исправно** (passes) if it has no defect lines, or if
every line has status „исправен" (status id 1). This mirrors the server's `DerivePass`.

Press **Сочувај** to save (validation: type, station, date and relation required). Saving a
tech-exam also generates its payment charges via the pricing engine, which is why the Наплата
panel updates afterward (see [Payments & pricing](05-payments-and-pricing.md);
irregular exams of type > 1 use the `TechnicalExamIrregular` trigger per `CLAUDE.md` #10).

### 7.3 The exam detail + printing Записник / Уверение

`/technical-exams/{id}` (`TechnicalExamReportView.vue`) shows the saved **Записник за технички
преглед** read-only, with a big pass/fail tag. Header buttons:

- **Уреди** (*Edit*) — back to the editor.
- **Печати записник** (*Print the report/Записник*) — opens `/technical-exams/{id}/zapisnik`
  (`print/TechExamZapisnik.vue`).
- **Печати потврда** (*Print the certificate/Уверение*) — opens `/technical-exams/{id}/print`
  (`print/TechExamCertificate.vue`). **Disabled when the vehicle failed** — the tooltip
  explains „Возилото не е технички исправно — потврда не се издава." (*The vehicle is not
  roadworthy — no certificate is issued*).

---

## 8. Admin & reference screens (Администрација)

Everything under **Администрација** in the sidebar is admin-only. Create/edit/delete buttons
appear only for administrators (`auth.isAdmin`). Deletes are **soft** — the standard confirm
says „…Можете да го реактивирате со уредување." (*You can reactivate it by editing*).

### 8.1 Ценовник — ставки за наплата (Price catalog)

`/prices` (`PricesView.vue`). A master-detail screen: the left rail is the list of vehicle-
payment **categories** with active-rule counts (search box, **Сокриј празни** / hide empty);
the right pane lists the **rules** in the selected category. Each rule is a charge-creating
template. Toolbar filters: search by name/code, **Сите тригери** (trigger filter), **Само
активни** (active only).

**Нова ставка / Уреди ставка** opens the editor. Key fields (locale `prices.form`):

| Field | Label | Meaning |
|---|---|---|
| Name * | **Назив** | rule name (shows as the УСЛУГА in Наплата) |
| Category | **Категорија возило за наплата** | which vehicle category bucket |
| Code | **Код** | optional code |
| Base price * | **Основна цена** | the amount |
| VAT group | **ДДВ група** | VAT rate id |
| Trigger * | **Тригер** | what fires this rule (see below) |
| Company | **Компанија** | a specific tenant, or **Сите компании (заедничка ставка)** for a shared rule |
| Community | **Општина** | community filter |
| Vehicle field | **Поле од возилото** | conditional field |
| Param from/to | **Парам. од / Парам. до** | numeric range |
| Vehicle-category filter | **Филтер по категории возила (CSV)** | e.g. `M1,N1` |

**Тригери** (`prices.trigger`): **Без тригер** (none), **Технички преглед** (technical exam),
**Барање** (request), **Сообраќајна дозвола** (traffic licence), **Дозвола** (permission),
**Меѓународна возачка** (IDL), **Вонреден техн. преглед** (irregular technical exam). These
map to the `PriceTrigger` enum. The deep behaviour (the three legacy levels, the active
cascade, per-tenant scoping) is documented in [Payments & pricing](05-payments-and-pricing.md).

### 8.2 Видови барања (Request types)

`/request-types` (`RequestTypesView.vue`). The workflow catalog. The list shows each type's
**НАЗИВ**, **ОБРАЗЕЦ** (print form), **ПРЕГЛЕД** (exam requirement) and **ОЗНАКИ** (flags).

The editor (**Нов вид барање / Вид барање #{id}**) has:

- **Назив** (name) *, **Опис** (description).
- **Образец** (*Form*) * — which print template (Плав/Зелен/Бел) this type uses. This is what
  determines the printed МВР sheet in §5.
- **Надреден вид** (*Parent type*) — builds the dashboard tree.
- **Технички преглед** (*Technical exam*): **Не е потребен / Потребен / Опционален** (not
  required / required / optional). > Note: on production this column also encodes the
  exam-TYPE id (a value like 9), per `CLAUDE.md` gotcha #16 — read it as an integer where
  `>0` means "create an exam".

**Ознаки на работниот тек (Workflow flags)** — these drive the End behaviour in §4.1:

| Flag | Label |
|---|---|
| `paymentRequired` | **Потребна е уплата** |
| `issuesNewRegistration` | **Издава нова регистрација** |
| `deactivatesRelation` | **Ја деактивира врската** |
| `deactivatesVehicle` | **Го деактивира возилото** |
| `transfersOwnership` | **Префрла сопственост** |
| `mutatesVehicleData` | **Менува податоци за возило** |
| `mutatesClientData` | **Менува податоци за клиент** |
| `isSufficient` | **Самостоен** |
| `previousRegistrationRequired` | **Потребна претходна регистрација** |

### 8.3 Other reference tables (RefManager)

Most lookup tables share one generic editor (`RefManagerView.vue`, routes under `/ref/...`):

| Route | Title (MK) | Notable fields |
|---|---|---|
| `/ref/countries` | **Држави** | Име, Кратко име, Активно |
| `/ref/communities` | **Општини** | Име, Држава, Код, **Префикс на таблици** |
| `/ref/cities` | **Градови** | Име, Општина, Поштенски код |
| `/ref/citizenships` | **Државјанства** | Име, Држава |
| `/ref/document-issuers` | **Издавачи на документи** | Име, Активно |
| `/ref/personal-data-types` | **Видови лични документи** | Id, Име |
| `/ref/request-document-prints` | **Обрасци за барања** | Код, Име, Патека до шаблон |
| `/ref/request-ownership-proof-types` | **Видови докази за сопственост** | Име (e.g. СООБРАЌАЈНА ДОЗВОЛА) |
| `/ref/request-payment-proof-types` | **Видови докази за уплата** | Име (e.g. СМЕТКА) |
| `/ref/request-attachment-types` | **Видови прилози** | Име |

> The proof-type tables feed the **Приложени документи** defaults in §3.4. The ownership/
> payment proof lists are matched by name (`СООБРАЌАЈНА ДОЗВОЛА`, `СМЕТКА`), so renaming them
> can change those defaults.

### 8.4 Компании / Станици (Companies / Stations)

- **Компании** (`/companies`, `CompaniesView.vue`) — the tenants. Fields: **Име** *, **Активна**.
- **Станици** (`/stations`, `StationsView.vue`) — inspection stations. Fields: **Име** *,
  **Компанија** * (the owning tenant).

> **Renaming an operator (миграција од легаси):** migrated legacy operators appear with
> usernames like `македонка*.119`. To make one functional, EDIT that account (clean
> username, e.g. `makedonka`) and set a password with **Ресетирај лозинка** — do NOT
> create a new account: editing keeps the same user id, so everything the person did in
> the old system (bills, exams) stays attributed to them, and prints show their name.
> Operators change their own password later via Подеси профил.

### 8.5 Оператори (Operators)

`/operators` (`OperatorsView.vue`). Manage user accounts. Filter by search, **Сите компании**,
**Сите улоги**. Per row: edit, **Промена на лозинка** (reset password, key icon), and
deactivate (user-minus icon).

Create (**Нов оператор**): **Корисничко име** *, **Полно име**, **E-mail** *, **Лозинка** *
(min 6), **Компанија**, **Улога** (Operator / Administrator). On edit, the **username, role
and password are not changeable** here (username shows a lock; reset the password via its own
dialog) — see `OperatorsView.vue` `saveForm`.

### 8.6 Синхронизација (Legacy sync)

`/legacy-sync` (`LegacySyncView.vue`, `admin.legacySync`) pulls new clients/vehicles/requests
from the old system. **Disabled in production** (`LegacySync__Enabled=false`). Only adds new
records (Id greater than current); never modifies existing ones.

---

## 9. Fiscal options (Фискални опции)

`/fiscal` (`FiscalOptionsView.vue`) configures fiscal printing on **this** PC. It is required
on the cashier machine that has the Accent PF-500 printer; it does nothing on machines without
it. Full device/driver setup is in the [Fiscal printing](06-fiscal.md) guide — here is the
operator-facing summary:

- **Фискална папка (Fiscal folder)** — **Избери папка** (*Pick folder*) chooses the folder the
  fiscal driver watches (the same folder the old app used). It's remembered per browser; a tag
  shows **дозволено** (granted) or **ќе побара дозвола** (will prompt). **Заборави** (*Forget*)
  clears it; **Тест запис во папката** (*Test write*) verifies access.
- **Извештаи (Reports)** — **Контролен извештај (X)** (the X control report) and **Дневно
  затворање (Z)** (the daily Z closure). The Z report asks for confirmation because „Дневното
  фискално затворање (Z извештај) е неповратно за денешниот ден." (*The daily Z closure is
  irreversible for today*).

> The browser must support the File System Access API. On an unsupported browser you'll see
> „Фискален принт бара Chrome или Edge." — use **Chrome or Edge** on the printer PC.

---

## 10. A normal day, end to end

1. **Log in**, confirm the MK/EN toggle is on MK.
2. From the dashboard, click the request type in the **Креирај барање** tree → fill **Сопственик
   и возило**, accept the default **СООБРАЌАЈНА ДОЗВОЛА** / **СМЕТКА** proofs, **Сочувај**.
3. If the type requires an exam, do the **технички преглед**, **Сочувај**, **Печати записник**
   and (if **Исправно**) **Печати потврда**.
4. Back on the request, **Печати** the right МВР form, then **Заврши** the request.
5. Open the dashboard, find the client in **Наплата**, select their charges, **Направи сметка**,
   pick **во готово** (or **по договор** for installments), confirm — the **Фискална сметка**
   prints automatically.
6. End of day on the cashier PC: **Фискални опции → Дневно затворање (Z)**.

---

### Related docs

- [Payments & pricing](05-payments-and-pricing.md) — how charges and bills are computed.
- [Fiscal printing](06-fiscal.md) — Accent PF-500 driver, folder, and receipt format.
- [Print system](07-prints.md) — МВР paper forms, mm-coordinate overlays, Plav/Zelen/Bel templates.
