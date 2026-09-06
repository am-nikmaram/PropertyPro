namespace PropertyPro.Application.Features.Rooms.DTOs;

public record RoomDto(
    int Id,
    int UnitId,
    int RoomTypeId,
    string RoomTypeName,
    string RoomNumber,
    string Name,
    string? Description,
    decimal? Area,
    int? Capacity,
    bool IsActive);