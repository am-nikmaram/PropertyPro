using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Common.Interfaces;

public interface ITokenService
{
    Task<AuthResponseDto> CreateTokenAsync(
    string userId,
    CancellationToken cancellationToken);
}
