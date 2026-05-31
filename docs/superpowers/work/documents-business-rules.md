# Documents module — business rules extracted from legacy

**Sources read:**
- `VTE.Library/Documents/Document.vb` (1,017 lines — generic workflow root for all issued documents)
- `VTE.Library/Documents/DocumentsTrafficLicence.vb` (670 lines)
- `VTE.Library/TehnicalExams/DocumentsTehnicalExamsReport.vb` (1,738 lines — the regulatory heart of the business)
- `VTE.Library/TehnicalExams/DocumentsTehnicalExamsReportsDetail.vb` (414 lines — per-part check results)

**Status:** rules extracted from CSLA. Stored-procedure bodies (43 document/tech-exam-related procs) NOT read in this pass — the document-numbering and validity-period generators likely live there.

## Domain summary

The legacy uses table-per-type inheritance: `Documents` is a generic workflow root, with concrete document types (`DocumentsTrafficLicence`, `DocumentsPermisions`, `DocumentsInternationalDriveingLicences`, `DocumentsTehnicalExamsReports`) as side-tables that join by document Id. Each document type has its own data fields.

In the new schema, each concrete document type is its own table (no abstract parent — EF Core friendly).

## Document types built in this pass

1. **TrafficLicence** — the road-traffic licence (registration document). Has extensions over time.
2. **Permission** — special permission documents.
3. **InternationalDrivingLicence** — international driving licence + valid-for category list.
4. **TechnicalExamReport** — **the actual inspection result** with brake-test measurements per axle, emissions, effects, and a per-part check-list. Pass/fail (`VehicleIsRight`).

## Validation rules — Document (generic workflow shell)

| Rule | Statement | Source |
|---|---|---|
| **BR-DOC-001** | `DateCreated` **required** | `Document.vb:347` |
| **BR-DOC-002** | `VehicleOwnershipProof` (free-text proof reference) max 250 | `Document.vb:349` |
| **BR-DOC-003** | `Note` max 250 | `Document.vb:351` |
| **BR-DOC-004** | `DocumentTypeId` must be > 0 (custom rule `IdGraterThan`) | `Document.vb:358` |
| **BR-DOC-005** | `CustomerVehicleRelationId` must be > 0 (custom rule `IdGraterThanRelation`) | `Document.vb:359` |
| **BR-DOC-006** | When `DocumentTypeOptionId` requires previous registration, `PreviousRegistrationId` must be set | `Document.vb:364-365` |

## Validation rules — TrafficLicence

| Rule | Statement | Source |
|---|---|---|
| **BR-DOC-100** | `TrafficLicenceNumber` max 50 (commented as required — Q-028) | `DocumentsTrafficLicence.vb:174-175` |
| **BR-DOC-101** | `MadeDate` (issue date) **required** | `DocumentsTrafficLicence.vb:177` |
| **BR-DOC-102** | `EndDate` (expiry date) **required** | `DocumentsTrafficLicence.vb:179` |
| **BR-DOC-103** | `Note` max 250 | `DocumentsTrafficLicence.vb:181` |
| **BR-DOC-104** | `CustomerVehicleRelationId` must be > 0 | `DocumentsTrafficLicence.vb:184` |

## Validation rules — TechnicalExamReport (the inspection result)

| Rule | Statement | Source |
|---|---|---|
| **BR-DOC-400** | `TechnicalExamTypeId` (legacy `IdTypeOfTehnicalExam`) must be > 0 | `DocumentsTehnicalExamsReport.vb:655` |
| **BR-DOC-401** | `OrganizationForTechnicalExamId` must be > 0 | `DocumentsTehnicalExamsReport.vb:657` |
| **BR-DOC-402** | `FirstInspectorOperatorId` (legacy `IdFirsControler` — typo) must be > 0 | `DocumentsTehnicalExamsReport.vb:659` |
| **BR-DOC-403** | **`MadeDate ≤ ValidTillDate`** — exam date must precede or equal expiry date (custom rule `MadeDateGTValidTillDate`) | `DocumentsTehnicalExamsReport.vb:666-667` |
| **BR-DOC-404** | First and second inspectors should be different (commented out — Q-029: still enforced?) | `DocumentsTehnicalExamsReport.vb:669-670` |

