using AutoMapper;
using PropertyPro.Application.Features.RoomTypes.Commands.CreateRoomType;
using PropertyPro.Application.Features.RoomTypes.Commands.UpdateRoomType;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.RoomTypes.Mappings;

public class RoomTypeProfile : Profile
{
    public RoomTypeProfile()
    {
        CreateMap<CreateRoomTypeCommand, RoomType>()
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
                dest => dest.Rooms,
                opt => opt.Ignore());

        CreateMap<UpdateRoomTypeCommand, RoomType>()
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
                dest => dest.Rooms,
                opt => opt.Ignore());
    }
}