namespace PropertyPro.Application.Features.Rooms.DTOs;

public record RoomListDto(
    int Id,
    int UnitId,
    int RoomTypeId,
    string RoomTypeName,
    string RoomNumber,
    string Name,
    decimal? Area,
    int? Capacity,
    bool IsActive);