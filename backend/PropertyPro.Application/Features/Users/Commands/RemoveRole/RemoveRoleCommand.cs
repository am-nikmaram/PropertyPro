using MediatR;

namespace PropertyPro.Application.Features.Users.Commands.RemoveRole;

public record RemoveRoleCommand(
    string UserId,
    string Role
) : IRequest;