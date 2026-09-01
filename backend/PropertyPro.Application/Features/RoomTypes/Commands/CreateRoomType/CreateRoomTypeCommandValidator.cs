using FluentValidation;

namespace PropertyPro.Application.Features.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeCommandValidator
    : AbstractValidator<CreateRoomTypeCommand>
{
    public CreateRoomTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}