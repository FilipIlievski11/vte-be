# start-dev.ps1 — пушти ги апликациите локално, со избор на режим.
#
#   .\deploy\start-dev.ps1                # прашува 1/2
#   .\deploy\start-dev.ps1 -Rezim dvete   # без прашање: двете локално
#   .\deploy\start-dev.ps1 -Rezim prod    # без прашање: FE → прод API
#
# Режими:
#   1) „dvete" — API прозорец (dotnet run, порта 5300, ЛОКАЛНА LocalDB база)
#               + FE прозорец (npm run dev, 5174) → сигурно за проби
#   2) „prod"  — САМО FE прозорец (5174), а /api оди кон ПРОД
#               (https://116.202.8.155.sslip.io) преку Vite proxy →
#               ВИСТИНСКИ ПОДАТОЦИ, вистински логин-креденцијали!
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
    Write-Host '  1) Двете локално   — API (5300, локална база) + FE (5174)' -ForegroundColor Cyan
    Write-Host '  2) FE -> ПРОД API  — само FE (5174), податоците се ВИСТИНСКИ' -ForegroundColor Yellow
    $izbor = Read-Host 'Режим [1/2] (Enter = 1)'
    $Rezim = if ($izbor -eq '2') { 'prod' } else { 'dvete' }
}

# LocalDB знае да заглави (зомби sqlservr → pipe „Access is denied" → API 500).
# Пред стартот: проба-конекција, па оздравување ако не одговара.
function Repair-LocalDb {
    $conn = New-Object System.Data.SqlClient.SqlConnection 'Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;Connect Timeout=8'
    try { $conn.Open(); $conn.Close(); return $true } catch { }
    Write-Host 'LocalDB не одговара — обид за оздравување…' -ForegroundColor Yellow
    sqllocaldb stop MSSQLLocalDB -k 2>&1 | Out-Null
    Get-CimInstance Win32_Process -Filter "Name='sqlservr.exe'" |
        Where-Object { $_.CommandLine -match 'Local DB|LOCALDB' } |
        ForEach-Object { Stop-Process -Id $_.ProcessId -Force -ErrorAction SilentlyContinue }
    Start-Sleep -Seconds 2
    sqllocaldb start MSSQLLocalDB 2>&1 | Out-Null
    Start-Sleep -Seconds 2
    try { $conn.Open(); $conn.Close(); Write-Host 'LocalDB оздравена.' -ForegroundColor Green; return $true }
    catch { Write-Host 'LocalDB сè уште не одговара — API-то нема да работи. Провери рачно: sqllocaldb info MSSQLLocalDB' -ForegroundColor Red; return $false }
}

# ---- API (само во режим „dvete") ----
if ($Rezim -eq 'dvete') {
    $null = Repair-LocalDb
    if (Test-VtePort 5300) {
        Write-Host 'API веќе работи на 5300 — не пуштам втор.' -ForegroundColor Yellow
    } else {
        $cmdApi = "`$Host.UI.RawUI.WindowTitle = 'VTE API (5300) - lokalna baza'; Set-Location '{0}'; dotnet run --launch-profile http" -f $api
        Start-Process powershell -ArgumentList '-NoExit', '-ExecutionPolicy', 'Bypass', '-Command', $cmdApi
        Write-Host 'API се пушта во свој прозорец (порта 5300, локална база)…' -ForegroundColor Cyan
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
    if ($Rezim -eq 'prod') {
        Write-Host 'Отворено: http://localhost:5174  (податоците се од ПРОД!)' -ForegroundColor Yellow
    } else {
        Write-Host 'Сè работи — отворено: http://localhost:5174 (локална база: admin / ChangeMe!Now1)' -ForegroundColor Green
    }
} else {
    if (-not $apiOk) { Write-Host 'API уште не одговара на 5300 — погледни во прозорецот „VTE API" за грешка.' -ForegroundColor Yellow }
    if (-not $feOk)  { Write-Host 'FE уште не одговара на 5174 — погледни во прозорецот „VTE FE" за грешка.'  -ForegroundColor Yellow }
    Write-Host 'Кога ќе се кренат, отвори рачно: http://localhost:5174' -ForegroundColor Yellow
}
