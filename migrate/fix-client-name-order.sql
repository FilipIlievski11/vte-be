-- fix-client-name-order.sql
-- ---------------------------------------------------------------------------------------
-- Legacy `Customers` stored most PERSON names surname-first: `CustomerFirstName` actually
-- held the surname (Презиме) and `CustomerSurname` the given name (Име). migrate-clients.sql
-- copied the columns verbatim, so v2 `Client` ended up with FirstName = surname for the
-- overwhelming majority of persons (verified: a random 20/20 sample was surname-first;
-- ~26.7k of ~30k persons by suffix analysis).
--
-- This corrects the order by swapping FirstName <-> LastName for PERSONS (Business = 0),
-- EXCEPT rows already in the correct (given, surname) order — detected as "FirstName is
-- NOT a Macedonian surname AND LastName IS". Ambiguous rows (e.g. Albanian names with no
-- Slavic suffix) are also swapped, because the sample shows those are reversed too.
--
-- SAFETY:
--   * A full pre-swap backup of (Id, FirstName, LastName) is written to
--     dbo.Client_NameBackup_idlfix. To revert:
--        UPDATE c SET c.FirstName=b.FirstName, c.LastName=b.LastName
--        FROM dbo.Client c JOIN dbo.Client_NameBackup_idlfix b ON b.Id=c.Id;
--   * Guarded by a DB-level extended property so a second run is a NO-OP — the swap is
--     NOT idempotent (running twice would undo it).
--
-- Applied to PROD 2026-07-01 (30,109 rows swapped). This script also brings a fresh
-- migrate/ run (LocalDB) into the same corrected state.
-- ---------------------------------------------------------------------------------------
SET NOCOUNT ON;

IF EXISTS (SELECT 1 FROM sys.fn_listextendedproperty(N'ClientNameOrderFixed', NULL, NULL, NULL, NULL, NULL, NULL))
BEGIN
    PRINT 'ClientNameOrderFixed already applied - skipping.';
    RETURN;
END

IF OBJECT_ID('dbo.Client_NameBackup_idlfix') IS NOT NULL DROP TABLE dbo.Client_NameBackup_idlfix;
SELECT Id, FirstName, LastName INTO dbo.Client_NameBackup_idlfix FROM dbo.Client;
PRINT 'Backed up: ' + CAST(@@ROWCOUNT AS varchar(20));

UPDATE dbo.Client
SET FirstName = LastName, LastName = FirstName
WHERE Business = 0
  AND FirstName IS NOT NULL AND LastName IS NOT NULL
  AND NOT (
        FirstName NOT LIKE N'%ски' AND FirstName NOT LIKE N'%ска'
    AND FirstName NOT LIKE N'%ов'  AND FirstName NOT LIKE N'%ова'
    AND FirstName NOT LIKE N'%ев'  AND FirstName NOT LIKE N'%ева'
    AND FirstName NOT LIKE N'%иќ'  AND FirstName NOT LIKE N'%оски'
    AND ( LastName LIKE N'%ски' OR LastName LIKE N'%ска'
       OR LastName LIKE N'%ов'  OR LastName LIKE N'%ова'
       OR LastName LIKE N'%ев'  OR LastName LIKE N'%ева'
       OR LastName LIKE N'%иќ'  OR LastName LIKE N'%оски' )
  );
PRINT 'Swapped: ' + CAST(@@ROWCOUNT AS varchar(20));

EXEC sys.sp_addextendedproperty @name = N'ClientNameOrderFixed', @value = N'2026-07-01';
PRINT 'Marker set.';
