##### `Request.VehicleOwnershipProofs`

Source: `WinApp/sqlData.sql:6277-6289`

```sql
CREATE TABLE [dbo].[Request.VehicleOwnershipProofs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequest] [bigint] NOT NULL,
	[IdVehicleOwnershipProof] [int] NOT NULL,
	[VehicleOwnershipProof] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Request.VehicleOwnershipProofs_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Request.VehicleOwnershipProofs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Request.PaymentProof`

Source: `WinApp/sqlData.sql:6313-6325`

```sql
CREATE TABLE [dbo].[Request.PaymentProof](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequest] [bigint] NOT NULL,
	[IdPaymentProof] [int] NOT NULL,
	[PaymentProof] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Request.PaymentProof_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Request.PaymentProof] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Requests`

Source: `WinApp/sqlData.sql:8357-8380`

```sql
CREATE TABLE [dbo].[Requests](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRequestType] [int] NOT NULL,
	[IdCustomerVehicleRelation] [bigint] NOT NULL,
	[IdCustomerVehicleRelationNew] [bigint] NULL,
	[IdOperatorCreated] [int] NOT NULL,
	[IdOperatorModified] [int] NULL,
	[IdOperatorEnded] [int] NOT NULL CONSTRAINT [DF_Requests_IdOperatorEnded]  DEFAULT ((0)),
	[IdTechnicalExamReport] [bigint] NULL,
	[IdPreviousRegistration] [int] NOT NULL CONSTRAINT [DF_Requests_IdPreviousRegistration]  DEFAULT ((0)),
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NULL,
	[DateEnded] [datetime] NULL,
	[Note] [nvarchar](250) NULL,
	[IsCustomerChanged] [bit] NOT NULL CONSTRAINT [DF_Requests_IsCustomerChanged]  DEFAULT ((0)),
	[IsVehicleChanged] [bit] NOT NULL CONSTRAINT [DF_Requests_IsVehicleChanged]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Requests_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Requests] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

