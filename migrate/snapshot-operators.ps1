# Pull legacy vtesecurity.Users into VTEZVV_Snapshot.LegacyUsers
param(
  [string]$Source = "Server=195.26.159.162,7899;Database=vtesecurity;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=True;Encrypt=False;Connection Timeout=30;",
  [string]$Target = "Server=(localdb)\MSSQLLocalDB;Database=VTEZVV_Snapshot;Integrated Security=True;TrustServerCertificate=True;"
)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Data

$createSql = @"
IF OBJECT_ID('dbo.LegacyUsers', 'U') IS NOT NULL DROP TABLE dbo.LegacyUsers;
CREATE TABLE dbo.LegacyUsers(
  Id BIGINT NOT NULL,
  IdRole INT NULL, IdStation INT NULL,
  UserFullName NVARCHAR(200) NULL,
  UserName NVARCHAR(200) NULL,
  FirstName NVARCHAR(200) NULL,
  SureName NVARCHAR(200) NULL,
  EMBG NVARCHAR(20) NULL,
  Active BIT NULL
);
"@
$tc = New-Object System.Data.SqlClient.SqlConnection $Target; $tc.Open()
$cmd = $tc.CreateCommand(); $cmd.CommandText = $createSql; [void]$cmd.ExecuteNonQuery()

$sc = New-Object System.Data.SqlClient.SqlConnection $Source; $sc.Open()
$reader = $sc.CreateCommand()
$reader.CommandText = @"
SELECT ID, IdRole, IdStation,
  UserFullName COLLATE Macedonian_FYROM_90_CI_AS AS UserFullName,
  UserName     COLLATE Macedonian_FYROM_90_CI_AS AS UserName,
  FirstName    COLLATE Macedonian_FYROM_90_CI_AS AS FirstName,
  SureName     COLLATE Macedonian_FYROM_90_CI_AS AS SureName,
  CAST(EMBG AS NVARCHAR(20)) COLLATE Macedonian_FYROM_90_CI_AS AS EMBG,
  Active
FROM dbo.Users
"@
$rdr = $reader.ExecuteReader()

$bulk = New-Object System.Data.SqlClient.SqlBulkCopy($Target, [System.Data.SqlClient.SqlBulkCopyOptions]::TableLock)
$bulk.DestinationTableName = "dbo.LegacyUsers"
$bulk.BulkCopyTimeout = 0
[void]$bulk.ColumnMappings.Add('ID','Id')
foreach ($c in @('IdRole','IdStation','UserFullName','UserName','FirstName','SureName','EMBG','Active')) {
  [void]$bulk.ColumnMappings.Add($c, $c)
}
$bulk.WriteToServer($rdr)
$rdr.Close(); $bulk.Close(); $sc.Close()

$count = $tc.CreateCommand(); $count.CommandText = "SELECT COUNT(*) FROM LegacyUsers"
Write-Host "LegacyUsers imported: $($count.ExecuteScalar())" -ForegroundColor Green
$tc.Close()
