# VTE Modern Rewrite - Plan 1: Foundation

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Set up the solution structure, core entities, EF Core infrastructure, and WPF shell with Fluent UI navigation — the foundation all modules build on.

**Architecture:** Clean Architecture with 4 layers: Core (entities/interfaces), Application (services/DTOs), Infrastructure (EF Core/repositories), WPF (UI shell). Each feature module is a separate WPF class library. DI wires everything together at startup.

**Tech Stack:** .NET 8, C# 12, WPF, WPF-UI 3.x, CommunityToolkit.Mvvm 8.x, EF Core 8, SQL Server, FluentValidation, Serilog, xUnit

---

## File Structure

```
VTE/
├── VTE.sln
├── src/
│   ├── VTE.Core/
│   │   ├── VTE.Core.csproj
│   │   ├── Entities/
│   │   │   ├── BaseEntity.cs
│   │   │   ├── AuditableEntity.cs
│   │   │   ├── LookupEntity.cs
│   │   │   ├── Customer.cs
│   │   │   ├── CustomerContactPerson.cs
│   │   │   ├── CustomerBankAccount.cs
│   │   │   ├── Vehicle.cs
│   │   │   ├── VehicleAxle.cs
│   │   │   ├── VehicleTyre.cs
│   │   │   ├── VehicleAxleDistance.cs
│   │   │   ├── CustomerVehicleRelation.cs
│   │   │   ├── TechnicalExamReport.cs
│   │   │   ├── TechnicalExamReportDetail.cs
│   │   │   ├── TechnicalExamVisualError.cs
│   │   │   ├── Request.cs
│   │   │   ├── RequestAttachment.cs
│   │   │   ├── RequestOwnershipProof.cs
│   │   │   ├── RequestPaymentProof.cs
│   │   │   ├── Document.cs
│   │   │   ├── DocumentAttachment.cs
│   │   │   ├── TrafficLicense.cs
│   │   │   ├── TrafficLicenseExtension.cs
│   │   │   ├── InternationalDrivingLicense.cs
│   │   │   ├── InternationalDrivingLicenseCategory.cs
│   │   │   ├── Permission.cs
│   │   │   ├── PaymentDocument.cs
│   │   │   ├── PaymentLineItem.cs
│   │   │   ├── PaymentInstallment.cs
│   │   │   ├── User.cs
│   │   │   ├── Role.cs
│   │   │   ├── RolePrivilege.cs
│   │   │   ├── TechnicalExamOrganization.cs
│   │   │   └── Company.cs
│   │   ├── Lookups/
│   │   │   ├── Country.cs
│   │   │   ├── City.cs
│   │   │   ├── Community.cs
│   │   │   ├── Street.cs
│   │   │   ├── BusinessType.cs
│   │   │   ├── VehicleBodyType.cs
│   │   │   ├── VehicleCategory.cs
│   │   │   ├── VehiclePaymentCategory.cs
│   │   │   ├── VehicleUseType.cs
│   │   │   ├── EngineType.cs
│   │   │   ├── EnginePowerSourceType.cs
│   │   │   ├── GearBoxType.cs
│   │   │   ├── BrakeType.cs
│   │   │   ├── SupportingType.cs
│   │   │   ├── EcoProgram.cs
│   │   │   ├── VehicleMaker.cs
│   │   │   ├── VehicleModel.cs
│   │   │   ├── TireType.cs
│   │   │   ├── Color.cs
│   │   │   ├── RegistrationIssuer.cs
│   │   │   ├── DocumentType.cs
│   │   │   ├── DocumentTypeOption.cs
│   │   │   ├── DocumentTypeOptionDetail.cs
│   │   │   ├── OwnershipProofType.cs
│   │   │   ├── PaymentProofType.cs
│   │   │   ├── AttachmentType.cs
│   │   │   ├── PaymentType.cs
│   │   │   ├── PaymentCategory.cs
│   │   │   ├── PaymentCatalogItem.cs
│   │   │   ├── VATRate.cs
│   │   │   ├── TechnicalExamType.cs
│   │   │   ├── TechnicalExamVehiclePart.cs
│   │   │   ├── ExamDetailStatus.cs
│   │   │   ├── RelationType.cs
│   │   │   ├── DrivingLicenseCategory.cs
│   │   │   └── RequestType.cs
│   │   └── Interfaces/
│   │       ├── IRepository.cs
│   │       ├── IUnitOfWork.cs
│   │       └── ICurrentUserService.cs
│   ├── VTE.Application/
│   │   ├── VTE.Application.csproj
│   │   ├── DependencyInjection.cs
│   │   └── Common/
│   │       ├── IAppService.cs
│   │       └── PagedResult.cs
│   ├── VTE.Infrastructure/
│   │   ├── VTE.Infrastructure.csproj
│   │   ├── DependencyInjection.cs
│   │   ├── Data/
│   │   │   ├── VteDbContext.cs
│   │   │   ├── Configurations/
│   │   │   │   ├── CustomerConfiguration.cs
│   │   │   │   ├── VehicleConfiguration.cs
│   │   │   │   ├── TechnicalExamReportConfiguration.cs
│   │   │   │   ├── RequestConfiguration.cs
│   │   │   │   ├── DocumentConfiguration.cs
│   │   │   │   ├── PaymentDocumentConfiguration.cs
│   │   │   │   ├── UserConfiguration.cs
│   │   │   │   └── LookupConfigurations.cs
│   │   │   └── Repositories/
│   │   │       └── Repository.cs
│   │   └── Services/
│   │       └── CurrentUserService.cs
│   └── VTE.WPF/
│       ├── VTE.WPF.csproj
│       ├── App.xaml
│       ├── App.xaml.cs
│       ├── appsettings.json
│       ├── Views/
│       │   ├── MainWindow.xaml
│       │   ├── MainWindow.xaml.cs
│       │   ├── LoginWindow.xaml
│       │   ├── LoginWindow.xaml.cs
│       │   └── Pages/
│       │       ├── DashboardPage.xaml
│       │       └── DashboardPage.xaml.cs
│       ├── ViewModels/
│       │   ├── MainWindowViewModel.cs
│       │   ├── LoginViewModel.cs
│       │   └── DashboardViewModel.cs
│       ├── Services/
│       │   ├── NavigationService.cs
│       │   └── INavigationService.cs
│       ├── Controls/
│       │   └── EntityTabControl.cs
│       └── Resources/
│           ├── Strings.resx
│           ├── Strings.mk-MK.resx
│           └── SharedStyles.xaml
└── tests/
    ├── VTE.Core.Tests/
    │   ├── VTE.Core.Tests.csproj
    │   └── Entities/
    │       ├── CustomerTests.cs
    │       └── VehicleTests.cs
    └── VTE.Infrastructure.Tests/
        ├── VTE.Infrastructure.Tests.csproj
        └── Data/
            └── VteDbContextTests.cs
```

---

### Task 1: Create Solution and Projects

**Files:**
- Create: `VTE/VTE.sln`
- Create: all `.csproj` files listed above

- [ ] **Step 1: Create solution and Core project**

```bash
mkdir VTE && cd VTE
dotnet new sln -n VTE
mkdir -p src/VTE.Core
dotnet new classlib -n VTE.Core -o src/VTE.Core -f net8.0
dotnet sln add src/VTE.Core/VTE.Core.csproj
```

- [ ] **Step 2: Create Application project**

```bash
dotnet new classlib -n VTE.Application -o src/VTE.Application -f net8.0
dotnet sln add src/VTE.Application/VTE.Application.csproj
cd src/VTE.Application
dotnet add reference ../VTE.Core/VTE.Core.csproj
cd ../..
```

- [ ] **Step 3: Create Infrastructure project**

```bash
dotnet new classlib -n VTE.Infrastructure -o src/VTE.Infrastructure -f net8.0
dotnet sln add src/VTE.Infrastructure/VTE.Infrastructure.csproj
cd src/VTE.Infrastructure
dotnet add reference ../VTE.Core/VTE.Core.csproj
dotnet add reference ../VTE.Application/VTE.Application.csproj
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.*
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.*
dotnet add package Serilog.Extensions.Logging --version 8.0.*
dotnet add package Serilog.Sinks.File --version 6.0.*
dotnet add package Serilog.Sinks.Console --version 6.0.*
cd ../..
```

- [ ] **Step 4: Create WPF shell project**

```bash
dotnet new wpf -n VTE.WPF -o src/VTE.WPF -f net8.0
dotnet sln add src/VTE.WPF/VTE.WPF.csproj
cd src/VTE.WPF
dotnet add reference ../VTE.Application/VTE.Application.csproj
dotnet add reference ../VTE.Infrastructure/VTE.Infrastructure.csproj
dotnet add package WPF-UI --version 3.*
dotnet add package CommunityToolkit.Mvvm --version 8.*
dotnet add package Microsoft.Extensions.Hosting --version 8.0.*
dotnet add package Microsoft.Extensions.Configuration.Json --version 8.0.*
cd ../..
```

- [ ] **Step 5: Create test projects**

```bash
dotnet new xunit -n VTE.Core.Tests -o tests/VTE.Core.Tests -f net8.0
dotnet sln add tests/VTE.Core.Tests/VTE.Core.Tests.csproj
cd tests/VTE.Core.Tests
dotnet add reference ../../src/VTE.Core/VTE.Core.csproj
cd ../..

dotnet new xunit -n VTE.Infrastructure.Tests -o tests/VTE.Infrastructure.Tests -f net8.0
dotnet sln add tests/VTE.Infrastructure.Tests/VTE.Infrastructure.Tests.csproj
cd tests/VTE.Infrastructure.Tests
dotnet add reference ../../src/VTE.Infrastructure/VTE.Infrastructure.csproj
dotnet add reference ../../src/VTE.Core/VTE.Core.csproj
dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 8.0.*
cd ../..
```

- [ ] **Step 6: Verify solution builds**

Run: `dotnet build VTE.sln`
Expected: Build succeeded, 0 errors

- [ ] **Step 7: Commit**

```bash
git init
echo "bin/\nobj/\n*.user\n.vs/\n*.suo" > .gitignore
git add .
git commit -m "feat: create solution structure with Core, Application, Infrastructure, WPF projects"
```

