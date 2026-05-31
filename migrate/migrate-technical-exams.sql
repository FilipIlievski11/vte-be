-- =============================================================================
-- TECHNICAL EXAM bulk migration — loads the full history of technical-exam
-- reports from the local snapshot DB (VTEZVV_Snapshot) into VTE.
--
-- Tables created by EF migration AddTechnicalExams:
--   TechnicalExamType, TechnicalExamDetailStatus, TechnicalExamVehiclePart,
--   TechnicalExamOrganization  (lookups)
--   TechnicalExamReport  +  TechnicalExamReportDetail  (data)
--
-- Source → target (legacy spelling is "Tehnical"):
--   TehnicalExamsTypes                       → TechnicalExamType
--   DocumentsTehnicalExamsReportsDetailsStatus → TechnicalExamDetailStatus
--   TehnicalExamVehicleParts                 → TechnicalExamVehiclePart
--   TehnicalExamOrganizations                → TechnicalExamOrganization
--   DocumentsTehnicalExamsReports            → TechnicalExamReport
--   DocumentsTehnicalExamsReportsDetails     → TechnicalExamReportDetail
--
-- Notes:
--   • Ids are preserved (IDENTITY_INSERT) so Request.TechnicalExamReportId keeps lining up.
--   • Measurements: legacy real NOT NULL with 0 = "not measured"; migrated via
--     NULLIF(x,0) into nullable float (a real "blank").
--   • Brake column rename: {Axis}LeftPj → {Axis}LeftRightDiff, {Axis}PN → {Axis}Coefficient.
--   • CompanyId is hard-set to 4 (same tenant the base migration used).
--   • Reports whose IdCustomerVehicleRelation no longer exists in v2 (≈131) get a
--     NULL relation rather than being dropped — every report still migrates.
--   • Re-runnable: every section guards with NOT EXISTS (by Id).
--
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-technical-exams.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET QUOTED_IDENTIFIER ON;   -- filtered indexes / safe string compares
SET ANSI_NULLS ON;
SET XACT_ABORT ON;
SET NOCOUNT ON;

DECLARE @src sysname = N'VTEZVV_Snapshot';
DECLARE @rows int;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN RAISERROR('Snapshot DB VTEZVV_Snapshot not found.', 16, 1); RETURN; END;

-- ============================================================================
-- 1. Lookups
-- ============================================================================
PRINT '=== TechnicalExamType ===';
SET IDENTITY_INSERT dbo.TechnicalExamType ON;
INSERT INTO dbo.TechnicalExamType (Id, Code, Description, ValidDays, PercentOfFullExam, IsInRegister, Active)
SELECT t.Id,
       NULLIF(LTRIM(RTRIM(t.Code)), N''),
       COALESCE(NULLIF(LTRIM(RTRIM(t.Description)), N''), N'?'),
       t.ValidNumOfDays, t.PercentOfFullExam, t.IsInRegistar, t.Active
FROM VTEZVV_Snapshot.dbo.TehnicalExamsTypes t
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamType x WHERE x.Id = t.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamType OFF;
DBCC CHECKIDENT('dbo.TechnicalExamType', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' types.');

PRINT '=== TechnicalExamDetailStatus ===';
SET IDENTITY_INSERT dbo.TechnicalExamDetailStatus ON;
INSERT INTO dbo.TechnicalExamDetailStatus (Id, Name, Active)
SELECT s.Id, COALESCE(NULLIF(LTRIM(RTRIM(s.StatusName)), N''), N'?'), s.Active
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReportsDetailsStatus s
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamDetailStatus x WHERE x.Id = s.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamDetailStatus OFF;
DBCC CHECKIDENT('dbo.TechnicalExamDetailStatus', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' statuses.');

PRINT '=== TechnicalExamVehiclePart ===';
SET IDENTITY_INSERT dbo.TechnicalExamVehiclePart ON;
INSERT INTO dbo.TechnicalExamVehiclePart (Id, CategoryId, Code, Description, Active)
SELECT p.Id, p.IdCategoryVehicleParts,
       COALESCE(NULLIF(LTRIM(RTRIM(p.Code)), N''), N'?'),
       COALESCE(NULLIF(LTRIM(RTRIM(p.Description)), N''), N'?'),
       p.Active
FROM VTEZVV_Snapshot.dbo.TehnicalExamVehicleParts p
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamVehiclePart x WHERE x.Id = p.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamVehiclePart OFF;
DBCC CHECKIDENT('dbo.TechnicalExamVehiclePart', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' parts.');

