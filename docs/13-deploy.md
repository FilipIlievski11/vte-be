# 13 · Рачно качување верзија на продукција

Продукцијата е на Hetzner (https://116.202.8.155.sslip.io) — Docker стек со `mssql`,
`api` (.NET + Vue во wwwroot) и `caddy`. „Нова верзија" значи: се гради сè локално,
се качува еден `release.zip`, и api контејнерот се препакува. **Базата не се допира**
— ако новата верзија носи EF миграции, тие се применуваат сами при стартот на API-то.

## Најбрзо: еден клик

На Desktop: кратенка **„VTE Kaci na prod"** — двоен клик, прашува „Продолжи? (Y/N)",
куцаш `Y` и чекаш ~3–5 минути.

Или од PowerShell, од било каде:

```powershell
& "C:\Users\filip\OneDrive\Documents\Repos\trunk\trunk\deploy\deploy-to-prod.ps1"
```

Скриптата по ред:

1. **Споредба на верзии** — ти покажува која верзија врти на прод и која ќе качиш
   (git комит hash). Ако се **исти**, добиваш жолто предупредување „прод ВЕЌЕ е на
   оваа верзија — нема нови промени" и прашањето станува „продолжи сепак?".
2. **Потврда** + гаси локален `VTE.Api` ако врти (инаку билдот паѓа со MSB3027).
3. **Гради**: `vite build` (frontend) + `dotnet publish -c Release` (backend) + zip
   — преку постоечкиот `deploy/build-release.ps1`. Билдот запишува `version.txt`
   (git hash + време) што прод потоа го служи.
4. **Качува** на серверот и чува **временска резерва** во `/opt/vte/app/releases/`
   (се чуваат последните 10 верзии — за враќање назад).
5. **Препакува** и рестартира: `docker compose up -d --build api`.
6. **Верифицира**: контејнерите се здрави, API одговара, прод служи **идентичен
   bundle** со локалниот билд, и `version.txt` на прод е точно верзијата што ја качи.

Ако на крај пише „ДЕПЛОЈ УСПЕШЕН… Верзија на прод: … — потврдено." со зелено —
готово. Освежи ја апликацијата со **Ctrl+F5** (за да не служи кеширана верзија).

## Која верзија е сега на прод?

Отвори **https://116.202.8.155.sslip.io/version.txt** — пишува git комит hash и кога
е качена верзијата, на пример `4f0faee 2026-07-25 02:40`. Истото го печати и
скриптата на самиот почеток („Прод сега врти: …"). Ознака `-dirty` значи дека
верзијата била качена со некомитирани локални промени.

## Кога се качува верзија

Само кога има промени во КОДОТ (frontend/backend). Податоците (sync со легаси,
промени низ апликацијата) се живи веднаш — за нив не треба деплој. Ако пушташ
деплој без нови промени, скриптата ќе те предупреди — безопасно е, само нема
што ново да качи.

## Рачно, чекор по чекор (ако скриптата не работи)

```powershell
# 1. Гаси локален API ако врти (build lock)
Get-Process | Where-Object { $_.Name -eq 'VTE.Api' } | Stop-Process -Force

# 2. Билд (прави deploy\out\release.zip)
cd C:\Users\filip\OneDrive\Documents\Repos\trunk\trunk
.\deploy\build-release.ps1

# 3. Качи
scp -i $env:USERPROFILE\.ssh\vte_deploy .\deploy\out\release.zip root@116.202.8.155:/opt/vte/app/release.zip

# 4. Распакувај + рестартирај (на серверот)
ssh -i $env:USERPROFILE\.ssh\vte_deploy root@116.202.8.155 "cd /opt/vte/app && rm -rf publish && (unzip -o -q release.zip -d publish || [ `$? -eq 1 ]) && cd /opt/vte && docker compose up -d --build api"

# 5. Провери
# отвори https://116.202.8.155.sslip.io — Ctrl+F5, најави се, кликни низ апликацијата
```

## Враќање на стара верзија (rollback)

Секој деплој остава резерва на серверот. За враќање:

```powershell
# погледни ги достапните резерви
ssh -i $env:USERPROFILE\.ssh\vte_deploy root@116.202.8.155 "ls -lt /opt/vte/app/releases/"

# врати конкретна (замени го датумот)
ssh -i $env:USERPROFILE\.ssh\vte_deploy root@116.202.8.155 "cd /opt/vte/app && rm -rf publish && (unzip -o -q releases/release-20260717-1030.zip -d publish || [ `$? -eq 1 ]) && cd /opt/vte && docker compose up -d --build api"
```

> ⚠ Rollback НЕ ги враќа миграциите на базата — ако новата верзија додала колони,
> старата верзија едноставно не ги користи (безопасно). Никогаш не бришеме колони.

## Најчести проблеми

| Симптом | Причина / решение |
|---|---|
| Билдот паѓа со MSB3027 „file is locked by VTE.Api" | Локалниот dev API врти — скриптата го гаси сама; рачно: `Get-Process VTE.Api \| Stop-Process -Force`. |
| `failed to resolve source metadata for mcr.microsoft.com/...` при docker build | Минлив мрежен прекин кон Microsoft registry — пушти го деплојот повторно, поминува. |
| Прод служи стар bundle по деплој | Кеширано во browser — Ctrl+F5. Ако и понатаму: провери дали scp/unzip навистина поминале (скриптата го проверува ова сама). |
| API не се крева по деплој | `ssh … "cd /opt/vte && docker compose logs api --tail 50"` — најчесто грешка при миграција; јави се. |
| `vite build` паѓа | Има TypeScript/build грешка во кодот — не се качува скршена верзија; поправи прво локално. |

## Кратенки на Desktop (сите три)

| Кратенка | Што прави | Скрипта |
|---|---|---|
| **VTE Sync so legacy** | Свежи податоци од стариот систем + офсајт DB backup | `deploy/run-legacy-sync.ps1` |
| **VTE Kaci na prod** | Нова верзија на продукција (прашува Y/N) | `deploy/deploy-to-prod.ps1` |
| **VTE Push na GitHub** | Ги пушта локалните комити на GitHub (код-резерва) | `deploy/push-to-github.ps1` |

> Push и деплој се независни: push е резерва на кодот на GitHub; деплој е она што
> станицата го користи. Редослед кога има нови промени: прво push, па деплој (или обратно
> — сеедно, само не заборавај ги двете).

## Поврзано

- [09 — Deployment & operations](09-deployment-and-operations.md) — целиот прод стек, тајни, DB re-lift.
- [12 — Рачен sync со легаси](12-legacy-sync.md) — за податоци (не бара деплој).
