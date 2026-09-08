using FluentValidation;

namespace PropertyPro.Application.Features.WorkOrders.Commands.ChangeWorkOrderStatus;

public class ChangeWorkOrderStatusValidator
    : AbstractValidator<ChangeWorkOrderStatusCommand>
{
    public ChangeWorkOrderStatusValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Status)
            .IsInEnum();
    }
}