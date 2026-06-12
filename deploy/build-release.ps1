# build-release.ps1 — produce a single deployable bundle (API + SPA in one).
#
#   .\deploy\build-release.ps1
#
# Output: deploy\out\release.zip — ready for Azure Web App (az webapp deploy),
# IIS, or any VPS. The zip contains the published .NET API with the Vue build
# in wwwroot/, so one app serves both the UI and /api on the same origin.

param([string]$OutDir = "$PSScriptRoot\out")

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

Write-Host "[1/4] Building frontend (vite)..." -ForegroundColor Cyan
Push-Location "$root\frontend-v2"
try { npm run build; if ($LASTEXITCODE -ne 0) { throw "vite build failed" } }
finally { Pop-Location }

Write-Host "[2/4] Publishing API (Release)..." -ForegroundColor Cyan
$publish = Join-Path $OutDir 'publish'
if (Test-Path $publish) { Remove-Item $publish -Recurse -Force }
dotnet publish "$root\backend-v2\src\VTE.Api\VTE.Api.csproj" -c Release -o $publish
if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed" }

Write-Host "[3/4] Bundling SPA into wwwroot..." -ForegroundColor Cyan
$wwwroot = Join-Path $publish 'wwwroot'
New-Item -ItemType Directory -Force $wwwroot | Out-Null
Copy-Item "$root\frontend-v2\dist\*" -Destination $wwwroot -Recurse -Force

Write-Host "[4/4] Zipping..." -ForegroundColor Cyan
$zip = Join-Path $OutDir 'release.zip'
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path "$publish\*" -DestinationPath $zip

Write-Host "Done: $zip" -ForegroundColor Green
