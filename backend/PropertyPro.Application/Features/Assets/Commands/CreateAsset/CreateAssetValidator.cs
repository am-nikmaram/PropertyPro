
using FluentValidation;

namespace PropertyPro.Application.Features.Assets.Commands.CreateAsset;

public class CreateAssetValidator
    : AbstractValidator<CreateAssetCommand>
{
    public CreateAssetValidator()
    {
        RuleFor(x => x.RoomId)
            .GreaterThan(0)
            .WithMessage("Room is required.");

        RuleFor(x => x.AssetCategoryId)
            .GreaterThan(0)
            .WithMessage("Asset category is required.");

        RuleFor(x => x.AssetTag)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.SerialNumber)
            .MaximumLength(100);

        RuleFor(x => x.Manufacturer)
            .MaximumLength(100);

        RuleFor(x => x.Model)
            .MaximumLength(100);

        RuleFor(x => x.PurchaseCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.PurchaseCost.HasValue);

        RuleFor(x => x.WarrantyExpiryDate)
            .GreaterThanOrEqualTo(x => x.PurchaseDate)
            .When(x =>
                x.PurchaseDate.HasValue &&
                x.WarrantyExpiryDate.HasValue)
            .WithMessage(
                "Warranty expiry date cannot be before purchase date.");
    }
}