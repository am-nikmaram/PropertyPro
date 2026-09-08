using FluentValidation;

namespace PropertyPro.Application.Features.WorkOrders.Commands.CreateWorkOrder;

public class CreateWorkOrderValidator
    : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderValidator()
    {
        RuleFor(x => x.MaintenanceRecordId)
            .GreaterThan(0);
        RuleFor(x => x.TechnicianId)
            .GreaterThan(0)
            .When(x => x.TechnicianId.HasValue);

        RuleFor(x => x.VendorId)
            .GreaterThan(0)
            .When(x => x.VendorId.HasValue);

        RuleFor(x => x.WorkOrderType)
            .IsInEnum();

        RuleFor(x => x.Priority)
            .IsInEnum();

        RuleFor(x => x.ScheduledDate)
            .NotEmpty();

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EstimatedCost.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Notes)
            .MaximumLength(2000);
    }
}