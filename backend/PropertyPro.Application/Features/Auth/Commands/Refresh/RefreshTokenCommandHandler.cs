using MediatR;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Refresh;

public class RefreshTokenCommandHandler
: IRequestHandler<RefreshTokenCommand, AuthResponseDto>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly ITokenService _tokenService;


public RefreshTokenCommandHandler(
    IRefreshTokenService refreshTokenService,
    ITokenService tokenService)
    {
        _refreshTokenService = refreshTokenService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var refreshToken = request.RefreshToken.Trim();

        var isActive = await _refreshTokenService.IsActiveAsync(
            refreshToken,
            cancellationToken);

        if (!isActive)
        {
            throw new UnauthorizedException(
                "The refresh token is invalid or has expired.");
        }

        var userId = await _refreshTokenService.GetUserIdAsync(
            refreshToken,
            cancellationToken);

        if (userId is null)
        {
            throw new UnauthorizedException(
                "The refresh token is invalid or has expired.");
        }

        var authResponse = await _tokenService.CreateTokenAsync(
            userId,
            cancellationToken);

        await _refreshTokenService.RevokeAsync(
            refreshToken,
            authResponse.RefreshToken,
            cancellationToken);

        return authResponse;
    }

}