## Inspection measurements captured (TechnicalExamReport)

This is the regulatory body of the document. The new schema preserves all measurement fields:

### Brake force tests — per axle (5 axles incl. parking)

For each axle 1, 2, 3, 4, plus parking-axle:
- `axisN_Left` — brake force on left wheel (decimal, units TBD — likely kN or daN)
- `axisN_Right` — brake force on right wheel
- `axisN_Gj` — likely braking efficiency / "горна јачина" (upper strength) — Macedonian abbreviation, Q-030
- `axisN_LeftPj` — left-right difference / "разлика помеѓу" — Q-030
- `axisN_PN` — coefficient — Q-030

= 5 fields × 5 axles = **25 measurement columns**

### Vehicle weight
- `Weight` (legacy `waight` — typo)

### Brake effect (overall)
- `EffectOfWorkingBrakeEmpty` (legacy `effectOfWorkingBreakEmpty` — "Break"→"Brake" typo)
- `EffectOfWorkingBrakeFull` (with load)
- `EffectOfSecondaryBrake` (handbrake)
- `EffectOfParkingBrake`

### Speed test
- `SpeedOfTurns` — speedometer reading at known speed?

### Emissions (catalytic converter / petrol)
- `CO` (carbon monoxide %)
- `NumEngineTurns` (RPM during measurement)
- `COPlusTurns` (CO at high RPM)
- `Lambda` (air-fuel ratio coefficient — for petrol/LPG cars with cat converter)

### Diesel-specific
- `Pinpoints` — light-absorption value (smoke opacity, m⁻¹)

### Other
- `Noise` — noise level dB(A)
- `TempOfEngineOil` — oil temperature at measurement (must be in spec range)
- `TechnicalChanges` — free-text describing any modifications

### Header
- `RegNumber` (vehicle's current registration number at exam time)
- `MadeDate` (when the exam was performed)
- `ValidTillDate` (when the exam result expires; typically 6 or 12 months)
- `VehicleIsRight` (BIT — **the pass/fail flag**; `True` = passed, `False` = failed)
- `ExplanationNote`, `DriversWarning`, `Note` (free-text)

### Children
- **`TechnicalExamReportDetails`** — per-part check (Front/Back/OnLeft/OnRight position flags, status, date entered, note). Linked to `TehnicalExamVehicleParts` REF (the catalog of parts to check).
- **`TechnicalExamReportVisualErrors`** — visual defects checklist.

## Validation rules — TechnicalExamReportDetail (per-part check)

| Rule | Statement | Source |
|---|---|---|
| **BR-DOC-450** | `DateEnter` **required** | `DocumentsTehnicalExamsReportsDetail.vb:197` |
| **BR-DOC-451** | `Note` max 150 | `DocumentsTehnicalExamsReportsDetail.vb:199` |
| **BR-DOC-452** | `TechnicalExamVehiclePartId` must be > 0 | `DocumentsTehnicalExamsReportsDetail.vb:201` |
| **BR-DOC-453** | `StatusId` must be > 0 (the pass/fail/conditional status per part) | `DocumentsTehnicalExamsReportsDetail.vb:203` |

## Open questions

- **Q-028:** Is `TrafficLicenceNumber` actually required? CSLA rule is commented out; possibly enforced by proc.
- **Q-029:** Is the "two inspectors must differ" rule (BR-DOC-404) enforced anywhere?
- **Q-030:** What do `Gj`, `LeftPj`, `PN` Macedonian abbreviations stand for in the brake-test columns? Need regulatory reference (likely from Macedonian Ministry of Transport rulebook).
- **Q-031:** Brake-force measurement units — kN, daN, kgf? Confirm.
- **Q-032:** TechnicalExamReport validity period — 6 months, 12 months, or depends on vehicle category? Likely encoded in proc body.
- **Q-033:** What happens to the existing TechnicalExamReport when a vehicle fails? Is a new report created on retest, or is the existing one updated?
