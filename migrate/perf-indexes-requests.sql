-- =============================================================================
-- VTE — performance indexes for the Requests module.
--
-- Idempotent: each create is gated by `sys.indexes` lookup.
--
-- Targeted queries (see backend-v2 RequestsController):
--   1. GET /api/requests?status=open      — operator workspace, default tab.
--                                           Filtered by Active = 1 AND EndedAt IS NULL.
--   2. GET /api/requests?status=closed    — closed audit tab.
--   3. GET /api/requests?q=...            — search via subqueries against Client name,
--                                           Vehicle VIN/Plate, RequestType name → relation/type ids.
--   4. GET /api/requests/{id}             — single fetch (PK covers).
--   5. GET /api/requests/{id}/print       — same as Get + joins to children.
--   6. Child grids on the form:
--      GET /api/requests/{id}/ownership-proofs  → IX_ROP_RequestId
--      GET /api/requests/{id}/payment-proofs    → IX_RPP_RequestId
--      GET /api/requests/{id}/attachments       → IX_RA_RequestId
--
-- The EF migration already created these indexes via `HasIndex`:
--   - IX_Request_CompanyId_CreatedAt              (composite non-filtered)
--   - IX_Request_Open                              (filtered: Active=1 AND EndedAt IS NULL)
--   - IX_Request_ClientVehicleRelationId
--   - IX_Request_NewClientVehicleRelationId
--   - IX_Request_RequestTypeId
--   - IX_Request_PreviousRegistrationId
--   - IX_RequestOwnershipProof_RequestId
--   - IX_RequestPaymentProof_RequestId
--   - IX_RequestAttachment_RequestId
--
-- This script ADDS covering indexes that EF can't easily express:
--   - INCLUDE columns to make the "open" workspace + closed audit go fully
--     index-only (no key lookups for the most-rendered columns).
--   - A filtered "closed" complement to IX_Request_Open.
--   - A covering index for the relation-anchored fetch.
--
-- Run with:
--   sqlcmd -S "(localdb)\MSSQLLocalDB" -i migrate\perf-indexes-requests.sql -b -X -I
-- =============================================================================
USE VTE;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @created int = 0;
DECLARE @existed int = 0;
DECLARE @sql nvarchar(max);
DECLARE @tbl sysname, @ix sysname;

DECLARE @indexes TABLE (TableName sysname, IndexName sysname, Sql nvarchar(max));

INSERT INTO @indexes VALUES

  -- ==============================================================
  -- Request — operator workspace (open) — covering, filtered
  -- Replaces the slimmer IX_Request_Open created by EF; adds INCLUDE
  -- columns so the list endpoint can render without key lookups.
  -- ==============================================================
  ('Request', 'IX_Request_Open_Covering',
   'CREATE INDEX IX_Request_Open_Covering ON dbo.Request(CompanyId, CreatedAt DESC)
      INCLUDE (RequestTypeId, ClientVehicleRelationId, EndedAt, CreatedByUserId, Note)
      WHERE Active = 1 AND EndedAt IS NULL;'),

  -- ==============================================================
  -- Request — closed audit tab — covering, filtered
  -- Mirrors the open index for the closed-history queries.
  -- ==============================================================
  ('Request', 'IX_Request_Closed_Covering',
   'CREATE INDEX IX_Request_Closed_Covering ON dbo.Request(CompanyId, EndedAt DESC)
      INCLUDE (RequestTypeId, ClientVehicleRelationId, CreatedAt, CreatedByUserId, EndedByUserId, Note)
      WHERE Active = 1 AND EndedAt IS NOT NULL;'),

  -- ==============================================================
  -- Request — anchored-by-relation history (form right rail, links from
  -- the client form''s related-requests view, future audit queries).
  -- ==============================================================
  ('Request', 'IX_Request_Relation_Covering',
   'CREATE INDEX IX_Request_Relation_Covering ON dbo.Request(ClientVehicleRelationId, CreatedAt DESC)
      INCLUDE (RequestTypeId, EndedAt, Active, CreatedByUserId);'),

  -- ==============================================================
  -- Request — anchored by NEW relation (ownership-transfer history)
  -- EF made the unfiltered version; add a filtered narrow one for the
  -- "did this relation come from a transfer?" lookup.
  -- ==============================================================
  ('Request', 'IX_Request_NewRelation_NotNull',
   'CREATE INDEX IX_Request_NewRelation_NotNull ON dbo.Request(NewClientVehicleRelationId)
      INCLUDE (RequestTypeId, EndedAt, CreatedAt)
      WHERE NewClientVehicleRelationId IS NOT NULL;'),

  -- ==============================================================
  -- Request — created-by audit (dashboard "your open requests" tile,
  -- per-operator productivity reports later).
  -- ==============================================================
  ('Request', 'IX_Request_CreatedBy_CreatedAt',
   'CREATE INDEX IX_Request_CreatedBy_CreatedAt ON dbo.Request(CreatedByUserId, CreatedAt DESC)
      INCLUDE (CompanyId, RequestTypeId, EndedAt, Active);'),

  -- ==============================================================
  -- Child collections — narrow active-only filtered indexes.
  -- EF created IX_*_RequestId; these add Active filter so the form''s
  -- active-only renders skip the inactive rows at the index level.
  -- ==============================================================
  ('RequestOwnershipProof', 'IX_ROP_RequestId_Active',
   'CREATE INDEX IX_ROP_RequestId_Active ON dbo.RequestOwnershipProof(RequestId, Active)
      INCLUDE (OwnershipProofTypeId, Detail);'),

  ('RequestPaymentProof', 'IX_RPP_RequestId_Active',
   'CREATE INDEX IX_RPP_RequestId_Active ON dbo.RequestPaymentProof(RequestId, Active)
      INCLUDE (PaymentProofTypeId, Detail);'),

  ('RequestAttachment', 'IX_RA_RequestId_Active',
   'CREATE INDEX IX_RA_RequestId_Active ON dbo.RequestAttachment(RequestId, Active)
      INCLUDE (AttachmentTypeId, FileName, ContentType, SizeBytes, UploadedAt, UploadedByUserId);'),

  -- ==============================================================
  -- Catalog name lookups — for the LIKE-search in the list endpoint
  -- and for the request-types admin grid sort.
  -- ==============================================================
  ('RequestType', 'IX_RequestType_Active_Name',
   'CREATE INDEX IX_RequestType_Active_Name ON dbo.RequestType(Active, Name)
      INCLUDE (DocumentPrintId, TechnicalExamRequirement);');

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
-- Refresh statistics so the planner picks the new indexes.
-- (Sampled, fast — for a full rescan use UPDATE STATISTICS dbo.Request WITH FULLSCAN.)
-- ---------------------------------------------------------------------------
PRINT '';
PRINT 'Refreshing statistics on the Request module tables...';
UPDATE STATISTICS dbo.Request                WITH SAMPLE 100 PERCENT;
UPDATE STATISTICS dbo.RequestOwnershipProof  WITH SAMPLE 100 PERCENT;
UPDATE STATISTICS dbo.RequestPaymentProof    WITH SAMPLE 100 PERCENT;
UPDATE STATISTICS dbo.RequestAttachment      WITH SAMPLE 100 PERCENT;
UPDATE STATISTICS dbo.RequestType            WITH SAMPLE 100 PERCENT;
PRINT 'Done.';
GO
