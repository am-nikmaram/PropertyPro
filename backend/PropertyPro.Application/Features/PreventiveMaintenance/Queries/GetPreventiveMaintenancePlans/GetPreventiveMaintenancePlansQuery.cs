using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlans;

public record GetPreventiveMaintenancePlansQuery(
    int? AssetId = null,
    PreventiveMaintenanceFrequency? Frequency = null,
    MaintenancePriority? Priority = null,
    DateTime? NextDueFrom = null,
    DateTime? NextDueTo = null,
    string? PlanNumber = null,
    string? Name = null,
    bool? IsActive = true,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResult<PreventiveMaintenancePlanListDto>>;