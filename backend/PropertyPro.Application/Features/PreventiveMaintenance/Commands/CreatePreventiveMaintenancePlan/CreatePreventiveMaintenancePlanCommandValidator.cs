using FluentValidation;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Commands.CreatePreventiveMaintenancePlan;

public class CreatePreventiveMaintenancePlanCommandValidator
    : AbstractValidator<CreatePreventiveMaintenancePlanCommand>
{
    public CreatePreventiveMaintenancePlanCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.AssetId)
            .GreaterThan(0);

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