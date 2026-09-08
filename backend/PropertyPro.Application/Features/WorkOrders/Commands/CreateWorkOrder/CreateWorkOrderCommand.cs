using MediatR;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public record CreateWorkOrderCommand(
    int MaintenanceRecordId,
    WorkOrderType WorkOrderType,
    WorkOrderPriority Priority,
    DateTime ScheduledDate,
    decimal? EstimatedCost,
    string? Description,
    string? Notes
) : IRequest<WorkOrderDto>;