using MediatR;
using PropertyPro.Application.Features.Assets.DTOs;

namespace PropertyPro.Application.Features.Assets.Queries.GetAssetById;

public record GetAssetByIdQuery(
    int Id
) : IRequest<AssetDto>;