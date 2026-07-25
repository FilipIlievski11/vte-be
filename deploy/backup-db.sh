#!/bin/bash
# backup-db.sh — daily backup of the VTE prod database. Lives at /opt/vte/backup-db.sh
# on the server, runs from root's crontab at 01:15 UTC (03:15 MK). See docs/14-db-backup.md.
#
# What it does:
#   1. BACKUP DATABASE [VTE] (with CHECKSUM) to the mssql container's volume.
#   2. RESTORE VERIFYONLY — validates the backup is restorable.
#   3. gzip the .bak into /opt/vte/backups/vte-YYYYMMDD.bak.gz (the .bak is removed).
#   4. Rotation: keep the newest 14 backups.
#   5. Appends to /opt/vte/backups/backup.log (trimmed to last 500 lines).
#
# The laptop pulls the newest .gz as an offsite copy every time run-legacy-sync.ps1 runs.

set -euo pipefail

OUT=/opt/vte/backups
LOG=$OUT/backup.log
VOL=/var/lib/docker/volumes/vte_mssql-data/_data
STAMP=$(date +%Y%m%d)
BAK="vte-$STAMP.bak"

mkdir -p "$OUT"
trap 'echo "[$(date "+%F %T")] ERROR — backup FAILED (see lines above)" >> "$LOG"' ERR

echo "[$(date '+%F %T')] starting backup $BAK" >> "$LOG"

# Password never on the command line: sqlcmd reads SQLCMDPASSWORD from the environment.
SA_PASSWORD=$(grep '^SA_PASSWORD=' /opt/vte/.env | cut -d= -f2-)

docker exec vte-mssql-1 mkdir -p /var/opt/mssql/backup

docker exec -e SQLCMDPASSWORD="$SA_PASSWORD" vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -C -b \
  -Q "BACKUP DATABASE [VTE] TO DISK='/var/opt/mssql/backup/$BAK' WITH INIT, CHECKSUM" >> "$LOG" 2>&1

docker exec -e SQLCMDPASSWORD="$SA_PASSWORD" vte-mssql-1 /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -C -b \
  -Q "RESTORE VERIFYONLY FROM DISK='/var/opt/mssql/backup/$BAK' WITH CHECKSUM" >> "$LOG" 2>&1

gzip -c "$VOL/backup/$BAK" > "$OUT/$BAK.gz.part"
mv "$OUT/$BAK.gz.part" "$OUT/$BAK.gz"
rm -f "$VOL/backup/$BAK"

ls -t "$OUT"/vte-*.bak.gz | tail -n +15 | xargs -r rm --

SIZE=$(du -h "$OUT/$BAK.gz" | cut -f1)
echo "[$(date '+%F %T')] OK $BAK.gz ($SIZE), verify passed" >> "$LOG"
tail -n 500 "$LOG" > "$LOG.tmp" && mv "$LOG.tmp" "$LOG"
