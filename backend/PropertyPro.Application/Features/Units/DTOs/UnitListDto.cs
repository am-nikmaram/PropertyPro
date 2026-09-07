namespace PropertyPro.Application.Features.Units.DTOs;

public record UnitListDto(
    int Id,
    int FloorId,
    int BuildingId,
    string Code,
    string Name,
    decimal? Area,
    bool IsActive);