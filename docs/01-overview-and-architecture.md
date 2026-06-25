# Overview & Architecture

**VTE** is a multi-tenant SaaS rewrite (.NET 10 backend + Vue 3 frontend) of a legacy
VB.NET WinForms vehicle technical-inspection system used by an inspection station in
Велес, Macedonia. The new code lives under `backend-v2/` (ASP.NET Core + EF Core) and
`frontend-v2/` (Vue 3 + TypeScript), with the original WinForms application (`VTE/`,
`WinApp/`, `VTE.Library/`, …) kept in-repo purely for parity-checking. This document
explains what the system is, its tech stack and solution layout, the Clean-ish layering,
the multi-tenancy and soft-delete models, the JWT auth flow, application startup, and how
to run everything locally.

For deeper dives, see the sibling docs (e.g. [Payments & pricing](05-payments-and-pricing.md)).
The repo's own quick-reference is [`CLAUDE.md`](../CLAUDE.md) and [`README.md`](../README.md).

---

## 1. What VTE is and who uses it

VTE digitises the day-to-day workflow of a Macedonian vehicle-inspection / registration
station:

- **Clients (Комитенти)** — citizens and businesses (`Client`, with `MB`/EMBG, tax number,
  address, citizenship).
- **Vehicles (Возила)** — the inspected/registered vehicles and their registrations
  (`Vehicle`, `VehicleRegistration`), linked to clients through `ClientVehicleRelation`.
- **Requests (Барања)** — registration/transfer/deregistration workflows backed by the
  three legacy paper forms: **Plav** (plav образец — registration), **Bel** (бел образец —
  ownership transfer), **Zelen** (зелен образец — deregistration). Seeded in
  `backend-v2/src/VTE.Api/Seed/DataSeeder.cs`.
- **Technical exams (Технички преглед)** — inspection reports with per-part detail rows
  (`TechnicalExamReport`, `TechnicalExamReportDetail`).
- **Payments / billing (Наплата)** — the pricing catalog, auto-generated debts
  (`CustomerDebt`), bills (`PaymentDocument` + lines), and installment plans.

The end users are **station operators** (role `Operator`) who process daily work, and
**administrators** (role `Administrator`) who manage companies, stations, users and
catalogs. The product is multi-tenant: each tenant is a **Company** and operators only ever
see their own company's data.

---

## 2. Tech stack

