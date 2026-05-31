-- =============================================================================
-- Seed dbo.Citizenship from Country list, with proper Macedonian adjectival
-- forms (the legacy Countries.Citizenship column is unusable — mostly "нема").
-- Citizenship.Id is set to match Country.Id 1:1, so a Country row's Id is
-- also its Citizenship row's Id.
--
-- Idempotent — wipes target first. Refuses to wipe if any Client points at
-- a Citizenship row.
-- =============================================================================
USE VTE;
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION;

DECLARE @inUse int = (SELECT COUNT(*) FROM dbo.Client WHERE CitizenshipId IS NOT NULL);
IF @inUse > 0
BEGIN
  RAISERROR('Refusing to wipe Citizenship: %d Client rows have CitizenshipId set. Clear them first.', 16, 1, @inUse);
  RETURN;
END;

PRINT '=== Wiping dbo.Citizenship ===';
DELETE FROM dbo.Citizenship;
DBCC CHECKIDENT ('dbo.Citizenship', RESEED, 0) WITH NO_INFOMSGS;

PRINT '=== Inserting curated citizenships (Citizenship.Id = Country.Id) ===';

-- Mapping table: (CountryId, CitizenshipName).
-- Every CountryId here must exist in dbo.Country — INNER JOIN below enforces this.
SET IDENTITY_INSERT dbo.Citizenship ON;

INSERT INTO dbo.Citizenship (Id, CountryId, Name)
SELECT
  CAST(v.CountryId AS tinyint),
  CAST(v.CountryId AS smallint),
  v.Name
FROM (VALUES
  (37,  N'Австриско'),                 -- АВСТРИЈА
  (38,  N'Белгиско'),                  -- БЕЛГИЈА
  (39,  N'Белоруско'),                 -- БЕЛОРУСИЈА
  (40,  N'Босанско'),                  -- БОСНА И ХЕРЦЕГОВИНА
  (41,  N'Бугарско'),                  -- БУГАРИЈА
  (42,  N'Британско'),                 -- ВЕЛИКА БРИТАНИЈА
  (43,  N'Германско'),                 -- ГЕРМАНИЈА
  (44,  N'Грчко'),                     -- ГРЦИЈА
  (45,  N'Албанско'),                  -- АЛБАНИЈА
  (46,  N'Данско'),                    -- ДАНСКА
  (47,  N'Индиско'),                   -- ИНДИЈА
  (48,  N'Ирско'),                     -- ИРСКА
  (49,  N'Италијанско'),               -- ИТАЛИЈА
  (50,  N'Јапонско'),                  -- ЈАПОНИЈА
  (51,  N'Корејско'),                  -- КОРЕЈА
  (52,  N'Канадско'),                  -- КАНАДА
  (53,  N'Кинеско'),                   -- КИНА
  (54,  N'Кипарско'),                  -- КИПАР
  (56,  N'Норвешко'),                  -- НОРВЕШКА
  (57,  N'Полско'),                    -- ПОЛСКА
  (58,  N'Романско'),                  -- РОМАНИЈА
  (59,  N'Руско'),                     -- РУСИЈА
  (60,  N'Американско'),               -- САД
  (61,  N'Словачко'),                  -- СЛОВАЧКА
  (62,  N'Словенечко'),                -- СЛОВЕНИЈА
  (63,  N'Југословенско'),             -- ЈУГОСЛАВИЈА (defunct, kept for legacy data)
  (65,  N'Тајландско'),                -- ТАЈЛАНД
  (66,  N'Турско'),                    -- ТУРЦИЈА
  (67,  N'Украинско'),                 -- УКРАИНА
  (68,  N'Унгарско'),                  -- УНГАРИЈА
  (69,  N'Финско'),                    -- ФИНСКА
  (70,  N'Француско'),                 -- ФРАНЦИЈА
  (71,  N'Холандско'),                 -- ХОЛАНДИЈА
  (72,  N'Хрватско'),                  -- ХРВАТСКА
  (73,  N'Чешко'),                     -- ЧЕШКА
  (74,  N'Шпанско'),                   -- ШПАНИЈА
  (75,  N'Шведско'),                   -- ШВЕДСКА
  (76,  N'Швајцарско'),                -- ШВАЈЦАРИЈА
  (77,  N'Македонско'),                -- Република Северна Македонија
  (78,  N'Непознато'),                 -- НЕПОЗНАТА (placeholder)
  (79,  N'тест'),                      -- тест (placeholder — review)
  (80,  N'Малезиско'),                 -- МАЛЕЗИЈА
  (81,  N'Црногорско'),                -- ЦРНА ГОРА
  (82,  N'Српско'),                    -- СРБИЈА
  (83,  N'Нема'),                      -- НЕМА (placeholder)
  (84,  N'Македонско'),                -- МАК (abbrev)
  (85,  N'Американско'),               -- АМЕРИКА
  (86,  N'Тајванско'),                 -- ТАЈВАН
  (87,  N'Македонско')                 -- МАКЕДОНИЈА (the example from the user)
) v (CountryId, Name)
INNER JOIN dbo.Country c ON c.Id = CAST(v.CountryId AS smallint);

DECLARE @rows int = @@ROWCOUNT;
SET IDENTITY_INSERT dbo.Citizenship OFF;

DECLARE @maxId tinyint = (SELECT ISNULL(MAX(Id), 0) FROM dbo.Citizenship);
DBCC CHECKIDENT ('dbo.Citizenship', RESEED, @maxId) WITH NO_INFOMSGS;

PRINT CONCAT('  -> ', @rows, ' rows inserted. Identity counter reseeded to ', @maxId, '.');

COMMIT;

PRINT '';
PRINT '=== Verification ===';
SELECT
  (SELECT COUNT(*) FROM dbo.Country)     AS Countries,
  (SELECT COUNT(*) FROM dbo.Citizenship) AS Citizenships;

PRINT 'Country / Citizenship side-by-side (first 10):';
SELECT TOP 10
  co.Id AS CountryId, co.Name AS Country,
  cz.Id AS CitizenshipId, cz.Name AS Citizenship
FROM dbo.Country co
LEFT JOIN dbo.Citizenship cz ON cz.CountryId = co.Id
ORDER BY co.Id;

PRINT 'Macedonia row:';
SELECT co.Id, co.Name AS Country, cz.Name AS Citizenship
FROM dbo.Country co
LEFT JOIN dbo.Citizenship cz ON cz.CountryId = co.Id
WHERE co.Name LIKE N'%МАКЕДОН%';

PRINT '=== Done ===';
END TRY
BEGIN CATCH
  IF @@TRANCOUNT > 0 ROLLBACK;
  PRINT '!!! Migration failed — rolled back';
  THROW;
END CATCH;
GO
