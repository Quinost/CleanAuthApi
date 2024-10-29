using CleanAuth.Application.Commands.Users;
using CleanAuth.Application.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuth.Api.Controllers;
[Route("api/user")]
[ApiController]
[Authorize]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers(int pageNumber, int pageSize, string? filter)
    {
        var result = await mediator.Send(new GetUsersQuery(filter ?? string.Empty, pageNumber, pageSize));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    } 

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddUser(AddUserCommand cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPut]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var username = HttpContext.User.Identity?.Name;
        var result = await mediator.Send(new ChangePasswordCommand(username, request.OldPassword, request.NewPassword));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await mediator.Send(new DeleteUserCommand(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }
}
