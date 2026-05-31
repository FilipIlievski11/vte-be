namespace VTE.Domain.Documents;

public class TrafficLicence
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public long CustomerVehicleRelationId { get; set; }
    public int? IssuingOrganizationId { get; set; }
    public string? TrafficLicenceNumber { get; set; }                            // BR-DOC-100
    public DateOnly MadeDate { get; set; }                                       // BR-DOC-101
    public DateOnly EndDate { get; set; }                                        // BR-DOC-102 (CHECK >= MadeDate)
    public string? Note { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class TechnicalExamReport
{
    public long Id { get; set; }
    public int StationId { get; set; }
    public long CustomerVehicleRelationId { get; set; }
    public int TechnicalExamTypeId { get; set; }                                 // BR-DOC-400
    public int OrganizationForTechnicalExamId { get; set; }                      // BR-DOC-401
    public string FirstInspectorOperatorUserId { get; set; } = string.Empty;    // BR-DOC-402
    public string? SecondInspectorOperatorUserId { get; set; }                   // BR-DOC-404 (CHECK <> First)
    public string? RegNumber { get; set; }
    public DateOnly MadeDate { get; set; }
    public DateOnly ValidTillDate { get; set; }                                  // BR-DOC-403 (CHECK >= MadeDate)
    public bool VehicleIsRight { get; set; } = true;                             // pass/fail

    // Brake-force tests — 5 fields × 5 axles. Map to columns directly.
    public decimal? Axis1Left { get; set; } public decimal? Axis1Right { get; set; } public decimal? Axis1Gj { get; set; } public decimal? Axis1LeftRightDiff { get; set; } public decimal? Axis1Coefficient { get; set; }
    public decimal? Axis2Left { get; set; } public decimal? Axis2Right { get; set; } public decimal? Axis2Gj { get; set; } public decimal? Axis2LeftRightDiff { get; set; } public decimal? Axis2Coefficient { get; set; }
    public decimal? Axis3Left { get; set; } public decimal? Axis3Right { get; set; } public decimal? Axis3Gj { get; set; } public decimal? Axis3LeftRightDiff { get; set; } public decimal? Axis3Coefficient { get; set; }
    public decimal? Axis4Left { get; set; } public decimal? Axis4Right { get; set; } public decimal? Axis4Gj { get; set; } public decimal? Axis4LeftRightDiff { get; set; } public decimal? Axis4Coefficient { get; set; }
    public decimal? AxisParkingLeft { get; set; } public decimal? AxisParkingRight { get; set; } public decimal? AxisParkingGj { get; set; } public decimal? AxisParkingLeftRightDiff { get; set; } public decimal? AxisParkingCoefficient { get; set; }

    public decimal? Weight { get; set; }
    public decimal? EffectOfWorkingBrakeEmpty { get; set; }
    public decimal? EffectOfWorkingBrakeFull { get; set; }
    public decimal? EffectOfSecondaryBrake { get; set; }
    public decimal? EffectOfParkingBrake { get; set; }
    public decimal? SpeedOfTurns { get; set; }
    public decimal? CO { get; set; }
    public decimal? NumEngineTurns { get; set; }
    public decimal? COPlusTurns { get; set; }
    public decimal? Lambda { get; set; }
    public decimal? Pinpoints { get; set; }
    public decimal? Noise { get; set; }
    public decimal? TempOfEngineOil { get; set; }
    public string? TechnicalChanges { get; set; }
    public string? ExplanationNote { get; set; }
    public string? DriversWarning { get; set; }
    public string? Note { get; set; }

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
