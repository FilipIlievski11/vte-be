IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [CompanyId] tinyint NULL,
        [FullName] nvarchar(200) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260525213117_InitialIdentity'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260525213117_InitialIdentity', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetRoles] (
        [Id] nvarchar(450) NOT NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetUsers] (
        [Id] nvarchar(450) NOT NULL,
        [CompanyId] tinyint NULL,
        [FullName] nvarchar(200) NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [ClientVehicleRelationType] (
        [Id] tinyint NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [IsOwner] bit NOT NULL,
        [IsAuthorized] bit NOT NULL,
        [IsCustomerOnly] bit NOT NULL,
        [Description] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_ClientVehicleRelationType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleBodyType] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(20) NULL,
        [Name] nvarchar(500) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleBodyType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleCategory] (
        [Id] smallint NOT NULL IDENTITY,
        [Code] nvarchar(20) NULL,
        [Name] nvarchar(100) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleCategory] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleColor] (
        [Id] smallint NOT NULL IDENTITY,
        [Code] nvarchar(100) NULL,
        [Name] nvarchar(500) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleColor] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleEcoProgram] (
        [Id] tinyint NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleEcoProgram] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleEngineType] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(200) NULL,
        [Name] nvarchar(500) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleEngineType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleFuel] (
        [Id] tinyint NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleFuel] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleMaker] (
        [Id] int NOT NULL IDENTITY,
        [CountryId] smallint NULL,
        [Name] nvarchar(500) NOT NULL,
        [Trademark] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleMaker] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehiclePaymentCategory] (
        [Id] tinyint NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehiclePaymentCategory] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleModel] (
        [Id] int NOT NULL IDENTITY,
        [MakerId] int NOT NULL,
        [Code] nvarchar(500) NULL,
        [Name] nvarchar(500) NOT NULL,
        [ProductionStart] datetime2 NULL,
        [ProductionEnd] datetime2 NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleModel] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_VehicleModel_VehicleMaker_MakerId] FOREIGN KEY ([MakerId]) REFERENCES [VehicleMaker] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [Vehicle] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [Vin] nvarchar(40) NOT NULL,
        [EngineNumber] nvarchar(40) NULL,
        [Plate] nvarchar(20) NULL,
        [CategoryId] smallint NULL,
        [BodyTypeId] int NULL,
        [ModelId] int NULL,
        [PrimaryColorId] smallint NULL,
        [SecondaryColorId] smallint NULL,
        [MadeCountryId] smallint NULL,
        [FuelId] tinyint NULL,
        [SecondFuelId] tinyint NULL,
        [EngineTypeId] int NULL,
        [EcoProgramId] tinyint NULL,
        [PaymentCategoryId] tinyint NULL,
        [EnginePowerKw] real NULL,
        [EngineWorkingCapacityCc] real NULL,
        [MaxRpm] int NULL,
        [MaxSpeedKmh] real NULL,
        [HasLpg] bit NULL,
        [LengthMm] real NULL,
        [WidthMm] real NULL,
        [HeightMm] real NULL,
        [EmptyWeightKg] real NULL,
        [MaxAllowedWeightKg] real NULL,
        [MaxLegalTotalMassKg] real NULL,
        [MaxConstructiveTotalMassKg] real NULL,
        [TrailerMassWithBrakesKg] nvarchar(40) NULL,
        [TrailerMassWithoutBrakesKg] nvarchar(40) NULL,
        [AxleCount] int NULL,
        [WheelCount] int NULL,
        [AxleLoad1Kg] int NULL,
        [AxleLoad2Kg] int NULL,
        [Seats] smallint NULL,
        [StandingSeats] smallint NULL,
        [Co2GKm] real NULL,
        [NoiseStaticDb] real NULL,
        [NoiseMovingDb] real NULL,
        [TypeText] nvarchar(300) NULL,
        [ModelVariant] nvarchar(400) NULL,
        [ApprovalMark] nvarchar(100) NULL,
        [Note] nvarchar(1000) NULL,
        [Active] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Vehicle] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Vehicle_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_Country_MadeCountryId] FOREIGN KEY ([MadeCountryId]) REFERENCES [Country] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleBodyType_BodyTypeId] FOREIGN KEY ([BodyTypeId]) REFERENCES [VehicleBodyType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleCategory_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [VehicleCategory] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleColor_PrimaryColorId] FOREIGN KEY ([PrimaryColorId]) REFERENCES [VehicleColor] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleColor_SecondaryColorId] FOREIGN KEY ([SecondaryColorId]) REFERENCES [VehicleColor] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleEcoProgram_EcoProgramId] FOREIGN KEY ([EcoProgramId]) REFERENCES [VehicleEcoProgram] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleEngineType_EngineTypeId] FOREIGN KEY ([EngineTypeId]) REFERENCES [VehicleEngineType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleFuel_FuelId] FOREIGN KEY ([FuelId]) REFERENCES [VehicleFuel] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleFuel_SecondFuelId] FOREIGN KEY ([SecondFuelId]) REFERENCES [VehicleFuel] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehicleModel_ModelId] FOREIGN KEY ([ModelId]) REFERENCES [VehicleModel] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Vehicle_VehiclePaymentCategory_PaymentCategoryId] FOREIGN KEY ([PaymentCategoryId]) REFERENCES [VehiclePaymentCategory] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [ClientVehicleRelation] (
        [Id] bigint NOT NULL IDENTITY,
        [ClientId] bigint NOT NULL,
        [VehicleId] bigint NULL,
        [RelationTypeId] tinyint NOT NULL,
        [StartDate] datetime2 NOT NULL,
        [EndDate] datetime2 NULL,
        [StartNote] nvarchar(500) NULL,
        [EndNote] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_ClientVehicleRelation] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ClientVehicleRelation_ClientVehicleRelationType_RelationTypeId] FOREIGN KEY ([RelationTypeId]) REFERENCES [ClientVehicleRelationType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_ClientVehicleRelation_Client_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Client] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ClientVehicleRelation_Vehicle_VehicleId] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicle] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE TABLE [VehicleRegistration] (
        [Id] bigint NOT NULL IDENTITY,
        [VehicleId] bigint NOT NULL,
        [IssuerId] tinyint NOT NULL,
        [PlateNumber] nvarchar(20) NOT NULL,
        [RegisteredDate] datetime2 NOT NULL,
        [ValidUntil] datetime2 NOT NULL,
        [IsFirstRegistration] bit NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VehicleRegistration] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_VehicleRegistration_DocumentIssuer_IssuerId] FOREIGN KEY ([IssuerId]) REFERENCES [DocumentIssuer] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_VehicleRegistration_Vehicle_VehicleId] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicle] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_ClientVehicleRelation_ClientId] ON [ClientVehicleRelation] ([ClientId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_ClientVehicleRelation_RelationTypeId] ON [ClientVehicleRelation] ([RelationTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_ClientVehicleRelation_VehicleId] ON [ClientVehicleRelation] ([VehicleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_BodyTypeId] ON [Vehicle] ([BodyTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_CategoryId] ON [Vehicle] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_CompanyId] ON [Vehicle] ([CompanyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_EcoProgramId] ON [Vehicle] ([EcoProgramId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_EngineTypeId] ON [Vehicle] ([EngineTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_FuelId] ON [Vehicle] ([FuelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_MadeCountryId] ON [Vehicle] ([MadeCountryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_ModelId] ON [Vehicle] ([ModelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_PaymentCategoryId] ON [Vehicle] ([PaymentCategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_Plate] ON [Vehicle] ([Plate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_PrimaryColorId] ON [Vehicle] ([PrimaryColorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_SecondaryColorId] ON [Vehicle] ([SecondaryColorId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_SecondFuelId] ON [Vehicle] ([SecondFuelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_Vehicle_Vin] ON [Vehicle] ([Vin]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_VehicleMaker_CountryId] ON [VehicleMaker] ([CountryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_VehicleModel_MakerId] ON [VehicleModel] ([MakerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_VehicleRegistration_IssuerId] ON [VehicleRegistration] ([IssuerId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_VehicleRegistration_PlateNumber] ON [VehicleRegistration] ([PlateNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    CREATE INDEX [IX_VehicleRegistration_VehicleId] ON [VehicleRegistration] ([VehicleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260526002937_AddVehicles'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260526002937_AddVehicles', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestAttachmentType] (
        [Id] tinyint NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestAttachmentType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestDocumentPrint] (
        [Id] tinyint NOT NULL IDENTITY,
        [Code] nvarchar(20) NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [TemplatePath] nvarchar(400) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestDocumentPrint] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestOwnershipProofType] (
        [Id] tinyint NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestOwnershipProofType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestPaymentProofType] (
        [Id] tinyint NOT NULL IDENTITY,
        [Name] nvarchar(200) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestPaymentProofType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestType] (
        [Id] tinyint NOT NULL IDENTITY,
        [ParentRequestTypeId] tinyint NULL,
        [DocumentPrintId] tinyint NOT NULL,
        [Name] nvarchar(250) NOT NULL,
        [Description] nvarchar(500) NULL,
        [TechnicalExamRequirement] tinyint NOT NULL,
        [PaymentRequired] bit NOT NULL,
        [IssuesNewRegistration] bit NOT NULL,
        [DeactivatesRelation] bit NOT NULL,
        [DeactivatesVehicle] bit NOT NULL,
        [TransfersOwnership] bit NOT NULL,
        [MutatesVehicleData] bit NOT NULL,
        [MutatesClientData] bit NOT NULL,
        [IsSufficient] bit NOT NULL,
        [PreviousRegistrationRequired] bit NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestType] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RequestType_RequestDocumentPrint_DocumentPrintId] FOREIGN KEY ([DocumentPrintId]) REFERENCES [RequestDocumentPrint] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RequestType_RequestType_ParentRequestTypeId] FOREIGN KEY ([ParentRequestTypeId]) REFERENCES [RequestType] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [Request] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [RequestTypeId] tinyint NOT NULL,
        [ClientVehicleRelationId] bigint NOT NULL,
        [NewClientVehicleRelationId] bigint NULL,
        [TechnicalExamReportId] bigint NULL,
        [PreviousRegistrationId] bigint NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ModifiedAt] datetime2 NULL,
        [EndedAt] datetime2 NULL,
        [CreatedByUserId] nvarchar(450) NOT NULL,
        [ModifiedByUserId] nvarchar(450) NULL,
        [EndedByUserId] nvarchar(450) NULL,
        [VehicleDataChanged] bit NOT NULL,
        [ClientDataChanged] bit NOT NULL,
        [Note] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_Request] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Request_ClientVehicleRelation_ClientVehicleRelationId] FOREIGN KEY ([ClientVehicleRelationId]) REFERENCES [ClientVehicleRelation] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Request_ClientVehicleRelation_NewClientVehicleRelationId] FOREIGN KEY ([NewClientVehicleRelationId]) REFERENCES [ClientVehicleRelation] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Request_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Request_RequestType_RequestTypeId] FOREIGN KEY ([RequestTypeId]) REFERENCES [RequestType] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestAttachment] (
        [Id] bigint NOT NULL IDENTITY,
        [RequestId] bigint NOT NULL,
        [AttachmentTypeId] tinyint NOT NULL,
        [FileName] nvarchar(260) NOT NULL,
        [ContentType] nvarchar(150) NOT NULL,
        [SizeBytes] bigint NOT NULL,
        [StoragePath] nvarchar(500) NOT NULL,
        [UploadedAt] datetime2 NOT NULL,
        [UploadedByUserId] nvarchar(450) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestAttachment] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RequestAttachment_RequestAttachmentType_AttachmentTypeId] FOREIGN KEY ([AttachmentTypeId]) REFERENCES [RequestAttachmentType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RequestAttachment_Request_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Request] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestOwnershipProof] (
        [Id] bigint NOT NULL IDENTITY,
        [RequestId] bigint NOT NULL,
        [OwnershipProofTypeId] tinyint NOT NULL,
        [Detail] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestOwnershipProof] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RequestOwnershipProof_RequestOwnershipProofType_OwnershipProofTypeId] FOREIGN KEY ([OwnershipProofTypeId]) REFERENCES [RequestOwnershipProofType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RequestOwnershipProof_Request_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Request] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE TABLE [RequestPaymentProof] (
        [Id] bigint NOT NULL IDENTITY,
        [RequestId] bigint NOT NULL,
        [PaymentProofTypeId] tinyint NOT NULL,
        [Detail] nvarchar(500) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_RequestPaymentProof] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RequestPaymentProof_RequestPaymentProofType_PaymentProofTypeId] FOREIGN KEY ([PaymentProofTypeId]) REFERENCES [RequestPaymentProofType] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_RequestPaymentProof_Request_RequestId] FOREIGN KEY ([RequestId]) REFERENCES [Request] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_Request_ClientVehicleRelationId] ON [Request] ([ClientVehicleRelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_Request_NewClientVehicleRelationId] ON [Request] ([NewClientVehicleRelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_Request_Open] ON [Request] ([CompanyId], [CreatedAt]) WHERE [Active] = 1 AND [EndedAt] IS NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_Request_PreviousRegistrationId] ON [Request] ([PreviousRegistrationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_Request_RequestTypeId] ON [Request] ([RequestTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestAttachment_AttachmentTypeId] ON [RequestAttachment] ([AttachmentTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestAttachment_RequestId] ON [RequestAttachment] ([RequestId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestOwnershipProof_OwnershipProofTypeId] ON [RequestOwnershipProof] ([OwnershipProofTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestOwnershipProof_RequestId] ON [RequestOwnershipProof] ([RequestId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestPaymentProof_PaymentProofTypeId] ON [RequestPaymentProof] ([PaymentProofTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestPaymentProof_RequestId] ON [RequestPaymentProof] ([RequestId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestType_DocumentPrintId] ON [RequestType] ([DocumentPrintId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestType_Name] ON [RequestType] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    CREATE INDEX [IX_RequestType_ParentRequestTypeId] ON [RequestType] ([ParentRequestTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527000008_AddRequests'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260527000008_AddRequests', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527104104_AddVehicleManufactureDate'
)
BEGIN
    ALTER TABLE [Vehicle] ADD [ManufactureDate] datetime2 NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527104104_AddVehicleManufactureDate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260527104104_AddVehicleManufactureDate', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527110831_AddRequestLegacyReferenceNumber'
)
BEGIN
    ALTER TABLE [Request] ADD [LegacyReferenceNumber] nvarchar(50) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260527110831_AddRequestLegacyReferenceNumber'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260527110831_AddRequestLegacyReferenceNumber', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528085806_AddVehicleMaxLegalGroupMass'
)
BEGIN
    ALTER TABLE [Vehicle] ADD [MaxLegalGroupMassKg] real NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528085806_AddVehicleMaxLegalGroupMass'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260528085806_AddVehicleMaxLegalGroupMass', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528095117_AddVehicleTowingFields'
)
BEGIN
    ALTER TABLE [Vehicle] ADD [MaxHitchLoadKg] real NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528095117_AddVehicleTowingFields'
)
BEGIN
    ALTER TABLE [Vehicle] ADD [MaxTrailerBrakedKg] real NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528095117_AddVehicleTowingFields'
)
BEGIN
    ALTER TABLE [Vehicle] ADD [MaxTrailerUnbrakedKg] real NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528095117_AddVehicleTowingFields'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260528095117_AddVehicleTowingFields', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528103317_AddPaymentCategoryZelenMap'
)
BEGIN
    ALTER TABLE [VehiclePaymentCategory] ADD [ZelenMap] tinyint NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528103317_AddPaymentCategoryZelenMap'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260528103317_AddPaymentCategoryZelenMap', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamDetailStatus] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_TechnicalExamDetailStatus] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamOrganization] (
        [Id] int NOT NULL IDENTITY,
        [CompanyId] tinyint NULL,
        [Code] nvarchar(40) NULL,
        [Name] nvarchar(300) NULL,
        [CityId] int NULL,
        [CommunityId] int NULL,
        [Address] nvarchar(200) NULL,
        [Phone] nvarchar(200) NULL,
        [Fax] nvarchar(200) NULL,
        [BankAccount] nvarchar(510) NULL,
        [Depositor] nvarchar(200) NULL,
        [TaxNumber] nvarchar(100) NULL,
        [ResponsibleOfficer] nvarchar(200) NULL,
        [Secretary] nvarchar(200) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_TechnicalExamOrganization] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamType] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(20) NULL,
        [Description] nvarchar(300) NOT NULL,
        [ValidDays] int NOT NULL,
        [PercentOfFullExam] int NOT NULL,
        [IsInRegister] bit NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_TechnicalExamType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamVehiclePart] (
        [Id] int NOT NULL IDENTITY,
        [CategoryId] int NOT NULL,
        [Code] nvarchar(100) NOT NULL,
        [Description] nvarchar(500) NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_TechnicalExamVehiclePart] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamReport] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [CustomerVehicleRelationId] bigint NULL,
        [TechnicalExamTypeId] int NOT NULL,
        [OrganizationId] int NOT NULL,
        [RegNumber] nvarchar(50) NULL,
        [MadeDate] date NOT NULL,
        [ValidTillDate] date NOT NULL,
        [FirstControllerLegacyId] int NULL,
        [SecondControllerLegacyId] int NULL,
        [VehicleIsRight] bit NOT NULL,
        [ExplanationNote] nvarchar(500) NULL,
        [DriversWarning] nvarchar(500) NULL,
        [Note] nvarchar(500) NULL,
        [TechnicalChanges] nvarchar(max) NULL,
        [Axis1Left] float NULL,
        [Axis1Right] float NULL,
        [Axis1Gj] float NULL,
        [Axis1LeftRightDiff] float NULL,
        [Axis1Coefficient] float NULL,
        [Axis2Left] float NULL,
        [Axis2Right] float NULL,
        [Axis2Gj] float NULL,
        [Axis2LeftRightDiff] float NULL,
        [Axis2Coefficient] float NULL,
        [Axis3Left] float NULL,
        [Axis3Right] float NULL,
        [Axis3Gj] float NULL,
        [Axis3LeftRightDiff] float NULL,
        [Axis3Coefficient] float NULL,
        [Axis4Left] float NULL,
        [Axis4Right] float NULL,
        [Axis4Gj] float NULL,
        [Axis4LeftRightDiff] float NULL,
        [Axis4Coefficient] float NULL,
        [AxisParkingLeft] float NULL,
        [AxisParkingRight] float NULL,
        [AxisParkingGj] float NULL,
        [AxisParkingLeftRightDiff] float NULL,
        [AxisParkingCoefficient] float NULL,
        [Weight] float NULL,
        [EffectOfWorkingBrakeEmpty] float NULL,
        [EffectOfWorkingBrakeFull] float NULL,
        [EffectOfSecondaryBrake] float NULL,
        [EffectOfParkingBrake] float NULL,
        [SpeedOfTurns] float NULL,
        [CO] float NULL,
        [EngineRpm] float NULL,
        [COPlusTurns] float NULL,
        [Lambda] float NULL,
        [Pinpoints] float NULL,
        [Noise] float NULL,
        [EngineOilTemp] float NULL,
        [Active] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ModifiedAt] datetime2 NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_TechnicalExamReport] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TechnicalExamReport_ClientVehicleRelation_CustomerVehicleRelationId] FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [ClientVehicleRelation] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TechnicalExamReport_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TechnicalExamReport_TechnicalExamOrganization_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [TechnicalExamOrganization] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TechnicalExamReport_TechnicalExamType_TechnicalExamTypeId] FOREIGN KEY ([TechnicalExamTypeId]) REFERENCES [TechnicalExamType] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE TABLE [TechnicalExamReportDetail] (
        [Id] bigint NOT NULL IDENTITY,
        [TechnicalExamReportId] bigint NOT NULL,
        [VehiclePartId] int NOT NULL,
        [StatusId] int NOT NULL,
        [Front] bit NOT NULL,
        [Back] bit NOT NULL,
        [OnLeft] bit NOT NULL,
        [OnRight] bit NOT NULL,
        [EnteredAt] datetime2 NOT NULL,
        [Note] nvarchar(300) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_TechnicalExamReportDetail] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_TechnicalExamReportDetail_TechnicalExamDetailStatus_StatusId] FOREIGN KEY ([StatusId]) REFERENCES [TechnicalExamDetailStatus] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_TechnicalExamReportDetail_TechnicalExamReport_TechnicalExamReportId] FOREIGN KEY ([TechnicalExamReportId]) REFERENCES [TechnicalExamReport] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_TechnicalExamReportDetail_TechnicalExamVehiclePart_VehiclePartId] FOREIGN KEY ([VehiclePartId]) REFERENCES [TechnicalExamVehiclePart] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamOrganization_CityId] ON [TechnicalExamOrganization] ([CityId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReport_CompanyId_MadeDate] ON [TechnicalExamReport] ([CompanyId], [MadeDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReport_CustomerVehicleRelationId] ON [TechnicalExamReport] ([CustomerVehicleRelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReport_OrganizationId] ON [TechnicalExamReport] ([OrganizationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReport_RegNumber] ON [TechnicalExamReport] ([RegNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReport_TechnicalExamTypeId] ON [TechnicalExamReport] ([TechnicalExamTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReportDetail_StatusId] ON [TechnicalExamReportDetail] ([StatusId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReportDetail_TechnicalExamReportId] ON [TechnicalExamReportDetail] ([TechnicalExamReportId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamReportDetail_VehiclePartId] ON [TechnicalExamReportDetail] ([VehiclePartId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    CREATE INDEX [IX_TechnicalExamVehiclePart_CategoryId] ON [TechnicalExamVehiclePart] ([CategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260528211909_AddTechnicalExams'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260528211909_AddTechnicalExams', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [InstallmentAgreement] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [Number] nvarchar(40) NOT NULL,
        [Date] date NOT NULL,
        [TotalInstallments] int NOT NULL,
        [GuarantorName] nvarchar(200) NULL,
        [GuarantorAddress] nvarchar(300) NULL,
        [GuarantorEmbg] nvarchar(13) NULL,
        [Active] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ModifiedAt] datetime2 NULL,
        CONSTRAINT [PK_InstallmentAgreement] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_InstallmentAgreement_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [PaymentType] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(40) NULL,
        [Name] nvarchar(150) NOT NULL,
        [IsCash] bit NOT NULL,
        [IsCard] bit NOT NULL,
        [IsInstallment] bit NOT NULL,
        [PrintsReceipt] bit NOT NULL,
        [PrintsInvoice] bit NOT NULL,
        [Prefix] nvarchar(20) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_PaymentType] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [VatRate] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(20) NULL,
        [Name] nvarchar(100) NOT NULL,
        [Percent] float NOT NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_VatRate] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [PaymentDocument] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [PaymentTypeId] int NOT NULL,
        [CustomerVehicleRelationId] bigint NOT NULL,
        [OperatorLegacyId] int NULL,
        [OrganizationId] int NOT NULL,
        [DocumentNumber] nvarchar(50) NOT NULL,
        [IssueDate] datetime2 NOT NULL,
        [DueDate] datetime2 NOT NULL,
        [Discount] float NULL,
        [Paid] bit NOT NULL,
        [Stornoed] bit NOT NULL,
        [StornoReason] nvarchar(500) NULL,
        [Note] nvarchar(500) NULL,
        [AgreementId] bigint NULL,
        [InvoicedToCompanyId] int NULL,
        [FiscalPrintedAt] datetime2 NULL,
        [LegacyId] bigint NULL,
        [Active] bit NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ModifiedAt] datetime2 NULL,
        [CreatedByUserId] nvarchar(450) NULL,
        [ModifiedByUserId] nvarchar(450) NULL,
        [RowVersion] rowversion NOT NULL,
        CONSTRAINT [PK_PaymentDocument] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PaymentDocument_ClientVehicleRelation_CustomerVehicleRelationId] FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [ClientVehicleRelation] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PaymentDocument_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PaymentDocument_InstallmentAgreement_AgreementId] FOREIGN KEY ([AgreementId]) REFERENCES [InstallmentAgreement] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_PaymentDocument_PaymentType_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [PaymentType] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [PriceCatalog] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(40) NULL,
        [Name] nvarchar(250) NOT NULL,
        [BasePrice] decimal(18,4) NOT NULL,
        [VatRateId] int NOT NULL,
        [Trigger] tinyint NOT NULL,
        [VehicleCategoryFilter] nvarchar(200) NULL,
        [BankAccount] nvarchar(50) NULL,
        [PaymentForm] nvarchar(20) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_PriceCatalog] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PriceCatalog_VatRate_VatRateId] FOREIGN KEY ([VatRateId]) REFERENCES [VatRate] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [InstallmentSchedule] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [PaymentDocumentId] bigint NOT NULL,
        [SequenceNo] int NOT NULL,
        [Amount] decimal(18,4) NOT NULL,
        [DueDate] date NULL,
        [Paid] bit NOT NULL,
        [PaidAt] datetime2 NULL,
        [PaidAmount] decimal(18,4) NULL,
        [OrganizationId] int NULL,
        [OperatorLegacyId] int NULL,
        [Note] nvarchar(300) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_InstallmentSchedule] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_InstallmentSchedule_PaymentDocument_PaymentDocumentId] FOREIGN KEY ([PaymentDocumentId]) REFERENCES [PaymentDocument] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE TABLE [PaymentDocumentLine] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [PaymentDocumentId] bigint NOT NULL,
        [PriceCatalogId] int NOT NULL,
        [UnitPrice] decimal(18,4) NOT NULL,
        [VatPercent] float NOT NULL,
        [Discount] float NOT NULL,
        [Quantity] int NOT NULL,
        [Note] nvarchar(300) NULL,
        [PrePaid] bit NOT NULL,
        [PrePaidNote] nvarchar(300) NULL,
        [CustomerDebtId] bigint NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_PaymentDocumentLine] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PaymentDocumentLine_PaymentDocument_PaymentDocumentId] FOREIGN KEY ([PaymentDocumentId]) REFERENCES [PaymentDocument] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_PaymentDocumentLine_PriceCatalog_PriceCatalogId] FOREIGN KEY ([PriceCatalogId]) REFERENCES [PriceCatalog] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_InstallmentAgreement_CompanyId] ON [InstallmentAgreement] ([CompanyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_InstallmentAgreement_Number] ON [InstallmentAgreement] ([Number]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_InstallmentSchedule_PaymentDocumentId] ON [InstallmentSchedule] ([PaymentDocumentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE UNIQUE INDEX [IX_InstallmentSchedule_PaymentDocumentId_SequenceNo] ON [InstallmentSchedule] ([PaymentDocumentId], [SequenceNo]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocument_AgreementId] ON [PaymentDocument] ([AgreementId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocument_CompanyId_IssueDate] ON [PaymentDocument] ([CompanyId], [IssueDate]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocument_CustomerVehicleRelationId] ON [PaymentDocument] ([CustomerVehicleRelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocument_DocumentNumber] ON [PaymentDocument] ([DocumentNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_PaymentDocument_LegacyId] ON [PaymentDocument] ([LegacyId]) WHERE [LegacyId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocument_PaymentTypeId] ON [PaymentDocument] ([PaymentTypeId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_PaymentDocumentLine_CustomerDebtId] ON [PaymentDocumentLine] ([CustomerDebtId]) WHERE [CustomerDebtId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocumentLine_PaymentDocumentId] ON [PaymentDocumentLine] ([PaymentDocumentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PaymentDocumentLine_PriceCatalogId] ON [PaymentDocumentLine] ([PriceCatalogId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PriceCatalog_Trigger] ON [PriceCatalog] ([Trigger]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    CREATE INDEX [IX_PriceCatalog_VatRateId] ON [PriceCatalog] ([VatRateId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260529231700_AddPayments'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260529231700_AddPayments', N'9.0.0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [CommunityId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [ParametarFrom] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [ParametarTo] float NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [PaymentCategoryGroupId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [VehicleField] nvarchar(60) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    ALTER TABLE [PriceCatalog] ADD [VehiclePaymentCategoryId] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE TABLE [CustomerDebt] (
        [Id] bigint NOT NULL IDENTITY,
        [CompanyId] tinyint NOT NULL,
        [CustomerVehicleRelationId] bigint NOT NULL,
        [PriceCatalogId] int NOT NULL,
        [Price] decimal(18,4) NOT NULL,
        [VatPercent] float NOT NULL,
        [Note] nvarchar(300) NULL,
        [Origin] tinyint NOT NULL,
        [OriginRequestId] bigint NULL,
        [OriginTechnicalExamId] bigint NULL,
        [OrganizationId] int NOT NULL,
        [Paid] bit NOT NULL,
        [SettledByLineId] bigint NULL,
        [CreatedAt] datetime2 NOT NULL,
        [CreatedByUserId] nvarchar(450) NULL,
        [Active] bit NOT NULL,
        CONSTRAINT [PK_CustomerDebt] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CustomerDebt_ClientVehicleRelation_CustomerVehicleRelationId] FOREIGN KEY ([CustomerVehicleRelationId]) REFERENCES [ClientVehicleRelation] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerDebt_Company_CompanyId] FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerDebt_PaymentDocumentLine_SettledByLineId] FOREIGN KEY ([SettledByLineId]) REFERENCES [PaymentDocumentLine] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_CustomerDebt_PriceCatalog_PriceCatalogId] FOREIGN KEY ([PriceCatalogId]) REFERENCES [PriceCatalog] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE INDEX [IX_PriceCatalog_Eval] ON [PriceCatalog] ([Trigger], [VehiclePaymentCategoryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE INDEX [IX_CustomerDebt_CompanyId] ON [CustomerDebt] ([CompanyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE INDEX [IX_CustomerDebt_CustomerVehicleRelationId] ON [CustomerDebt] ([CustomerVehicleRelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_CustomerDebt_Open] ON [CustomerDebt] ([CustomerVehicleRelationId], [Paid]) WHERE [Active] = 1 AND [Paid] = 0');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_CustomerDebt_OriginRequestId] ON [CustomerDebt] ([OriginRequestId]) WHERE [OriginRequestId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    EXEC(N'CREATE INDEX [IX_CustomerDebt_OriginTechnicalExamId] ON [CustomerDebt] ([OriginTechnicalExamId]) WHERE [OriginTechnicalExamId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE INDEX [IX_CustomerDebt_PriceCatalogId] ON [CustomerDebt] ([PriceCatalogId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    CREATE INDEX [IX_CustomerDebt_SettledByLineId] ON [CustomerDebt] ([SettledByLineId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260530003046_AddCustomerDebtsAndExpandPriceCatalog', N'9.0.0');
END;

COMMIT;
GO


-- ============================================================================
-- Post-migration schema drift (not yet captured in EF migrations)
-- ============================================================================
-- The PriceCompanyId column on PriceCatalog was added via raw SQL in May 2026
-- to fix a duplicate-debt bug (multiple companies' "Operating fee" rules all
-- firing for one vehicle). It's idempotent.
-- See: migrate/add-pricecatalog-company-and-backfill.sql for full context.

IF NOT EXISTS (SELECT 1 FROM sys.columns
               WHERE object_id = OBJECT_ID('dbo.PriceCatalog') AND name = 'PriceCompanyId')
BEGIN
    ALTER TABLE dbo.PriceCatalog ADD PriceCompanyId tinyint NULL;
END;
GO
