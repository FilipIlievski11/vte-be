/*
  backfill-pricecatalog-rules.sql

  Populates the rule-evaluator columns added to PriceCatalog in Phase 3
  (Trigger, VehiclePaymentCategoryId, CommunityId, PaymentCategoryGroupId,
  VehicleField, ParametarFrom, ParametarTo) from the legacy hierarchy:

    PaymentCategories  ─→  PaymentItems  ─→  PaymentItemParametars
      (trigger flags +        (vehicle-category +    (vehicle-field +
       community)              payment-category id)   from/to + price)

  Source rows: VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars (= our PriceCatalog.Id).
  Idempotent: only updates rows whose Id matches a legacy parametar.

  Run AFTER:
    1. AddCustomerDebtsAndExpandPriceCatalog EF migration
    2. (optional) migrate-payments.sql — needed if PriceCatalog is empty
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;
PRINT '== backfill-pricecatalog-rules start =='

-- 1. Stage the legacy hierarchy joined into one wide row per PaymentItemParametar.
PRINT '1. Staging legacy rule rows (Parametars JOIN Items JOIN Categories)'

SELECT
    pip.Id                                                 AS PriceCatalogId,
    pi.IdVehicleCategoryForPayments                        AS VehiclePaymentCategoryId,
    pc.IdCommunity                                         AS CommunityId,
    pc.Id                                                  AS PaymentCategoryGroupId,
    -- Translate legacy Vehicle property names to v2 names AND null-out the literal "Null" string.
    -- Legacy NULL / empty / 'Null' all mean "fixed fee — always applies once category matches".
    CASE LTRIM(RTRIM(ISNULL(pip.VehicleField, N'')))
        WHEN N''                       THEN NULL
        WHEN N'Null'                   THEN NULL
        WHEN N'NumberOfSeats'          THEN N'Seats'
        WHEN N'NumberOfStandingSeats'  THEN N'StandingSeats'
        WHEN N'MaximunAllowedWaight'   THEN N'MaxAllowedWeightKg'     -- (legacy typo)
        WHEN N'TotalWaight'            THEN N'MaxLegalTotalMassKg'
        WHEN N'EnginePowerOutPut'      THEN N'EnginePowerKw'
        WHEN N'EngineWorkingCapacity'  THEN N'EngineWorkingCapacityCc'
        WHEN N'IdVehicleCategories'    THEN N'CategoryId'
        WHEN N'IdVehicleBodyType'      THEN N'BodyTypeId'
        WHEN N'IdEnginePowerSource'    THEN N'FuelId'
        ELSE LEFT(LTRIM(RTRIM(pip.VehicleField)), 60)               -- pass-through (already matches or unknown)
    END                                                    AS VehicleField,
    pip.ParametarFrom                                      AS ParametarFrom,
    pip.ParametarTo                                        AS ParametarTo,
    -- Pick ONE trigger value based on the PaymentCategory's TrigerdBy* flags.
    -- Order of precedence: IrregularTechExam → TechExam → Request → TrafficLicence → Permission → IDL.
    CASE
        WHEN COALESCE(pc.TrigerdByIrregularTechnicalExam, 0) = 1 THEN CONVERT(tinyint, 6)
        WHEN COALESCE(pc.TrigerdByTechnicalExam,          0) = 1 THEN CONVERT(tinyint, 1)
        WHEN COALESCE(pc.TrigerdByRequest,                0) = 1 THEN CONVERT(tinyint, 2)
        WHEN COALESCE(pc.TrigerdByTrafficLicence,         0) = 1 THEN CONVERT(tinyint, 3)
        WHEN COALESCE(pc.TrigerdByPremisionForVehicle,    0) = 1 THEN CONVERT(tinyint, 4)
        WHEN COALESCE(pc.TrigerdByInternationalDrivierLicence, 0) = 1 THEN CONVERT(tinyint, 5)
        ELSE CONVERT(tinyint, 0)   -- None
    END                                                    AS Trigger_
INTO #rules
FROM VTEZVV_LIVE.VTEZVV.dbo.PaymentItemParametars pip
LEFT JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentItems       pi ON pi.Id = pip.IdPaymentItem
LEFT JOIN VTEZVV_LIVE.VTEZVV.dbo.PaymentCategories  pc ON pc.Id = pi.IdPymentCategory;

CREATE UNIQUE CLUSTERED INDEX IX_rules ON #rules(PriceCatalogId);

DECLARE @staged int = (SELECT COUNT(*) FROM #rules);
PRINT '   Staged rules: ' + CAST(@staged AS varchar);

-- 2. Apply to PriceCatalog. NULL-safe: leave columns NULL where the legacy field was NULL/0
--    (treated as "no filter" by the evaluator).
PRINT '2. Updating PriceCatalog'

UPDATE pc
SET
    pc.[Trigger]                  = r.Trigger_,
    pc.VehiclePaymentCategoryId   = NULLIF(r.VehiclePaymentCategoryId, 0),
    pc.CommunityId                = NULLIF(r.CommunityId, 0),
    pc.PaymentCategoryGroupId     = NULLIF(r.PaymentCategoryGroupId, 0),
    pc.VehicleField               = r.VehicleField,
    pc.ParametarFrom              = r.ParametarFrom,
    pc.ParametarTo                = r.ParametarTo
FROM dbo.PriceCatalog pc
INNER JOIN #rules r ON r.PriceCatalogId = pc.Id;

DECLARE @updated int = @@ROWCOUNT;
PRINT '   Updated rows: ' + CAST(@updated AS varchar);

DROP TABLE #rules;

PRINT ''
PRINT '== backfill complete =='
SELECT 'Total PriceCatalog rows'             AS metric, COUNT(*) AS rows_ FROM dbo.PriceCatalog
UNION ALL
SELECT '  with Trigger != None',                       COUNT(*) FROM dbo.PriceCatalog WHERE [Trigger] <> 0
UNION ALL
SELECT '  with VehiclePaymentCategoryId set',          COUNT(*) FROM dbo.PriceCatalog WHERE VehiclePaymentCategoryId IS NOT NULL
UNION ALL
SELECT '  with VehicleField set (ranged rule)',        COUNT(*) FROM dbo.PriceCatalog WHERE VehicleField IS NOT NULL
UNION ALL
SELECT '  TechnicalExam-trigger rules',                COUNT(*) FROM dbo.PriceCatalog WHERE [Trigger] = 1
UNION ALL
SELECT '  Request-trigger rules',                      COUNT(*) FROM dbo.PriceCatalog WHERE [Trigger] = 2
UNION ALL
SELECT '  IrregularTechExam-trigger rules',            COUNT(*) FROM dbo.PriceCatalog WHERE [Trigger] = 6;
