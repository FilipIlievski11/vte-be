-- VTE2 — default seed data
-- Purpose: make a fresh VTE2 database actually usable.
-- Idempotent (every INSERT is guarded with NOT EXISTS).
-- Run AFTER all bootstrap-*.sql scripts have populated the schema.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- =============================================================================
-- Countries (Macedonia + neighbours + common others)
-- =============================================================================
INSERT INTO [dbo].[Countries] ([Name], [Iso2], [Iso3])
SELECT v.Name, v.Iso2, v.Iso3 FROM (VALUES
    (N'Северна Македонија', 'MK', 'MKD'),
    (N'Албанија',           'AL', 'ALB'),
    (N'Косово',             'XK', 'XKK'),
    (N'Србија',             'RS', 'SRB'),
    (N'Бугарија',           'BG', 'BGR'),
    (N'Грција',             'GR', 'GRC'),
    (N'Турција',            'TR', 'TUR'),
    (N'Германија',          'DE', 'DEU'),
    (N'Италија',            'IT', 'ITA'),
    (N'Австрија',           'AT', 'AUT'),
    (N'Швајцарија',         'CH', 'CHE'),
    (N'Хрватска',           'HR', 'HRV'),
    (N'Словенија',          'SI', 'SVN'),
    (N'Босна и Херцеговина','BA', 'BIH'),
    (N'Црна Гора',          'ME', 'MNE'),
    (N'Унгарија',           'HU', 'HUN')
) v(Name, Iso2, Iso3)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Countries] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle categories (EU type-approval codes)
-- =============================================================================
INSERT INTO [dbo].[VehicleCategories] ([Name], [Code])
SELECT v.Name, v.Code FROM (VALUES
    (N'Мопед',              'L1'),
    (N'Мотоцикл лесен',     'L3'),
    (N'Мотоцикл',           'L4'),
    (N'Трицикл',            'L5'),
    (N'Лесно четиритркало', 'L6'),
    (N'Тешко четиритркало', 'L7'),
    (N'Патнички автомобил', 'M1'),
    (N'Автобус',            'M2'),
    (N'Тежок автобус',      'M3'),
    (N'Лесен товарен',      'N1'),
    (N'Среден товарен',     'N2'),
    (N'Тежок товарен',      'N3'),
    (N'Лесна приколка',     'O1'),
    (N'Лесна приколка 2',   'O2'),
    (N'Тешка приколка',     'O3'),
    (N'Тешка приколка 2',   'O4'),
    (N'Трактор',            'T'),
    (N'Земјоделски',        'C')
) v(Name, Code)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleCategories] WHERE Code = v.Code);
GO

-- =============================================================================
-- Vehicle body types (typical)
-- =============================================================================
INSERT INTO [dbo].[VehicleBodyTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Седан'), (N'Хечбек'), (N'Караван (стејшн)'), (N'Купе'), (N'Кабриолет'),
    (N'Внедорожник (SUV)'), (N'Минивен'), (N'Пикап'), (N'Бус'), (N'Тенда'),
    (N'Сандак'), (N'Цистерна'), (N'Самосвал'), (N'Камион'), (N'Полуприколка')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleBodyTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle uses
-- =============================================================================
INSERT INTO [dbo].[VehicleUses] ([Name])
SELECT v.Name FROM (VALUES
    (N'Приватна употреба'),
    (N'Јавна употреба'),
    (N'Такси'),
    (N'Рент-а-кар'),
    (N'Авто-школа'),
    (N'Службена'),
    (N'Транспорт на патници'),
    (N'Транспорт на стока'),
    (N'Брза помош'),
    (N'Полиција'),
    (N'Противпожарна')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleUses] WHERE Name = v.Name);
GO

-- =============================================================================
-- Engine types / power source / eco programs
-- =============================================================================
INSERT INTO [dbo].[VehicleEngineTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Бензин'), (N'Дизел'), (N'Хибрид'), (N'Електричен'),
    (N'Бензин + ТНГ'), (N'Бензин + ЦНГ'), (N'Воден')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEngineTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleEnginePowerSourceTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Бензин'), (N'Дизел'), (N'ТНГ (LPG)'), (N'ЦНГ (CNG)'),
    (N'Електрична енергија'), (N'Хибрид')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEnginePowerSourceTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleEngineEcoPrograms] ([Name])
SELECT v.Name FROM (VALUES
    (N'Euro 1'), (N'Euro 2'), (N'Euro 3'), (N'Euro 4'),
    (N'Euro 5'), (N'Euro 6'), (N'EEV'), (N'Електричен (нула емисии)')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleEngineEcoPrograms] WHERE Name = v.Name);
