using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.AssetCategories.Commands.CreateAssetCategory;
using PropertyPro.Application.Features.AssetCategories.Commands.DeleteAssetCategory;
using PropertyPro.Application.Features.AssetCategories.Commands.UpdateAssetCategory;
using PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategories;
using PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryById;
using PropertyPro.Application.Features.AssetCategories.Queries.GetAssetCategoryLookup;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssetCategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public AssetCategoriesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAssetCategories(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAssetCategoriesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAssetCategoryById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAssetCategoryByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> CreateAssetCategory(
        CreateAssetCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetAssetCategoryById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> UpdateAssetCategory(
        int id,
        UpdateAssetCategoryCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest(
                "Route ID and request ID must match.");

        var result = await _sender.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> DeleteAssetCategory(
        int id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteAssetCategoryCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("lookup")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetLookup(
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetAssetCategoryLookupQuery(),
            cancellationToken);

        return Ok(result);
    }


}