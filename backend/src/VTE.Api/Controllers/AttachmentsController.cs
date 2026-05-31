using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Attachments;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// One controller covers all 5 attachment table types via a {kind} route param.
// Routes: POST   /api/attachments/{kind}/{parentId}     (multipart upload)
//         GET    /api/attachments/{kind}/{parentId}     (list metadata only)
//         GET    /api/attachments/{kind}/file/{id}      (download bytes)
//         DELETE /api/attachments/{kind}/file/{id}
[ApiController]
[Route("api/attachments")]
[Authorize]
public class AttachmentsController : ControllerBase
{
    private readonly VteDbContext _db;
    public AttachmentsController(VteDbContext db) => _db = db;

    public record AttachmentDto(long Id, string FileName, string ContentType, long FileSize, string? Description, DateTime CreatedUtc);

    [HttpGet("{kind}/{parentId:long}")]
    public async Task<ActionResult<List<AttachmentDto>>> List(string kind, long parentId)
        => kind switch
        {
            "customer" => Ok(await Project(_db.CustomerAttachments.Where(a => a.CustomerId == parentId))),
            "vehicle"  => Ok(await Project(_db.VehicleAttachments.Where(a => a.VehicleId == parentId))),
            "request"  => Ok(await Project(_db.RequestAttachments.Where(a => a.RequestId == parentId))),
            "exam"     => Ok(await Project(_db.TechnicalExamReportAttachments.Where(a => a.TechnicalExamReportId == parentId))),
            "payment"  => Ok(await Project(_db.PaymentDocumentAttachments.Where(a => a.PaymentDocumentId == parentId))),
            _ => BadRequest(new { error = $"Unknown attachment kind '{kind}'." }),
        };

    private static async Task<List<AttachmentDto>> Project<T>(IQueryable<T> q) where T : AttachmentBase
        => await q.OrderByDescending(a => a.CreatedUtc)
            .Select(a => new AttachmentDto(a.Id, a.FileName, a.ContentType, a.FileSize, a.Description, a.CreatedUtc))
            .ToListAsync();

    [HttpPost("{kind}/{parentId:long}")]
    [RequestSizeLimit(20 * 1024 * 1024)]                 // 20 MB cap
    public async Task<ActionResult<AttachmentDto>> Upload(string kind, long parentId, IFormFile file, [FromQuery] string? description)
    {
        if (file is null || file.Length == 0) return BadRequest(new { error = "Empty file." });
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        var bytes = ms.ToArray();
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        switch (kind)
        {
            case "customer":
                var ca = new CustomerAttachment { CustomerId = parentId, FileName = file.FileName, ContentType = file.ContentType, FileSize = bytes.Length, Content = bytes, Description = description, UploadedByUserId = userId };
                _db.CustomerAttachments.Add(ca); await _db.SaveChangesAsync();
                return Ok(new AttachmentDto(ca.Id, ca.FileName, ca.ContentType, ca.FileSize, ca.Description, ca.CreatedUtc));
            case "vehicle":
                var va = new VehicleAttachment { VehicleId = parentId, FileName = file.FileName, ContentType = file.ContentType, FileSize = bytes.Length, Content = bytes, Description = description, UploadedByUserId = userId };
                _db.VehicleAttachments.Add(va); await _db.SaveChangesAsync();
                return Ok(new AttachmentDto(va.Id, va.FileName, va.ContentType, va.FileSize, va.Description, va.CreatedUtc));
            case "request":
                var ra = new RequestAttachment { RequestId = parentId, FileName = file.FileName, ContentType = file.ContentType, FileSize = bytes.Length, Content = bytes, Description = description, UploadedByUserId = userId };
                _db.RequestAttachments.Add(ra); await _db.SaveChangesAsync();
                return Ok(new AttachmentDto(ra.Id, ra.FileName, ra.ContentType, ra.FileSize, ra.Description, ra.CreatedUtc));
            case "exam":
                var ea = new TechnicalExamReportAttachment { TechnicalExamReportId = parentId, FileName = file.FileName, ContentType = file.ContentType, FileSize = bytes.Length, Content = bytes, Description = description, UploadedByUserId = userId };
                _db.TechnicalExamReportAttachments.Add(ea); await _db.SaveChangesAsync();
                return Ok(new AttachmentDto(ea.Id, ea.FileName, ea.ContentType, ea.FileSize, ea.Description, ea.CreatedUtc));
            case "payment":
                var pa = new PaymentDocumentAttachment { PaymentDocumentId = parentId, FileName = file.FileName, ContentType = file.ContentType, FileSize = bytes.Length, Content = bytes, Description = description, UploadedByUserId = userId };
                _db.PaymentDocumentAttachments.Add(pa); await _db.SaveChangesAsync();
                return Ok(new AttachmentDto(pa.Id, pa.FileName, pa.ContentType, pa.FileSize, pa.Description, pa.CreatedUtc));
            default: return BadRequest(new { error = $"Unknown attachment kind '{kind}'." });
        }
    }

    [HttpGet("{kind}/file/{id:long}")]
    public async Task<IActionResult> Download(string kind, long id)
    {
        AttachmentBase? a = kind switch
        {
            "customer" => await _db.CustomerAttachments.FindAsync(id),
            "vehicle"  => await _db.VehicleAttachments.FindAsync(id),
            "request"  => await _db.RequestAttachments.FindAsync(id),
            "exam"     => await _db.TechnicalExamReportAttachments.FindAsync(id),
            "payment"  => await _db.PaymentDocumentAttachments.FindAsync(id),
            _ => null,
        };
        if (a is null) return NotFound();
        return File(a.Content, a.ContentType, a.FileName);
    }

    [HttpDelete("{kind}/file/{id:long}")]
    public async Task<IActionResult> Delete(string kind, long id)
    {
        switch (kind)
        {
            case "customer": { var x = await _db.CustomerAttachments.FindAsync(id);            if (x is null) return NotFound(); _db.Remove(x); break; }
            case "vehicle":  { var x = await _db.VehicleAttachments.FindAsync(id);             if (x is null) return NotFound(); _db.Remove(x); break; }
            case "request":  { var x = await _db.RequestAttachments.FindAsync(id);             if (x is null) return NotFound(); _db.Remove(x); break; }
            case "exam":     { var x = await _db.TechnicalExamReportAttachments.FindAsync(id); if (x is null) return NotFound(); _db.Remove(x); break; }
            case "payment":  { var x = await _db.PaymentDocumentAttachments.FindAsync(id);    if (x is null) return NotFound(); _db.Remove(x); break; }
            default: return BadRequest(new { error = $"Unknown attachment kind '{kind}'." });
        }
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
