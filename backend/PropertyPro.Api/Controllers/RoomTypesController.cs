using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Features.RoomTypes.Commands.CreateRoomType;
using PropertyPro.Application.Features.RoomTypes.Commands.DeleteRoomType;
using PropertyPro.Application.Features.RoomTypes.Commands.UpdateRoomType;
using PropertyPro.Application.Features.RoomTypes.DTOs;
using PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypeById;
using PropertyPro.Application.Features.RoomTypes.Queries.GetRoomTypes;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomTypesController : ControllerBase
{
    private readonly ISender _sender;

    public RoomTypesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<
        ActionResult<IReadOnlyList<RoomTypeListDto>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRoomTypesQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomTypeDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRoomTypeByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RoomTypeDto>> Create(
        CreateRoomTypeCommand command,
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
    public async Task<ActionResult<RoomTypeDto>> Update(
        int id,
        UpdateRoomTypeCommand command,
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
            new DeleteRoomTypeCommand(id),
            cancellationToken);

        return NoContent();
    }
}