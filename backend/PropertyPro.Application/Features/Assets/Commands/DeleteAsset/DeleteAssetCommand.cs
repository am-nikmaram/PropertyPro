using MediatR;

namespace PropertyPro.Application.Features.Assets.Commands.DeleteAsset;

public record DeleteAssetCommand(
    int Id
) : IRequest;