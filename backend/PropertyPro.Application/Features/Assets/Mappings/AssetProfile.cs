
using AutoMapper;
using PropertyPro.Application.Features.Assets.Commands.CreateAsset;
using PropertyPro.Application.Features.Assets.Commands.UpdateAsset;
using PropertyPro.Domain.Entities;

namespace PropertyPro.Application.Features.Assets.Mappings;

public class AssetProfile : Profile
{
    public AssetProfile()
    {
        CreateMap<CreateAssetCommand, Asset>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(dest => dest.Room,
                opt => opt.Ignore())
            .ForMember(dest => dest.AssetCategory,
                opt => opt.Ignore());

        CreateMap<UpdateAssetCommand, Asset>()
            .ForMember(dest => dest.Id,
                opt => opt.Ignore())
            .ForMember(dest => dest.IsActive,
                opt => opt.Ignore())
            .ForMember(dest => dest.Room,
                opt => opt.Ignore())
            .ForMember(dest => dest.AssetCategory,
                opt => opt.Ignore());
    }
}