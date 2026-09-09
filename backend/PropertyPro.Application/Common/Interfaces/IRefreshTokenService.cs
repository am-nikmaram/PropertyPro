namespace PropertyPro.Application.Common.Interfaces;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(
    string userId,
    CancellationToken cancellationToken);

Task<bool> IsActiveAsync(
    string token,
    CancellationToken cancellationToken);

    Task<string?> GetUserIdAsync(
        string token,
        CancellationToken cancellationToken);

    Task RevokeAsync(
        string token,
        string? replacedByToken,
        CancellationToken cancellationToken);

}
