# Runs migrate-from-snapshot.sql with proper batch isolation:
# - splits on GO
# - between batches, forces IDENTITY_INSERT OFF for any stuck table
# - continues past errors so a single bad INSERT doesn't block subsequent ones
param(
  [string]$Conn = "Server=(localdb)\MSSQLLocalDB;Database=VTE2;Integrated Security=True;TrustServerCertificate=True;Connection Timeout=60;",
  [string]$File = "$PSScriptRoot\migrate-from-snapshot.sql",
  [int]$BatchTimeoutSec = 0
)
$ErrorActionPreference = 'Continue'
Add-Type -AssemblyName System.Data

$sql = Get-Content $File -Raw
$batches = [regex]::Split($sql, '(?im)^\s*GO\s*$') | Where-Object { $_.Trim() -ne '' }
Write-Host "Loaded $($batches.Count) batches from $File" -ForegroundColor Cyan

$c = New-Object System.Data.SqlClient.SqlConnection $Conn
$c.Open()

# Capture infomessages (PRINT outputs)
$c.add_InfoMessage({ param($s, $e) Write-Host "  $($e.Message)" -ForegroundColor Gray })
$c.FireInfoMessageEventOnUserErrors = $false

# Force connection-level options upfront
$pre = $c.CreateCommand()
$pre.CommandText = "SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET ANSI_PADDING ON; SET ANSI_WARNINGS ON; SET ARITHABORT ON; SET CONCAT_NULL_YIELDS_NULL ON; SET NUMERIC_ROUNDABORT OFF; SET XACT_ABORT OFF; SET NOCOUNT ON;"
[void]$pre.ExecuteNonQuery()

$resetSql = @"
DECLARE @t SYSNAME, @sql NVARCHAR(MAX) = '';
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR
  SELECT t.name FROM sys.tables t
  WHERE OBJECTPROPERTY(t.object_id, 'TableHasIdentity') = 1
    AND OBJECTPROPERTY(t.object_id, 'TableHasIdentityInsert') = 1;
OPEN cur; FETCH NEXT FROM cur INTO @t;
WHILE @@FETCH_STATUS = 0
BEGIN
  SET @sql = @sql + 'SET IDENTITY_INSERT [' + @t + '] OFF;';
  FETCH NEXT FROM cur INTO @t;
END
CLOSE cur; DEALLOCATE cur;
IF LEN(@sql) > 0 EXEC sp_executesql @sql;
"@

$i = 0
foreach ($batch in $batches) {
  $i++
  # Quick label: first non-empty non-comment line
  $label = ($batch -split "`n" | Where-Object { $_.Trim() -ne '' -and $_.Trim() -notmatch '^--' } | Select-Object -First 1).Trim()
  if ($label.Length -gt 80) { $label = $label.Substring(0, 80) }
  Write-Host "[$i/$($batches.Count)] $label" -ForegroundColor Cyan

  $cmd = $c.CreateCommand()
  $cmd.CommandText = $batch
  $cmd.CommandTimeout = $BatchTimeoutSec
  try {
    [void]$cmd.ExecuteNonQuery()
  } catch {
    Write-Host "  ERROR: $($_.Exception.Message)" -ForegroundColor Red
  }

  # Always reset stuck IDENTITY_INSERT
  $reset = $c.CreateCommand()
  $reset.CommandText = $resetSql
  try { [void]$reset.ExecuteNonQuery() } catch { }
}

$c.Close()
Write-Host "==== Done ====" -ForegroundColor Cyan
