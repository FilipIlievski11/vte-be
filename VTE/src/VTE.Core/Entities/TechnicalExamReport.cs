namespace VTE.Core.Entities;

using VTE.Core.Lookups;

public class TechnicalExamReport : AuditableEntity
{
    public long CustomerVehicleRelationId { get; set; }
    public CustomerVehicleRelation CustomerVehicleRelation { get; set; } = null!;
    public long ExamTypeId { get; set; }
    public TechnicalExamType ExamType { get; set; } = null!;
    public long OrganizationId { get; set; }
    public TechnicalExamOrganization Organization { get; set; } = null!;
    public string RegistrationNumber { get; set; } = string.Empty;
    public DateTime ExamDate { get; set; }
    public DateTime ValidUntilDate { get; set; }
    public long FirstControllerId { get; set; }
    public User FirstController { get; set; } = null!;
    public long? SecondControllerId { get; set; }
    public User? SecondController { get; set; }
    public bool VehiclePassed { get; set; }
    public decimal? Axle1BrakeLeftKN { get; set; }
    public decimal? Axle1BrakeRightKN { get; set; }
    public decimal? Axle1BrakeGj { get; set; }
    public decimal? Axle1BrakeLeftPj { get; set; }
    public decimal? Axle1BrakePN { get; set; }
    public decimal? Axle2BrakeLeftKN { get; set; }
    public decimal? Axle2BrakeRightKN { get; set; }
    public decimal? Axle2BrakeGj { get; set; }
    public decimal? Axle2BrakeLeftPj { get; set; }
    public decimal? Axle2BrakePN { get; set; }
    public decimal? Axle3BrakeLeftKN { get; set; }
    public decimal? Axle3BrakeRightKN { get; set; }
    public decimal? Axle3BrakeGj { get; set; }
    public decimal? Axle3BrakeLeftPj { get; set; }
    public decimal? Axle3BrakePN { get; set; }
    public decimal? Axle4BrakeLeftKN { get; set; }
    public decimal? Axle4BrakeRightKN { get; set; }
    public decimal? Axle4BrakeGj { get; set; }
    public decimal? Axle4BrakeLeftPj { get; set; }
    public decimal? Axle4BrakePN { get; set; }
    public decimal? ParkingBrakeLeftKN { get; set; }
    public decimal? ParkingBrakeRightKN { get; set; }
    public decimal? ParkingBrakeGj { get; set; }
    public decimal? ParkingBrakeLeftPj { get; set; }
    public decimal? ParkingBrakePN { get; set; }
    public decimal? WorkingBrakeEffectivenessEmpty { get; set; }
    public decimal? WorkingBrakeEffectivenessFull { get; set; }
    public decimal? SecondaryBrakeEffectiveness { get; set; }
    public decimal? ParkingBrakeEffectiveness { get; set; }
    public decimal? VehicleWeightKG { get; set; }
    public int? EngineSpeedRPM { get; set; }
    public decimal? CO { get; set; }
    public int? EngineTurns { get; set; }
    public decimal? COPlusTurns { get; set; }
    public decimal? Lambda { get; set; }
    public decimal? Pinpoints { get; set; }
    public decimal? NoiseDB { get; set; }
    public decimal? EngineOilTemperatureC { get; set; }
    public string? TechnicalChanges { get; set; }
    public string? ExplanationNote { get; set; }
    public string? DriverWarning { get; set; }
    public string? Note { get; set; }
    public List<TechnicalExamReportDetail> Details { get; set; } = [];
    public List<TechnicalExamVisualError> VisualErrors { get; set; } = [];
}
