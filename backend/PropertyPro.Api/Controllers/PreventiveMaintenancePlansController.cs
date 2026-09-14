using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.PreventiveMaintenance.Commands.CreatePreventiveMaintenancePlan;
using PropertyPro.Application.Features.PreventiveMaintenance.Commands.DeletePreventiveMaintenancePlan;
using PropertyPro.Application.Features.PreventiveMaintenance.Commands.UpdatePreventiveMaintenancePlan;
using PropertyPro.Application.Features.PreventiveMaintenance.DTOs;
using PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetOverduePreventiveMaintenance;
using PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanById;
using PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlanLookup;
using PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetPreventiveMaintenancePlans;
using PropertyPro.Application.Features.PreventiveMaintenance.Queries.GetUpcomingPreventiveMaintenance;
using PropertyPro.Application.Common.Models;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/preventive-maintenance-plans")]
[Authorize]
public class PreventiveMaintenancePlansController : ControllerBase
{
    private readonly IMediator _mediator;

    public PreventiveMaintenancePlansController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    [ProducesResponseType(
        typeof(PagedResult<PreventiveMaintenancePlanListDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        PagedResult<PreventiveMaintenancePlanListDto>>> GetAll(
        [FromQuery] GetPreventiveMaintenancePlansQuery query,
        CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(
                query,
                cancellationToken));
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    [ProducesResponseType(
        typeof(PreventiveMaintenancePlanDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PreventiveMaintenancePlanDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(
                new GetPreventiveMaintenancePlanByIdQuery(id),
                cancellationToken));
    }

    [HttpGet("lookup")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    [ProducesResponseType(
        typeof(IReadOnlyList<PreventiveMaintenancePlanLookupDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<PreventiveMaintenancePlanLookupDto>>> Lookup(
        CancellationToken cancellationToken)
    {
        return Ok(
            await _mediator.Send(
                new GetPreventiveMaintenancePlanLookupQuery(),
                cancellationToken));
    }

    [HttpGet("upcoming")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    [ProducesResponseType(
        typeof(IReadOnlyList<PreventiveMaintenanceScheduleDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<PreventiveMaintenanceScheduleDto>>> GetUpcoming(
        [FromQuery] int days = 30,
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        return Ok(
            await _mediator.Send(
                new GetUpcomingPreventiveMaintenanceQuery(
                    days,
                    take),
                cancellationToken));
    }

    [HttpGet("overdue")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    [ProducesResponseType(
        typeof(IReadOnlyList<PreventiveMaintenanceScheduleDto>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<
        IReadOnlyList<PreventiveMaintenanceScheduleDto>>> GetOverdue(
        [FromQuery] int take = 10,
        CancellationToken cancellationToken = default)
    {
        return Ok(
            await _mediator.Send(
                new GetOverduePreventiveMaintenanceQuery(take),
                cancellationToken));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    [ProducesResponseType(
        typeof(PreventiveMaintenancePlanDto),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PreventiveMaintenancePlanDto>> Create(
        CreatePreventiveMaintenancePlanCommand command,
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
    [ProducesResponseType(
        typeof(PreventiveMaintenancePlanDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<PreventiveMaintenancePlanDto>> Update(
        int id,
        UpdatePreventiveMaintenancePlanCommand command,
        CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest(
                "The route ID does not match the command ID.");
        }

        return Ok(
            await _mediator.Send(
                command,
                cancellationToken));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.MaintenanceManagement)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new DeletePreventiveMaintenancePlanCommand(id),
            cancellationToken);

        return NoContent();
    }
}