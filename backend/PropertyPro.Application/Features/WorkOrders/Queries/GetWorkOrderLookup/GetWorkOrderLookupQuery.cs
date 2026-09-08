using MediatR;
using PropertyPro.Application.Features.WorkOrders.DTOs;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderLookup;

public record GetWorkOrderLookupQuery(
    int? MaintenanceRecordId
) : IRequest<IReadOnlyList<WorkOrderLookupDto>>;