##### `DataBases`

Source: `WinApp/sqlData.sql:285-297`

```sql
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
```

##### `CSLAObjects`

Source: `WinApp/sqlData.sql:306-318`

```sql
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
```

##### `Employes`

Source: `WinApp/sqlData.sql:461-479`

```sql
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
```

##### `Roles`

Source: `WinApp/sqlData.sql:493-503`

```sql
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
```

##### `Users`

Source: `WinApp/sqlData.sql:516-538`

```sql
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
```

##### `FieldsPrivileges`

Source: `WinApp/sqlData.sql:550-562`

```sql
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
```

##### `ObjectPrivileges`

Source: `WinApp/sqlData.sql:572-587`

```sql
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
```

##### `SecurityPolicies`

Source: `WinApp/sqlData.sql:12650-12661`

```sql
CREATE TABLE [dbo].[SecurityPolicies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdScurityHouse] [int] NOT NULL,
	[NumberOfPolicy] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_SecurityPolicies_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_SecurityPolicies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

