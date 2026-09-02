
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;

namespace PropertyPro.Application.Features.AssetCategories.Commands.DeleteAssetCategory;

public class DeleteAssetCategoryCommandHandler
    : IRequestHandler<DeleteAssetCategoryCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAssetCategoryCommandHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteAssetCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await _context.AssetCategories
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.IsActive,
                cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Asset category with ID {request.Id} was not found.");
        }

        var hasActiveAssets = await _context.Assets
            .AnyAsync(
                x => x.AssetCategoryId == category.Id &&
                     x.IsActive,
                cancellationToken);

        if (hasActiveAssets)
        {
            throw new ConflictException(
                $"Asset category '{category.Name}' cannot be deleted because it is used by active assets.");
        }

        category.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}

