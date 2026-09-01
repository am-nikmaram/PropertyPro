namespace PropertyPro.Application.Features.RoomTypes.DTOs;

public record RoomTypeDto(
    int Id,
    string Name,
    string? Description,
    bool IsActive);