using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.DTOs;

public record WorkOrderDto(
    int Id,
    string WorkOrderNumber,
    int MaintenanceRecordId,
    string MaintenanceNumber,
    int AssetId,
    string AssetNumber,
    string AssetName,
    WorkOrderType WorkOrderType,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    DateTime ScheduledDate,
    DateTime? StartedDate,
    DateTime? CompletedDate,
    decimal? EstimatedCost,
    decimal? ActualCost,
    string? Description,
    string? Notes,
    bool IsActive);