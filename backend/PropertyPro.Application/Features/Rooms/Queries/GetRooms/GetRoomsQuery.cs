using MediatR;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Rooms.DTOs;

namespace PropertyPro.Application.Features.Rooms.Queries.GetRooms;

public record GetRoomsQuery(
    int FloorId,
    int PageNumber = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<PagedResult<RoomListDto>>;