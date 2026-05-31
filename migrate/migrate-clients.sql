-- =============================================================================
-- VTEZVV.Customers → VTE.Client (+ chained ClientPersonalData)
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot.dbo.Customers
-- Target: (localdb)\MSSQLLocalDB\VTE.dbo.Client + dbo.ClientPersonalData
--
-- Decisions (per stakeholder):
--   1. Address = StreetName + ' ' + LivingAddressNumber + ' ' + CityName
--      (Streets looked up in legacy snapshot; CityName from the already-migrated
--      dbo.City table. CONCAT_WS skips NULLs so missing parts collapse cleanly.)
--   2. CompanyId = 4 for every Client (АВТО-БЕЗБЕДНОСТ МНС ДООЕЛ Велес)
--   3. MiddleName = legacy ParentName
--   4. CitizenshipId resolves via Citizenship.CountryId = legacy.IdCitizenship.
--      If no match, leave NULL (don't drop the client).
--   5. After Clients land, three legacy fields explode into ClientPersonalData rows:
--        BLK                   → PersonalDataTypeId = 3 (Personal Id)
--        PassportNumber        → PersonalDataTypeId = 2 (Passport)
--        DriveingLicenceNumber → PersonalDataTypeId = 1 (Driving Licence)
--      Issuer FK falls back to DocumentIssuer Id 1 if legacy issuer is NULL,
--      0, out of tinyint range, or missing from DocumentIssuer.
--
-- Idempotent — wipes target first. Refuses to run if downstream tables that
-- could reference Client (none yet) exist.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-clients.sql -b -X -I -f 65001
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Run migrate\snapshot-legacy.ps1 first.', 16, 1);
  RETURN;
END;

-- Required parents
IF NOT EXISTS (SELECT 1 FROM dbo.Company WHERE Id = 4)
BEGIN
  RAISERROR('Target Company Id=4 not found.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.City) = 0
BEGIN
  RAISERROR('No City rows. Run migrate-cities.sql first.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.Citizenship) = 0
BEGIN
  RAISERROR('No Citizenship rows. Run migrate-citizenships.sql first.', 16, 1);
  RETURN;
END;
IF (SELECT COUNT(*) FROM dbo.DocumentIssuer) = 0
BEGIN
  RAISERROR('No DocumentIssuer rows. Run migrate-document-issuers.sql first.', 16, 1);
  RETURN;
END;

-- Wipe order: ClientPersonalData (FK to Client) before Client.
PRINT '=== Wiping dbo.ClientPersonalData, dbo.Client ===';
DELETE FROM dbo.ClientPersonalData;
DELETE FROM dbo.Client;
DBCC CHECKIDENT ('dbo.ClientPersonalData', RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Client',             RESEED, 0) WITH NO_INFOMSGS;

-- Diagnostics
DECLARE @legacyTotal int = (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Customers);
PRINT CONCAT('Legacy Customers: ', @legacyTotal);

-- =============================================================================
-- Clients
-- =============================================================================
PRINT '=== Inserting Clients (CompanyId=4, Address = Street + space + Number) ===';
SET IDENTITY_INSERT dbo.Client ON;
INSERT INTO dbo.Client (
  Id, CompanyId, CityId, CitizenshipId, Business,
  FirstName, MiddleName, LastName, MB, Address,
  TaxNumber, PhoneNumber, Email, DateOfBirth, Note,
  Active, CreatedAt
)
SELECT
  c.Id,
  CAST(4 AS tinyint)                                                              AS CompanyId,
  ci.Id                                                                            AS CityId,
  cz.Id                                                                            AS CitizenshipId,
  c.IsCompany                                                                      AS Business,
  NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'')                                   AS FirstName,
  NULLIF(LTRIM(RTRIM(c.ParentName)),         N'')                                  AS MiddleName,
  NULLIF(LTRIM(RTRIM(c.CustomerSurname)),    N'')                                  AS LastName,
  NULLIF(LTRIM(RTRIM(c.MB)),                 N'')                                  AS MB,
  -- Address: StreetName + ' ' + LivingAddressNumber + ' ' + City.Name
  -- CONCAT_WS uses the separator only between non-null segments, so a Client
  -- missing one or two of {street, number, city} collapses cleanly without
  -- leftover blank spaces.
  NULLIF(LTRIM(RTRIM(
    CONCAT_WS(
      N' ',
      NULLIF(LTRIM(RTRIM(s.StreetName)),           N''),
      NULLIF(LTRIM(RTRIM(c.LivingAddressNumber)),  N''),
      NULLIF(LTRIM(RTRIM(ci.Name)),                N'')
    )
  )), N'')                                                                          AS Address,
  NULLIF(LTRIM(RTRIM(c.TaxNumber)),          N'')                                  AS TaxNumber,
  NULLIF(LTRIM(RTRIM(c.PhoneNumber)),        N'')                                  AS PhoneNumber,
  NULLIF(LTRIM(RTRIM(c.eMail)),              N'')                                  AS Email,
  c.DateOfBirth                                                                    AS DateOfBirth,
  NULLIF(LTRIM(RTRIM(c.Note)),               N'')                                  AS Note,
  c.Active                                                                         AS Active,
  GETUTCDATE()                                                                     AS CreatedAt
FROM VTEZVV_Snapshot.dbo.Customers c
LEFT JOIN VTEZVV_Snapshot.dbo.Streets s ON s.Id           = NULLIF(c.IdLivingAddress, 0)
LEFT JOIN dbo.City                  ci ON ci.Id           = NULLIF(c.IdLivingCity,    0)
LEFT JOIN dbo.Citizenship           cz ON cz.CountryId    = NULLIF(c.IdCitizenship,   0);
DECLARE @clientRows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Client OFF;

DECLARE @maxClientId bigint = (SELECT ISNULL(MAX(Id), 0) FROM dbo.Client);
DBCC CHECKIDENT ('dbo.Client', RESEED, @maxClientId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @clientRows, ' Clients. Identity counter reseeded to ', @maxClientId, '.');

-- =============================================================================
-- ClientPersonalData (chained, three types)
-- =============================================================================
DECLARE @FallbackIssuer tinyint = (SELECT TOP 1 Id FROM dbo.DocumentIssuer ORDER BY Id);

PRINT '=== Inserting ClientPersonalData ===';

-- 3 = Personal Id (BLK / id card)
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT
  c.Id,
  CAST(3 AS tinyint),
  COALESCE(d.Id, @FallbackIssuer),
  LTRIM(RTRIM(c.BLK)),
  ISNULL(c.BLKDateIssued, GETUTCDATE()),
  c.Active
FROM VTEZVV_Snapshot.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE
                                            WHEN c.BLKIssuer BETWEEN 1 AND 255
                                            THEN CAST(c.BLKIssuer AS tinyint)
                                         END
WHERE c.BLK IS NOT NULL AND LEN(LTRIM(RTRIM(c.BLK))) > 0;
DECLARE @pdId int = @@ROWCOUNT;
PRINT CONCAT('  Personal Id  -> ', @pdId, ' rows');

-- 2 = Passport
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT
  c.Id,
  CAST(2 AS tinyint),
  COALESCE(d.Id, @FallbackIssuer),
  LTRIM(RTRIM(c.PassportNumber)),
  ISNULL(c.PassDateIssued, GETUTCDATE()),
  c.Active
FROM VTEZVV_Snapshot.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE
                                            WHEN c.PassIssuer BETWEEN 1 AND 255
                                            THEN CAST(c.PassIssuer AS tinyint)
                                         END
WHERE c.PassportNumber IS NOT NULL AND LEN(LTRIM(RTRIM(c.PassportNumber))) > 0;
DECLARE @pdPass int = @@ROWCOUNT;
PRINT CONCAT('  Passport     -> ', @pdPass, ' rows');

-- 1 = Driving Licence
INSERT INTO dbo.ClientPersonalData (ClientId, PersonalDataTypeId, DocumentIssuerId, Number, CreatedAt, Active)
SELECT
  c.Id,
  CAST(1 AS tinyint),
  COALESCE(d.Id, @FallbackIssuer),
  LTRIM(RTRIM(c.DriveingLicenceNumber)),
  ISNULL(c.DriveingLicenceDateIssued, GETUTCDATE()),
  c.Active
FROM VTEZVV_Snapshot.dbo.Customers c
LEFT JOIN dbo.DocumentIssuer d ON d.Id = CASE
                                            WHEN c.DriveingLicenceIssuer BETWEEN 1 AND 255
                                            THEN CAST(c.DriveingLicenceIssuer AS tinyint)
                                         END
WHERE c.DriveingLicenceNumber IS NOT NULL
  AND LEN(LTRIM(RTRIM(c.DriveingLicenceNumber))) > 0;
DECLARE @pdDl int = @@ROWCOUNT;
PRINT CONCAT('  Driving Lic. -> ', @pdDl, ' rows');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Customers) AS LegacyCustomers,
  (SELECT COUNT(*) FROM dbo.Client)                    AS NewClients,
  (SELECT COUNT(*) FROM dbo.Client WHERE Active = 1)   AS ActiveClients,
  (SELECT COUNT(*) FROM dbo.Client WHERE CityId IS NULL)        AS ClientsNoCity,
  (SELECT COUNT(*) FROM dbo.Client WHERE CitizenshipId IS NULL) AS ClientsNoCitizenship,
  (SELECT COUNT(*) FROM dbo.Client WHERE Address IS NULL)       AS ClientsNoAddress;

SELECT
  (SELECT COUNT(*) FROM dbo.ClientPersonalData)                            AS PersonalDataTotal,
  (SELECT COUNT(*) FROM dbo.ClientPersonalData WHERE PersonalDataTypeId=3) AS PersonalId,
  (SELECT COUNT(*) FROM dbo.ClientPersonalData WHERE PersonalDataTypeId=2) AS Passport,
  (SELECT COUNT(*) FROM dbo.ClientPersonalData WHERE PersonalDataTypeId=1) AS DrivingLicence;

PRINT 'Sample 5 Clients (with built address):';
SELECT TOP 5 cl.Id, cl.FirstName, cl.MiddleName, cl.LastName, cl.MB, cl.Address, ci.Name AS City, cz.Name AS Citizenship
FROM dbo.Client cl
LEFT JOIN dbo.City        ci ON ci.Id = cl.CityId
LEFT JOIN dbo.Citizenship cz ON cz.Id = cl.CitizenshipId
WHERE cl.Address IS NOT NULL
ORDER BY cl.Id;

PRINT 'Sample ClientPersonalData for Client #1:';
SELECT cp.ClientId, pt.Name AS DocType, cp.Number, di.Name AS Issuer, cp.Active
FROM dbo.ClientPersonalData cp
JOIN dbo.PersonalDataType pt ON pt.Id = cp.PersonalDataTypeId
JOIN dbo.DocumentIssuer   di ON di.Id = cp.DocumentIssuerId
WHERE cp.ClientId = (SELECT MIN(Id) FROM dbo.Client);

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