PRINT '=== TechnicalExamOrganization ===';
SET IDENTITY_INSERT dbo.TechnicalExamOrganization ON;
INSERT INTO dbo.TechnicalExamOrganization
  (Id, CompanyId, Code, Name, CityId, CommunityId, Address, Phone, Fax,
   BankAccount, Depositor, TaxNumber, ResponsibleOfficer, Secretary, Active)
SELECT o.Id,
       CAST(NULLIF(o.IdCompany, 0) AS tinyint),
       NULLIF(LTRIM(RTRIM(o.Code)), N''),
       NULLIF(LTRIM(RTRIM(o.Station)), N''),
       NULLIF(o.IdCity, 0), NULLIF(o.IdCommunity, 0),
       NULLIF(LTRIM(RTRIM(o.StationAddress)), N''),
       NULLIF(LTRIM(RTRIM(o.Tel)), N''),
       NULLIF(LTRIM(RTRIM(o.Fax)), N''),
       NULLIF(LTRIM(RTRIM(o.ZiroSmetka)), N''),
       NULLIF(LTRIM(RTRIM(o.Deponent)), N''),
       NULLIF(LTRIM(RTRIM(o.EDB)), N''),
       NULLIF(LTRIM(RTRIM(o.OdgovorenOrgan)), N''),
       NULLIF(LTRIM(RTRIM(o.Sekretar)), N''),
       o.Active
FROM VTEZVV_Snapshot.dbo.TehnicalExamOrganizations o
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamOrganization x WHERE x.Id = o.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamOrganization OFF;
DBCC CHECKIDENT('dbo.TechnicalExamOrganization', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' organizations.');

-- ============================================================================
-- 2. TechnicalExamReport (the 107k history). Measurements NULLIF(,0).
-- ============================================================================
PRINT '=== TechnicalExamReport ===';
SET IDENTITY_INSERT dbo.TechnicalExamReport ON;
INSERT INTO dbo.TechnicalExamReport (
  Id, CompanyId, CustomerVehicleRelationId, TechnicalExamTypeId, OrganizationId,
  RegNumber, MadeDate, ValidTillDate, FirstControllerLegacyId, SecondControllerLegacyId,
  VehicleIsRight, ExplanationNote, DriversWarning, Note, TechnicalChanges,
  Axis1Left, Axis1Right, Axis1Gj, Axis1LeftRightDiff, Axis1Coefficient,
  Axis2Left, Axis2Right, Axis2Gj, Axis2LeftRightDiff, Axis2Coefficient,
  Axis3Left, Axis3Right, Axis3Gj, Axis3LeftRightDiff, Axis3Coefficient,
  Axis4Left, Axis4Right, Axis4Gj, Axis4LeftRightDiff, Axis4Coefficient,
  AxisParkingLeft, AxisParkingRight, AxisParkingGj, AxisParkingLeftRightDiff, AxisParkingCoefficient,
  Weight, EffectOfWorkingBrakeEmpty, EffectOfWorkingBrakeFull, EffectOfSecondaryBrake, EffectOfParkingBrake,
  SpeedOfTurns, CO, EngineRpm, COPlusTurns, Lambda, Pinpoints, Noise, EngineOilTemp,
  Active, CreatedAt, ModifiedAt)
