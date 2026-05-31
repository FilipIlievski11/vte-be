# Payments module — business rules extracted from legacy

**Sources read:**
- `VTE.Library/Payment/PaymentDocument.vb` (1,065 lines — root receipt/bill)
- `VTE.Library/Payment/PaymentDocumentsDetail.vb` (548 lines — line items)
- `VTE.Library/Payment/PaymentDocumentsRata.vb` (323 lines — installment row)
- `VTE.Library/Payment/PaymentRatiDogovor.vb` (481 lines — installment contract `DogovorZaRati`)
- `VTE.Library/Payment/CalculationItem.vb` (486 lines — fee catalog item)

**Status:** rules extracted from CSLA. Stored-procedure bodies (56 payment-related procs) NOT read in this pass — the VAT formula and the installment-schedule generator likely live in those procs.

## Domain summary

A `PaymentDocument` is a bill/receipt issued for a `Request`. It has:
- One or more **detail lines** (`PaymentDocumentDetails`) — what's being charged. Each line has a price, a VAT rate, an optional line-level discount, and references a `PriceCatalog` entry.
- Zero or more **installment rows** (`PaymentDocumentInstallments`) — if the customer is paying in instalments under a `DogovorZaRati` (installment contract).
- A document-level discount on top of line-level discounts.
- A `Storno` (void/cancelled) flag — receipts are never deleted, just storno'd.
- An optional `InstallmentContractId` (legacy `IdDogovor`) — if the customer signed an installment contract this bill is part of.

A `DogovorZaRati` is the **installment contract** with a guarantor (`Garant`) — name, address, EMBG.

A `CalculationItem` is **a payable fee type with its target bank account** — important: each fee type has its own bank account, because fees go to different government accounts. The `Form` field is the formal regulatory form number.

## Validation rules — PaymentDocument

| Rule | Statement | Source |
|---|---|---|
| **BR-PAY-001** | `DatePay` **required** | `PaymentDocument.vb:380` |
| **BR-PAY-002** | `DateRequired` **required** | `PaymentDocument.vb:382` |
| **BR-PAY-003** | `Note` max 150 | `PaymentDocument.vb:384` |
| **BR-PAY-004** | `PaymentTypeId` (legacy `IdPaymentType`) must be > 0 | `PaymentDocument.vb:386` |
| **BR-PAY-005** | `CustomerVehicleRelationId` must be > 0 | `PaymentDocument.vb:388` |
| **BR-PAY-006** | At least one PaymentDocumentDetail must exist (CheckChild) | `PaymentDocument.vb:390` |
| **BR-PAY-007** | **Sum of installments ≤ sum of detail line items.** "Збирот на ратите не смее да биде поголем од вкупната сметка" — **the installment integrity rule** | `PaymentDocument.vb:391, 393-408` |

## Validation rules — PaymentDocumentDetail (line item)

| Rule | Statement | Source |
|---|---|---|
| **BR-PAY-010** | `Note` max 150 | `PaymentDocumentsDetail.vb:229` |
| **BR-PAY-011** | `NotePrePayed` max 150 | `PaymentDocumentsDetail.vb:231` |
| **BR-PAY-012** | **Discount < 100%.** "Процентот на попустот мора да е помал од 100" — line-level discount rule | `PaymentDocumentsDetail.vb:237, 255-263` |
| **BR-PAY-013** | `PriceCatalogId` (legacy `IdPriceCatalog`) must be > 0 | `PaymentDocumentsDetail.vb:239` |

## Validation rules — PaymentDocumentInstallment (legacy `PaymentDocumentsRata`)

| Rule | Statement | Source |
|---|---|---|
| **BR-PAY-020** | Installment `Note` max 150 | `PaymentDocumentsRata.vb:103` |

(Most installment rules are in commented-out code or in the proc body, not the CSLA class.)

## Validation rules — InstallmentContract (legacy `DogovorZaRati`)

