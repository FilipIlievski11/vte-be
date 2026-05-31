-- Re-seed RequestTypes with the legacy hierarchical structure.
-- Three top-level categories (parents) with children matching the screenshots.
USE [VTE2];
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

-- Clear existing requests + types
DELETE FROM [dbo].[Requests];
DELETE FROM [dbo].[RequestTypes];
DBCC CHECKIDENT('RequestTypes', RESEED, 0);
GO

-- Group 1: Барање - Пријава (the four sub-codes А/Б/В/Г)
DECLARE @gPrijava INT, @gRegistracija INT, @gRegList INT, @gOther INT;

INSERT INTO [dbo].[RequestTypes] ([TypeName], [TypeDescription], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES (N'Барање - Пријава', NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
SET @gPrijava = SCOPE_IDENTITY();

INSERT INTO [dbo].[RequestTypes] ([TypeName], [TypeDescription], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES (N'Барање за регистрација на моторно-приклучно возило', NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
SET @gRegistracija = SCOPE_IDENTITY();

INSERT INTO [dbo].[RequestTypes] ([TypeName], [TypeDescription], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES (N'Регистрационен лист', NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
SET @gRegList = SCOPE_IDENTITY();

INSERT INTO [dbo].[RequestTypes] ([TypeName], [TypeDescription], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES (N'Други барања', NULL, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
SET @gOther = SCOPE_IDENTITY();
GO

DECLARE @gPrijava INT = (SELECT Id FROM [dbo].[RequestTypes] WHERE TypeName = N'Барање - Пријава');
DECLARE @gRegistracija INT = (SELECT Id FROM [dbo].[RequestTypes] WHERE TypeName = N'Барање за регистрација на моторно-приклучно возило');
DECLARE @gRegList INT = (SELECT Id FROM [dbo].[RequestTypes] WHERE TypeName = N'Регистрационен лист');
DECLARE @gOther INT = (SELECT Id FROM [dbo].[RequestTypes] WHERE TypeName = N'Други барања');

-- Children of "Барање - Пријава" (the А/Б/В/Г sub-codes)
INSERT INTO [dbo].[RequestTypes] ([ParentRequestTypeId], [TypeName], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES
    (@gPrijava, N'А - Продолжување на регистрацијата, промена на податоците за возилото и сопственикот', 1, 1, 1, 0, 0, 0, 1, 1, 0, 1),
    (@gPrijava, N'Б - Промена на податоци за возилото и сопственикот',                                  0, 1, 0, 0, 0, 0, 1, 1, 0, 1),
    (@gPrijava, N'В - Промена на бројот на регистрарски таблици',                                       0, 1, 1, 0, 0, 0, 0, 0, 0, 1),
    (@gPrijava, N'Г - Одјавување на возилото',                                                          0, 1, 0, 1, 0, 0, 0, 0, 0, 1);

-- Children of "Барање за регистрација на моторно-приклучно возило"
INSERT INTO [dbo].[RequestTypes] ([ParentRequestTypeId], [TypeName], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES
    (@gRegistracija, N'По прв пат',                                  1, 1, 1, 0, 0, 0, 0, 0, 0, 0),
    (@gRegistracija, N'Повторно',                                    1, 1, 1, 0, 0, 0, 0, 0, 0, 1),
    (@gRegistracija, N'Привремено',                                  1, 1, 1, 0, 0, 0, 0, 0, 0, 0),
    (@gRegistracija, N'Повторно со важечки регистрарски таблици',    1, 1, 0, 0, 0, 0, 0, 0, 0, 1),
    (@gRegistracija, N'Прв пат со важечки технички преглед',         0, 1, 1, 0, 0, 0, 0, 0, 0, 0);

-- Children of "Регистрационен лист"
INSERT INTO [dbo].[RequestTypes] ([ParentRequestTypeId], [TypeName], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES
    (@gRegList, N'1. Прва регистрација на возилото со промена на сопственик', 1, 1, 1, 1, 0, 1, 0, 0, 0, 1),
    (@gRegList, N'4. Промена на техничката состојба на возилото',             1, 1, 0, 0, 0, 0, 1, 0, 0, 1),
    (@gRegList, N'3. Промена на живеалиштето, односно седиштето во друга општина', 0, 1, 0, 0, 0, 0, 0, 1, 0, 1),
    (@gRegList, N'4. Бришење на возилото од евиденција',                       0, 1, 0, 1, 1, 0, 0, 0, 0, 1),
    (@gRegList, N'5. Замена на регистрарската табла',                          0, 1, 1, 0, 0, 0, 0, 0, 0, 1),
    (@gRegList, N'1.1. Прва регистрација на возилото',                         1, 1, 1, 0, 0, 0, 0, 0, 0, 0);

-- Children of "Други барања" — non-registration helpers
INSERT INTO [dbo].[RequestTypes] ([ParentRequestTypeId], [TypeName], [IsTechnicalExamRequired], [IsPayRequired], [IsNewRegistration], [IsRelationDeleted], [IsVehicleDeleted], [IsNewCustomer], [IsVehicleChanged], [IsCustomerChanged], [IsSufficient], [IsPreviousRegistrationRequired])
VALUES
    (@gOther, N'Само технički преглед',                  1, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (@gOther, N'Издавање меѓународна возачка дозвола',   0, 1, 0, 0, 0, 0, 0, 0, 1, 0),
    (@gOther, N'Издавање одобрение за управување',       0, 1, 0, 0, 0, 0, 0, 0, 1, 0);
GO

PRINT '';
PRINT 'RequestTypes re-seeded with hierarchy';
SELECT
    p.TypeName AS [Категорија],
    COUNT(c.Id) AS [Бр. на барања]
FROM [dbo].[RequestTypes] p
LEFT JOIN [dbo].[RequestTypes] c ON c.ParentRequestTypeId = p.Id
WHERE p.ParentRequestTypeId IS NULL
GROUP BY p.TypeName
ORDER BY p.TypeName;
GO
