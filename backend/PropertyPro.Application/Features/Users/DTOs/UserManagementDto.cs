namespace PropertyPro.Application.Features.Users.DTOs;

public record UserManagementDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    bool EmailConfirmed,
    bool LockoutEnabled,
    bool IsLockedOut,
    int? OrganizationId,
    IReadOnlyList<string> Roles);

public record OrganizationOptionDto(int Id, string Name, string Code);
