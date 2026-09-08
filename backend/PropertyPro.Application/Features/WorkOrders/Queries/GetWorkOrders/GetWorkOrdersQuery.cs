using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.WorkOrders.DTOs;
using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrders;

public record GetWorkOrdersQuery(
    int? MaintenanceRecordId,
    int? AssetId,
    int? TechnicianId,
    int? VendorId,
    WorkOrderType? WorkOrderType,
    WorkOrderPriority? Priority,
    WorkOrderStatus? Status,
    DateTime? ScheduledFrom,
    DateTime? ScheduledTo,
    string? WorkOrderNumber,
    bool? IsActive,
    int PageNumber = 1,
    int PageSize = 20
) : IRequest<PagedResult<WorkOrderListDto>>;