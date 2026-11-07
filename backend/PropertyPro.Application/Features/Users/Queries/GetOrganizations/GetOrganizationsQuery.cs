using MediatR;
using PropertyPro.Application.Features.Users.DTOs;

namespace PropertyPro.Application.Features.Users.Queries.GetOrganizations;

public record GetOrganizationsQuery : IRequest<IReadOnlyList<OrganizationOptionDto>>;
