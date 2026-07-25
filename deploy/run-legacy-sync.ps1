# run-legacy-sync.ps1 — sync на прод базата со живата легаси база, со еден клик.
#
#   .\deploy\run-legacy-sync.ps1
#
# Што прави, по ред:
#   1. Проверува/крева SSH тунел до прод SQL (порта 14333).
#   2. Проверува/крева локален dev API (порта 5300) — тој е врската кон прод базата.
#   3. Се најавува како admin (лозинка од deploy\prod.secrets.local — никогаш не се印 печати).
#   4. Повикува POST /api/admin/legacy-sync и го чека резултатот (~2 минути).
#   5. Ги печати бројките: колку нови барања/прегледи/возила/клиенти... дошле.
#
# Скриптата е безбедна за повторување — sync-от е идемпотентен (само дополнува
# што недостига + ги освежува статусите на сметките).

$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
$secretsPath = Join-Path $PSScriptRoot 'prod.secrets.local'
$apiDir = Join-Path $repo 'backend-v2\src\VTE.Api'

function Test-Port14333 {
    return (Test-NetConnection -ComputerName 127.0.0.1 -Port 14333 -InformationLevel Quiet -WarningAction SilentlyContinue)
}
function Test-Api {
    try {
        Invoke-WebRequest -Uri 'http://localhost:5300/api/citizenships' -UseBasicParsing -TimeoutSec 3 | Out-Null
        return $true
    } catch {
        if ($null -ne $_.Exception.Response) { return $true }   # 401 = API-то е живо, бара најава
        return $false
    }
}

# --- 0. Лозинка (никогаш не се печати) ---
if (-not (Test-Path $secretsPath)) { throw "Не постои $secretsPath — sync може само од машина со тајните." }
$adminPw = ((Get-Content $secretsPath | Where-Object { $_ -like 'ADMIN_PASSWORD=*' }) -replace '^ADMIN_PASSWORD=','').Trim()
if (-not $adminPw) { throw 'ADMIN_PASSWORD недостига во prod.secrets.local.' }

# --- 1. Тунел ---
if (Test-Port14333) {
    Write-Host '[1/4] Тунелот до прод SQL е веќе крентат.' -ForegroundColor Green
} else {
    Write-Host '[1/4] Кревам SSH тунел до прод SQL…'
    Start-Process ssh -ArgumentList '-i', "$env:USERPROFILE\.ssh\vte_deploy", '-N',
        '-o', 'ServerAliveInterval=30', '-o', 'ExitOnForwardFailure=yes',
        '-L', '127.0.0.1:14333:localhost:1433', 'root@116.202.8.155' -WindowStyle Minimized
    $ok = $false
    foreach ($i in 1..15) { Start-Sleep 2; if (Test-Port14333) { $ok = $true; break } }
    if (-not $ok) {
        throw 'Тунелот не се крена. Најчесто виси стар ssh процес — затвори ги со: Get-Process ssh | Stop-Process -Force, па пушти повторно.'
    }
    Write-Host '      Тунелот е горе.' -ForegroundColor Green
}

# --- 2. Dev API ---
if (Test-Api) {
    Write-Host '[2/4] Локалниот API (порта 5300) е веќе крентат.' -ForegroundColor Green
} else {
    Write-Host '[2/4] Кревам локален API (нов прозорец; првиот старт бilda ~1-2 мин)…'
    Start-Process dotnet -ArgumentList 'run', '--launch-profile', 'http' -WorkingDirectory $apiDir
    $ok = $false
    foreach ($i in 1..60) { Start-Sleep 3; if (Test-Api) { $ok = $true; break } }
    if (-not $ok) { throw 'API-то не одговори за 3 минути — погледни го неговиот прозорец за грешка (најчесто тунелот паднал).' }
    Write-Host '      API-то е горе.' -ForegroundColor Green
}

# --- 3. Најава ---
Write-Host '[3/4] Најава како admin…'
$login = Invoke-RestMethod -Method Post -Uri 'http://localhost:5300/api/auth/login' `
    -ContentType 'application/json' `
    -Body (@{ userName = 'admin'; password = $adminPw } | ConvertTo-Json)
$token = $login.token
if (-not $token) { throw 'Најавата не успеа.' }

# --- 4. Sync ---
Write-Host "[4/4] Синхронизирам со легаси ($(Get-Date -Format HH:mm:ss)) — трае ~2 минути, не затворај…"
$sw = [System.Diagnostics.Stopwatch]::StartNew()
$r = Invoke-RestMethod -Method Post -Uri 'http://localhost:5300/api/admin/legacy-sync' `
    -Headers @{ Authorization = "Bearer $token" } -TimeoutSec 600
$sw.Stop()

Write-Host ''
Write-Host ("Sync ЗАВРШИ за {0:n0} секунди. Нови записи:" -f $sw.Elapsed.TotalSeconds) -ForegroundColor Green
Write-Host ("  Барања:               {0}" -f $r.requests)
Write-Host ("  Технички прегледи:    {0}" -f $r.technicalExamReports)
Write-Host ("  Регистрации:          {0}" -f $r.registrations)
Write-Host ("  Возила:               {0}" -f $r.vehicles)
Write-Host ("  Клиенти:              {0}" -f $r.clients)
Write-Host ("  Врски клиент-возило:  {0}" -f $r.relations)
Write-Host ("  Полномошна:           {0}" -f $r.vehiclePermissions)
Write-Host ("  Меѓ. возачки дозволи: {0}" -f $r.internationalDrivingLicences)
Write-Host ("  Докази сопственост:   {0}" -f $r.ownershipProofs)
Write-Host ("  Докази уплата:        {0}" -f $r.paymentProofs)
Write-Host ''
Write-Host ("Водени маркери: клиент {0} · возило {1} · барање {2}" -f $r.maxClientId, $r.maxVehicleId, $r.maxRequestId) -ForegroundColor DarkGray
Write-Host 'Тунелот и API-то остануваат кренати (слободно затвори ги нивните прозорци кога ќе завршиш).' -ForegroundColor DarkGray
