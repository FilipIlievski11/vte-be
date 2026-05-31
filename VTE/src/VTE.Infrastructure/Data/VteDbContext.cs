namespace VTE.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;
using VTE.Core.Lookups;

public class VteDbContext : DbContext
{
    public VteDbContext(DbContextOptions<VteDbContext> options) : base(options) { }

    // Core entities
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerContactPerson> CustomerContactPersons => Set<CustomerContactPerson>();
    public DbSet<CustomerBankAccount> CustomerBankAccounts => Set<CustomerBankAccount>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleAxle> VehicleAxles => Set<VehicleAxle>();
    public DbSet<VehicleTyre> VehicleTyres => Set<VehicleTyre>();
    public DbSet<VehicleAxleDistance> VehicleAxleDistances => Set<VehicleAxleDistance>();
    public DbSet<CustomerVehicleRelation> CustomerVehicleRelations => Set<CustomerVehicleRelation>();
    public DbSet<TechnicalExamReport> TechnicalExamReports => Set<TechnicalExamReport>();
    public DbSet<TechnicalExamReportDetail> TechnicalExamReportDetails => Set<TechnicalExamReportDetail>();
    public DbSet<TechnicalExamVisualError> TechnicalExamVisualErrors => Set<TechnicalExamVisualError>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestAttachment> RequestAttachments => Set<RequestAttachment>();
    public DbSet<RequestOwnershipProof> RequestOwnershipProofs => Set<RequestOwnershipProof>();
    public DbSet<RequestPaymentProof> RequestPaymentProofs => Set<RequestPaymentProof>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<DocumentAttachment> DocumentAttachments => Set<DocumentAttachment>();
    public DbSet<TrafficLicense> TrafficLicenses => Set<TrafficLicense>();
    public DbSet<TrafficLicenseExtension> TrafficLicenseExtensions => Set<TrafficLicenseExtension>();
    public DbSet<InternationalDrivingLicense> InternationalDrivingLicenses => Set<InternationalDrivingLicense>();
    public DbSet<InternationalDrivingLicenseCategory> InternationalDrivingLicenseCategories => Set<InternationalDrivingLicenseCategory>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<PaymentDocument> PaymentDocuments => Set<PaymentDocument>();
    public DbSet<PaymentLineItem> PaymentLineItems => Set<PaymentLineItem>();
    public DbSet<PaymentInstallment> PaymentInstallments => Set<PaymentInstallment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<RolePrivilege> RolePrivileges => Set<RolePrivilege>();
    public DbSet<TechnicalExamOrganization> TechnicalExamOrganizations => Set<TechnicalExamOrganization>();
    public DbSet<Company> Companies => Set<Company>();

    // Lookups
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<Street> Streets => Set<Street>();
    public DbSet<BusinessType> BusinessTypes => Set<BusinessType>();
    public DbSet<VehicleBodyType> VehicleBodyTypes => Set<VehicleBodyType>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<VehiclePaymentCategory> VehiclePaymentCategories => Set<VehiclePaymentCategory>();
    public DbSet<VehicleUseType> VehicleUseTypes => Set<VehicleUseType>();
    public DbSet<EngineType> EngineTypes => Set<EngineType>();
    public DbSet<EnginePowerSourceType> EnginePowerSourceTypes => Set<EnginePowerSourceType>();
    public DbSet<GearBoxType> GearBoxTypes => Set<GearBoxType>();
    public DbSet<BrakeType> BrakeTypes => Set<BrakeType>();
    public DbSet<SupportingType> SupportingTypes => Set<SupportingType>();
    public DbSet<EcoProgram> EcoPrograms => Set<EcoProgram>();
    public DbSet<VehicleMaker> VehicleMakers => Set<VehicleMaker>();
    public DbSet<VehicleModel> VehicleModels => Set<VehicleModel>();
    public DbSet<TireType> TireTypes => Set<TireType>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<RegistrationIssuer> RegistrationIssuers => Set<RegistrationIssuer>();
    public DbSet<DocumentType> DocumentTypes => Set<DocumentType>();
    public DbSet<DocumentTypeOption> DocumentTypeOptions => Set<DocumentTypeOption>();
    public DbSet<DocumentTypeOptionDetail> DocumentTypeOptionDetails => Set<DocumentTypeOptionDetail>();
    public DbSet<OwnershipProofType> OwnershipProofTypes => Set<OwnershipProofType>();
    public DbSet<PaymentProofType> PaymentProofTypes => Set<PaymentProofType>();
    public DbSet<AttachmentType> AttachmentTypes => Set<AttachmentType>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<PaymentCategory> PaymentCategories => Set<PaymentCategory>();
    public DbSet<PaymentCatalogItem> PaymentCatalogItems => Set<PaymentCatalogItem>();
    public DbSet<VATRate> VATRates => Set<VATRate>();
    public DbSet<TechnicalExamType> TechnicalExamTypes => Set<TechnicalExamType>();
    public DbSet<TechnicalExamVehiclePart> TechnicalExamVehicleParts => Set<TechnicalExamVehiclePart>();
    public DbSet<ExamDetailStatus> ExamDetailStatuses => Set<ExamDetailStatus>();
    public DbSet<RelationType> RelationTypes => Set<RelationType>();
    public DbSet<DrivingLicenseCategory> DrivingLicenseCategories => Set<DrivingLicenseCategory>();
    public DbSet<RequestType> RequestTypes => Set<RequestType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VteDbContext).Assembly);
    }
}
