using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Vehicles;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/customer-vehicle-relations")]
[Authorize]
public class CustomerVehicleRelationsController : ControllerBase
{
    private readonly VteDbContext _db;
    public CustomerVehicleRelationsController(VteDbContext db) => _db = db;

    public record RelationDto(long Id, long CustomerId, long VehicleId, int? RelationTypeId, DateOnly? ValidFrom, DateOnly? ValidTo, bool IsActive);
    public record CreateRelationRequest(long CustomerId, long VehicleId, int? RelationTypeId, DateOnly? ValidFrom, string? Note);

    [HttpGet]
    public async Task<ActionResult<List<RelationDto>>> List([FromQuery] long? customerId, [FromQuery] long? vehicleId)
    {
        var q = _db.CustomerVehicleRelations.AsQueryable();
        if (customerId is not null) q = q.Where(r => r.CustomerId == customerId);
        if (vehicleId is not null) q = q.Where(r => r.VehicleId == vehicleId);
        return await q.OrderByDescending(r => r.ValidFrom)
            .Select(r => new RelationDto(r.Id, r.CustomerId, r.VehicleId, r.RelationTypeId, r.ValidFrom, r.ValidTo, r.IsActive))
            .ToListAsync();
    }

    [HttpPost("ensure")]
    public async Task<ActionResult<RelationDto>> Ensure([FromQuery] long customerId, [FromQuery] long vehicleId)
    {
        var existing = await _db.CustomerVehicleRelations
            .Where(r => r.CustomerId == customerId && r.VehicleId == vehicleId && r.IsActive)
            .OrderByDescending(r => r.ValidFrom).FirstOrDefaultAsync();
        if (existing is not null)
            return Ok(new RelationDto(existing.Id, existing.CustomerId, existing.VehicleId, existing.RelationTypeId, existing.ValidFrom, existing.ValidTo, existing.IsActive));
        if (!await _db.Customers.AnyAsync(c => c.Id == customerId))
            return BadRequest(new { error = "Customer not found in current tenant." });
        if (!await _db.Vehicles.AnyAsync(v => v.Id == vehicleId))
            return BadRequest(new { error = "Vehicle not found in current tenant." });
        var r = new CustomerVehicleRelation { CustomerId = customerId, VehicleId = vehicleId, ValidFrom = DateOnly.FromDateTime(DateTime.UtcNow) };
        _db.CustomerVehicleRelations.Add(r);
        await _db.SaveChangesAsync();
        return Ok(new RelationDto(r.Id, r.CustomerId, r.VehicleId, r.RelationTypeId, r.ValidFrom, r.ValidTo, r.IsActive));
    }

    [HttpPost]
    public async Task<ActionResult<RelationDto>> Create(CreateRelationRequest req)
    {
        if (!await _db.Customers.AnyAsync(c => c.Id == req.CustomerId))
            return BadRequest(new { error = "Customer not found in current tenant." });
        if (!await _db.Vehicles.AnyAsync(v => v.Id == req.VehicleId))
            return BadRequest(new { error = "Vehicle not found in current tenant." });

        var r = new CustomerVehicleRelation
        {
            CustomerId = req.CustomerId,
            VehicleId = req.VehicleId,
            RelationTypeId = req.RelationTypeId,
            ValidFrom = req.ValidFrom ?? DateOnly.FromDateTime(DateTime.UtcNow),
            Note = req.Note
        };
        _db.CustomerVehicleRelations.Add(r);
        await _db.SaveChangesAsync();
        return Ok(new RelationDto(r.Id, r.CustomerId, r.VehicleId, r.RelationTypeId, r.ValidFrom, r.ValidTo, r.IsActive));
    }
}
