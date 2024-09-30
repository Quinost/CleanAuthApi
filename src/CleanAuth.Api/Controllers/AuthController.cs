using CleanAuth.Application.Commands.Auth;
using CleanAuth.Application.Queries.Auth;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuth.Api.Controllers;
[Route("api/auth")]
[ApiController]
public class AuthController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
    {
        var result = await mediator.Send(cmd);
        return result.IsSuccess ? Ok(result) : Unauthorized(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var accessToken = await HttpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
        await mediator.Send(new LogoutCommand(accessToken!));
        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("publickey")]
    public async Task<IActionResult> GetPublicKey()
    {
        return Ok(await mediator.Send(new GetPublicKeyQuery()));
    }
}
