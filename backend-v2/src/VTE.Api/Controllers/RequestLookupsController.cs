using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Identity;
using VTE.Domain.Requests;
using VTE.Infrastructure.Persistence;

namespace VTE.Api.Controllers;

/// <summary>
/// CRUD for the simple Request catalog tables:
///   - RequestDocumentPrint   (Plav / Bel / Zelen print templates)
///   - RequestOwnershipProofType
///   - RequestPaymentProofType
///   - RequestAttachmentType
/// Reads are visible to all authenticated users (operators populate dropdowns
/// from these). Writes are Administrator-only.
/// </summary>
[ApiController]
[Authorize]
public class RequestLookupsController : ControllerBase
{
    private readonly VteDbContext _db;
    public RequestLookupsController(VteDbContext db) => _db = db;

    // ---------- RequestDocumentPrint -------------------------------------

    [HttpGet("api/request-document-prints")]
    public async Task<IActionResult> ListDocumentPrints() =>
        Ok(await _db.RequestDocumentPrints.AsNoTracking()
            .OrderBy(x => x.Id).ToListAsync());

    [HttpGet("api/request-document-prints/{id:int}")]
    public async Task<IActionResult> GetDocumentPrint(int id)
    {
        var x = await _db.RequestDocumentPrints.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == (byte)id);
        return x == null ? NotFound() : Ok(x);
    }

    [HttpPost("api/request-document-prints")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreateDocumentPrint([FromBody] RequestDocumentPrint dto)
    {
        dto.Id = 0;
        _db.RequestDocumentPrints.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDocumentPrint), new { id = dto.Id }, dto);
    }

    [HttpPut("api/request-document-prints/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> UpdateDocumentPrint(int id, [FromBody] RequestDocumentPrint dto)
    {
        var x = await _db.RequestDocumentPrints.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Code = dto.Code;
        x.Name = dto.Name;
        x.TemplatePath = dto.TemplatePath;
        x.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/request-document-prints/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeleteDocumentPrint(int id)
    {
        var x = await _db.RequestDocumentPrints.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---------- RequestOwnershipProofType --------------------------------

    [HttpGet("api/request-ownership-proof-types")]
    public async Task<IActionResult> ListOwnershipProofTypes() =>
        Ok(await _db.RequestOwnershipProofTypes.AsNoTracking()
            .OrderBy(x => x.Name).ToListAsync());

    [HttpGet("api/request-ownership-proof-types/{id:int}")]
    public async Task<IActionResult> GetOwnershipProofType(int id)
    {
        var x = await _db.RequestOwnershipProofTypes.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == (byte)id);
        return x == null ? NotFound() : Ok(x);
    }

    [HttpPost("api/request-ownership-proof-types")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreateOwnershipProofType([FromBody] RequestOwnershipProofType dto)
    {
        dto.Id = 0;
        _db.RequestOwnershipProofTypes.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOwnershipProofType), new { id = dto.Id }, dto);
    }

    [HttpPut("api/request-ownership-proof-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> UpdateOwnershipProofType(int id, [FromBody] RequestOwnershipProofType dto)
    {
        var x = await _db.RequestOwnershipProofTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Name = dto.Name;
        x.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/request-ownership-proof-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeleteOwnershipProofType(int id)
    {
        var x = await _db.RequestOwnershipProofTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---------- RequestPaymentProofType ----------------------------------

    [HttpGet("api/request-payment-proof-types")]
    public async Task<IActionResult> ListPaymentProofTypes() =>
        Ok(await _db.RequestPaymentProofTypes.AsNoTracking()
            .OrderBy(x => x.Name).ToListAsync());

    [HttpGet("api/request-payment-proof-types/{id:int}")]
    public async Task<IActionResult> GetPaymentProofType(int id)
    {
        var x = await _db.RequestPaymentProofTypes.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == (byte)id);
        return x == null ? NotFound() : Ok(x);
    }

    [HttpPost("api/request-payment-proof-types")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreatePaymentProofType([FromBody] RequestPaymentProofType dto)
    {
        dto.Id = 0;
        _db.RequestPaymentProofTypes.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPaymentProofType), new { id = dto.Id }, dto);
    }

    [HttpPut("api/request-payment-proof-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> UpdatePaymentProofType(int id, [FromBody] RequestPaymentProofType dto)
    {
        var x = await _db.RequestPaymentProofTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Name = dto.Name;
        x.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/request-payment-proof-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeletePaymentProofType(int id)
    {
        var x = await _db.RequestPaymentProofTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---------- RequestAttachmentType ------------------------------------

    [HttpGet("api/request-attachment-types")]
    public async Task<IActionResult> ListAttachmentTypes() =>
        Ok(await _db.RequestAttachmentTypes.AsNoTracking()
            .OrderBy(x => x.Name).ToListAsync());

    [HttpGet("api/request-attachment-types/{id:int}")]
    public async Task<IActionResult> GetAttachmentType(int id)
    {
        var x = await _db.RequestAttachmentTypes.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == (byte)id);
        return x == null ? NotFound() : Ok(x);
    }

    [HttpPost("api/request-attachment-types")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> CreateAttachmentType([FromBody] RequestAttachmentType dto)
    {
        dto.Id = 0;
        _db.RequestAttachmentTypes.Add(dto);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAttachmentType), new { id = dto.Id }, dto);
    }

    [HttpPut("api/request-attachment-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> UpdateAttachmentType(int id, [FromBody] RequestAttachmentType dto)
    {
        var x = await _db.RequestAttachmentTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Name = dto.Name;
        x.Active = dto.Active;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("api/request-attachment-types/{id:int}")]
    [Authorize(Roles = Roles.Administrator)]
    public async Task<IActionResult> DeleteAttachmentType(int id)
    {
        var x = await _db.RequestAttachmentTypes.FirstOrDefaultAsync(r => r.Id == (byte)id);
        if (x == null) return NotFound();
        x.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
