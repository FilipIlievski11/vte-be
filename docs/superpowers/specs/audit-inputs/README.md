# audit-inputs

Companion folder for the VTE Domain & Schema Audit
(`docs/superpowers/specs/2026-04-30-vte-domain-audit.md`).

## What goes here (committable)

- `app.config.redacted.xml` — a redacted production `app.config` from any
  station. All encrypted connection-string values must be replaced with the
  literal string `«REDACTED — see audit-inputs/README»`. The structure of the
  file (XML elements, attribute names, encryption indicators) is preserved.
- Redacted printed samples (PDF or image) of each official document:
  - `print-sample-traffic-licence-extension.*`
  - `print-sample-request-blue.*` (Plav)
  - `print-sample-request-white.*` (Bel)
  - `print-sample-request-green.*` (Zelen)
  - `print-sample-payment-receipt.*`
  - `print-sample-invoice-faktura.*`
  - `print-sample-cash-report-kasov-izvestaj.*`
  Personal data (names, ID numbers, plate numbers, addresses) redacted.

## What does NOT go here (NEVER committed)

- Raw encrypted `app.config` (it's a credential).
- Encryption key, salt, or passphrase used by the `Crypt` /
  `RijndaelCryptography` class.
- Production database backups or extracts.
- Customer-identifying data of any kind.

Out-of-git location for sensitive material: agreed separately with the
stakeholder.

## Redaction policy for printed samples

- Personal names → `«NAME»`.
- ID numbers (EMBG, passport, driver-licence number) → `«ID»`.
- Plate numbers → `«PLATE»`.
- Addresses → `«ADDRESS»`.
- Phone numbers → `«PHONE»`.
- Bank account numbers → `«ACCOUNT»`.
- Stamps and signatures stay (they're layout, not data).
- Static labels, headings, and form structure stay verbatim.
