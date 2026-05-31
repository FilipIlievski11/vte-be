using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Requests;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

/// <summary>
/// File attachments on a Request. Stores file blobs on disk under a configurable
/// root (Storage:RequestAttachmentsRoot). Each blob is renamed to "{guid}{ext}"
/// to avoid filename collisions and path-traversal attempts. Metadata stays in
/// the database; deleting a row is soft-delete (Active=false) — the blob remains
/// on disk and can be swept by a separate housekeeping job later.
/// </summary>
[ApiController]
[Route("api/requests/{requestId:long}/attachments")]
[Authorize]
public class RequestAttachmentsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IConfiguration _cfg;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<RequestAttachmentsController> _log;

    public RequestAttachmentsController(
        VteDbContext db, ITenantContext tenant, IConfiguration cfg,
        IWebHostEnvironment env, ILogger<RequestAttachmentsController> log)
    {
        _db = db; _tenant = tenant; _cfg = cfg; _env = env; _log = log;
    }

    public record AttachmentDto(
        long Id, long RequestId, byte AttachmentTypeId, string? AttachmentTypeName,
        string FileName, string ContentType, long SizeBytes,
        DateTime UploadedAt, string? UploadedByUserName, bool Active);

    /// <summary>
    /// Multipart upload payload. Swashbuckle requires the IFormFile + sibling
    /// form fields to share a single DTO class (cannot use two separate
    /// [FromForm] parameters when one is an IFormFile).
    /// </summary>
    public class UploadFormDto
    {
        public IFormFile? File { get; set; }
        public byte AttachmentTypeId { get; set; }
    }

    private string StorageRoot
    {
        get
        {
            var raw = _cfg["Storage:RequestAttachmentsRoot"] ?? "./storage/request-attachments";
            return Path.IsPathRooted(raw) ? raw : Path.Combine(_env.ContentRootPath, raw);
        }
    }

    private long MaxBytes =>
        long.TryParse(_cfg["Storage:MaxAttachmentBytes"], out var v) ? v : 20L * 1024 * 1024;

    private HashSet<string> AllowedMimes =>
        new(_cfg.GetSection("Storage:AllowedAttachmentMimeTypes").Get<string[]>()
            ?? new[] { "image/jpeg", "image/png", "application/pdf" },
            StringComparer.OrdinalIgnoreCase);

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AttachmentDto>>> List(long requestId)
    {
        if (!await _db.Requests.AnyAsync(r => r.Id == requestId)) return NotFound();

        var rows = await _db.RequestAttachments.AsNoTracking()
            .Where(a => a.RequestId == requestId)
            .OrderByDescending(a => a.Id)
            .ToListAsync();

        var typeIds = rows.Select(a => a.AttachmentTypeId).Distinct().ToList();
        var typeNames = await _db.RequestAttachmentTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);

        var userIds = rows.Select(a => a.UploadedByUserId).Distinct().ToList();
        var users = await _db.Users.AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToDictionaryAsync(u => u.Id);

        string? userName(string id) =>
            users.TryGetValue(id, out var u) ? (u.FullName ?? u.UserName) : null;

        return Ok(rows.Select(a => new AttachmentDto(
            a.Id, a.RequestId, a.AttachmentTypeId,
            typeNames.GetValueOrDefault(a.AttachmentTypeId),
            a.FileName, a.ContentType, a.SizeBytes,
            a.UploadedAt, userName(a.UploadedByUserId), a.Active)).ToList());
    }

    [HttpPost]
    [RequestSizeLimit(50_000_000)]   // 50 MB hard ceiling at the framework level
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<AttachmentDto>> Upload(
        long requestId,
        [FromForm] UploadFormDto form)
    {
        if (_tenant.UserId is null) return Unauthorized();

        var file = form.File;
        var attachmentTypeId = form.AttachmentTypeId;

        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null)
            return BadRequest(new { error = "Завршеното барање не може да се менува." });

        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded." });
        if (file.Length > MaxBytes)
            return BadRequest(new { error = $"File too large (max {MaxBytes} bytes)." });
        if (!AllowedMimes.Contains(file.ContentType))
            return BadRequest(new { error = $"Disallowed content type: {file.ContentType}" });
        if (!await _db.RequestAttachmentTypes.AnyAsync(t => t.Id == attachmentTypeId))
            return BadRequest(new { error = "AttachmentTypeId not found." });

        // Bucket files by yyyy/mm/requestId so directory listings stay manageable
        var bucket = Path.Combine(
            DateTime.UtcNow.ToString("yyyy"),
            DateTime.UtcNow.ToString("MM"),
            requestId.ToString());
        var dir = Path.Combine(StorageRoot, bucket);
        Directory.CreateDirectory(dir);

        var ext = Path.GetExtension(file.FileName);
        var safeName = $"{Guid.NewGuid():N}{ext}";
        var diskPath = Path.Combine(dir, safeName);

        try
        {
            await using var stream = System.IO.File.Create(diskPath);
            await file.CopyToAsync(stream);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Failed to write attachment {Path}", diskPath);
            return StatusCode(500, new { error = "Failed to write file." });
        }

        var entity = new RequestAttachment
        {
            RequestId = requestId,
            AttachmentTypeId = attachmentTypeId,
            FileName = Path.GetFileName(file.FileName),  // strip path components
            ContentType = file.ContentType,
            SizeBytes = file.Length,
            StoragePath = Path.Combine(bucket, safeName).Replace('\\', '/'),
            UploadedAt = DateTime.UtcNow,
            UploadedByUserId = _tenant.UserId,
            Active = true,
        };
        _db.RequestAttachments.Add(entity);
        await _db.SaveChangesAsync();

        var typeName = await _db.RequestAttachmentTypes.AsNoTracking()
            .Where(t => t.Id == entity.AttachmentTypeId)
            .Select(t => t.Name).FirstOrDefaultAsync();

        return Ok(new AttachmentDto(
            entity.Id, entity.RequestId, entity.AttachmentTypeId, typeName,
            entity.FileName, entity.ContentType, entity.SizeBytes,
            entity.UploadedAt, null, entity.Active));
    }

    [HttpGet("{id:long}/download")]
    public async Task<IActionResult> Download(long requestId, long id)
    {
        var entity = await _db.RequestAttachments.AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id && a.RequestId == requestId);
        if (entity == null) return NotFound();

        var fullPath = Path.GetFullPath(Path.Combine(StorageRoot, entity.StoragePath));
        var root = Path.GetFullPath(StorageRoot);
        // Path-traversal guard: resolved file must live under StorageRoot
        if (!fullPath.StartsWith(root, StringComparison.OrdinalIgnoreCase))
            return BadRequest(new { error = "Invalid storage path." });
        if (!System.IO.File.Exists(fullPath))
            return NotFound(new { error = "File missing from disk." });

        var stream = System.IO.File.OpenRead(fullPath);
        return File(stream, entity.ContentType, entity.FileName);
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long requestId, long id)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
        if (request == null) return NotFound();
        if (request.EndedAt is not null)
            return BadRequest(new { error = "Завршеното барање не може да се менува." });

        var entity = await _db.RequestAttachments.FirstOrDefaultAsync(a => a.Id == id && a.RequestId == requestId);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
