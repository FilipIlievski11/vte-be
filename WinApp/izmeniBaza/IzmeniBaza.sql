set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- Description: Procedura za dodavanje na nov zapis

ALTER PROC [dbo].[addVehicleTire]
		@IdVehicle int,
		@TireAxisNumber int,
		@IdTireType int,
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [Vehicle.Tires]
		(
		IdVehicle,
		TireAxisNumber,
		IdTireType
		)
		VALUES
		(
		@IdVehicle,
		@TireAxisNumber,
		@IdTireType
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [Vehicle.Tires] WHERE Id=SCOPE_IDENTITY()
	RETURN

go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- Description: Procedura za zimanje na red po Primaren kluc
 
ALTER PROC [dbo].[getVehicleTireById] 
	@id int
AS
    SELECT 
        Id,
        IdVehicle,
        TireAxisNumber,
		IdTireType,
        LastChanged
    FROM   [Vehicle.Tires]
	WHERE (Id = @id) AND (Active = 1)

go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go



ALTER PROC [dbo].[getVehicleTireByIdVehicle]
	@IdVehicle bigint
AS
    SELECT 
        Id,
        IdVehicle,
        TireAxisNumber,
        IdTireType,
        LastChanged
    FROM   [Vehicle.Tires]
	WHERE (IdVehicle = @IdVehicle) AND (Active = 1)


go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- Description: Procedura za zimanje site zapisi

ALTER PROC [dbo].[getVehicleTires]
AS
    SELECT 
        Id,
        IdVehicle,
        TireAxisNumber,
        IdTireType,
        LastChanged
    FROM [Vehicle.Tires]
 WHERE Active = 1 

go

set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go


-- Description: Procedura za UPDATE na zapis

ALTER PROC [dbo].[updateVehicleTire]
		@Id int,
		@IdVehicle int,
		@TireAxisNumber int,
		@IdTireType nvarchar(50),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [Vehicle.Tires]
		SET
		IdVehicle = @idvehicle,
		TireAxisNumber = @tireaxisnumber,
		IdTireType = @IdTireType
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [Vehicle.Tires] WHERE ID=@id
	RETURN

go


-- Description: Procedura za zimanje site zapisi

CREATE PROC getVehicleTireTypes
AS
    SELECT 
        Id,
        TireType,
        LastChanged
    FROM [VehicleTireTypes]
 WHERE Active = 1 
GO

-- Description: Procedura za zimanje na red po Primaren kluc
 
CREATE PROC getVehicleTireTypeById 
	@id int
AS
    SELECT 
        Id,
        TireType,
        LastChanged
    FROM   [VehicleTireTypes]
	WHERE (Id = @id) AND (Active = 1)
GO


-- Description: Procedura za dodavanje na nov zapis

CREATE PROC addVehicleTireType
		@TireType nvarchar(150),
		@NewId int output,
		@newLastChanged timestamp output
AS
    INSERT INTO [VehicleTireTypes]
		(
		TireType
		)
		VALUES
		(
		@TireType
		)
	
	SELECT @NewId = Id, @newLastChanged = LastChanged
	FROM [VehicleTireTypes] WHERE Id=SCOPE_IDENTITY()
	RETURN
GO

-- Description: Procedura za UPDATE na zapis

CREATE PROC updateVehicleTireType
		@Id int,
		@TireType nvarchar(150),
		@LastChanged timestamp,
		@newLastChanged timestamp output
AS
    UPDATE [VehicleTireTypes]
		SET
		TireType = @tiretype
	WHERE ID=@id AND LastChanged=@lastChanged
  IF @@ROWCOUNT = 0
    RAISERROR('Redot e edituvan od drug korisnik', 16, 1)  
	
	SELECT @newLastChanged = LastChanged 
	FROM [VehicleTireTypes] WHERE ID=@id
	RETURN
GO

-- Description: Procedura za brisenje(AKTIVEN=FALSE) na zapis po primaren kluc

CREATE PROC deleteVehicleTireType
	@id int
AS
    UPDATE [VehicleTireTypes]
	SET
		Active=0
	WHERE ID=@id
GO