using CleanAuth.Application.Commands.Roles;
using CleanAuth.Application.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuth.Api.Controllers;
[Route("api/role")]
[ApiController]
[Authorize]
public class RoleController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRoles(int pageNumber, int pageSize, string? filter)
    {
        var result = await mediator.Send(new GetRolesQuery(filter ?? string.Empty, pageNumber, pageSize));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRole(Guid id)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddRole(AddRoleCommand cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id)
    {
        var result = await mediator.Send(new DeleteRoleCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