| Rule | Statement | Source |
|---|---|---|
| **BR-PAY-030** | Contract number (`Broj`) max 50 (commented as required — Q-019: still enforced?) | `PaymentRatiDogovor.vb:91-92` |
| **BR-PAY-031** | Guarantor name (`GarantNaziv`) max 50 (commented as required) | `PaymentRatiDogovor.vb:94-95` |
| **BR-PAY-032** | Guarantor address (`GarantAdresa`) max 250 (commented as required) | `PaymentRatiDogovor.vb:97-98` |
| **BR-PAY-033** | Guarantor EMBG (`GartEMB`) max 20 (commented as required) | `PaymentRatiDogovor.vb:100-101` |
| **BR-PAY-034** | `BrNaRati` (number of installments) is an integer — Q-020: min/max bounds? | `PaymentRatiDogovor.vb:24` |

## Validation rules — CalculationItem (fee catalog)

| Rule | Statement | Source |
|---|---|---|
| **BR-PAY-040** | `ItemName` **required** + max 150 | `CalculationItem.vb:130-131` |
| **BR-PAY-041** | `BankAccount` **required** + max 50 (each fee type has its own target bank account) | `CalculationItem.vb:133-134` |
| **BR-PAY-042** | `Bank` (depository bank name) **required** + max 150 | `CalculationItem.vb:136-137` |
| **BR-PAY-043** | `Form` (regulatory form number) **required** + max 50 | `CalculationItem.vb:139-140` |

## Computed totals (visible in legacy CSLA, derived not stored)

The legacy code calls `vkupnoRati` (sum of installments) and `vkupnoDetali` (sum of details) — so the new system should:
- **Computed:** PaymentDocument.TotalAmount = SUM(line.Price * (1 - line.Discount/100)) * (1 - document.Discount/100)
- **Computed:** PaymentDocument.TotalVat = SUM(line.Price * line.Ddv / 100) — note: Q-021 — is VAT applied before or after discount?
- **Computed:** PaymentDocument.TotalInstallments = SUM(installment.Price)
- **Computed:** PaymentDocument.TotalPaid = SUM(installment.Price WHERE Payed=true) + (Payed=true ? TotalAmount : 0)
- **Computed:** PaymentDocument.RemainingBalance = TotalAmount - TotalPaid

**The new system implements these as computed columns or LINQ projections — NOT as stored columns** (avoids consistency bugs).

## Status semantics

A PaymentDocument has these state-bearing flags:
- `Storno` — voided. Voided receipts retain their data but don't count toward any total. Receipts are never hard-deleted.
- `Payed` — paid in full at the document level. For non-installment docs this is set when the customer pays the full amount. For installment docs this is set when ALL installments have `Payed = true`.

## Stored procs of business interest (bodies NOT read)

The following 56 procs were inventoried in audit doc §4.6.2 but their bodies haven't been read. The most business-critical ones — flagged for future deep-read:

- `addPaymentDocument`, `updatePaymentDocument` — likely contain the document-numbering logic (DocumentNumber generation), and likely insert/update CustomerFinancialState rows as a side-effect.
- `GetPaymentDocumentForFiscalPrintByIdDocument` — formats a payment document for the fiscal printer.
- `GetPaymentDocumentPrefix` — generates the document number prefix.
- `addDogovorZaRat`, `updateDogovorZaRat` — installment contract creation; likely contains the schedule generator (split total into N equal-or-decreasing installments with due dates).
- The CustomerFinancialState recompute logic — Q-022.

## Open questions

- **Q-019..Q-022** as noted inline above.
- **Q-023:** Document-numbering scheme — per station? per year? gap-handling on storno?
- **Q-024:** Does VAT include or exclude the line discount? (BR-PAY-012 + Ddv interaction)
- **Q-025:** Late-fee logic on overdue installments — does it exist?
- **Q-026:** What is `Polisa` (Double) on PaymentDocument? Insurance policy reference?
- **Q-027:** What is `IdFakturiraNa` ("invoice billed to") — a different customer ID for B2B billing where the bill is sent to a parent company while the request is for a sub-entity?
