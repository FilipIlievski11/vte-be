using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Dedicated VehicleCategories CRUD that mirrors the legacy uxVehicleCategories form:
// the category itself plus three child collections — Relations (Category × BodyType × Use ×
// PaymentCategory), RequiredFields (per-category required fields on the Vehicle form),
// and DisabledFields (per-category disabled fields on the Vehicle form).
[ApiController]
[Route("api/vehicle-categories")]
[Authorize]
public class VehicleCategoriesController : ControllerBase
{
    private readonly VteDbContext _db;
    public VehicleCategoriesController(VteDbContext db) => _db = db;

    public record CategoryListDto(int Id, string? Code, string Name, string? Mksjus, string? Iso, bool IsActive);

    public record RelationDto(int? Id, int? BodyTypeId, int? UseId, int? CategoryForPaymentsId, string? DetailDescription);
    public record FieldDto(int? Id, string FieldName);

    public record CategoryDetailDto(int Id, string? Code, string Name, string? OldName,
        string? Mksjus, string? Iso, string? MksjusDescription, string? PicturePath,
        string? Description, string? DetailDescription, bool IsActive,
        IReadOnlyList<RelationDto> Relations,
        IReadOnlyList<FieldDto> RequiredFields,
        IReadOnlyList<FieldDto> DisabledFields);

    public record CategoryRequest(string? Code, string Name, string? OldName,
        string? Mksjus, string? Iso, string? MksjusDescription, string? PicturePath,
        string? Description, string? DetailDescription, bool IsActive,
        IReadOnlyList<RelationDto>? Relations,
        IReadOnlyList<FieldDto>? RequiredFields,
        IReadOnlyList<FieldDto>? DisabledFields);

    [HttpGet]
    public async Task<List<CategoryListDto>> List()
        => await _db.VehicleCategories.AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoryListDto(c.Id, c.Code, c.Name, c.Mksjus, c.Iso, c.IsActive))
            .ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDetailDto>> Get(int id)
    {
        var c = await _db.VehicleCategories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();
        var rels = await _db.VehicleCategoryRelations.AsNoTracking()
            .Where(r => r.CategoryId == id)
            .OrderBy(r => r.Id)
            .Select(r => new RelationDto(r.Id, r.BodyTypeId, r.UseId, r.CategoryForPaymentsId, r.DetailDescription))
            .ToListAsync();
        var reqs = await _db.VehicleCategoryRequiredFields.AsNoTracking()
            .Where(r => r.CategoryId == id)
            .OrderBy(r => r.Id)
            .Select(r => new FieldDto(r.Id, r.FieldName))
            .ToListAsync();
        var diss = await _db.VehicleCategoryDisabledFields.AsNoTracking()
            .Where(r => r.CategoryId == id)
            .OrderBy(r => r.Id)
            .Select(r => new FieldDto(r.Id, r.FieldName))
            .ToListAsync();
        return Ok(new CategoryDetailDto(c.Id, c.Code, c.Name, c.OldName, c.Mksjus, c.Iso,
            c.MksjusDescription, c.PicturePath, c.Description, c.DetailDescription, c.IsActive,
            rels, reqs, diss));
    }

    [HttpPost]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CategoryDetailDto>> Create(CategoryRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });
        var c = new VehicleCategory();
        Apply(c, req);
        _db.VehicleCategories.Add(c);
        await _db.SaveChangesAsync();
        await ReplaceChildren(c.Id, req);
        return await Get(c.Id);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<ActionResult<CategoryDetailDto>> Update(int id, CategoryRequest req)
    {
        if (!Validate(req, out var error)) return BadRequest(new { error });
        var c = await _db.VehicleCategories.FindAsync(id);
        if (c is null) return NotFound();
        Apply(c, req);
        await _db.SaveChangesAsync();
        await ReplaceChildren(id, req);
        return await Get(id);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> Delete(int id)
    {
        var c = await _db.VehicleCategories.FindAsync(id);
        if (c is null) return NotFound();
        try
        {
            // remove children first to avoid orphans
            _db.VehicleCategoryRelations.RemoveRange(_db.VehicleCategoryRelations.Where(r => r.CategoryId == id));
            _db.VehicleCategoryRequiredFields.RemoveRange(_db.VehicleCategoryRequiredFields.Where(r => r.CategoryId == id));
            _db.VehicleCategoryDisabledFields.RemoveRange(_db.VehicleCategoryDisabledFields.Where(r => r.CategoryId == id));
            _db.VehicleCategories.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new { error = "Cannot delete: category is referenced by vehicles. Deactivate it instead." });
        }
    }

    // ---- helpers ----

    private static bool Validate(CategoryRequest r, out string error)
    {
        if (string.IsNullOrWhiteSpace(r.Name)) { error = "Name is required."; return false; }
        if (r.Code is { Length: > 10 })        { error = "Code max length is 10."; return false; }
        error = ""; return true;
    }

    private static void Apply(VehicleCategory c, CategoryRequest r)
    {
        c.Code              = Trim(r.Code);
        c.Name              = r.Name.Trim();
        c.OldName           = Trim(r.OldName);
        c.Mksjus            = Trim(r.Mksjus);
        c.Iso               = Trim(r.Iso);
        c.MksjusDescription = Trim(r.MksjusDescription);
        c.PicturePath       = Trim(r.PicturePath);
        c.Description       = Trim(r.Description);
        c.DetailDescription = Trim(r.DetailDescription);
        c.IsActive          = r.IsActive;
        c.LastModifiedUtc   = DateTime.UtcNow;
    }

    private async Task ReplaceChildren(int categoryId, CategoryRequest req)
    {
        // Wholesale replace — simplest semantics for the form. The user always sees the
        // full list of children on the edit page and saves them as one unit.
        _db.VehicleCategoryRelations.RemoveRange(_db.VehicleCategoryRelations.Where(r => r.CategoryId == categoryId));
        _db.VehicleCategoryRequiredFields.RemoveRange(_db.VehicleCategoryRequiredFields.Where(r => r.CategoryId == categoryId));
        _db.VehicleCategoryDisabledFields.RemoveRange(_db.VehicleCategoryDisabledFields.Where(r => r.CategoryId == categoryId));

        if (req.Relations is not null)
            foreach (var r in req.Relations)
                _db.VehicleCategoryRelations.Add(new VehicleCategoryRelation
                {
                    CategoryId = categoryId,
                    BodyTypeId = r.BodyTypeId,
                    UseId = r.UseId,
                    CategoryForPaymentsId = r.CategoryForPaymentsId,
                    DetailDescription = Trim(r.DetailDescription),
                });
        if (req.RequiredFields is not null)
            foreach (var f in req.RequiredFields.Where(x => !string.IsNullOrWhiteSpace(x.FieldName)))
                _db.VehicleCategoryRequiredFields.Add(new VehicleCategoryRequiredField { CategoryId = categoryId, FieldName = f.FieldName.Trim() });
        if (req.DisabledFields is not null)
            foreach (var f in req.DisabledFields.Where(x => !string.IsNullOrWhiteSpace(x.FieldName)))
                _db.VehicleCategoryDisabledFields.Add(new VehicleCategoryDisabledField { CategoryId = categoryId, FieldName = f.FieldName.Trim() });
        await _db.SaveChangesAsync();
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
