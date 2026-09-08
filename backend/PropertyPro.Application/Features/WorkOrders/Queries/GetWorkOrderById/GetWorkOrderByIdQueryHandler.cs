using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderById;

public class GetWorkOrderByIdQueryHandler
    : IRequestHandler<GetWorkOrderByIdQuery, WorkOrderDto>
{
    private readonly IApplicationDbContext _context;

    public GetWorkOrderByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<WorkOrderDto> Handle(
        GetWorkOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new WorkOrderDto(
                x.Id,
                x.WorkOrderNumber,
                x.MaintenanceRecordId,
                x.MaintenanceRecord.MaintenanceNumber,
                x.MaintenanceRecord.AssetId,
                x.MaintenanceRecord.Asset.AssetNumber,
                x.MaintenanceRecord.Asset.Name,
                x.TechnicianId,
                x.Technician == null
                    ? null
                    : x.Technician.FirstName + " " + x.Technician.LastName,
                x.VendorId,
                x.Vendor == null
                    ? null
                    : x.Vendor.Name,
                x.WorkOrderType,
                x.Priority,
                x.Status,
                x.ScheduledDate,
                x.StartedDate,
                x.CompletedDate,
                x.EstimatedCost,
                x.ActualCost,
                x.Description,
                x.Notes,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (workOrder is null)
        {
            throw new NotFoundException(
                nameof(WorkOrder),
                request.Id);
        }

        return workOrder;
    }
}