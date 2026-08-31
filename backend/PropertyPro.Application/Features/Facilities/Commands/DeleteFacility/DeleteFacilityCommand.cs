using MediatR;

namespace PropertyPro.Application.Features.Facilities.Commands.DeleteFacility;

public record DeleteFacilityCommand(int Id)
    : IRequest;