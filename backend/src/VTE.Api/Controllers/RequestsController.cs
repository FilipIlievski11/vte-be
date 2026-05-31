using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Requests;
using VTE.Domain.Vehicles;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/request-types")]
[Authorize]
public class RequestTypesController : ControllerBase
{
    private readonly VteDbContext _db;
    public RequestTypesController(VteDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<RequestType>>> List()
        => await _db.RequestTypes.Where(t => t.IsActive).OrderBy(t => t.TypeName).ToListAsync();
}

[ApiController]
[Route("api/requests")]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public RequestsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record RequestListDto(
        long Id, int RequestTypeId, DateOnly DateCreated, DateOnly? DateEnded, string? Note,
        string? CustomerName, string? VehicleVin, string? VehicleReg);

    public record CreateRequestRequest(
        int RequestTypeId,
        long? CustomerVehicleRelationId,            // legacy path: pass the relation directly
        long? CustomerId,                           // OR pass customer + vehicle and we resolve / create the CVR
        long? VehicleId,
        int?  RelationTypeId,                       // used when creating a new CVR
        long? NewCustomerVehicleRelationId,         // either pass the relation id directly...
        long? NewCustomerId,                        // ...or pass the new customer + we'll resolve/create the CVR against the same vehicle
        long? PreviousRegistrationId,
        int?  TechnicalExamOrganizationId,
        string? Note,
        IReadOnlyList<OwnershipProofDto>? OwnershipProofs = null,   // attached "доказ за потеклото на возилото" rows
        IReadOnlyList<PaymentProofDto>? PaymentProofs = null);      // attached "потврда за платени давачки" rows

    public record EndRequestRequest(bool IsCustomerChanged, bool IsVehicleChanged);

    public record OwnershipProofDto(long? Id, int? OwnershipProofId, string? Number, DateOnly? DateIssued, string? Note);
    public record PaymentProofDto(long? Id, int? PaymentProofId, string? Number, decimal? Amount, DateOnly? DateIssued, string? Note);

    public record RequestDetailDto(
        Request Request,
        long? CustomerId, long? VehicleId,
        string? CustomerName, string? VehicleVin, string? VehicleReg,
        IReadOnlyList<OwnershipProofDto> OwnershipProofs,
        IReadOnlyList<PaymentProofDto> PaymentProofs);

    public record UpdateRequestRequest(
        int? RelationTypeId, long? NewCustomerVehicleRelationId, long? PreviousRegistrationId,
        int? TechnicalExamOrganizationId, string? Note,
        IReadOnlyList<OwnershipProofDto>? OwnershipProofs,
        IReadOnlyList<PaymentProofDto>? PaymentProofs);

    [HttpGet]
    public async Task<ActionResult<PagedResult<RequestListDto>>> List(
        [FromQuery] bool? open, [FromQuery] int? page, [FromQuery] int? pageSize,
        [FromQuery] string? sortBy, [FromQuery] string? sortDir)
    {
        var q = _db.Requests.AsNoTracking().AsQueryable();
        if (open == true)  q = q.Where(r => r.DateEnded == null);
        if (open == false) q = q.Where(r => r.DateEnded != null);

        var rows =
            from r in q
            join rel in _db.CustomerVehicleRelations on r.CustomerVehicleRelationId equals rel.Id into rj
            from rel in rj.DefaultIfEmpty()
            join c in _db.Customers on rel.CustomerId equals c.Id into cj
            from c in cj.DefaultIfEmpty()
            join v in _db.Vehicles on rel.VehicleId equals v.Id into vj
            from v in vj.DefaultIfEmpty()
            select new {
                r.Id, r.RequestTypeId, r.DateCreated, r.DateEnded, r.Note,
                CustomerName = c == null ? null : ((c.Surname ?? "") + " " + c.FirstName).Trim(),
                VehicleVin   = v == null ? null : v.ShellNumber,
                VehicleReg   = v == null ? null : v.LastRegistrationNumber,
            };

        var desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);
        rows = (sortBy?.ToLowerInvariant()) switch {
            "datecreated" => desc ? rows.OrderByDescending(x => x.DateCreated) : rows.OrderBy(x => x.DateCreated),
            "customername"=> desc ? rows.OrderByDescending(x => x.CustomerName) : rows.OrderBy(x => x.CustomerName),
            "vehiclereg"  => desc ? rows.OrderByDescending(x => x.VehicleReg)  : rows.OrderBy(x => x.VehicleReg),
            "id"          => desc ? rows.OrderByDescending(x => x.Id)          : rows.OrderBy(x => x.Id),
            _             => rows.OrderByDescending(x => x.DateCreated),
        };