---

### Task 2: Core Entity Base Classes

**Files:**
- Create: `src/VTE.Core/Entities/BaseEntity.cs`
- Create: `src/VTE.Core/Entities/AuditableEntity.cs`
- Create: `src/VTE.Core/Entities/LookupEntity.cs`

- [ ] **Step 1: Write test for BaseEntity**

Create `tests/VTE.Core.Tests/Entities/BaseEntityTests.cs`:

```csharp
namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class BaseEntityTests
{
    [Fact]
    public void NewBaseEntity_HasDefaultId()
    {
        var entity = new TestEntity();
        Assert.Equal(0, entity.Id);
    }

    [Fact]
    public void BaseEntity_CanSetId()
    {
        var entity = new TestEntity { Id = 42 };
        Assert.Equal(42, entity.Id);
    }

    private class TestEntity : BaseEntity { }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VTE.Core.Tests --filter "BaseEntityTests" -v n`
Expected: FAIL — `BaseEntity` does not exist

- [ ] **Step 3: Implement BaseEntity, AuditableEntity, LookupEntity**

Create `src/VTE.Core/Entities/BaseEntity.cs`:

```csharp
namespace VTE.Core.Entities;

public abstract class BaseEntity
{
    public long Id { get; set; }
}
```

Create `src/VTE.Core/Entities/AuditableEntity.cs`:

```csharp
namespace VTE.Core.Entities;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public long CreatedByUserId { get; set; }
    public long? ModifiedByUserId { get; set; }
}
```

Create `src/VTE.Core/Entities/LookupEntity.cs`:

```csharp
namespace VTE.Core.Entities;

public abstract class LookupEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
```

Delete the auto-generated `Class1.cs` from VTE.Core if it exists.

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/VTE.Core.Tests --filter "BaseEntityTests" -v n`
Expected: PASS (2 tests)

- [ ] **Step 5: Commit**

```bash
git add .
git commit -m "feat: add BaseEntity, AuditableEntity, LookupEntity base classes"
```

---

### Task 3: Core Interfaces

**Files:**
- Create: `src/VTE.Core/Interfaces/IRepository.cs`
- Create: `src/VTE.Core/Interfaces/IUnitOfWork.cs`
- Create: `src/VTE.Core/Interfaces/ICurrentUserService.cs`

- [ ] **Step 1: Create IRepository**

Create `src/VTE.Core/Interfaces/IRepository.cs`:

```csharp
namespace VTE.Core.Interfaces;

using System.Linq.Expressions;
using VTE.Core.Entities;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<List<T>> GetAllAsync(CancellationToken ct = default);
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<T> AddAsync(T entity, CancellationToken ct = default);
    void Update(T entity);
    void Delete(T entity);
}
```

- [ ] **Step 2: Create IUnitOfWork**

Create `src/VTE.Core/Interfaces/IUnitOfWork.cs`:

```csharp
namespace VTE.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
```

- [ ] **Step 3: Create ICurrentUserService**

Create `src/VTE.Core/Interfaces/ICurrentUserService.cs`:

```csharp
namespace VTE.Core.Interfaces;

