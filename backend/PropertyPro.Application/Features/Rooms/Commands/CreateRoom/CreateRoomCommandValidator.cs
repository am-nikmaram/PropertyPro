using FluentValidation;

namespace PropertyPro.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator
    : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.FloorId)
            .GreaterThan(0)
            .WithMessage("Floor is required.");

        RuleFor(x => x.RoomTypeId)
            .GreaterThan(0)
            .WithMessage("Room type is required.");

        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Area)
            .GreaterThan(0)
            .When(x => x.Area.HasValue);

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue);
    }
}