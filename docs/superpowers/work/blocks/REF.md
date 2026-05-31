##### `VehicleDisabledFields`

Source: `WinApp/sqlData.sql:4783-4794`

```sql
CREATE TABLE [dbo].[VehicleDisabledFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[FieldName] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleDisabledFields_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleDisabledFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategoryForPayments`

Source: `WinApp/sqlData.sql:4848-4860`

```sql
CREATE TABLE [dbo].[VehicleCategoryForPayments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ZelenMap] [int] NOT NULL CONSTRAINT [DF_VehicleCategoryForPayments_ZelenMap]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategoryForPayments_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleCategoryForPayments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PaymentTypes`

Source: `WinApp/sqlData.sql:4884-4901`

```sql
CREATE TABLE [dbo].[PaymentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Fiskalna_kes] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Fiskalna_kes]  DEFAULT ((0)),
	[Fiskalna_karticka] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Fiskalna_karticka]  DEFAULT ((0)),
	[Rati] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Rati]  DEFAULT ((0)),
	[Smetka] [bit] NOT NULL,
	[Faktura] [bit] NOT NULL,
	[PrintText] [nvarchar](50) NOT NULL CONSTRAINT [DF_PaymentTypes_PrintText]  DEFAULT (N'print'),
	[Prefix] [nvarchar](15) NULL CONSTRAINT [DF_PaymentTypes_Prefix]  DEFAULT ((5)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_PaymentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleMakers`

Source: `WinApp/sqlData.sql:4915-4927`

```sql
CREATE TABLE [dbo].[VehicleMakers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCountry] [int] NOT NULL CONSTRAINT [DF_VehicleMakers_IdCountry]  DEFAULT ((1)),
	[CompanyName] [nvarchar](250) NOT NULL,
	[CompanyTrademark] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleMakers_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleMakers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleJUSCategories`

Source: `WinApp/sqlData.sql:5054-5066`

```sql
CREATE TABLE [dbo].[VehicleJUSCategories](
	[Id] [int] NOT NULL,
	[PictureJusPath] [nvarchar](250) NOT NULL,
	[CategoryJusCode] [nvarchar](20) NOT NULL,
	[CategoryJusName] [nvarchar](50) NOT NULL,
	[CategoryJusDescription] [ntext] NULL,
	[CategoryJusPath] [nvarchar](250) NULL,
 CONSTRAINT [PK_VehicleJUSCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

##### `PaymentCategories`

Source: `WinApp/sqlData.sql:5072-5092`

```sql
CREATE TABLE [dbo].[PaymentCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDDV] [int] NOT NULL,
	[IdCalculationItem] [int] NOT NULL CONSTRAINT [DF_PaymentCategories_IdCalculationItem]  DEFAULT ((0)),
	[CategoryName] [nvarchar](250) NOT NULL,
	[AllowDiscount] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_AllowDiscount]  DEFAULT ((0)),
	[TrigerdByRequest] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByRequest]  DEFAULT ((0)),
	[TrigerdByTechnicalExam] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByTechnicalExam]  DEFAULT ((0)),
	[TrigerdByTrafficLicence] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByTrafficLicence]  DEFAULT ((0)),
	[TrigerdByPremisionForVehicle] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByPremisionForVehicle]  DEFAULT ((0)),
	[TrigerdByInternationalDrivierLicence] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByInternationalDrivierLicence]  DEFAULT ((0)),
	[TrigerdByIrregularTechnicalExam] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_TrigerdByIrregularTechnicalExam]  DEFAULT ((0)),
	[VisibleOrder] [int] NOT NULL CONSTRAINT [DF_PaymentCategories_VisibleOrder]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PaymentCategories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PaymentCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleGearBox`

Source: `WinApp/sqlData.sql:5120-5131`

```sql
CREATE TABLE [dbo].[VehicleGearBox](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GearBoxCode] [int] NULL,
	[GearBoxDescription] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesGearBox_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehiclesGearBox] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleUse`

Source: `WinApp/sqlData.sql:5149-5160`

```sql
CREATE TABLE [dbo].[VehicleUse](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UseDescription] [nvarchar](250) NOT NULL,
	[RegistrationMask] [nvarchar](150) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleUse_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleUse] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleTireTypes`

Source: `WinApp/sqlData.sql:5225-5239`

```sql
CREATE TABLE [dbo].[VehicleTireTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleModel] [int] NULL CONSTRAINT [DF_VehicleTireTypes_IdVehicleModel]  DEFAULT ((1)),
	[Seria] [nvarchar](50) NOT NULL,
	[TireType] [nvarchar](150) NOT NULL,
	[Dimenzions] [decimal](18, 0) NOT NULL,
	[Note] [nvarchar](50) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleTireTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleTireTypes_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleSupporting`

Source: `WinApp/sqlData.sql:5327-5338`

```sql
CREATE TABLE [dbo].[VehicleSupporting](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SupportingCode] [int] NULL,
	[SupportingDescription] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleSupporting_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleSupporting] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamOrganizations`

Source: `WinApp/sqlData.sql:5351-5363`

```sql
CREATE TABLE [dbo].[TehnicalExamOrganizations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrganizationName] [nvarchar](150) NOT NULL,
	[Station] [nvarchar](150) NULL,
	[IdCity] [int] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamOrganizations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamOrganizations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamVehiclePartsCategories`

Source: `WinApp/sqlData.sql:5477-5488`

```sql
CREATE TABLE [dbo].[TehnicalExamVehiclePartsCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](150) NOT NULL,
	[CategoryCode] [nvarchar](50) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamCategoryOfVehicleParts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamCategoryOfVehicleParts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamVehicleParts`

Source: `WinApp/sqlData.sql:5494-5507`

```sql
CREATE TABLE [dbo].[TehnicalExamVehicleParts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategoryVehicleParts] [int] NOT NULL CONSTRAINT [DF_TehnicalExamVehicleParts_IdCategoryVehicleParts]  DEFAULT ((0)),
	[Code] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[PicturePath] [nvarchar](150) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TehnicalExamVehicleParts_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_TehnicalExamVehicleParts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `TehnicalExamsTypes`

