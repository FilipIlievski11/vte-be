-- ===================================================================
-- Step 1: Import legacy operators (vtesecurity.Users → AspNetUsers + Operators)
-- Step 2: Backfill TechnicalExamReports.{First,Second}InspectorOperatorUserId
-- Step 3: Re-attempt Permissions migration (was 0 last run)
-- Step 4: CustomerFinancialState backfill (with CustomerId derived from CVR)
-- Idempotent — safe to re-run.
-- ===================================================================
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
USE VTE2;
GO

-- =====================================================================
-- 0. Mapping table for legacy user-id → modern AspNetUser GUID
-- =====================================================================
IF OBJECT_ID('dbo.LegacyUserMap', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.LegacyUserMap(
    LegacyId    BIGINT       NOT NULL PRIMARY KEY,
    ModernId    NVARCHAR(450) NOT NULL,
    ImportedUtc DATETIME2     NOT NULL DEFAULT(SYSUTCDATETIME())
  );
END

-- Bring across the Operator role hash & stamp from the existing admin so all imported
-- users have the same default password "ChangeMe!Now1". They MUST change it on first login.
DECLARE @AdminHash  NVARCHAR(MAX) = (SELECT TOP 1 PasswordHash FROM AspNetUsers WHERE NormalizedUserName = 'ADMIN');
DECLARE @AdminStamp NVARCHAR(MAX) = (SELECT TOP 1 SecurityStamp FROM AspNetUsers WHERE NormalizedUserName = 'ADMIN');
DECLARE @OperatorRoleId NVARCHAR(450) = (SELECT TOP 1 Id FROM AspNetRoles WHERE NormalizedName = 'OPERATOR');
DECLARE @StationId INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

IF @OperatorRoleId IS NULL
BEGIN
  SET @OperatorRoleId = LOWER(NEWID());
  INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
  VALUES (@OperatorRoleId, N'Operator', N'OPERATOR', LOWER(NEWID()));
END

PRINT '=== Step 1: Importing legacy operators ===';

;WITH base AS (
  SELECT
    Id AS LegacyId,
    LTRIM(RTRIM(ISNULL(NULLIF(LTRIM(RTRIM(UserName)), ''), CONCAT('legacy-', Id)))) AS RawUserName,
    LTRIM(RTRIM(COALESCE(
      NULLIF(UserFullName, ''),
      NULLIF(LTRIM(RTRIM(CONCAT(FirstName, ' ', SureName))), ''),
      CONCAT('Legacy User ', Id)))) AS FullName,
    NULLIF(LTRIM(RTRIM(EMBG)), '') AS EMBG,
    Active
  FROM VTEZVV_Snapshot.dbo.LegacyUsers
),
ranked AS (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY UPPER(RawUserName) ORDER BY LegacyId) AS rn FROM base
),
cleaned AS (
  SELECT LegacyId, FullName, EMBG, Active,
    CASE WHEN rn = 1 THEN RawUserName ELSE CONCAT(RawUserName, '-', LegacyId) END AS UserName
  FROM ranked
)
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail,
  EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
  PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount)
SELECT
  LOWER(CAST(NEWID() AS NVARCHAR(50))),
  c.UserName,
  UPPER(c.UserName),
  CONCAT(c.UserName, '@legacy.local'),
  UPPER(CONCAT(c.UserName, '@legacy.local')),
  1, @AdminHash, LOWER(CAST(NEWID() AS NVARCHAR(50))), LOWER(CAST(NEWID() AS NVARCHAR(50))),
  0, 0, 1, 0
FROM cleaned c
WHERE NOT EXISTS (SELECT 1 FROM dbo.LegacyUserMap m WHERE m.LegacyId = c.LegacyId)
  AND NOT EXISTS (SELECT 1 FROM AspNetUsers u WHERE u.NormalizedUserName COLLATE Macedonian_FYROM_90_CI_AS = UPPER(c.UserName));

