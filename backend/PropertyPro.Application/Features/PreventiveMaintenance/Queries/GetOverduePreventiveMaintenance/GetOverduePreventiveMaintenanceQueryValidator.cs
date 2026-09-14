using FluentValidation;

namespace PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetOverduePreventiveMaintenance;

public class GetOverduePreventiveMaintenanceQueryValidator
    : AbstractValidator<GetOverduePreventiveMaintenanceQuery>
{
    public GetOverduePreventiveMaintenanceQueryValidator()
    {
        RuleFor(x => x.Take)
            .InclusiveBetween(1, 100);
    }
}