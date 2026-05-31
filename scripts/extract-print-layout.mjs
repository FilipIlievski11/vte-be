// One-shot extractor: legacy DevExpress XtraReport Designer.vb files →
// JSON layout manifests we can use to build pixel-perfect Vue templates.
//
// Usage:
//   node scripts/extract-print-layout.mjs
//
// Output:
//   scripts/print-layout-plav.json
//   scripts/print-layout-bel.json
//   scripts/print-layout-zelen.json
//
// Each control's coordinates are kept in their native 0.1mm units (Dpi=254).
// Convert to CSS millimeters by dividing by 10 when rendering.

import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const __dirname = path.dirname(fileURLToPath(import.meta.url));
const ROOT = path.resolve(__dirname, '..');

const sources = [
  { code: 'plav',  file: 'WinApp/Stampa/PrintRequests/rptPlav.Designer.vb' },
  { code: 'bel',   file: 'WinApp/Stampa/PrintRequests/rptBel.Designer.vb' },
  { code: 'zelen', file: 'WinApp/Stampa/PrintRequests/rptZelen.Designer.vb' },
];

/**
 * Parse a Designer.vb file. We walk line-by-line collecting blocks per control:
 *
 *   'ControlName
 *   '
 *   Me.ControlName.Prop = value
 *   Me.ControlName.Prop = value
 *
 * After each block we capture the most useful properties.
 *
 * Critical caveat: DevExpress XtraReports controls nested inside an XRPanel use
 * positions RELATIVE to the panel's top-left, not absolute on the page. We
 * detect panel parents from `Me.XrPanel2.Controls.AddRange(...)` lines and
 * fold the parent's offset into each child's position post-parse.
 */
function parseDesigner(vbText) {
  const lines = vbText.split(/\r?\n/);
  /** @type {Record<string, any>} */
  const controls = {};
  let pageSize = null;
  /** @type {Record<string, string>}  child name → panel name */
  const parentOfChild = {};

  // Helper: append property to a named control.
  function setProp(name, key, value) {
    if (!controls[name]) controls[name] = { name };
    controls[name][key] = value;
  }

  // First pass: extract parent → children mapping from `Controls.AddRange` calls.
  // These can span multiple lines, so we work with the joined text and pull
  // each block out via regex.
  const joined = vbText.replace(/\r?\n/g, ' ');
  const addRangeRe = /Me\.(\w+)\.Controls\.AddRange\s*\(\s*New\s+DevExpress\.XtraReports\.UI\.XRControl\(\)\s*\{([^}]*)\}\s*\)/g;
  let m;
  while ((m = addRangeRe.exec(joined)) !== null) {
    const parent = m[1];
    // Skip the Detail band — its children ARE absolutely positioned at design time.
    if (/^Detail/i.test(parent)) continue;
    const children = m[2].split(',').map(s => s.trim()).filter(Boolean);
    for (const c of children) {
      const childMatch = c.match(/Me\.(\w+)/);
      if (childMatch) parentOfChild[childMatch[1]] = parent;
    }
  }

  for (let i = 0; i < lines.length; i++) {
    const line = lines[i].trim();
    if (!line.startsWith('Me.')) continue;

    // Skip the outer Me.X = New ... declarations (single line) that just
    // instantiate controls — we want assignments to .Property.
    // Pattern: Me.Name.Property = value
    const propMatch = line.match(/^Me\.([\w]+)\.([\w]+)\s*=\s*(.*)$/);
    if (!propMatch) continue;
    const [, name, prop, rawValue] = propMatch;

    if (prop === 'Location') {
      // New System.Drawing.Point(X, Y)
      const m = rawValue.match(/Point\((-?\d+),\s*(-?\d+)\)/);
      if (m) {
        setProp(name, 'x', parseInt(m[1], 10));
        setProp(name, 'y', parseInt(m[2], 10));
      }
    } else if (prop === 'Size') {
      // New System.Drawing.Size(W, H)
      const m = rawValue.match(/Size\((-?\d+),\s*(-?\d+)\)/);
      if (m) {
        setProp(name, 'w', parseInt(m[1], 10));
        setProp(name, 'h', parseInt(m[2], 10));
      }
    } else if (prop === 'Text') {
      // "literal" — strip outer quotes and N""-style empties
      const m = rawValue.match(/^"(.*)"$/s);
      if (m) {
        setProp(name, 'text', m[1]);
      }
    } else if (prop === 'TextAlignment') {
      // DevExpress.XtraPrinting.TextAlignment.MiddleLeft → "MiddleLeft"
      const m = rawValue.match(/TextAlignment\.(\w+)/);
      if (m) setProp(name, 'align', m[1]);
    } else if (prop === 'Visible') {
      setProp(name, 'visible', rawValue === 'True');
    } else if (prop === 'Multiline') {
      setProp(name, 'multiline', rawValue === 'True');
    } else if (prop === 'WordWrap') {
      setProp(name, 'wordWrap', rawValue === 'True');
    } else if (prop === 'Checked') {
      setProp(name, 'checked', rawValue === 'True');
    } else if (prop === 'Font') {
      // Font is typically: New System.Drawing.Font("Arial", 10.0!, FontStyle.Bold, ...)
      const fam = rawValue.match(/Font\("([^"]+)",\s*([\d.]+)!?/);
      if (fam) {
        setProp(name, 'fontFamily', fam[1]);
        setProp(name, 'fontSize', parseFloat(fam[2]));
      }
      if (rawValue.includes('FontStyle.Bold')) setProp(name, 'bold', true);
      if (rawValue.includes('FontStyle.Italic')) setProp(name, 'italic', true);
      if (rawValue.includes('FontStyle.Underline')) setProp(name, 'underline', true);
    } else if (prop === 'BorderWidth') {
      setProp(name, 'borderWidth', parseFloat(rawValue.replace('!', '')));
    } else if (prop === 'Borders') {
      // DevExpress.XtraPrinting.BorderSide.All
      const m = rawValue.match(/BorderSide\.(\w+)/);
      if (m) setProp(name, 'borders', m[1]);
    } else if (prop === 'BackColor') {
      setProp(name, 'backColor', rawValue);
    } else if (prop === 'ForeColor') {
      setProp(name, 'foreColor', rawValue);
    } else if (prop === 'BorderColor') {
      setProp(name, 'borderColor', rawValue);
    } else if (prop === 'Name') {
      // Sanity check
      const m = rawValue.match(/^"([^"]+)"$/);
      if (m) setProp(name, 'declaredName', m[1]);
    } else if (prop === 'CheckBoxFont' || prop === 'Padding' || prop === 'DataBindings' || prop === 'Dpi' || prop === 'StylePriority') {
      // ignore noisy props
    }

    // Page size lives on the XtraReport class itself (no Me.Name. prefix —
    // it's on whatever the partial class is named, captured as the root).
  }

  // Page properties show up on the root class — pick them up.
  const pageWidth  = /Me\.PageWidth\s*=\s*(\d+)/.exec(vbText);
  const pageHeight = /Me\.PageHeight\s*=\s*(\d+)/.exec(vbText);
  if (pageWidth && pageHeight) {
    pageSize = { w: parseInt(pageWidth[1], 10), h: parseInt(pageHeight[1], 10) };
  }

  // Fold parent-panel offsets into each nested child's position. Handles
  // chains (panel-in-panel) by walking up until we hit the root. Records the
  // parent name so the manifest stays inspectable.
  function offsetOf(name) {
    const parent = parentOfChild[name];
    if (!parent || !controls[parent]) return { x: 0, y: 0 };
    const o = offsetOf(parent);
    return {
      x: o.x + (controls[parent].x ?? 0),
      y: o.y + (controls[parent].y ?? 0),
    };
  }
  for (const name of Object.keys(controls)) {
    if (parentOfChild[name]) {
      const off = offsetOf(name);
      controls[name].parent = parentOfChild[name];
      if (controls[name].x !== undefined) controls[name].x += off.x;
      if (controls[name].y !== undefined) controls[name].y += off.y;
    }
  }

  return { pageSize, controls };
}

