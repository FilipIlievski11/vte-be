# Database setup — fresh machine

This folder gives you a working `VTE` database on a clean Windows + SQL Server LocalDB install, **without** needing the legacy `VTEZVV` database. You won't have the real production data, but you will have a fully functional app with seed data: one company, one station, one Administrator, and the request-catalog tree.

**If you want real production data** (clients, vehicles, requests, bills migrated from legacy), see [`SEED-PRODUCTION-DATA.md`](SEED-PRODUCTION-DATA.md) — the fastest path is restoring a `.bak` from OneDrive. The full re-migration path is in [`../migrate/README.md`](../migrate/README.md).

## Two ways to set up

### Option A — Recommended: let the API do everything

The API automatically applies EF migrations + seeds default data on first startup. So the shortest path is:

```bash
# from repo root
cd backend-v2/src/VTE.Api
dotnet run
```

Watch the console:
```
[Info] DataSeeder: Seeded role: Administrator
[Info] DataSeeder: Seeded default Company: Default Company (Id=1)
[Info] DataSeeder: Seeded default Station: Default Station (Id=1)
[Info] DataSeeder: Seeded default Administrator: admin
...
```

That's it. Database `VTE` is created, schema applied, seed data inserted.

### Option B — Apply the SQL by hand

Use this if EF tooling fails (no internet to restore NuGet, broken `dotnet` install, etc.):

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q "CREATE DATABASE VTE"
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -i db\schema.sql
```

Then start the API as in Option A — it'll detect the schema exists, skip migration, and just run the seeder.

## Default credentials

After seeding, log in to the frontend with:

| Field    | Value           |
|----------|-----------------|
| Username | `admin`         |
| Password | `ChangeMe!Now1` |

**Change this password immediately** via the Account Settings panel.

You can override the defaults before first run by adding these to `appsettings.Development.json` (or env vars):

```json
{
  "Seed": {
    "DefaultCompanyName": "...",
    "DefaultStationName": "...",
    "DefaultAdminUserName": "...",
    "DefaultAdminEmail": "...",
    "DefaultAdminPassword": "..."
  }
}
```

## Prerequisites

| Tool                       | Version            | Notes                                                         |
|----------------------------|--------------------|---------------------------------------------------------------|
| Windows 10/11              | —                  | LocalDB is Windows-only                                       |
| SQL Server LocalDB         | 2019 (15.0) or 2022 (16.0) or 17 | Comes with [SSMS](https://aka.ms/ssms) or [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads) |
| .NET SDK                   | 10.0               | `winget install Microsoft.DotNet.SDK.10`                      |
| Node.js                    | ≥ 22               | `winget install OpenJS.NodeJS.LTS`                            |
| `dotnet ef` global tool    | 10.x               | `dotnet tool install -g dotnet-ef`                            |

Verify LocalDB is up:

```cmd
sqllocaldb info
sqllocaldb start MSSQLLocalDB
```

## Connection string

The API expects `appsettings.json` (or `appsettings.Development.json`) to set:

```json
{
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\MSSQLLocalDB;Database=VTE;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  }
}
```

This is the default — no edits needed on a fresh machine.

## What the seeder creates

`backend-v2/src/VTE.Api/Seed/DataSeeder.cs`:

| Item                  | Default value                                                  |
|-----------------------|----------------------------------------------------------------|
| Role                  | `Administrator`                                                |
| Company               | `Default Company` (Id=1)                                       |
| Station               | `Default Station` (Id=1)                                       |
| Admin user            | `admin` / `admin@local` / `ChangeMe!Now1`                      |
| RequestDocumentPrints | `Plav` (1), `Bel` (2), `Zelen` (3) — the three legacy form colours |
| RequestTypes          | Starter tree (Plav root, Bel root, Zelen root, common leaves)  |
| ProofTypes, AttachmentTypes | Standard catalog                                         |

The seeder is idempotent — running it twice is a no-op.

## What's in this folder

| File          | Purpose                                                                  |
|---------------|--------------------------------------------------------------------------|
| `schema.sql`  | Idempotent CREATE TABLE / ALTER TABLE script for the entire VTE schema. Auto-generated via `dotnet ef migrations script --idempotent`, plus the manual `PriceCompanyId` column add. |
| `README.md`   | This file.                                                               |

## Regenerating `schema.sql`

If you change the EF model and want to refresh the script:

```bash
cd backend-v2
dotnet ef migrations script --no-build --idempotent \
  --project src/VTE.Infrastructure \
  --startup-project src/VTE.Api \
  --output ../db/schema.sql
```

Then re-append the manual drift section at the end of the file (the `PriceCompanyId` block — see the end of `schema.sql`).

## Schema drift notes

| Column / change                         | Why it's not in EF                                | Status                          |
|-----------------------------------------|---------------------------------------------------|---------------------------------|
| `PriceCatalog.PriceCompanyId` (tinyint, NULL) | Applied via raw SQL after migration was deployed | Captured at the bottom of `schema.sql` — fold into a real EF migration when convenient |

## Troubleshooting

**"LocalDB instance 'MSSQLLocalDB' could not be started"** — typically a stuck `sqlservr.exe`. Run:
```cmd
sqllocaldb stop MSSQLLocalDB -i
taskkill /F /IM sqlservr.exe
sqllocaldb start MSSQLLocalDB
```

**"Login failed for user"** — make sure the API process and `sqlcmd` are run as the same Windows user. LocalDB authenticates via Windows integrated security.

**Seeder never logs anything** — check `appsettings.json`'s connection string actually points at LocalDB, and the `Seed` keys aren't empty strings.
