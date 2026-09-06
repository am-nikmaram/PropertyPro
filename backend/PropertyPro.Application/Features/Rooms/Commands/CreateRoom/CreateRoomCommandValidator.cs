
using FluentValidation;

namespace PropertyPro.Application.Features.Rooms.Commands.CreateRoom;

public class CreateRoomValidator
    : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomValidator()
    {
        RuleFor(x => x.UnitId)
            .GreaterThan(0)
            .WithMessage("Unit is required.");

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
            .When(x => x.Area.HasValue)
            .WithMessage("Area must be greater than zero.");

        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .When(x => x.Capacity.HasValue)
            .WithMessage("Capacity must be greater than zero.");
    }
}

