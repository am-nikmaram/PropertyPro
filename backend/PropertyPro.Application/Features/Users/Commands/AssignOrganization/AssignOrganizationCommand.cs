using MediatR;

namespace PropertyPro.Application.Features.Users.Commands.AssignOrganization;

public record AssignOrganizationCommand(string UserId, int OrganizationId) : IRequest;
