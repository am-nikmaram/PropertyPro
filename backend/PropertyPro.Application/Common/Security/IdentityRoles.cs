namespace PropertyPro.Application.Common.Security;

public static class IdentityRoles
{
    public const string Admin = "Admin";

    public const string FacilityManager = "FacilityManager";

    public const string MaintenanceManager = "MaintenanceManager";

    public const string Technician = "Technician";

    public const string Viewer = "Viewer";

    public static readonly string[] All =
    [
        Admin,
        FacilityManager,
        MaintenanceManager,
        Technician,
        Viewer
    ];

    public static string Normalize(string role)
    {
        return All.First(
            x => string.Equals(
                x,
                role.Trim(),
                StringComparison.OrdinalIgnoreCase));
    }
}