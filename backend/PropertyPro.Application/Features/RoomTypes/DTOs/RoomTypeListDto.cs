namespace PropertyPro.Application.Features.RoomTypes.DTOs;

public record RoomTypeListDto(
    int Id,
    string Name,
    string? Description);