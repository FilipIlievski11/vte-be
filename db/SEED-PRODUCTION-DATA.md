# Seeding the new laptop with production data

`db/schema.sql` + the API's auto-seeder give you an **empty-but-functional** VTE database. This doc covers the harder path: how to get your **real production data** (clients, vehicles, requests, bills, tech-exams) onto the new laptop.

The data isn't in this repo — it's too large and contains client PII. You have to transfer it out-of-band. Pick one of three paths.

---

## Path A — Fastest: restore your already-migrated `VTE.bak`

If you've been working on the main laptop and have a current `VTE` database, just back it up and copy it. **This skips re-running the migration scripts entirely.**

### On the main laptop (before leaving)

```cmd
:: 1) Open a cmd prompt. Make sure LocalDB is running.
sqllocaldb start MSSQLLocalDB

:: 2) Shrink the log first (recovers ~1.5 GB of empty log space)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"USE VTE; CHECKPOINT; DBCC SHRINKFILE (VTE_log, 100);"

:: 3) Back up to OneDrive so it syncs automatically
mkdir "%USERPROFILE%\OneDrive\vte-snapshots" 2>nul
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"BACKUP DATABASE VTE TO DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTE.bak' ^
 WITH FORMAT, INIT, NAME = N'VTE-full', STATS = 25;"
```

Wait for the OneDrive cloud-sync icon to go green (the `.bak` will be ~550–650 MB).

### On the vacation laptop

```cmd
:: 1) Make sure OneDrive has finished syncing
::    Check %USERPROFILE%\OneDrive\vte-snapshots\VTE.bak exists locally

:: 2) Confirm the logical file names (they're 'VTE' and 'VTE_log')
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"RESTORE FILELISTONLY FROM DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTE.bak';"

:: 3) Restore. Adjust paths so LocalDB writes the .mdf/.ldf somewhere stable.
::    %USERPROFILE% (your user folder) is fine and is where LocalDB likes user dbs.
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"RESTORE DATABASE VTE FROM DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTE.bak' ^
 WITH MOVE 'VTE'     TO N'%USERPROFILE%\VTE.mdf', ^
      MOVE 'VTE_log' TO N'%USERPROFILE%\VTE_log.ldf', ^
      REPLACE, STATS = 25;"
```

Done. Start the API; it'll see the schema already exists, skip migration, and just run idempotent seed checks.

> ⚠️ **The migrated `VTE` carries real client data.** Don't push the `.bak` to GitHub. Keep it in OneDrive (private to your account) or on a USB drive.

---

## Path B — Re-run the migration from a fresh legacy `VTEZVV` snapshot

Use this when you want to re-do the import (e.g. legacy data has changed, or you're testing migration-script fixes).

### On the main laptop (snapshot the legacy DB)

```cmd
:: 1) Make a point-in-time copy of VTEZVV into VTEZVV_Snapshot so the live legacy
::    DB stays untouched. (PowerShell helper does this — see migrate/snapshot-legacy.ps1.)
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"BACKUP DATABASE VTEZVV TO DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTEZVV.bak' ^
 WITH FORMAT, INIT, NAME = N'VTEZVV-source';"
```

If you also need user/permission data:

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"BACKUP DATABASE VTESecurity TO DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTESecurity.bak' ^
 WITH FORMAT, INIT, NAME = N'VTESecurity-source';"
```

### On the vacation laptop

```cmd
:: 1) Restore legacy databases under names the migrate scripts expect.
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"RESTORE DATABASE VTEZVV FROM DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTEZVV.bak' ^
 WITH MOVE 'VTEZVV'     TO N'%USERPROFILE%\VTEZVV.mdf', ^
      MOVE 'VTEZVV_log' TO N'%USERPROFILE%\VTEZVV_log.ldf', ^
      REPLACE;"

:: optional, only if migrating users
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"RESTORE DATABASE VTESecurity FROM DISK = N'%USERPROFILE%\OneDrive\vte-snapshots\VTESecurity.bak' ^
 WITH MOVE 'VTESecurity'     TO N'%USERPROFILE%\VTESecurity.mdf', ^
      MOVE 'VTESecurity_log' TO N'%USERPROFILE%\VTESecurity_log.ldf', ^
      REPLACE;"
```

:: 2) Create an empty `VTE` (let the API auto-create + auto-migrate by running `dotnet run` once).

:: 3) Run the migration scripts in order (see `migrate/README.md` "Quick start" section).
```

Walltime: ~150 s for the full pipeline against ~228k bills.

---

## Path C — No legacy access at all

You're on the vacation laptop, you forgot to bring any `.bak`, and the legacy DB isn't reachable over VPN. You're stuck with the auto-seeded empty schema (the `admin` user, one default Company, one default Station, request-type catalogs).

You can still:
- Develop UI features and test them against fabricated data.
- Add new clients/vehicles/requests through the UI itself.
- Write and validate new migration scripts (without running them).

You can't:
- Test against the 228k-row scale.
- Verify a parity check.
- Reproduce production-only bugs.

If you need real data later, ask the user to email/share the `.bak` from OneDrive.

---

## Verifying after a Path A restore

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -d VTE -Q ^
"SELECT 'Clients' AS T, COUNT(*) FROM Clients UNION ALL ^
 SELECT 'Vehicles', COUNT(*) FROM Vehicles UNION ALL ^
 SELECT 'Requests', COUNT(*) FROM Requests UNION ALL ^
 SELECT 'TechnicalExamReports', COUNT(*) FROM TechnicalExamReports UNION ALL ^
 SELECT 'CustomerDebts', COUNT(*) FROM CustomerDebts UNION ALL ^
 SELECT 'PaymentDocuments', COUNT(*) FROM PaymentDocuments;"
```

Expected order-of-magnitude (snapshot-dependent):

| Table                  | Rows  |
|------------------------|-------|
| Clients                | ~7k   |
| Vehicles               | ~6.9k |
| Requests               | ~18k  |
| TechnicalExamReports   | ~varies (low) |
| CustomerDebts          | varies — may be empty if you haven't triggered the auto-hooks |
| PaymentDocuments       | ~228k |

If the counts look right, you're good.

---

## Don't forget

1. **OneDrive sync.** Both laptops must be signed into the same Microsoft account, both must have OneDrive running, and you must wait for the `.bak` to be green (cloud + computer icons) before unplugging.
2. **`appsettings.Development.json`**. If you customized the connection string or seed defaults on the main laptop, that file is gitignored — recreate it on the vacation laptop.
3. **Rotate `admin`'s password**. Even if it's an offline dev box, get into the habit.
4. **Don't push `.bak` files to git.** They contain PII. `.bak` is already in `.gitignore`.

## Troubleshooting

**"The database 'VTE' already exists"** during RESTORE — drop it first:

```cmd
sqlcmd -S "(localdb)\MSSQLLocalDB" -E -Q ^
"IF DB_ID('VTE') IS NOT NULL ALTER DATABASE VTE SET SINGLE_USER WITH ROLLBACK IMMEDIATE; ^
 DROP DATABASE VTE;"
```

**"Cannot open backup device. Operating system error 5 (Access denied)"** — LocalDB runs as your user; make sure that user has read on the `.bak` path. OneDrive sometimes locks files mid-sync; wait for the cloud icon to settle.

**"The transaction log for database 'VTE' is full"** during migration — `sqlcmd ... -Q "ALTER DATABASE VTE SET RECOVERY SIMPLE;"` before the bulk load, then back to FULL afterwards if you care about point-in-time restore.
