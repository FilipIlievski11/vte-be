USE [master]
GO
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'VTE')
	DROP DATABASE [KenoBingo]
GO
DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1
/****** Object:  Database [KenoBingo]    Script Date: 07/13/2009 15:22:26 ******/
EXECUTE (N'CREATE DATABASE VTE
  ON PRIMARY (NAME = N''vtesecurity'', FILENAME = N''' + @device_directory + N'vtesecurity.mdf'')
  LOG ON (NAME = N''vtesecurity_log'',  FILENAME = N''' + @device_directory + N'vtesecurity_log.ldf'')')
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'vtesecurity', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [vtesecurity].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [vtesecurity] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [vtesecurity] SET ANSI_NULLS OFF
GO
ALTER DATABASE [vtesecurity] SET ANSI_PADDING OFF
GO
ALTER DATABASE [vtesecurity] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [vtesecurity] SET ARITHABORT OFF
GO
ALTER DATABASE [vtesecurity] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [vtesecurity] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [vtesecurity] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [vtesecurity] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [vtesecurity] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [vtesecurity] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [vtesecurity] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [vtesecurity] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [vtesecurity] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [vtesecurity] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [vtesecurity] SET  DISABLE_BROKER
GO
ALTER DATABASE [vtesecurity] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [vtesecurity] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [vtesecurity] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [vtesecurity] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [vtesecurity] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [vtesecurity] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [vtesecurity] SET  READ_WRITE
GO
ALTER DATABASE [vtesecurity] SET RECOVERY SIMPLE
GO
ALTER DATABASE [vtesecurity] SET  MULTI_USER
GO
ALTER DATABASE [vtesecurity] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [vtesecurity] SET DB_CHAINING OFF
GO
USE [vtesecurity]
GO
/****** Object:  ForeignKey [FK_CSLAObjects_CSLAObjects]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[CSLAObjects] DROP CONSTRAINT [FK_CSLAObjects_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_Users_DataBases]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_DataBases]
GO
/****** Object:  ForeignKey [FK_Users_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Roles]
GO
/****** Object:  ForeignKey [FK_FieldsPrivileges_CSLAObjects]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[FieldsPrivileges] DROP CONSTRAINT [FK_FieldsPrivileges_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_FieldsPrivileges_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[FieldsPrivileges] DROP CONSTRAINT [FK_FieldsPrivileges_Roles]
GO
/****** Object:  ForeignKey [FK_ObjectPrivileges_CSLAObjects]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[ObjectPrivileges] DROP CONSTRAINT [FK_ObjectPrivileges_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_ObjectPrivileges_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[ObjectPrivileges] DROP CONSTRAINT [FK_ObjectPrivileges_Roles]
GO
/****** Object:  StoredProcedure [dbo].[updateRole]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateRole]
GO
/****** Object:  StoredProcedure [dbo].[addRole]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addRole]
GO
/****** Object:  StoredProcedure [dbo].[getRoleById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getRoleById]
GO
/****** Object:  StoredProcedure [dbo].[getRoles]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getRoles]
GO
/****** Object:  StoredProcedure [dbo].[deleteRole]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteRole]
GO
/****** Object:  StoredProcedure [dbo].[updateEmploye]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateEmploye]
GO
/****** Object:  StoredProcedure [dbo].[getEmployeById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getEmployeById]
GO
/****** Object:  StoredProcedure [dbo].[getEmployes]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getEmployes]
GO
/****** Object:  StoredProcedure [dbo].[addEmploye]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addEmploye]
GO
/****** Object:  StoredProcedure [dbo].[deleteEmploye]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteEmploye]
GO
/****** Object:  StoredProcedure [dbo].[addUser]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addUser]
GO
/****** Object:  StoredProcedure [dbo].[updateUser]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateUser]
GO
/****** Object:  StoredProcedure [dbo].[getUserById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getUserById]
GO
/****** Object:  StoredProcedure [dbo].[getUsers]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getUsers]
GO
/****** Object:  StoredProcedure [dbo].[deleteUser]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteUser]
GO
/****** Object:  StoredProcedure [dbo].[deleteObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteObjectPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[updateObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateObjectPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[getObjectPrivilegeById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getObjectPrivilegeById]
GO
/****** Object:  StoredProcedure [dbo].[getObjectPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getObjectPrivileges]
GO
/****** Object:  StoredProcedure [dbo].[addObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addObjectPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[addFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addFieldsPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[deleteFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteFieldsPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[getFieldsPrivilegeById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getFieldsPrivilegeById]
GO
/****** Object:  StoredProcedure [dbo].[getFieldsPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getFieldsPrivileges]
GO
/****** Object:  StoredProcedure [dbo].[updateFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateFieldsPrivilege]
GO
/****** Object:  StoredProcedure [dbo].[updateCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateCSLAObject]
GO
/****** Object:  View [dbo].[ObjectPrivilegesList]    Script Date: 08/06/2009 13:33:43 ******/
DROP VIEW [dbo].[ObjectPrivilegesList]
GO
/****** Object:  StoredProcedure [dbo].[addCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addCSLAObject]
GO
/****** Object:  StoredProcedure [dbo].[deleteCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteCSLAObject]
GO
/****** Object:  StoredProcedure [dbo].[getCSLAObjectById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getCSLAObjectById]
GO
/****** Object:  StoredProcedure [dbo].[getCSLAObjects]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getCSLAObjects]
GO
/****** Object:  View [dbo].[FieldPrivilegesList]    Script Date: 08/06/2009 13:33:43 ******/
DROP VIEW [dbo].[FieldPrivilegesList]
GO
/****** Object:  StoredProcedure [dbo].[GetChildFieldsPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[GetChildFieldsPrivileges]
GO
/****** Object:  StoredProcedure [dbo].[GetChildObjectPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[GetChildObjectPrivileges]
GO
/****** Object:  StoredProcedure [dbo].[GetPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[GetPrivileges]
GO
/****** Object:  StoredProcedure [dbo].[getConnectionString]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getConnectionString]
GO
/****** Object:  StoredProcedure [dbo].[getDataBaseById]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getDataBaseById]
GO
/****** Object:  StoredProcedure [dbo].[getDataBases]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[getDataBases]
GO
/****** Object:  StoredProcedure [dbo].[deleteDataBase]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[deleteDataBase]
GO
/****** Object:  StoredProcedure [dbo].[addDataBase]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[addDataBase]
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[Login]
GO
/****** Object:  StoredProcedure [dbo].[updateDataBase]    Script Date: 08/06/2009 13:33:43 ******/
DROP PROCEDURE [dbo].[updateDataBase]
GO
/****** Object:  View [dbo].[View_1]    Script Date: 08/06/2009 13:33:42 ******/
DROP VIEW [dbo].[View_1]
GO
/****** Object:  Table [dbo].[ObjectPrivileges]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[ObjectPrivileges] DROP CONSTRAINT [FK_ObjectPrivileges_CSLAObjects]
GO
ALTER TABLE [dbo].[ObjectPrivileges] DROP CONSTRAINT [FK_ObjectPrivileges_Roles]
GO
ALTER TABLE [dbo].[ObjectPrivileges] DROP CONSTRAINT [DF_ObjectPrivileges_Active]
GO
DROP TABLE [dbo].[ObjectPrivileges]
GO
/****** Object:  Table [dbo].[FieldsPrivileges]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[FieldsPrivileges] DROP CONSTRAINT [FK_FieldsPrivileges_CSLAObjects]
GO
ALTER TABLE [dbo].[FieldsPrivileges] DROP CONSTRAINT [FK_FieldsPrivileges_Roles]
GO
ALTER TABLE [dbo].[FieldsPrivileges] DROP CONSTRAINT [DF_FieldsPrivileges_Active]
GO
DROP TABLE [dbo].[FieldsPrivileges]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_DataBases]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Roles]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_Users_DateOfHireing]
GO
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF_KORISNICI_AKTIVEN]
GO
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[Roles] DROP CONSTRAINT [DF_ULOGI_AKTIVEN]
GO
DROP TABLE [dbo].[Roles]
GO
/****** Object:  Table [dbo].[Employes]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[Employes] DROP CONSTRAINT [DF_Employes_DateOfHireing]
GO
ALTER TABLE [dbo].[Employes] DROP CONSTRAINT [DF_Employes_Active]
GO
DROP TABLE [dbo].[Employes]
GO
/****** Object:  Table [dbo].[CSLAObjects]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[CSLAObjects] DROP CONSTRAINT [FK_CSLAObjects_CSLAObjects]
GO
ALTER TABLE [dbo].[CSLAObjects] DROP CONSTRAINT [DF_CSLAObjects_Active]
GO
DROP TABLE [dbo].[CSLAObjects]
GO
/****** Object:  Table [dbo].[DataBases]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[DataBases] DROP CONSTRAINT [DF_BAZI_AKTIVEN]
GO
DROP TABLE [dbo].[DataBases]
GO
/****** Object:  Table [dbo].[DataBases]    Script Date: 08/06/2009 13:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataBases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[EndUserName] [nvarchar](50) NOT NULL,
	[DatabaseName] [nvarchar](50) NULL,
	[ConnetionString] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_BAZI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_BAZI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[DataBases] ON
INSERT [dbo].[DataBases] ([Id], [EndUserName], [DatabaseName], [ConnetionString], [Active]) VALUES (1, N'Локална', N'VTE', N'data source=.\SQLEXPRESS;Initial Catalog={0};Integrated Security=true;', 1)
SET IDENTITY_INSERT [dbo].[DataBases] OFF
/****** Object:  Table [dbo].[CSLAObjects]    Script Date: 08/06/2009 13:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CSLAObjects](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCSLAObject] [int] NULL,
	[CSLAObjectName] [nvarchar](100) NOT NULL,
	[CSLAObjectType] [nvarchar](100) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CSLAObjects_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_CSLAObjects] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CSLAObjects] ON
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (1, NULL, N'Cities', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (3, NULL, N'Countries', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (5, NULL, N'VehicleBrakes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (6, NULL, N'BusinessTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (7, NULL, N'Streets', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (8, NULL, N'Customers', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (9, NULL, N'VehicleGearBoxes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (12, NULL, N'CSLAObjects', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (17, NULL, N'VehicleCategories', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (19, NULL, N'VehicleEnginePowerSourceTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (21, NULL, N'VehicleEngineTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (22, NULL, N'VehicleBodytypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (23, NULL, N'VehicleModels', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (24, NULL, N'VehicleMakers', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (25, NULL, N'Colors', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (26, NULL, N'VehicleSupportings', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (27, NULL, N'VehicleEngineMarks', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (28, NULL, N'Vehicle', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (29, NULL, N'VehicleModel', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (30, NULL, N'VehicleMaker', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (31, NULL, N'Country', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (32, NULL, N'CustomerVehicleRelationTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (33, NULL, N'VehicleTireTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (34, NULL, N'Communities', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (36, NULL, N'CustomerVehicleRelationType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (37, NULL, N'CustomerVehiclesRelations', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (38, NULL, N'DocumentTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (39, NULL, N'DocumentTypes', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (40, NULL, N'CustomerVehiclesRelation', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (41, NULL, N'Document', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (42, NULL, N'TehnicalExamOrganizations', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (43, NULL, N'DocumentType', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (44, NULL, N'DocumentTypePrints', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (45, NULL, N'Employes', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (46, NULL, N'DocumentsTehnicalExamsReport', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (47, NULL, N'TehnicalExamsType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (48, NULL, N'DocumentsTrafficLicence', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (49, NULL, N'PriceCatalogs', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (50, NULL, N'DDVCatalogs', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (51, NULL, N'VehicleCategorie', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (52, NULL, N'VehicleRegistration', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (53, NULL, N'VehicleRegistrations', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (54, NULL, N'TehnicalExamVehiclePartsCategories', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (55, NULL, N'TehnicalExamVehicleParts', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (56, NULL, N'TehnicalExamsTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (57, NULL, N'PaymentDocument', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (58, NULL, N'PaymentTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (59, NULL, N'DocumentsTehnicalExamsReportsDetailsStatuses', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (60, NULL, N'DocumentsPermision', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (61, NULL, N'DocumentsPermisions', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (62, NULL, N'DriveingLicenceCtegories', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (63, NULL, N'DocumentsInternationalDriveingLicence', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (64, NULL, N'EngineTypeModelRelation', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (65, NULL, N'Community', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (66, NULL, N'VehicleEnginePowerSourceType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (68, NULL, N'TehnicalExamVehiclePartsCategory', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (69, NULL, N'VehicleUses', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (70, NULL, N'VehicleCategoriesRelations', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (71, NULL, N'VehicleCategoryForPayments', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (72, NULL, N'VehicleEngineEcoPrograms', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (74, NULL, N'AttachmentTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (75, NULL, N'DocumentsTehnicalExamsReports', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (76, NULL, N'PaymentCategories', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (77, NULL, N'Users', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (78, NULL, N'DocumentVehicleOwnershipProofs', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (79, NULL, N'DocumentPaymentProofes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (80, NULL, N'VehicleTireType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (81, NULL, N'RegistrationIssuers', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (82, NULL, N'CalculationItems', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (83, NULL, N'RequestTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (84, NULL, N'Request', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (86, NULL, N'ObjectPrivileges', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (87, NULL, N'CSLAObjects', N'EditableRoot', 0)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (90, NULL, N'CSLAObject', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (93, NULL, N'City', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (94, NULL, N'Color', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (95, NULL, N'Customer', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (96, NULL, N'CustomersContactPerson', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (97, NULL, N'CustomersContactPersons', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (98, NULL, N'Street', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (99, NULL, N'DocumentPaymentProof', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (100, NULL, N'DocumentsTrafficLicences', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (101, NULL, N'DocumentTypePrint', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (102, NULL, N'DocumentTypesOption', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (103, NULL, N'DocumentTypesOptions', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (104, NULL, N'DocumentVehicleOwnershipProof', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (105, NULL, N'DriveingLicenceCtegory', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (106, NULL, N'RegistrationIssuer', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (107, NULL, N'TrafficLicencesExtension', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (108, NULL, N'TrafficLicencesExtensions', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (110, NULL, N'User', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (111, NULL, N'DDVCatalog', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (112, NULL, N'PaymentCategorie', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (113, NULL, N'PaymentDocumentsDetail', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (114, NULL, N'PaymentDocumentsDetails', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (115, NULL, N'PaymentItem', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (116, NULL, N'PaymentItems', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (117, NULL, N'PaymentType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (118, NULL, N'RequestPaymentProof', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (119, NULL, N'RequestPaymentProofs', N'EditableRootList', 1)
GO
print 'Processed 100 total records'
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (120, NULL, N'RequestType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (121, NULL, N'RequestVehicleOwnershipProof', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (122, NULL, N'RequestVehicleOwnershipProofs', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (123, NULL, N'ObjectPrivilege', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (124, NULL, N'DocumentsTehnicalExamsReport', N'EditableRoot', 0)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (125, NULL, N'DocumentsTehnicalExamsReportsDetail', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (126, NULL, N'DocumentsTehnicalExamsReportsDetails', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (127, NULL, N'DocumentsTehnicalExamsReportsDetailsStatus', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (128, NULL, N'TehnicalExamVehiclePart', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (129, NULL, N'EngineTypeModelRelations', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (130, NULL, N'TehnicalExamOrganization', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (131, NULL, N'TireType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (132, NULL, N'TireTypes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (133, NULL, N'VehicleAxes', N'EditableRootList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (134, NULL, N'VehicleAxis', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (135, NULL, N'VehicleBetweenAxesDestination', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (136, NULL, N'VehicleBetweenAxesDestinations', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (137, NULL, N'VehicleBodytype', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (138, NULL, N'VehicleBrake', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (139, NULL, N'VehicleCategoriesRelation', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (140, NULL, N'VehicleCategoryForPayment', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (141, NULL, N'VehicleEngineEcoProgram', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (142, NULL, N'VehicleEngineType', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (143, NULL, N'VehicleGearBox', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (144, NULL, N'VehicleLastTehnicalExam', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (145, NULL, N'VehicleLastTehnicalExams', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (146, NULL, N'VehicleSupporting', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (147, NULL, N'VehicleUse', N'EditableRoot', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (148, 63, N'DocumentsInternationalDriveingLicenceValidForCategories', N'EditableChildList', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (149, 63, N'DocumentsInternationalDriveingLicenceValidForCategorie', N'EditableChild', 1)
INSERT [dbo].[CSLAObjects] ([Id], [IdCSLAObject], [CSLAObjectName], [CSLAObjectType], [Active]) VALUES (150, NULL, N'PaymentRatiDogovor', N'EditableRoot', 1)
SET IDENTITY_INSERT [dbo].[CSLAObjects] OFF
/****** Object:  Table [dbo].[Employes]    Script Date: 08/06/2009 13:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Employes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdWorkPosition] [int] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[Prezime] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[EMBG] [char](13) NOT NULL,
	[BLK] [nvarchar](10) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[DateOfHireing] [datetime] NOT NULL CONSTRAINT [DF_Employes_DateOfHireing]  DEFAULT (getdate()),
	[RFID] [nvarchar](50) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Employes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Employes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Employes] ON
INSERT [dbo].[Employes] ([Id], [IdWorkPosition], [FirstName], [Prezime], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (1, 1, N'Ангелина', N'Чушкова', N'сдаф', N'432434       ', N'213214', CAST(0x0000774600000000 AS DateTime), CAST(0x00009ADB00000000 AS DateTime), N'3423', 1)
INSERT [dbo].[Employes] ([Id], [IdWorkPosition], [FirstName], [Prezime], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (2, 1, N'Иван', N'Карпузов', N'клнх', N'4234432      ', N'42342', CAST(0x00009ADB00000000 AS DateTime), CAST(0x00009ADB00000000 AS DateTime), N'67', 1)
INSERT [dbo].[Employes] ([Id], [IdWorkPosition], [FirstName], [Prezime], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (3, 5, N'Бранко', N'Трајковски', N'5', N'5            ', N'5', CAST(0x00009ADB00000000 AS DateTime), CAST(0x00009ADB00000000 AS DateTime), N'5', 1)
INSERT [dbo].[Employes] ([Id], [IdWorkPosition], [FirstName], [Prezime], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (4, 3, N'Стефан', N'Цветановски', N'3', N'3            ', N'3', CAST(0x00009ADB00000000 AS DateTime), CAST(0x00009ADB00000000 AS DateTime), N'3', 1)
SET IDENTITY_INSERT [dbo].[Employes] OFF
/****** Object:  Table [dbo].[Roles]    Script Date: 08/06/2009 13:33:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ULOGI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ULOGI] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([Id], [RoleName], [Active]) VALUES (1, N'Administrator', 1)
INSERT [dbo].[Roles] ([Id], [RoleName], [Active]) VALUES (2, N'Operator', 1)
INSERT [dbo].[Roles] ([Id], [RoleName], [Active]) VALUES (3, N'Kontrolor', 1)
SET IDENTITY_INSERT [dbo].[Roles] OFF
/****** Object:  Table [dbo].[Users]    Script Date: 08/06/2009 13:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdDataBase] [int] NOT NULL,
	[UserFullName] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserPass] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[SureName] [nvarchar](50) NULL,
	[Address] [nvarchar](200) NULL,
	[EMBG] [char](13) NULL,
	[BLK] [nvarchar](10) NULL,
	[DateOfBirth] [datetime] NULL,
	[DateOfHireing] [datetime] NULL CONSTRAINT [DF_Users_DateOfHireing]  DEFAULT (getdate()),
	[RFID] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_KORISNICI_AKTIVEN]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_KORISNICI] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([ID], [IdRole], [IdDataBase], [UserFullName], [UserName], [UserPass], [FirstName], [SureName], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (1, 1, 1, N'aaa', N'1', N'1', N'a', N'a', N'', N'0000         ', N'', NULL, CAST(0x00009AF800CDD7B4 AS DateTime), N'', 1)
INSERT [dbo].[Users] ([ID], [IdRole], [IdDataBase], [UserFullName], [UserName], [UserPass], [FirstName], [SureName], [Address], [EMBG], [BLK], [DateOfBirth], [DateOfHireing], [RFID], [Active]) VALUES (2, 2, 1, N'bbb', N'2', N'2', N'b', N'b', NULL, N'0000         ', NULL, NULL, CAST(0x00009AF800CDD82D AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
/****** Object:  Table [dbo].[FieldsPrivileges]    Script Date: 08/06/2009 13:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FieldsPrivileges](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdCSLAObject] [int] NOT NULL,
	[CSLAObjectPropertyName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_FieldsPrivileges_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_FieldsPrivileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[FieldsPrivileges] ON
INSERT [dbo].[FieldsPrivileges] ([Id], [IdRole], [IdCSLAObject], [CSLAObjectPropertyName], [Active]) VALUES (1, 2, 8, N'DateOfBirth', 0)
INSERT [dbo].[FieldsPrivileges] ([Id], [IdRole], [IdCSLAObject], [CSLAObjectPropertyName], [Active]) VALUES (2, 2, 1, N'CommunityCode', 0)
SET IDENTITY_INSERT [dbo].[FieldsPrivileges] OFF
/****** Object:  Table [dbo].[ObjectPrivileges]    Script Date: 08/06/2009 13:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectPrivileges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRole] [int] NOT NULL,
	[IdCSLAObject] [int] NOT NULL,
	[CanAddObject] [bit] NOT NULL,
	[CanGetObject] [bit] NOT NULL,
	[CanDeleteObject] [bit] NOT NULL,
	[CanEditObject] [bit] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ObjectPrivileges_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ObjectPrivileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ObjectPrivileges] ON
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (1, 1, 1, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (2, 2, 1, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (3, 1, 3, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (4, 1, 5, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (6, 1, 6, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (8, 1, 8, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (10, 1, 7, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (11, 1, 9, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (12, 1, 17, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (13, 1, 19, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (16, 1, 21, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (17, 1, 22, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (19, 1, 23, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (20, 1, 24, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (21, 1, 25, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (22, 1, 26, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (23, 1, 27, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (25, 1, 28, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (26, 1, 29, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (27, 1, 30, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (28, 1, 31, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (29, 1, 32, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (30, 1, 33, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (31, 1, 34, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (32, 1, 36, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (33, 1, 37, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (34, 1, 38, 1, 1, 1, 1, 0)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (35, 1, 39, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (36, 1, 40, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (37, 1, 41, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (38, 1, 42, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (39, 1, 43, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (40, 1, 44, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (41, 1, 45, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (42, 1, 46, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (43, 1, 47, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (44, 1, 48, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (45, 1, 49, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (46, 1, 50, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (47, 1, 51, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (48, 1, 53, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (49, 1, 52, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (50, 1, 54, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (51, 1, 55, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (52, 1, 56, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (53, 1, 57, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (54, 1, 58, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (55, 1, 59, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (56, 1, 60, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (57, 1, 61, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (58, 1, 62, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (59, 1, 63, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (61, 1, 64, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (63, 1, 65, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (66, 1, 66, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (67, 1, 68, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (68, 1, 69, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (69, 1, 70, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (71, 1, 71, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (72, 1, 72, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (74, 1, 74, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (76, 1, 75, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (77, 1, 76, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (78, 1, 77, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (79, 1, 78, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (80, 1, 79, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (81, 1, 80, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (82, 1, 81, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (83, 1, 82, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (84, 1, 83, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (85, 1, 84, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (86, 1, 86, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (87, 2, 93, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (88, 1, 93, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (89, 1, 90, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (90, 1, 94, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (91, 1, 12, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (92, 1, 95, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (93, 1, 96, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (94, 1, 97, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (95, 1, 111, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (96, 1, 99, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (97, 1, 127, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (98, 1, 125, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (99, 1, 126, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (100, 1, 100, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (101, 1, 101, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (102, 1, 104, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (103, 1, 105, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (104, 1, 129, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (105, 1, 123, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (106, 1, 112, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (107, 1, 113, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (108, 1, 114, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (109, 1, 115, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (110, 1, 116, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (111, 1, 117, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (112, 1, 106, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (113, 1, 118, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (114, 1, 119, 1, 1, 1, 1, 1)
GO
print 'Processed 100 total records'
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (115, 1, 120, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (116, 1, 98, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (117, 1, 130, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (118, 1, 128, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (119, 1, 131, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (120, 1, 132, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (121, 1, 110, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (122, 1, 133, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (123, 1, 134, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (124, 1, 137, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (125, 1, 138, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (126, 1, 139, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (127, 1, 140, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (128, 1, 141, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (129, 1, 142, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (130, 1, 143, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (131, 1, 144, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (132, 1, 145, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (133, 1, 146, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (134, 1, 147, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (135, 2, 74, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (136, 2, 6, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (137, 2, 25, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (138, 2, 94, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (139, 2, 34, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (140, 2, 65, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (141, 2, 3, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (142, 2, 31, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (143, 2, 95, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (144, 2, 8, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (145, 2, 96, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (146, 2, 97, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (147, 2, 37, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (148, 2, 40, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (149, 2, 41, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (150, 2, 79, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (151, 2, 99, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (152, 2, 63, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (153, 2, 60, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (154, 2, 61, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (155, 2, 46, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (156, 2, 75, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (157, 2, 125, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (158, 2, 126, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (159, 2, 48, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (160, 2, 100, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (161, 2, 43, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (162, 2, 39, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (163, 2, 78, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (164, 2, 104, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (165, 2, 62, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (166, 2, 105, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (167, 2, 64, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (168, 2, 129, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (169, 2, 76, 0, 1, 0, 0, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (170, 2, 112, 0, 1, 0, 0, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (171, 2, 113, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (172, 2, 114, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (173, 2, 115, 0, 1, 0, 0, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (174, 2, 116, 0, 1, 0, 0, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (175, 2, 58, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (176, 2, 117, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (177, 2, 49, 0, 1, 0, 0, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (178, 2, 81, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (179, 2, 106, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (180, 2, 84, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (181, 2, 118, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (182, 2, 119, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (183, 2, 83, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (184, 2, 120, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (185, 2, 7, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (186, 2, 98, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (187, 2, 42, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (188, 2, 130, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (189, 2, 56, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (190, 2, 47, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (191, 2, 55, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (192, 2, 128, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (193, 2, 54, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (194, 2, 68, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (195, 2, 131, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (196, 2, 132, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (197, 2, 28, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (198, 2, 133, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (199, 2, 134, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (200, 2, 22, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (201, 2, 137, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (202, 2, 5, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (203, 2, 138, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (204, 2, 17, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (205, 2, 51, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (206, 2, 70, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (207, 2, 139, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (208, 2, 71, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (209, 2, 140, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (210, 2, 72, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (211, 2, 141, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (212, 2, 27, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (213, 2, 19, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (214, 2, 66, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (215, 2, 21, 1, 1, 1, 1, 1)
GO
print 'Processed 200 total records'
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (216, 2, 142, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (217, 2, 9, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (218, 2, 143, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (219, 2, 144, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (220, 2, 145, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (221, 2, 24, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (222, 2, 30, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (223, 2, 29, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (224, 2, 23, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (225, 2, 52, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (226, 2, 53, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (227, 2, 26, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (228, 2, 146, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (229, 2, 33, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (230, 2, 80, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (231, 2, 69, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (232, 2, 147, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (233, 2, 82, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (234, 2, 57, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (235, 3, 46, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (236, 3, 75, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (237, 3, 126, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (238, 3, 125, 1, 1, 1, 1, 1)
INSERT [dbo].[ObjectPrivileges] ([Id], [IdRole], [IdCSLAObject], [CanAddObject], [CanGetObject], [CanDeleteObject], [CanEditObject], [Active]) VALUES (239, 1, 150, 1, 1, 1, 1, 1)
SET IDENTITY_INSERT [dbo].[ObjectPrivileges] OFF
/****** Object:  View [dbo].[View_1]    Script Date: 08/06/2009 13:33:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[View_1]
AS
SELECT     dbo.Users.ID AS IdUser, dbo.Users.IdRole, dbo.Users.IdDataBase, dbo.Users.UserFullName, dbo.Users.UserName, dbo.Users.UserPass, 
                      dbo.Roles.RoleName, dbo.DataBases.EndUserName, dbo.DataBases.DatabaseName, dbo.DataBases.ConnetionString
FROM         dbo.Users INNER JOIN
                      dbo.Roles ON dbo.Users.IdRole = dbo.Roles.Id INNER JOIN
                      dbo.DataBases ON dbo.Users.IdDataBase = dbo.DataBases.Id
WHERE     (dbo.Users.Active = 1)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 4
         End
         Begin Table = "Roles"
            Begin Extent = 
               Top = 6
               Left = 228
               Bottom = 121
               Right = 380
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "DataBases"
            Begin Extent = 
               Top = 6
               Left = 418
               Bottom = 121
               Right = 576
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1560
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_1'
GO
/****** Object:  StoredProcedure [dbo].[updateDataBase]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateDataBase]
		@Id int,
		@EndUserName nvarchar(50),
		@DatabaseName nvarchar(50),
		@ConnetionString nvarchar(250),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [DataBases]
		SET
		EndUserName = @endusername,
		DatabaseName = @databasename,
		ConnetionString = @connetionstring
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [DataBases] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[Login]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan Karpuzov
-- Create date: 
-- Description:	procedura za logiranje i vcuituvanje na privilegii
-- =============================================
CREATE PROCEDURE [dbo].[Login] 
	-- Add the parameters for the stored procedure here
	@user nvarchar(50), 
	@pw nvarchar(50)
AS
SELECT	dbo.Users.ID AS IdUser,
		dbo.Users.IdRole,
		dbo.Users.IdDataBase,
		dbo.Users.UserFullName,
		dbo.Users.UserName,
		dbo.Users.UserPass, 
        dbo.Roles.RoleName,
		dbo.DataBases.EndUserName,
		dbo.DataBases.DatabaseName,
		dbo.DataBases.ConnetionString,
		dbo.Users.ID As IdEmployee
		
FROM	dbo.Users INNER JOIN
		dbo.Roles ON dbo.Users.IdRole = dbo.Roles.Id INNER JOIN
		dbo.DataBases ON dbo.Users.IdDataBase = dbo.DataBases.Id
WHERE (dbo.Users.Active = 1) AND (dbo.Users.UserName=@user) AND (dbo.Users.UserPass=@pw)
GO
/****** Object:  StoredProcedure [dbo].[addDataBase]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addDataBase]
		@EndUserName nvarchar(50),
		@DatabaseName nvarchar(50),
		@ConnetionString nvarchar(250),
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [DataBases]
		(
		EndUserName,
		DatabaseName,
		ConnetionString
		)
		VALUES
		(
		@EndUserName,
		@DatabaseName,
		@ConnetionString
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [DataBases] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[deleteDataBase]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteDataBase]
	@id int
AS
    UPDATE [DataBases]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[getDataBases]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getDataBases]
AS
    SELECT 
        Id,
        EndUserName,
        DatabaseName,
        ConnetionString,
        LastChanged
    FROM [DataBases]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getDataBaseById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getDataBaseById] 
	@id int
AS
    SELECT 
        Id,
        EndUserName,
        DatabaseName,
        ConnetionString,
        LastChanged
    FROM   [DataBases]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[getConnectionString]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[getConnectionString] 
	-- Add the parameters for the stored procedure here
	@userId bigint
AS
SELECT     dbo.Users.ID AS UserId, dbo.DataBases.ConnetionString, dbo.DataBases.DatabaseName
FROM         dbo.Users INNER JOIN
                      dbo.DataBases ON dbo.Users.IdDataBase = dbo.DataBases.Id
WHERE Users.id=@userId
GO
/****** Object:  StoredProcedure [dbo].[GetPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ivan
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetPrivileges]
AS
SELECT     dbo.Roles.RoleName, dbo.CSLAObjects.CSLAObjectName, dbo.ObjectPrivileges.CanAddObject, dbo.ObjectPrivileges.CanGetObject, 
                      dbo.ObjectPrivileges.CanDeleteObject, dbo.ObjectPrivileges.CanEditObject
FROM         dbo.Roles INNER JOIN
                      dbo.ObjectPrivileges ON dbo.Roles.Id = dbo.ObjectPrivileges.IdRole INNER JOIN
                      dbo.CSLAObjects ON dbo.ObjectPrivileges.IdCSLAObject = dbo.CSLAObjects.Id
WHERE     (dbo.ObjectPrivileges.Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[GetChildObjectPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetChildObjectPrivileges] 
	-- Add the parameters for the stored procedure here
	@user nvarchar(50), 
	@pw nvarchar(50)
as
select  dbo.ObjectPrivileges.CanAddObject,
		dbo.ObjectPrivileges.CanGetObject,
		dbo.ObjectPrivileges.CanDeleteObject,
		dbo.ObjectPrivileges.CanEditObject,
		dbo.CSLAObjects.CSLAObjectName,
		dbo.CSLAObjects.CSLAObjectType,
		dbo.Users.ID
from    dbo.ObjectPrivileges inner join	dbo.CSLAObjects 
		on dbo.ObjectPrivileges.IdCSLAObject = dbo.CSLAObjects.Id 
		inner join dbo.Users
		on dbo.ObjectPrivileges.IdRole = dbo.Users.IdRole
where (dbo.ObjectPrivileges.Active = 1) AND (dbo.Users.UserName=@user) AND (dbo.Users.UserPass=@pw) 
		and (dbo.ObjectPrivileges.Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[GetChildFieldsPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create PROCEDURE [dbo].[GetChildFieldsPrivileges] 
	-- Add the parameters for the stored procedure here
	@user nvarchar(50), 
	@pw nvarchar(50)
as
select  dbo.FieldsPrivileges.CSLAObjectPropertyName,
		dbo.CSLAObjects.CSLAObjectName,
		dbo.CSLAObjects.CSLAObjectType,
		dbo.Users.ID
from    dbo.FieldsPrivileges inner join	dbo.CSLAObjects 
		on dbo.FieldsPrivileges.IdCSLAObject = dbo.CSLAObjects.Id 
		inner join dbo.Users
		on dbo.FieldsPrivileges.IdRole = dbo.Users.IdRole
where (dbo.Users.Active = 1) AND (dbo.Users.UserName=@user) AND (dbo.Users.UserPass=@pw) 
		and (dbo.FieldsPrivileges.Active = 1)
GO
/****** Object:  View [dbo].[FieldPrivilegesList]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[FieldPrivilegesList]
AS
SELECT     dbo.Roles.RoleName, dbo.CSLAObjects.CSLAObjectName, dbo.CSLAObjects.CSLAObjectType, dbo.FieldsPrivileges.CSLAObjectPropertyName, 
                      dbo.FieldsPrivileges.Id, dbo.Roles.Id AS IdRole, dbo.CSLAObjects.Id AS IdObject
FROM         dbo.Roles INNER JOIN
                      dbo.FieldsPrivileges ON dbo.Roles.Id = dbo.FieldsPrivileges.IdRole INNER JOIN
                      dbo.CSLAObjects ON dbo.FieldsPrivileges.IdCSLAObject = dbo.CSLAObjects.Id
WHERE     (dbo.FieldsPrivileges.Active = 1)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Roles"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 190
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FieldsPrivileges"
            Begin Extent = 
               Top = 97
               Left = 462
               Bottom = 212
               Right = 669
            End
            DisplayFlags = 280
            TopColumn = 1
         End
         Begin Table = "CSLAObjects"
            Begin Extent = 
               Top = 12
               Left = 287
               Bottom = 127
               Right = 452
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 2490
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FieldPrivilegesList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'FieldPrivilegesList'
GO
/****** Object:  StoredProcedure [dbo].[getCSLAObjects]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getCSLAObjects]
AS
    SELECT 
        Id,
        IdCSLAObject,
        CSLAObjectName,
        CSLAObjectType,
        LastChanged
    FROM [CSLAObjects]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getCSLAObjectById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getCSLAObjectById] 
	@id int
AS
    SELECT 
        Id,
        IdCSLAObject,
        CSLAObjectName,
        CSLAObjectType,
        LastChanged
    FROM   [CSLAObjects]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[deleteCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteCSLAObject]
	@id int
AS
    UPDATE [CSLAObjects]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[addCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addCSLAObject]
		@IdCSLAObject int,
		@CSLAObjectName nvarchar(100),
		@CSLAObjectType nvarchar(100),
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [CSLAObjects]
		(
		IdCSLAObject,
		CSLAObjectName,
		CSLAObjectType
		)
		VALUES
		(
		@IdCSLAObject,
		@CSLAObjectName,
		@CSLAObjectType
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [CSLAObjects] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  View [dbo].[ObjectPrivilegesList]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ObjectPrivilegesList]
AS
SELECT     dbo.ObjectPrivileges.Id, dbo.ObjectPrivileges.IdRole, dbo.Roles.RoleName, dbo.ObjectPrivileges.IdCSLAObject, dbo.CSLAObjects.CSLAObjectName, 
                      dbo.ObjectPrivileges.CanAddObject, dbo.ObjectPrivileges.CanGetObject, dbo.ObjectPrivileges.CanDeleteObject, 
                      dbo.ObjectPrivileges.CanEditObject
FROM         dbo.ObjectPrivileges INNER JOIN
                      dbo.CSLAObjects ON dbo.ObjectPrivileges.IdCSLAObject = dbo.CSLAObjects.Id INNER JOIN
                      dbo.Roles ON dbo.ObjectPrivileges.IdRole = dbo.Roles.Id
WHERE     (dbo.ObjectPrivileges.Active = 1)
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "ObjectPrivileges"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 121
               Right = 201
            End
            DisplayFlags = 280
            TopColumn = 5
         End
         Begin Table = "CSLAObjects"
            Begin Extent = 
               Top = 6
               Left = 239
               Bottom = 121
               Right = 404
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Roles"
            Begin Extent = 
               Top = 6
               Left = 442
               Bottom = 121
               Right = 594
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ObjectPrivilegesList'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'ObjectPrivilegesList'
GO
/****** Object:  StoredProcedure [dbo].[updateCSLAObject]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateCSLAObject]
		@Id int,
		@IdCSLAObject int,
		@CSLAObjectName nvarchar(100),
		@CSLAObjectType nvarchar(100),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [CSLAObjects]
		SET
		IdCSLAObject = @idcslaobject,
		CSLAObjectName = @cslaobjectname,
		CSLAObjectType = @cslaobjecttype
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [CSLAObjects] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[updateFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateFieldsPrivilege]
		@Id bigint,
		@IdRole int,
		@IdCSLAObject int,
		@CSLAObjectPropertyName nvarchar(50),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [FieldsPrivileges]
		SET
		IdRole = @idrole,
		IdCSLAObject = @idcslaobject,
		CSLAObjectPropertyName = @cslaobjectpropertyname
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [FieldsPrivileges] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[getFieldsPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getFieldsPrivileges]
AS
    SELECT 
        Id,
        IdRole,
        IdCSLAObject,
        CSLAObjectPropertyName,
        LastChanged
    FROM [FieldsPrivileges]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getFieldsPrivilegeById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getFieldsPrivilegeById] 
	@id bigint
AS
    SELECT 
        Id,
        IdRole,
        IdCSLAObject,
        CSLAObjectPropertyName,
        LastChanged
    FROM   [FieldsPrivileges]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[deleteFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteFieldsPrivilege]
	@id bigint
AS
    UPDATE [FieldsPrivileges]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[addFieldsPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addFieldsPrivilege]
		@IdRole int,
		@IdCSLAObject int,
		@CSLAObjectPropertyName nvarchar(50),
		@NewId bigint output,
		@newLastChanged timestamp output
AS
    INSERT INTO [FieldsPrivileges]
		(
		IdRole,
		IdCSLAObject,
		CSLAObjectPropertyName
		)
		VALUES
		(
		@IdRole,
		@IdCSLAObject,
		@CSLAObjectPropertyName
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [FieldsPrivileges] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[addObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addObjectPrivilege]
		@IdRole int,
		@IdCSLAObject int,
		@CanAddObject bit,
		@CanGetObject bit,
		@CanDeleteObject bit,
		@CanEditObject bit,
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [ObjectPrivileges]
		(
		IdRole,
		IdCSLAObject,
		CanAddObject,
		CanGetObject,
		CanDeleteObject,
		CanEditObject
		)
		VALUES
		(
		@IdRole,
		@IdCSLAObject,
		@CanAddObject,
		@CanGetObject,
		@CanDeleteObject,
		@CanEditObject
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [ObjectPrivileges] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[getObjectPrivileges]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[getObjectPrivileges]
AS
    SELECT 
        Id,
        IdRole,
        IdCSLAObject,
        CanAddObject,
        CanGetObject,
        CanDeleteObject,
        CanEditObject,
        LastChanged
    FROM [ObjectPrivileges]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getObjectPrivilegeById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getObjectPrivilegeById] 
	@id int
AS
    SELECT 
        Id,
        IdRole,
        IdCSLAObject,
        CanAddObject,
        CanGetObject,
        CanDeleteObject,
        CanEditObject,
        LastChanged
    FROM   [ObjectPrivileges]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[updateObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateObjectPrivilege]
		@Id int,
		@IdRole int,
		@IdCSLAObject int,
		@CanAddObject bit,
		@CanGetObject bit,
		@CanDeleteObject bit,
		@CanEditObject bit,
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [ObjectPrivileges]
		SET
		IdRole = @idrole,
		IdCSLAObject = @idcslaobject,
		CanAddObject = @canaddobject,
		CanGetObject = @cangetobject,
		CanDeleteObject = @candeleteobject,
		CanEditObject = @caneditobject
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [ObjectPrivileges] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[deleteObjectPrivilege]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteObjectPrivilege]
	@id int
AS
    UPDATE [ObjectPrivileges]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[deleteUser]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteUser]
	@id bigint
AS
    UPDATE [Users]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[getUsers]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getUsers]
AS
    SELECT 
        ID,
        IdRole,
        IdDataBase,
        UserFullName,
        UserName,
        UserPass,
        FirstName,
        SureName,
        Address,
        EMBG,
        BLK,
        DateOfBirth,
        DateOfHireing,
        RFID,
        LastChanged
    FROM [Users]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getUserById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getUserById] 
	@id bigint
AS
    SELECT 
        ID,
        IdRole,
        IdDataBase,
        UserFullName,
        UserName,
        UserPass,
        FirstName,
        SureName,
        Address,
        EMBG,
        BLK,
        DateOfBirth,
        DateOfHireing,
        RFID,
        LastChanged
    FROM   [Users]
	WHERE (ID = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[updateUser]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateUser]
		@ID bigint,
		@IdRole int,
		@IdDataBase int,
		@UserFullName nvarchar(50),
		@UserName nvarchar(50),
		@UserPass nvarchar(50),
		@FirstName nvarchar(50),
		@SureName nvarchar(50),
		@Address nvarchar(200),
		@EMBG char(13),
		@BLK nvarchar(10),
		@DateOfBirth datetime,
		@DateOfHireing datetime,
		@RFID nvarchar(50),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [Users]
		SET
		IdRole = @idrole,
		IdDataBase = @iddatabase,
		UserFullName = @userfullname,
		UserName = @username,
		UserPass = @userpass,
		FirstName = @firstname,
		SureName = @surename,
		Address = @address,
		EMBG = @embg,
		BLK = @blk,
		DateOfBirth = @dateofbirth,
		DateOfHireing = @dateofhireing,
		RFID = @rfid
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [Users] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[addUser]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addUser]
		@IdRole int,
		@IdDataBase int,
		@UserFullName nvarchar(50),
		@UserName nvarchar(50),
		@UserPass nvarchar(50),
		@FirstName nvarchar(50),
		@SureName nvarchar(50),
		@Address nvarchar(200),
		@EMBG char(13),
		@BLK nvarchar(10),
		@DateOfBirth datetime,
		@DateOfHireing datetime,
		@RFID nvarchar(50),
		@NewId bigint output,
		@newLastChanged timestamp output
AS
    INSERT INTO [Users]
		(
		IdRole,
		IdDataBase,
		UserFullName,
		UserName,
		UserPass,
		FirstName,
		SureName,
		Address,
		EMBG,
		BLK,
		DateOfBirth,
		DateOfHireing,
		RFID
		)
		VALUES
		(
		@IdRole,
		@IdDataBase,
		@UserFullName,
		@UserName,
		@UserPass,
		@FirstName,
		@SureName,
		@Address,
		@EMBG,
		@BLK,
		@DateOfBirth,
		@DateOfHireing,
		@RFID
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [Users] WHERE ID=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[deleteEmploye]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteEmploye]
	@id int
AS
    UPDATE [Employes]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[addEmploye]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addEmploye]
		@IdWorkPosition int,
		@FirstName nvarchar(50),
		@Prezime nvarchar(50),
		@Address nvarchar(200),
		@EMBG char(13),
		@BLK nvarchar(10),
		@DateOfBirth datetime,
		@DateOfHireing datetime,
		@RFID nvarchar(50),
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [Employes]
		(
		IdWorkPosition,
		FirstName,
		Prezime,
		Address,
		EMBG,
		BLK,
		DateOfBirth,
		DateOfHireing,
		RFID
		)
		VALUES
		(
		@IdWorkPosition,
		@FirstName,
		@Prezime,
		@Address,
		@EMBG,
		@BLK,
		@DateOfBirth,
		@DateOfHireing,
		@RFID
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [Employes] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[getEmployes]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getEmployes]
AS
    SELECT 
        Id,
        IdWorkPosition,
        FirstName,
        Prezime,
        Address,
        EMBG,
        BLK,
        DateOfBirth,
        DateOfHireing,
        RFID,
        LastChanged
    FROM [Employes]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getEmployeById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getEmployeById] 
	@id int
AS
    SELECT 
        Id,
        IdWorkPosition,
        FirstName,
        Prezime,
        Address,
        EMBG,
        BLK,
        DateOfBirth,
        DateOfHireing,
        RFID,
        LastChanged
    FROM   [Employes]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[updateEmploye]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateEmploye]
		@Id int,
		@IdWorkPosition int,
		@FirstName nvarchar(50),
		@Prezime nvarchar(50),
		@Address nvarchar(200),
		@EMBG char(13),
		@BLK nvarchar(10),
		@DateOfBirth datetime,
		@DateOfHireing datetime,
		@RFID nvarchar(50),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [Employes]
		SET
		IdWorkPosition = @idworkposition,
		FirstName = @firstname,
		Prezime = @prezime,
		Address = @address,
		EMBG = @embg,
		BLK = @blk,
		DateOfBirth = @dateofbirth,
		DateOfHireing = @dateofhireing,
		RFID = @rfid
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [Employes] WHERE ID=@id
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[deleteRole]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC [dbo].[deleteRole]
	@id int
AS
    UPDATE [Roles]
	SET
		Active=0
	WHERE ID=@id
GO
/****** Object:  StoredProcedure [dbo].[getRoles]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje site zapisi

CREATE PROC [dbo].[getRoles]
AS
    SELECT 
        Id,
        RoleName,
        LastChanged
    FROM [Roles]
 WHERE Active = 1
GO
/****** Object:  StoredProcedure [dbo].[getRoleById]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC [dbo].[getRoleById] 
	@id int
AS
    SELECT 
        Id,
        RoleName,
        LastChanged
    FROM   [Roles]
	WHERE (Id = @id) AND (Active = 1)
GO
/****** Object:  StoredProcedure [dbo].[addRole]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za dodavanje na nov zapis

CREATE PROC [dbo].[addRole]
		@RoleName nvarchar(50),
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [Roles]
		(
		RoleName
		)
		VALUES
		(
		@RoleName
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [Roles] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO
/****** Object:  StoredProcedure [dbo].[updateRole]    Script Date: 08/06/2009 13:33:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Description: Procedura za UPDATE na zapis

CREATE PROC [dbo].[updateRole]
		@Id int,
		@RoleName nvarchar(50),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [Roles]
		SET
		RoleName = @rolename
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [Roles] WHERE ID=@id
	RETURN
GO
/****** Object:  ForeignKey [FK_CSLAObjects_CSLAObjects]    Script Date: 08/06/2009 13:33:41 ******/
ALTER TABLE [dbo].[CSLAObjects]  WITH NOCHECK ADD  CONSTRAINT [FK_CSLAObjects_CSLAObjects] FOREIGN KEY([IdCSLAObject])
REFERENCES [dbo].[CSLAObjects] ([Id])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[CSLAObjects] CHECK CONSTRAINT [FK_CSLAObjects_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_Users_DataBases]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_DataBases] FOREIGN KEY([IdDataBase])
REFERENCES [dbo].[DataBases] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_DataBases]
GO
/****** Object:  ForeignKey [FK_Users_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Roles] FOREIGN KEY([IdRole])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Roles]
GO
/****** Object:  ForeignKey [FK_FieldsPrivileges_CSLAObjects]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[FieldsPrivileges]  WITH CHECK ADD  CONSTRAINT [FK_FieldsPrivileges_CSLAObjects] FOREIGN KEY([IdCSLAObject])
REFERENCES [dbo].[CSLAObjects] ([Id])
GO
ALTER TABLE [dbo].[FieldsPrivileges] CHECK CONSTRAINT [FK_FieldsPrivileges_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_FieldsPrivileges_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[FieldsPrivileges]  WITH CHECK ADD  CONSTRAINT [FK_FieldsPrivileges_Roles] FOREIGN KEY([IdRole])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[FieldsPrivileges] CHECK CONSTRAINT [FK_FieldsPrivileges_Roles]
GO
/****** Object:  ForeignKey [FK_ObjectPrivileges_CSLAObjects]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[ObjectPrivileges]  WITH CHECK ADD  CONSTRAINT [FK_ObjectPrivileges_CSLAObjects] FOREIGN KEY([IdCSLAObject])
REFERENCES [dbo].[CSLAObjects] ([Id])
GO
ALTER TABLE [dbo].[ObjectPrivileges] CHECK CONSTRAINT [FK_ObjectPrivileges_CSLAObjects]
GO
/****** Object:  ForeignKey [FK_ObjectPrivileges_Roles]    Script Date: 08/06/2009 13:33:42 ******/
ALTER TABLE [dbo].[ObjectPrivileges]  WITH CHECK ADD  CONSTRAINT [FK_ObjectPrivileges_Roles] FOREIGN KEY([IdRole])
REFERENCES [dbo].[Roles] ([Id])
GO
ALTER TABLE [dbo].[ObjectPrivileges] CHECK CONSTRAINT [FK_ObjectPrivileges_Roles]
GO
