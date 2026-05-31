# Snapshots every table from production VTEZVV (195.26.159.162,7899) into a fresh local
# LocalDB database VTEZVV_Snapshot, schema 1:1. Uses SqlBulkCopy for speed.
# Idempotent: drops & recreates VTEZVV_Snapshot every run.

param(
  [string]$SourceConn = "Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;",
  [string]$TargetConn = "Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;TrustServerCertificate=True;",
  [string]$SnapshotDb = "VTEZVV_Snapshot",
  [string[]]$ExcludeTables = @()
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data

function Invoke-Scalar {
  param([string]$Conn, [string]$Sql)
  $c = New-Object System.Data.SqlClient.SqlConnection $Conn
  $c.Open()
  try {
    $cmd = $c.CreateCommand(); $cmd.CommandText = $Sql; $cmd.CommandTimeout = 300
    return $cmd.ExecuteScalar()
  } finally { $c.Close() }
}

function Invoke-NonQuery {
  param([string]$Conn, [string]$Sql)
  $c = New-Object System.Data.SqlClient.SqlConnection $Conn
  $c.Open()
  try {
    $cmd = $c.CreateCommand(); $cmd.CommandText = $Sql; $cmd.CommandTimeout = 600
    [void]$cmd.ExecuteNonQuery()
  } finally { $c.Close() }
}

function Get-DataTable {
  param([string]$Conn, [string]$Sql)
  $c = New-Object System.Data.SqlClient.SqlConnection $Conn
  $c.Open()
  try {
    $cmd = $c.CreateCommand(); $cmd.CommandText = $Sql; $cmd.CommandTimeout = 300
    $da = New-Object System.Data.SqlClient.SqlDataAdapter $cmd
    $dt = New-Object System.Data.DataTable
    [void]$da.Fill($dt)
    return ,$dt
  } finally { $c.Close() }
}

$snapExists = Invoke-Scalar $TargetConn "SELECT CASE WHEN DB_ID('$SnapshotDb') IS NULL THEN 0 ELSE 1 END"
if ($snapExists -eq 0) {
  Write-Host "==== Creating local snapshot DB [$SnapshotDb] ====" -ForegroundColor Cyan
  Invoke-NonQuery $TargetConn "CREATE DATABASE [$SnapshotDb] COLLATE Macedonian_FYROM_90_CI_AS;"
} else {
  Write-Host "==== Resuming into existing [$SnapshotDb] ====" -ForegroundColor Cyan
}

$snapTargetConn = $TargetConn -replace 'Database=master', "Database=$SnapshotDb"

# 1. Get list of tables from source
$tableQuery = "SELECT TABLE_SCHEMA AS TS, TABLE_NAME AS TN FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME"
$tables = Get-DataTable $SourceConn $tableQuery
Write-Host "Found $($tables.Rows.Count) tables in source." -ForegroundColor Cyan
Write-Host "Columns: $(($tables.Columns | ForEach-Object { $_.ColumnName }) -join ', ')" -ForegroundColor DarkCyan

# 2. For each table: create in snapshot, then bulk copy
$totalRows = 0
$startedAt = Get-Date

for ($i = 0; $i -lt $tables.Rows.Count; $i++) {
  $row = $tables.Rows[$i]
  $schema = [string]$row.ItemArray[0]
  $table  = [string]$row.ItemArray[1]
  if ([string]::IsNullOrWhiteSpace($table)) { continue }
  if ($ExcludeTables -contains $table) { Write-Host "  SKIP $table" -ForegroundColor DarkGray; continue }
  $fq = "[$schema].[$table]"

  # Skip tables already populated
  $existsAndPop = Invoke-Scalar $snapTargetConn "
    IF OBJECT_ID('$fq') IS NULL SELECT 0
    ELSE SELECT (SELECT COUNT_BIG(*) FROM $fq)
  "
  $srcCount = Invoke-Scalar $SourceConn "SELECT COUNT_BIG(*) FROM $fq WITH (NOLOCK)"
  if ($existsAndPop -ge $srcCount -and $srcCount -gt 0) {
    Write-Host ("  {0,-50} {1,10:N0} rows ALREADY (skip)" -f $table, $existsAndPop) -ForegroundColor DarkGray
    continue
  }
  # If table exists but row count mismatched, drop and reload
  if ((Invoke-Scalar $snapTargetConn "SELECT CASE WHEN OBJECT_ID('$fq') IS NULL THEN 0 ELSE 1 END") -eq 1) {
    Invoke-NonQuery $snapTargetConn "DROP TABLE $fq"
  }

  # Get column definitions from source
  $colSql = @"
SELECT
  COLUMN_NAME,
  DATA_TYPE,
  CHARACTER_MAXIMUM_LENGTH,
  NUMERIC_PRECISION,
  NUMERIC_SCALE,
  IS_NULLABLE,
  COLUMN_DEFAULT,
  COLUMNPROPERTY(OBJECT_ID('$schema.$table'), COLUMN_NAME, 'IsIdentity') AS IsIdentity,
  COLLATION_NAME
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA='$schema' AND TABLE_NAME='$table'
ORDER BY ORDINAL_POSITION
"@
  $cols = Get-DataTable $SourceConn $colSql

  $colDefs = @()
  $hasIdentity = $false
  for ($j = 0; $j -lt $cols.Rows.Count; $j++) {
    $c = $cols.Rows[$j]
    $colName  = [string]$c.ItemArray[0]
    $dt       = [string]$c.ItemArray[1]
    $maxLen   = $c.ItemArray[2]
    $prec     = $c.ItemArray[3]
    $scale    = $c.ItemArray[4]
    $isNullVal = [string]$c.ItemArray[5]
    $isIdVal  = $c.ItemArray[7]
    $collation = [string]$c.ItemArray[8]
    $nullable = if ($isNullVal -eq 'YES') { 'NULL' } else { 'NOT NULL' }
    $isIdentity = if ($isIdVal -eq 1) { ' IDENTITY(1,1)' } else { '' }
    if ($isIdVal -eq 1) { $hasIdentity = $true }

    $typeSpec = switch ($dt) {
      'nvarchar'   { if ($maxLen -eq -1) { 'nvarchar(max)' } else { "nvarchar($maxLen)" } }
      'varchar'    { if ($maxLen -eq -1) { 'varchar(max)' }  else { "varchar($maxLen)" } }
      'nchar'      { "nchar($maxLen)" }
      'char'       { "char($maxLen)" }
      'varbinary'  { if ($maxLen -eq -1) { 'varbinary(max)' } else { "varbinary($maxLen)" } }
      'binary'     { "binary($maxLen)" }
      'decimal'    { "decimal($prec,$scale)" }
      'numeric'    { "numeric($prec,$scale)" }
      'datetime2'  { "datetime2($scale)" }
      'datetimeoffset' { "datetimeoffset($scale)" }
      'time'       { "time($scale)" }
      'timestamp'  { 'binary(8)' } # rowversion not copyable; store raw bytes
      default      { $dt }
    }
    $collateSpec = ''
    if ($dt -in 'nvarchar','varchar','nchar','char','text','ntext' -and ![string]::IsNullOrEmpty($collation)) {
      $collateSpec = " COLLATE $collation"
    }
    $colDefs += "  [$colName] $typeSpec$collateSpec$isIdentity $nullable"
  }

  $createSql = "CREATE TABLE $fq (`r`n" + ($colDefs -join ",`r`n") + "`r`n)"
  try {
    Invoke-NonQuery $snapTargetConn $createSql
  } catch {
    Write-Host "FAILED CREATE for ${fq}:" -ForegroundColor Red
    Write-Host $createSql -ForegroundColor Yellow
    throw
  }

  # Open reader on source, bulk copy to snapshot
  $rowCount = Invoke-Scalar $SourceConn "SELECT COUNT_BIG(*) FROM $fq WITH (NOLOCK)"
  if ($rowCount -eq 0) {
    Write-Host ("  {0,-50} 0 rows" -f $table) -ForegroundColor DarkGray
    continue
  }

  $colList = ($cols.Rows | ForEach-Object { "[$($_.COLUMN_NAME)]" }) -join ", "
  $srcConn2 = New-Object System.Data.SqlClient.SqlConnection $SourceConn
  $srcConn2.Open()
  try {
    $cmd = $srcConn2.CreateCommand()
    $cmd.CommandText = "SELECT $colList FROM $fq WITH (NOLOCK)"
    $cmd.CommandTimeout = 0
    $reader = $cmd.ExecuteReader()

    $bulkOpts = [System.Data.SqlClient.SqlBulkCopyOptions]::TableLock -bor [System.Data.SqlClient.SqlBulkCopyOptions]::KeepIdentity
    $bulk = New-Object System.Data.SqlClient.SqlBulkCopy($snapTargetConn, $bulkOpts)
    $bulk.DestinationTableName = $fq
    $bulk.BatchSize = 5000
    $bulk.BulkCopyTimeout = 0
    foreach ($c in $cols.Rows) {
      [void]$bulk.ColumnMappings.Add($c.COLUMN_NAME, $c.COLUMN_NAME)
    }
    $tStart = Get-Date
    try {
      $bulk.WriteToServer($reader)
      $elapsed = ((Get-Date) - $tStart).TotalSeconds
      $rate = if ($elapsed -gt 0) { [int]($rowCount / $elapsed) } else { $rowCount }
      Write-Host ("  {0,-50} {1,10:N0} rows in {2,6:N1}s ({3:N0}/s)" -f $table, $rowCount, $elapsed, $rate) -ForegroundColor Green
      $totalRows += $rowCount
    } catch {
      Write-Host ("  {0,-50} FAILED bulk copy: {1}" -f $table, $_.Exception.Message) -ForegroundColor Red
    } finally {
      $reader.Close()
      $bulk.Close()
    }
  } finally {
    $srcConn2.Close()
  }
}

$totalElapsed = ((Get-Date) - $startedAt).TotalMinutes
Write-Host ""
Write-Host ("==== Snapshot complete: {0:N0} rows in {1:N1} min ====" -f $totalRows, $totalElapsed) -ForegroundColor Cyan
