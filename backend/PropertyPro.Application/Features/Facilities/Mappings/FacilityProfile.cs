using AutoMapper;
using PropertyPro.Application.Features.Facilities.Commands.CreateFacility;
using PropertyPro.Application.Features.Facilities.Commands.UpdateFacility;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateFacilityCommand, Facility>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedAt,
                options => options.Ignore())
            .ForMember(
                destination => destination.UpdatedAt,
                options => options.Ignore())
            .ForMember(
                destination => destination.IsActive,
                options => options.Ignore());

        CreateMap<UpdateFacilityCommand, Facility>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.CreatedAt,
                options => options.Ignore())
            .ForMember(
                destination => destination.UpdatedAt,
                options => options.Ignore())
            .ForMember(
                destination => destination.IsActive,
                options => options.Ignore())
            .ForMember(
                destination => destination.Organization,
                options => options.Ignore())
            .ForMember(
                destination => destination.Buildings,
                options => options.Ignore());
    }
}