# build-release.ps1 — produce a single deployable bundle (API + SPA in one).
#
#   .\deploy\build-release.ps1
#
# Output: deploy\out\release.zip — ready for Azure Web App (az webapp deploy),
# IIS, or any VPS. The zip contains the published .NET API with the Vue build
# in wwwroot/, so one app serves both the UI and /api on the same origin.

param(
    [string]$OutDir = "$PSScriptRoot\out",
    # FE кодот живее во посебно репо (Repos\VTE\FE) од septemvri 2026; кога
    # frontend-v2 не постои во ова репо, се бара во соседното FE репо.
    [string]$FrontendDir = ''
)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent

if (-not $FrontendDir) {
    $FrontendDir = if (Test-Path "$root\frontend-v2") { "$root\frontend-v2" }
                   else { Join-Path (Split-Path $root -Parent) 'FE\frontend-v2' }
}
if (-not (Test-Path "$FrontendDir\package.json")) { throw "Frontend не е најден: $FrontendDir" }

Write-Host "[1/4] Building frontend (vite): $FrontendDir" -ForegroundColor Cyan
Push-Location $FrontendDir
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
Copy-Item "$FrontendDir\dist\*" -Destination $wwwroot -Recurse -Force

# Version marker: "<git-hash>[-dirty] <build time>" -> served at /version.txt on prod.
# deploy-to-prod.ps1 reads it to warn when prod already runs the same version.
$gitHash = (git -C $root rev-parse --short HEAD)
if ($LASTEXITCODE -eq 0 -and $gitHash) {
    if (git -C $root status --porcelain) { $gitHash = "$gitHash-dirty" }
    $verLine = "{0} {1}" -f $gitHash, (Get-Date -Format 'yyyy-MM-dd HH:mm')
    [IO.File]::WriteAllText((Join-Path $wwwroot 'version.txt'), $verLine, [Text.UTF8Encoding]::new($false))
    Write-Host "      Version marker: $verLine" -ForegroundColor DarkGray
}

Write-Host "[4/4] Zipping..." -ForegroundColor Cyan
$zip = Join-Path $OutDir 'release.zip'
if (Test-Path $zip) { Remove-Item $zip -Force }
Compress-Archive -Path "$publish\*" -DestinationPath $zip

Write-Host "Done: $zip" -ForegroundColor Green
