# push-to-github.ps1 — пушни ги ДВЕТЕ репоа (BE + FE) на GitHub, со еден клик.
#
#   .\deploy\push-to-github.ps1
#
# Што прави, за секое репо (Repos\VTE\BE → vte-be, Repos\VTE\FE → vte-fe):
#   1. Проверува дали има некомитирани промени (тие НЕ одат со push — само комити).
#   2. Ги листа комитите што ќе се пушнат.
#   3. git push кон origin/main и печати резултат.

$ErrorActionPreference = 'Stop'
$be = Split-Path -Parent $PSScriptRoot          # ...\Repos\VTE\BE
$fe = Join-Path (Split-Path $be -Parent) 'FE'   # ...\Repos\VTE\FE

$vkupno = 0
$neuspesni = 0

foreach ($repo in @($be, $fe)) {
    $ime = Split-Path $repo -Leaf
    Write-Host ("=== {0} ({1}) ===" -f $ime, $repo) -ForegroundColor Cyan
    if (-not (Test-Path (Join-Path $repo '.git'))) {
        Write-Host '  Не е git репо — прескокнато.' -ForegroundColor Red
        $neuspesni++
        Write-Host ''
        continue
    }
    Push-Location $repo
    try {
        # 0. Некомитирани промени — само предупредување, push-от продолжува
        $dirty = git status --porcelain
        if ($dirty) {
            Write-Host '  ВНИМАНИЕ: има некомитирани промени — тие НЕ се пуштаат (одат само комитите):' -ForegroundColor Yellow
            $dirty | Select-Object -First 10 | ForEach-Object { Write-Host "    $_" -ForegroundColor DarkGray }
            if (@($dirty).Count -gt 10) { Write-Host ("    … и уште {0}" -f (@($dirty).Count - 10)) -ForegroundColor DarkGray }
        }

        # 1. Што има за push
        git fetch origin --quiet
        $unpushed = @(git log origin/main..HEAD --oneline)
        if ($unpushed.Count -eq 0) {
            Write-Host '  Нема нови комити — GitHub е веќе изедначен.' -ForegroundColor Green
            Write-Host ''
            continue
        }
        Write-Host ("  Комити за push ({0}):" -f $unpushed.Count)
        $unpushed | ForEach-Object { Write-Host "    $_" }

        # 2. Push
        git push
        if ($LASTEXITCODE -eq 0) {
            Write-Host ("  PUSH УСПЕШЕН — {0} комити." -f $unpushed.Count) -ForegroundColor Green
            $vkupno += $unpushed.Count
        } else {
            Write-Host '  Push-от не успеа — прочитај ја грешката погоре (најчесто: нема интернет).' -ForegroundColor Red
            $neuspesni++
        }
    } finally { Pop-Location }
    Write-Host ''
}

if ($neuspesni -eq 0) {
    Write-Host ("ГОТОВО — вкупно {0} нови комити на GitHub (vte-be + vte-fe)." -f $vkupno) -ForegroundColor Green
} else {
    Write-Host 'Завршено со грешки — види погоре.' -ForegroundColor Red
}
