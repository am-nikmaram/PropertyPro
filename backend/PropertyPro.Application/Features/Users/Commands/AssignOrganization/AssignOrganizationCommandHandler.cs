using MediatR;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.Users.Commands.AssignOrganization;

public sealed class AssignOrganizationCommandHandler : IRequestHandler<AssignOrganizationCommand>
{
    private readonly IUserManagementService _users;
    public AssignOrganizationCommandHandler(IUserManagementService users) => _users = users;

    public Task Handle(AssignOrganizationCommand request, CancellationToken cancellationToken) =>
        _users.AssignOrganizationAsync(request.UserId, request.OrganizationId, cancellationToken);
}
