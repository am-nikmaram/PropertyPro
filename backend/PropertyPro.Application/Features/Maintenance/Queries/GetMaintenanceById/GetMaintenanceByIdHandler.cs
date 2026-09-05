using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Maintenance.Queries.GetMaintenanceById;

public class GetMaintenanceByIdHandler
    : IRequestHandler<
        GetMaintenanceByIdQuery,
        MaintenanceRecordDto>
{
    private readonly IApplicationDbContext _context;

    public GetMaintenanceByIdHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<MaintenanceRecordDto> Handle(
        GetMaintenanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var maintenance =
            await _context.MaintenanceRecords
                .AsNoTracking()
                .Where(x =>
                    x.Id == request.Id &&
                    x.IsActive)
                .Select(x => new MaintenanceRecordDto(
                    x.Id,
                    x.MaintenanceNumber,
                    x.AssetId,
                    x.Asset.AssetNumber,
                    x.Asset.Name,
                    x.MaintenanceType,
                    x.Priority,
                    x.Status,
                    x.ScheduledDate,
                    x.StartedDate,
                    x.CompletedDate,
                    x.Cost,
                    x.Description,
                    x.Notes,
                    x.IsActive
                ))
                .FirstOrDefaultAsync(
                    cancellationToken);


        if (maintenance is null)
        {
            throw new NotFoundException(
                nameof(MaintenanceRecord),
                request.Id);
        }


        return maintenance;
    }
}