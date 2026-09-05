using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Maintenance.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.Maintenance.Queries.GetMaintenances;

public record GetMaintenancesQuery(

    int PageNumber = 1,

    int PageSize = 10,

    int? AssetId = null,

    MaintenanceStatus? Status = null,

    MaintenancePriority? Priority = null,

    MaintenanceType? MaintenanceType = null,

    DateTime? FromDate = null,

    DateTime? ToDate = null

) : IRequest<PagedResult<MaintenanceListDto>>;