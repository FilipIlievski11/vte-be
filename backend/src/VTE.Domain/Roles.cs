namespace VTE.Domain;

// Hard-coded role names — match the seed inserts in bootstrap-vte2.sql.
public static class Roles
{
    public const string Administrator = "Administrator";
    public const string Operator = "Operator";

    public static readonly string[] All = { Administrator, Operator };
}

// JWT claim type constants for tenancy.
public static class VteClaims
{
    public const string StationId = "stationId";
}
