using FluentValidation;
using PropertyPro.Application.Common.Security;

namespace PropertyPro.Application.Features.Users.Commands.AssignRole;

public class AssignRoleCommandValidator
    : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Role)
            .NotEmpty()
            .Must(role =>
                IdentityRoles.All.Contains(
                    role,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage("The specified role is invalid.");
    }
}