using MediatR;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<AuthResponseDto>;
