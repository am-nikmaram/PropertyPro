using AutoMapper;
using PropertyPro.Application.Features.PreventiveMaintenance.Commands.CreatePreventiveMaintenancePlan;
using PropertyPro.Application.Features.PreventiveMaintenance.Commands.UpdatePreventiveMaintenancePlan;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Mappings;

public class PreventiveMaintenanceProfile : Profile
{
    public PreventiveMaintenanceProfile()
    {
        CreateMap<CreatePreventiveMaintenancePlanCommand,
            PreventiveMaintenancePlan>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.PlanNumber,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Asset,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore());

        CreateMap<UpdatePreventiveMaintenancePlanCommand,
            PreventiveMaintenancePlan>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.PlanNumber,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.AssetId,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Asset,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore());
    }
}