using MediatR;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Security;

namespace PropertyPro.Application.Features.Users.Commands.AssignRole;

public class AssignRoleCommandHandler
    : IRequestHandler<AssignRoleCommand>
{
    private readonly IUserManagementService _userManagementService;

    public AssignRoleCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task Handle(
        AssignRoleCommand request,
        CancellationToken cancellationToken)
    {
        

        var existingUser =
            await _userManagementService.GetUserByIdAsync(
                request.UserId,
                cancellationToken);

        if (existingUser is null)
        {
            throw new NotFoundException(
                $"User '{request.UserId}' was not found.");
        }
        var role = IdentityRoles.Normalize(request.Role);

        await _userManagementService.AssignRoleAsync(
            request.UserId,
            role,
            cancellationToken);
    }
}