-- VTE2 — Vehicles module schema
-- Generated: 2026-04-30
-- Source: docs/superpowers/work/vehicles-business-rules.md (rules BR-VEH-001..107)
--         docs/superpowers/specs/2026-04-30-vte-domain-audit.md §4.3.2
--
-- SAFE TO RE-RUN: idempotent (IF NOT EXISTS guards).
-- Reference-data FKs (BodyTypeId, VehicleCategoryId, etc.) are NOT enforced yet —
-- the REF tables don't exist in VTE2 in this Pass. They become FK constraints
-- when the REF module is migrated.

SET NOCOUNT ON;
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULLS ON;
GO

USE [VTE2];
GO

-- 1. Vehicles (main entity)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'Vehicles')
BEGIN
    CREATE TABLE [dbo].[Vehicles] (
        [Id]                                BIGINT IDENTITY(1,1) NOT NULL,
        [StationId]                         INT                  NOT NULL,

        -- Identity (the central business value of a vehicle record)
        [ShellNumber]                       NVARCHAR(17)         NOT NULL,                       -- VIN; BR-VEH-001, BR-VEH-002, BR-VEH-003

        -- Classification (FKs deferred to REF migration)
        [BodyTypeId]                        INT                  NULL,                           -- legacy IdVehicleBodyType
        [VehicleCategoryId]                 INT                  NULL,
        [VehicleUseId]                      INT                  NULL,
        [VehicleModelId]                    INT                  NULL,
        [VehicleModelAdding]                NVARCHAR(200)        NULL,                           -- free-text extra after model
        [MadeCountryId]                     INT                  NULL,
        [VehicleCategoryForPaymentsId]      INT                  NULL,

        -- Engine
        [EngineNumber]                      NVARCHAR(50)         NULL,
        [EngineTypeId]                      INT                  NULL,
        [EnginePowerSourceId]               INT                  NULL,
        [EngineSecondPowerSourceId]         INT                  NULL,
        [EngineEcoProgramId]                INT                  NULL,
        [EnginePowerKw]                     DECIMAL(10,2)        NULL,                           -- legacy EnginePower (kW); BR-VEH-091
        [EnginePowerOutput]                 DECIMAL(10,2)        NULL,
        [EngineTorque]                      NVARCHAR(50)         NULL,
        [EngineTorqueUnderGas]              DECIMAL(10,2)        NULL,                           -- typo fix
        [EngineWorkingCapacity]             DECIMAL(10,2)        NULL,                           -- cc displacement
        [RPM]                               INT                  NULL,                           -- legacy BrojNaVrtezi
        [EngineIdentificationLocationMethod] NVARCHAR(200)       NULL,                           -- legacy IdentifikacijaNaMotorMestoMetod
        [GearBoxId]                         INT                  NULL,
        [KwToCcRatio]                       NVARCHAR(50)         NULL,                           -- legacy OdnosKwCcm

        -- Brakes / suspension
        [BrakesId]                          INT                  NULL,                           -- typo fix (was IdBreakes)
        [SupportingId]                      INT                  NULL,

        -- Geometry
        [VehicleHeight]                     DECIMAL(10,2)        NULL,                           -- typo fix (was VehicleSizeHight); BR-VEH-104
        [VehicleWidth]                      DECIMAL(10,2)        NULL,
        [VehicleLength]                     DECIMAL(10,2)        NULL,

        -- Doors / seats
        [NumberOfDoors]                     INT                  NULL,                           -- BR-VEH-092
        [NumberOfSeats]                     SMALLINT             NULL,                           -- BR-VEH-093
        [NumberOfStandingSeats]             SMALLINT             NULL,
        [NumberOfLyingSeats]                SMALLINT             NULL,                           -- typo fix (was Lieing)

        -- Wheels / axles
        [NumberOfAxes]                      INT                  NULL,                           -- BR-VEH-006
        [NumberOfPropulsionAxes]            INT                  NULL,                           -- legacy PropulsionAxis
        [NumberOfWheels]                    INT                  NULL,                           -- BR-VEH-007
        [NumberOfPropulsionWheels]          INT                  NULL,

        -- Mass (basic)
        [EmptyWeight]                       DECIMAL(10,2)        NULL,                           -- typo fix; BR-VEH-094, 105
        [MaxAllowedWeight]                  DECIMAL(10,2)        NULL,                           -- typo fix; BR-VEH-095, 105
        [MaxConstructiveTotalMass]          DECIMAL(10,2)        NULL,                           -- legacy MaxKonstVkMasa
        [MaxLegalTotalMass]                 DECIMAL(10,2)        NULL,
        [MaxLegalTotalMassGroup]            DECIMAL(10,2)        NULL,

        -- Mass (per axle, ax 1-5 + trailer)
        [AxleLoad1]                         INT                  NULL,                           -- legacy MasaPoOska1
        [AxleLoad2]                         INT                  NULL,
        [AxleLoad3]                         INT                  NULL,
        [AxleLoad4]                         INT                  NULL,
        [AxleLoad5]                         INT                  NULL,
        [TrailerAxleLoad]                   INT                  NULL,                           -- legacy MasaPoOskaPriklucna
        [AxleBaseLoad1]                     INT                  NULL,                           -- legacy OsnoOptovaruvanje1
        [AxleBaseLoad2]                     INT                  NULL,
        [AxleBaseLoad3]                     INT                  NULL,
        [AxleBaseLoad4]                     INT                  NULL,
        [AxleBaseLoad5]                     INT                  NULL,
        [TrailerAxleBaseLoad]               INT                  NULL,

        -- Trailer / coupling specs
        [TrailerWeightBraked]               NVARCHAR(20)         NULL,                           -- typo fix (was TrailerWaightWithBreak)
        [TrailerWeightUnbraked]             NVARCHAR(20)         NULL,                           -- typo fix
        [MaxConstructiveBrakedTrailerMass]  INT                  NULL,
        [MaxConstructiveUnbrakedTrailerMass] INT                 NULL,
        [MaxConstructiveCouplingLoad]       INT                  NULL,
        [MaxConstructiveCombinationMass]    INT                  NULL,
        [MaxConstructiveSemiTrailerMass]    INT                  NULL,
        [MaxConstructiveTrailerMass]        INT                  NULL,
        [MaxConstructiveTrailerMassWithCentralAxle] INT          NULL,
        [MaxConstructiveAttachableTrailerMass] INT               NULL,
        [MaxHorizontalVerticalCouplingLoad] INT                  NULL,
        [MinMass]                           INT                  NULL,
        [MechanicalCouplingType]            NVARCHAR(100)        NULL,
        [MechanicalCouplingMark]            NVARCHAR(100)        NULL,
        [MechanicalCouplingApprovalNumber]  NVARCHAR(100)        NULL,
        [CouplingDeviceApprovalMark]        NVARCHAR(100)        NULL,

        -- Type approval / homologation
        [HomologationCertificateNumber]     NVARCHAR(100)        NULL,                           -- typo fix; BR-VEH-099
        [ApprovalMark]                      NVARCHAR(100)        NULL,
        [EUCertificateNumber]               NVARCHAR(100)        NULL,
        [VariantImplementation]             NVARCHAR(100)        NULL,
        [Type]                              NVARCHAR(100)        NULL,                           -- legacy "Tip"

        -- Performance
        [MaxSpeed]                          DECIMAL(6,2)         NULL,
        [TempOfEngineOil]                   DECIMAL(6,2)         NULL,

        -- Emissions / noise
        [NoiseStatic]                       DECIMAL(6,2)         NULL,
        [NoiseMovement]                     DECIMAL(6,2)         NULL,                           -- typo fix
        [NoiseTechnicalSpec]                NVARCHAR(50)         NULL,
        [Co]                                DECIMAL(6,3)         NULL,
        [Hc]                                DECIMAL(6,3)         NULL,
        [NOx]                               DECIMAL(6,3)         NULL,
        [HCNOx]                             DECIMAL(6,3)         NULL,
        [Co2]                               DECIMAL(6,3)         NULL,
        [Blackening]                        NVARCHAR(20)         NULL,                           -- BR-VEH-100
        [Pinpoints]                         NVARCHAR(20)         NULL,
        [FuelConsumption]                   NVARCHAR(20)         NULL,                           -- BR-VEH-102
        [CapacityFuelTank]                  DECIMAL(8,2)         NULL,

        -- Color
        [ColorCode]                         NVARCHAR(20)         NULL,
        [PrimaryColorId]                    INT                  NULL,
        [SecondaryColorId]                  INT                  NULL,

        -- Build dates
        [MakeDate]                          DATE                 NULL,                           -- BR-VEH-096

        -- Registration history (denormalized; full history in VehicleRegistrations)
        [FirstRegistrationNumber]           NVARCHAR(50)         NOT NULL,                       -- BR-VEH-004
        [LastRegistrationNumber]            NVARCHAR(50)         NOT NULL,                       -- BR-VEH-005 (typo fix)
        [FirstRegistrationMakeDate]         DATE                 NULL,
        [FirstRegistrationValidTill]        DATE                 NULL,
        [LastRegistrationMakeDate]          DATE                 NULL,
        [LastRegistrationValidTill]         DATE                 NULL,
        [FirstRegistrationIssuerId]         INT                  NULL,
        [LastRegistrationIssuerId]          INT                  NULL,

        -- Equipment flags
        [Suffocation]                       BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Suffocation]                DEFAULT (0),
        [Hook]                              BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Hook]                       DEFAULT (0),
        [Winch]                             BIT                  NOT NULL CONSTRAINT [DF_Vehicles_Winch]                      DEFAULT (0), -- legacy "Vitlo"
        [VerticalBurdenOnTheSeat]           BIT                  NOT NULL CONSTRAINT [DF_Vehicles_VerticalBurdenOnTheSeat]    DEFAULT (0),
        [VerticalBurdenOnTheSeatNote]       NVARCHAR(200)        NULL,
        [TNG]                               BIT                  NOT NULL CONSTRAINT [DF_Vehicles_TNG]                        DEFAULT (0), -- compressed natural gas
        [ProtectiveCabin]                   NVARCHAR(50)         NULL,
        [ProtectiveFrame]                   NVARCHAR(50)         NULL,
        [IsSocialNotPrivate]                BIT                  NOT NULL CONSTRAINT [DF_Vehicles_IsSocialNotPrivate]         DEFAULT (0), -- Q-012
        [ForPrivateTransportNotPublic]      BIT                  NOT NULL CONSTRAINT [DF_Vehicles_ForPrivateTransportNotPublic] DEFAULT (0),

        -- Free-text
        [Note]                              NVARCHAR(500)        NULL,                           -- BR-VEH-103

        -- Soft-delete
        [IsActive]                          BIT                  NOT NULL CONSTRAINT [DF_Vehicles_IsActive]                   DEFAULT (1),

        -- Audit
        [CreatedUtc]                        DATETIME2            NOT NULL CONSTRAINT [DF_Vehicles_CreatedUtc]                 DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]                   DATETIME2            NOT NULL CONSTRAINT [DF_Vehicles_LastModifiedUtc]            DEFAULT (SYSUTCDATETIME()),
        [CreatedByUserId]                   NVARCHAR(450)        NULL,
        [LastModifiedByUserId]              NVARCHAR(450)        NULL,

        -- Concurrency
        [RowVersion]                        ROWVERSION           NOT NULL,

        CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_Vehicles_Stations_StationId]
            FOREIGN KEY ([StationId]) REFERENCES [dbo].[Stations]([Id]),
        CONSTRAINT [FK_Vehicles_AspNetUsers_CreatedByUserId]
            FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [FK_Vehicles_AspNetUsers_LastModifiedByUserId]
            FOREIGN KEY ([LastModifiedByUserId]) REFERENCES [dbo].[AspNetUsers]([Id]),
        CONSTRAINT [CK_Vehicles_AxesGEPropulsion]   CHECK ([NumberOfAxes]   IS NULL OR [NumberOfPropulsionAxes]   IS NULL OR [NumberOfAxes]   >= [NumberOfPropulsionAxes]),    -- BR-VEH-006
        CONSTRAINT [CK_Vehicles_WheelsGEPropulsion] CHECK ([NumberOfWheels] IS NULL OR [NumberOfPropulsionWheels] IS NULL OR [NumberOfWheels] >= [NumberOfPropulsionWheels])  -- BR-VEH-007
    );

    -- BR-VEH-003: VIN unique per tenant
    CREATE UNIQUE INDEX [UX_Vehicles_StationId_ShellNumber]
        ON [dbo].[Vehicles]([StationId], [ShellNumber]);

    -- Search by registration number
    CREATE INDEX [IX_Vehicles_StationId_LastRegistrationNumber]
        ON [dbo].[Vehicles]([StationId], [LastRegistrationNumber]);

    -- Tenant filter
    CREATE INDEX [IX_Vehicles_StationId]
        ON [dbo].[Vehicles]([StationId]);

    PRINT 'Table [dbo].[Vehicles] created.';
