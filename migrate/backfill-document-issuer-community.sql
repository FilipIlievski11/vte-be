-- =============================================================================
-- Restore DocumentIssuer.CommunityId — the legacy RegistrationIssuers.IdCommunity
-- link that migrate-document-issuers.sql dropped ("no equivalent in the new schema").
-- It maps each registration issuer (MVR office) to the community it serves, so the
-- Plav print can resolve the NEW owner's destination MVR ("ДО МВР") instead of
-- echoing the old registration's issuer.
--
-- Issuer names + ids already align 1:1 between legacy RegistrationIssuers and v2
-- DocumentIssuer (the migration preserved both), so ONLY the integer community link
-- is restored here — no Cyrillic transfer. Community ids are identical across legacy
-- and v2. The CommunityId column is added by the EF migration AddDocumentIssuerCommunityId.
--
-- Idempotent (plain UPDATE by Id; re-runnable). Generated from live legacy VTEZVV
-- (195.26.159.162,7899) RegistrationIssuers WHERE IdCommunity > 0 (83 rows).
--   sqlcmd -S <server> -U sa -P <pw> -C -d VTE -i migrate\backfill-document-issuer-community.sql -b
-- =============================================================================
SET NOCOUNT ON;

UPDATE di SET di.CommunityId = v.CommunityId
FROM dbo.DocumentIssuer di
JOIN (VALUES
  (1,21),(2,87),(3,62),(4,13),(5,25),(6,26),(7,42),(8,44),
  (9,46),(10,48),(11,51),(12,66),(13,68),(14,75),(15,76),(16,79),
  (17,84),(18,101),(19,90),(20,103),(21,58),(22,21),(23,21),(24,21),
  (25,72),(26,18),(27,12),(28,30),(29,22),(30,70),(31,32),(32,56),
  (33,50),(34,28),(35,47),(36,55),(37,64),(38,67),(39,95),(40,2),
  (41,21),(42,27),(43,81),(44,13),(45,87),(46,66),(47,76),(48,87),
  (49,87),(50,72),(51,30),(52,48),(53,21),(54,84),(55,62),(56,75),
  (57,70),(58,32),(59,44),(60,42),(61,47),(62,46),(63,51),(64,12),
  (65,68),(66,76),(67,25),(68,26),(69,79),(70,58),(71,72),(72,56),
  (73,22),(74,50),(75,28),(76,64),(77,55),(78,67),(79,101),(80,48),
  (81,31),(82,20),(83,18)
) v(Id, CommunityId) ON v.Id = di.Id;

DECLARE @n int = @@ROWCOUNT;
PRINT '=== DocumentIssuer.CommunityId backfilled: ' + CAST(@n AS varchar(10)) + ' rows ===';
