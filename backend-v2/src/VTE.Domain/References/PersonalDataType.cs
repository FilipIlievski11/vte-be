namespace VTE.Domain.References;

/// <summary>
/// Fixed enumeration. PK is not IDENTITY; seeded as: 1=Driving License, 2=Passport, 3=Personal Id.
/// </summary>
public class PersonalDataType
{
    public byte Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
