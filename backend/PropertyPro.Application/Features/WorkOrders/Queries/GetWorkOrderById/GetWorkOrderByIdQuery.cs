using MediatR;
using PropertyPro.Application.Features.WorkOrders.DTOs;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderById;

public record GetWorkOrderByIdQuery(
    int Id
) : IRequest<WorkOrderDto>;