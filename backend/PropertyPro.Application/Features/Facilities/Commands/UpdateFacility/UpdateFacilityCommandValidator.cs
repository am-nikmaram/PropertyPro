using FluentValidation;

namespace PropertyPro.Application.Features.Facilities.Commands.UpdateFacility;

public class UpdateFacilityCommandValidator
    : AbstractValidator<UpdateFacilityCommand>
{
    public UpdateFacilityCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.OrganizationId)
            .GreaterThan(0)
            .WithMessage("Organization is required.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50)
            .Matches("^[A-Za-z0-9_-]+$")
            .WithMessage(
                "Code can contain only letters, numbers, '-' and '_'.");

        RuleFor(x => x.Address)
            .MaximumLength(500);

        RuleFor(x => x.City)
            .MaximumLength(100);

        RuleFor(x => x.Country)
            .MaximumLength(100);

        RuleFor(x => x.PostalCode)
            .MaximumLength(20);
    }
}