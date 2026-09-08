using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Entities;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderHandler
    : IRequestHandler<CreateWorkOrderCommand, WorkOrderDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateWorkOrderHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<WorkOrderDto> Handle(
        CreateWorkOrderCommand request,
        CancellationToken cancellationToken)
    {
        var maintenance = await _context.MaintenanceRecords
            .AsNoTracking()
            .Where(x =>
                x.Id == request.MaintenanceRecordId &&
                x.IsActive)
            .Select(x => new
            {
                x.Id,
                x.MaintenanceNumber,
                x.AssetId,
                AssetNumber = x.Asset.AssetNumber,
                AssetName = x.Asset.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (maintenance is null)
        {
            throw new NotFoundException(
                nameof(MaintenanceRecord),
                request.MaintenanceRecordId);
        }

        string? technicianName = null;

        if (request.TechnicianId.HasValue)
        {
            technicianName = await _context.Technicians
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.TechnicianId.Value &&
                    x.IsActive)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstOrDefaultAsync(cancellationToken);

            if (technicianName is null)
            {
                throw new NotFoundException(
                    nameof(Technician),
                    request.TechnicianId.Value);
            }
        }

        string? vendorName = null;

        if (request.VendorId.HasValue)
        {
            vendorName = await _context.Vendors
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.VendorId.Value &&
                    x.IsActive)
                .Select(x => x.Name)
                .FirstOrDefaultAsync(cancellationToken);

            if (vendorName is null)
            {
                throw new NotFoundException(
                    nameof(Vendor),
                    request.VendorId.Value);
            }
        }

        var workOrderNumber =
            await GenerateWorkOrderNumberAsync(cancellationToken);

        var workOrder = _mapper.Map<WorkOrder>(request);

        workOrder.WorkOrderNumber = workOrderNumber;
        workOrder.Status = WorkOrderStatus.Open;
        workOrder.IsActive = true;

        workOrder.Description =
            string.IsNullOrWhiteSpace(workOrder.Description)
                ? null
                : workOrder.Description.Trim();

        workOrder.Notes =
            string.IsNullOrWhiteSpace(workOrder.Notes)
                ? null
                : workOrder.Notes.Trim();

        _context.WorkOrders.Add(workOrder);

        await _context.SaveChangesAsync(cancellationToken);

        return new WorkOrderDto(
            workOrder.Id,
            workOrder.WorkOrderNumber,
            workOrder.MaintenanceRecordId,
            maintenance.MaintenanceNumber,
            maintenance.AssetId,
            maintenance.AssetNumber,
            maintenance.AssetName,
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

    private async Task<string> GenerateWorkOrderNumberAsync(
        CancellationToken cancellationToken)
    {
        var year = DateTime.UtcNow.Year;

        var prefix = $"WO-{year}-";

        var lastNumber = await _context.WorkOrders
            .AsNoTracking()
            .Where(x => x.WorkOrderNumber.StartsWith(prefix))
            .OrderByDescending(x => x.WorkOrderNumber)
            .Select(x => x.WorkOrderNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var nextNumber = 1;

        if (lastNumber is not null &&
            int.TryParse(
                lastNumber[prefix.Length..],
                out var currentNumber))
        {
            nextNumber = currentNumber + 1;
        }

        return $"{prefix}{nextNumber:D4}";
    }
}