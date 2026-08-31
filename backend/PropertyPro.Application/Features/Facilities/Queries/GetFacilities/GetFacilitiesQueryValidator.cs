using FluentValidation;

namespace PropertyPro.Application.Features.Facilities.Queries.GetFacilities;

public class GetFacilitiesQueryValidator
    : AbstractValidator<GetFacilitiesQuery>
{
    public GetFacilitiesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.SortBy)
            .Must(BeValidSortField)
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage(
                "SortBy must be one of: name, code, city.");
    }

    private static bool BeValidSortField(string? sortBy)
    {
        return sortBy?.ToLower() switch
        {
            "name" => true,
            "code" => true,
            "city" => true,
            _ => false
        };
    }
}