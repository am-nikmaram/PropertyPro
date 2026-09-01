using FluentValidation;

namespace PropertyPro.Application.Features.Floors.Queries.GetFloors;

public class GetFloorsQueryValidator
    : AbstractValidator<GetFloorsQuery>
{
    public GetFloorsQueryValidator()
    {
        RuleFor(x => x.BuildingId)
            .GreaterThan(0);

        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(100);
    }
}