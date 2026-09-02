using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.AssetCategories.DTOs;

namespace PropertyPro.Application.Features.AssetCategories.Commands.UpdateAssetCategory;

public class UpdateAssetCategoryCommandHandler
    : IRequestHandler<UpdateAssetCategoryCommand, AssetCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateAssetCategoryCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AssetCategoryDto> Handle(
        UpdateAssetCategoryCommand request,
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

        var code = request.Code
            .Trim()
            .ToUpperInvariant();

        var name = request.Name.Trim();

        var duplicateCode = await _context.AssetCategories
            .AnyAsync(
                x => x.Id != request.Id &&
                     x.Code == code &&
                     x.IsActive,
                cancellationToken);

        if (duplicateCode)
        {
            throw new ConflictException(
                $"Asset category code '{code}' already exists.");
        }

        var duplicateName = await _context.AssetCategories
            .AnyAsync(
                x => x.Id != request.Id &&
                     x.Name == name &&
                     x.IsActive,
                cancellationToken);

        if (duplicateName)
        {
            throw new ConflictException(
                $"Asset category '{name}' already exists.");
        }

        _mapper.Map(request, category);

        category.Code = code;
        category.Name = name;
        category.Description = request.Description?.Trim();

        await _context.SaveChangesAsync(cancellationToken);

        return new AssetCategoryDto(
            category.Id,
            category.Code,
            category.Name,
            category.Description,
            category.IsActive);
    }
}

