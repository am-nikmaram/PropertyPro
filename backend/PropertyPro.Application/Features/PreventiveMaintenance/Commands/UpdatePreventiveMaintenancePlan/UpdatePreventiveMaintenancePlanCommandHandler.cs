using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.UpdatePreventiveMaintenancePlan;

public class UpdatePreventiveMaintenancePlanCommandHandler
    : IRequestHandler<
        UpdatePreventiveMaintenancePlanCommand,
        PreventiveMaintenancePlanDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdatePreventiveMaintenancePlanCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PreventiveMaintenancePlanDto> Handle(
        UpdatePreventiveMaintenancePlanCommand request,
        CancellationToken cancellationToken)
    {
        var plan = await _context.PreventiveMaintenancePlans
            .Include(x => x.Asset)
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (plan is null || !plan.IsActive)
        {
            throw new NotFoundException(
                $"Preventive maintenance plan '{request.Id}' was not found.");
        }

        if (!plan.Asset.IsActive)
        {
            throw new ConflictException(
                "The preventive maintenance plan cannot be updated because its asset is inactive.");
        }

        _mapper.Map(request, plan);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new PreventiveMaintenancePlanDto(
            plan.Id,
            plan.PlanNumber,
            plan.Name,
            plan.AssetId,
            plan.Asset.AssetNumber,
            plan.Asset.Name,
            plan.Frequency,
            plan.NextDueDate,
            plan.Priority,
            plan.Description,
            plan.AutoGenerateWorkOrder,
            plan.IsActive);
    }
}