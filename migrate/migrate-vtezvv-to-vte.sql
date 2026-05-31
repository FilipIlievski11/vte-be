-- =============================================================================
-- VTEZVV (legacy)  →  VTE (new schema)
-- Source: (localdb)\MSSQLLocalDB\VTEZVV_Snapshot  (taken via snapshot-legacy.ps1)
-- Target: (localdb)\MSSQLLocalDB\VTE              (the new clean schema)
--
-- DESTRUCTIVE: wipes Company, Station, Country, Citizenship, Community, City,
-- DocumentIssuer, Client, ClientPersonalData before inserting. Re-run safe.
-- AspNet* + PersonalDataType (seed rows 1/2/3) are left alone.
--
-- Apply with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\migrate-vtezvv-to-vte.sql -b -X -I
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

-- =============================================================================
-- 0. Sanity checks
-- =============================================================================
IF DB_ID('VTEZVV_Snapshot') IS NULL
BEGIN
  RAISERROR('Source DB VTEZVV_Snapshot is missing. Run migrate\snapshot-legacy.ps1 first.', 16, 1);
  RETURN;
END;

PRINT '=== Wiping target tables in FK order ===';
DELETE FROM dbo.ClientPersonalData;
DELETE FROM dbo.Client;
DELETE FROM dbo.Citizenship;
DELETE FROM dbo.DocumentIssuer;
DELETE FROM dbo.City;
DELETE FROM dbo.Community;
DELETE FROM dbo.Country;
DELETE FROM dbo.Station;
DELETE FROM dbo.Company;
-- PersonalDataType stays (seeded with 1=Driving Licence, 2=Passport, 3=Personal Id).

