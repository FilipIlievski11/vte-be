-- =============================================================================
-- VTE — performance indexes for the list/search hot paths.
--
-- Idempotent: each create is gated by `sys.indexes` lookup, so re-running
-- this script is a no-op once everything is in place.
--
-- Targeted queries (see backend-v2 controllers):
--   1. GET /api/vehicles      — relation-driven, joins Vehicle + Maker/Model + Client
--   2. GET /api/clients       — paged search by name/MB/tax #
--   3. GET /api/users         — admin user list (AspNetUsers + UserRoles + Roles)
--   4. GET /api/vehicle-registrations?vehicleId=…
--   5. GET /api/client-vehicle-relations?vehicleId=…|clientId=…
--
-- Notes:
--   - LIKE '%foo%' (leading wildcard) cannot use an index seek, but an index
--     scan is much smaller than a heap scan; narrow indexes on the LIKE-searched
--     columns are still worth it.
--   - INCLUDE-style covering indexes are kept minimal so we don't bloat
--     write-heavy tables.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\perf-indexes-vte.sql -b -X -I
-- =============================================================================
USE VTE;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @created int = 0;
DECLARE @existed int = 0;
DECLARE @sql nvarchar(max);
DECLARE @tbl sysname, @ix sysname, @def nvarchar(max);

DECLARE @indexes TABLE (TableName sysname, IndexName sysname, Sql nvarchar(max));

INSERT INTO @indexes VALUES
  -- ============================================================
  -- Client — most-searched entity (32k+ rows)
  -- ============================================================
  ('Client', 'IX_Client_CompanyId',
   'CREATE INDEX IX_Client_CompanyId ON dbo.Client(CompanyId) INCLUDE (Active);'),
  ('Client', 'IX_Client_MB',
   'CREATE INDEX IX_Client_MB ON dbo.Client(MB) WHERE MB IS NOT NULL;'),
  ('Client', 'IX_Client_LastName',
   'CREATE INDEX IX_Client_LastName ON dbo.Client(LastName) WHERE LastName IS NOT NULL;'),
  ('Client', 'IX_Client_FirstName',
   'CREATE INDEX IX_Client_FirstName ON dbo.Client(FirstName) WHERE FirstName IS NOT NULL;'),
  ('Client', 'IX_Client_TaxNumber',
   'CREATE INDEX IX_Client_TaxNumber ON dbo.Client(TaxNumber) WHERE TaxNumber IS NOT NULL;'),
  ('Client', 'IX_Client_CityId',
   'CREATE INDEX IX_Client_CityId ON dbo.Client(CityId) WHERE CityId IS NOT NULL;'),
  ('Client', 'IX_Client_CitizenshipId',
   'CREATE INDEX IX_Client_CitizenshipId ON dbo.Client(CitizenshipId) WHERE CitizenshipId IS NOT NULL;'),

  -- ============================================================
  -- ClientPersonalData
  -- ============================================================
  ('ClientPersonalData', 'IX_ClientPersonalData_ClientId_Active',
   'CREATE INDEX IX_ClientPersonalData_ClientId_Active ON dbo.ClientPersonalData(ClientId, Active) INCLUDE (PersonalDataTypeId, DocumentIssuerId);'),
  ('ClientPersonalData', 'IX_ClientPersonalData_PersonalDataTypeId',
   'CREATE INDEX IX_ClientPersonalData_PersonalDataTypeId ON dbo.ClientPersonalData(PersonalDataTypeId);'),
  ('ClientPersonalData', 'IX_ClientPersonalData_DocumentIssuerId',
   'CREATE INDEX IX_ClientPersonalData_DocumentIssuerId ON dbo.ClientPersonalData(DocumentIssuerId);'),

  -- ============================================================
  -- Geography (small but joined often)
  -- ============================================================
  ('City',       'IX_City_CommunityId',     'CREATE INDEX IX_City_CommunityId      ON dbo.City(CommunityId);'),
  ('Community',  'IX_Community_CountryId',  'CREATE INDEX IX_Community_CountryId   ON dbo.Community(CountryId);'),
  ('Citizenship','IX_Citizenship_CountryId','CREATE INDEX IX_Citizenship_CountryId ON dbo.Citizenship(CountryId);'),

  -- ============================================================
  -- Vehicle (EF already added IX on Vin, Plate, CompanyId)
  -- ============================================================
  ('Vehicle', 'IX_Vehicle_Active',
   'CREATE INDEX IX_Vehicle_Active ON dbo.Vehicle(Active) WHERE Active = 1;'),

  -- ============================================================
  -- VehicleModel + VehicleMaker — LIKE-searched
  -- ============================================================
  ('VehicleModel', 'IX_VehicleModel_Name',
   'CREATE INDEX IX_VehicleModel_Name ON dbo.VehicleModel(Name);'),
  ('VehicleMaker', 'IX_VehicleMaker_Name',
   'CREATE INDEX IX_VehicleMaker_Name ON dbo.VehicleMaker(Name);'),

  -- ============================================================
  -- VehicleRegistration
  -- ============================================================
  ('VehicleRegistration', 'IX_VehicleRegistration_IssuerId',
   'CREATE INDEX IX_VehicleRegistration_IssuerId ON dbo.VehicleRegistration(IssuerId);'),
  ('VehicleRegistration', 'IX_VehicleRegistration_VehicleId_Active',
   'CREATE INDEX IX_VehicleRegistration_VehicleId_Active ON dbo.VehicleRegistration(VehicleId, Active) INCLUDE (PlateNumber, RegisteredDate, ValidUntil);'),

  -- ============================================================
  -- ClientVehicleRelation — driver of /api/vehicles
  -- ============================================================
  ('ClientVehicleRelation', 'IX_CVR_VehicleId_Active',
   'CREATE INDEX IX_CVR_VehicleId_Active ON dbo.ClientVehicleRelation(VehicleId, Active) INCLUDE (ClientId, RelationTypeId, StartDate);'),
  ('ClientVehicleRelation', 'IX_CVR_ClientId_Active',
   'CREATE INDEX IX_CVR_ClientId_Active ON dbo.ClientVehicleRelation(ClientId, Active) INCLUDE (VehicleId, RelationTypeId, StartDate);'),
  ('ClientVehicleRelation', 'IX_CVR_RelationTypeId',
   'CREATE INDEX IX_CVR_RelationTypeId ON dbo.ClientVehicleRelation(RelationTypeId);'),
  ('ClientVehicleRelation', 'IX_CVR_VehicleNotNull',
   'CREATE INDEX IX_CVR_VehicleNotNull ON dbo.ClientVehicleRelation(VehicleId, RelationTypeId, Active) INCLUDE (ClientId, StartDate) WHERE VehicleId IS NOT NULL;'),

  -- ============================================================
  -- AspNetUsers — admin user list endpoint
  -- ============================================================
  ('AspNetUsers', 'IX_AspNetUsers_CompanyId',
   'CREATE INDEX IX_AspNetUsers_CompanyId ON dbo.AspNetUsers(CompanyId) WHERE CompanyId IS NOT NULL;'),
  ('AspNetUsers', 'IX_AspNetUsers_FullName',
   'CREATE INDEX IX_AspNetUsers_FullName ON dbo.AspNetUsers(FullName) WHERE FullName IS NOT NULL;');

