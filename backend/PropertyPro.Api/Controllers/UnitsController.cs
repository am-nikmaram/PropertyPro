using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.Units.Commands.CreateUnit;
using PropertyPro.Application.Features.Units.Commands.DeleteUnit;
using PropertyPro.Application.Features.Units.Commands.UpdateUnit;
using PropertyPro.Application.Features.Units.Queries.GetUnitById;
using PropertyPro.Application.Features.Units.Queries.GetUnitLookup;
using PropertyPro.Application.Features.Units.Queries.GetUnits;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitsController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<IActionResult> Create(
        CreateUnitCommand command,
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
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUnitByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetUnitsQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<IActionResult> Update(
        int id,
        UpdateUnitCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "Route ID and command ID must match.");
        }

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeleteUnitCommand(id),
            cancellationToken);

        return NoContent();
    }


    [HttpGet("lookup")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> Lookup(
    [FromQuery] GetUnitLookupQuery query,
    CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }




}