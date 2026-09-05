using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Maintenance.Commands.UpdateMaintenance;

public class UpdateMaintenanceHandler
    : IRequestHandler<
        UpdateMaintenanceCommand,
        MaintenanceRecordDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;


    public UpdateMaintenanceHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<MaintenanceRecordDto> Handle(
        UpdateMaintenanceCommand request,
        CancellationToken cancellationToken)
    {
        var maintenance =
            await _context.MaintenanceRecords
                .Include(x => x.Asset)
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id &&
                         x.IsActive,
                    cancellationToken);


        if (maintenance is null)
        {
            throw new NotFoundException(
                nameof(MaintenanceRecord),
                request.Id);
        }


        _mapper.Map(
            request,
            maintenance);


        maintenance.Description =
            string.IsNullOrWhiteSpace(
                maintenance.Description)
                ? null
                : maintenance.Description.Trim();


        maintenance.Notes =
            string.IsNullOrWhiteSpace(
                maintenance.Notes)
                ? null
                : maintenance.Notes.Trim();


        await _context.SaveChangesAsync(
            cancellationToken);


        return new MaintenanceRecordDto(
            maintenance.Id,
            maintenance.MaintenanceNumber,
            maintenance.AssetId,
            maintenance.Asset.AssetNumber,
            maintenance.Asset.Name,
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