-- Build the LegacyUserMap rows using the same dedup logic so each legacy user
-- maps to its OWN modern user (we appended -<LegacyId> to dup usernames above).
;WITH base2 AS (
  SELECT
    Id AS LegacyId,
    LTRIM(RTRIM(ISNULL(NULLIF(LTRIM(RTRIM(UserName)), ''), CONCAT('legacy-', Id)))) AS RawUserName
  FROM VTEZVV_Snapshot.dbo.LegacyUsers
),
ranked2 AS (
  SELECT *, ROW_NUMBER() OVER (PARTITION BY UPPER(RawUserName) ORDER BY LegacyId) AS rn FROM base2
),
cleaned2 AS (
  SELECT LegacyId,
    CASE WHEN rn = 1 THEN RawUserName ELSE CONCAT(RawUserName, '-', LegacyId) END AS UserName
  FROM ranked2
)
INSERT INTO dbo.LegacyUserMap (LegacyId, ModernId)
SELECT c.LegacyId, u.Id
FROM cleaned2 c
JOIN AspNetUsers u ON u.NormalizedUserName COLLATE Macedonian_FYROM_90_CI_AS = UPPER(c.UserName)
WHERE NOT EXISTS (SELECT 1 FROM dbo.LegacyUserMap m WHERE m.LegacyId = c.LegacyId);

-- Add the Operator role for any newly-imported users (skip ones who are already in the role)
INSERT INTO AspNetUserRoles (UserId, RoleId)
SELECT m.ModernId, @OperatorRoleId
FROM dbo.LegacyUserMap m
WHERE NOT EXISTS (SELECT 1 FROM AspNetUserRoles ur WHERE ur.UserId = m.ModernId AND ur.RoleId = @OperatorRoleId);

-- Operators table (one per AspNetUser)
INSERT INTO Operators (UserId, StationId, FullName, EMBG, IsActive, CreatedUtc, LastModifiedUtc)
SELECT
  m.ModernId, @StationId,
  LTRIM(RTRIM(COALESCE(NULLIF(lu.UserFullName, ''), CONCAT(lu.FirstName, ' ', lu.SureName), CONCAT('Legacy ', lu.Id)))),
  LEFT(NULLIF(LTRIM(RTRIM(lu.EMBG)), ''), 13),
  ISNULL(lu.Active, 1), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM dbo.LegacyUserMap m
JOIN VTEZVV_Snapshot.dbo.LegacyUsers lu ON lu.Id = m.LegacyId
WHERE NOT EXISTS (SELECT 1 FROM Operators o WHERE o.UserId = m.ModernId);

DECLARE @OpCount INT = (SELECT COUNT(*) FROM dbo.LegacyUserMap);
PRINT CONCAT('  Legacy users mapped: ', @OpCount);
GO

-- =====================================================================
-- Step 2: Backfill TechnicalExamReports inspector GUIDs
-- =====================================================================
PRINT '=== Step 2: Backfilling tech-exam inspectors ===';
DECLARE @rc INT;

UPDATE r
SET    r.FirstInspectorOperatorUserId = m.ModernId
FROM   dbo.TechnicalExamReports r
JOIN   VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReports lr ON lr.Id = r.Id
JOIN   dbo.LegacyUserMap m ON m.LegacyId = lr.IdFirsControler
WHERE  m.ModernId IS NOT NULL;
SET @rc = @@ROWCOUNT;
PRINT CONCAT('  FirstInspector backfilled: ', @rc);

UPDATE r
SET    r.SecondInspectorOperatorUserId = m.ModernId
FROM   dbo.TechnicalExamReports r
JOIN   VTEZVV_Snapshot.dbo.DocumentsTehnicalExamsReports lr ON lr.Id = r.Id
JOIN   dbo.LegacyUserMap m ON m.LegacyId = lr.IdSecondControler
WHERE  m.ModernId IS NOT NULL AND lr.IdSecondControler <> lr.IdFirsControler;
SET @rc = @@ROWCOUNT;
PRINT CONCAT('  SecondInspector backfilled: ', @rc);
GO

-- =====================================================================
-- Step 3: Permissions backfill
-- =====================================================================
PRINT '=== Step 3: Permissions ===';
DECLARE @PermStation INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

DELETE FROM Permissions;  -- start fresh; the table was empty before
SET IDENTITY_INSERT Permissions ON;
INSERT INTO Permissions (Id, StationId, CustomerVehicleRelationId, PermissionNumber, PermissionTypeId,
  MadeDate, EndDate, Note, IsActive, CreatedUtc, LastModifiedUtc)
