using MediatR;
using PropertyPro.Application.Features.FileAttachments.DTOs;

namespace PropertyPro.Application.Features.FileAttachments.Commands
    .UploadFileAttachment;

public record UploadFileAttachmentCommand(
    Stream Content,
    string OriginalFileName,
    string ContentType,
    long SizeInBytes,
    int? AssetId,
    int? MaintenanceRecordId,
    int? WorkOrderId
) : IRequest<FileAttachmentDto>;