namespace PropertyPro.Application.Features.WorkOrders.DTOs;

public record WorkOrderLookupDto(
    int Id,
    string WorkOrderNumber,
    int MaintenanceRecordId);