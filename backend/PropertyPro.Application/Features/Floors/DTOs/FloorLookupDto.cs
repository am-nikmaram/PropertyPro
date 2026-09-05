namespace PropertyPro.Application.Features.Floors.DTOs;

public record FloorLookupDto(
    int Id,
    int BuildingId,
    int FloorNumber,
    string Name);