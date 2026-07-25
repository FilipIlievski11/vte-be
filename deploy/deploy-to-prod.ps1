# deploy-to-prod.ps1 — качи нова верзија на продукција, со еден клик.
#
#   .\deploy\deploy-to-prod.ps1            (прашува за потврда)
#   .\deploy\deploy-to-prod.ps1 -Force     (без прашање)
#
# Што прави, по ред:
#   1. Потврда (прод се заменува!) + гаси локален VTE.Api ако врти (build lock).
#   2. Гради сè: vite build + dotnet publish + zip  (преку build-release.ps1).
#   3. Го качува release.zip на серверот и чува временска резерва во /opt/vte/app/releases/
#      (последните 10 — за враќање на стара верзија, види docs/13-deploy.md).
#   4. Го препакува и рестартира api контејнерот (docker compose up -d --build api).
#   5. Верификација: контејнерите се здрави, API одговара, и bundle-от на прод е
#      ИДЕНТИЧЕН со локалниот (спореди hash на index-*.js).
#
# Базата НЕ се допира при деплој — EF миграциите се применуваат автоматски при
# стартот на API-то ако има нови.

param([switch]$Force)

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$zip = Join-Path $PSScriptRoot 'out\release.zip'
$sshKey = "$env:USERPROFILE\.ssh\vte_deploy"
$server = 'root@116.202.8.155'
$prodUrl = 'https://116.202.8.155.sslip.io'

# --- 0. Спореди верзии: што врти на прод vs што ќе качиш ---
$localHash = (git -C $root rev-parse --short HEAD)
$dirty = [bool](git -C $root status --porcelain)
if ($dirty) { $localVer = "$localHash-dirty" } else { $localVer = $localHash }
$prodVer = $null; $prodStamp = $null
try {
    $vt = (Invoke-WebRequest -Uri "$prodUrl/version.txt" -UseBasicParsing -TimeoutSec 10).Content.Trim()
    if ($vt -match '^([0-9a-f]+(?:-dirty)?) (.+)$') { $prodVer = $Matches[1]; $prodStamp = $Matches[2] }
} catch { }

if ($prodVer) {
    Write-Host ("Прод сега врти:  {0}  (качено {1})" -f $prodVer, $prodStamp)
} else {
    Write-Host 'Прод сега врти:  (непозната верзија — качена пред да постои маркерот)' -ForegroundColor DarkGray
}
if ($dirty) {
    Write-Host ("Ќе качиш:        {0}  (внимание: има некомитирани локални промени)" -f $localVer) -ForegroundColor Yellow
} else {
    Write-Host ("Ќе качиш:        {0}" -f $localVer)
}
$sameVersion = (-not $dirty) -and ($prodVer -eq $localVer)
if ($sameVersion) {
    Write-Host 'ВНИМАНИЕ: прод ВЕЌЕ е на оваа верзија — нема нови промени за качување.' -ForegroundColor Yellow
}

if (-not $Force) {
    Write-Host 'Ова ќе ја ЗАМЕНИ верзијата на продукција (116.202.8.155.sslip.io).' -ForegroundColor Yellow
    if ($sameVersion) { $prompt = 'Прод е веќе на оваа верзија — продолжи сепак? (Y/N)' } else { $prompt = 'Продолжи? (Y/N)' }
    $answer = Read-Host $prompt
    if ($answer -notin @('Y', 'y', 'D', 'd', 'da', 'DA', 'Da')) { Write-Host 'Откажано.'; return }
}

# --- 1. Гаси локален API (го држи exe-то заклучено за build) ---
$api = Get-CimInstance Win32_Process -Filter "Name='VTE.Api.exe'" -ErrorAction SilentlyContinue
if ($api) {
    Write-Host '[1/5] Гасам локален VTE.Api (build lock)…'
    $api | ForEach-Object { Stop-Process -Id $_.ProcessId -Force }
    Start-Sleep 2
} else {
    Write-Host '[1/5] Нема локален VTE.Api што врти.' -ForegroundColor Green
}

