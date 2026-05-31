using VTE.Domain.Common;

namespace VTE.Domain.Vehicles;

// Vehicle entity — see docs/superpowers/work/vehicles-business-rules.md
// Many of the 110+ legacy properties are technical-spec measurements that
// don't need separate navigation. Navigation properties are added per-need.
public class Vehicle : TenantEntity
{
    public long Id { get; set; }

    // Identity (BR-VEH-001..003)
    public string ShellNumber { get; set; } = string.Empty;  // VIN; max 17, min 4, unique per tenant

    // Classification (FKs to REF)
    public int? BodyTypeId { get; set; }
    public int? VehicleCategoryId { get; set; }
    public int? VehicleUseId { get; set; }
    public int? VehicleModelId { get; set; }
    public string? VehicleModelAdding { get; set; }
    public int? MadeCountryId { get; set; }
    public int? VehicleCategoryForPaymentsId { get; set; }

    // Engine
    public string? EngineNumber { get; set; }
    public int? EngineTypeId { get; set; }
    public int? EnginePowerSourceId { get; set; }
    public int? EngineSecondPowerSourceId { get; set; }
    public int? EngineEcoProgramId { get; set; }
    public decimal? EnginePowerKw { get; set; }
    public decimal? EngineWorkingCapacity { get; set; }
    public int? RPM { get; set; }
    public int? GearBoxId { get; set; }

    // Brakes / suspension
    public int? BrakesId { get; set; }
    public int? SupportingId { get; set; }

    // Geometry
    public decimal? VehicleHeight { get; set; }
    public decimal? VehicleWidth { get; set; }
    public decimal? VehicleLength { get; set; }

    // Doors / seats / wheels / axles (BR-VEH-006, BR-VEH-007 enforced via DB CHECKs)
    public int? NumberOfDoors { get; set; }
    public short? NumberOfSeats { get; set; }
    public short? NumberOfStandingSeats { get; set; }
    public short? NumberOfLyingSeats { get; set; }
    public int? NumberOfAxes { get; set; }
    public int? NumberOfPropulsionAxes { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? NumberOfPropulsionWheels { get; set; }

    // Mass
    public decimal? EmptyWeight { get; set; }
    public decimal? MaxAllowedWeight { get; set; }

    // Build / registration history (denormalized; full history in VehicleRegistrations)
    public DateOnly? MakeDate { get; set; }
    public string FirstRegistrationNumber { get; set; } = string.Empty;  // BR-VEH-004
    public string LastRegistrationNumber { get; set; } = string.Empty;   // BR-VEH-005
    public DateOnly? FirstRegistrationMakeDate { get; set; }
    public DateOnly? FirstRegistrationValidTill { get; set; }
    public DateOnly? LastRegistrationMakeDate { get; set; }
    public DateOnly? LastRegistrationValidTill { get; set; }
    public int? FirstRegistrationIssuerId { get; set; }
    public int? LastRegistrationIssuerId { get; set; }

    // Color
    public string? ColorCode { get; set; }
    public int? PrimaryColorId { get; set; }
    public int? SecondaryColorId { get; set; }

    // Equipment
    public bool Suffocation { get; set; }
    public bool Hook { get; set; }
    public bool Winch { get; set; }
    public bool TNG { get; set; }

    public string? Note { get; set; }

    // ===== Print-only fields (legacy parity). Many of these are denormalized
    //       readings that the regulatory PDFs bind to directly. Backfilled from
    //       legacy dbo.Vehicles by LegacyMigrationService.
    public string? Tip { get; set; }                                    // legacy Tip nvarchar(150)
    public string? BrojEUPotvrda { get; set; }                          // EU certificate number
    public string? OznakaNaOdobrenie { get; set; }                      // homologation mark
    public string? OznakaNaOdobrenieZaPriklucUred { get; set; }         // coupling homologation mark
    public string? IdentifikacijaNaMotorMestoMetod { get; set; }        // engine ID location/method

    // Trailer / coupling text fields
    public string? TBrOdobrenieMehanPriklucok { get; set; }
    public string? TMarkaMehanPriklucok { get; set; }
    public string? TTipMehanPriklucok { get; set; }
    public string? TZastitnaKabina { get; set; }
    public string? TZastitnaRamka { get; set; }

    public string? NoiseTechnicalSpec { get; set; }                     // nvarchar(100)
    public string? OdnosKwCcm { get; set; }                             // kW/cc ratio (legacy stored as text)

    // Engine extras
    public decimal? EnginePowerOutPut { get; set; }                     // separate from EnginePowerKw
    public decimal? NoiseStatic { get; set; }
    public decimal? CO2 { get; set; }
    public decimal? MaxSpeed { get; set; }

    // Mass / load (kg/t)
    public decimal? MaxKonstVkMasa { get; set; }                        // max construction total weight
    public decimal? MaxLegVkMasa { get; set; }                          // max legal total weight (separate from MaxAllowedWeight)
    public decimal? MaxLegVkMasaGrupa { get; set; }                     // max legal weight in group

    // Per-axle mass (kg). Legacy stored as int columns Mass1..5; we keep the same shape
    // so the print overlays bind by index without joining VehicleAxles.
    public int? MasaPoOska1 { get; set; }
    public int? MasaPoOska2 { get; set; }
    public int? MasaPoOska3 { get; set; }
    public int? MasaPoOska4 { get; set; }
    public int? MasaPoOska5 { get; set; }

    // Per-axle load (kg). Same shape as MasaPoOska#.
    public int? OsnoOptovaruvanje1 { get; set; }
    public int? OsnoOptovaruvanje2 { get; set; }
    public int? OsnoOptovaruvanje3 { get; set; }
    public int? OsnoOptovaruvanje4 { get; set; }
    public int? OsnoOptovaruvanje5 { get; set; }

    public int? MaxKonstOptovaruvanjeVoPriklucok { get; set; }          // max load at coupling
    public int? TMaxHorVerOptovaruvanjePriklucok { get; set; }          // max horizontal/vertical load at coupling
    public int? TMaxKonstVkMasaNaKombinacija { get; set; }              // max combination weight
    public int? TMaxKonstVkMasaPoluprikolka { get; set; }               // max semi-trailer weight
    public int? TMaxKonstVkMasaPrikolka { get; set; }                   // max trailer weight
    public int? TMaxKonstVkMasaPrikolkaSoCenOska { get; set; }          // max trailer-with-central-axle weight
    public int? TMaxKonstVkMasaPrikolkaStoMozePrikluci { get; set; }    // max trailer weight that can couple
    public int? TMinMasa { get; set; }                                  // min trailer mass
}
