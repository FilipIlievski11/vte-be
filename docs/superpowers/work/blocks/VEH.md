##### `Vehicles`

Source: `WinApp/sqlData.sql:5401-5471`

```sql
CREATE TABLE [dbo].[Vehicles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicleBodyType] [int] NULL,
	[IdVehicleCategories] [int] NOT NULL,
	[IdEngineType] [int] NULL,
	[IdEnginePowerSource] [int] NULL,
	[IdEngineSecondPowerSource] [int] NULL,
	[IdGearBox] [int] NULL,
	[IdBreakes] [int] NULL,
	[IdSupporting] [int] NULL,
	[IdVehicleModel] [int] NULL,
	[IdPrimaryColor] [int] NULL CONSTRAINT [DF_Vehicles_IdPrimaryColor]  DEFAULT ((0)),
	[IdSecondaryColor] [int] NULL,
	[IdVehicleCategoryForPayments] [int] NULL,
	[IdEngineEcoProgram] [int] NULL,
	[IdMadeCountry] [int] NULL,
	[ColorCode] [nvarchar](50) NULL,
	[EngineNumber] [nvarchar](20) NULL,
	[EnginePower] [real] NULL,
	[EngineTorque] [nvarchar](250) NULL,
	[EngineTorqueUnderGass] [real] NULL,
	[EngineWorkingCapacity] [real] NULL,
	[EnginePowerOutPut] [real] NULL,
	[ShellNumber] [nvarchar](17) NULL,
	[MakeDate] [datetime] NOT NULL,
	[NumberOfDoors] [int] NULL,
	[NumberOfSeats] [smallint] NULL,
	[NumberOfStandingSeats] [smallint] NULL,
	[NumberOfLieingSeats] [smallint] NULL,
	[EmptyWaight] [real] NULL,
	[MaximunAllowedWaight] [real] NULL,
	[TrailerWaightWithBreak] [nvarchar](20) NULL,
	[TrailerWaightWithoutBreak] [nvarchar](20) NULL,
	[NumberOfAxis] [int] NULL,
	[PropulsionAxis] [int] NULL,
	[NumberOfWheels] [int] NULL,
	[NumberOfPropulsionWheels] [int] NULL,
	[VehicleSizeHight] [real] NULL,
	[VehicleSizeWidth] [real] NULL,
	[VehicleSizeLength] [real] NULL,
	[Suffocation] [bit] NULL CONSTRAINT [DF_Vehicles_Suffocation]  DEFAULT ((0)),
	[Hook] [bit] NULL CONSTRAINT [DF_Vehicles_Hook]  DEFAULT ((0)),
	[Vitlo] [bit] NULL CONSTRAINT [DF_Vehicles_Vitlo]  DEFAULT ((0)),
	[VerticalBurdenOnTheSeat] [bit] NULL,
	[VerticalBurdenOnTheSeatNote] [nvarchar](250) NULL,
	[HologationSertificateNumber] [nvarchar](100) NULL,
	[NoiseStatic] [real] NULL,
	[NoiseMovment] [real] NULL,
	[CO] [real] NULL,
	[HC] [real] NULL,
	[NOx] [real] NULL,
	[HCNOx] [real] NULL,
	[Blackening] [nvarchar](20) NULL,
	[Pinpoints] [nvarchar](20) NULL,
	[CO2] [real] NULL,
	[FuelConsumption] [nvarchar](20) NULL,
	[CapacityFuelTank] [real] NULL,
	[Note] [nvarchar](500) NULL,
	[IsSocialNotPrivate] [bit] NULL CONSTRAINT [DF_Vehicles_IsSocialNotPrivate]  DEFAULT ((0)),
	[ForPrivateTransportNotPublic] [bit] NULL CONSTRAINT [DF_Vehicles_ForPrivateTransportNotPublic]  DEFAULT ((1)),
	[TNG] [bit] NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicles_Active]  DEFAULT ((1)),
	[IdVehicleUse] [int] NOT NULL CONSTRAINT [DF_Vehicles_IdVehicleUse]  DEFAULT ((0)),
	[VehicleModelAdding] [nvarchar](200) NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Registrations`

Source: `WinApp/sqlData.sql:8446-8462`

```sql
CREATE TABLE [dbo].[Vehicle.Registrations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[IdRegistrationIssuer] [int] NOT NULL CONSTRAINT [DF_Vehicle.Registrations_IdRegistrationIssuer]  DEFAULT ((1)),
	[RegistrationNumber] [nvarchar](10) NOT NULL,
	[DateOfRegistration] [datetime] NOT NULL,
	[DateRegistrationValidTill] [datetime] NOT NULL,
	[PlaceOfRegistration] [nvarchar](150) NOT NULL,
	[IsFirstRegistration] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Registrations_IsFirstRegistration]  DEFAULT ((0)),
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Registration_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.Registration] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `CustomerVehiclesRelations`

Source: `WinApp/sqlData.sql:10710-10726`

```sql
CREATE TABLE [dbo].[CustomerVehiclesRelations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdRelationType] [int] NOT NULL,
	[IdCustomer] [bigint] NOT NULL,
	[IdVehicle] [bigint] NULL,
	[StartDate] [datetime] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelations_StartDate]  DEFAULT (getdate()),
	[EndDate] [datetime] NULL,
	[BeginNote] [nvarchar](250) NULL,
	[TerminationNote] [nvarchar](250) NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_CustomerVehiclesRelations_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_CustomerVehicles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Axis`

Source: `WinApp/sqlData.sql:11579-11592`

```sql
CREATE TABLE [dbo].[Vehicle.Axis](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[AxisNumber] [int] NOT NULL,
	[CarryingCapacity] [real] NOT NULL,
	[AxisLength] [real] NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.AxisCarryingCapacity_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.AxisCarryingCapacity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.BetweenAxesDestinations`

Source: `WinApp/sqlData.sql:11598-11610`

```sql
CREATE TABLE [dbo].[Vehicle.BetweenAxesDestinations](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[FromTo] [nvarchar](5) NOT NULL,
	[Destination] [decimal](18, 0) NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.BetweenAxesDestinations_Active]  DEFAULT ((1)),
	[LastChanged] [timestamp] NOT NULL,
 CONSTRAINT [PK_Vehicle.BetweenAxesDestinations] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `Vehicle.Tyres`

Source: `WinApp/sqlData.sql:11767-11778`

```sql
CREATE TABLE [dbo].[Vehicle.Tyres](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehicle] [bigint] NOT NULL,
	[IdTireType] [int] NOT NULL,
	[LastChanged] [timestamp] NOT NULL,
	[Active] [bit] NOT NULL CONSTRAINT [DF_Vehicle.Tyres_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Vehicle.Tyres] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

##### `VehiclePayToll`

Source: `WinApp/sqlData.sql:11820-11836`

```sql
CREATE TABLE [dbo].[VehiclePayToll](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdVehiclePaytollCategory] [int] NOT NULL,
	[LitreFrom] [decimal](18, 0) NOT NULL,
	[LitreTo] [decimal](18, 0) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[NumberOfSeatsFrom] [int] NULL,
	[NumberOfSeatsTo] [int] NULL,
	[TypeOfMeasure] [nvarchar](5) NULL,
	[MeasureFrom] [decimal](18, 0) NULL,
	[MeasureTo] [decimal](18, 0) NULL,
 CONSTRAINT [PK_VehiclePayToll] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
```

