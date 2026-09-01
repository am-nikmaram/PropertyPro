using FluentValidation;

namespace PropertyPro.Application.Features.Buildings.Queries.GetBuildings;

public class GetBuildingsQueryValidator
    : AbstractValidator<GetBuildingsQuery>
{
    public GetBuildingsQueryValidator()
    {
        RuleFor(x => x.FacilityId)
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(100);
    }
}