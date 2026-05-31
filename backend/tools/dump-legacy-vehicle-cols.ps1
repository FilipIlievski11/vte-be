# Lists every column on the legacy VTEZVV.dbo.Vehicles table.
# Need to confirm names + types for the missing fields the print templates bind to.
$dll = Get-ChildItem "$env:USERPROFILE\.nuget\packages\microsoft.data.sqlclient" -Recurse -Filter Microsoft.Data.SqlClient.dll -ErrorAction SilentlyContinue |
       Where-Object { $_.FullName -match "lib\\net[89]" } |
       Select-Object -First 1
Add-Type -Path $dll.FullName

$cs   = "Server=195.26.159.162,7899;Database=VTEZVV;User Id=testapp1;Password=2_Snegot;TrustServerCertificate=true;Encrypt=False"
$conn = New-Object Microsoft.Data.SqlClient.SqlConnection $cs
$conn.Open()
$cmd  = $conn.CreateCommand()
$cmd.CommandText = @"
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME='Vehicles' ORDER BY ORDINAL_POSITION
"@
$rdr  = $cmd.ExecuteReader()
while ($rdr.Read()) {
    $name = $rdr.GetString(0)
    $type = $rdr.GetString(1)
    $len  = if ($rdr.IsDBNull(2)) { '' } else { "($($rdr.GetInt32(2)))" }
    $prec = if ($rdr.IsDBNull(3)) { '' } else { "($($rdr.GetByte(3)),$($rdr.GetInt32(4)))" }
    "$name`t$type$len$prec"
}
$rdr.Close()
$conn.Close()
