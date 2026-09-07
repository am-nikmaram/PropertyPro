using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Rooms.Commands.CreateRoom;
using PropertyPro.Application.Features.Rooms.Commands.DeleteRoom;
using PropertyPro.Application.Features.Rooms.Commands.UpdateRoom;
using PropertyPro.Application.Features.Rooms.DTOs;
using PropertyPro.Application.Features.Rooms.Queries.GetRoomById;
using PropertyPro.Application.Features.Rooms.Queries.GetRoomLookup;
using PropertyPro.Application.Features.Rooms.Queries.GetRooms;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly ISender _sender;

    public RoomsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<
        ActionResult<PagedResult<RoomListDto>>> GetAll(
        [FromQuery] GetRoomsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RoomDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetRoomByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(
        CreateRoomCommand command,
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
    public async Task<ActionResult<RoomDto>> Update(
        int id,
        UpdateRoomCommand command,
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
            new DeleteRoomCommand(id),
            cancellationToken);

        return NoContent();
    }


[HttpGet("lookup")]
public async Task<IActionResult> Lookup(
    [FromQuery] GetRoomLookupQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }


}