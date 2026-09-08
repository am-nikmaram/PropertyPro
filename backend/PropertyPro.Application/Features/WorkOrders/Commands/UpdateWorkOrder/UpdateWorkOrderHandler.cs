using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Entities;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public class UpdateWorkOrderCommandHandler
    : IRequestHandler<UpdateWorkOrderCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateWorkOrderCommandHandler(
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

        if (workOrder.Status is WorkOrderStatus.Completed
            or WorkOrderStatus.Cancelled)
        {
            throw new ConflictException(
                "A completed or cancelled work order cannot be updated.");
        }

        string? technicianName = null;

        if (request.TechnicianId.HasValue)
        {
            var technician = await _context.Technicians
                .Where(x =>
                    x.Id == request.TechnicianId.Value &&
                    x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    Name = x.FirstName + " " + x.LastName
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (technician is null)
            {
                throw new NotFoundException(
                    nameof(Technician),
                    request.TechnicianId.Value);
            }

            technicianName = technician.Name;
        }

        string? vendorName = null;

        if (request.VendorId.HasValue)
        {
            var vendor = await _context.Vendors
                .Where(x =>
                    x.Id == request.VendorId.Value &&
                    x.IsActive)
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (vendor is null)
            {
                throw new NotFoundException(
                    nameof(Vendor),
                    request.VendorId.Value);
            }

            vendorName = vendor.Name;
        }

        _mapper.Map(request, workOrder);

        workOrder.Description = request.Description?.Trim();
        workOrder.Notes = request.Notes?.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return new WorkOrderDto(
            workOrder.Id,
            workOrder.WorkOrderNumber,
            workOrder.MaintenanceRecordId,
            workOrder.MaintenanceRecord.MaintenanceNumber,
            workOrder.MaintenanceRecord.AssetId,
            workOrder.MaintenanceRecord.Asset.AssetNumber,
            workOrder.MaintenanceRecord.Asset.Name,
            workOrder.TechnicianId,
            technicianName,
            workOrder.VendorId,
            vendorName,
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