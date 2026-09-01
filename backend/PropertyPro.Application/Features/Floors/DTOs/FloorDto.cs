namespace PropertyPro.Application.Features.Floors.DTOs;

public record FloorDto(
    int Id,
    int BuildingId,
    int FloorNumber,
    string Name,
    string? Description,
    decimal? TotalArea,
    bool IsActive);