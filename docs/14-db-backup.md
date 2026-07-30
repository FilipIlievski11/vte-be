# 14 · Резерви (backup) на прод базата

Прод базата ги носи ВИСТИНСКИТЕ податоци на станицата. Легаси миророт може повторно
да се синхронизира, но сè што е внесено директно во новиот систем (сметки, прегледи,
барања со Id ≥ 10.000.000) постои САМО таму — затоа има двослоен backup:

1. **Серверот сам прави дневна резерва** — секоја ноќ во **03:15** (наше време).
2. **Лаптопот повлекува офсајт копија** — секој пат кога ќе пуштиш sync со легаси.

Ништо од ова не бара рачна работа — поставено е и работи само.

## Што прави серверот (автоматски)

Cron на серверот ја пушта `/opt/vte/backup-db.sh` (изворот е во `deploy/backup-db.sh`):

1. `BACKUP DATABASE [VTE]` со CHECKSUM (цела база, ~770 MB).
2. `RESTORE VERIFYONLY` — проверува дека резервата е навистина рестабилна.
3. Компресија во `/opt/vte/backups/vte-YYYYMMDD.bak.gz` (~100 MB).
4. Ротација: се чуваат последните **14 дневни** резерви (~1.4 GB, дискот има 56 GB).
5. Сè се запишува во `/opt/vte/backups/backup.log`.

Целата постапка трае ~20 секунди и не ја попречува апликацијата (SQL Server прави
online backup — станицата може нормално да работи).

## Офсајт копија на лаптопот (автоматски со sync)

`deploy/run-legacy-sync.ps1` (кратенката „VTE Sync so legacy") на крајот од секој
sync ја повлекува најновата резерва во:

```
C:\Users\filip\VTE-backups\
```

Се чуваат последните **10** (~1 GB). Ако повлекувањето не успее (нема интернет,
серверот зафатен), sync-от сепак завршува нормално — ќе се повлече следниот пат.
Папката е НАДВОР од OneDrive за да не го полни облакот.

> Значи: серверска резерва имаш секое утро; офсајт копија имаш од последниот ден
> кога си пуштил sync. Пушташ sync секако секој ден — тоа е доволно.

## Алармирање — апликацијата сама кажува

Не мора ништо да проверуваш рачно: backup скриптата запишува статус
(`/data/attachments/ops/backup-status.json` во attachments volume-от), а прод
API-то го чита преку `GET /api/admin/ops-health`. Ако ноќниот backup **не успеал**
или **нема успешен повеќе од 26 часа**, на секој екран (само за администратор) се
појавува црвен banner: „ВНИМАНИЕ — ноќните задачи: Резерва на базата…". Истиот
banner јавува и ако автоматскиот sync не поминал. Нема banner = сè е во ред.

## Како да проверам дека работи?

Најбрзо — во прозорецот на sync-от, чекор `[5/5]` кажува што е повлечено. Или:

```powershell
# локалните офсајт копии
dir C:\Users\filip\VTE-backups

# што има на серверот + дали ноќешната поминала
ssh -i $env:USERPROFILE\.ssh\vte_deploy root@116.202.8.155 "ls -lh /opt/vte/backups/ && tail -5 /opt/vte/backups/backup.log"
```

Во логот бараш линија `OK vte-YYYYMMDD.bak.gz (98M), verify passed` со денешен датум.

## Враќање на базата од резерва (restore)

**Само во катастрофа** — ова ја ЗАМЕНУВА целата база со состојбата од резервата.
Сè внесено потоа се губи. Ако е можно, прво јави се / консултирај се.

На серверот (`ssh -i ~/.ssh/vte_deploy root@116.202.8.155`):

```bash
cd /opt/vte
docker compose stop api

# избери резерва (или прво scp-ирај ја од лаптопот во /opt/vte/backups/)
ls -lt backups/

# распакувај и стави ја каде што SQL може да ја види
gunzip -kf backups/vte-YYYYMMDD.bak.gz
VOL=/var/lib/docker/volumes/vte_mssql-data/_data
mkdir -p $VOL/backup
cp backups/vte-YYYYMMDD.bak $VOL/backup/restore.bak
chown 10001:0 $VOL/backup/restore.bak

# врати ја базата (лозинката се чита од .env, не се куца)
SA_PASSWORD=$(grep '^SA_PASSWORD=' /opt/vte/.env | cut -d= -f2-)
docker exec -e SQLCMDPASSWORD="$SA_PASSWORD" vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -C -b \
  -Q "ALTER DATABASE [VTE] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; RESTORE DATABASE [VTE] FROM DISK='/var/opt/mssql/backup/restore.bak' WITH REPLACE; ALTER DATABASE [VTE] SET MULTI_USER"

rm $VOL/backup/restore.bak backups/vte-YYYYMMDD.bak
docker compose start api
```

Потоа отвори ја апликацијата и провери дека податоците се од очекуваниот ден.

Ако серверот е целосно мртов (нов сервер): подигни го стекот по
[09 — Deployment & operations](09-deployment-and-operations.md), scp-ирај ја
последната копија од `C:\Users\filip\VTE-backups\` и следи ја истата restore постапка.

## Најчести проблеми

| Симптом | Причина / решение |
|---|---|
| Во логот нема денешна линија | Cron-от не се пуштил (сервер рестартиран?) — пушти рачно: `ssh … "/opt/vte/backup-db.sh"` и погледни го логот. |
| `ERROR — backup FAILED` во логот | Прочитај ги линиите над грешката — најчесто дискот е полн (`df -h`) или SQL контејнерот не врти (`docker compose ps`). |
| Sync-от кажува „Повлекувањето не успеа" | Мрежен прекин — безопасно, следниот sync ќе ја земе. |
| Нема нови `.gz` во C:\Users\filip\VTE-backups | Не си пуштал sync тие денови — серверските резерви сепак постојат (14 дена наназад). |

## Поврзано

- [09 — Deployment & operations](09-deployment-and-operations.md) — прод стекот, re-lift постапка.
- [12 — Рачен sync со легаси](12-legacy-sync.md) — каде се случува офсајт повлекувањето.
