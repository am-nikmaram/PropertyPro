using MediatR;
using PropertyPro.Application.Features.RoomTypes.DTOs;

namespace PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypes;

public record GetRoomTypesQuery
    : IRequest<IReadOnlyList<RoomTypeListDto>>;