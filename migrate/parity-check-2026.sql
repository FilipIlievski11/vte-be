-- =============================================================================
-- 2026 PRINT-PARITY CHECK — live legacy vs migrated v2.
-- Compares the print-relevant fields (anchor owner + vehicle + tech-exam) for
-- every 2026 request, by request Id (Ids are preserved across the migration).
-- Reports: count parity, per-field mismatch counts, and sample mismatches.
--
-- Read-only. Run anytime to verify the prints would match.
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\parity-check-2026.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO
SET QUOTED_IDENTIFIER ON; SET ANSI_NULLS ON; SET NOCOUNT ON;

-- Normalize helper: empty/whitespace → NULL so legacy "" vs v2 NULL isn't a false diff.
-- (Inlined via NULLIF(LTRIM(RTRIM(x)),'') everywhere below.)

-- 1) Stage the live 2026 print fields (run the join ON the remote server).
IF OBJECT_ID('tempdb..#live') IS NOT NULL DROP TABLE #live;
SELECT * INTO #live FROM OPENQUERY(VTEZVV_LIVE, '
  SELECT r.Id AS ReqId,
         -- migration maps legacy CustomerFirstName→v2.FirstName, CustomerSurname→v2.LastName
         NULLIF(LTRIM(RTRIM(cu.CustomerFirstName)),  '''') AS Surname,
         NULLIF(LTRIM(RTRIM(cu.CustomerSurname)),    '''') AS GivenName,
         NULLIF(LTRIM(RTRIM(cu.MB)),                 '''') AS MB,
         NULLIF(LTRIM(RTRIM(ve.ShellNumber)),        '''') AS Vin,
         NULLIF(LTRIM(RTRIM(ve.LastRegistratinNumber)),'''') AS Plate,
         NULLIF(ve.IdVehicleModel, 0)                     AS ModelId,
         YEAR(ve.MakeDate)                                AS MakeYear,
         NULLIF(r.IdTechnicalExamReport, 0)               AS TechExam
  FROM VTEZVV.dbo.Requests r
  JOIN VTEZVV.dbo.CustomerVehiclesRelations cvr ON cvr.Id = r.IdCustomerVehicleRelation
  JOIN VTEZVV.dbo.Customers cu ON cu.Id = cvr.IdCustomer
  LEFT JOIN VTEZVV.dbo.Vehicles ve ON ve.Id = cvr.IdVehicle
  WHERE YEAR(r.DateCreated) = 2026');

-- 2) v2 print fields for 2026 (anchor relation).
IF OBJECT_ID('tempdb..#v2') IS NOT NULL DROP TABLE #v2;
SELECT
  r.Id AS ReqId,
  NULLIF(LTRIM(RTRIM(c.FirstName)), N'') AS Surname,   -- MK convention: FirstName = surname
  NULLIF(LTRIM(RTRIM(c.LastName)),  N'') AS GivenName,
  NULLIF(LTRIM(RTRIM(c.MB)),        N'') AS MB,
  NULLIF(LTRIM(RTRIM(v.Vin)),       N'') AS Vin,
  NULLIF(LTRIM(RTRIM(v.Plate)),     N'') AS Plate,
  v.ModelId                              AS ModelId,
  YEAR(v.ManufactureDate)                AS MakeYear,
  CAST(r.TechnicalExamReportId AS int)   AS TechExam
INTO #v2
FROM dbo.Request r
JOIN dbo.ClientVehicleRelation cvr ON cvr.Id = r.ClientVehicleRelationId
JOIN dbo.Client c ON c.Id = cvr.ClientId
LEFT JOIN dbo.Vehicle v ON v.Id = cvr.VehicleId
WHERE YEAR(r.CreatedAt) = 2026;

-- 3) Count parity
PRINT '=== Count parity (2026) ===';
SELECT
  (SELECT COUNT(*) FROM #live)                                   AS LiveRequests,
  (SELECT COUNT(*) FROM #v2)                                     AS V2Requests,
  (SELECT COUNT(*) FROM #live l WHERE NOT EXISTS (SELECT 1 FROM #v2 v WHERE v.ReqId=l.ReqId)) AS MissingInV2;

-- 4) Per-field mismatch counts (only rows present in both)
PRINT '=== Per-field mismatches (present in both) ===';
SELECT
  SUM(CASE WHEN ISNULL(l.Surname,N'#')   <> ISNULL(v.Surname,N'#')   THEN 1 ELSE 0 END) AS SurnameDiff,
  SUM(CASE WHEN ISNULL(l.GivenName,N'#') <> ISNULL(v.GivenName,N'#') THEN 1 ELSE 0 END) AS GivenNameDiff,
  SUM(CASE WHEN ISNULL(l.MB,N'#')        <> ISNULL(v.MB,N'#')        THEN 1 ELSE 0 END) AS MbDiff,
  SUM(CASE WHEN ISNULL(l.Vin,N'#')       <> ISNULL(v.Vin,N'#')       THEN 1 ELSE 0 END) AS VinDiff,
  SUM(CASE WHEN ISNULL(l.Plate,N'#')     <> ISNULL(v.Plate,N'#')     THEN 1 ELSE 0 END) AS PlateDiff,
  SUM(CASE WHEN ISNULL(l.ModelId,-1)     <> ISNULL(v.ModelId,-1)     THEN 1 ELSE 0 END) AS ModelDiff,
  SUM(CASE WHEN ISNULL(l.MakeYear,-1)    <> ISNULL(v.MakeYear,-1)    THEN 1 ELSE 0 END) AS YearDiff,
  SUM(CASE WHEN ISNULL(l.TechExam,-1)    <> ISNULL(v.TechExam,-1)    THEN 1 ELSE 0 END) AS TechExamDiff
FROM #live l JOIN #v2 v ON v.ReqId = l.ReqId;

-- 5) Sample mismatches (top 20) for inspection
PRINT '=== Sample mismatches (top 20) ===';
SELECT TOP 20 l.ReqId,
  l.Surname AS L_Surname, v.Surname AS V_Surname,
  l.Plate   AS L_Plate,   v.Plate   AS V_Plate,
  l.Vin     AS L_Vin,     v.Vin     AS V_Vin,
  l.ModelId AS L_Model,   v.ModelId AS V_Model,
  l.TechExam AS L_Tex,    v.TechExam AS V_Tex
FROM #live l JOIN #v2 v ON v.ReqId = l.ReqId
WHERE ISNULL(l.Surname,N'#')<>ISNULL(v.Surname,N'#')
   OR ISNULL(l.GivenName,N'#')<>ISNULL(v.GivenName,N'#')
   OR ISNULL(l.MB,N'#')<>ISNULL(v.MB,N'#')
   OR ISNULL(l.Vin,N'#')<>ISNULL(v.Vin,N'#')
   OR ISNULL(l.Plate,N'#')<>ISNULL(v.Plate,N'#')
   OR ISNULL(l.ModelId,-1)<>ISNULL(v.ModelId,-1)
   OR ISNULL(l.MakeYear,-1)<>ISNULL(v.MakeYear,-1)
   OR ISNULL(l.TechExam,-1)<>ISNULL(v.TechExam,-1)
ORDER BY l.ReqId DESC;

-- 6) Which 2026 requests are missing in v2
PRINT '=== 2026 requests in live but missing in v2 ===';
SELECT TOP 20 l.ReqId, l.Surname, l.Plate FROM #live l
WHERE NOT EXISTS (SELECT 1 FROM #v2 v WHERE v.ReqId=l.ReqId) ORDER BY l.ReqId;

DROP TABLE #live; DROP TABLE #v2;
PRINT '=== Done ===';
