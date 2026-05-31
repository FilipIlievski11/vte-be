namespace VTE.Domain.Payments;

/// <summary>
/// Replaces the legacy boolean cluster on PriceCatalog (IsRequest/IsTehnicalExam/
/// IsTrafficLicence/IsPermisionForVehicle/IsInernationalDriveingLicence) with a
/// single typed value. A price-catalog entry fires when its owning workflow ends.
/// </summary>
public enum PriceTrigger : byte
{
    None = 0,
    TechnicalExam = 1,
    Request = 2,
    TrafficLicence = 3,
    Permission = 4,
    InternationalDrivingLicence = 5,
    /// <summary>Irregular tech-exam type — legacy TrigerdByIrregularTechnicalExam.</summary>
    TechnicalExamIrregular = 6,
}
