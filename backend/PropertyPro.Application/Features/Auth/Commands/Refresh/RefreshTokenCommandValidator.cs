using FluentValidation;

namespace PropertyPro.Application.Features.Auth.Commands.Refresh;

public class RefreshTokenCommandValidator
: AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken)
        .NotEmpty()
        .MaximumLength(500);
    }
}
