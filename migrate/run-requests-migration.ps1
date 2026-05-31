# Runs the 4 Request-module migration scripts in order, against LocalDB.
# Stops on the first failure. Prints PRINT output line-by-line so you can
# follow row counts as they happen.
#
# Usage (from your own PowerShell, NOT the sandbox):
#   cd C:\Users\filip\OneDrive\Documents\Repos\trunk\trunk
#   .\migrate\run-requests-migration.ps1
#
# Skip indexes (if you want to run only the data migration first, then judge
# whether indexes are needed):
#   .\migrate\run-requests-migration.ps1 -SkipIndexes
#
# Override server / DB if your LocalDB instance isn't the default:
#   .\migrate\run-requests-migration.ps1 -Server "(localdb)\Other" -Database "VTE"

param(
  [string]$Server = "(localdb)\MSSQLLocalDB",
  [string]$Database = "VTE",
  [switch]$SkipIndexes
)

$ErrorActionPreference = 'Stop'

$scripts = @(
  @{ Name = 'diag';     Path = "$PSScriptRoot\migrate-requests-diag.sql";     Desc = 'Pre-flight diagnostic (read-only)' },
  @{ Name = 'catalogs'; Path = "$PSScriptRoot\migrate-request-catalogs.sql";  Desc = 'Migrate 5 catalog tables (DocumentTypePrint, RequestTypes, Ownership/Payment proof types, AttachmentTypes)' },
  @{ Name = 'requests'; Path = "$PSScriptRoot\migrate-requests.sql";          Desc = 'Migrate Requests + child OwnershipProofs + PaymentProofs (~150k rows)' }
)

if (-not $SkipIndexes) {
  $scripts += @{ Name = 'indexes'; Path = "$PSScriptRoot\perf-indexes-requests.sql"; Desc = 'Add filtered + covering indexes' }
}

Write-Host ""
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "  VTE Requests migration runner" -ForegroundColor Cyan
Write-Host "  Server   : $Server" -ForegroundColor Cyan
Write-Host "  Database : $Database" -ForegroundColor Cyan
Write-Host "  Scripts  : $($scripts.Count)" -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host ""

$startTotal = Get-Date

foreach ($s in $scripts) {
  Write-Host "----[ $($s.Name) ]----------------------------------------------" -ForegroundColor Yellow
  Write-Host "  $($s.Desc)" -ForegroundColor Yellow
  Write-Host "  $($s.Path)" -ForegroundColor DarkGray
  Write-Host ""

  if (-not (Test-Path $s.Path)) {
    Write-Host "  !! Missing: $($s.Path)" -ForegroundColor Red
    exit 1
  }

  $start = Get-Date
  # -b: terminate on error; -X: disable interactive features; -I: enable QUOTED_IDENTIFIER;
  # -f 65001: UTF-8 for Cyrillic literals
  & sqlcmd -S $Server -d $Database -i $s.Path -b -X -I -f 65001
  $exit = $LASTEXITCODE
  $elapsed = (Get-Date) - $start

  Write-Host ""
  if ($exit -ne 0) {
    Write-Host "  !! $($s.Name) FAILED with exit $exit  (after $([int]$elapsed.TotalSeconds)s)" -ForegroundColor Red
    Write-Host "     Aborting. Earlier scripts already committed; only this script rolled back." -ForegroundColor Red
    exit $exit
  }

  Write-Host "  OK ($([int]$elapsed.TotalSeconds)s)" -ForegroundColor Green
  Write-Host ""
}

$total = (Get-Date) - $startTotal
Write-Host "================================================================" -ForegroundColor Cyan
Write-Host "  All scripts done in $([int]$total.TotalSeconds)s total." -ForegroundColor Cyan
Write-Host "  Next: start the backend (dotnet run --project backend-v2\src\VTE.Api)" -ForegroundColor Cyan
Write-Host "  then visit http://localhost:5173/requests to verify." -ForegroundColor Cyan
Write-Host "================================================================" -ForegroundColor Cyan
