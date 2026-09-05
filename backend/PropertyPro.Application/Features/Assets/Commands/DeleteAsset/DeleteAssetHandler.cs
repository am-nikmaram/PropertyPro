using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Assets.Commands.DeleteAsset;

public class DeleteAssetHandler
    : IRequestHandler<DeleteAssetCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteAssetHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        DeleteAssetCommand request,
        CancellationToken cancellationToken)
    {
        var asset = await _context.Assets
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && x.IsActive,
                cancellationToken);

        if (asset is null)
        {
            throw new NotFoundException(
                nameof(Asset),
                request.Id);
        }

        asset.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}