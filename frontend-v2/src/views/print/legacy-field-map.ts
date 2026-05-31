// Maps legacy DevExpress XtraReport control names (lbl*, XrLabel*, *CheckBox*)
// to functions that extract the right value from our PrintBundle.
//
// Coverage:
//   - Most labels resolve from the bundle directly (vehicle.vin, client.mb, etc.)
//   - A few are computed (e.g. lblIsReady/lblIsNotReady set the technical-exam
//     stamp; checkbox visibility depends on RequestType options)
//   - Anything we don't know is left blank — the corresponding paper field
//     will simply be empty on output.
//
// Conventions in the legacy code:
//   - "Color1" = primary color description, "Color11" = primary color code
//   - "Color2" = secondary color description, "Color21" = secondary color code
//   - "Old*" / no-prefix = current/from side of a transfer
//   - "New*" = transferred-to side (only set when TransfersOwnership)

import type { RequestPrintBundle, PrintClientMeta, PrintVehicleMeta } from '@/types';

type Resolver = (b: RequestPrintBundle) => string;

function fmtDate(iso: string | null | undefined): string {
  if (!iso) return '';
  const d = new Date(iso);
  if (Number.isNaN(d.getTime())) return '';
  const dd = String(d.getDate()).padStart(2, '0');
  const mm = String(d.getMonth() + 1).padStart(2, '0');
  return `${dd}/${mm}/${d.getFullYear()}`;
}

function num(v: number | null | undefined, digits = 0): string {
  if (v == null) return '';
  return digits > 0 ? v.toFixed(digits) : String(Math.round(v));
}

function clientLine(c: PrintClientMeta | null | undefined): string {
  if (!c) return '';
  return c.fullName ?? '';
}

function vehicleMakerModel(v: PrintVehicleMeta | null | undefined): string {
  if (!v) return '';
  return [v.maker, v.model].filter(Boolean).join(' ');
}

/** Legacy address rendering: "УЛ. {street} БР.{number} {city}" — we don't
 *  have the parts broken out, so just upper-case the full migrated string. */
function fullAddress(c: PrintClientMeta | null | undefined): string {
  if (!c?.address) return '';
  return c.address;
}

// Common-case fields shared across templates ----------------------------------

