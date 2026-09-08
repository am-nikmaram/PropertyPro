using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public class UpdateWorkOrderHandler
    : IRequestHandler<UpdateWorkOrderCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorkOrderHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkOrderDto> Handle(
        UpdateWorkOrderCommand request,
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

        if (workOrder.Status is
            Domain.Enums.WorkOrderStatus.Completed or
            Domain.Enums.WorkOrderStatus.Cancelled)
        {
            throw new ConflictException(
                "A completed or cancelled work order cannot be updated.");
        }

        _mapper.Map(request, workOrder);

        workOrder.Description =
            string.IsNullOrWhiteSpace(workOrder.Description)
                ? null
                : workOrder.Description.Trim();

        workOrder.Notes =
            string.IsNullOrWhiteSpace(workOrder.Notes)
                ? null
                : workOrder.Notes.Trim();

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
}