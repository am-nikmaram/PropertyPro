using MediatR;

namespace PropertyPro.Application.Features.Users.Commands.AssignRole;

public record AssignRoleCommand(
    string UserId,
    string Role
) : IRequest;