const FIELD_MAP: Record<string, Resolver> = {
  // ----- Vehicle identity / classification -----
  lblShellNumber:     b => b.vehicle?.vin ?? '',
  lblShellNumberOld:  b => b.vehicle?.vin ?? '',
  lblEngineNumber:    b => b.vehicle?.engineNumber ?? '',
  lblEngineNumberOld: b => b.vehicle?.engineNumber ?? '',
  lblEngineType:      b => b.vehicle?.engineTypeName ?? '',
  lblBodyType:        b => b.vehicle?.bodyType ?? '',
  lblCategoryPayment: b => b.vehicle?.categoryForPayments ?? '',
  lblColor1:          b => b.vehicle?.primaryColorName ?? '',
  lblColor11:         b => b.vehicle?.primaryColorCode ?? '',
  lblColor2:          b => b.vehicle?.secondaryColorName ?? '',
  lblColor21:         b => b.vehicle?.secondaryColorCode ?? '',
  lblModel:           b => b.vehicle?.model ?? '',
  lblType:            b => b.vehicle?.maker ?? '',
  lblTypeMaker:       b => b.vehicle?.maker ?? '',
  lblVariant:         b => b.vehicle?.variant ?? '',
  lblFuel:            b => b.vehicle?.fuelName ?? '',
  lblCarreingCapacity:b => num(b.vehicle?.maxAllowedWeightKg) || num(b.vehicle?.emptyWeightKg),  // legacy: MaximunAllowedWaight
  lblEmptyWeight:     b => num(b.vehicle?.emptyWeightKg),
  lblMaxAllowedWeight:b => num(b.vehicle?.maxAllowedWeightKg),

  // ----- Last registration -----
  lblLastRegistration:        b => b.lastRegistration?.plateNumber ?? '',
  lblLastRegistrationValidTill: b => fmtDate(b.lastRegistration?.validUntil),
  XrLabel28:                  b => fmtDate(b.lastRegistration?.validUntil),
  XrLabel3:                   b => b.lastRegistration?.plateNumber ?? '',
  // Technical-exam organization name (from BindingSourceLastTehnicalExam.OrganizationName).
  // We don't have the tech-exam module yet — fall back to the registration issuer.
  XrLabel17:                  b => b.lastRegistration?.issuer ?? '',

  // ----- XrLabel* anonymous controls (identified via DataBindings in Designer.vb) -----
  XrLabel4:  b => b.client?.firstName ?? '',                        // CustomerSurname (legacy field name = our firstName)
  XrLabel5:  b => b.client?.lastName ?? '',                         // CustomerFirstName (legacy field name = our lastName)
  XrLabel7:  b => b.client?.address ?? '',                          // LivingAddressNumber (legacy address column)
  XrLabel8:  _ => '; ',                                              // static separator on legacy
  XrLabel9:  b => b.client?.cityName ?? '',                         // CityName
  XrLabel10: b => b.client?.mb ?? '',                               // EMBG
  XrLabel15: b => num(b.vehicle?.enginePowerKw),                    // EnginePower (kW)
  XrLabel16: b => num(b.vehicle?.engineWorkingCapacityCc),          // EngineWorkingCapacity (cc)
  XrLabel18: b => num(b.vehicle?.seats),                            // NumberOfSeats
  XrLabel24: b => num(b.vehicle?.standingSeats),                    // NumberOfStandingSeats

  // ----- Current owner (existing client side) -----
  // Macedonian DB convention: firstName field holds the SURNAME (Презиме),
  // lastName field holds the GIVEN NAME (Име) — opposite of what the legacy
  // labels are named. Swap them on the way out.
  lblFirstName:     b => b.client?.lastName ?? '',     // Име
  lblSurname:       b => b.client?.firstName ?? '',    // Презиме
  lblFullName:      b => b.client?.fullName ?? '',
  lblEMBG:          b => b.client?.mb ?? '',
  lblBirthDate:     b => fmtDate(b.client?.dateOfBirth),
  lblLivingAddress: b => b.client?.address ?? '',
  lblStreet:        b => b.client?.address ?? '',
  lblCity:          b => b.client?.cityName ?? '',
  lblCommunity:     b => b.client?.communityName ?? '',
  lblOpstina:       b => b.client?.communityName ?? b.company?.communityName ?? '',
  lblCountry:       b => b.client?.countryName ?? '',
  lblLivingCountry: b => b.client?.countryName ?? '',
  lblCitizenship:   b => b.client?.citizenshipName ?? '',
  lblBirthCity:     b => '',           // not currently captured
  lblBirthCommunity:b => '',           // not currently captured
  lblBirthCountry:  b => '',
  lblOccupation:    b => '',
  lblBusinessType:  b => b.client?.isBusiness ? 'ДОО' : '',
  lblTaxNumber:     b => b.client?.taxNumber ?? '',
  lblPhone:         b => b.client?.phoneNumber ?? '',
  lblEmail:         b => b.client?.email ?? '',
  lblWorksInCompany:b => '',

  // ----- New owner (only when TransfersOwnership) -----
  lblNewFirstName:        b => b.newClient?.lastName ?? '',     // Име
  lblNewSurname:          b => b.newClient?.firstName ?? '',    // Презиме
  lblNewFirstNameCompany: b => b.newClient?.lastName ?? '',
  lblNewSurnameCompany:   b => b.newClient?.firstName ?? '',
  lblNewEMBG:             b => b.newClient?.mb ?? '',
  lblNewEMBGCompany:      b => b.newClient?.mb ?? '',
  lblNewBirthDate:        b => fmtDate(b.newClient?.dateOfBirth),
  lblNewLivingAddress:    b => b.newClient?.address ?? '',
  lblNewLivingAddressCompany: b => b.newClient?.address ?? '',
  lblNewCity:             b => b.newClient?.cityName ?? '',
  lblNewCityCompany:      b => b.newClient?.cityName ?? '',
  lblNewCommunity:        b => b.newClient?.communityName ?? '',
  lblNewCommunityCompany: b => b.newClient?.communityName ?? '',
  lblNewLivingCountry:    b => b.newClient?.countryName ?? '',
  lblNewLivingCountryCompany: b => b.newClient?.countryName ?? '',
  lblNewCitizenship:      b => b.newClient?.citizenshipName ?? '',
  lblNewBirthCity:        b => '',
  lblNewBirthCommunity:   b => '',
  lblNewBirthCountry:     b => '',
  lblNewOccupation:       b => '',
  lblNewBusinessTypeCompany: b => b.newClient?.isBusiness ? 'ДОО' : '',
  lblNewWorksInCompany:   b => '',

  // ----- Request artifacts -----
  lblOwnershipProof: b => {
    const p = b.ownershipProofs?.[0];
    if (!p) return '';
    return [p.typeName, p.detail].filter(Boolean).join(' ');
  },
  lblPaymentProof: b => {
    const p = b.paymentProofs?.[0];
    if (!p) return '';
    return [p.typeName, p.detail].filter(Boolean).join(' ');
  },

  // ----- Technical-exam stamp -----
  // The legacy app shows the appropriate one as visible based on the last exam
  // result. We don't have that data yet, so leave both blank — the operator
  // can fill the box by hand on the printed paper.
  lblIsReady:    _ => '',
  lblIsNotReady: _ => '',
};

/** Resolve a single layout control's display text. */
export function resolveFieldText(controlName: string, bundle: RequestPrintBundle): string {
  const fn = FIELD_MAP[controlName];
  return fn ? fn(bundle) : '';
}

/** True when a control name represents a checkbox. */
export function isCheckBox(controlName: string): boolean {
  return controlName.startsWith('CheckBox') || controlName.startsWith('XrCheckBox');
}

/**
 * Determine whether a checkbox should be shown as checked. We mirror the
 * legacy runtime logic at a high level:
 *   CheckBoxIsCompany / IsNotCompany — driven by client.isBusiness
 *   CheckBoxA / CheckBoxB / CheckBoxV — driven by request-type variant
 *   XrCheckBox1..8 (Zelen) — driven by vehicle payment category leading digit
 *
 * For unknown checkboxes we render an empty box; the operator can tick the
 * right one on the printed paper.
 */
export function isCheckBoxChecked(controlName: string, bundle: RequestPrintBundle): boolean {
  const c = bundle.client;
  const v = bundle.vehicle;
  const t = bundle.type;

  switch (controlName) {
    case 'CheckBoxIsCompany':    return c?.isBusiness === true;
    case 'CheckBoxIsNotCompany': return c?.isBusiness === false;
    case 'CheckBoxA': return /^A/i.test(t?.name || '');
    case 'CheckBoxB': return /^Б|^B/i.test(t?.name || '');
    case 'CheckBoxV': return /^В|^V/i.test(t?.name || '');
  }
  // Vehicle payment-category leading digit → XrCheckBox{digit}
  const m = controlName.match(/^XrCheckBox(\d)$/);
  if (m && v?.categoryForPayments) {
    const lead = v.categoryForPayments.trim().charAt(0);
    return lead === m[1];
  }
  return false;
}