        var dtoQuery = rows.Select(x => new RequestListDto(
            x.Id, x.RequestTypeId, x.DateCreated, x.DateEnded, x.Note,
            x.CustomerName, x.VehicleVin, x.VehicleReg));

        return Ok(await dtoQuery.ToPagedAsync(page, pageSize));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RequestDetailDto>> Get(long id)
    {
        var r = await _db.Requests.AsNoTracking().Include(x => x.RequestType).FirstOrDefaultAsync(x => x.Id == id);
        if (r is null) return NotFound();
        return Ok(await BuildDetailAsync(r));
    }

    [HttpPut("{id:long}")]
    public async Task<ActionResult<RequestDetailDto>> Update(long id, UpdateRequestRequest req)
    {
        var r = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        if (r is null) return NotFound();
        if (r.DateEnded is not null) return BadRequest(new { error = "Cannot edit an ended request." });

        r.NewCustomerVehicleRelationId = req.NewCustomerVehicleRelationId;
        r.PreviousRegistrationId       = req.PreviousRegistrationId;
        r.TechnicalExamOrganizationId  = req.TechnicalExamOrganizationId;
        r.Note                         = req.Note;
        r.DateModified                 = DateOnly.FromDateTime(DateTime.UtcNow);
        r.ModifiedByOperatorUserId     = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _db.SaveChangesAsync();
        await ReplaceProofsAsync(id, req.OwnershipProofs, req.PaymentProofs);
        return Ok(await BuildDetailAsync(r));
    }

    [HttpGet("{id:long}/print/{kind}")]
    public async Task<IActionResult> Print(long id, string kind, CancellationToken ct)
    {
        if (!Enum.TryParse<Pdf.RequestPrint.OverlayRenderer.Kind>(kind, ignoreCase: true, out var k))
            return BadRequest(new { error = "Unknown print kind. Use plav, bel or zelen." });

        var loader = new Pdf.RequestPrint.PrintDataLoader(_db);
        var data = await loader.LoadAsync(id, ct);
        if (data is null) return NotFound();

        var margins = await ResolveMarginsAsync(k, ct);
        var bytes = new Pdf.RequestPrint.OverlayRenderer().Render(k, data, margins);
        return File(bytes, "application/pdf", $"request-{id}-{kind.ToLowerInvariant()}.pdf");
    }

    private Task<Pdf.RequestPrint.OverlayMargins> ResolveMarginsAsync(Pdf.RequestPrint.OverlayRenderer.Kind k, CancellationToken ct)
    {
        // TODO: read per-station Plav/Bel/Zelen Top/Left/Right/Bottom margins from a settings table
        //       once Station settings are wired (legacy objOpcii.{Plav,Bel,Zelen}{Top,Left,Right,Bottom}Margin).
        //       For now we default to zero so the layout aligns to the legacy designer origin.
        return Task.FromResult(Pdf.RequestPrint.OverlayMargins.Zero);
    }

