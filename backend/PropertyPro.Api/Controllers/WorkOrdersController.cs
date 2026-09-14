using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.WorkOrders.Commands.ChangeWorkOrderStatus;
using PropertyPro.Application.Features.WorkOrders.Commands.CreateWorkOrder;
using PropertyPro.Application.Features.WorkOrders.Commands.DeleteWorkOrder;
using PropertyPro.Application.Features.WorkOrders.Commands.UpdateWorkOrder;
using PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderById;
using PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrderLookup;
using PropertyPro.Application.Features.WorkOrders.Queries.GetWorkOrders;

namespace PropertyPro.Api.Controllers;

/// <summary>
/// Provides endpoints for managing maintenance work orders.
/// </summary>

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly IMediator _mediator;


public WorkOrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets a paginated list of work orders.
    /// </summary>
    /// <returns>A paginated collection of work orders.</returns>
    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetWorkOrders(
        [FromQuery] GetWorkOrdersQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            query,
            cancellationToken);

        return Ok(result);
    }


    /// <summary>
    /// Gets a work order by its identifier.
    /// </summary>
    /// <param name="id">The work order identifier.</param>
    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    [ProducesResponseType(
    StatusCodes.Status200OK)]
    [ProducesResponseType(
    StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(
    StatusCodes.Status403Forbidden)]
    [ProducesResponseType(
    StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetWorkOrderById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetWorkOrderByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("lookup")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetLookup(
        [FromQuery] int? maintenanceRecordId,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetWorkOrderLookupQuery(maintenanceRecordId),
            cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkOrderCommand command,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            command,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetWorkOrderById),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateWorkOrderCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "The route id does not match the command id.");
        }

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
            new DeleteWorkOrderCommand(id),
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceExecution)]
    public async Task<IActionResult> ChangeStatus(
        int id,
        [FromBody] ChangeWorkOrderStatusCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "The route id does not match the command id.");
        }

        var result = await _mediator.Send(
            command,
            cancellationToken);

        return Ok(result);
    }


}
