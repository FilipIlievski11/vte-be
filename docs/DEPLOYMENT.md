# Deployment runbook — VTE v2

## ✅ CURRENT PRODUCTION (deployed 2026-06-12): Hetzner + Docker

> Azure rejected the account ("not eligible" — opaque card/region gate), so production runs on a **Hetzner CPX22** (€8.49/mo, Falkenstein DE, Ubuntu 24.04) at:
>
> **https://116.202.8.155.sslip.io**
>
> Server stack at `/opt/vte` (Docker Compose): `mssql` = SQL Server 2025 Express (free licence, 1.5 GB memory cap, named volume) · `api` = .NET 10 container serving API + Vue SPA from wwwroot · `caddy` = reverse proxy with automatic Let's Encrypt HTTPS via the sslip.io hostname. 4 GB swap on the host, ufw allows only SSH/80/443. Secrets live in `/opt/vte/.env` (server) and `deploy/prod.secrets.local` (laptop, gitignored).

### Redeploy (every future release)

```powershell
.\deploy\build-release.ps1
scp -i ~/.ssh/vte_deploy deploy/out/release.zip root@116.202.8.155:/opt/vte/app/release.zip
ssh -i ~/.ssh/vte_deploy root@116.202.8.155 "cd /opt/vte/app && rm -rf publish && (unzip -q release.zip -d publish || true) && rm release.zip && cd /opt/vte && docker compose up -d --build api"
```

Schema changes apply automatically on boot (EF migrations with 4-attempt retry).

### Useful server commands

```bash
ssh -i ~/.ssh/vte_deploy root@116.202.8.155
docker logs -f vte-api-1                  # app logs
docker compose -f /opt/vte/docker-compose.yml ps
# SQL shell:
docker exec -it vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "<SA pw from .env>" -C
```

### Still to do on this deployment

- [ ] Real-data lift: copy a `.bak` of the local VTE up and `RESTORE DATABASE` inside the mssql container (works — both are SQL 2025 generation), or re-run `migrate/` scripts against a restored legacy snapshot.
- [ ] Buy a real domain (e.g. `.mk`) and point it at 116.202.8.155 — then change one line in `/opt/vte/Caddyfile` and `docker compose restart caddy`.
- [ ] Toggle Hetzner's server backup option (€1.40/mo) in the console, or set up nightly `.bak` dumps to Hetzner Storage Box.
- [ ] Tighten Identity password policy (`Program.cs`) before onboarding real operators.

---

## Alternative path: Azure (kept for reference — requires a subscription)

Goal: the app live at `https://<yourname>.azurewebsites.net` with no server administration. One Azure **Web App** serves both the API and the Vue frontend (bundled into `wwwroot` by `deploy/build-release.ps1`); one **Azure SQL** database holds the data.

---

## Phase 0 — already in the repo (done)

- `Program.cs` serves the SPA from `wwwroot` when present (same origin → no CORS needed in prod).
- EF catch-up migration `AddPriceCatalogPriceCompanyId` — a fresh database now migrates correctly on first boot.
- `deploy/build-release.ps1` — builds frontend + API into `deploy/out/release.zip`.

## Phase 1 — accounts & tools (~20 min, one-time)

1. **Azure account**: https://azure.microsoft.com/free — sign in with your Microsoft account, add a credit card (required even for free tier). New accounts get ~$200 of 30-day credit.
2. **Azure CLI**: `winget install Microsoft.AzureCLI`, then restart the terminal and run `az login` (opens browser).
3. (For the data lift, Phase 5) **SqlPackage**: `dotnet tool install -g microsoft.sqlpackage`

## Phase 2 — create the cloud resources (~10 min)

Pick a globally unique app name (e.g. `vte-velesapp`) and a strong SQL admin password, then run:

```powershell
$APP  = "vte-velesapp"          # must be globally unique → becomes <APP>.azurewebsites.net
$RG   = "vte-rg"
$LOC  = "westeurope"            # Amsterdam — ~25 ms from Macedonia
$SQLPW = "<STRONG-PASSWORD-HERE>"   # SQL admin password — save it!

az group create -n $RG -l $LOC

# App Service: B1 Basic (~€12/mo), Linux, .NET 10
az appservice plan create -n vte-plan -g $RG --sku B1 --is-linux
az webapp create -n $APP -g $RG --plan vte-plan --runtime "DOTNETCORE:10.0"

# Azure SQL: logical server + serverless DB on the FREE offer
# (100k vCore-seconds + 32 GB free each month — plenty for one station)
az sql server create -n "$APP-sql" -g $RG -l $LOC -u vteadmin -p $SQLPW
az sql db create -n vte -g $RG -s "$APP-sql" `
  --edition GeneralPurpose --compute-model Serverless -f Gen5 -c 1 `
  --use-free-limit --free-limit-exhaustion-behavior AutoPause
# Let Azure services (your Web App) reach the SQL server:
az sql server firewall-rule create -g $RG -s "$APP-sql" -n AllowAzure `
  --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
```

## Phase 3 — configure the app (~10 min)

Settings go into the Web App as environment variables — **never** into the committed `appsettings.json`. Note the double underscore `__` = section separator.

```powershell
# Generate a fresh JWT secret (64 random chars):
$JWT = -join ((48..57)+(65..90)+(97..122) | Get-Random -Count 64 | ForEach-Object {[char]$_})

