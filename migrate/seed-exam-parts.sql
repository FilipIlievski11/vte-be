-- Seed TechnicalExamVehiclePartCategories + Parts (the inspection checklist)
USE [VTE2];
SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

DELETE FROM [dbo].[TechnicalExamReportDetails];
DELETE FROM [dbo].[TechnicalExamVehicleParts];
DELETE FROM [dbo].[TechnicalExamVehiclePartCategories];
DBCC CHECKIDENT('TechnicalExamVehiclePartCategories', RESEED, 0);
DBCC CHECKIDENT('TechnicalExamVehicleParts', RESEED, 0);
GO

-- Categories (the legacy "Неисправности" tree)
INSERT INTO [dbo].[TechnicalExamVehiclePartCategories] ([Name], [SortOrder]) VALUES
    (N'МОТОР И ПОГОНСКИ УРЕДИ',                10),
    (N'УРЕД ЗА УПРАВУВАЊЕ',                    20),
    (N'УРЕД ЗА СОПИРАЊЕ',                      30),
    (N'СВЕТЛОСНО СИГНАЛНИ УРЕДИ',              40),
    (N'КОНТРОЛНИ И СИГНАЛНИ УРЕДИ',            50),
    (N'УРЕДИ ЗА НОРМАЛНА ВИДЛИВОСТ',           60),
    (N'ТРКАЛА, ПНЕВМАТИЦИ И ОСИ',              70),
    (N'НОСЕЧКИ СИСТЕМ',                        80),
    (N'РАМКА И КАРОСЕРИЈА',                    90),
    (N'ИНСТРУМЕНТАЛНА ТАБЛА',                 100),
    (N'УРЕДИ ЗА БЕЗБЕДНОСТ',                  110),
    (N'СИСТЕМ ЗА ИЗДУВНИ ГАСОВИ',             120),
    (N'ЕЛЕКТРИЧЕН СИСТЕМ',                    130),
    (N'ПРЕНОСНИК НА ПОГОН',                   140),
    (N'ИДЕНТИФИКАЦИЈА НА ВОЗИЛОТО',           150);
GO

DECLARE @c INT;

-- МОТОР И ПОГОНСКИ УРЕДИ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'МОТОР И ПОГОНСКИ УРЕДИ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Мотор - општа состојба', 10), (@c, N'Капак на моторот', 20),
    (@c, N'Држачи на моторот', 30), (@c, N'Систем за подмачкување', 40),
    (@c, N'Систем за ладење', 50), (@c, N'Систем за вшмукување', 60);

-- УРЕД ЗА УПРАВУВАЊЕ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'УРЕД ЗА УПРАВУВАЊЕ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Управувач (волан)', 10), (@c, N'Колона на управувачот', 20),
    (@c, N'Зглобови', 30), (@c, N'Серво уред', 40), (@c, N'Ослонци', 50);

-- УРЕД ЗА СОПИРАЊЕ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'УРЕД ЗА СОПИРАЊЕ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Папучка / рачица', 10), (@c, N'Главен сопирачки цилиндар', 20),
    (@c, N'Сопирачки дискови', 30), (@c, N'Сопирачки тапани', 40),
    (@c, N'Сопирачки облоги', 50), (@c, N'Сопирачки цевки', 60),
    (@c, N'Паркинг кочница', 70), (@c, N'ABS', 80);

-- СВЕТЛОСНО СИГНАЛНИ УРЕДИ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'СВЕТЛОСНО СИГНАЛНИ УРЕДИ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Главни (долги) светла', 10), (@c, N'Кратки светла', 20),
    (@c, N'Позициони светла', 30), (@c, N'Стоп светла', 40),
    (@c, N'Покажувачи на правец', 50), (@c, N'Магла-светла', 60),
    (@c, N'Светло за рикверц', 70), (@c, N'Регистарска табла светло', 80);

-- КОНТРОЛНИ И СИГНАЛНИ УРЕДИ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'КОНТРОЛНИ И СИГНАЛНИ УРЕДИ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Звучен сигнал (сирена)', 10), (@c, N'Брзинометар', 20),
    (@c, N'Тахограф', 30), (@c, N'Уреди на инструментална табла', 40);

