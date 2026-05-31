using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Admin CRUD for the three payment-side catalogs:
//  - CalculationItems  (per-tenant fee items with target bank account + form)
//  - PriceCatalog      (per-tenant pricing list)
//  - DDVCatalog        (global VAT rates with effective dates)
// Read endpoints are open to any authenticated user.

[ApiController]
[Route("api/calculation-items")]
[Authorize]
public class CalculationItemsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public CalculationItemsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record DTO(int Id, string ItemName, string BankAccount, string Bank, string Form, bool IsActive);
    public record SaveRequest(string ItemName, string BankAccount, string Bank, string Form, bool IsActive);

    [HttpGet]
    public async Task<ActionResult<List<DTO>>> List() =>
        await _db.CalculationItems.OrderBy(c => c.ItemName)
            .Select(c => new DTO(c.Id, c.ItemName, c.BankAccount, c.Bank, c.Form, c.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<DTO>> Create(SaveRequest req)
    {
        if (!Validate(req, out var err)) return BadRequest(new { error = err });
        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });
        var c = new CalculationItem { StationId = stationId.Value, ItemName = req.ItemName, BankAccount = req.BankAccount, Bank = req.Bank, Form = req.Form, IsActive = req.IsActive };
        _db.CalculationItems.Add(c); await _db.SaveChangesAsync();
        return Ok(new DTO(c.Id, c.ItemName, c.BankAccount, c.Bank, c.Form, c.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, SaveRequest req)
    {
        if (!Validate(req, out var err)) return BadRequest(new { error = err });
        var c = await _db.CalculationItems.FindAsync(id);
        if (c is null) return NotFound();
        c.ItemName = req.ItemName; c.BankAccount = req.BankAccount; c.Bank = req.Bank; c.Form = req.Form; c.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static bool Validate(SaveRequest r, out string err)
    {
        if (string.IsNullOrWhiteSpace(r.ItemName))    { err = "BR-PAY-040: ItemName required."; return false; }
        if (string.IsNullOrWhiteSpace(r.BankAccount)) { err = "BR-PAY-041: BankAccount required."; return false; }
        if (string.IsNullOrWhiteSpace(r.Bank))        { err = "BR-PAY-042: Bank required."; return false; }
        if (string.IsNullOrWhiteSpace(r.Form))        { err = "BR-PAY-043: Form required."; return false; }
        err = ""; return true;
    }
}

[ApiController]
[Route("api/price-catalog")]
[Authorize]
public class PriceCatalogController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    public PriceCatalogController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record DTO(int Id, string Name, decimal Price, int? CalculationItemId, int? DDVId, DateOnly? EffectiveFrom, DateOnly? EffectiveTo, bool IsActive);
    public record SaveRequest(string Name, decimal Price, int? CalculationItemId, int? DDVId, DateOnly? EffectiveFrom, DateOnly? EffectiveTo, bool IsActive);

    [HttpGet]
    public async Task<ActionResult<List<DTO>>> List() =>
        await _db.PriceCatalogs.OrderBy(p => p.Name)
            .Select(p => new DTO(p.Id, p.Name, p.Price, p.CalculationItemId, p.DDVId, p.EffectiveFrom, p.EffectiveTo, p.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<DTO>> Create(SaveRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name required." });
        if (req.Price < 0) return BadRequest(new { error = "Price must be >= 0." });
        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });
        var p = new PriceCatalog { StationId = stationId.Value, Name = req.Name, Price = req.Price, CalculationItemId = req.CalculationItemId, DDVId = req.DDVId, EffectiveFrom = req.EffectiveFrom, EffectiveTo = req.EffectiveTo, IsActive = req.IsActive };
        _db.PriceCatalogs.Add(p); await _db.SaveChangesAsync();
        return Ok(new DTO(p.Id, p.Name, p.Price, p.CalculationItemId, p.DDVId, p.EffectiveFrom, p.EffectiveTo, p.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, SaveRequest req)
    {
        var p = await _db.PriceCatalogs.FindAsync(id);
        if (p is null) return NotFound();
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name required." });
        if (req.Price < 0) return BadRequest(new { error = "Price must be >= 0." });
        p.Name = req.Name; p.Price = req.Price; p.CalculationItemId = req.CalculationItemId; p.DDVId = req.DDVId;
        p.EffectiveFrom = req.EffectiveFrom; p.EffectiveTo = req.EffectiveTo; p.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}

[ApiController]
[Route("api/ddv-catalog")]
[Authorize]
public class DDVCatalogController : ControllerBase
{
    private readonly VteDbContext _db;
    public DDVCatalogController(VteDbContext db) => _db = db;

    public record DTO(int Id, string Name, decimal Rate, DateOnly EffectiveFrom, DateOnly? EffectiveTo, bool IsActive);
    public record SaveRequest(string Name, decimal Rate, DateOnly EffectiveFrom, DateOnly? EffectiveTo, bool IsActive);

    [HttpGet]
    public async Task<ActionResult<List<DTO>>> List() =>
        await _db.DDVCatalogs.OrderBy(d => d.Rate)
            .Select(d => new DTO(d.Id, d.Name, d.Rate, d.EffectiveFrom, d.EffectiveTo, d.IsActive))
            .ToListAsync();

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<DTO>> Create(SaveRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name required." });
        if (req.Rate < 0 || req.Rate > 100) return BadRequest(new { error = "Rate must be in [0, 100]." });
        if (req.EffectiveTo is not null && req.EffectiveTo < req.EffectiveFrom) return BadRequest(new { error = "EffectiveTo must be on or after EffectiveFrom." });
        var d = new DDVCatalog { Name = req.Name, Rate = req.Rate, EffectiveFrom = req.EffectiveFrom, EffectiveTo = req.EffectiveTo, IsActive = req.IsActive };
        _db.DDVCatalogs.Add(d); await _db.SaveChangesAsync();
        return Ok(new DTO(d.Id, d.Name, d.Rate, d.EffectiveFrom, d.EffectiveTo, d.IsActive));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Update(int id, SaveRequest req)
    {
        var d = await _db.DDVCatalogs.FindAsync(id);
        if (d is null) return NotFound();
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name required." });
        if (req.Rate < 0 || req.Rate > 100) return BadRequest(new { error = "Rate must be in [0, 100]." });
        d.Name = req.Name; d.Rate = req.Rate; d.EffectiveFrom = req.EffectiveFrom; d.EffectiveTo = req.EffectiveTo; d.IsActive = req.IsActive;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