END
ELSE
    PRINT 'Table [dbo].[Vehicles] already exists.';
GO

-- 2. VehicleRegistrations (registration history)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleRegistrations')
BEGIN
    CREATE TABLE [dbo].[VehicleRegistrations] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [RegistrationNumber] NVARCHAR(50)        NOT NULL,
        [MakeDate]          DATE                 NULL,
        [ValidTill]         DATE                 NULL,
        [IssuerId]          INT                  NULL,                                            -- FK → RegistrationIssuers (deferred)
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleRegistrations_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleRegistrations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleRegistrations] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleRegistrations_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_VehicleRegistrations_VehicleId] ON [dbo].[VehicleRegistrations]([VehicleId]);
    PRINT 'Table [dbo].[VehicleRegistrations] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleRegistrations] already exists.';
GO

-- 3. VehicleAxles (legacy: Vehicle.Axis — per-vehicle axle inventory)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleAxles')
BEGIN
    CREATE TABLE [dbo].[VehicleAxles] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [AxleNumber]        INT                  NOT NULL,
        [IsPropulsion]      BIT                  NOT NULL CONSTRAINT [DF_VehicleAxles_IsPropulsion]      DEFAULT (0),
        [IsSteering]        BIT                  NOT NULL CONSTRAINT [DF_VehicleAxles_IsSteering]        DEFAULT (0),
        [Note]              NVARCHAR(200)        NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxles_CreatedUtc]        DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxles_LastModifiedUtc]   DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleAxles] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleAxles_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_VehicleAxles_VehicleId_AxleNumber] UNIQUE ([VehicleId], [AxleNumber])
    );
    PRINT 'Table [dbo].[VehicleAxles] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleAxles] already exists.';
