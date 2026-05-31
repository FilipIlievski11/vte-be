# Parse a DevExpress .prnx (already-decompressed .xml) and emit absolute
# positions in millimetres for every Label/CheckBox brick, folding panel
# offsets so child bricks land at their true page coordinates.
#
# Usage:
#   powershell -File parse-prnx.ps1 -Xml C:\path\to\PrintPlav.xml
param(
  [Parameter(Mandatory=$true)][string]$Xml
)

# 300 DPI: A4 2481x3507 px = 210x297 mm  ->  1 mm = 11.811 px
$PX_PER_MM = 11.811

[xml]$doc = Get-Content -Raw -Encoding UTF8 $Xml

function Conv([double]$px) { return [math]::Round($px / $PX_PER_MM, 1) }

# Recurse a node's children, accumulating panel offset (ox, oy) in px.
function Walk($node, [double]$ox, [double]$oy, [int]$page) {
  foreach ($child in $node.ChildNodes) {
    if ($child.NodeType -ne 'Element') { continue }
    $bt = $child.GetAttribute('BrickType')
    $rect = $child.GetAttribute('Rect')
    if (-not $rect) { continue }
    $parts = $rect.Split(',')
    if ($parts.Count -lt 4) { continue }
    $rx = [double]$parts[0]; $ry = [double]$parts[1]
    $rw = [double]$parts[2]; $rh = [double]$parts[3]
    $absX = $ox + $rx; $absY = $oy + $ry

    if ($bt -eq 'Panel') {
      # Descend into panel; its <Bricks> children are relative to panel origin.
      $bricks = $child.SelectSingleNode('Bricks')
      if ($bricks) { Walk $bricks $absX $absY $page }
      $inner = $child.SelectSingleNode('InnerBricks')
      if ($inner) { Walk $inner $absX $absY $page }
      continue
    }

    if ($bt -eq 'Label' -or $bt -eq 'CheckBoxText' -or $bt -eq 'PageInfoText') {
      $text = $child.GetAttribute('Text')
      $idx  = $child.GetAttribute('Index')
      $style= $child.GetAttribute('Style')
      $check= $child.GetAttribute('CheckState')
      [PSCustomObject]@{
        Page  = $page
        Index = $idx
        Type  = $bt
        Style = $style
        Check = $check
        X_mm  = Conv $absX
        Y_mm  = Conv $absY
        W_mm  = Conv $rw
        H_mm  = Conv $rh
        Text  = $text
      }
    }

    # A non-panel might still contain nested bricks (rare) — descend anyway.
    if ($child.HasChildNodes) {
      $inner = $child.SelectSingleNode('InnerBricks')
      if ($inner) { Walk $inner $absX $absY $page }
    }
  }
}

$rows = @()
foreach ($pageNode in $doc.PreviewSerializer.ChildNodes) {
  if ($pageNode.Name -notmatch '^Page(\d+)$') { continue }
  $pageNum = [int]$Matches[1]
  $inner = $pageNode.SelectSingleNode('InnerBricks')
  if ($inner) {
    $sub = New-Object System.Collections.ArrayList
    # The outer Item1 is a full-page wrapper panel at 0,0 — descend through it.
    Walk $inner 0 0 $pageNum | ForEach-Object { [void]$sub.Add($_) }
    $rows += $sub
  }
}

$rows | Sort-Object Page, Y_mm, X_mm |
  Format-Table Page, Index, Type, Style, Check, X_mm, Y_mm, W_mm, H_mm, Text -AutoSize -Wrap
