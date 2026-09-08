using PropertyPro.Domain.Enums;

namespace PropertyPro.Application.Features.WorkOrders.DTOs;

public record WorkOrderListDto(
    int Id,
    string WorkOrderNumber,
    int MaintenanceRecordId,
    string MaintenanceNumber,
    string AssetNumber,
    string AssetName,
    int? TechnicianId,
    string? TechnicianName,
    int? VendorId,
    string? VendorName,
    WorkOrderType WorkOrderType,
    WorkOrderPriority Priority,
    WorkOrderStatus Status,
    DateTime ScheduledDate,
    decimal? EstimatedCost,
    decimal? ActualCost,
    bool IsActive);