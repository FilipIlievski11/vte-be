# Vehicles module — business rules extracted from legacy

**Sources read:**
- `VTE.Library/Vehicles/Vehicle.vb` (3,691 lines — main editable CSLA business object)
- Legacy table DDL (already in audit doc §4.3.2): `Vehicles`, `Vehicle.Registrations`, `Vehicle.Axis`, `Vehicle.BetweenAxesDestinations`, `Vehicle.Tyres`, `VehiclePayToll`, `CustomerVehiclesRelations`

**Status:** rules extracted from CSLA `AddBusinessRules` and the custom-rule helpers (`NumOfAxes`, `NumOfWheels`, `NoDuplicates`, `MinEnginePower`, `SizeHight`). Stored-procedure bodies (77 vehicle-related procs in §4.3.2 inventory) NOT read in this pass.

The Vehicle entity has ~110 registered CSLA properties — much larger than Customer. Many fields are technical specifications (engine, emissions, weights per axle, dimensions). A substantial portion are **transliterated Macedonian** in the legacy code (e.g. `MasaPoOska1` = "axle 1 mass"); the new schema translates these to English.

## Active validation rules

| Rule | Statement | Source |
|---|---|---|
| **BR-VEH-001** | `ShellNumber` (VIN) max 17 chars (matches international VIN standard) | `Vehicle.vb:1668` |
| **BR-VEH-002** | `ShellNumber` min 4 chars | `Vehicle.vb:1669` |
| **BR-VEH-003** | `ShellNumber` **unique per tenant** (uniqueness check via `getVehicleByShellNum` proc; error: `"Шасијата мора да биде единствена"` — "VIN must be unique") | `Vehicle.vb:1670, 1834-1842` |
| **BR-VEH-004** | `FirstRegistrationNumber` **required** + max 50 | `Vehicle.vb:1664, 1671-1672` |
| **BR-VEH-005** | `LastRegistrationNumber` (legacy typo `LastRegistratinNumber`) **required** + max 50 | `Vehicle.vb:1665, 1673-1674` |
| **BR-VEH-006** | `NumberOfAxes ≥ NumberOfPropulsionAxes` — vehicle cannot have more driven axles than total axles. Error: `"Број на оски мора да биде поголем или еднаков на број на носечки оски"` | `Vehicle.vb:1710-1711, 1778-1786` |
| **BR-VEH-007** | `NumberOfWheels ≥ NumberOfPropulsionWheels` — same logic for wheels. Error: `"Број на тркала мора да биде поголем или еднаков на број на носечки тркала"` | `Vehicle.vb:1715-1716, 1824-1832` |

## Commented-out / disabled rules (worth flagging)

The following rules exist in source as comments — they were active at some point but were turned off. The new system should decide whether to re-enable them:

- `EngineNumber` required (BR-VEH-090)
- `MinEnginePower`: `EnginePower > 0` (BR-VEH-091)
- `NumberOfDoors / NumberOfSeats / EmptyWeight / MaxAllowedWeight ≥ 0` (BR-VEH-092..095)
- `MakeDate` required (BR-VEH-096)
- `TrailerWeightWithBrake / WithoutBrake` max 20 (BR-VEH-097, 098)
- `HomologationCertificateNumber` max 100 (BR-VEH-099)
- `Blackening / Pinpoints / FuelConsumption` max 20 (BR-VEH-100..102)
- `Note` max 500 (BR-VEH-103)
- `VehicleSizeHight > 0` ("Висината на возилото мора да е >=0") (BR-VEH-104)
- `EmptyWeight ≤ MaxAllowedWeight` (BR-VEH-105)
- `RegistrationRequired` (vehicle must have at least one registration) (BR-VEH-106)
- `NoDuplicatesRegistrationNumber` (registration number unique) (BR-VEH-107)

→ Q-009: Are any of these enforced today via stored procs or DB constraints? Or just deactivated? Decision needed for new system.

## Reference-data dependencies (FKs in legacy)

The Vehicle entity references many lookup tables. All deferred until REF module migration:

- `BodyTypeId` (legacy `IdVehicleBodyType` — display name "Каросерија" / body type)
- `VehicleCategoryId` (legacy `IdVehicleCategories`) — A/B/C/N1/N2/etc. road-traffic categories
- `VehicleUseId` (legacy `IdVehicleUse`) — private/commercial/etc.
- `EngineTypeId` (legacy `IdEngineType`) — petrol / diesel / hybrid / electric
- `EnginePowerSourceId` + `EngineSecondPowerSourceId` (dual-fuel like LPG)
- `GearBoxId` (legacy `IdGearBox`) — manual / auto / semi-auto
- `BrakesId` (legacy `IdBreakes` — typo) — disc / drum / etc.
- `SupportingId` (legacy `IdSupporting`) — suspension type
- `VehicleModelId` (legacy `IdVehicleModel`) — make + model
- `VehicleCategoryForPaymentsId` — for VAT/registration-fee calculation
- `EngineEcoProgramId` — Euro 4 / 5 / 6 etc.
- `MadeCountryId` (legacy `IdMadeCountry`) — country of manufacture
- `PrimaryColorId` / `SecondaryColorId`
- `FirstRegistrationIssuerId` / `LastRegistrationIssuerId`

