# start-dev.ps1 — пушти ги двете апликации локално, со еден клик.
#
#   .\deploy\start-dev.ps1
#
# Отвора два прозорца:
#   • „VTE API (5300)" — dotnet run во BE\backend-v2\src\VTE.Api (LocalDB база,
#     од appsettings.json; ако сакаш прод преку тунел — копирај си
#     appsettings.Development.json од стариот trunk)
#   • „VTE FE (5174)"  — npm run dev во FE\frontend-v2 (прв пат прави npm install)
# и по 8 секунди го отвора http://localhost:5174 во прегледувачот.
# Гаснење: затвори ги двата прозорца (или Ctrl+C во нив).

$ErrorActionPreference = 'Stop'
$be    = Split-Path -Parent $PSScriptRoot
$fe    = Join-Path (Split-Path $be -Parent) 'FE'
$api   = Join-Path $be 'backend-v2\src\VTE.Api'
$feApp = Join-Path $fe 'frontend-v2'

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

# Предупреди ако API-то е веќе пуштено (портата 5300 зафатена) — не пуштај двојно.
if (Test-VtePort 5300) {
    Write-Host 'API веќе работи на 5300 — не пуштам втор.' -ForegroundColor Yellow
} else {
    $cmdApi = "`$Host.UI.RawUI.WindowTitle = 'VTE API (5300)'; Set-Location '{0}'; dotnet run --launch-profile http" -f $api
    Start-Process powershell -ArgumentList '-NoExit', '-ExecutionPolicy', 'Bypass', '-Command', $cmdApi
    Write-Host 'API се пушта во свој прозорец (порта 5300)…' -ForegroundColor Cyan
}

if (Test-VtePort 5174) {
    Write-Host 'FE веќе работи на 5174 — не пуштам втор.' -ForegroundColor Yellow
} else {
    $cmdFe = "`$Host.UI.RawUI.WindowTitle = 'VTE FE (5174)'; Set-Location '{0}'; if (-not (Test-Path node_modules)) {{ Write-Host 'Прв пат: npm install…'; npm install }}; npm run dev" -f $feApp
    Start-Process powershell -ArgumentList '-NoExit', '-ExecutionPolicy', 'Bypass', '-Command', $cmdFe
    Write-Host 'FE се пушта во свој прозорец (порта 5174)…' -ForegroundColor Cyan
}

Start-Sleep -Seconds 8
Start-Process 'http://localhost:5174'
Write-Host 'Отворено: http://localhost:5174' -ForegroundColor Green
