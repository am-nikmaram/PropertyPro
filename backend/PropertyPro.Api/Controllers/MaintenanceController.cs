using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.Maintenance.Commands.CreateMaintenance;
using PropertyPro.Application.Features.Maintenance.Commands.DeleteMaintenance;
using PropertyPro.Application.Features.Maintenance.Commands.UpdateMaintenance;
using PropertyPro.Application.Features.Maintenance.Queries.GetMaintenanceById;
using PropertyPro.Application.Features.Maintenance.Queries.GetMaintenances;

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
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
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



    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> Update(
        int id,
        UpdateMaintenanceCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();


        var result = await _mediator.Send(
            command,
            cancellationToken);


        return Ok(result);
    }



  
    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteMaintenanceCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetById(
    int id,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetMaintenanceByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }


    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAll(
    [FromQuery] GetMaintenancesQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

}