using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/installment-contracts")]
[Authorize]
public class InstallmentContractsController : ControllerBase
{
    private readonly VteDbContext _db;
    private readonly ITenantContext _tenant;

    public InstallmentContractsController(VteDbContext db, ITenantContext tenant) { _db = db; _tenant = tenant; }

    public record ContractDto(long Id, string ContractNumber, DateOnly ContractDate, int NumberOfInstallments, string? GuarantorName);
    public record CreateContractRequest(string ContractNumber, DateOnly ContractDate, int NumberOfInstallments, string? GuarantorName, string? GuarantorAddress, string? GuarantorEMBG, string? Note);

    [HttpGet]
    public async Task<ActionResult<PagedResult<ContractDto>>> List([FromQuery] int? page, [FromQuery] int? pageSize)
        => Ok(await _db.InstallmentContracts.AsNoTracking().OrderByDescending(c => c.ContractDate)
            .Select(c => new ContractDto(c.Id, c.ContractNumber, c.ContractDate, c.NumberOfInstallments, c.GuarantorName))
            .ToPagedAsync(page, pageSize));

    [HttpPost]
    public async Task<ActionResult<ContractDto>> Create(CreateContractRequest req)
    {
        // BR-PAY-034 (DB CHECK enforces 2..60 too — pre-check for friendlier error)
        if (req.NumberOfInstallments < 2 || req.NumberOfInstallments > 60)
            return BadRequest(new { error = "BR-PAY-034: NumberOfInstallments must be in [2, 60]." });
        if (_tenant.StationId is null) return BadRequest(new { error = "StationId could not be resolved." });

        var c = new InstallmentContract
        {
            StationId = _tenant.StationId.Value,
            ContractNumber = req.ContractNumber,
            ContractDate = req.ContractDate,
            NumberOfInstallments = req.NumberOfInstallments,
            GuarantorName = req.GuarantorName,
            GuarantorAddress = req.GuarantorAddress,
            GuarantorEMBG = req.GuarantorEMBG,
            Note = req.Note
        };
        _db.InstallmentContracts.Add(c);
        await _db.SaveChangesAsync();
        return Ok(new ContractDto(c.Id, c.ContractNumber, c.ContractDate, c.NumberOfInstallments, c.GuarantorName));
    }
}
