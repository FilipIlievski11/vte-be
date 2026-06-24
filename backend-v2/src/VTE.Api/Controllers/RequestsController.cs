using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Api.Dtos;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Domain.Requests;
using VTE.Domain.TechnicalExams;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Persistence;
using VTE.Infrastructure.Pricing;
using VTE.Infrastructure.Tenancy;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/requests")]
[Authorize]
public class RequestsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;
    private readonly IDebtService _debts;
    private readonly IConfiguration _config;

    public RequestsController(VteDbContext db, ITenantContext tenant, IDebtService debts, IConfiguration config)
    {
        _db = db;
        _tenant = tenant;
        _debts = debts;
        _config = config;
    }

    /// <summary>
    /// Paged list of Requests. Status: "open" = EndedAt IS NULL, "closed" = EndedAt IS NOT NULL,
    /// "all" / null = both. Operators see only their company's requests (tenant filter).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedDto<RequestListItem>>> List(
        [FromQuery] string? q = null,
        [FromQuery] string? status = "open",
        [FromQuery] byte? companyId = null,
        [FromQuery] long? clientVehicleRelationId = null,
        [FromQuery] bool includeInactive = false,
        [FromQuery] string? sort = null,
        [FromQuery] string? dir = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 50;

        var query = _db.Requests.AsNoTracking().AsQueryable();

        if (!includeInactive) query = query.Where(r => r.Active);
        if (companyId.HasValue) query = query.Where(r => r.CompanyId == companyId.Value);
        if (clientVehicleRelationId.HasValue)
            query = query.Where(r => r.ClientVehicleRelationId == clientVehicleRelationId.Value);

        var s = (status ?? "open").Trim().ToLowerInvariant();
        if (s == "open")        query = query.Where(r => r.EndedAt == null);
        else if (s == "closed") query = query.Where(r => r.EndedAt != null);

        // Search: pre-resolve matching relation/vehicle/type ids via small subqueries
        // so we don't blow EF's nullable-join translation or the 2100-parameter limit.
        if (!string.IsNullOrWhiteSpace(q))
        {
            var qTrim = q.Trim();
            var like = $"%{qTrim}%";
            // Request number: exact match on the Id (the "#" shown in the list) plus a
            // partial match, and the legacy reference number (e.g. "168077128/2026").
            long qIdExact = long.TryParse(qTrim, out var parsedId) ? parsedId : -1;

            // Matching client display name → relation ids (via Client join)
            var matchingClientIds = _db.Clients.AsNoTracking()
                .Where(c =>
                    (c.FirstName != null && EF.Functions.Like(c.FirstName, like)) ||
                    (c.MiddleName != null && EF.Functions.Like(c.MiddleName, like)) ||
                    (c.LastName != null && EF.Functions.Like(c.LastName, like)) ||
                    (c.MB != null && EF.Functions.Like(c.MB, like)))
                .Select(c => c.Id);
            var relationsByClient = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => matchingClientIds.Contains(r.ClientId))
                .Select(r => r.Id);

            // Matching vehicle VIN/plate → relation ids
            var matchingVehicleIds = _db.Vehicles.AsNoTracking()
                .Where(v =>
                    EF.Functions.Like(v.Vin, like) ||
                    (v.Plate != null && EF.Functions.Like(v.Plate, like)))
                .Select(v => v.Id);
            var relationsByVehicle = _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.VehicleId != null && matchingVehicleIds.Contains(r.VehicleId!.Value))
                .Select(r => r.Id);

            // Matching request-type names
            var matchingTypeIds = _db.RequestTypes.AsNoTracking()
                .Where(t => EF.Functions.Like(t.Name, like))
                .Select(t => t.Id);

            var unionRelationIds = relationsByClient.Union(relationsByVehicle);

            query = query.Where(r =>
                r.Id == qIdExact ||
                EF.Functions.Like(r.Id.ToString(), like) ||
                (r.LegacyReferenceNumber != null && EF.Functions.Like(r.LegacyReferenceNumber, like)) ||
                unionRelationIds.Contains(r.ClientVehicleRelationId) ||
                matchingTypeIds.Contains(r.RequestTypeId));
        }

        var total = await query.CountAsync();

        // Sorting. Default newest-first. Join-based columns (type/client/vehicle/
        // operator) sort via correlated subqueries so the order spans the whole
        // filtered set, not just the current page.
        var asc = string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase);
        IQueryable<Request> Order<TKey>(System.Linq.Expressions.Expression<Func<Request, TKey>> key)
            => asc ? query.OrderBy(key) : query.OrderByDescending(key);
        var ordered = (sort ?? "").Trim().ToLowerInvariant() switch
        {
            "id"       => Order(r => r.Id),
            "created"  => Order(r => r.CreatedAt),
            "status"   => Order(r => r.EndedAt),
            "type"     => Order(r => _db.RequestTypes.Where(t => t.Id == r.RequestTypeId).Select(t => (string?)t.Name).FirstOrDefault()),
            "operator" => Order(r => _db.Users.Where(u => u.Id == r.CreatedByUserId).Select(u => u.UserName).FirstOrDefault()),
            "client"   => Order(r => _db.ClientVehicleRelations.Where(cvr => cvr.Id == r.ClientVehicleRelationId)
                                       .Select(cvr => _db.Clients.Where(c => c.Id == cvr.ClientId).Select(c => c.LastName).FirstOrDefault())
                                       .FirstOrDefault()),
            "vehicle"  => Order(r => _db.ClientVehicleRelations.Where(cvr => cvr.Id == r.ClientVehicleRelationId)
                                       .Select(cvr => cvr.VehicleId == null ? null
                                              : _db.Vehicles.Where(v => v.Id == cvr.VehicleId).Select(v => v.Plate).FirstOrDefault())
                                       .FirstOrDefault()),
            _          => query.OrderByDescending(r => r.CreatedAt),
        };

        var pageRows = await ordered
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new
            {
                r.Id, r.CompanyId, r.RequestTypeId, r.ClientVehicleRelationId,
                r.CreatedAt, r.ModifiedAt, r.EndedAt,
                r.CreatedByUserId,
                r.Note, r.Active,
            })
            .ToListAsync();

        // Batch-resolve joins
        var typeIds       = pageRows.Select(r => r.RequestTypeId).Distinct().ToList();
        var relationIds   = pageRows.Select(r => r.ClientVehicleRelationId).Distinct().ToList();
        var userIds       = pageRows.Select(r => r.CreatedByUserId).Distinct().ToList();

        var typeNames = await _db.RequestTypes.AsNoTracking()
            .Where(t => typeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);

        var relations = await _db.ClientVehicleRelations.AsNoTracking()
            .Where(r => relationIds.Contains(r.Id))
            .Select(r => new { r.Id, r.ClientId, r.VehicleId })
            .ToDictionaryAsync(r => r.Id);

        var clientIds  = relations.Values.Select(r => r.ClientId).Distinct().ToList();
        var vehicleIds = relations.Values.Where(r => r.VehicleId.HasValue)
                                         .Select(r => r.VehicleId!.Value).Distinct().ToList();

        var clients = await _db.Clients.AsNoTracking()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => new { c.Id, c.FirstName, c.MiddleName, c.LastName })
            .ToDictionaryAsync(c => c.Id);

        var vehicles = await _db.Vehicles.AsNoTracking()
            .Where(v => vehicleIds.Contains(v.Id))
            .Select(v => new { v.Id, v.Vin, v.Plate })
            .ToDictionaryAsync(v => v.Id);

        var users = await _db.Users.AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToDictionaryAsync(u => u.Id);

        string? joinClientName(long cid)
        {
            if (!clients.TryGetValue(cid, out var c)) return null;
            return string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        }
        string? userName(string id)
        {
            if (!users.TryGetValue(id, out var u)) return null;
            return u.FullName ?? u.UserName;
        }

        var items = pageRows.Select(r =>
        {
            string? vin = null, plate = null, clientName = null;
            long? vehicleId = null;
            if (relations.TryGetValue(r.ClientVehicleRelationId, out var rel))
            {
                clientName = joinClientName(rel.ClientId);
                vehicleId = rel.VehicleId;
                if (rel.VehicleId.HasValue && vehicles.TryGetValue(rel.VehicleId.Value, out var v))
                {
                    vin = v.Vin;
                    plate = v.Plate;
                }
            }
            return new RequestListItem(
                r.Id, r.CompanyId, r.RequestTypeId,
                typeNames.GetValueOrDefault(r.RequestTypeId),
                r.ClientVehicleRelationId,
                clientName,
                vehicleId, vin, plate,
                r.CreatedAt, r.ModifiedAt, r.EndedAt,
                userName(r.CreatedByUserId),
                r.Note, r.Active);
        }).ToList();

        return Ok(new PagedDto<RequestListItem>(page, pageSize, total, items));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<RequestReadDto>> Get(long id)
    {
        var r = await _db.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        // Resolve creator/modifier/ender usernames in a single batch
        var ids = new[] { r.CreatedByUserId, r.ModifiedByUserId, r.EndedByUserId }
            .Where(x => !string.IsNullOrEmpty(x)).Cast<string>().Distinct().ToList();
        var users = await _db.Users.AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToDictionaryAsync(u => u.Id);
        string? userName(string? id) =>
            id != null && users.TryGetValue(id, out var u) ? (u.FullName ?? u.UserName) : null;

        return Ok(new RequestReadDto(
            r.Id, r.CompanyId, r.RequestTypeId,
            r.ClientVehicleRelationId, r.NewClientVehicleRelationId,
            r.TechnicalExamReportId, r.PreviousRegistrationId,
            r.CreatedAt, r.ModifiedAt, r.EndedAt,
            r.CreatedByUserId, r.ModifiedByUserId, r.EndedByUserId,
            userName(r.CreatedByUserId), userName(r.ModifiedByUserId), userName(r.EndedByUserId),
            r.VehicleDataChanged, r.ClientDataChanged,
            r.Note, r.Active, r.LegacyReferenceNumber, r.RowVersion));
    }

    [HttpPost]
    public async Task<ActionResult<RequestReadDto>> Create([FromBody] RequestWriteDto dto)
    {
        if (_tenant.UserId is null) return Unauthorized();

        // Resolve tenant (same pattern as ClientsController)
        byte companyId;
        if (_tenant.IsAdmin && dto.CompanyId.HasValue)
        {
            if (!await _db.Companies.AnyAsync(co => co.Id == dto.CompanyId.Value))
                return BadRequest(new { error = $"Company {dto.CompanyId.Value} does not exist." });
            companyId = dto.CompanyId.Value;
        }
        else if (_tenant.CompanyId.HasValue)
        {
            companyId = _tenant.CompanyId.Value;
        }
        else
        {
            return BadRequest(new { error = "No tenant CompanyId resolved from the token." });
        }

        // Validate relation exists (and is visible — tenant filter applies here)
        var relation = await _db.ClientVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == dto.ClientVehicleRelationId);
        if (relation == null)
            return BadRequest(new { error = "ClientVehicleRelation not found or not visible." });

        // Validate request type
        var type = await _db.RequestTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == dto.RequestTypeId);
        if (type == null)
            return BadRequest(new { error = "RequestType not found." });

        // New owner ("Нов сопственик", legacy uxRequestEdit.RebindUI:159-171): for
        // ownership-transfer types the operator free-searches ANY client; the server
        // resolves-or-creates the (newClient + anchor's vehicle) owner relation so that
        // NewClientVehicleRelationId always points at a real relation. A directly-supplied
        // relation id is still honored for migrated/legacy data.
        long? newOwnerRelationId = dto.NewClientVehicleRelationId;
        if (type.TransfersOwnership)
        {
            if (dto.NewOwnerClientId is long nc && nc > 0)
            {
                if (!await _db.Clients.AnyAsync(c => c.Id == nc))
                    return BadRequest(new { error = "Новиот сопственик (клиент) не постои." });
                if (relation.VehicleId is null)
                    return BadRequest(new { error = "Анкер-врската нема возило за пренос на сопственост." });
                newOwnerRelationId = await ResolveOrCreateNewOwnerRelationAsync(
                    relation.VehicleId.Value, relation.RelationTypeId, nc);
            }
            if (newOwnerRelationId is null || newOwnerRelationId < 1)
                return BadRequest(new { error = "Новиот сопственик е задолжителен за пренос на сопственост." });
            if (newOwnerRelationId == dto.ClientVehicleRelationId)
                return BadRequest(new { error = "Новиот сопственик мора да се разликува од тековниот сопственик." });
            if (!await _db.ClientVehicleRelations.AnyAsync(r => r.Id == newOwnerRelationId.Value))
                return BadRequest(new { error = "Новата врска не постои." });
        }
        else if (dto.NewClientVehicleRelationId is long nrel && nrel > 0)
        {
            if (!await _db.ClientVehicleRelations.AnyAsync(r => r.Id == nrel))
                return BadRequest(new { error = "NewClientVehicleRelation not found." });
        }

        var entity = new Request
        {
            CompanyId = companyId,
            RequestTypeId = dto.RequestTypeId,
            ClientVehicleRelationId = dto.ClientVehicleRelationId,
            NewClientVehicleRelationId = newOwnerRelationId,
            TechnicalExamReportId = dto.TechnicalExamReportId,
            PreviousRegistrationId = dto.PreviousRegistrationId,
            Note = dto.Note,
            Active = dto.Active ?? true,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = _tenant.UserId,
        };
        _db.Requests.Add(entity);
        await _db.SaveChangesAsync();

        // Legacy parity (uxRequestEdit.vb:173-195): auto-create the technical-exam stub for
        // renewal/first-registration/tech-exam request types. Runs BEFORE the request-fee debt
        // block so that block inherits the new exam's organization (was 0 before). Best-effort
        // like the debt hook — a failure here never rolls back the request.
        try { await TryAutoCreateExamAsync(entity, type); }
        catch { /* swallow — see TryAutoCreateExamAsync remarks */ }

        // Phase 3: auto-debt creation — mirrors legacy AddDeptsToCustomer (Request.vb:721,840-971).
        // Fires only on Insert (legacy DataPortal_Insert), not Update — matching legacy.
        try
        {
            // BR-DEBT-1: for ownership-transfer requests, debts go to the NEW owner relation.
            //   Legacy Request.vb:856-861 —
            //     If reqType.IsNewCustomer Then  IdRelation = IdCustomerVehicleRelationNew
            //     Else                           IdRelation = IdCustomerVehicleRelation
            var debtRelationId = (type.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue)
                ? entity.NewClientVehicleRelationId.Value
                : entity.ClientVehicleRelationId;

            // BR-DEBT-2: resolve the customer's living community for the per-municipality
            // rule filter (Request.vb:868-878 + 889-891). Rules with non-null CommunityId
            // only fire when this value matches; rules with NULL CommunityId always fire.
            int? communityId = null;
            var rel = await _db.ClientVehicleRelations.AsNoTracking()
                .Where(r => r.Id == debtRelationId)
                .Select(r => new { r.ClientId })
                .FirstOrDefaultAsync();
            if (rel != null)
            {
                var cityId = await _db.Clients.AsNoTracking()
                    .Where(c => c.Id == rel.ClientId)
                    .Select(c => c.CityId)
                    .FirstOrDefaultAsync();
                if (cityId.HasValue)
                    communityId = await _db.Cities.AsNoTracking()
                        .Where(c => c.Id == cityId.Value)
                        .Select(c => (int?)c.CommunityId)
                        .FirstOrDefaultAsync();
            }

            // Inherit org from the linked tech-exam when present; otherwise 0.
            var orgId = 0;
            if (entity.TechnicalExamReportId.HasValue)
            {
                orgId = await _db.TechnicalExamReports.AsNoTracking()
                    .Where(t => t.Id == entity.TechnicalExamReportId.Value)
                    .Select(t => t.OrganizationId)
                    .FirstOrDefaultAsync();
            }

            await _debts.CreateDebtsForSourceAsync(
                origin:                     DebtOrigin.Request,
                originId:                   entity.Id,
                customerVehicleRelationId:  debtRelationId,
                organizationId:             orgId,
                trigger:                    PriceTrigger.Request,
                communityId:                communityId,
                note:                       $"по барање бр. {entity.Id}");
        }
        catch
        {
            // Phase 5 will move this to a transactional outbox for proper retry semantics.
        }

        return await Get(entity.Id);
    }

    /// <summary>
    /// Legacy "auto technical exam on request save" (uxRequestEdit.vb:173-195). When a renewal /
    /// first-registration / tech-exam request type is created, the legacy app auto-created a
    /// passing technical-exam stub (the operator fills the details later) and back-linked it to
    /// the request; the exam's own save generated the exam-fee debt.
    ///
    /// v2 port:
    ///   • Gate (legacy `objCurentTehExamOrganization.AutmateProceses`) → config flag
    ///     <c>Requests:AutoCreateTechExam</c>.
    ///   • Gate (legacy `IsTehnicalExamRequired &gt; 0`) → the migrated
    ///     <see cref="RequestType.TechnicalExamRequirement"/> column carries the legacy
    ///     IsTehnicalExamRequired value, which IS the exam-type id (0 = none). We read it as an int.
    ///   • Gate (legacy `IdTechnicalExamReport = 0`) → only when the request has no exam linked yet.
    ///   • No "skip if a recent valid exam exists" check — legacy has none.
    /// </summary>
    /// <summary>
    /// Resolves (or creates) the new-owner relation for an ownership transfer, mirroring legacy
    /// uxRequestEdit.RebindUI:159-171: reuse an existing (client + vehicle) relation if present,
    /// otherwise create one. Created INACTIVE — the End flow activates it (and deactivates the
    /// anchor) when the transfer actually completes, so an abandoned request never leaves a vehicle
    /// with two active owners.
    /// </summary>
    private async Task<long> ResolveOrCreateNewOwnerRelationAsync(long vehicleId, byte ownerRelationTypeId, long newOwnerClientId)
    {
        var existing = await _db.ClientVehicleRelations
            .Where(r => r.ClientId == newOwnerClientId && r.VehicleId == vehicleId)
            .OrderByDescending(r => r.Active).ThenByDescending(r => r.StartDate)
            .Select(r => (long?)r.Id)
            .FirstOrDefaultAsync();
        if (existing.HasValue) return existing.Value;

        var rel = new ClientVehicleRelation
        {
            ClientId = newOwnerClientId,
            VehicleId = vehicleId,
            RelationTypeId = ownerRelationTypeId,
            StartDate = DateTime.UtcNow,
            Active = false,   // activated by the End flow when ownership transfer completes
        };
        _db.ClientVehicleRelations.Add(rel);
        await _db.SaveChangesAsync();
        return rel.Id;
    }

    private async Task TryAutoCreateExamAsync(Request entity, RequestType type)
    {
        if (!_config.GetValue("Requests:AutoCreateTechExam", false)) return;
        if (entity.TechnicalExamReportId.HasValue) return;                 // legacy IdTechnicalExamReport = 0 guard

        var carriedTypeId = (int)type.TechnicalExamRequirement;            // migrated column = legacy exam-type id
        if (carriedTypeId <= 0) return;                                    // legacy IsTehnicalExamRequired > 0 guard

        // Resolve the exam type: prefer the value the request type carries; fall back to the
        // configured default when that id is unknown (don't guess a type that doesn't exist).
        var typeId = await _db.TechnicalExamTypes.AnyAsync(t => t.Id == carriedTypeId)
            ? carriedTypeId
            : _config.GetValue("Requests:DefaultTechnicalExamTypeId", 1);
        if (!await _db.TechnicalExamTypes.AnyAsync(t => t.Id == typeId)) return;

        // The request path has no station context (legacy used the operator's current org);
        // resolve from configuration. Must be a real organization for the FK + RegNumber.
        var orgId = _config.GetValue("Requests:DefaultTechnicalExamOrganizationId", 0);
        if (orgId <= 0 || !await _db.TechnicalExamOrganizations.AnyAsync(o => o.Id == orgId)) return;

        // Anchor to the NEW owner on ownership transfer, else the current relation
        // (uxRequestEdit.vb:180-184 — identical to the request-fee debt routing below).
        var relationId = (type.TransfersOwnership && entity.NewClientVehicleRelationId.HasValue)
            ? entity.NewClientVehicleRelationId.Value
            : entity.ClientVehicleRelationId;

        var validDays = await _db.TechnicalExamTypes.Where(t => t.Id == typeId).Select(t => t.ValidDays).FirstAsync();
        var madeDate = DateOnly.FromDateTime(DateTime.UtcNow);
        var validTill = madeDate.AddDays(validDays > 0 ? validDays : 365);

        // RegNumber = {org}-{seq}/{year}; continues the org's sequence (mirror of
        // TechnicalExamReportsController.Create:655-666 — same format, same sequence space).
        var prefix = $"{orgId}-";
        var lastRn = await _db.TechnicalExamReports.AsNoTracking()
            .Where(r => r.OrganizationId == orgId && r.RegNumber != null && r.RegNumber.StartsWith(prefix))
            .OrderByDescending(r => r.Id).Select(r => r.RegNumber).FirstOrDefaultAsync();
        int seq = 1;
        if (lastRn != null)
        {
            int dash = lastRn.IndexOf('-'), slash = lastRn.IndexOf('/');
            if (dash >= 0 && slash > dash && int.TryParse(lastRn.Substring(dash + 1, slash - dash - 1), out var n)) seq = n + 1;
        }
        var regNumber = $"{orgId}-{seq}/{DateTime.UtcNow.Year}";

        var exam = new TechnicalExamReport
        {
            CompanyId = entity.CompanyId,
            CustomerVehicleRelationId = relationId,
            TechnicalExamTypeId = typeId,
            OrganizationId = orgId,
            RegNumber = regNumber,
            MadeDate = madeDate,
            ValidTillDate = validTill,
            FirstControllerLegacyId = int.TryParse(_tenant.UserId, out var uid) ? uid : null,
            VehicleIsRight = true,                                          // legacy auto-exam defaults to pass
            Active = true,
            CreatedAt = DateTime.UtcNow,
        };
        _db.TechnicalExamReports.Add(exam);
        await _db.SaveChangesAsync();

        entity.TechnicalExamReportId = exam.Id;                            // back-link (uxRequestEdit.vb:193)
        await _db.SaveChangesAsync();

        // Exam-fee debt — identical to TechnicalExamReportsController.Create:711-723
        // (the v2 equivalent of legacy insertFinancialStatePriceCatalogForTehnicalExams).
        var isIrregular = typeId > 1;                                      // legacy: type 1 is the regular РЕД-12М
        await _debts.CreateDebtsForSourceAsync(
            origin:                     isIrregular ? DebtOrigin.TechnicalExamIrregular : DebtOrigin.TechnicalExam,
            originId:                   exam.Id,
            customerVehicleRelationId:  relationId,
            organizationId:             orgId,
            trigger:                    isIrregular ? PriceTrigger.TechnicalExamIrregular : PriceTrigger.TechnicalExam,
            communityId:                null,
            note:                       $"технички преглед бр. {regNumber}");
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, [FromBody] RequestWriteDto dto)
    {
        if (_tenant.UserId is null) return Unauthorized();

        var entity = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return NotFound();

        // Once a request is ended, it can't be edited (status is terminal).
        if (entity.EndedAt is not null)
            return BadRequest(new { error = "Завршеното барање не може да се менува." });

        // CompanyId is locked after creation
        if (dto.CompanyId.HasValue && dto.CompanyId.Value != entity.CompanyId)
            return BadRequest(new { error = "CompanyId is immutable after creation." });

        var type = await _db.RequestTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Id == dto.RequestTypeId);
        if (type == null) return BadRequest(new { error = "RequestType not found." });

        // New owner: same resolve-or-create as Create. The anchor (ClientVehicleRelationId)
        // is locked on update, so the new-owner relation is keyed to the existing anchor's vehicle.
        long? newOwnerRelationId = dto.NewClientVehicleRelationId;
        if (type.TransfersOwnership)
        {
            if (dto.NewOwnerClientId is long nc && nc > 0)
            {
                if (!await _db.Clients.AnyAsync(c => c.Id == nc))
                    return BadRequest(new { error = "Новиот сопственик (клиент) не постои." });
                var anchor = await _db.ClientVehicleRelations.AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == entity.ClientVehicleRelationId);
                if (anchor?.VehicleId is null)
                    return BadRequest(new { error = "Анкер-врската нема возило за пренос на сопственост." });
                newOwnerRelationId = await ResolveOrCreateNewOwnerRelationAsync(
                    anchor.VehicleId.Value, anchor.RelationTypeId, nc);
            }
            if (newOwnerRelationId is null || newOwnerRelationId < 1)
                return BadRequest(new { error = "Новиот сопственик е задолжителен за пренос на сопственост." });
            if (newOwnerRelationId == entity.ClientVehicleRelationId)
                return BadRequest(new { error = "Новиот сопственик мора да се разликува од тековниот сопственик." });
            if (!await _db.ClientVehicleRelations.AnyAsync(r => r.Id == newOwnerRelationId.Value))
                return BadRequest(new { error = "Новата врска не постои." });
        }

        entity.RequestTypeId = dto.RequestTypeId;
        // ClientVehicleRelationId is the anchor — keep it locked once a request exists
        // (operator should soft-delete + create new request if the anchor is wrong)
        // entity.ClientVehicleRelationId = dto.ClientVehicleRelationId;
        entity.NewClientVehicleRelationId = newOwnerRelationId;
        entity.TechnicalExamReportId = dto.TechnicalExamReportId;
        entity.PreviousRegistrationId = dto.PreviousRegistrationId;
        entity.Note = dto.Note;
        entity.Active = dto.Active ?? entity.Active;
        entity.ModifiedAt = DateTime.UtcNow;
        entity.ModifiedByUserId = _tenant.UserId;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Soft-delete: sets Active=false. Row stays in DB; reactivate via PUT.</summary>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        var entity = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return NotFound();
        entity.Active = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }

    public record EndRequestResultDto(
        long Id,
        DateTime EndedAt,
        bool RelationDeactivated,
        bool VehicleDeactivated,
        bool OwnershipTransferred,
        bool NeedsNewRegistration,
        string[] Notes);

    /// <summary>
    /// Finalize a Request: validate prerequisites, then apply side-effects atomically
    /// per the RequestType flags. Sets EndedAt + EndedByUserId. Closed requests
    /// are immutable from this point on.
    /// </summary>
    [HttpPost("{id:long}/end")]
    public async Task<ActionResult<EndRequestResultDto>> End(long id)
    {
        if (_tenant.UserId is null) return Unauthorized();

        // Load request + type
        var entity = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);
        if (entity == null) return NotFound();
        if (entity.EndedAt is not null)
            return BadRequest(new { error = "Барањето е веќе завршено." });
        if (!entity.Active)
            return BadRequest(new { error = "Барањето е неактивно." });

        var type = await _db.RequestTypes.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == entity.RequestTypeId);
        if (type == null) return BadRequest(new { error = "RequestType not found." });

        // -------- Pre-validation (server-side mirror of UI checks) --------
        if (type.PaymentRequired)
        {
            var hasPaymentProof = await _db.RequestPaymentProofs
                .AnyAsync(p => p.RequestId == entity.Id && p.Active);
            if (!hasPaymentProof)
                return BadRequest(new { error = "Потребен е најмалку еден доказ за уплата." });
        }
        if (type.TechnicalExamRequirement == TechnicalExamRequirement.Required
            && entity.TechnicalExamReportId is null)
        {
            return BadRequest(new { error = "Потребен е технички преглед." });
        }
        if (type.TransfersOwnership)
        {
            if (entity.NewClientVehicleRelationId is null)
                return BadRequest(new { error = "Новиот сопственик не е избран." });
            var newRel = await _db.ClientVehicleRelations
                .FirstOrDefaultAsync(r => r.Id == entity.NewClientVehicleRelationId.Value);
            if (newRel == null)
                return BadRequest(new { error = "Новата врска не постои." });
        }
        if (type.PreviousRegistrationRequired && entity.PreviousRegistrationId is null)
            return BadRequest(new { error = "Потребна е претходна регистрација." });

        // -------- Side-effects (atomic) --------
        await using var tx = await _db.Database.BeginTransactionAsync();
        var notes = new List<string>();
        var relationDeactivated = false;
        var vehicleDeactivated = false;
        var ownershipTransferred = false;

        // Load the anchor relation once — needed by several side-effects
        var anchor = await _db.ClientVehicleRelations
            .FirstOrDefaultAsync(r => r.Id == entity.ClientVehicleRelationId);
        if (anchor == null)
        {
            await tx.RollbackAsync();
            return BadRequest(new { error = "Anchor relation missing." });
        }

        if (type.TransfersOwnership)
        {
            // Activate the new relation, deactivate the old. Both rows touched in one go.
            var newRel = await _db.ClientVehicleRelations
                .FirstAsync(r => r.Id == entity.NewClientVehicleRelationId!.Value);
            anchor.Active = false;
            anchor.EndDate = DateTime.UtcNow;
            newRel.Active = true;
            ownershipTransferred = true;
            notes.Add("Сопственоста е префрлена на новата врска.");
        }
        else if (type.DeactivatesRelation)
        {
            anchor.Active = false;
            anchor.EndDate = DateTime.UtcNow;
            relationDeactivated = true;
            notes.Add("Врската сопственик–возило е деактивирана.");
        }

        if (type.DeactivatesVehicle && anchor.VehicleId.HasValue)
        {
            var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == anchor.VehicleId.Value);
            if (vehicle != null)
            {
                vehicle.Active = false;
                vehicleDeactivated = true;
                notes.Add("Возилото е одјавено / деактивирано.");
            }
        }

        var needsNewRegistration = type.IssuesNewRegistration;
        if (needsNewRegistration)
        {
            // Deliberately deferred — operator creates the new VehicleRegistration row
            // from the Vehicle form afterward. Recorded as a hint so the UI can prompt.
            notes.Add("Потребно е да се изда нова регистрација (внеси од формата на возилото).");
        }

        // Stamp end-time
        entity.EndedAt = DateTime.UtcNow;
        entity.EndedByUserId = _tenant.UserId;
        entity.VehicleDataChanged = type.MutatesVehicleData || vehicleDeactivated;
        entity.ClientDataChanged = type.MutatesClientData;

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return Ok(new EndRequestResultDto(
            entity.Id, entity.EndedAt.Value,
            relationDeactivated, vehicleDeactivated, ownershipTransferred,
            needsNewRegistration, notes.ToArray()));
    }

    // -------- Print bundle --------

    public record PrintBundleDto(
        RequestReadDto Request,
        RequestTypeMeta Type,
        ClientMeta Client,
        VehicleMeta? Vehicle,
        VehicleMeta? NewVehicle,
        ClientMeta? NewClient,
        IReadOnlyList<ProofMeta> OwnershipProofs,
        IReadOnlyList<ProofMeta> PaymentProofs,
        CompanyMeta Company,
        RegistrationMeta? LastRegistration,
        /// <summary>The registration matching the vehicle's stored plate — i.e. the
        /// PREVIOUS registration on a re-registration/transfer (legacy
        /// CurrentVehicle.LastRegistration block). LastRegistration is the newest
        /// row overall, which on a transfer is the newly-issued one.</summary>
        RegistrationMeta? PreviousRegistration);

    public record RequestTypeMeta(byte Id, string Name, string? Description, byte DocumentPrintId, string DocumentPrintCode, string DocumentPrintName,
        bool TransfersOwnership, bool DeactivatesRelation, bool DeactivatesVehicle, bool IssuesNewRegistration);
    public record ClientMeta(long Id, string? FullName, string? FirstName, string? MiddleName, string? LastName,
        string? MB, string? TaxNumber, string? Address, string? CityName, string? CommunityName, string? CountryName,
        string? CitizenshipName, string? PhoneNumber, string? Email,
        bool? IsBusiness, DateTime? DateOfBirth);
    public record VehicleMeta(long Id, string Vin, string? EngineNumber, string? Plate,
        string? Maker, string? Model, string? ModelCode, string? Variant, string? TypeText,
        string? BodyType, string? Category, string? CategoryForPayments, byte? CategoryZelenMap,
        string? PrimaryColorName, string? PrimaryColorCode,
        string? SecondaryColorName, string? SecondaryColorCode,
        string? FuelName, string? EngineTypeName, string? EngineTypeCode, string? EcoProgramName,
        float? EnginePowerKw, float? EngineWorkingCapacityCc,
        float? EmptyWeightKg, float? MaxAllowedWeightKg,
        short? Seats, short? StandingSeats,
        string? MadeCountry,
        bool? HasLpg, DateTime? ManufactureDate,
        // Plav page-2 technical specs
        float? LengthMm, float? WidthMm, float? HeightMm,
        float? MaxLegalTotalMassKg, float? MaxConstructiveTotalMassKg, float? MaxLegalGroupMassKg,
        int? AxleCount, int? MaxRpm, float? MaxSpeedKmh,
        float? Co2GKm, float? NoiseStaticDb,
        int? AxleLoad1Kg, int? AxleLoad2Kg,
        float? MaxTrailerBrakedKg, float? MaxTrailerUnbrakedKg, float? MaxHitchLoadKg,
        string? ApprovalMark);
    public record RegistrationMeta(long Id, string PlateNumber, DateTime RegisteredDate, DateTime ValidUntil, string? Issuer);
    public record ProofMeta(long Id, string? TypeName, string? Detail);
    public record CompanyMeta(byte Id, string Name, string? CommunityName);

    /// <summary>
    /// Returns the full data bundle the frontend needs to render the printable
    /// paper form for this Request — request header + workflow type + client +
    /// vehicle + proofs + company. Replaces the legacy printPlav/Bel/Zelen procs.
    /// </summary>
    [HttpGet("{id:long}/print")]
    public async Task<ActionResult<PrintBundleDto>> PrintBundle(long id)
    {
        var r = await _db.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (r == null) return NotFound();

        // Build the read DTO inline (mirror of Get())
        var ids = new[] { r.CreatedByUserId, r.ModifiedByUserId, r.EndedByUserId }
            .Where(x => !string.IsNullOrEmpty(x)).Cast<string>().Distinct().ToList();
        var users = await _db.Users.AsNoTracking()
            .Where(u => ids.Contains(u.Id))
            .Select(u => new { u.Id, u.UserName, u.FullName })
            .ToDictionaryAsync(u => u.Id);
        string? userName(string? uid) =>
            uid != null && users.TryGetValue(uid, out var u) ? (u.FullName ?? u.UserName) : null;

        var requestDto = new RequestReadDto(
            r.Id, r.CompanyId, r.RequestTypeId,
            r.ClientVehicleRelationId, r.NewClientVehicleRelationId,
            r.TechnicalExamReportId, r.PreviousRegistrationId,
            r.CreatedAt, r.ModifiedAt, r.EndedAt,
            r.CreatedByUserId, r.ModifiedByUserId, r.EndedByUserId,
            userName(r.CreatedByUserId), userName(r.ModifiedByUserId), userName(r.EndedByUserId),
            r.VehicleDataChanged, r.ClientDataChanged,
            r.Note, r.Active, r.LegacyReferenceNumber, r.RowVersion);

        // Type + print template
        var type = await _db.RequestTypes.AsNoTracking().FirstAsync(t => t.Id == r.RequestTypeId);
        var print = await _db.RequestDocumentPrints.AsNoTracking()
            .FirstAsync(p => p.Id == type.DocumentPrintId);
        var typeMeta = new RequestTypeMeta(type.Id, type.Name, type.Description,
            type.DocumentPrintId, print.Code, print.Name,
            type.TransfersOwnership, type.DeactivatesRelation, type.DeactivatesVehicle, type.IssuesNewRegistration);

        // Client + Vehicle via the anchor relation
        var relation = await _db.ClientVehicleRelations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == r.ClientVehicleRelationId);
        ClientMeta clientMeta = await BuildClientMeta(relation?.ClientId)
            ?? new ClientMeta(0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
        VehicleMeta? vehicleMeta = await BuildVehicleMeta(relation?.VehicleId);
        RegistrationMeta? lastReg = await BuildLastRegistrationMeta(relation?.VehicleId);
        // Previous registration = the row whose plate matches the vehicle's stored
        // Plate (legacy Vehicles.LastRegistratinNumber). On a transfer/re-registration
        // this is the OLD plate (e.g. "SN 4604 AC"), distinct from the newly-issued
        // sentinel ("VE-000-AA") that BuildLastRegistrationMeta returns as newest.
        RegistrationMeta? prevReg = await BuildPreviousRegistrationMeta(relation?.VehicleId, vehicleMeta?.Plate);

        // New owner (Б "промена на сопственикот" panel / re-registration).
        // The legacy IdCustomerVehicleRelationNew — migrated into
        // Request.NewClientVehicleRelationId — is a CLIENT id (legacy overloads the
        // column: customer-id while editing, relation-id after save), whereas
        // v2-native requests store a real RELATION id. Mirror the legacy print, which
        // shows NewOwner = Customer(IdCustomerVehicleRelationNew):
        //   • if the value is a client that has a relation on THIS vehicle → it's the
        //     migrated new-owner customer id. Covers same-owner data changes
        //     (e.g. ПЕШАНКОВСКИ, value = the page-1 client) AND true transfers
        //     (e.g. КОВАЧЕВ, a different client).
        //   • otherwise treat it as a relation id (v2-native) and take that
        //     relation's client.
        // (Replaces the old "newest non-anchor active relation on the vehicle"
        // heuristic, which wrongly resolved to a PRIOR owner.)
        ClientMeta? newClientMeta = null;
        VehicleMeta? newVehicleMeta = null;
        if (r.NewClientVehicleRelationId is long newRef && newRef > 0 && relation?.VehicleId is long newVid)
        {
            bool isClientOnVehicle = await _db.ClientVehicleRelations.AsNoTracking()
                .AnyAsync(x => x.VehicleId == newVid && x.ClientId == newRef);
            if (isClientOnVehicle)
            {
                newClientMeta = await BuildClientMeta(newRef);
                newVehicleMeta = await BuildVehicleMeta(newVid);
            }
            else
            {
                var newRel = await _db.ClientVehicleRelations.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == newRef);
                if (newRel != null)
                {
                    newClientMeta = await BuildClientMeta(newRel.ClientId);
                    newVehicleMeta = await BuildVehicleMeta(newRel.VehicleId);
                }
            }
        }

        // Active proofs only — they're what we want on the paper form
        var ownerProofsRaw = await _db.RequestOwnershipProofs.AsNoTracking()
            .Where(p => p.RequestId == r.Id && p.Active)
            .OrderBy(p => p.Id)
            .ToListAsync();
        var oTypeIds = ownerProofsRaw.Select(p => p.OwnershipProofTypeId).Distinct().ToList();
        var oTypeNames = await _db.RequestOwnershipProofTypes.AsNoTracking()
            .Where(t => oTypeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);
        var ownerProofs = ownerProofsRaw.Select(p => new ProofMeta(
            p.Id, oTypeNames.GetValueOrDefault(p.OwnershipProofTypeId), p.Detail)).ToList();

        var paymentProofsRaw = await _db.RequestPaymentProofs.AsNoTracking()
            .Where(p => p.RequestId == r.Id && p.Active)
            .OrderBy(p => p.Id)
            .ToListAsync();
        var pTypeIds = paymentProofsRaw.Select(p => p.PaymentProofTypeId).Distinct().ToList();
        var pTypeNames = await _db.RequestPaymentProofTypes.AsNoTracking()
            .Where(t => pTypeIds.Contains(t.Id))
            .ToDictionaryAsync(t => t.Id, t => t.Name);
        var paymentProofs = paymentProofsRaw.Select(p => new ProofMeta(
            p.Id, pTypeNames.GetValueOrDefault(p.PaymentProofTypeId), p.Detail)).ToList();

        // Company header — try to resolve the company's home community via its first Station's City.
        // Falls back to null when the chain is incomplete.
        var company = await _db.Companies.AsNoTracking()
            .Where(c => c.Id == r.CompanyId)
            .Select(c => new { c.Id, c.Name })
            .FirstAsync();
        string? companyCommunity = null;
        var anyStation = await _db.Stations.AsNoTracking()
            .IgnoreQueryFilters()                            // company-side lookup; tenant filter would block admins-of-other-tenants
            .Where(s => s.CompanyId == company.Id && s.Active == true)
            .Select(s => s.Id)
            .FirstOrDefaultAsync();
        // Stations don't currently carry a city; community surfaces only if the
        // legacy migration tagged Company → Community somewhere. Leave null for now.
        var companyMeta = new CompanyMeta(company.Id, company.Name, companyCommunity);

        return Ok(new PrintBundleDto(
            requestDto, typeMeta,
            clientMeta, vehicleMeta, newVehicleMeta, newClientMeta,
            ownerProofs, paymentProofs, companyMeta, lastReg, prevReg));
    }

    private async Task<ClientMeta?> BuildClientMeta(long? clientId)
    {
        if (!clientId.HasValue) return null;
        var c = await _db.Clients.AsNoTracking()
            .Where(x => x.Id == clientId.Value)
            .Select(x => new
            {
                x.Id, x.FirstName, x.MiddleName, x.LastName,
                x.MB, x.TaxNumber, x.Address, x.PhoneNumber, x.Email,
                x.CityId, x.CitizenshipId, x.Business, x.DateOfBirth,
            })
            .FirstOrDefaultAsync();
        if (c == null) return null;

        var name = string.Join(' ', new[] { c.FirstName, c.MiddleName, c.LastName }
            .Where(p => !string.IsNullOrWhiteSpace(p)));

        // City → Community → Country names are joined with their parent lookups.
        string? cityName = null, communityName = null, countryName = null;
        if (c.CityId.HasValue)
        {
            var cy = await _db.Cities.AsNoTracking()
                .Where(x => x.Id == c.CityId.Value)
                .Select(x => new { x.Name, x.CommunityId })
                .FirstOrDefaultAsync();
            if (cy != null)
            {
                cityName = cy.Name;
                var comm = await _db.Communities.AsNoTracking()
                    .Where(x => x.Id == cy.CommunityId)
                    .Select(x => new { x.Name, x.CountryId })
                    .FirstOrDefaultAsync();
                if (comm != null)
                {
                    communityName = comm.Name;
                    countryName = await _db.Countries.AsNoTracking()
                        .Where(x => x.Id == comm.CountryId)
                        .Select(x => x.Name).FirstOrDefaultAsync();
                }
            }
        }
        string? citizenshipName = null;
        if (c.CitizenshipId.HasValue)
        {
            citizenshipName = await _db.Citizenships.AsNoTracking()
                .Where(x => x.Id == c.CitizenshipId.Value)
                .Select(x => x.Name).FirstOrDefaultAsync();
        }

        return new ClientMeta(
            c.Id,
            string.IsNullOrWhiteSpace(name) ? null : name,
            c.FirstName, c.MiddleName, c.LastName,
            c.MB, c.TaxNumber, c.Address,
            cityName, communityName, countryName,
            citizenshipName,
            c.PhoneNumber, c.Email,
            c.Business, c.DateOfBirth);
    }

    private async Task<VehicleMeta?> BuildVehicleMeta(long? vehicleId)
    {
        if (!vehicleId.HasValue) return null;
        var v = await _db.Vehicles.AsNoTracking()
            .Where(x => x.Id == vehicleId.Value)
            .Select(x => new
            {
                x.Id, x.Vin, x.EngineNumber, x.Plate, x.ModelId,
                x.BodyTypeId, x.CategoryId, x.PaymentCategoryId,
                x.PrimaryColorId, x.SecondaryColorId,
                x.FuelId, x.EngineTypeId, x.EcoProgramId,
                x.EnginePowerKw, x.EngineWorkingCapacityCc,
                x.EmptyWeightKg, x.MaxAllowedWeightKg,
                x.Seats, x.StandingSeats,
                x.ModelVariant, x.TypeText, x.MadeCountryId,
                x.HasLpg, x.ManufactureDate,
                // Plav page-2 technical specs
                x.LengthMm, x.WidthMm, x.HeightMm,
                x.MaxLegalTotalMassKg, x.MaxConstructiveTotalMassKg, x.MaxLegalGroupMassKg,
                x.AxleCount, x.MaxRpm, x.MaxSpeedKmh,
                x.Co2GKm, x.NoiseStaticDb,
                x.AxleLoad1Kg, x.AxleLoad2Kg,
                x.MaxTrailerBrakedKg, x.MaxTrailerUnbrakedKg, x.MaxHitchLoadKg,
                x.ApprovalMark,
            })
            .FirstOrDefaultAsync();
        if (v == null) return null;

        string? maker = null, model = null, modelCode = null;
        if (v.ModelId.HasValue)
        {
            var m = await _db.VehicleModels.AsNoTracking()
                .Where(x => x.Id == v.ModelId.Value)
                .Select(x => new { x.Name, x.Code, x.MakerId })
                .FirstOrDefaultAsync();
            if (m != null)
            {
                model = m.Name;
                modelCode = m.Code;
                maker = await _db.VehicleMakers.AsNoTracking()
                    .Where(x => x.Id == m.MakerId)
                    .Select(x => x.Name).FirstOrDefaultAsync();
            }
        }

        string? bodyType = null;
        if (v.BodyTypeId.HasValue)
        {
            // Legacy renders this field as "{Code}  {Name}" (two spaces) — e.g.
            // "AB  VOZILO SO PODVIZHNA ZADNA VRATA". Match that exactly.
            var bt = await _db.VehicleBodyTypes.AsNoTracking()
                .Where(x => x.Id == v.BodyTypeId.Value)
                .Select(x => new { x.Code, x.Name })
                .FirstOrDefaultAsync();
            if (bt != null)
            {
                bodyType = string.IsNullOrWhiteSpace(bt.Code)
                    ? bt.Name
                    : $"{bt.Code}  {bt.Name}";
            }
        }
        string? category = null;
        if (v.CategoryId.HasValue)
        {
            // Legacy J field prints "{Code} {Name}" — e.g. "M1 ПАТНИЧКО ВОЗИЛО".
            var c = await _db.VehicleCategories.AsNoTracking()
                .Where(x => x.Id == v.CategoryId.Value)
                .Select(x => new { x.Code, x.Name })
                .FirstOrDefaultAsync();
            if (c != null)
            {
                category = string.IsNullOrWhiteSpace(c.Code)
                    ? c.Name
                    : $"{c.Code} {c.Name}";
            }
        }
        string? catPay = null;
        byte? categoryZelenMap = null;
        if (v.PaymentCategoryId.HasValue)
        {
            var pc = await _db.VehiclePaymentCategories.AsNoTracking()
                .Where(x => x.Id == v.PaymentCategoryId.Value)
                .Select(x => new { x.Name, x.ZelenMap }).FirstOrDefaultAsync();
            if (pc != null) { catPay = pc.Name; categoryZelenMap = pc.ZelenMap; }
        }

        string? primaryColorName = null, primaryColorCode = null;
        if (v.PrimaryColorId.HasValue)
        {
            var c1 = await _db.VehicleColors.AsNoTracking()
                .Where(x => x.Id == v.PrimaryColorId.Value)
                .Select(x => new { x.Name, x.Code }).FirstOrDefaultAsync();
            if (c1 != null) { primaryColorName = c1.Name; primaryColorCode = c1.Code; }
        }
        string? secondaryColorName = null, secondaryColorCode = null;
        if (v.SecondaryColorId.HasValue)
        {
            var c2 = await _db.VehicleColors.AsNoTracking()
                .Where(x => x.Id == v.SecondaryColorId.Value)
                .Select(x => new { x.Name, x.Code }).FirstOrDefaultAsync();
            if (c2 != null) { secondaryColorName = c2.Name; secondaryColorCode = c2.Code; }
        }

        string? fuelName = v.FuelId.HasValue
            ? await _db.VehicleFuels.AsNoTracking().Where(x => x.Id == v.FuelId.Value).Select(x => x.Name).FirstOrDefaultAsync()
            : null;
        string? engineTypeName = null, engineTypeCode = null;
        if (v.EngineTypeId.HasValue)
        {
            var et = await _db.VehicleEngineTypes.AsNoTracking()
                .Where(x => x.Id == v.EngineTypeId.Value)
                .Select(x => new { x.Name, x.Code }).FirstOrDefaultAsync();
            if (et != null) { engineTypeName = et.Name; engineTypeCode = et.Code; }
        }
        string? ecoProgramName = v.EcoProgramId.HasValue
            ? await _db.VehicleEcoPrograms.AsNoTracking().Where(x => x.Id == v.EcoProgramId.Value).Select(x => x.Name).FirstOrDefaultAsync()
            : null;
        string? madeCountry = v.MadeCountryId.HasValue
            ? await _db.Countries.AsNoTracking().Where(x => x.Id == v.MadeCountryId.Value).Select(x => x.Name).FirstOrDefaultAsync()
            : null;

        return new VehicleMeta(
            v.Id, v.Vin, v.EngineNumber, v.Plate,
            maker, model, modelCode, v.ModelVariant, v.TypeText,
            bodyType, category, catPay, categoryZelenMap,
            primaryColorName, primaryColorCode,
            secondaryColorName, secondaryColorCode,
            fuelName, engineTypeName, engineTypeCode, ecoProgramName,
            v.EnginePowerKw, v.EngineWorkingCapacityCc,
            v.EmptyWeightKg, v.MaxAllowedWeightKg,
            v.Seats, v.StandingSeats,
            madeCountry,
            v.HasLpg, v.ManufactureDate,
            v.LengthMm, v.WidthMm, v.HeightMm,
            v.MaxLegalTotalMassKg, v.MaxConstructiveTotalMassKg, v.MaxLegalGroupMassKg,
            v.AxleCount, v.MaxRpm, v.MaxSpeedKmh,
            v.Co2GKm, v.NoiseStaticDb,
            v.AxleLoad1Kg, v.AxleLoad2Kg,
            v.MaxTrailerBrakedKg, v.MaxTrailerUnbrakedKg, v.MaxHitchLoadKg,
            v.ApprovalMark);
    }

    private async Task<RegistrationMeta?> BuildLastRegistrationMeta(long? vehicleId)
    {
        if (!vehicleId.HasValue) return null;
        var reg = await _db.VehicleRegistrations.AsNoTracking()
            .Where(x => x.VehicleId == vehicleId.Value && x.Active)
            .OrderByDescending(x => x.ValidUntil)
            .ThenByDescending(x => x.RegisteredDate)
            .Select(x => new { x.Id, x.PlateNumber, x.RegisteredDate, x.ValidUntil, x.IssuerId })
            .FirstOrDefaultAsync();
        if (reg == null) return null;
        var issuerName = await _db.DocumentIssuers.AsNoTracking()
            .Where(x => x.Id == reg.IssuerId).Select(x => x.Name).FirstOrDefaultAsync();
        return new RegistrationMeta(reg.Id, reg.PlateNumber, reg.RegisteredDate, reg.ValidUntil, issuerName);
    }

    /// <summary>The registration whose PlateNumber equals the vehicle's stored Plate
    /// (legacy "LastRegistration"). On a transfer this is the PREVIOUS plate, distinct
    /// from the newest (newly-issued) row. Falls back to the second-newest row when no
    /// plate match exists (e.g. the vehicle plate is itself a sentinel).</summary>
    private async Task<RegistrationMeta?> BuildPreviousRegistrationMeta(long? vehicleId, string? vehiclePlate)
    {
        if (!vehicleId.HasValue) return null;

        var regs = await _db.VehicleRegistrations.AsNoTracking()
            .Where(x => x.VehicleId == vehicleId.Value && x.Active)
            .OrderByDescending(x => x.ValidUntil)
            .ThenByDescending(x => x.RegisteredDate)
            .Select(x => new { x.Id, x.PlateNumber, x.RegisteredDate, x.ValidUntil, x.IssuerId })
            .ToListAsync();
        if (regs.Count == 0) return null;

        // Prefer the row whose plate matches the vehicle's stored plate.
        var match = !string.IsNullOrWhiteSpace(vehiclePlate)
            ? regs.FirstOrDefault(r => string.Equals(
                  (r.PlateNumber ?? "").Trim(), vehiclePlate!.Trim(), StringComparison.OrdinalIgnoreCase))
            : null;
        // Otherwise fall back to the second-newest (the one before the newest/new reg),
        // or the only row if there is just one.
        var reg = match ?? (regs.Count > 1 ? regs[1] : regs[0]);

        var issuerName = await _db.DocumentIssuers.AsNoTracking()
            .Where(x => x.Id == reg.IssuerId).Select(x => x.Name).FirstOrDefaultAsync();
        return new RegistrationMeta(reg.Id, reg.PlateNumber, reg.RegisteredDate, reg.ValidUntil, issuerName);
    }
}
