using MediatR;
using PropertyPro.Application.Features.Maintenance.DTOs;

namespace PropertyPro.Application.Features.Maintenance.Queries.GetMaintenanceById;

public record GetMaintenanceByIdQuery(
    int Id
) : IRequest<MaintenanceRecordDto>;