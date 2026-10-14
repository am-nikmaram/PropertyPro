using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Models.FileAttachments;
using PropertyPro.Application.Features.FileAttachments.Commands
    .UploadFileAttachment;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/file-attachments")]
[Authorize]
public class FileAttachmentsController : ControllerBase
{
    private readonly ISender _sender;

    public FileAttachmentsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(26 * 1024 * 1024)]
    [RequestFormLimits(
        MultipartBodyLengthLimit = 26 * 1024 * 1024)]
    public async Task<IActionResult> Upload(
        [FromForm] UploadFileAttachmentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.File is null || request.File.Length == 0)
        {
            return BadRequest(new
            {
                message = "Please select a non-empty file."
            });
        }

        await using var stream = request.File.OpenReadStream();

        var command = new UploadFileAttachmentCommand(
            Content: stream,
            OriginalFileName: request.File.FileName,
            ContentType: string.IsNullOrWhiteSpace(
                request.File.ContentType)
                    ? "application/octet-stream"
                    : request.File.ContentType,
            SizeInBytes: request.File.Length,
            AssetId: request.AssetId,
            MaintenanceRecordId: request.MaintenanceRecordId,
            WorkOrderId: request.WorkOrderId);

        var result = await _sender.Send(
            command,
            cancellationToken);

        return Ok(result);
    }
}