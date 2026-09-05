using FluentValidation;

namespace PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;

public class CreateMaintenanceValidator
    : AbstractValidator<CreateMaintenanceCommand>
{
    public CreateMaintenanceValidator()
    {
        RuleFor(x => x.MaintenanceNumber)
            .NotEmpty()
            .MaximumLength(30);

        RuleFor(x => x.AssetId)
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
            .When(x => x.StartedDate.HasValue)
            .WithMessage(
                "Started date must be greater than or equal to scheduled date.");

        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(x => x.StartedDate)
            .When(x => x.CompletedDate.HasValue && x.StartedDate.HasValue)
            .WithMessage(
                "Completed date must be greater than or equal to started date.");

        RuleFor(x => x.CompletedDate)
            .GreaterThanOrEqualTo(x => x.ScheduledDate)
            .When(x => x.CompletedDate.HasValue)
            .WithMessage(
                "Completed date must be greater than or equal to scheduled date.");
    }
}