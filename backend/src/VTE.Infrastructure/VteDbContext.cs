using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Attachments;
using VTE.Domain.Customers;
using VTE.Domain.Documents;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Domain.Reference;
using VTE.Domain.Requests;
using VTE.Domain.Stations;
using VTE.Domain.Vehicles;

namespace VTE.Infrastructure;

public class VteDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    private readonly ITenantContext? _tenant;

    public VteDbContext(DbContextOptions<VteDbContext> options, ITenantContext? tenant = null)
        : base(options)
    {
        _tenant = tenant;
    }

    // Auth / tenancy
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Operator> Operators => Set<Operator>();

    // Domain
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerContactPerson> CustomerContactPersons => Set<CustomerContactPerson>();
    public DbSet<CustomerBankAccount> CustomerBankAccounts => Set<CustomerBankAccount>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleRegistration> VehicleRegistrations => Set<VehicleRegistration>();
    public DbSet<VehicleAxle> VehicleAxles => Set<VehicleAxle>();
    public DbSet<VehicleAxleDistance> VehicleAxleDistances => Set<VehicleAxleDistance>();
    public DbSet<VehicleTyre> VehicleTyres => Set<VehicleTyre>();
    public DbSet<CustomerVehicleRelation> CustomerVehicleRelations => Set<CustomerVehicleRelation>();
    public DbSet<RequestType> RequestTypes => Set<RequestType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestVehicleOwnershipProof> RequestVehicleOwnershipProofs => Set<RequestVehicleOwnershipProof>();
    public DbSet<RequestPaymentProof> RequestPaymentProofs => Set<RequestPaymentProof>();
    public DbSet<InstallmentContract> InstallmentContracts => Set<InstallmentContract>();
    public DbSet<PaymentDocument> PaymentDocuments => Set<PaymentDocument>();
    public DbSet<PaymentDocumentDetail> PaymentDocumentDetails => Set<PaymentDocumentDetail>();
    public DbSet<PaymentDocumentInstallment> PaymentDocumentInstallments => Set<PaymentDocumentInstallment>();
    public DbSet<PaymentCategory> PaymentCategories => Set<PaymentCategory>();
    public DbSet<PaymentItem> PaymentItems => Set<PaymentItem>();
    public DbSet<PaymentItemParametar> PaymentItemParametars => Set<PaymentItemParametar>();
    public DbSet<PaymentDocumentNumber> PaymentDocumentNumbers => Set<PaymentDocumentNumber>();
    public DbSet<VTE.Domain.Payments.CustomerFinancialState> CustomerFinancialStates => Set<VTE.Domain.Payments.CustomerFinancialState>();
    public DbSet<TrafficLicence> TrafficLicences => Set<TrafficLicence>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<InternationalDrivingLicence> InternationalDrivingLicences => Set<InternationalDrivingLicence>();
    public DbSet<TechnicalExamReport> TechnicalExamReports => Set<TechnicalExamReport>();
    public DbSet<TechnicalExamReportDetail> TechnicalExamReportDetails => Set<TechnicalExamReportDetail>();
    public DbSet<TechnicalExamReportVisualError> TechnicalExamReportVisualErrors => Set<TechnicalExamReportVisualError>();

    // Attachments
    public DbSet<CustomerAttachment> CustomerAttachments => Set<CustomerAttachment>();
    public DbSet<VehicleAttachment> VehicleAttachments => Set<VehicleAttachment>();
    public DbSet<RequestAttachment> RequestAttachments => Set<RequestAttachment>();
    public DbSet<TechnicalExamReportAttachment> TechnicalExamReportAttachments => Set<TechnicalExamReportAttachment>();
    public DbSet<PaymentDocumentAttachment> PaymentDocumentAttachments => Set<PaymentDocumentAttachment>();

    // Reference data
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Street> Streets => Set<Street>();
    public DbSet<BusinessType> BusinessTypes => Set<BusinessType>();
    public DbSet<RegistrationIssuer> RegistrationIssuers => Set<RegistrationIssuer>();
    public DbSet<CustomerVehicleRelationType> CustomerVehicleRelationTypes => Set<CustomerVehicleRelationType>();
    public DbSet<VehicleBodyType> VehicleBodyTypes => Set<VehicleBodyType>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<VehicleCategoryRelation> VehicleCategoryRelations => Set<VehicleCategoryRelation>();
    public DbSet<VehicleCategoryRequiredField> VehicleCategoryRequiredFields => Set<VehicleCategoryRequiredField>();
    public DbSet<VehicleCategoryDisabledField> VehicleCategoryDisabledFields => Set<VehicleCategoryDisabledField>();
    public DbSet<VehicleUse> VehicleUses => Set<VehicleUse>();
    public DbSet<VehicleMaker> VehicleMakers => Set<VehicleMaker>();
    public DbSet<VehicleModel> VehicleModels => Set<VehicleModel>();
    public DbSet<VehicleEngineType> VehicleEngineTypes => Set<VehicleEngineType>();
    public DbSet<VehicleEnginePowerSourceType> VehicleEnginePowerSourceTypes => Set<VehicleEnginePowerSourceType>();
    public DbSet<VehicleEngineEcoProgram> VehicleEngineEcoPrograms => Set<VehicleEngineEcoProgram>();
    public DbSet<VehicleGearBox> VehicleGearBoxes => Set<VehicleGearBox>();
    public DbSet<VehicleBrake> VehicleBrakes => Set<VehicleBrake>();
    public DbSet<VehicleSupporting> VehicleSupportings => Set<VehicleSupporting>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<VehicleCategoryForPayments> VehicleCategoriesForPayments => Set<VehicleCategoryForPayments>();
    public DbSet<VehicleTireType> VehicleTireTypes => Set<VehicleTireType>();
    public DbSet<TechnicalExamOrganization> TechnicalExamOrganizations => Set<TechnicalExamOrganization>();
    public DbSet<TechnicalExamType> TechnicalExamTypes => Set<TechnicalExamType>();
    public DbSet<TechnicalExamVehiclePartCategory> TechnicalExamVehiclePartCategories => Set<TechnicalExamVehiclePartCategory>();
    public DbSet<TechnicalExamVehiclePart> TechnicalExamVehicleParts => Set<TechnicalExamVehiclePart>();
    public DbSet<TechnicalExamReportDetailStatus> TechnicalExamReportDetailStatuses => Set<TechnicalExamReportDetailStatus>();
    public DbSet<DrivingLicenceCategory> DrivingLicenceCategories => Set<DrivingLicenceCategory>();
    public DbSet<VehicleOwnershipProofType> VehicleOwnershipProofTypes => Set<VehicleOwnershipProofType>();
    public DbSet<PaymentProofType> PaymentProofTypes => Set<PaymentProofType>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<DDVCatalog> DDVCatalogs => Set<DDVCatalog>();
    public DbSet<CalculationItem> CalculationItems => Set<CalculationItem>();
    public DbSet<PriceCatalog> PriceCatalogs => Set<PriceCatalog>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // ===== Auth / tenancy =====
        b.Entity<Station>(e =>
        {
            e.ToTable("Stations");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Code).HasMaxLength(20).IsRequired();
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<Operator>(e =>
        {
            e.ToTable("Operators");
            e.HasKey(x => x.UserId);
            e.Property(x => x.UserId).HasMaxLength(450);
            e.Property(x => x.FullName).HasMaxLength(200).IsRequired();
            e.Property(x => x.EMBG).HasMaxLength(13);
            e.HasOne(x => x.User).WithOne().HasForeignKey<Operator>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        // ===== Domain =====
        b.Entity<Customer>(e =>
        {
            e.ToTable("Customers");
            e.HasKey(x => x.Id);
            e.Property(x => x.EMBG).HasMaxLength(13);
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Surname).HasMaxLength(100);
            e.Property(x => x.ParentName).HasMaxLength(100);
            e.Property(x => x.LivingAddressNumber).HasMaxLength(100);
            e.Property(x => x.BirthAddressNumber).HasMaxLength(100);
            e.Property(x => x.Occupation).HasMaxLength(50);
            e.Property(x => x.WorksInCompany).HasMaxLength(200);
            e.Property(x => x.PhoneNumber).HasMaxLength(20);
            e.Property(x => x.Fax).HasMaxLength(20);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.IDCardNumber).HasMaxLength(20);
            e.Property(x => x.PassportNumber).HasMaxLength(20);
            e.Property(x => x.DrivingLicenceNumber).HasMaxLength(20);
            e.Property(x => x.TaxNumber).HasMaxLength(15);
            e.Property(x => x.Status).HasMaxLength(50);
            e.Property(x => x.RowVersion).IsRowVersion();
            // Non-unique on purpose — the legacy system allows the same EMBG to appear
            // more than once (a person can re-register), and the migration must accept that.
            // Kept as a regular index for lookup performance on (StationId, EMBG).
            e.HasIndex(x => new { x.StationId, x.EMBG }).HasFilter("[EMBG] IS NOT NULL")
                .HasDatabaseName("IX_Customers_StationId_EMBG");
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<CustomerContactPerson>(e =>
        {
            e.ToTable("CustomerContactPersons"); e.HasKey(x => x.Id);
            e.Property(x => x.EMBG).HasMaxLength(13);
            e.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
            e.Property(x => x.Surname).HasMaxLength(50).IsRequired();
            e.Property(x => x.PhoneNumber).HasMaxLength(20);
            e.Property(x => x.MobileNumber).HasMaxLength(20);
            e.Property(x => x.Email).HasMaxLength(200);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<CustomerBankAccount>(e =>
        {
            e.ToTable("CustomerBankAccounts"); e.HasKey(x => x.Id);
            e.Property(x => x.BankAccount).HasMaxLength(50).IsRequired();
            e.Property(x => x.DeponentBank).HasMaxLength(50).IsRequired();
            e.Property(x => x.TaxNumber).HasMaxLength(15);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<Vehicle>(e =>
        {
            e.ToTable("Vehicles");
            e.HasKey(x => x.Id);
            e.Property(x => x.ShellNumber).HasMaxLength(17).IsRequired();
            e.Property(x => x.FirstRegistrationNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.LastRegistrationNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.VehicleModelAdding).HasMaxLength(200);
            e.Property(x => x.EngineNumber).HasMaxLength(50);
            e.Property(x => x.ColorCode).HasMaxLength(20);
            e.Property(x => x.Note).HasMaxLength(500);
            // Print-only legacy parity fields
            e.Property(x => x.Tip).HasMaxLength(150);
            e.Property(x => x.BrojEUPotvrda).HasMaxLength(50);
            e.Property(x => x.OznakaNaOdobrenie).HasMaxLength(50);
            e.Property(x => x.OznakaNaOdobrenieZaPriklucUred).HasMaxLength(100);
            e.Property(x => x.IdentifikacijaNaMotorMestoMetod).HasMaxLength(100);
            e.Property(x => x.TBrOdobrenieMehanPriklucok).HasMaxLength(100);
            e.Property(x => x.TMarkaMehanPriklucok).HasMaxLength(100);
            e.Property(x => x.TTipMehanPriklucok).HasMaxLength(100);
            e.Property(x => x.TZastitnaKabina).HasMaxLength(100);
            e.Property(x => x.TZastitnaRamka).HasMaxLength(100);
            e.Property(x => x.NoiseTechnicalSpec).HasMaxLength(100);
            e.Property(x => x.OdnosKwCcm).HasMaxLength(20);
            e.Property(x => x.EnginePowerOutPut).HasColumnType("decimal(18,2)");
            e.Property(x => x.NoiseStatic).HasColumnType("decimal(18,2)");
            e.Property(x => x.CO2).HasColumnType("decimal(18,4)");
            e.Property(x => x.MaxSpeed).HasColumnType("decimal(18,2)");
            e.Property(x => x.MaxKonstVkMasa).HasColumnType("decimal(18,2)");
            e.Property(x => x.MaxLegVkMasa).HasColumnType("decimal(18,2)");
            e.Property(x => x.MaxLegVkMasaGrupa).HasColumnType("decimal(18,2)");

            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => new { x.StationId, x.ShellNumber }).IsUnique().HasDatabaseName("UX_Vehicles_StationId_ShellNumber");
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<CustomerVehicleRelation>(e =>
        {
            e.ToTable("CustomerVehicleRelations");
            e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(200);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<VehicleRegistration>(e =>
        {
            e.ToTable("VehicleRegistrations");
            e.HasKey(x => x.Id);
            e.Property(x => x.RegistrationNumber).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.VehicleId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<VehicleAxle>(e =>
        {
            e.ToTable("VehicleAxles");
            e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(250);
            e.HasIndex(x => x.VehicleId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<VehicleAxleDistance>(e =>
        {
            e.ToTable("VehicleAxleDistances");
            e.HasKey(x => x.Id);
            e.Property(x => x.Distance).HasColumnType("decimal(18,3)");
            e.HasIndex(x => x.VehicleId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<VehicleTyre>(e =>
        {
            e.ToTable("VehicleTyres");
            e.HasKey(x => x.Id);
            e.Property(x => x.PositionNote).HasMaxLength(100);
            e.Property(x => x.Dimensions).HasMaxLength(50);
            e.Property(x => x.PressureFront).HasColumnType("decimal(8,2)");
            e.Property(x => x.PressureRear).HasColumnType("decimal(8,2)");
            e.HasIndex(x => x.VehicleId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<RequestType>(e =>
        {
            e.ToTable("RequestTypes");
            e.HasKey(x => x.Id);
            e.Property(x => x.TypeName).HasMaxLength(250).IsRequired();
            e.Property(x => x.TypeDescription).HasMaxLength(250);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<Request>(e =>
        {
            e.ToTable("Requests");
            e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(250);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasOne(x => x.RequestType).WithMany().HasForeignKey(x => x.RequestTypeId);
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<RequestVehicleOwnershipProof>(e =>
        {
            e.ToTable("RequestVehicleOwnershipProofs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).HasMaxLength(100);
            e.Property(x => x.Note).HasMaxLength(250);
            e.HasIndex(x => x.RequestId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<RequestPaymentProof>(e =>
        {
            e.ToTable("RequestPaymentProofs");
            e.HasKey(x => x.Id);
            e.Property(x => x.Number).HasMaxLength(100);
            e.Property(x => x.Note).HasMaxLength(250);
            e.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            e.HasIndex(x => x.RequestId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<InstallmentContract>(e =>
        {
            e.ToTable("InstallmentContracts");
            e.HasKey(x => x.Id);
            e.Property(x => x.ContractNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.GuarantorName).HasMaxLength(50);
            e.Property(x => x.GuarantorAddress).HasMaxLength(250);
            e.Property(x => x.GuarantorEMBG).HasMaxLength(20);
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<PaymentDocument>(e =>
        {
            e.ToTable("PaymentDocuments");
            e.HasKey(x => x.Id);
            e.Property(x => x.DocumentNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Note).HasMaxLength(150);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasMany(x => x.Details).WithOne().HasForeignKey(d => d.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(x => x.Installments).WithOne().HasForeignKey(i => i.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<PaymentDocumentDetail>(e =>
        {
            e.ToTable("PaymentDocumentDetails");
            e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(150);
            e.Property(x => x.NotePrePayed).HasMaxLength(150);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<VTE.Domain.Payments.CustomerFinancialState>(e =>
        {
            e.ToTable("CustomerFinancialState"); e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(250);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<PaymentDocumentInstallment>(e =>
        {
            e.ToTable("PaymentDocumentInstallments");
            e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(150);
            e.Property(x => x.RowVersion).IsRowVersion();
        });

        b.Entity<PaymentCategory>(e =>
        {
            e.ToTable("PaymentCategories"); e.HasKey(x => x.Id);
            e.Property(x => x.CategoryName).HasMaxLength(250).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => new { x.TriggerdByRequest, x.IsActive }).HasDatabaseName("IX_PaymentCategories_Trigger_Request");
        });

        b.Entity<PaymentItem>(e =>
        {
            e.ToTable("PaymentItems"); e.HasKey(x => x.Id);
            e.Property(x => x.ItemName).HasMaxLength(250).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => new { x.PaymentCategoryId, x.VehicleCategoryForPaymentsId }).HasDatabaseName("IX_PaymentItems_Category_VehCat");
        });

        b.Entity<PaymentItemParametar>(e =>
        {
            e.ToTable("PaymentItemParametars"); e.HasKey(x => x.Id);
            e.Property(x => x.ParametarName).HasMaxLength(250).IsRequired();
            e.Property(x => x.VehicleField).HasMaxLength(150);
            e.Property(x => x.ParametarFrom).HasColumnType("decimal(18,4)");
            e.Property(x => x.ParametarTo).HasColumnType("decimal(18,4)");
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => x.PaymentItemId).HasDatabaseName("IX_PaymentItemParametars_Item");
        });

        b.Entity<PaymentDocumentNumber>(e =>
        {
            e.ToTable("PaymentDocumentNumbers"); e.HasKey(x => x.Id);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => new { x.StationId, x.PaymentTypeId, x.IsTechExamReport })
             .IsUnique().HasDatabaseName("UX_PaymentDocumentNumbers_Station_Type_Tech");
        });

        b.Entity<TrafficLicence>(e =>
        {
            e.ToTable("TrafficLicences");
            e.HasKey(x => x.Id);
            e.Property(x => x.TrafficLicenceNumber).HasMaxLength(50);
            e.Property(x => x.Note).HasMaxLength(250);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<Permission>(e =>
        {
            e.ToTable("Permissions"); e.HasKey(x => x.Id);
            e.Property(x => x.PermissionNumber).HasMaxLength(50);
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<InternationalDrivingLicence>(e =>
        {
            e.ToTable("InternationalDrivingLicences"); e.HasKey(x => x.Id);
            e.Property(x => x.LicenceNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        b.Entity<TechnicalExamReport>(e =>
        {
            e.ToTable("TechnicalExamReports");
            e.HasKey(x => x.Id);
            e.Property(x => x.RegNumber).HasMaxLength(50);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });

        // ===== Attachments =====
        ConfigureAttachment<CustomerAttachment>(b, "CustomerAttachments");
        ConfigureAttachment<VehicleAttachment>(b, "VehicleAttachments");
        ConfigureAttachment<RequestAttachment>(b, "RequestAttachments");
        ConfigureAttachment<TechnicalExamReportAttachment>(b, "TechnicalExamReportAttachments");
        ConfigureAttachment<PaymentDocumentAttachment>(b, "PaymentDocumentAttachments");

        // ===== Reference data — uniform mapping by table-name convention =====
        ConfigureRef<Country>(b, "Countries", e => { e.Property(x => x.Iso2).HasMaxLength(5); e.Property(x => x.Iso3).HasMaxLength(5); e.Property(x => x.Citizenship).HasMaxLength(100); });
        ConfigureRef<Community>(b, "Communities", e => { e.Property(x => x.CommunityCode).HasMaxLength(50); e.Property(x => x.RegistrationCode).HasMaxLength(50); });
        ConfigureRef<City>(b, "Cities", e => { e.Property(x => x.PostalCode).HasMaxLength(20); e.Property(x => x.Description).HasMaxLength(250); });
        ConfigureRef<Street>(b, "Streets", e => e.Property(x => x.Name).HasMaxLength(200));
        ConfigureRef<BusinessType>(b, "BusinessTypes", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<RegistrationIssuer>(b, "RegistrationIssuers", e => e.Property(x => x.Name).HasMaxLength(200));
        ConfigureRef<CustomerVehicleRelationType>(b, "CustomerVehicleRelationTypes");
        ConfigureRef<VehicleBodyType>(b, "VehicleBodyTypes", e => { e.Property(x => x.Code).HasMaxLength(20); e.Property(x => x.OldName).HasMaxLength(100); });
        ConfigureRef<VehicleCategory>(b, "VehicleCategories", e =>
        {
            e.Property(x => x.Name).HasMaxLength(50);
            e.Property(x => x.Code).HasMaxLength(10);
            e.Property(x => x.OldName).HasMaxLength(50);
            e.Property(x => x.Mksjus).HasMaxLength(50);
            e.Property(x => x.Iso).HasMaxLength(50);
            e.Property(x => x.MksjusDescription).HasMaxLength(250);
            e.Property(x => x.PicturePath).HasMaxLength(250);
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.DetailDescription).HasColumnType("nvarchar(max)");
        });
        b.Entity<VehicleCategoryRelation>(e =>
        {
            e.ToTable("VehicleCategoryRelations");
            e.HasKey(x => x.Id);
            e.Property(x => x.DetailDescription).HasMaxLength(250);
            e.HasIndex(x => x.CategoryId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<VehicleCategoryRequiredField>(e =>
        {
            e.ToTable("VehicleCategoryRequiredFields");
            e.HasKey(x => x.Id);
            e.Property(x => x.FieldName).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.CategoryId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<VehicleCategoryDisabledField>(e =>
        {
            e.ToTable("VehicleCategoryDisabledFields");
            e.HasKey(x => x.Id);
            e.Property(x => x.FieldName).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.CategoryId);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        ConfigureRef<VehicleUse>(b, "VehicleUses");
        ConfigureRef<VehicleMaker>(b, "VehicleMakers");
        ConfigureRef<VehicleModel>(b, "VehicleModels", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<VehicleEngineType>(b, "VehicleEngineTypes");
        ConfigureRef<VehicleEnginePowerSourceType>(b, "VehicleEnginePowerSourceTypes");
        ConfigureRef<VehicleEngineEcoProgram>(b, "VehicleEngineEcoPrograms", e => e.Property(x => x.Name).HasMaxLength(50));
        ConfigureRef<VehicleGearBox>(b, "VehicleGearBoxes", e => e.Property(x => x.Name).HasMaxLength(50));
        ConfigureRef<VehicleBrake>(b, "VehicleBrakes");
        ConfigureRef<VehicleSupporting>(b, "VehicleSupportings");
        ConfigureRef<Color>(b, "Colors", e => { e.Property(x => x.Name).HasMaxLength(50); e.Property(x => x.HexCode).HasMaxLength(7); });
        ConfigureRef<VehicleCategoryForPayments>(b, "VehicleCategoriesForPayments", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<VehicleTireType>(b, "VehicleTireTypes", e => e.Property(x => x.Dimensions).HasMaxLength(50));
        ConfigureRef<TechnicalExamOrganization>(b, "TechnicalExamOrganizations", e => { e.Property(x => x.Name).HasMaxLength(200); e.Property(x => x.Address).HasMaxLength(250); e.Property(x => x.TaxNumber).HasMaxLength(15); e.Property(x => x.Phone).HasMaxLength(50); });
        ConfigureRef<TechnicalExamType>(b, "TechnicalExamTypes", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<TechnicalExamVehiclePartCategory>(b, "TechnicalExamVehiclePartCategories", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<TechnicalExamVehiclePart>(b, "TechnicalExamVehicleParts", e => e.Property(x => x.Name).HasMaxLength(200));
        ConfigureRef<TechnicalExamReportDetailStatus>(b, "TechnicalExamReportDetailStatuses", e => e.Property(x => x.Name).HasMaxLength(50));

        b.Entity<TechnicalExamReportDetail>(e =>
        {
            e.ToTable("TechnicalExamReportDetails"); e.HasKey(x => x.Id);
            e.Property(x => x.Note).HasMaxLength(150);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => x.TechnicalExamReportId);
        });
        b.Entity<TechnicalExamReportVisualError>(e =>
        {
            e.ToTable("TechnicalExamReportVisualErrors"); e.HasKey(x => x.Id);
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.Property(x => x.Severity).HasMaxLength(50);
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasIndex(x => x.TechnicalExamReportId);
        });
        ConfigureRef<DrivingLicenceCategory>(b, "DrivingLicenceCategories", e => { e.Property(x => x.Name).HasMaxLength(20); e.Property(x => x.Description).HasMaxLength(200); });
        ConfigureRef<VehicleOwnershipProofType>(b, "VehicleOwnershipProofTypes", e => e.Property(x => x.Name).HasMaxLength(150));
        ConfigureRef<PaymentProofType>(b, "PaymentProofTypes", e => e.Property(x => x.Name).HasMaxLength(150));

        b.Entity<PaymentType>(e =>
        {
            e.ToTable("PaymentTypes"); e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<DDVCatalog>(e =>
        {
            e.ToTable("DDVCatalog"); e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
        });
        b.Entity<CalculationItem>(e =>
        {
            e.ToTable("CalculationItems"); e.HasKey(x => x.Id);
            e.Property(x => x.ItemName).HasMaxLength(150).IsRequired();
            e.Property(x => x.BankAccount).HasMaxLength(50).IsRequired();
            e.Property(x => x.Bank).HasMaxLength(150).IsRequired();
            e.Property(x => x.Form).HasMaxLength(50).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });
        b.Entity<PriceCatalog>(e =>
        {
            e.ToTable("PriceCatalog"); e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            e.HasQueryFilter(x => _tenant == null || _tenant.IsAdministrator || _tenant.StationId == null || x.StationId == _tenant.StationId);
        });
    }

    private static void ConfigureAttachment<T>(ModelBuilder b, string tableName) where T : VTE.Domain.Attachments.AttachmentBase
    {
        b.Entity<T>(e =>
        {
            e.ToTable(tableName);
            e.HasKey(x => x.Id);
            e.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            e.Property(x => x.ContentType).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.RowVersion).IsRowVersion();
        });
    }

    // Generic REF mapper: all fields uniform, optional per-table tweaks via callback.
    private static void ConfigureRef<T>(ModelBuilder b, string tableName, Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T>>? extra = null)
        where T : RefBaseInt
    {
        b.Entity<T>(e =>
        {
            e.ToTable(tableName);
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.RowVersion).IsRowVersion();
            extra?.Invoke(e);
        });
    }

    public override int SaveChanges()
    {
        SetAuditTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetAuditTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void SetAuditTimestamps()
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<VTE.Domain.Common.AuditableEntity>())
        {
            if (entry.State == EntityState.Added) { entry.Entity.CreatedUtc = now; entry.Entity.LastModifiedUtc = now; }
            else if (entry.State == EntityState.Modified) entry.Entity.LastModifiedUtc = now;
        }
    }
}

public interface ITenantContext
{
    int? StationId { get; }
    bool IsAdministrator { get; }
}
