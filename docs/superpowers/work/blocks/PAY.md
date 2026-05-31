##### `CalculationItems`

Source: `WinApp/sqlData.sql:8329-8342`

```sql
CREATE TABLE [dbo].[CalculationItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](150) NOT NULL,
	[BankAccount] [nvarchar](50) NOT NULL,
	[Bank] [nvarchar](150) NOT NULL,
	[Form] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CalculationItems_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CalculationItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DogovorZaRati`

Source: `WinApp/sqlData.sql:9103-9118`

```sql
CREATE TABLE [dbo].[DogovorZaRati](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Broj] [nvarchar](50) NOT NULL,
	[Datum] [datetime] NOT NULL,
	[GarantNaziv] [nvarchar](50) NOT NULL,
	[GarantAdresa] [nvarchar](250) NOT NULL,
	[GartEMB] [nvarchar](20) NOT NULL,
	[BrNaRati] [int] NOT NULL CONSTRAINT [DF_DogovorZaRati_BrNaRati]  DEFAULT ((1)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_DogovorZaRati_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_DogovorZaRati] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocuments`

Source: `WinApp/sqlData.sql:10732-10752`

```sql
CREATE TABLE [dbo].[PaymentDocuments](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentType] [int] NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdOperator] [int] NOT NULL,
	[DocumentNumber] [nvarchar](25) NOT NULL CONSTRAINT [DF_PaymentDocuments_DocumentNumber]  DEFAULT (N'/'),
	[DatePay] [datetime] NOT NULL,
	[DateRequired] [datetime] NOT NULL CONSTRAINT [DF_PaymentDocuments_DateRequired_1]  DEFAULT (getdate()),
	[Discount] [real] NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Payed]  DEFAULT ((0)),
	[Note] [nvarchar](150) NULL,
	[Storno] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Storno]  DEFAULT ((0)),
	[IdDogovor] [bigint] NOT NULL CONSTRAINT [DF_PaymentDocuments_IdDogovor]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocuments_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocuments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentItems`

Source: `WinApp/sqlData.sql:11220-11232`

```sql
CREATE TABLE [dbo].[PaymentItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPymentCategory] [int] NOT NULL,
	[IdVehicleCategoryForPayments] [int] NOT NULL,
	[ItemName] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentItems_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `SecurityHouses`

Source: `WinApp/sqlData.sql:11867-11878`

```sql
CREATE TABLE [dbo].[SecurityHouses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCity] [int] NOT NULL,
	[SecurityHouseName] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_SecurityHouses_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_SecurityHouses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentItemParametars`

Source: `WinApp/sqlData.sql:11906-11922`

```sql
CREATE TABLE [dbo].[PaymentItemParametars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdPaymentItem] [int] NOT NULL,
	[PrametarName] [nvarchar](250) NOT NULL,
	[VehicleField] [nvarchar](150) NOT NULL,
	[ParametarFrom] [real] NOT NULL,
	[ParametarTo] [real] NOT NULL,
	[Price] [money] NOT NULL,
	[IsOptional] [bit] NOT NULL CONSTRAINT [DF_PaymentItemParametars_IsOptional]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentItemParametars_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentItemParametars] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocumentsDetails`

Source: `WinApp/sqlData.sql:12482-12499`

```sql
CREATE TABLE [dbo].[PaymentDocumentsDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentDocuments] [bigint] NOT NULL,
	[IdPriceCatalog] [int] NOT NULL,
	[Price] [money] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Price]  DEFAULT ((0)),
	[DDV] [real] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_DDV]  DEFAULT ((0)),
	[Note] [nvarchar](150) NULL,
	[PrePayed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Payed]  DEFAULT ((0)),
	[NotePrePayed] [nvarchar](150) NULL,
	[Discount] [real] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Discount]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsDetails_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocumentsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentDocumentsRata`

Source: `WinApp/sqlData.sql:12505-12519`

```sql
CREATE TABLE [dbo].[PaymentDocumentsRata](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPaymentDocument] [bigint] NOT NULL,
	[Price] [money] NOT NULL,
	[Payed] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsRata_Payed]  DEFAULT ((0)),
	[DatePayed] [datetime] NULL,
	[Note] [nchar](10) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentDocumentsRata_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentDocumentsRata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

