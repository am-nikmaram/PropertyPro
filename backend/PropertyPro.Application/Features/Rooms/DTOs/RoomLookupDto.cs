namespace PropertyPro.Application.Features.Rooms.DTOs;

public record RoomLookupDto(
    int Id,
    int UnitId,
    int RoomTypeId,
    string RoomNumber,
    string Name);