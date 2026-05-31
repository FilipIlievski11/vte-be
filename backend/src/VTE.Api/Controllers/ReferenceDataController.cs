using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Payments;
using VTE.Domain.Reference;
using VTE.Infrastructure;

namespace VTE.Api.Controllers;

// Read-only reference-data lookups. Authenticated users can read.
// Write endpoints to be added later (Administrator-only).
[ApiController]
[Route("api/ref")]
[Authorize]
public class ReferenceDataController : ControllerBase
{
    private readonly VteDbContext _db;
    public ReferenceDataController(VteDbContext db) => _db = db;

    public record RefItem(int Id, string Name);
    public record RefItemWithCode(int Id, string Name, string? Code);

    private static IQueryable<T> Active<T>(IQueryable<T> q) where T : RefBaseInt
        => q.Where(x => x.IsActive);

    [HttpGet("countries")]                               public async Task<List<RefItem>> Countries()
        => await Active(_db.Countries).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("communities")]                              public async Task<List<RefItem>> Communities()
        => await Active(_db.Communities).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("cities")]                                  public async Task<List<RefItem>> Cities()
        => await Active(_db.Cities).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("streets")]                                 public async Task<List<RefItem>> Streets([FromQuery] int? cityId)
        => await Active(_db.Streets).Where(x => cityId == null || x.CityId == cityId).OrderBy(x => x.Name)
            .Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("business-types")]                          public async Task<List<RefItem>> BusinessTypes()
        => await Active(_db.BusinessTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("registration-issuers")]                    public async Task<List<RefItem>> RegistrationIssuers()
        => await Active(_db.RegistrationIssuers).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("customer-vehicle-relation-types")]         public async Task<List<RefItem>> CustomerVehicleRelationTypes()
        => await Active(_db.CustomerVehicleRelationTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-body-types")]                      public async Task<List<RefItem>> VehicleBodyTypes()
        => await Active(_db.VehicleBodyTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-categories")]                      public async Task<List<RefItemWithCode>> VehicleCategories()
        => await Active(_db.VehicleCategories).OrderBy(x => x.Name).Select(x => new RefItemWithCode(x.Id, x.Name, x.Code)).ToListAsync();

    [HttpGet("vehicle-uses")]                            public async Task<List<RefItem>> VehicleUses()
        => await Active(_db.VehicleUses).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-makers")]                          public async Task<List<RefItem>> VehicleMakers()
        => await Active(_db.VehicleMakers).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-models")]                          public async Task<List<RefItem>> VehicleModels([FromQuery] int? makerId)
        => await Active(_db.VehicleModels).Where(x => makerId == null || x.VehicleMakerId == makerId).OrderBy(x => x.Name)
            .Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-engine-types")]                    public async Task<List<RefItem>> VehicleEngineTypes()
        => await Active(_db.VehicleEngineTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-engine-power-source-types")]       public async Task<List<RefItem>> VehicleEnginePowerSourceTypes()
        => await Active(_db.VehicleEnginePowerSourceTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-engine-eco-programs")]             public async Task<List<RefItem>> VehicleEngineEcoPrograms()
        => await Active(_db.VehicleEngineEcoPrograms).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-gearboxes")]                       public async Task<List<RefItem>> VehicleGearBoxes()
        => await Active(_db.VehicleGearBoxes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-brakes")]                          public async Task<List<RefItem>> VehicleBrakes()
        => await Active(_db.VehicleBrakes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-supportings")]                     public async Task<List<RefItem>> VehicleSupportings()
        => await Active(_db.VehicleSupportings).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("colors")]                                  public async Task<List<RefItem>> Colors()
        => await Active(_db.Colors).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("vehicle-categories-for-payments")]         public async Task<List<RefItem>> VehicleCategoriesForPayments()
        => await Active(_db.VehicleCategoriesForPayments).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("technical-exam-organizations")]            public async Task<List<RefItem>> TechnicalExamOrganizations()
        => await Active(_db.TechnicalExamOrganizations).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("technical-exam-types")]                    public async Task<List<RefItem>> TechnicalExamTypes()
        => await Active(_db.TechnicalExamTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("technical-exam-vehicle-parts")]            public async Task<List<RefItem>> TechnicalExamVehicleParts()
        => await Active(_db.TechnicalExamVehicleParts).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("technical-exam-vehicle-part-categories")]  public async Task<List<RefItem>> TechnicalExamVehiclePartCategories()
        => await Active(_db.TechnicalExamVehiclePartCategories).OrderBy(x => x.SortOrder).ThenBy(x => x.Name)
            .Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    public record PartsTreeNode(int Id, string Name, List<PartLeaf> Parts);
    public record PartLeaf(int Id, string Name);

    [HttpGet("exam-parts-tree")]
    public async Task<List<PartsTreeNode>> ExamPartsTree()
    {
        var cats = await _db.TechnicalExamVehiclePartCategories.Where(c => c.IsActive)
            .OrderBy(c => c.SortOrder).ThenBy(c => c.Name)
            .Select(c => new { c.Id, c.Name }).ToListAsync();
        var parts = await _db.TechnicalExamVehicleParts.Where(p => p.IsActive)
            .OrderBy(p => p.SortOrder).ThenBy(p => p.Name)
            .Select(p => new { p.Id, p.Name, p.CategoryId }).ToListAsync();
        return cats.Select(c => new PartsTreeNode(c.Id, c.Name,
            parts.Where(p => p.CategoryId == c.Id).Select(p => new PartLeaf(p.Id, p.Name)).ToList())).ToList();
    }

    [HttpGet("technical-exam-report-detail-statuses")]   public async Task<List<RefItem>> TechnicalExamReportDetailStatuses()
        => await Active(_db.TechnicalExamReportDetailStatuses).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("driving-licence-categories")]              public async Task<List<RefItem>> DrivingLicenceCategories()
        => await Active(_db.DrivingLicenceCategories).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    public record PaymentTypeDto(int Id, string Name, bool IsInvoice, bool IsCash, bool IsFiscalCard,
        bool IsAccount, bool IsInstallments, string? Prefix);
    [HttpGet("payment-types")]
    public async Task<List<PaymentTypeDto>> PaymentTypes()
        => await _db.PaymentTypes.Where(x => x.IsActive).OrderBy(x => x.Name)
            .Select(x => new PaymentTypeDto(x.Id, x.Name, x.IsInvoice, x.IsCash, x.IsFiscalCard, x.IsAccount, x.IsInstallments, x.Prefix))
            .ToListAsync();

    [HttpGet("vehicle-ownership-proof-types")]           public async Task<List<RefItem>> VehicleOwnershipProofTypes()
        => await Active(_db.VehicleOwnershipProofTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    [HttpGet("payment-proof-types")]                     public async Task<List<RefItem>> PaymentProofTypes()
        => await Active(_db.PaymentProofTypes).OrderBy(x => x.Name).Select(x => new RefItem(x.Id, x.Name)).ToListAsync();

    public record PaymentCategoryDto(int Id, string CategoryName, int? DDVId, int? CalculationItemId,
        bool AllowDiscount, bool TriggerdByRequest, bool TriggerdByTechnicalExam, bool TriggerdByTrafficLicence,
        bool TriggerdByPermissionForVehicle, bool TriggerdByInternationalDriverLicence, bool TriggerdByIrregularTechnicalExam,
        int? VisibleOrder);
    [HttpGet("payment-categories")]
    public async Task<List<PaymentCategoryDto>> PaymentCategories()
        => await _db.PaymentCategories.Where(x => x.IsActive).OrderBy(x => x.VisibleOrder ?? 0).ThenBy(x => x.CategoryName)
            .Select(x => new PaymentCategoryDto(x.Id, x.CategoryName, x.DDVId, x.CalculationItemId,
                x.AllowDiscount, x.TriggerdByRequest, x.TriggerdByTechnicalExam, x.TriggerdByTrafficLicence,
                x.TriggerdByPermissionForVehicle, x.TriggerdByInternationalDriverLicence, x.TriggerdByIrregularTechnicalExam,
                x.VisibleOrder)).ToListAsync();

    public record PaymentItemDto(int Id, int PaymentCategoryId, int? VehicleCategoryForPaymentsId, string ItemName);
    [HttpGet("payment-items")]
    public async Task<List<PaymentItemDto>> PaymentItems([FromQuery] int? categoryId, [FromQuery] int? vehicleCategoryId)
    {
        var q = _db.PaymentItems.Where(x => x.IsActive);
        if (categoryId is not null)        q = q.Where(x => x.PaymentCategoryId == categoryId);
        if (vehicleCategoryId is not null) q = q.Where(x => x.VehicleCategoryForPaymentsId == vehicleCategoryId);
        return await q.OrderBy(x => x.ItemName)
            .Select(x => new PaymentItemDto(x.Id, x.PaymentCategoryId, x.VehicleCategoryForPaymentsId, x.ItemName))
            .ToListAsync();
    }

    public record PaymentItemParametarDto(int Id, int PaymentItemId, string ParametarName, string? VehicleField,
        decimal? ParametarFrom, decimal? ParametarTo, decimal Price, bool IsOptional);
    [HttpGet("payment-item-parametars")]
    public async Task<List<PaymentItemParametarDto>> PaymentItemParametars([FromQuery] int? itemId)
    {
        var q = _db.PaymentItemParametars.Where(x => x.IsActive);
        if (itemId is not null) q = q.Where(x => x.PaymentItemId == itemId);
        return await q.OrderBy(x => x.PaymentItemId).ThenBy(x => x.Id)
            .Select(x => new PaymentItemParametarDto(x.Id, x.PaymentItemId, x.ParametarName, x.VehicleField,
                x.ParametarFrom, x.ParametarTo, x.Price, x.IsOptional)).ToListAsync();
    }
}