public interface ICurrentUserService
{
    long? UserId { get; }
    string? Username { get; }
    string? RoleName { get; }
    bool IsAuthenticated { get; }
    bool HasPermission(string entityName, string action);
}
```

- [ ] **Step 4: Verify build**

Run: `dotnet build src/VTE.Core/VTE.Core.csproj`
Expected: Build succeeded

- [ ] **Step 5: Commit**

```bash
git add .
git commit -m "feat: add IRepository, IUnitOfWork, ICurrentUserService interfaces"
```

---

### Task 4: Lookup Entities

**Files:**
- Create: all files in `src/VTE.Core/Lookups/`

- [ ] **Step 1: Create all lookup entities**

These all follow the same pattern — inheriting from `LookupEntity` (Id, Name, IsActive). Some have extra fields or FKs.

Create `src/VTE.Core/Lookups/Country.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Country : LookupEntity
{
    public string ShortName { get; set; } = string.Empty;
}
```

Create `src/VTE.Core/Lookups/Community.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Community : LookupEntity
{
    public long CountryId { get; set; }
    public Country Country { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/City.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class City : LookupEntity
{
    public long CommunityId { get; set; }
    public Community Community { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/Street.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Street : LookupEntity
{
    public long CityId { get; set; }
    public City City { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/BusinessType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class BusinessType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehicleBodyType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleBodyType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehicleCategory.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleCategory : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehiclePaymentCategory.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehiclePaymentCategory : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehicleUseType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleUseType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/EngineType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class EngineType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/EnginePowerSourceType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class EnginePowerSourceType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/GearBoxType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class GearBoxType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/BrakeType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class BrakeType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/SupportingType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class SupportingType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/EcoProgram.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class EcoProgram : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehicleMaker.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleMaker : LookupEntity { }
```

Create `src/VTE.Core/Lookups/VehicleModel.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VehicleModel : LookupEntity
{
    public long VehicleMakerId { get; set; }
    public VehicleMaker VehicleMaker { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/TireType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class TireType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/Color.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class Color : LookupEntity { }
```

Create `src/VTE.Core/Lookups/RegistrationIssuer.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class RegistrationIssuer : LookupEntity { }
```

Create `src/VTE.Core/Lookups/DocumentType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentType : LookupEntity
{
    public bool IsVehicleRequired { get; set; }
    public bool IsTechnicalExamRequired { get; set; }
    public bool IsPaymentRequired { get; set; }
    public List<DocumentTypeOption> Options { get; set; } = [];
}
```

Create `src/VTE.Core/Lookups/DocumentTypeOption.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentTypeOption : LookupEntity
{
    public long DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; } = null!;
    public List<DocumentTypeOptionDetail> Details { get; set; } = [];
}
```

Create `src/VTE.Core/Lookups/DocumentTypeOptionDetail.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DocumentTypeOptionDetail : LookupEntity
{
    public long DocumentTypeOptionId { get; set; }
    public DocumentTypeOption DocumentTypeOption { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/OwnershipProofType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class OwnershipProofType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/PaymentProofType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class PaymentProofType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/AttachmentType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class AttachmentType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/PaymentType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class PaymentType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/PaymentCategory.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class PaymentCategory : LookupEntity { }
```

Create `src/VTE.Core/Lookups/PaymentCatalogItem.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class PaymentCatalogItem : LookupEntity
{
    public long PaymentCategoryId { get; set; }
    public PaymentCategory PaymentCategory { get; set; } = null!;
    public long VehiclePaymentCategoryId { get; set; }
    public VehiclePaymentCategory VehiclePaymentCategory { get; set; } = null!;
}
```

Create `src/VTE.Core/Lookups/VATRate.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class VATRate : LookupEntity
{
    public decimal Rate { get; set; }
}
```

Create `src/VTE.Core/Lookups/TechnicalExamType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class TechnicalExamType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/TechnicalExamVehiclePart.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class TechnicalExamVehiclePart : LookupEntity
{
    public long? ParentId { get; set; }
    public TechnicalExamVehiclePart? Parent { get; set; }
    public List<TechnicalExamVehiclePart> Children { get; set; } = [];
}
```

Create `src/VTE.Core/Lookups/ExamDetailStatus.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class ExamDetailStatus : LookupEntity { }
```

Create `src/VTE.Core/Lookups/RelationType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class RelationType : LookupEntity { }
```

Create `src/VTE.Core/Lookups/DrivingLicenseCategory.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class DrivingLicenseCategory : LookupEntity { }
```

Create `src/VTE.Core/Lookups/RequestType.cs`:

```csharp
namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class RequestType : LookupEntity { }
```

- [ ] **Step 2: Verify build**

Run: `dotnet build src/VTE.Core/VTE.Core.csproj`
Expected: Build succeeded

- [ ] **Step 3: Commit**

```bash
git add .
git commit -m "feat: add all lookup entities (35 types)"
```

---

### Task 5: Core Business Entities — Customer, Vehicle

**Files:**
- Create: `src/VTE.Core/Entities/Customer.cs`
- Create: `src/VTE.Core/Entities/CustomerContactPerson.cs`
- Create: `src/VTE.Core/Entities/CustomerBankAccount.cs`
- Create: `src/VTE.Core/Entities/Vehicle.cs`
- Create: `src/VTE.Core/Entities/VehicleAxle.cs`
- Create: `src/VTE.Core/Entities/VehicleTyre.cs`
- Create: `src/VTE.Core/Entities/VehicleAxleDistance.cs`
- Test: `tests/VTE.Core.Tests/Entities/CustomerTests.cs`
- Test: `tests/VTE.Core.Tests/Entities/VehicleTests.cs`

- [ ] **Step 1: Write Customer test**

Create `tests/VTE.Core.Tests/Entities/CustomerTests.cs`:

```csharp
namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class CustomerTests
{
    [Fact]
    public void Customer_IsAuditableEntity()
    {
        var customer = new Customer();
        Assert.IsAssignableFrom<AuditableEntity>(customer);
    }

    [Fact]
    public void Customer_DefaultIsCompany_IsFalse()
    {
        var customer = new Customer();
        Assert.False(customer.IsCompany);
    }

    [Fact]
    public void Customer_ContactPersons_InitializedAsEmptyList()
    {
        var customer = new Customer();
        Assert.NotNull(customer.ContactPersons);
        Assert.Empty(customer.ContactPersons);
    }

    [Fact]
    public void Customer_BankAccounts_InitializedAsEmptyList()
    {
        var customer = new Customer();
        Assert.NotNull(customer.BankAccounts);
        Assert.Empty(customer.BankAccounts);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VTE.Core.Tests --filter "CustomerTests" -v n`
Expected: FAIL — `Customer` does not exist

- [ ] **Step 3: Implement Customer and children**

Create `src/VTE.Core/Entities/Customer.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Customer : AuditableEntity
{
    public string IdentificationNumber { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? ParentName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public bool IsCompany { get; set; }
    public string? CompanyName { get; set; }
    public string? TaxNumber { get; set; }
    public string? Occupation { get; set; }
    public string? WorksInCompany { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Fax { get; set; }
    public string? Email { get; set; }
    public string? PassportNumber { get; set; }
    public DateTime? PassportDateIssued { get; set; }
    public string? PassportIssuer { get; set; }
    public string? DrivingLicenseNumber { get; set; }
    public string? IdentityCardNumber { get; set; }
    public bool CanSendNotifications { get; set; }
    public int Status { get; set; }
    public string? Note { get; set; }

    // Foreign keys
    public long? CitizenshipId { get; set; }
    public Country? Citizenship { get; set; }
    public long? BirthCityId { get; set; }
    public City? BirthCity { get; set; }
    public long? BirthAddressStreetId { get; set; }
    public Street? BirthAddressStreet { get; set; }
    public long? LivingCityId { get; set; }
    public City? LivingCity { get; set; }
    public long? LivingAddressStreetId { get; set; }
    public Street? LivingAddressStreet { get; set; }
    public long? BusinessTypeId { get; set; }
    public BusinessType? BusinessType { get; set; }

    // Children
    public List<CustomerContactPerson> ContactPersons { get; set; } = [];
    public List<CustomerBankAccount> BankAccounts { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/CustomerContactPerson.cs`:

```csharp
namespace VTE.Core.Entities;

public class CustomerContactPerson : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/CustomerBankAccount.cs`:

```csharp
namespace VTE.Core.Entities;

public class CustomerBankAccount : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string BankName { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? Note { get; set; }
}
```

- [ ] **Step 4: Run Customer tests**

Run: `dotnet test tests/VTE.Core.Tests --filter "CustomerTests" -v n`
Expected: PASS (4 tests)

- [ ] **Step 5: Write Vehicle test**

Create `tests/VTE.Core.Tests/Entities/VehicleTests.cs`:

```csharp
namespace VTE.Core.Tests.Entities;

using VTE.Core.Entities;

public class VehicleTests
{
    [Fact]
    public void Vehicle_IsAuditableEntity()
    {
        var vehicle = new Vehicle();
        Assert.IsAssignableFrom<AuditableEntity>(vehicle);
    }

    [Fact]
    public void Vehicle_Axles_InitializedAsEmptyList()
    {
        var vehicle = new Vehicle();
        Assert.NotNull(vehicle.Axles);
        Assert.Empty(vehicle.Axles);
    }
}
```

- [ ] **Step 6: Implement Vehicle and children**

Create `src/VTE.Core/Entities/Vehicle.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Vehicle : AuditableEntity
{
    public string ShellNumber { get; set; } = string.Empty;
    public string? EngineNumber { get; set; }
    public DateTime? MakeDate { get; set; }

    // Registration
    public string? FirstRegistrationNumber { get; set; }
    public DateTime? FirstRegistrationDate { get; set; }
    public DateTime? FirstRegistrationValidUntil { get; set; }
    public long? FirstRegistrationIssuerId { get; set; }
    public RegistrationIssuer? FirstRegistrationIssuer { get; set; }
    public string? LastRegistrationNumber { get; set; }
    public DateTime? LastRegistrationDate { get; set; }
    public DateTime? LastRegistrationValidUntil { get; set; }
    public long? LastRegistrationIssuerId { get; set; }
    public RegistrationIssuer? LastRegistrationIssuer { get; set; }

    // Classification FKs
    public long? VehicleModelId { get; set; }
    public VehicleModel? VehicleModel { get; set; }
    public long? BodyTypeId { get; set; }
    public VehicleBodyType? BodyType { get; set; }
    public long? CategoryId { get; set; }
    public VehicleCategory? Category { get; set; }
    public long? PaymentCategoryId { get; set; }
    public VehiclePaymentCategory? PaymentCategory { get; set; }
    public long? UseTypeId { get; set; }
    public VehicleUseType? UseType { get; set; }
    public long? EngineTypeId { get; set; }
    public EngineType? EngineType { get; set; }
    public long? PrimaryPowerSourceId { get; set; }
    public EnginePowerSourceType? PrimaryPowerSource { get; set; }
    public long? SecondaryPowerSourceId { get; set; }
    public EnginePowerSourceType? SecondaryPowerSource { get; set; }
    public long? GearBoxTypeId { get; set; }
    public GearBoxType? GearBoxType { get; set; }
    public long? BrakeTypeId { get; set; }
    public BrakeType? BrakeType { get; set; }
    public long? SupportingTypeId { get; set; }
    public SupportingType? SupportingType { get; set; }
    public long? EcoProgramId { get; set; }
    public EcoProgram? EcoProgram { get; set; }
    public long? MadeInCountryId { get; set; }
    public Country? MadeInCountry { get; set; }
    public long? PrimaryColorId { get; set; }
    public Color? PrimaryColor { get; set; }
    public long? SecondaryColorId { get; set; }
    public Color? SecondaryColor { get; set; }
    public string? ColorCode { get; set; }

    // Engine specs
    public decimal? EnginePowerKW { get; set; }
    public decimal? EngineTorqueNM { get; set; }
    public decimal? EngineWorkingCapacityCM3 { get; set; }

    // Dimensions
    public int? HeightMM { get; set; }
    public int? WidthMM { get; set; }
    public int? LengthMM { get; set; }

    // Weight
    public decimal? EmptyWeightKG { get; set; }
    public decimal? MaxAllowedWeightKG { get; set; }
    public decimal? TrailerWeightBrakedKG { get; set; }
    public decimal? TrailerWeightUnbrakedKG { get; set; }

    // Seating
    public int? NumberOfDoors { get; set; }
    public int? NumberOfSeats { get; set; }
    public int? NumberOfStandingSeats { get; set; }
    public int? NumberOfLyingSeats { get; set; }

    // Axles & Wheels
    public int? NumberOfAxles { get; set; }
    public int? PropulsionAxle { get; set; }
    public int? NumberOfWheels { get; set; }
    public int? NumberOfPropulsionWheels { get; set; }

    // Emissions
    public decimal? CO { get; set; }
    public decimal? HC { get; set; }
    public decimal? NOx { get; set; }
    public decimal? HCNOx { get; set; }
    public decimal? CO2 { get; set; }
    public decimal? NoiseStaticDB { get; set; }
    public decimal? NoiseMovementDB { get; set; }
    public decimal? Blackening { get; set; }
    public decimal? Pinpoints { get; set; }

    // Fuel
    public decimal? FuelConsumption { get; set; }
    public decimal? FuelTankCapacityL { get; set; }
    public bool HasLPG { get; set; }

    // Other
    public decimal? MaxSpeedKMH { get; set; }
    public bool HasHook { get; set; }
    public bool IsSocialVehicle { get; set; }
    public bool IsPrivateTransport { get; set; }

    // Children
    public List<VehicleAxle> Axles { get; set; } = [];
    public List<VehicleTyre> Tyres { get; set; } = [];
    public List<VehicleAxleDistance> AxleDistances { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/VehicleAxle.cs`:

```csharp
namespace VTE.Core.Entities;

public class VehicleAxle : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int AxleNumber { get; set; }
    public decimal? MaxWeightKG { get; set; }
}
```

Create `src/VTE.Core/Entities/VehicleTyre.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class VehicleTyre : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public long TireTypeId { get; set; }
    public TireType TireType { get; set; } = null!;
    public int AxleNumber { get; set; }
}
```

Create `src/VTE.Core/Entities/VehicleAxleDistance.cs`:

```csharp
namespace VTE.Core.Entities;

public class VehicleAxleDistance : BaseEntity
{
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public int FromAxle { get; set; }
    public int ToAxle { get; set; }
    public decimal DistanceMM { get; set; }
}
```

- [ ] **Step 7: Run all tests**

Run: `dotnet test tests/VTE.Core.Tests -v n`
Expected: PASS (all tests)

- [ ] **Step 8: Commit**

```bash
git add .
git commit -m "feat: add Customer, Vehicle entities with children"
```

---

### Task 6: Core Business Entities — Remaining

**Files:**
- Create: `src/VTE.Core/Entities/CustomerVehicleRelation.cs`
- Create: `src/VTE.Core/Entities/TechnicalExamReport.cs`
- Create: `src/VTE.Core/Entities/TechnicalExamReportDetail.cs`
- Create: `src/VTE.Core/Entities/TechnicalExamVisualError.cs`
- Create: `src/VTE.Core/Entities/Request.cs` and children
- Create: `src/VTE.Core/Entities/Document.cs` and children
- Create: `src/VTE.Core/Entities/PaymentDocument.cs` and children
- Create: `src/VTE.Core/Entities/User.cs`, `Role.cs`, `RolePrivilege.cs`
- Create: `src/VTE.Core/Entities/TechnicalExamOrganization.cs`, `Company.cs`

- [ ] **Step 1: Create CustomerVehicleRelation**

Create `src/VTE.Core/Entities/CustomerVehicleRelation.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class CustomerVehicleRelation : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public long VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    public long RelationTypeId { get; set; }
    public RelationType RelationType { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? BeginNote { get; set; }
    public string? TerminationNote { get; set; }
}
```

- [ ] **Step 2: Create TechnicalExamReport and children**

Create `src/VTE.Core/Entities/TechnicalExamReport.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class TechnicalExamReport : AuditableEntity
{
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long ExamTypeId { get; set; }
    public TechnicalExamType ExamType { get; set; } = null!;
    public long OrganizationId { get; set; }
    public TechnicalExamOrganization Organization { get; set; } = null!;
    public string RegistrationNumber { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public long FirstControllerId { get; set; }
    public User FirstController { get; set; } = null!;
    public long? SecondControllerId { get; set; }
    public User? SecondController { get; set; }
    public bool VehiclePassed { get; set; }

    // Brake measurements — Axle 1
    public decimal? Axle1BrakeLeftKN { get; set; }
    public decimal? Axle1BrakeRightKN { get; set; }
    public decimal? Axle1BrakeGj { get; set; }
    public decimal? Axle1BrakeLeftPj { get; set; }
    public decimal? Axle1BrakePN { get; set; }
    // Axle 2
    public decimal? Axle2BrakeLeftKN { get; set; }
    public decimal? Axle2BrakeRightKN { get; set; }
    public decimal? Axle2BrakeGj { get; set; }
    public decimal? Axle2BrakeLeftPj { get; set; }
    public decimal? Axle2BrakePN { get; set; }
    // Axle 3
    public decimal? Axle3BrakeLeftKN { get; set; }
    public decimal? Axle3BrakeRightKN { get; set; }
    public decimal? Axle3BrakeGj { get; set; }
    public decimal? Axle3BrakeLeftPj { get; set; }
    public decimal? Axle3BrakePN { get; set; }
    // Axle 4
    public decimal? Axle4BrakeLeftKN { get; set; }
    public decimal? Axle4BrakeRightKN { get; set; }
    public decimal? Axle4BrakeGj { get; set; }
    public decimal? Axle4BrakeLeftPj { get; set; }
    public decimal? Axle4BrakePN { get; set; }
    // Parking brake
    public decimal? ParkingBrakeLeftKN { get; set; }
    public decimal? ParkingBrakeRightKN { get; set; }
    public decimal? ParkingBrakeGj { get; set; }
    public decimal? ParkingBrakeLeftPj { get; set; }
    public decimal? ParkingBrakePN { get; set; }

    // Effectiveness
    public decimal? WorkingBrakeEffectivenessEmpty { get; set; }
    public decimal? WorkingBrakeEffectivenessFull { get; set; }
    public decimal? SecondaryBrakeEffectiveness { get; set; }
    public decimal? ParkingBrakeEffectiveness { get; set; }
    public decimal? VehicleWeightKG { get; set; }

    // Emissions
    public int? EngineSpeedRPM { get; set; }
    public decimal? CO { get; set; }
    public int? EngineTurns { get; set; }
    public decimal? COPlusTurns { get; set; }
    public decimal? Lambda { get; set; }
    public decimal? Pinpoints { get; set; }
    public decimal? NoiseDB { get; set; }
    public decimal? EngineOilTemperatureC { get; set; }

    public string? TechnicalChanges { get; set; }
    public string? ExplanationNote { get; set; }
    public string? DriverWarning { get; set; }
    public string? Note { get; set; }

    // Children
    public List<TechnicalExamReportDetail> Details { get; set; } = [];
    public List<TechnicalExamVisualError> VisualErrors { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/TechnicalExamReportDetail.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class TechnicalExamReportDetail : BaseEntity
{
    public long ReportId { get; set; }
    public TechnicalExamReport Report { get; set; } = null!;
    public long VehiclePartId { get; set; }
    public TechnicalExamVehiclePart VehiclePart { get; set; } = null!;
    public long StatusId { get; set; }
    public ExamDetailStatus Status { get; set; } = null!;
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/TechnicalExamVisualError.cs`:

```csharp
namespace VTE.Core.Entities;

public class TechnicalExamVisualError : BaseEntity
{
    public long ReportId { get; set; }
    public TechnicalExamReport Report { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public string? Note { get; set; }
}
```

- [ ] **Step 3: Create Request and children**

Create `src/VTE.Core/Entities/Request.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Request : AuditableEntity
{
    public long RequestTypeId { get; set; }
    public RequestType RequestType { get; set; } = null!;
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long? NewCustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation? NewCustomerVehicleRelation { get; set; }
    public long? TechnicalExamReportId { get; set; }
    public TechnicalExamReport? TechnicalExamReport { get; set; }
    public long? PreviousRegistrationId { get; set; }
    public bool IsCustomerChanged { get; set; }
    public bool IsVehicleChanged { get; set; }
    public long? OrganizationId { get; set; }
    public TechnicalExamOrganization? Organization { get; set; }
    public string? Note { get; set; }
    public DateTime? DateEnded { get; set; }
    public long? EndedByUserId { get; set; }
    public User? EndedByUser { get; set; }

    public List<RequestAttachment> Attachments { get; set; } = [];
    public List<RequestOwnershipProof> OwnershipProofs { get; set; } = [];
    public List<RequestPaymentProof> PaymentProofs { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/RequestAttachment.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestAttachment : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long AttachmentTypeId { get; set; }
    public AttachmentType AttachmentType { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public byte[]? FileData { get; set; }
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/RequestOwnershipProof.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestOwnershipProof : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long OwnershipProofTypeId { get; set; }
    public OwnershipProofType OwnershipProofType { get; set; } = null!;
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/RequestPaymentProof.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class RequestPaymentProof : BaseEntity
{
    public long RequestId { get; set; }
    public Request Request { get; set; } = null!;
    public long PaymentProofTypeId { get; set; }
    public PaymentProofType PaymentProofType { get; set; } = null!;
    public string? Note { get; set; }
}
```

- [ ] **Step 4: Create Document and children**

Create `src/VTE.Core/Entities/Document.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class Document : AuditableEntity
{
    public long DocumentTypeId { get; set; }
    public DocumentType DocumentType { get; set; } = null!;
    public long? DocumentTypeOptionId { get; set; }
    public DocumentTypeOption? DocumentTypeOption { get; set; }
    public long? DocumentTypeOptionDetailId { get; set; }
    public DocumentTypeOptionDetail? DocumentTypeOptionDetail { get; set; }
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long? TechnicalExamReportId { get; set; }
    public TechnicalExamReport? TechnicalExamReport { get; set; }
    public long? PreviousRegistrationId { get; set; }
    public long? OwnershipProofId { get; set; }
    public OwnershipProofType? OwnershipProof { get; set; }
    public long? PaymentProofId { get; set; }
    public PaymentProofType? PaymentProof { get; set; }
    public string? Note { get; set; }
    public DateTime? DateEnded { get; set; }
    public long? EndedByUserId { get; set; }
    public User? EndedByUser { get; set; }

    public List<DocumentAttachment> Attachments { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/DocumentAttachment.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class DocumentAttachment : BaseEntity
{
    public long DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public long AttachmentTypeId { get; set; }
    public AttachmentType AttachmentType { get; set; } = null!;
    public string FileName { get; set; } = string.Empty;
    public byte[]? FileData { get; set; }
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/TrafficLicense.cs`:

```csharp
namespace VTE.Core.Entities;

public class TrafficLicense : BaseEntity
{
    public long DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
    public string? PlateNumber { get; set; }
    public List<TrafficLicenseExtension> Extensions { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/TrafficLicenseExtension.cs`:

```csharp
namespace VTE.Core.Entities;

public class TrafficLicenseExtension : BaseEntity
{
    public long TrafficLicenseId { get; set; }
    public TrafficLicense TrafficLicense { get; set; } = null!;
    public DateTime ExtensionDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public string? Note { get; set; }
}
```

Create `src/VTE.Core/Entities/InternationalDrivingLicense.cs`:

```csharp
namespace VTE.Core.Entities;

public class InternationalDrivingLicense : BaseEntity
{
    public long CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public List<InternationalDrivingLicenseCategory> ValidCategories { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/InternationalDrivingLicenseCategory.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class InternationalDrivingLicenseCategory : BaseEntity
{
    public long InternationalDrivingLicenseId { get; set; }
    public InternationalDrivingLicense InternationalDrivingLicense { get; set; } = null!;
    public long DrivingLicenseCategoryId { get; set; }
    public DrivingLicenseCategory DrivingLicenseCategory { get; set; } = null!;
}
```

Create `src/VTE.Core/Entities/Permission.cs`:

```csharp
namespace VTE.Core.Entities;

public class Permission : BaseEntity
{
    public long DocumentId { get; set; }
    public Document Document { get; set; } = null!;
    public string PermissionNumber { get; set; } = string.Empty;
    public DateTime IssuedDate { get; set; }
    public DateTime? ValidUntilDate { get; set; }
    public string? Note { get; set; }
}
```

- [ ] **Step 5: Create PaymentDocument and children**

Create `src/VTE.Core/Entities/PaymentDocument.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class PaymentDocument : AuditableEntity
{
    public long PaymentTypeId { get; set; }
    public PaymentType PaymentType { get; set; } = null!;
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public string DocumentNumber { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
    public DateTime? DueDate { get; set; }
    public decimal DiscountPercent { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCancelled { get; set; }
    public decimal? InsurancePolicy { get; set; }
    public long? AgreementId { get; set; }
    public long? InvoicedToId { get; set; }
    public long? OrganizationId { get; set; }
    public TechnicalExamOrganization? Organization { get; set; }
    public string? Note { get; set; }

    public List<PaymentLineItem> LineItems { get; set; } = [];
    public List<PaymentInstallment> Installments { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/PaymentLineItem.cs`:

```csharp
namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class PaymentLineItem : BaseEntity
{
    public long PaymentDocumentId { get; set; }
    public PaymentDocument PaymentDocument { get; set; } = null!;
    public long PaymentItemId { get; set; }
    public PaymentCatalogItem PaymentItem { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public decimal VATPercent { get; set; }
    public int SortOrder { get; set; }

    public decimal VATAmount => UnitPrice * Quantity * VATPercent / 100m;
    public decimal TotalAmount => UnitPrice * Quantity + VATAmount;
}
```

Create `src/VTE.Core/Entities/PaymentInstallment.cs`:

```csharp
namespace VTE.Core.Entities;

public class PaymentInstallment : BaseEntity
{
    public long PaymentDocumentId { get; set; }
    public PaymentDocument PaymentDocument { get; set; } = null!;
    public int InstallmentNumber { get; set; }
    public DateTime DueDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PaidDate { get; set; }
}
```

- [ ] **Step 6: Create Admin entities**

Create `src/VTE.Core/Entities/User.cs`:

```csharp
namespace VTE.Core.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? LegacyPasswordHash { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? IdentificationNumber { get; set; }
    public string? IdentityCardNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? DateOfHiring { get; set; }
    public string? RFID { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public long OrganizationId { get; set; }
    public TechnicalExamOrganization Organization { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}
```

Create `src/VTE.Core/Entities/Role.cs`:

```csharp
namespace VTE.Core.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public List<RolePrivilege> Privileges { get; set; } = [];
}
```

Create `src/VTE.Core/Entities/RolePrivilege.cs`:

```csharp
namespace VTE.Core.Entities;

public class RolePrivilege : BaseEntity
{
    public long RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public string EntityName { get; set; } = string.Empty;
    public bool CanCreate { get; set; }
    public bool CanRead { get; set; }
    public bool CanUpdate { get; set; }
    public bool CanDelete { get; set; }
}
```

Create `src/VTE.Core/Entities/TechnicalExamOrganization.cs`:

```csharp
namespace VTE.Core.Entities;

public class TechnicalExamOrganization : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public long CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}
```

Create `src/VTE.Core/Entities/Company.cs`:

```csharp
namespace VTE.Core.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? TaxNumber { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
}
```

- [ ] **Step 7: Verify full build**

Run: `dotnet build VTE.sln`
Expected: Build succeeded

- [ ] **Step 8: Commit**

```bash
git add .
git commit -m "feat: add all core business entities (relations, exams, requests, documents, payments, admin)"
```

---

### Task 7: EF Core DbContext and Configurations

**Files:**
- Create: `src/VTE.Infrastructure/Data/VteDbContext.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/CustomerConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/VehicleConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/TechnicalExamReportConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/RequestConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/DocumentConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/PaymentDocumentConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/UserConfiguration.cs`
- Create: `src/VTE.Infrastructure/Data/Configurations/LookupConfigurations.cs`
- Test: `tests/VTE.Infrastructure.Tests/Data/VteDbContextTests.cs`

- [ ] **Step 1: Write DbContext test**

Create `tests/VTE.Infrastructure.Tests/Data/VteDbContextTests.cs`:

```csharp
namespace VTE.Infrastructure.Tests.Data;

using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;
using VTE.Infrastructure.Data;

public class VteDbContextTests
{
    private VteDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<VteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new VteDbContext(options);
    }

    [Fact]
    public async Task CanAddAndRetrieveCustomer()
    {
        using var ctx = CreateContext();
        var customer = new Customer
        {
            IdentificationNumber = "12345",
            FirstName = "Test",
            LastName = "User",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1
        };
        ctx.Customers.Add(customer);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Customers.FirstAsync();
        Assert.Equal("Test", loaded.FirstName);
        Assert.Equal("12345", loaded.IdentificationNumber);
    }

    [Fact]
    public async Task CanAddAndRetrieveVehicle()
    {
        using var ctx = CreateContext();
        var vehicle = new Vehicle
        {
            ShellNumber = "ABC123",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1
        };
        ctx.Vehicles.Add(vehicle);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Vehicles.FirstAsync();
        Assert.Equal("ABC123", loaded.ShellNumber);
    }

    [Fact]
    public async Task CanAddCustomerWithContactPersons()
    {
        using var ctx = CreateContext();
        var customer = new Customer
        {
            IdentificationNumber = "99999",
            FirstName = "Jane",
            LastName = "Doe",
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = 1,
            ContactPersons =
            [
                new CustomerContactPerson { FullName = "Contact One" },
                new CustomerContactPerson { FullName = "Contact Two" }
            ]
        };
        ctx.Customers.Add(customer);
        await ctx.SaveChangesAsync();

        var loaded = await ctx.Customers.Include(c => c.ContactPersons).FirstAsync();
        Assert.Equal(2, loaded.ContactPersons.Count);
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/VTE.Infrastructure.Tests --filter "VteDbContextTests" -v n`
Expected: FAIL — `VteDbContext` does not exist

- [ ] **Step 3: Implement VteDbContext**

Create `src/VTE.Infrastructure/Data/VteDbContext.cs`:

```csharp
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
```

- [ ] **Step 4: Create entity configurations**

Create `src/VTE.Infrastructure/Data/Configurations/CustomerConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.IdentificationNumber).HasMaxLength(50);
        builder.Property(c => c.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.LastName).HasMaxLength(100).IsRequired();
        builder.Property(c => c.ParentName).HasMaxLength(100);
        builder.Property(c => c.CompanyName).HasMaxLength(200);
        builder.Property(c => c.TaxNumber).HasMaxLength(50);
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Fax).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.PassportNumber).HasMaxLength(50);
        builder.Property(c => c.DrivingLicenseNumber).HasMaxLength(50);
        builder.Property(c => c.IdentityCardNumber).HasMaxLength(50);

        builder.HasMany(c => c.ContactPersons).WithOne(cp => cp.Customer).HasForeignKey(cp => cp.CustomerId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(c => c.BankAccounts).WithOne(ba => ba.Customer).HasForeignKey(ba => ba.CustomerId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => c.IdentificationNumber);
        builder.HasIndex(c => c.LastName);
    }
}

public class CustomerContactPersonConfiguration : IEntityTypeConfiguration<CustomerContactPerson>
{
    public void Configure(EntityTypeBuilder<CustomerContactPerson> builder)
    {
        builder.ToTable("CustomerContactPersons");
        builder.Property(c => c.FullName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(200);
    }
}

public class CustomerBankAccountConfiguration : IEntityTypeConfiguration<CustomerBankAccount>
{
    public void Configure(EntityTypeBuilder<CustomerBankAccount> builder)
    {
        builder.ToTable("CustomerBankAccounts");
        builder.Property(c => c.BankName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.AccountNumber).HasMaxLength(50).IsRequired();
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/VehicleConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
{
    public void Configure(EntityTypeBuilder<Vehicle> builder)
    {
        builder.ToTable("Vehicles");
        builder.HasKey(v => v.Id);
        builder.Property(v => v.ShellNumber).HasMaxLength(100).IsRequired();
        builder.Property(v => v.EngineNumber).HasMaxLength(100);
        builder.Property(v => v.FirstRegistrationNumber).HasMaxLength(50);
        builder.Property(v => v.LastRegistrationNumber).HasMaxLength(50);
        builder.Property(v => v.ColorCode).HasMaxLength(20);

        builder.Property(v => v.EnginePowerKW).HasPrecision(10, 2);
        builder.Property(v => v.EngineTorqueNM).HasPrecision(10, 2);
        builder.Property(v => v.EngineWorkingCapacityCM3).HasPrecision(10, 2);
        builder.Property(v => v.EmptyWeightKG).HasPrecision(10, 2);
        builder.Property(v => v.MaxAllowedWeightKG).HasPrecision(10, 2);
        builder.Property(v => v.MaxSpeedKMH).HasPrecision(10, 2);
        builder.Property(v => v.FuelConsumption).HasPrecision(10, 2);
        builder.Property(v => v.FuelTankCapacityL).HasPrecision(10, 2);

        builder.HasOne(v => v.FirstRegistrationIssuer).WithMany().HasForeignKey(v => v.FirstRegistrationIssuerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.LastRegistrationIssuer).WithMany().HasForeignKey(v => v.LastRegistrationIssuerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.PrimaryPowerSource).WithMany().HasForeignKey(v => v.PrimaryPowerSourceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.SecondaryPowerSource).WithMany().HasForeignKey(v => v.SecondaryPowerSourceId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.PrimaryColor).WithMany().HasForeignKey(v => v.PrimaryColorId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(v => v.SecondaryColor).WithMany().HasForeignKey(v => v.SecondaryColorId).OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.Axles).WithOne(a => a.Vehicle).HasForeignKey(a => a.VehicleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.Tyres).WithOne(t => t.Vehicle).HasForeignKey(t => t.VehicleId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(v => v.AxleDistances).WithOne(d => d.Vehicle).HasForeignKey(d => d.VehicleId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(v => v.ShellNumber);
        builder.HasIndex(v => v.LastRegistrationNumber);
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/UserConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.Property(u => u.Username).HasMaxLength(50).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(200).IsRequired();
        builder.Property(u => u.FullName).HasMaxLength(200).IsRequired();
        builder.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(100).IsRequired();
        builder.HasIndex(u => u.Username).IsUnique();
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.Property(r => r.Name).HasMaxLength(100).IsRequired();
        builder.HasMany(r => r.Privileges).WithOne(p => p.Role).HasForeignKey(p => p.RoleId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class RolePrivilegeConfiguration : IEntityTypeConfiguration<RolePrivilege>
{
    public void Configure(EntityTypeBuilder<RolePrivilege> builder)
    {
        builder.ToTable("RolePrivileges");
        builder.Property(p => p.EntityName).HasMaxLength(100).IsRequired();
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/LookupConfigurations.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Lookups;

public class LookupConfigurations :
    IEntityTypeConfiguration<Country>,
    IEntityTypeConfiguration<Community>,
    IEntityTypeConfiguration<City>,
    IEntityTypeConfiguration<Street>,
    IEntityTypeConfiguration<VehicleModel>,
    IEntityTypeConfiguration<DocumentType>,
    IEntityTypeConfiguration<TechnicalExamVehiclePart>,
    IEntityTypeConfiguration<PaymentCatalogItem>,
    IEntityTypeConfiguration<VATRate>
{
    public void Configure(EntityTypeBuilder<Country> b)
    {
        b.ToTable("Countries");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.Property(e => e.ShortName).HasMaxLength(10);
    }

    public void Configure(EntityTypeBuilder<Community> b)
    {
        b.ToTable("Communities");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<City> b)
    {
        b.ToTable("Cities");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<Street> b)
    {
        b.ToTable("Streets");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<VehicleModel> b)
    {
        b.ToTable("VehicleModels");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<DocumentType> b)
    {
        b.ToTable("DocumentTypes");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.HasMany(e => e.Options).WithOne(o => o.DocumentType).HasForeignKey(o => o.DocumentTypeId).OnDelete(DeleteBehavior.Cascade);
    }

    public void Configure(EntityTypeBuilder<TechnicalExamVehiclePart> b)
    {
        b.ToTable("TechnicalExamVehicleParts");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
        b.HasOne(e => e.Parent).WithMany(e => e.Children).HasForeignKey(e => e.ParentId).OnDelete(DeleteBehavior.Restrict);
    }

    public void Configure(EntityTypeBuilder<PaymentCatalogItem> b)
    {
        b.ToTable("PaymentCatalogItems");
        b.Property(e => e.Name).HasMaxLength(200).IsRequired();
    }

    public void Configure(EntityTypeBuilder<VATRate> b)
    {
        b.ToTable("VATRates");
        b.Property(e => e.Rate).HasPrecision(5, 2);
    }
}
```

Create minimal configurations for the remaining entities. Create `src/VTE.Infrastructure/Data/Configurations/TechnicalExamReportConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class TechnicalExamReportConfiguration : IEntityTypeConfiguration<TechnicalExamReport>
{
    public void Configure(EntityTypeBuilder<TechnicalExamReport> builder)
    {
        builder.ToTable("TechnicalExamReports");
        builder.Property(e => e.RegistrationNumber).HasMaxLength(50);
        builder.HasOne(e => e.FirstController).WithMany().HasForeignKey(e => e.FirstControllerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.SecondController).WithMany().HasForeignKey(e => e.SecondControllerId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Details).WithOne(d => d.Report).HasForeignKey(d => d.ReportId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.VisualErrors).WithOne(v => v.Report).HasForeignKey(v => v.ReportId).OnDelete(DeleteBehavior.Cascade);
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/RequestConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("Requests");
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne(e => e.NewCustomerVehicleRelation).WithMany().HasForeignKey(e => e.NewCustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Attachments).WithOne(a => a.Request).HasForeignKey(a => a.RequestId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.OwnershipProofs).WithOne(p => p.Request).HasForeignKey(p => p.RequestId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.PaymentProofs).WithOne(p => p.Request).HasForeignKey(p => p.RequestId).OnDelete(DeleteBehavior.Cascade);
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/DocumentConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.Attachments).WithOne(a => a.Document).HasForeignKey(a => a.DocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class TrafficLicenseConfiguration : IEntityTypeConfiguration<TrafficLicense>
{
    public void Configure(EntityTypeBuilder<TrafficLicense> builder)
    {
        builder.ToTable("TrafficLicenses");
        builder.Property(e => e.LicenseNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.PlateNumber).HasMaxLength(20);
        builder.HasMany(e => e.Extensions).WithOne(x => x.TrafficLicense).HasForeignKey(x => x.TrafficLicenseId).OnDelete(DeleteBehavior.Cascade);
    }
}
```

Create `src/VTE.Infrastructure/Data/Configurations/PaymentDocumentConfiguration.cs`:

```csharp
namespace VTE.Infrastructure.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VTE.Core.Entities;

public class PaymentDocumentConfiguration : IEntityTypeConfiguration<PaymentDocument>
{
    public void Configure(EntityTypeBuilder<PaymentDocument> builder)
    {
        builder.ToTable("PaymentDocuments");
        builder.Property(e => e.DocumentNumber).HasMaxLength(50).IsRequired();
        builder.Property(e => e.DiscountPercent).HasPrecision(5, 2);
        builder.Property(e => e.InsurancePolicy).HasPrecision(18, 2);
        builder.HasOne(e => e.CustomerVehicleRelation).WithMany().HasForeignKey(e => e.CustomerVehicleRelationId).OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(e => e.LineItems).WithOne(i => i.PaymentDocument).HasForeignKey(i => i.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Installments).WithOne(i => i.PaymentDocument).HasForeignKey(i => i.PaymentDocumentId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class PaymentLineItemConfiguration : IEntityTypeConfiguration<PaymentLineItem>
{
    public void Configure(EntityTypeBuilder<PaymentLineItem> builder)
    {
        builder.ToTable("PaymentLineItems");
        builder.Property(e => e.Description).HasMaxLength(500);
        builder.Property(e => e.UnitPrice).HasPrecision(18, 2);
        builder.Property(e => e.VATPercent).HasPrecision(5, 2);
        builder.Ignore(e => e.VATAmount);
        builder.Ignore(e => e.TotalAmount);
    }
}

public class PaymentInstallmentConfiguration : IEntityTypeConfiguration<PaymentInstallment>
{
    public void Configure(EntityTypeBuilder<PaymentInstallment> builder)
    {
        builder.ToTable("PaymentInstallments");
        builder.Property(e => e.Amount).HasPrecision(18, 2);
    }
}
```

Delete auto-generated `Class1.cs` from VTE.Infrastructure and VTE.Application if they exist.

- [ ] **Step 5: Run tests**

Run: `dotnet test tests/VTE.Infrastructure.Tests --filter "VteDbContextTests" -v n`
Expected: PASS (3 tests)

- [ ] **Step 6: Commit**

```bash
git add .
git commit -m "feat: add VteDbContext with entity configurations for all tables"
```

---

### Task 8: Generic Repository and Unit of Work

**Files:**
- Create: `src/VTE.Infrastructure/Data/Repositories/Repository.cs`
- Create: `src/VTE.Infrastructure/DependencyInjection.cs`
- Create: `src/VTE.Infrastructure/Services/CurrentUserService.cs`
- Create: `src/VTE.Application/DependencyInjection.cs`
- Create: `src/VTE.Application/Common/IAppService.cs`
- Create: `src/VTE.Application/Common/PagedResult.cs`

- [ ] **Step 1: Implement Repository**

Create `src/VTE.Infrastructure/Data/Repositories/Repository.cs`:

```csharp
namespace VTE.Infrastructure.Data.Repositories;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;
using VTE.Core.Interfaces;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly VteDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(VteDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ToListAsync(ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        return entity;
    }

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly VteDbContext _context;

    public UnitOfWork(VteDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
```

- [ ] **Step 2: Implement CurrentUserService**

Create `src/VTE.Infrastructure/Services/CurrentUserService.cs`:

```csharp
namespace VTE.Infrastructure.Services;

using VTE.Core.Interfaces;

public class CurrentUserService : ICurrentUserService
{
    public long? UserId { get; set; }
    public string? Username { get; set; }
    public string? RoleName { get; set; }
    public bool IsAuthenticated => UserId.HasValue;

    private readonly HashSet<string> _permissions = [];

    public void SetPermissions(IEnumerable<string> permissions)
    {
        _permissions.Clear();
        foreach (var p in permissions) _permissions.Add(p);
    }

    public bool HasPermission(string entityName, string action)
        => _permissions.Contains($"{entityName}:{action}");

    public void Clear()
    {
        UserId = null;
        Username = null;
        RoleName = null;
        _permissions.Clear();
    }
}
```

- [ ] **Step 3: Create Application layer basics**

Create `src/VTE.Application/Common/PagedResult.cs`:

```csharp
namespace VTE.Application.Common;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}
```

Create `src/VTE.Application/DependencyInjection.cs`:

```csharp
namespace VTE.Application;

using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
```

Create `src/VTE.Infrastructure/DependencyInjection.cs`:

```csharp
namespace VTE.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VTE.Core.Interfaces;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Data.Repositories;
using VTE.Infrastructure.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VteDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("VTE")));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<CurrentUserService>();
        services.AddSingleton<ICurrentUserService>(sp => sp.GetRequiredService<CurrentUserService>());

        return services;
    }
}
```

Delete auto-generated `Class1.cs` from VTE.Application if it exists.

- [ ] **Step 4: Verify build**

Run: `dotnet build VTE.sln`
Expected: Build succeeded

- [ ] **Step 5: Commit**

```bash
git add .
git commit -m "feat: add Repository, UnitOfWork, CurrentUserService, DI registration"
```

---

### Task 9: WPF Shell — App Startup and Login Window

**Files:**
- Create: `src/VTE.WPF/App.xaml` (overwrite default)
- Create: `src/VTE.WPF/App.xaml.cs` (overwrite default)
- Create: `src/VTE.WPF/appsettings.json`
- Create: `src/VTE.WPF/Views/LoginWindow.xaml`
- Create: `src/VTE.WPF/Views/LoginWindow.xaml.cs`
- Create: `src/VTE.WPF/ViewModels/LoginViewModel.cs`

- [ ] **Step 1: Create appsettings.json**

Create `src/VTE.WPF/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "VTE": "Server=.;Database=VTE_Modern;Trusted_Connection=true;TrustServerCertificate=true;"
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "File", "Args": { "path": "logs/vte-.log", "rollingInterval": "Day" } }
    ]
  }
}
```

Make sure appsettings.json is copied to output. Add to `VTE.WPF.csproj`:

```xml
<ItemGroup>
  <None Update="appsettings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </None>
</ItemGroup>
```

- [ ] **Step 2: Create LoginViewModel**

Create `src/VTE.WPF/ViewModels/LoginViewModel.cs`:

```csharp
namespace VTE.WPF.ViewModels;

using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using VTE.Infrastructure.Data;
using VTE.Infrastructure.Services;

public partial class LoginViewModel : ObservableObject
{
    private readonly VteDbContext _dbContext;
    private readonly CurrentUserService _currentUserService;

    [ObservableProperty]
    private string _username = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private bool _isLoggingIn;

    public event Action? LoginSucceeded;

    public LoginViewModel(VteDbContext dbContext, CurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        IsLoggingIn = true;

        try
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .ThenInclude(r => r.Privileges)
                .FirstOrDefaultAsync(u => u.Username == Username && u.IsActive);

            if (user is null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
            {
                ErrorMessage = "Invalid username or password.";
                return;
            }

            _currentUserService.UserId = user.Id;
            _currentUserService.Username = user.Username;
            _currentUserService.RoleName = user.Role.Name;
            _currentUserService.SetPermissions(
                user.Role.Privileges.SelectMany(p => new[]
                {
                    p.CanCreate ? $"{p.EntityName}:Create" : null,
                    p.CanRead ? $"{p.EntityName}:Read" : null,
                    p.CanUpdate ? $"{p.EntityName}:Update" : null,
                    p.CanDelete ? $"{p.EntityName}:Delete" : null,
                }.Where(x => x is not null).Cast<string>()));

            LoginSucceeded?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Connection error: {ex.Message}";
        }
        finally
        {
            IsLoggingIn = false;
        }
    }
}
```

Add BCrypt.Net-Next package to VTE.WPF:

```bash
cd src/VTE.WPF
dotnet add package BCrypt.Net-Next --version 4.*
```

- [ ] **Step 3: Create LoginWindow XAML**

Create `src/VTE.WPF/Views/LoginWindow.xaml`:

```xml
<ui:FluentWindow
    x:Class="VTE.WPF.Views.LoginWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
    Title="VTE - Login"
    Width="400" Height="350"
    WindowStartupLocation="CenterScreen"
    ResizeMode="NoResize"
    ExtendsContentIntoTitleBar="True">

    <Grid Margin="30">
        <StackPanel VerticalAlignment="Center" MaxWidth="300">
            <TextBlock Text="VTE" FontSize="32" FontWeight="Bold" HorizontalAlignment="Center" Margin="0,0,0,8"/>
            <TextBlock Text="Vehicle Technical Examination System" HorizontalAlignment="Center" Margin="0,0,0,30" Foreground="Gray"/>

            <ui:TextBox
                PlaceholderText="Username"
                Text="{Binding Username, UpdateSourceTrigger=PropertyChanged}"
                Margin="0,0,0,12"/>

            <ui:PasswordBox
                PlaceholderText="Password"
                Password="{Binding Password, UpdateSourceTrigger=PropertyChanged}"
                Margin="0,0,0,12"/>

            <TextBlock
                Text="{Binding ErrorMessage}"
                Foreground="Red"
                TextWrapping="Wrap"
                Margin="0,0,0,12"
                Visibility="{Binding ErrorMessage, Converter={StaticResource StringToVisibilityConverter}}"/>

            <ui:Button
                Content="Login"
                Appearance="Primary"
                HorizontalAlignment="Stretch"
                Command="{Binding LoginCommand}"
                IsEnabled="{Binding IsLoggingIn, Converter={StaticResource InverseBoolConverter}}"
                Margin="0,4,0,0"/>
        </StackPanel>
    </Grid>
</ui:FluentWindow>
```

Create `src/VTE.WPF/Views/LoginWindow.xaml.cs`:

```csharp
namespace VTE.WPF.Views;

using System.Windows;
using System.Windows.Data;
using Wpf.Ui.Controls;
using VTE.WPF.ViewModels;

public partial class LoginWindow : FluentWindow
{
    public LoginWindow(LoginViewModel viewModel)
    {
        Resources["StringToVisibilityConverter"] = new StringToVisibilityConverter();
        Resources["InverseBoolConverter"] = new InverseBoolConverter();

        DataContext = viewModel;
        InitializeComponent();

        viewModel.LoginSucceeded += () =>
        {
            DialogResult = true;
            Close();
        };
    }
}

public class StringToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => throw new NotImplementedException();
}

public class InverseBoolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => value is bool b ? !b : value;

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        => value is bool b ? !b : value;
}
```

- [ ] **Step 4: Verify build**

Run: `dotnet build src/VTE.WPF/VTE.WPF.csproj`
Expected: Build succeeded

- [ ] **Step 5: Commit**

```bash
git add .
git commit -m "feat: add login window with BCrypt auth and Fluent UI"
```

---

### Task 10: WPF Shell — Main Window with Navigation

**Files:**
- Create: `src/VTE.WPF/Views/MainWindow.xaml`
- Create: `src/VTE.WPF/Views/MainWindow.xaml.cs`
- Create: `src/VTE.WPF/ViewModels/MainWindowViewModel.cs`
- Create: `src/VTE.WPF/Views/Pages/DashboardPage.xaml`
- Create: `src/VTE.WPF/Views/Pages/DashboardPage.xaml.cs`
- Create: `src/VTE.WPF/ViewModels/DashboardViewModel.cs`
- Create: `src/VTE.WPF/Services/INavigationService.cs`
- Create: `src/VTE.WPF/Services/NavigationService.cs`
- Create: `src/VTE.WPF/Controls/EntityTabControl.cs`

- [ ] **Step 1: Create INavigationService**

Create `src/VTE.WPF/Services/INavigationService.cs`:

```csharp
namespace VTE.WPF.Services;

using System.Windows.Controls;

public interface INavigationService
{
    void OpenTab(string title, UserControl content);
    void CloseTab(string title);
    bool IsTabOpen(string title);
    void ActivateTab(string title);
}
```

- [ ] **Step 2: Create MainWindowViewModel**

Create `src/VTE.WPF/ViewModels/MainWindowViewModel.cs`:

```csharp
namespace VTE.WPF.ViewModels;

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using VTE.Core.Interfaces;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ICurrentUserService _currentUserService;

    [ObservableProperty]
    private string _title = "VTE";

    [ObservableProperty]
    private string _currentUser = string.Empty;

    [ObservableProperty]
    private int _selectedTabIndex;

    public ObservableCollection<TabItemViewModel> OpenTabs { get; } = [];

    public MainWindowViewModel(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        CurrentUser = currentUserService.Username ?? "Unknown";
    }

    public void AddTab(string header, object content)
    {
        var existing = OpenTabs.FirstOrDefault(t => t.Header == header);
        if (existing is not null)
        {
            SelectedTabIndex = OpenTabs.IndexOf(existing);
            return;
        }

        var tab = new TabItemViewModel { Header = header, Content = content };
        OpenTabs.Add(tab);
        SelectedTabIndex = OpenTabs.Count - 1;
    }

    [RelayCommand]
    private void CloseTab(TabItemViewModel tab)
    {
        OpenTabs.Remove(tab);
    }
}

public partial class TabItemViewModel : ObservableObject
{
    [ObservableProperty]
    private string _header = string.Empty;

    [ObservableProperty]
    private object? _content;
}
```

- [ ] **Step 3: Create DashboardViewModel and Page**

Create `src/VTE.WPF/ViewModels/DashboardViewModel.cs`:

```csharp
namespace VTE.WPF.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class DashboardViewModel : ObservableObject
{
    [ObservableProperty]
    private string _welcomeMessage = "Welcome to VTE";

    [ObservableProperty]
    private int _totalCustomers;

    [ObservableProperty]
    private int _totalVehicles;

    [ObservableProperty]
    private int _todayExams;

    [ObservableProperty]
    private int _pendingPayments;
}
```

Create `src/VTE.WPF/Views/Pages/DashboardPage.xaml`:

```xml
<UserControl
    x:Class="VTE.WPF.Views.Pages.DashboardPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">

    <Grid Margin="24">
        <StackPanel>
            <TextBlock Text="{Binding WelcomeMessage}" FontSize="28" FontWeight="SemiBold" Margin="0,0,0,24"/>

            <WrapPanel>
                <Border Background="{DynamicResource ControlFillColorDefaultBrush}" CornerRadius="8" Padding="24" Margin="0,0,12,12" MinWidth="180">
                    <StackPanel>
                        <TextBlock Text="Customers" Foreground="Gray"/>
                        <TextBlock Text="{Binding TotalCustomers}" FontSize="32" FontWeight="Bold"/>
                    </StackPanel>
                </Border>
                <Border Background="{DynamicResource ControlFillColorDefaultBrush}" CornerRadius="8" Padding="24" Margin="0,0,12,12" MinWidth="180">
                    <StackPanel>
                        <TextBlock Text="Vehicles" Foreground="Gray"/>
                        <TextBlock Text="{Binding TotalVehicles}" FontSize="32" FontWeight="Bold"/>
                    </StackPanel>
                </Border>
                <Border Background="{DynamicResource ControlFillColorDefaultBrush}" CornerRadius="8" Padding="24" Margin="0,0,12,12" MinWidth="180">
                    <StackPanel>
                        <TextBlock Text="Exams Today" Foreground="Gray"/>
                        <TextBlock Text="{Binding TodayExams}" FontSize="32" FontWeight="Bold"/>
                    </StackPanel>
                </Border>
                <Border Background="{DynamicResource ControlFillColorDefaultBrush}" CornerRadius="8" Padding="24" Margin="0,0,12,12" MinWidth="180">
                    <StackPanel>
                        <TextBlock Text="Pending Payments" Foreground="Gray"/>
                        <TextBlock Text="{Binding PendingPayments}" FontSize="32" FontWeight="Bold"/>
                    </StackPanel>
                </Border>
            </WrapPanel>
        </StackPanel>
    </Grid>
</UserControl>
```

Create `src/VTE.WPF/Views/Pages/DashboardPage.xaml.cs`:

```csharp
namespace VTE.WPF.Views.Pages;

using System.Windows.Controls;
using VTE.WPF.ViewModels;

public partial class DashboardPage : UserControl
{
    public DashboardPage(DashboardViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();
    }
}
```

- [ ] **Step 4: Create MainWindow XAML**

Create `src/VTE.WPF/Views/MainWindow.xaml`:

```xml
<ui:FluentWindow
    x:Class="VTE.WPF.Views.MainWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml"
    Title="{Binding Title}"
    Width="1400" Height="900"
    WindowStartupLocation="CenterScreen"
    ExtendsContentIntoTitleBar="True">

    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="250"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <!-- Left Navigation -->
        <Border Grid.Column="0" Background="{DynamicResource ControlFillColorDefaultBrush}" Padding="0,40,0,0">
            <DockPanel>
                <!-- User info at bottom -->
                <Border DockPanel.Dock="Bottom" Padding="16,8" Background="{DynamicResource ControlFillColorSecondaryBrush}">
                    <StackPanel>
                        <TextBlock Text="{Binding CurrentUser}" FontWeight="SemiBold" FontSize="13"/>
                    </StackPanel>
                </Border>

                <!-- Navigation items -->
                <ScrollViewer VerticalScrollBarVisibility="Auto">
                    <StackPanel Margin="8,0">
                        <TextBlock Text="VTE" FontSize="22" FontWeight="Bold" Margin="12,4,0,16"/>

                        <ui:Button Content="Dashboard" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavDashboard" Margin="0,1"/>

                        <TextBlock Text="OPERATIONS" FontSize="11" Foreground="Gray" Margin="12,16,0,4"/>
                        <ui:Button Content="Customers" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavCustomers" Margin="0,1"/>
                        <ui:Button Content="Vehicles" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavVehicles" Margin="0,1"/>
                        <ui:Button Content="Requests" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavRequests" Margin="0,1"/>
                        <ui:Button Content="Technical Exams" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavExams" Margin="0,1"/>
                        <ui:Button Content="Documents" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavDocuments" Margin="0,1"/>
                        <ui:Button Content="Payments" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavPayments" Margin="0,1"/>

                        <TextBlock Text="ANALYTICS" FontSize="11" Foreground="Gray" Margin="12,16,0,4"/>
                        <ui:Button Content="Reports" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavReports" Margin="0,1"/>

                        <TextBlock Text="CONFIGURATION" FontSize="11" Foreground="Gray" Margin="12,16,0,4"/>
                        <ui:Button Content="Lookups" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavLookups" Margin="0,1"/>
                        <ui:Button Content="Administration" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavAdmin" Margin="0,1"/>
                        <ui:Button Content="Settings" HorizontalAlignment="Stretch" Appearance="Transparent" Click="OnNavSettings" Margin="0,1"/>
                    </StackPanel>
                </ScrollViewer>
            </DockPanel>
        </Border>

        <!-- Main Content Area with Tabs -->
        <Grid Grid.Column="1">
            <TabControl
                ItemsSource="{Binding OpenTabs}"
                SelectedIndex="{Binding SelectedTabIndex}">
                <TabControl.ItemTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Horizontal">
                            <TextBlock Text="{Binding Header}" VerticalAlignment="Center" Margin="0,0,8,0"/>
                            <ui:Button Content="✕" Appearance="Transparent" FontSize="10" Padding="4,2"
                                       Command="{Binding DataContext.CloseTabCommand, RelativeSource={RelativeSource AncestorType=Window}}"
                                       CommandParameter="{Binding}"/>
                        </StackPanel>
                    </DataTemplate>
                </TabControl.ItemTemplate>
                <TabControl.ContentTemplate>
                    <DataTemplate>
                        <ContentPresenter Content="{Binding Content}"/>
                    </DataTemplate>
                </TabControl.ContentTemplate>
            </TabControl>
        </Grid>
    </Grid>
</ui:FluentWindow>
```

Create `src/VTE.WPF/Views/MainWindow.xaml.cs`:

```csharp
namespace VTE.WPF.Views;

using System.Windows;
using Wpf.Ui.Controls;
using VTE.WPF.ViewModels;
using VTE.WPF.Views.Pages;
using Microsoft.Extensions.DependencyInjection;

public partial class MainWindow : FluentWindow
{
    private readonly MainWindowViewModel _viewModel;
    private readonly IServiceProvider _services;

    public MainWindow(MainWindowViewModel viewModel, IServiceProvider services)
    {
        _viewModel = viewModel;
        _services = services;
        DataContext = viewModel;
        InitializeComponent();

        // Open dashboard on startup
        Loaded += (_, _) => OnNavDashboard(this, new RoutedEventArgs());
    }

    private void OnNavDashboard(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Dashboard", _services.GetRequiredService<DashboardPage>());

    // Placeholder handlers — modules will register their pages later
    private void OnNavCustomers(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Customers", CreatePlaceholder("Customers module — coming soon"));

    private void OnNavVehicles(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Vehicles", CreatePlaceholder("Vehicles module — coming soon"));

    private void OnNavRequests(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Requests", CreatePlaceholder("Requests module — coming soon"));

    private void OnNavExams(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Technical Exams", CreatePlaceholder("Technical Exams module — coming soon"));

    private void OnNavDocuments(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Documents", CreatePlaceholder("Documents module — coming soon"));

    private void OnNavPayments(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Payments", CreatePlaceholder("Payments module — coming soon"));

    private void OnNavReports(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Reports", CreatePlaceholder("Reports module — coming soon"));

    private void OnNavLookups(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Lookups", CreatePlaceholder("Lookups module — coming soon"));

    private void OnNavAdmin(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Administration", CreatePlaceholder("Admin module — coming soon"));

    private void OnNavSettings(object sender, RoutedEventArgs e)
        => _viewModel.AddTab("Settings", CreatePlaceholder("Settings — coming soon"));

    private static System.Windows.Controls.UserControl CreatePlaceholder(string text)
    {
        var control = new System.Windows.Controls.UserControl();
        control.Content = new System.Windows.Controls.TextBlock
        {
            Text = text,
            FontSize = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = System.Windows.Media.Brushes.Gray
        };
        return control;
    }
}
```

- [ ] **Step 5: Verify build**

Run: `dotnet build src/VTE.WPF/VTE.WPF.csproj`
Expected: Build succeeded

- [ ] **Step 6: Commit**

```bash
git add .
git commit -m "feat: add main window with sidebar navigation and tabbed content area"
```

---

### Task 11: App.xaml Startup Wiring

**Files:**
- Modify: `src/VTE.WPF/App.xaml`
- Modify: `src/VTE.WPF/App.xaml.cs`

- [ ] **Step 1: Update App.xaml**

Replace `src/VTE.WPF/App.xaml` content with:

```xml
<Application
    x:Class="VTE.WPF.App"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:ui="http://schemas.lepo.co/wpfui/2022/xaml">

    <Application.Resources>
        <ResourceDictionary>
            <ResourceDictionary.MergedDictionaries>
                <ui:ThemesDictionary Theme="Dark"/>
                <ui:ControlsDictionary/>
            </ResourceDictionary.MergedDictionaries>
        </ResourceDictionary>
    </Application.Resources>
</Application>
```

- [ ] **Step 2: Update App.xaml.cs**

Replace `src/VTE.WPF/App.xaml.cs` content with:

```csharp
namespace VTE.WPF;

using System.IO;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using VTE.Application;
using VTE.Infrastructure;
using VTE.Infrastructure.Data;
using VTE.WPF.ViewModels;
using VTE.WPF.Views;
using VTE.WPF.Views.Pages;

public partial class App : System.Windows.Application
{
    private ServiceProvider? _serviceProvider;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddLogging(builder => builder.AddSerilog());

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<DashboardViewModel>();

        // Views
        services.AddTransient<LoginWindow>();
        services.AddSingleton<MainWindow>();
        services.AddTransient<DashboardPage>();

        _serviceProvider = services.BuildServiceProvider();

        // Ensure database exists
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            db.Database.Migrate();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to initialize database");
        }

        // Show login
        var login = _serviceProvider.GetRequiredService<LoginWindow>();
        if (login.ShowDialog() == true)
        {
            var main = _serviceProvider.GetRequiredService<MainWindow>();
            main.Show();
        }
        else
        {
            Shutdown();
        }
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }
}
```

- [ ] **Step 3: Remove auto-generated MainWindow.xaml if present**

Delete the default `MainWindow.xaml` and `MainWindow.xaml.cs` from the project root (the ones auto-generated by `dotnet new wpf`), as we have our own in `Views/`.

Also update the `.csproj` to remove the `StartupUri` if present — we handle startup in code.

- [ ] **Step 4: Build and verify**

Run: `dotnet build VTE.sln`
Expected: Build succeeded

- [ ] **Step 5: Run all tests**

Run: `dotnet test VTE.sln -v n`
Expected: All tests pass

- [ ] **Step 6: Commit**

```bash
git add .
git commit -m "feat: wire up App startup with DI, login flow, database migration, and main window"
```

---

### Task 12: Initial EF Core Migration

**Files:**
- Create: migration files in `src/VTE.Infrastructure/Migrations/`

- [ ] **Step 1: Add EF Core Design package**

```bash
cd src/VTE.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.*
```

- [ ] **Step 2: Create initial migration**

From the solution root:

```bash
dotnet ef migrations add InitialCreate --project src/VTE.Infrastructure --startup-project src/VTE.WPF --output-dir Data/Migrations
```

Expected: Migration files created in `src/VTE.Infrastructure/Data/Migrations/`

- [ ] **Step 3: Seed an admin user**

Add a seed data method. Create `src/VTE.Infrastructure/Data/SeedData.cs`:

```csharp
namespace VTE.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;

public static class SeedData
{
    public static async Task SeedAsync(VteDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var company = new Company { Name = "Default Company" };
        context.Companies.Add(company);
        await context.SaveChangesAsync();

        var org = new TechnicalExamOrganization
        {
            Name = "Main Station",
            CompanyId = company.Id,
            IsActive = true
        };
        context.TechnicalExamOrganizations.Add(org);
        await context.SaveChangesAsync();

        var role = new Role
        {
            Name = "Administrator",
            Privileges =
            [
                new RolePrivilege { EntityName = "Customer", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Vehicle", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Request", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "TechnicalExamReport", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "Document", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "PaymentDocument", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
                new RolePrivilege { EntityName = "User", CanCreate = true, CanRead = true, CanUpdate = true, CanDelete = true },
            ]
        };
        context.Roles.Add(role);
        await context.SaveChangesAsync();

        var admin = new User
        {
            Username = "admin",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
            FullName = "System Administrator",
            FirstName = "System",
            LastName = "Administrator",
            RoleId = role.Id,
            OrganizationId = org.Id,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}
```

Add BCrypt.Net-Next to Infrastructure:

```bash
cd src/VTE.Infrastructure
dotnet add package BCrypt.Net-Next --version 4.*
```

- [ ] **Step 4: Call SeedData from App startup**

Update the database initialization block in `App.xaml.cs` — replace the try block:

```csharp
try
{
    using var scope = _serviceProvider.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
    db.Database.Migrate();
    SeedData.SeedAsync(db).GetAwaiter().GetResult();
}
catch (Exception ex)
{
    Log.Error(ex, "Failed to initialize database");
}
```

Add `using VTE.Infrastructure.Data;` to the top of App.xaml.cs.

- [ ] **Step 5: Verify build**

Run: `dotnet build VTE.sln`
Expected: Build succeeded

- [ ] **Step 6: Commit**

```bash
git add .
git commit -m "feat: add initial EF Core migration and admin user seed data"
```

---

## Summary

After completing all 12 tasks, you have:

- **VTE.Core**: All 50+ entities and lookups, base classes, interfaces
- **VTE.Application**: DI registration, PagedResult helper
- **VTE.Infrastructure**: EF Core DbContext with full configuration, generic repository, unit of work, seed data, initial migration
- **VTE.WPF**: Working shell app with Fluent UI dark theme, login window with BCrypt auth, sidebar navigation, tabbed content area, dashboard placeholder

The app will start, show a login screen (admin/admin), then open the main window with sidebar navigation. All nav items show placeholder content — the feature modules (Plans 2-8) will replace these.

**Default credentials**: username `admin`, password `admin`
