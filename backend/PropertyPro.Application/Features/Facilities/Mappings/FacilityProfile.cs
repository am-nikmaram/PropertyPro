using AutoMapper;
using PropertyPro.Application.Features.Facilities.Commands.CreateFacility;
using PropertyPro.Application.Features.Facilities.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Facilities.Mappings;

public class FacilityProfile : Profile
{
    public FacilityProfile()
    {
        // Command → Entity
        CreateMap<CreateFacilityCommand, Facility>();

        // Entity → DTO
        CreateMap<Facility, FacilityDto>();
    }
}