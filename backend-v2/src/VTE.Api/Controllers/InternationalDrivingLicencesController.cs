using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.InternationalDrivingLicences;
using VTE.Domain.Payments;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Pricing;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// Меѓународни возачки дозволи (International Driving Licences). A client applies for one;
/// on save the operator prints two documents (request form + the issued booklet/permit) and
/// a flat fee is billed automatically. Person-level — no vehicle involved.
///
/// Legacy source: <c>DocumentsInternationalDriveingLicences</c> (+ ValidForCategories child).
/// </summary>
[ApiController]
[Route("api/international-driving-licences")]
[Authorize]
public class InternationalDrivingLicencesController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IDebtService _debts;

    public InternationalDrivingLicencesController(VteDbContext db, ITenantContext tenant, IDebtService debts)
    {
        _db = db;
        _tenant = tenant;
        _debts = debts;
    }

    // ---- DTOs ----

    public record IdlListItem(
        long Id, byte CompanyId,
        long ClientId, string? ClientName, string? ClientMb,
        string NumberOfLicence, string NumberOfNationalLicence,
        DateTime IssuedDate, DateTime ValidTillDate,
        IReadOnlyList<string> CategoryCodes, bool Active);

    public record IdlCategoryOption(int Id, string Code, string? Description);

    /// <summary>Editable applicant snapshot — auto-filled from the picked client but stored
    /// on the licence (never written back to the client). Used verbatim for the prints.</summary>
    public record IdlApplicantDto(
        string? FirstName, string? LastName, string? ParentName, string? Citizenship,
        DateTime? DateOfBirth, string? BirthPlace, string? Address,
        string? PassportNumber, string? PassportIssuer, DateTime? PassportDate, DateTime? PassportExpiry,
        string? IdCardNumber, string? IdCardIssuer, DateTime? IdCardDate, DateTime? IdCardExpiry,
        string? NationalLicenceIssuer, DateTime? NationalLicenceDate, DateTime? NationalLicenceExpiry);

    public record IdlFullDto(
        long Id, byte CompanyId,
        long ClientId, string? ClientName, string? ClientMb,
        int IssuerOrganizationId, string? IssuerOrganizationName,
        string NumberOfLicence, string NumberOfNationalLicence,
        DateTime IssuedDate, DateTime ValidTillDate,
        string? Note, bool Active, DateTime CreatedAt, DateTime? ModifiedAt,
        IReadOnlyList<int> CategoryIds, IdlApplicantDto Applicant, byte[] RowVersion);

    public record IdlWriteDto(
        long ClientId, int IssuerOrganizationId,
        string NumberOfLicence, string NumberOfNationalLicence,
        DateTime IssuedDate, DateTime ValidTillDate,
        string? Note, IReadOnlyList<int> CategoryIds,
        IdlApplicantDto? Applicant = null,
        bool? Active = null, byte? CompanyId = null);

    // ---- List (register) ----

    [HttpGet]
    public async Task<ActionResult<PagedDto<IdlListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? sort = null,
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.InternationalDrivingLicences.AsNoTracking().AsQueryable();
        if (!includeInactive) query = query.Where(x => x.Active);

        // Search: licence numbers (own table) UNION client name parts/MB (separate small
        // IN-subqueries, unioned — not one big OR across a join, for a clean index seek).
        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";

            var hitByNumber = _db.InternationalDrivingLicences.AsNoTracking()
                .Where(x => EF.Functions.Like(x.NumberOfLicence, like) || EF.Functions.Like(x.NumberOfNationalLicence, like))
                .Select(x => x.Id);

            var matchingClientIds = _db.Clients.AsNoTracking()
                .Where(c =>
                    (c.FirstName  != null && EF.Functions.Like(c.FirstName,  like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName   != null && EF.Functions.Like(c.LastName,   like)) ||
                    (c.MB         != null && EF.Functions.Like(c.MB,         like)))
                .Select(c => c.Id);
            var hitByClient = _db.InternationalDrivingLicences.AsNoTracking()
                .Where(x => matchingClientIds.Contains(x.ClientId))
                .Select(x => x.Id);

            var hitIds = hitByNumber.Union(hitByClient);
            query = query.Where(x => hitIds.Contains(x.Id));
        }

        var total = await query.CountAsync();
        var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
        query = (sort?.ToLowerInvariant()) switch
        {
            "number"  => asc ? query.OrderBy(x => x.NumberOfLicence)  : query.OrderByDescending(x => x.NumberOfLicence),
            "issued"  => asc ? query.OrderBy(x => x.IssuedDate)       : query.OrderByDescending(x => x.IssuedDate),
            "expires" => asc ? query.OrderBy(x => x.ValidTillDate)    : query.OrderByDescending(x => x.ValidTillDate),
            _         => asc ? query.OrderBy(x => x.Id)               : query.OrderByDescending(x => x.Id),
        };

        var page1 = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new
            {
                x.Id, x.CompanyId, x.ClientId, x.NumberOfLicence, x.NumberOfNationalLicence,
                x.IssuedDate, x.ValidTillDate, x.Active,
            }).ToListAsync();

        var ids = page1.Select(x => x.Id).ToList();
        var clientIds = page1.Select(x => x.ClientId).Distinct().ToList();

        var clients = clientIds.Count == 0
            ? new Dictionary<long, (string? Name, string? Mb)>()
            : await _db.Clients.AsNoTracking()
                .Where(c => clientIds.Contains(c.Id))
                .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName, c.MB })
                .ToDictionaryAsync(
                    c => c.Id,
                    c => (
                        Name: string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }
                            .Where(p => !string.IsNullOrWhiteSpace(p))) is var n && n.Length > 0 ? n : null,
                        Mb: c.MB));

        var categoryRows = ids.Count == 0
            ? []
            : await (from cat in _db.InternationalDrivingLicenceCategories.AsNoTracking()
                     join dl in _db.DrivingLicenceCategories.AsNoTracking() on cat.DrivingLicenceCategoryId equals dl.Id
                     where ids.Contains(cat.InternationalDrivingLicenceId)
                     select new { cat.InternationalDrivingLicenceId, dl.Code }).ToListAsync();
        var categoriesByIdl = categoryRows.GroupBy(x => x.InternationalDrivingLicenceId)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<string>)g.Select(x => x.Code).OrderBy(c => c).ToList());

        var items = page1.Select(x =>
        {
            var (name, mb) = clients.TryGetValue(x.ClientId, out var c) ? c : (null, null);
            return new IdlListItem(
                x.Id, x.CompanyId, x.ClientId, name, mb,
                x.NumberOfLicence, x.NumberOfNationalLicence, x.IssuedDate, x.ValidTillDate,
                categoriesByIdl.GetValueOrDefault(x.Id, Array.Empty<string>()), x.Active);
        }).ToList();

        return Ok(new PagedDto<IdlListItem>(page, pageSize, total, items));
    }

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<IdlCategoryOption>>> Categories() =>
        Ok(await _db.DrivingLicenceCategories.AsNoTracking().Where(c => c.Active)
            .OrderBy(c => c.Id)
            .Select(c => new IdlCategoryOption(c.Id, c.Code, c.Description)).ToListAsync());

    /// <summary>Resolve the auto-fill applicant snapshot for a client (name, citizenship name,
    /// DOB, address, and the 3 personal-data documents with issuer names + dates). Read-only —
    /// mutates nothing. The FE seeds the editable Сопственик fields from this on client pick;
    /// the operator can then edit freely before saving the snapshot onto the licence.</summary>
    [HttpGet("applicant-defaults/{clientId:long}")]
    public async Task<ActionResult<IdlApplicantDto>> ApplicantDefaults(long clientId)
    {
        var c = await _db.Clients.AsNoTracking().Where(x => x.Id == clientId)
            .Select(x => new { x.FirstName, x.LastName, x.CitizenshipId, x.DateOfBirth, x.Address })
            .FirstOrDefaultAsync();
        if (c == null) return NotFound();

        string? citizenship = null;
        if (c.CitizenshipId.HasValue)
            citizenship = await _db.Citizenships.AsNoTracking()
                .Where(z => z.Id == c.CitizenshipId.Value).Select(z => z.Name).FirstOrDefaultAsync();
        // Legacy never stored per-client citizenship (Customers.IdCitizenship = 0 for all customers),
        // so every migrated Client has CitizenshipId = NULL. The station is in Macedonia and virtually
        // every applicant is a Macedonian citizen — default it so the printed form isn't blank. It
        // stays fully editable in the Сопственик section for the rare foreign applicant.
        if (string.IsNullOrWhiteSpace(citizenship)) citizenship = "Македонско";

        var docs = await (from pd in _db.ClientPersonalData.AsNoTracking()
                          join iss in _db.DocumentIssuers.AsNoTracking() on pd.DocumentIssuerId equals iss.Id into j
                          from iss in j.DefaultIfEmpty()
                          where pd.ClientId == clientId && pd.Active
                          select new { pd.PersonalDataTypeId, pd.Number, IssuerName = iss.Name, pd.CreatedAt, pd.ExpiresAt }).ToListAsync();
        var passport = docs.FirstOrDefault(d => d.PersonalDataTypeId == 2);
        var idCard = docs.FirstOrDefault(d => d.PersonalDataTypeId == 3);
        var licence = docs.FirstOrDefault(d => d.PersonalDataTypeId == 1);

        // ParentName + BirthPlace aren't stored on the v2 Client, so they auto-fill blank —
        // the operator types them in (they remain fully editable for the print).
        return Ok(new IdlApplicantDto(
            c.FirstName, c.LastName, null, citizenship,
            c.DateOfBirth, null, c.Address,
            passport?.Number, passport?.IssuerName, passport?.CreatedAt, passport?.ExpiresAt,
            idCard?.Number, idCard?.IssuerName, idCard?.CreatedAt, idCard?.ExpiresAt,
            licence?.IssuerName, licence?.CreatedAt, licence?.ExpiresAt));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<IdlFullDto>> Get(long id)
    {
        var x = await _db.InternationalDrivingLicences.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (x == null) return NotFound();
        return Ok(await ToFullDtoAsync(x));
    }

    // ---- Create / Update / Delete ----

    [HttpPost]
    public async Task<ActionResult<IdlFullDto>> Create([FromBody] IdlWriteDto dto)
    {
        var (error, companyId) = await ResolveCompanyIdAsync(dto.CompanyId);
        if (error != null) return BadRequest(new { error });

        var validationError = await ValidateAsync(dto, currentId: null);
        if (validationError != null) return BadRequest(new { error = validationError });

        var entity = new InternationalDrivingLicence
        {
            CompanyId = companyId,
            ClientId = dto.ClientId,
            IssuerOrganizationId = dto.IssuerOrganizationId,
            NumberOfLicence = dto.NumberOfLicence.Trim(),
            NumberOfNationalLicence = dto.NumberOfNationalLicence.Trim(),
            IssuedDate = dto.IssuedDate.Date,
            ValidTillDate = dto.ValidTillDate.Date,
            Note = dto.Note,
            Active = dto.Active ?? true,
            CreatedAt = DateTime.UtcNow,
        };
        ApplyApplicant(entity, dto.Applicant);
        _db.InternationalDrivingLicences.Add(entity);
        await _db.SaveChangesAsync();

        await ReplaceCategoriesAsync(entity.Id, dto.CategoryIds);

        // Auto-bill: person-level debt, no vehicle. Best-effort — a pricing/relation hiccup
        // should never roll back the licence (mirrors Request/TechnicalExam's debt hooks).
        try
        {
            var personalRelationId = await GetOrCreatePersonalRelationAsync(dto.ClientId);
            var clientCityId = await _db.Clients.AsNoTracking()
                .Where(c => c.Id == dto.ClientId).Select(c => c.CityId).FirstOrDefaultAsync();
            int? communityId = clientCityId.HasValue
                ? await _db.Cities.AsNoTracking().Where(ci => ci.Id == clientCityId.Value).Select(ci => (int?)ci.CommunityId).FirstOrDefaultAsync()
                : null;
            await _debts.CreateDebtsForSourceAsync(
                origin: DebtOrigin.InternationalDrivingLicence,
                originId: entity.Id,
                customerVehicleRelationId: personalRelationId,
                organizationId: entity.IssuerOrganizationId,
                trigger: PriceTrigger.InternationalDrivingLicence,
                communityId: communityId,
                note: $"меѓународна возачка дозвола бр. {entity.NumberOfLicence}");
        }
        catch { /* best-effort, see remarks above */ }

        return CreatedAtAction(nameof(Get), new { id = entity.Id }, await ToFullDtoAsync(entity));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] IdlWriteDto dto)
    {
        var entity = await _db.InternationalDrivingLicences.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return NotFound();

        var validationError = await ValidateAsync(dto, currentId: id);
        if (validationError != null) return BadRequest(new { error = validationError });

        entity.ClientId = dto.ClientId;
        entity.IssuerOrganizationId = dto.IssuerOrganizationId;
        entity.NumberOfLicence = dto.NumberOfLicence.Trim();
        entity.NumberOfNationalLicence = dto.NumberOfNationalLicence.Trim();
        entity.IssuedDate = dto.IssuedDate.Date;
        entity.ValidTillDate = dto.ValidTillDate.Date;
        entity.Note = dto.Note;
        entity.Active = dto.Active ?? entity.Active;
        ApplyApplicant(entity, dto.Applicant);
        entity.ModifiedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        await ReplaceCategoriesAsync(id, dto.CategoryIds);
        // No debt re-fire on Update — matches Request's "Insert-only" debt-hook convention
        // (legacy DataPortal_Insert); the idempotency check in DebtService would no-op a
        // re-fire anyway, but Update simply never attempts one.
        return NoContent();
    }

    /// <summary>Soft-delete (Active=false).</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _db.InternationalDrivingLicences.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---- Print bundles ----

    public record IdlRequestPrintDto(
        long Id, string? ClientFullName, DateTime? DateOfBirth, string? BirthCityName, string? CitizenshipName,
        string NumberOfNationalLicence, string NumberOfLicence,
        string? PassportNumber, string? PassportIssuerName, DateTime? PassportDate,
        string? IdCardNumber, string? IdCardIssuerName, DateTime? IdCardDate,
        string? NationalLicenceIssuerName, DateTime? NationalLicenceDate, DateTime? NationalLicenceExpiry,
        string? LivingAddress, string? LivingCityName, string? Embg,
        string? IssuerOrgName, string? CompanyName, string? StationCityName, DateTime IssuedDate);

    /// <summary>A4 application form (legacy rptInternationalDrivLicenceRequest). Renders the
    /// editable applicant snapshot stored on the licence — NOT live client data.</summary>
    [HttpGet("{id:long}/print")]
    public async Task<ActionResult<IdlRequestPrintDto>> Print(long id)
    {
        var x = await _db.InternationalDrivingLicences.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (x == null) return NotFound();

        var fullName = string.Join(' ', new[] { x.ApplicantFirstName, x.ApplicantLastName }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

        // Issuing org (name for the header) + its city (place of submission), fixed at print time.
        var org = await (from o in _db.TechnicalExamOrganizations.AsNoTracking()
                         join ci in _db.Cities.AsNoTracking() on o.CityId equals ci.Id into ciJ
                         from ci in ciJ.DefaultIfEmpty()
                         where o.Id == x.IssuerOrganizationId
                         select new { o.Name, CityName = (string?)ci.Name }).FirstOrDefaultAsync();

        // EMBG + living city come from the client (identity data, not part of the editable snapshot).
        var client = await (from c in _db.Clients.AsNoTracking()
                            join ci in _db.Cities.AsNoTracking() on c.CityId equals ci.Id into ciJ
                            from ci in ciJ.DefaultIfEmpty()
                            where c.Id == x.ClientId
                            select new { c.MB, LivingCity = (string?)ci.Name }).FirstOrDefaultAsync();

        // Header "назив и седиште" = the legal entity (company) name, e.g. "АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ Велес".
        var companyName = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == x.CompanyId).Select(c => c.Name).FirstOrDefaultAsync();

        return Ok(new IdlRequestPrintDto(
            x.Id, fullName.Length > 0 ? fullName : null, x.ApplicantDateOfBirth, x.ApplicantBirthPlace, x.ApplicantCitizenship,
            x.NumberOfNationalLicence, x.NumberOfLicence,
            x.ApplicantPassportNumber, x.ApplicantPassportIssuer, x.ApplicantPassportDate,
            x.ApplicantIdCardNumber, x.ApplicantIdCardIssuer, x.ApplicantIdCardDate,
            x.ApplicantNationalLicenceIssuer, x.ApplicantNationalLicenceDate, x.ApplicantNationalLicenceExpiry,
            x.ApplicantAddress, client?.LivingCity, client?.MB,
            org?.Name, companyName, org?.CityName, x.IssuedDate));
    }

    public record IdlPermitPrintDto(
        long Id, DateTime IssuedDate, DateTime ValidTillDate,
        string? IssuingCityName, string? IssuingOrgName, string? CompanyName,
        string NumberOfLicence, string NumberOfNationalLicence,
        string? ClientSurname, string? ClientFirstName, string? BirthCityName, DateTime? DateOfBirth,
        string? LivingCityName, string? Note);

    /// <summary>Booklet/strip insert (legacy rptInternationalDriveingLicence). Renders the
    /// editable applicant snapshot stored on the licence — NOT live client data.</summary>
    [HttpGet("{id:long}/permit")]
    public async Task<ActionResult<IdlPermitPrintDto>> Permit(long id)
    {
        var x = await _db.InternationalDrivingLicences.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        if (x == null) return NotFound();

        var org = await (from o in _db.TechnicalExamOrganizations.AsNoTracking()
                         join ci in _db.Cities.AsNoTracking() on o.CityId equals ci.Id into ciJ
                         from ci in ciJ.DefaultIfEmpty()
                         where o.Id == x.IssuerOrganizationId
                         select new { o.Name, CityName = (string?)ci.Name }).FirstOrDefaultAsync();

        // Booklet prints the legal-entity (company) name — same as the request form header.
        var companyName = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == x.CompanyId).Select(c => c.Name).FirstOrDefaultAsync();

        // Living CITY (not the street address) — the booklet's 5th identity line.
        var livingCity = await (from c in _db.Clients.AsNoTracking()
                                join ci in _db.Cities.AsNoTracking() on c.CityId equals ci.Id
                                where c.Id == x.ClientId
                                select ci.Name).FirstOrDefaultAsync();

        return Ok(new IdlPermitPrintDto(
            x.Id, x.IssuedDate, x.ValidTillDate,
            org?.CityName, org?.Name, companyName,
            x.NumberOfLicence, x.NumberOfNationalLicence,
            x.ApplicantLastName, x.ApplicantFirstName, x.ApplicantBirthPlace, x.ApplicantDateOfBirth,
            livingCity, x.Note));
    }

    // ---- Helpers ----

    private async Task<(string? Error, byte CompanyId)> ResolveCompanyIdAsync(byte? requestedCompanyId)
    {
        if (_tenant.IsAdmin && requestedCompanyId.HasValue)
        {
            if (!await _db.Companies.AnyAsync(c => c.Id == requestedCompanyId.Value))
                return ($"Company {requestedCompanyId.Value} does not exist.", 0);
            return (null, requestedCompanyId.Value);
        }
        if (_tenant.CompanyId.HasValue) return (null, _tenant.CompanyId.Value);
        return ("No tenant CompanyId resolved from the token.", 0);
    }

    private async Task<string?> ValidateAsync(IdlWriteDto dto, long? currentId)
    {
        if (string.IsNullOrWhiteSpace(dto.NumberOfLicence))
            return "Бр. на меѓународна дозвола е задолжителен.";
        if (string.IsNullOrWhiteSpace(dto.NumberOfNationalLicence))
            return "Бр. на национална дозвола е задолжителен.";
        if (dto.IssuedDate.Date > dto.ValidTillDate.Date)
            return "Датумот на издавање мора да биде пред датумот на важност.";
        if (dto.CategoryIds == null || dto.CategoryIds.Count == 0)
            return "Изберете најмалку една категорија на возила.";
        // Лични податоци: сè задолжително освен Име на родител.
        var a = dto.Applicant;
        if (a is null || string.IsNullOrWhiteSpace(a.FirstName) || string.IsNullOrWhiteSpace(a.LastName)
            || string.IsNullOrWhiteSpace(a.Citizenship) || a.DateOfBirth is null
            || string.IsNullOrWhiteSpace(a.BirthPlace) || string.IsNullOrWhiteSpace(a.Address))
            return "Личните податоци се задолжителни — име, презиме, државјанство, датум на раѓање, место на раѓање и адреса.";
        // Документи: лична карта + национална возачка задолжителни (пасошот не е — не е во ПРИЛОГ-от).
        if (string.IsNullOrWhiteSpace(a.IdCardNumber) || a.IdCardDate is null || string.IsNullOrWhiteSpace(a.IdCardIssuer))
            return "Личната карта е задолжителна — број, датум и издадена од.";
        if (a.NationalLicenceDate is null || a.NationalLicenceExpiry is null || string.IsNullOrWhiteSpace(a.NationalLicenceIssuer))
            return "Националната возачка е задолжителна — датум, важи до и издадена од.";
        // Меѓународната не смее да важи подолго од националната возачка.
        if (dto.ValidTillDate.Date > a.NationalLicenceExpiry.Value.Date)
            return "Датумот на важност на меѓународната возачка не може да биде поголем од датумот на важност на националната возачка.";
        if (!await _db.Clients.AnyAsync(c => c.Id == dto.ClientId))
            return "Клиентот не постои.";
        if (!await _db.TechnicalExamOrganizations.AnyAsync(o => o.Id == dto.IssuerOrganizationId && o.Active))
            return "Издавачот (организацијата) не постои.";
        var validCategoryCount = await _db.DrivingLicenceCategories
            .Where(c => dto.CategoryIds.Contains(c.Id)).CountAsync();
        if (validCategoryCount != dto.CategoryIds.Distinct().Count())
            return "Една или повеќе категории не се валидни.";
        // Legacy rule: "Бројот на дозволата мора да биде единствен" — app-level check for a
        // friendly message; the DB unique index is the race-safe backstop.
        var trimmedNumber = dto.NumberOfLicence.Trim();
        var duplicate = await _db.InternationalDrivingLicences.AsNoTracking()
            .AnyAsync(x => x.NumberOfLicence == trimmedNumber && x.Id != (currentId ?? 0));
        if (duplicate) return "Бројот на дозволата мора да биде единствен.";
        return null;
    }

    private async Task ReplaceCategoriesAsync(long idlId, IReadOnlyList<int> categoryIds)
    {
        var existing = await _db.InternationalDrivingLicenceCategories
            .Where(c => c.InternationalDrivingLicenceId == idlId).ToListAsync();
        _db.InternationalDrivingLicenceCategories.RemoveRange(existing);
        foreach (var catId in categoryIds.Distinct())
            _db.InternationalDrivingLicenceCategories.Add(new InternationalDrivingLicenceCategory
            {
                InternationalDrivingLicenceId = idlId,
                DrivingLicenceCategoryId = catId,
            });
        await _db.SaveChangesAsync();
    }

    /// <summary>Get-or-create the client's vehicle-less anchor relation (RelationTypeId=3,
    /// "не е потребно возило" — already a real legacy/seeded relation type) so a person-level
    /// debt (IDL today; Permission/TrafficLicence later) has something to attach to. Mirrors
    /// RequestsController.ResolveOrCreateNewOwnerRelationAsync's reuse-or-create shape.</summary>
    private async Task<long> GetOrCreatePersonalRelationAsync(long clientId)
    {
        const byte personalRelationTypeId = 3;
        var existing = await _db.ClientVehicleRelations
            .Where(r => r.ClientId == clientId && r.VehicleId == null && r.RelationTypeId == personalRelationTypeId)
            .OrderByDescending(r => r.Active).ThenByDescending(r => r.StartDate)
            .Select(r => (long?)r.Id)
            .FirstOrDefaultAsync();
        if (existing.HasValue) return existing.Value;

        var rel = new ClientVehicleRelation
        {
            ClientId = clientId,
            VehicleId = null,
            RelationTypeId = personalRelationTypeId,
            StartDate = DateTime.UtcNow,
            Active = true,
        };
        _db.ClientVehicleRelations.Add(rel);
        await _db.SaveChangesAsync();
        return rel.Id;
    }

    private async Task<IdlFullDto> ToFullDtoAsync(InternationalDrivingLicence x)
    {
        var c = await _db.Clients.AsNoTracking().Where(c => c.Id == x.ClientId)
            .Select(c => new { c.FirstName, c.MiddleName, c.LastName, c.MB }).FirstOrDefaultAsync();
        string? clientName = null;
        if (c != null)
        {
            var n = string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }.Where(s => !string.IsNullOrWhiteSpace(s)));
            clientName = n.Length > 0 ? n : null;
        }
        var orgName = await _db.TechnicalExamOrganizations.AsNoTracking()
            .Where(o => o.Id == x.IssuerOrganizationId).Select(o => o.Name).FirstOrDefaultAsync();
        var categoryIds = await _db.InternationalDrivingLicenceCategories.AsNoTracking()
            .Where(cat => cat.InternationalDrivingLicenceId == x.Id)
            .Select(cat => cat.DrivingLicenceCategoryId).OrderBy(i => i).ToListAsync();

        return new IdlFullDto(
            x.Id, x.CompanyId, x.ClientId, clientName, c?.MB,
            x.IssuerOrganizationId, orgName,
            x.NumberOfLicence, x.NumberOfNationalLicence,
            x.IssuedDate, x.ValidTillDate,
            x.Note, x.Active, x.CreatedAt, x.ModifiedAt,
            categoryIds, ToApplicantDto(x), x.RowVersion);
    }

    private static IdlApplicantDto ToApplicantDto(InternationalDrivingLicence x) => new(
        x.ApplicantFirstName, x.ApplicantLastName, x.ApplicantParentName, x.ApplicantCitizenship,
        x.ApplicantDateOfBirth, x.ApplicantBirthPlace, x.ApplicantAddress,
        x.ApplicantPassportNumber, x.ApplicantPassportIssuer, x.ApplicantPassportDate, x.ApplicantPassportExpiry,
        x.ApplicantIdCardNumber, x.ApplicantIdCardIssuer, x.ApplicantIdCardDate, x.ApplicantIdCardExpiry,
        x.ApplicantNationalLicenceIssuer, x.ApplicantNationalLicenceDate, x.ApplicantNationalLicenceExpiry);

    /// <summary>Copy the editable applicant snapshot from the DTO onto the licence. Trims
    /// strings; a null Applicant (older clients) leaves the snapshot untouched.</summary>
    private static void ApplyApplicant(InternationalDrivingLicence e, IdlApplicantDto? a)
    {
        if (a is null) return;
        static string? T(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        e.ApplicantFirstName = T(a.FirstName);
        e.ApplicantLastName = T(a.LastName);
        e.ApplicantParentName = T(a.ParentName);
        e.ApplicantCitizenship = T(a.Citizenship);
        e.ApplicantDateOfBirth = a.DateOfBirth;
        e.ApplicantBirthPlace = T(a.BirthPlace);
        e.ApplicantAddress = T(a.Address);
        e.ApplicantPassportNumber = T(a.PassportNumber);
        e.ApplicantPassportIssuer = T(a.PassportIssuer);
        e.ApplicantPassportDate = a.PassportDate;
        e.ApplicantPassportExpiry = a.PassportExpiry;
        e.ApplicantIdCardNumber = T(a.IdCardNumber);
        e.ApplicantIdCardIssuer = T(a.IdCardIssuer);
        e.ApplicantIdCardDate = a.IdCardDate;
        e.ApplicantIdCardExpiry = a.IdCardExpiry;
        e.ApplicantNationalLicenceIssuer = T(a.NationalLicenceIssuer);
        e.ApplicantNationalLicenceDate = a.NationalLicenceDate;
        e.ApplicantNationalLicenceExpiry = a.NationalLicenceExpiry;
    }
}
