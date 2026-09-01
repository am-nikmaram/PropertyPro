namespace PropertyPro.Application.Features.Buildings.DTOs;

public record BuildingDto(
    int Id,
    int FacilityId,
    string Name,
    string Code,
    string? Description,
    int? YearBuilt,
    decimal? TotalArea,
    bool IsActive);