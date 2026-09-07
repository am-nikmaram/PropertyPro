using FluentValidation;

namespace PropertyPro.Application.Features.Units.Commands.UpdateUnit;

public class UpdateUnitValidator
    : AbstractValidator<UpdateUnitCommand>
{
    public UpdateUnitValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage(
                "Code can contain only letters, numbers, hyphens and underscores.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Area)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Area.HasValue);
    }
}