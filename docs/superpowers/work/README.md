# Working files for the VTE Domain & Schema Audit

This folder contains derivative working files used during the audit. They are NOT the audit deliverable. The deliverable is `docs/superpowers/specs/2026-04-30-vte-domain-audit.md`.

## SAFETY — DO NOT EXECUTE THESE SQL FILES

`sqlData.utf8.sql` and `emSecurity.utf8.sql` are UTF-8 conversions of the legacy SQL files in `WinApp/`. They are exact copies of the original content, only re-encoded.

The legacy files contain DESTRUCTIVE DDL at the top:

- Line 3-4 of both files: `IF EXISTS (...) DROP DATABASE [...]`
- Line 10: `CREATE DATABASE ...`

If piped into `sqlcmd`, `Invoke-Sqlcmd`, or any SQL Server client against a live database server, **these scripts will drop and recreate databases**. Never do that.

These files are read-only documentation for the audit. Use only text utilities (grep, awk, sed, diff, iconv) on them. The migration to the new system must be built as its own idempotent, verified script — not by running these legacy exports.

## Other contents

- `blocks/` — per-module markdown files containing extracted CREATE TABLE blocks (used as input for the audit doc merge).
- `blocks-procs/` — per-module markdown files containing the proc inventory.
- `procs.tsv`, `procs-classified.tsv`, `views.tsv` — intermediate tab-separated lists used by the classification scripts.

These can all be regenerated from `sqlData.utf8.sql` and the locked-in module assignments in the audit plan.
