using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PropertyPro.Api.Authorization;
using PropertyPro.Application.Features.Users.Commands.AssignRole;
using PropertyPro.Application.Features.Users.Commands.RemoveRole;
using PropertyPro.Application.Features.Users.Queries.GetUserById;
using PropertyPro.Application.Features.Users.Queries.GetUsers;

namespace PropertyPro.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = AuthorizationPolicies.AdminOnly)]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers(
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUsersQuery(),
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(
        string id,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new GetUserByIdQuery(id),
            cancellationToken);

        return Ok(result);
    }

    [HttpPut("{id}/roles")]
    public async Task<IActionResult> AssignRole(
        string id,
        [FromBody] AssignRoleRequest request,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new AssignRoleCommand(
                id,
                request.Role),
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id}/roles/{role}")]
    public async Task<IActionResult> RemoveRole(
        string id,
        string role,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(
            new RemoveRoleCommand(
                id,
                role),
            cancellationToken);

        return NoContent();
    }
}

public record AssignRoleRequest(string Role);