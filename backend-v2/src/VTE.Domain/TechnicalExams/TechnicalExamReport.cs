using VTE.Domain.Common;

namespace VTE.Domain.TechnicalExams;

/// <summary>
/// A vehicle technical-exam report (Технички преглед). Anchored to a
/// <c>ClientVehicleRelation</c> (the inspected customer+vehicle), it records the
/// exam type, issuing organization, the two inspectors, the pass/fail outcome
/// and the measured brake-force / emission test values. Defective parts live in
/// <see cref="TechnicalExamReportDetail"/>.
///
/// Legacy source: <c>DocumentsTehnicalExamsReports</c>. Improvements over legacy:
/// measurements are nullable (a real "not measured" instead of a 0 sentinel),
/// dates are <see cref="DateOnly"/>, lookups are real FKs, the row is tenant-scoped,
/// and the dead <c>LastChanged</c>/VisualErrors columns are dropped.
///
/// Business rules (preserved from the legacy WinForms app):
///   • RegNumber  = "{StationId}-{seq}/{year}".
///   • ValidTillDate = MadeDate + <see cref="TechnicalExamType.ValidDays"/>.
///   • VehicleIsRight = true ⇔ every detail line is status 1 (исправен), else false.
/// </summary>
public class TechnicalExamReport : ITenantOwned
{
    public long Id { get; set; }
    public byte CompanyId { get; set; }

    /// <summary>Inspected customer+vehicle. Nullable: 131 legacy reports point at a
    /// relation that no longer exists (deleted), and migrate as NULL rather than
    /// being dropped. Legacy: IdCustomerVehicleRelation.</summary>
    public long? CustomerVehicleRelationId { get; set; }

    /// <summary>Exam type. Legacy: IdTypeOfTehnicalExam.</summary>
    public int TechnicalExamTypeId { get; set; }

    /// <summary>Issuing organization/station. Legacy: IdOrganizationForTehnicalExam.</summary>
    public int OrganizationId { get; set; }

    /// <summary>Report number, format "{StationId}-{seq}/{year}". Legacy: RegNumber.</summary>
    public string? RegNumber { get; set; }

    public DateOnly MadeDate { get; set; }
    public DateOnly ValidTillDate { get; set; }

    /// <summary>The two inspectors. Stored as the legacy employee ids because the
    /// inspector directory lives in a separate security DB not part of this
    /// migration; resolve to v2 users later. 0 → null. Legacy: IdFirsControler / IdSecondControler.</summary>
    public int? FirstControllerLegacyId { get; set; }
    public int? SecondControllerLegacyId { get; set; }

    /// <summary>Pass (true) / fail (false). Legacy: VehicleIsRight.</summary>
    public bool VehicleIsRight { get; set; } = true;

    public string? ExplanationNote { get; set; }   // Legacy: ExplanationNote
    public string? DriversWarning { get; set; }     // Legacy: DriversWarning
    public string? Note { get; set; }               // Legacy: Note
    public string? TechnicalChanges { get; set; }   // Legacy: TechnicalChanges (nvarchar max)

    // ---- Brake-force test: 4 axles + parking brake. Each: ----
    //   Left / Right            — measured force per wheel
    //   Gj                      — legacy "Gj" (imbalance/limit value), kept verbatim
    //   LeftRightDiff           — legacy {Axis}LeftPj (left/right difference %)
    //   Coefficient             — legacy {Axis}PN (efficiency coefficient %)
    public double? Axis1Left { get; set; }
    public double? Axis1Right { get; set; }
    public double? Axis1Gj { get; set; }
    public double? Axis1LeftRightDiff { get; set; }
    public double? Axis1Coefficient { get; set; }

    public double? Axis2Left { get; set; }
    public double? Axis2Right { get; set; }
    public double? Axis2Gj { get; set; }
    public double? Axis2LeftRightDiff { get; set; }
    public double? Axis2Coefficient { get; set; }

    public double? Axis3Left { get; set; }
    public double? Axis3Right { get; set; }
    public double? Axis3Gj { get; set; }
    public double? Axis3LeftRightDiff { get; set; }
    public double? Axis3Coefficient { get; set; }

    public double? Axis4Left { get; set; }
    public double? Axis4Right { get; set; }
    public double? Axis4Gj { get; set; }
    public double? Axis4LeftRightDiff { get; set; }
    public double? Axis4Coefficient { get; set; }

    public double? AxisParkingLeft { get; set; }
    public double? AxisParkingRight { get; set; }
    public double? AxisParkingGj { get; set; }
    public double? AxisParkingLeftRightDiff { get; set; }
    public double? AxisParkingCoefficient { get; set; }

    // ---- Summary measurements ----
    public double? Weight { get; set; }                     // Legacy: Waight
    public double? EffectOfWorkingBrakeEmpty { get; set; }  // Legacy: EffectOfWorkingBreakEmpty
    public double? EffectOfWorkingBrakeFull { get; set; }   // Legacy: EffectOfWorkingBreakFull
    public double? EffectOfSecondaryBrake { get; set; }     // Legacy: EffectOfSecondaryBreak
    public double? EffectOfParkingBrake { get; set; }       // Legacy: EffectOfParkingBreak
    public double? SpeedOfTurns { get; set; }               // Legacy: SpeedOfTurns
    public double? CO { get; set; }                         // Legacy: CO
    public double? EngineRpm { get; set; }                  // Legacy: NumEngineTurns
    public double? COPlusTurns { get; set; }                // Legacy: COPlusTurns
    public double? Lambda { get; set; }                     // Legacy: Lambda
    public double? Pinpoints { get; set; }                  // Legacy: Pinpoints
    public double? Noise { get; set; }                      // Legacy: Noise
    public double? EngineOilTemp { get; set; }              // Legacy: TempOfEngineOil

    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ModifiedAt { get; set; }

    /// <summary>EF concurrency token.</summary>
    public byte[] RowVersion { get; set; } = [];
}
