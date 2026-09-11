using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.Assets.Commands.CreateAsset;
using PropertyPro.Application.Features.Assets.Commands.DeleteAsset;
using PropertyPro.Application.Features.Assets.Commands.UpdateAsset;
using PropertyPro.Application.Features.Assets.Queries.GetAssetById;
using PropertyPro.Application.Features.Assets.Queries.GetAssets;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAssets(
        [FromQuery] GetAssetsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAssetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetAssetByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> CreateAsset(
        [FromBody] CreateAssetCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetAssetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> UpdateAsset(
        int id,
        [FromBody] UpdateAssetCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route id does not match command id.");
        }

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> DeleteAsset(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteAssetCommand(id),
            cancellationToken);

        return NoContent();
    }
}