using MediatR;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Refresh;

public record RefreshTokenCommand(
string RefreshToken
) : IRequest<AuthResponseDto>;
