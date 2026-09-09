using MediatR;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IIdentityService identityService,
        ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var userId = await _identityService.ValidateUserAsync(
            email,
            request.Password,
            cancellationToken);

        if (userId is null)
        {
            throw new UnauthorizedException(
                "Invalid email or password.");
        }

        return await _tokenService.CreateTokenAsync(
            userId,
            cancellationToken);
    }
}