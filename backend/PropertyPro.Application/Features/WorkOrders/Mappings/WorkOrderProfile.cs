using AutoMapper;
using PropertyPro.Application.Features.WorkOrders.Commands.CreateWorkOrder;
using PropertyPro.Application.Features.WorkOrders.Commands.UpdateWorkOrder;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.WorkOrders.Mappings;

public class WorkOrderProfile : Profile
{
    public WorkOrderProfile()
    {
        CreateMap<CreateWorkOrderCommand, WorkOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.StartedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.MaintenanceRecord, opt => opt.Ignore());

        CreateMap<UpdateWorkOrderCommand, WorkOrder>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.MaintenanceRecordId, opt => opt.Ignore())
            .ForMember(dest => dest.WorkOrderNumber, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore())
            .ForMember(dest => dest.StartedDate, opt => opt.Ignore())
            .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
            .ForMember(dest => dest.ActualCost, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.Ignore())
            .ForMember(dest => dest.MaintenanceRecord, opt => opt.Ignore());
    }
}