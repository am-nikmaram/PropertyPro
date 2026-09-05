using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;

public class CreateMaintenanceHandler
    : IRequestHandler<CreateMaintenanceCommand, MaintenanceRecordDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateMaintenanceHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MaintenanceRecordDto> Handle(
        CreateMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        // Check Asset exists and active
        var asset = await _context.Assets
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.AssetId &&
                     x.IsActive,
                cancellationToken);

        if (asset is null)
        {
            throw new NotFoundException(
                nameof(Asset),
                request.AssetId);
        }


        // Check duplicate Maintenance Number
        var exists = await _context.MaintenanceRecords
            .AnyAsync(
                x => x.MaintenanceNumber == request.MaintenanceNumber,
                cancellationToken);

        if (exists)
        {
            throw new ConflictException(
                $"Maintenance number '{request.MaintenanceNumber}' already exists.");
        }


        // Map Command -> Entity
        var maintenance = _mapper.Map<MaintenanceRecord>(request);


        // Normalize business values
        maintenance.MaintenanceNumber =
            maintenance.MaintenanceNumber
                .Trim()
                .ToUpperInvariant();

        maintenance.Description =
            string.IsNullOrWhiteSpace(maintenance.Description)
                ? null
                : maintenance.Description.Trim();

        maintenance.Notes =
            string.IsNullOrWhiteSpace(maintenance.Notes)
                ? null
                : maintenance.Notes.Trim();


        await _context.MaintenanceRecords.AddAsync(
            maintenance,
            cancellationToken);


        await _context.SaveChangesAsync(
            cancellationToken);


        return new MaintenanceRecordDto(
            maintenance.Id,
            maintenance.MaintenanceNumber,
            maintenance.AssetId,
            asset.AssetNumber,
            asset.Name,
            maintenance.MaintenanceType,
            maintenance.Priority,
            maintenance.Status,
            maintenance.ScheduledDate,
            maintenance.StartedDate,
            maintenance.CompletedDate,
            maintenance.Cost,
            maintenance.Description,
            maintenance.Notes,
            maintenance.IsActive);
    }
}