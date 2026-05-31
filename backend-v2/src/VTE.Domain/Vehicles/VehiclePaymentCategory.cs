namespace VTE.Domain.Vehicles;

/// <summary>Pricing category, drives fees in the price catalog.
/// Legacy table: VehicleCategoryForPayments.</summary>
public class VehiclePaymentCategory
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>Legacy ZelenMap (0-11): which category checkbox the Plav/Zelen forms
    /// tick. Only 1-4 have a checkbox (1=passenger, 2=cargo, 3=trailer, 4=moto);
    /// 0 and 5-11 render no checkbox.</summary>
    public byte? ZelenMap { get; set; }
    public bool Active { get; set; } = true;
}
