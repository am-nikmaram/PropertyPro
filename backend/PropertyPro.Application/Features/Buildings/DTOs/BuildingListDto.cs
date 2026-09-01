namespace PropertyPro.Application.Features.Buildings.DTOs;

public record BuildingListDto(
    int Id,
    int FacilityId,
    string Name,
    string Code,
    int? YearBuilt,
    decimal? TotalArea,
    bool IsActive);