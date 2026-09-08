using MediatR;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.ChangeWorkOrderStatus;

public record ChangeWorkOrderStatusCommand(
    int Id,
    WorkOrderStatus Status
) : IRequest<WorkOrderDto>;