GO

-- 4. VehicleAxleDistances (legacy: Vehicle.BetweenAxesDestinations — distances between axles)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleAxleDistances')
BEGIN
    CREATE TABLE [dbo].[VehicleAxleDistances] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [FromAxleNumber]    INT                  NOT NULL,
        [ToAxleNumber]      INT                  NOT NULL,
        [Distance]          DECIMAL(8,2)         NULL,                           -- in mm or cm — confirm with regulatory unit
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxleDistances_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleAxleDistances_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleAxleDistances] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleAxleDistances_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE,
        CONSTRAINT [CK_VehicleAxleDistances_FromLTTo] CHECK ([FromAxleNumber] < [ToAxleNumber])
    );
    PRINT 'Table [dbo].[VehicleAxleDistances] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleAxleDistances] already exists.';
GO

-- 5. VehicleTyres (legacy: Vehicle.Tyres — per-vehicle tyre inventory; not the catalog REF table)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'VehicleTyres')
BEGIN
    CREATE TABLE [dbo].[VehicleTyres] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [TireTypeId]        INT                  NULL,                                            -- FK → VehicleTireTypes (deferred REF)
        [PositionNote]      NVARCHAR(50)         NULL,                                            -- e.g. "front-left", "rear-right"
        [Dimensions]        NVARCHAR(50)         NULL,                                            -- e.g. "205/55 R16"
        [PressureFront]     DECIMAL(5,2)         NULL,
        [PressureRear]      DECIMAL(5,2)         NULL,
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_VehicleTyres_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_VehicleTyres_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_VehicleTyres] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_VehicleTyres_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE CASCADE
    );
    PRINT 'Table [dbo].[VehicleTyres] created.';
