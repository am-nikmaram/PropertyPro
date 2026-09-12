using MediatR;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler
    : IRequestHandler<
        GetUsersQuery,
        IReadOnlyList<UserManagementDto>>
{
    private readonly IUserManagementService _userManagementService;

    public GetUsersQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<IReadOnlyList<UserManagementDto>> Handle(
        GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        return await _userManagementService.GetUsersAsync(
            cancellationToken);
    }
}