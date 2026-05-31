USE VTE2;
GO
UPDATE Stations SET Name = N'Бранзис Скопје' WHERE Code = N'BRZ-SK';
UPDATE Stations SET Name = N'Битола'         WHERE Code = N'BRZ-BT';
UPDATE Stations SET Name = N'Тетово'         WHERE Code = N'BRZ-TE';
GO
SELECT Id, Name FROM Stations ORDER BY Id;
GO
