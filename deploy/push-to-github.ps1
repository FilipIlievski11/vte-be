# push-to-github.ps1 — пушни ги локалните комити на GitHub, со еден клик.
#
#   .\deploy\push-to-github.ps1
#
# Што прави:
#   1. Проверува дали има некомитирани промени (тие НЕ одат со push — само комити).
#   2. Ги листа комитите што ќе се пушнат.
#   3. git push кон origin/main и печати резултат.

$ErrorActionPreference = 'Stop'
$repo = Split-Path -Parent $PSScriptRoot
Set-Location $repo

# 0. Некомитирани промени — само предупредување, push-от продолжува
$dirty = git status --porcelain
if ($dirty) {
    Write-Host 'ВНИМАНИЕ: има некомитирани промени — тие НЕ се пуштаат (одат само комитите):' -ForegroundColor Yellow
    $dirty | Select-Object -First 10 | ForEach-Object { Write-Host "  $_" -ForegroundColor DarkGray }
    if (@($dirty).Count -gt 10) { Write-Host ("  … и уште {0}" -f (@($dirty).Count - 10)) -ForegroundColor DarkGray }
    Write-Host ''
}

# 1. Што има за push
git fetch origin --quiet
$unpushed = @(git log origin/main..HEAD --oneline)
if ($unpushed.Count -eq 0) {
    Write-Host 'Нема нови комити — GitHub е веќе изедначен со локалното.' -ForegroundColor Green
    return
}
Write-Host ("Комити за push ({0}):" -f $unpushed.Count)
$unpushed | ForEach-Object { Write-Host "  $_" }
Write-Host ''

# 2. Push
git push
if ($LASTEXITCODE -eq 0) {
    Write-Host ''
    Write-Host ("PUSH УСПЕШЕН — {0} комити се на GitHub." -f $unpushed.Count) -ForegroundColor Green
} else {
    Write-Host ''
    Write-Host 'Push-от не успеа — прочитај ја грешката погоре (најчесто: нема интернет).' -ForegroundColor Red
}
