namespace VTE.Api.Pdf.RequestPrint;

// Shape consumed by all three legacy templates. Field names exactly match legacy
// binding paths (CurrentVehicle.X / NewOwner.X / CurrentOwner.X / top-level X).
public sealed class PrintRequestData
{
    public long RequestId { get; init; }
    public int RequestTypeId { get; init; }
    public string RequestTypeName { get; init; } = string.Empty;
    public string RequestTypeDisplayText { get; init; } = string.Empty;
    public bool IsNewCustomer { get; init; }
    public bool IsNewRegistration { get; init; }
    public bool IsPreviosRegistrationReqired { get; init; }
    public bool IsCustomerChanged { get; init; }
    public bool IsVehicleChanged { get; init; }

    public string OrganizationName { get; init; } = string.Empty;
    public string Station { get; init; } = string.Empty;
    public string Note { get; init; } = string.Empty;
    public string RegNumberDolg { get; init; } = string.Empty;
    public string RegistrationNumberPrevios { get; init; } = string.Empty;

    public PrintCustomer NewOwner { get; init; } = new();
    public PrintCustomer CurrentOwner { get; init; } = new();
    public PrintCustomer PrethodnaReg { get; init; } = new();
    public PrintVehicle CurrentVehicle { get; init; } = new();

    public string TargetCommunityRegCode { get; init; } = string.Empty;
    public string TargetIssuerName { get; init; } = string.Empty;

    // Maps used by checkbox-overlay logic
    public int? ZelenCategoryMap { get; init; }
    public int? BelCategoryMap { get; init; }
    public int? EnginePowerSourceId { get; init; }
}

public sealed class PrintCustomer
{
    public bool IsCompany { get; init; }
    public string CustomerFirstName { get; init; } = string.Empty;
    public string CustomerSurname { get; init; } = string.Empty;
    public string MB { get; init; } = string.Empty;
    public string LivingAddress { get; init; } = string.Empty;
    public string CommunityNameLiving { get; init; } = string.Empty;
    public int IdCommunityCode { get; init; }
}

public sealed class PrintVehicle
{
    // Identity
    public string Tip { get; init; } = string.Empty;
    public string VehicleMaker { get; init; } = string.Empty;
    public string VehicleModelAndModelAdding { get; init; } = string.Empty;
    public string ShellNumber { get; init; } = string.Empty;
    public string EngineNumber { get; init; } = string.Empty;
    public string EngineTypeCode { get; init; } = string.Empty;
    public string BodytypeDescriprion { get; init; } = string.Empty;
    public string ColorDescription { get; init; } = string.Empty;
    public string ColorDescriptionSecondary { get; init; } = string.Empty;
    public string UseDescription { get; init; } = string.Empty;
    public string PowerSourceName { get; init; } = string.Empty;

    // Dimensions
    public string VehicleSizeLength { get; init; } = string.Empty;
    public string VehicleSizeWidth { get; init; } = string.Empty;
    public string VehicleSizeHight { get; init; } = string.Empty;
    public string EmptyWaight { get; init; } = string.Empty;

    // Engine
    public string EngineWorkingCapacity { get; init; } = string.Empty;
    public string EnginePowerOutPut { get; init; } = string.Empty;
    public string BrojNaVrtezi { get; init; } = string.Empty;
    public string OdnosKwCcm { get; init; } = string.Empty;
    public string MaxSpeed { get; init; } = string.Empty;
    public string BrojEUPotvrda { get; init; } = string.Empty;
    public string OznakaNaOdobrenie { get; init; } = string.Empty;
    public string IdentifikacijaNaMotorMestoMetod { get; init; } = string.Empty;

    // Emissions
    public string CO2 { get; init; } = string.Empty;
    public string NoiseStatic { get; init; } = string.Empty;
    public string NoiseTechnicalSpec { get; init; } = string.Empty;

    // Seats / wheels / axles
    public string NumberOfSeats { get; init; } = string.Empty;
    public string NumberOfStandingSeats { get; init; } = string.Empty;
    public string NumberOfLieingSeats { get; init; } = string.Empty;
    public string NumberOfWheels { get; init; } = string.Empty;
    public string NumberOfAxis { get; init; } = string.Empty;
    public string Tyre { get; init; } = string.Empty;

    // Mass
    public string MaxKonstVkMasa { get; init; } = string.Empty;
    public string MaxLegVkMasa { get; init; } = string.Empty;
    public string MaxLegVkMasaGrupa { get; init; } = string.Empty;
    public string MaxKonstOptovaruvanjeVoPriklucok { get; init; } = string.Empty;

    public string MasaPoOska1 { get; init; } = string.Empty;
    public string MasaPoOska2 { get; init; } = string.Empty;
    public string MasaPoOska3 { get; init; } = string.Empty;
    public string MasaPoOska4 { get; init; } = string.Empty;
    public string MasaPoOska5 { get; init; } = string.Empty;
    public string OsnoOptovaruvanje1 { get; init; } = string.Empty;
    public string OsnoOptovaruvanje2 { get; init; } = string.Empty;
    public string OsnoOptovaruvanje3 { get; init; } = string.Empty;
    public string OsnoOptovaruvanje4 { get; init; } = string.Empty;
    public string OsnoOptovaruvanje5 { get; init; } = string.Empty;

    // Registration history
    public string MakeDate { get; init; } = string.Empty;
    public string FirstRegistration { get; init; } = string.Empty;
    public string FirstRegistrationDate { get; init; } = string.Empty;
    public string FirstRegistrationPlace { get; init; } = string.Empty;
    public string FirstRegIssuer { get; init; } = string.Empty;
    public string LastRegistration { get; init; } = string.Empty;
    public string LastRegistrationValidTill { get; init; } = string.Empty;
    public string LastRegIssuer { get; init; } = string.Empty;
    public string DateOfLastRegistrationa { get; init; } = string.Empty;

    // Equipment
    public bool Hook { get; init; }
    public bool Vitlo { get; init; }
    public string TZastitnaKabina { get; init; } = string.Empty;
    public string TZastitnaRamka { get; init; } = string.Empty;

    // Trailer hookup details
    public string TMarkaMehanPriklucok { get; init; } = string.Empty;
    public string TMinMasa { get; init; } = string.Empty;
    public string TBrOdobrenieMehanPriklucok { get; init; } = string.Empty;
    public string TTipMehanPriklucok { get; init; } = string.Empty;
    public string TMaxHorVerOptovaruvanjePriklucok { get; init; } = string.Empty;
    public string TMaxKonstVkMasaNaKombinacija { get; init; } = string.Empty;
    public string TMaxKonstVkMasaPoluprikolka { get; init; } = string.Empty;
    public string TMaxKonstVkMasaPrikolka { get; init; } = string.Empty;
    public string TMaxKonstVkMasaPrikolkaSoCenOska { get; init; } = string.Empty;
    public string TMaxKonstVkMasaPrikolkaStoMozePrikluci { get; init; } = string.Empty;
    public string OznakaNaOdobrenieZaPriklucUred { get; init; } = string.Empty;

    public int IdVehicleCategoryForPayments { get; init; }
    public int IdEnginePowerSource { get; init; }
    public int LastRegIdCommunity { get; init; }
}
