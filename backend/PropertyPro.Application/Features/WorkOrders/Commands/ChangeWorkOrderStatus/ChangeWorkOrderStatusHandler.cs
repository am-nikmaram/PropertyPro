using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Entities;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.ChangeWorkOrderStatus;

public class ChangeWorkOrderStatusHandler
    : IRequestHandler<ChangeWorkOrderStatusCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _context;

    public ChangeWorkOrderStatusHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrderDto> Handle(
        ChangeWorkOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders
            .Include(x => x.MaintenanceRecord)
                .ThenInclude(x => x.Asset)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (workOrder is null)
        {
            throw new NotFoundException(
                nameof(WorkOrder),
                request.Id);
        }

        ValidateTransition(
            workOrder.Status,
            request.Status);

        workOrder.Status = request.Status;

        switch (request.Status)
        {
            case WorkOrderStatus.InProgress:
                workOrder.StartedDate ??= DateTime.UtcNow;
                break;

            case WorkOrderStatus.Completed:
                workOrder.StartedDate ??= DateTime.UtcNow;
                workOrder.CompletedDate = DateTime.UtcNow;
                break;

            case WorkOrderStatus.Cancelled:
                workOrder.CompletedDate = null;
                break;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new WorkOrderDto(
            workOrder.Id,
            workOrder.WorkOrderNumber,
            workOrder.MaintenanceRecordId,
            workOrder.MaintenanceRecord.MaintenanceNumber,
            workOrder.MaintenanceRecord.AssetId,
            workOrder.MaintenanceRecord.Asset.AssetNumber,
            workOrder.MaintenanceRecord.Asset.Name,
            workOrder.WorkOrderType,
            workOrder.Priority,
            workOrder.Status,
            workOrder.ScheduledDate,
            workOrder.StartedDate,
            workOrder.CompletedDate,
            workOrder.EstimatedCost,
            workOrder.ActualCost,
            workOrder.Description,
            workOrder.Notes,
            workOrder.IsActive);
    }

    private static void ValidateTransition(
        WorkOrderStatus currentStatus,
        WorkOrderStatus newStatus)
    {
        if (currentStatus == newStatus)
        {
            throw new ConflictException(
                "The work order is already in the requested status.");
        }

        var isValid = currentStatus switch
        {
            WorkOrderStatus.Open =>
                newStatus is
                    WorkOrderStatus.Assigned or
                    WorkOrderStatus.Cancelled,

            WorkOrderStatus.Assigned =>
                newStatus is
                    WorkOrderStatus.InProgress or
                    WorkOrderStatus.Cancelled,

            WorkOrderStatus.InProgress =>
                newStatus is
                    WorkOrderStatus.OnHold or
                    WorkOrderStatus.Completed or
                    WorkOrderStatus.Cancelled,

            WorkOrderStatus.OnHold =>
                newStatus is
                    WorkOrderStatus.InProgress or
                    WorkOrderStatus.Cancelled,

            WorkOrderStatus.Completed => false,

            WorkOrderStatus.Cancelled => false,

            _ => false
        };

        if (!isValid)
        {
            throw new ConflictException(
                $"Invalid work order status transition: " +
                $"{currentStatus} → {newStatus}.");
        }
    }
}