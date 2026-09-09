namespace PropertyPro.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> CreateUserAsync(
        string email,
        string password,
        string firstName,
        string lastName,
        CancellationToken cancellationToken);

    Task<string?> ValidateUserAsync(
        string email,
        string password,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<string>> GetUserRolesAsync(
        string userId,
        CancellationToken cancellationToken);
}