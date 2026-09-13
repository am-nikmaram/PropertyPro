using MediatR;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Common.Security;

namespace PropertyPro.Application.Features.Users.Commands.RemoveRole;

public class RemoveRoleCommandHandler
    : IRequestHandler<RemoveRoleCommand>
{
    private readonly IUserManagementService _userManagementService;

    public RemoveRoleCommandHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task Handle(
        RemoveRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = IdentityRoles.Normalize(request.Role);

        var user =
            await _userManagementService.GetUserByIdAsync(
                request.UserId,
                cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                $"User '{request.UserId}' was not found.");
        }

        var hasRole = user.Roles.Any(
            x => string.Equals(
                x,
                role,
                StringComparison.OrdinalIgnoreCase));

        if (!hasRole)
        {
            return;
        }

        if (string.Equals(
                role,
                IdentityRoles.Admin,
                StringComparison.OrdinalIgnoreCase))
        {
            var adminUsers =
                await _userManagementService.GetUsersAsync(
                    cancellationToken);

            var adminCount = adminUsers.Count(
                x => x.Roles.Any(
                    r => string.Equals(
                        r,
                        IdentityRoles.Admin,
                        StringComparison.OrdinalIgnoreCase)));

            if (adminCount <= 1)
            {
                throw new ConflictException(
                    "The last Admin role cannot be removed.");
            }
        }

        await _userManagementService.RemoveRoleAsync(
            request.UserId,
            role,
            cancellationToken);
    }
}