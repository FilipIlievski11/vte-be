# Parses a legacy DevExpress XtraReport Designer.vb file and emits a JSON layout.
# Coordinates are in TenthsOfAMillimeter (legacy ReportUnit) -> we keep them as-is.
# Run with: pwsh parse-print-designer.ps1 -InputFile <path.Designer.vb> -OutputFile <path.json>

param(
  [Parameter(Mandatory=$true)] [string]$InputFile,
  [Parameter(Mandatory=$true)] [string]$OutputFile
)

$ErrorActionPreference = 'Stop'
$lines = Get-Content -LiteralPath $InputFile -Encoding UTF8

# control map keyed by name. Each value: hashtable of properties.
$controls = [ordered]@{}
$current = $null

# Page-level
$page = @{ width=2101; height=2969; unit='TenthsOfAMillimeter'; paperKind='A4' }

# Regexes
$rxNameComment = '^\s*''([A-Za-z][A-Za-z0-9_]*)\s*$'
$rxLocation    = '^\s*Me\.([A-Za-z0-9_]+)\.Location\s*=\s*New\s+System\.Drawing\.Point\((-?\d+),\s*(-?\d+)\)'
$rxSize        = '^\s*Me\.([A-Za-z0-9_]+)\.Size\s*=\s*New\s+System\.Drawing\.Size\((-?\d+),\s*(-?\d+)\)'
$rxLocationF   = '^\s*Me\.([A-Za-z0-9_]+)\.LocationFloat\s*=\s*New\s+DevExpress\.Utils\.PointFloat\((-?[\d\.]+)!?,\s*(-?[\d\.]+)!?\)'
$rxSizeF       = '^\s*Me\.([A-Za-z0-9_]+)\.SizeF\s*=\s*New\s+System\.Drawing\.SizeF\((-?[\d\.]+)!?,\s*(-?[\d\.]+)!?\)'
$rxText        = '^\s*Me\.([A-Za-z0-9_]+)\.Text\s*=\s*"((?:[^"]|"")*)"\s*$'
$rxVisible     = '^\s*Me\.([A-Za-z0-9_]+)\.Visible\s*=\s*(True|False)'
$rxMultiline   = '^\s*Me\.([A-Za-z0-9_]+)\.Multiline\s*=\s*(True|False)'
$rxFont        = '^\s*Me\.([A-Za-z0-9_]+)\.Font\s*=\s*New\s+System\.Drawing\.Font\("([^"]+)",\s*([\d\.]+)!?,\s*System\.Drawing\.FontStyle\.([A-Za-z]+)'
$rxAlign       = '^\s*Me\.([A-Za-z0-9_]+)\.TextAlignment\s*=\s*DevExpress\.XtraPrinting\.TextAlignment\.(\w+)'
$rxBorders     = '^\s*Me\.([A-Za-z0-9_]+)\.Borders\s*=\s*(.+)$'
$rxBinding     = '^\s*Me\.([A-Za-z0-9_]+)\.DataBindings\.AddRange\(New\s+DevExpress\.XtraReports\.UI\.XRBinding\(\)\s*\{New\s+DevExpress\.XtraReports\.UI\.XRBinding\("Text",\s*Nothing,\s*"([^"]+)"'
$rxChildren    = '^\s*Me\.([A-Za-z0-9_]+)\.Controls\.AddRange\(New\s+DevExpress\.XtraReports\.UI\.XRControl\(\)\s*\{(.+?)\}\)'
$rxPageH       = '^\s*Me\.PageHeight\s*=\s*(\d+)'
$rxPageW       = '^\s*Me\.PageWidth\s*=\s*(\d+)'
$rxRptUnit     = '^\s*Me\.ReportUnit\s*=\s*DevExpress\.XtraReports\.UI\.ReportUnit\.(\w+)'

function Ensure-Control([string]$name) {
  if (-not $controls.Contains($name)) {
    $controls[$name] = [ordered]@{
      name=$name; type=$null; x=$null; y=$null; w=$null; h=$null;
      text=$null; binding=$null; visible=$true; multiline=$false;
      fontName=$null; fontSize=$null; fontStyle='Regular';
      align=$null; borders=$null; children=@(); parent=$null;
    }
  }
  return $controls[$name]
}

foreach ($line in $lines) {
  if ($line -match $rxPageH) { $page.height = [int]$matches[1]; continue }
  if ($line -match $rxPageW) { $page.width  = [int]$matches[1]; continue }
  if ($line -match $rxRptUnit){ $page.unit  = $matches[1]; continue }

  if ($line -match $rxLocation)  { $c = Ensure-Control $matches[1]; $c.x = [int]$matches[2]; $c.y = [int]$matches[3]; continue }
  if ($line -match $rxSize)      { $c = Ensure-Control $matches[1]; $c.w = [int]$matches[2]; $c.h = [int]$matches[3]; continue }
  if ($line -match $rxLocationF) { $c = Ensure-Control $matches[1]; $c.x = [double]$matches[2]; $c.y = [double]$matches[3]; continue }
  if ($line -match $rxSizeF)     { $c = Ensure-Control $matches[1]; $c.w = [double]$matches[2]; $c.h = [double]$matches[3]; continue }
  if ($line -match $rxText)      { $c = Ensure-Control $matches[1]; $c.text = $matches[2] -replace '""','"'; continue }
  if ($line -match $rxVisible)   { $c = Ensure-Control $matches[1]; $c.visible = ($matches[2] -eq 'True'); continue }
  if ($line -match $rxMultiline) { $c = Ensure-Control $matches[1]; $c.multiline = ($matches[2] -eq 'True'); continue }
  if ($line -match $rxFont)      { $c = Ensure-Control $matches[1]; $c.fontName = $matches[2]; $c.fontSize = [double]$matches[3]; $c.fontStyle = $matches[4]; continue }
  if ($line -match $rxAlign)     { $c = Ensure-Control $matches[1]; $c.align = $matches[2]; continue }
  if ($line -match $rxBorders)   { $c = Ensure-Control $matches[1]; $c.borders = $matches[2].Trim(); continue }
  if ($line -match $rxBinding)   { $c = Ensure-Control $matches[1]; $c.binding = $matches[2]; continue }
  if ($line -match $rxChildren) {
    $parent = Ensure-Control $matches[1]
    $kids = $matches[2] -split ',\s*Me\.' | ForEach-Object { ($_ -replace '^\s*Me\.','').Trim() }
    foreach ($k in $kids) { if ($k) { $parent.children += $k; $kid = Ensure-Control $k; $kid.parent = $matches[1] } }
    continue
  }
}

# Type inference: anything with children = Panel, anything with text="" + binding = Bound, else Label/Checkbox
foreach ($name in $controls.Keys) {
  $c = $controls[$name]
  if ($c.children.Count -gt 0) { $c.type = 'Panel' }
  elseif ($name -match 'CheckBox') { $c.type = 'CheckBox' }
  else { $c.type = 'Label' }
}

$result = [ordered]@{ page = $page; controls = $controls.Values }
$json = $result | ConvertTo-Json -Depth 6 -Compress:$false
Set-Content -LiteralPath $OutputFile -Value $json -Encoding UTF8
Write-Output "wrote $($controls.Count) controls -> $OutputFile"
