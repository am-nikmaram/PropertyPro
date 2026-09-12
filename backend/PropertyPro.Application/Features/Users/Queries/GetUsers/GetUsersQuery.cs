using MediatR;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery
    : IRequest<IReadOnlyList<UserManagementDto>>;