/** Pretty-print + classify controls so the JSON is easy to scan by eye. */
function summarize(parsed) {
  const list = Object.values(parsed.controls).filter(c => c.x !== undefined);
  // Sort top-to-bottom, then left-to-right — same order an operator reads.
  list.sort((a, b) => (a.y - b.y) || (a.x - b.x));
  const byKind = {
    labels: list.filter(c => /XrLabel|^lbl/.test(c.name)),
    checkboxes: list.filter(c => /XrCheckBox|^CheckBox/.test(c.name)),
    panels: list.filter(c => /XrPanel|panel/.test(c.name)),
    lines: list.filter(c => /XrLine|line/i.test(c.name)),
    other: list.filter(c =>
      !/XrLabel|^lbl|XrCheckBox|^CheckBox|XrPanel|panel|XrLine|line/i.test(c.name)),
  };
  return {
    page: parsed.pageSize,
    total: list.length,
    counts: Object.fromEntries(Object.entries(byKind).map(([k, v]) => [k, v.length])),
    controls: list,
  };
}

for (const { code, file } of sources) {
  const abs = path.join(ROOT, file);
  if (!fs.existsSync(abs)) {
    console.error(`!! ${abs} not found`);
    continue;
  }
  const text = fs.readFileSync(abs, 'utf8');
  const parsed = parseDesigner(text);
  const summary = summarize(parsed);
  const outPath = path.join(__dirname, `print-layout-${code}.json`);
  fs.writeFileSync(outPath, JSON.stringify(summary, null, 2), 'utf8');
  console.log(
    `  ${code.padEnd(6)} page ${summary.page?.w}×${summary.page?.h} (0.1mm) — ` +
    `${summary.total} controls (${Object.entries(summary.counts).map(([k, v]) => `${v} ${k}`).join(', ')}) → ${path.relative(ROOT, outPath)}`);
}
