namespace PropertyPro.Application.Features.Buildings.DTOs;

public record BuildingLookupDto(
    int Id,
    int FacilityId,
    string Code,
    string Name);