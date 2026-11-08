namespace PropertyPro.Application.Features.Rooms.DTOs;

public record RoomLookupDto(
    int Id,
    int UnitId,
    int RoomTypeId,
    int BuildingId,
    int FacilityId,
    string RoomNumber,
    string Name);