Source: `WinApp/sqlData.sql:5558-5571`

```sql
CREATE TABLE [dbo].[TehnicalExamsTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](150) NOT NULL,
	[Code] [nvarchar](10) NULL,
	[ValidNumOfDays] [int] NOT NULL,
	[PercentOfFullExam] [int] NOT NULL CONSTRAINT [DF_TehnicalExamsTypes_PercentOfFullExam]  DEFAULT ((100)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_TypesOfTehnicalExams_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_TypesOfTehnicalExams] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleRequiredFields`

Source: `WinApp/sqlData.sql:5594-5605`

```sql
CREATE TABLE [dbo].[VehicleRequiredFields](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[FieldName] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleRequiredFields_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleRequiredFields] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehiclePayTollCategory`

Source: `WinApp/sqlData.sql:5786-5794`

```sql
CREATE TABLE [dbo].[VehiclePayTollCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VehiclePaytollCategoryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_VehiclePayTollCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Streets`

Source: `WinApp/sqlData.sql:5800-5811`

```sql
CREATE TABLE [dbo].[Streets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StreetName] [nvarchar](100) NOT NULL,
	[Note] [nvarchar](150) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Streets_Active_1]  DEFAULT ((1)),
 CONSTRAINT [PK_Streets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Countries`

Source: `WinApp/sqlData.sql:6079-6091`

```sql
CREATE TABLE [dbo].[Countries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](150) NOT NULL,
	[CountryShortName] [nvarchar](150) NULL,
	[Citizenship] [nvarchar](50) NOT NULL CONSTRAINT [DF_Countries_Citizenship]  DEFAULT (N'нема'),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Countries_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Communities`

Source: `WinApp/sqlData.sql:6163-6175`

```sql
CREATE TABLE [dbo].[Communities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CommunityCode] [nvarchar](50) NOT NULL,
	[CommunityName] [nvarchar](50) NOT NULL,
	[RegistrationCode] [nvarchar](2) NOT NULL CONSTRAINT [DF_Communities_RegistrationCode]  DEFAULT (N'SK'),
	[Active] [bit] NOT NULL CONSTRAINT [DF_Communities_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Communities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `ColorsDetails`

Source: `WinApp/sqlData.sql:6295-6307`

```sql
CREATE TABLE [dbo].[ColorsDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdColor] [int] NOT NULL,
	[ColorCode] [nvarchar](50) NOT NULL,
	[ColorDescription] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_ColorsDetails_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_ColorsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `RegistrationIssuers`

Source: `WinApp/sqlData.sql:6331-6341`

```sql
CREATE TABLE [dbo].[RegistrationIssuers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IssuerName] [nvarchar](50) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_RegistrationIssuers_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_RegistrationIssuers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Colors`

Source: `WinApp/sqlData.sql:6372-6386`

