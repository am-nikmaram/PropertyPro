namespace PropertyPro.Application.Features.Units.DTOs;

public record UnitLookupDto(
    int Id,
    int FloorId,
    int BuildingId,
    string Code,
    string Name);