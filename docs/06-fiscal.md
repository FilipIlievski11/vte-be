# Fiscal integration (Accent PF-500)

VTE prints legally-required fiscal receipts (фискална сметка) on an **Accent PF-500** fiscal
printer using a **file-exchange protocol**: a command file with a tiny, device-specific text
format is dropped into a folder that the Accent vendor driver watches, and the driver prints the
receipt. The legacy VB.NET WinApp did exactly this (`WinApp/Hepers/FiskalModule.vb`). v2 keeps
the *identical* device protocol but splits the work across the browser/server boundary so the SaaS
backend never needs filesystem access to the operator's PC:

- the **server** composes the exact command-file bytes
  (`GET /api/payment-documents/{id}/fiscal-file`, in
  `backend-v2/src/VTE.Api/Controllers/PaymentDocumentsController.cs`), and
- the **browser** writes those bytes into the operator's vendor-watched folder via the
  [File System Access API](https://developer.mozilla.org/docs/Web/API/File_System_Access_API)
  (`frontend-v2/src/fiscal/fiscal.ts`).

Nothing new is installed on operator PCs — the PF-500 hardware, the vendor driver, and the watched
folder already exist from the legacy deployment. See [Payments & pricing](05-payments-and-pricing.md)
for how the bills being fiscalized are created.

---

## 1. The three-layer wiring

Fiscalization spans three layers that must all be paired correctly on each operator PC. Only the
top layer is VTE code; the bottom two are the pre-existing vendor stack.

| Layer | What it is | Owned by | Configured where |
|---|---|---|---|
| **Hardware** | The physical Accent **PF-500** fiscal printer, connected to the operator PC. | Vendor (Accent) | Physically, by the technician who installed it. |
| **Vendor driver** | The Accent driver process that *watches a folder* for `*.txt` command files and prints them on the PF-500. (Legacy comments reference `Fiscal32.exe` in the same folder — see the commented-out `Shell(...)` calls in `FiskalModule.vb`.) | Vendor (Accent) | Pre-installed; runs on the operator PC. |
| **App folder-pairing** | VTE must know *which folder* the driver watches, so it can drop command files there. v2 stores a `FileSystemDirectoryHandle` to that folder in the browser's IndexedDB. | VTE (this app) | Per-PC, per-browser, in **Fiscal options** (`FiscalOptionsView.vue`). |

The flow at runtime:

```
PaymentDocument  ──GET /fiscal-file──▶  Server composes command bytes (base64)
                                              │
                          Browser (fiscal.ts) decodes base64 → Uint8Array
                                              │
                  File System Access API: write "smetkaID{id}.txt" into watched folder
                                              │
                       Accent driver sees the file ─▶ prints on PF-500
                                              │
            Browser POST /fiscal-printed ─▶ Server stamps FiscalPrintedAt (outbox marker)
```

The legacy WinApp did the folder write itself (`My.Computer.FileSystem.WriteAllText` into
`objOpcii.FiskalFolderPath`); v2 moves that single step into the browser because the server is now
remote.

---

## 2. The command-file format (exact byte layout)

The format is reverse-engineered from the legacy
`PecatiFiskalnaSmetaAccentPF500` (`WinApp/Hepers/FiskalModule.vb`) and reproduced byte-for-byte by
`FiscalFile(...)` in `backend-v2/src/VTE.Api/Controllers/PaymentDocumentsController.cs`. All line
breaks are CR-LF (`\r\n`). Field separators inside a line item are an ASCII TAB (`\t`).

### 2.1 Header (open receipt)

| Case | Header line |
|---|---|
| Normal sale | `" 01,0000,1"` (note the **leading space**) |
| Storno (cancellation) | `" U1,0000,1"` |

```csharp
// PaymentDocumentsController.cs — FiscalFile
sb.Append(doc.Stornoed ? " U1,0000,1" : " 01,0000,1").Append("\r\n");
```

This matches the legacy header exactly (`strSmetka &= " 01,0000,1"` / `" U1,0000,1"`).

### 2.2 Line items (one per payment CATEGORY — folded, like legacy)

The receipt does **not** print one line per bill line. Faithful to the legacy pipeline
(`GetPaymentDocumentForFiscalPrintByIdDocument` SP + `PaymentDocumentFiscalPrintList.ContainsP/AddPrice`),
the bill's active, non-`PrePaid` lines are **folded by payment-category name** into one receipt
line per category:

- **Source rows**: `PaymentDocumentLine` where `Active && !PrePaid`, ordered by `Id`, category
  resolved `PriceCatalogId → PriceCatalog.PaymentCategoryGroupId → PaymentCategoryGroup.Name`
  (legacy: `PaymentItemParametars → PaymentItems → PaymentCategories.CategoryName`). The joins are
  INNER — a line whose category chain is broken silently drops out, exactly like the legacy SP.
  Lines flagged `PrePaid` are excluded (legacy SP filter `PrePayed = 0`).
- **Folding**: rows sharing the same category **name** (ordinal compare — per-company duplicate
  category ids like 86/1086 merge, because legacy folded on the name string) collapse into one
  entry. The first row contributes its raw price (`UnitPrice*Quantity`) and keeps its `Discount`
  and `VatPercent`; every subsequent same-name row adds its raw price **pre-rounded through
  `FicalRound`** (legacy `AddPrice(info.Price)` goes through the `Price` getter).

Each folded entry emits:

```
<marker><name (ToLat, Left 24)>\t<VAT-class byte><price>\r\n
```

- **Marker — alternates per folded index.** Even-indexed lines start with `'1`, odd-indexed lines
  start with `" 1"` (leading space). This is faithful to the legacy
  `If boDocument.IndexOf(item) Mod 2 = 0 Then "'1" Else " 1"`.

  ```csharp
  sb.Append(i % 2 == 0 ? "'1" : " 1")   // legacy alternates the marker
  ```

  The exact reason for the alternation lives in the driver; VTE only reproduces the legacy behavior.

- **Name — `Left(ToLat(name), 24)`.** The payment-category name (`PaymentCategoryGroup.Name`) is
  transliterated Cyrillic→Latin by `ToLat` (§3) and then truncated to **24 characters** (`Left`).
  Legacy: `Strings.Left(ToLat(payIteam.CategoryName), 24)` — also the category, via the catalog.

- **TAB** separates the name from the VAT byte (`vbTab` in legacy, `'\t'` in v2).

- **VAT-class byte — a single raw byte** identifying the tax class (these are CP1251 code points for
  Cyrillic А/Б/В):

  | VAT % | Byte | Char | Note |
  |---|---|---|---|
  | 18 % | `192` | А (CP1251) | Standard rate |
  | 5 %  | `193` | Б (CP1251) | Reduced rate |
  | 0 %  | `194` | В (CP1251) | Zero / exempt |
  | other | — | — | **Refused** (see below) |

  ```csharp
  var vatByte = f.VatPercent switch {   // f = the folded entry; VAT comes from the category's FIRST row
      18 => (byte)192,
      5  => (byte)193,
      0  => (byte)194,
      _  => (byte)0,
  };
  if (vatByte == 0)
      return BadRequest(new { error = $"Ставка со непозната ДДВ стапка ({f.VatPercent}%) — не може да се фискализира." });
  ```

  > **Behavior difference vs. legacy.** The legacy code did `Exit For` on an unknown rate — it
  > *silently truncated* the receipt (dropping that line and every line after it). v2 **refuses
  > loudly** with a 400 so a mis-priced line can never produce a quietly-wrong fiscal receipt.

- **Price — legacy `FicalRound` + banker's discount round, printed like VB `FormatNumber`.**
  The folded sum goes through `FicalRound` (legacy `RoundHelper.FicalRound`, ported verbatim:
  fraction pre-rounded to 2 decimals; `≤ 0.49` truncates, `> 0.49` rounds up — so `x.50` always
  goes **up**, unlike banker's). The category's first-row discount is then applied and the result
  rounded to whole denars with `Math.Round(..., 0)` (banker's — matches VB), and finally printed
  via `FormatFiscal` — the exact `FormatNumber(x, 2, IncludeLeadingDigit:=False, GroupDigits:=False)
  .Replace(",", ".")` replica (two decimals, dot separator, **values below 1 print without the
  leading zero**: `0 → ".00"`):

  ```csharp
  var p = FicalRound(f.Accum);
  var price = Math.Round(p - p * f.Discount / 100, 0);
  sb.Append(...).Append((char)vatByte).Append(FormatFiscal(price));
  ```

  Legacy: `cenaSoPopust = Math.Round(item.Price - (item.Price * item.Discount / 100), 0)` where
  `item.Price` is the `FicalRound`-ed folded sum — identical math, verified byte-for-byte against
  100 real bills (see §10 verification note).

A two-line example (names already transliterated, `‹TAB›` = `\t`, `‹A›` = byte 192):

```
'1Operativni trosoci      ‹TAB›‹A›1200.00
 1Atest                   ‹TAB›‹A›600.00
```

### 2.3 Subtotal command

After all line items:

```csharp
sb.Append(" 5 Smetka\t\r\n");   // subtotal command
```

i.e. the literal `" 5 Smetka"` + TAB + CR-LF. Legacy:
`strSmetka &= " " & Chr(53) & " Smetka" & vbTab & vbCrLf` (`Chr(53)` = `'5'`).

### 2.4 Close receipt

| Case | Close line |
|---|---|
| Normal sale | `"%8"` (`Chr(56)` in legacy) |
| Storno | `"%V"` (`Chr(86)` in legacy) |

```csharp
sb.Append(doc.Stornoed ? "%V" : "%8").Append("\r\n");   // close receipt
```

### 2.5 Why the bytes are emitted with a raw `(byte)char` cast

The composed string is **not** UTF-8 encoded. The controller copies each `char` to a byte with a
direct cast so that the VAT-class code points (192/193/194) survive as **single bytes** exactly as
the legacy CP1251 ASCII stream expected:

```csharp
var text = sb.ToString();
var bytes = new byte[text.Length];
for (var i = 0; i < text.Length; i++) bytes[i] = (byte)text[i];
return Ok(new FiscalFileDto(true, null, $"smetkaID{doc.Id}.txt",
    Convert.ToBase64String(bytes), doc.FiscalPrintedAt));
```

The bytes are returned base64-encoded; the browser reverses this with `atob` →
`charCodeAt` (`base64ToBytes` in `fiscal.ts`) so the exact byte stream reaches the folder.

**Consequence:** any `char` above `~` (126) in the composed string would cast to a wrong/garbage
byte (often a control byte) and corrupt the receipt. That is precisely why `ToLat` must fold every
non-ASCII character — see §3.

---

## 3. `ToLat` transliteration and the em-dash control-byte bug

`ToLat` (private method in `PaymentDocumentsController.cs`) maps Macedonian Cyrillic to Latin,
including the digraphs (Ѕ→DZ, Љ→LJ, Њ→NJ, Ѓ→GJ, Ж→ZH, Ќ→KJ, Ч→CH, Ш→SH, Џ→DJ). It is ported from
the legacy `KondnaTastaturaModule.ToLat` (`VTE.Library/KondnaTastaturaModule.vb`).

Two corrections were made versus the legacy version, both safety-critical given the raw `(byte)char`
emission:

1. **Capital "А" off-by-one (legacy bug).** Legacy used
   `If Array.IndexOf(_cyr, ch) > 0 Then ... ` with `_cyr(0) = "А"`. Because the test is `> 0`, the
   very first entry (capital А, index 0) **never matched** the table and fell through to a
   `Select Case` that had no `Case "А"`, so it landed in `Case Else` and emitted the **Cyrillic А
   untransliterated**. In v2 that stray Cyrillic byte would corrupt the fiscal stream. The v2 switch
   explicitly maps `'А' => "A"`.

2. **Unicode dashes → ASCII hyphen, and a hard ASCII fence (the "control-byte bug" fix).** Catalog
   names contained em/en dashes (`— – ― ‒`). Emitted via `(byte)char`, an em-dash (U+2014, 8212)
   truncates to byte `20` — a control byte that breaks the receipt. `ToLat` now folds all Unicode
   dashes to `-`, and **any** remaining char outside printable ASCII becomes `?`:

   ```csharp
   '—' or '–' or '―' or '‒' => "-",   // Unicode dashes → ASCII hyphen
   _ => ch is >= ' ' and <= '~' ? ch.ToString() : "?",  // never emit a stray > 126 byte
   ```

   The legacy code got away without this fence only because it relied on CP1251 encoding at the
   `WriteAllText` step; v2's byte-cast pipeline has no such fallback, so `ToLat` is the single point
   that guarantees an all-ASCII-or-VAT-byte stream.

---

## 4. Gating rules: when a bill fiscalizes

`FiscalFile(...)` returns a `FiscalFileDto` whose `PrintsFiscal` flag tells the browser whether to
write anything. Three gates can skip fiscalization (each returns `PrintsFiscal=false` + a Macedonian
`SkipReason`, **not** an error):

| Gate | Condition | `SkipReason` (MK) |
|---|---|---|
| **Cash-only** | The bill's `PaymentType.IsCash` is false. | "Фискална сметка се печати само за готовинско плаќање." |
| **Zero / empty** | No active lines, or the rounded total is 0. | "Сметката нема износ за фискализација." |
| **Unpaid installment** (rata path) | The requested installment is not yet paid. | "Ратата не е платена — нема што да се фискализира." |

```csharp
// Cash-only gate — legacy Fiskalna_kes flag (PaymentType.IsCash)
var type = await _db.PaymentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == doc.PaymentTypeId);
if (type is not { IsCash: true })
    return Ok(new FiscalFileDto(false, "Фискална сметка се печати само за готовинско плаќање.", "", "", doc.FiscalPrintedAt));
...
// Zero gate — legacy GetTotalAmmount over the FOLDED rows (FicalRound'ed price,
// first-row discount, no whole-denar rounding at the gate).
var total = folded.Sum(f => { var p = FicalRound(f.Accum); return p - p * f.Discount / 100; });
if (folded.Count == 0 || total == 0)
    return Ok(new FiscalFileDto(false, "Сметката нема износ за фискализација.", "", "", doc.FiscalPrintedAt));
```

`IsCash` maps to the legacy `PaymentTypes.Fiskalna_kes` flag
(`backend-v2/src/VTE.Domain/Payments/PaymentType.cs`). The legacy gate was identical:
`If Not plak.FiskalnaKes Then Exit Sub` followed by `If boDocument.GetTotalAmmount = 0 Then Exit Sub`.

> Note: only `IsCash` gates fiscalization in v2. The legacy module had commented-out branches for
> card (`FiskalnaKarticka` → close char `D`) and other tenders; those are dead in legacy and not
> ported.

---

## 5. Installment ("Uplata po rata") receipts

For installment bills (`по договор`), each paid installment gets its own small fiscal receipt,
mirroring the legacy `PecatiFiskalnaSmetaZaRataAccentPF500`. Triggered by passing
`?installment={seq}` to the fiscal-file endpoint (and `installmentSeq` to `printFiscalForDocument`).

The rata receipt is a **single line** labeled `"Uplata po rata"` at **0 % VAT** (byte 194) for the
installment's paid amount:

```csharp
rsb.Append(doc.Stornoed ? " U1,0000,1" : " 01,0000,1").Append("\r\n");
rsb.Append("'1").Append("Uplata po rata").Append('\t').Append((char)194)
   .Append(FormatFiscal(FicalRound((double)(rata.PaidAmount ?? rata.Amount))))
   .Append("\r\n");
rsb.Append(" 5 Smetka\t\r\n");
rsb.Append(doc.Stornoed ? "%V" : "%8").Append("\r\n");
```

The amount goes through `FicalRound` — the legacy rata BO's `Price` getter applies it too
(`PaymentDocumentRataFiscalPrintInfo.Price → FicalRound(_price)`).

Differences from the full-bill path:

- The header text `"Uplata po rata"` is hard-coded (legacy: `Strings.Left(ToLat("Uplata po rata"), 24)`).
- VAT is always 0 % (byte 194) — legacy `ddvStapkaASC = 194`.
- The marker is always `'1` (a single line, always even index).
- File name is `smetkaID{docId}rata{seq}.txt` (the full-bill file is `smetkaID{docId}.txt`).
- The cash-only and zero gates are **not** applied on the rata path (only "is the rata paid?"),
  matching legacy, which commented out the total check for ratas.
- A rata print **does not** touch the document-level `FiscalPrintedAt` marker (see §6).

---

## 6. The `FiscalPrintedAt` outbox marker

`PaymentDocument.FiscalPrintedAt` (`backend-v2/src/VTE.Domain/Payments/PaymentDocument.cs:69`) is a
nullable timestamp acting as an **outbox / printed marker**. It is set by a separate confirmation
call the browser makes only after it has successfully written the command file:

```csharp
[HttpPost("{id:long}/fiscal-printed")]
public async Task<IActionResult> FiscalPrinted(long id) {
    var doc = await _db.PaymentDocuments.FirstOrDefaultAsync(x => x.Id == id);
    if (doc == null) return NotFound();
    doc.FiscalPrintedAt ??= DateTime.UtcNow;   // idempotent — keeps the first timestamp
    await _db.SaveChangesAsync();
    return NoContent();
}
```

Browser side (`fiscal.ts`, `printFiscalForDocument`): the POST is sent **only for full-bill prints**,
never for ratas:

```ts
await writeFiscalFile(folder, data.fileName, base64ToBytes(data.contentBase64));
if (installmentSeq == null)
  await api.post(`/payment-documents/${documentId}/fiscal-printed`);
return { status: 'printed', fileName: data.fileName };
```

The marker is surfaced in the UI: `PaymentView.vue` shows a "Fiscalized" (`Фискализирано`) tag with
the timestamp on hover when `bill.fiscalPrintedAt` is set, and the detail card shows the value
(`payments.col.fiscalPrintedAt`). Because the device protocol is fire-and-forget (the driver never
reports back), `FiscalPrintedAt` records only that *the file was written*, not a confirmed paper
print.

---

## 7. The browser write pipeline (`fiscal.ts`)

`frontend-v2/src/fiscal/fiscal.ts` owns folder selection, permission handling, and the atomic write.

**Folder handle persistence.** The picked `FileSystemDirectoryHandle` is stored in IndexedDB
(`vte-fiscal` DB, `kv` store, key `fiscal-folder`). It is NOT re-picked every session — the browser
re-grants with a one-click permission prompt:

```ts
export async function ensureFolder(interactive: boolean): Promise<FileSystemDirectoryHandle | null> {
  const handle = await getSavedFolder();
  if (!handle) return null;
  let perm = await handle.queryPermission({ mode: 'readwrite' });
  if (perm === 'prompt' && interactive) perm = await handle.requestPermission({ mode: 'readwrite' });
  return perm === 'granted' ? handle : null;
}
```

`requestPermission` requires a **user gesture**, so it only runs when `interactive` is true (manual
"Fiscal receipt" button). Auto-prints after bill creation pass `interactive=false` so they fail
quietly to `no-folder` rather than throwing an un-gestured prompt.

**Atomic write.** `writeFiscalFile` uses `createWritable()`, which commits atomically on `close()`,
so the vendor driver never sees a half-written file. This replaces the legacy `.tx` → `.txt` rename
dance (legacy wrote `smetkaID….tx` then `RenameFile(..., "smetkaID….txt")` so the watcher only ever
saw a complete `.txt`):

```ts
export async function writeFiscalFile(folder, fileName, bytes) {
  const fh = await folder.getFileHandle(fileName, { create: true });
  const w = await fh.createWritable();
  await w.write(bytes.buffer as ArrayBuffer);
  await w.close();   // atomic on close
}
```

**Result type.** `printFiscalForDocument` returns a discriminated union the views switch on:
`printed` | `skipped` (with `reason`) | `no-folder` | `unsupported` | `error` (with `message`).

**Call sites:**

- `DashboardView.vue` — after **Направи сметка** (create bill), an automatic quiet attempt
  (`interactive=false`); for installment bills it prints rata 1 (the down payment):
  `printFiscalForDocument(data.id, false, isRati ? 1 : undefined)`.
- `PaymentView.vue` — the manual **"Фискална сметка"** button (`interactive=true`, full switch on
  the result), and an auto quiet rata receipt when an installment is paid
  (`printFiscalForDocument(props.id, false, seq)`).

---

## 8. Operator setup — Fiscal options (per PC, per browser)

The operator pairs VTE with the watched folder in **Fiscal options** (Macedonian: **Фискални
опции**), at route `/fiscal` (`router/index.ts`, sidebar entry in `AppLayout.vue`), implemented by
`frontend-v2/src/views/FiscalOptionsView.vue`.

Per-PC setup steps:

1. **Open the app in a supported browser on the PC physically connected to the PF-500.** If the
   browser lacks the File System Access API, the page shows the
   `fiscal.unsupported` warning instead of the controls.
2. **Fiscal folder → "Pick folder" (Избери папка).** Opens `showDirectoryPicker` and saves the
   handle to IndexedDB. Pick the *same folder the old application used* — the one the Accent driver
   watches (`fiscal.folderHelp`). The chosen folder name and a permission tag (`allowed` /
   `will ask permission`) are shown.
3. **"Test write into folder" (Тест запис).** Writes `vte-test.txt` with a timestamp into the
   folder so the operator can confirm pairing and permissions without printing a real receipt.
4. **Reports** (sent as command files into the same folder):
   - **Control report (X)** — `printControlReport` writes `DnevenKontrolenIzvestaj.txt` with `" E2"`.
   - **Daily closure (Z)** — `printDailyClosure` writes `DnevnoFiskalnoZatvaranje.txt` with `" E"`.
     Behind a confirm dialog (`fiscal.zConfirm`: "…irreversible for today. Continue?") because Z is
     the irreversible daily fiscal close.

```ts
// fiscal.ts — one-line driver commands (legacy FiskalModule parity)
const ascii = (s: string) => new TextEncoder().encode(s);
export async function printDailyClosure(folder) {     // Z report
  await writeFiscalFile(folder, 'DnevnoFiskalnoZatvaranje.txt', ascii(' E\r\n'));
}
export async function printControlReport(folder) {    // X report
  await writeFiscalFile(folder, 'DnevenKontrolenIzvestaj.txt', ascii(' E2\r\n'));
}
```

These mirror the legacy `PecatiDnevnoFiskalnoZatvaranjePF500` (`" " & Chr(69)` = `" E"`) and
`PecatiDnevenKontrolenIzvestajPF500` (`" " & Chr(69) & "2"` = `" E2"`).

> The handle and permission are **per browser profile on that PC**. A different browser, a new
> profile, or cleared site data means re-picking the folder. There is one "Forget" (Заборави)
> button to clear the saved handle.

### Other legacy device commands not ported to v2

`FiskalModule.vb` also implements commands that v2 does not yet expose in the UI (kept here for
completeness / parity reference):

| Legacy sub | Command | Purpose |
|---|---|---|
| `PecatiSkratenIzvPoDatumePF500` | `" O<from>,<to>"` (`Chr(79)`) | Short report by date range |
| `PecatiDetalenIzvPoDatumePF500` | `" ^<from>,<to>"` (`Chr(94)`) | Detailed report by date range |
| `PromenaNaDatumPF500` | `" =<date>"` (`Chr(61)`) | Set device date |
| `SluzbenoVnesuvanjePariPF500` | `" F<amount>"` (`Chr(70)`) | Cash-in (drawer deposit) |
| `SluzbenoVadenjePariPF500` | `" F-<amount>"` | Cash-out (drawer withdrawal) |

---

## 9. Requirements

- **A Chromium-family browser** (Chrome / Edge). The integration depends on
  `window.showDirectoryPicker` and the File System Access API; `isFiscalSupported()` checks for it.
  Firefox/Safari are unsupported and show `fiscal.unsupported`.
- **A secure context.** The File System Access API requires HTTPS (or `localhost`). Production runs
  over HTTPS (`https://116.202.8.155.sslip.io`); dev runs on `https://localhost:7165` /
  `http://localhost:5173` (localhost is treated as secure).
- **The browser session must run on the PC physically wired to the PF-500**, because the watched
  folder is local to that machine. The server only emits bytes; the *writing* is local.

---

## 10. What CANNOT be tested without the physical device

Because the protocol is one-way file-drop with no driver acknowledgement, the following are
**unverifiable** outside the actual station with a live PF-500:

- That the device **accepts and prints** the composed format (header/marker/VAT-byte/close). Only
  the legacy app's real-world use confirms the byte layout; v2 reproduces it but cannot self-verify.
- The **VAT-class byte mapping** (192/193/194) actually selecting the right tax class on the printed
  receipt.
- Whether the **marker alternation** (`'1` / `" 1"`) matters to the driver — VTE copies legacy
  behavior blind.
- That **X / Z reports** and the other one-line commands produce the correct device actions.
- End-to-end **timing** between the atomic file write and the driver picking it up.

What *can* be tested without the device:

- The **server byte composition** is fully deterministic and unit-testable (header, transliteration,
  VAT bytes, rounding, close — given a `PaymentDocument` + lines).
- The **gating logic** (cash-only, zero, unpaid rata) via the `PrintsFiscal` / `SkipReason` response.
- The **browser pipeline** up to and including the folder write — the **"Test write"** button proves
  folder selection, permission, and atomic write work on a given PC.
- The `FiscalPrintedAt` outbox marker round-trip.

> **Byte-parity verification (2026-07-15).** The v2 composition was verified byte-for-byte against a
> line-for-line simulation of the legacy VB pipeline (SP rows pulled from the LIVE legacy DB,
> `ContainsP`/`AddPrice` folding, `FicalRound`, `FormatNumber`, legacy `ToLat`) on **100 real bills**
> stratified across: plain multi-line, same-category folding, storno, bills with `PrePayed` lines,
> discounts, fractional prices, single-line, and the untrimmed-name categories 135/1135. Result:
> zero mismatches — the only accepted difference is legacy's CP1251 `А` byte (`0xC0`) where v2 emits
> Latin `A` (`0x41`), same glyph on paper (legacy `ToLat`'s index-0 bug, deliberately fixed in v2).

---

## 11. File / endpoint reference

| Concern | Location |
|---|---|
| Server: compose command file | `backend-v2/src/VTE.Api/Controllers/PaymentDocumentsController.cs` → `FiscalFile`, `GET /api/payment-documents/{id}/fiscal-file[?installment=]` |
| Server: outbox marker | same file → `FiscalPrinted`, `POST /api/payment-documents/{id}/fiscal-printed` |
| Server: transliteration | same file → `ToLat`, `Left` |
| Domain: marker field | `backend-v2/src/VTE.Domain/Payments/PaymentDocument.cs` → `FiscalPrintedAt` |
| Domain: cash gate flag | `backend-v2/src/VTE.Domain/Payments/PaymentType.cs` → `IsCash` (legacy `Fiskalna_kes`) |
| Browser: folder + write + commands | `frontend-v2/src/fiscal/fiscal.ts` |
| Operator UI | `frontend-v2/src/views/FiscalOptionsView.vue` (route `/fiscal`) |
| Print buttons (bill detail) | `frontend-v2/src/views/PaymentView.vue` |
| Auto-print after bill | `frontend-v2/src/views/DashboardView.vue` |
| Locale strings | `frontend-v2/src/locales/{mk,en}.ts` → `fiscal.*`, `nav.fiscal` |
| Legacy parity reference | `WinApp/Hepers/FiskalModule.vb`, `VTE.Library/Payment/PaymentDocumentFiscalPrintList.vb` (fold), `VTE.Library/RoundHelper.vb` (FicalRound), `VTE.Library/KondnaTastaturaModule.vb` (ToLat), SP `GetPaymentDocumentForFiscalPrintByIdDocument` |
