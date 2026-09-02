
using AutoMapper;
using PropertyPro.Application.Features.AssetCategories.Commands.CreateAssetCategory;
using PropertyPro.Application.Features.AssetCategories.Commands.UpdateAssetCategory;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.AssetCategories.Mappings;

public class AssetCategoryProfile : Profile
{
    public AssetCategoryProfile()
    {
        CreateMap<CreateAssetCategoryCommand, AssetCategory>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(dest => dest.Assets,
                opt => opt.Ignore());

        CreateMap<UpdateAssetCategoryCommand, AssetCategory>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(dest => dest.Assets,
                opt => opt.Ignore());
    }
}

