using FluentValidation;

namespace PropertyPro.Application.Features.FileAttachments.Commands
    .UploadFileAttachment;

public class UploadFileAttachmentCommandValidator
    : AbstractValidator<UploadFileAttachmentCommand>
{
    private const long MaxFileSize = 25 * 1024 * 1024;

    public UploadFileAttachmentCommandValidator()
    {
        RuleFor(x => x.Content)
            .NotNull();

        RuleFor(x => x.OriginalFileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.ContentType)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.SizeInBytes)
            .GreaterThan(0)
            .LessThanOrEqualTo(MaxFileSize);

        RuleFor(x => x)
            .Must(HaveExactlyOneParent)
            .WithMessage(
                "An attachment must belong to exactly one asset, " +
                "maintenance record, or work order.");
    }

    private static bool HaveExactlyOneParent(
        UploadFileAttachmentCommand command)
    {
        var parentCount = new[]
        {
            command.AssetId,
            command.MaintenanceRecordId,
            command.WorkOrderId
        }.Count(id => id.HasValue);

        return parentCount == 1;
    }
}