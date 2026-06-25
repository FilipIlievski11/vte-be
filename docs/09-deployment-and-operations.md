# Deployment & operations

This is the operational reference for running VTE — both the **production** deployment (a single Hetzner VPS running Docker Compose) and the **local development** loop. It covers the build-and-release pipeline (`deploy/build-release.ps1`), how the three containers fit together, where secrets live (and why they're never committed), the SSH tunnel that lets the local machine talk to the production database, the EF auto-migration behaviour on startup, and the database re-lift procedure with its post-restore admin-password gotcha.

Everything here is grounded in the repo: the release script (`deploy/build-release.ps1`), the tunnel helper (`deploy/sql-tunnel.ps1`), the API host (`backend-v2/src/VTE.Api/Program.cs`), the config files (`backend-v2/src/VTE.Api/appsettings*.json`), the seeder (`backend-v2/src/VTE.Api/Seed/DataSeeder.cs`), and the runbook (`docs/DEPLOYMENT.md`). The server-side compose/Dockerfile/Caddyfile live only on the server at `/opt/vte` (they are **not** in the repo); their behaviour is documented in `docs/DEPLOYMENT.md` and `CLAUDE.md` and is summarised here.

See also: [Seeding production data](../db/SEED-PRODUCTION-DATA.md) (the `.bak` lift) and the existing one-page runbook [`DEPLOYMENT.md`](DEPLOYMENT.md) which this document expands on.

---

## 1. Production at a glance

| Item | Value | Source |
|------|-------|--------|
| URL | `https://116.202.8.155.sslip.io` | `CLAUDE.md`, `docs/DEPLOYMENT.md` |
| Host | Hetzner **CPX22** (€8.49/mo), Falkenstein DE, **Ubuntu 24.04** | `docs/DEPLOYMENT.md` |
| Live since | 2026-06-12 | `CLAUDE.md` |
| App root on server | `/opt/vte` | `docs/DEPLOYMENT.md` |
| Orchestration | Docker Compose — 3 services | `docs/DEPLOYMENT.md` |
| SSH | `ssh -i ~/.ssh/vte_deploy root@116.202.8.155` | `CLAUDE.md` |
| Firewall | `ufw` allows only SSH / 80 / 443; 4 GB swap on host | `docs/DEPLOYMENT.md` |
| TLS | Automatic Let's Encrypt via the `*.sslip.io` hostname (Caddy) | `docs/DEPLOYMENT.md` |

`sslip.io` is a wildcard DNS service: any hostname of the form `<ip>.sslip.io` resolves to that IP, so `116.202.8.155.sslip.io` points at the server with zero DNS setup — and because it's a real hostname (not a bare IP), Caddy can obtain a Let's Encrypt certificate for it automatically.

> Why not Azure? Azure rejected the account ("not eligible" — an opaque card/region gate), so production moved to Hetzner. The Azure path is kept in [`DEPLOYMENT.md`](DEPLOYMENT.md#alternative-path-azure-kept-for-reference--requires-a-subscription) for reference only and is **not** the live setup.

### 1.1 The three Compose services

Defined in `/opt/vte/docker-compose.yml` on the server (not in the repo). Per `docs/DEPLOYMENT.md`:

| Service | Container name | What it is |
|---------|----------------|------------|
| `mssql` | `vte-mssql-1` | **SQL Server 2025 Express** (free licence). Memory capped at **1.5 GB**, data in a **named volume**. Listens on the server's **loopback only** (`localhost:1433`) — never exposed to the internet. |
| `api` | `vte-api-1` | The **.NET 10** API container. Serves the REST API **and** the Vue SPA from `wwwroot` on the same origin (so no CORS in prod). This is what `build-release.ps1` produces. |
| `caddy` | `vte-caddy-1` | Reverse proxy. Terminates HTTPS (auto Let's Encrypt via the sslip.io hostname) and forwards to `api`. |

The default Compose project name (`vte`) yields the `vte-<service>-1` container names used in every server command below.

### 1.2 How a request flows in production

```
Browser ──HTTPS──▶ caddy (443, auto-TLS) ──HTTP──▶ api (.NET 10)
                                                     ├── /api/*  → ASP.NET Core controllers
                                                     └── /*      → Vue SPA from wwwroot (index.html fallback)
                                                          │
                                                          └──TDS──▶ mssql (localhost:1433, loopback)
```

The SPA-vs-API split is decided in `Program.cs`. After `MapControllers()`, the host checks for a built SPA and, if found, serves it with an `index.html` fallback for client-side routes; otherwise `/` redirects to Swagger (the dev case, where Vite serves the frontend):

```csharp
// backend-v2/src/VTE.Api/Program.cs
var webRoot = app.Environment.WebRootPath
    ?? Path.Combine(app.Environment.ContentRootPath, "wwwroot");
if (File.Exists(Path.Combine(webRoot, "index.html")))
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.MapFallbackToFile("index.html");   // /clients/42 → index.html
}
else
{
    app.MapGet("/", () => Results.Redirect("/swagger"));
}
```

So "the site shows the Swagger page instead of the app" always means **the SPA wasn't in the zip** — re-run the release build with `frontend-v2/dist` present (`docs/DEPLOYMENT.md` troubleshooting).

---

## 2. The release artifact: `build-release.ps1`

`deploy/build-release.ps1` produces **one** deployable bundle — the published .NET API with the Vue build folded into `wwwroot/`, so a single process serves both UI and `/api`. Output: `deploy/out/release.zip`.

The script has four steps (`deploy/build-release.ps1`):

1. **Build the frontend** — `npm run build` in `frontend-v2` (Vite). Throws on non-zero `$LASTEXITCODE`.
2. **Publish the API** — `dotnet publish backend-v2/src/VTE.Api/VTE.Api.csproj -c Release -o deploy/out/publish`. The `publish` folder is wiped first.
3. **Bundle the SPA** — copies `frontend-v2/dist/*` into `deploy/out/publish/wwwroot`.
4. **Zip** — `Compress-Archive` of `publish/*` → `deploy/out/release.zip`.

```powershell
# deploy/build-release.ps1 (abridged)
npm run build                                   # [1/4] vite
dotnet publish ...\VTE.Api.csproj -c Release -o $publish   # [2/4]
Copy-Item ...\frontend-v2\dist\* -Destination $wwwroot -Recurse -Force   # [3/4]
Compress-Archive -Path "$publish\*" -DestinationPath $zip  # [4/4]
```

`deploy/out/` is gitignored (`.gitignore`: `deploy/out/`), so the build output never lands in git.

### 2.1 The `2>&1` / stderr gotcha

> When running `build-release.ps1` from a non-PowerShell shell or tool that redirects a native command's stderr (`2>&1`), **`dotnet`/`npm` writing to stderr can be misread as a failure even on a successful build (exit 0).** In Windows PowerShell 5.1, redirecting a native exe's stderr wraps each line in a `NativeCommandError` and can flip `$?` to `$false`. The script itself guards correctly — it checks `$LASTEXITCODE` after each native call rather than relying on `$?` — but if you wrap the whole invocation in `2>&1`, treat a non-empty stderr stream as informational and trust the real exit code / the presence of `deploy/out/release.zip`, not the wrapped error lines. **Do not pipe this script's native commands through `2>&1`.**

### 2.2 Note on the build host vs. the runtime host

`build-release.ps1` runs `dotnet publish` **framework-dependent for the build machine's RID** by default — the `publish/runtimes/` folder visible in `deploy/out/` carries `win-x64` SNI natives, etc. That's fine: the `api` container image is built on the server (`docker compose up -d --build api`), and the container's base image supplies the Linux .NET 10 runtime. The zip is the *application payload*, not a self-contained Linux binary; the Dockerfile at `/opt/vte` is what turns it into a runnable Linux image.

---

## 3. Redeploying production

The canonical redeploy sequence (`docs/DEPLOYMENT.md`, `CLAUDE.md`):

```powershell
# 1) Build the bundle locally
.\deploy\build-release.ps1

# 2) Copy it to the server
scp -i ~/.ssh/vte_deploy deploy/out/release.zip root@116.202.8.155:/opt/vte/app/release.zip

# 3) Unzip into publish/ and rebuild + restart the api container
ssh -i ~/.ssh/vte_deploy root@116.202.8.155 `
  "cd /opt/vte/app && rm -rf publish && (unzip -q release.zip -d publish || true) && rm release.zip && cd /opt/vte && docker compose up -d --build api"
```

What each part does:

- `/opt/vte/app/publish/` is the directory the `api` image's Dockerfile copies from (the build context). Wiping and re-unzipping replaces the app payload.
- `(unzip ... || true)` tolerates the occasional unzip warning without aborting the chained command.
- `docker compose up -d --build api` rebuilds **only** the `api` image from the new `publish/` and restarts that one container. `mssql` and `caddy` are left running and untouched.
- **Schema changes apply automatically on the next boot** of the `api` container (see [§5 EF auto-migration](#5-ef-auto-migration-on-startup)). There is no separate migration step in the deploy.

After deploy, watch the container come up:

```bash
ssh -i ~/.ssh/vte_deploy root@116.202.8.155
docker logs -f vte-api-1
```

---

## 4. Secrets

**Secrets are never committed.** They live in exactly two places:

| Location | Scope | Contents |
|----------|-------|----------|
| `deploy/prod.secrets.local` (Filip's laptop) | local, **gitignored** | `SERVER_IP`, `SA_PASSWORD`, `JWT_SECRET`, `ADMIN_PASSWORD` |
| `/opt/vte/.env` (server) | server, never in git | the same values, consumed by `docker-compose.yml` as env vars |

`deploy/prod.secrets.local` is kept out of git by the `*.local` pattern in `.gitignore` (the same `.gitignore` also excludes `.env` / `.env.*` and `appsettings.Development.json`, which hold the other secrets — see [§4.1](#41-why-the-committed-jwtsecret-must-be-overridden)). The `prod.secrets.local` file is the laptop-side mirror of `/opt/vte/.env`; keep them in sync by hand when rotating a secret. Its keys are exactly `SERVER_IP`, `SA_PASSWORD`, `JWT_SECRET`, `ADMIN_PASSWORD` (`deploy/prod.secrets.local`). **Refer to the file for the actual values — never paste real secret values into code, logs, commits, or chat.**

The `.env` keys map onto config via the `__` (double-underscore) section separator that ASP.NET Core uses for hierarchical config, e.g.:

| `.env` / env var | Overrides config key | Read by |
|------------------|----------------------|---------|
| `ConnectionStrings__Default` | `ConnectionStrings:Default` | `Program.cs` → `AddDbContext` |
| `Jwt__Secret` | `Jwt:Secret` | `Program.cs` → `JwtOptions` |
| `Seed__DefaultAdminPassword` | `Seed:DefaultAdminPassword` | `DataSeeder.cs` (admin seed) |
| `LegacySync__Enabled` | `LegacySync:Enabled` | `LegacySyncController.cs` |
| `Storage__RequestAttachmentsRoot` | `Storage:RequestAttachmentsRoot` | attachments controller |
| `ASPNETCORE_ENVIRONMENT` | (host environment) | selects `appsettings.{Env}.json` |

### 4.1 Why the committed `Jwt:Secret` must be overridden

`backend-v2/src/VTE.Api/appsettings.json` ships a **placeholder** secret:

```json
"Jwt": { "Secret": "change-me-in-production-use-at-least-32-chars-long-secret-vte-v2", ... }
```

This value is public (it's in the repo). In production it **must** be replaced by `Jwt__Secret` from `/opt/vte/.env` — otherwise anyone who saw the repo could forge admin JWTs. The real prod secret is the `JWT_SECRET` line in `deploy/prod.secrets.local`.

### 4.2 The legacy-DB password caveat

`appsettings.json` also contains the **legacy SQL credentials** in the `LegacySync` section (host, user, password for the live legacy `VTEZVV` DB). These are committed. That's tolerated only because sync is **disabled in prod** (see [§7](#7-legacysync-disabled-in-prod)); the standing TODO is to rotate that legacy password and move the dev value into the gitignored `appsettings.Development.json` (`docs/DEPLOYMENT.md` Phase 6).

---

## 5. EF auto-migration on startup

The API **applies pending EF migrations automatically every time it boots** — including in production. This is why deploys carry no explicit migrate step. From `Program.cs`:

```csharp
// backend-v2/src/VTE.Api/Program.cs (abridged)
try
{
    for (var attempt = 1; ; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<VteDbContext>();
            await db.Database.MigrateAsync();   // applies any un-applied migrations
            break;
        }
        catch (Exception ex) when (attempt < 4)
        {
            // SQL Server may accept connections while its engine is still warming up,
            // failing the first DDL batch. Retry up to 4 times, 10s apart.
            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }
    await DataSeeder.SeedAsync(app.Services);   // idempotent: roles, default company/station, admin, catalogs
}
catch (Exception ex)
{
    // A transient DB outage must not stop the host from booting; DB routes surface
    // the real error on first call, non-DB routes (Swagger, /) stay healthy.
}
```

Key properties:

- **4-attempt retry, 10 s apart** — tolerates the container race where `mssql` accepts TCP connections before its engine can run DDL.
- **Whole block is `try/catch`** — a DB outage never prevents the host from starting; it just logs and serves non-DB routes until the DB is reachable.
- **The DB command timeout is raised to 120 s** (`Program.cs` `sql.CommandTimeout(120)`) because the first vehicles search across ~66k relations can exceed the 30 s default until plans warm up.
- **Seeding is idempotent** (`DataSeeder.cs`): it creates roles, a default `Company` + `Station`, the `admin` user, and request catalogs **only if missing**. Crucially, the request-catalog seed **short-circuits if any `RequestType` already exists**, so it never re-adds starter rows on a database that already has the migrated legacy catalog.

Migrations live in EF's default folder `backend-v2/src/VTE.Infrastructure/Migrations/`. As of this writing, the chain is:

```
20260612231002_InitialSchema            ← the squashed single schema (see CLAUDE.md gotcha #6)
20260612234823_AddCustomerDebtLegacyId
20260625115717_AddDocumentIssuerCommunityId
```

> **Do not restore the pre-2026-06-12 migration chain** from git history (`CLAUDE.md` gotcha #6): migrations were squashed into a single `InitialSchema` because the old chain could never build a fresh DB. EF now owns the full schema.

To add a migration that will then auto-apply on the next deploy (from `README.md`):

```bash
cd backend-v2
dotnet ef migrations add MyChange --project src/VTE.Infrastructure --startup-project src/VTE.Api
```

---

## 6. The production-DB SSH tunnel (local → prod)

The `mssql` container listens only on the **server's loopback** (`localhost:1433`) and is not exposed by `ufw`. The **only** way to reach it from outside is an SSH tunnel. `deploy/sql-tunnel.ps1` opens one:

```powershell
# deploy/sql-tunnel.ps1
ssh -i "$env:USERPROFILE\.ssh\vte_deploy" -N -L 127.0.0.1:14333:localhost:1433 root@116.202.8.155
```

While that window stays open, anything on the laptop can reach the prod DB at **`127.0.0.1,14333`**:

| Setting | Value |
|---------|-------|
| Server | `127.0.0.1,14333` |
| Auth | SQL Server Authentication |
| Login | `sa` |
| Password | `SA_PASSWORD` from `deploy/prod.secrets.local` |
| Extra | tick **Trust server certificate** (SSMS Connection Properties) |

Close the window (Ctrl+C) when done. The local-port `14333` → server-side `localhost:1433` mapping is what every local-against-prod workflow uses.

### 6.1 The local API points at the tunnel

`backend-v2/src/VTE.Api/appsettings.Development.json` (gitignored) overrides the connection string to use the tunnel — so running the API locally in Development talks to the **production** database:

```json
// backend-v2/src/VTE.Api/appsettings.Development.json
"ConnectionStrings": {
  "Default": "Server=127.0.0.1,14333;Database=VTE;User Id=sa;Password=<SA_PASSWORD>;TrustServerCertificate=True;Encrypt=False;MultipleActiveResultSets=true"
}
```

> This is a deliberate convenience but a sharp edge: with the tunnel open and the API in Development, your local backend is reading/writing **live production data**. The base `appsettings.json` (committed) instead points at LocalDB (`Server=(localdb)\MSSQLLocalDB;Database=VTE;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`). Because `appsettings.Development.json` is gitignored, a fresh clone gets the safe LocalDB default until someone recreates the dev override.

---

## 7. LegacySync disabled in prod

The "Sync from legacy" feature pulls new rows from the live legacy `VTEZVV` DB through a linked server. It is **disabled in production** by `LegacySync__Enabled=false` in `/opt/vte/.env` (`CLAUDE.md`, `docs/DEPLOYMENT.md`). The controller enforces this server-side — `LegacySyncController.Sync` refuses with HTTP 400 when the flag is off:

```csharp
// backend-v2/src/VTE.Api/Controllers/LegacySyncController.cs
if (!_cfg.GetValue("LegacySync:Enabled", false))
    return Problem("Legacy sync is disabled (LegacySync:Enabled=false).", statusCode: 400);
```

Rationale: the cloud app must never reach the legacy SQL server (whose credentials sit in `appsettings.json`). Sync is a **local-only** operation. The intended data-refresh flow is therefore **two-stage**: sync legacy→local on the laptop, then lift local→prod via `.bak` (see [§8](#8-database-re-lift-bak-restore)). In `appsettings.json` the dev default is `LegacySync.Enabled = true`; only prod's `.env` turns it off.

---

## 8. Database re-lift (.bak restore)

Production carries the **real** migrated data (lifted 2026-06-13: ~32k clients / 66k vehicles / 128k requests / 100k tech-exams, per `CLAUDE.md`). To refresh prod with newer data, you restore a `.bak` taken from the laptop's `VTE` database into the `mssql` container. The historical `PaymentDocuments` bulk was a one-time snapshot — the re-lift covers the **operational register**, and new v2 debts/bills are generated by the app itself (`docs/DEPLOYMENT.md`).

### 8.1 The procedure (three stages)

From `docs/DEPLOYMENT.md` and `CLAUDE.md`:

**Stage 1 — sync legacy → local** (laptop, local API running, NOT prod):
Press **"Синхронизирај"** on the Legacy Sync screen, or `POST /api/admin/legacy-sync` with an admin token. This top-ups new clients/vehicles/registrations/relations/requests/proofs/tech-exams from the live legacy DB via the `VTEZVV_LIVE` linked server.

**Stage 2 — lift local → prod** (.bak round-trip):

```bash
# On the laptop: backup local VTE, gzip, scp to the server
#   (see db/SEED-PRODUCTION-DATA.md for the exact BACKUP DATABASE command;
#    shrink the log first to recover ~1.5 GB of empty log space)

# On the server: get the .bak into the mssql container and restore it.
# IMPORTANT: stop the api container first so it isn't holding connections to VTE.
docker stop vte-api-1
docker cp VTE.bak vte-mssql-1:/var/opt/mssql/VTE.bak
docker exec -it vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<SA pw from .env>" -C \
  -Q "RESTORE DATABASE VTE FROM DISK=N'/var/opt/mssql/VTE.bak' WITH REPLACE, MOVE ...;"
docker start vte-api-1
```

The restore uses `WITH REPLACE` (overwrite the existing prod `VTE`) and `MOVE` to place the data/log files at the container's SQL paths. **Stop `vte-api-1` before restoring** — an open connection to `VTE` will block the restore.

**Stage 3 — re-secure the admin password** (the gotcha below).

For the laptop-side `.bak` mechanics (backup, log-shrink, OneDrive transfer, `RESTORE FILELISTONLY` to confirm logical names `VTE` / `VTE_log`, verification counts), see [`db/SEED-PRODUCTION-DATA.md`](../db/SEED-PRODUCTION-DATA.md). That doc is laptop↔laptop, but the BACKUP/RESTORE commands and logical-file-name handling are identical.

### 8.2 The admin-password-after-relift gotcha

> **A `.bak` restore overwrites the prod `AspNetUsers` table — including the admin password hash — with the LOCAL value.** After a re-lift, the prod `admin` account has the **local** (dev) password, not the strong production one. You must re-secure it:
>
> 1. Log in to prod once with the **local** admin password (the one your laptop DB had).
> 2. Re-set it to the strong prod password (`ADMIN_PASSWORD` in `deploy/prod.secrets.local`) via `POST /api/users/{adminId}/reset-password` (`UsersController.ResetPassword`, body `{ "newPassword": "..." }`).

Related operational hazards with the prod admin login (`CLAUDE.md` gotchas #15):

- **Never pass the prod admin password through a bash double-quoted string** — it can contain `!`-sequences that bash history-expands into the wrong password. ~5 wrong tries trips ASP.NET **lockout** ("Account is locked."). Log in via a script that reads `deploy/prod.secrets.local` directly.
- **Clear a lockout** with: `UPDATE AspNetUsers SET LockoutEnd=NULL, AccessFailedCount=0`.
- **ASP.NET Identity password hashes are portable** — you can copy a known-good hash between the local and prod `AspNetUsers` to reset a password by SQL. (`sqlcmd` needs `-I` for `QUOTED_IDENTIFIER` because of that table's filtered index.)
- The admin seed only runs when the user is **missing** (`DataSeeder.cs`), so it will **not** silently re-set the password on an existing admin after a relift — you must do step 2 yourself.

---

## 9. Useful server commands

From `docs/DEPLOYMENT.md`:

```bash
ssh -i ~/.ssh/vte_deploy root@116.202.8.155

docker logs -f vte-api-1                       # tail app logs (incl. migration/seed output)
docker compose -f /opt/vte/docker-compose.yml ps   # service status
docker compose up -d --build api               # rebuild + restart only the API
docker restart vte-caddy-1                      # restart the proxy (e.g. after Caddyfile edit)

# SQL shell inside the mssql container (note: mssql-tools18 + -C to trust the cert):
docker exec -it vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "<SA pw from .env>" -C
```

---

## 10. Local development run recipe

Two everyday modes. Choose by which database you want to hit.

### 10.1 Against LocalDB (the safe default)

Base `appsettings.json` points at `(localdb)\MSSQLLocalDB`, DB `VTE`. The API auto-creates/migrates the DB and seeds the admin on first run (`README.md`).

```bash
# Backend (auto-migrates + seeds on first start)
cd backend-v2/src/VTE.Api
dotnet run            # https://localhost:7165 by default

# Frontend (new terminal)
cd frontend-v2
npm install           # first time only
npm run dev           # http://localhost:5173
```

Default login: `admin` / `ChangeMe!Now1` (seeded by `DataSeeder.cs`; `Seed:DefaultAdminPassword` in `appsettings.json`). The committed CORS allow-list (`appsettings.json` → `Cors:AllowedOrigins`) is `http://localhost:5173` and `http://localhost:5174`.

### 10.2 Against the production DB (tunnel)

This is the "tunnel + API + FE" recipe implied by `appsettings.Development.json` and `sql-tunnel.ps1`. Three terminals:

```powershell
# Terminal 1 — open the SSH tunnel (127.0.0.1:14333 → server localhost:1433)
.\deploy\sql-tunnel.ps1

# Terminal 2 — API in Development, which reads appsettings.Development.json → tunnel DB.
#   Bind to port 5300 (the local-against-prod port) so it doesn't collide with the
#   default https://localhost:7165 instance.
cd backend-v2/src/VTE.Api
dotnet run --urls http://localhost:5300

# Terminal 3 — frontend on 5174 (the second allowed CORS origin)
cd frontend-v2
npm run dev -- --port 5174
```

Notes:
- `dotnet run` defaults to the `Development` environment, so `appsettings.Development.json` (the tunnel connection string) takes effect automatically.
- Port **5300** keeps the prod-DB-backed API distinct from a LocalDB instance; **5174** is the second CORS origin already allowed in `appsettings.json`. Both are conventions, not hardcoded — adjust the FE's API base URL accordingly if you change them.
- **You are now writing to live production data.** SSMS can use the same tunnel (`127.0.0.1,14333`, `sa`) per [§6](#6-the-production-db-ssh-tunnel-local--prod).

### 10.3 Build-lock gotcha during dev

`dotnet build` fails with **MSB3027** while the API is running (Visual Studio / IIS Express holds the exe). For EF commands during dev, add `--no-build` (`CLAUDE.md` gotcha #1).

---

## 11. Operational TODOs / known gaps

From `docs/DEPLOYMENT.md` ("Still to do") and `CLAUDE.md`:

- [ ] **Real domain**: buy a `.mk` domain, point it at `116.202.8.155`, change one line in `/opt/vte/Caddyfile`, then `docker compose restart caddy`.
- [ ] **Backups**: enable Hetzner's server-backup option (~€1.40/mo) or set up nightly `.bak` dumps to a Hetzner Storage Box. There is currently **no automated DB backup** in prod.
- [ ] **Tighten Identity password policy** before onboarding real operators — it is relaxed for dev in `Program.cs` (6 chars, no digit/upper/lower/non-alphanumeric requirement).
- [ ] **Rotate the legacy-DB password** committed in `appsettings.json`'s `LegacySync` section, and move the dev value into gitignored `appsettings.Development.json`.

---

## 12. Quick reference

| Task | Command |
|------|---------|
| Build release bundle | `.\deploy\build-release.ps1` → `deploy/out/release.zip` |
| Deploy to prod | `scp ... release.zip ...:/opt/vte/app/` then `... docker compose up -d --build api` (see [§3](#3-redeploying-production)) |
| SSH to prod | `ssh -i ~/.ssh/vte_deploy root@116.202.8.155` |
| Tail API logs | `docker logs -f vte-api-1` |
| Open DB tunnel | `.\deploy\sql-tunnel.ps1` → SSMS at `127.0.0.1,14333` (`sa`) |
| Prod SQL shell | `docker exec -it vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<SA pw>" -C` |
| Clear admin lockout | `UPDATE AspNetUsers SET LockoutEnd=NULL, AccessFailedCount=0` |
| Reset admin pw (post-relift) | `POST /api/users/{adminId}/reset-password` (after logging in with the local pw) |
| Local run (LocalDB) | `dotnet run` (API) + `npm run dev` (FE on 5173) |
| Local run (prod DB) | tunnel + `dotnet run --urls http://localhost:5300` + `npm run dev -- --port 5174` |

---

_Sources: `deploy/build-release.ps1`, `deploy/sql-tunnel.ps1`, `deploy/prod.secrets.local` (values not reproduced), `backend-v2/src/VTE.Api/Program.cs`, `backend-v2/src/VTE.Api/appsettings.json`, `backend-v2/src/VTE.Api/appsettings.Development.json`, `backend-v2/src/VTE.Api/Seed/DataSeeder.cs`, `backend-v2/src/VTE.Api/Controllers/LegacySyncController.cs`, `backend-v2/src/VTE.Api/Controllers/UsersController.cs`, `backend-v2/src/VTE.Infrastructure/Migrations/`, `docs/DEPLOYMENT.md`, `db/SEED-PRODUCTION-DATA.md`, `README.md`, `CLAUDE.md`, `.gitignore`._
