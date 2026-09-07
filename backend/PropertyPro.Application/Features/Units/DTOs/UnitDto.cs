namespace PropertyPro.Application.Features.Units.DTOs;

public record UnitDto(
    int Id,
    int FloorId,
    int BuildingId,
    string Code,
    string Name,
    string? Description,
    decimal? Area,
    bool IsActive);