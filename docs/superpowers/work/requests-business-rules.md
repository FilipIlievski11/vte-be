# Requests module — business rules extracted from legacy

**Sources read:**
- `VTE.Library/Requests/Request.vb` (984 lines — main editable CSLA business object)
- `VTE.Library/Requests/RequestType.vb` (701 lines — drives the request workflow via boolean flags)
- Legacy table DDL (audit doc §4.4.2): `Requests`, `Request.VehicleOwnershipProofs`, `Request.PaymentProof`

**Status:** rules extracted from CSLA. Stored-procedure bodies (14 request-related procs) NOT read in this pass.

## RequestType is a configurable workflow engine

This is the core finding. A `RequestType` has 10 boolean / integer flags that drive what each Request must contain and what side-effects the Request execution causes:

| Flag | Meaning | Drives… |
|---|---|---|
| `IsTechnicalExamRequired` | int (legacy `IsTehnicalExamRequired` — typo) | does this request type require the vehicle to pass a technical inspection? Not just yes/no — int suggests tri-state (0=no, 1=yes, 2=optional/conditional). |
| `IsPayRequired` | bool | request needs at least one PaymentProof linked before it can be ended |
| `IsNewRegistration` | bool | request issues a new registration number on the vehicle |
| `IsRelationDeleted` | bool | request **deletes** the customer-vehicle relation (deregistration) |
| `IsVehicleDeleted` | bool | request **deletes** the vehicle entirely (vehicle write-off) |
| `IsNewCustomer` | bool | request transfers ownership to a new customer; requires `NewCustomerVehicleRelationId` |
| `IsVehicleChanged` | bool | request mutates the vehicle's technical data |
| `IsCustomerChanged` | bool | request mutates the customer's data |
| `IsSufficient` | bool | request can be issued without further documentation (Q-014: confirm) |
| `IsPreviousRegistrationRequired` | bool (legacy `IsPreviosRegistrationReqired` — typo) | request must reference a previous registration |

Plus structural fields:
- `IdRequestType` (parent type — hierarchical) — Q-015: how is the parent used?
- `IdDocumentPrint` → ties to one of three colored print templates (Plav/Bel/Zelen) per `DocumentTypePrint`
- `TypeName` (max 250, **required**)
- `TypeDescription` (max 250)

## Validation rules — Request

| Rule | Statement | Source |
|---|---|---|
| **BR-REQ-001** | `DateCreated` **required** | `Request.vb:370` |
| **BR-REQ-002** | `Note` max 250 | `Request.vb:372` |
| **BR-REQ-003** | `RequestTypeId` (legacy `IdRequestType`) must be > 0 (foreign-id-select) | `Request.vb:374` |
| **BR-REQ-004** | `CustomerVehicleRelationId` must be > 0 — every request is for a specific (customer, vehicle, relation) tuple | `Request.vb:376` |
| **BR-REQ-005** | When the chosen `RequestType.IsNewCustomer = true`, `NewCustomerVehicleRelationId` (legacy `IdCustomerVehicleRelationNew`) must be set (>=1). Error: `"Потребно е да се внесе новиот сопственик"` ("New owner must be entered") | `Request.vb:379-380, 398-408` |
| **BR-REQ-006** | When `RequestType.IsPreviousRegistrationRequired = true`, `PreviousRegistrationId` must be set (currently **commented out** — Q-016: still enforced via proc?) | `Request.vb:383-384, 410-420` |

## Validation rules — RequestType

| Rule | Statement | Source |
|---|---|---|
| **BR-REQ-020** | `TypeName` **required** + max 250 | `RequestType.vb:284-285` |
| **BR-REQ-021** | `TypeDescription` max 250 | `RequestType.vb:287` |

## Workflow / lifecycle (extracted from properties)

A Request goes through three timestamps:
1. `DateCreated` (REQUIRED at creation; matched with `CreatedByOperatorId` audit)
2. `DateModified` (set on each subsequent edit; matched with `ModifiedByOperatorId`)
3. `DateEnded` (set when the request is finalized; matched with `EndedByOperatorId`)

A Request also has two boolean trace flags reflecting whether the request actually applied changes when ended:
- `IsCustomerChanged` — true if the request modified the customer's data
- `IsVehicleChanged` — true if the request modified the vehicle's data

These mirror `RequestType.IsCustomerChanged` / `IsVehicleChanged` — Q-017: are these derived (computed when ending the request) or independently set (operator overrides)?

## Side-effects driven by RequestType flags

When a Request is ended (`DateEnded` set), the workflow is expected to execute side-effects based on the RequestType's flags:

- `IsNewRegistration` → issue a new `VehicleRegistrations` row (registration number generation logic — Q-018: where? proc or app?)
- `IsRelationDeleted` → set `CustomerVehicleRelations.IsActive = 0` (or `ValidTo = today`)
- `IsVehicleDeleted` → set `Vehicles.IsActive = 0`
- `IsNewCustomer` → activate `NewCustomerVehicleRelationId`, deactivate the old `CustomerVehicleRelationId`

These side-effects are likely implemented in the `updateRequest` stored proc body (Q-018).

## Children (CSLA composition)

- `RequestVehicleOwnershipProofs` — multiple ownership-proof attachments per request (legacy `Request.VehicleOwnershipProofs`)
- `RequestPaymentProofs` — multiple payment-proof attachments per request (legacy `Request.PaymentProof`)
- `RequestAttachments` — generic attachments (file uploads — TWAIN scans in legacy)

## Open questions

- **Q-014:** `RequestType.IsSufficient` — what does this gate?
- **Q-015:** `RequestType.IdRequestType` (self-reference) — hierarchy of types? Composition?
- **Q-016:** Is the `PreviousRegistrationRequired` rule (BR-REQ-006) enforced anywhere now?
- **Q-017:** Are `Request.IsCustomerChanged` / `IsVehicleChanged` derived from actual changes, or operator-set flags?
- **Q-018:** Side-effects of ending a request — which RequestType flags trigger which DB writes? Likely embedded in `updateRequest` proc body.