SELECT
  r.Id, CAST(4 AS tinyint),
  cvr.Id,                                          -- NULL when the relation no longer exists
  r.IdTypeOfTehnicalExam, r.IdOrganizationForTehnicalExam,
  NULLIF(LTRIM(RTRIM(r.RegNumber)), N''),
  CAST(r.MadeDate AS date), CAST(r.ValidTillDate AS date),
  NULLIF(r.IdFirsControler, 0), NULLIF(r.IdSecondControler, 0),
  r.VehicleIsRight,
  NULLIF(LTRIM(RTRIM(r.ExplanationNote)), N''),
  NULLIF(LTRIM(RTRIM(r.DriversWarning)), N''),
  NULLIF(LTRIM(RTRIM(r.Note)), N''),
  NULLIF(LTRIM(RTRIM(r.TechnicalChanges)), N''),
  NULLIF(r.Axis1Left,0), NULLIF(r.Axis1Right,0), NULLIF(r.Axis1Gj,0), NULLIF(r.Axis1LeftPj,0), NULLIF(r.Axis1PN,0),
  NULLIF(r.Axis2Left,0), NULLIF(r.Axis2Right,0), NULLIF(r.Axis2Gj,0), NULLIF(r.Axis2LeftPj,0), NULLIF(r.Axis2PN,0),
  NULLIF(r.Axis3Left,0), NULLIF(r.Axis3Right,0), NULLIF(r.Axis3Gj,0), NULLIF(r.Axis3LeftPj,0), NULLIF(r.Axis3PN,0),
  NULLIF(r.Axis4Left,0), NULLIF(r.Axis4Right,0), NULLIF(r.Axis4Gj,0), NULLIF(r.Axis4LeftPj,0), NULLIF(r.Axis4PN,0),
  NULLIF(r.AxisParkingLeft,0), NULLIF(r.AxisParkingRight,0), NULLIF(r.AxisParkingGj,0), NULLIF(r.AxisParkingLeftPj,0), NULLIF(r.AxisParkingPN,0),
  NULLIF(r.Waight,0), NULLIF(r.EffectOfWorkingBreakEmpty,0), NULLIF(r.EffectOfWorkingBreakFull,0), NULLIF(r.EffectOfSecondaryBreak,0), NULLIF(r.EffectOfParkingBreak,0),
  NULLIF(r.SpeedOfTurns,0), NULLIF(r.CO,0), NULLIF(r.NumEngineTurns,0), NULLIF(r.COPlusTurns,0), NULLIF(r.Lambda,0), NULLIF(r.Pinpoints,0), NULLIF(r.Noise,0), NULLIF(r.TempOfEngineOil,0),
  r.Active, CAST(r.MadeDate AS datetime2), NULL
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReports r
LEFT JOIN dbo.ClientVehicleRelation cvr ON cvr.Id = r.IdCustomerVehicleRelation
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamReport x WHERE x.Id = r.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamReport OFF;
DBCC CHECKIDENT('dbo.TechnicalExamReport', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' reports.');

-- ============================================================================
-- 3. TechnicalExamReportDetail (defective-parts lines)
-- ============================================================================
PRINT '=== TechnicalExamReportDetail ===';
SET IDENTITY_INSERT dbo.TechnicalExamReportDetail ON;
INSERT INTO dbo.TechnicalExamReportDetail
  (Id, TechnicalExamReportId, VehiclePartId, StatusId, Front, Back, OnLeft, OnRight, EnteredAt, Note, Active)
SELECT d.Id, d.IdTehnicalExamsReports, d.IdTehnicalExamVehivlePart, d.IdStatus,
       d.Front, d.Back, d.OnLeft, d.OnRight,
       CAST(d.DateEnter AS datetime2),
       NULLIF(LTRIM(RTRIM(d.Note)), N''),
       d.Active
FROM VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReportsDetails d
WHERE NOT EXISTS (SELECT 1 FROM dbo.TechnicalExamReportDetail x WHERE x.Id = d.Id);
SET @rows = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.TechnicalExamReportDetail OFF;
DBCC CHECKIDENT('dbo.TechnicalExamReportDetail', RESEED) WITH NO_INFOMSGS;
PRINT CONCAT('  -> ', @rows, ' detail lines.');

COMMIT TRANSACTION;

-- ---- summary ----
PRINT '';
PRINT '=== Counts (v2) ===';
SELECT N'TechnicalExamType'         AS [Table], COUNT(*) AS [Rows] FROM dbo.TechnicalExamType
UNION ALL SELECT N'TechnicalExamDetailStatus',  COUNT(*) FROM dbo.TechnicalExamDetailStatus
UNION ALL SELECT N'TechnicalExamVehiclePart',   COUNT(*) FROM dbo.TechnicalExamVehiclePart
UNION ALL SELECT N'TechnicalExamOrganization',  COUNT(*) FROM dbo.TechnicalExamOrganization
UNION ALL SELECT N'TechnicalExamReport',        COUNT(*) FROM dbo.TechnicalExamReport
UNION ALL SELECT N'  (reports w/ NULL relation)', COUNT(*) FROM dbo.TechnicalExamReport WHERE CustomerVehicleRelationId IS NULL
UNION ALL SELECT N'TechnicalExamReportDetail',  COUNT(*) FROM dbo.TechnicalExamReportDetail;
PRINT '=== Technical exam bulk migration done ===';
END TRY
BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
    DECLARE @msg nvarchar(2048) = ERROR_MESSAGE(), @ln int = ERROR_LINE();
    RAISERROR('Technical exam migration failed at line %d: %s', 16, 1, @ln, @msg);
END CATCH;
