using FluentValidation;

namespace PropertyPro.Application.Features.WorkOrders.Commands.UpdateWorkOrder;

public class UpdateWorkOrderValidator
    : AbstractValidator<UpdateWorkOrderCommand>
{
    public UpdateWorkOrderValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

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