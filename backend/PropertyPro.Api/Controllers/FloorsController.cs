using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Floors.Commands.CreateFloor;
using PropertyPro.Application.Features.Floors.Commands.DeleteFloor;
using PropertyPro.Application.Features.Floors.Commands.UpdateFloor;
using PropertyPro.Application.Features.Floors.DTOs;
using PropertyPro.Application.Features.Floors.Queries.GetFloorById;
using PropertyPro.Application.Features.Floors.Queries.GetFloors;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FloorsController : ControllerBase
{
    private readonly ISender _sender;

    public FloorsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<
        ActionResult<PagedResult<FloorListDto>>> GetAll(
        [FromQuery] GetFloorsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FloorDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetFloorByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<FloorDto>> Create(
        CreateFloorCommand command,
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
    public async Task<ActionResult<FloorDto>> Update(
        int id,
        UpdateFloorCommand command,
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
            new DeleteFloorCommand(id),
            cancellationToken);

        return NoContent();
    }
}