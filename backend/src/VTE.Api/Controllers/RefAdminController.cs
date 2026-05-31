using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Admin-only write endpoints for the simple REF tables.
// The matching read endpoints live on ReferenceDataController (any role).
[ApiController]
[Route("api/ref")]
[Authorize(Roles = Roles.Administrator)]
public class RefAdminController : ControllerBase
{
    private readonly VteDbContext _db;
    public RefAdminController(VteDbContext db) => _db = db;

    public record SimpleRefDto(int Id, string Name, bool IsActive);
    public record SimpleRefRequest(string Name, bool IsActive);

    // Single create/update/delete pair per kind. Each switch dispatches to the
    // right DbSet by kind name. Keeps the surface area predictable.

    [HttpPost("{kind}")]
    public async Task<ActionResult<SimpleRefDto>> Create(string kind, SimpleRefRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var (id, ok, err) = await CreateAsync(kind, req);
        if (!ok) return BadRequest(new { error = err });
        return Ok(new SimpleRefDto(id, req.Name, req.IsActive));
    }

    [HttpPut("{kind}/{id:int}")]
    public async Task<IActionResult> Update(string kind, int id, SimpleRefRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest(new { error = "Name is required." });
        var (ok, err) = await UpdateAsync(kind, id, req);
        if (!ok) return err == "notfound" ? NotFound() : BadRequest(new { error = err });
        return NoContent();
    }

    [HttpDelete("{kind}/{id:int}")]
    public async Task<IActionResult> Delete(string kind, int id)
    {
        var (ok, err) = await DeleteAsync(kind, id);
        if (!ok) return err == "notfound" ? NotFound() : BadRequest(new { error = err });
        return NoContent();
    }

    // ---- dispatch helpers ----

    private async Task<(int id, bool ok, string err)> CreateAsync(string kind, SimpleRefRequest req)
    {
        switch (kind)
        {
            case "countries":            { var e = new Country               { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "communities":          { var e = new Community             { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "cities":               { var e = new City                  { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "streets":              { var e = new Street                { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "business-types":       { var e = new BusinessType          { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "registration-issuers": { var e = new RegistrationIssuer    { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "customer-vehicle-relation-types": { var e = new CustomerVehicleRelationType { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-body-types":   { var e = new VehicleBodyType       { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-categories":   { var e = new VehicleCategory       { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-uses":         { var e = new VehicleUse            { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-makers":       { var e = new VehicleMaker          { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-models":       { var e = new VehicleModel          { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-engine-types": { var e = new VehicleEngineType     { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-engine-power-source-types": { var e = new VehicleEnginePowerSourceType { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-engine-eco-programs":       { var e = new VehicleEngineEcoProgram { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-gearboxes":    { var e = new VehicleGearBox        { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-brakes":       { var e = new VehicleBrake          { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-supportings":  { var e = new VehicleSupporting     { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "colors":               { var e = new Color                 { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-categories-for-payments": { var e = new VehicleCategoryForPayments { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-tire-types":   { var e = new VehicleTireType       { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "technical-exam-organizations": { var e = new TechnicalExamOrganization { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "technical-exam-types":         { var e = new TechnicalExamType { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "technical-exam-vehicle-parts": { var e = new TechnicalExamVehiclePart { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "technical-exam-report-detail-statuses": { var e = new TechnicalExamReportDetailStatus { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "driving-licence-categories":   { var e = new DrivingLicenceCategory { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "vehicle-ownership-proof-types":{ var e = new VehicleOwnershipProofType { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            case "payment-proof-types":          { var e = new PaymentProofType { Name = req.Name, IsActive = req.IsActive }; _db.Add(e); await _db.SaveChangesAsync(); return (e.Id, true, ""); }
            default: return (0, false, $"Unknown kind '{kind}'.");
        }
    }

    private async Task<(bool ok, string err)> UpdateAsync(string kind, int id, SimpleRefRequest req)
    {
        var found = await Find(kind, id);
        if (found is null) return (false, "notfound");
        // All RefBaseInt subclasses share Name + IsActive
        if (found is RefBaseInt rb) { rb.Name = req.Name; rb.IsActive = req.IsActive; }
        else return (false, $"Cannot update kind '{kind}'.");
        await _db.SaveChangesAsync();
        return (true, "");
    }

    private async Task<(bool ok, string err)> DeleteAsync(string kind, int id)
    {
        var found = await Find(kind, id);
        if (found is null) return (false, "notfound");
        try
        {
            _db.Remove(found);
            await _db.SaveChangesAsync();
            return (true, "");
        }
        catch (DbUpdateException)
        {
            return (false, "Cannot delete: this record is referenced by other rows. Deactivate it instead.");
        }
    }

    private async Task<object?> Find(string kind, int id) => kind switch
    {
        "countries"            => await _db.Countries.FindAsync(id),
        "communities"          => await _db.Communities.FindAsync(id),
        "cities"               => await _db.Cities.FindAsync(id),
        "streets"              => await _db.Streets.FindAsync(id),
        "business-types"       => await _db.BusinessTypes.FindAsync(id),
        "registration-issuers" => await _db.RegistrationIssuers.FindAsync(id),
        "customer-vehicle-relation-types"  => await _db.CustomerVehicleRelationTypes.FindAsync(id),
        "vehicle-body-types"   => await _db.VehicleBodyTypes.FindAsync(id),
        "vehicle-categories"   => await _db.VehicleCategories.FindAsync(id),
        "vehicle-uses"         => await _db.VehicleUses.FindAsync(id),
        "vehicle-makers"       => await _db.VehicleMakers.FindAsync(id),
        "vehicle-models"       => await _db.VehicleModels.FindAsync(id),
        "vehicle-engine-types" => await _db.VehicleEngineTypes.FindAsync(id),
        "vehicle-engine-power-source-types" => await _db.VehicleEnginePowerSourceTypes.FindAsync(id),
        "vehicle-engine-eco-programs" => await _db.VehicleEngineEcoPrograms.FindAsync(id),
        "vehicle-gearboxes"    => await _db.VehicleGearBoxes.FindAsync(id),
        "vehicle-brakes"       => await _db.VehicleBrakes.FindAsync(id),
        "vehicle-supportings"  => await _db.VehicleSupportings.FindAsync(id),
        "colors"               => await _db.Colors.FindAsync(id),
        "vehicle-categories-for-payments" => await _db.VehicleCategoriesForPayments.FindAsync(id),
        "vehicle-tire-types"   => await _db.VehicleTireTypes.FindAsync(id),
        "technical-exam-organizations"  => await _db.TechnicalExamOrganizations.FindAsync(id),
        "technical-exam-types"          => await _db.TechnicalExamTypes.FindAsync(id),
        "technical-exam-vehicle-parts"  => await _db.TechnicalExamVehicleParts.FindAsync(id),
        "technical-exam-report-detail-statuses" => await _db.TechnicalExamReportDetailStatuses.FindAsync(id),
        "driving-licence-categories"    => await _db.DrivingLicenceCategories.FindAsync(id),
        "vehicle-ownership-proof-types" => await _db.VehicleOwnershipProofTypes.FindAsync(id),
        "payment-proof-types"           => await _db.PaymentProofTypes.FindAsync(id),
        _ => null,
    };
}
