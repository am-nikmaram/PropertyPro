using MediatR;
using PropertyPro.Application.Features.Auth.DTOs;

namespace PropertyPro.Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<AuthResponseDto>;