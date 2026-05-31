namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Vehicle : AuditableEntity
{
    public string ShellNumber { get; set; } = string.Empty;
    public string? EngineNumber { get; set; }
    public DateTime? MakeDate { get; set; }
    public string? FirstRegistrationNumber { get; set; }
    public DateTime? FirstRegistrationDate { get; set; }
    public DateTime? FirstRegistrationValidUntil { get; set; }
    public long? FirstRegistrationIssuerId { get; set; }
    public RegistrationIssuer? FirstRegistrationIssuer { get; set; }
    public string? LastRegistrationNumber { get; set; }
    public DateTime? LastRegistrationDate { get; set; }
    public DateTime? LastRegistrationValidUntil { get; set; }
    public long? LastRegistrationIssuerId { get; set; }
    public RegistrationIssuer? LastRegistrationIssuer { get; set; }
    public long? VehicleModelId { get; set; }
    public VehicleModel? VehicleModel { get; set; }
    public long? BodyTypeId { get; set; }
    public VehicleBodyType? BodyType { get; set; }
    public long? CategoryId { get; set; }
    public VehicleCategory? Category { get; set; }
    public long? PaymentCategoryId { get; set; }
    public VehiclePaymentCategory? PaymentCategory { get; set; }
    public long? UseTypeId { get; set; }
    public VehicleUseType? UseType { get; set; }
    public long? EngineTypeId { get; set; }
    public EngineType? EngineType { get; set; }
    public long? PrimaryPowerSourceId { get; set; }
    public EnginePowerSourceType? PrimaryPowerSource { get; set; }
    public long? SecondaryPowerSourceId { get; set; }
    public EnginePowerSourceType? SecondaryPowerSource { get; set; }
    public long? GearBoxTypeId { get; set; }
    public GearBoxType? GearBoxType { get; set; }
    public long? BrakeTypeId { get; set; }
    public BrakeType? BrakeType { get; set; }
    public long? SupportingTypeId { get; set; }
    public SupportingType? SupportingType { get; set; }
    public long? EcoProgramId { get; set; }
    public EcoProgram? EcoProgram { get; set; }
    public long? MadeInCountryId { get; set; }
    public Country? MadeInCountry { get; set; }
    public long? PrimaryColorId { get; set; }
    public Color? PrimaryColor { get; set; }
    public long? SecondaryColorId { get; set; }
    public Color? SecondaryColor { get; set; }
    public string? ColorCode { get; set; }
    public decimal? EnginePowerKW { get; set; }
    public decimal? EngineTorqueNM { get; set; }
    public decimal? EngineWorkingCapacityCM3 { get; set; }
    public int? HeightMM { get; set; }
    public int? WidthMM { get; set; }
    public int? LengthMM { get; set; }
    public decimal? EmptyWeightKG { get; set; }
    public decimal? MaxAllowedWeightKG { get; set; }
    public decimal? TrailerWeightBrakedKG { get; set; }
    public decimal? TrailerWeightUnbrakedKG { get; set; }
    public int? NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }
    public int? NumberOfStandingSeats { get; set; }
    public int? NumberOfLyingSeats { get; set; }
    public int? NumberOfAxles { get; set; }
    public int? PropulsionAxle { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? NumberOfPropulsionWheels { get; set; }
    public decimal? CO { get; set; }
    public decimal? HC { get; set; }
    public decimal? NOx { get; set; }
    public decimal? HCNOx { get; set; }
    public decimal? CO2 { get; set; }
    public decimal? NoiseStaticDB { get; set; }
    public decimal? NoiseMovementDB { get; set; }
    public decimal? Blackening { get; set; }
    public decimal? Pinpoints { get; set; }
    public decimal? FuelConsumption { get; set; }
    public decimal? FuelTankCapacityL { get; set; }
    public bool HasLPG { get; set; }
    public decimal? MaxSpeedKMH { get; set; }
    public bool HasHook { get; set; }
    public bool IsSocialVehicle { get; set; }
    public bool IsPrivateTransport { get; set; }
    public List<VehicleAxle> Axles { get; set; } = [];
    public List<VehicleTyre> Tyres { get; set; } = [];
    public List<VehicleAxleDistance> AxleDistances { get; set; } = [];
}
