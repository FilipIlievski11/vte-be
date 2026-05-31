namespace VTE.Domain.Vehicles;

// Per-vehicle registration history — one row per registration period.
// Already populated from legacy `Vehicle.Registrations` during the bulk migration.
public class VehicleRegistration
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public string RegistrationNumber { get; set; } = string.Empty;
    public DateOnly? MakeDate { get; set; }
    public DateOnly? ValidTill { get; set; }
    public int? IssuerId { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

// One row per axle on a vehicle. Carries IsPropulsion / IsSteering flags + a free-text Note
// (the legacy CarryingCapacity + AxleLength values were stuffed into Note during bulk load).
public class VehicleAxle
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public int AxleNumber { get; set; }
    public bool IsPropulsion { get; set; }
    public bool IsSteering { get; set; }
    public string? Note { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

// Distance between two axles. Legacy stored "1-2" as text; new schema parses into two int FKs.
public class VehicleAxleDistance
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public int FromAxleNumber { get; set; }
    public int ToAxleNumber { get; set; }
    public decimal? Distance { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

// Per-vehicle tyre configuration with dimensions + recommended pressures.
public class VehicleTyre
{
    public long Id { get; set; }
    public long VehicleId { get; set; }
    public int? TireTypeId { get; set; }
    public string? PositionNote { get; set; }
    public string? Dimensions { get; set; }
    public decimal? PressureFront { get; set; }
    public decimal? PressureRear { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