```sql
CREATE TABLE [dbo].[Colors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ColorCode] [nvarchar](50) NOT NULL,
	[ColorDescription] [nvarchar](250) NOT NULL,
	[NewColorEffects] [nvarchar](1) NULL,
	[NewColorCode] [int] NULL,
	[NewColorDarkness] [nvarchar](1) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Colors_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Colors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `RequestTypes`

Source: `WinApp/sqlData.sql:6567-6590`

```sql
CREATE TABLE [dbo].[RequestTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdRequestType] [int] NULL,
	[IdDocumentPrint] [int] NOT NULL,
	[IsTehnicalExamRequired] [bit] NOT NULL,
	[IsPayRequired] [bit] NOT NULL,
	[IsNewRegistration] [bit] NOT NULL,
	[IsPreviosRegistrationReqired] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_IsPreviosRegistrationReqired]  DEFAULT ((0)),
	[IsRelationDeleted] [bit] NOT NULL,
	[IsVehicleDeleted] [bit] NOT NULL,
	[IsNewCustomer] [bit] NOT NULL,
	[IsVehicleChanged] [bit] NOT NULL,
	[IsCustomerChanged] [bit] NOT NULL,
	[IsSufficient] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_IsSufficient]  DEFAULT ((0)),
	[TypeName] [nvarchar](250) NOT NULL,
	[TypeDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_RequestTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_RequestTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Cities`

Source: `WinApp/sqlData.sql:6630-6643`

```sql
CREATE TABLE [dbo].[Cities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCommunityCode] [int] NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
	[CityZip] [int] NULL,
	[IdCountry] [int] NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Cities_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `BusinessTypes`

Source: `WinApp/sqlData.sql:8386-8397`

```sql
CREATE TABLE [dbo].[BusinessTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessTypeCode] [nvarchar](5) NOT NULL,
	[BusinessTypeDescription] [nvarchar](250) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_BusinessTypes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_BusinessTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `AttachmentTypes`

Source: `WinApp/sqlData.sql:8403-8413`

```sql
CREATE TABLE [dbo].[AttachmentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AttachmentType] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_AttachmentTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_AttachmentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DDVCatalog`

Source: `WinApp/sqlData.sql:8424-8435`

```sql
CREATE TABLE [dbo].[DDVCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DDVName] [nvarchar](50) NOT NULL,
	[DDVValue] [decimal](18, 0) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DDVCatalog_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DDVCatalog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerVehiclesRelationTypes`

Source: `WinApp/sqlData.sql:8468-8482`

```sql
CREATE TABLE [dbo].[CustomerVehiclesRelationTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RelationTypeName] [nvarchar](50) NOT NULL,
	[IsCustomerOnly] [bit] NOT NULL,
	[IsOwner] [bit] NULL,
	[IsAuthorized] [bit] NULL,
	[RelationDescription] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelationTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerVehiclesRelationTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategories`

Source: `WinApp/sqlData.sql:8493-8511`

```sql
CREATE TABLE [dbo].[VehicleCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CategoryCode] [nvarchar](5) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[PicturePath] [nvarchar](250) NULL,
	[Deskription] [nvarchar](250) NULL,
	[DetailDescription] [ntext] NULL,
	[OldCategoryName] [nvarchar](50) NULL,
	[MKSJUS] [nvarchar](50) NULL,
	[ISO] [nvarchar](50) NULL,
	[MKSJUSDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategories_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
```

##### `DocumentTypes`

Source: `WinApp/sqlData.sql:8556-8571`

```sql
CREATE TABLE [dbo].[DocumentTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeName] [nvarchar](50) NOT NULL,
	[IsBidirectional] [bit] NOT NULL,
	[IsVehiceRequired] [bit] NOT NULL,
	[IsTechnicalExamRequired] [bit] NOT NULL,
	[IsPayRequired] [bit] NOT NULL,
	[IdDocumentTypePrint] [int] NOT NULL CONSTRAINT [DF_DocumentTypes_IdDocumentTypePrint]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleBrakes`

Source: `WinApp/sqlData.sql:8582-8593`

```sql
CREATE TABLE [dbo].[VehicleBrakes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BreakesCode] [int] NULL,
	[BreakesDescription] [nvarchar](150) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesBrakes_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehiclesBrakes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypePrint`

Source: `WinApp/sqlData.sql:8618-8628`

```sql
CREATE TABLE [dbo].[DocumentTypePrint](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Opis] [nvarchar](150) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VidoviStampa_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VidoviStampa] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleBodytype`

Source: `WinApp/sqlData.sql:8639-8651`

```sql
CREATE TABLE [dbo].[VehicleBodytype](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BodytypeCode] [nvarchar](5) NOT NULL,
	[BodytypeDescriprion] [nvarchar](250) NOT NULL,
	[OldBodytypeDescription] [nvarchar](150) NULL CONSTRAINT [DF_VehicleBodytype_OldBodytypeDescription]  DEFAULT (''),
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleBodytype_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleBodyworkShapes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DriveingLicenceCtegories`

Source: `WinApp/sqlData.sql:9079-9090`

```sql
CREATE TABLE [dbo].[DriveingLicenceCtegories](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](1) NOT NULL,
	[Description] [nvarchar](250) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DriveingLicenceCtegories_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DriveingLicenceCtegories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `EngineTypeModelRelations`

Source: `WinApp/sqlData.sql:9252-9263`

```sql
CREATE TABLE [dbo].[EngineTypeModelRelations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdEngineType] [int] NOT NULL,
	[IdVehicleModel] [int] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_EngineTypeModelRelations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_EngineTypeModelRelations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEnginePowerSourceTypes`

Source: `WinApp/sqlData.sql:9540-9550`

```sql
CREATE TABLE [dbo].[VehicleEnginePowerSourceTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PowerSourceName] [nvarchar](50) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehiclesEnginePowerSourceTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehiclesEnginePowerSourceTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEngineEcoProgram`

Source: `WinApp/sqlData.sql:9564-9577`

```sql
CREATE TABLE [dbo].[VehicleEngineEcoProgram](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](10) NOT NULL,
	[EcoProgram] [nvarchar](250) NOT NULL,
	[TechincalDescription] [nvarchar](250) NOT NULL,
	[PercentForPayment] [real] NOT NULL CONSTRAINT [DF_VehicleEngineEcoProgram_PercentForPayment]  DEFAULT ((4)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleEngineMarks_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleEngineMarks] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `PriceCatalog`

Source: `WinApp/sqlData.sql:9615-9632`

```sql
CREATE TABLE [dbo].[PriceCatalog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDDVCatalog] [int] NOT NULL,
	[IsRequest] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsRequest]  DEFAULT ((0)),
	[IsTehnicalExam] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsTehnicalExam]  DEFAULT ((0)),
	[IsTrafficLicence] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsTrafficLicence]  DEFAULT ((0)),
	[IsPermisionForVehicle] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsPermisionForVehicle]  DEFAULT ((0)),
	[IsInernationalDriveingLicence] [bit] NOT NULL CONSTRAINT [DF_PriceList_IsInernationalDriveingLicence]  DEFAULT ((0)),
	[Name] [nvarchar](150) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_PriceList_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PriceList] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleCategoriesRelations`

Source: `WinApp/sqlData.sql:9642-9656`

```sql
CREATE TABLE [dbo].[VehicleCategoriesRelations](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdCategory] [int] NOT NULL,
	[IdBodytype] [int] NOT NULL,
	[IdUse] [int] NULL,
	[IdVehicleCategoryForPayments] [int] NULL,
	[DetailDescription] [nvarchar](250) NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleCategoriesRelations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_VehicleCategoriesRelations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleModel`

Source: `WinApp/sqlData.sql:10758-10772`

```sql
CREATE TABLE [dbo].[VehicleModel](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleMaker] [int] NOT NULL CONSTRAINT [DF_VehicleModel_IdVehicleMaker]  DEFAULT ((1)),
	[ModelCode] [nvarchar](250) NULL,
	[ModelName] [nvarchar](250) NOT NULL,
	[YearOfBeginingProduction] [datetime] NULL CONSTRAINT [DF_VehicleModel_YearOfBeginingProduction]  DEFAULT (getdate()),
	[YearOfEndingProduction] [datetime] NULL CONSTRAINT [DF_VehicleModel_YearOfEndingProduction]  DEFAULT (getdate()),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleModel_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleModel] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypesOptions`

Source: `WinApp/sqlData.sql:12525-12540`

```sql
CREATE TABLE [dbo].[DocumentTypesOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentTypes] [int] NOT NULL,
	[OptionName] [nvarchar](500) NOT NULL,
	[IsNewRegistration] [bit] NOT NULL,
	[IsTehnicalExamRquired] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_IsTehnicalExamRquired]  DEFAULT ((0)),
	[RelationDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_RelationDeleted]  DEFAULT ((0)),
	[VehicleDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_VehicleDeleted]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptions_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_DocumentTypesOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `DocumentTypesOptionsDetails`

Source: `WinApp/sqlData.sql:12560-12574`

```sql
CREATE TABLE [dbo].[DocumentTypesOptionsDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdDocumentTypesOptions] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[IsVehicleDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsVehicleDeleted]  DEFAULT ((0)),
	[IsNewCustomer] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsNewCustomer]  DEFAULT ((0)),
	[IsRelationDeleted] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_IsRelationDeleted]  DEFAULT ((0)),
	[Active] [bit] NOT NULL CONSTRAINT [DF_DocumentTypesOptionsDetails_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_DocumentTypesOptionsDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehicleEngineTypes`

Source: `WinApp/sqlData.sql:12667-12683`

```sql
CREATE TABLE [dbo].[VehicleEngineTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicleMaker] [int] NOT NULL,
	[EngineTypeCode] [nvarchar](250) NOT NULL,
	[TechincalDescription] [nvarchar](250) NULL,
	[IdDefaultPowerSource] [int] NULL,
	[DefaultPower] [real] NULL,
	[DefaultPowerOutPut] [real] NULL,
	[DefaultTorque] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_VehicleEngineTypes_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_VehicleEngineTyes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

