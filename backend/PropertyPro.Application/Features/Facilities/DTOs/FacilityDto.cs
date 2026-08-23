namespace PropertyPro.Application.Features.Facilities.DTOs;

public record FacilityDto(
    int Id,
    string Name,
    string Code,
    string? Address,
    string? City,
    string? Country,
    string? PostalCode,
    bool IsActive);