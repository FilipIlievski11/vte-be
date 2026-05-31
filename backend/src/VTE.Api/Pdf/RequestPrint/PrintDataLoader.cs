using System.Globalization;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Pdf.RequestPrint;

// Builds a PrintRequestData snapshot for a Request id by joining every reference
// table needed by the three legacy templates. Fields not yet present in the
// new schema return string.Empty — they will populate automatically as the
// vehicle schema is back-filled.
public sealed class PrintDataLoader
{
    private readonly VteDbContext _db;
    public PrintDataLoader(VteDbContext db) => _db = db;

    public async Task<PrintRequestData?> LoadAsync(long requestId, CancellationToken ct = default)
    {
        var req = await _db.Set<VTE.Domain.Requests.Request>().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId, ct);
        if (req is null) return null;

        var rt = await _db.Set<VTE.Domain.Requests.RequestType>().AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == req.RequestTypeId, ct);

        var curRel = await _db.Set<VTE.Domain.Vehicles.CustomerVehicleRelation>().AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == req.CustomerVehicleRelationId, ct);

        var newRel = req.NewCustomerVehicleRelationId.HasValue
            ? await _db.Set<VTE.Domain.Vehicles.CustomerVehicleRelation>().AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == req.NewCustomerVehicleRelationId.Value, ct)
            : null;

        var vehicleId = curRel?.VehicleId ?? 0;
        var vehicle = vehicleId > 0
            ? await _db.Set<VTE.Domain.Vehicles.Vehicle>().AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == vehicleId, ct)
            : null;

        var curCustomer = curRel != null
            ? await _db.Set<VTE.Domain.Customers.Customer>().AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == curRel.CustomerId, ct)
            : null;

        var newCustomer = newRel != null
            ? await _db.Set<VTE.Domain.Customers.Customer>().AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == newRel.CustomerId, ct)
            : curCustomer;

        var org = req.TechnicalExamOrganizationId.HasValue
            ? await _db.Set<TechnicalExamOrganization>().AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == req.TechnicalExamOrganizationId.Value, ct)
            : null;

        // Reference lookups for vehicle
        var bodyType   = vehicle?.BodyTypeId      .HasValue == true ? await _db.Set<VehicleBodyType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.BodyTypeId.Value, ct) : null;
        var modelEnt   = vehicle?.VehicleModelId  .HasValue == true ? await _db.Set<VehicleModel>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.VehicleModelId.Value, ct) : null;
        var maker      = modelEnt != null         ? await _db.Set<VehicleMaker>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == modelEnt.VehicleMakerId, ct) : null;
        var use        = vehicle?.VehicleUseId    .HasValue == true ? await _db.Set<VehicleUse>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.VehicleUseId.Value, ct) : null;
        var color1     = vehicle?.PrimaryColorId  .HasValue == true ? await _db.Set<Color>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.PrimaryColorId.Value, ct) : null;
        var color2     = vehicle?.SecondaryColorId.HasValue == true ? await _db.Set<Color>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.SecondaryColorId.Value, ct) : null;
        var powerSrc   = vehicle?.EnginePowerSourceId.HasValue == true ? await _db.Set<VehicleEnginePowerSourceType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.EnginePowerSourceId.Value, ct) : null;
        var engineType = vehicle?.EngineTypeId    .HasValue == true ? await _db.Set<VehicleEngineType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.EngineTypeId.Value, ct) : null;
        var firstIssuer= vehicle?.FirstRegistrationIssuerId.HasValue == true ? await _db.Set<RegistrationIssuer>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.FirstRegistrationIssuerId.Value, ct) : null;
        var lastIssuer = vehicle?.LastRegistrationIssuerId .HasValue == true ? await _db.Set<RegistrationIssuer>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == vehicle.LastRegistrationIssuerId .Value, ct) : null;

        var curLivingCity = curCustomer?.LivingCityId.HasValue == true ? await _db.Set<City>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == curCustomer.LivingCityId.Value, ct) : null;
        var newLivingCity = newCustomer?.LivingCityId.HasValue == true ? await _db.Set<City>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == newCustomer.LivingCityId.Value, ct) : null;
        var curLivingCommunity = curLivingCity?.CommunityId.HasValue == true ? await _db.Set<Community>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == curLivingCity.CommunityId.Value, ct) : null;
        var newLivingCommunity = newLivingCity?.CommunityId.HasValue == true ? await _db.Set<Community>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == newLivingCity.CommunityId.Value, ct) : null;

        // Target community + issuer for new registration: based on new owner's community
        var targetCommunity = newLivingCommunity ?? curLivingCommunity;
        // Issuer-by-community lookup: pick first active issuer (legacy uses GetRegistrationIssuerInfoByCommunity)
        var targetIssuer = lastIssuer; // closest available; backfill when issuer-by-community mapping exists

        return new PrintRequestData
        {
            RequestId = req.Id,
            RequestTypeId = req.RequestTypeId,
            RequestTypeName = rt?.TypeName ?? string.Empty,
            RequestTypeDisplayText = rt?.TypeDescription ?? rt?.TypeName ?? string.Empty,
            IsNewCustomer = rt?.IsNewCustomer ?? false,
            IsNewRegistration = rt?.IsNewRegistration ?? false,
            IsPreviosRegistrationReqired = rt?.IsPreviousRegistrationRequired ?? false,
            IsCustomerChanged = req.IsCustomerChanged,
            IsVehicleChanged = req.IsVehicleChanged,

            OrganizationName = org?.Name ?? string.Empty,
            Station = org?.Name ?? string.Empty,
            Note = req.Note ?? string.Empty,
            RegistrationNumberPrevios = vehicle?.LastRegistrationNumber ?? string.Empty,
            RegNumberDolg = vehicle?.LastRegistrationNumber ?? string.Empty,
            TargetCommunityRegCode = targetCommunity?.RegistrationCode ?? string.Empty,
            TargetIssuerName = targetIssuer?.Name ?? string.Empty,

            CurrentOwner = MakeCustomer(curCustomer, curLivingCommunity, curLivingCity),
            NewOwner = MakeCustomer(newCustomer, newLivingCommunity, newLivingCity),
            PrethodnaReg = MakeCustomer(curCustomer, curLivingCommunity, curLivingCity),

            CurrentVehicle = vehicle == null ? new PrintVehicle() : new PrintVehicle
            {
                Tip = string.IsNullOrEmpty(vehicle.Tip) ? (bodyType?.Code ?? string.Empty) : vehicle.Tip!,
                VehicleMaker = maker?.Name ?? string.Empty,
                VehicleModelAndModelAdding = string.IsNullOrEmpty(vehicle.VehicleModelAdding)
                    ? (modelEnt?.Name ?? string.Empty)
                    : $"{modelEnt?.Name} {vehicle.VehicleModelAdding}".Trim(),
                ShellNumber = vehicle.ShellNumber,
                EngineNumber = vehicle.EngineNumber ?? string.Empty,
                EngineTypeCode = engineType?.Name ?? string.Empty,
                BodytypeDescriprion = bodyType?.Name ?? string.Empty,
                ColorDescription = color1?.Name ?? string.Empty,
                ColorDescriptionSecondary = color2?.Name ?? string.Empty,
                UseDescription = use?.Name ?? string.Empty,
                PowerSourceName = powerSrc?.Name ?? string.Empty,

                VehicleSizeLength = Num(vehicle.VehicleLength),
                VehicleSizeWidth = Num(vehicle.VehicleWidth),
                VehicleSizeHight = Num(vehicle.VehicleHeight),
                EmptyWaight = Num(vehicle.EmptyWeight),

                EngineWorkingCapacity = Num(vehicle.EngineWorkingCapacity),
                BrojNaVrtezi = vehicle.RPM?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,

                NumberOfSeats = vehicle.NumberOfSeats?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                NumberOfStandingSeats = vehicle.NumberOfStandingSeats?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                NumberOfLieingSeats = vehicle.NumberOfLyingSeats?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                NumberOfAxis = vehicle.NumberOfAxes?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,
                NumberOfWheels = vehicle.NumberOfWheels?.ToString(CultureInfo.InvariantCulture) ?? string.Empty,

                MakeDate = DateText(vehicle.MakeDate),
                FirstRegistration = vehicle.FirstRegistrationNumber,
                FirstRegistrationDate = DateText(vehicle.FirstRegistrationMakeDate),
                FirstRegIssuer = firstIssuer?.Name ?? string.Empty,
                LastRegistration = vehicle.LastRegistrationNumber,
                LastRegistrationValidTill = DateText(vehicle.LastRegistrationValidTill),
                LastRegIssuer = lastIssuer?.Name ?? string.Empty,
                DateOfLastRegistrationa = DateText(vehicle.LastRegistrationMakeDate),

                Hook = vehicle.Hook,
                Vitlo = vehicle.Winch,

                IdVehicleCategoryForPayments = vehicle.VehicleCategoryForPaymentsId ?? 0,
                IdEnginePowerSource = vehicle.EnginePowerSourceId ?? 0,
                LastRegIdCommunity = 0,

                // Print-overlay (legacy parity) — directly from new Vehicle columns
                BrojEUPotvrda = vehicle.BrojEUPotvrda ?? string.Empty,
                OznakaNaOdobrenie = vehicle.OznakaNaOdobrenie ?? string.Empty,
                OznakaNaOdobrenieZaPriklucUred = vehicle.OznakaNaOdobrenieZaPriklucUred ?? string.Empty,
                IdentifikacijaNaMotorMestoMetod = vehicle.IdentifikacijaNaMotorMestoMetod ?? string.Empty,

                TBrOdobrenieMehanPriklucok = vehicle.TBrOdobrenieMehanPriklucok ?? string.Empty,
                TMarkaMehanPriklucok = vehicle.TMarkaMehanPriklucok ?? string.Empty,
                TTipMehanPriklucok = vehicle.TTipMehanPriklucok ?? string.Empty,
                TZastitnaKabina = vehicle.TZastitnaKabina ?? string.Empty,
                TZastitnaRamka = vehicle.TZastitnaRamka ?? string.Empty,

                NoiseTechnicalSpec = vehicle.NoiseTechnicalSpec ?? string.Empty,
                // OdnosKwCcm is stored on the vehicle if backfilled, otherwise compute from kw/cc.
                OdnosKwCcm = !string.IsNullOrEmpty(vehicle.OdnosKwCcm)
                    ? vehicle.OdnosKwCcm!
                    : (vehicle.EnginePowerKw.HasValue && vehicle.EngineWorkingCapacity is > 0
                        ? (vehicle.EnginePowerKw.Value / vehicle.EngineWorkingCapacity.Value).ToString("0.000", CultureInfo.InvariantCulture)
                        : string.Empty),

                EnginePowerOutPut = Num(vehicle.EnginePowerOutPut ?? vehicle.EnginePowerKw),
                NoiseStatic = Num(vehicle.NoiseStatic),
                CO2 = Num(vehicle.CO2),
                MaxSpeed = Num(vehicle.MaxSpeed),
                MaxKonstVkMasa = Num(vehicle.MaxKonstVkMasa),
                MaxLegVkMasa = Num(vehicle.MaxLegVkMasa ?? vehicle.MaxAllowedWeight),
                MaxLegVkMasaGrupa = Num(vehicle.MaxLegVkMasaGrupa),

                MasaPoOska1 = IntStr(vehicle.MasaPoOska1),
                MasaPoOska2 = IntStr(vehicle.MasaPoOska2),
                MasaPoOska3 = IntStr(vehicle.MasaPoOska3),
                MasaPoOska4 = IntStr(vehicle.MasaPoOska4),
                MasaPoOska5 = IntStr(vehicle.MasaPoOska5),
                OsnoOptovaruvanje1 = IntStr(vehicle.OsnoOptovaruvanje1),
                OsnoOptovaruvanje2 = IntStr(vehicle.OsnoOptovaruvanje2),
                OsnoOptovaruvanje3 = IntStr(vehicle.OsnoOptovaruvanje3),
                OsnoOptovaruvanje4 = IntStr(vehicle.OsnoOptovaruvanje4),
                OsnoOptovaruvanje5 = IntStr(vehicle.OsnoOptovaruvanje5),

                MaxKonstOptovaruvanjeVoPriklucok = IntStr(vehicle.MaxKonstOptovaruvanjeVoPriklucok),
                TMaxHorVerOptovaruvanjePriklucok = IntStr(vehicle.TMaxHorVerOptovaruvanjePriklucok),
                TMaxKonstVkMasaNaKombinacija = IntStr(vehicle.TMaxKonstVkMasaNaKombinacija),
                TMaxKonstVkMasaPoluprikolka = IntStr(vehicle.TMaxKonstVkMasaPoluprikolka),
                TMaxKonstVkMasaPrikolka = IntStr(vehicle.TMaxKonstVkMasaPrikolka),
                TMaxKonstVkMasaPrikolkaSoCenOska = IntStr(vehicle.TMaxKonstVkMasaPrikolkaSoCenOska),
                TMaxKonstVkMasaPrikolkaStoMozePrikluci = IntStr(vehicle.TMaxKonstVkMasaPrikolkaStoMozePrikluci),
                TMinMasa = IntStr(vehicle.TMinMasa),
            },

            EnginePowerSourceId = vehicle?.EnginePowerSourceId,
            ZelenCategoryMap = vehicle?.VehicleCategoryForPaymentsId,
            BelCategoryMap = vehicle?.VehicleCategoryForPaymentsId,
        };
    }

    private static PrintCustomer MakeCustomer(VTE.Domain.Customers.Customer? c, Community? comm, City? city)
    {
        if (c is null) return new PrintCustomer();
        var addr = string.IsNullOrEmpty(c.LivingAddressNumber)
            ? (city?.Name ?? string.Empty)
            : $"{city?.Name ?? string.Empty} {c.LivingAddressNumber}".Trim();
        return new PrintCustomer
        {
            IsCompany = c.IsCompany,
            CustomerFirstName = c.FirstName,
            CustomerSurname = c.Surname ?? string.Empty,
            MB = c.EMBG ?? string.Empty,
            LivingAddress = addr,
            CommunityNameLiving = comm?.Name ?? string.Empty,
            IdCommunityCode = comm?.Id ?? 0,
        };
    }

    private static string Num(decimal? v) => v.HasValue ? v.Value.ToString("0.##", CultureInfo.InvariantCulture) : string.Empty;
    private static string IntStr(int? v) => v.HasValue ? v.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
    private static string DateText(DateOnly? d) => d.HasValue ? d.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture) : string.Empty;
}
