-- =============================================================================
-- Fix the technical-exam organization display name to match the legacy prints.
--
-- Legacy "OrganizationAndStation" (Записник header + Потврда letterhead) =
--   UPPER( Companies.CompanyName + "-" + TehnicalExamOrganizations.Station )
-- e.g. "АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ ВЕЛЕС-ВЕЛЕС" (company id 4 + station "Велес").
--
-- The bulk migration only stored the station ("Велес") in TechnicalExamOrganization.Name,
-- because the legacy company name lives in a separate VTEZVV.Companies table.
-- This recomposes the display name from the snapshot (station + company) so the
-- prints show the full organization. Idempotent — always derives from the snapshot,
-- never from the (already-overwritten) current Name. Orgs with no company (IdCompany=0)
-- keep their station name.
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\fix-tech-exam-org-name.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET NOCOUNT ON;

UPDATE o
   SET o.Name = UPPER(LTRIM(RTRIM(co.CompanyName)) + N'-' + LTRIM(RTRIM(so.Station)))
FROM dbo.TechnicalExamOrganization o
JOIN VTEZVV_Snapshot.dbo.TehnicalExamOrganizations so ON so.Id = o.Id
JOIN VTEZVV_Snapshot.dbo.Companies co ON co.Id = so.IdCompany
WHERE so.IdCompany > 0
  AND NULLIF(LTRIM(RTRIM(so.Station)), N'') IS NOT NULL
  AND NULLIF(LTRIM(RTRIM(co.CompanyName)), N'') IS NOT NULL;
PRINT CONCAT('Organizations renamed: ', @@ROWCOUNT);

SELECT Id, Name FROM dbo.TechnicalExamOrganization WHERE Id IN (1, 5, 32, 37) ORDER BY Id;