    private async Task<RequestDetailDto> BuildDetailAsync(Request r)
    {
        var rel = await _db.CustomerVehicleRelations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == r.CustomerVehicleRelationId);
        string? custName = null, vin = null, reg = null;
        long? custId = rel?.CustomerId, vehId = rel?.VehicleId;
        if (rel != null)
        {
            custName = await _db.Customers.AsNoTracking().Where(c => c.Id == rel.CustomerId)
                .Select(c => ((c.Surname ?? "") + " " + c.FirstName).Trim()).FirstOrDefaultAsync();
            var v = await _db.Vehicles.AsNoTracking().Where(x => x.Id == rel.VehicleId)
                .Select(x => new { x.ShellNumber, x.LastRegistrationNumber }).FirstOrDefaultAsync();
            vin = v?.ShellNumber; reg = v?.LastRegistrationNumber;
        }
        var owns = await _db.RequestVehicleOwnershipProofs.AsNoTracking()
            .Where(p => p.RequestId == r.Id).OrderBy(p => p.Id)
            .Select(p => new OwnershipProofDto(p.Id, p.OwnershipProofId, p.Number, p.DateIssued, p.Note))
            .ToListAsync();
        var pays = await _db.RequestPaymentProofs.AsNoTracking()
            .Where(p => p.RequestId == r.Id).OrderBy(p => p.Id)
            .Select(p => new PaymentProofDto(p.Id, p.PaymentProofId, p.Number, p.Amount, p.DateIssued, p.Note))
            .ToListAsync();
        return new RequestDetailDto(r, custId, vehId, custName, vin, reg, owns, pays);
    }

    private async Task ReplaceProofsAsync(long requestId, IReadOnlyList<OwnershipProofDto>? owns, IReadOnlyList<PaymentProofDto>? pays)
    {
        if (owns is not null)
        {
            _db.RequestVehicleOwnershipProofs.RemoveRange(_db.RequestVehicleOwnershipProofs.Where(p => p.RequestId == requestId));
            foreach (var p in owns.Where(x => x.OwnershipProofId is not null))
                _db.RequestVehicleOwnershipProofs.Add(new RequestVehicleOwnershipProof
                {
                    RequestId = requestId, OwnershipProofId = p.OwnershipProofId,
                    Number = Trim(p.Number), DateIssued = p.DateIssued, Note = Trim(p.Note),
                });
        }
        if (pays is not null)
        {
            _db.RequestPaymentProofs.RemoveRange(_db.RequestPaymentProofs.Where(p => p.RequestId == requestId));
            foreach (var p in pays.Where(x => x.PaymentProofId is not null))
                _db.RequestPaymentProofs.Add(new RequestPaymentProof
                {
                    RequestId = requestId, PaymentProofId = p.PaymentProofId,
                    Number = Trim(p.Number), Amount = p.Amount, DateIssued = p.DateIssued, Note = Trim(p.Note),
                });
        }
        await _db.SaveChangesAsync();
    }

    private static string? Trim(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    [HttpPost]
    public async Task<ActionResult<RequestListDto>> Create(CreateRequestRequest req)
    {
        var rt = await _db.RequestTypes.FindAsync(req.RequestTypeId);
        if (rt is null) return BadRequest(new { error = "RequestType not found." });

        if (rt.IsNewCustomer && req.NewCustomerVehicleRelationId is null && req.NewCustomerId is null)
            return BadRequest(new { error = "Потребно е да се внесе новиот сопственик (a new owner is required for this request type)." });
        if (rt.IsPreviousRegistrationRequired && req.PreviousRegistrationId is null)
            return BadRequest(new { error = "Потребно е да се избере претходна регистрација (a previous registration is required)." });

        var stationId = _tenant.StationId
            ?? await _db.Stations.Where(s => s.IsActive).OrderBy(s => s.Id).Select(s => (int?)s.Id).FirstOrDefaultAsync();
        if (stationId is null) return BadRequest(new { error = "No Station configured." });

        // Resolve CustomerVehicleRelationId from either the explicit id or the (customerId, vehicleId) pair.
        var cvrId = req.CustomerVehicleRelationId;
        if (cvrId is null)
        {
            if (req.CustomerId is null || req.VehicleId is null)
                return BadRequest(new { error = "Provide either CustomerVehicleRelationId or both CustomerId and VehicleId." });

            // verify the customer + vehicle exist within the tenant filter
            var customerExists = await _db.Customers.AnyAsync(c => c.Id == req.CustomerId);
            var vehicleExists  = await _db.Vehicles.AnyAsync(v => v.Id == req.VehicleId);
            if (!customerExists) return BadRequest(new { error = "Customer not found." });
            if (!vehicleExists)  return BadRequest(new { error = "Vehicle not found." });

            // try to find an existing CVR for this pair (any relation type, any time)
            var existing = await _db.CustomerVehicleRelations
                .Where(x => x.CustomerId == req.CustomerId && x.VehicleId == req.VehicleId && x.IsActive)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => (long?)x.Id).FirstOrDefaultAsync();

            if (existing is not null)
            {
                cvrId = existing;
            }
            else
            {
                var newCvr = new CustomerVehicleRelation
                {
                    CustomerId = req.CustomerId.Value,
                    VehicleId  = req.VehicleId.Value,
                    RelationTypeId = req.RelationTypeId,
                    ValidFrom = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = true,
                };
                _db.CustomerVehicleRelations.Add(newCvr);
                await _db.SaveChangesAsync();
                cvrId = newCvr.Id;
            }
        }

        // Resolve NewCustomerVehicleRelationId from either the explicit id or the new-customer id
        // (against the same vehicle). When IsNewCustomer is true this represents the transfer target.
        var newCvrId = req.NewCustomerVehicleRelationId;
        if (newCvrId is null && req.NewCustomerId is not null && req.VehicleId is not null)
        {
            var existingNew = await _db.CustomerVehicleRelations
                .Where(x => x.CustomerId == req.NewCustomerId && x.VehicleId == req.VehicleId)
                .OrderByDescending(x => x.ValidFrom)
                .Select(x => (long?)x.Id).FirstOrDefaultAsync();
            if (existingNew is not null) newCvrId = existingNew;
            else
            {
                var newOwnerCvr = new CustomerVehicleRelation
                {
                    CustomerId = req.NewCustomerId.Value,
                    VehicleId  = req.VehicleId.Value,
                    RelationTypeId = req.RelationTypeId,
                    ValidFrom = DateOnly.FromDateTime(DateTime.UtcNow),
                    IsActive = false,        // activated when the request is ended
                };
                _db.CustomerVehicleRelations.Add(newOwnerCvr);
                await _db.SaveChangesAsync();
                newCvrId = newOwnerCvr.Id;
            }
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var r = new Request
        {
            StationId = stationId.Value,
            RequestTypeId = req.RequestTypeId,
            CustomerVehicleRelationId = cvrId.Value,
            NewCustomerVehicleRelationId = newCvrId,
            PreviousRegistrationId = req.PreviousRegistrationId,
            TechnicalExamOrganizationId = req.TechnicalExamOrganizationId,
            DateCreated = DateOnly.FromDateTime(DateTime.UtcNow),
            CreatedByOperatorUserId = userId,
            Note = req.Note,
        };
        _db.Requests.Add(r);
        await _db.SaveChangesAsync();

        // Persist the attached proofs in the same call so the create-page form can save them together.
        await ReplaceProofsAsync(r.Id, req.OwnershipProofs, req.PaymentProofs);

        // build the return dto
        var dto = await (
            from rel in _db.CustomerVehicleRelations
            where rel.Id == cvrId
            join c in _db.Customers on rel.CustomerId equals c.Id
            join v in _db.Vehicles  on rel.VehicleId  equals v.Id
            select new RequestListDto(
                r.Id, r.RequestTypeId, r.DateCreated, r.DateEnded, r.Note,
                ((c.Surname ?? "") + " " + c.FirstName).Trim(), v.ShellNumber, v.LastRegistrationNumber)
        ).FirstOrDefaultAsync();

        return CreatedAtAction(nameof(Get), new { id = r.Id }, dto);
    }

    [HttpPost("{id:long}/end")]
    public async Task<IActionResult> End(long id, EndRequestRequest req)
    {
        var r = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        if (r is null) return NotFound();
        if (r.DateEnded is not null) return BadRequest(new { error = "Request already ended." });

        r.DateEnded = DateOnly.FromDateTime(DateTime.UtcNow);
        r.EndedByOperatorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        r.IsCustomerChanged = req.IsCustomerChanged;
        r.IsVehicleChanged = req.IsVehicleChanged;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
