namespace PropertyPro.Application.Features.Rooms.DTOs;

public record RoomLookupDto(
    int Id,
    int FloorId,
    int RoomTypeId,
    string RoomNumber,
    string Name);