##### `CustomersBankAccounts`

Source: `WinApp/sqlData.sql:9381-9394`

```sql
CREATE TABLE [dbo].[CustomersBankAccounts](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[BankAccount] [nvarchar](50) NOT NULL,
	[DeponentBank] [nvarchar](50) NOT NULL,
	[TaxNumber] [nvarchar](15) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomersBankAccounts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_CustomersBankAccounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Customers`

Source: `WinApp/sqlData.sql:10673-10704`

```sql
CREATE TABLE [dbo].[Customers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MB] [nvarchar](13) NULL,
	[CustomerSurname] [nvarchar](100) NULL,
	[CustomerFirstName] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Fax] [nvarchar](20) NULL,
	[IdLivingAddress] [int] NULL,
	[LivingAddressNumber] [nvarchar](100) NULL,
	[IdLivingCity] [int] NULL,
	[IdBirhCity] [int] NULL,
	[IdBirthAddress] [int] NULL,
	[BrithAddressNumber] [nvarchar](100) NULL,
	[DateOfBirth] [datetime] NULL,
	[IdCitizenship] [int] NULL,
	[IsCompany] [bit] NOT NULL CONSTRAINT [DF_Customers_IsCompany]  DEFAULT ((0)),
	[Occupation] [nvarchar](50) NULL,
	[WorksInCompany] [nvarchar](150) NULL,
	[IdBusinessType] [int] NULL,
	[eMail] [nvarchar](100) NULL,
	[PassportNumber] [nvarchar](100) NULL,
	[BLK] [nvarchar](50) NULL,
	[CanSendNotifications] [bit] NOT NULL CONSTRAINT [DF_Customers_CanSendNotifications]  DEFAULT ((0)),
	[TaxNumber] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Customers_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerFinancialState`

Source: `WinApp/sqlData.sql:11842-11861`

```sql
CREATE TABLE [dbo].[CustomerFinancialState](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL CONSTRAINT [DF_Table_1_IdCustomer]  DEFAULT ((0)),
	[IdDocument] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocument]  DEFAULT ((0)),
	[IdDocumentTehnicalExam] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentTehnicalExam]  DEFAULT ((0)),
	[IdDocumentsTrafficLicences] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentsTrafficLicences]  DEFAULT ((0)),
	[IdDocumentIternationalDriveingLicence] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentIternationalDriveingLicence]  DEFAULT ((0)),
	[IdDocumentPermisions] [bigint] NOT NULL CONSTRAINT [DF_CustomerFinancialState_IdDocumentPermisions]  DEFAULT ((0)),
	[IdPriceCatalog] [int] NOT NULL,
	[Note] [nvarchar](150) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_CustomerFinancialState_Payed]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerFinancialState_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerFinancialState] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Customers.ContactPersons`

Source: `WinApp/sqlData.sql:12628-12644`

```sql
CREATE TABLE [dbo].[Customers.ContactPersons](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[MB] [nvarchar](13) NOT NULL,
	[PersonName] [nvarchar](50) NOT NULL,
	[PersonSurname] [nvarchar](50) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[MobileNumber] [nvarchar](20) NULL,
	[Email] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Customers.ContactPersons_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Customers.ContactPersons] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