DECLARE c CURSOR LOCAL FAST_FORWARD FOR
  SELECT TableName, IndexName, Sql FROM @indexes;
OPEN c;
FETCH NEXT FROM c INTO @tbl, @ix, @sql;
WHILE @@FETCH_STATUS = 0
BEGIN
  IF OBJECT_ID('dbo.' + QUOTENAME(@tbl)) IS NULL
  BEGIN
    PRINT CONCAT('  SKIP (no table): ', @tbl, '.', @ix);
  END
  ELSE IF EXISTS (
    SELECT 1 FROM sys.indexes i
    WHERE i.object_id = OBJECT_ID('dbo.' + QUOTENAME(@tbl))
      AND i.name = @ix
  )
  BEGIN
    PRINT CONCAT('  exists: ', @tbl, '.', @ix);
    SET @existed = @existed + 1;
  END
  ELSE
  BEGIN
    BEGIN TRY
      EXEC sp_executesql @sql;
      PRINT CONCAT('  CREATED: ', @tbl, '.', @ix);
      SET @created = @created + 1;
    END TRY
    BEGIN CATCH
      PRINT CONCAT('  FAILED: ', @tbl, '.', @ix, ' -- ', ERROR_MESSAGE());
    END CATCH;
  END;
  FETCH NEXT FROM c INTO @tbl, @ix, @sql;
END;
CLOSE c;
DEALLOCATE c;

PRINT '';
PRINT CONCAT('=== Done: ', @created, ' created, ', @existed, ' already present. ===');
GO

-- ---------------------------------------------------------------------------
-- Statistics refresh: brings the planner up to date so it picks the new indexes.
-- ---------------------------------------------------------------------------
PRINT '';
PRINT 'Refreshing statistics (sampled, fast)...';
EXEC sp_updatestats;
PRINT 'Done.';
GO
