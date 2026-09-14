using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanById;

public class GetPreventiveMaintenancePlanByIdQueryHandler
    : IRequestHandler<
        GetPreventiveMaintenancePlanByIdQuery,
        PreventiveMaintenancePlanDto>
{
    private readonly IApplicationDbContext _context;

    public GetPreventiveMaintenancePlanByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PreventiveMaintenancePlanDto> Handle(
        GetPreventiveMaintenancePlanByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _context.PreventiveMaintenancePlans
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new PreventiveMaintenancePlanDto(
                x.Id,
                x.PlanNumber,
                x.Name,
                x.AssetId,
                x.Asset.AssetNumber,
                x.Asset.Name,
                x.Frequency,
                x.NextDueDate,
                x.Priority,
                x.Description,
                x.AutoGenerateWorkOrder,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                $"Preventive maintenance plan '{request.Id}' was not found.");
        }

        return result;
    }
}