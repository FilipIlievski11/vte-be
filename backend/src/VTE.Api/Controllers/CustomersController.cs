using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Customers;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/customers")]
[Authorize]
public class CustomersController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public CustomersController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record CustomerListDto(long Id, string FirstName, string? Surname, string? EMBG, bool IsCompany,
        string? PhoneNumber, string? Email, bool IsActive,
        string? LivingStreet, string? LivingAddressNumber, string? LivingCity);

    public record ContactPersonDto(int? Id, string FirstName, string Surname, string? EMBG, string? PhoneNumber, string? MobileNumber, string? Email);
    public record BankAccountDto  (long? Id, string BankAccount, string DeponentBank, string? TaxNumber);

    public record CustomerDetailDto(
        long Id, int StationId, bool IsCompany, bool IsActive,
        string? EMBG, string FirstName, string? Surname, string? ParentName,
        DateOnly? DateOfBirth, int? CitizenshipId, int? BusinessTypeId,
        int? LivingAddressId, string? LivingAddressNumber, int? LivingCityId,
        int? BirthCityId, int? BirthAddressId, string? BirthAddressNumber,
        string? Occupation, string? WorksInCompany,
        string? PhoneNumber, string? Fax, string? Email, bool CanSendNotifications,
        string? IDCardNumber, DateOnly? IDCardDateIssued, int? IDCardIssuerId,
        string? PassportNumber, DateOnly? PassportDateIssued, int? PassportIssuerId,
        string? DrivingLicenceNumber, DateOnly? DrivingLicenceDateIssued, int? DrivingLicenceIssuerId,
        string? TaxNumber, string? Status, string? Note,
        IReadOnlyList<ContactPersonDto> ContactPersons,
        IReadOnlyList<BankAccountDto> BankAccounts);

    public record CustomerSaveRequest(
        bool IsCompany,
        string? EMBG, string FirstName, string? Surname, string? ParentName,
        DateOnly? DateOfBirth, int? CitizenshipId, int? BusinessTypeId,
        int? LivingAddressId, string? LivingAddressNumber, int? LivingCityId,
        int? BirthCityId, int? BirthAddressId, string? BirthAddressNumber,
        string? Occupation, string? WorksInCompany,
        string? PhoneNumber, string? Fax, string? Email, bool CanSendNotifications,
        string? IDCardNumber, DateOnly? IDCardDateIssued, int? IDCardIssuerId,
        string? PassportNumber, DateOnly? PassportDateIssued, int? PassportIssuerId,
        string? DrivingLicenceNumber, DateOnly? DrivingLicenceDateIssued, int? DrivingLicenceIssuerId,
        string? TaxNumber, string? Status, string? Note,
        IReadOnlyList<ContactPersonDto>? ContactPersons,
        IReadOnlyList<BankAccountDto>? BankAccounts);

    // Default ORDER BY mirrors the legacy stored procedure `getCustomersList`:
    // newest first by primary key (legacy ran `ORDER BY Customers.Id DESC`).
    private static readonly SortMap<Customer> CustomerSort = new SortMap<Customer>(c => c.Id, defaultDescending: true)
        .Add("customer",    c => c.Surname)
        .Add("firstName",   c => c.FirstName)
        .Add("surname",     c => c.Surname)
        .Add("embg",        c => c.EMBG)
        .Add("phoneNumber", c => c.PhoneNumber)
        .Add("email",       c => c.Email)
        .Add("id",          c => c.Id);

    [HttpGet]
    public async Task<ActionResult<PagedResult<CustomerListDto>>> List(
        [FromQuery] string? q, [FromQuery] bool? isCompany,
        [FromQuery] bool includeInactive,
        [FromQuery] int? page, [FromQuery] int? pageSize,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir, [FromQuery] int? take)
    {
        var query = _db.Customers.AsNoTracking().AsQueryable();
        if (!includeInactive) query = query.Where(c => c.IsActive);
        if (isCompany is bool b) query = query.Where(c => c.IsCompany == b);
        if (!string.IsNullOrWhiteSpace(q))
        {
            // Whitespace-separated tokens are AND'd; each token may match any field.
            // Lets users type "Иван Петров" and find a row whose FirstName matches one
            // token and Surname matches the other, in either order.
            var tokens = q.Trim().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                var pattern = $"%{token}%";
                query = query.Where(c =>
                    EF.Functions.Like(c.FirstName, pattern) ||
                    EF.Functions.Like(c.Surname  ?? "", pattern) ||
                    EF.Functions.Like(c.EMBG     ?? "", pattern) ||
                    EF.Functions.Like(c.Email    ?? "", pattern) ||
                    EF.Functions.Like(c.PhoneNumber ?? "", pattern));
            }
        }
        var ordered = CustomerSort.Apply(query, sortBy, sortDir)
            .Select(c => new CustomerListDto(
                c.Id, c.FirstName, c.Surname, c.EMBG, c.IsCompany, c.PhoneNumber, c.Email, c.IsActive,
                _db.Streets.Where(s => s.Id == c.LivingAddressId).Select(s => s.Name).FirstOrDefault(),
                c.LivingAddressNumber,
                _db.Cities .Where(ci => ci.Id == c.LivingCityId).Select(ci => ci.Name).FirstOrDefault()));

        if (take is > 0)
            return Ok(new PagedResult<CustomerListDto>(1, take.Value, await ordered.CountAsync(), await ordered.Take(take.Value).ToListAsync()));

        return Ok(await ordered.ToPagedAsync(page, pageSize));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<CustomerDetailDto>> Get(long id)
    {
        var c = await _db.Customers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();

        var contacts = await _db.Set<CustomerContactPerson>().AsNoTracking()
            .Where(p => p.CustomerId == id)
            .Select(p => new ContactPersonDto(p.Id, p.FirstName, p.Surname, p.EMBG, p.PhoneNumber, p.MobileNumber, p.Email))
            .ToListAsync();
        var accounts = await _db.Set<CustomerBankAccount>().AsNoTracking()
            .Where(a => a.CustomerId == id)
            .Select(a => new BankAccountDto(a.Id, a.BankAccount, a.DeponentBank, a.TaxNumber))
            .ToListAsync();

        return ToDetail(c, contacts, accounts);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDetailDto>> Create(CustomerSaveRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });

        // Resolve StationId: operators get it from JWT claim;
        // administrators fall back to the first active Station (seeded "Default Station").
        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var c = new Customer
        {
            StationId = stationId.Value,
            CreatedByUserId = userId,
            LastModifiedByUserId = userId,
        };
        Apply(c, req);
        _db.Customers.Add(c);
        await _db.SaveChangesAsync();

        await ReplaceContactPersons(c.Id, req.ContactPersons);
        await ReplaceBankAccounts(c.Id, req.BankAccounts);

        return CreatedAtAction(nameof(Get), new { id = c.Id }, await BuildDetail(c.Id));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<CustomerDetailDto>> Update(long id, CustomerSaveRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });
        var c = await _db.Customers.FindAsync(id);
        if (c is null) return NotFound();

        Apply(c, req);
        c.LastModifiedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();

        await ReplaceContactPersons(id, req.ContactPersons);
        await ReplaceBankAccounts(id, req.BankAccounts);

        return await BuildDetail(id);
    }

    // ----- helpers -----

    private static bool Validate(CustomerSaveRequest req, out string error)
    {
        if (string.IsNullOrWhiteSpace(req.FirstName))
        { error = "BR-CUS-005: FirstName is required."; return false; }
        if (req.CanSendNotifications && string.IsNullOrWhiteSpace(req.Email))
        { error = "BR-CUS-013: Email is required when CanSendNotifications is true."; return false; }
        if (!string.IsNullOrEmpty(req.EMBG) && req.EMBG.Length > 13)
        { error = "BR-CUS-001: EMBG max length is 13."; return false; }
        error = ""; return true;
    }

    private static void Apply(Customer c, CustomerSaveRequest r)
    {
        c.IsCompany             = r.IsCompany;
        c.EMBG                  = string.IsNullOrWhiteSpace(r.EMBG) ? null : r.EMBG.Trim();
        c.FirstName             = r.FirstName.Trim();
        c.Surname               = string.IsNullOrWhiteSpace(r.Surname) ? null : r.Surname.Trim();
        c.ParentName            = r.ParentName;
        c.DateOfBirth           = r.DateOfBirth;
        c.CitizenshipId         = r.CitizenshipId;
        c.BusinessTypeId        = r.BusinessTypeId;
        c.LivingAddressId       = r.LivingAddressId;
        c.LivingAddressNumber   = r.LivingAddressNumber;
        c.LivingCityId          = r.LivingCityId;
        c.BirthCityId           = r.BirthCityId;
        c.BirthAddressId        = r.BirthAddressId;
        c.BirthAddressNumber    = r.BirthAddressNumber;
        c.Occupation            = r.Occupation;
        c.WorksInCompany        = r.WorksInCompany;
        c.PhoneNumber           = r.PhoneNumber;
        c.Fax                   = r.Fax;
        c.Email                 = r.Email;
        c.CanSendNotifications  = r.CanSendNotifications;
        c.IDCardNumber          = r.IDCardNumber;
        c.IDCardDateIssued      = r.IDCardDateIssued;
        c.IDCardIssuerId        = r.IDCardIssuerId;
        c.PassportNumber        = r.PassportNumber;
        c.PassportDateIssued    = r.PassportDateIssued;
        c.PassportIssuerId      = r.PassportIssuerId;
        c.DrivingLicenceNumber  = r.DrivingLicenceNumber;
        c.DrivingLicenceDateIssued = r.DrivingLicenceDateIssued;
        c.DrivingLicenceIssuerId   = r.DrivingLicenceIssuerId;
        c.TaxNumber             = r.TaxNumber;
        c.Status                = r.Status;
        c.Note                  = r.Note;
    }

    private async Task ReplaceContactPersons(long customerId, IReadOnlyList<ContactPersonDto>? items)
    {
        var existing = await _db.Set<CustomerContactPerson>().Where(p => p.CustomerId == customerId).ToListAsync();
        _db.RemoveRange(existing);
        if (items is not null)
        {
            foreach (var p in items.Where(x => !string.IsNullOrWhiteSpace(x.FirstName) || !string.IsNullOrWhiteSpace(x.Surname)))
            {
                _db.Add(new CustomerContactPerson
                {
                    CustomerId = customerId,
                    FirstName = p.FirstName, Surname = p.Surname, EMBG = p.EMBG,
                    PhoneNumber = p.PhoneNumber, MobileNumber = p.MobileNumber, Email = p.Email,
                });
            }
        }
        await _db.SaveChangesAsync();
    }

    private async Task ReplaceBankAccounts(long customerId, IReadOnlyList<BankAccountDto>? items)
    {
        var existing = await _db.Set<CustomerBankAccount>().Where(a => a.CustomerId == customerId).ToListAsync();
        _db.RemoveRange(existing);
        if (items is not null)
        {
            foreach (var a in items.Where(x => !string.IsNullOrWhiteSpace(x.BankAccount) || !string.IsNullOrWhiteSpace(x.DeponentBank)))
            {
                _db.Add(new CustomerBankAccount
                {
                    CustomerId = customerId,
                    BankAccount = a.BankAccount, DeponentBank = a.DeponentBank, TaxNumber = a.TaxNumber,
                });
            }
        }
        await _db.SaveChangesAsync();
    }

    private async Task<CustomerDetailDto> BuildDetail(long id)
    {
        var c = await _db.Customers.FindAsync(id);
        var contacts = await _db.Set<CustomerContactPerson>()
            .Where(p => p.CustomerId == id)
            .Select(p => new ContactPersonDto(p.Id, p.FirstName, p.Surname, p.EMBG, p.PhoneNumber, p.MobileNumber, p.Email))
            .ToListAsync();
        var accounts = await _db.Set<CustomerBankAccount>()
            .Where(a => a.CustomerId == id)
            .Select(a => new BankAccountDto(a.Id, a.BankAccount, a.DeponentBank, a.TaxNumber))
            .ToListAsync();
        return ToDetail(c!, contacts, accounts);
    }

    private static CustomerDetailDto ToDetail(Customer c, IReadOnlyList<ContactPersonDto> contacts, IReadOnlyList<BankAccountDto> accounts)
        => new(
            c.Id, c.StationId, c.IsCompany, c.IsActive,
            c.EMBG, c.FirstName, c.Surname, c.ParentName,
            c.DateOfBirth, c.CitizenshipId, c.BusinessTypeId,
            c.LivingAddressId, c.LivingAddressNumber, c.LivingCityId,
            c.BirthCityId, c.BirthAddressId, c.BirthAddressNumber,
            c.Occupation, c.WorksInCompany,
            c.PhoneNumber, c.Fax, c.Email, c.CanSendNotifications,
            c.IDCardNumber, c.IDCardDateIssued, c.IDCardIssuerId,
            c.PassportNumber, c.PassportDateIssued, c.PassportIssuerId,
            c.DrivingLicenceNumber, c.DrivingLicenceDateIssued, c.DrivingLicenceIssuerId,
            c.TaxNumber, c.Status, c.Note,
            contacts, accounts);
}