SELECT p.Id, @PermStation, p.IdCustomerVehicleRelation,
  LEFT(p.PermissionNumber, 50), NULL,
  CAST(p.DateCreated AS DATE),
  CAST(p.ValidTillDate AS DATE),
  LEFT(CONCAT(
    CASE WHEN p.Note IS NULL THEN '' ELSE CONCAT(p.Note, N' | ') END,
    CASE WHEN p.TrafficLicenceNumber IS NULL THEN '' ELSE CONCAT(N'СД: ', p.TrafficLicenceNumber, N' | ') END,
    CASE WHEN p.TriptiqueNumber IS NULL THEN '' ELSE CONCAT(N'Триптик: ', p.TriptiqueNumber) END
  ), 500),
  ISNULL(p.Active, 1), SYSUTCDATETIME(), SYSUTCDATETIME()
FROM VTEZVV_Snapshot.dbo.DocumentsPermisions p
WHERE EXISTS(SELECT 1 FROM CustomerVehicleRelations cvr WHERE cvr.Id = p.IdCustomerVehicleRelation);
SET IDENTITY_INSERT Permissions OFF;
DECLARE @PCount INT = (SELECT COUNT(*) FROM Permissions);
PRINT CONCAT('  Permissions migrated: ', @PCount);
GO

-- =====================================================================
-- Step 4: CustomerFinancialState backfill (derive CustomerId from CVR)
-- =====================================================================
PRINT '=== Step 4: CustomerFinancialState ===';
DECLARE @CFSStation INT = (SELECT TOP 1 Id FROM Stations WHERE Code = N'BRZ-SK');

DELETE FROM CustomerFinancialState;
SET IDENTITY_INSERT CustomerFinancialState ON;
;WITH src AS (
  SELECT
    f.Id, cvr.CustomerId, @CFSStation AS StationId,
    -- Date: prefer the related PaymentDocument.DatePay if traceable, else today
    COALESCE(
      (SELECT TOP 1 CAST(pd.DatePay AS DATE) FROM dbo.PaymentDocuments pd WHERE pd.Id = f.IdDocument),
      CAST(SYSUTCDATETIME() AS DATE)
    ) AS Date,
    LEFT(f.Note, 250) AS Description,
    CAST(CASE WHEN f.Payed = 0 THEN f.Price ELSE 0 END AS DECIMAL(18,2)) AS DebitAmount,
    CAST(CASE WHEN f.Payed = 1 THEN f.Price ELSE 0 END AS DECIMAL(18,2)) AS CreditAmount,
    CASE WHEN EXISTS(SELECT 1 FROM dbo.PaymentDocuments pd WHERE pd.Id = f.IdDocument) THEN f.IdDocument ELSE NULL END AS PaymentDocumentId
  FROM VTEZVV_Snapshot.dbo.CustomerFinancialState f
  JOIN CustomerVehicleRelations cvr ON cvr.Id = f.IdCustomerVehicleRelation
  WHERE EXISTS(SELECT 1 FROM Customers c WHERE c.Id = cvr.CustomerId)
)
INSERT INTO CustomerFinancialState (Id, CustomerId, StationId, Date, Description,
  DebitAmount, CreditAmount, RunningBalance, PaymentDocumentId, CreatedUtc)
SELECT Id, CustomerId, StationId, Date, Description,
  DebitAmount, CreditAmount,
  -- Running balance per customer (debit positive, credit negative)
  SUM(DebitAmount - CreditAmount) OVER (PARTITION BY CustomerId ORDER BY Date, Id) AS RunningBalance,
  PaymentDocumentId, SYSUTCDATETIME()
FROM src;
SET IDENTITY_INSERT CustomerFinancialState OFF;
DECLARE @CFSCount INT = (SELECT COUNT(*) FROM CustomerFinancialState);
PRINT CONCAT('  CustomerFinancialState migrated: ', @CFSCount);
GO

PRINT '=== Done ===';
SELECT 'Operators' Tab, COUNT(*) Cnt FROM Operators UNION ALL
SELECT 'Permissions', COUNT(*) FROM Permissions UNION ALL
SELECT 'CustomerFinancialState', COUNT(*) FROM CustomerFinancialState UNION ALL
SELECT 'Tech-exam inspectors filled',
  (SELECT COUNT(*) FROM TechnicalExamReports
   WHERE FirstInspectorOperatorUserId IS NOT NULL AND FirstInspectorOperatorUserId NOT IN (SELECT Id FROM AspNetUsers WHERE NormalizedUserName='ADMIN'))
ORDER BY Tab;
GO
