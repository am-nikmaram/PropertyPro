using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Buildings.Commands.CreateBuilding;
using PropertyPro.Application.Features.Buildings.DTOs;
using PropertyPro.Application.Features.Buildings.Queries.GetBuildingById;
using PropertyPro.Application.Features.Buildings.Queries.GetBuildings;
using PropertyPro.Application.Features.Buildings.Commands.UpdateBuilding;
using PropertyPro.Application.Features.Buildings.Commands.DeleteBuilding;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BuildingsController : ControllerBase
{
    private readonly ISender _sender;

    public BuildingsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<
        ActionResult<PagedResult<BuildingListDto>>> GetAll(
        [FromQuery] GetBuildingsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BuildingDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetBuildingByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<BuildingDto>> Create(
        CreateBuildingCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BuildingDto>> Update(
    int id,
    UpdateBuildingCommand command,
    CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID does not match request ID.");
        }

        var result = await _sender.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteBuildingCommand(id),
            cancellationToken);

        return NoContent();
    }
}