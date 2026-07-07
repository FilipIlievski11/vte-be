using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Payments;
using VTE.Domain.Permissions;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Pricing;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Полномошна — Одобрение за управување со туѓо возило. The operator picks a vehicle
/// (by plate), the owner comes with it, then picks the authorized person; on save the
/// two documents print (барање + одобрение) and a flat fee is billed automatically to
/// the OWNER's vehicle relation (mirrors legacy insertFinancialStatePriceCatalogForPermisions).
///
/// Legacy source: <c>DocumentsPermisions</c> [sic] — screens WinApp/Documents/Permisions/*.
/// </summary>
[ApiController]
[Route("api/vehicle-permissions")]
[Authorize]
public class VehiclePermissionsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IDebtService _debts;

    public VehiclePermissionsController(VteDbContext db, ITenantContext tenant, IDebtService debts)
    {
        _db = db;
        _tenant = tenant;
        _debts = debts;
    }

    // ---- DTOs ----

    public record PermListItem(
        long Id, byte CompanyId,
        string? PermissionNumber, string TrafficLicenceNumber,
        string? PlateNumber, string? VehicleDisplay,
        string? OwnerName, string? AuthorizedName,
        DateTime IssuedDate, DateTime ValidTillDate, bool Active);

    public record PermFullDto(
        long Id, byte CompanyId,
        long ClientVehicleRelationId, long AuthorizedClientId,
        byte IssuerId, int IssuingCityId, int IssuerOrganizationId, string? IssuerOrganizationName,
        string? PermissionNumber, string TrafficLicenceNumber, string? TriptiqueNumber,
        DateTime IssuedDate, DateTime? StartDate, DateTime ValidTillDate,
        string? Note,
        string? OwnerName, string? OwnerIdNumber, string? OwnerAddress,
        string? AuthorizedName, string? AuthorizedEmbg, string? AuthorizedIdCardNumber,
        string? AuthorizedPassportNumber, string? AuthorizedAddress,
        string? VehicleDisplay, string? PlateNumber, string? VehicleVin, string? VehicleEngineNumber,
        bool Active, DateTime CreatedAt, DateTime? ModifiedAt, byte[] RowVersion);

    public record PermWriteDto(
        long ClientVehicleRelationId, long AuthorizedClientId,
        byte IssuerId, int IssuingCityId, int IssuerOrganizationId,
        string? PermissionNumber, string TrafficLicenceNumber, string? TriptiqueNumber,
        DateTime IssuedDate, DateTime? StartDate, DateTime ValidTillDate,
        string? Note,
        string? OwnerName, string? OwnerIdNumber, string? OwnerAddress,
        string? AuthorizedName, string? AuthorizedEmbg, string? AuthorizedIdCardNumber,
        string? AuthorizedPassportNumber, string? AuthorizedAddress,
        string? VehicleDisplay, string? PlateNumber, string? VehicleVin, string? VehicleEngineNumber,
        bool? Active = null, byte? CompanyId = null);

    /// <summary>Owner + vehicle defaults resolved from a picked client↔vehicle relation.</summary>
    public record PermVehicleDefaultsDto(
        long RelationId, long OwnerClientId, long? VehicleId,
        string? OwnerName, string? OwnerIdNumber, string? OwnerAddress,
        string? VehicleDisplay, string? PlateNumber, string? VehicleVin, string? VehicleEngineNumber);

    /// <summary>Authorized-person defaults from a picked client.</summary>
    public record PermAuthorizedDefaultsDto(
        long ClientId, string? Name, string? Embg, string? Address,
        string? IdCardNumber, string? PassportNumber);

    /// <summary>Flat bundle both print pages consume (легacy printDocumentPermisionByIdView).</summary>
    public record PermPrintDto(
        long Id, string? PermissionNumber, string TrafficLicenceNumber, string? TriptiqueNumber,
        DateTime IssuedDate, DateTime? StartDate, DateTime ValidTillDate, string? Note,
        string? OwnerName, string? OwnerIdNumber, string? OwnerAddress,
        string? AuthorizedName, string? AuthorizedEmbg, string? AuthorizedIdCardNumber,
        string? AuthorizedPassportNumber, string? AuthorizedAddress,
        string? VehicleDisplay, string? PlateNumber, string? VehicleVin, string? VehicleEngineNumber,
        string? IssuerName, string? IssuingCityName, string? IssuerOrgName, string? CompanyName);

    // ---- List ----

    [HttpGet]
    public async Task<ActionResult<PagedDto<PermListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? sort = null,
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.VehiclePermissions.AsNoTracking().AsQueryable();
        if (!includeInactive) query = query.Where(x => x.Active);

        // Snapshot columns make the search local to this table — no joins needed.
        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";
            query = query.Where(x =>
                (x.PermissionNumber != null && EF.Functions.Like(x.PermissionNumber, like)) ||
                EF.Functions.Like(x.TrafficLicenceNumber, like) ||
                (x.PlateNumber != null && EF.Functions.Like(x.PlateNumber, like)) ||
                (x.OwnerName != null && EF.Functions.Like(x.OwnerName, like)) ||
                (x.AuthorizedName != null && EF.Functions.Like(x.AuthorizedName, like)) ||
                (x.AuthorizedEmbg != null && EF.Functions.Like(x.AuthorizedEmbg, like)) ||
                (x.OwnerIdNumber != null && EF.Functions.Like(x.OwnerIdNumber, like)));
        }

        var desc = !string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
        query = (sort?.ToLowerInvariant()) switch
        {
            "number"  => desc ? query.OrderByDescending(x => x.PermissionNumber) : query.OrderBy(x => x.PermissionNumber),
            "issued"  => desc ? query.OrderByDescending(x => x.IssuedDate)       : query.OrderBy(x => x.IssuedDate),
            "expires" => desc ? query.OrderByDescending(x => x.ValidTillDate)    : query.OrderBy(x => x.ValidTillDate),
            "plate"   => desc ? query.OrderByDescending(x => x.PlateNumber)      : query.OrderBy(x => x.PlateNumber),
            _         => desc ? query.OrderByDescending(x => x.Id)               : query.OrderBy(x => x.Id),
        };

        var total = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new PermListItem(
                x.Id, x.CompanyId, x.PermissionNumber, x.TrafficLicenceNumber,
                x.PlateNumber, x.VehicleDisplay, x.OwnerName, x.AuthorizedName,
                x.IssuedDate, x.ValidTillDate, x.Active))
            .ToListAsync();

        return Ok(new PagedDto<PermListItem>(page, pageSize, total, items));
    }

    // ---- Defaults (form auto-fill) ----

    /// <summary>Resolve owner + vehicle snapshot defaults from a picked relation
    /// (the FE's vehicle search returns relation rows).</summary>
    [HttpGet("defaults")]
    public async Task<ActionResult<PermVehicleDefaultsDto>> VehicleDefaults([FromQuery] long relationId)
    {
        var rel = await _db.ClientVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == relationId);
        if (rel is null) return NotFound();

        var c = await _db.Clients.AsNoTracking().Where(x => x.Id == rel.ClientId)
            .Select(x => new { x.Id, x.FirstName, x.LastName, x.MB, x.Address, x.CityId })
            .FirstOrDefaultAsync();
        if (c is null) return NotFound();

        string? cityName = c.CityId.HasValue
            ? await _db.Cities.AsNoTracking().Where(ci => ci.Id == c.CityId.Value).Select(ci => ci.Name).FirstOrDefaultAsync()
            : null;
        var ownerName = $"{c.FirstName} {c.LastName}".Trim();
        var ownerAddress = string.Join(' ', new[] { c.Address, cityName }.Where(s => !string.IsNullOrWhiteSpace(s)));

        string? display = null, vin = null, engine = null, plate = null;
        if (rel.VehicleId.HasValue)
        {
            var v = await (from ve in _db.Vehicles.AsNoTracking()
                           join m in _db.VehicleModels.AsNoTracking() on ve.ModelId equals m.Id into mj
                           from m in mj.DefaultIfEmpty()
                           join mk in _db.VehicleMakers.AsNoTracking() on m.MakerId equals mk.Id into mkj
                           from mk in mkj.DefaultIfEmpty()
                           where ve.Id == rel.VehicleId.Value
                           select new { ve.Vin, ve.EngineNumber, ve.Plate, ModelName = (string?)m.Name, MakerName = (string?)mk.Name })
                          .FirstOrDefaultAsync();
            if (v != null)
            {
                display = string.Join(' ', new[] { v.MakerName, v.ModelName }.Where(s => !string.IsNullOrWhiteSpace(s)));
                vin = v.Vin; engine = v.EngineNumber; plate = v.Plate;
            }
        }

        return Ok(new PermVehicleDefaultsDto(
            rel.Id, c.Id, rel.VehicleId, ownerName, c.MB, ownerAddress, display, plate, vin, engine));
    }

    /// <summary>Authorized-person snapshot defaults from a picked client.</summary>
    [HttpGet("authorized-defaults")]
    public async Task<ActionResult<PermAuthorizedDefaultsDto>> AuthorizedDefaults([FromQuery] long clientId)
    {
        var c = await _db.Clients.AsNoTracking().Where(x => x.Id == clientId)
            .Select(x => new { x.Id, x.FirstName, x.LastName, x.MB, x.Address, x.CityId })
            .FirstOrDefaultAsync();
        if (c is null) return NotFound();

        string? cityName = c.CityId.HasValue
            ? await _db.Cities.AsNoTracking().Where(ci => ci.Id == c.CityId.Value).Select(ci => ci.Name).FirstOrDefaultAsync()
            : null;

        var docs = await _db.ClientPersonalData.AsNoTracking()
            .Where(pd => pd.ClientId == clientId && pd.Active)
            .Select(pd => new { pd.PersonalDataTypeId, pd.Number })
            .ToListAsync();

        return Ok(new PermAuthorizedDefaultsDto(
            c.Id,
            $"{c.FirstName} {c.LastName}".Trim(),
            c.MB,
            string.Join(' ', new[] { c.Address, cityName }.Where(s => !string.IsNullOrWhiteSpace(s))),
            docs.FirstOrDefault(d => d.PersonalDataTypeId == 3)?.Number,
            docs.FirstOrDefault(d => d.PersonalDataTypeId == 2)?.Number));
    }

    // ---- Get ----

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PermFullDto>> Get(long id)
    {
        var x = await _db.VehiclePermissions.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (x == null) return NotFound();
        return Ok(await ToFullDtoAsync(x));
    }

    // ---- Create ----

    [HttpPost]
    public async Task<ActionResult<PermFullDto>> Create([FromBody] PermWriteDto dto)
    {
        var err = await ValidateAsync(dto, existingId: null);
        if (err != null) return BadRequest(new { error = err });

        var entity = new VehiclePermission
        {
            CompanyId = ResolveCompanyId(dto.CompanyId),
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };
        Apply(entity, dto);
        _db.VehiclePermissions.Add(entity);
        await _db.SaveChangesAsync();

        // Auto-bill (best-effort): flat "Одобрение за туѓо возило" fee anchored to the
        // OWNER's relation. A billing hiccup must not lose the saved document.
        try
        {
            var ownerClientId = await _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.Id == entity.ClientVehicleRelationId).Select(r => r.ClientId).FirstAsync();
            var ownerCityId = await _db.Clients.AsNoTracking()
                .Where(c => c.Id == ownerClientId).Select(c => c.CityId).FirstOrDefaultAsync();
            int? communityId = ownerCityId.HasValue
                ? await _db.Cities.AsNoTracking().Where(ci => ci.Id == ownerCityId.Value).Select(ci => (int?)ci.CommunityId).FirstOrDefaultAsync()
                : null;
            await _debts.CreateDebtsForSourceAsync(
                origin: DebtOrigin.Permission,
                originId: entity.Id,
                customerVehicleRelationId: entity.ClientVehicleRelationId,
                organizationId: entity.IssuerOrganizationId,
                trigger: PriceTrigger.Permission,
                communityId: communityId,
                note: $"одобрение за туѓо возило бр. {(string.IsNullOrWhiteSpace(entity.PermissionNumber) ? entity.Id.ToString() : entity.PermissionNumber)}");
        }
        catch { /* best-effort, see remark above */ }

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, await ToFullDtoAsync(entity));
    }

    // ---- Update ----

    [HttpPut("{id:long}")]
    public async Task<ActionResult<PermFullDto>> Update(long id, [FromBody] PermWriteDto dto)
    {
        var entity = await _db.VehiclePermissions.FirstOrDefaultAsync(r => r.Id == id);
        if (entity == null) return NotFound();

        var err = await ValidateAsync(dto, existingId: id);
        if (err != null) return BadRequest(new { error = err });

        Apply(entity, dto);
        if (dto.Active.HasValue) entity.Active = dto.Active.Value;
        entity.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return Ok(await ToFullDtoAsync(entity));
    }

    // ---- Soft delete ----

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _db.VehiclePermissions.FirstOrDefaultAsync(r => r.Id == id);
        if (entity == null) return NotFound();
        entity.Active = false;
        entity.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---- Print bundle (both documents read the same flat DTO) ----

    [HttpGet("{id:long}/print")]
    public async Task<ActionResult<PermPrintDto>> Print(long id)
    {
        var x = await _db.VehiclePermissions.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (x == null) return NotFound();

        var issuerName = await _db.DocumentIssuers.AsNoTracking()
            .Where(i => i.Id == x.IssuerId).Select(i => i.Name).FirstOrDefaultAsync();
        var cityName = await _db.Cities.AsNoTracking()
            .Where(c => c.Id == x.IssuingCityId).Select(c => c.Name).FirstOrDefaultAsync();
        var orgName = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == x.IssuerOrganizationId).Select(o => o.Name).FirstOrDefaultAsync();
        var companyName = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == x.CompanyId).Select(c => c.Name).FirstOrDefaultAsync();

        return Ok(new PermPrintDto(
            x.Id, x.PermissionNumber, x.TrafficLicenceNumber, x.TriptiqueNumber,
            x.IssuedDate, x.StartDate, x.ValidTillDate, x.Note,
            x.OwnerName, x.OwnerIdNumber, x.OwnerAddress,
            x.AuthorizedName, x.AuthorizedEmbg, x.AuthorizedIdCardNumber,
            x.AuthorizedPassportNumber, x.AuthorizedAddress,
            x.VehicleDisplay, x.PlateNumber, x.VehicleVin, x.VehicleEngineNumber,
            issuerName, cityName, orgName, companyName?.Trim()));
    }

    // ---- helpers ----

    private byte ResolveCompanyId(byte? requested)
    {
        if (_tenant.IsAdmin && requested.HasValue) return requested.Value;
        return _tenant.CompanyId ?? requested ?? 4;
    }

    private void Apply(VehiclePermission e, PermWriteDto dto)
    {
        e.ClientVehicleRelationId = dto.ClientVehicleRelationId;
        e.AuthorizedClientId = dto.AuthorizedClientId;
        e.IssuerId = dto.IssuerId;
        e.IssuingCityId = dto.IssuingCityId;
        e.IssuerOrganizationId = dto.IssuerOrganizationId;
        e.PermissionNumber = Trimmed(dto.PermissionNumber);
        e.TrafficLicenceNumber = dto.TrafficLicenceNumber.Trim();
        e.TriptiqueNumber = Trimmed(dto.TriptiqueNumber);
        e.IssuedDate = dto.IssuedDate.Date;
        e.StartDate = dto.StartDate?.Date;
        e.ValidTillDate = dto.ValidTillDate.Date;
        e.Note = Trimmed(dto.Note);
        e.OwnerName = Trimmed(dto.OwnerName);
        e.OwnerIdNumber = Trimmed(dto.OwnerIdNumber);
        e.OwnerAddress = Trimmed(dto.OwnerAddress);
        e.AuthorizedName = Trimmed(dto.AuthorizedName);
        e.AuthorizedEmbg = Trimmed(dto.AuthorizedEmbg);
        e.AuthorizedIdCardNumber = Trimmed(dto.AuthorizedIdCardNumber);
        e.AuthorizedPassportNumber = Trimmed(dto.AuthorizedPassportNumber);
        e.AuthorizedAddress = Trimmed(dto.AuthorizedAddress);
        e.VehicleDisplay = Trimmed(dto.VehicleDisplay);
        e.PlateNumber = Trimmed(dto.PlateNumber);
        e.VehicleVin = Trimmed(dto.VehicleVin);
        e.VehicleEngineNumber = Trimmed(dto.VehicleEngineNumber);
    }

    private static string? Trimmed(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    /// <summary>Mirrors the legacy DocumentsPermision rules (lines 253-327).</summary>
    private async Task<string?> ValidateAsync(PermWriteDto dto, long? existingId)
    {
        if (string.IsNullOrWhiteSpace(dto.TrafficLicenceNumber))
            return "Бр. на сообраќајна дозвола е задолжителен.";
        if (dto.IssuerId < 1) return "Издавачот е задолжителен.";
        if (dto.IssuingCityId < 1) return "Градот на издавање е задолжителен.";
        if (dto.ValidTillDate.Date <= dto.IssuedDate.Date)
            return "Датумот на важност мора да биде поголем од датумот на издавање.";

        var rel = await _db.ClientVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == dto.ClientVehicleRelationId);
        if (rel is null) return "Возилото/сопственикот не е избран.";

        var authorizedExists = await _db.Clients.AsNoTracking().AnyAsync(c => c.Id == dto.AuthorizedClientId);
        if (!authorizedExists) return "Овластеното лице не е избрано.";

        // Legacy IdRelation rule: сопственикот не може самиот себе да се овласти.
        if (rel.ClientId == dto.AuthorizedClientId)
            return "Сопственикот не може да биде овластено лице.";

        // Legacy PremisionUnique rule: no duplicate ACTIVE permission for the same pair.
        var dup = await _db.VehiclePermissions.AsNoTracking().AnyAsync(p =>
            p.Active
            && p.AuthorizedClientId == dto.AuthorizedClientId
            && p.ClientVehicleRelationId == dto.ClientVehicleRelationId
            && (existingId == null || p.Id != existingId.Value));
        if (dup) return "Веќе постои активно одобрение за истото лице и возило.";

        var orgExists = await _db.TechnicalExamOrganizations.AsNoTracking().AnyAsync(o => o.Id == dto.IssuerOrganizationId);
        if (!orgExists) return "Издавачката организација не постои.";

        return null;
    }

    private async Task<PermFullDto> ToFullDtoAsync(VehiclePermission x)
    {
        var orgName = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == x.IssuerOrganizationId).Select(o => o.Name).FirstOrDefaultAsync();
        return new PermFullDto(
            x.Id, x.CompanyId,
            x.ClientVehicleRelationId, x.AuthorizedClientId,
            x.IssuerId, x.IssuingCityId, x.IssuerOrganizationId, orgName,
            x.PermissionNumber, x.TrafficLicenceNumber, x.TriptiqueNumber,
            x.IssuedDate, x.StartDate, x.ValidTillDate,
            x.Note,
            x.OwnerName, x.OwnerIdNumber, x.OwnerAddress,
            x.AuthorizedName, x.AuthorizedEmbg, x.AuthorizedIdCardNumber,
            x.AuthorizedPassportNumber, x.AuthorizedAddress,
            x.VehicleDisplay, x.PlateNumber, x.VehicleVin, x.VehicleEngineNumber,
            x.Active, x.CreatedAt, x.ModifiedAt, x.RowVersion);
    }
}
