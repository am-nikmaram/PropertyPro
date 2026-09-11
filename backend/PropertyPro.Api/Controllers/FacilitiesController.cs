using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Facilities.Commands.CreateFacility;
using PropertyPro.Application.Features.Facilities.Commands.DeleteFacility;
using PropertyPro.Application.Features.Facilities.Commands.UpdateFacility;
using PropertyPro.Application.Features.Facilities.DTOs;
using PropertyPro.Application.Features.Facilities.Queries.GetFacilities;
using PropertyPro.Application.Features.Facilities.Queries.GetFacilityById;
using PropertyPro.Application.Features.Facilities.Queries.GetFacilityLookup;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FacilitiesController : ControllerBase
{
    private readonly ISender _sender;

    public FacilitiesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<ActionResult<PagedResult<FacilityListDto>>> GetAll(
        [FromQuery] GetFacilitiesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            query,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<ActionResult<FacilityDto>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetFacilityByIdQuery(id),
            cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<ActionResult<FacilityDto>> Create(
    CreateFacilityCommand command,
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
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<ActionResult<FacilityDto>> Update(
    int id,
    UpdateFacilityCommand command,
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
    [Authorize(Policy = AuthorizationPolicies.FacilityManagement)]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        await _sender.Send(
            new DeleteFacilityCommand(id),
            cancellationToken);

        return NoContent();
    }


    [HttpGet("lookup")]
    [Authorize(Policy = AuthorizationPolicies.ReadOnly)]
    public async Task<IActionResult> GetLookup(
    CancellationToken cancellationToken)
    {
        var result = await _sender.Send(
            new GetFacilityLookupQuery(),
            cancellationToken);

        return Ok(result);
    }


}