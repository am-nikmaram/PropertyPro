using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.CreatePreventiveMaintenancePlan;

public class CreatePreventiveMaintenancePlanCommandHandler
    : IRequestHandler<
        CreatePreventiveMaintenancePlanCommand,
        PreventiveMaintenancePlanDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreatePreventiveMaintenancePlanCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PreventiveMaintenancePlanDto> Handle(
        CreatePreventiveMaintenancePlanCommand request,
        CancellationToken cancellationToken)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(
                x => x.Id == request.AssetId,
                cancellationToken);

        if (asset is null)
        {
            throw new NotFoundException(
                $"Asset '{request.AssetId}' was not found.");
        }

        if (!asset.IsActive)
        {
            throw new ConflictException(
                "A preventive maintenance plan cannot be created for an inactive asset.");
        }

        var plan = _mapper.Map<PreventiveMaintenancePlan>(
            request);

        plan.PlanNumber = GeneratePlanNumber();

        _context.PreventiveMaintenancePlans.Add(plan);

        await _context.SaveChangesAsync(
            cancellationToken);

        return new PreventiveMaintenancePlanDto(
            plan.Id,
            plan.PlanNumber,
            plan.Name,
            asset.Id,
            asset.AssetNumber,
            asset.Name,
            plan.Frequency,
            plan.NextDueDate,
            plan.Priority,
            plan.Description,
            plan.AutoGenerateWorkOrder,
            plan.IsActive);
    }

    private static string GeneratePlanNumber()
    {
        return $"PM-{Guid.NewGuid():N}"[..11]
            .ToUpperInvariant();
    }
}