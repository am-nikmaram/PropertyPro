namespace PropertyPro.Application.Features.PreventiveMaintenance.DTOs;

public record PreventiveMaintenancePlanLookupDto(
    int Id,
    string PlanNumber,
    string Name,
    int AssetId);