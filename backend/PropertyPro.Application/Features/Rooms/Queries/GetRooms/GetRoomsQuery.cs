using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRooms;

public record GetRoomsQuery(
    int PageNumber = 1,
    int PageSize = 10,
    int? UnitId = null,
    int? FloorId = null,
    int? BuildingId = null,
    int? RoomTypeId = null,
    string? RoomNumber = null,
    string? Name = null,
    bool? IsActive = true
) : IRequest<PagedResult<RoomListDto>>;