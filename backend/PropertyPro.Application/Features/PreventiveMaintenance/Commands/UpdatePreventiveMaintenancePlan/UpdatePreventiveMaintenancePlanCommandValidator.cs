using FluentValidation;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.UpdatePreventiveMaintenancePlan;

public class UpdatePreventiveMaintenancePlanCommandValidator
    : AbstractValidator<UpdatePreventiveMaintenancePlanCommand>
{
    public UpdatePreventiveMaintenancePlanCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Frequency)
            .IsInEnum();

        RuleFor(x => x.NextDueDate)
            .NotEmpty();

        RuleFor(x => x.Priority)
            .IsInEnum();

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}