az webapp config appsettings set -n $APP -g $RG --settings `
  ASPNETCORE_ENVIRONMENT="Production" `
  "ConnectionStrings__Default=Server=tcp:$APP-sql.database.windows.net,1433;Initial Catalog=vte;User ID=vteadmin;Password=$SQLPW;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" `
  "Jwt__Secret=$JWT" `
  "Seed__DefaultAdminPassword=<PICK-AN-ADMIN-PASSWORD>" `
  "LegacySync__Enabled=false" `
  "Storage__RequestAttachmentsRoot=/home/storage/request-attachments"
```

Why each one matters:

| Setting | Why |
|---|---|
| `ConnectionStrings__Default` | Points at Azure SQL instead of LocalDB. |
| `Jwt__Secret` | The committed value is a public placeholder — anyone who saw the repo could forge admin tokens. Must be unique in prod. |
| `Seed__DefaultAdminPassword` | So the seeded admin doesn't have the well-known default. |
| `LegacySync__Enabled=false` | **Critical.** Legacy sync connects to the live legacy DB with credentials in appsettings. The cloud app must not reach it. |
| `Storage__RequestAttachmentsRoot=/home/...` | `/home` is the only persistent disk on App Service Linux — attachments survive restarts/redeploys there. |

## Phase 4 — build & deploy (~10 min)

```powershell
cd <repo>\trunk
.\deploy\build-release.ps1
az webapp deploy -n $APP -g $RG --src-path .\deploy\out\release.zip --type zip
```

First boot: the API connects to the empty Azure SQL DB, runs **all EF migrations**, and seeds the Administrator + default company/station + request catalogs (watch with `az webapp log tail -n $APP -g $RG`).

Open `https://<APP>.azurewebsites.net` → login screen → `admin` / the password you set in Phase 3. HTTPS is automatic.

## Phase 5 — real data (optional; +30–60 min)

The app now runs with an empty, seeded database. To lift your local data into the cloud:

```powershell
# 1. Export local DB to a .bacpac (LocalDB must be running)
sqlpackage /Action:Export `
  /SourceConnectionString:"Server=(localdb)\MSSQLLocalDB;Database=VTE;Trusted_Connection=True;TrustServerCertificate=True" `
  /TargetFile:"$env:USERPROFILE\VTE.bacpac"

# 2. Import it into Azure as a NEW database (cannot overwrite an existing one)
#    Temporarily allow your home IP on the SQL firewall:
az sql server firewall-rule create -g $RG -s "$APP-sql" -n MyHomeIp `
  --start-ip-address <YOUR-IP> --end-ip-address <YOUR-IP>     # whatismyip.com

sqlpackage /Action:Import `
  /TargetConnectionString:"Server=tcp:$APP-sql.database.windows.net,1433;Initial Catalog=vte2;User ID=vteadmin;Password=$SQLPW;Encrypt=True" `
  /SourceFile:"$env:USERPROFILE\VTE.bacpac"
# (~650 MB of data — expect 20–60 min)

# 3. Point the app at the imported DB and restart
az webapp config appsettings set -n $APP -g $RG --settings `
  "ConnectionStrings__Default=...same as before but Initial Catalog=vte2;..."
az webapp restart -n $APP -g $RG

# 4. Remove the home-IP firewall rule and (optionally) delete the empty 'vte' DB
az sql server firewall-rule delete -g $RG -s "$APP-sql" -n MyHomeIp
```

Migrations are already recorded inside the bacpac (`__EFMigrationsHistory` travels with it), so the app boots without re-running them.

> **PII note:** this data contains real client names/EMBG/plates. Only import it after Phase 3's secrets are set, and keep the repo private.

## Phase 6 — before you give the URL to anyone

- [ ] Log in and **change the admin password** via Account Settings (even though it's no longer the default).
- [ ] Check `az webapp log tail` for errors after a few clicks around the app.
- [ ] The legacy-DB password sits in the committed `appsettings.json` (`LegacySync` section). Sync is disabled in prod, but **rotate that password** on the legacy SQL server when you get a chance, and move the dev value into `appsettings.Development.json` (gitignored).
- [ ] Identity password rules are relaxed (6 chars, no complexity — `Program.cs:31`). Fine for tonight; tighten before real operators get accounts.
- [ ] Azure SQL gives automatic point-in-time restore (7 days) out of the box — no backup setup needed for now.

## Costs (monthly, after the $200 trial credit)

| Item | EUR/mo |
|---|---|
| App Service B1 (Linux) | ~12 |
| Azure SQL serverless (free offer) | 0 (auto-pauses when quota used) |
| Bandwidth at this scale | ~0 |
| **Total** | **~€12–15** |

Scale-up later: B1 → P0v3 (~€60) when you onboard more stations; SQL free → paid serverless (~€5–15) when you outgrow the free quota.

## Every future deploy

```powershell
.\deploy\build-release.ps1
az webapp deploy -n $APP -g $RG --src-path .\deploy\out\release.zip --type zip
```

That's it — schema changes apply automatically on boot via EF migrations.

## Troubleshooting

- **Site shows the Swagger page instead of the app** → the zip was built without the frontend; re-run `build-release.ps1` and check `frontend-v2/dist` exists.
- **500 on every /api call** → connection string wrong; `az webapp log tail` shows the real SQL error.
- **"Login failed for user 'vteadmin'"** → password typo in the connection string, or firewall rule missing.
- **App is slow on first request after idle** → B1 has Always-On available: `az webapp config set -n $APP -g $RG --always-on true`.
