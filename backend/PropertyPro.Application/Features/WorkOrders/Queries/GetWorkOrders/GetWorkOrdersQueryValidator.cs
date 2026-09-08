using FluentValidation;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrders;

public class GetWorkOrdersQueryValidator
    : AbstractValidator<GetWorkOrdersQuery>
{
    public GetWorkOrdersQueryValidator()
    {
        RuleFor(x => x.MaintenanceRecordId)
            .GreaterThan(0)
            .When(x => x.MaintenanceRecordId.HasValue);

        RuleFor(x => x.AssetId)
            .GreaterThan(0)
            .When(x => x.AssetId.HasValue);

        RuleFor(x => x.TechnicianId)
            .GreaterThan(0)
            .When(x => x.TechnicianId.HasValue);

        RuleFor(x => x.VendorId)
            .GreaterThan(0)
            .When(x => x.VendorId.HasValue);

        RuleFor(x => x.ScheduledTo)
            .GreaterThanOrEqualTo(x => x.ScheduledFrom)
            .When(x =>
                x.ScheduledFrom.HasValue &&
                x.ScheduledTo.HasValue);

        RuleFor(x => x.WorkOrderNumber)
            .MaximumLength(30);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}