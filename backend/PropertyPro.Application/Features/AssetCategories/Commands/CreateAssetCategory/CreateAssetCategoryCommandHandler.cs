using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PropertyPro.Application.Common.Exceptions;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Application.Features.AssetCategories.DTOs;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.AssetCategories.Commands.CreateAssetCategory;

public class CreateAssetCategoryCommandHandler
    : IRequestHandler<CreateAssetCategoryCommand, AssetCategoryDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateAssetCategoryCommandHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AssetCategoryDto> Handle(
        CreateAssetCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code
            .Trim()
            .ToUpperInvariant();

        var name = request.Name.Trim();

        var codeExists = await _context.AssetCategories
            .AnyAsync(
                x => x.Code == code &&
                     x.IsActive,
                cancellationToken);

        if (codeExists)
        {
            throw new ConflictException(
                $"Asset category code '{code}' already exists.");
        }

        var nameExists = await _context.AssetCategories
            .AnyAsync(
                x => x.Name == name &&
                     x.IsActive,
                cancellationToken);

        if (nameExists)
        {
            throw new ConflictException(
                $"Asset category '{name}' already exists.");
        }

        var category = _mapper.Map<AssetCategory>(request);

        category.Code = code;
        category.Name = name;
        category.Description = request.Description?.Trim();

        _context.AssetCategories.Add(category);

        await _context.SaveChangesAsync(cancellationToken);

        return new AssetCategoryDto(
            category.Id,
            category.Code,
            category.Name,
            category.Description,
            category.IsActive);
    }
}

