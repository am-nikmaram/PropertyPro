using AutoMapper;
using PropertyPro.Application.Features.Floors.Commands.CreateFloor;
using PropertyPro.Application.Features.Floors.Commands.UpdateFloor;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Floors.Mappings;

public class FloorProfile : Profile
{
    public FloorProfile()
    {
        CreateMap<CreateFloorCommand, Floor>()
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
                dest => dest.Building,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Units,
                opt => opt.Ignore());

        CreateMap<UpdateFloorCommand, Floor>()
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
                dest => dest.Building,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Units,
                opt => opt.Ignore());
    }
}