# --- 2. Билд ---
Write-Host '[2/5] Градам (vite + dotnet publish + zip)…'
& (Join-Path $PSScriptRoot 'build-release.ps1')
if (-not (Test-Path $zip)) { throw 'release.zip не е произведен — погледни ги грешките погоре.' }
$localBundle = (Get-ChildItem (Join-Path $PSScriptRoot 'out\publish\wwwroot\assets') -Filter 'index-*.js').Name
Write-Host ("      Локален bundle: {0}" -f $localBundle) -ForegroundColor DarkGray

# --- 3. Качување + резерва ---
Write-Host '[3/5] Качувам на серверот…'
scp -i $sshKey $zip ("{0}:/opt/vte/app/release.zip" -f $server)
if ($LASTEXITCODE -ne 0) { throw 'scp не успеа — провери интернет/SSH клуч.' }
$stamp = Get-Date -Format 'yyyyMMdd-HHmm'
$remote = @"
set -e
cd /opt/vte/app
mkdir -p releases
cp release.zip releases/release-$stamp.zip
ls -t releases/release-*.zip | tail -n +11 | xargs -r rm --
rm -rf publish
unzip -o -q release.zip -d publish || [ `$? -eq 1 ]
cd /opt/vte
docker compose up -d --build api
docker compose ps --format '{{.Name}} {{.Status}}'
"@
ssh -i $sshKey $server $remote
if ($LASTEXITCODE -ne 0) { throw 'Далечинскиот деплој падна — прочитај ја грешката погоре (мрежен прекин кон docker registry се решава со повторно пуштање).' }

# --- 4. Чекај API да се крене ---
Write-Host '[4/5] Чекам прод API…'
Start-Sleep 10
$alive = $false
foreach ($i in 1..12) {
    try {
        Invoke-WebRequest -Uri "$prodUrl/api/citizenships" -UseBasicParsing -TimeoutSec 8 | Out-Null
        $alive = $true; break
    } catch {
        if ($null -ne $_.Exception.Response) { $alive = $true; break }   # 401 = живо
        Start-Sleep 5
    }
}
if (-not $alive) { throw 'Прод API не одговори за ~1 мин — провери docker logs на серверот.' }

# --- 5. Bundle паритет ---
Write-Host '[5/5] Проверувам дека прод служи иста верзија…'
$html = (Invoke-WebRequest -Uri $prodUrl -UseBasicParsing -TimeoutSec 15).Content
if ($html -match 'index-[A-Za-z0-9_-]+\.js') { $prodBundle = $Matches[0] } else { $prodBundle = '(не најден)' }
$newProdVer = $null
try {
    $vt2 = (Invoke-WebRequest -Uri "$prodUrl/version.txt" -UseBasicParsing -TimeoutSec 10).Content.Trim()
    if ($vt2 -match '^([0-9a-f]+(?:-dirty)?) (.+)$') { $newProdVer = $Matches[1] }
} catch { }

Write-Host ''
if ($prodBundle -eq $localBundle) {
    Write-Host ("ДЕПЛОЈ УСПЕШЕН — прод е на {0} (идентично со локалниот билд)." -f $prodBundle) -ForegroundColor Green
} else {
    Write-Host ("ВНИМАНИЕ: прод служи {0}, локално {1} — освежи со Ctrl+F5 па провери повторно; ако не се совпадне, повтори го деплојот." -f $prodBundle, $localBundle) -ForegroundColor Yellow
}
if ($newProdVer -eq $localVer) {
    Write-Host ("Верзија на прод: {0} — потврдено." -f $newProdVer) -ForegroundColor Green
} elseif ($newProdVer) {
    Write-Host ("ВНИМАНИЕ: прод пријавува верзија {0}, а качуваше {1}." -f $newProdVer, $localVer) -ForegroundColor Yellow
}
Write-Host ("Резерва зачувана на серверот: /opt/vte/app/releases/release-{0}.zip" -f $stamp) -ForegroundColor DarkGray
Write-Host 'Забелешка: локалниот dev API беше изгасен за билдот — пушти го повторно ако ти треба (dotnet run во backend-v2/src/VTE.Api).' -ForegroundColor DarkGray
