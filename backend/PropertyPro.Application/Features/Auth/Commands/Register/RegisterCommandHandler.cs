using MediatR;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler
    : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public RegisterCommandHandler(
        IIdentityService identityService,
        ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();

        var userId = await _identityService.CreateUserAsync(
            email,
            request.Password,
            firstName,
            lastName,
            cancellationToken);

        return await _tokenService.CreateTokenAsync(
            userId,
            cancellationToken);
    }
}