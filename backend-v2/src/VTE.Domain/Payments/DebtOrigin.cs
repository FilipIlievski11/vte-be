namespace VTE.Domain.Payments;

/// <summary>
/// What workflow created a <see cref="CustomerDebt"/>. Maps to the legacy
/// "TrigerdByX" boolean cluster on PaymentCategories.
/// </summary>
public enum DebtOrigin : byte
{
    Manual = 0,
    Request = 1,
    TechnicalExam = 2,
    TechnicalExamIrregular = 3,    // legacy: TrigerdByIrregularTechnicalExam
    TrafficLicence = 4,
    Permission = 5,
    InternationalDrivingLicence = 6,
}
