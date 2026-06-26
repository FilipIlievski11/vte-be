using VTE.Domain.Common;

namespace VTE.Domain.Vehicles;

/// <summary>
/// A physical vehicle. Tenant-owned via CompanyId. Trimmed from the legacy 113-column
/// schema down to the ~35 columns operators actually fill in.
/// </summary>
public class Vehicle : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    // Identity
    public string Vin { get; set; } = string.Empty;          // legacy ShellNumber
    public string? EngineNumber { get; set; }
    public string? Plate { get; set; }                       // legacy LastRegistratinNumber
    public DateTime? LastRegistrationValidUntil { get; set; } // legacy Vehicles.LastRegistrationValidTill — the
                                                              // authoritative reg-expiry the MVR forms print
                                                              // (the VehicleRegistration table can hold only a
                                                              // sentinel {Code}-000-AA placeholder for some vehicles)

    // Lookups
    public short? CategoryId { get; set; }
    public int? BodyTypeId { get; set; }
    public int? ModelId { get; set; }
    public short? PrimaryColorId { get; set; }
    public short? SecondaryColorId { get; set; }
    public short? MadeCountryId { get; set; }
    public byte? FuelId { get; set; }
    public byte? SecondFuelId { get; set; }
    public int? EngineTypeId { get; set; }
    public byte? EcoProgramId { get; set; }
    public byte? PaymentCategoryId { get; set; }

    // Engine / power
    public float? EnginePowerKw { get; set; }                // legacy EnginePowerOutPut
    public float? EngineWorkingCapacityCc { get; set; }
    public int? MaxRpm { get; set; }                         // legacy BrojNaVrtezi
    public float? MaxSpeedKmh { get; set; }
    public bool? HasLpg { get; set; }                        // legacy TNG
    public DateTime? ManufactureDate { get; set; }           // legacy MakeDate

    // Size + mass
    public float? LengthMm { get; set; }
    public float? WidthMm { get; set; }
    public float? HeightMm { get; set; }
    public float? EmptyWeightKg { get; set; }                // legacy EmptyWaight
    public float? MaxAllowedWeightKg { get; set; }
    public float? MaxLegalTotalMassKg { get; set; }          // legacy MaxLegVkMasa
    public float? MaxConstructiveTotalMassKg { get; set; }   // legacy MaxKonstVkMasa
    public float? MaxLegalGroupMassKg { get; set; }          // legacy MaxLegVkMasaGrupa (vehicle+trailer)
    public string? TrailerMassWithBrakesKg { get; set; }
    public string? TrailerMassWithoutBrakesKg { get; set; }
    // The values the Plav form actually prints for towing (legacy reg columns):
    public float? MaxTrailerBrakedKg { get; set; }           // legacy MaxKonstVkMasaKocnaPrikolka
    public float? MaxTrailerUnbrakedKg { get; set; }         // legacy MaxKonstVkMasaNeKocnaPrikolka
    public float? MaxHitchLoadKg { get; set; }               // legacy MaxKonstOptovaruvanjeVoPriklucok

    // Axles
    public int? AxleCount { get; set; }                      // legacy NumberOfAxis
    public int? WheelCount { get; set; }
    public int? AxleLoad1Kg { get; set; }                    // legacy OsnoOptovaruvanje1
    public int? AxleLoad2Kg { get; set; }

    // Seats
    public short? Seats { get; set; }
    public short? StandingSeats { get; set; }

    // Emissions / noise
    public float? Co2GKm { get; set; }
    public float? NoiseStaticDb { get; set; }
    public float? NoiseMovingDb { get; set; }

    // Variant text
    public string? TypeText { get; set; }                    // legacy Tip
    public string? ModelVariant { get; set; }                // legacy VehicleModelAdding
    public string? ApprovalMark { get; set; }                // legacy OznakaNaOdobrenie

    // Lifecycle
    public string? Note { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
