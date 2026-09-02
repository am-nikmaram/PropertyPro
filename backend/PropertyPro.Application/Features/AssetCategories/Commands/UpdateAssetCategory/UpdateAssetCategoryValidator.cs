
using FluentValidation;

namespace PropertyPro.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public class UpdateAssetCategoryValidator
    : AbstractValidator<UpdateAssetCategoryCommand>
{
    public UpdateAssetCategoryValidator()
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
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}

