using FluentValidation;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetUpcomingPreventiveMaintenance;

public class GetUpcomingPreventiveMaintenanceQueryValidator
    : AbstractValidator<GetUpcomingPreventiveMaintenanceQuery>
{
    public GetUpcomingPreventiveMaintenanceQueryValidator()
    {
        RuleFor(x => x.Days)
            .InclusiveBetween(1, 365);

        RuleFor(x => x.Take)
            .InclusiveBetween(1, 100);
    }
}