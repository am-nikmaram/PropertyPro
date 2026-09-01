using FluentValidation;

namespace PropertyPro.Application.Features.Buildings.Commands.CreateBuilding;

public class CreateBuildingCommandValidator
    : AbstractValidator<CreateBuildingCommand>
{
    public CreateBuildingCommandValidator()
    {
        RuleFor(x => x.FacilityId)
            .GreaterThan(0)
            .WithMessage("Facility is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
        RuleFor(x => x.YearBuilt)
    .InclusiveBetween(1800, DateTime.UtcNow.Year)
    .When(x => x.YearBuilt.HasValue)
    .WithMessage("Year built must be a valid year.");

        RuleFor(x => x.TotalArea)
            .GreaterThan(0)
            .When(x => x.TotalArea.HasValue)
            .WithMessage("Total area must be greater than zero.");

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage(
                "Code can contain only letters, numbers, '-' and '_'.");

        RuleFor(x => x.Description)
            .MaximumLength(1000);
    }
}