## Macedonian → English column rename map

A LOT of legacy columns are transliterated Macedonian. Translation per the audit's rules in §3.5:

| Legacy | New | Meaning |
|---|---|---|
| `MasaPoOska1..5` | `AxleLoad1..5` | mass per axle 1..5 |
| `MasaPoOskaPriklucna` | `TrailerAxleLoad` | trailer (priključna) axle mass |
| `OsnoOptovaruvanje1..5` | `AxleBaseLoad1..5` | axial loading 1..5 |
| `OsnoOptovaruvanjePriklucna` | `TrailerAxleBaseLoad` | trailer axial loading |
| `MaxKonstOptovaruvanjeVoPriklucok` | `MaxConstructiveCouplingLoad` | max constructive load on coupling |
| `MaxKonstVkMasaKocnaPrikolka` | `MaxConstructiveBrakedTrailerMass` | "Vk masa" = total mass; "kočna" = braked |
| `MaxKonstVkMasaNeKocnaPrikolka` | `MaxConstructiveUnbrakedTrailerMass` | un-braked trailer |
| `MaxKonstVkMasa` | `MaxConstructiveTotalMass` | constructive total mass |
| `MaxLegVkMasa` | `MaxLegalTotalMass` | "leg" = legalna |
| `MaxLegVkMasaGrupa` | `MaxLegalTotalMassGroup` | per axle group |
| `OdnosKwCcm` | `KwToCcRatio` | kW per cc engine ratio |
| `OznakaNaOdobrenie` | `ApprovalMark` | type-approval mark |
| `OznakaNaOdobrenieZaPriklucUred` | `CouplingDeviceApprovalMark` | trailer-coupling approval |
| `TBrOdobrenieMehanPriklucok` | `MechanicalCouplingApprovalNumber` | |
| `TMarkaMehanPriklucok` | `MechanicalCouplingMark` | |
| `TMaxHorVerOptovaruvanjePriklucok` | `MaxHorizontalVerticalCouplingLoad` | |
| `TMaxKonstVkMasaNaKombinacija` | `MaxConstructiveCombinationMass` | tractor + trailer combined |
| `TMaxKonstVkMasaPoluprikolka` | `MaxConstructiveSemiTrailerMass` | "polu-prikolka" = semi-trailer |
| `TMaxKonstVkMasaPrikolka` | `MaxConstructiveTrailerMass` | |
| `TMaxKonstVkMasaPrikolkaSoCenOska` | `MaxConstructiveTrailerMassWithCentralAxle` | |
| `TMaxKonstVkMasaPrikolkaStoMozePrikluci` | `MaxConstructiveAttachableTrailerMass` | |
| `TMinMasa` | `MinMass` | |
| `TTipMehanPriklucok` | `MechanicalCouplingType` | |
| `TZastitnaKabina` | `ProtectiveCabin` | |
| `TZastitnaRamka` | `ProtectiveFrame` (rollbar) | |
| `VarijantaIzvedba` | `VariantImplementation` | |
| `BrojEUPotvrda` | `EUCertificateNumber` | EU type-approval certificate |
| `BrojNaVrtezi` | `RPM` | engine revolutions |
| `IdentifikacijaNaMotorMestoMetod` | `EngineIdentificationLocationMethod` | where/how the engine is ID'd |
| `Vitlo` | `Winch` | already in §2.1 glossary |
| `Tip` | `Type` | |

Plus typo fixes: `IdBreakes`→`BrakesId`, `LastRegistratinNumber`→`LastRegistrationNumber`, `EmptyWaight`→`EmptyWeight`, `MaximunAllowedWaight`→`MaxAllowedWeight`, `VehicleSizeHight`→`VehicleHeight`, `TrailerWaightWithBreak`→`TrailerWeightBraked`, `TrailerWaightWithoutBreak`→`TrailerWeightUnbraked`, `HologationSertificateNumber`→`HomologationCertificateNumber`, `EngineTorqueUnderGass`→`EngineTorqueUnderGas`, `NoiseMovment`→`NoiseMovement`.

## Open questions

- **Q-009:** Which of the 14 commented-out validation rules are still enforced via DB constraints or proc-side checks? (See "Commented-out / disabled rules" above.)
- **Q-010:** Can a `Vehicle` exist without any `VehicleRegistration`? Legacy code has the `RegistationRequired` rule commented out — implies no DB-level enforcement.
- **Q-011:** The `Vehicle.Tyres` table is a child of Vehicle (per-vehicle tyres), but the audit also has `VehicleTireTypes` as a REF lookup. Are these the same concept or distinct (per-vehicle inventory vs catalog of types)? From CSLA, `VehicleTyres` is the per-vehicle child collection and `VehicleTireTypes` is the lookup. Confirm.
- **Q-012:** What does `IsSocialNotPrivate` mean? "Social vehicle, not private" — possibly state-owned / publicly-owned? Macedonian context needed.
- **Q-013:** What's the difference between the various trailer-mass fields (`MaxConstructiveTrailerMass`, `MaxConstructiveSemiTrailerMass`, `MaxConstructiveCombinationMass`, etc.)? Probably defined by EU vehicle-type-approval directives — confirm with regulatory ref.
