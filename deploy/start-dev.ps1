# start-dev.ps1 — пушти ги апликациите локално, со избор на режим.
#
#   .\deploy\start-dev.ps1                # прашува 1/2
#   .\deploy\start-dev.ps1 -Rezim dvete   # без прашање: двете локално
#   .\deploy\start-dev.ps1 -Rezim prod    # без прашање: FE → прод API
#
# Режими (И ДВАТА работат врз ПРОД податоците — одлука 24.09.2026):
#   1) „dvete" — API прозорец (dotnet run, 5300, база = ПРОД преку SSH тунел
#               од appsettings.Development.json) + FE прозорец (5174).
#               За тестирање на НОВ BE-код врз вистински податоци.
#   2) „prod"  — САМО FE прозорец (5174), /api → КАЦЕНИОТ прод API
#               (https://116.202.8.155.sslip.io). За чист FE-развој.
# Логин: секогаш прод-креденцијалите.
#
# Гаснење: затвори ги прозорците (или Ctrl+C во нив). За смена на режим
# прво затвори го стариот FE прозорец — портата 5174 е една.

param(
    [ValidateSet('', 'dvete', 'prod')]
    [string]$Rezim = ''
)

$ErrorActionPreference = 'Stop'
$be      = Split-Path -Parent $PSScriptRoot
$fe      = Join-Path (Split-Path $be -Parent) 'FE'
$api     = Join-Path $be 'backend-v2\src\VTE.Api'
$feApp   = Join-Path $fe 'frontend-v2'
$prodUrl = 'https://116.202.8.155.sslip.io'

if (-not (Test-Path (Join-Path $api 'VTE.Api.csproj'))) { throw "Не постои API проектот: $api" }
if (-not (Test-Path (Join-Path $feApp 'package.json')))  { throw "Не постои frontend-от: $feApp" }

# Порта-проверка на IPv4 И IPv6 loopback — vite врзува само ::1, API 127.0.0.1.
# Конструкторот (host, port) ја бира правата фамилија; голиот TcpClient во
# PS 5.1 е IPv4-only и лажно кажува „слободно" за IPv6-порти.
function Test-VtePort([int]$port) {
    foreach ($adr in '127.0.0.1', '::1') {
        try { (New-Object Net.Sockets.TcpClient($adr, $port)).Close(); return $true } catch { }
    }
    return $false
}

# ---- Избор на режим ----
if (-not $Rezim) {
    Write-Host ''
    Write-Host '  1) API + FE локално — API-то работи врз ПРОД базата (тунел)' -ForegroundColor Cyan
    Write-Host '  2) само FE          — /api оди кон КАЦЕНИОТ прод API' -ForegroundColor Yellow
    Write-Host '  (двата режима = вистински податоци, прод логин)' -ForegroundColor DarkGray
    $izbor = Read-Host 'Режим [1/2] (Enter = 1)'
    $Rezim = if ($izbor -eq '2') { 'prod' } else { 'dvete' }
}

# Локалниот API СЕКОГАШ работи врз ПРОД базата (appsettings.Development.json →
# 127.0.0.1:14333 = SSH тунел до прод SQL, одлука 24.09.2026). Тунелот често
# паѓа — крени го ако не стои.
function Ensure-Tunnel {
    if (Test-VtePort 14333) { return $true }
    Write-Host 'Тунелот до прод базата не стои — го кревам…' -ForegroundColor Yellow
    Get-Process ssh -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 1
    Start-Process ssh -ArgumentList '-i', "$env:USERPROFILE\.ssh\vte_deploy", '-N',
        '-o', 'ServerAliveInterval=30', '-o', 'ExitOnForwardFailure=yes',
        '-L', '127.0.0.1:14333:localhost:1433', 'root@116.202.8.155' -WindowStyle Minimized
    $rokT = (Get-Date).AddSeconds(20)
    do { Start-Sleep -Seconds 2; $t = Test-VtePort 14333 } while (-not $t -and (Get-Date) -lt $rokT)
    if ($t) { Write-Host 'Тунелот е горе.' -ForegroundColor Green }
    else { Write-Host 'Тунелот НЕ се крена — провери интернет/SSH. API-то нема да работи без него.' -ForegroundColor Red }
    return $t
}

