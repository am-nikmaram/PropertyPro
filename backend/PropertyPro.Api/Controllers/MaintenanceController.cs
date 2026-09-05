using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMediator _mediator;

    public MaintenanceController(
        IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMaintenanceCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            result);
    }


    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        // We will implement this in the next query step
        return Ok();
    }
}