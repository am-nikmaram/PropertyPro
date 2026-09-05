using AutoMapper;
using PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;
using PropertyPro.Application.Features.Maintenance.Commands.UpdateMaintenance;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Maintenance.Mappings;

public class MaintenanceProfile : Profile
{
    public MaintenanceProfile()
    {
        CreateMap<CreateMaintenanceCommand, MaintenanceRecord>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Asset,
                opt => opt.Ignore());

        CreateMap<UpdateMaintenanceCommand, MaintenanceRecord>()
            .ForMember(
                dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.MaintenanceNumber,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.Asset,
                opt => opt.Ignore())
            .ForMember(
                dest => dest.IsActive,
                opt => opt.Ignore());
    }
}