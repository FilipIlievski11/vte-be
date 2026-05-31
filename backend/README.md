# VTE backend (.NET 9)

Multi-tenant SaaS API for the rewritten VTE (vehicle technical-inspection) system.

## Stack

- ASP.NET Core 9 Web API
- EF Core 9 (SQL Server provider)
- ASP.NET Core Identity (custom `ApplicationUser`, two hard-coded roles `Administrator` and `Operator`)
- JWT bearer auth
- Swagger / OpenAPI

## Solution layout

```
backend/
├── VTE.slnx
├── src/
│   ├── VTE.Domain/          ← entities (POCOs)
│   ├── VTE.Infrastructure/  ← DbContext, EF Core mappings
│   └── VTE.Api/             ← controllers, auth, tenancy, seed
└── tests/
    └── VTE.Api.Tests/       ← xUnit
```

## Prerequisites

- .NET 9 SDK installed (verified: `dotnet --list-sdks` shows `9.0.x`).
- SQL Server LocalDB running (`(localdb)\MSSQLLocalDB`) with the `VTE2` database created. Run the bootstrap SQL scripts in `docs/superpowers/work/` in this order before first run:
  1. `bootstrap-vte2.sql` — auth tables (Stations, Operators, AspNet*).
  2. `bootstrap-customers.sql`
  3. `bootstrap-vehicles.sql`
  4. `bootstrap-requests.sql`
  5. `bootstrap-payments.sql`
  6. `bootstrap-documents.sql`
  7. `bootstrap-reference-data.sql`
  8. `bootstrap-fks-and-attachments.sql`

## Run

```bash
cd backend
dotnet run --project src/VTE.Api --launch-profile http
```

Default URL: `http://localhost:5258`.
Swagger: `http://localhost:5258/swagger`.

On first run, the seed step creates:
- The two roles (Administrator, Operator) — already in DB seed, re-checked here.
- A "Default Station" if `Stations` is empty.
- An Administrator user from `appsettings.json` → `Seed:DefaultAdminPassword`.

**Default admin credentials (change in production):**
- Username: `admin`
- Password: `ChangeMe!Now1`

## Authentication flow

1. `POST /api/auth/login` with `{ "userName": "admin", "password": "..." }`.
   Response: `{ "token": "<JWT>", "userName": "admin", "roles": ["Administrator"], "stationId": null }`.
2. Subsequent requests: `Authorization: Bearer <JWT>`.

The JWT carries the role(s) and (for Operators) a `stationId` claim. Administrators have no `stationId`; their requests are not station-scoped.

## Tenancy model

- **Operator** requests: API resolves `StationId` from JWT and the EF Core query filter on `Customer` and `Vehicle` automatically restricts results to that station.
- **Administrator** requests: cross-tenant. Query filter is bypassed because `StationId` is null.

`ITenantContext` (in `VTE.Infrastructure`) is the abstraction; `TenantContext` (in `VTE.Api/Tenancy`) is the JWT-claim implementation.

## Endpoints (initial set)

- `POST /api/auth/login` — issue JWT
- `GET /api/stations` — list stations (Administrator only)
- `GET /api/stations/{id}` — get station
- `POST /api/stations` — create station
- `PUT /api/stations/{id}` — update station
- `GET /api/customers` — list customers (tenant-filtered)
- `GET /api/customers/{id}` — get customer
- `POST /api/customers` — create customer (validates BR-CUS-013)

More endpoints to follow per module.

## Configuration

`appsettings.json`:

| Section | Purpose |
|---|---|
| `ConnectionStrings:Default` | SQL connection (defaults to LocalDB / VTE2) |
| `Jwt:Secret` | JWT signing key — **change in production** |
| `Jwt:AccessTokenMinutes` | Token lifetime (default 60) |
| `Cors:AllowedOrigins` | Vue dev server origins (default 5173, 5174) |
| `Seed:DefaultAdmin*` | First-run admin seed |

## Testing

```bash
dotnet test
```

(One xUnit project scaffolded; tests not yet written.)