GO

-- =============================================================================
-- Gearboxes / brakes / supportings
-- =============================================================================
INSERT INTO [dbo].[VehicleGearBoxes] ([Name])
SELECT v.Name FROM (VALUES (N'Рачен'), (N'Автоматски'), (N'Полуавтоматски'), (N'CVT'), (N'DCT'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleGearBoxes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleBrakes] ([Name])
SELECT v.Name FROM (VALUES (N'Дискови'), (N'Тапани'), (N'Дискови + тапани'), (N'ABS дискови'), (N'ABS + EBD'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleBrakes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[VehicleSupportings] ([Name])
SELECT v.Name FROM (VALUES (N'Макферсон'), (N'Двојни виши вилици'), (N'Многузглобни'), (N'Торзиона греда'), (N'Воздушни'))
v(Name) WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleSupportings] WHERE Name = v.Name);
GO

-- =============================================================================
-- Colors (basic palette)
-- =============================================================================
INSERT INTO [dbo].[Colors] ([Name], [HexCode])
SELECT v.Name, v.HexCode FROM (VALUES
    (N'Бела',    '#FFFFFF'),
    (N'Црна',    '#000000'),
    (N'Сива',    '#808080'),
    (N'Сребрена','#C0C0C0'),
    (N'Црвена',  '#C8102E'),
    (N'Сина',    '#1F4E8C'),
    (N'Зелена',  '#2E7D32'),
    (N'Жолта',   '#FBC02D'),
    (N'Кафена',  '#5D4037'),
    (N'Беж',     '#D7B98E'),
    (N'Портокалова', '#F57C00')
) v(Name, HexCode)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Colors] WHERE Name = v.Name);
GO

-- =============================================================================
-- Customer-Vehicle relation types
-- =============================================================================
INSERT INTO [dbo].[CustomerVehicleRelationTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Сопственик'), (N'Корисник'), (N'Закупец'), (N'Сосопственик'), (N'Овластен возач')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[CustomerVehicleRelationTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Driving licence categories
-- =============================================================================
INSERT INTO [dbo].[DrivingLicenceCategories] ([Name], [Description])
SELECT v.Name, v.Description FROM (VALUES
    (N'AM',  N'Мопеди'),
    (N'A1',  N'Лесни мотоцикли до 125 cm³'),
    (N'A2',  N'Мотоцикли до 35 kW'),
    (N'A',   N'Мотоцикли без ограничување'),
    (N'B1',  N'Четиритркала'),
    (N'B',   N'Патнички автомобили'),
    (N'BE',  N'Патнички со приколка'),
    (N'C1',  N'Лесни товарни 3.5–7.5 t'),
    (N'C1E', N'Лесни товарни со приколка'),
    (N'C',   N'Тешки товарни преку 7.5 t'),
    (N'CE',  N'Тешки товарни со приколка'),
    (N'D1',  N'Минибуси до 16 патници'),
    (N'D1E', N'Минибуси со приколка'),
    (N'D',   N'Автобуси'),
    (N'DE',  N'Автобуси со приколка'),
    (N'F',   N'Трактори'),
    (N'G',   N'Земјоделски возила'),
    (N'H',   N'Работни машини'),
    (N'M',   N'Самовозечки трицикли')
) v(Name, Description)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[DrivingLicenceCategories] WHERE Name = v.Name);
GO

-- =============================================================================
-- Payment types (drives the legacy IsInvoice/IsCash/etc. flags from PaymentTypes)
-- =============================================================================
INSERT INTO [dbo].[PaymentTypes] ([Name], [IsCash], [IsInvoice], [IsFiscalCard], [IsAccount], [IsInstallments])
SELECT v.Name, v.IsCash, v.IsInvoice, v.IsFiscalCard, v.IsAccount, v.IsInstallments FROM (VALUES
    (N'Кеш (фискална)',         CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Картичка (фискална)',    CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Фактура',                CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT)),
    (N'Уплата на сметка',       CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT), CAST(0 AS BIT)),
    (N'Договор за рати',        CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(0 AS BIT), CAST(1 AS BIT))
) v(Name, IsCash, IsInvoice, IsFiscalCard, IsAccount, IsInstallments)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[PaymentTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- DDV (VAT) catalog — current Macedonian rates
-- Sources: https://www.ujp.gov.mk (Macedonian Public Revenue Office)
-- Rates current as of 2026: 18 % standard, 5 % preferential, 0 % zero-rated
-- =============================================================================
INSERT INTO [dbo].[DDVCatalog] ([Name], [Rate], [EffectiveFrom])
SELECT v.Name, v.Rate, v.EffectiveFrom FROM (VALUES
    (N'ДДВ 0%',  CAST(0.00  AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE)),
    (N'ДДВ 5%',  CAST(5.00  AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE)),
    (N'ДДВ 18%', CAST(18.00 AS DECIMAL(5,2)), CAST('2000-01-01' AS DATE))
) v(Name, Rate, EffectiveFrom)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[DDVCatalog] WHERE Name = v.Name);
GO

