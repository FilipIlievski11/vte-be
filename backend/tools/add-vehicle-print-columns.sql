-- Adds the 35 print-overlay columns to dbo.Vehicles in VTE2.
-- Idempotent: each ALTER only runs when the column doesn't already exist.
-- Run with: sqlcmd -S "(localdb)\MSSQLLocalDB" -d VTE2 -E -i add-vehicle-print-columns.sql

SET NOCOUNT ON;

DECLARE @sql nvarchar(max) = N'';

;WITH cols(name, def) AS (
    SELECT 'Tip',                                       'nvarchar(150) NULL' UNION ALL
    SELECT 'BrojEUPotvrda',                             'nvarchar(50)  NULL' UNION ALL
    SELECT 'OznakaNaOdobrenie',                         'nvarchar(50)  NULL' UNION ALL
    SELECT 'OznakaNaOdobrenieZaPriklucUred',            'nvarchar(100) NULL' UNION ALL
    SELECT 'IdentifikacijaNaMotorMestoMetod',           'nvarchar(100) NULL' UNION ALL
    SELECT 'TBrOdobrenieMehanPriklucok',                'nvarchar(100) NULL' UNION ALL
    SELECT 'TMarkaMehanPriklucok',                      'nvarchar(100) NULL' UNION ALL
    SELECT 'TTipMehanPriklucok',                        'nvarchar(100) NULL' UNION ALL
    SELECT 'TZastitnaKabina',                           'nvarchar(100) NULL' UNION ALL
    SELECT 'TZastitnaRamka',                            'nvarchar(100) NULL' UNION ALL
    SELECT 'NoiseTechnicalSpec',                        'nvarchar(100) NULL' UNION ALL
    SELECT 'OdnosKwCcm',                                'nvarchar(20)  NULL' UNION ALL
    SELECT 'EnginePowerOutPut',                         'decimal(18,2) NULL' UNION ALL
    SELECT 'NoiseStatic',                               'decimal(18,2) NULL' UNION ALL
    SELECT 'CO2',                                       'decimal(18,4) NULL' UNION ALL
    SELECT 'MaxSpeed',                                  'decimal(18,2) NULL' UNION ALL
    SELECT 'MaxKonstVkMasa',                            'decimal(18,2) NULL' UNION ALL
    SELECT 'MaxLegVkMasa',                              'decimal(18,2) NULL' UNION ALL
    SELECT 'MaxLegVkMasaGrupa',                         'decimal(18,2) NULL' UNION ALL
    SELECT 'MasaPoOska1',                               'int NULL' UNION ALL
    SELECT 'MasaPoOska2',                               'int NULL' UNION ALL
    SELECT 'MasaPoOska3',                               'int NULL' UNION ALL
    SELECT 'MasaPoOska4',                               'int NULL' UNION ALL
    SELECT 'MasaPoOska5',                               'int NULL' UNION ALL
    SELECT 'OsnoOptovaruvanje1',                        'int NULL' UNION ALL
    SELECT 'OsnoOptovaruvanje2',                        'int NULL' UNION ALL
    SELECT 'OsnoOptovaruvanje3',                        'int NULL' UNION ALL
    SELECT 'OsnoOptovaruvanje4',                        'int NULL' UNION ALL
    SELECT 'OsnoOptovaruvanje5',                        'int NULL' UNION ALL
    SELECT 'MaxKonstOptovaruvanjeVoPriklucok',          'int NULL' UNION ALL
    SELECT 'TMaxHorVerOptovaruvanjePriklucok',          'int NULL' UNION ALL
    SELECT 'TMaxKonstVkMasaNaKombinacija',              'int NULL' UNION ALL
    SELECT 'TMaxKonstVkMasaPoluprikolka',               'int NULL' UNION ALL
    SELECT 'TMaxKonstVkMasaPrikolka',                   'int NULL' UNION ALL
    SELECT 'TMaxKonstVkMasaPrikolkaSoCenOska',          'int NULL' UNION ALL
    SELECT 'TMaxKonstVkMasaPrikolkaStoMozePrikluci',    'int NULL' UNION ALL
    SELECT 'TMinMasa',                                  'int NULL'
)
SELECT @sql = @sql + N'ALTER TABLE dbo.Vehicles ADD ' + name + N' ' + def + N';' + CHAR(13)
FROM cols
WHERE NOT EXISTS (
    SELECT 1 FROM sys.columns c
    JOIN sys.tables t ON t.object_id = c.object_id
    WHERE t.name = 'Vehicles' AND c.name = cols.name
);

IF LEN(@sql) > 0
BEGIN
    PRINT 'Adding columns:';
    PRINT @sql;
    EXEC sp_executesql @sql;
    PRINT 'Done.';
END
ELSE
    PRINT 'No columns to add. Vehicles already has all print columns.';
