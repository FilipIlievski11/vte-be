# Re-pull legacy PaymentDocumentsDetails (snapshot was truncated at ~590k of 1.15M rows)
param(
  [string]$Source = "Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;",
  [string]$Target = "Server=(localdb)\MSSQLLocalDB;Database=VTEZVV_Snapshot;Integrated Security=True;TrustServerCertificate=True;"
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data

$tc = New-Object System.Data.SqlClient.SqlConnection $Target; $tc.Open()
$cmd = $tc.CreateCommand(); $cmd.CommandText = "TRUNCATE TABLE dbo.PaymentDocumentsDetails;"
[void]$cmd.ExecuteNonQuery()

# Pull all rows (no batching; SqlBulkCopy streams). Increase command timeout to forever.
$sc = New-Object System.Data.SqlClient.SqlConnection $Source; $sc.Open()
$reader = $sc.CreateCommand()
$reader.CommandText = "SELECT * FROM dbo.PaymentDocumentsDetails WITH (NOLOCK)"
$reader.CommandTimeout = 0
$rdr = $reader.ExecuteReader()

$opts = [System.Data.SqlClient.SqlBulkCopyOptions]::TableLock -bor [System.Data.SqlClient.SqlBulkCopyOptions]::KeepIdentity
$bulk = New-Object System.Data.SqlClient.SqlBulkCopy($Target, $opts)
$bulk.DestinationTableName = "dbo.PaymentDocumentsDetails"
$bulk.BatchSize = 10000
$bulk.BulkCopyTimeout = 0

# Map columns by ordinal
for ($i = 0; $i -lt $rdr.FieldCount; $i++) {
  $name = $rdr.GetName($i)
  [void]$bulk.ColumnMappings.Add($name, $name)
}

$tStart = Get-Date
$bulk.WriteToServer($rdr)
$rdr.Close(); $bulk.Close(); $sc.Close()
$elapsed = ((Get-Date) - $tStart).TotalSeconds

$count = $tc.CreateCommand(); $count.CommandText = "SELECT COUNT_BIG(*) FROM PaymentDocumentsDetails"
$total = $count.ExecuteScalar()
Write-Host ("PaymentDocumentsDetails re-pulled: {0:N0} rows in {1:N1}s" -f $total, $elapsed) -ForegroundColor Green
$tc.Close()
