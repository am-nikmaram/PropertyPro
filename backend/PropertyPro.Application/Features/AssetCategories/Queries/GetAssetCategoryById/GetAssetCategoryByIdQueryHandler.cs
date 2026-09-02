
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryById;

public class GetAssetCategoryByIdQueryHandler
    : IRequestHandler<GetAssetCategoryByIdQuery, AssetCategoryDto>
{
    private readonly IApplicationDbContext _context;

    public GetAssetCategoryByIdQueryHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AssetCategoryDto> Handle(
        GetAssetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _context.AssetCategories
            .AsNoTracking()
            .Where(x =>
                x.Id == request.Id &&
                x.IsActive)
            .Select(x => new AssetCategoryDto(
                x.Id,
                x.Code,
                x.Name,
                x.Description,
                x.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(
                $"Asset category with ID {request.Id} was not found.");
        }

        return category;
    }
}

