using MediatR;
using PropertyPro.Application.Features.Facilities.DTOs;

namespace PropertyPro.Application.Features.Facilities.Commands.UpdateFacility;

public record UpdateFacilityCommand(
    int Id,
    string Name,
    string Code,
    string? Address,
    string? City,
    string? Country,
    string? PostalCode)
    : IRequest<FacilityDto>;
