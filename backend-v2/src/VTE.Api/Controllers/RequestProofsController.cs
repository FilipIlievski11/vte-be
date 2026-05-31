using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Requests;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// CRUD for the two proof child collections of a <see cref="Request"/>:
///   - Ownership proofs   (e.g. sales contract, inheritance ruling)
///   - Payment proofs     (e.g. fiscal receipt, bank transfer slip)
/// Mounted as nested routes under /api/requests/{requestId}. The parent request's
/// tenant filter implicitly scopes who can see / mutate these children — if the
/// caller can't see the parent, they can't see the proofs.
/// </summary>
[ApiController]
[Authorize]
public class RequestProofsController : ControllerBase
{
    private readonly VteDbContext _db;
    public RequestProofsController(VteDbContext db) => _db = db;

    public record OwnershipProofDto(long Id, long RequestId, byte OwnershipProofTypeId,
        string? OwnershipProofTypeName, string? Detail, bool Active);
    public record OwnershipProofWriteDto(byte OwnershipProofTypeId, string? Detail, bool Active = true);

    public record PaymentProofDto(long Id, long RequestId, byte PaymentProofTypeId,
        string? PaymentProofTypeName, string? Detail, bool Active);
    public record PaymentProofWriteDto(byte PaymentProofTypeId, string? Detail, bool Active = true);

    // ---------- Ownership proofs ----------

    [HttpGet("api/requests/{requestId:long}/ownership-proofs")]
    public async Task<ActionResult<IReadOnlyList<OwnershipProofDto>>> ListOwnership(long requestId)
    {
        if (!await _db.Requests.AnyAsync(r => r.Id == requestId)) return NotFound();

        var rows = await _db.RequestOwnershipProofs.AsNoTracking()
            .Where(p => p.RequestId == requestId)
            .OrderBy(p => p.Id)
            .ToListAsync();

        var typeIds = rows.Select(p => p.OwnershipProofTypeId).Distinct().ToList();
        var types = await _db.RequestOwnershipProofTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);

        return Ok(rows.Select(p => new OwnershipProofDto(
            p.Id, p.RequestId, p.OwnershipProofTypeId,
            types.GetValueOrDefault(p.OwnershipProofTypeId),
            p.Detail, p.Active)).ToList());
    }

    [HttpPost("api/requests/{requestId:long}/ownership-proofs")]
    public async Task<ActionResult<OwnershipProofDto>> AddOwnership(long requestId, [FromBody] OwnershipProofWriteDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        if (!await _db.RequestOwnershipProofTypes.AnyAsync(t => t.Id == dto.OwnershipProofTypeId))
            return BadRequest(new { error = "OwnershipProofTypeId not found." });

        var entity = new RequestOwnershipProof
        {
            RequestId = requestId,
            OwnershipProofTypeId = dto.OwnershipProofTypeId,
            Detail = dto.Detail,
            Active = dto.Active,
        };
        _db.RequestOwnershipProofs.Add(entity);
        await _db.SaveChangesAsync();

        var typeName = await _db.RequestOwnershipProofTypes.AsNoTracking()
            .Where(t => t.Id == entity.OwnershipProofTypeId)
            .Select(t => t.Name).FirstOrDefaultAsync();

        return Ok(new OwnershipProofDto(
            entity.Id, entity.RequestId, entity.OwnershipProofTypeId, typeName, entity.Detail, entity.Active));
    }

    [HttpPut("api/requests/{requestId:long}/ownership-proofs/{id:long}")]
    public async Task<IActionResult> UpdateOwnership(long requestId, long id, [FromBody] OwnershipProofWriteDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        var entity = await _db.RequestOwnershipProofs.FirstOrDefaultAsync(p => p.Id == id && p.RequestId == requestId);
        if (entity == null) return NotFound();

        if (!await _db.RequestOwnershipProofTypes.AnyAsync(t => t.Id == dto.OwnershipProofTypeId))
            return BadRequest(new { error = "OwnershipProofTypeId not found." });

        entity.OwnershipProofTypeId = dto.OwnershipProofTypeId;
        entity.Detail = dto.Detail;
        entity.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/requests/{requestId:long}/ownership-proofs/{id:long}")]
    public async Task<IActionResult> DeleteOwnership(long requestId, long id)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        var entity = await _db.RequestOwnershipProofs.FirstOrDefaultAsync(p => p.Id == id && p.RequestId == requestId);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---------- Payment proofs ----------

    [HttpGet("api/requests/{requestId:long}/payment-proofs")]
    public async Task<ActionResult<IReadOnlyList<PaymentProofDto>>> ListPayment(long requestId)
    {
        if (!await _db.Requests.AnyAsync(r => r.Id == requestId)) return NotFound();

        var rows = await _db.RequestPaymentProofs.AsNoTracking()
            .Where(p => p.RequestId == requestId)
            .OrderBy(p => p.Id)
            .ToListAsync();

        var typeIds = rows.Select(p => p.PaymentProofTypeId).Distinct().ToList();
        var types = await _db.RequestPaymentProofTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);

        return Ok(rows.Select(p => new PaymentProofDto(
            p.Id, p.RequestId, p.PaymentProofTypeId,
            types.GetValueOrDefault(p.PaymentProofTypeId),
            p.Detail, p.Active)).ToList());
    }

    [HttpPost("api/requests/{requestId:long}/payment-proofs")]
    public async Task<ActionResult<PaymentProofDto>> AddPayment(long requestId, [FromBody] PaymentProofWriteDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        if (!await _db.RequestPaymentProofTypes.AnyAsync(t => t.Id == dto.PaymentProofTypeId))
            return BadRequest(new { error = "PaymentProofTypeId not found." });

        var entity = new RequestPaymentProof
        {
            RequestId = requestId,
            PaymentProofTypeId = dto.PaymentProofTypeId,
            Detail = dto.Detail,
            Active = dto.Active,
        };
        _db.RequestPaymentProofs.Add(entity);
        await _db.SaveChangesAsync();

        var typeName = await _db.RequestPaymentProofTypes.AsNoTracking()
            .Where(t => t.Id == entity.PaymentProofTypeId)
            .Select(t => t.Name).FirstOrDefaultAsync();

        return Ok(new PaymentProofDto(
            entity.Id, entity.RequestId, entity.PaymentProofTypeId, typeName, entity.Detail, entity.Active));
    }

    [HttpPut("api/requests/{requestId:long}/payment-proofs/{id:long}")]
    public async Task<IActionResult> UpdatePayment(long requestId, long id, [FromBody] PaymentProofWriteDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        var entity = await _db.RequestPaymentProofs.FirstOrDefaultAsync(p => p.Id == id && p.RequestId == requestId);
        if (entity == null) return NotFound();

        if (!await _db.RequestPaymentProofTypes.AnyAsync(t => t.Id == dto.PaymentProofTypeId))
            return BadRequest(new { error = "PaymentProofTypeId not found." });

        entity.PaymentProofTypeId = dto.PaymentProofTypeId;
        entity.Detail = dto.Detail;
        entity.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/requests/{requestId:long}/payment-proofs/{id:long}")]
    public async Task<IActionResult> DeletePayment(long requestId, long id)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null) return BadRequest(new { error = "Завршеното барање не може да се менува." });

        var entity = await _db.RequestPaymentProofs.FirstOrDefaultAsync(p => p.Id == id && p.RequestId == requestId);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
