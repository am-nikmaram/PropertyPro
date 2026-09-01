namespace PropertyPro.Application.Features.Floors.DTOs;

public record FloorListDto(
    int Id,
    int BuildingId,
    int FloorNumber,
    string Name,
    decimal? TotalArea,
    bool IsActive);