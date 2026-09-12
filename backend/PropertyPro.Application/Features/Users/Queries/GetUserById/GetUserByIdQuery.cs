using MediatR;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(
    string UserId
) : IRequest<UserManagementDto>;