# ---- API (само во режим „dvete") ----
if ($Rezim -eq 'dvete') {
    $null = Ensure-Tunnel
    if (Test-VtePort 5300) {
        Write-Host 'API веќе работи на 5300 — не пуштам втор.' -ForegroundColor Yellow
    } else {
        $cmdApi = "`$Host.UI.RawUI.WindowTitle = 'VTE API (5300) -> PROD baza'; Write-Host 'ВНИМАНИЕ: базата е ПРОД (преку тунел) — вистински податоци!' -ForegroundColor Red; Set-Location '{0}'; dotnet run --launch-profile http" -f $api
        Start-Process powershell -ArgumentList '-NoExit', '-ExecutionPolicy', 'Bypass', '-Command', $cmdApi
        Write-Host 'API се пушта во свој прозорец (порта 5300, база = ПРОД преку тунел)…' -ForegroundColor Cyan
    }
}

# ---- FE ----
if (Test-VtePort 5174) {
    Write-Host 'FE веќе работи на 5174 — не пуштам втор.' -ForegroundColor Yellow
    Write-Host '  (Ако сакаш ДРУГ режим, прво затвори го постоечкиот FE прозорец па пушти пак.)' -ForegroundColor DarkGray
} else {
    if ($Rezim -eq 'prod') {
        $cmdFe = "`$Host.UI.RawUI.WindowTitle = 'VTE FE (5174) -> PROD'; Write-Host 'ВНИМАНИЕ: /api оди кон ПРОД — вистински податоци!' -ForegroundColor Red; `$env:VITE_PROXY_TARGET = '{0}'; Set-Location '{1}'; if (-not (Test-Path node_modules)) {{ Write-Host 'Прв пат: npm install…'; npm install }}; npm run dev" -f $prodUrl, $feApp
        Write-Host ("FE се пушта во свој прозорец (порта 5174) -> ПРОД API ({0})…" -f $prodUrl) -ForegroundColor Yellow
        Write-Host 'ВНИМАНИЕ: работиш врз ВИСТИНСКАТА прод база — логин со прод-креденцијали.' -ForegroundColor Red
    } else {
        $cmdFe = "`$Host.UI.RawUI.WindowTitle = 'VTE FE (5174) - lokalen API'; Set-Location '{0}'; if (-not (Test-Path node_modules)) {{ Write-Host 'Прв пат: npm install…'; npm install }}; npm run dev" -f $feApp
        Write-Host 'FE се пушта во свој прозорец (порта 5174) -> локален API…' -ForegroundColor Cyan
    }
    Start-Process powershell -ArgumentList '-NoExit', '-ExecutionPolicy', 'Bypass', '-Command', $cmdFe
}

# ---- Почекај па отвори прегледувач ----
# (првиот dotnet run билдира Debug од нула — со OneDrive тоа знае да трае минута+)
$cekamZa = if ($Rezim -eq 'dvete') { 'API (5300) и FE (5174)' } else { 'FE (5174)' }
Write-Host ("Чекам {0} да се крене…" -f $cekamZa) -ForegroundColor DarkGray
$rok = (Get-Date).AddSeconds(120)
$apiOk = ($Rezim -ne 'dvete'); $feOk = $false
while ((Get-Date) -lt $rok -and -not ($apiOk -and $feOk)) {
    if (-not $apiOk) { $apiOk = Test-VtePort 5300 }
    if (-not $feOk)  { $feOk  = Test-VtePort 5174 }
    Start-Sleep -Seconds 2
}
if ($apiOk -and $feOk) {
    Start-Process 'http://localhost:5174'
    Write-Host 'Сè работи — отворено: http://localhost:5174 (ПРОД податоци, прод логин)' -ForegroundColor Green
} else {
    if (-not $apiOk) { Write-Host 'API уште не одговара на 5300 — погледни во прозорецот „VTE API" за грешка.' -ForegroundColor Yellow }
    if (-not $feOk)  { Write-Host 'FE уште не одговара на 5174 — погледни во прозорецот „VTE FE" за грешка.'  -ForegroundColor Yellow }
    Write-Host 'Кога ќе се кренат, отвори рачно: http://localhost:5174' -ForegroundColor Yellow
}
