using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VTE.Domain.Clients;
using VTE.Domain.Companies;
using VTE.Domain.Geography;
using VTE.Domain.Identity;
using VTE.Domain.Payments;
using VTE.Domain.References;
using VTE.Domain.Requests;
using VTE.Domain.Stations;
using VTE.Domain.TechnicalExams;
using VTE.Domain.Vehicles;
using VTE.Infrastructure.Tenancy;

namespace VTE.Infrastructure.Persistence;

public class VteDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly ITenantContext _tenant;

    public VteDbContext(DbContextOptions<VteDbContext> options, ITenantContext tenant)
        : base(options)
    {
        _tenant = tenant;
    }

    // Business entity sets
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Station> Stations => Set<Station>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Community> Communities => Set<Community>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Citizenship> Citizenships => Set<Citizenship>();
    public DbSet<DocumentIssuer> DocumentIssuers => Set<DocumentIssuer>();
    public DbSet<PersonalDataType> PersonalDataTypes => Set<PersonalDataType>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientPersonalData> ClientPersonalData => Set<ClientPersonalData>();

    // Vehicle module — owned by EF (created via AddVehicles migration).
    public DbSet<VehicleBodyType> VehicleBodyTypes => Set<VehicleBodyType>();
    public DbSet<VehicleCategory> VehicleCategories => Set<VehicleCategory>();
    public DbSet<VehicleMaker> VehicleMakers => Set<VehicleMaker>();
    public DbSet<VehicleModel> VehicleModels => Set<VehicleModel>();
    public DbSet<VehicleColor> VehicleColors => Set<VehicleColor>();
    public DbSet<VehicleFuel> VehicleFuels => Set<VehicleFuel>();
    public DbSet<VehicleEcoProgram> VehicleEcoPrograms => Set<VehicleEcoProgram>();
    public DbSet<VehicleEngineType> VehicleEngineTypes => Set<VehicleEngineType>();
    public DbSet<VehiclePaymentCategory> VehiclePaymentCategories => Set<VehiclePaymentCategory>();
    public DbSet<ClientVehicleRelationType> ClientVehicleRelationTypes => Set<ClientVehicleRelationType>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<VehicleRegistration> VehicleRegistrations => Set<VehicleRegistration>();
    public DbSet<ClientVehicleRelation> ClientVehicleRelations => Set<ClientVehicleRelation>();

    // Request module — EF-owned (created via AddRequests migration).
    public DbSet<RequestType> RequestTypes => Set<RequestType>();
    public DbSet<RequestDocumentPrint> RequestDocumentPrints => Set<RequestDocumentPrint>();
    public DbSet<RequestOwnershipProofType> RequestOwnershipProofTypes => Set<RequestOwnershipProofType>();
    public DbSet<RequestPaymentProofType> RequestPaymentProofTypes => Set<RequestPaymentProofType>();
    public DbSet<RequestAttachmentType> RequestAttachmentTypes => Set<RequestAttachmentType>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<RequestOwnershipProof> RequestOwnershipProofs => Set<RequestOwnershipProof>();
    public DbSet<RequestPaymentProof> RequestPaymentProofs => Set<RequestPaymentProof>();
    public DbSet<RequestAttachment> RequestAttachments => Set<RequestAttachment>();

    // Technical Exam module — EF-owned (created via AddTechnicalExams migration).
    public DbSet<TechnicalExamType> TechnicalExamTypes => Set<TechnicalExamType>();
    public DbSet<TechnicalExamOrganization> TechnicalExamOrganizations => Set<TechnicalExamOrganization>();
    public DbSet<TechnicalExamVehiclePart> TechnicalExamVehicleParts => Set<TechnicalExamVehiclePart>();
    public DbSet<TechnicalExamDetailStatus> TechnicalExamDetailStatuses => Set<TechnicalExamDetailStatus>();
    public DbSet<TechnicalExamReport> TechnicalExamReports => Set<TechnicalExamReport>();
    public DbSet<TechnicalExamReportDetail> TechnicalExamReportDetails => Set<TechnicalExamReportDetail>();

    // Payment module — EF-owned (created via AddPayments migration).
    public DbSet<VatRate> VatRates => Set<VatRate>();
    public DbSet<PaymentType> PaymentTypes => Set<PaymentType>();
    public DbSet<PriceCatalog> PriceCatalogs => Set<PriceCatalog>();
    public DbSet<InstallmentAgreement> InstallmentAgreements => Set<InstallmentAgreement>();
    public DbSet<PaymentDocument> PaymentDocuments => Set<PaymentDocument>();
    public DbSet<PaymentDocumentLine> PaymentDocumentLines => Set<PaymentDocumentLine>();
    public DbSet<InstallmentSchedule> InstallmentSchedules => Set<InstallmentSchedule>();
    public DbSet<CustomerDebt> CustomerDebts => Set<CustomerDebt>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // ------------------------------------------------------------------
        // Identity table names (use the AspNet* defaults — those are NEW
        // tables created by the InitialIdentity migration into the VTE DB).
        // ------------------------------------------------------------------
        b.Entity<ApplicationUser>(e =>
        {
            e.Property(u => u.FullName).HasMaxLength(200);
            // CompanyId is just a discriminator column on AspNetUsers — we deliberately
            // do NOT emit a FK constraint to Company because the Company table is
            // ExcludeFromMigrations (the project does not own its schema). The application
            // enforces tenant scoping via JWT claim + EF query filter, not via a DB FK.
        });

        // ------------------------------------------------------------------
        // Existing business tables — EXCLUDED from migrations because they
        // were created by hand against the live VTE database. EF still
        // queries/updates them via the mappings below.
        // ------------------------------------------------------------------

        b.Entity<Company>(e =>
        {
            e.ToTable("Company", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(255).IsRequired();
        });

        b.Entity<Station>(e =>
        {
            e.ToTable("Station", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("smallint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            // Tenant filter — admins bypass.
            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<Country>(e =>
        {
            e.ToTable("Country", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("smallint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(150);
            e.Property(x => x.ShortName).HasMaxLength(150);
        });

        b.Entity<Community>(e =>
        {
            e.ToTable("Community", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.CountryId).HasColumnType("smallint");
            e.Property(x => x.Code).HasMaxLength(50);
            e.Property(x => x.PlateNumberPrefix).HasMaxLength(2);
            e.HasOne<Country>().WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<City>(e =>
        {
            e.ToTable("City", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.PostalCode).HasMaxLength(20).IsRequired();
            e.HasOne<Community>().WithMany().HasForeignKey(x => x.CommunityId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Citizenship>(e =>
        {
            e.ToTable("Citizenship", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.CountryId).HasColumnType("smallint");
            e.Property(x => x.Name).HasMaxLength(50).IsRequired();
            e.HasOne<Country>().WithMany().HasForeignKey(x => x.CountryId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<DocumentIssuer>(e =>
        {
            e.ToTable("DocumentIssuer", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<PersonalDataType>(e =>
        {
            e.ToTable("PersonalDataType", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            // PK is NOT identity in the schema — values are seeded by hand.
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedNever();
            e.Property(x => x.Name).HasMaxLength(30).IsRequired();
        });

        b.Entity<Client>(e =>
        {
            e.ToTable("Client", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.CitizenshipId).HasColumnType("tinyint");
            e.Property(x => x.FirstName).HasMaxLength(100);
            e.Property(x => x.MiddleName).HasMaxLength(100);
            e.Property(x => x.LastName).HasMaxLength(100);
            e.Property(x => x.MB).HasMaxLength(13);
            e.Property(x => x.Address).HasMaxLength(100);
            e.Property(x => x.TaxNumber).HasMaxLength(50);
            e.Property(x => x.PhoneNumber).HasMaxLength(20);
            e.Property(x => x.Email).HasMaxLength(100);
            e.Property(x => x.Note).HasMaxLength(250);
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<City>().WithMany().HasForeignKey(x => x.CityId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Citizenship>().WithMany().HasForeignKey(x => x.CitizenshipId).OnDelete(DeleteBehavior.Restrict);
            // Tenant filter — admins bypass.
            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<ClientPersonalData>(e =>
        {
            e.ToTable("ClientPersonalData", t => t.ExcludeFromMigrations());
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.PersonalDataTypeId).HasColumnType("tinyint");
            e.Property(x => x.DocumentIssuerId).HasColumnType("tinyint");
            e.Property(x => x.Number).HasMaxLength(100).IsRequired();
            e.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<PersonalDataType>().WithMany().HasForeignKey(x => x.PersonalDataTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<DocumentIssuer>().WithMany().HasForeignKey(x => x.DocumentIssuerId).OnDelete(DeleteBehavior.Restrict);
        });

        // ------------------------------------------------------------------
        // Vehicle module — EF-owned (created by the AddVehicles migration).
        // ------------------------------------------------------------------

        b.Entity<VehicleBodyType>(e =>
        {
            e.ToTable("VehicleBodyType");
            e.Property(x => x.Code).HasMaxLength(20);
            e.Property(x => x.Name).HasMaxLength(500).IsRequired();
        });

        b.Entity<VehicleCategory>(e =>
        {
            e.ToTable("VehicleCategory");
            e.Property(x => x.Code).HasMaxLength(20);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        b.Entity<VehicleMaker>(e =>
        {
            e.ToTable("VehicleMaker");
            e.Property(x => x.Name).HasMaxLength(500).IsRequired();
            e.Property(x => x.Trademark).HasMaxLength(500);
            // No FK to Country — keep it loose; CountryId may reference legacy ids
            // that weren't preserved during migration. Index it though.
            e.HasIndex(x => x.CountryId);
        });

        b.Entity<VehicleModel>(e =>
        {
            e.ToTable("VehicleModel");
            e.Property(x => x.Code).HasMaxLength(500);
            e.Property(x => x.Name).HasMaxLength(500).IsRequired();
            e.HasOne<VehicleMaker>().WithMany()
              .HasForeignKey(x => x.MakerId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<VehicleColor>(e =>
        {
            e.ToTable("VehicleColor");
            e.Property(x => x.Code).HasMaxLength(100);
            e.Property(x => x.Name).HasMaxLength(500).IsRequired();
        });

        b.Entity<VehicleFuel>(e =>
        {
            e.ToTable("VehicleFuel");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<VehicleEcoProgram>(e =>
        {
            e.ToTable("VehicleEcoProgram");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<VehicleEngineType>(e =>
        {
            e.ToTable("VehicleEngineType");
            e.Property(x => x.Code).HasMaxLength(200);
            e.Property(x => x.Name).HasMaxLength(500).IsRequired();
        });

        b.Entity<VehiclePaymentCategory>(e =>
        {
            e.ToTable("VehiclePaymentCategory");
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<ClientVehicleRelationType>(e =>
        {
            e.ToTable("ClientVehicleRelationType");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
        });

        b.Entity<Vehicle>(e =>
        {
            e.ToTable("Vehicle");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Vin).HasMaxLength(40).IsRequired();
            e.Property(x => x.EngineNumber).HasMaxLength(40);
            e.Property(x => x.Plate).HasMaxLength(20);
            e.Property(x => x.TrailerMassWithBrakesKg).HasMaxLength(40);
            e.Property(x => x.TrailerMassWithoutBrakesKg).HasMaxLength(40);
            e.Property(x => x.TypeText).HasMaxLength(300);
            e.Property(x => x.ModelVariant).HasMaxLength(400);
            e.Property(x => x.ApprovalMark).HasMaxLength(100);
            e.Property(x => x.Note).HasMaxLength(1000);

            // Tenant FK + Restrict
            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            // Lookups — Restrict deletes, nullable FKs.
            e.HasOne<VehicleCategory>().WithMany().HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleBodyType>().WithMany().HasForeignKey(x => x.BodyTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleModel>().WithMany().HasForeignKey(x => x.ModelId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleColor>().WithMany().HasForeignKey(x => x.PrimaryColorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleColor>().WithMany().HasForeignKey(x => x.SecondaryColorId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Country>().WithMany().HasForeignKey(x => x.MadeCountryId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleFuel>().WithMany().HasForeignKey(x => x.FuelId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleFuel>().WithMany().HasForeignKey(x => x.SecondFuelId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleEngineType>().WithMany().HasForeignKey(x => x.EngineTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehicleEcoProgram>().WithMany().HasForeignKey(x => x.EcoProgramId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<VehiclePaymentCategory>().WithMany().HasForeignKey(x => x.PaymentCategoryId).OnDelete(DeleteBehavior.Restrict);

            // Useful indexes for searching
            e.HasIndex(x => x.Vin);
            e.HasIndex(x => x.Plate);
            e.HasIndex(x => x.CompanyId);

            // Tenant filter — admins bypass
            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<VehicleRegistration>(e =>
        {
            e.ToTable("VehicleRegistration");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.IssuerId).HasColumnType("tinyint");
            e.Property(x => x.PlateNumber).HasMaxLength(20).IsRequired();

            e.HasOne<Vehicle>().WithMany().HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<DocumentIssuer>().WithMany().HasForeignKey(x => x.IssuerId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.VehicleId);
            e.HasIndex(x => x.PlateNumber);
        });

        b.Entity<ClientVehicleRelation>(e =>
        {
            e.ToTable("ClientVehicleRelation");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.RelationTypeId).HasColumnType("tinyint");
            e.Property(x => x.StartNote).HasMaxLength(500);
            e.Property(x => x.EndNote).HasMaxLength(500);

            e.HasOne<Client>().WithMany().HasForeignKey(x => x.ClientId).OnDelete(DeleteBehavior.Cascade);
            // VehicleId is nullable (RelationType=3 has no vehicle); use Restrict so we
            // don't accidentally remove relations when a Vehicle is hard-deleted.
            e.HasOne<Vehicle>().WithMany().HasForeignKey(x => x.VehicleId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ClientVehicleRelationType>().WithMany().HasForeignKey(x => x.RelationTypeId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.ClientId);
            e.HasIndex(x => x.VehicleId);
        });

        // ------------------------------------------------------------------
        // Request module — EF-owned (created by the AddRequests migration).
        // ------------------------------------------------------------------

        b.Entity<RequestDocumentPrint>(e =>
        {
            e.ToTable("RequestDocumentPrint");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Code).HasMaxLength(20).IsRequired();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.TemplatePath).HasMaxLength(400);
        });

        b.Entity<RequestType>(e =>
        {
            e.ToTable("RequestType");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.ParentRequestTypeId).HasColumnType("tinyint");
            e.Property(x => x.DocumentPrintId).HasColumnType("tinyint");
            e.Property(x => x.Name).HasMaxLength(250).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
            e.Property(x => x.TechnicalExamRequirement).HasConversion<byte>();

            e.HasOne<RequestType>()
                .WithMany()
                .HasForeignKey(x => x.ParentRequestTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<RequestDocumentPrint>()
                .WithMany()
                .HasForeignKey(x => x.DocumentPrintId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.Name);
        });

        b.Entity<RequestOwnershipProofType>(e =>
        {
            e.ToTable("RequestOwnershipProofType");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<RequestPaymentProofType>(e =>
        {
            e.ToTable("RequestPaymentProofType");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<RequestAttachmentType>(e =>
        {
            e.ToTable("RequestAttachmentType");
            e.Property(x => x.Id).HasColumnType("tinyint").ValueGeneratedOnAdd();
            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
        });

        b.Entity<Request>(e =>
        {
            e.ToTable("Request");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.RequestTypeId).HasColumnType("tinyint");
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.CreatedByUserId).HasMaxLength(450).IsRequired();
            e.Property(x => x.ModifiedByUserId).HasMaxLength(450);
            e.Property(x => x.EndedByUserId).HasMaxLength(450);
            e.Property(x => x.LegacyReferenceNumber).HasMaxLength(50);
            e.Property(x => x.RowVersion).IsRowVersion();

            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<RequestType>().WithMany().HasForeignKey(x => x.RequestTypeId).OnDelete(DeleteBehavior.Restrict);

            e.HasOne<ClientVehicleRelation>()
                .WithMany()
                .HasForeignKey(x => x.ClientVehicleRelationId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ClientVehicleRelation>()
                .WithMany()
                .HasForeignKey(x => x.NewClientVehicleRelationId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.PreviousRegistrationId);

            e.HasIndex(x => new { x.CompanyId, x.CreatedAt });
            e.HasIndex(x => x.ClientVehicleRelationId);
            e.HasIndex(x => x.RequestTypeId);
            e.HasIndex(x => new { x.CompanyId, x.CreatedAt })
                .HasDatabaseName("IX_Request_Open")
                .HasFilter("[Active] = 1 AND [EndedAt] IS NULL");

            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<RequestOwnershipProof>(e =>
        {
            e.ToTable("RequestOwnershipProof");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.OwnershipProofTypeId).HasColumnType("tinyint");
            e.Property(x => x.Detail).HasMaxLength(500);

            e.HasOne<Request>().WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<RequestOwnershipProofType>().WithMany().HasForeignKey(x => x.OwnershipProofTypeId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.RequestId);
        });

        b.Entity<RequestPaymentProof>(e =>
        {
            e.ToTable("RequestPaymentProof");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.PaymentProofTypeId).HasColumnType("tinyint");
            e.Property(x => x.Detail).HasMaxLength(500);

            e.HasOne<Request>().WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<RequestPaymentProofType>().WithMany().HasForeignKey(x => x.PaymentProofTypeId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.RequestId);
        });

        b.Entity<RequestAttachment>(e =>
        {
            e.ToTable("RequestAttachment");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.AttachmentTypeId).HasColumnType("tinyint");
            e.Property(x => x.FileName).HasMaxLength(260).IsRequired();
            e.Property(x => x.ContentType).HasMaxLength(150).IsRequired();
            e.Property(x => x.StoragePath).HasMaxLength(500).IsRequired();
            e.Property(x => x.UploadedByUserId).HasMaxLength(450).IsRequired();

            e.HasOne<Request>().WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<RequestAttachmentType>().WithMany().HasForeignKey(x => x.AttachmentTypeId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.RequestId);
        });

        // ------------------------------------------------------------------
        // Technical Exam module — EF-owned (created by the AddTechnicalExams
        // migration). Bulk data is loaded by migrate-technical-exams.sql.
        // ------------------------------------------------------------------

        b.Entity<TechnicalExamType>(e =>
        {
            e.ToTable("TechnicalExamType");
            e.Property(x => x.Code).HasMaxLength(20);
            e.Property(x => x.Description).HasMaxLength(300).IsRequired();
        });

        b.Entity<TechnicalExamDetailStatus>(e =>
        {
            e.ToTable("TechnicalExamDetailStatus");
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        b.Entity<TechnicalExamVehiclePart>(e =>
        {
            e.ToTable("TechnicalExamVehiclePart");
            e.Property(x => x.Code).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500).IsRequired();
            e.HasIndex(x => x.CategoryId);
        });

        b.Entity<TechnicalExamOrganization>(e =>
        {
            e.ToTable("TechnicalExamOrganization");
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Code).HasMaxLength(40);
            e.Property(x => x.Name).HasMaxLength(300);
            e.Property(x => x.Address).HasMaxLength(200);
            e.Property(x => x.Phone).HasMaxLength(200);
            e.Property(x => x.Fax).HasMaxLength(200);
            e.Property(x => x.BankAccount).HasMaxLength(510);
            e.Property(x => x.Depositor).HasMaxLength(200);
            e.Property(x => x.TaxNumber).HasMaxLength(100);
            e.Property(x => x.ResponsibleOfficer).HasMaxLength(200);
            e.Property(x => x.Secretary).HasMaxLength(200);
            // CityId/CommunityId are loose legacy ids — index, no FK.
            e.HasIndex(x => x.CityId);
        });

        b.Entity<TechnicalExamReport>(e =>
        {
            e.ToTable("TechnicalExamReport");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.RegNumber).HasMaxLength(50);
            e.Property(x => x.ExplanationNote).HasMaxLength(500);
            e.Property(x => x.DriversWarning).HasMaxLength(500);
            e.Property(x => x.Note).HasMaxLength(500);
            // TechnicalChanges stays nvarchar(max).
            e.Property(x => x.RowVersion).IsRowVersion();

            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            // Anchor relation is optional (some legacy reports point at a deleted relation).
            e.HasOne<ClientVehicleRelation>().WithMany().HasForeignKey(x => x.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<TechnicalExamType>().WithMany().HasForeignKey(x => x.TechnicalExamTypeId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<TechnicalExamOrganization>().WithMany().HasForeignKey(x => x.OrganizationId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.CustomerVehicleRelationId);
            e.HasIndex(x => x.RegNumber);
            e.HasIndex(x => new { x.CompanyId, x.MadeDate });

            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<TechnicalExamReportDetail>(e =>
        {
            e.ToTable("TechnicalExamReportDetail");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.Note).HasMaxLength(300);

            e.HasOne<TechnicalExamReport>().WithMany().HasForeignKey(x => x.TechnicalExamReportId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<TechnicalExamVehiclePart>().WithMany().HasForeignKey(x => x.VehiclePartId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<TechnicalExamDetailStatus>().WithMany().HasForeignKey(x => x.StatusId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.TechnicalExamReportId);
        });

        // ------------------------------------------------------------------
        // Payment module — EF-owned (created by the AddPayments migration).
        // Bulk historical data is loaded by migrate-payments.sql (Phase 1).
        // ------------------------------------------------------------------

        b.Entity<VatRate>(e =>
        {
            e.ToTable("VatRate");
            e.Property(x => x.Code).HasMaxLength(20);
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        b.Entity<PaymentType>(e =>
        {
            e.ToTable("PaymentType");
            e.Property(x => x.Code).HasMaxLength(40);
            e.Property(x => x.Name).HasMaxLength(150).IsRequired();
            e.Property(x => x.Prefix).HasMaxLength(20);
        });

        b.Entity<PriceCatalog>(e =>
        {
            e.ToTable("PriceCatalog");
            e.Property(x => x.Code).HasMaxLength(40);
            e.Property(x => x.Name).HasMaxLength(250).IsRequired();
            e.Property(x => x.BasePrice).HasColumnType("decimal(18,4)");
            e.Property(x => x.Trigger).HasConversion<byte>();
            e.Property(x => x.VehicleField).HasMaxLength(60);
            e.Property(x => x.PriceCompanyId).HasColumnType("tinyint");
            e.Property(x => x.VehicleCategoryFilter).HasMaxLength(200);
            e.Property(x => x.BankAccount).HasMaxLength(50);
            e.Property(x => x.PaymentForm).HasMaxLength(20);
            e.HasOne<VatRate>().WithMany().HasForeignKey(x => x.VatRateId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.Trigger);
            // Composite index for the rule evaluator (Phase 3): look up by trigger + vehicle category.
            e.HasIndex(x => new { x.Trigger, x.VehiclePaymentCategoryId })
              .HasDatabaseName("IX_PriceCatalog_Eval");
        });

        b.Entity<InstallmentAgreement>(e =>
        {
            e.ToTable("InstallmentAgreement");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Number).HasMaxLength(40).IsRequired();
            e.Property(x => x.GuarantorName).HasMaxLength(200);
            e.Property(x => x.GuarantorAddress).HasMaxLength(300);
            e.Property(x => x.GuarantorEmbg).HasMaxLength(13);

            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasIndex(x => x.CompanyId);
            e.HasIndex(x => x.Number);

            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<PaymentDocument>(e =>
        {
            e.ToTable("PaymentDocument");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.DocumentNumber).HasMaxLength(50).IsRequired();
            e.Property(x => x.Note).HasMaxLength(500);
            e.Property(x => x.StornoReason).HasMaxLength(500);
            e.Property(x => x.CreatedByUserId).HasMaxLength(450);
            e.Property(x => x.ModifiedByUserId).HasMaxLength(450);
            e.Property(x => x.RowVersion).IsRowVersion();

            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<PaymentType>().WithMany().HasForeignKey(x => x.PaymentTypeId).OnDelete(DeleteBehavior.Restrict);
            // Anchor relation is required (every bill bills someone for something).
            e.HasOne<ClientVehicleRelation>().WithMany().HasForeignKey(x => x.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
            // Agreement optional — only set for installment bills.
            e.HasOne<InstallmentAgreement>().WithMany().HasForeignKey(x => x.AgreementId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.CustomerVehicleRelationId);
            e.HasIndex(x => new { x.CompanyId, x.IssueDate });
            e.HasIndex(x => x.DocumentNumber);
            e.HasIndex(x => x.LegacyId).HasFilter("[LegacyId] IS NOT NULL");

            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });

        b.Entity<PaymentDocumentLine>(e =>
        {
            e.ToTable("PaymentDocumentLine");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.UnitPrice).HasColumnType("decimal(18,4)");
            e.Property(x => x.Note).HasMaxLength(300);
            e.Property(x => x.PrePaidNote).HasMaxLength(300);

            e.HasOne<PaymentDocument>().WithMany().HasForeignKey(x => x.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne<PriceCatalog>().WithMany().HasForeignKey(x => x.PriceCatalogId).OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => x.PaymentDocumentId);
            e.HasIndex(x => x.CustomerDebtId).HasFilter("[CustomerDebtId] IS NOT NULL");
        });

        b.Entity<InstallmentSchedule>(e =>
        {
            e.ToTable("InstallmentSchedule");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Amount).HasColumnType("decimal(18,4)");
            e.Property(x => x.PaidAmount).HasColumnType("decimal(18,4)");
            e.Property(x => x.Note).HasMaxLength(300);

            e.HasOne<PaymentDocument>().WithMany().HasForeignKey(x => x.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => x.PaymentDocumentId);
            e.HasIndex(x => new { x.PaymentDocumentId, x.SequenceNo }).IsUnique();
        });

        b.Entity<CustomerDebt>(e =>
        {
            e.ToTable("CustomerDebt");
            e.Property(x => x.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            e.Property(x => x.CompanyId).HasColumnType("tinyint");
            e.Property(x => x.Price).HasColumnType("decimal(18,4)");
            e.Property(x => x.Note).HasMaxLength(300);
            e.Property(x => x.Origin).HasConversion<byte>();
            e.Property(x => x.CreatedByUserId).HasMaxLength(450);

            e.HasOne<Company>().WithMany().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<ClientVehicleRelation>().WithMany().HasForeignKey(x => x.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
            e.HasOne<PriceCatalog>().WithMany().HasForeignKey(x => x.PriceCatalogId).OnDelete(DeleteBehavior.Restrict);
            // SettledByLineId is a forward link; use Restrict so deleting a paid line doesn't orphan the debt.
            e.HasOne<PaymentDocumentLine>().WithMany().HasForeignKey(x => x.SettledByLineId).OnDelete(DeleteBehavior.Restrict);

            // Open-debt queries are by relation + Paid=false; partial filtered index keeps them fast.
            e.HasIndex(x => x.CustomerVehicleRelationId);
            e.HasIndex(x => new { x.CustomerVehicleRelationId, x.Paid })
              .HasDatabaseName("IX_CustomerDebt_Open")
              .HasFilter("[Active] = 1 AND [Paid] = 0");
            // Forensic lookups by origin
            e.HasIndex(x => x.OriginRequestId).HasFilter("[OriginRequestId] IS NOT NULL");
            e.HasIndex(x => x.OriginTechnicalExamId).HasFilter("[OriginTechnicalExamId] IS NOT NULL");

            e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
        });
    }
}
