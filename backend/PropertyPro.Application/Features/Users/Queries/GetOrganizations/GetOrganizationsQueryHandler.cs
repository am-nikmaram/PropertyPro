using MediatR;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetOrganizations;

public sealed class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, IReadOnlyList<OrganizationOptionDto>>
{
    private readonly IUserManagementService _users;
    public GetOrganizationsQueryHandler(IUserManagementService users) => _users = users;

    public Task<IReadOnlyList<OrganizationOptionDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken) =>
        _users.GetOrganizationsAsync(cancellationToken);
}
