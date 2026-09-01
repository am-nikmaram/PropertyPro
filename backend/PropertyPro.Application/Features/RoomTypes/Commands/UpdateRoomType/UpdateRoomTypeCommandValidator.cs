using FluentValidation;

namespace PropertyPro.Application.Features.RoomTypes.Commands.UpdateRoomType;

public class UpdateRoomTypeCommandValidator
    : AbstractValidator<UpdateRoomTypeCommand>
{
    public UpdateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}