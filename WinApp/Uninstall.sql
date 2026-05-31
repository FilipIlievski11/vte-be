IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'vtesecurity')
	DROP DATABASE [vtesecurity]
GO
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'VTE')
	DROP DATABASE [VTE]