END
ELSE
    PRINT 'Table [dbo].[VehicleTyres] already exists.';
GO

-- 6. CustomerVehicleRelations (legacy: CustomerVehiclesRelations — many-to-many between Customers and Vehicles with relation type)
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = N'CustomerVehicleRelations')
BEGIN
    CREATE TABLE [dbo].[CustomerVehicleRelations] (
        [Id]                BIGINT IDENTITY(1,1) NOT NULL,
        [CustomerId]        BIGINT               NOT NULL,
        [VehicleId]         BIGINT               NOT NULL,
        [RelationTypeId]    INT                  NULL,                                            -- FK → CustomerVehicleRelationTypes (deferred REF)
        [ValidFrom]         DATE                 NULL,
        [ValidTo]           DATE                 NULL,                                            -- NULL = current
        [Note]              NVARCHAR(200)        NULL,
        [IsActive]          BIT                  NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_IsActive]        DEFAULT (1),
        [CreatedUtc]        DATETIME2            NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_CreatedUtc]      DEFAULT (SYSUTCDATETIME()),
        [LastModifiedUtc]   DATETIME2            NOT NULL CONSTRAINT [DF_CustomerVehicleRelations_LastModifiedUtc] DEFAULT (SYSUTCDATETIME()),
        [RowVersion]        ROWVERSION           NOT NULL,
        CONSTRAINT [PK_CustomerVehicleRelations] PRIMARY KEY CLUSTERED ([Id]),
        CONSTRAINT [FK_CustomerVehicleRelations_Customers_CustomerId]
            FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customers]([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerVehicleRelations_Vehicles_VehicleId]
            FOREIGN KEY ([VehicleId]) REFERENCES [dbo].[Vehicles]([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_CustomerVehicleRelations_CustomerId] ON [dbo].[CustomerVehicleRelations]([CustomerId]);
    CREATE INDEX [IX_CustomerVehicleRelations_VehicleId]  ON [dbo].[CustomerVehicleRelations]([VehicleId]);
    PRINT 'Table [dbo].[CustomerVehicleRelations] created.';
END
ELSE
    PRINT 'Table [dbo].[CustomerVehicleRelations] already exists.';
GO

PRINT '';
PRINT '=== Vehicles module schema applied ===';
SELECT name AS [Tables in VTE2 (Vehicles module)]
FROM sys.tables
WHERE name IN (N'Vehicles', N'VehicleRegistrations', N'VehicleAxles', N'VehicleAxleDistances', N'VehicleTyres', N'CustomerVehicleRelations')
ORDER BY name;
GO
