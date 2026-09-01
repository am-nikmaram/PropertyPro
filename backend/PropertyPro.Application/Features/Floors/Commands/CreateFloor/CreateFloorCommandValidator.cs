using FluentValidation;

namespace PropertyPro.Application.Features.Floors.Commands.CreateFloor;

public class CreateFloorCommandValidator
    : AbstractValidator<CreateFloorCommand>
{
    public CreateFloorCommandValidator()
    {
        RuleFor(x => x.BuildingId)
            .GreaterThan(0)
            .WithMessage("Building is required.");

        RuleFor(x => x.FloorNumber)
            .InclusiveBetween(-10, 200)
            .WithMessage(
                "Floor number must be between -10 and 200.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.TotalArea)
            .GreaterThan(0)
            .When(x => x.TotalArea.HasValue)
            .WithMessage(
                "Total area must be greater than zero.");
    }
}