using FluentValidation;

namespace PropertyPro.Application.Features.Maintenance.Commands.UpdateMaintenance;

public class UpdateMaintenanceValidator
    : AbstractValidator<UpdateMaintenanceCommand>
{
    public UpdateMaintenanceValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.ScheduledDate)
            .NotEmpty();

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Cost.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Notes)
            .MaximumLength(2000);


        RuleFor(x => x.StartedDate)
            .GreaterThanOrEqualTo(x => x.ScheduledDate)
            .When(x => x.StartedDate.HasValue);


        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(x => x.ScheduledDate)
            .When(x => x.CompletedDate.HasValue);


        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(x => x.StartedDate)
            .When(x =>
                x.CompletedDate.HasValue &&
                x.StartedDate.HasValue);
    }
}