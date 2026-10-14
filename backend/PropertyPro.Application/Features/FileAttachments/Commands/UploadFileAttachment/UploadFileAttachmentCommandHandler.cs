using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.FileAttachments.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.FileAttachments.Commands
    .UploadFileAttachment;

public class UploadFileAttachmentCommandHandler
    : IRequestHandler<UploadFileAttachmentCommand, FileAttachmentDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IFileStorageService _fileStorage;
    private readonly ILogger<UploadFileAttachmentCommandHandler> _logger;

    public UploadFileAttachmentCommandHandler(
        IApplicationDbContext context,
        IFileStorageService fileStorage,
        ILogger<UploadFileAttachmentCommandHandler> logger)
    {
        _context = context;
        _fileStorage = fileStorage;
        _logger = logger;
    }

    public async Task<FileAttachmentDto> Handle(
        UploadFileAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        await ValidateParentAsync(request, cancellationToken);

        var storageKey = $"attachments/{Guid.NewGuid():N}";

        await _fileStorage.UploadAsync(
            request.Content,
            storageKey,
            request.ContentType,
            cancellationToken);

        try
        {
            var attachment = new FileAttachment
            {
                OriginalFileName = Path.GetFileName(
                    request.OriginalFileName),
                StorageKey = storageKey,
                ContentType = request.ContentType,
                SizeInBytes = request.SizeInBytes,
                AssetId = request.AssetId,
                MaintenanceRecordId = request.MaintenanceRecordId,
                WorkOrderId = request.WorkOrderId
            };

            _context.FileAttachments.Add(attachment);

            await _context.SaveChangesAsync(cancellationToken);

            return new FileAttachmentDto
            {
                Id = attachment.Id,
                OriginalFileName = attachment.OriginalFileName,
                ContentType = attachment.ContentType,
                SizeInBytes = attachment.SizeInBytes,
                AssetId = attachment.AssetId,
                MaintenanceRecordId = attachment.MaintenanceRecordId,
                WorkOrderId = attachment.WorkOrderId,
                CreatedAt = attachment.CreatedAt
            };
        }
        catch
        {
            try
            {
                await _fileStorage.DeleteAsync(
                    storageKey,
                    cancellationToken);
            }
            catch (Exception cleanupException)
            {
                _logger.LogError(
                    cleanupException,
                    "Failed to clean up uploaded object {StorageKey}.",
                    storageKey);
            }

            throw;
        }
    }

    private async Task ValidateParentAsync(
        UploadFileAttachmentCommand request,
        CancellationToken cancellationToken)
    {
        if (request.AssetId.HasValue)
        {
            var exists = await _context.Assets.AnyAsync(
                x => x.Id == request.AssetId.Value && x.IsActive,
                cancellationToken);

            if (!exists)
                throw new NotFoundException("Asset not found.");
        }

        if (request.MaintenanceRecordId.HasValue)
        {
            var exists = await _context.MaintenanceRecords.AnyAsync(
                x => x.Id == request.MaintenanceRecordId.Value
                     && x.IsActive,
                cancellationToken);

            if (!exists)
                throw new NotFoundException(
                    "Maintenance record not found.");
        }

        if (request.WorkOrderId.HasValue)
        {
            var exists = await _context.WorkOrders.AnyAsync(
                x => x.Id == request.WorkOrderId.Value && x.IsActive,
                cancellationToken);

            if (!exists)
                throw new NotFoundException("Work order not found.");
        }
    }
}