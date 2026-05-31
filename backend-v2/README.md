# VTE API v2

Greenfield .NET API targeting the **VTE** database (the new clean schema with singular table names: `Client`, `City`, `Country`, `Station`, ...). Sibling to `backend/` (v1, which targets VTE2).

- **Framework:** .NET 10, ASP.NET Core, EF Core 9
- **Auth:** ASP.NET Core Identity, both **JWT bearer** (for SPA / Swagger) and **cookie** (browser flows). One `/api/auth/login` issues a JWT; pass `useCookie: true` to also set the cookie.
- **Multi-tenancy:** users have an optional `CompanyId`. JWT carries it as the `companyId` claim. EF Core query filter scopes `Client` and `Station` to that company. Administrators bypass.
- **Database:** uses the existing `VTE` database on `(localdb)\MSSQLLocalDB`. The 10 hand-rolled business tables (Country, City, Company, Client, ...) are mapped with `ExcludeFromMigrations()` — the project does not own their schema. The **Identity tables (AspNet*)** *are* owned by this project and ship via the `InitialIdentity` EF migration.

## Run

```powershell
# 1. Apply Identity migration (idempotent — only AspNet* tables are owned by us)
cd backend-v2
dotnet ef database update --project src/VTE.Infrastructure --startup-project src/VTE.Api

# 2. Run the API
dotnet run --project src/VTE.Api --launch-profile http
# → http://localhost:5300 (Swagger at /swagger)
```

Default seeded admin: **`admin` / `ChangeMe!Now1`** (Administrator role, no CompanyId — cross-tenant).

## Endpoints (Swagger lists them all)

**Auth**
- `POST /api/auth/login` — `{ userName, password, useCookie? }` → `{ token, expiresAt, userId, userName, fullName, companyId, roles }`
- `POST /api/auth/logout` — clears the cookie session
- `GET  /api/auth/me` — current authenticated identity
- `POST /api/auth/register` *(Administrator)* — create an Operator (or another Administrator)
- `POST /api/auth/change-password` — change own password

**Reference data** (GET = any authenticated; POST/PUT/DELETE = Administrator)
- `/api/countries`, `/api/communities`, `/api/cities`, `/api/citizenships`
- `/api/document-issuers`, `/api/personal-data-types`

**Companies** *(Administrator only — manages tenants)*
- `/api/companies`

**Stations** (tenant-scoped: Operators see their own company's stations only)
- `/api/stations`

**Clients & Personal Data** (tenant-scoped)
- `/api/clients` — list (paged, `?search=&page=&pageSize=`), get, create, update, delete (admin)
- `/api/client-personal-data/by-client/{clientId}` — list, get, create, update, delete

## Project layout

```
backend-v2/
  VTE.slnx
  src/
    VTE.Domain/                # POCO entities, AspNet Identity user/role subclasses
    VTE.Infrastructure/        # VteDbContext, ITenantContext, DesignTimeDbContextFactory, migrations
    VTE.Api/                   # Program.cs, Auth, Controllers, Dtos, Seed, Tenancy impl
```

## Notes on migration ownership

This project deliberately does NOT own the schema of the 10 business tables (`Client`, `ClientPersonalData`, `Country`, `Community`, `City`, `Citizenship`, `Company`, `Station`, `DocumentIssuer`, `PersonalDataType`) — they were created by hand against the VTE database. They are `ExcludeFromMigrations()` in `VteDbContext`. If those tables ever need EF-managed evolution, drop the exclusion and scaffold a baseline migration.
