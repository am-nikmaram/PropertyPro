using AutoMapper;
using PropertyPro.Application.Features.Units.Commands.CreateUnit;
using PropertyPro.Application.Features.Units.Commands.UpdateUnit;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Units.Mappings;

public class UnitProfile : Profile
{
    public UnitProfile()
    {
        CreateMap<CreateUnitCommand, Unit>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Floor,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Rooms,
                opt => opt.Ignore());

        CreateMap<UpdateUnitCommand, Unit>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.FloorId,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Floor,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Rooms,
                opt => opt.Ignore());
    }
}