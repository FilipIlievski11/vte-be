##### `DocumentsTrafficLicences.Extensions`

Source: `WinApp/sqlData.sql:8977-8990`

```sql
CREATE TABLE [dbo].[DocumentsTrafficLicences.Extensions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdTrafficLicence] [int] NOT NULL,
	[IdOperator] [int] NOT NULL,
	[ValidTill] [datetime] NOT NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTrafficLicences.Extensions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTrafficLicences.Extensions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTrafficLicences`

Source: `WinApp/sqlData.sql:9037-9052`

```sql
CREATE TABLE [dbo].[DocumentsTrafficLicences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdTehnicalExamOrganizationsIssuedBy] [int] NOT NULL,
	[TrafficLicenceNumber] [nvarchar](50) NULL,
	[MadeDate] [datetime] NOT NULL,
	[EndDate] [datetime] NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTrafficLicences_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTrafficLicences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReportsDetailsStatus`

Source: `WinApp/sqlData.sql:9058-9068`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReportsDetailsStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetailsStatus_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTehnicalExamsReportsDetailsStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentVehicleOwnershipProof`

Source: `WinApp/sqlData.sql:9221-9231`

```sql
CREATE TABLE [dbo].[DocumentVehicleOwnershipProof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VehicleOwnershipProofName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentVehicleOwnershipProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentVehicleOwnershipProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentPaymentProof`

Source: `WinApp/sqlData.sql:9426-9436`

```sql
CREATE TABLE [dbo].[DocumentPaymentProof](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PaymentProofName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentPaymentProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentPaymentProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsPermisions`

Source: `WinApp/sqlData.sql:9447-9467`

```sql
CREATE TABLE [dbo].[DocumentsPermisions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationOwner] [bigint] NOT NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdIssuer] [int] NOT NULL CONSTRAINT [DF_DocumentsPermisions_IdIssuer]  DEFAULT ((1)),
	[IdCityOfIssuing] [int] NOT NULL CONSTRAINT [DF_DocumentsPermisions_IdCityOfIssuing]  DEFAULT ((1)),
	[PermissionNumber] [nvarchar](50) NULL,
	[TrafficLicenceNumber] [nvarchar](50) NOT NULL CONSTRAINT [DF_DocumentsPermisions_TrafficLicenceNumber]  DEFAULT (N'непознат'),
	[TriptiqueNumber] [nvarchar](50) NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_DocumentsPermisions_DateCreated]  DEFAULT (getdate()),
	[ValidTillDate] [datetime] NOT NULL CONSTRAINT [DF_DocumentsPermisions_ValidTillDate]  DEFAULT (getdate()),
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsPermisions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsPermisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsInternationalDriveingLicences.ValidForCategories`

Source: `WinApp/sqlData.sql:9480-9492`

```sql
CREATE TABLE [dbo].[DocumentsInternationalDriveingLicences.ValidForCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdInternationalDrivingLicence] [bigint] NOT NULL,
	[IdLicenceCategorie] [int] NOT NULL,
	[IsCheck] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences.ValidForCategories_IsCheck]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences.ValidForCategories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsInternationalDriveingLicences.ValidForCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsInternationalDriveingLicences`

Source: `WinApp/sqlData.sql:9517-9534`

```sql
CREATE TABLE [dbo].[DocumentsInternationalDriveingLicences](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdIssuer] [int] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_IdCountryIssuedFor]  DEFAULT ((1)),
	[NumberOfLicence] [nvarchar](50) NOT NULL,
	[NumberOfNationalLicence] [nvarchar](50) NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_NumberOfNationalLicence]  DEFAULT (N'непознат'),
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_DateCreated]  DEFAULT (getdate()),
	[ValidTillDate] [datetime] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_ValidTillDate]  DEFAULT (getdate()),
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsInternationalDriveingLicences_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsInternationalDriveingLicences] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReports`

Source: `WinApp/sqlData.sql:11552-11573`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReports](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdTypeOfTehnicalExam] [int] NOT NULL,
	[RegNumber] [nvarchar](25) NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReports_RegNumber]  DEFAULT (N'/'),
	[MadeDate] [datetime] NOT NULL,
	[ValidTillDate] [datetime] NOT NULL,
	[IdOrganizationForTehnicalExam] [int] NOT NULL,
	[IdFirsControler] [int] NOT NULL,
	[IdSecondControler] [int] NOT NULL,
	[VehicleIsRight] [bit] NOT NULL CONSTRAINT [DF_Vehicle.TehnicalExams_VehicleIsRight]  DEFAULT ((0)),
	[ExplanationNote] [nvarchar](250) NULL,
	[DriversWarning] [nvarchar](250) NULL,
	[Note] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.TehnicalExams_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Vehicle.TehnicalExams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentsTehnicalExamsReportsDetails`

Source: `WinApp/sqlData.sql:11796-11814`

```sql
CREATE TABLE [dbo].[DocumentsTehnicalExamsReportsDetails](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdTehnicalExamsReports] [bigint] NOT NULL,
	[IdTehnicalExamVehivlePart] [int] NOT NULL,
	[IdStatus] [int] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_IdVehiclePartStatus]  DEFAULT ((3)),
	[Front] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Front]  DEFAULT ((0)),
	[Back] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Back]  DEFAULT ((0)),
	[OnLeft] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_OnLeft]  DEFAULT ((0)),
	[OnRight] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_OnRight]  DEFAULT ((0)),
	[DateEnter] [datetime] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Date]  DEFAULT (getdate()),
	[Note] [nvarchar](150) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentsTehnicalExamsReportsDetails_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentsTehnicalExamsReportsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Documents`

Source: `WinApp/sqlData.sql:12594-12622`

```sql
CREATE TABLE [dbo].[Documents](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdDocumentType] [int] NOT NULL,
	[IdDocumentTypeOption] [int] NULL,
	[IdDocumentTypeOptionDetail] [int] NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationHistory] [bigint] NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdOperatorModified] [int] NULL,
	[IdOperatorEnded] [int] NOT NULL CONSTRAINT [DF_Documents_IdOperatorEnded]  DEFAULT ((0)),
	[IdTechnicalExamReport] [bigint] NULL,
	[IdPreviousRegistration] [int] NOT NULL CONSTRAINT [DF_Documents_IdPreviousRegistration]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[DateEnded] [datetime] NULL,
	[IdVehicleOwnershipProof] [int] NULL,
	[IdPaymentProof] [int] NULL,
	[VehicleOwnershipProof] [nvarchar](250) NULL,
	[PaymentProof] [nvarchar](250) NULL,
	[IdPayAttachment] [bigint] NULL,
	[Note] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Documents_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Documents] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

