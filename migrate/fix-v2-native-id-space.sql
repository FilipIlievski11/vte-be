-- fix-v2-native-id-space.sql
-- One-time repair so the legacy incremental sync can keep working now that v2 is live.
--
-- Problem: v2 went live 2026-06-13 and creates its OWN rows whose identity ids continue
-- from the legacy max — while legacy keeps inserting with ITS identity from the same
-- point. Same id ranges, different records → the watermark sync (Id > MAX) skips real
-- legacy rows and/or collides.
--
-- Fix (approved by Filip 2026-07-06, "napraj mi sync na podatoci"):
--   1. Move the only v2-native strays out of the legacy id range:
--        - ClientVehicleRelation 124614/124615/124616 (IDL "Лично" vehicle-less relations)
--          → 10,000,001..3, re-pointing their CustomerDebt anchors (only referencers).
--        - Client 52877 (ТЕСТ-CLAUDE, zero references) → hard-deleted (test junk).
--   2. Reseed the legacy-mirrored tables to 10,000,000 so every FUTURE v2-native row
--      lives far above legacy ids forever. Legacy syncs keep filling the space below.
--      (migrate-incremental.sql computes watermarks as MAX(Id) WHERE Id < 10,000,000.)
--
-- Idempotent: guarded by the existence of the stray rows.

SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRAN;

-- ---- 1a. relocate the three Лично relations ----
IF EXISTS (SELECT 1 FROM dbo.ClientVehicleRelation WHERE Id IN (124614, 124615, 124616) AND VehicleId IS NULL AND RelationTypeId = 3)
BEGIN
    SET IDENTITY_INSERT dbo.ClientVehicleRelation ON;

    INSERT INTO dbo.ClientVehicleRelation (Id, ClientId, VehicleId, RelationTypeId, StartDate, EndDate, StartNote, EndNote, Active)
    SELECT 10000000 + (Id - 124613), ClientId, VehicleId, RelationTypeId, StartDate, EndDate, StartNote, EndNote, Active
    FROM dbo.ClientVehicleRelation
    WHERE Id IN (124614, 124615, 124616) AND VehicleId IS NULL AND RelationTypeId = 3;

    SET IDENTITY_INSERT dbo.ClientVehicleRelation OFF;

    UPDATE dbo.CustomerDebt
    SET CustomerVehicleRelationId = 10000000 + (CustomerVehicleRelationId - 124613)
    WHERE CustomerVehicleRelationId IN (124614, 124615, 124616);

    DELETE FROM dbo.ClientVehicleRelation
    WHERE Id IN (124614, 124615, 124616) AND VehicleId IS NULL AND RelationTypeId = 3;

    PRINT 'Relocated 3 Лично relations to 10,000,001-3 (+ CustomerDebt anchors).';
END
ELSE PRINT 'Relations already relocated - skipped.';

-- ---- 1b. delete the test client (verified zero references) ----
IF EXISTS (SELECT 1 FROM dbo.Client WHERE Id = 52877 AND CreatedAt > '2026-07-01')
BEGIN
    DELETE FROM dbo.Client WHERE Id = 52877;
    PRINT 'Deleted test client 52877 (ТЕСТ-CLAUDE).';
END
ELSE PRINT 'Test client 52877 already gone - skipped.';

-- ---- 2. reseed the legacy-mirrored tables to the 10M v2-native floor ----
-- RESEED only raises the NEXT identity value; existing rows are untouched.
DECLARE @floor bigint = 10000000;
IF IDENT_CURRENT('dbo.Client')                    < @floor DBCC CHECKIDENT ('dbo.Client',                    RESEED, 10000000);
IF IDENT_CURRENT('dbo.Vehicle')                   < @floor DBCC CHECKIDENT ('dbo.Vehicle',                   RESEED, 10000000);
IF IDENT_CURRENT('dbo.ClientVehicleRelation')     < @floor DBCC CHECKIDENT ('dbo.ClientVehicleRelation',     RESEED, 10000003);
IF IDENT_CURRENT('dbo.VehicleRegistration')       < @floor DBCC CHECKIDENT ('dbo.VehicleRegistration',       RESEED, 10000000);
IF IDENT_CURRENT('dbo.Request')                   < @floor DBCC CHECKIDENT ('dbo.Request',                   RESEED, 10000000);
IF IDENT_CURRENT('dbo.RequestOwnershipProof')     < @floor DBCC CHECKIDENT ('dbo.RequestOwnershipProof',     RESEED, 10000000);
IF IDENT_CURRENT('dbo.RequestPaymentProof')       < @floor DBCC CHECKIDENT ('dbo.RequestPaymentProof',       RESEED, 10000000);
IF IDENT_CURRENT('dbo.TechnicalExamReport')       < @floor DBCC CHECKIDENT ('dbo.TechnicalExamReport',       RESEED, 10000000);
IF IDENT_CURRENT('dbo.TechnicalExamReportDetail') < @floor DBCC CHECKIDENT ('dbo.TechnicalExamReportDetail', RESEED, 10000000);
PRINT 'Identity floors raised to 10,000,000 on the 9 mirrored tables.';

COMMIT;
