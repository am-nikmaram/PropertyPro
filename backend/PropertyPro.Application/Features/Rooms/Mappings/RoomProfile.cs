using AutoMapper;
using PropertyPro.Application.Features.Rooms.Commands.CreateRoom;
using PropertyPro.Application.Features.Rooms.Commands.UpdateRoom;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Rooms.Mappings;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<CreateRoomCommand, Room>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.CreatedBy,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.UpdatedBy,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Unit,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.RoomType,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Assets,
                opt => opt.Ignore());

        CreateMap<UpdateRoomCommand, Room>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.CreatedAt,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.CreatedBy,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.UpdatedAt,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.UpdatedBy,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Unit,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.RoomType,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Assets,
                opt => opt.Ignore());
    }
}