-- =============================================================================
-- Technical exam types
-- =============================================================================
INSERT INTO [dbo].[TechnicalExamTypes] ([Name], [ValidityMonths])
SELECT v.Name, v.ValidityMonths FROM (VALUES
    (N'Редовен годишен',       12),
    (N'Шестомесечен',           6),
    (N'Прв (за нови возила)',  24),
    (N'Вонреден',              NULL),
    (N'По сообраќајна несреќа',NULL),
    (N'По преправка',          NULL)
) v(Name, ValidityMonths)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Technical exam vehicle-part categories + parts (the inspection checklist)
-- =============================================================================
DECLARE @cat TABLE (Name NVARCHAR(150), SortOrder INT);
INSERT INTO @cat VALUES
    (N'Кочни систем',                10),
    (N'Управувачки систем',          20),
    (N'Носачки систем',              30),
    (N'Светла и сигнализација',      40),
    (N'Гуми и тркала',               50),
    (N'Каросерија и шасија',         60),
    (N'Безбедносни уреди',           70),
    (N'Систем за издувни гасови',    80),
    (N'Електричен систем',           90),
    (N'Идентификација на возилото', 100);

INSERT INTO [dbo].[TechnicalExamVehiclePartCategories] ([Name], [SortOrder])
SELECT c.Name, c.SortOrder FROM @cat c
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = c.Name);
GO

-- Status set
INSERT INTO [dbo].[TechnicalExamReportDetailStatuses] ([Name], [IsPass])
SELECT v.Name, v.IsPass FROM (VALUES
    (N'Исправно',                CAST(1 AS BIT)),
    (N'Исправно со забелешка',   CAST(1 AS BIT)),
    (N'Условно исправно',        CAST(0 AS BIT)),
    (N'Неисправно',              CAST(0 AS BIT)),
    (N'Опасно неисправно',       CAST(0 AS BIT))
) v(Name, IsPass)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[TechnicalExamReportDetailStatuses] WHERE Name = v.Name);
GO

-- =============================================================================
-- Vehicle ownership proof types & payment proof types
-- =============================================================================
INSERT INTO [dbo].[VehicleOwnershipProofTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Сообраќајна дозвола'),
    (N'Договор за купопродажба'),
    (N'Договор за подарок'),
    (N'Решение за наследство'),
    (N'Решение за лизинг'),
    (N'Царинска декларација'),
    (N'Друго')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[VehicleOwnershipProofTypes] WHERE Name = v.Name);
GO

INSERT INTO [dbo].[PaymentProofTypes] ([Name])
SELECT v.Name FROM (VALUES
    (N'Уплатница'),
    (N'Уплатна сметка'),
    (N'Бесплатно (ослободено)'),
    (N'Друго')
) v(Name)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[PaymentProofTypes] WHERE Name = v.Name);
GO

