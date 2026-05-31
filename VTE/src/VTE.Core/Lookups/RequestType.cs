namespace VTE.Core.Lookups;

using VTE.Core.Entities;

public class RequestType : LookupEntity
{
    public bool IsTehnicalExamRequired { get; set; }
    public bool IsPayRequired { get; set; }
    public bool IsNewRegistration { get; set; }
    public bool IsPreviousRegistrationRequired { get; set; }
    public bool IsRelationDeleted { get; set; }
    public bool IsVehicleDeleted { get; set; }
    public bool IsNewCustomer { get; set; }
    public bool IsVehicleChanged { get; set; }
    public bool IsCustomerChanged { get; set; }
}
