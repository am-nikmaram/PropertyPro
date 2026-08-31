using MediatR;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Application.Common.Models;
using PropertyPro.Application.Features.Facilities.Commands.CreateFacility;
using PropertyPro.Application.Features.Facilities.DTOs;
using PropertyPro.Application.Features.Facilities.Queries.GetFacilities;
using PropertyPro.Application.Features.Facilities.Queries.GetFacilityById;

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
}