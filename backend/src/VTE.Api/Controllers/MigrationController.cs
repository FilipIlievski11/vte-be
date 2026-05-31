using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VTE.Api.Migration;
using VTE.Domain;

namespace VTE.Api.Controllers;

[ApiController]
[Route("api/admin/migration")]
[Authorize(Roles = Roles.Administrator)]
public class MigrationController : ControllerBase
{
    private readonly LegacyMigrationService _svc;

    public MigrationController(LegacyMigrationService svc) { _svc = svc; }

    // GET /api/admin/migration/customers/status
    // How many customers exist in VTE2, how far the legacy DB has progressed,
    // and how many rows are pending. Cheap — call freely.
    [HttpGet("customers/status")]
    public async Task<ActionResult<MigrationStatus>> Status(CancellationToken ct)
        => Ok(await _svc.GetStatusAsync(ct));

    // POST /api/admin/migration/customers/delta
    // Idempotent in spirit: inserts every legacy customer with Id greater than
    // the current VTE2 max. Skips and reports rows that hit any constraint.
    [HttpPost("customers/delta")]
    public async Task<ActionResult<MigrationResult>> RunDelta(CancellationToken ct)
        => Ok(await _svc.MigrateNewerCustomersAsync(ct));

    [HttpGet("streets/status")]
    public async Task<ActionResult<MigrationStatus>> StreetsStatus(CancellationToken ct)
        => Ok(await _svc.GetStreetsStatusAsync(ct));

    [HttpPost("streets/delta")]
    public async Task<ActionResult<MigrationResult>> RunStreetsDelta(CancellationToken ct)
        => Ok(await _svc.MigrateNewerStreetsAsync(ct));

    [HttpGet("vehicle-makers/status")]
    public async Task<ActionResult<MigrationStatus>> VehicleMakersStatus(CancellationToken ct)
        => Ok(await _svc.GetVehicleMakersStatusAsync(ct));

    [HttpPost("vehicle-makers/delta")]
    public async Task<ActionResult<MigrationResult>> RunVehicleMakersDelta(CancellationToken ct)
        => Ok(await _svc.MigrateVehicleMakersAsync(ct));

    [HttpGet("vehicle-models/status")]
    public async Task<ActionResult<MigrationStatus>> VehicleModelsStatus(CancellationToken ct)
        => Ok(await _svc.GetVehicleModelsStatusAsync(ct));

    [HttpPost("vehicle-models/delta")]
    public async Task<ActionResult<MigrationResult>> RunVehicleModelsDelta(CancellationToken ct)
        => Ok(await _svc.MigrateVehicleModelsAsync(ct));

    [HttpGet("vehicles/status")]
    public async Task<ActionResult<MigrationStatus>> VehiclesStatus(CancellationToken ct)
        => Ok(await _svc.GetVehiclesStatusAsync(ct));

    [HttpPost("vehicles/delta")]
    public async Task<ActionResult<MigrationResult>> RunVehiclesDelta(CancellationToken ct)
        => Ok(await _svc.MigrateNewerVehiclesAsync(ct));

    [HttpGet("relations/status")]
    public async Task<ActionResult<MigrationStatus>> RelationsStatus(CancellationToken ct)
        => Ok(await _svc.GetRelationsStatusAsync(ct));

    [HttpPost("relations/delta")]
    public async Task<ActionResult<MigrationResult>> RunRelationsDelta(CancellationToken ct)
        => Ok(await _svc.MigrateNewerRelationsAsync(ct));

    // One-shot: copies the print-overlay columns (Tip, BrojEUPotvrda, OznakaNaOdobrenie,
    // axle masses, trailer specs, etc.) from the legacy DB into the local Vehicles table
    // for every row that already exists locally. Safe to re-run.
    [HttpPost("vehicles/backfill-print-cols")]
    public async Task<ActionResult<BackfillResult>> BackfillVehiclePrintCols(CancellationToken ct)
        => Ok(await _svc.BackfillVehiclePrintColumnsAsync(ct));

    // One-shot: imports PaymentCategories + PaymentItems + PaymentItemParametars,
    // updates PaymentTypes flag columns from legacy (Prefix, PrintText, Fiskalna_kes...),
    // and seeds PaymentDocumentNumbers (per-station per-type sequence). Idempotent.
    [HttpPost("payments/catalog")]
    public async Task<ActionResult<PaymentCatalogMigrationResult>> MigratePaymentCatalog(CancellationToken ct)
        => Ok(await _svc.MigratePaymentCatalogAsync(ct));

    // Copies legacy PaymentDocumentsDetails.IdPriceCatalog (a PaymentItems FK,
    // misleadingly named) into local PaymentDocumentDetails.PaymentItemId.
    [HttpPost("payments/backfill-item-id")]
    public async Task<ActionResult<PaymentItemIdBackfillResult>> BackfillPaymentItemId(CancellationToken ct)
        => Ok(await _svc.BackfillPaymentItemIdAsync(ct));
}
