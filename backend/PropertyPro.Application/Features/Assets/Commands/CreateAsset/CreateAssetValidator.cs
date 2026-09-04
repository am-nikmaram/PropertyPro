
using FluentValidation;

namespace PropertyPro.Application.Features.Assets.Commands.CreateAsset;

public class CreateAssetValidator : AbstractValidator<CreateAssetCommand>
{
    public CreateAssetValidator()
    {
        RuleFor(x => x.AssetNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.AssetCategoryId)
            .GreaterThan(0);

        RuleFor(x => x.RoomId)
            .GreaterThan(0);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .MaximumLength(100);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100);

        RuleFor(x => x.PurchaseCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PurchaseCost.HasValue);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.WarrantyEndDate)
            .GreaterThanOrEqualTo(x => x.WarrantyStartDate)
            .When(x =>
                x.WarrantyStartDate.HasValue &&
                x.WarrantyEndDate.HasValue)
            .WithMessage(
                "Warranty end date must be greater than or equal to warranty start date.");

        RuleFor(x => x.WarrantyStartDate)
            .LessThanOrEqualTo(x => x.WarrantyEndDate)
            .When(x =>
                x.WarrantyStartDate.HasValue &&
                x.WarrantyEndDate.HasValue)
            .WithMessage(
                "Warranty start date must be less than or equal to warranty end date.");
    }
}
