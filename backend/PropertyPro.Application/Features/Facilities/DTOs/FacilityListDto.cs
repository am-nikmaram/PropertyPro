namespace PropertyPro.Application.Features.Facilities.DTOs;

public record FacilityListDto(
    int Id,
    string Name,
    string Code,
    string? City,
    string? Country,
    bool IsActive);