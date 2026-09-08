using FluentValidation;

namespace PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderLookup;

public class GetWorkOrderLookupQueryValidator
    : AbstractValidator<GetWorkOrderLookupQuery>
{
    public GetWorkOrderLookupQueryValidator()
    {
        RuleFor(x => x.MaintenanceRecordId)
            .GreaterThan(0)
            .When(x => x.MaintenanceRecordId.HasValue);
    }
}