| Layer | Technology | Notes / source |
|-------|-----------|----------------|
| Backend runtime | .NET 10 / ASP.NET Core | `backend-v2/src/VTE.Api/Program.cs` |
| ORM | EF Core (SQL Server provider) | `Program.cs` → `opts.UseSqlServer(...)` |
| AuthN/Z | ASP.NET Identity + JWT bearer + cookie | `Program.cs`, `VTE.Api/Auth/` |
| DB (local dev) | SQL Server LocalDB `(localdb)\MSSQLLocalDB`, database `VTE` | `appsettings.json` |
| DB (dev override) | SQL Server on `127.0.0.1,14333` (sa) | `appsettings.Development.json` |
| DB (prod) | SQL Server 2025 Express in Docker | see [§11](#11-deployment-pointers) |
| API docs | Swagger / Swashbuckle | `Program.cs` → `AddSwaggerGen` |
| Frontend | Vue 3 + TypeScript | `frontend-v2/` |
| Build/dev server | Vite 8 | `frontend-v2/vite.config.ts` |
| State | Pinia 3 | `frontend-v2/src/stores/` |
| UI kit | PrimeVue 4 (+ `@primeuix/themes`, primeicons) | `frontend-v2/package.json` |
| i18n | vue-i18n 9 (MK + EN) | `frontend-v2/src/locales/{mk,en}.ts` |
| HTTP client | axios | `frontend-v2/src/api/client.ts` |
| Routing | vue-router 4 | `frontend-v2/src/router/` |

Frontend dependency versions are pinned in `frontend-v2/package.json` (Vue `^3.5.32`,
PrimeVue `^4.5.5`, Pinia `^3.0.4`, vue-i18n `^9.14.5`, Vite `^8.0.10`, TypeScript `~6.0.2`).

---

## 3. Repository & solution layout

```
trunk/
├── backend-v2/
│   └── src/
│       ├── VTE.Domain/          POCO entities + enums. No EF dependency.
│       ├── VTE.Infrastructure/  VteDbContext, EF configs, Migrations/, tenancy, pricing.
│       └── VTE.Api/             Controllers, DTOs, Program.cs, Seed/DataSeeder.cs, Auth/.
├── frontend-v2/                 Vue 3 + TS + Vite + Pinia + PrimeVue + vue-i18n (MK/EN).
├── migrate/                     SQL scripts: legacy VTEZVV → v2.
├── db/                          v2 schema script + fresh-machine setup README.
├── docs/                        This documentation set.
├── deploy/                      build-release.ps1 + prod deployment helpers.
├── VTE/, WinApp/, VTE.Library/, VTE.BaseParts/   Legacy VB.NET WinForms (reference only).
└── backend/, frontend/          Older v1 rewrite attempts — superseded, do not run.
```
(Source: `README.md` "Folder layout"; `CLAUDE.md` "Architecture".)

### Backend project structure (Clean-ish layering)

The backend follows a layered dependency rule: **Api → Infrastructure → Domain** (inner
layers never reference outer ones).

| Project | Responsibility | Key contents |
|---------|---------------|--------------|
| **VTE.Domain** | Pure POCO entities + enums, no EF/framework deps | `Clients/`, `Vehicles/`, `Requests/`, `TechnicalExams/`, `Payments/`, `Companies/`, `Stations/`, `Geography/`, `References/`, `Identity/`, `Common/` (`ITenantOwned.cs`). |
| **VTE.Infrastructure** | Data access + cross-cutting services | `Persistence/VteDbContext.cs`, `Migrations/`, `Tenancy/ITenantContext.cs`, `Pricing/` (`PricingEvaluator`, `DebtService`). |
| **VTE.Api** | HTTP surface | `Program.cs`, `Controllers/` (25 controllers), `Auth/` (`AuthController`, `JwtTokenService`), `Tenancy/TenantContext.cs`, `Seed/DataSeeder.cs`, DTOs. |

The one subtlety to the layering: the **`ITenantContext` interface lives in
Infrastructure** (`VTE.Infrastructure/Tenancy/ITenantContext.cs`) so `VteDbContext` can
consume it, but its **HTTP-backed implementation `TenantContext` lives in the Api layer**
(`VTE.Api/Tenancy/TenantContext.cs`) because it reads `HttpContext`. DI wires the two
together in `Program.cs`.

---

## 4. Multi-tenancy

Multi-tenancy is the architectural backbone. Each tenant is a **Company**
(`VTE.Domain/Companies/Company.cs` — `Id` is a `byte`/`tinyint`). Every business entity is
owned by a company and is filtered automatically; **controllers never write
`.Where(x => x.CompanyId == ...)` for per-tenant scoping** — the DbContext does it.

### 4.1 `ITenantOwned` marker

`backend-v2/src/VTE.Domain/Common/ITenantOwned.cs`:

```csharp
/// <summary>Marker interface for entities that are scoped to a Company (tenant).</summary>
public interface ITenantOwned
{
    byte CompanyId { get; }
}
```

The tenant-owned entities (each carries `byte CompanyId` and implements `ITenantOwned`):
`Client`, `Vehicle`, `Request`, `TechnicalExamReport`, `PaymentDocument`,
`PaymentDocumentLine`, `InstallmentAgreement`, `InstallmentSchedule`, `CustomerDebt`,
`Station` (verified via grep for `ITenantOwned` across `VTE.Domain`).

### 4.2 `ITenantContext` — resolving the current tenant from JWT claims

The tenant for the in-flight request is resolved from the authenticated principal, not from
any per-call parameter. Interface (`VTE.Infrastructure/Tenancy/ITenantContext.cs`):

```csharp
public interface ITenantContext
{
    byte? CompanyId { get; }   // from the JWT/cookie claim; null for Admins / anonymous
    bool IsAdmin { get; }      // true when caller has the Administrator role
    string? UserId { get; }    // AspNetUsers.Id, or null when unauthenticated
}
```

The HTTP implementation (`VTE.Api/Tenancy/TenantContext.cs`) reads the claims off
`HttpContext.User`:

```csharp
public class TenantContext : ITenantContext
{
    public const string CompanyIdClaim = "companyId";

    public byte? CompanyId
    {
        get
        {
            var raw = _http.HttpContext?.User?.FindFirst(CompanyIdClaim)?.Value;
            return byte.TryParse(raw, out var v) ? v : (byte?)null;
        }
    }

    public bool IsAdmin => _http.HttpContext?.User?.IsInRole("Administrator") ?? false;

    public string? UserId => _http.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
}
```

The `"companyId"` claim is minted into the token at login time (see [§6](#6-jwt-authentication-flow)).
A `NullTenantContext` (in the same interface file) is the design-time/seeding stand-in —
`IsAdmin => true`, so migrations and the seeder bypass all filters.

### 4.3 The global EF query filter

Every tenant-owned entity declares a global query filter in
`backend-v2/src/VTE.Infrastructure/Persistence/VteDbContext.cs`. The DbContext receives
`ITenantContext` via constructor injection and uses it inside the filter expression. The
pattern is identical on every tenant entity:

```csharp
// e.g. Client (VteDbContext.cs ~line 205)
e.HasQueryFilter(x => _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId);
```

The same `_tenant.IsAdmin || x.CompanyId == _tenant.CompanyId` filter is applied to
`Station`, `Client`, `Vehicle`, `Request`, `TechnicalExamReport`, `InstallmentAgreement`,
`PaymentDocument`, `CustomerDebt` (verified in `VteDbContext.cs`). Consequences:

- An **Operator** (whose token carries a `companyId`) transparently sees only their own
  company's rows on every read, and writes are scoped the same way.
- An **Administrator** (`IsAdmin == true`, no `companyId` claim) **bypasses** the filter and
  sees all tenants' data.

To deliberately read across the filter (e.g. to look up a company name for an admin) the
code calls `.IgnoreQueryFilters()` — for example in `AuthController.CompanyNameAsync` and in
`DataSeeder` when finding/creating the default Company and Station.

### 4.4 Why controllers don't write `.Where(CompanyId == …)`

Because the filter is global, the typical read is just `_db.Clients.AsNoTracking()…` and it
is already tenant-scoped. The handful of `CompanyId ==` `.Where` clauses that *do* exist in
controllers are **admin-facing optional filters**, not the security boundary. Example from
`Controllers/ClientsController.cs`:

```csharp
// For Administrators, optionally narrow to a single Company via ?companyId=.
// For Operators, the tenant query filter on the DbContext already scopes results.
if (companyId.HasValue)
    query = query.Where(c => c.CompanyId == companyId.Value);
```

(Similar optional `?companyId=` narrowing appears in `VehiclesController`,
`RequestsController`, `UsersController`; `PriceCatalogsController` filters on
`PriceCompanyId`.) These only let an admin drill into one tenant — they are not what keeps
operators isolated.

### 4.5 How `CompanyId` is stamped on insert

On create, controllers set `CompanyId` from the tenant context rather than trusting client
input. From `ClientsController.Create`:

```csharp
byte companyId;
if (_tenant.IsAdmin && dto.CompanyId.HasValue) {            // admin may choose a tenant
    var exists = await _db.Companies.AnyAsync(co => co.Id == dto.CompanyId.Value);
    if (!exists) return BadRequest(...);
    companyId = dto.CompanyId.Value;
} else if (_tenant.CompanyId.HasValue) {                    // operator → own company
    companyId = _tenant.CompanyId.Value;
} else {                                                     // admin w/o choice → first company
    companyId = (await _db.Companies.OrderBy(c => c.Id)...).Value;
}
var entity = new Client { CompanyId = companyId, ... };
```

So an operator's submitted `dto.CompanyId` is ignored; the value comes from their JWT claim.

---

## 5. Soft-delete (`Active`)

Business entities are **soft-deleted** by setting `Active = false` rather than removing the
row. For example, `ClientsController.Delete` does:

```csharp
c.Active = false;   // not _db.Clients.Remove(c)
```

`Company` itself carries `bool Active` (`Companies/Company.cs`), as do the other business
entities.

> **Important nuance — read this carefully.** The global query filters in `VteDbContext`
> filter on **`CompanyId` only**; they do **not** automatically exclude `Active = false`
> rows. (`CLAUDE.md` states the filter "typically excludes `Active = false`" — that is **not**
> accurate for the v2 query filters as written.) `Active` is returned in read DTOs (e.g.
> `ClientReadDto` includes `c.Active`) and is filtered explicitly only where a feature needs
> it. Where `Active` matters for performance it appears in **filtered indexes**, e.g.
> `IX_Request_Open` (`[Active] = 1 AND [EndedAt] IS NULL`) and `IX_CustomerDebt_Open`
> (`[Active] = 1 AND [Paid] = 0`) in `VteDbContext.cs`. Treat "is this row active?" as an
> explicit, per-query concern — not something the ORM hides for you.

Foreign keys mostly use `DeleteBehavior.Restrict` (so a hard delete of a parent doesn't
silently cascade), with `Cascade` reserved for owned children (e.g.
`ClientPersonalData → Client`, `RequestOwnershipProof → Request`,
`TechnicalExamReportDetail → TechnicalExamReport`).

---

## 6. JWT authentication flow

Auth lives in `backend-v2/src/VTE.Api/Auth/`. The API accepts **either** a JWT bearer token
**or** an auth cookie; JWT is the default scheme (`Program.cs`).

### 6.1 Roles

`VTE.Domain/Identity/ApplicationRole.cs`:

```csharp
public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Operator      = "Operator";
}
```

`ApplicationUser` (`VTE.Domain/Identity/ApplicationUser.cs`) extends `IdentityUser` with:
`byte? CompanyId` (null for cross-tenant Administrators), `string? FullName`,
`bool IsActive`, `DateTime CreatedAt`.

### 6.2 Login → token

`POST /api/auth/login` (`AuthController.Login`, `[AllowAnonymous]`):

1. Find the user by name; reject if missing or `!IsActive`.
2. `_signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true)` — wrong
   passwords increment the lockout counter; a locked account returns
   `401 "Account is locked."`.
3. Load roles; issue a JWT via `JwtTokenService.IssueToken`.
4. If `req.UseCookie`, also sign in the cookie scheme.
5. Return `LoginResponse(token, expires, userId, userName, fullName, companyId, companyName, roles)`.

`JwtTokenService.IssueToken` (`Auth/JwtTokenService.cs`) packs these claims:
`sub` / `NameIdentifier` (user id), `unique_name` / `Name` (username), `jti`, `email`
(if present), one `ClaimTypes.Role` per role, and — crucially for tenancy —

```csharp
if (user.CompanyId.HasValue)
    claims.Add(new Claim(TenantContext.CompanyIdClaim, user.CompanyId.Value.ToString()));
```

i.e. the `"companyId"` claim that `TenantContext.CompanyId` later reads. The token is signed
HMAC-SHA256 with `Jwt:Secret` and expires after `Jwt:AccessTokenMinutes` (default 480 = 8h).

### 6.3 Token validation

`Program.cs` configures `AddJwtBearer` with full validation (issuer, audience, lifetime,
signing key) against `Jwt:Issuer` / `Jwt:Audience` / `Jwt:Secret`, with a 2-minute
`ClockSkew`. `RequireHttpsMetadata = false` (dev-friendly). The default authorization policy
accepts **either** the JWT **or** the cookie scheme and requires an authenticated user.

For an API, the cookie handler is configured to **return 401/403 instead of redirecting** to
a login page (`OnRedirectToLogin` / `OnRedirectToAccessDenied` overrides in `Program.cs`).

### 6.4 Other auth endpoints

| Method & route | Auth | Purpose |
|----------------|------|---------|
| `POST /api/auth/login` | anonymous | Issue JWT (+ optional cookie). |
| `POST /api/auth/logout` | anonymous (no `[Authorize]`) | Sign out the cookie session (no-op for pure-JWT). |
| `GET /api/auth/me` | authenticated | Current identity + roles + company. |
| `POST /api/auth/register` | **Administrator only** | Create an Operator/Administrator bound to a `CompanyId`. |
| `POST /api/auth/change-password` | authenticated | Set a new password (no current-password challenge — identity already proven by token). |
| `PUT /api/auth/profile` | authenticated | Update own `FullName`/`Email` (username & company are not self-mutable). |

### 6.5 Frontend side

`frontend-v2/src/api/client.ts` creates an axios instance with `baseURL: '/api'`,
attaches `Authorization: Bearer <token>` from the Pinia auth store on every request, and on
any `401` clears the session and redirects to `/login`.

---

## 7. Application startup (`Program.cs`)

`backend-v2/src/VTE.Api/Program.cs` wires everything in this order:

1. **JWT options** bound from config (`Jwt` section) and registered as a singleton, plus
   `JwtTokenService`.
2. **EF Core + Identity** — `AddDbContext<VteDbContext>` using SQL Server with a **120s
   command timeout** (the default 30s is too tight for the first-run vehicles search across
   ~66k relations until query plans warm up). Identity is added with **relaxed password
   rules** for dev (min length 6, no digit/upper/lower/non-alnum requirements,
   `RequireUniqueEmail = false`) — the comment in code says "Tighten before production."
3. **Tenancy** — `AddHttpContextAccessor()` and `AddScoped<ITenantContext, TenantContext>()`.
4. **Pricing** — `IPricingEvaluator → PricingEvaluator`, `IDebtService → DebtService`
   (scoped). See [Payments & pricing](05-payments-and-pricing.md).
5. **Authentication** — JWT (default) + cookie, as in [§6](#6-jwt-authentication-flow).
6. **Authorization** — default policy accepts JWT or cookie and requires an authenticated user.
7. **CORS** — a default policy allowing the origins in `Cors:AllowedOrigins`, with
   `AllowCredentials()`.
8. **MVC controllers + Swagger** (with a `Bearer` security definition).

The HTTP pipeline: `UseSwagger` / `UseSwaggerUI` (at `/swagger`) → `UseCors` →
`UseAuthentication` → `UseAuthorization` → `MapControllers`.

**SPA hosting:** in production the Vue build is copied into `wwwroot`
(`deploy/build-release.ps1`) and served from the same origin (no CORS/separate host); unknown
client-side routes fall back to `index.html`. In dev there is no `wwwroot/index.html`, so the
API maps `GET /` to a redirect to `/swagger` and Vite serves the SPA separately.

### 7.1 Migrate + seed on boot

After `app.Build()`, the API **migrates and seeds automatically** (`Program.cs`, lines
~179-206):

```csharp
try
{
    for (var attempt = 1; ; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            await db.Database.MigrateAsync();
            break;
        }
        catch (Exception ex) when (attempt < 4)
        {
            // log + wait 10s and retry (SQL may accept connections before its
            // engine is fully warm in container/cloud deployments)
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
    await DataSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    // Don't crash the host on a transient DB outage — DB-bound endpoints will
    // surface the real error on first call; Swagger / "/" stay healthy.
}
```

So a fresh `dotnet run` against an empty database creates the schema and seeds a working app.
The migration step retries up to 4 times (10s apart) and the whole block is wrapped so a DB
outage doesn't prevent the host from booting.

### 7.2 What `DataSeeder` seeds

`backend-v2/src/VTE.Api/Seed/DataSeeder.cs` (idempotent — every step checks-then-inserts):

1. **Roles** `Administrator` and `Operator`.
2. **Default Company** (`Seed:DefaultCompanyName`, default "Default Company") — looked up via
   `IgnoreQueryFilters()` so seeding works without a tenant context.
3. **Default Station** (`Seed:DefaultStationName`) bound to that company.
4. **Default Administrator** — created from config and added to the `Administrator` role.
   Note: `CompanyId = null` ("Administrators are cross-tenant"), `IsActive = true`,
   `EmailConfirmed = true`.
5. **Request-module catalogs** (`SeedRequestCatalogsAsync`) — but **only if no `RequestType`
   exists yet**, so migrated legacy data is never re-seeded. On an empty DB it seeds:
   - The three `RequestDocumentPrint` rows: `PLAV`, `BEL`, `ZELEN`.
   - Starter `RequestType` rows (Прва регистрација, Продолжување на регистрација, Пренос на
     сопственост, Одјава на возило, Технички преглед) with their print form, tech-exam
     requirement and flags (`PaymentRequired`, `IssuesNewRegistration`, `TransfersOwnership`,
     `DeactivatesRelation`, …).
   - Proof-type / attachment-type catalogs (ownership proofs, payment proofs, attachment
     types).

### 7.3 Default seeded admin login

| Field | Value | Config key (`appsettings.json` → `Seed`) |
|-------|-------|------------------------------------------|
| Username | `admin` | `Seed:DefaultAdminUserName` |
| Password | `ChangeMe!Now1` | `Seed:DefaultAdminPassword` |
| Email | `admin@local` | `Seed:DefaultAdminEmail` |
| Role | `Administrator` | — |
| CompanyId | `null` (cross-tenant) | — |

> **Production note:** the *production* admin password is **not** this dev default — it is the
> strong secret in `deploy/prod.secrets.local` (per `CLAUDE.md`). Change the dev default from
> the Account Settings panel after first login. Repeated wrong passwords trip ASP.NET
> Identity lockout (`CheckPasswordSignInAsync(..., lockoutOnFailure: true)`).

---

## 8. Idempotent operations

The system is designed so re-running ingestion/auto-creation is safe:

- **Auto-created debts** (`CustomerDebt`) use the source as a uniqueness key. `DebtService`
  (`VTE.Infrastructure/Pricing/DebtService.cs`) checks before inserting:

  ```csharp
  // Idempotency: if any debt already exists for this source, skip.
  var existing = await _db.CustomerDebts.AsNoTracking().AnyAsync(d =>
        ...
     && ((isRequest  && d.OriginRequestId       == originId)
      || (isTechExam && d.OriginTechnicalExamId == originId)), ...);
  ```

  i.e. re-creating a debt for the same `(Origin, OriginRequestId)` or
  `(Origin, OriginTechnicalExamId)` is a no-op. `CustomerDebt` also has a unique filtered
  index on `LegacyId` (`IX` with `[LegacyId] IS NOT NULL`) so legacy-sync imports one v2 row
  per legacy source row.
- **The seeder** is idempotent (check-then-insert at every step; skips catalog seeding once
  any `RequestType` exists).
- **Migration scripts** in `migrate/` are written to be safely re-runnable (per `CLAUDE.md`
  working-style notes).

See [Payments & pricing](05-payments-and-pricing.md) for the full debt/pricing model.

---

## 9. Configuration

Backend configuration is standard ASP.NET layered config: `appsettings.json` (committed
defaults) overlaid by `appsettings.Development.json` (gitignored override) and then
environment variables (`Section__Key` form, used in prod).

### 9.1 `appsettings.json` (committed defaults)

| Section / key | Default | Meaning |
|---------------|---------|---------|
| `ConnectionStrings:Default` | `Server=(localdb)\MSSQLLocalDB;Database=VTE;Trusted_Connection=True;…` | LocalDB connection used by `AddDbContext`. |
| `Jwt:Secret` | `change-me-in-production-…` | HMAC signing key (override in prod). |
| `Jwt:Issuer` / `Jwt:Audience` | `VTE-v2` / `VTE-v2` | Validated on every token. |
| `Jwt:AccessTokenMinutes` | `480` | Token lifetime (8h). |
| `Cookie:Name` | `vte.v2.auth` | Auth-cookie name. |
| `Cookie:ExpireHours` | `8` | Cookie sliding lifetime. |
| `Cors:AllowedOrigins` | `http://localhost:5173`, `http://localhost:5174` | Allowed SPA origins. |
| `Seed:*` | admin/company/station defaults | See [§7.3](#73-default-seeded-admin-login). |
| `Storage:RequestAttachmentsRoot` | `./storage/request-attachments` | Upload root. |
| `Storage:MaxAttachmentBytes` | `20971520` (20 MB) | Max upload size. |
| `Storage:AllowedAttachmentMimeTypes` | jpeg/png/gif/webp/pdf | Allowed upload types. |
| `LegacySync:Enabled` | `true` (dev); **`false` in prod** | Toggle for the legacy DB sync. |
| `LegacySync:*` | linked-server/host/db/user | Legacy `VTEZVV` connection. |
| `Requests:AutoCreateTechExam` | `true` | Auto-create a tech exam on qualifying requests. |
| `Requests:DefaultTechnicalExamTypeId` | `1` | РЕД-12М exam type. |
| `Requests:DefaultTechnicalExamOrganizationId` | `37` | The station's tech-exam org (АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ). |

### 9.2 `appsettings.Development.json` (override)

Overrides logging and the connection string to point at a local SQL Server on
`127.0.0.1,14333` with `sa` auth (instead of LocalDB):

```json
"ConnectionStrings": {
  "Default": "Server=127.0.0.1,14333;Database=VTE;User Id=sa;Password=…;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true"
}
```

> **Note:** because this file sets a `Default` connection string and is loaded in the
> `Development` environment (the `http` launch profile sets `ASPNETCORE_ENVIRONMENT=Development`),
> **a local `dotnet run` connects to `127.0.0.1,14333`, not LocalDB**, unless this override is
> absent. `README.md`/`CLAUDE.md` describe the LocalDB path; both connection strings target a
> database named `VTE`.

### 9.3 Production config

In production, secrets are supplied via environment variables in `/opt/vte/.env` (mirrored
from `deploy/prod.secrets.local` on the maintainer's laptop): SA password, JWT secret, admin
password, and `LegacySync__Enabled=false`. See [§11](#11-deployment-pointers).

---

## 10. Running locally

### 10.1 Backend

```bash
cd backend-v2/src/VTE.Api
dotnet restore        # first time
dotnet run            # auto-migrates the DB + seeds the admin user on first start
```

The committed launch profile (`VTE.Api/Properties/launchSettings.json`) is `http` and listens
on **`http://localhost:5300`** with `ASPNETCORE_ENVIRONMENT=Development`.

> **Port caveat:** `README.md`/`CLAUDE.md` mention `https://localhost:7165`, but the actual
> committed `launchSettings.json` profile binds **`http://localhost:5300`**, and
> `vite.config.ts` proxies the SPA's `/api` calls to `http://localhost:5300`. Treat 5300 as
> the authoritative dev API port. Swagger UI is at `/swagger`.

### 10.2 Frontend

```bash
cd frontend-v2
npm install           # first time
npm run dev           # Vite dev server
```

`frontend-v2/vite.config.ts` pins the dev server to **port 5174** (`strictPort: true`) and
**proxies `/api` to `http://localhost:5300`**, so the SPA and API behave as same-origin in
dev. (Both `5173` and `5174` are whitelisted in `Cors:AllowedOrigins` for direct
cross-origin calls.) The axios client uses `baseURL: '/api'`, relying on that proxy.

> **Port caveat:** `README.md`/`CLAUDE.md` say the frontend runs on `5173`; the committed
> `vite.config.ts` uses **`5174`**. Use whichever your local config actually binds.

### 10.3 First login

Open the Vite dev URL and log in with `admin` / `ChangeMe!Now1` (the seeded admin), then
change the password from Account Settings.

### 10.4 Common dev commands (from `README.md`)

```bash
# Type-check the frontend
cd frontend-v2 && npx vue-tsc -b --force

# Add an EF migration after a model change
cd backend-v2
dotnet ef migrations add MyChange --project src/VTE.Infrastructure --startup-project src/VTE.Api

# Apply pending migrations
dotnet ef database update --project src/VTE.Infrastructure --startup-project src/VTE.Api
```

> **Build-lock gotcha (`CLAUDE.md` #1):** `dotnet build` fails with MSB3027 while the API is
> running (the exe is locked). For EF commands during dev, add `--no-build`.

### 10.5 Migrations

EF owns the **full** schema. As of the 2026-06-12 squash, the migration chain lives in
`backend-v2/src/VTE.Infrastructure/Migrations/` (EF's default folder), currently:
`20260612231002_InitialSchema`, `20260612234823_AddCustomerDebtLegacyId`,
`20260625115717_AddDocumentIssuerCommunityId`, plus `VteDbContextModelSnapshot.cs`.

> **Do not restore the old `Persistence/Migrations/` chain from git history** (`CLAUDE.md`
> #6) — it could never build a fresh DB. The single squash is authoritative.

---

## 11. Deployment pointers

(Summary only — confirm against `deploy/` and the server before acting; see `CLAUDE.md`
"Production server".)

- **URL:** `https://116.202.8.155.sslip.io` — Hetzner CPX22 (Falkenstein), Ubuntu 24.04, live
  since 2026-06-12.
- **Stack:** Docker Compose at `/opt/vte` — `mssql` (SQL Server 2025 Express, 1.5 GB cap),
  `api` (.NET 10 + the SPA build in `wwwroot`), `caddy` (auto-HTTPS via sslip.io).
- **Redeploy:** `.\deploy\build-release.ps1` → scp `release.zip` to `/opt/vte/app/` → unzip →
  `docker compose up -d --build api`.
- **Secrets:** `/opt/vte/.env` on the server (mirrors `deploy/prod.secrets.local`); admin
  password is the strong one, **not** the dev default; `LegacySync__Enabled=false` in prod.
- **Data scale (prod):** ~228k bills, ~1.15M lines, ~254k installments, ~7k clients, ~6.9k
  vehicles, ~18k requests; DB ~785 MB. (Per `CLAUDE.md` — figures are operational notes, not
  verified from code here.)

---

## 12. Quick architecture map

```
                         ┌──────────────────────────────────────────┐
   Browser (Vue 3 SPA)   │  frontend-v2/                              │
   axios baseURL '/api'  │   stores/auth (JWT)  router (guards)       │
        │  Bearer <jwt>   │   PrimeVue 4 · vue-i18n (MK/EN)           │
        ▼                 └──────────────────────────────────────────┘
   ┌────────────────────────────────────────────────────────────────┐
   │ VTE.Api                                                          │
   │   Program.cs (JWT+cookie, CORS, Swagger, Migrate()+Seed())      │
   │   Controllers ── never .Where(CompanyId==) for scoping          │
   │   Auth/ (AuthController, JwtTokenService)                        │
   │   Tenancy/TenantContext  ── reads "companyId" claim             │
   └───────────────┬────────────────────────────────────────────────┘
                   │ ITenantContext (CompanyId, IsAdmin, UserId)
   ┌───────────────▼────────────────────────────────────────────────┐
   │ VTE.Infrastructure                                              │
   │   VteDbContext  ── global query filter:                         │
   │        _tenant.IsAdmin || x.CompanyId == _tenant.CompanyId      │
   │   Migrations/ (InitialSchema squash + 2)                        │
   │   Pricing/ (PricingEvaluator, DebtService — idempotent)         │
   └───────────────┬────────────────────────────────────────────────┘
                   │ POCOs implement ITenantOwned (byte CompanyId)
   ┌───────────────▼────────────────────────────────────────────────┐
   │ VTE.Domain  ── entities + enums, no framework deps              │
   └────────────────────────────────────────────────────────────────┘
                   │
                   ▼
            SQL Server (database "VTE")
```

---

### See also

- [`README.md`](../README.md) — fresh-machine setup and folder layout.
- [`CLAUDE.md`](../CLAUDE.md) — hot gotchas, legacy pricing hierarchy, domain enums.
- [Data model](02-data-model.md) — the full entity/table reference for every module.
- [Requests](03-requests.md), [Technical exams](04-technical-exams.md) — the two core
  workflows.
- [Payments & pricing](05-payments-and-pricing.md) — the pricing catalog, `CustomerDebt`,
  `PricingEvaluator`/`DebtService`, and billing flow.
- [Frontend](08-frontend.md) — the Vue 3 SPA structure, auth store, routing and i18n.
- [Deployment & operations](09-deployment-and-operations.md) and
  [Data migration](10-data-migration.md) — production stack and legacy import.
