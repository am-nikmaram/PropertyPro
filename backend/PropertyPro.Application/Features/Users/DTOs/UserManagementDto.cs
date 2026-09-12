namespace PropertyPro.Application.Features.Users.DTOs;

public record UserManagementDto(
    string Id,
    string Email,
    string FirstName,
    string LastName,
    bool EmailConfirmed,
    bool LockoutEnabled,
    bool IsLockedOut,
    IReadOnlyList<string> Roles);