-- УРЕДИ ЗА НОРМАЛНА ВИДЛИВОСТ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'УРЕДИ ЗА НОРМАЛНА ВИДЛИВОСТ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Шофершајбна', 10), (@c, N'Странични стакла', 20),
    (@c, N'Бришачи', 30), (@c, N'Перачи на стакла', 40),
    (@c, N'Огледала странични', 50), (@c, N'Огледало внатрешно', 60);

-- ТРКАЛА, ПНЕВМАТИЦИ И ОСИ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'ТРКАЛА, ПНЕВМАТИЦИ И ОСИ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Гуми - предни', 10), (@c, N'Гуми - задни', 20),
    (@c, N'Бандажи (џанти)', 30), (@c, N'Лежишта на тркала', 40),
    (@c, N'Осовини', 50);

-- НОСЕЧКИ СИСТЕМ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'НОСЕЧКИ СИСТЕМ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Опруги / пера', 10), (@c, N'Амортизери', 20),
    (@c, N'Трапова виљушка', 30), (@c, N'Стабилизатор', 40);

-- РАМКА И КАРОСЕРИЈА
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'РАМКА И КАРОСЕРИЈА');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Рамка', 10), (@c, N'Каросерија / лимарија', 20),
    (@c, N'Браници', 30), (@c, N'Врати', 40), (@c, N'Седишта', 50);

-- ИНСТРУМЕНТАЛНА ТАБЛА
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'ИНСТРУМЕНТАЛНА ТАБЛА');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Контролни лампи', 10), (@c, N'Осветлување на табла', 20);

-- УРЕДИ ЗА БЕЗБЕДНОСТ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'УРЕДИ ЗА БЕЗБЕДНОСТ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Сигурносни појаси', 10), (@c, N'Воздушни перници (АБС)', 20),
    (@c, N'Триаголник за предупредување', 30), (@c, N'Прва помош', 40),
    (@c, N'Резервна гума', 50), (@c, N'Алат', 60), (@c, N'Противпожарен апарат', 70);

-- СИСТЕМ ЗА ИЗДУВНИ ГАСОВИ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'СИСТЕМ ЗА ИЗДУВНИ ГАСОВИ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Издувна цевка', 10), (@c, N'Аусфунг (гушнич)', 20),
    (@c, N'Каталитички конвертор', 30), (@c, N'Емисии', 40);

-- ЕЛЕКТРИЧЕН СИСТЕМ
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'ЕЛЕКТРИЧЕН СИСТЕМ');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Акумулатор', 10), (@c, N'Алтернатор', 20),
    (@c, N'Електрични водови', 30), (@c, N'Осигурувачи', 40);

-- ПРЕНОСНИК НА ПОГОН
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'ПРЕНОСНИК НА ПОГОН');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Спојка (квачило)', 10), (@c, N'Менувач', 20),
    (@c, N'Кардан', 30), (@c, N'Диференцијал', 40);

-- ИДЕНТИФИКАЦИЈА НА ВОЗИЛОТО
SET @c = (SELECT Id FROM [dbo].[TechnicalExamVehiclePartCategories] WHERE Name = N'ИДЕНТИФИКАЦИЈА НА ВОЗИЛОТО');
INSERT INTO [dbo].[TechnicalExamVehicleParts] ([CategoryId], [Name], [SortOrder]) VALUES
    (@c, N'Број на шасија (VIN)', 10), (@c, N'Број на мотор', 20),
    (@c, N'Регистарски таблици', 30), (@c, N'Сообраќајна дозвола', 40);
GO

PRINT '';
PRINT 'Exam parts seeded';
SELECT
    cat.Name AS [Категорија],
    COUNT(p.Id) AS [Бр. на делови]
FROM [dbo].[TechnicalExamVehiclePartCategories] cat
LEFT JOIN [dbo].[TechnicalExamVehicleParts] p ON p.CategoryId = cat.Id
GROUP BY cat.Name, cat.SortOrder
ORDER BY cat.SortOrder;
GO
