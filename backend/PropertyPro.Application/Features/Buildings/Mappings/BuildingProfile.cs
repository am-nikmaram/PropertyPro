using AutoMapper;
using PropertyPro.Application.Features.Buildings.Commands.CreateBuilding;
using PropertyPro.Application.Features.Buildings.Commands.UpdateBuilding;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Buildings.Mappings;

public class BuildingProfile : Profile
{
    public BuildingProfile()
    {
        CreateMap<CreateBuildingCommand, Building>()
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
                dest => dest.Facility,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Floors,
                opt => opt.Ignore());

        CreateMap<UpdateBuildingCommand, Building>()
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
                dest => dest.Facility,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Floors,
                opt => opt.Ignore());
    }
}