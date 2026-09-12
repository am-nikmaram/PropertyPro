using PropertyPro.Application.Features.Users.DTOs;
namespace PropertyPro.Application.Common.Interfaces;

public interface IUserManagementService
{
    Task<IReadOnlyList<UserManagementDto>> GetUsersAsync(
        CancellationToken cancellationToken);

    Task<UserManagementDto?> GetUserByIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetRolesAsync(
        string userId,
        CancellationToken cancellationToken);

    Task AssignRoleAsync(
        string userId,
        string role,
        CancellationToken cancellationToken);

    Task RemoveRoleAsync(
        string userId,
        string role,
        CancellationToken cancellationToken);
}