-- =============================================================================
-- Request types (the workflow engine — drives Plav/Bel/Zelen behaviour)
-- =============================================================================
INSERT INTO [dbo].[RequestTypes] (
    [TypeName], [TypeDescription],
    [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration],
    [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer],
    [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired]
)
SELECT v.* FROM (VALUES
    -- (TypeName, TypeDescription, TER, PR, NReg, RelDel, VehDel, NewCust, VehCh, CustCh, Suf, PrevReg)
    (N'Прва регистрација',                  N'Регистрација на ново возило',                    1, 1, 1, 0, 0, 0, 0, 0, 0, 0),
    (N'Продолжување (редовно)',             N'Продолжување на регистрација по технички',       1, 1, 1, 0, 0, 0, 0, 0, 0, 1),
    (N'Промена на сопственик',              N'Префрлање на сопственост',                       1, 1, 1, 1, 0, 1, 0, 0, 0, 1),
    (N'Промена на возило',                  N'Само-промена на технички податоци',              1, 1, 0, 0, 0, 0, 1, 0, 0, 1),
    (N'Промена на сопственички податоци',   N'Адреса, име, презиме',                           0, 1, 0, 0, 0, 0, 0, 1, 0, 1),
    (N'Дерегистрација',                     N'Привремено отстранување од сообраќај',           0, 1, 0, 1, 0, 0, 0, 0, 0, 1),
    (N'Бришење на возило',                  N'Конечно бришење — расходувано',                  0, 1, 0, 1, 1, 0, 0, 0, 0, 1),
    (N'Само технички преглед',              N'Без регистрациски дејствија',                    1, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (N'Издавање меѓународна возачка',       N'Меѓународна возачка дозвола за возач',           0, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (N'Издавање дозвола за управување',     N'Специјална дозвола',                             0, 1, 0, 0, 0, 0, 0, 0, 1, 0)
) v(TypeName, TypeDescription, IsTechnicalExamRequired, IsPayRequired, IsNewRegistration,
    IsRelationDeleted, IsVehicleDeleted, IsNewCustomer, IsVehicleChanged, IsCustomerChanged,
    IsSufficient, IsPreviousRegistrationRequired)
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[RequestTypes] WHERE TypeName = v.TypeName);
GO

-- =============================================================================
-- Cities (a starter set for North Macedonia)
-- =============================================================================
DECLARE @mkId INT = (SELECT TOP 1 Id FROM [dbo].[Countries] WHERE Iso2 = 'MK');

IF @mkId IS NOT NULL
BEGIN
    INSERT INTO [dbo].[Cities] ([Name], [PostalCode], [CountryId])
    SELECT v.Name, v.PostalCode, @mkId FROM (VALUES
        (N'Скопје',     '1000'),
        (N'Битола',     '7000'),
        (N'Куманово',   '1300'),
        (N'Прилеп',     '7500'),
        (N'Тетово',     '1200'),
        (N'Велес',      '1400'),
        (N'Штип',       '2000'),
        (N'Охрид',      '6000'),
        (N'Гостивар',   '1230'),
        (N'Струмица',   '2400'),
        (N'Кавадарци',  '1430'),
        (N'Кочани',     '2300'),
        (N'Кичево',     '6250'),
        (N'Струга',     '6330'),
        (N'Радовиш',    '2420'),
        (N'Гевгелија',  '1480'),
        (N'Дебар',      '1250'),
        (N'Свети Николе','2220'),
        (N'Неготино',   '1440'),
        (N'Делчево',    '2320'),
        (N'Кратово',    '1360'),
        (N'Берово',     '2330'),
        (N'Пехчево',    '2326'),
        (N'Виница',     '2310'),
        (N'Македонски Брод', '6510'),
        (N'Крушево',    '7550'),
        (N'Демир Хисар','7240'),
        (N'Ресен',      '7310'),
        (N'Валандово',  '1460'),
        (N'Дојран',     '1487'),
        (N'Пробиштип',  '2210')
    ) v(Name, PostalCode)
    WHERE NOT EXISTS (SELECT 1 FROM [dbo].[Cities] WHERE Name = v.Name AND CountryId = @mkId);
END
GO

PRINT '';
PRINT '=== seed-vte2-defaults.sql complete ===';
SELECT
    (SELECT COUNT(*) FROM [dbo].[Countries])                          AS Countries,
    (SELECT COUNT(*) FROM [dbo].[Cities])                             AS Cities,
    (SELECT COUNT(*) FROM [dbo].[VehicleCategories])                  AS VehicleCategories,
    (SELECT COUNT(*) FROM [dbo].[VehicleBodyTypes])                   AS BodyTypes,
    (SELECT COUNT(*) FROM [dbo].[VehicleEngineTypes])                 AS EngineTypes,
    (SELECT COUNT(*) FROM [dbo].[Colors])                             AS Colors,
    (SELECT COUNT(*) FROM [dbo].[DrivingLicenceCategories])           AS DrivingLicenceCategories,
    (SELECT COUNT(*) FROM [dbo].[PaymentTypes])                       AS PaymentTypes,
    (SELECT COUNT(*) FROM [dbo].[DDVCatalog])                         AS DDV,
    (SELECT COUNT(*) FROM [dbo].[TechnicalExamTypes])                 AS TechExamTypes,
    (SELECT COUNT(*) FROM [dbo].[TechnicalExamReportDetailStatuses])  AS TechExamStatuses,
    (SELECT COUNT(*) FROM [dbo].[RequestTypes])                       AS RequestTypes;
GO