DBCC CHECKIDENT ('dbo.Company',            RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Station',            RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Country',            RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Community',          RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.City',               RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Citizenship',        RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.DocumentIssuer',     RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.Client',             RESEED, 0) WITH NO_INFOMSGS;
DBCC CHECKIDENT ('dbo.ClientPersonalData', RESEED, 0) WITH NO_INFOMSGS;

-- =============================================================================
-- 1. Companies — legacy has 4 rows, all fit in tinyint. Preserve Ids.
-- =============================================================================
PRINT '=== Companies ===';
SET IDENTITY_INSERT dbo.Company ON;
INSERT INTO dbo.Company (Id, Name, CreatedAt, Active)
SELECT CAST(c.Id AS tinyint), c.CompanyName, GETUTCDATE(), c.Active
FROM VTEZVV_Snapshot.dbo.Companies c
WHERE c.Id BETWEEN 1 AND 255;
SET IDENTITY_INSERT dbo.Company OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 2. Stations — legacy has no Stations table. Create one default Station
--    per Company, name = CompanyName. Adjust by hand later if needed.
-- =============================================================================
PRINT '=== Stations (one per Company, defaulted) ===';
INSERT INTO dbo.Station (CompanyId, Name, Active)
SELECT CAST(Id AS tinyint), CompanyName, Active
FROM VTEZVV_Snapshot.dbo.Companies
WHERE Id BETWEEN 1 AND 255;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 3. Countries — preserve Ids (49 rows, all fit in smallint).
-- =============================================================================
PRINT '=== Countries ===';
SET IDENTITY_INSERT dbo.Country ON;
INSERT INTO dbo.Country (Id, Name, ShortName, Active)
SELECT CAST(c.Id AS smallint), c.CountryName, c.CountryShortName, c.Active
FROM VTEZVV_Snapshot.dbo.Countries c
WHERE c.Id BETWEEN 1 AND 32767;
SET IDENTITY_INSERT dbo.Country OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 4. Citizenships — derived from legacy Countries.Citizenship column.
--    Use Country.Id as Citizenship.Id (tinyint). Legacy has ≤49 Countries.
-- =============================================================================
PRINT '=== Citizenships (derived from Countries.Citizenship) ===';
SET IDENTITY_INSERT dbo.Citizenship ON;
INSERT INTO dbo.Citizenship (Id, CountryId, Name)
SELECT CAST(c.Id AS tinyint), CAST(c.Id AS smallint), c.Citizenship
FROM VTEZVV_Snapshot.dbo.Countries c
WHERE c.Id BETWEEN 1 AND 255
  AND c.Citizenship IS NOT NULL
  AND LEN(LTRIM(RTRIM(c.Citizenship))) > 0;
SET IDENTITY_INSERT dbo.Citizenship OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 5. Communities — legacy has no CountryId; default to Macedonia.
-- =============================================================================
DECLARE @MacedoniaId smallint = (
  SELECT TOP 1 Id FROM dbo.Country
  WHERE ShortName IN (N'MK', N'MKD')
     OR Name LIKE N'%МАКЕДОН%'
     OR Name LIKE N'%MACEDON%'
  ORDER BY Id
);
IF @MacedoniaId IS NULL
BEGIN
  RAISERROR('Macedonia not found in Country table — cannot default CountryId for Communities.', 16, 1);
  RETURN;
END;
PRINT CONCAT('=== Communities (defaulting CountryId = ', @MacedoniaId, ' / Macedonia) ===');

SET IDENTITY_INSERT dbo.Community ON;
INSERT INTO dbo.Community (Id, Name, CountryId, Code, PlateNumberPrefix, Active)
SELECT
  c.Id,
  c.CommunityName,
  @MacedoniaId,
  NULLIF(LTRIM(RTRIM(c.CommunityCode)), ''),
  NULLIF(LTRIM(RTRIM(c.RegistrationCode)), ''),
  c.Active
FROM VTEZVV_Snapshot.dbo.Communities c;
SET IDENTITY_INSERT dbo.Community OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 6. Cities — preserve Ids; skip rows where IdCommunityCode is missing or
--    points at a non-existent Community.
-- =============================================================================
PRINT '=== Cities ===';
SET IDENTITY_INSERT dbo.City ON;
INSERT INTO dbo.City (Id, Name, CommunityId, PostalCode, Active)
SELECT
  c.Id,
  c.CityName,
  c.IdCommunityCode,
  CASE
    WHEN c.CityZip IS NULL OR c.CityZip = 0 THEN N'0000'
    ELSE CAST(c.CityZip AS nvarchar(20))
  END,
  c.Active
FROM VTEZVV_Snapshot.dbo.Cities c
INNER JOIN dbo.Community comm ON comm.Id = c.IdCommunityCode;
SET IDENTITY_INSERT dbo.City OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows (legacy total: ',
            (SELECT COUNT(*) FROM VTEZVV_Snapshot.dbo.Cities), ')');

-- =============================================================================
-- 7. DocumentIssuers — from legacy RegistrationIssuers.
--    Drop IdCommunity (no equivalent). Cap to tinyint range (255).
-- =============================================================================
PRINT '=== DocumentIssuers (from RegistrationIssuers) ===';
SET IDENTITY_INSERT dbo.DocumentIssuer ON;
INSERT INTO dbo.DocumentIssuer (Id, Name, Active)
SELECT CAST(Id AS tinyint), IssuerName, Active
FROM VTEZVV_Snapshot.dbo.RegistrationIssuers
WHERE Id BETWEEN 1 AND 255;
SET IDENTITY_INSERT dbo.DocumentIssuer OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 8. Clients — preserve legacy Customer.Id (bigint).
--    All assigned to CompanyId = 1 (single-tenant legacy DB).
--    FK columns NULL'd when they don't resolve in the new tables.
-- =============================================================================
PRINT '=== Clients ===';
SET IDENTITY_INSERT dbo.Client ON;
INSERT INTO dbo.Client (
  Id, CompanyId, CityId, CitizenshipId, Business,
  FirstName, MiddleName, LastName, MB, Address,
  TaxNumber, PhoneNumber, Email, DateOfBirth, Note,
  Active, CreatedAt
)
SELECT
  c.Id,
  CAST(1 AS tinyint) AS CompanyId,
  CASE WHEN ci.Id IS NOT NULL THEN c.IdLivingCity END                              AS CityId,
  CASE WHEN cz.Id IS NOT NULL THEN CAST(c.IdCitizenship AS tinyint) END            AS CitizenshipId,
  c.IsCompany                                                                      AS Business,
  NULLIF(LTRIM(RTRIM(c.CustomerFirstName)), N'')                                   AS FirstName,
  NULLIF(LTRIM(RTRIM(c.ParentName)),         N'')                                  AS MiddleName,
  NULLIF(LTRIM(RTRIM(c.CustomerSurname)),    N'')                                  AS LastName,
  NULLIF(LTRIM(RTRIM(c.MB)),                 N'')                                  AS MB,
  NULLIF(LTRIM(RTRIM(c.LivingAddressNumber)),N'')                                  AS Address,
  NULLIF(LTRIM(RTRIM(c.TaxNumber)),          N'')                                  AS TaxNumber,
  NULLIF(LTRIM(RTRIM(c.PhoneNumber)),        N'')                                  AS PhoneNumber,
  NULLIF(LTRIM(RTRIM(c.eMail)),              N'')                                  AS Email,
  c.DateOfBirth                                                                    AS DateOfBirth,
  NULLIF(LTRIM(RTRIM(c.Note)),               N'')                                  AS Note,
  c.Active                                                                         AS Active,
  GETUTCDATE()                                                                     AS CreatedAt
FROM VTEZVV_Snapshot.dbo.Customers c
LEFT JOIN dbo.City        ci ON ci.Id = NULLIF(c.IdLivingCity, 0)
LEFT JOIN dbo.Citizenship cz ON cz.Id = CASE
                                          WHEN c.IdCitizenship BETWEEN 1 AND 255
                                          THEN CAST(c.IdCitizenship AS tinyint)
                                        END;
SET IDENTITY_INSERT dbo.Client OFF;
PRINT CONCAT('  -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 9. ClientPersonalData — explode three legacy columns (BLK / Passport /
--    DriveingLicence) into one ClientPersonalData row per non-empty value.
--    PersonalDataType seeds: 1=Driving Licence, 2=Passport, 3=Personal Id.
--    Missing issuer → fall back to DocumentIssuer Id 1.
-- =============================================================================
DECLARE @FallbackIssuer tinyint = (SELECT TOP 1 Id FROM dbo.DocumentIssuer ORDER BY Id);
IF @FallbackIssuer IS NULL
BEGIN
  RAISERROR('No DocumentIssuer rows available to use as fallback.', 16, 1);
  RETURN;
END;

PRINT '=== ClientPersonalData ===';

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
PRINT CONCAT('  Personal Id  -> ', @@ROWCOUNT, ' rows');

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
PRINT CONCAT('  Passport     -> ', @@ROWCOUNT, ' rows');

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
PRINT CONCAT('  Driving Lic. -> ', @@ROWCOUNT, ' rows');

-- =============================================================================
-- 10. Final row-count verification
-- =============================================================================
PRINT '';
PRINT '=== Row counts ===';
SELECT [Table] = N'Company',            [Rows] = (SELECT COUNT(*) FROM dbo.Company)
UNION ALL SELECT N'Station',                    (SELECT COUNT(*) FROM dbo.Station)
UNION ALL SELECT N'Country',                    (SELECT COUNT(*) FROM dbo.Country)
UNION ALL SELECT N'Citizenship',                (SELECT COUNT(*) FROM dbo.Citizenship)
UNION ALL SELECT N'Community',                  (SELECT COUNT(*) FROM dbo.Community)
UNION ALL SELECT N'City',                       (SELECT COUNT(*) FROM dbo.City)
UNION ALL SELECT N'DocumentIssuer',             (SELECT COUNT(*) FROM dbo.DocumentIssuer)
UNION ALL SELECT N'Client',                     (SELECT COUNT(*) FROM dbo.Client)
UNION ALL SELECT N'ClientPersonalData',         (SELECT COUNT(*) FROM dbo.ClientPersonalData);

COMMIT;
PRINT '=== Migration complete ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
