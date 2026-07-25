# 12 · Рачен sync со легаси базата

Прод базата (Hetzner) се дополнува со работата што станицата ја внесува во стариот
VB.NET систем. Синхронизацијата НЕ е автоматска — ја пушташ рачно кога сакаш свежи
податоци (обично еднаш-двапати дневно, или пред извештаи).

## Најбрзо: еден клик

На Desktop постои кратенка **„VTE Sync so legacy"** — двоен клик и готово (прозорецот
останува отворен за да ги видиш бројките).

Или рачно, од БИЛО КАДЕ во PowerShell (со полна патека):

```powershell
& "C:\Users\filip\OneDrive\Documents\Repos\trunk\trunk\deploy\run-legacy-sync.ps1"
```

Или прво до репото, па релативно:

```powershell
cd C:\Users\filip\OneDrive\Documents\Repos\trunk\trunk
.\deploy\run-legacy-sync.ps1
```

> Ако PowerShell се жали „running scripts is disabled", пушти ја вака:
> `powershell -ExecutionPolicy Bypass -File "C:\...\deploy\run-legacy-sync.ps1"`
> (кратенката на Desktop веќе го прави тоа сама).

Скриптата сама:

1. крева SSH тунел до прод SQL ако го нема (порта 14333, минимизиран прозорец),
2. крева локален dev API ако го нема (порта 5300 — нов прозорец, првиот старт билда 1–2 мин),
3. се најавува како `admin` (лозинката се чита од `deploy\prod.secrets.local`, никогаш не се печати),
4. го пушта sync-от (**~2 минути**) и ги печати бројките на новите записи.

Безбедно е да се пушти повеќепати — sync-от е идемпотентен: само дополнува што недостига
и ги освежува статусите (сторно, платено, видови плаќање).

## Што точно прави sync-от

`POST /api/admin/legacy-sync` на dev API-то го извршува вградениот
`migrate/migrate-incremental.sql` врз прод базата (низ тунелот). Тој преку linked
серверот `VTEZVV_LIVE` (кој живее на прод SQL контејнерот) чита од живата легаси база
и:

- **дополнува нови записи** од водемарк наваму: клиенти, возила, регистрации, врски,
  барања, технички прегледи, полномошна, МВД, докази, референци, долгови (Наплата),
  сметки и ставки;
- **самолекува ги финансиите**: state-sync на сметки (сторно/статуси), пополнување
  празнини кај ставки (90-дневен прозорец), освежување вид на плаќање по возило,
  PayedAmount ознаки, нови типови на плаќање.

Легаси мирор редовите имаат Id < 10.000.000; сè што станицата ќе создаде директно во
новиот систем добива Id ≥ 10.000.000 и sync-от никогаш не го допира.

## Рачно, чекор по чекор (ако скриптата не работи)

Три PowerShell прозорци:

**Прозорец 1 — тунел** (остави го отворен):

```powershell
.\deploy\sql-tunnel.ps1
```

**Прозорец 2 — dev API** (остави го отворен; чекај „Now listening on … 5300"):

```powershell
cd backend-v2\src\VTE.Api
dotnet run --launch-profile http
```

**Прозорец 3 — најава + sync:**

```powershell
$pw = ((Get-Content .\deploy\prod.secrets.local | Where-Object { $_ -like 'ADMIN_PASSWORD=*' }) -replace '^ADMIN_PASSWORD=','').Trim()
$login = Invoke-RestMethod -Method Post -Uri 'http://localhost:5300/api/auth/login' -ContentType 'application/json' -Body (@{ userName='admin'; password=$pw } | ConvertTo-Json)
Invoke-RestMethod -Method Post -Uri 'http://localhost:5300/api/admin/legacy-sync' -Headers @{ Authorization = "Bearer $($login.token)" } -TimeoutSec 600
```

Одговорот е JSON со бројот на нови записи по ентитет + `durationMs`.

> ⚠ Лозинката НИКОГАШ не ја куцај рачно во bash/да ја вметнуваш во команди со
> наводници — по ~5 погрешни обиди ASP.NET ја заклучува сметката (гоtcha #15 во
> CLAUDE.md). PowerShell читањето од фајлот погоре е безбедниот пат.

## Најчести проблеми

| Симптом | Причина / решение |
|---|---|
| „Address already in use" за 14333 / тунелот не се крева | Виси стар ssh процес: `Get-Process ssh \| Stop-Process -Force`, па повторно. |
| API-то фрла SqlException среде работа | Тунелот паднал (мрежен прекин). Скриптата/tunel-от повторно, па повтори го sync-от. |
| API-то не крева, MSB3027 build lock | Веќе врти друга инстанца на VTE.Api — искористи ја неа или згаси ја. |
| „Account is locked" при најава | Премногу погрешни лозинки. Отклучување преку SQL: `UPDATE AspNetUsers SET LockoutEnd=NULL, AccessFailedCount=0` (низ тунелот, sqlcmd со `-I`). |
| Sync трае >5 мин / timeout | Легаси серверот (195.26.…) е спор или недостапен — пробај подоцна. |

## Колку често

Колку сакаш — секој sync зема ~2 минути и е безопасен. Практично: наутро пред работа
и/или пред да вадиш извештаи (Дневна распределба, Преглед за наплата) за да ги имаш
најновите сметки од легаси.
