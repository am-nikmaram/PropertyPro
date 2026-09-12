using MediatR;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler
    : IRequestHandler<GetUserByIdQuery, UserManagementDto>
{
    private readonly IUserManagementService _userManagementService;

    public GetUserByIdQueryHandler(
        IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    public async Task<UserManagementDto> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _userManagementService.GetUserByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(
                $"User '{request.UserId}' was not found.");
        }

        return user;
    }
}